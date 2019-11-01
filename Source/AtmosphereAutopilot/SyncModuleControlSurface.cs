/*
Atmosphere Autopilot, plugin for Kerbal Space Program.
Copyright (C) 2015-2016, Baranin Alexander aka Boris-Barboris.

Changes (c) 2019-2020 LisiasT
 
Atmosphere Autopilot is free software: you can redistribute it and/or modify
it under the terms of the GNU General Public License as published by
the Free Software Foundation, either version 3 of the License, or
(at your option) any later version.
Atmosphere Autopilot is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
GNU General Public License for more details.
You should have received a copy of the GNU General Public License
along with Atmosphere Autopilot.  If not, see <http://www.gnu.org/licenses/>. 
*/

using System;
using UnityEngine;

namespace AtmosphereAutopilot
{
    using KspModuleControlSurface = global::ModuleControlSurface;

    /// <summary>
    /// Synchronised ModuleControlSurface implementation, greatly simplifies control and flight model regression 
    /// by making all control surfaces move in one phase.
    /// </summary>
    public class SyncModuleControlSurface : KspModuleControlSurface
    {
        public const float CSURF_SPD = 2.0f;

        // normalized [-1.0, 1.0] previous actions for separate axes
        protected float prev_pitch_normdeflection = 0.0f;
        protected float prev_roll_normdeflection = 0.0f;
        protected float prev_yaw_normdeflection = 0.0f;

        protected bool was_deployed = false;
        protected bool already_checked = false;

        public override void OnAwake()
        {
            base.OnAwake();
            Log.dbg("It's me, Mario!!!");
        }

        public override void OnStart(PartModule.StartState state)
        {
            base.OnStart(state);

            if (!HighLogic.LoadedSceneIsFlight) return;
            if (already_checked) return;
            already_checked = true;

            if (usesMirrorDeploy) return;
            // This code is needed due savegames previous from the AA first installation (as well crafts, and also ones
            // downloaded or copied from other savegames) looses the condiguration when loaded with AA.
            // Only savegames and crafts made after AA installation have their Control Surfaces settings restoned!
            // Once this small inconvenience =P is fixed, this code can go away.
            if (part.symMethod == SymmetryMethod.Mirror &&
                part.symmetryCounterparts != null &&
                part.symmetryCounterparts.Count > 0)
            {
                usesMirrorDeploy = true;
                Part p = part.symmetryCounterparts[0];
                this.mirrorDeploy =
                    (Mathf.Abs(part.transform.localRotation.w) < Mathf.Abs(p.transform.localRotation.w))
                    ||
                    (Mathf.Abs(part.transform.localRotation.w) == Mathf.Abs(p.transform.localRotation.w)
                        && part.transform.localRotation.x < p.transform.localRotation.x)
                    ;
            }
        }

