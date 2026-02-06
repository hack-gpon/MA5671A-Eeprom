using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using HackGpon.MA5671A.Eeprom.Avalonia.Utils;
using MimeKit.Encodings;
using MsBox.Avalonia;
using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace HackGpon.MA5671A.Eeprom.Avalonia.ViewModels;

public partial class MainViewModel : ObservableObject
{
    private Base64FormatInfo? _lastFormat;

    [ObservableProperty]
    private string? hexValue;

    [ObservableProperty]
    private string? base64Value;

    [ObservableProperty]
    private MainWindowModel mainWindowModel;

    public MainViewModel(MainWindowModel mainWindowModel)
    {
        MainWindowModel = mainWindowModel;
    }

    /// <summary>
    /// Auto-decode when Base64Value changes (if it contains valid data)
    /// </summary>
    partial void OnBase64ValueChanged(string? value)
    {
        if (string.IsNullOrWhiteSpace(value) || !value.Contains('@'))
        {
            HexValue = null;
            _lastFormat = null;
            return;
        }

        try
        {
            var (cleaned, format) = ParseBase64WithFormat(value);
            _lastFormat = format;
            HexValue = DecodeBase64ToHex(cleaned);
        }
        catch
        {
            // Silently ignore parse errors during typing
            HexValue = null;
        }
    }

    /// <summary>
    /// Parses a base64-encoded string with custom formatting, extracting the cleaned base64 content and associated
    /// format information.
    /// </summary>
    /// <remarks>The input string is expected to use '@' as a separator and may include prefixes or padding.
    /// The returned format information includes any prefix, separated base64 parts, and padding details. This method
    /// does not validate the base64 content; it only parses and cleans the format.</remarks>
    /// <param name="input">The input string containing base64 data, which may include custom prefixes, padding, and separator characters.
    /// Cannot be null.</param>
    /// <returns>A tuple containing the cleaned base64 string and a <see cref="Base64FormatInfo"/> object describing the
    /// extracted format details.</returns>
    private (string CleanBase64, Base64FormatInfo FormatInfo) ParseBase64WithFormat(string input)
    {
        var format = new Base64FormatInfo();

        // 1. Extract prefix before first @
        int firstAtPos = input.IndexOf('@');
        if (firstAtPos > 0)
        {
            format.PrefixBeforeAt = input[..firstAtPos];
        }

        // 2. Get remaining content after first @
        string remaining = input[(firstAtPos + 1)..];

        // 3. Find the first '='
        int firstEqualIndex = remaining.IndexOf('=');
        if (firstEqualIndex >= 0)
        {
            // Find the last '@' before '='
            int lastAtBeforeEqual = remaining.LastIndexOf('@', firstEqualIndex);
            if (lastAtBeforeEqual >= 0)
            {
                // Everything up to that @ is valid base64 parts
                string base64Part = remaining[..(lastAtBeforeEqual + 1)];
                format.ArrobaParts = [.. base64Part.Split('@', StringSplitOptions.RemoveEmptyEntries)];

                // Everything after is padding
                format.Padding = remaining[(lastAtBeforeEqual + 1)..];
            }
            else
            {
                // No @ before '=', treat as direct padding
                format.ArrobaParts = [.. remaining[..firstEqualIndex].Split('@', StringSplitOptions.RemoveEmptyEntries)];
                format.Padding = remaining[firstEqualIndex..];
            }
        }
        else
        {
            // No padding, just split by @
            format.ArrobaParts = [.. remaining.Split('@', StringSplitOptions.RemoveEmptyEntries)];
            format.Padding = "";
        }

        // 4. Clean base64: join all parts without @
        string cleanedRaw = string.Join("", format.ArrobaParts).Replace("\r", "").Replace("\n", "").Trim();

        return (cleanedRaw, format);
    }

