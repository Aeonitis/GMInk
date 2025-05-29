# GMInk Build Script for Windows
Write-Host "Building GMInk DLL for GameMaker Studio..." -ForegroundColor Green
Write-Host ""

# Check if .NET is installed
try {
    $dotnetVersion = dotnet --version
    Write-Host ".NET SDK found: $dotnetVersion" -ForegroundColor Yellow
    Write-Host ""
}
catch {
    Write-Host "ERROR: .NET SDK is not installed or not in PATH" -ForegroundColor Red
    Write-Host "Please install .NET 8.0 SDK from https://dotnet.microsoft.com/download" -ForegroundColor Red
    Read-Host "Press Enter to exit"
    exit 1
}

# Clean previous builds
Write-Host "Cleaning previous builds..." -ForegroundColor Yellow
if (Test-Path "bin") { Remove-Item -Recurse -Force "bin" }
if (Test-Path "obj") { Remove-Item -Recurse -Force "obj" }
Write-Host ""

# Change to project directory
Set-Location "GMInk"

# Restore packages
Write-Host "Restoring NuGet packages..." -ForegroundColor Yellow
$restoreResult = dotnet restore
if ($LASTEXITCODE -ne 0) {
    Write-Host "ERROR: Failed to restore packages" -ForegroundColor Red
    Read-Host "Press Enter to exit"
    exit 1
}
Write-Host ""

# Build x86 version (32-bit)
Write-Host "Building x86 (32-bit) version..." -ForegroundColor Yellow
$buildResult = dotnet build -c Release -p:Platform=x86
if ($LASTEXITCODE -ne 0) {
    Write-Host "ERROR: Failed to build x86 version" -ForegroundColor Red
    Read-Host "Press Enter to exit"
    exit 1
}
Write-Host "x86 build completed successfully!" -ForegroundColor Green
Write-Host ""

# Build x64 version (64-bit)
Write-Host "Building x64 (64-bit) version..." -ForegroundColor Yellow
$buildResult = dotnet build -c Release -p:Platform=x64
if ($LASTEXITCODE -ne 0) {
    Write-Host "ERROR: Failed to build x64 version" -ForegroundColor Red
    Read-Host "Press Enter to exit"
    exit 1
}
Write-Host "x64 build completed successfully!" -ForegroundColor Green
Write-Host ""

# Show output locations
Write-Host "Build completed successfully!" -ForegroundColor Green
Write-Host ""
Write-Host "Output files:" -ForegroundColor Yellow
Write-Host "- x86 (32-bit): bin\x86\Release\net8.0\GMInk.dll"
Write-Host "- x64 (64-bit): bin\x64\Release\net8.0\GMInk.dll"
Write-Host ""

# Copy to common output directory
Set-Location ".."
if (!(Test-Path "output")) { New-Item -ItemType Directory -Path "output" }
Copy-Item "GMInk\bin\x86\Release\net8.0\GMInk.dll" "output\GMInk_x86.dll"
Copy-Item "GMInk\bin\x64\Release\net8.0\GMInk.dll" "output\GMInk_x64.dll"

Write-Host "DLLs copied to output directory:" -ForegroundColor Green
Write-Host "- output\GMInk_x86.dll (for 32-bit GameMaker)"
Write-Host "- output\GMInk_x64.dll (for 64-bit GameMaker)"
Write-Host ""

Write-Host "Build process complete!" -ForegroundColor Green
Read-Host "Press Enter to exit"