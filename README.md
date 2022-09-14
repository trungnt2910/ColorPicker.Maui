<img src="https://raw.githubusercontent.com/trungnt2910/ColorPicker.Maui/master/Art/icon.png" width="200px" />

# ColorPicker.Maui 🎨
a color picker control for .NET MAUI powered on SkiaSharp.

[![CI](https://github.com/trungnt2910/ColorPicker.Maui/actions/workflows/ci.yml/badge.svg)](https://github.com/trungnt2910/ColorPicker.Maui/actions/workflows/ci.yml)
[![](https://img.shields.io/nuget/v/ColorPicker.Maui)](https://www.nuget.org/packages/ColorPicker.Maui)
[![](https://img.shields.io/nuget/dt/ColorPicker.Maui)](https://www.nuget.org/packages/ColorPicker.Maui)

This is a **fork** of [Maui.ColorPicker](https://github.com/nor0x/Maui.ColorPicker) with extra fixes and features maintained by me.

this is largely based on `XFColorPickerControl` for Xamarin.Forms (https://github.com/UdaraAlwis/XFColorPickerControl) by [UdaraAlwis](https://github.com/UdaraAlwis) who allowed me to publish this updated version of the control 🙌

## Getting Started
add namespace
```xml
 xmlns:cp="https://trungnt2910.github.io/schemas/maui/2022/colorpicker"
```
create control
```xml
<cp:ColorPicker
    x:Name="ColorPicker"
    ColorListDirection="Horizontal"
    GradientColorStyle="DarkToColorsToLightStyle"
    PickedColorChanged="ColorPicker_PickedColorChanged"
    PickedColor="Blue">
</cp:ColorPicker>
```

more to come...  🔜

## Exclusive features
- Setting `PickedColor` field. This enables many features such as two-way binding and setting an initial picked color.
- Improved performance due to less unnecessary re-rendering.
- Some other bug fixes (`PointerRingPositionXUnits`, `PointerRingPositionYUnits` properties are fixed).

## Documentation
You can access the library's latest documentation [here](https://trungnt2910.github.io/ColorPicker.Maui/).
