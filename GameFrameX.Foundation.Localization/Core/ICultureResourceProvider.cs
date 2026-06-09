using System.Globalization;

namespace GameFrameX.Foundation.Localization.Core;

/// <summary>
/// Supports resource lookup for an explicit culture without mutating global culture state.
/// </summary>
public interface ICultureResourceProvider : IResourceProvider
{
    /// <summary>
    /// Gets a localized string for the specified culture.
    /// </summary>
    /// <param name="key">The resource key.</param>
    /// <param name="culture">The culture to query.</param>
    /// <returns>The localized value when found; otherwise the original key.</returns>
    string GetString(string key, CultureInfo culture);
}
