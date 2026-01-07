# Clean Script for GameLauncher
# This script removes all build artifacts and resets the project to a clean state.

Write-Host "Cleaning build artifacts..." -ForegroundColor Cyan

# Remove the publish directory if it exists
if (Test-Path ".\publish") {
    Remove-Item -Recurse -Force ".\publish"
    Write-Host "Removed publish directory." -ForegroundColor Green
} else {
    Write-Host "No publish directory found." -ForegroundColor Yellow
}

# Remove the bin and obj directories
$directories = @(".\bin", ".\obj")
foreach ($dir in $directories) {
    if (Test-Path $dir) {
        Remove-Item -Recurse -Force $dir
        Write-Host "Removed $dir directory." -ForegroundColor Green
    } else {
        Write-Host "No $dir directory found." -ForegroundColor Yellow
    }
}

Write-Host "Clean complete!" -ForegroundColor Green