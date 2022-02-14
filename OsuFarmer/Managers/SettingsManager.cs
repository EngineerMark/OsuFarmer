using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OsuFarmer.Core;
using OsuFarmer.Core.Interfaces;
using Newtonsoft.Json;
using System.IO;

namespace OsuFarmer.Managers
{
    public class SettingsManager : Manager<SettingsManager>, IUsesStorage
    {
        public string FileLocation { get => @"Data\";}

        public string SettingsDirectory { get => Path.Combine(FileManager.GetExecutableDirectory()??string.Empty, FileLocation); }
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

        public async Task ApplySettings(Settings? settings){
            this.settings = settings;
            this.settings.UpdateTrackers();
            await UIManager.Instance?.ApplySettings(settings);
            //TrackerPage trackerPage = PageManager.Instance?.GetPage<TrackerPage>();

            //trackerPage.UpdateTrackerFields(settings);
            //trackerPage.HeaderImageVisible = settings.ShowHeaderImage;

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
