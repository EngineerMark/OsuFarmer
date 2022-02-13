using OsuFarmer.Managers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MessageBox.Avalonia.Enums;
using OsuFarmer.Core;

namespace OsuFarmer.ViewModels
{
    public class SessionItemControlViewModel : BaseViewModel
    {
        private string _Username = "peppy";
        private string _Filename = "epic ppy session";
        private string _Fileage = "2 days";
        public string Username { get { return _Username; } set { _Username = value; OnPropertyChanged(nameof(Username)); } }
        public string Filename { get { return _Filename; } set { _Filename = value; OnPropertyChanged(nameof(Filename)); } }
        public string Fileage { get { return _Fileage; } set { _Fileage = value; OnPropertyChanged(nameof(Fileage)); } }

        public void OnLoad()
        {
            Avalonia.Threading.Dispatcher.UIThread.InvokeAsync(async () =>
            {
                await LoadSessionSequence();
            });
        }

        public void OnDelete()
        {
            Avalonia.Threading.Dispatcher.UIThread.InvokeAsync(async () =>
            {
                await RemoveSessionSequence();
            });
        }

        private async Task LoadSessionSequence()
        {
            await Avalonia.Threading.Dispatcher.UIThread.InvokeAsync(async () =>
            {
                Session? s = SessionManager.Instance?.GetSessionByName(Filename);
                if (s == null)
                {
                    await UIManager.Instance.DisplayAlertAsync("Error", "Something went wrong preparing this session file, please retry later", ButtonEnum.Ok);
                    return;
                }

                bool response = (await UIManager.Instance.DisplayAlertAsync("Load", "Loading a session will also adjust your settings like username and gamemode to it. Are you sure?", ButtonEnum.YesNo))==ButtonResult.Yes;

                if (response)
                {
                    UIManager.Instance.GoToPage("TrackerPage");
                    await AppManager.Instance?.BreakLoopAsync();
                    await SessionManager.Instance?.LoadSession(s);
                    AppManager.Instance?.StartLoop(false);
                }
            });
        }

        private async Task RemoveSessionSequence()
        {
            await Avalonia.Threading.Dispatcher.UIThread.InvokeAsync(async () =>
            {
                bool response = (await UIManager.Instance.DisplayAlertAsync("Warning", "This will delete the session file permanently. Are you sure?", ButtonEnum.YesNo))==ButtonResult.Yes;

                if (response)
                    SessionManager.Instance?.RemoveSession(Filename);
            });
            //Page p = OsuFarmer.Helpers.ViewExtensions.GetParentPage(this);

        }
    }
}
