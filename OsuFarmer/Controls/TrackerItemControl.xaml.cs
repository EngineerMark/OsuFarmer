namespace OsuFarmer;

public partial class TrackerItemControl : ContentView
{
	public string AttachedProperty { get; set; } = "null";

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
		ChangedValueElement.Text = string.Format("{0:n0}", value);
	}

	public TrackerItemControl()
	{
		InitializeComponent();
		BindingContext = this;
	}
}