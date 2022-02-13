using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using OsuFarmer.Core.Osu;
using OsuFarmer.Managers;
using OsuFarmer.Core;
using MessageBox.Avalonia.Enums;
using OsuFarmer.Controls;

namespace OsuFarmer.ViewModels
{
    public class SettingsPageViewModel : BaseViewModel
    {
        private string _APIKey = string.Empty;
        private string _APIUsername = string.Empty;
        private int _APIGamemode = 0;
        private bool _VisualsHeaderEnabled = true;

        public string APIKey { get { return _APIKey; } set { _APIKey = value; OnPropertyChanged(nameof(APIKey)); } }
        public string APIUsername { get { return _APIUsername; } set { _APIUsername = value; OnPropertyChanged(nameof(APIUsername)); } }
        public Mode APIGamemode { get { return (Mode)_APIGamemode; } set { _APIGamemode = (int)value; OnPropertyChanged(nameof(APIGamemode)); } }
        public bool VisualsHeaderEnabled { get { return _VisualsHeaderEnabled; } set { _VisualsHeaderEnabled = value; OnPropertyChanged(nameof(VisualsHeaderEnabled)); } }

        public void OnReset()
        {
            UIManager.Instance?.PrefillSettings(SettingsManager.Instance?.Settings??null);
        }

        public void OnSave()
        {
            Avalonia.Threading.Dispatcher.UIThread.InvokeAsync(async () =>
            {
                UIManager.Instance.SetLoadState(true);
                bool resetSession = false;
                bool apply = true;

                Settings? currentSettings = SettingsManager.Instance?.Settings;
                Settings newSettings = GetSettings();

                if (currentSettings?.ApiUsername != newSettings.ApiUsername || currentSettings?.ApiGamemode != newSettings.ApiGamemode)
                {
                    bool res = (await UIManager.Instance?.DisplayAlertAsync("Warning", "The changes you made will reset the current session. Are you sure?", ButtonEnum.YesNo)) == ButtonResult.Yes;
                    if (res)
                        resetSession = true;
                    else
                        apply = false;
                }

                if (apply)
                {
                    await SettingsManager.Instance?.ApplySettings(newSettings);
                    if (resetSession)
                    {
                        AppManager.Instance?.StartLoop();
                    }
                }
                UIManager.Instance.SetLoadState(false);
            });
        }

        private Settings GetSettings()
        {
            Settings settings = new Settings()
            {
                ApiKey = APIKey,
                ApiUsername = APIUsername,
                ApiGamemode = APIGamemode
            };

            settings.Trackers = new Dictionary<string, bool>();
            foreach (TrackerItem item in Settings.PrefabTrackers)
            {

                TrackerOptionControl? control = UIManager.Instance.FindTrackerOption(item.Property);
                TrackerOptionControlViewModel? context = (TrackerOptionControlViewModel)control.DataContext;
                if (control != null && context!=null)
                {
                    settings.Trackers.Add(item.Property, context.IsToggled);
                }
            }

            settings.ShowHeaderImage = VisualsHeaderEnabled;


            return settings;
        }
    }
}
