```
 _   _               _       ____  ____    ___   _   _ 
| | | |  __ _   ___ | | __  / ___||  _ \  / _ \ | \ | |
| |_| | / _` | / __|| |/ / | |  _ | |_) || | | ||  \| |
|  _  || (_| || (__ |   <  | |_| ||  __/ | |_| || |\  |
|_| |_| \__,_| \___||_|\_\  \____||_|     \___/ |_| \_|
```

# MA5671A EEPROM Editor
cross-platform desktop application for editing the EEPROM of the Huawei MA5671A SFP module (A0 and A2 addresses)

features
- edit the EEPROM fields of the MA5671A SFP module
- support for both A0 and A2 EEPROM addresses
- load an EEPROM dump or enter hex values manually, then export the modified data
- cross-platform (Windows, Linux, macOS) with a Fluent UI built on [Avalonia](https://avaloniaui.net/)

## Download
Prebuilt self-contained executables for Windows (x64, arm64), Linux (x64, arm64) and macOS (x64, arm64) are available in the [releases](https://github.com/hack-gpon/MA5671A-Eeprom/releases). No .NET installation required.

## Usage
1. Launch the application
2. Load your EEPROM dump file or enter hex values manually
3. Edit the fields as needed
4. Export the modified EEPROM data

## Build
Requires the [.NET 10 SDK](https://dotnet.microsoft.com/download/dotnet/10.0).
```
dotnet run --project HackGpon.MA5671A.Eeprom.Avalonia
```
```
dotnet publish HackGpon.MA5671A.Eeprom.Avalonia/HackGpon.MA5671A.Eeprom.Avalonia.csproj -c Release -r <win-x64|win-arm64|linux-x64|linux-arm64|osx-x64|osx-arm64>
```

.NET 10.0 + Avalonia UI + self-contained single file

## Release
Releases are created automatically by CI on every push to `main`: the version is read from `<Version>` in `HackGpon.MA5671A.Eeprom.Avalonia.csproj`, and if the `v<Version>` tag does not exist yet it is created together with the GitHub release. Bump `<Version>` to publish a new release.

## License
See [LICENSE.txt](LICENSE.txt).

More resources on [hack-gpon.org](https://hack-gpon.org/).
