using CommunityToolkit.Mvvm.ComponentModel;

namespace HackGpon.MA5671A.Eeprom.Avalonia.Utils;

/// <summary>
/// Specifies the available EEPROM device addresses for hardware communication.
/// </summary>
/// <remarks>Use this enumeration to select the appropriate EEPROM address when interacting with devices that
/// support multiple address options, such as A0 or A2. The specific address to use depends on the hardware
/// configuration and the requirements of the connected device.</remarks>
public enum Eeprom
{
    A0,
    A2
}

/// <summary>
/// Defines an EEPROM field structure (read-only definition)
/// </summary>
public partial class EepromField : ObservableObject
{
    [ObservableProperty]
    private int startAddress;
    
    [ObservableProperty]
    private int size;
    
    [ObservableProperty]
    private string name = string.Empty;
    
    [ObservableProperty]
    private string defaultValue = string.Empty;
    
    [ObservableProperty]
    private string description = string.Empty;
}

/// <summary>
/// Represents an EEPROM field with its current value (editable)
/// </summary>
public partial class EepromFieldValue : ObservableObject
{
    [ObservableProperty]
    private EepromField field = null!;
    
    [ObservableProperty]
    private string value = string.Empty;

    /// <summary>Display label showing address range and field name</summary>
    public string Label => $"{Field.StartAddress}-{Field.StartAddress + Field.Size - 1} ({Field.Size}) - {Field.Name}";
    
    /// <summary>Maximum input length (2 hex chars per byte)</summary>
    public int MaxLength => Field.Size * 2;
}
