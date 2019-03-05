/*
Atmosphere Autopilot, plugin for Kerbal Space Program.
Copyright (C) 2015-2016, Baranin Alexander aka Boris-Barboris.

Changes (c) 2019, LisiasT
 
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
    /// Synchronised ModuleControlSurface realization, greatly simplifies control and flight model regression 
    /// by making all control surfaces move in one phase.
    /// </summary>
    [KSPModule("ModuleControlSurface")]﻿ 
    public class ModuleControlSurface: KspModuleControlSurface
    {
        public const float CSURF_SPD = 2.0f;

        protected float prev_pitch_action = 0.0f;
        protected float prev_roll_action = 0.0f;
        protected float prev_yaw_action = 0.0f;

        protected bool was_deployed = false;
        protected bool already_checked = false;

        public override void OnAwake()
        {
            base.OnAwake();
        }
        
        //public override void OnStart(PartModule.StartState state)
        //{
        //    base.OnStart(state);
            
        //    if (!HighLogic.LoadedSceneIsFlight) return;
        //    if (already_checked) return;
        //    already_checked = true;

        //    if (usesMirrorDeploy) return;                 
        //    // This code is needed due savegames previous from the AA first installation (as well crafts, and also ones
        //    // downloaded or copied from other savegames) looses the condiguration when loaded with AA.
        //    // Only savegames and crafts made after AA installation have their Control Surfaces settings restoned!
        //    // Onde this small inconvenience =P is fixed, this code can go away.
        //    if (part.symMethod == SymmetryMethod.Mirror && 
        //        part.symmetryCounterparts != null &&
        //        part.symmetryCounterparts.Count > 0)
        //    {
        //        usesMirrorDeploy = true;
        //        Part p = part.symmetryCounterparts[0];
        //        this.mirrorDeploy = 
        //            (Mathf.Abs(part.transform.localRotation.w) < Mathf.Abs(p.transform.localRotation.w))
        //            ||
        //            (Mathf.Abs(part.transform.localRotation.w) == Mathf.Abs(p.transform.localRotation.w) 
        //                && part.transform.localRotation.x < p.transform.localRotation.x)
        //            ;
        //    }
        //}

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

        protected override void CtrlSurfaceUpdate(Vector3 vel)
        {
            if (vessel.transform == null)
                return;

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
                if (!ignorePitch)   prev_pitch_action = target;
                if (!ignoreRoll)    prev_roll_action = target;
                if (!ignoreYaw)     prev_yaw_action = target;
                was_deployed = true;
                deflection = action = action + Common.Clampf(target - action, spd_factor);
                ctrlSurface.localRotation = Quaternion.AngleAxis(deflection * ctrlSurfaceRange * 0.01f * authorityLimiter, Vector3.right) * neutral;
                return;
            }
                
            if (ignorePitch)
                prev_pitch_action = 0.0f;
            else
            {
                float axis_factor = Vector3.Dot(vessel.ReferenceTransform.right, baseTransform.right) * fwd_airstream_factor;
                float pitch_factor = axis_factor * Math.Sign(Vector3.Dot(world_com - baseTransform.position, vessel.ReferenceTransform.up));
                if (was_deployed)
                    prev_pitch_action = Common.Clampf(prev_pitch_action, Mathf.Abs(pitch_factor));
                float new_pitch_action = pitch_input * pitch_factor;
				prev_pitch_action = useExponentialSpeed
					? Mathf.Lerp(prev_pitch_action, new_pitch_action, exp_spd_factor)
					: prev_pitch_action + Common.Clampf(new_pitch_action - prev_pitch_action, spd_factor * Math.Abs(axis_factor));
			}

			if (ignoreRoll)
                prev_roll_action = 0.0f;
            else
            {
                float axis_factor = Vector3.Dot(vessel.ReferenceTransform.up, baseTransform.up) * fwd_airstream_factor;
                float roll_factor = axis_factor * Math.Sign(Vector3.Dot(vessel.ReferenceTransform.up, 
                    Vector3.Cross(world_com - baseTransform.position, baseTransform.forward)));
                if (was_deployed)
                    prev_roll_action = Common.Clampf(prev_roll_action, Mathf.Abs(roll_factor));
                float new_roll_action = roll_input * roll_factor;
				prev_roll_action = useExponentialSpeed
					? Mathf.Lerp(prev_roll_action, new_roll_action, exp_spd_factor)
					: prev_roll_action + Common.Clampf(new_roll_action - prev_roll_action, spd_factor * axis_factor);
			}

			if (ignoreYaw)
                prev_yaw_action = 0.0f;
            else
            {
                float axis_factor = Vector3.Dot(vessel.ReferenceTransform.forward, baseTransform.right) * fwd_airstream_factor;
                float yaw_factor = axis_factor * Math.Sign(Vector3.Dot(world_com - baseTransform.position, vessel.ReferenceTransform.up));
                if (was_deployed)
                    prev_yaw_action = Common.Clampf(prev_yaw_action, Mathf.Abs(yaw_factor));
                float new_yaw_action = yaw_input * yaw_factor;
				prev_yaw_action = useExponentialSpeed
					? Mathf.Lerp(prev_yaw_action, new_yaw_action, exp_spd_factor)
					: prev_yaw_action + Common.Clampf(new_yaw_action - prev_yaw_action, spd_factor * Math.Abs(axis_factor));
			}

			was_deployed = false;

            deflection = action =  0.01f * authorityLimiter * Common.Clampf(prev_pitch_action + prev_roll_action + prev_yaw_action, 1.0f);
            ctrlSurface.localRotation = Quaternion.AngleAxis(deflection * ctrlSurfaceRange, Vector3.right) * neutral;
        }
    }
}
