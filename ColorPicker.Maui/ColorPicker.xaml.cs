using Microsoft.Maui.Graphics.Converters;
using System.Collections;

namespace ColorPicker.Maui;

/// <summary>
/// A control that allows the user to pick a <see cref="Color"/>.
/// </summary>
public partial class ColorPicker : ContentView, IDrawable
{
    private Color? _pendingPickedColor = null;
    private bool _rendering = false;

    /// <summary>
    /// Initializes a new instance of the <see cref="ColorPicker"/> class.
    /// </summary>
	public ColorPicker()
	{
		InitializeComponent();
	}

    /// <summary>
    /// Occurs when the Picked Color changes
    /// </summary>
    public event EventHandler<PickedColorChangedEventArgs>? PickedColorChanged;

    /// <summary>
    /// Checks whether this <see cref="ColorPicker"/> is in a rendering state.
    /// A <see cref="ColorPicker"/> in a rendering state cannot have its properties
    /// modified by outside code.
    /// </summary>
    public bool IsRendering => _rendering;

    /// <summary>
    /// Backing store for the <see cref="PickedColor"/> property.
    /// </summary>
    public static readonly BindableProperty PickedColorProperty
        = BindableProperty.Create(
            nameof(PickedColor),
            typeof(Color),
            typeof(ColorPicker),
            propertyChanged: (bindable, value, newValue) =>
            {
                if (!newValue.Equals(value) && (bindable is ColorPicker picker))
                {
                    picker.PickedColorChanged?
                        .Invoke(picker, new PickedColorChangedEventArgs((Color?)value, (Color)newValue));
                    if (!picker._rendering)
                    {
                        picker._pendingPickedColor = (Color)newValue;
                        picker.CanvasView.Invalidate();
                    }
                }
            });

    /// <summary>
    /// Gets and sets the current picked <see cref="Color"/>. This is a bindable property.
    /// </summary>
    /// <value>
    /// A <see cref="Color"/> containing the picked color. The default value is <see langword="null"/>.
    /// </value>
    /// <remarks>
    /// Setting this value to <see langword="null"/> makes the control honor the values set
    /// to <see cref="PointerRingPositionXUnits"/> and <see cref="PointerRingPositionYUnits"/>
    /// instead.
    /// <br/>
    /// Setting this property will cause the <see cref="PickedColorChanged"/> event to be emitted.
    /// </remarks>
    public Color PickedColor
    {
        get { return (Color)GetValue(PickedColorProperty); }
        set 
        {
            if (!_rendering)
            {
                SetValue(PickedColorProperty, value);
            }
        }
    }

    /// <summary>
    /// Backing store for the <see cref="ColorSpectrumStyle"/> property.
    /// </summary>
    public static readonly BindableProperty ColorSpectrumStyleProperty
     = BindableProperty.Create(
         nameof(ColorSpectrumStyle),
         typeof(ColorSpectrumStyle),
         typeof(ColorPicker),
         ColorSpectrumStyle.HueToShadeStyle,
         BindingMode.Default, null,
         propertyChanged: (bindable, value, newValue) =>
         {
             if (newValue != null)
                 ((ColorPicker)bindable).CanvasView.Invalidate();
             else
                 ((ColorPicker)bindable).ColorSpectrumStyle = default;
         });

    /// <summary>
    /// Gets or sets the Color Spectrum Gradient Style.
    /// </summary>
    public ColorSpectrumStyle ColorSpectrumStyle
    {
        get { return (ColorSpectrumStyle)GetValue(ColorSpectrumStyleProperty); }
        set { SetValue(ColorSpectrumStyleProperty, value); }
    }

    /// <summary>
    /// Backing store for the <see cref="BaseColorList"/> property.
    /// </summary>
    public static readonly BindableProperty BaseColorListProperty
            = BindableProperty.Create(
                nameof(BaseColorList),
                typeof(IEnumerable),
                typeof(ColorPicker),
                new string[]
                {
                    new Color(255, 0, 0).ToHex(), // Red
					new Color(255, 255, 0).ToHex(), // Yellow
					new Color(0, 255, 0).ToHex(), // Green (Lime)
					new Color(0, 255, 255).ToHex(), // Aqua
					new Color(0, 0, 255).ToHex(), // Blue
					new Color(255, 0, 255).ToHex(), // Fuchsia
					new Color(255, 0, 0).ToHex(), // Red
				},
                BindingMode.Default, null,
                propertyChanged: (bindable, value, newValue) =>
                {
                    if (newValue != null)
                        ((ColorPicker)bindable).CanvasView.Invalidate();
                    else
                        ((ColorPicker)bindable).BaseColorList = Array.Empty<string>();
                });

