using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace OsuFarmer.Pages
{
    public partial class InfoPage : UserControl
    {
        public InfoPage()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
