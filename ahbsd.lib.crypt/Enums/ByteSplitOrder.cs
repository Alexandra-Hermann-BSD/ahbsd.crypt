using JetBrains.Annotations;

namespace ahbsd.lib.crypt.Enums;

/// <summary>
/// Represents the order in which a byte should be split into two nibbles: first second or second first.
/// </summary>
[PublicAPI]
public enum ByteSplitOrder
{
    /// <summary>
    /// Indicates that the byte should be split into two nibbles in the order of first nibble followed by second nibble.
    /// </summary>
    FirstSecond,
    /// <summary>
    /// Indicates that the byte should be split into two nibbles in the order of second nibble followed by first nibble.
    /// </summary>
    SecondFirst,
}