        public override void OnSave(ConfigNode node)
        {
            base.OnSave(node);

            // Hack to prevent AA to hijack the savagames and craft files.
            // MODULE sections with name="ModuleControlSurface" not only is ignored by this partModule, but once this is 
            // saved with name="SyncModuleControlSurface", only KSP installments with AA installed will correctly handle it,
            // and that disconfigure all Contol Surfaces parts when you uninstall AA!!      
            //node.SetValue("name", "ModuleControlSurface", false);
            //node.SetValue("AtmosphericAutopilot", true, true);
        }

#if KSP_1_8
        protected override void CtrlSurfaceUpdate(Vector3 vel)
        {
            if (null == vessel.transform) return;

            this.alwaysRecomputeLift = true;

            Vector3 world_com = vessel.CoM;
            float pitch_input = ignorePitch ? 0.0f : vessel.ctrlState.pitch;
            float roll_input = ignoreRoll ? 0.0f : vessel.ctrlState.roll;
            float yaw_input = ignoreYaw ? 0.0f : vessel.ctrlState.yaw;

            if (base.vessel.atmDensity == 0.0)
                pitch_input = roll_input = yaw_input = 0.0f;

            float spd_factor = TimeWarp.fixedDeltaTime * CSURF_SPD;
            float fwd_airstream_factor = Mathf.Sign(Vector3.Dot(vessel.ReferenceTransform.up, vessel.srf_velocity) + 0.1f);
            float exp_spd_factor = actuatorSpeed / actuatorSpeedNormScale * TimeWarp.fixedDeltaTime;

            if (deploy)
            {
                float normaction = action / deployAngle;
                if (float.IsNaN(normaction))
                    normaction = 0.0f;
                float target = deployInvert ? 1.0f : -1.0f;
                if (usesMirrorDeploy && mirrorDeploy) target *= -1.0f;
                if (!ignorePitch)   prev_pitch_normdeflection = target;
                if (!ignoreRoll)    prev_roll_normdeflection = target;
                if (!ignoreYaw)     prev_yaw_normdeflection = target;
                was_deployed = true;
                action = target;
                deflection = deflection + deployAngle * Common.Clampf(target - normdeflection, spd_factor);
                ctrlSurface.localRotation = Quaternion.AngleAxis(deflection, Vector3.right) * neutral;
                return;
            }

            if (ignorePitch)
                prev_pitch_normdeflection = 0.0f;
            else
            {
                float axis_factor = Vector3.Dot(vessel.ReferenceTransform.right, baseTransform.right) * fwd_airstream_factor;
                float pitch_factor = axis_factor * Math.Sign(Vector3.Dot(world_com - baseTransform.position, vessel.ReferenceTransform.up));
                if (was_deployed)
                    prev_pitch_normdeflection = Common.Clampf(prev_pitch_normdeflection, Mathf.Abs(pitch_factor));
                float new_pitch_action = pitch_input * pitch_factor;
                if (useExponentialSpeed)
                    prev_pitch_normdeflection = Mathf.Lerp(prev_pitch_normdeflection, new_pitch_action, exp_spd_factor);
                else
                    prev_pitch_normdeflection = prev_pitch_normdeflection + Common.Clampf(new_pitch_action - prev_pitch_normdeflection, spd_factor * Math.Abs(axis_factor));
            }

            if (ignoreRoll)
                prev_roll_normdeflection = 0.0f;
            else
            {
                float axis_factor = Vector3.Dot(vessel.ReferenceTransform.up, baseTransform.up) * fwd_airstream_factor;
                float roll_factor = axis_factor * Math.Sign(Vector3.Dot(vessel.ReferenceTransform.up,
                    Vector3.Cross(world_com - baseTransform.position, baseTransform.forward)));
                if (was_deployed)
                    prev_roll_normdeflection = Common.Clampf(prev_roll_normdeflection, Mathf.Abs(roll_factor));
                float new_roll_action = roll_input * roll_factor;
                if (useExponentialSpeed)
                    prev_roll_normdeflection = Mathf.Lerp(prev_roll_normdeflection, new_roll_action, exp_spd_factor);
                else
                    prev_roll_normdeflection = prev_roll_normdeflection + Common.Clampf(new_roll_action - prev_roll_normdeflection, spd_factor * axis_factor);
            }

            if (ignoreYaw)
                prev_yaw_normdeflection = 0.0f;
            else
            {
                float axis_factor = Vector3.Dot(vessel.ReferenceTransform.forward, baseTransform.right) * fwd_airstream_factor;
                float yaw_factor = axis_factor * Math.Sign(Vector3.Dot(world_com - baseTransform.position, vessel.ReferenceTransform.up));
                if (was_deployed)
                    prev_yaw_normdeflection = Common.Clampf(prev_yaw_normdeflection, Mathf.Abs(yaw_factor));
                float new_yaw_action = yaw_input * yaw_factor;
                if (useExponentialSpeed)
                    prev_yaw_normdeflection = Mathf.Lerp(prev_yaw_normdeflection, new_yaw_action, exp_spd_factor);
                else
                    prev_yaw_normdeflection = prev_yaw_normdeflection + Common.Clampf(new_yaw_action - prev_yaw_normdeflection, spd_factor * Math.Abs(axis_factor));
            }

            was_deployed = false;

            deflection = action = ctrlSurfaceRange * authorityLimiter * 0.01f * Common.Clampf(prev_pitch_normdeflection + prev_roll_normdeflection + prev_yaw_normdeflection, 1.0f);
            ctrlSurface.localRotation = Quaternion.AngleAxis(deflection, Vector3.right) * neutral;
        }
#else
        protected override void CtrlSurfaceUpdate(Vector3 vel)
        {
            if (null == vessel.transform) return;

            this.alwaysRecomputeLift = true;

            Vector3 world_com = vessel.CoM;
            float pitch_input = ignorePitch ? 0.0f : vessel.ctrlState.pitch;
            float roll_input = ignoreRoll ? 0.0f : vessel.ctrlState.roll;
            float yaw_input = ignoreYaw ? 0.0f : vessel.ctrlState.yaw;

            if (base.vessel.atmDensity == 0.0)
                pitch_input = roll_input = yaw_input = 0.0f;

            float spd_factor = TimeWarp.fixedDeltaTime * CSURF_SPD;
            float fwd_airstream_factor = Mathf.Sign(Vector3.Dot(vessel.ReferenceTransform.up, vessel.srf_velocity) + 0.1f);
            float exp_spd_factor = actuatorSpeed / actuatorSpeedNormScale * TimeWarp.fixedDeltaTime;

            if (deploy)
            {
                float target = deployInvert ? 1.0f : -1.0f;
                if (usesMirrorDeploy && mirrorDeploy) target *= -1.0f;
                if (!ignorePitch) prev_pitch_normdeflection = target;
                if (!ignoreRoll) prev_roll_normdeflection = target;
                if (!ignoreYaw) prev_yaw_normdeflection = target;
                was_deployed = true;
                deflection = action = action + Common.Clampf(target - action, spd_factor);
                ctrlSurface.localRotation = Quaternion.AngleAxis(deflection * ctrlSurfaceRange * 0.01f * authorityLimiter, Vector3.right) * neutral;
                return;
            }

            if (ignorePitch)
                prev_pitch_normdeflection = 0.0f;
            else
            {
                float axis_factor = Vector3.Dot(vessel.ReferenceTransform.right, baseTransform.right) * fwd_airstream_factor;
                float pitch_factor = axis_factor * Math.Sign(Vector3.Dot(world_com - baseTransform.position, vessel.ReferenceTransform.up));
                if (was_deployed)
                    prev_pitch_normdeflection = Common.Clampf(prev_pitch_normdeflection, Mathf.Abs(pitch_factor));
                float new_pitch_action = pitch_input * pitch_factor;
                prev_pitch_normdeflection = useExponentialSpeed
                    ? Mathf.Lerp(prev_pitch_normdeflection, new_pitch_action, exp_spd_factor)
                    : prev_pitch_normdeflection + Common.Clampf(new_pitch_action - prev_pitch_normdeflection, spd_factor * Math.Abs(axis_factor));
            }

            if (ignoreRoll)
                prev_roll_normdeflection = 0.0f;
            else
            {
                float axis_factor = Vector3.Dot(vessel.ReferenceTransform.up, baseTransform.up) * fwd_airstream_factor;
                float roll_factor = axis_factor * Math.Sign(Vector3.Dot(vessel.ReferenceTransform.up,
                    Vector3.Cross(world_com - baseTransform.position, baseTransform.forward)));
                if (was_deployed)
                    prev_roll_normdeflection = Common.Clampf(prev_roll_normdeflection, Mathf.Abs(roll_factor));
                float new_roll_action = roll_input * roll_factor;
                prev_roll_normdeflection = useExponentialSpeed
                    ? Mathf.Lerp(prev_roll_normdeflection, new_roll_action, exp_spd_factor)
                    : prev_roll_normdeflection + Common.Clampf(new_roll_action - prev_roll_normdeflection, spd_factor * axis_factor);
            }

            if (ignoreYaw)
                prev_yaw_normdeflection = 0.0f;
            else
            {
                float axis_factor = Vector3.Dot(vessel.ReferenceTransform.forward, baseTransform.right) * fwd_airstream_factor;
                float yaw_factor = axis_factor * Math.Sign(Vector3.Dot(world_com - baseTransform.position, vessel.ReferenceTransform.up));
                if (was_deployed)
                    prev_yaw_normdeflection = Common.Clampf(prev_yaw_normdeflection, Mathf.Abs(yaw_factor));
                float new_yaw_action = yaw_input * yaw_factor;
                prev_yaw_normdeflection = useExponentialSpeed
                    ? Mathf.Lerp(prev_yaw_normdeflection, new_yaw_action, exp_spd_factor)
                    : prev_yaw_normdeflection + Common.Clampf(new_yaw_action - prev_yaw_normdeflection, spd_factor * Math.Abs(axis_factor));
            }

            was_deployed = false;

            deflection = action = 0.01f * authorityLimiter * Common.Clampf(prev_pitch_normdeflection + prev_roll_normdeflection + prev_yaw_normdeflection, 1.0f);
            ctrlSurface.localRotation = Quaternion.AngleAxis(deflection * ctrlSurfaceRange, Vector3.right) * neutral;
        }
#endif
    }
}
