namespace osu_farmer;

public partial class TrackerOptionControl : ContentView
{
	public string? AttachedProperty { get; set; }

	public bool Toggled
	{
		get { return ToggleElement.IsToggled; }
		set { ToggleElement.IsToggled = value; }
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

	public TrackerOptionControl()
	{
		InitializeComponent();
		BindingContext = this;
	}
}