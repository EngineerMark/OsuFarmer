using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OsuFarmer.Core.Github
{
    public class Release
    {
        [JsonProperty("url")]
        public string? URL { get; set; }

        [JsonProperty("assets_url")]
        public string? AssetsURL { get; set; }

        [JsonProperty("upload_url")]
        public string? UploadURL { get; set; }

        [JsonProperty("html_url")]
        public string? HTMLURL { get; set; }

        [JsonProperty("id")]
        public long ID { get; set; }

        [JsonProperty("author")]
        public Author? Author { get; set; }

        [JsonProperty("node_id")]
        public string? NodeID { get; set; }

        [JsonProperty("tag_name")]
        public string TagName { get; set; } = string.Empty;

        [JsonIgnore]
        public Version? Version { get
            {
                return new Version(TagName.Remove(0, 1));
            } 
        }

        [JsonProperty("target_commitish")]
        public string? TargetCommitish { get; set; }

        [JsonProperty("name")]
        public string? Name { get; set; }

        [JsonProperty("draft")]
        public bool IsDraft { get; set; }

        [JsonProperty("prerelease")]
        public bool IsPrerelease { get; set; }

        [JsonProperty("created_at")]
        public string? CreatedAt { get; set; }

        [JsonProperty("published_at")]
        public string? PublishedAt { get; set; }

        [JsonProperty("assets")]
        public List<Asset>? Assets { get; set; }

        [JsonProperty("body")]
        public string? Body { get; set; }
    }
}
