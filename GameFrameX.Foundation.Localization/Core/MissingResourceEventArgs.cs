using System.Globalization;

namespace GameFrameX.Foundation.Localization.Core;

/// <summary>
/// Provides details for a missing localization key.
/// </summary>
public sealed class MissingResourceEventArgs : EventArgs
{
    /// <summary>
    /// Initializes a new instance of the <see cref="MissingResourceEventArgs"/> class.
    /// </summary>
    public MissingResourceEventArgs(string key, CultureInfo culture, IReadOnlyList<CultureInfo> fallbackChain, IReadOnlyList<string> providerNames)
    {
        Key = key;
        Culture = culture;
        FallbackChain = fallbackChain;
        ProviderNames = providerNames;
    }

    /// <summary>
    /// Gets the missing resource key.
    /// </summary>
    public string Key { get; }

    /// <summary>
    /// Gets the requested culture.
    /// </summary>
    public CultureInfo Culture { get; }

    /// <summary>
    /// Gets the cultures that were queried.
    /// </summary>
    public IReadOnlyList<CultureInfo> FallbackChain { get; }

    /// <summary>
    /// Gets the providers that were queried.
    /// </summary>
    public IReadOnlyList<string> ProviderNames { get; }
}
