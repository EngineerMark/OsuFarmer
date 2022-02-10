using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using System.Text;
using System.Threading.Tasks;
using osu_farmer.Core.Osu;
using osu_farmer.Managers;

namespace osu_farmer.Core
{
    public class Settings
    {
        public string? ApiKey { get; set; } = "";
        public string? ApiUsername { get; set; } = "";
        public Mode ApiGamemode { get; set; } = Mode.Standard;

        public static List<TrackerItem> PrefabTrackers { get; set; } = new List<TrackerItem>(){
            new TrackerItem("Total SS", "TotalSS", false),
            new TrackerItem("Silver SS", "CountSSH", true),
            new TrackerItem("Gold SS", "CountSS", true),

            new TrackerItem("Total S", "TotalS", false),
            new TrackerItem("Silver S", "CountSH", true),
            new TrackerItem("Gold S", "CountS", true),

            new TrackerItem("Total A", "TotalA", false),

            new TrackerItem("Ranked Score", "RankedScore", true),
            new TrackerItem("Total Score", "TotalScore", true),

            new TrackerItem("Global Rank", "WorldRank", true),
            new TrackerItem("Country Rank", "CountryRank", true),

            new TrackerItem("PP", "PP", true),
            new TrackerItem("Accuracy", "Accuracy", true),

            new TrackerItem("Clears", "Clears", true),

            new TrackerItem("300 Hits", "Count300", false),
            new TrackerItem("100 Hits", "Count100", false),
            new TrackerItem("50 Hits", "Count50", false),

            new TrackerItem("Playcount", "Playcount", true),

        };

        public Dictionary<string, bool> Trackers { get; set; }

        [JsonIgnore]
        public List<TrackerItem> RunningTrackers { get; set; }

        public Settings(){
            UpdateTrackers();
        }

        public void UpdateTrackers(){
            if (Trackers == null)
            {
                Trackers = new Dictionary<string, bool>();
                foreach (TrackerItem item in PrefabTrackers)
                    Trackers.Add(item.Property, item.Enabled);
            }



            if (Trackers.Count > 0)
            {
                RunningTrackers = new List<TrackerItem>();
                foreach (KeyValuePair<string, bool> item in Trackers)
                {
                    if (item.Value)
                    {
                        TrackerItem? prefab = PrefabTrackers.Find(i => i.Property.Equals(item.Key, StringComparison.OrdinalIgnoreCase));
                        if (prefab != null)
                        {
                            TrackerItem newItem = new TrackerItem(prefab);
                            if (newItem != null)
                                RunningTrackers.Add(newItem);
                            else
                            {
                                string s = "";
                            }
                        }
                    }
                }
            }
        }
    }
}
