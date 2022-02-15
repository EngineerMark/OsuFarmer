using Aura.UI.Controls.Navigation;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using OsuFarmer.Managers;
using OsuFarmer.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OsuFarmer
{
    public partial class MainWindow : Window
    {
        AppManager AppManager;
        UIManager UIManager;

        public MainWindow()
        {
            InitializeComponent();
#if DEBUG
            this.AttachDevTools();
#endif
            this.DataContext = new MainWindowViewModel();
            //this.FindControl<Grid>("UILocker").IsVisible = false;

            ClientSize = new Size(AppSettings.Default.Width, AppSettings.Default.Height);
            if (AppSettings.Default.X != -1)
            {
                Position = new PixelPoint(AppSettings.Default.X, AppSettings.Default.Y);
            }

            UIManager = new UIManager(this);
            AppManager = new AppManager(new AppManagerData()
            {
                Window = this,
                UIManager = UIManager
            });
            AppManager.Start();

            Closing += (object? sender, System.ComponentModel.CancelEventArgs e) => OnClose();
        }

        private void OnClose()
        {
            AppSettings.Default.X = Position.X;
            AppSettings.Default.Y = Position.Y;
            AppSettings.Default.Width = ClientSize.Width;
            AppSettings.Default.Height = ClientSize.Height;
            AppSettings.Default.Save();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }

        public void GoToPage(string page)
        {
            Control UILock = this.FindControl<Control>("UILocker");
            UILock.IsVisible = true;
            foreach (UserControl userControl in this.FindControl<Grid>("PagesList").Children)
            {
                userControl.IsVisible = false;

                if(page.Equals(userControl.Name))
                    userControl.IsVisible = true;
            }
			UILock.IsVisible = false;
        }
    }
}
