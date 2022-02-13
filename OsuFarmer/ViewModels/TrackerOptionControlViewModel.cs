using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OsuFarmer.ViewModels
{
    public class TrackerOptionControlViewModel : BaseViewModel
    {
        public string AttachedProperty { get; set; }

        private string _Title = "Example Tracker";
        private bool _IsToggled = true;
        public string Title { get { return _Title; } set { _Title = value; OnPropertyChanged(nameof(Title)); } }
        public bool IsToggled { get { return _IsToggled; } set { _IsToggled = value; OnPropertyChanged(nameof(IsToggled)); } }
    }
}
