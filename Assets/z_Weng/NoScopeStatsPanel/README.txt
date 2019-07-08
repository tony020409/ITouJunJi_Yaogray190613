==============================================
 _____
|   | |___
| | | | . |
|_|___|___|
 _____
|   __|___ ___ ___ ___
|__   |  _| . | . | -_|
|_____|___|___|  _|___|
              |_|
 _____ _       _          _____             _
|   __| |_ ___| |_ ___   |  _  |___ ___ ___| |
|__   |  _| .'|  _|_ -|  |   __| .'|   | -_| |
|_____|_| |__,|_| |___|  |__|  |__,|_|_|___|_|

==============================================
v1.2.1
Tarfmagougou Games <tarfmagougou@gmail.com>


------------
INSTRUCTIONS
------------
1) Open the panel: Window > NoScope Stats Panel.
2) Probably dock it.
3) Select an object(s) in the hierarchy.
4) Read stats.
5) NoScope heavy assets.


-----------
DESCRIPTION
-----------
This extension allows you to drill down your hierarchy to find problematic
and resource heavy assets. It is useful when profiling a game at a late
stage of development, or porting to less powerful platforms.

The Unity3D stats panel is great, but finding what is causing so many draw calls,
or what is using so much heap space, can be daunting. NoScope Stats Panel
helps you do that. Then you can 360 noscope the hell outta those assets ;)


---------------
VERSION HISTORY
---------------
v1.2.1	- Fix errors in Unity >= 5.5. Profiler is now in seperate namespace.

v1.2	- Add audio memory stats (we now have all memory stats from profiler APIs).
		- Add 2D sprite textures and alphasplits to texture memory stats.
		- Add 2D packing info and triangles.
		- Backported to Unity 5.0 (scene fixes, extract #ifdefs to helper). Some
				memory statistics aren't available in pre-Unity 5.3.
		- Fix counting textures multiple times (unity textures are shared).

v1.1	- UI update (icons, shorter button names, more info, hopefully clearer).
		- Add specific memory info (Meshes, Materials, Textures, AnimationClips).
		- Add AnimationClips memory usage.
		- Add logging button. This outputs a report with all info + instance names.
		- Report is copied to clipboard. Ready to use in your client reports.
		- Add static batching info.
		- Add particle triangles to total tris (2 tris per plane).
		- Add disable cache option.
		- Fix counting static batched meshes multiple times.
		- Fix crashes.

v1.0	- Initial Release


----
TODO
----
Here is a public todo list, so you know if your desired feature is planned.

- Recurse on Selection.objects to gather stats on selected hierarchy assets too.
- How does Unity Memory Profiling get script memory references? If at all humanly
	possible, figure out a way to get custom components' references to memory. That
	would be a killer feature (99% improbable).
