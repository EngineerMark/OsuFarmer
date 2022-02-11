namespace OsuFarmer;

public partial class TrackerOptionControl : ContentView
{
	public string? AttachedProperty { get; set; }

	public static readonly BindableProperty IsToggledProperty = BindableProperty.Create(nameof(IsToggled), typeof(bool), typeof(TrackerOptionControl), false, BindingMode.TwoWay, propertyChanged: IsToggledChanged);


    public bool IsToggled {
		get {
			return (bool)base.GetValue(IsToggledProperty);
        }
		set {
			base.SetValue(IsToggledProperty, value);
        }
    }

	private string? title;
	public string? Title
	{
		get { return title; }
		set
		{
			title = value;
			OnPropertyChanged(nameof(Title)); // Notify that there was a change on this property
		}
	}

	public Switch GetToggle(){
		return ToggleElement;
    }

    private static void IsToggledChanged(BindableObject bindable, object oldValue, object newValue)
    {
		TrackerOptionControl control = (TrackerOptionControl)bindable;
		control.ToggleElement.IsToggled = (bool)newValue;
	}

    public TrackerOptionControl()
	{
		InitializeComponent();
		BindingContext = this;
	}
}