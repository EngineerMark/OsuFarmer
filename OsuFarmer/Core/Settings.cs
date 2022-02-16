using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using System.Text;
using System.Threading.Tasks;
using OsuFarmer.Core.Osu;
using OsuFarmer.Managers;

namespace OsuFarmer.Core
{
    public class Settings
    {
        public string? ApiKey { get; set; } = "";
        public string? ApiUsername { get; set; } = "";
        public Mode ApiGamemode { get; set; } = Mode.Standard;

        public bool ShowHeaderImage { get; set; } = true;
        public bool UseExpandedTracker { get; set; } = true;

        public static List<TrackerItem> PrefabTrackers { get; set; } = new List<TrackerItem>(){
            new TrackerItem("Total SS", "TotalSS", false),
            new TrackerItem("Silver SS", "CountSSH", true),
            new TrackerItem("Gold SS", "CountSS", true),

            new TrackerItem("Total S", "TotalS", false),
            new TrackerItem("Silver S", "CountSH", true),
            new TrackerItem("Gold S", "CountS", true),

            new TrackerItem("Total A", "CountA", false),

            new TrackerItem("Level", "Level", true, false, 2),
            new TrackerItem("Ranked Score", "RankedScore", true),
            new TrackerItem("Total Score", "TotalScore", true),

            new TrackerItem("Global Rank", "WorldRank", true, true),
            new TrackerItem("Country Rank", "CountryRank", true, true),

            new TrackerItem("PP", "PP", true, false, 2),
            new TrackerItem("Accuracy", "Accuracy", false, false, 2),

            new TrackerItem("Clears", "Clears", true),

            new TrackerItem("Total Hits", "TotalHits", false),
            new TrackerItem("Hits per Play", "HitsPerPlay", false, false, 2),
            new TrackerItem("300 Hits", "Count300", false),
            new TrackerItem("100 Hits", "Count100", false),
            new TrackerItem("50 Hits", "Count50", false),

            //new TrackerItem("Playtime", "Playtime", true, "{0:%h}h {0:%m}m", typeof(TimeSpan)),
            new TrackerItem("Playcount", "Playcount", true),
        };

        public Dictionary<string, bool>? Trackers { get; set; }

        [JsonIgnore]
        public List<TrackerItem>? RunningTrackers { get; set; }

        public void UpdateTrackers(){
            if (Trackers == null)
            {
                Trackers = new Dictionary<string, bool>();
                foreach (TrackerItem item in PrefabTrackers)
                    if(item.Property!=null)
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
                        }
                    }
                }
            }
        }
    }
}
