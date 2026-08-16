using System.ComponentModel;
using ahbsd.lib.crypt.EventArguments;
using ahbsd.lib.crypt.Interfaces;
using JetBrains.Annotations;
// ReSharper disable VirtualMemberCallInConstructor

namespace ahbsd.lib.crypt;

/// <summary>
/// Abstract base class for a component that holds the original data before encryption or decryption and provides an
/// event for the duration of the encryption/decryption process.
/// </summary>
[Description("Abstract base class for a component that holds the original data before encryption or decryption and provides an event for the duration of the encryption/decryption process.")]
[Category("Cryptography")]
[PublicAPI]
public abstract class CryptData : Component, ICryptData, IComparable<CryptData>, IComparable
{

    private string _name;
    
    /// <summary>
    /// Default constructor.
    /// </summary>
    protected CryptData()
    {
        _name = GetType().Name;
        Site = new CryptSite(_name, this);
        ((CryptSite)Site).OnNameChanged += OnOnNameChanged;
    }

    /// <summary>
    /// Constructor with a given container.
    /// </summary>
    /// <param name="container">The given container.</param>
    protected CryptData(IContainer container)
    {
        _name = GetType().Name;
        container.Add(this);
        Site = new CryptSite(_name, this, container);
        ((CryptSite)Site).OnNameChanged += OnOnNameChanged;
    }

    /// <summary>
    /// Constructor with a given name and optional a given container.
    /// </summary>
    /// <param name="name">The name of this <see cref="IComponent"/>.</param>
    /// <param name="container">[optional] The given container.</param>
    protected CryptData(string name, IContainer? container = null)
    {
        _name = name;
        if (container is not null)
        {
            container.Add(this);
            Site = new CryptSite(_name, this, container);
            ((CryptSite)Site).OnNameChanged += OnOnNameChanged;
        }
    }

    /// <summary>
    /// Deconstructor
    /// </summary>
    ~CryptData()
    {
        ((CryptSite)Site!).OnNameChanged -= OnOnNameChanged;
        Site = null;
    }

    private void OnOnNameChanged(object? sender, GenericChangeEventArgs<string>? e) 
        => OnNameChanged?.Invoke(sender ?? this, e);

    /// <summary>
    /// Happens when the <see cref="Name"/> property has changed.
    /// </summary>
    public event GenericChangeEventHandler<string>? OnNameChanged; 

    /// <summary>
    /// Gets or sets the name of this <see cref="IComponent"/>.
    /// </summary>
    /// <value>The name of this <see cref="IComponent"/>.</value>
    public string Name
    {
        get => Site?.Name ?? _name;
        set
        {
            if (GenericChangeEventArgs<string>.HasChanged(_name, value, out var e, 
                    StringComparison.CurrentCulture) && e is not null)
            {
                if (Site is CryptSite cryptSite)
                {
                    cryptSite.Name = value;
                }
                else
                {
                    Site = new CryptSite(value, this, Container);
                    ((CryptSite)Site!).OnNameChanged += OnOnNameChanged;
                    _name = value;
                    OnNameChanged?.Invoke(this, e);
                }
            }
        }
    }


    #region implementation of ICryptData
    
    /// <inheritdoc />
    public virtual object Clone() => MemberwiseClone();

    /// <inheritdoc />
    public object? OriginalData { get; protected set; }

    /// <inheritdoc />
    public abstract event EventHandler<DurationEventArgs>? CryptionDuration;

    #endregion

    #region implementation of IComparable<CryptData>
    
    /// <inheritdoc />
    public int CompareTo(CryptData? other) 
        => other is null ? 1 : OriginalData?.GetHashCode() - other.OriginalData?.GetHashCode() ?? 0;

    /// <inheritdoc />
    public int CompareTo(object? obj)
    {
        if (obj is null) return 1;
        if (ReferenceEquals(this, obj)) return 0;
        return obj is CryptData other 
            ? CompareTo(other) 
            : throw new ArgumentException($"Object must be of type {nameof(CryptData)}");
    }

    /// <summary>
    /// Compares two <see cref="CryptData"/> objects.
    /// </summary>
    /// <param name="left">The left <see cref="CryptData"/> object.</param>
    /// <param name="right">The right <see cref="CryptData"/> object.</param>
    /// <returns><c>true</c> if <paramref name="left"/> is less than <paramref name="right"/>; otherwise, <c>false</c>.</returns>
    public static bool operator <(CryptData? left, CryptData? right) 
        => Comparer<CryptData>.Default.Compare(left, right) < 0;

    /// <summary>
    /// Compares two <see cref="CryptData"/> objects.
    /// </summary>
    /// <param name="left">The left <see cref="CryptData"/> object.</param>
    /// <param name="right">The right <see cref="CryptData"/> object.</param>
    /// <returns><c>true</c> if <paramref name="left"/> is greater than <paramref name="right"/>; otherwise, <c>false</c>.</returns>
    public static bool operator >(CryptData? left, CryptData? right) 
        => Comparer<CryptData>.Default.Compare(left, right) > 0;

    /// <summary>
    /// Compares two <see cref="CryptData"/> objects.
    /// </summary>
    /// <param name="left">The left <see cref="CryptData"/> object.</param>
    /// <param name="right">The right <see cref="CryptData"/> object.</param>
    /// <returns><c>true</c> if <paramref name="left"/> is less than or equal to <paramref name="right"/>; otherwise, <c>false</c>.</returns>
    public static bool operator <=(CryptData? left, CryptData? right)
        => Comparer<CryptData>.Default.Compare(left, right) <= 0;

    /// <summary>
    /// Compares two <see cref="CryptData"/> objects.
    /// </summary>
    /// <param name="left">The left <see cref="CryptData"/> object.</param>
    /// <param name="right">The right <see cref="CryptData"/> object.</param>
    /// <returns><c>true</c> if <paramref name="left"/> is greater than or equal to <paramref name="right"/>; otherwise, <c>false</c>.</returns>
    public static bool operator >=(CryptData? left, CryptData? right)
        => Comparer<CryptData>.Default.Compare(left, right) >= 0;
    
    #endregion
}