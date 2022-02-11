namespace osu_farmer;

public partial class TrackerItemControl : ContentView
{
	public string AttachedProperty { get; set; } = "null";

	public string Title { get; set; } = "Example Stat";
	public static readonly BindableProperty TitleProperty = BindableProperty.Create(
														 propertyName: "Title",
														 returnType: typeof(string),
														 declaringType: typeof(TrackerItemControl),
														 defaultValue: "",
														 defaultBindingMode: BindingMode.TwoWay,
														 propertyChanged: TitlePropertyChanged);

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
		CurrentValueElement.Text = string.Format("{0:n0}", value);
	}

	public TrackerItemControl()
	{
		InitializeComponent();
		BindingContext = this;
	}

	private static void TitlePropertyChanged(BindableObject bindable, object oldValue, object newValue)
	{
		var control = (TrackerItemControl)bindable;
		control.TitleElement.Text = newValue.ToString();
	}
}