using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using HackGpon.MA5671A.Eeprom.Avalonia.Utils;
using MsBox.Avalonia;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace HackGpon.MA5671A.Eeprom.Avalonia.ViewModels;

public partial class EEPROMViewModel : ObservableObject
{
    [ObservableProperty]
    private MainViewModel mainViewModel;

    private static Dictionary<Utils.Eeprom, List<EepromField>> EepromFieldDefinitions =
    new Dictionary<Utils.Eeprom, List<EepromField>>{
        { Utils.Eeprom.A0, [
            // BASE ID FIELDS (SFF-8472)
            new EepromField { StartAddress = 0, Size = 1, Name = "Identifier", DefaultValue = "0x03 (SFP)", Description = "Type of transceiver" },
            new EepromField { StartAddress = 1, Size = 1, Name = "Ext identifier", DefaultValue = "0x04 (MOD_DEF 4)", Description = "Additional information about the transceiver" },
            new EepromField { StartAddress = 2, Size = 1, Name = "Connector", DefaultValue = "0x01 (SC)", Description = "Type of media connector" },
            new EepromField { StartAddress = 3, Size = 8, Name = "Transceiver", DefaultValue = "0x00 0x00 0x00 0x00 0x00 0x00 0x00 0x00", Description = "Code for optical compatibility" },
            new EepromField { StartAddress = 11, Size = 1, Name = "Encoding", DefaultValue = "0x03 (NRZ)", Description = "High speed serial encoding algorithm" },
            new EepromField { StartAddress = 12, Size = 1, Name = "Signaling Rate, Nominal", DefaultValue = "0x0C (1.244Gbps)", Description = "Nominal signaling rate" },
            new EepromField { StartAddress = 13, Size = 1, Name = "Rate Identifier", DefaultValue = "0x00 (Not used)", Description = "Type of rate select functionality" },
            new EepromField { StartAddress = 14, Size = 1, Name = "Length (SMF,km)", DefaultValue = "0x14 (20 km)", Description = "Link length supported for single-mode fiber, units of km" },
            new EepromField { StartAddress = 15, Size = 1, Name = "Length (SMF)", DefaultValue = "0xC8 (200 x 100m)", Description = "Link length supported for single-mode fiber, units of 100 m" },
            new EepromField { StartAddress = 16, Size = 1, Name = "Length (50 um, OM2)", DefaultValue = "0x00 (No support)", Description = "Link length supported for 50 um OM2 fiber, units of 10 m" },
            new EepromField { StartAddress = 17, Size = 1, Name = "Length (62.5 um, OM1)", DefaultValue = "0x00 (No support)", Description = "Link length supported for 62.5 um OM1 fiber, units of 10 m" },
            new EepromField { StartAddress = 18, Size = 1, Name = "Length copper cable", DefaultValue = "0x00 (No support)", Description = "Link length supported for copper or direct attach cable, units of m" },
            new EepromField { StartAddress = 19, Size = 1, Name = "Length (50 um, OM3)", DefaultValue = "0x00 (No support)", Description = "Link length supported for 50 um OM3 fiber, units of 10 m" },
            new EepromField { StartAddress = 20, Size = 16, Name = "Vendor name", DefaultValue = "0x48 0x55 0x41 0x57 0x45 0x49 0x20 0x20 0x20 0x20 0x20 0x20 0x20 0x20 0x20 0x20 (HUAWEI)", Description = "SFP vendor name (ASCII)" },
            new EepromField { StartAddress = 36, Size = 1, Name = "Transceiver", DefaultValue = "0x00 (No support)", Description = "Code for optical compatibility" },
            new EepromField { StartAddress = 37, Size = 3, Name = "Vendor OUI", DefaultValue = "0x00 0x00 0x00 (No specified)", Description = "SFP vendor IEEE company ID" },
            new EepromField { StartAddress = 40, Size = 16, Name = "Vendor PN", DefaultValue = "0x4D 0x41 0x35 0x36 0x37 0x31 0x41 0x20 0x20 0x20 0x20 0x20 0x20 0x20 0x20 0x20 (MA5671A)", Description = "Part number provided by SFP vendor (ASCII)" },
            new EepromField { StartAddress = 56, Size = 4, Name = "Vendor rev", DefaultValue = "0x30 0x30 0x30 0x30 (0000)", Description = "Revision level for part number provided by vendor (ASCII)" },
            new EepromField { StartAddress = 60, Size = 2, Name = "Wavelength", DefaultValue = "0x05 0x1E (1310nm TX)", Description = "Laser wavelength" },
            new EepromField { StartAddress = 62, Size = 1, Name = "Fibre Channel Speed 2", DefaultValue = "0x00 (No support)", Description = "Transceiver’s Fibre Channel speed capabilities" },
            new EepromField { StartAddress = 63, Size = 1, Name = "CC_BASE", DefaultValue = "", Description = "Check code for Base ID Fields (addresses 0 to 62)" },

            // EXTENDED ID FIELDS (SFF-8472)
            new EepromField { StartAddress = 64, Size = 2, Name = "Options", DefaultValue = "0x00 0x1A (TX DISABLE, TX FAULT, RX LOS)", Description = "Indicates which optional transceiver signals are implemented" },
            new EepromField { StartAddress = 66, Size = 1, Name = "Signaling Rate, max", DefaultValue = "0x00 (No specified)", Description = "Upper signaling rate margin, units of %" },
            new EepromField { StartAddress = 67, Size = 1, Name = "Signaling Rate, min", DefaultValue = "0x00 (No specified)", Description = "Lower signaling rate margin, units of %" },
            new EepromField { StartAddress = 68, Size = 16, Name = "Vendor SN", DefaultValue = "Unique in each SFP", Description = "Serial number provided by vendor (ASCII)" },
            new EepromField { StartAddress = 84, Size = 8, Name = "Date code", DefaultValue = "Unique in each SFP", Description = "Vendor’s manufacturing date code" },
            new EepromField { StartAddress = 92, Size = 1, Name = "Diagnostic Monitoring Type", DefaultValue = "0x68 (Digital diagnostic, Internally calibrated, Received average power type)", Description = "Indicates which type of diagnostic monitoring is implemented" },
            new EepromField { StartAddress = 93, Size = 1, Name = "Enhanced Options", DefaultValue = "0xE0 (Alarm/warning flags, soft TX_DISABLE control, soft TX_FAULT monitoring)", Description = "Indicates which optional enhanced features are implemented" },
            new EepromField { StartAddress = 94, Size = 1, Name = "SFF-8472 Compliance", DefaultValue = "0x03 (Rev 10.2 of SFF-8472)", Description = "Indicates which revision of SFF-8472 the transceiver complies with" },
            new EepromField { StartAddress = 95, Size = 1, Name = "CC_EXT", DefaultValue = "", Description = "Check code for the Extended ID Fields (addresses 64 to 94)" },

            // VENDOR SPECIFIC FIELDS
            new EepromField { StartAddress = 96, Size = 32, Name = "Vendor data", DefaultValue = "Not sure if it’s unique or not", Description = "Vendor specific data (ASCII)" },


            ] }, { Utils.Eeprom.A2, [
                new EepromField { StartAddress = 0, Size = 2, Name = "Temp High Alarm", DefaultValue = "0x5F 0x00 (95?)", Description = "Value expressed in two’s complement" },
                new EepromField { StartAddress = 2, Size = 2, Name = "Temp Low Alarm", DefaultValue = "0xCE 0x00 (-50?)", Description = "Value expressed in two’s complement" },
                new EepromField { StartAddress = 4, Size = 2, Name = "Temp High Warning", DefaultValue = "0x5A 0x00 (90?)", Description = "Value expressed in two’s complement" },
                new EepromField { StartAddress = 6, Size = 2, Name = "Temp Low Warning", DefaultValue = "0xD3 0x00 (-45?)", Description = "Value expressed in two’s complement" },
                new EepromField { StartAddress = 8, Size = 2, Name = "Voltage High Alarm", DefaultValue = "0x8C 0xA0 (3.6V)", Description = "Value expressed in volt subunits1" },
                new EepromField { StartAddress = 10, Size = 2, Name = "Voltage Low Alarm", DefaultValue = "0x75 0x30 (3.0V)", Description = "Value expressed in volt subunits1" },
                new EepromField { StartAddress = 12, Size = 2, Name = "Voltage High Warning", DefaultValue = "0x88 0xB8 (3.5V)", Description = "Value expressed in volt subunits1" },
                new EepromField { StartAddress = 14, Size = 2, Name = "Voltage Low Warning", DefaultValue = "0x79 0x18 (3.1V)", Description = "Value expressed in volt subunits1" },
                new EepromField { StartAddress = 16, Size = 2, Name = "Bias High Alarm", DefaultValue = "0xAF 0xC8 (90mA)", Description = "Value expressed in milliampere subunits1" },
                new EepromField { StartAddress = 18, Size = 2, Name = "Bias Low Alarm", DefaultValue = "0x00 0x00 (0mA)", Description = "Value expressed in milliampere subunits1" },
                new EepromField { StartAddress = 20, Size = 2, Name = "Bias High Warning", DefaultValue = "0x88 0xB8 (70mA)", Description = "Value expressed in milliampere subunits1" },
                new EepromField { StartAddress = 22, Size = 2, Name = "Bias Low Warning", DefaultValue = "0x00 0x00 (0mA)", Description = "Value expressed in milliampere subunits1" },
                new EepromField { StartAddress = 24, Size = 2, Name = "TX Power High Alarm", DefaultValue = "0x9B 0x82 (6dBm)", Description = "Value expressed in watts subunits1" },
                new EepromField { StartAddress = 26, Size = 2, Name = "TX Power Low Alarm", DefaultValue = "0x22 0xD0 (-1dBm)", Description = "Value expressed in watts subunits1" },
                new EepromField { StartAddress = 28, Size = 2, Name = "TX Power High Warning", DefaultValue = "0x7B 0x86 (5dBm)", Description = "Value expressed in watts subunits1" },
                new EepromField { StartAddress = 30, Size = 2, Name = "TX Power Low Warning", DefaultValue = "0x2B 0xD4 (0dBm)", Description = "Value expressed in watts subunits1" },
                new EepromField { StartAddress = 32, Size = 2, Name = "RX Power High Alarm", DefaultValue = "0x09 0xCF (-6dBm)", Description = "Value expressed in watts subunits1" },
                new EepromField { StartAddress = 34, Size = 2, Name = "RX Power Low Alarm", DefaultValue = "0x00 0x0D (-29dBm)", Description = "Value expressed in watts subunits1" },
                new EepromField { StartAddress = 36, Size = 2, Name = "RX Power High Warning", DefaultValue = "0x07 0xCB (-7dBm)", Description = "Value expressed in watts subunits1" },
                new EepromField { StartAddress = 38, Size = 2, Name = "RX Power Low Warning", DefaultValue = "0x00 0x10 (-28dBm)", Description = "Value expressed in watts subunits1" },
                new EepromField { StartAddress = 40, Size = 6, Name = "MAC address", DefaultValue = "Unique in each SFP", Description = "Contains the mac address of the SFP, it could also be empty" },
                new EepromField { StartAddress = 56, Size = 4, Name = "RX_PWR(4) Calibration", DefaultValue = "0x00 0x00 0x00 0x00", Description = "4th order RSSI calibration coefficient" },
                new EepromField { StartAddress = 60, Size = 4, Name = "RX_PWR(3) Calibration", DefaultValue = "0x00 0x00 0x00 0x00", Description = "3rd order RSSI calibration coefficient" },
                new EepromField { StartAddress = 64, Size = 4, Name = "RX_PWR(2) Calibration", DefaultValue = "0x00 0x00 0x00 0x00", Description = "2nd order RSSI calibration coefficient" },
                new EepromField { StartAddress = 68, Size = 4, Name = "RX_PWR(1) Calibration", DefaultValue = "0x3F 0x80 0x00 0x00", Description = "1st order RSSI calibration coefficient" },
                new EepromField { StartAddress = 72, Size = 4, Name = "RX_PWR(0) Calibration", DefaultValue = "0x00 0x00 0x00 0x00", Description = "0th order RSSI calibration coefficient" },
                new EepromField { StartAddress = 76, Size = 2, Name = "TX_I(Slope) Calibration", DefaultValue = "0x01 0x00", Description = "Slope for Bias calibration" },
                new EepromField { StartAddress = 78, Size = 2, Name = "TX_I(Offset) Calibration", DefaultValue = "0x00 0x00", Description = "Offset for Bias calibration" },
                new EepromField { StartAddress = 80, Size = 2, Name = "TX_PWR(Slope) Calibration", DefaultValue = "0x01 0x00", Description = "Slope for TX Power calibration" },
                new EepromField { StartAddress = 82, Size = 2, Name = "TX_PWR(Offset) Calibration", DefaultValue = "0x00 0x00", Description = "Offset for TX Power calibration" },
                new EepromField { StartAddress = 84, Size = 2, Name = "T(Slope) Calibration", DefaultValue = "0x01 0x00", Description = "Slope for Temperature calibration" },
                new EepromField { StartAddress = 86, Size = 2, Name = "T(Offset) Calibration", DefaultValue = "0x00 0x00", Description = "Offset for Temperature calibration, in units of 256ths °C" },
                new EepromField { StartAddress = 88, Size = 2, Name = "V(Slope) Calibration", DefaultValue = "0x01 0x00", Description = "Slope for VCC calibration" },
                new EepromField { StartAddress = 90, Size = 2, Name = "V(Offset) Calibration", DefaultValue = "0x00 0x00", Description = "Offset for VCC calibration" },
                new EepromField { StartAddress = 95, Size = 1, Name = "CC_DMI", DefaultValue = "", Description = "Check code for Base Diagnostic Fields (addresses 0 to 94)" },
                new EepromField { StartAddress = 96, Size = 1, Name = "Temperature MSB", DefaultValue = "", Description = "Internally measured module temperature" },
                new EepromField { StartAddress = 97, Size = 1, Name = "Temperature LSB", DefaultValue = "", Description = "" },
                new EepromField { StartAddress = 98, Size = 1, Name = "Vcc MSB", DefaultValue = "", Description = "Internally measured supply voltage in transceiver" },
                new EepromField { StartAddress = 99, Size = 1, Name = "Vcc LSB", DefaultValue = "", Description = "" },
                new EepromField { StartAddress = 100, Size = 1, Name = "TX Bias MSB", DefaultValue = "", Description = "Internally measured TX Bias Current" },
                new EepromField { StartAddress = 101, Size = 1, Name = "TX Bias LSB", DefaultValue = "", Description = "" },
                new EepromField { StartAddress = 102, Size = 1, Name = "TX Power MSB", DefaultValue = "", Description = "Measured TX output power" },
                new EepromField { StartAddress = 103, Size = 1, Name = "TX Power LSB", DefaultValue = "", Description = "" },
                new EepromField { StartAddress = 104, Size = 1, Name = "RX Power MSB", DefaultValue = "", Description = "Measured RX input power" },
                new EepromField { StartAddress = 105, Size = 1, Name = "RX Power LSB", DefaultValue = "", Description = "" },
                new EepromField { StartAddress = 106, Size = 4, Name = "Optional Diagnostics", DefaultValue = "0xFF 0xFF 0xFF 0xFF (No support)", Description = "Monitor Data for Optional Laser temperature and TEC current" },
                new EepromField { StartAddress = 110, Size = 1, Name = "Status/Control", DefaultValue = "0x00 (No support)", Description = "Optional Status and Control Bits" },
                new EepromField { StartAddress = 112, Size = 2, Name = "Alarm Flags", DefaultValue = "Supported", Description = "Diagnostic Alarm Flag Status Bits" },
                new EepromField { StartAddress = 114, Size = 1, Name = "Tx Input EQ control", DefaultValue = "0xFF (No support)", Description = "Tx Input equalization level control" },
                new EepromField { StartAddress = 115, Size = 1, Name = "RX Input EQ control", DefaultValue = "0xFF (No support)", Description = "RX Input equalizer settings" },
                new EepromField { StartAddress = 116, Size = 2, Name = "Warning Flags", DefaultValue = "Supported", Description = "Diagnostic Warning Flag Status Bits" },
                new EepromField { StartAddress = 118, Size = 2, Name = "Ext Status/Control", DefaultValue = "0x00 0x00 (No support)", Description = "Extended module control and status bytes" },
                new EepromField { StartAddress = 120, Size = 7, Name = "Vendor Specific", DefaultValue = "0x70 0x00 0x00 0x00 0x00 0x00 0x00", Description = "Vendor specific memory addresses" },
                new EepromField { StartAddress = 127, Size = 1, Name = "Table Select", DefaultValue = "0x00", Description = "Optional Page Select" },
                new EepromField { StartAddress = 191, Size = 24, Name = "GPON LOID or PLOAM", DefaultValue = "Depends on the configuration of the SFP", Description = "GPON Logical ONU ID or PLOAM, depends on GPON LOID/PLOAM switch" },
                new EepromField { StartAddress = 215, Size = 17, Name = "GPON LPWD", DefaultValue = "Depends on the configuration of the SFP", Description = "GPON Logical Password" },
                new EepromField { StartAddress = 232, Size = 1, Name = "GPON LOID/PLOAM switch", DefaultValue = "Depends on the configuration of the SFP", Description = "0x01 to enable LOID, 0x02 to enable PLOAM" },
                new EepromField { StartAddress = 233, Size = 8, Name = "GPON SN", DefaultValue = "Unique in each SFP", Description = "GPON Serial Number (ME 256)" },
                new EepromField { StartAddress = 248, Size = 8, Name = "Vendor Control", DefaultValue = "0xFF 0xFF 0xFF... (Not used)", Description = "Vendor specific control functions" },
                new EepromField { StartAddress = 256, Size = 256, Name = "Unknown vendor specific", DefaultValue = "Probably not used in current SFPs", Description = "Probably not used in current SFPs" },
                new EepromField { StartAddress = 512, Size = 20, Name = "GPON Equipment ID", DefaultValue = "GPON Equipment ID (ME 257), may not work in some firmwares", Description = "GPON Equipment ID (ME 257)" },
                new EepromField { StartAddress = 532, Size = 4, Name = "GPON Vendor ID", DefaultValue = "GPON Vendor ID (ME 256 and more), may not work in some firmware", Description = "GPON Vendor ID (ME 256 and more)" },

                ]  }
    };

