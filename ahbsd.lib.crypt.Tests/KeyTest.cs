using ahbsd.lib.crypt.Interfaces;
using JetBrains.Annotations;
using Xunit;

namespace ahbsd.lib.crypt.Tests;

[TestSubject(typeof(Key))]
public class KeyTest
{

    [Fact]
    public void TestKey()
    {
        IKey key = new Key("TestKey");
        
        Assert.Equal("TestKey", key.KeyName);
        Assert.Equal(16, key.Count);
    }
}