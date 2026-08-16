using System.ComponentModel;
using ahbsd.lib.crypt.EventArguments;
using ahbsd.lib.crypt.Interfaces;
using JetBrains.Annotations;

namespace ahbsd.lib.crypt;

/// <summary>
/// Class for generating keys.
/// </summary>
[Description("Implementation of the ICreateKey interface for generating cryptographic keys.")]
[Category("Cryptography")]
[PublicAPI]
public class KeyGenerator : Component, ICreateKey
{
    /// <inheritdoc />
    public IKey CreateKey(string name, out TimeSpan createDuration, int? seed = null)
    {
        var key = new Key(name);

        var r = seed.HasValue ? new Random(seed.Value) : new Random();

        var i = (char)0;
        DateTime start = DateTime.Now;
        while (!key.IsReady)
        {
            var c = Key.GetHexChar(r.Next(0, 16)); 
            if (!key[c].IsFull && key[c].AddValue(i))
            {
                i++;
            }
        }
        
        createDuration = DateTime.Now - start;
        CreationDuration?.Invoke(this, new DurationEventArgs(createDuration));
        KeyGenerated?.Invoke(this, new KeyGeneratedEventArgs(key, createDuration));
        
        return key;
    }

    /// <inheritdoc />
    [Description("Happens when a new cryptographic key is created, providing the duration it took to create the key.")]
    [Category("Cryptography")]
    public event EventHandler<DurationEventArgs>? CreationDuration;

    /// <inheritdoc />
    [Description("Happens when a new cryptographic key is generated, providing the generated key and the duration it took to create the key.")]
    [Category("Cryptography")]
    public event EventHandler<KeyGeneratedEventArgs>? KeyGenerated;
}