using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using OsuFarmer.ViewModels;

namespace OsuFarmer.Controls
{
    public partial class TrackerOptionControl : UserControl
    {
        public TrackerOptionControl()
        {
            InitializeComponent();
            this.DataContext = new TrackerOptionControlViewModel();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
