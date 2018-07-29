AtmosphereAutopilot :: Change Log
=================================

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
