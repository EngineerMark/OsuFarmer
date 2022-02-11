using OsuFarmer.Core;

namespace OsuFarmer;

public partial class SessionsPage : ContentPage
{
	public SessionsPage()
	{
		InitializeComponent();
	}

	public void PopulateSessions(List<Session> sessions)
    {
		SessionListElement.Children.Clear();

		if(sessions!=null && sessions.Count > 0)
        {
			foreach(Session session in sessions)
            {
				SessionItemControl control = new SessionItemControl();
				SessionListElement.Children.Add(control);

				control.Name = session.Name;
				control.Age = session.LastUpdatedAt.ToString("MM/dd/yyyy h:mm tt");
				control.Username = session.Start.Username;
            }
        }
	}
}