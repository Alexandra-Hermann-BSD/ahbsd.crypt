using ahbsd.lib.crypt.Interfaces;
using JetBrains.Annotations;
using Xunit;

namespace ahbsd.lib.crypt.Tests.Interfaces;

[TestSubject(typeof(IKeyPart))]
public class KeyPartTest
{

    private readonly IKey _testKey;
    
    public KeyPartTest()
    {
        var gen = new KeyGenerator();
        _testKey = gen.CreateKey("TestKey", out _, 12345);
    }

    [Fact]
    public void Test1()
    {
        Assert.NotNull(_testKey);
        Assert.True(_testKey.IsReady);
        Assert.Equal("TestKey", _testKey.KeyName);
        Assert.Equal(16, _testKey.Count);
    }
}