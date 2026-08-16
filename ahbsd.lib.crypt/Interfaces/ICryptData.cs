using System.ComponentModel;
using ahbsd.lib.crypt.EventArguments;
using JetBrains.Annotations;

namespace ahbsd.lib.crypt.Interfaces;

/// <summary>
/// Interface for a component that holds the original data before encryption or decryption and provides an event for the
/// duration of the encryption/decryption process.
/// </summary>
[Description("Interface for a component that holds the original data before encryption or decryption and provides an event for the duration of the encryption/decryption process.")]
[Category("Cryptography")]
[PublicAPI]
public interface ICryptData : IComponent, ICloneable
{
    /// <summary>
    /// Gets the original data before the encryption process.
    /// </summary>
    /// <value>The original data</value>
    object? OriginalData { get; }
    
    /// <summary>
    /// Happens when the encryption or decryption is finished and the duration of the process is available.
    /// </summary>
    event EventHandler<DurationEventArgs>? CryptionDuration;
}