    [ObservableProperty]
    private Utils.Eeprom eepromType;


    [ObservableProperty]
    private string hexData = string.Empty;

    [ObservableProperty]
    private ObservableCollection<EepromFieldValue> fields = [];

    /// <summary>
    /// Initializes a new instance of the EEPROMViewModel class with the specified main view model, hexadecimal
    /// data, and EEPROM type.
    /// </summary>
    /// <remarks>The constructor sets the EEPROM type and loads the corresponding fields. The
    /// hexadecimal data is assigned directly to avoid triggering change handlers before the EEPROM type is
    /// set.</remarks>
    /// <param name="mvm">The main view model instance that provides context and data binding for the EEPROM view model.</param>
    /// <param name="hex">A string containing the hexadecimal representation of the EEPROM data to be loaded.</param>
    /// <param name="eeprom">The EEPROM type definition used to determine field layout and supported operations.</param>
    /// <exception cref="ArgumentException">Thrown if the specified EEPROM type is not supported.</exception>
    public EEPROMViewModel(MainViewModel mvm, string hex, Utils.Eeprom eeprom)
    {
        if (!EepromFieldDefinitions.ContainsKey(eeprom))
            throw new ArgumentException("Unsupported EEPROM type");

        MainViewModel = mvm;
        hexData = hex;  // Set backing field directly to avoid triggering change handler before EepromType is set
        EepromType = eeprom;  // This will trigger OnEepromTypeChanged which loads fields
    }

