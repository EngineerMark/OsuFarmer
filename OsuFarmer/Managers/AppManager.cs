using Avalonia.Controls;
using OsuFarmer.Alerts;
using OsuFarmer.Core;
using OsuFarmer.Core.Osu;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OsuFarmer.Managers
{
    public class AppManager : Manager<AppManager>
    {
        private AppManagerData data;

        public bool IsLoopRunning { get; set; } = false;
        public bool CancelLoop { get; private set; } = false;

        public SessionManager? SessionManager { get; set; } = null;
        //public PageManager? PageManager { get; set; } = null;
        public SettingsManager? SettingsManager { get; set; } = null;
        public FileManager? FileManager { get; set; } = null;
        public NetworkManager? NetworkManager { get; set; } = null;

        public AppManager(AppManagerData data)
        {
            Register(this);

            this.data = data;

            Console.WriteLine("Created AppManager");
        }

        public void Start(){
            FileManager = new FileManager();
            SettingsManager = new SettingsManager();
            SessionManager = new SessionManager();
            //PageManager = new PageManager();
            NetworkManager = new NetworkManager();

            StartLoop();
        }

        public void StartLoop(bool reset = true){
            BreakLoop();
            Task.Run(async ()=> {
                //PageManager.Instance?.GetPage<TrackerPage>().SetLoadedState(false);
                if (CancelLoop)
                    while (IsLoopRunning) await Task.Delay(25);
                await ApplicationLoop(reset);
            });
        }

        public void BreakLoop(){
            Task.Run(() =>
            {
                if(IsLoopRunning) CancelLoop = true;
            });
        }

        public async Task BreakLoopAsync()
        {
            //PageManager.Instance?.GetPage<TrackerPage>().SetLoadedState(false);
            if (IsLoopRunning)
                CancelLoop = true;
            while(IsLoopRunning)
                await Task.Delay(25);
        }

        private async Task ApplicationLoop(bool reset)
        {
            IsLoopRunning = true;
            CancelLoop = false;

            UIManager.Instance.SetLoadState(true);

            await SettingsManager.Instance?.LoadSettings();
            await UIManager.Instance.GenerateTrackerFields(SettingsManager.Instance.Settings);

            SessionManager.Instance.ReloadFiles();

            if (SettingsManager.Instance.Settings == null){
                await UIManager.Instance.DisplayAlertAsync("Error", "Something went wrong. Please retry!");
                await ApplicationLoop(reset);
                return;
            }

            //if (!NetworkManager.CheckForInternetConnection())
            //{
            //    await PageManager.Instance?.GetPage<TrackerPage>()?.DisplayAlert("No internet", "There is no internet connection available", "Retry");
            //    await ApplicationLoop(reset);
            //    return;
            //}

            await Task.Delay(2000);

            ////test api
            if (!(await OsuHelper.IsApiValid()))
            {
                //string? val = await PageManager.Instance?.GetPage<TrackerPage>()?.DisplayPromptAsync("API Key", "No or invalid osu! API key is in use, please enter it", "Continue", "Cancel");
                AlertResult? val = await UIManager.Instance.DisplayInputAlertAsync("API Key", "No or invalid osu! API key is in use, please enter it", true);
                if(val.Value.Input!=null)
                    SettingsManager.Instance.settings.ApiKey = val.Value.Input;
                if (val.Value.Input==null || !(await OsuHelper.IsApiValid()))
                {
                    await ApplicationLoop(reset);
                    return;
                }
            }

            await Task.Delay(200);

            if (string.IsNullOrEmpty(SettingsManager.Instance?.Settings?.ApiUsername))
            {
                AlertResult? val = await UIManager.Instance.DisplayInputAlertAsync("Username", "No or invalid osu! username is in use, please enter it", true);
                if (val.Value.Input != null)
                    SettingsManager.Instance.settings.ApiUsername = val.Value.Input;
                if (!(await OsuHelper.IsUserValid(SettingsManager.Instance.settings.ApiUsername)))
                {
                    await ApplicationLoop(reset);
                    return;
                }
            }

            await Task.Delay(50);

            await SettingsManager.Instance.SaveSettings();
            //PageManager.Instance?.GetPage<SettingsPage>()?.PrefillSettings(SettingsManager.Instance.Settings);
            UIManager.Instance?.PrefillSettings(SettingsManager.Instance.Settings);
            await UIManager.Instance?.ApplySettings(SettingsManager.Instance.Settings);

            await Task.Delay(50);

            ////test
            User? user = await OsuHelper.GetUser(SettingsManager.Instance.settings.ApiUsername, (int)SettingsManager.Instance.settings.ApiGamemode);

            await UIManager.Instance.TrackersApplyUser(user);

            if (reset)
                SessionManager.Instance?.StartNewSession(user);
            else if (SessionManager.Instance?.CurrentSession != null)
                SessionManager.Instance?.IterateSession(user);

            //PageManager.Instance?.GetPage<TrackerPage>().SetLoadedState(true);
            UIManager.Instance.SetLoadState(false);

            while (true)
            {
                if (CancelLoop)
                    break;

                if (!NetworkManager.CheckForInternetConnection())
                {
                    await Task.Delay(1500);
                    continue;
                }

                await Task.Delay(4500);
                user = await OsuHelper.GetUser(SettingsManager.Instance.settings.ApiUsername, (int)SettingsManager.Instance.settings.ApiGamemode);
                SessionManager.Instance?.IterateSession(user);
            }

            IsLoopRunning = false;
        }
    }


    public struct AppManagerData {
        public Window Window;
        public UIManager UIManager;
    }
}
