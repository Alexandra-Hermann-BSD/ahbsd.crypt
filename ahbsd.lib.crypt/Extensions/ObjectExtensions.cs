using System.Text;
using System.Text.Json;

namespace ahbsd.lib.crypt.Extensions;

/// <summary>
/// Extensions for <see cref="object"/>.
/// </summary>
public static class ObjectExtensions
{
    
    /// <summary>
    /// Gets an hexadecimal representation of the <see cref="object"/>.
    /// </summary>
    /// <param name="obj">The calling object</param>
    /// <returns>The hexadecimal representation of the <see cref="object"/>.</returns>
    public static char[] GetHexArray(this object obj)
    {
        // 1. Objekt in einen JSON-String umwandeln
        var jsonString = JsonSerializer.Serialize(obj);
    
        // 2. Den String in ein Byte-Array umwandeln (UTF-8 Format)
        var bytes = Encoding.UTF8.GetBytes(jsonString);
        
        var resultList = new List<char>(bytes.Length * 2);
        
        foreach (var b in bytes)
        {
            var hexChars = b.ToString("X2").ToCharArray();
            var firstChar = hexChars[0];
            var secondChar = hexChars[1];
            
            resultList.Add(firstChar);
            resultList.Add(secondChar);
        }
        
        return resultList.ToArray();
    }
    
    
}