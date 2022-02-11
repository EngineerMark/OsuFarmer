using OsuFarmer.Core;
using OsuFarmer.Core.Osu;
using OsuFarmer.Managers;
using System.Globalization;

namespace OsuFarmer;

public partial class TrackerPage : ContentPage
{
	public TrackerPage()
	{
		InitializeComponent();
	}

	public void ApplyUser(User user){
		trackerUserAvatar.Source = "https://a.ppy.sh/" + user.ID;
		trackerUsername.Text = user.Username;
		trackerUserFlag.Source = "https://assets.ppy.sh/old-flags/" + user.Country + ".png";

		CultureInfo[] cultures = CultureInfo.GetCultures(CultureTypes.SpecificCultures);
		string? country = null;
		foreach (CultureInfo culture in cultures)
		{
			RegionInfo region = new RegionInfo(culture.LCID);
			if (region.TwoLetterISORegionName.ToUpper() == user.Country.ToUpper())
			{
				country = region.DisplayName;
				break;
			}
		}
		trackerUserCountry.Text = country ?? "Unknown";
	}

	public void ApplySession(Session session){
		foreach (TrackerItem item in SettingsManager.Instance.Settings.RunningTrackers){

			double original = Convert.ToInt64(session.Start[item.Property]);
			double current = Convert.ToInt64(session.Latest[item.Property]);

			double diff = current - original;

			TrackerItemControl? tracker = GetTrackerControl(item.Property);
			tracker.SetCurrentValue(current);
			tracker.SetChangedValue(diff);
		}
    }

	public void SetLoadedState(bool state){
		trackerViewer.IsVisible = state;
		trackerLoader.IsRunning = !state;
	}

	public TrackerItemControl? GetTrackerControl(string property){
		IView[] children = TrackerItemList.Children.ToArray();

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

	public async void GenerateTrackerFields(Settings settings){
		if (settings == null)
			return;

		TrackerItemList.Children.Clear();
		if (settings.RunningTrackers!=null && settings.RunningTrackers.Count>0)
		{
			foreach(TrackerItem item in Settings.PrefabTrackers)
			{
				TrackerItemControl c = new TrackerItemControl();
				TrackerItemList.Children.Add(c);
				c.SetTitle(item.Name);
				c.SetChangedValue(0);
				c.AttachedProperty = item.Property;
				c.IsVisible = settings.RunningTrackers.Exists(_item => _item.Property == item.Property);
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
	}

    private void Button_LoadSession(object sender, EventArgs e)
    {
		PageManager.Instance?.GoTo<SessionsPage>();
    }

    private void Button_NewSession(object sender, EventArgs e)
    {
		//SessionManager.Instance?.StartNewSession();
		Device.InvokeOnMainThreadAsync(async () =>
		{
			bool reset = await DisplayAlert("New session", "Are you sure you want to start a new session?", "Yes", "No");
			if(reset)
				AppManager.Instance?.StartLoop(true);
		});
	}

	private void Button_SaveSession(object sender, EventArgs e)
    {
		Device.InvokeOnMainThreadAsync(async () =>
		{
			SetLoadedState(false);
			bool replace = false;
			string name = SessionManager.Instance?.CurrentSession?.Name;
			//check if it exists
			Session? s = SessionManager.Instance?.GetSessionByName(name);
            if (s != null)
            {
				//ask to replace
				replace = await DisplayAlert("Session already exists", "Do you want to overwrite the existing stored session?", "Yes", "No");
			}
			if(!replace)
				name = await DisplayPromptAsync("Save Session", "Please enter a name for this session", "Save", "Cancel");

			SessionManager.Instance?.CurrentSession?.SetName(name);

			bool result = await SessionManager.Instance?.SaveSessionToFile(SessionManager.Instance.CurrentSession);

            if (!result)
				await DisplayAlert("Oops", "Something went wrong while saving, please retry later.", "Ok");

			SetLoadedState(true);
		});
	}
}