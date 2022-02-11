using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OsuFarmer.Core;
using OsuFarmer.Core.Osu;

namespace OsuFarmer.Managers
{
    public class SessionManager : Manager<SessionManager>
    {
        public Session CurrentSession { get; set; }

        public List<Session> StoredSessions;

        public SessionManager(){
            Register(this);

            ReloadFiles();
        }

        public void StartNewSession(User? user){
            CurrentSession = new Session();
            CurrentSession.Start = user;
            CurrentSession.Latest = user;

            CurrentSession.CreatedAt = DateTime.Now;
            CurrentSession.LastUpdatedAt = DateTime.Now;

            PageManager.Instance.GetPage<TrackerPage>().ApplySession(CurrentSession);
        }

        public void IterateSession(User? user){
            if(CurrentSession==null){
                throw new NullReferenceException("No session currently exists, yet trying to update it");
            }

            CurrentSession.Latest = user;
            CurrentSession.LastUpdatedAt = DateTime.Now;

            PageManager.Instance.GetPage<TrackerPage>().ApplySession(CurrentSession);
        }

        /// <summary>
        /// Empties out the current session list and reloads from the files
        /// </summary>
        public void ReloadFiles(){
            if (StoredSessions == null)
                StoredSessions = new List<Session>();
            StoredSessions.Clear();
            
        }

        public void RemoveSession(string identifier){
            Page? p = PageManager.Instance?.GetPage<SettingsPage>();
            string s = "";
        }
    }
}
