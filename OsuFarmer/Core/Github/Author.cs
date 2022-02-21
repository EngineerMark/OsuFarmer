using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace OsuFarmer.Core.Github
{
    public class Author
    {
        [JsonProperty("login")]
        public string? Login { get; set; }

        [JsonProperty("id")]
        public long ID { get; set; }
    }
}
