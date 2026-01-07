# Game Launcher Build Script
# This script generates a standalone single-file executable for Windows.

Write-Host "Cleaning previous builds..." -ForegroundColor Cyan
if (Test-Path ".\publish") { Remove-Item -Recurse -Force ".\publish" }

Write-Host "Publishing Game Launcher..." -ForegroundColor Cyan
dotnet publish -c Release -r win-x64 --self-contained true -p:PublishSingleFile=true -p:IncludeNativeLibrariesForSelfExtract=true -p:PublishReadyToRun=true -o .\publish

Write-Host "Build Complete!" -ForegroundColor Green
Write-Host "Executable location: .\publish\GameLauncher.exe" -ForegroundColor Yellow
