using OsuFarmer.Core;
using OsuFarmer.Managers;

namespace OsuFarmer;

public partial class SessionItemControl : ContentView
{
    public static readonly BindableProperty UsernameProperty = BindableProperty.Create(nameof(Username), typeof(string), typeof(SessionItemControl), string.Empty);
    public static readonly BindableProperty NameProperty = BindableProperty.Create(nameof(Name), typeof(string), typeof(SessionItemControl), string.Empty);
    public static readonly BindableProperty AgeProperty = BindableProperty.Create(nameof(Age), typeof(string), typeof(SessionItemControl), string.Empty);
    
    public string Username
    {
        get => (string)GetValue(SessionItemControl.UsernameProperty);
        set => SetValue(SessionItemControl.UsernameProperty, value);
    }

    public string Name
    {
        get => (string)GetValue(SessionItemControl.NameProperty);
        set => SetValue(SessionItemControl.NameProperty, value);
    }

    public string Age
    {
        get => (string)GetValue(SessionItemControl.AgeProperty);
        set => SetValue(SessionItemControl.AgeProperty, value);
    }

    public SessionItemControl()
	{
		InitializeComponent();
        BindingContext = this;
	}

    private void ButtonOpenElement_Clicked(object sender, EventArgs e)
    {
        Device.BeginInvokeOnMainThread(LoadSessionSequence);
    }

    private void ButtonDeleteElement_Clicked(object sender, EventArgs e)
    {
        Device.BeginInvokeOnMainThread(RemoveSessionSequence);
    }

    private async void LoadSessionSequence()
    {
        Page p = OsuFarmer.Helpers.ViewExtensions.GetParentPage(this);
        Session? s = SessionManager.Instance?.GetSessionByName(Name);
        if (s == null)
        {
            await p.DisplayAlert("Error", "Something went wrong preparing this session file", "Cancel");
            return;
        }

        bool response = await p.DisplayAlert("Load", "Loading a session will also adjust your settings like username and gamemode to it. Are you sure?", "Yes", "No");

        if (response)
        {
            PageManager.Instance?.GoTo<TrackerPage>();
            await AppManager.Instance?.BreakLoopAsync();
            await SessionManager.Instance?.LoadSession(s);
            AppManager.Instance?.StartLoop(false);
        }
    }

    private async void RemoveSessionSequence(){
        Page p = OsuFarmer.Helpers.ViewExtensions.GetParentPage(this);
        bool response = await p.DisplayAlert("Warning", "This will delete the session file permanently. Are you sure?", "Yes", "No");

        if(response){
            Console.WriteLine("Delete session");
            SessionManager.Instance?.RemoveSession(Name);
        }
    }
}