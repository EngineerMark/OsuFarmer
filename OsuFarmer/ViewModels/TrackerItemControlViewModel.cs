using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OsuFarmer.ViewModels
{
    public class TrackerItemControlViewModel : BaseViewModel
    {
        public string AttachedProperty { get; set; }

        private string _Title = "Example Tracker";
        private double _OriginalValue = 0;
        private double _ChangedValue = 0;

        public string Title { get { return _Title; } set { _Title = value; OnPropertyChanged(nameof(Title)); } }
        public string OriginalValue { get { return string.Format("{0:n0}", _OriginalValue); } set { _OriginalValue = Convert.ToDouble(value); OnPropertyChanged(nameof(OriginalValue)); } }
        public string ChangedValue { get { return string.Format("{0:n0}", _ChangedValue); } set { _ChangedValue = Convert.ToDouble(value); OnPropertyChanged(nameof(ChangedValue)); } }
    }
}
