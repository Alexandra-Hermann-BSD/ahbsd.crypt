using System.ComponentModel;
using ahbsd.lib.crypt.EventArguments;
using ahbsd.lib.crypt.Extensions;
using ahbsd.lib.crypt.Interfaces;
using JetBrains.Annotations;

namespace ahbsd.lib.crypt;

/// <summary>
/// Component to encrypt data.
/// </summary>
[PublicAPI]
public class EncryptorComponent : CryptData, IEncryptor
{
    /// <summary>
    /// Default constructor for the EncryptorComponent class.
    /// </summary>
    /// <remarks>
    /// Initializes the EncryptedData property to an empty character array and sets the Site property to null.
    /// </remarks>
    public EncryptorComponent() => EncryptedData = [];

    /// <summary>
    /// Constructor for the EncryptorComponent class that takes an IContainer as a parameter.
    /// </summary>
    /// <param name="container">The container to add the EncryptorComponent to.</param>
    public EncryptorComponent(IContainer container) : base(container) 
        => EncryptedData = [];
    
    /// <inheritdoc />
    public EncryptorComponent(string name, IContainer container) : base(name, container) 
        => EncryptedData = [];

    #region implementation of IEncryptor
    
    /// <inheritdoc />
    public override event EventHandler<DurationEventArgs>? CryptionDuration;

    /// <summary>
    /// Compares the current instance with another object of the same type and returns an integer that indicates
    /// whether the current instance precedes, follows, or occurs in the same position in the sort order as the
    /// other object.
    /// </summary>
    /// <param name="other">The object to compare with the current instance.</param>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    public int CompareTo(IEncryptor? other)
    {
        var result = 1;

        if (other is ICryptData otherCryptData)
        {
            result = CompareTo(otherCryptData);
        }
        
        return result;
    }

    /// <inheritdoc />
    public char[] EncryptObject(object obj, IList<IKey> keys, int? seed = null)
    {
        OriginalData = obj;
        
        EncryptedData = Encrypt(obj.GetHexArray(), keys, out var duration, seed);
        CryptionDuration?.Invoke(this, new DurationEventArgs(duration));
        return EncryptedData;
    }

    /// <inheritdoc />
    public char[] EncryptString(string str, IList<IKey> keys, int? seed = null)
    {
        OriginalData = str;
        
        EncryptedData = Encrypt(str.GetHexArray(), keys, out var duration, seed);
        CryptionDuration?.Invoke(this, new DurationEventArgs(duration));
        
        return EncryptedData;
    }

    /// <inheritdoc />
    public char[] EncryptedData { get; private set; }
    
    #endregion
    
    /// <summary>
    /// Encrypts the given data.
    /// </summary>
    /// <param name="data">The data to encrypt.</param>
    /// <param name="keys">The keys to use for encryption.</param>
    /// <param name="duration">[out] The duration of the encryption process.</param>
    /// <param name="seed">[optional] The seed to use for encryption.</param>
    /// <returns>The encrypted data.</returns>
    private static char[] Encrypt(char[] data, IList<IKey> keys, out TimeSpan duration, int? seed = null)
    {
        var counter = 0;
        var result = new char[data.Length];
        var now = DateTime.Now;
        duration = TimeSpan.Zero;
        
        foreach (var character in data)
        {
            result[counter] = GetNextKey(keys, counter).EncryptChar(character, seed);
            counter++;
        }
        
        duration = DateTime.Now - now;
        
        return result;
    }

    /// <summary>
    /// Gets the next key from the list of keys.
    /// </summary>
    /// <param name="keys">The given list of keys.</param>
    /// <param name="counter">The counter, which is used to determine the next key.</param>
    /// <returns>The next key from the list of keys.</returns>
    private static IKey GetNextKey(IList<IKey> keys, int counter) => keys[counter % keys.Count];
}