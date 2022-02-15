using Avalonia;
using Avalonia.Controls;
using OsuFarmer.Managers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OsuFarmer.ViewModels
{
    public class MainWindowViewModel : BaseViewModel
    {
        public void OnTabButtonPress(string page)
        {
            UIManager.Instance.GoToPage(page);
        }
    }
}
