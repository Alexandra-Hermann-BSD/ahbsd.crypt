using ahbsd.lib.crypt.Attributes;
using ahbsd.lib.crypt.Enums;
using ahbsd.lib.crypt.Extensions;
using ahbsd.lib.crypt.Interfaces;
using JetBrains.Annotations;
using Xunit;

namespace ahbsd.lib.crypt.Tests;

[TestSubject(typeof(ByteSplit))]
public class ByteSplitTest
{

    [Theory]
    [InlineData(0x12, 0x1, 0x2, ByteSplitOrder.FirstSecond)]
    [InlineData(0x12, 0x2, 0x1, ByteSplitOrder.SecondFirst)]
    [InlineData(0xAB, 0xA, 0xB)]
    [InlineData(0xAB, 0xB, 0xA, ByteSplitOrder.SecondFirst)]
    public void TstByteSplit(byte originalByte, ushort expectedFirstNibble, ushort expectedSecondNibble,
        ByteSplitOrder? order = null)
    {
        order ??= ByteSplitOrder.FirstSecond;
        var split = new ByteSplit(originalByte, order.Value);

        Assert.Equal(originalByte, split.OriginalByte);
        Assert.Equal(expectedFirstNibble, split.FirstNibble);
        Assert.Equal(expectedSecondNibble, split.SecondNibble);

        switch (order.Value)
        {
            case ByteSplitOrder.FirstSecond:
                Assert.Equal(originalByte, split.ChangedByte);
                break;
            case ByteSplitOrder.SecondFirst:
                Assert.NotEqual(originalByte, split.ChangedByte);
                break;
        }

        Assert.Equal((char)(expectedSecondNibble < 10 ? '0' + expectedSecondNibble : 'A' + (expectedSecondNibble - 10)),
            split.SecondNibbleChar);
    }
}