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

            Device.BeginInvokeOnMainThread(ApplicationLoop);
        }

        private async void ApplicationLoop(){
            bool runLoop = true;

            SettingsManager.Instance?.LoadSettings();

            if (SettingsManager.Instance.Settings == null){
                await PageManager.Instance?.GetPage<TrackerPage>()?.DisplayAlert("Error", "Something went wrong. Please retry!", "Retry");
                ApplicationLoop();
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
                    ApplicationLoop();
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
                    ApplicationLoop();
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

            while (runLoop)
            {
                await Task.Delay(1000);
            }

            await Task.Delay(500);
            ApplicationLoop();
        }

        public AppShell? GetShell(){
            return data.Shell;
        }
    }


    public struct AppManagerData {
        public AppShell Shell;
    }
}
