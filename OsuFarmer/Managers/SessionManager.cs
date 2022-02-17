using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using OsuFarmer.Core;
using OsuFarmer.Core.Interfaces;
using OsuFarmer.Core.Osu;

namespace OsuFarmer.Managers
{
    public class SessionManager : Manager<SessionManager>, IUsesStorage
    {
        public string FileLocation { get { return Path.Combine(SettingsManager.Instance.FileLocation, "Sessions"); } }
        public string SessionsDirectory { get => Path.Combine(FileManager.GetExecutableDirectory()??string.Empty, FileLocation); }

        public Session? CurrentSession { get; set; }

        public List<Session>? StoredSessions;

        public SessionManager(){
            Register(this);
        }

        public Session? GetSessionByName(string? name)
        {
            if (name == null)
                return null;

            foreach (var c in Path.GetInvalidFileNameChars())
            {
                name = name?.Replace(c, '-');
            }
            return StoredSessions?.Find(s => s.Name.Equals(name, StringComparison.InvariantCultureIgnoreCase));
        }

        public async void StartNewSession(User? user){
            CurrentSession = new Session();
            CurrentSession.Start = user;
            CurrentSession.Latest = user;

            CurrentSession.CreatedAt = DateTime.Now;
            CurrentSession.LastUpdatedAt = DateTime.Now;

            CurrentSession.Mode = SettingsManager.Instance?.Settings?.ApiGamemode ?? Mode.Standard;

            //PageManager.Instance.GetPage<TrackerPage>().ApplySession(CurrentSession);
            await UIManager.Instance.TrackersApplySession(CurrentSession);
        }

        public async Task LoadSession(Session? s)
        {
            if (s == null)
                return;

            string? user = s.Start.Username;
            if(!SettingsManager.Instance.settings.ApiUsername.Equals(user, StringComparison.InvariantCultureIgnoreCase))
                SettingsManager.Instance.settings.ApiUsername = user;

            Mode mode = s.Mode;
            if (SettingsManager.Instance.settings.ApiGamemode != mode)
                SettingsManager.Instance.settings.ApiGamemode = mode;

            await SettingsManager.Instance?.SaveSettings();
            UIManager.Instance.PrefillSettings(SettingsManager.Instance.settings);

            CurrentSession = new Session(s);
            await UIManager.Instance.TrackersApplySession(CurrentSession);
        }


        public async Task IterateSession(User? user){
            if(CurrentSession==null){
                throw new NullReferenceException("No session currently exists, yet trying to update it");
            }

            CurrentSession.Latest = user;
            CurrentSession.LastUpdatedAt = DateTime.Now;

            await UIManager.Instance.TrackersApplySession(CurrentSession);
        }

        public async Task<bool> SaveSessionToFile(Session? s)
        {
            if (s != null)
            {
                Directory.CreateDirectory(SessionsDirectory);

                string? name = s.Name;

                foreach (var c in Path.GetInvalidFileNameChars())
                {
                    name = name?.Replace(c, '-');
                }

                string path = Path.Combine(SessionsDirectory, name + ".session");

                string data = string.Empty;
                try
                {
                    data = JsonConvert.SerializeObject(s);
                }
                catch (Exception) { }

                bool state = await FileManager.WriteFile(path, data);
                if (state)
                    ReloadFiles();
                return state;
            }
            return false;
        }

        public bool DeleteSessionFile(string name)
        {
            if (Directory.Exists(SessionsDirectory))
            {
                string path = Path.Combine(SessionsDirectory, name + ".session");
                if (File.Exists(path))
                {
                    if (FileManager.IsFileLocked(path))
                        return false;

                    File.Delete(path);
                    return true;
                }
            }
            return false;
        }
        /// <summary>
        /// Empties out the current session list and reloads from the files, generally for sorting it correctly
        /// </summary>
        public void ReloadFiles(){
            if (StoredSessions == null)
                StoredSessions = new List<Session>();
            StoredSessions.Clear();

            if (Directory.Exists(SessionsDirectory))
            {
                string[] allFiles = Directory.GetFiles(SessionsDirectory);

                if (allFiles!=null && allFiles.Length > 0)
                {
                    List<Session> temp_sessions = new List<Session>();

                    foreach(string file in allFiles)
                    {
                        string fileData = File.ReadAllText(file);

                        Session? data = null;
                        try
                        {
                            data = JsonConvert.DeserializeObject<Session>(fileData);
                        }
                        catch(Exception) { }
                        if (data != null)
                            temp_sessions.Add(data);
                    }

                    if (temp_sessions.Count > 0)
                    {
                        temp_sessions = temp_sessions.OrderBy(x => x.LastUpdatedAt).Reverse().ToList();
                    }

                    StoredSessions = temp_sessions;
                }
            }
            UIManager.Instance.PopulateSessions(StoredSessions);
        }

        public void RemoveSession(string name){
            
            bool deleted = DeleteSessionFile(name);
            if (deleted)
                ReloadFiles();
        }
    }
}
