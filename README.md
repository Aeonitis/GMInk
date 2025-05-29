# GMInk

C# DLL bridge for using Inkle's INK narrative scripting language in GameMaker Studio.

## Features

- Story loading and continuation
- Choice handling
- Variable manipulation (get/set)
- State save/load
- Tag support (current, global, path-specific)
- Variable observers
- External function binding
- Visit count tracking

## Requirements

- Windows (for building and usage)
- .NET 8.0 SDK
- GameMaker Studio (x86 or x64)

## Building

### Batch Script
```bash
build.bat
```

### PowerShell
```powershell
.\build.ps1
```

### Manual
```bash
cd GMInk
dotnet restore
dotnet build -c Release -p:Platform=x86
dotnet build -c Release -p:Platform=x64
```

## Output

- `output/GMInk_x86.dll` - 32-bit GameMaker
- `output/GMInk_x64.dll` - 64-bit GameMaker

## Usage

Copy the appropriate DLL to your GameMaker project and use the exported functions:

### Core Functions
- `Load(file)` - Load INK JSON file
- `CanContinue()` - Check if story can continue
- `Continue()` - Get next story content
- `CurrentChoicesCount()` - Number of available choices
- `CurrentChoice(index)` - Get choice text
- `ChooseChoiceIndex(index)` - Select choice

### State Management
- `SaveState()` - Get JSON state
- `LoadState(json)` - Load JSON state
- `ChoosePathString(path)` - Jump to story path

### Variables
- `VariableGetReal(name)` - Get numeric variable
- `VariableGetString(name)` - Get string variable
- `VariableSetReal(name, value)` - Set numeric variable
- `VariableSetString(name, value)` - Set string variable

### Tags
- `TagCount()` - Current tags count
- `GetTag(index)` - Get current tag
- `GlobalTagCount()` - Global tags count
- `GlobalTag(index)` - Get global tag
- `TagForContentAtPathCount(path)` - Path tags count
- `TagForContentAtPath(path, index)` - Get path tag

### Advanced
- `ObserveVariable(name, script)` - Watch variable changes
- `BindExternal(name, script)` - Bind external function
- `VisitCountAtPathString(path)` - Get visit count
- `RegisterCallbacks(...)` - Initialize GML callbacks

## Dependencies

- [Ink](https://www.nuget.org/packages/Ink/) - Official INK runtime
- [DllExport](https://www.nuget.org/packages/DllExport/) - Native export support

## License

Based on original GMInk extension for GameMaker Studio.