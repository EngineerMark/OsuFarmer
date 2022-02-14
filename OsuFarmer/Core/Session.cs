using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OsuFarmer.Core.Osu;
using Newtonsoft.Json;

namespace OsuFarmer.Core
{
    public class Session
    {
        public string? Name { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime LastUpdatedAt { get; set; }

        public User? Start { get; set; }
        public User? Latest { get; set; }

        public Dictionary<DateTime, User>? HistoricData { get; set; }

        public Mode Mode { get; set; }

        public void SetName(string? name) => Name = name;

        public Session() {
            HistoricData = new Dictionary<DateTime, User>();
        }
        public Session(Session s)
        {
            this.Name = s.Name;
            this.CreatedAt = s.CreatedAt;
            this.LastUpdatedAt = s.LastUpdatedAt;
            this.Mode = s.Mode;
            this.Start = s.Start;
            this.Latest = s.Latest;
            this.HistoricData = HistoricData != null ? new Dictionary<DateTime, User>(HistoricData) : new Dictionary<DateTime, User>();
        }
    }
}
