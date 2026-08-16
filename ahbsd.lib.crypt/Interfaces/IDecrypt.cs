using System.ComponentModel;

namespace ahbsd.lib.crypt.Interfaces;

/// <summary>
/// Interface for a decryptor component that can be compared with other decryptors.
/// </summary>
public interface IDecrypt : ICryptData, IComparable<IDecrypt>
{
    /// <summary>
    /// Decrypts the given object using the specified keys.
    /// </summary>
    /// <param name="obj">The given object</param>
    /// <param name="keys">The specified keys</param>
    /// <returns>The decrypted object</returns>
    object DecryptObject(char[] obj, IList<IKey> keys);
    
    /// <summary>
    /// Gets the decrypted object after the decryption process is completed.
    /// </summary>
    /// <value>The decrypted object</value>
    object? DecryptedObject { get; }
}