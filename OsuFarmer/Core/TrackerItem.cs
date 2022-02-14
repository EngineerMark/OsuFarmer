using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OsuFarmer.Core
{
    public class TrackerItem
    {
        /// <summary>
        /// Name of the tracker that shows up on the tracker list
        /// </summary>
        public string? Name { get; set; }

        /// <summary>
        /// Attached property string for getting the data from <see cref="OsuFarmer.Core.Osu.User"></see> object
        /// </summary>
        public string? Property { get; set; }

        /// <summary>
        /// Toggles whether the tracker shows up on the tracker list
        /// </summary>
        public bool Enabled { get; set; }

        /// <summary>
        /// The priority of this tracker, lower is more important (higher up the list)
        /// </summary>
        public int Priority { get; set; }

        public TrackerItem(){ }
        public TrackerItem(string name, string property, bool enabled = true){
            this.Name = name;
            this.Property = property;
            this.Enabled = enabled;
        }
        public TrackerItem(TrackerItem item){
            this.Name = item.Name;
            this.Property = item.Property;
            this.Enabled = item.Enabled;
        }
    }
}
