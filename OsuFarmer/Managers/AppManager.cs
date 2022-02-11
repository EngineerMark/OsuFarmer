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

        public SessionManager SessionManager { get; set; }
        public PageManager PageManager { get; set; }
        public SettingsManager SettingsManager { get; set; }
        public FileManager FileManager { get; set; }

        private CancellationTokenSource _cts;

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
            PageManager = new PageManager();

            StartLoop();
        }

        public void StartLoop(bool reset = true){
            BreakLoop();
            _cts = new CancellationTokenSource();
            Device.InvokeOnMainThreadAsync(async ()=> {
                PageManager.Instance?.GetPage<TrackerPage>().SetLoadedState(false);
                if (CancelLoop)
                    while (IsLoopRunning) await Task.Delay(25);
                await ApplicationLoop(_cts.Token, reset);
            });
        }

        public void BreakLoop(){
            Device.BeginInvokeOnMainThread(async () =>
            {
                if(IsLoopRunning) CancelLoop = true;
            });
        }

        public async Task BreakLoopAsync()
        {
            PageManager.Instance?.GetPage<TrackerPage>().SetLoadedState(false);
            if (IsLoopRunning)
                CancelLoop = true;
            while(IsLoopRunning)
                await Task.Delay(25);
        }

        private async Task ApplicationLoop(CancellationToken ct, bool reset)
        {
            IsLoopRunning = true;
            CancelLoop = false;

            PageManager.Instance?.GetPage<TrackerPage>().SetLoadedState(false);

            await SettingsManager.Instance?.LoadSettings();
            PageManager.Instance?.GetPage<TrackerPage>().GenerateTrackerFields(SettingsManager.Instance.Settings);

            SessionManager.Instance.ReloadFiles();

            if (SettingsManager.Instance.Settings == null){
                Page? p = PageManager.Instance?.GetPage<TrackerPage>();
                await p?.DisplayAlert("Error", "Something went wrong. Please retry!", "Retry");
                await ApplicationLoop(ct, reset);
                return;
            }

            await Task.Delay(2000);

            //test api
            if(!(await OsuHelper.IsApiValid()))
            {
                string? val = await PageManager.Instance?.GetPage<TrackerPage>()?.DisplayPromptAsync("API Key", "No or invalid osu! API key is in use, please enter it", "Continue", "Cancel");
                SettingsManager.Instance.settings.ApiKey = val;
                if (!(await OsuHelper.IsApiValid()))
                {
                    await ApplicationLoop(ct, reset);
                    return;
                }
            }

            await Task.Delay(200);

            if (string.IsNullOrEmpty(SettingsManager.Instance?.Settings?.ApiUsername))
            {
                string? val = await PageManager.Instance?.GetPage<TrackerPage>()?.DisplayPromptAsync("Username", "No or invalid osu! username is in use, please enter it", "Continue", "Cancel");
                SettingsManager.Instance.settings.ApiUsername = val;
                if (!(await OsuHelper.IsUserValid(SettingsManager.Instance.settings.ApiUsername)))
                {
                    await ApplicationLoop(ct, reset);
                    return;
                }
            }

            await Task.Delay(50);

            await SettingsManager.Instance.SaveSettings();
            PageManager.Instance?.GetPage<SettingsPage>()?.PrefillSettings(SettingsManager.Instance.Settings);

            await Task.Delay(50);

            //test
            User? user = await OsuHelper.GetUser(SettingsManager.Instance.settings.ApiUsername, (int)SettingsManager.Instance.settings.ApiGamemode);

            PageManager.Instance?.GetPage<TrackerPage>().ApplyUser(user);

            if(reset)
                SessionManager.Instance?.StartNewSession(user);
            else
                if (SessionManager.Instance?.CurrentSession != null)
                    SessionManager.Instance?.IterateSession(user);

            PageManager.Instance?.GetPage<TrackerPage>().SetLoadedState(true);

            while (true)
            {
                if (CancelLoop)
                    break;

                await Task.Delay(4500);
                user = await OsuHelper.GetUser(SettingsManager.Instance.settings.ApiUsername, (int)SettingsManager.Instance.settings.ApiGamemode);
                SessionManager.Instance?.IterateSession(user);
            }

            IsLoopRunning = false;
        }

        public AppShell? GetShell(){
            return data.Shell;
        }
    }


    public struct AppManagerData {
        public AppShell Shell;
    }
}
