AtmosphereAutopilot :: Change Log
=================================

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
