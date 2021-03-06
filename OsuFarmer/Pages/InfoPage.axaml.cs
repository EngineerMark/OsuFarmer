using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using OsuFarmer.ViewModels;

namespace OsuFarmer.Pages
{
    public partial class InfoPage : UserControl
    {
        public InfoPage()
        {
            InitializeComponent();
            DataContext = new InfoPageViewModel();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
