using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace osu_farmer.Core.Osu
{
    public class User
    {
        [JsonProperty("user_id")]
        public long ID { get; set; }

        [JsonProperty("username")]
        public string? Username { get; set; }

        [JsonProperty("join_date")]
        public string? JoinDate { get; set; }

        [JsonProperty("count300")]
        public long Count300 { get; set; }

        [JsonProperty("count100")]
        public long Count100 { get; set; }

        [JsonProperty("count50")]
        public long Count50 { get; set; }

        [JsonProperty("playcount")]
        public long Playcount { get; set; }

        [JsonProperty("ranked_score")]
        public long RankedScore { get; set; }

        [JsonProperty("total_score")]
        public long TotalScore { get; set; }

        [JsonProperty("pp_rank")]
        public long WorldRank { get; set; }

        [JsonProperty("level")]
        public float Level { get; set; }

        [JsonProperty("pp_raw")]
        public float PP { get; set; }

        [JsonProperty("accuracy")]
        public float Accuracy { get; set; }

        [JsonProperty("count_rank_ss")]
        public long CountSS { get; set; }

        [JsonProperty("count_rank_ssh")]
        public long CountSSH { get; set; }

        [JsonProperty("count_rank_s")]
        public long CountS { get; set; }

        [JsonProperty("count_rank_sh")]
        public long CountSH { get; set; }

        [JsonProperty("count_rank_a")]
        public long CountA { get; set; }

        [JsonProperty("country")]
        public string? Country { get; set; }

        [JsonProperty("total_seconds_played")]
        public long Playtime { get; set; }

        [JsonProperty("pp_country_rank")]
        public long CountryRank { get; set; }

        public long TotalHits { get => Count300 + Count100 + Count50; }
        public long Clears { get => CountSSH + CountSS + CountSH + CountS + CountA; }
        public long TotalSS { get => CountSSH + CountSS; }
        public long TotalS { get => CountSH + CountS; }

        public object this[string propertyName]
        {
            get
            {
                // probably faster without reflection:
                // like:  return Properties.Settings.Default.PropertyValues[propertyName] 
                // instead of the following
                Type myType = typeof(User);
                PropertyInfo myPropInfo = myType.GetProperty(propertyName);
                return myPropInfo.GetValue(this, null);
            }
            set
            {
                Type myType = typeof(User);
                PropertyInfo myPropInfo = myType.GetProperty(propertyName);
                myPropInfo.SetValue(this, value, null);
            }
        }
    }
}
