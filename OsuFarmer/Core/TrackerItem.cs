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
        /// If true, negative numbers are seen as good, like decrease in ranking
        /// </summary>
        public bool Inverted { get; set; }

        /// <summary>
        /// The priority of this tracker, lower is more important (higher up the list)
        /// </summary>
        public int Priority { get; set; }

        public TrackerItem(){ }
        public TrackerItem(string name, string property, bool enabled = true, bool inverted = false){
            this.Name = name;
            this.Property = property;
            this.Enabled = enabled;
            this.Inverted = inverted;
        }
        public TrackerItem(TrackerItem item){
            this.Name = item.Name;
            this.Property = item.Property;
            this.Enabled = item.Enabled;
        }
    }
}
