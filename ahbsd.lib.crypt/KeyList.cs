using System.Collections;
using System.ComponentModel;
using ahbsd.lib.crypt.Extensions;
using ahbsd.lib.crypt.Interfaces;

namespace ahbsd.lib.crypt;

/// <summary>
/// Represents a list of cryptographic keys that can be compared with other key lists.
/// </summary>
public class KeyList : Component, IKeyList
{
    private readonly Dictionary<IKey, bool> _keys;

    /// <summary>
    /// Initializes a new instance of the <see cref="KeyList"/> class.
    /// </summary>
    /// <param name="sentence">The sentence to initialize the key list with.</param>
    public KeyList(string sentence)
    {
        _keys = new Dictionary<IKey, bool>();
        Sentence = sentence;
    }

    #region implementation of IKeyList
    
    /// <inheritdoc />
    public string Sentence { get; }

    /// <inheritdoc />
    public bool AddKey(string name, IKey key)
    {
        var result = false;
        if (key == null) throw new ArgumentNullException(nameof(key));
        if (string.IsNullOrWhiteSpace(name)) throw new ArgumentException("The name must not be null or whitespace.", nameof(name));
        if (!name.Equals(key.KeyName, StringComparison.InvariantCultureIgnoreCase)) throw new ArgumentException($"The name '{name}' does not match the key's name '{key.KeyName}'.", nameof(name));
        
        if (!_keys.ContainsKey(key))
        {
            _keys.Add(key, char.IsUpper(name[0]));
            result = true;
        }
        
        return result;
    }

    /// <inheritdoc />
    public bool AddKeys(string sentence, IList<IKey> keys)
    {
        if (string.IsNullOrWhiteSpace(sentence)) throw new ArgumentException("The sentence must not be null or whitespace.", nameof(sentence));
        if (keys == null) throw new ArgumentNullException(nameof(keys));
        if (keys.Count == 0) throw new ArgumentException("The keys must not be empty.", nameof(keys));

        var result = false;
        var keyNames = sentence.SplitToList(' ');
        
        try
        {
            foreach (var keyName in keyNames)
            {
                if (TryFindKeyByName(keyName, keys, out var foundKey) && foundKey != null)
                {
                    result |= AddKey(keyName, foundKey);
                }
            }
        }
        catch (Exception e)
        {
            throw new ArgumentException("An error occurred while adding keys.", e);
        }
        
        return result;
    }

    #endregion
    
    /// <summary>
    /// Tries to find a key by its name in the given collection of keys.
    /// </summary>
    /// <param name="searchName">The name of the key to find.</param>
    /// <param name="keys">The collection of keys to search.</param>
    /// <param name="foundKey">When this method returns true, contains the found key; otherwise, <c>null</c>.</param>
    /// <returns><c>true</c> if the key was found; otherwise, <c>false</c>.</returns>
    internal static bool TryFindKeyByName(string searchName, IEnumerable<IKey> keys, out IKey? foundKey)
    {
        foundKey = null;
        var result = false;
        foreach (var key in keys.Where(key => key.KeyName.Equals(searchName, StringComparison.InvariantCultureIgnoreCase)))
        {
            foundKey = key;
            result = true;
            if (result) break;
        }

        return result;
    }
    
    #region implementation of IReadOnlyDictionary<IKey, bool>
    
    /// <inheritdoc />
    public IEnumerator<KeyValuePair<IKey, bool>> GetEnumerator() => _keys.GetEnumerator();

    /// <inheritdoc />
    IEnumerator IEnumerable.GetEnumerator() => ((IEnumerable)_keys).GetEnumerator();

    /// <inheritdoc />
    public int Count => _keys.Count;

    /// <inheritdoc />
    public bool ContainsKey(IKey key) => _keys.ContainsKey(key);

    /// <inheritdoc />
    public bool TryGetValue(IKey key, out bool value) => _keys.TryGetValue(key, out value);

    /// <inheritdoc />
    public bool this[IKey key] => _keys[key];

    /// <inheritdoc />
    public IEnumerable<IKey> Keys => ((IReadOnlyDictionary<IKey, bool>)_keys).Keys;

    /// <inheritdoc />
    public IEnumerable<bool> Values => ((IReadOnlyDictionary<IKey, bool>)_keys).Values;
    
    #endregion

    #region implementation of IComparable<IKeyList>
    /// <inheritdoc />
    public int CompareTo(IKeyList? other)
    {
        var result = 0;
        if (other == null)
        {
            result = 1;
        }
        else
        {
            var comparison = Count.CompareTo(other.Count);
            if (comparison == 0)
            {
                var thisKeys = Keys.OrderBy(k => k.KeyName).ToList();
                var otherKeys = other.Keys.OrderBy(k => k.KeyName).ToList();

                result = CompareKeys(thisKeys, otherKeys);
            }
            else
            {
                result = comparison;
            }
        }
        return result;
    }

    /// <summary>
    /// Compares two lists of keys and returns an integer that indicates their relative order.
    /// </summary>
    /// <param name="thisKeys">The first list of keys to compare.</param>
    /// <param name="otherKeys">The second list of keys to compare.</param>
    /// <returns>An integer that indicates the relative order of the two lists.</returns>
    private static int CompareKeys(List<IKey> thisKeys, List<IKey> otherKeys)
    {
        var result = 0;
        for (var i = 0; i < thisKeys.Count; i++)
        {
            var comparison = string.Compare(thisKeys[i].KeyName, otherKeys[i].KeyName,
                StringComparison.InvariantCultureIgnoreCase);
            if (comparison != 0)
            {
                result = comparison;
                break;
            }
            else
            {
                comparison = thisKeys[i].CompareTo(otherKeys[i]);
                if (comparison != 0)
                {
                    result = comparison;
                    break;
                }
            }
        }

        return result;
    }

    #endregion

    #region implementation of ICloneable
    
    /// <inheritdoc />
    public object Clone()
    {
        var clone = new KeyList(Sentence);
        var keys = new List<IKey>(_keys.Count);
        keys.AddRange(_keys.Keys);
        
        clone.AddKeys(Sentence, keys);
        
        return clone;
    }
    #endregion
}