    /// <summary>
    /// Converts a Base64-encoded string to its hexadecimal representation.
    /// </summary>
    /// <remarks>The returned hexadecimal string uses uppercase letters and contains no separators. If the
    /// input is not valid Base64, the method may throw an exception depending on the decoder implementation.</remarks>
    /// <param name="input">The Base64-encoded string to decode. Cannot be null or empty.</param>
    /// <returns>A string containing the hexadecimal representation of the decoded bytes. The string will be empty if the input
    /// is empty.</returns>
    private static string DecodeBase64ToHex(string input)
    {
        byte[] inputBytes = Encoding.ASCII.GetBytes(input);
        var decoder = new Base64Decoder();

        using var output = new MemoryStream();
        int offset = 0;
        int count = inputBytes.Length;
        const int chunkSize = 4096;
        byte[] buffer = new byte[chunkSize];

        while (offset < count)
        {
            int toCopy = Math.Min(chunkSize, count - offset);
            int written = decoder.Decode(inputBytes, offset, toCopy, buffer);
            output.Write(buffer, 0, written);
            offset += toCopy;
        }

        return BitConverter.ToString(output.ToArray()).Replace("-", "");
    }

    /// <summary>
    /// Encodes a hexadecimal string to a Base64 string and formats it according to the specified format information.
    /// </summary>
    /// <remarks>The output string preserves the original formatting elements, such as prefix and padding, as
    /// specified in the format information. The method does not validate the contents of the hexadecimal string;
    /// callers should ensure that the input is valid and properly formatted.</remarks>
    /// <param name="hex">The hexadecimal string to encode. Must contain an even number of valid hexadecimal characters.</param>
    /// <param name="format">The format information that specifies how the resulting Base64 string should be structured, including prefix,
    /// parts, and padding.</param>
    /// <returns>A formatted Base64 string that represents the encoded hexadecimal input, structured according to the provided
    /// format information.</returns>
    private static string EncodeHexToBase64WithFormat(string hex, Base64FormatInfo format)
    {
        // Convert HEX to bytes
        byte[] bytes = new byte[hex.Length / 2];
        for (int i = 0; i < bytes.Length; i++)
        {
            bytes[i] = Convert.ToByte(hex.Substring(i * 2, 2), 16);
        }

        // Convert to Base64
        string base64 = Convert.ToBase64String(bytes);

        // Rebuild original format
        var result = new StringBuilder();

        // 1. Add prefix exactly as it was
        if (!string.IsNullOrEmpty(format.PrefixBeforeAt))
        {
            result.Append(format.PrefixBeforeAt);
        }
        result.Append('@');

        // 2. Rebuild parts with @
        int currentPos = 0;
        foreach (string part in format.ArrobaParts)
        {
            int partLength = Math.Min(part.Length, base64.Length - currentPos);
            if (partLength > 0)
            {
                result.Append(base64.AsSpan(currentPos, partLength));
                result.Append('@');
                currentPos += partLength;
            }
        }

        // 3. Add padding exactly as it was
        if (!string.IsNullOrEmpty(format.Padding))
        {
            result.Append(format.Padding);
        }

        return result.ToString();
    }

    /// <summary>
    /// Encodes the current hexadecimal value to a Base64 string using the previously stored format.
    /// </summary>
    /// <remarks>If the original format or hexadecimal value is not available, a warning message is displayed
    /// and encoding does not proceed. Any errors encountered during encoding are reported via an error message. This
    /// method should be called after decoding to ensure the required format is available.</remarks>
    /// <returns>A task that represents the asynchronous encoding operation. The task completes when the encoding process
    /// finishes.</returns>
    public async Task Encode()
    {
        try
        {
            if (_lastFormat == null || HexValue == null)
            {
                await ShowMessageAsync("Warning", "Original format not stored. Please decode first.");
                return;
            }

            string hex = HexValue.Trim();
            Base64Value = EncodeHexToBase64WithFormat(hex, _lastFormat);
        }
        catch (Exception ex)
        {
            await ShowMessageAsync("Error", $"Encoding error: {ex.Message}");
        }
    }

