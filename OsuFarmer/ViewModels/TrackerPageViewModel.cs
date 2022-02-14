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
        private bool _ShowHeader = true;
        private string _Username = "peppy";
        private string _CountryName = "Australia";

        public bool ShowHeader { get { return _ShowHeader; } set { _ShowHeader = value; OnPropertyChanged(nameof(ShowHeader)); } }
        public string Username { get { return _Username; } set { _Username = value; OnPropertyChanged(nameof(Username)); } }
        public string CountryName { get { return _CountryName; } set { _CountryName = value; OnPropertyChanged(nameof(CountryName)); } }

        public void OnReset()
        {
            Avalonia.Threading.Dispatcher.UIThread.InvokeAsync(async () =>
            {
                UIManager.Instance?.SetLoadState(true);
                AlertResult res = await UIManager.Instance.DisplayAlertAsync("New session", "Are you sure you want to start a new session?", new string[] { "Yes", "No" });
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
                string name = SessionManager.Instance?.CurrentSession?.Name;
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
