using Microsoft.Extensions.Localization;
using System.Globalization;

namespace GameFrameX.Foundation.Localization.Core;

/// <summary>
/// Adapts <see cref="ResourceManager"/> to the Microsoft <see cref="IStringLocalizer"/> abstraction.
/// </summary>
public class GameFrameXStringLocalizer : IStringLocalizer
{
    private readonly ResourceManager _resourceManager;

    /// <summary>
    /// Initializes a new instance of the <see cref="GameFrameXStringLocalizer"/> class.
    /// </summary>
    public GameFrameXStringLocalizer(ResourceManager resourceManager)
    {
        _resourceManager = resourceManager ?? throw new ArgumentNullException(nameof(resourceManager));
    }

    /// <inheritdoc />
    public LocalizedString this[string name]
    {
        get
        {
            var value = _resourceManager.GetString(name, CultureInfo.CurrentUICulture);
            var notFound = value == name;
            return new LocalizedString(name, value, notFound, _resourceManager.GetType().FullName);
        }
    }

    /// <inheritdoc />
    public LocalizedString this[string name, params object[] arguments]
    {
        get
        {
            var template = _resourceManager.GetString(name, CultureInfo.CurrentUICulture);
            var notFound = template == name;
            var value = arguments == null || arguments.Length == 0
                            ? template
                            : _resourceManager.FormatString(name, CultureInfo.CurrentUICulture, arguments);

            return new LocalizedString(name, value, notFound, _resourceManager.GetType().FullName);
        }
    }

    /// <inheritdoc />
    public IEnumerable<LocalizedString> GetAllStrings(bool includeParentCultures)
    {
        return Array.Empty<LocalizedString>();
    }
}

/// <summary>
/// Generic <see cref="IStringLocalizer{T}"/> adapter for <see cref="ResourceManager"/>.
/// </summary>
/// <typeparam name="T">The resource owner type.</typeparam>
public sealed class GameFrameXStringLocalizer<T> : GameFrameXStringLocalizer, IStringLocalizer<T>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="GameFrameXStringLocalizer{T}"/> class.
    /// </summary>
    public GameFrameXStringLocalizer(ResourceManager resourceManager)
        : base(resourceManager)
    {
    }
}
