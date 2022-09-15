namespace ColorPicker.Maui;

internal static class Helpers
{
    public static void DrawGradientRectangle(this ICanvas canvas, RectF rect, ColorFlowDirection direction, IList<Color> colors)
    {
        var gradientPaint = new LinearGradientPaint()
        {
            StartColor = colors.FirstOrDefault(),
            EndColor = colors.LastOrDefault(),
            StartPoint = new Point(0, 0),
            EndPoint = direction switch
            {
                ColorFlowDirection.Horizontal => new Point(1, 0),
                ColorFlowDirection.Vertical => new Point(0, 1),
                _ => throw new InvalidOperationException($"Invalid value for {nameof(ColorFlowDirection)}")
            }
        };

        if (colors.Count > 2)
        {
            var distance = 1.0d / (colors.Count - 1);
            for (int i = 1; i < colors.Count - 1; ++i)
            {
                gradientPaint.AddOffset((float)(distance * i), colors[i]);
            }
        }

        canvas.SetFillPaint(gradientPaint, rect);
        canvas.FillRectangle(rect);
    }
}
