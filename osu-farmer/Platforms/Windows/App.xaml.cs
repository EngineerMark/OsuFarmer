using Microsoft.UI.Xaml;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace osu_farmer.WinUI;

/// <summary>
/// Provides application-specific behavior to supplement the default Application class.
/// </summary>
public partial class App : MauiWinUIApplication
{
    Microsoft.Win32.SafeHandles.SafeFileHandle iIcon;

    /// <summary>
    /// Initializes the singleton application object.  This is the first line of authored code
    /// executed, and as such is the logical equivalent of main() or WinMain().
    /// </summary>
    public App()
    {
        this.InitializeComponent();

        Microsoft.Maui.Handlers.WindowHandler.WindowMapper.AppendToMapping(nameof(IWindow), (h, v) =>
        {
            (v.Handler.NativeView as MauiWinUIWindow).ExtendsContentIntoTitleBar = false;
            (v.Handler.NativeView as MauiWinUIWindow).Title = "osu!Farmer";
        });
    }

    protected override MauiApp CreateMauiApp() => MauiProgram.CreateMauiApp();

    protected override void OnLaunched(LaunchActivatedEventArgs args)
    {
        try
        {
            base.OnLaunched(args);
            Platform.OnLaunched(args);
        }
        catch (Exception ex)
        {
			System.Diagnostics.Debug.WriteLine(ex.Message);
        }

    }
}

