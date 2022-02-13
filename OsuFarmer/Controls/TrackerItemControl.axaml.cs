using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using OsuFarmer.ViewModels;

namespace OsuFarmer.Controls
{
    public partial class TrackerItemControl : UserControl
    {
        public TrackerItemControl()
        {
            InitializeComponent();
            DataContext = new TrackerItemControlViewModel();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
