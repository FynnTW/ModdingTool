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

## Download

* [Nightly Build](https://nightly.link/FynnTW/ModdingTool/workflows/build-modding-tool/master/M2TW-Modding-Tool.zip)

## Build

#### [Install .NET 7.0 and verify installation](https://dotnet.microsoft.com/en-us/download/dotnet/7.0)

`dotnet --version`

#### Install the dependencies
`dotnet restore`

#### Build the executable

`dotnet publish -c Release --no-restore`

#### Run the executable
`.\bin\Release\net7.0-windows10.0.17763.0\win-x64\publish\ModdingTool.exe`

## Screenshots
![](https://i.imgur.com/dneVvyt.png)
![](https://i.imgur.com/qK9CPyV.png)
![](https://i.imgur.com/5M9LVTB.png)

## Technology
- .NET Framework
- C#

## Credits
- FynnTW
