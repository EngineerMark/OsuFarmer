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
        public void OnUpdateDownload()
        {
            MiscHelper.OpenWeb("https://github.com/EngineerMark/OsuFarmer/releases/latest");
        }
    }
}
