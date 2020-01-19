# AtmosphereAutopilot :: Changes

* 2020-0119: 1.5.15.1 (Lisias) for 1.4 <= KSP <= 1.7.3
	+ Merging updates from upstream
		- 1.5.12
			- Adds Tgt and Wpt buttons to the Cruise Flight controller waypoint display.  Copies the current lon/lat of the Target or Waypoint, if one is selected, to the lon/lat display and begins navigation to the waypoint.
		- 1.5.14
			- Fix bug when AA was unable to distinguish landed, splashed and flying situations, causing uncontrollability after model collapse.
		- 1.5.15
			- Do not replace control surface module for parts that contain "propeller" or "blade" in their name.
	+ Using KSPe facilities
		- Logging
		- Abstract File System
		- Installment check
	+ Moving the whoel shebang to the `net.lisias.ksp` vendor hierarchy to prevent clashes on the mainstream.