    /// <summary>
    /// When EEPROM type changes, reload field definitions
    /// </summary>
    partial void OnEepromTypeChanged(Utils.Eeprom value)
    {
        if (EepromFieldDefinitions.TryGetValue(value, out var fieldsList))
        {
            ReloadFields(fieldsList);
        }
    }

    /// <summary>
    /// When HexData changes externally, update all field values
    /// </summary>
    partial void OnHexDataChanged(string value)
    {
        if (string.IsNullOrEmpty(value) || Fields.Count == 0)
            return;

        // Update existing field values without recreating the collection
        foreach (var fieldValue in Fields)
        {
            fieldValue.Value = TryReadHex(value, fieldValue.Field.StartAddress, fieldValue.Field.Size);
        }
    }

    /// <summary>
    /// Reloads the collection of EEPROM field values using the specified list of fields.
    /// </summary>
    /// <remarks>Calling this method replaces the current field values with new values read from the
    /// underlying data source. Any previously loaded field values will be cleared before reloading.</remarks>
    /// <param name="fieldsList">A list of EEPROM fields to be reloaded. Cannot be null. Each field in the list determines the address and
    /// size used to read values.</param>
    private void ReloadFields(List<EepromField> fieldsList)
    {
        Fields.Clear();
        foreach (var field in fieldsList)
        {
            var value = TryReadHex(HexData, field.StartAddress, field.Size);
            Fields.Add(new EepromFieldValue { Field = field, Value = value });
        }
    }

