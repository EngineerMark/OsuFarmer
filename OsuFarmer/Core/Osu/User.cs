using Newtonsoft.Json;
using HtmlAgilityPack;
using OsuFarmer.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace OsuFarmer.Core.Osu
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
        public double HitsPerPlay { get => (double)TotalHits / (double)Playcount; }
        public long Clears { get => CountSSH + CountSS + CountSH + CountS + CountA; }
        public long TotalSS { get => CountSSH + CountSS; }
        public long TotalS { get => CountSH + CountS; }

        [JsonIgnore]
        public WebData? WebData { get; set; }

        public async Task<bool> PopulateWebProfile(int mode)
        {
            string? _mode = "osu";
            switch (mode)
            {
                case 0:
                    _mode = "osu";
                    break;
                case 1:
                    _mode = "taiko";
                    break;
                case 2:
                    _mode = "fruits";
                    break;
                case 3:
                    _mode = "mania";
                    break;
            }

            string? web = null;
            try
            {
                web = await ApiHelper.GetData("https://osu.ppy.sh/users/" + ID + "/" + _mode);
            }
            catch (Exception)
            {
                return false;
            }

            if (string.IsNullOrEmpty(web))
                return false;

            HtmlDocument? doc = new HtmlDocument();
            doc.LoadHtml(web);
            HtmlNode jsonNode = doc.DocumentNode.SelectSingleNode("//div[@class='js-react--profile-page osu-layout osu-layout--full']");
            string data = jsonNode.GetAttributeValue("data-initial-data", "{}");
            string decoded = HttpUtility.HtmlDecode(data);

            try
            {
                WebData = JsonConvert.DeserializeObject<WebData>(decoded);
            }
            catch (Exception)
            {
                return false;
            }

            //if (WebData != null && WebData.User != null)
            //{
            //    DateTime now = (DateTime)DateTime.UtcNow;
            //    DateTime? first = null;
            //    int months = -1;
            //    //Fill in empty playcount data
            //    if (WebData?.User?.Playcount != null && WebData?.User?.Playcount?.Count > 1)
            //    {
            //        first = (DateTime)WebData?.User?.Playcount[0]?.Start;
            //        months = Math.Abs((now.Month - ((DateTime)first).Month) + 12 * (now.Year - ((DateTime)first).Year)) + 1;
            //    }


            //    if (WebData?.User?.Playcount != null && WebData?.User?.Playcount?.Count > 1)
            //    {
            //        if (WebData?.User?.Replayswatched != null)
            //        {
            //            List<WebUserMonthlyData?>? replays = new List<WebUserMonthlyData?>();

            //            for (int i = 0; i < months; i++)
            //            {
            //                DateTime m = ((DateTime)first).AddMonths(i);

            //                WebUserMonthlyData s = WebData?.User?.Replayswatched.Find(x => x.Start.Equals(m)) ?? null;
            //                if (s == null)
            //                {
            //                    replays.Add(new WebUserMonthlyData()
            //                    {
            //                        Start = m,
            //                        Count = 0
            //                    });
            //                }
            //                else
            //                {
            //                    replays.Add(s);
            //                }
            //            }

            //            WebData?.User?.SetReplayswatched(replays);
            //        }
            //        List<WebUserMonthlyData?>? playcount = new List<WebUserMonthlyData?>();

            //        for (int i = 0; i < months; i++)
            //        {
            //            DateTime m = ((DateTime)first).AddMonths(i);

            //            WebUserMonthlyData s = WebData?.User?.Playcount.Find(x => x.Start.Equals(m)) ?? null;
            //            if (s == null)
            //            {
            //                playcount.Add(new WebUserMonthlyData()
            //                {
            //                    Start = m,
            //                    Count = 0
            //                });
            //            }
            //            else
            //            {
            //                playcount.Add(s);
            //            }
            //        }

            //        WebData?.User?.SetPlaycount(playcount);
            //    }
            //}
            return WebData != null;
        }

        public object? this[string propertyName]
        {
            get
            {
                // probably faster without reflection:
                // like:  return Properties.Settings.Default.PropertyValues[propertyName] 
                // instead of the following
                Type myType = typeof(User);
                PropertyInfo? myPropInfo = myType.GetProperty(propertyName);
                return myPropInfo.GetValue(this, null);
            }
            set
            {
                Type myType = typeof(User);
                PropertyInfo? myPropInfo = myType.GetProperty(propertyName);
                myPropInfo.SetValue(this, value, null);
            }
        }
    }
}
