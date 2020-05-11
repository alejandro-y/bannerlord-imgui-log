# ImguiLogModule

Mount & Blade II: Bannerlord mod to display log entries in a separate window.

Differences to Native:

* In Native log messages disappear after some time.
* Native log has a lot of spam messages like "$Lord has been taken prisoner".
* Native log does not retain messages from Missions, in particular messages in battle that you/your companion gained a skill point.

Hotkey (currently not configurable): NumpadMinus.

# Installation

Git clone, build (see below), run the game launcher and enable the module.

# Usage

Press NumpadMinus on a campaign map to toggle the log window.

# Development

Works fine with VS2019 Community Edition.

VSCode + .NET Core SDK + `dotnet build` @ powershell should be fine too.

Copy `env.example.xml` to `env.xml` and edit the settings according to your environment. Watch out for the ampersand in XML files.

The `PostBuild.ps1` script will auto execute on successful builds, and assemble the final distributable folder of the module inside the `.\dist` directory as well as install it to the game dir.

To build using CLI:
```ps1
PS C:\path-to-src\> dotnet build -c Debug # or Release
```

# Credits

Project structure inspired by https://github.com/haggen/bannerlord-module-template & https://github.com/Tyler-IN/MnB2-Bannerlord-CommunityPatch.

# Legal

© 2020 alejandro-y

This modification is not created by, affiliated with or sponsored by TaleWorlds Entertainment or its affiliates. The Mount & Blade II Bannerlord API and related logos are intelectual property of TaleWorlds Entertainment. All rights reserved.