    /// <summary>
    /// Attempts to read a hexadecimal substring from the specified input and returns its value as a string.
    /// </summary>
    /// <remarks>No exception is thrown if the input is invalid or the operation fails; instead, an
    /// empty string is returned. This method is useful for safely extracting hexadecimal values without handling
    /// exceptions.</remarks>
    /// <param name="hex">The input string containing hexadecimal characters to read from.</param>
    /// <param name="start">The zero-based starting index in the input string at which to begin reading.</param>
    /// <param name="length">The number of characters to read from the input string, starting at the specified index.</param>
    /// <returns>A string containing the hexadecimal value read from the input. Returns an empty string if the operation
    /// fails or the input is invalid.</returns>
    private static string TryReadHex(string hex, int start, int length)
    {
        try
        {
            return HexUtils.ReadHex(hex, start, length);
        }
        catch
        {
            return string.Empty;
        }
    }

    /// <summary>
    /// Calculates an 8-bit checksum by summing the bytes represented by a hexadecimal string and taking the result
    /// modulo 256.
    /// </summary>
    /// <remarks>The method expects the input string to contain only valid hexadecimal characters
    /// (0-9, a-f, A-F) and to represent complete bytes. If the input is not valid hexadecimal or has an odd number
    /// of digits, an exception may be thrown.</remarks>
    /// <param name="hex">A string containing hexadecimal characters representing the bytes to be summed. The string must contain an
    /// even number of valid hexadecimal digits.</param>
    /// <returns>A byte value representing the checksum of the input data, calculated as the sum of all bytes modulo 256.</returns>
    public static byte Checksum8Modulo256(string hex)
    {
        byte[] dataBytes = ConvertHexToBytes(hex);
        int totalSum = dataBytes.Sum(b => b);
        return (byte)(totalSum % 256);
    }

