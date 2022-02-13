using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using OsuFarmer.ViewModels;

namespace OsuFarmer.Pages
{
    public partial class TrackerPage : UserControl
    {
        public TrackerPage()
        {
            InitializeComponent();
            this.DataContext = new TrackerPageViewModel();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
