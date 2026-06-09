namespace GameFrameX.Foundation.Localization.Core;

/// <summary>
/// Defines how formatting failures are handled.
/// </summary>
public enum LocalizationFormatErrorBehavior
{
    /// <summary>
    /// Return the unformatted localization template.
    /// </summary>
    ReturnTemplate = 0,

    /// <summary>
    /// Return the resource key.
    /// </summary>
    ReturnKey = 1,

    /// <summary>
    /// Rethrow the formatting exception.
    /// </summary>
    Throw = 2
}
