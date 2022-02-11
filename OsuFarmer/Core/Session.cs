using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OsuFarmer.Core.Osu;

namespace OsuFarmer.Core
{
    public class Session
    {
        public DateTime CreatedAt { get; set; }
        public DateTime LastUpdatedAt { get; set; }

        public User? Start;
        public User? Latest;

        public Mode Mode { get; set; }
    }
}
