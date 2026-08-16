using System.ComponentModel;
using ahbsd.lib.crypt.EventArguments;
using ahbsd.lib.crypt.Interfaces;
using JetBrains.Annotations;

namespace ahbsd.lib.crypt;

/// <summary>
/// Component to decrypt objects.
/// </summary>
[PublicAPI]
public class DecryptorComponent : CryptData, IDecrypt
{
    /// <inheritdoc/>
    public DecryptorComponent()
    {
        DecryptedObject = null;
    }
    
    /// <inheritdoc/>
    public DecryptorComponent(IContainer container) : base(container)
    {
        DecryptedObject = null;
    }
    
    #region implementation of IDecrypt
    
    /// <inheritdoc/>
    public override event EventHandler<DurationEventArgs>? CryptionDuration;
    
    /// <inheritdoc/>
    public int CompareTo(IDecrypt? other)
    {
        throw new NotImplementedException();
    }

    /// <inheritdoc/>
    public object DecryptObject(char[] obj, IList<IKey> keys)
    {
        object result;
        var list = new List<char>(obj.Length);

        var start = DateTime.Now;
        for (int i = 0; i < obj.Length; i++)
        {
            int index = i % keys.Count;
            var key = keys[index];
            var temp = key.DecryptChar(obj[i]);
            list.Add(temp);
        }
        var duration = DateTime.Now - start;
        CryptionDuration?.Invoke(this, new DurationEventArgs(duration));

        DecryptedObject = list.ToArray();
        result = DecryptedObject;
        return result;
    }

    /// <inheritdoc/>
    public object? DecryptedObject { get; private set; }
    
    #endregion
}