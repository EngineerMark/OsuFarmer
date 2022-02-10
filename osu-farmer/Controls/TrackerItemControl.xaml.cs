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

	public string CurrentValue { get; set; } = "0";
	public static readonly BindableProperty CurrentValueProperty = BindableProperty.Create(
														 propertyName: "CurrentValue",
														 returnType: typeof(string),
														 declaringType: typeof(TrackerItemControl),
														 defaultValue: "",
														 defaultBindingMode: BindingMode.TwoWay,
														 propertyChanged: CurrentValuePropertyChanged);

	public string ChangedValue { get; set; } = "0";
	public static readonly BindableProperty ChangedValueProperty = BindableProperty.Create(
														 propertyName: "ChangedValue",
														 returnType: typeof(string),
														 declaringType: typeof(TrackerItemControl),
														 defaultValue: "",
														 defaultBindingMode: BindingMode.TwoWay,
														 propertyChanged: ChangedValuePropertyChanged);

	public void SetTitle(string title)
	{
		TitleElement.Text = title;
	}

	public void SetCurrentValue(double value)
	{
		CurrentValueElement.Text = string.Format("{0:n0}", value);
	}

	public void SetChangedValue(double value)
	{
		CurrentValueElement.Text = string.Format("{0:n0}", value);
	}

	public TrackerItemControl()
	{
		InitializeComponent();
	}

	private static void TitlePropertyChanged(BindableObject bindable, object oldValue, object newValue)
	{
		var control = (TrackerItemControl)bindable;
		control.TitleElement.Text = newValue.ToString();
	}

	private static void CurrentValuePropertyChanged(BindableObject bindable, object oldValue, object newValue)
	{
		var control = (TrackerItemControl)bindable;
		control.CurrentValueElement.Text = newValue.ToString();
	}

	private static void ChangedValuePropertyChanged(BindableObject bindable, object oldValue, object newValue)
	{
		var control = (TrackerItemControl)bindable;
		control.ChangedValueElement.Text = newValue.ToString();
	}
}