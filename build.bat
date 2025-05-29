@echo off
echo Building GMInk DLL for GameMaker Studio...
echo.

REM Check if .NET is installed
dotnet --version >nul 2>&1
if errorlevel 1 (
    echo ERROR: .NET SDK is not installed or not in PATH
    echo Please install .NET 8.0 SDK from https://dotnet.microsoft.com/download
    pause
    exit /b 1
)

echo .NET SDK found: 
dotnet --version
echo.

REM Clean previous builds
echo Cleaning previous builds...
if exist "bin" rmdir /s /q "bin"
if exist "obj" rmdir /s /q "obj"
echo.

REM Change to project directory
cd GMInk

REM Restore packages
echo Restoring NuGet packages...
dotnet restore
if errorlevel 1 (
    echo ERROR: Failed to restore packages
    pause
    exit /b 1
)
echo.

REM Build x86 version (32-bit)
echo Building x86 (32-bit) version...
dotnet build -c Release -p:Platform=x86
if errorlevel 1 (
    echo ERROR: Failed to build x86 version
    pause
    exit /b 1
)
echo x86 build completed successfully!
echo.

REM Build x64 version (64-bit)
echo Building x64 (64-bit) version...
dotnet build -c Release -p:Platform=x64
if errorlevel 1 (
    echo ERROR: Failed to build x64 version
    pause
    exit /b 1
)
echo x64 build completed successfully!
echo.

REM Show output locations
echo Build completed successfully!
echo.
echo Output files:
echo - x86 (32-bit): bin\x86\Release\net8.0\GMInk.dll
echo - x64 (64-bit): bin\x64\Release\net8.0\GMInk.dll
echo.

REM Copy to common output directory
cd ..
if not exist "output" mkdir "output"
copy "GMInk\bin\x86\Release\net8.0\GMInk.dll" "output\GMInk_x86.dll" >nul
copy "GMInk\bin\x64\Release\net8.0\GMInk.dll" "output\GMInk_x64.dll" >nul

echo DLLs copied to output directory:
echo - output\GMInk_x86.dll (for 32-bit GameMaker)
echo - output\GMInk_x64.dll (for 64-bit GameMaker)
echo.

echo Build process complete!
pause