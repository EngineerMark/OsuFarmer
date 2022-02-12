namespace OsuFarmer;

public partial class TrackerOptionControl : ContentView
{
	public string? AttachedProperty { get; set; }

    public static readonly BindableProperty IsToggledProperty = BindableProperty.Create(nameof(IsToggled), typeof(bool), typeof(TrackerOptionControl), true, BindingMode.TwoWay);
    public static readonly BindableProperty TitleProperty = BindableProperty.Create(nameof(Title), typeof(string), typeof(TrackerOptionControl), string.Empty);

	public bool IsToggled
	{
		get => (bool)GetValue(TrackerOptionControl.IsToggledProperty);
		set => SetValue(TrackerOptionControl.IsToggledProperty, value);
	}

	public string Title
	{
		get => (string)GetValue(TrackerOptionControl.TitleProperty);
		set => SetValue(TrackerOptionControl.TitleProperty, value);
	}

	public Switch GetToggle(){
		return ToggleElement;
    }

    public TrackerOptionControl()
	{
		InitializeComponent();
		BindingContext = this;
	}
}