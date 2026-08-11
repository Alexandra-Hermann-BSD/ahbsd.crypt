using System.ComponentModel;
using System.Text;
using JetBrains.Annotations;

namespace ahbsd.lib.crypt.EventArguments;

/// <summary>
/// Represents the event arguments for a generic change event, containing the old and new values of type T.
/// </summary>
/// <typeparam name="T">The type of the values being changed.</typeparam>
[PublicAPI]
public class GenericChangeEventArgs<T> : EventArgs
{

    /// <summary>
    /// Constructor with a given old and new value.
    /// </summary>
    /// <param name="oldValue">The old value.</param>
    /// <param name="newValue">The new value.</param>
    public GenericChangeEventArgs(T? oldValue, T? newValue)
    {
        OldValue = oldValue;
        NewValue = newValue;
    }
    
    /// <summary>
    /// Gets the old value before the change.
    /// </summary>
    /// <value>The old value.</value>
    public T? OldValue { get; }
    /// <summary>
    /// Gets the new value after the change.
    /// </summary>
    /// <value>The new value.</value>
    public T? NewValue { get; }
    
    /// <inheritdoc />
    [Localizable(false)]
    public override string ToString() 
        => $"OldValue: {GetValueString(OldValue)}, NewValue: {GetValueString(NewValue)}";

    [Localizable(false)]
    private static string GetValueString(T? value)
    {
        StringBuilder resultBuilder = new StringBuilder();
        
        switch (GetTypeOfValue(value))
        {
            case { } type when type == typeof(string):
                resultBuilder.Append($"\"{value}\" [string]");
                break;
            case { } type when type == typeof(char):
                resultBuilder.Append($"'{value}' [char]");
                break;
            default:
                if (value == null)
                {
                    resultBuilder.Append($"null [{typeof(T).Name}]");
                }
                else
                {
                    resultBuilder.Append($"{value} [{GetTypeOfValue(value)}]");
                }
                break;
        }

        return resultBuilder.ToString();
    }
    
    private static Type GetTypeOfValue(T? value) => value?.GetType() ?? typeof(T);
    
    /// <summary>
    /// Determines whether the old and new values have changed, and returns a GenericChangeEventArgs instance if they have.
    /// Optional <see cref="StringComparison"/> can be specified for string values; otherwise, the default comparison
    /// (<see cref="StringComparison.Ordinal"/>) is used.
    /// </summary>
    /// <remarks>
    /// The optional <see cref="StringComparison"/> parameter is only relevant when comparing string values.<br/>
    /// If <typeparamref name="T"/> isn't <see cref="string"/> or <see cref="object"/>, the <paramref name="comparison"/>
    /// parameter is ignored.<br/>
    /// If <typeparamref name="T"/> is <see cref="object"/> and the <see cref="Type"/> of <paramref name="oldValue"/> is 
    /// <see cref="string"/> or <paramref name="newValue"/> is <see cref="string"/>, the <paramref name="comparison"/>
    /// parameter is used.<br/>
    /// For other types, the default equality comparison is used.
    /// </remarks>
    /// <param name="oldValue">The given old value.</param>
    /// <param name="newValue">The given new value.</param>
    /// <param name="e">When this method returns, contains the GenericChangeEventArgs instance if the values have
    /// changed, or <c>null</c> if they have not.</param>
    /// <param name="comparison">[optional] The string comparison to use.</param>
    /// <returns><c>true</c> if the values have changed; otherwise, <c>false</c>.</returns>
    public static bool HasChanged(T? oldValue, T? newValue, out GenericChangeEventArgs<T>? e, StringComparison comparison = StringComparison.Ordinal)
    {
        bool result;
        
        if (GetTypeOfValue(oldValue) == typeof(string) || GetTypeOfValue(newValue) == typeof(string))
        {
            result = !string.Equals(oldValue?.ToString(), newValue?.ToString(), comparison);
        }
        else if (GetTypeOfValue(oldValue) == typeof(char) && GetTypeOfValue(newValue) == typeof(char))
        {
            result = !char.Equals(oldValue as char?, newValue as char?);
        }
        else
        {
            result = !Equals(oldValue, newValue);
        }

        e = result ? new GenericChangeEventArgs<T>(oldValue, newValue) : null;

        return result;
    }
}

/// <summary>
/// Represents a delegate for handling generic change events, which takes an object sender and GenericChangeEventArgs of type T as parameters.
/// </summary>
/// <param name="sender">The source of the event.</param>
/// <param name="e">The GenericChangeEventArgs instance containing the event data.</param>
/// <typeparam name="T">The type of the values being compared.</typeparam>
[PublicAPI]
public delegate void GenericChangeEventHandler<T>(object? sender, GenericChangeEventArgs<T>? e);