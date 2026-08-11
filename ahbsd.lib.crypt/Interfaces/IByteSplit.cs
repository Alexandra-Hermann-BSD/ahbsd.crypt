using ahbsd.lib.crypt.Enums;
using JetBrains.Annotations;

namespace ahbsd.lib.crypt.Interfaces;

/// <summary>
/// Interface to split a given byte into two nibbles in the order first second, or second first.
/// The nibbles are given out as a <see cref="ushort"/> with a value between 0 and 15. Additionally, each part of the split
/// is given out as a <see cref="char"/> with a value between '0' and '9' or 'A' and 'F'.
/// </summary>
[PublicAPI]
public interface IByteSplit
{
    /// <summary>
    /// Gets the original byte before the split.
    /// </summary>
    /// <value>The original byte.</value>
    byte OriginalByte { get; }
    /// <summary>
    /// Gets the byte after the split.
    /// </summary>
    /// <value>The changed byte.</value>
    byte ChangedByte { get; }
    /// <summary>
    /// Gets the first nibble.
    /// </summary>
    /// <value>The first nibble.</value>
    /// <remarks>Not the original first nibble, but the one after the split.</remarks>
    ushort FirstNibble { get; }
    /// <summary>
    /// Gets the second nibble.
    /// </summary>
    /// <value>The second nibble.</value>
    /// <remarks>Not the original second nibble, but the one after the split.</remarks>
    ushort SecondNibble { get; }
    /// <summary>
    /// Gets the first nibble as a character.
    /// </summary>
    /// <value>The first nibble as a character.</value>
    /// <remarks>Not the original first nibble, but the one after the split.</remarks>
    char FirstNibbleChar { get; }
    /// <summary>
    /// Gets the second nibble as a character.
    /// </summary>
    /// <value>The second nibble as a character.</value>
    /// <remarks>Not the original second nibble, but the one after the split.</remarks>
    char SecondNibbleChar { get; }
    /// <summary>
    /// Gets the order in which the byte was split.
    /// </summary>
    /// <value>The order.</value>
    ByteSplitOrder Order { get; }
}