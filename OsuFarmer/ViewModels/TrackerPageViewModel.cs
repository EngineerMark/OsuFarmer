using Avalonia.Controls;
using Avalonia.Media;
using Avalonia.Media.Imaging;
using OsuFarmer.Helpers;
using OsuFarmer.Managers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OsuFarmer.Core;
using OsuFarmer.Alerts;

namespace OsuFarmer.ViewModels
{
    public class TrackerPageViewModel : BaseViewModel
    {
        public static readonly Bitmap? DefaultHeaderImage = ImageHelper.GetAvaloniaBitmapFromAssets("avares://OsuFarmer/Assets/Images/Placeholders/header.jpg");
        public static readonly Bitmap? DefaultAvatarImage = ImageHelper.GetAvaloniaBitmapFromAssets("avares://OsuFarmer/Assets/Images/Placeholders/avatar-guest.png");
        public static readonly Bitmap? DefaultFlagImage = ImageHelper.GetAvaloniaBitmapFromAssets("avares://OsuFarmer/Assets/Images/Flags/__.png");

        private bool _ExpansiveModeEnabled = false;
        private double _ThinTrackerWidth = double.NaN;
        private string _Username = "peppy";
        private string _CountryName = "Australia";
        private Bitmap? _HeaderImage = DefaultHeaderImage;
        private Bitmap? _AvatarImage = DefaultAvatarImage;
        private Bitmap? _FlagImage = DefaultFlagImage;

        private bool _showTimer = true;
        private double _TimerProgress = 0;
        private string _TimerText = "Next update in 0 seconds";
        private bool _ShowHeader = true;
        private bool _ShowClock = true;
        private string _ClockValue = "--:--:--";

        public bool ExpansiveModeEnabled { get { return _ExpansiveModeEnabled; } set { _ExpansiveModeEnabled = value; OnPropertyChanged(nameof(ExpansiveModeEnabled)); } }
        public double ThinTrackerWidth { get { return _ThinTrackerWidth; } set { _ThinTrackerWidth = value; OnPropertyChanged(nameof(ThinTrackerWidth)); } }
        public string Username { get { return _Username; } set { _Username = value; OnPropertyChanged(nameof(Username)); } }
        public string CountryName { get { return _CountryName; } set { _CountryName = value; OnPropertyChanged(nameof(CountryName)); } }
        public Bitmap? HeaderImage { get { return _HeaderImage; } set { _HeaderImage = value; OnPropertyChanged(nameof(HeaderImage)); } }
        public Bitmap? AvatarImage { get { return _AvatarImage; } set { _AvatarImage = value; OnPropertyChanged(nameof(AvatarImage)); } }
        public Bitmap? FlagImage { get { return _FlagImage; } set { _FlagImage = value; OnPropertyChanged(nameof(FlagImage)); } }
        public bool ShowTimer { get { return _showTimer; } set { _showTimer = value; OnPropertyChanged(nameof(ShowTimer)); } }
        public double TimerProgress { get { return _TimerProgress; } set { _TimerProgress = value; OnPropertyChanged(nameof(TimerProgress)); } }
        public string TimerText { get { return _TimerText; } set { _TimerText = value; OnPropertyChanged(nameof(TimerText)); } }
        public bool ShowHeader { get { return _ShowHeader; } set { _ShowHeader = value; OnPropertyChanged(nameof(ShowHeader)); } }
        public bool ShowClock { get { return _ShowClock; } set { _ShowClock = value; OnPropertyChanged(nameof(ShowClock)); } }
        public string ClockValue { get { return _ClockValue; } set { _ClockValue = value; OnPropertyChanged(nameof(ClockValue)); } }

        public void OnReset()
        {
            Avalonia.Threading.Dispatcher.UIThread.InvokeAsync(async () =>
            {
                await UIManager.Instance?.SetLoadState(true);
                AlertResult res = await UIManager.Instance?.DisplayAlertAsync("New session", "Are you sure you want to start a new session?", new string[] { "Yes", "No" });
                if (res.PressedButton == "Yes")
                {
                    await AppManager.Instance?.BreakLoopAsync();
                    AppManager.Instance?.StartLoop(true);
                }
                await UIManager.Instance?.SetLoadState(false);
            });
        }

        public void OnSave()
        {
            Avalonia.Threading.Dispatcher.UIThread.InvokeAsync(async () =>
            {
                bool replace = false;
                string? name = SessionManager.Instance?.CurrentSession?.Name;
                Session? s = SessionManager.Instance?.GetSessionByName(name);
                if (s != null)
                {
                    //ask to replace
                    replace = (await UIManager.Instance.DisplayAlertAsync("Session already exists", "Do you want to overwrite the existing stored session?", new string[] { "Yes", "No" })).PressedButton == "Yes";
                }
                if (!replace)
                    name = (await UIManager.Instance.DisplayInputAlertAsync("Save Session", "Please enter a name for this session")).Input ?? string.Empty;

                SessionManager.Instance?.CurrentSession?.SetName(name);
                bool result = await SessionManager.Instance?.SaveSessionToFile(SessionManager.Instance.CurrentSession);
                if (!result)
                    await UIManager.Instance.DisplayAlertAsync("Oops", "Something went wrong while saving, please retry later.", new string[] { "Ok" });
            });
        }
    }
}