    /// <summary>
    /// Validates and decodes the Base64 input, converting it to hexadecimal format and updating the application state
    /// based on the detected EEPROM type.
    /// </summary>
    /// <remarks>If the Base64 input is empty or does not contain the required 'begin-base64' tag, a warning
    /// message is displayed and the operation is aborted. If decoding fails or the EEPROM type cannot be determined, an
    /// error or warning message is shown to the user. The method updates the view model to display EEPROM data when a
    /// valid type is detected.</remarks>
    /// <returns>A task that represents the asynchronous decode operation.</returns>
    [RelayCommand]
    private async Task Decode()
    {
        if (string.IsNullOrWhiteSpace(Base64Value) || !Base64Value.Contains("begin-base64"))
        {
            await ShowMessageAsync("Warning", "Base64 field is empty or missing 'begin-base64' tag.");
            return;
        }

        try
        {
            var (cleaned, format) = ParseBase64WithFormat(Base64Value);
            _lastFormat = format;

            HexValue = DecodeBase64ToHex(cleaned);

            var eepromType = DetectEepromType(Base64Value);
            if (eepromType.HasValue)
            {
                MainWindowModel.CurrentViewModel = new EEPROMViewModel(this, HexValue, eepromType.Value);
            }
            else
            {
                await ShowMessageAsync("Warning", "Invalid data at Base64 field. Unknown EEPROM type.");
            }
        }
        catch (Exception ex)
        {
            await ShowMessageAsync("Error", $"Decoding error: {ex.Message}");
        }
    }

    /// <summary>
    /// Determines the EEPROM type based on the specified base64-encoded content.
    /// </summary>
    /// <param name="base64Content">The base64-encoded string representing EEPROM data to analyze. Cannot be null.</param>
    /// <returns>An <see cref="Utils.Eeprom"/> value indicating the detected EEPROM type, or <see langword="null"/> if the type
    /// cannot be determined.</returns>
    private static Utils.Eeprom? DetectEepromType(string base64Content)
    {
        if (base64Content.Contains("sfp_a0_low_128"))
            return Utils.Eeprom.A0;
        if (base64Content.Contains("sfp_a2_info"))
            return Utils.Eeprom.A2;
        return null;
    }

    /// <summary>
    /// Displays information about how to use the EEPROM Base64 encoding and decoding tool.
    /// </summary>
    /// <remarks>The information message provides step-by-step instructions for decoding, editing, and
    /// encoding EEPROM data using Base64 format. This method is intended to assist users in understanding the workflow
    /// of the tool.</remarks>
    /// <returns>A task that represents the asynchronous operation of showing the information message.</returns>
    [RelayCommand]
    private async Task Info()
    {
        await ShowMessageAsync("Info", 
            "This tool allows you to decode and encode EEPROM data in Base64 format.\n\n" +
            "To use it:\n" +
            "1. Paste the Base64 string into the 'Base64' field\n" +
            "2. Click 'Decode'\n" +
            "3. Edit the EEPROM fields as needed\n" +
            "4. Click 'Save' to encode back to Base64");
    }

    /// <summary>
    /// Displays a message box asynchronously with the specified title and message.
    /// </summary>
    /// <remarks>The method does not block the calling thread. Use 'await' to ensure the message box is shown
    /// before continuing execution.</remarks>
    /// <param name="title">The title text to display in the message box. Cannot be null or empty.</param>
    /// <param name="message">The message content to display in the message box. Cannot be null or empty.</param>
    /// <returns>A task that represents the asynchronous operation of showing the message box.</returns>
    private static async Task ShowMessageAsync(string title, string message)
    {
        var box = MessageBoxManager.GetMessageBoxStandard(title, message);
        await box.ShowAsync();
    }
}