    /// <summary>
    /// Converts a hexadecimal string to its equivalent byte array.
    /// </summary>
    /// <remarks>The hexadecimal string may contain only valid hexadecimal digits (0-9, A-F, a-f).
    /// Each pair of characters is interpreted as a single byte.</remarks>
    /// <param name="hex">A string containing hexadecimal characters. The string must have an even number of characters.</param>
    /// <returns>A byte array representing the binary data encoded by the hexadecimal string.</returns>
    /// <exception cref="ArgumentException">Thrown if <paramref name="hex"/> has an odd number of characters.</exception>
    public static byte[] ConvertHexToBytes(string hex)
    {
        if (hex.Length % 2 != 0)
            throw new ArgumentException("Invalid hexadecimal. Number of characters must be even.");

        byte[] bytes = new byte[hex.Length / 2];
        for (int i = 0; i < hex.Length; i += 2)
        {
            bytes[i / 2] = Convert.ToByte(hex.Substring(i, 2), 16);
        }
        return bytes;
    }

    /// <summary>
    /// Determines whether the specified string represents a valid hexadecimal value of the given byte size.
    /// </summary>
    /// <remarks>The method checks that the string contains only valid hexadecimal characters (0-9,
    /// A-F, a-f) and that its length matches the expected size in bytes. This method does not accept strings with a
    /// '0x' prefix or whitespace.</remarks>
    /// <param name="hexValue">The string to validate as a hexadecimal value. Must contain only hexadecimal characters and have a length
    /// equal to twice the specified size.</param>
    /// <param name="size">The expected number of bytes represented by the hexadecimal string. Must be non-negative.</param>
    /// <returns>true if the string is a valid hexadecimal representation of the specified size; otherwise, false.</returns>
    private static bool IsValidHex(string hexValue, int size)
    {
        if (hexValue.Length != size * 2)
            return false;

        return hexValue.All(c => "0123456789ABCDEFabcdef".Contains(c));
    }
    

