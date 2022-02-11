using osu_farmer.Core;
using osu_farmer.Core.Osu;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace osu_farmer.Managers
{
    public class AppManager : Manager<AppManager>
    {
        private AppManagerData data;

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

        public void StartLoop(){
            BreakLoop();
            _cts = new CancellationTokenSource();
            Device.BeginInvokeOnMainThread(()=>ApplicationLoop(_cts.Token).ContinueWith((args) => { }));
        }

        public void BreakLoop(){
            Device.BeginInvokeOnMainThread(() =>
            {
                if (_cts!=null){
                    _cts.Cancel();
                }
            });
        }

        private async Task ApplicationLoop(CancellationToken ct)
        {
            PageManager.Instance?.GetPage<TrackerPage>().SetLoadedState(false);
            bool runLoop = true;

            SettingsManager.Instance?.LoadSettings();
            PageManager.Instance?.GetPage<TrackerPage>().GenerateTrackerFields(SettingsManager.Instance.Settings);

            if (SettingsManager.Instance.Settings == null){
                await PageManager.Instance?.GetPage<TrackerPage>()?.DisplayAlert("Error", "Something went wrong. Please retry!", "Retry");
                await ApplicationLoop(ct);
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
                    await ApplicationLoop(ct);
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
                    await ApplicationLoop(ct);
                    return;
                }
            }

            await Task.Delay(50);

            await SettingsManager.Instance.SaveSettings();
            PageManager.Instance?.GetPage<SettingsPage>()?.PrefillSettings(SettingsManager.Instance.Settings);

            await Task.Delay(50);

            //test
            User? user = await OsuHelper.GetUser(SettingsManager.Instance.settings.ApiUsername, (int)SettingsManager.Instance.settings.ApiGamemode);

            if(user!=null){
                foreach(TrackerItem tracker in SettingsManager.Instance.Settings.RunningTrackers){
                    double data = Convert.ToInt64(user[tracker.Property]);
                    PageManager.Instance?.GetPage<TrackerPage>()?.SetCurrentValue(tracker.Property, data);
                }
            }

            PageManager.Instance?.GetPage<TrackerPage>().ApplyUser(user);
            PageManager.Instance?.GetPage<TrackerPage>().SetLoadedState(true);

            while (runLoop)
            {
                await Task.Delay(500, ct);
                if (ct.IsCancellationRequested)
                    break;

                await Task.Delay(1000);
            }

            await Task.Delay(500);
            await ApplicationLoop(ct);
        }

        public AppShell? GetShell(){
            return data.Shell;
        }
    }


    public struct AppManagerData {
        public AppShell Shell;
    }
}
