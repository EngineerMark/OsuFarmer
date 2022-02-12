using Microsoft.Maui.LifecycleEvents;
#if WINDOWS
using OsuFarmer.Platforms.Windows;
#endif

namespace OsuFarmer;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        string title = "osu!Farmer";

        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .ConfigureFonts(fonts =>
            {
                //fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("Torus Regular.otf", "TorusRegular");
            });

#if WINDOWS
        builder.ConfigureLifecycleEvents(lifecycle =>
        {
            lifecycle
            .AddWindows(wndLifeCycleBuilder =>
                {
                    wndLifeCycleBuilder.OnNativeMessage((app, args) =>
                    {

                        app.ExtendsContentIntoTitleBar = false;
                    });

                    wndLifeCycleBuilder.OnWindowCreated(window =>
                    {
                        IntPtr hwnd = WinRT.Interop.WindowNative.GetWindowHandle(window);
                        PInvoke.User32.SetWindowPos(hwnd, PInvoke.User32.SpecialWindowHandles.HWND_TOP,
                        (Int32)AppInternalSettings.Default.X, 
                        (Int32)AppInternalSettings.Default.Y, 
                        (Int32)AppInternalSettings.Default.Width, 
                        (Int32)AppInternalSettings.Default.Height,
                        PInvoke.User32.SetWindowPosFlags.SWP_SHOWWINDOW);

                        PInvoke.User32.SetWindowText(hwnd, title);

                        if (WindowExtensions.Hwnd == IntPtr.Zero)
                        {
                            WindowExtensions.Hwnd = hwnd;
                            WindowExtensions.SetIcon("Platforms/Windows/icon.ico");
                        }
                    });
                    wndLifeCycleBuilder.OnClosed((window, args) =>
                    {
                        IntPtr hwnd = WinRT.Interop.WindowNative.GetWindowHandle(window);
                        if (PInvoke.User32.GetWindowRect(hwnd, out PInvoke.RECT rect1))
                        {
                            AppInternalSettings.Default.Width = rect1.right - rect1.left;
                            AppInternalSettings.Default.Height = rect1.bottom - rect1.top;
                            AppInternalSettings.Default.X = rect1.left;
                            AppInternalSettings.Default.Y = rect1.top;
                        }
                        AppInternalSettings.Default.Save();
                    });
                });
        });
#endif
        return builder.Build();
    }
}
