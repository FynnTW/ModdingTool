<h1 align="center">
  Medieval 2: Total War - Modding Tool
</h1>

![](https://i.imgur.com/pS2ChZI.png)

---

The following is a modding tool for the game Medieval II: Total War.

**NOTE: This project is currently a Work In Progress. While the tool shouldn't cause any harm to your build, if you haven't already made backups of any files you care about, do it NOW before using this tool.**

## Features

- Fast and lightweight
- GUI for easy editing of EDU and BMDB
- CLI for import/export EDU, EDB and BMDB in json formats
- Run custom Lua scripts on the data with documented Lua plugin
- Filter and sorting functionality of EDU units
- Detailed error log for common mistakes in multiple files

## Download

- [Nightly Build](https://nightly.link/FynnTW/ModdingTool/workflows/build-modding-tool/master/M2TW-Modding-Tool.zip)

## Build

#### [Install .NET 7.0 and verify installation](https://dotnet.microsoft.com/en-us/download/dotnet/7.0)

`dotnet --version`

#### Install the dependencies

`dotnet restore`

#### Build the executable

`./Build.ps1`

#### Run the executable

`.\CombinedOutput\ModdingTool.exe`

## Screenshots

![](https://imgur.com/xhLKsC6)
![](https://imgur.com/D5n3ppz)
![](https://imgur.com/0f2MuPi)
![](https://imgur.com/BGR2M9q)

## Technology

- .NET Framework
- C#

## Credits

- FynnTW
