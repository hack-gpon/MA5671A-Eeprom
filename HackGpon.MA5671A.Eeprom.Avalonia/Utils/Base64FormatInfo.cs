using System.Collections.Generic;

namespace HackGpon.MA5671A.Eeprom.Avalonia.Utils;

/// <summary>
/// Stores the original format of a Base64 encoded string for reconstruction after editing
/// </summary>
public class Base64FormatInfo
{
    /// <summary>Content before the first @ (e.g., "begin-base64 644 sfp_a2_info ")</summary>
    public string PrefixBeforeAt { get; set; } = string.Empty;
    
    /// <summary>The Base64 content parts split by @</summary>
    public List<string> ArrobaParts { get; set; } = [];
    
    /// <summary>Padding at the end (e.g., "====")</summary>
    public string Padding { get; set; } = string.Empty;
}
