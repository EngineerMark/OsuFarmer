using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace osu_farmer.Core
{
    public class TrackerItem
    {
        public string? Name { get; set; }
        public string? Property { get; set; }
        public bool Enabled { get; set; }

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
