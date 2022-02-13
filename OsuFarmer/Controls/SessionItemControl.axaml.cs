using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using OsuFarmer.ViewModels;

namespace OsuFarmer.Controls
{
    public partial class SessionItemControl : UserControl
    {
        public SessionItemControl()
        {
            InitializeComponent();
            DataContext = new SessionItemControlViewModel();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
