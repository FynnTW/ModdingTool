<h1 align="center">
  Medieval 2: Total War - Modding Tool
</h1>

![](https://i.imgur.com/pS2ChZI.png)

-----------------

The following is a modding tool for the game Medieval II: Total War.

**NOTE: This project is currently a Work In Progress. While the tool shouldn't cause any harm to your build, if you haven't already made backups of any files you care about, do it NOW before using this tool.**

## Features
- Fast and lightweight
- GUI for easy editing of EDU and BMDB
- Filter and sorting functionality of EDU units
- Detailed error log for common mistakes in multiple files
- Import/Export BMDB as JSON

## Build
Open `Developer Powershell for Visual Studio 2022` and run the following command

```powershell
 msbuild -t:restore
```

```powershell
 msbuild
```

## Usage
1. Open `ModdingTool.exe` in the `bin\Debug\net7.0-windows` directory.
2. `File -> Load Mod` and select your mod's folder

## Screenshots
![](https://i.imgur.com/dneVvyt.png)
![](https://i.imgur.com/qK9CPyV.png)
![](https://i.imgur.com/5M9LVTB.png)

## Technology
- .NET Framework
- C#

## Credits
- FynnTW
