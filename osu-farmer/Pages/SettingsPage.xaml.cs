using osu_farmer.Core.Osu;
using osu_farmer.Core;
using osu_farmer.Managers;

namespace osu_farmer;

public partial class SettingsPage : ContentPage
{
    public SettingsPage()
    {
        InitializeComponent();
    }

    public void SetApiKey(string key)
    {
        settingsApiKey.Text = key;
        settingsApiKey.IsPassword = false;
        settingsApiKey.IsPassword = true;
    }
    public void SetUsername(string name) => settingsApiUsername.Text = name;
    public void SetGamemode(Mode mode) => settingsModePicker.SelectedIndex = (int)mode;

    public void PrefillSettings(Settings settings)
    {
        SetApiKey(settings.ApiKey ?? String.Empty);
        SetUsername(settings.ApiUsername ?? String.Empty);
        SetGamemode(settings.ApiGamemode);

        settingsTrackerList.Clear();

        foreach (TrackerItem item in Settings.PrefabTrackers)
        {
            TrackerOptionControl optionControl = new TrackerOptionControl();

            optionControl.AttachedProperty = item.Property;
            optionControl.IsToggled = settings.RunningTrackers.Exists(_item => _item.Property == item.Property);
            optionControl.Title = item.Name;

            settingsTrackerList.Add(optionControl);
        }
    }

    public TrackerOptionControl? GetTrackerOptionControl(string property)
    {
        IView[] children = settingsTrackerList.ToArray();

        foreach (IView item in children)
        {
            if (item is TrackerOptionControl)
            {
                TrackerOptionControl control = (TrackerOptionControl)item;
                if (control != null)
                {
                    if (control.AttachedProperty.Equals(property, StringComparison.InvariantCultureIgnoreCase))
                        return control;
                }
            }
        }
        return null;
    }

    public Settings GetSettings()
    {
        Settings settings = new Settings()
        {
            ApiKey = settingsApiKey.Text,
            ApiUsername = settingsApiUsername.Text,
            ApiGamemode = (Mode)settingsModePicker.SelectedIndex
        };

        settings.Trackers = new Dictionary<string, bool>();
        foreach (TrackerItem item in Settings.PrefabTrackers)
        {
            TrackerOptionControl? control = GetTrackerOptionControl(item.Property);
            if(control!=null){
                settings.Trackers.Add(item.Property, control.IsToggled);
            }
        }

        return settings;
    }

    private void Button_SaveSettings(object sender, EventArgs e)
    {
        Device.BeginInvokeOnMainThread(async () =>
        {
            ButtonSave.IsEnabled = false;
            ButtonReset.IsEnabled = false;

            bool resetSession = false;
            bool apply = true;

            Settings currentSettings = SettingsManager.Instance.Settings;
            Settings newSettings = GetSettings();

            if (currentSettings.ApiUsername != newSettings.ApiUsername || currentSettings.ApiGamemode != newSettings.ApiGamemode)
            {
                bool res = await AppManager.Instance.GetShell().CurrentPage.DisplayAlert("Warning", "The changes you made will reset the current session. Are you sure?", "Yes", "No");
                if (res)
                    resetSession = true;
                else
                    apply = false;
            }

            if (apply)
            {
                SettingsManager.Instance.ApplySettings(newSettings);
                if(resetSession){
                    AppManager.Instance?.StartLoop();
                }
            }

            ButtonSave.IsEnabled = true;
            ButtonReset.IsEnabled = true;
        });

    }

    private void Button_ResetSettings(object sender, EventArgs e)
    {
        PrefillSettings(SettingsManager.Instance.Settings);
    }
}