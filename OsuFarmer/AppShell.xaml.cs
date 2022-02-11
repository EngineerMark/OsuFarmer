using OsuFarmer.Managers;

namespace OsuFarmer;

public partial class AppShell : Shell
{
	private AppManager? appManager;

	public AppShell()
	{
		InitializeComponent();

		Title = "osu!Tracker";

        appManager = new AppManager(new AppManagerData()
        {
            Shell = this,
        });
        appManager.Start();
    }

	public ContentPage? AddPage(Type t){
		if (!t.IsSubclassOf(typeof(ContentPage)))
			return null;

		ContentPage? page = (ContentPage?)Activator.CreateInstance(t);
		if (page == null)
			return null;

		ShellContent content = new ShellContent();
		content.Content = page;
		content.Route = page.Title.ToLower();

		FlyoutItem flyoutItem = new FlyoutItem();
		flyoutItem.Items.Add(content);
		flyoutItem.Title = page?.Title;

		Items.Add(flyoutItem);
		return (ContentPage?)content.Content;
	}
}