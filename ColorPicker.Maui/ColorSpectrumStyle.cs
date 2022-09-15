namespace ColorPicker.Maui;

/// <summary>
/// Enumerate values that describe color spectrum styles of a <see cref="ColorPicker"/>.
/// </summary>
public enum ColorSpectrumStyle
{
    /// <summary>
    /// Hue only style.
    /// </summary>
    HueOnlyStyle,
    /// <summary>
    /// Hue to shade style.
    /// </summary>
    HueToShadeStyle,
    /// <summary>
    /// Shade to hue style.
    /// </summary>
    ShadeToHueStyle,
    /// <summary>
    /// Hue to tint style.
    /// </summary>
    HueToTintStyle,
    /// <summary>
    /// Tint to hue style.
    /// </summary>
    TintToHueStyle,
    /// <summary>
    /// Tint to hue to shade style.
    /// </summary>
    TintToHueToShadeStyle,
    /// <summary>
    /// Shade to hue to tint style.
    /// </summary>
    ShadeToHueToTintStyle
}