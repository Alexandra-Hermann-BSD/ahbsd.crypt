using System.ComponentModel;
using JetBrains.Annotations;

namespace ahbsd.lib.crypt.Interfaces;

/// <summary>
/// Interface for a part of a key.
/// </summary>
[PublicAPI]
public interface IKeyPart : IComponent, IReadOnlyList<char>, IComparable<IKeyPart>
{
    /// <summary>
    /// Gets the key for this key part.
    /// </summary>
    /// <value>The key for this key part.</value>
    char KeyPartKey { get; }
    
    /// <summary>
    /// Adds a value to the key part.
    /// </summary>
    /// <param name="value">The value to add.</param>
    /// <returns><c>true</c> if the value was added; otherwise, <c>false</c>.</returns>
    bool AddValue(char value);
    
    /// <summary>
    /// Gets whether the key part is full (has reached its maximum number of values).
    /// </summary>
    /// <value><c>true</c> if the key part is full; otherwise, <c>false</c>.</value>
    bool IsFull { get; }
}