    /// <summary>
    /// Saves the current changes to the hex data and updates the main view model accordingly.
    /// </summary>
    /// <remarks>If changes are detected in the hex data, a success message is displayed. If an error
    /// occurs during the save process, an error message is shown to the user. The method updates the main view
    /// model with the new hex value and triggers encoding.</remarks>
    /// <returns>A task that represents the asynchronous save operation.</returns>
    [RelayCommand]
    private async Task Save()
    {
        try
        {
            var hexUnchanged = HexData;

            if (!TryUpdateHexFromFields())
                return;

            RecalculateChecksums();

            if (hexUnchanged != HexData)
                await ShowMessageAsync("Success", "Changes saved successfully.");

            MainViewModel.HexValue = HexData;
            await MainViewModel.Encode();
            MainViewModel.MainWindowModel.CurrentViewModel = MainViewModel;
        }
        catch (Exception ex)
        {
            await ShowMessageAsync("Error", $"Error saving changes: {ex.Message}");
        }
    }

    /// <summary>
    /// Validates and updates the hexadecimal data based on the current field values.
    /// </summary>
    /// <remarks>If any field value fails validation, the method displays a validation error message
    /// and returns false without updating the data. The method processes all fields sequentially and stops at the
    /// first invalid value.</remarks>
    /// <returns>true if all field values are valid hexadecimal strings and the data is updated successfully; otherwise,
    /// false.</returns>
    private bool TryUpdateHexFromFields()
    {
        foreach (var fieldValue in Fields)
        {
            string newHexValue = fieldValue.Value?.Trim().ToUpper() ?? "";

            if (!IsValidHex(newHexValue, fieldValue.Field.Size))
            {
                _ = ShowMessageAsync("Validation Error", $"Invalid value for field {fieldValue.Field.Name}.");
                return false;
            }

            HexData = HexUtils.WriteHex(HexData, fieldValue.Field.StartAddress, fieldValue.Field.Size, newHexValue);
        }
        return true;
    }

