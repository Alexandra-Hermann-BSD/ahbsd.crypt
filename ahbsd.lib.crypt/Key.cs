using System.Collections;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using ahbsd.lib.crypt.Exceptions;
using ahbsd.lib.crypt.Interfaces;
using JetBrains.Annotations;

namespace ahbsd.lib.crypt;

/// <summary>
/// Key class that implements the IKey interface representing a collection of key parts for encryption and decryption.
/// </summary>
[PublicAPI]
public class Key : Component, IKey, IComparable<Key>
{
    private Dictionary<char, IKeyPart> _keyParts;
    
    /// <summary>
    /// Constructor with a given key name.
    /// </summary>
    /// <param name="name">The name of the key.</param>
    /// <param name="site">[Optional] The site for the key.</param>
    public Key(string name, ISite? site = null) 
    {
        KeyName = name;
        // ReSharper disable once VirtualMemberCallInConstructor
        Site = site;
        _keyParts = new Dictionary<char, IKeyPart>(16);
        Init();
    }
    
    /// <summary>
    /// Initializes the key parts dictionary with valid hexadecimal characters (0-9, A-F)
    /// as keys and corresponding KeyPart instances as values.
    /// </summary>
    private void Init()
    {
        for (var i = 0; i < 16; i++)
        {
            var c = GetHexChar(i);

            _keyParts.Add(c, new KeyPart(c));
        }
    }

    /// <summary>
    /// Gets the hexadecimal character corresponding to the given integer value (0-15).
    /// </summary>
    /// <param name="i">The given integer value (0-15)</param>
    /// <returns>The hexadecimal character corresponding to the given integer value</returns>
    internal static char GetHexChar(int i)
    {
        char c;
        if (i < 10)
        {
            c = (char)('0' + i);
        }
        else
        {
            c = (char)('A' + i - 10);
        }

        return c;
    }

    #region implementation of IKey
    
    /// <inheritdoc />
    public string KeyName { get; }
    
    /// <inheritdoc />
    [Localizable(false)]
    public bool AddKeyPart(IKeyPart keyPart) 
    {
        if (!KeyPart.IsValidChar(keyPart.KeyPartKey)) throw new ArgumentException($"KeyPart {keyPart.KeyPartKey} is not valid.");
        var result = false;

        if (!_keyParts[keyPart.KeyPartKey].IsFull)
        {
            _keyParts[keyPart.KeyPartKey] = keyPart;
            result = true;
        }

        return result;
    }

    /// <inheritdoc />
    public char DecryptChar(char toDecrypt)
    {
        var result = '\0';

        foreach (var keyPartsValue in _keyParts.Values
                     .Where(keyPartsValue => keyPartsValue
                         .Any(keyPart => keyPart == toDecrypt)))
        {
            result = keyPartsValue.KeyPartKey;
        }
        
        return result;
    }
    
    /// <inheritdoc />
    public char EncryptChar(char toEncrypt, decimal? randomSeed = null)
    {
        char? result = null;

        if (!InvalidCharacterException.IsValidCharacter(toEncrypt, out var exception) && exception != null)
            throw exception;
        
        var random = randomSeed.HasValue ? new Random((int)randomSeed.Value) : new Random();

        var keyPart = _keyParts[toEncrypt];
        
        while (result == null)
        {
            try
            {
                result = keyPart[random.Next(0, keyPart.Count)];
            }
            catch (Exception)
            {
                result = null;
            }
        }

        return result.Value;
    }

    /// <inheritdoc />
    public bool IsReady
    {
        get
        {
            var result = false;

            if (_keyParts.Count == 16)
            {
                result = _keyParts.Values.All(keyPart => keyPart.IsFull);
            }
            return result;
        }
    }

    #endregion

    #region implementation of IReadOnlyDictionary<char, IKeyPart>
    
    /// <inheritdoc />
    public IEnumerator<KeyValuePair<char, IKeyPart>> GetEnumerator() => _keyParts.GetEnumerator();

    /// <inheritdoc />
    IEnumerator IEnumerable.GetEnumerator() => ((IEnumerable)_keyParts).GetEnumerator();

    /// <inheritdoc />
    public int Count => _keyParts.Count;

    /// <inheritdoc />
    public bool ContainsKey(char key) => _keyParts.ContainsKey(key);

    /// <inheritdoc />
    public bool TryGetValue(char key, [MaybeNullWhen(false)] out IKeyPart value) 
        => _keyParts.TryGetValue(key, out value);

    /// <inheritdoc />
    public IKeyPart this[char key] => _keyParts[key];

    /// <inheritdoc />
    public IEnumerable<char> Keys => ((IReadOnlyDictionary<char, IKeyPart>)_keyParts).Keys;

    /// <inheritdoc />
    public IEnumerable<IKeyPart> Values => ((IReadOnlyDictionary<char, IKeyPart>)_keyParts).Values;
    
    #endregion
    
    #region overrides

    /// <inheritdoc />
    public int CompareTo(IKey? other) => CompareTo(other as Key);

    /// <inheritdoc />
    [Localizable(false)]
    public override string ToString() => $"Key – Name: {KeyName}";
    
    #endregion

    #region implementation of IComparable<Key>
    
    /// <inheritdoc />
    public int CompareTo(Key? other)
    {
        if (ReferenceEquals(this, other)) return 0;
        if (other is null) return 1;
        var result = string.Compare(KeyName, other.KeyName, StringComparison.OrdinalIgnoreCase);

        if (result == 0)
        {
            foreach (var keyPart in _keyParts)
            {
                result = keyPart.Value.CompareTo(other[keyPart.Key]);
                if (result != 0) break;
            }
        }

        return result;
    }
    #endregion
}