using System;
using ahbsd.lib.crypt.EventArguments;
using ahbsd.lib.crypt.Interfaces;
using JetBrains.Annotations;
using Xunit;

namespace ahbsd.lib.crypt.Tests;

[TestSubject(typeof(KeyGenerator))]
public class KeyGeneratorTest
{
    private const string KeyName = "TestKey";
    private IKey? _key;
    private readonly ICreateKey _keyGenerator;

    public KeyGeneratorTest()
    {
        _keyGenerator = new KeyGenerator();
        _keyGenerator.CreationDuration += OnCreated;
        _keyGenerator.KeyGenerated += KeyGeneratorOnKeyGenerated;
    }

    private static void KeyGeneratorOnKeyGenerated(object? sender, KeyGeneratedEventArgs e)
    {
        Assert.NotNull(sender);
        Assert.NotNull(e);
        Assert.NotNull(sender as ICreateKey);
        Assert.True(e.Duration > TimeSpan.Zero);
        Assert.NotNull(e.Key);
        Assert.Equal(KeyName, e.Key.KeyName);
        Assert.True(e.Key.IsReady);
    }

    private static void OnCreated(object? sender, DurationEventArgs e)
    {
        Assert.NotNull(sender);
        Assert.NotNull(e);
        Assert.NotNull(sender as ICreateKey);
        Assert.True(e.Duration > TimeSpan.Zero);
    }

    [Fact]
    public void TestGenerate()
    {
        _key = _keyGenerator.CreateKey(KeyName, out var createDuration);
        Assert.NotNull(_key);
        Assert.Equal(KeyName, _key.KeyName);
        Assert.True(_key.IsReady);
        Console.Out.WriteLine($"Key generation took {createDuration.TotalMilliseconds} ms.");
    }
}