    /// <summary>
    /// Gets or sets the Base Color List.
    /// </summary>
    public IEnumerable BaseColorList
    {
        get { return (IEnumerable)GetValue(BaseColorListProperty); }
        set { SetValue(BaseColorListProperty, value); }
    }

    /// <summary>
    /// Backing store for the <see cref="ColorFlowDirection"/> property.
    /// </summary>
    public static readonly BindableProperty ColorFlowDirectionProperty
        = BindableProperty.Create(
            nameof(ColorFlowDirection),
            typeof(ColorFlowDirection),
            typeof(ColorPicker),
            ColorFlowDirection.Horizontal,
            BindingMode.Default, null,
            propertyChanged: (bindable, value, newValue) =>
            {
                if (newValue != null)
                    ((ColorPicker)bindable).CanvasView.Invalidate();
                else
                    ((ColorPicker)bindable).ColorFlowDirection = default;
            });

    /// <summary>
    /// Gets or sets the Color List flow Direction.
    /// </summary>
    /// <value>
    /// Either <see cref="ColorFlowDirection.Horizontal"/> or <see cref="ColorFlowDirection.Vertical"/>.
    /// </value>
    public ColorFlowDirection ColorFlowDirection
    {
        get { return (ColorFlowDirection)GetValue(ColorFlowDirectionProperty); }
        set { SetValue(ColorFlowDirectionProperty, value); }
    }

    /// <summary>
    /// Backing store for the <see cref="PointerRingDiameterUnits"/> property.
    /// </summary>
    public static readonly BindableProperty PointerRingDiameterUnitsProperty
        = BindableProperty.Create(
            nameof(PointerRingDiameterUnits),
            typeof(double),
            typeof(ColorPicker),
            0.6,
            BindingMode.Default,
            validateValue: (bindable, value) =>
            {
                return (((double)value > -1) && ((double)value <= 1));
            },
            propertyChanged: (bindable, value, newValue) =>
            {
                if (newValue != null)
                    ((ColorPicker)bindable).CanvasView.Invalidate();
                else
                    ((ColorPicker)bindable).PointerRingDiameterUnits = default;
            });

    /// <summary>
    /// Gets or sets the Picker Pointer Ring Diameter.
    /// The size is calculated relative to the view canvas size.
    /// </summary>
    /// <value>
    /// A number between 0 - 1.
    /// </value>
    public double PointerRingDiameterUnits
    {
        get { return (double)GetValue(PointerRingDiameterUnitsProperty); }
        set { SetValue(PointerRingDiameterUnitsProperty, value); }
    }

    /// <summary>
    /// Backing store for the <see cref="PointerRingBorderUnits"/> property.
    /// </summary>
    public static readonly BindableProperty PointerRingBorderUnitsProperty
        = BindableProperty.Create(
            nameof(PointerRingBorderUnits),
            typeof(double),
            typeof(ColorPicker),
            0.3,
            BindingMode.Default,
            validateValue: (bindable, value) =>
            {
                return (((double)value > -1) && ((double)value <= 1));
            },
            propertyChanged: (bindable, value, newValue) =>
            {
                if (newValue != null)
                    ((ColorPicker)bindable).CanvasView.Invalidate();
                else
                    ((ColorPicker)bindable).PointerRingBorderUnits = default;
            });

    /// <summary>
    /// Gets or sets the Picker Pointer Ring Border Size.
    /// The size is calculated relative to the pixel size of the picker pointer.
    /// </summary>
    /// <value>
    /// A number between 0 - 1.
    /// </value>
    public double PointerRingBorderUnits
    {
        get { return (double)GetValue(PointerRingBorderUnitsProperty); }
        set { SetValue(PointerRingBorderUnitsProperty, value); }
    }

    /// <summary>
    /// Backing store for the <see cref="PointerRingPositionXUnits"/> property.
    /// </summary>
    public static readonly BindableProperty PointerRingPositionXUnitsProperty
        = BindableProperty.Create(
            nameof(PointerRingPositionXUnits),
            typeof(double),
            typeof(ColorPicker),
            0.5,
            BindingMode.OneTime,
            validateValue: (bindable, value) =>
            {
                return (((double)value > -1) && ((double)value <= 1));
            },
            propertyChanged: (bindable, value, newValue) =>
            {
                if ((double)newValue != (double)value && bindable is ColorPicker picker && !picker._rendering)
                {
                    picker._pendingPickedColor = null;
                    picker.CanvasView.Invalidate();
                }
            });

