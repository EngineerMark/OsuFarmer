using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using OsuFarmer.Core.Osu;
using OsuFarmer.Managers;
using OsuFarmer.ViewModels;
using System;
using System.ComponentModel;

namespace OsuFarmer.Pages
{
    public partial class SettingsPage : UserControl
    {
        public SettingsPage()
        {
            InitializeComponent();
            this.DataContext = new SettingsPageViewModel();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
