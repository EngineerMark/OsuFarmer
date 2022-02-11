using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using osu_farmer.Core;

namespace osu_farmer.Managers
{
    public class SessionManager : Manager<SessionManager>
    {
        public Session CurrentSession { get; set; }

        public List<Session> StoredSessions;

        public SessionManager(){
            Register(this);

            ReloadFiles();
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
