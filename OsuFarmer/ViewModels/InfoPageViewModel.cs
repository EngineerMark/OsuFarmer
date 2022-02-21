using OsuFarmer.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OsuFarmer.ViewModels
{
    public class InfoPageViewModel : BaseViewModel
    {
        private bool _HasUpdateAvailable = false;
        private string? _UpdateDetails = "Placeholder Update details";

        public bool HasUpdateAvailable { get { return _HasUpdateAvailable; } set { _HasUpdateAvailable = value; OnPropertyChanged(nameof(HasUpdateAvailable)); } }
        public string? UpdateDetails { get { return _UpdateDetails; } set { _UpdateDetails = value; OnPropertyChanged(nameof(UpdateDetails)); } }

        public void OnUpdateDownload()
        {
            MiscHelper.OpenWeb("https://github.com/EngineerMark/OsuFarmer/releases/latest");
        }
    }
}
