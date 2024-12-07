param(
    $shouldZip = 'False'
)

# Define project paths
$desktopProjectPath = "ModdingTool.csproj"
$cliProjectPath = "ModdingTool_CLI/ModdingTool_CLI.csproj"

# Define output directories
$desktopOutputDir = "bin/Release/net7.0-windows10.0.17763.0/win-x64/publish"
$cliOutputDir = "ModdingTool_CLI/bin/Release/net7.0-windows10.0.17763.0/win-x64/publish"
$combinedOutputDir = "CombinedOutput"

# Publish the desktop application
dotnet publish $desktopProjectPath -c Release -r win-x64

# Publish the CLI application
dotnet publish $cliProjectPath -c Release -r win-x64

# Create the combined output directory
if (-Not (Test-Path -Path $combinedOutputDir)) {
    New-Item -ItemType Directory -Path $combinedOutputDir
}

# Copy the published files to the combined output directory
Copy-Item -Path "$cliOutputDir\*" -Destination $combinedOutputDir -Recurse -Force
Copy-Item -Path "$desktopOutputDir\*" -Destination $combinedOutputDir -Recurse -Force

if ($shouldZip -eq 'True') {
    Write-Host "`n`Generate Release ZIP`n" -ForegroundColor Magenta
    Remove-item ModdingTool.zip -erroraction 'silentlycontinue'
    Compress-Archive -Path "./$combinedOutputDir/*"  -DestinationPath "ModdingTool.zip"
}

Write-Host "Publishing and combining completed. Output is in the '$combinedOutputDir' directory."

