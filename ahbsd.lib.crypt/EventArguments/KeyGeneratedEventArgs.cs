using System.ComponentModel;
using ahbsd.lib.crypt.Interfaces;
using JetBrains.Annotations;

namespace ahbsd.lib.crypt.EventArguments;

/// <summary>
/// Event arguments for a created key.
/// </summary>
[PublicAPI]
[Description("Event arguments for a created key.")]
[Category("Cryptography")]
public class KeyGeneratedEventArgs : DurationEventArgs
{
    /// <summary>
    /// Constructor with a given creation duration and generated key.
    /// </summary>
    /// <param name="key">The generated key</param>
    /// <param name="duration">The given creation duration</param>
    public KeyGeneratedEventArgs(IKey key, TimeSpan duration) : base(duration) => Key = key;

    /// <summary>
    /// Gets the generated key.
    /// </summary>
    /// <value>The generated key.</value>
    public IKey Key { get; }
    
    /// <summary>
    /// Gets the name of the generated key.
    /// </summary>
    /// <value>The name of the generated key.</value>
    public string KeyName => Key.KeyName;
}