    /// <summary>
    /// Gets or sets the Picker Pointer X position.
    /// The value is calculated relative to the view canvas width.
    /// </summary>
    /// <value>
    /// A number between 0 - 1.
    /// </value>
    public double PointerRingPositionXUnits
    {
        get { return (double)GetValue(PointerRingPositionXUnitsProperty); }
        set
        {
            if (!_rendering)
            {
                SetValue(PointerRingPositionXUnitsProperty, value);
            }
        }
    }

    /// <summary>
    /// Backing store for the <see cref="PointerRingPositionYUnits"/> property.
    /// </summary>
    public static readonly BindableProperty PointerRingPositionYUnitsProperty
        = BindableProperty.Create(
            nameof(PointerRingPositionYUnits),
            typeof(double),
            typeof(ColorPicker),
            0.5,
            BindingMode.OneTime,
            validateValue: (bindable, value) =>
            {
                return (((double)value > -1) && ((double)value <= 1));
            },
            propertyChanged: (bindable, value, newValue) =>
            {
                if ((double)newValue != (double)value && bindable is ColorPicker picker && !picker._rendering)
                {
                    picker._pendingPickedColor = null;
                    picker.CanvasView.Invalidate();
                }
            });

    /// <summary>
    /// Gets or sets the Picker Pointer Y position.
    /// The value is calculated relative to the view canvas height.
    /// </summary>
    /// <value>
    /// A number between 0 - 1.
    /// </value>
    public double PointerRingPositionYUnits
    {
        get { return (double)GetValue(PointerRingPositionYUnitsProperty); }
        set 
        { 
            if (!_rendering)
            {
                SetValue(PointerRingPositionYUnitsProperty, value);
            } 
        }
    }

    /// <summary>
    /// To be added.
    /// </summary>
    /// <param name="canvas"></param>
    /// <param name="rect"></param>
    public void Draw(ICanvas canvas, RectF rect)
    {
        _rendering = true;

        canvas.Antialias = true;

        // Initiate the base Color list
        var converter = new ColorTypeConverter();
        IList<Color> colors = BaseColorList
            .Cast<object>()
            .Select(color => converter.ConvertFromInvariantString(color?.ToString() ?? string.Empty))
            .Where(color => color != null)
            .Cast<Color>()
            .ToList();

        // Draw gradient rainbow Color spectrum
        canvas.DrawGradientRectangle(rect, ColorFlowDirection, colors);

        // Draw secondary gradient color spectrum
        canvas.DrawGradientRectangle(rect,
            ColorFlowDirection == ColorFlowDirection.Horizontal ?
                ColorFlowDirection.Vertical : ColorFlowDirection.Horizontal,
            GetSecondaryLayerColors(ColorSpectrumStyle)
            );

        _rendering = false;
    }

    private void CanvasView_Interaction(object sender, TouchEventArgs e)
    {
        var touch = e.Touches.FirstOrDefault();

        // Check for each touch point XY position to be inside Canvas
        // Ignore any Touch event occured outside the Canvas region 
        if (touch.X > 0 && touch.X < CanvasView.Width &&
            touch.Y > 0 && touch.Y < CanvasView.Height)
        {
            _pendingPickedColor = null;

            // Prevent double re-rendering.
            _rendering = true;
            SetValue(PointerRingPositionXUnitsProperty, touch.X / CanvasView.Width);
            SetValue(PointerRingPositionYUnitsProperty, touch.Y / CanvasView.Height);
            _rendering = false;

            // Explicitly render things now.
            CanvasView.Invalidate();
        }
    }

    private Color[] GetSecondaryLayerColors(ColorSpectrumStyle colorSpectrumStyle)
    {
        switch (colorSpectrumStyle)
        {
            case ColorSpectrumStyle.HueOnlyStyle:
                return new Color[]
                {
                        Colors.Transparent
                };
            case ColorSpectrumStyle.HueToShadeStyle:
                return new Color[]
                {
                        Colors.Transparent,
                        Colors.Black
                };
            case ColorSpectrumStyle.ShadeToHueStyle:
                return new Color[]
                {
                        Colors.Black,
                        Colors.Transparent
                };
            case ColorSpectrumStyle.HueToTintStyle:
                return new Color[]
                {
                        Colors.Transparent,
                        Colors.White
                };
            case ColorSpectrumStyle.TintToHueStyle:
                return new Color[]
                {
                        Colors.White,
                        Colors.Transparent
                };
            case ColorSpectrumStyle.TintToHueToShadeStyle:
                return new Color[]
                {
                        Colors.White,
                        Colors.Transparent,
                        Colors.Black
                };
            case ColorSpectrumStyle.ShadeToHueToTintStyle:
                return new Color[]
                {
                        Colors.Black,
                        Colors.Transparent,
                        Colors.White
                };
            default:
                return new Color[]
                {
                        Colors.Transparent,
                        Colors.Black
                };
        }
    }
}