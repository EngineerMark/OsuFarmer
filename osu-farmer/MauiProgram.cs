namespace osu_farmer;

public static class MauiProgram
{
	public static MauiApp CreateMauiApp()
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

		return builder.Build();
	}
}
