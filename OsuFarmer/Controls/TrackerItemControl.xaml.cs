namespace OsuFarmer;

public partial class TrackerItemControl : ContentView
{
    public string AttachedProperty { get; set; } = "null";
    public bool InvertColors = false;

    private static readonly Color ColorNegative = Colors.Red;
    private static readonly Color ColorPositive = Colors.LightGreen;
    private static readonly Color ColorNeutral = Colors.White;

    public static readonly BindableProperty ChangedTextColorProperty = BindableProperty.Create(nameof(ChangedTextColor), typeof(Color), typeof(TrackerItemControl), Colors.White);

    public Color ChangedTextColor
    {
        get => (Color)GetValue(TrackerItemControl.ChangedTextColorProperty);
        set => SetValue(TrackerItemControl.ChangedTextColorProperty, value);
    }

    public void SetTitle(string title)
    {
        TitleElement.Text = title;
    }

    public void SetCurrentValue(double value)
    {
        CurrentValueElement.Text = string.Format("{0:n0}", value);
        //CurrentValue = string.Format("{0:n0}", value);
    }

    public void SetChangedValue(double value)
    {
        if (value == 0)
            ChangedValueSignElement.Text = string.Empty;
        else if (value > 0)
            ChangedValueSignElement.Text = "\x2227";
        else
            ChangedValueSignElement.Text = "\x2228";

        ChangedValueElement.Text = string.Format("{0:n0}", value);
        ChangedTextColor = !InvertColors ?
            (value == 0 ? ColorNeutral : (value > 0 ? ColorPositive : ColorNegative)) :
            (value == 0 ? ColorNeutral : (value > 0 ? ColorNegative : ColorPositive));
    }

    public TrackerItemControl()
    {
        InitializeComponent();
        BindingContext = this;
    }
}