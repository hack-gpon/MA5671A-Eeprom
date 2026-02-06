# MA5671A EEPROM Editor

A cross-platform desktop application for editing MA5671A SFP module EEPROM data (A0 and A2 addresses).

## Features

- ?? Edit EEPROM fields for MA5671A SFP modules
- ?? Support for both A0 and A2 EEPROM addresses
- ?? Cross-platform support (Windows, Linux, macOS)
- ?? Modern Fluent UI design with Avalonia

## Screenshots

<!-- Add screenshots here -->

## Requirements

- **From Releases**: Nessun requisito - l'applicazione è self-contained con .NET incluso
- **Build from Source**: .NET 8.0 SDK

## Installation

### From Releases (Recommended)

Download the latest release for your platform from the [Releases](../../releases) page. No .NET installation required!

### Build from Source

1. Clone the repository:
   ```bash
   git clone https://github.com/yourusername/MA5671A_EEPROM_A0A2_VS.git
   cd MA5671A_EEPROM_A0A2_VS
   ```

2. Build the project:
   ```bash
   dotnet build -c Release
   ```

3. Run the application:
   ```bash
   dotnet run --project HackGpon.MA5671A.Eeprom.Avalonia
   ```

## Usage

1. Launch the application
2. Load your EEPROM dump file or enter hex values manually
3. Edit the fields as needed
4. Export the modified EEPROM data

## Technologies

- [.NET 8](https://dotnet.microsoft.com/)
- [Avalonia UI](https://avaloniaui.net/) - Cross-platform UI framework
- [CommunityToolkit.Mvvm](https://learn.microsoft.com/en-us/dotnet/communitytoolkit/mvvm/) - MVVM toolkit

## License

This project is open source. See the LICENSE file for details.

## Contributing

Contributions are welcome! Please feel free to submit a Pull Request.

## Related Projects

- [hack-gpon.org](https://hack-gpon.org/) - Resources for GPON hacking
