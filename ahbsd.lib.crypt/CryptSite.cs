using System.ComponentModel;
using ahbsd.lib.crypt.EventArguments;

namespace ahbsd.lib.crypt;

/// <summary>
/// Site for the Crypt Components..
/// </summary>
/// <remarks>Implements <see cref="ISite"/>.</remarks>
public class CryptSite : ISite
{
    private string? _name;
    
    /// <summary>
    /// Internal constructor with a given name, component and optional a container.
    /// </summary>
    /// <param name="name">The given name.</param>
    /// <param name="component">The given component.</param>
    /// <param name="container">[optional] The optional given container.</param>
    internal CryptSite(string name, IComponent component, IContainer? container = null)
    {
        _name = name;
        Component = component;
        Container = container;
        DesignMode = false;
    }
    
    /// <summary>
    /// Happens when the <see cref="Name"/> has changed.
    /// </summary>
    public event GenericChangeEventHandler<string>? OnNameChanged;

    #region implementation of ISite
    
    /// <inheritdoc/>
    public object? GetService(Type serviceType) => throw new NotImplementedException();

    /// <inheritdoc/>
    public IComponent Component { get; }
    /// <inheritdoc/>
    public IContainer? Container { get; }
    /// <inheritdoc/>
    public bool DesignMode { get; internal set; }

    /// <inheritdoc/>
    public string? Name
    {
        get => _name;
        set
        {
            if (GenericChangeEventArgs<string>.HasChanged(_name, value, out var e, StringComparison.CurrentCulture))
            {
                _name = value;
                OnNameChanged?.Invoke(this, e);
            }
        }
    }
    
    #endregion
}