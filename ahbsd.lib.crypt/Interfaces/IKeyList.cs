using System.ComponentModel;
using JetBrains.Annotations;

namespace ahbsd.lib.crypt.Interfaces;

/// <summary>
/// Interface for a list of cryptographic keys that can be compared with other key lists.
/// </summary>
[Description("Interface for a list of cryptographic keys that can be compared with other key lists.")]
[Category("Cryptography")]
[PublicAPI]
public interface IKeyList : IComponent, IReadOnlyDictionary<IKey, bool>, IComparable<IKeyList>, ICloneable
{
    /// <summary>
    /// Gets the sentence representation of the key list.
    /// </summary>
    /// <value>The sentence representation of the key list.</value>
    string Sentence { get; }
    
    /// <summary>
    /// Adds a key to the key list with the specified name.
    /// </summary>
    /// <param name="name">The name of the key.</param>
    /// <param name="key">The key to add.</param>
    /// <returns><c>true</c> if the key was added; otherwise, <c>false</c>.</returns>
    /// <exception cref="ArgumentException">Thrown when the name is invalid.</exception>
    /// <exception cref="ArgumentNullException">Thrown when the name or key is null.</exception>
    bool AddKey(string name, IKey key);
    
    /// <summary>
    /// Adds multiple keys to the key list with the specified sentence.
    /// </summary>
    /// <param name="sentence">The sentence to initialize the key list with.</param>
    /// <param name="keys">The keys to add.</param>
    /// <returns><c>true</c> if the keys were added; otherwise, <c>false</c>.</returns>
    /// <exception cref="ArgumentException">Thrown when the sentence is invalid.</exception>
    /// <exception cref="ArgumentNullException">Thrown when the sentence or keys is null.</exception>
    bool AddKeys(string sentence, IList<IKey> keys);
}