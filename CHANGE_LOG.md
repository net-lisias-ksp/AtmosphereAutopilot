# AtmosphereAutopilot :: Change Log

* 2019-1112: 1.5.16 (BorisBarboris) for KSP 1.7.3
	+ Update for KSP 1.8.0
* 2019-0715: 1.5.15 (BorisBarboris) for KSP 1.7.3
	+ Do not replace control surface module for parts that contain "propeller" or "blade" in their name.
	+ Default 0.95 value for dir_strength.
* 2019-0614: 1.5.14 (BorisBarboris) for KSP 1.7.1
	+ Fix bug when AA was unable to distinguish landed, splashed and flying situations, causing uncontrollability after model collapse.
* 2019-0302: 1.5.12 (BorisBarboris) for KSP 1.6.1
	+ Adds Tgt and Wpt buttons to the Cruise Flight controller waypoint display.  Copies the current lon/lat of the Target or Waypoint, if one is selected, to the lon/lat display and begins navigation to the waypoint.
* 2018-0819: 1.5.11 (BorisBarboris) for KSP 1.4.5
	+ Gimbal rearrangement regression fixed, gimbal-controlled vessels should feel as good as they did in 1.3.X KSP.
		- director strength* parameter is in craft settings.
	+ CF waypoint can now be entered in text. Mouse picking on the map gets dedicated button.
* 2018-0821: 1.5.11.1 (Lisias) for KSP 1.4.x
	+ Merging (new) fixes from upstream v1.5.11.0:
		- Gimbal rearrangement regression fixed, gimbal-controlled vessels should feel as good as they did in 1.3.X KSP.
		- director strength parameter is in craft settings.
		- CF waypoint can now be entered in text. Mouse picking on the map gets dedicated button.
* 2018-0816: 1.5.10.2 (Lisias) for KSP 1.4.x
	+ Moving settings/configurations/log to <KSP_ROOT>/PluginData, where God intended it to be. :)
* 2018-0731: 1.5.10.1 (Lisias) for KSP 1.4.x
	+ Recompile to 1.4.x
	+ Fixes on waypoint selecting and use by Hotel26
	+ Code stability and Math fixes from Boris-Barboris 
* 2017-0208: 1.5.10 (Boris-Barboris) for KSP 1.2.1
	+ moment of inertia math fix
	+ mod now turns off stock SAS by itself
	+ distance to waypoint back in GUI
	+ watch_precision_mode toggle in ang vel controllers GUI toggles
	+ handling of precision mode. Each ang vel controller has separate option.
	+ precision_mode_factor in config to tune precision input mode factor (0.33 by default). Each ang vel controller has separate option.
* 2016-1210: 1.5.9 (Boris-Barboris) for KSP 1.2.1
	+ Version 1.2.1+ control surface module compatibility.
	+ CruiseFlight autopilot GUI tweaks by CraigCottingham. You can now enter waypoint coordinates in text form.
* 2016-1012: 1.5.8 (Boris-Barboris) for KSP 1.2
	+ KSP v 1.2 compatibility
	+ Removed ModuleManager from the archive, since it's no longer mandatory for non-FAR installs and with FAR it will be there anyway.
* 2016-0810: 1.5.7 (Boris-Barboris) for KSP 1.1.3
	+ ModuleSurfaceFX compatibility
	+ altitude control is off in neo-gui when switching to cruise flight
	+ "Use PID" in ProgradeThrustController is globally saved
	+ Thrust balancer math rewritten
