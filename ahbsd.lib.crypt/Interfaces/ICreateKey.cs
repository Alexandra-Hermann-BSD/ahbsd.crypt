using JetBrains.Annotations;
using System.ComponentModel;
using ahbsd.lib.crypt.EventArguments;

namespace ahbsd.lib.crypt.Interfaces;

/// <summary>
/// Interface for creating cryptographic keys.
/// </summary>
[Description("Interface for creating cryptographic keys.")]
[Category("Cryptography")]
[PublicAPI]
public interface ICreateKey : IComponent
{
    /// <summary>
    /// Creates a new cryptographic key with the specified name and optional seed value.
    /// </summary>
    /// <param name="name">The given key name.</param>
    /// <param name="createDuration">[out] The duration it took to create the key.</param>
    /// <param name="seed">[optional] The seed value.</param>
    /// <returns>The created cryptographic key.</returns>
    IKey CreateKey(string name, out TimeSpan createDuration, int? seed = null);

    /// <summary>
    /// Happens when a new cryptographic key is created, providing the duration it took to create the key.
    /// </summary>
    [Description("Happens when a new cryptographic key is created, providing the duration it took to create the key.")]
    [Category("Cryptography")]
    event EventHandler<DurationEventArgs> CreationDuration;
    
    /// <summary>
    /// Happens when a new cryptographic key is generated, providing the generated key and the duration it took to create the key.
    /// </summary>
    [Description("Happens when a new cryptographic key is generated, providing the generated key and the duration it took to create the key.")]
    [Category("Cryptography")]
    event EventHandler<KeyGeneratedEventArgs> KeyGenerated;
}