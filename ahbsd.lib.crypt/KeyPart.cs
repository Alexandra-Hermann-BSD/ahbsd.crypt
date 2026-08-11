using System.Collections;
using System.ComponentModel;
using ahbsd.lib.crypt.Interfaces;

namespace ahbsd.lib.crypt;

/// <summary>
/// Class for a part of a key, which consists of a key character and a list of associated values.
/// </summary>
/// <remarks>
/// The class implements the IKeyPart interface and provides methods to add values and access the stored values.
/// </remarks>
public class KeyPart : IKeyPart
{
    private List<char> _values;
    
    /// <summary>
    /// Constructor with a given key character.
    /// </summary>
    /// <param name="key">The given key character.</param>
    /// <exception cref="ArgumentException">If the given <paramref name="key"/> is not a valid key part.</exception>
    public KeyPart(char key)
    {
        if (!IsValidChar(key)) throw new ArgumentException($"'{key}' is not a valid key part.", nameof(key));

        KeyPartKey = key;
        _values = new List<char>(MaxValuesPerKeyPart);
    }

    #region implementation of IKeyPart
    
    /// <inheritdoc />
    public char KeyPartKey { get; }
    
    /// <inheritdoc />
    public bool AddValue(char value)
    {
        var result = false;
        if (!_values.Contains(value) && _values.Count < MaxValuesPerKeyPart)
        {
            _values.Add(value);
            result = true;
        }

        return result;
    }
    
    /// <inheritdoc />
    public bool IsFull => _values.Count == MaxValuesPerKeyPart;

    #endregion
    
    #region implementation of IList<char>

    /// <inheritdoc />
    public IEnumerator<char> GetEnumerator()
    {
        return _values.GetEnumerator();
    }

    /// <inheritdoc />
    IEnumerator IEnumerable.GetEnumerator()
    {
        return ((IEnumerable)_values).GetEnumerator();
    }

    /// <inheritdoc />
    public int Count => _values.Count;

    /// <inheritdoc />
    public char this[int index] => _values[index];
    
    #endregion

    #region overrides
    
    /// <inheritdoc />
    [Localizable(false)]
    public override string ToString() => $"KeyPart – Key: {KeyPartKey}; {Count} Values";
    
    #endregion

    /// <summary>
    /// Gets the maximum number of values that can be stored in a single <see cref="KeyPart"/>.
    /// </summary>
    /// <value>The maximum number of values.</value>
    internal static int MaxValuesPerKeyPart => char.MaxValue / 16;
    
    /// <summary>
    /// Gets whether the given character is a valid hexadecimal character (0-9, A-F).
    /// </summary>
    /// <param name="c">The given character.</param>
    /// <returns><c>true</c> if the character is valid; otherwise, <c>false</c>.</returns>
    internal static bool IsValidChar(char c) => c is >= '0' and <= '9' or >= 'A' and <= 'F';
}