    /// <summary>
    /// Recalculates and updates the checksums for the EEPROM data based on the current EEPROM type.
    /// </summary>
    /// <remarks>This method should be called after modifying the EEPROM data to ensure that the
    /// checksums remain valid. The recalculation logic depends on the value of the EepromType property. Calling
    /// this method is necessary for maintaining data integrity when working with EEPROM contents.</remarks>
    private void RecalculateChecksums()
    {
        switch (EepromType)
        {
            case Utils.Eeprom.A0:
                WriteChecksum(63, HexUtils.ReadHex(HexData, 0, 63));
                WriteChecksum(95, HexUtils.ReadHex(HexData, 64, 31));
                break;
            case Utils.Eeprom.A2:
                WriteChecksum(95, HexUtils.ReadHex(HexData, 0, 95));
                break;
        }
    }

    /// <summary>
    /// Calculates the checksum for the specified hexadecimal range and writes it to the given address in the hex
    /// data.
    /// </summary>
    /// <param name="address">The address, in bytes, at which to write the computed checksum.</param>
    /// <param name="hexRange">A string representing the hexadecimal range over which the checksum is calculated. Cannot be null or empty.</param>
    private void WriteChecksum(int address, string hexRange)
    {
        byte checksum = Checksum8Modulo256(hexRange);
        HexData = HexUtils.WriteHex(HexData, address, 1, checksum.ToString("X2"));
    }

    /// <summary>
    /// Displays a modal message box with the specified title and message asynchronously.
    /// </summary>
    /// <param name="title">The text to display in the title bar of the message box. Cannot be null.</param>
    /// <param name="message">The content to display in the body of the message box. Cannot be null.</param>
    /// <returns>A task that represents the asynchronous operation of showing the message box.</returns>
    private static async Task ShowMessageAsync(string title, string message)
    {
        var box = MessageBoxManager.GetMessageBoxStandard(title, message);
        await box.ShowAsync();
    }
}
