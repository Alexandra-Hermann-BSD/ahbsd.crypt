using ahbsd.lib.crypt.Enums;
using JetBrains.Annotations;

namespace ahbsd.lib.crypt.Attributes;

/// <summary>
/// Attribute to set whether a byte should be split into two nibbles in the order first second, or second first.
/// </summary>
[PublicAPI]
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Constructor, AllowMultiple = false)]
public class ByteSplitAttribute : Attribute
{
    /// <summary>
    /// Constructor to create a new instance of the ByteSplitAttribute with the specified order.
    /// </summary>
    /// <param name="order">[optional] The order in which to split the byte into two nibbles.</param>
    public ByteSplitAttribute(ByteSplitOrder order = ByteSplitOrder.FirstSecond) => Order = order;

    /// <summary>
    /// Gets the order in which the byte should be split into two nibbles.
    /// </summary>
    /// <value>The order in which the byte should be split into two nibbles.</value>
    public ByteSplitOrder Order { get; }
}