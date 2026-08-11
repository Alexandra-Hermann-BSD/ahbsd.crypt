using ahbsd.lib.crypt.Enums;
using JetBrains.Annotations;

namespace ahbsd.lib.crypt.Interfaces;

/// <summary>
/// Interface for a byte that can be split into two nibbles.
/// </summary>
[PublicAPI]
public interface IByteToSplit
{
    /// <summary>
    /// Gets or sets the byte value to be split into two nibbles.
    /// </summary>
    /// <value>The byte value to be split into two nibbles</value>
    byte Value { get; set; }

    /// <summary>
    /// Gets the order in which the byte should be split into two nibbles.
    /// </summary>
    /// <value>The order in which the byte should be split into two nibbles</value>
    ByteSplitOrder Order { get; }
}