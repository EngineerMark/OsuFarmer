using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using osu_farmer.Core;
using osu_farmer.Core.Interfaces;
using Newtonsoft.Json;

namespace osu_farmer.Managers
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

        public async void LoadSettings()
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
                    if (IsFileLocked(SettingsPath))
                        return false;

                string data = string.Empty;
                try{
                    data = JsonConvert.SerializeObject(this.settings);
                }catch (Exception){ }

                if (string.IsNullOrEmpty(data))
                    return false;


                //empty file
                await File.WriteAllTextAsync(SettingsPath, String.Empty);

                //fill file with data
                await File.WriteAllTextAsync(SettingsPath, data);

                return true;
            }
            return false;
        }

        protected bool IsFileLocked(string file){
            try{
                using (FileStream stream = File.Open(file, FileMode.Open, FileAccess.Read, FileShare.None))
                {
                    stream.Close();
                }
            }
            catch (IOException)
            {
                return true;
            }

            return false;
        }
    }
}
