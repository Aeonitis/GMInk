@echo off
echo GMInk Build Diagnostics
echo =======================
echo.

REM Check .NET version
echo Checking .NET SDK...
dotnet --version
if errorlevel 1 (
    echo ERROR: .NET SDK not found
    pause
    exit /b 1
)
echo.

REM Check NuGet sources
echo Checking NuGet sources...
dotnet nuget list source
echo.

REM Check project file
echo Checking project file...
cd GMInk
if not exist "GMInk.csproj" (
    echo ERROR: GMInk.csproj not found in GMInk directory
    cd ..
    pause
    exit /b 1
)

echo Project file found. Contents:
type GMInk.csproj
echo.

REM Try restore with verbose output
echo Attempting package restore with verbose output...
dotnet restore --verbosity detailed
echo.

REM Check specific packages
echo Checking package availability...
echo Searching for Ink package...
dotnet package search Ink --take 5
echo.
echo Searching for DllExport package...
dotnet package search DllExport --take 5
echo.

cd ..
echo Diagnostics complete.
pause