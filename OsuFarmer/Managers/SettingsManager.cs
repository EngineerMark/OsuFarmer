using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OsuFarmer.Core;
using OsuFarmer.Core.Interfaces;
using Newtonsoft.Json;

namespace OsuFarmer.Managers
{
    public class SettingsManager : Manager<SettingsManager>, IUsesStorage
    {
#if DEBUG
        public string FileLocation { get => @"Data\";}
#else
        public string FileLocation { get => @"..\Data\";}

#endif

        public string SettingsDirectory { get => Path.Combine(FileManager.GetExecutableDirectory(), FileLocation); }
        public string SettingsPath { get => Path.Combine(SettingsDirectory, "Settings.json"); }

        public Settings? settings = null;
        public Settings? Settings { get => settings; }

        public SettingsManager(){
            Register(this);
        }

        public async Task LoadSettings()
        {
            Settings? settings = null;
            if (File.Exists(SettingsPath))
            {
                string fileData = await File.ReadAllTextAsync(SettingsPath);
                if(!string.IsNullOrEmpty(fileData)){
                    try
                    {
                        settings = JsonConvert.DeserializeObject<Settings>(fileData);
                    }
                    catch (Exception){ }
                    if(settings!=null){
                        this.settings = settings;
                    }
                }
            }
            if (settings == null){
                settings = GenerateSettings();
                this.settings = settings;
                await SaveSettings();
            }

            settings.UpdateTrackers();
        }

        public async void ApplySettings(Settings settings){
            this.settings = settings;
            this.settings.UpdateTrackers();
            PageManager.Instance?.GetPage<TrackerPage>()?.UpdateTrackerFields(settings);
            await SaveSettings();
            //restart app loop
        }

        private Settings GenerateSettings(){
            Settings settings = new Settings();
            return settings;
        }

        public async Task<bool> SaveSettings()
        {
            if (this.settings != null)
            {
                Directory.CreateDirectory(SettingsDirectory);

                if(File.Exists(SettingsPath))
                    if (FileManager.IsFileLocked(SettingsPath))
                        return false;

                string data = string.Empty;
                try{
                    data = JsonConvert.SerializeObject(this.settings);
                }catch (Exception){ }

                return await FileManager.WriteFile(SettingsPath, data);
            }
            return false;
        }
    }
}
