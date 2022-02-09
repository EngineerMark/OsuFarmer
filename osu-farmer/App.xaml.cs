using System.Runtime.Loader;

namespace osu_farmer;

public partial class App : Application
{
	public App()
	{
#if DEBUG
		System.Diagnostics.Debugger.Launch();
#endif
		InitializeComponent();

		MainPage = new AppShell();
	}
}
