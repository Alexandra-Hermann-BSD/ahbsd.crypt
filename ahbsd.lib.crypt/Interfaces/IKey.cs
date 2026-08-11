using JetBrains.Annotations;

namespace ahbsd.lib.crypt.Interfaces;

/// <summary>
/// Interface for a key dictionary that maps characters to key parts,
/// allowing for read-only access to the key parts associated with each character.
/// </summary>
[PublicAPI]
public interface IKey : IReadOnlyDictionary<char, IKeyPart>
{
    /// <summary>
    /// Gets the name of the key.
    /// </summary>
    /// <value>The name of the key.</value>
    string KeyName { get; }
    
    /// <summary>
    /// Adds a new key part to the key dictionary.
    /// </summary>
    /// <param name="keyPart">The given key part.</param>
    /// <returns><c>true</c> if the key part was added; otherwise, <c>false</c>.</returns>
    /// <exception cref="ArgumentException">Thrown when the <see cref="IKeyPart.KeyPartKey"/> of the given
    /// <paramref name="keyPart"/> is not valid.</exception>
    bool AddKeyPart(IKeyPart keyPart);
    
    /// <summary>
    /// Decrypts the given character.
    /// </summary>
    /// <param name="toDecrypt">The character to decrypt.</param>
    /// <returns>The decrypted character.</returns>
    char DecryptChar(char toDecrypt);
    
    /// <summary>
    /// Encrypts the given character.
    /// </summary>
    /// <param name="toEncrypt">The character to encrypt.</param>
    /// <param name="randomSeed">[optional] The random seed for encryption.</param>
    /// <returns>The encrypted character.</returns>
    char EncryptChar(char toEncrypt, decimal? randomSeed = null);
    
    /// <summary>
    /// Gets whether the key is ready for encryption and decryption operations.
    /// </summary>
    /// <value><c>true</c> if the key is ready; otherwise, <c>false</c>.</value>
    bool IsReady { get; }
}