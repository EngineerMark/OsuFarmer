using System.Runtime.Loader;

namespace osu_farmer;

public partial class App : Application
{
	public App()
	{
		InitializeComponent();

		MainPage = new AppShell();
	}
}
