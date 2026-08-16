using JetBrains.Annotations;

namespace ahbsd.lib.crypt.Extensions;

/// <summary>
/// Extensions for the <see cref="string"/> class.
/// </summary>
[PublicAPI]
public static class StringExtension
{
    /// <summary>
    /// Splits the given string into a list of substrings based on the optional specified separator character.
    /// </summary>
    /// <param name="str">The calling string to split.</param>
    /// <param name="separator">[Optional] The character to use as the separator.</param>
    /// <returns>A list of substrings.</returns>
    /// <remarks>If the string is null or whitespace, an empty list is returned.</remarks>
    public static IReadOnlyList<string> SplitToList(this string? str, char separator = ',')
    {
        var result = new List<string>();
        
        if (!string.IsNullOrWhiteSpace(str))
        {
            result.AddRange(str.Split(separator));
        }
        
        return result;
    }
}