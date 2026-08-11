using ahbsd.lib.crypt.Attributes;
using ahbsd.lib.crypt.Enums;
using ahbsd.lib.crypt.Interfaces;
using JetBrains.Annotations;

namespace ahbsd.lib.crypt.Extensions;

/// <summary>
/// An extension class for the <see cref="byte"/> type, providing additional methods and functionality for working with bytes.
/// </summary>
[PublicAPI] 
public static class ByteExtension
{
    /// <summary>
    /// Splits the given byte into two nibbles based on the specified order defined by the <see cref="ByteSplitAttribute"/>.
    /// </summary>
    /// <param name="b">The calling byte</param>
    /// <returns>The splitted byte</returns>
    public static IByteSplit GetByteSplit(this IByteToSplit b)
    {
        var order = ByteSplitOrder.FirstSecond; // Default order

        var customAttributes = b.GetType().GetCustomAttributes(true);
        
        if (customAttributes.Contains(typeof(ByteSplitAttribute)) && customAttributes.FirstOrDefault(a => a is ByteSplitAttribute) is ByteSplitAttribute attribute)
        {
            order = attribute.Order;
        }
        
        IByteSplit result = new ByteSplit(b.Value, order);
        return result;
    }
}