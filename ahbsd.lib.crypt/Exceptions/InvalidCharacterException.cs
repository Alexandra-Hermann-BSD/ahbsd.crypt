using JetBrains.Annotations;

namespace ahbsd.lib.crypt.Exceptions;

/// <summary>
/// Exception for invalid characters.
/// </summary>
[PublicAPI]
public class InvalidCharacterException : Exception
{
    /// <summary>
    /// Private constructor with a given invalid character.
    /// </summary>
    /// <param name="invalidCharacter">The given invalid character</param>
    /// <remarks>
    /// This constructor is private to ensure that the static method
    /// <see cref="IsValidCharacter(char, out InvalidCharacterException?)"/> can create the exception.
    /// </remarks>
    private InvalidCharacterException(char invalidCharacter) : base(GetMessage(invalidCharacter))
        => InvalidCharacter = invalidCharacter;

    /// <summary>
    /// Gets the invalid character that was used.
    /// </summary>
    /// <value>The invalid character.</value>
    public char InvalidCharacter { get; }

    private static string GetMessage(char invalidCharacter)
        => $"The given character '{invalidCharacter}' is invalid. Valid characters are 0-9 and A-F.";

    /// <summary>
    /// Tests whether the given character is valid.
    /// </summary>
    /// <param name="testChar">The character to test.</param>
    /// <param name="exception">
    /// [out] If the <paramref name="testChar"/> is invalid, this will contain the exception; otherwise <c>null</c>.
    /// </param>
    /// <returns><c>true</c> if the character is valid; otherwise <c>false</c>.</returns>
    public static bool IsValidCharacter(char testChar, out InvalidCharacterException? exception)
    {
        var result = false;
        exception = null;

        if (testChar >= '0' && (testChar <= '9' || testChar >= 'A') && testChar <= 'F')
        {
            result = true;
        }
        else
        {
            exception = new InvalidCharacterException(testChar);
            result = false;
        }

        return result;
    }
}