using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using OsuFarmer.Managers;
using OsuFarmer.ViewModels;
using System;
using System.Threading.Tasks;

namespace OsuFarmer.Alerts
{
    public partial class AlertWindow : Window
    {
        AlertResult res;

        public AlertWindow()
        {
            InitializeComponent();
#if DEBUG
            this.AttachDevTools();
#endif
            this.DataContext = new AlertWindowViewModel();
            this.Owner = UIManager.Instance.MainWindow;
            this.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            res = new AlertResult();

            Closing += AlertWindow_Closing;
        }

        private void AlertWindow_Closing(object? sender, System.ComponentModel.CancelEventArgs e)
        {
            ((AlertWindowViewModel?)DataContext).IsFinished = true;
        }

        public async Task<AlertResult> Run(string? title = null, string? description = null, string[]? buttons = null, bool hasInputField = false, bool isPassword = false)
        {
            Show();
            AlertWindowViewModel? vm = (AlertWindowViewModel?)this.DataContext;

            vm.ShowInputField = hasInputField;

            vm.Title = title;
            vm.Description = description;
            vm.InputFieldPassword = isPassword;

            if (vm.InputFieldPassword)
                this.FindControl<TextBox>("EditField").PasswordChar = '*';

            buttons = buttons ?? new string[] { "Ok", "Cancel" };

            await Avalonia.Threading.Dispatcher.UIThread.InvokeAsync(async () =>
            {
                StackPanel ButtonsList = this.FindControl<StackPanel>("ButtonsList");
                ButtonsList.Children.Clear();
                foreach (var button in buttons)
                {
                    Button b = new Button();
                    b.Content = button;
                    b.Click += (object? sender, Avalonia.Interactivity.RoutedEventArgs e) => OnClick(button);
                    ButtonsList.Children.Add(b);
                }

                while (!vm.IsFinished && IsEnabled)
                    await Task.Delay(25);
            });

            res.SuccessfullResult = vm.IsFinished;
            res.Input = vm.ShowInputField ? vm.InputData : null;

            Close();
            return res;
        }

        private void OnClick(string button)
        {
            res.PressedButton = button;
            AlertWindowViewModel? vm = (AlertWindowViewModel?)this.DataContext;
            vm.IsFinished = true;
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
