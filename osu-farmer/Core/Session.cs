using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using osu_farmer.Core.Osu;

namespace osu_farmer.Core
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
