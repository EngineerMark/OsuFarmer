using Microsoft.Maui.LifecycleEvents;

namespace osu_farmer;

public static class MauiProgram
{
	public static MauiApp? CreateMauiApp()
	{
		try
		{
			var builder = MauiApp.CreateBuilder();
			builder
				.UseMauiApp<App>()
				.ConfigureFonts(fonts =>
				{
					fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
					fonts.AddFont("Font Awesome 6 Brands-Regular-400.otf", "FAB");
					fonts.AddFont("Font Awesome 6 Free-Regular-400.otf", "FAR");
					fonts.AddFont("Font Awesome 6 Free-Solid-400.otf", "FAS");
					fonts.AddFont("Torus Regular.otf", "TorusRegular");
				});

			builder.ConfigureLifecycleEvents(lifecycle =>
			{
#if WINDOWS
			lifecycle.AddWindows(windows =>
				windows.OnNativeMessage((app, args) => {
					if (WindowExtensions.Hwnd == IntPtr.Zero)
					{
						WindowExtensions.Hwnd = args.Hwnd;
						WindowExtensions.SetIcon("Platforms/Windows/icon.ico");
					}
				}));
#endif
			});

			return builder.Build();
		}catch (Exception e){
			Console.WriteLine(e.Message);
			return null;
        }
	}
}