* 2016-0709: 1.5.6 (Boris-Barboris) for KSP 1.1.3
	+ Stock aero control surface authority limiter fixed
	+ Fixed some crashes with cheesy named vessels
	+ Sketch of thrust-balancing for VTOL-lovers (very raw, don't expect much), toggled in Flight Model gui by button "balance_engines". Has hotkey ("thrust balancing" in hotkey manager).
		- Short video: https://youtu.be/8nDSSh-7dQw
* 2016-0625: 1.5.5 (Boris-Barboris) for KSP 1.1
	+ 2-seconds input delay for desired speed, course, altitude and vertical speed fields.
	+ additional hotkeys for speed control toggle (on\off), cruise flight vertical motion control toggle and altitude\vertical speed toggle.
	+ neo-gui by Morse (http://i.imgur.com/pMWmYuX.jpg), togglable by setting use_neo_gui in GlobalSettings.txt\AtmosphereAutopilot to true. Flag is updated every scene change, no need to restart KSP to toggle.
* 2016-0617: 1.5.4 (Boris-Barboris) for KSP 1.1
	+ fixed bug with incorrect handling of navball in surface velocity mode;
	+ Mini-AVC version files fixed;
	+ stock-alike deploy direction of control surfaces under stock aero;
	+ corrected GUI hiding (F2) code;
	+ fixed old bug with stock control surfaces, wich snapped deflection to zero on small control signals. Behaviour on extreme dynamic pressure regimes significantly improved under stock aero, for both manual and automated flight;
	+ new window in AppLauncher window list - "Hotkey manager". Allows to change hotkeys in runtime. Escape to unset hotkey (yes, it pauses the game as well, i know, i hate GUIs);
speed control setpoint can now be controlled by ThrottleU and 	+ ThrottleDown keys. Can be toggled and tweaked in Prograde thrust controller's GUI, consult docs;
	+ close window buttons added;
	+ Cruise Flight window reworked, advanced settings hidden, distance to waypoint readout added;
	+ Cruise Flight setpoints are controllable by pitch and yaw keys if needed. Right Alt (configurable in hotkeys manager) toggles this behaviour. To toggle and tweak, adress CF window advanced options and docs;
	+ corrected vertical speed behaviour on very large turns under CF control;
	+ vertical speed setpoint added to Cruise Flight;
	+ added hotkeys for Rocket mode and Coordinated turn flags of FBW;
* 2016-0430: 1.5.3 (Boris-Barboris) for KSP 1.1
	+ control surface module friendliness to MAF
	+ internal fixes
	+ max_neg_g of director controller is now serialized
	+ virtual rotation to improve behaviour on wobbly crafts even without KJR
* 2016-0406: 1.5.2 (Boris-Barboris) for KSP 1.1
	+ Fixed crash, connected to changing scenes or vessels.
	+ Stock aero control surface module respects new authority slider. 
* 2016-0318: 1.5.1 (Boris-Barboris) for KSP 1.1
	+ KSP 1.1 beta adaptation release
* 2016-0318: 1.5 (Boris-Barboris) for KSP 1.0.5
	+ MiniAVC integration
	+ Some math, control and logic fixes
	+ Coordinated turn mode for Fly-By-Wire
	+ Speed control reworked, support of ias, mach, knots
pseudo-FLC law for ascent of Cruise Flight autopilot
	+ craft settings window can create autopilot context without pressing master switch
	+ moder_cutoff_ias parameter visible in ang vel controllers GUI to tune minimal ias, below wich moderation is turned off
* 2016-0129: 1.4 (Boris-Barboris) for KSP 1.0.5
	+ max_aoa and max_sideslip binding in settings window bugfix
	+ Reduced CPU loading
	+ Fixed MOI calculations
	+ Tweaked regression again to reduce bucking on sustained turns and move a little bit towards v1.2 behaviour
	+ GUI compacting and recoloring
	+ Shift+P hotkey for Autopilot module manager GUI
	+ Cruise flight controller initial implementation
* 2016-0124: 1.3 (Boris-Barboris) for KSP 1.0.5
	+ km_Gimbal version 3.0.6.0 and later support.
	+ various math, model and control fixes.
	+ using .txt for internal data instead of .cfg serialization to not confuse MM's cache.
	+ regression refactoring and improved reliability.
	+ craft settings window for user-friendly tuning.
* 2015-1201: 1.2 (Boris-Barboris) for KSP 1.0.5
	+ Gimbaling speed incorporation in model.
	+ Prograde thrust controller initial implementation.
	+ Acceleration controller initial implementation.
	+ Mouse Director autopilot concept and initial implementation.
	+ Lot's of bugfixing and mathfixing.
* 2015-1111: 1.1 (Boris-Barboris) for KSP 1.0.5
	+ Added moderation button for FbW. Toggles all AoA and G moderation for pitch and yaw.
	+ Default hotkey for FbW is letter O. Can be changed in config.
	+ Decreased sideslip control noise in comparison to v1.0.
	+ Wrong yaw model thrust vector fixed.
	+ Incorrect Flight Model behaviour when Control From part is changed - fixed.
	+ Control surfaces now correctly react to reversed airflow (flying backwards). 
* 2015-1103: 1.0 (Boris-Barboris)
	+ Initial release, containing single Autopilot - Standard Fly-By-Wire.
