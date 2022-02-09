using osu_farmer.Managers;

namespace osu_farmer;

public partial class SessionItemControl : ContentView
{
	public SessionItemControl()
	{
		InitializeComponent();
	}

    private void ButtonOpenElement_Clicked(object sender, EventArgs e)
    {

    }

    private void ButtonDeleteElement_Clicked(object sender, EventArgs e)
    {
        Device.BeginInvokeOnMainThread(RemoveSessionSequence);
    }

    private async void RemoveSessionSequence(){
        Page p = osu_farmer.Helpers.ViewExtensions.GetParentPage(this);
        bool response = await p.DisplayAlert("Warning", "Deleting the session will also delete the file. Are you sure?", "Yes", "No");

        if(response){
            Console.WriteLine("Delete session");
            SessionManager.Instance.RemoveSession(SessionIdentifierElement.Text);
        }
    }
}