using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace OsuFarmer.Pages
{
    public partial class LocalApiPage : UserControl
    {
        public LocalApiPage()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
