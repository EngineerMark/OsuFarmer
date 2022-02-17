using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using OsuFarmer.Core.Osu;
using OsuFarmer.Managers;
using OsuFarmer.Core;
using OsuFarmer.Controls;

namespace OsuFarmer.ViewModels
{
    public class SettingsPageViewModel : BaseViewModel
    {
        private string _APIKey = string.Empty;
        private string _APIUsername = string.Empty;
        private int _APIGamemode = 0;
        private int _APIUpdateRate = 30;
        private bool _VisualsHeaderEnabled = true;
        private bool _VisualsProgressTimerEnabled = true;
        private bool _VisualsSmoothProgressTimerEnabled = true;

        public string APIKey { get { return _APIKey; } set { _APIKey = value; OnPropertyChanged(nameof(APIKey)); } }
        public string APIUsername { get { return _APIUsername; } set { _APIUsername = value; OnPropertyChanged(nameof(APIUsername)); } }
        public Mode APIGamemode { get { return (Mode)_APIGamemode; } set { _APIGamemode = (int)value; OnPropertyChanged(nameof(APIGamemode)); } }
        public int APIUpdateRate { get { return _APIUpdateRate; } set { _APIUpdateRate = value; OnPropertyChanged(nameof(APIUpdateRate)); } }
        public bool VisualsHeaderEnabled { get { return _VisualsHeaderEnabled; } set { _VisualsHeaderEnabled = value; OnPropertyChanged(nameof(VisualsHeaderEnabled)); } }
        public bool VisualsProgressTimerEnabled { get { return _VisualsProgressTimerEnabled; } set { _VisualsProgressTimerEnabled = value; OnPropertyChanged(nameof(VisualsProgressTimerEnabled)); } }
        public bool VisualsSmoothProgressTimerEnabled { get { return _VisualsSmoothProgressTimerEnabled; } set { _VisualsSmoothProgressTimerEnabled = value; OnPropertyChanged(nameof(VisualsSmoothProgressTimerEnabled)); } }

        public void OnReset()
        {
            UIManager.Instance?.PrefillSettings(SettingsManager.Instance?.Settings??null);
        }

        public void OnSave()
        {
            Avalonia.Threading.Dispatcher.UIThread.InvokeAsync(async () =>
            {
                await UIManager.Instance?.SetLoadState(true);
                bool resetSession = false;
                bool apply = true;

                Settings? currentSettings = SettingsManager.Instance?.Settings;
                Settings newSettings = GetSettings();

                if (currentSettings?.ApiUsername != newSettings.ApiUsername || currentSettings?.ApiGamemode != newSettings.ApiGamemode)
                {
                    bool res = (await UIManager.Instance?.DisplayAlertAsync("Warning", "The changes you made will reset the current session. Are you sure?", new string[] { "Yes", "No" })).PressedButton == "Yes";
                    if (res)
                        resetSession = true;
                    else
                        apply = false;
                }

                if (apply)
                {
                    await SettingsManager.Instance.ApplySettings(newSettings);
                    if (resetSession)
                    {
                        AppManager.Instance?.StartLoop();
                    }
                }
                await UIManager.Instance.SetLoadState(false);
            });
        }

        private Settings GetSettings()
        {
            Settings settings = new Settings()
            {
                ApiKey = APIKey,
                ApiUsername = APIUsername,
                ApiGamemode = APIGamemode,
                ApiUpdateInterval = APIUpdateRate,
                ShowTrackerTimer = VisualsProgressTimerEnabled,
                SmoothTrackerTimer = VisualsSmoothProgressTimerEnabled
            };

            settings.Trackers = new Dictionary<string, bool>();
            foreach (TrackerItem item in Settings.PrefabTrackers)
            {

                TrackerOptionControl? control = UIManager.Instance.FindTrackerOption(item.Property??string.Empty);
                TrackerOptionControlViewModel? context = (TrackerOptionControlViewModel?)control.DataContext;
                if (control != null && context!=null)
                {
                    settings.Trackers.Add(item.Property ?? string.Empty, context.IsToggled);
                }
            }

            settings.ShowHeaderImage = VisualsHeaderEnabled;


            return settings;
        }
    }
}
