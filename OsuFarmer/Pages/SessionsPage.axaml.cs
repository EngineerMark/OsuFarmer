using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace OsuFarmer.Pages
{
    public partial class SessionsPage : UserControl
    {
        public SessionsPage()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
