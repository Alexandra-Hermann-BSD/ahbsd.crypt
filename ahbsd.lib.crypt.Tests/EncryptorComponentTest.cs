using System.Collections.Generic;
using System.ComponentModel;
using ahbsd.lib.crypt;
using ahbsd.lib.crypt.Extensions;
using ahbsd.lib.crypt.Interfaces;
using JetBrains.Annotations;
using Xunit;

namespace ahbsd.lib.crypt.Tests;

[TestSubject(typeof(EncryptorComponent))]
public class EncryptorComponentTest
{
    private readonly IEncryptor _encryptor;
    private readonly ICreateKey _keyGenerator;
    
    public EncryptorComponentTest()
    {
        IContainer container = new Container();
        _encryptor = new EncryptorComponent("Test", container);
        _keyGenerator = new KeyGenerator();
    }

    [Fact]
    public void Test1()
    {
        var theKey = _keyGenerator.CreateKey("the", out _);
        var dogKey = _keyGenerator.CreateKey("dog", out _);
        var catKey = _keyGenerator.CreateKey("cat", out _);
        var isKey = _keyGenerator.CreateKey("is", out _);
        var stupidKey = _keyGenerator.CreateKey("stupid", out _);
        
        var keys = new List<IKey> { theKey, catKey, isKey, stupidKey };
        
        var origin = "Hello world, this is a string to encrypt.";

        var encrypted = new string(_encryptor.EncryptString(origin, keys));

        Assert.NotNull(encrypted);
        Assert.NotEqual(origin, encrypted);
        Assert.NotEqual(origin.GetHexArray(), encrypted.GetHexArray());
    }
}