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
				fonts.AddFont("fa-brands-400.ttf", "FAB");
				fonts.AddFont("fa-regular-400.ttf", "FAR");
				fonts.AddFont("fa-solid-400.ttf", "FAS");
			});

		return builder.Build();
	}
}
