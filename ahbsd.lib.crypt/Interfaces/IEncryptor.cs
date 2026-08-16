using System.ComponentModel;
using ahbsd.lib.crypt.EventArguments;
using JetBrains.Annotations;

namespace ahbsd.lib.crypt.Interfaces;

/// <summary>
/// Interface for an encryptor component that can be compared with other encryptors.
/// </summary>
[Description("Interface for an encryptor component that can be compared with other encryptors.")]
[Category("Cryptography")]
[PublicAPI]
public interface IEncryptor : ICryptData, IComparable<IEncryptor>
{
    /// <summary>
    /// Encrypts the given object using the specified keys and an optional seed value.
    /// </summary>
    /// <param name="obj">The given object</param>
    /// <param name="keys">The specified keys</param>
    /// <param name="seed">[Optional] The optional seed value</param>
    /// <returns>The encrypted object</returns>
    char[] EncryptObject(object obj, IList<IKey> keys, int? seed = null);
    
    /// <summary>
    /// Encrypts the given string using the specified keys and an optional seed value.
    /// </summary>
    /// <param name="str">The given string</param>
    /// <param name="keys">The specified keys</param>
    /// <param name="seed">[Optional] The optional seed value</param>
    /// <returns>The encrypted string</returns>
    char[] EncryptString(string str, IList<IKey> keys, int? seed = null);
    
    /// <summary>
    /// Gets the encrypted data after the encryption process is completed.
    /// </summary>
    /// <value>The encrypted data</value>
    char[] EncryptedData { get; }
}