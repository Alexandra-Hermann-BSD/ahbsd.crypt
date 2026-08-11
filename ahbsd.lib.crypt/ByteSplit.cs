using System.ComponentModel;
using ahbsd.lib.crypt.Attributes;
using ahbsd.lib.crypt.Enums;
using ahbsd.lib.crypt.Interfaces;
using JetBrains.Annotations;

namespace ahbsd.lib.crypt;

/// <summary>
/// Represents a split of a byte into two nibbles, providing access to the original byte, the changed byte,
/// the first and second nibbles as both ushort and char, and the order of the split.
/// </summary>
[PublicAPI]
public class ByteSplit : IByteSplit, IEquatable<ByteSplit>
{

    /// <summary>
    /// Constructor with a given <see cref="IByteToSplit"/> instance.
    /// </summary>
    /// <param name="byteToSplit">The given <see cref="IByteToSplit"/> instance.</param>
    /// <remarks>
    /// The order of the split is determined by the <see cref="ByteSplitAttribute"/> applied to the type of
    /// the <see cref="IByteToSplit"/> instance, if present;
    /// otherwise, it defaults to <see cref="ByteSplitOrder.FirstSecond"/>.
    /// </remarks>
    public ByteSplit(IByteToSplit byteToSplit)
    {
        OriginalByte = byteToSplit.Value;
        
        try
        {
            if (byteToSplit.GetType().GetCustomAttributes(typeof(ByteSplitAttribute), false)
                    .FirstOrDefault() is ByteSplitAttribute byteSplitAttribute)
            {
                Order = byteSplitAttribute.Order;
            }
        }
        catch (Exception)
        {
            Order = ByteSplitOrder.FirstSecond;
        }
        
        Init();
    }
    
    /// <summary>
    /// Constructor with a given original byte and an optional order for splitting the byte into two nibbles.
    /// </summary>
    /// <param name="originalByte">The given original byte.</param>
    /// <param name="order">[optional] The order for splitting the byte into two nibbles.</param>
    public ByteSplit(byte originalByte, ByteSplitOrder order = ByteSplitOrder.FirstSecond)
    {
        OriginalByte = originalByte;
        Order = order;

        Init();
    }

    private void Init()
    {
        switch (Order)
        {
            case ByteSplitOrder.FirstSecond:
                FirstNibble = (ushort)((OriginalByte >> 4) & 0xF);
                SecondNibble = (ushort)(OriginalByte & 0xF);
                break;
            case ByteSplitOrder.SecondFirst:
                FirstNibble = (ushort)(OriginalByte & 0xF);
                SecondNibble = (ushort)((OriginalByte >> 4) & 0xF);
                break;
        }
        
        FirstNibbleChar = (char)(FirstNibble < 10 ? '0' + FirstNibble : 'A' + (FirstNibble - 10));
        SecondNibbleChar = (char)(SecondNibble < 10 ? '0' + SecondNibble : 'A' + (SecondNibble - 10));
        ChangedByte = (byte)((FirstNibble << 4) | SecondNibble);
    }

    #region implementation of IByteSplit
    
    /// <inheritdoc />
    public byte OriginalByte { get; }
    /// <inheritdoc />
    public byte ChangedByte { get; private set; }

    /// <inheritdoc />
    public ushort FirstNibble { get; private set; }

    /// <inheritdoc />
    public ushort SecondNibble { get; private set; }

    /// <inheritdoc />
    public char FirstNibbleChar { get; private set; }

    /// <inheritdoc />
    public char SecondNibbleChar { get; private set; }

    /// <inheritdoc />
    public ByteSplitOrder Order { get; }
    
    #endregion

    /// <inheritdoc />
    [Localizable(false)]
    public override string ToString() => $"{ChangedByte} (0x{FirstNibbleChar}{SecondNibbleChar})";

    /// <inheritdoc />
    public bool Equals(ByteSplit? other)
    {
        if (other is null) return false;
        if (ReferenceEquals(this, other)) return true;
        return ChangedByte == other.ChangedByte;
    }

    /// <inheritdoc />
    public override bool Equals(object? obj)
    {
        if (obj is null) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != GetType()) return false;
        return Equals((ByteSplit)obj);
    }

    /// <inheritdoc />
    public override int GetHashCode() => ChangedByte.GetHashCode();

    /// <summary>
    /// Compares two <see cref="ByteSplit"/> instances for equality based on their <see cref="ChangedByte"/> values.
    /// </summary>
    /// <param name="left">The first <see cref="ByteSplit"/> instance to compare.</param>
    /// <param name="right">The second <see cref="ByteSplit"/> instance to compare.</param>
    /// <returns><c>true</c> if the instances are equal; otherwise, <c>false</c>.</returns>
    public static bool operator ==(ByteSplit? left, ByteSplit? right) => Equals(left, right);

    /// <summary>
    /// Compares two <see cref="ByteSplit"/> instances for inequality based on their <see cref="ChangedByte"/> values.
    /// </summary>
    /// <param name="left">The first <see cref="ByteSplit"/> instance to compare.</param>
    /// <param name="right">The second <see cref="ByteSplit"/> instance to compare.</param>
    /// <returns><c>true</c> if the instances are not equal; otherwise, <c>false</c>.</returns>
    public static bool operator !=(ByteSplit? left, ByteSplit? right) => !Equals(left, right);
}