using System;

namespace HackGpon.MA5671A.Eeprom.Avalonia.Utils;

/// <summary>
/// Utility methods for reading and writing hexadecimal data
/// </summary>
public static class HexUtils
{
    /// <summary>
    /// Reads a portion of hex data from the specified address
    /// </summary>
    /// <param name="hex">The full hex string</param>
    /// <param name="address">Start address in bytes</param>
    /// <param name="length">Number of bytes to read</param>
    public static string ReadHex(string hex, int address, int length)
    {
        ValidateHexString(hex);
        
        int charStart = address * 2;
        int charLength = length * 2;

        if (charStart + charLength > hex.Length)
            throw new ArgumentOutOfRangeException(nameof(address), "Address/length outside HEX limits");

        return hex.Substring(charStart, charLength);
    }

    /// <summary>
    /// Writes a new hex value at the specified address
    /// </summary>
    /// <param name="hex">The full hex string</param>
    /// <param name="address">Start address in bytes</param>
    /// <param name="length">Number of bytes to write</param>
    /// <param name="newValue">New hex value to write</param>
    public static string WriteHex(string hex, int address, int length, string newValue)
    {
        ValidateHexString(hex);

        if (newValue.Length != length * 2)
            throw new ArgumentException("New value size does not match specified length", nameof(newValue));

        int charStart = address * 2;
        int charLength = length * 2;

        if (charStart + charLength > hex.Length)
            throw new ArgumentOutOfRangeException(nameof(address), "Address/length outside HEX limits");

        return string.Concat(hex.AsSpan(0, charStart), newValue, hex.AsSpan(charStart + charLength));
    }

    /// <summary>
    /// Validates that the specified string is a non-null, non-empty hexadecimal string with an even number of
    /// characters.
    /// </summary>
    /// <param name="hex">The string to validate as a hexadecimal value. Must not be null, empty, or have an odd number of characters.</param>
    /// <exception cref="ArgumentException">Thrown if the string is null, empty, or contains an odd number of characters.</exception>
    private static void ValidateHexString(string hex)
    {
        if (string.IsNullOrEmpty(hex) || hex.Length % 2 != 0)
            throw new ArgumentException("Invalid HEX string (null, empty, or odd number of characters)", nameof(hex));
    }
}
