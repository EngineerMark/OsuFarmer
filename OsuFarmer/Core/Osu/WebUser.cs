using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace OsuFarmer.Core.Osu
{
    public class WebData
    {
        [JsonProperty("user")]
        public WebUser? User { get; set; }

        [JsonProperty("extras")]
        public WebExtras? Extras { get; set; }
    }

    public class WebExtras
    {
        [JsonProperty("scoresPinned")]
        public List<PinnedScore?>? PinnedScores { get; set; }
    }

    public class PinnedScore
    {
        [JsonProperty("id")]
        public long ID;

        [JsonProperty("user_id")]
        public long UserID;

        [JsonProperty("accuracy")]
        public float Accuracy;

        [JsonProperty("mods")]
        public string[]? Mods;

        [JsonProperty("score")]
        public long Score;

        [JsonProperty("max_combo")]
        public int Combo;

        [JsonProperty("passed")]
        public bool Passed;

        [JsonProperty("perfect")]
        public bool Perfect;

        [JsonProperty("rank")]
        public string? Grade;

        [JsonProperty("created_at")]
        public string? CreatedAt { get; set; }

        [JsonProperty("best_id")]
        public long BestID;

        [JsonProperty("pp")]
        public double PP;

        [JsonProperty("mode_int")]
        public Mode Mode;

        [JsonProperty("replay")]
        public bool Replay;

        [JsonProperty("beatmapset")]
        public WebBeatmapSet? BeatmapSet;

        [JsonProperty("beatmap")]
        public WebBeatmap? Beatmap;

        [JsonProperty("statistics")]
        public PinnedScoreStats? Stats;

        //public Play ConvertToPlay()
        //{
        //    return new Play(this);
        //}
    }

    public class PinnedScoreStats
    {
        [JsonProperty("count_300")]
        public long Count300 { get; set; }

        [JsonProperty("count_100")]
        public long Count100 { get; set; }

        [JsonProperty("count_50")]
        public long Count50 { get; set; }

        [JsonProperty("count_miss")]
        public long CountMiss { get; set; }

        [JsonProperty("count_katu")]
        public long CountKatu { get; set; }

        [JsonProperty("count_geki")]
        public long CountGeki { get; set; }
    }

    public class WebBeatmapSet
    {
        [JsonProperty("artist")]
        public string? Artist;

        [JsonProperty("artist_unicode")]
        public string? ArtistUnicode;

        [JsonProperty("creator")]
        public string? Creator;

        [JsonProperty("title")]
        public string? Title;

        [JsonProperty("title_unicode")]
        public string? TitleUnicode;

        [JsonProperty("status")]
        public string? Status;
    }

    public class WebBeatmap
    {
        [JsonProperty("beatmapset_id")]
        public long BeatmapSetID;

        [JsonProperty("difficulty_rating")]
        public double DifficultyRating;

        [JsonProperty("id")]
        public long ID;

        [JsonProperty("mode")]
        public string? Mode;

        [JsonProperty("status")]
        public string? Status;

        [JsonProperty("total_length")]
        public int TotalLength;

        [JsonProperty("user_id")]
        public long UserID;

        [JsonProperty("version")]
        public string? Version;

        [JsonProperty("accuracy")]
        public double OD;

        [JsonProperty("ar")]
        public double AR;

        [JsonProperty("bpm")]
        public double BPM;

        [JsonProperty("convert")]
        public bool IsConvert;

        [JsonProperty("count_circles")]
        public int Circles;

        [JsonProperty("count_sliders")]
        public int Sliders;

        [JsonProperty("count_spinners")]
        public int Spinners;

        [JsonProperty("cs")]
        public double CS;

        [JsonProperty("deleted_at")]
        public string? DeletedAt { get; set; }

        [JsonProperty("drain")]
        public double HP;

        [JsonProperty("hit_length")]
        public int Length;

        [JsonProperty("is_scoreable")]
        public bool Scoreable;

        [JsonProperty("last_updated")]
        public string? LastUpdated { get; set; }

        [JsonProperty("mode_int")]
        public int ModeInt;

        [JsonProperty("passcount")]
        public int Passcount;

        [JsonProperty("playcount")]
        public int Playcount;

        [JsonProperty("ranked")]
        public int Ranked;
    }

    public class WebUser
    {
        [JsonProperty("avatar_url")]
        public string? AvatarUrl { get; set; }

        [JsonProperty("country_code")]
        public string? CountryCode { get; set; }

        [JsonProperty("last_visit")]
        public string? LastVisit { get; set; }

        [JsonProperty("discord")]
        public string? Discord { get; set; }

        [JsonProperty("follower_count")]
        public int Followers { get; set; }

        [JsonProperty("beatmap_playcounts_count")]
        public int UniquePlaycount { get; set; }

        [JsonProperty("cover")]
        public WebUserCover? Cover { get; set; }

        [JsonProperty("monthly_playcounts")]
        public List<WebUserMonthlyData?>? Playcount { get; set; }

        [JsonProperty("replays_watched_counts")]
        public List<WebUserMonthlyData?>? Replayswatched { get; set; }

        public void SetPlaycount(List<WebUserMonthlyData?>? l)
        {
            Playcount = l;
        }

        public void SetReplayswatched(List<WebUserMonthlyData?>? l)
        {
            Replayswatched = l;
        }
    }

    public class WebUserMonthlyData
    {
        [JsonProperty("start_date")]
        public string? Start { get; set; }

        [JsonProperty("count")]
        public int Count { get; set; }
    }

    public class WebUserCover
    {
        [JsonProperty("custom_url")]
        public string? CustomURL { get; set; }

        [JsonProperty("url")]
        public string? Url { get; set; }
    }
}
