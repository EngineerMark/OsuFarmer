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
        private bool _ShowHeader = true;
        private string _Username = "peppy";
        private string _CountryName = "Australia";
        private Bitmap? _HeaderImage = DefaultHeaderImage;
        private Bitmap? _AvatarImage = DefaultAvatarImage;
        private Bitmap? _FlagImage = DefaultFlagImage;

        public bool ExpansiveModeEnabled { get { return _ExpansiveModeEnabled; } set { _ExpansiveModeEnabled = value; OnPropertyChanged(nameof(ExpansiveModeEnabled)); } }
        public double ThinTrackerWidth { get { return _ThinTrackerWidth; } set { _ThinTrackerWidth = value; OnPropertyChanged(nameof(ThinTrackerWidth)); } }
        public bool ShowHeader { get { return _ShowHeader; } set { _ShowHeader = value; OnPropertyChanged(nameof(ShowHeader)); } }
        public string Username { get { return _Username; } set { _Username = value; OnPropertyChanged(nameof(Username)); } }
        public string CountryName { get { return _CountryName; } set { _CountryName = value; OnPropertyChanged(nameof(CountryName)); } }
        public Bitmap? HeaderImage { get { return _HeaderImage; } set { _HeaderImage = value; OnPropertyChanged(nameof(HeaderImage)); } }
        public Bitmap? AvatarImage { get { return _AvatarImage; } set { _AvatarImage = value; OnPropertyChanged(nameof(AvatarImage)); } }
        public Bitmap? FlagImage { get { return _FlagImage; } set { _FlagImage = value; OnPropertyChanged(nameof(FlagImage)); } }

        public void OnReset()
        {
            Avalonia.Threading.Dispatcher.UIThread.InvokeAsync(async () =>
            {
                UIManager.Instance?.SetLoadState(true);
                AlertResult res = await UIManager.Instance?.DisplayAlertAsync("New session", "Are you sure you want to start a new session?", new string[] { "Yes", "No" });
                if (res.PressedButton == "Yes")
                {
                    await AppManager.Instance?.BreakLoopAsync();
                    AppManager.Instance?.StartLoop(true);
                }
                UIManager.Instance?.SetLoadState(false);
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
