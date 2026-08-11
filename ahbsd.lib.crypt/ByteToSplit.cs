using ahbsd.lib.crypt.Enums;
using ahbsd.lib.crypt.Interfaces;
using JetBrains.Annotations;

namespace ahbsd.lib.crypt;

/// <summary>
/// Represents a byte that can be split into two nibbles.
/// </summary>
[PublicAPI]
public class ByteToSplit : IByteToSplit
{
    /// <summary>
    /// Default constructor.
    /// </summary>
    /// <remarks>This initializes the byte value to 0.</remarks>
    public ByteToSplit() => Value = 0;
    
    /// <summary>
    /// Constructor with a given byte value.
    /// </summary>
    /// <param name="value">The given byte value</param>
    /// <param name="order">The order in which to split the byte into two nibbles</param>
    public ByteToSplit(byte value, ByteSplitOrder order = ByteSplitOrder.FirstSecond)
    {
        Value = value;
    }

    /// <inheritdoc/>
    public byte Value { get; set; }
    
    /// <inheritdoc/>
    public ByteSplitOrder Order { get; }
}