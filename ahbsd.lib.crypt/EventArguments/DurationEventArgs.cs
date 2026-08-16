using System.ComponentModel;
using JetBrains.Annotations;

namespace ahbsd.lib.crypt.EventArguments;

/// <summary>
/// Event arguments for the creation duration of a cryptographic key.
/// </summary>
[PublicAPI]
[Description("Event arguments for the creation duration.")]
[Category("Cryptography")]
public class DurationEventArgs : EventArgs
{
    /// <summary>
    /// Constructor with a given creation duration.
    /// </summary>
    /// <param name="duration">The given creation duration</param>
    public DurationEventArgs(TimeSpan duration)
    {
        Duration = duration;
    }
    
    /// <summary>
    /// Gets the creation duration.
    /// </summary>
    /// <value>The creation duration.</value>
    public TimeSpan Duration { get; }
}