using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace OsuFarmer.Core.Github
{
    public class Asset
    {
        [JsonProperty("uploader")]
        public Author? Uploader { get; set; }
    }
}
