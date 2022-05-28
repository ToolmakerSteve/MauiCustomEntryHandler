namespace MauiCustomEntryHandler;

public class MyEntry : Entry
{
    /// <summary>
    /// Color of bottom border.
    /// </summary>
    public static BindableProperty UnderlineColorProperty = BindableProperty.Create(
            nameof(UnderlineColor), typeof(Color), typeof(MyEntry), Colors.Black);
    public Color UnderlineColor
    {
        get => (Color)GetValue(UnderlineColorProperty);
        set => SetValue(UnderlineColorProperty, value);
    }

    /// <summary>
    /// Thickness of bottom border.
    /// </summary>
    public static BindableProperty UnderlineThicknessProperty = BindableProperty.Create(
            nameof(UnderlineThickness), typeof(int), typeof(MyEntry), 0);
    public int UnderlineThickness
    {
        get => (int)GetValue(UnderlineThicknessProperty);
        set => SetValue(UnderlineThicknessProperty, value);
    }

    public MyEntry()
	{
	}
}