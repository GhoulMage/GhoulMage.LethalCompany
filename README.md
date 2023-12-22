GhoulMage.LethalCompany v1.0.0 (stable)
==============

Common API for some Lethal Company plugins. Free to use.<br>

Features
--------
* GhoulMagePlugin - Abstract class that automatically manages compatibility with the game (see LethalGameVersions class)
* LC_Info - Info about the game. For now just convenient shortcuts for the game version and check if a plugin is loaded.
* Templates for quickly creating a GhoulMagePlugin using VSCode and Visual Studio<br>

Soft Dependencies
--------
* LC_API - For correctly fetching the game version if ModdedServer.ModdedOnly is true.<br>

Bugs
--------
* Shouldn't have.
<br>
<br>
Report any bugs, improvements, incompatibilities or push request the appropiate fixes and I'll look into them.<br>

Change-log
--------
* 2.0.0
    * Updated to consider LC_API 3.0.0
    * Changed uint Version to string Version (major.minor.patch format) in the GhoulMagePlugin class. Currently 1.0.1 due to no real breaking changes
    * Renamed class 'LethalApp' to 'LC_Info' to better reflect the class' responsibility.
    * Move 'VersionPrefix' from LethalGameVersions to 'LC_Info'
    * Added const string APIVersion in GhoulMagePlugin class to quickly refer to the GhoulMage.LethalCompany Assembly version.
    * Added some XML comments on some stuff for better documentation.<br>

* 1.0.0
    * First Version<br>

For Players
-----------
Go into Releases and download the latest version.<br>

For Devs
--------
1. **There are templates for vscode and visual studio**<br>
<br>

* GhoulMagePlugin.Version refers to GhoulMagePlugin's api.
* GhoulMagePlugin.APIVersion refers to the Assembly's api.
* After important api changes, GhoulMagePlugin.Version and GhoulMagePlugin.APIVersion will update accordingly.<br>

2. **Dependencies**
* Assembly-CSharp.dll (From Lethal Company_Data/Managed)
* UnityEngine.dll
<br>

3. **Build**
* Build normaly using dotnet or whatever.
<br>

4. **Copyright**
* MIT License. For details see LICENSE
<br>
