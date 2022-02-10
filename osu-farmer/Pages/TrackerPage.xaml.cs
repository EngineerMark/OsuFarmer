using osu_farmer.Core;
namespace osu_farmer;

public partial class TrackerPage : ContentPage
{
	public TrackerPage()
	{
		InitializeComponent();
	}

	public TrackerItemControl? GetTrackerControl(string property){
		IView[] children = TrackerItemList.ToArray();

		foreach (IView item in children)
		{
			if (item is TrackerItemControl){
				TrackerItemControl control = (TrackerItemControl)item;
				if(control!=null){
					if (control.AttachedProperty.Equals(property, StringComparison.InvariantCultureIgnoreCase))
						return control;
                }
			}
		}
		return null;
	}

	public void SetCurrentValue(string property, double value){
		TrackerItemControl? tracker = GetTrackerControl(property);
		if(tracker==null){
			return;
        }

		tracker.SetCurrentValue(value);
	}

	public void GenerateTrackerFields(Settings settings){
		if (settings == null)
			return;
		
		TrackerItemList.Clear();

		if(settings.RunningTrackers!=null && settings.RunningTrackers.Count>0)
		{
			foreach(TrackerItem item in Settings.PrefabTrackers)
			{
				TrackerItemControl control = new TrackerItemControl();
				control.SetTitle(item.Name);
				control.SetCurrentValue(0);
				control.SetChangedValue(0);
				control.AttachedProperty = item.Property;
				control.IsVisible = settings.RunningTrackers.Exists(_item => _item.Property == item.Property); ;

				TrackerItemList.Add(control);
			}
		}
	}

	public void UpdateTrackerFields(Settings settings){
		if (settings == null)
			return;

		if (settings.RunningTrackers != null && settings.RunningTrackers.Count > 0)
		{
			foreach (TrackerItem item in Settings.PrefabTrackers)
			{
				TrackerItemControl? control = GetTrackerControl(item.Property);
				control.IsVisible = settings.RunningTrackers.Exists(_item => _item.Property == item.Property); ;
			}
		}
		string s = "";
	}
}