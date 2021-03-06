using Avalonia.Controls;
using OsuFarmer.Controls;
using OsuFarmer.Core;
using OsuFarmer.Pages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OsuFarmer.ViewModels;
using OsuFarmer.Core.Osu;
using System.Globalization;
using OsuFarmer.Alerts;
using Avalonia.Media.Imaging;
using OsuFarmer.Helpers;
using Avalonia.Media;
using OsuFarmer.Core.Github;

namespace OsuFarmer.Managers
{
    /// <summary>
    /// This manager handles all communication between UI and logic. Every control update is going through here first.
    /// </summary>
    public class UIManager : Manager<UIManager>
    {
		public MainWindow MainWindow { get; private set; }

		private int _loaders = 0;
		public async Task SetLoadState(bool state)
        {
			await Avalonia.Threading.Dispatcher.UIThread.InvokeAsync(() =>
			{
				_loaders += (state ? 1 : -1);

				Control UILock = MainWindow.FindControl<Control>("UILocker");
				Control UIContent = MainWindow.FindControl<Control>("UIContent");
                if (_loaders > 0)
                {
					UILock.IsVisible = true;
					UIContent.IsVisible = false;
				}
                else
                {
					UILock.IsVisible = false;
					UIContent.IsVisible = true;
                }
			});
		}

		public async void ApplyUpdate(Release rel)
        {
			await Avalonia.Threading.Dispatcher.UIThread.InvokeAsync(() =>
			{
				InfoPage infoPage = MainWindow.FindControl<InfoPage>("InfoPage");
				InfoPageViewModel? context = (InfoPageViewModel?)infoPage.DataContext;

				context.HasUpdateAvailable = true;
				context.UpdateDetails = rel.Body;
			});
		}

		public async Task ApplySettings(Settings? settings)
        {
			if (settings == null)
				return;

			await Avalonia.Threading.Dispatcher.UIThread.InvokeAsync(async () =>
			{
				await UpdateTrackerFields(settings);
				TrackerPage trackerPage = MainWindow.FindControl<TrackerPage>("TrackerPage");
				TrackerPageViewModel? context = (TrackerPageViewModel?)trackerPage.DataContext;

				context.ShowHeader = settings.ShowHeaderImage;
				context.ShowTimer = settings.ShowTrackerTimer;
				context.ShowClock = settings.ShowClock;
			});
		}

		public async Task TrackersApplyUser(User? user)
        {
			if (user == null)
				return;

			await Avalonia.Threading.Dispatcher.UIThread.InvokeAsync(async () =>
			{
				bool web = await user.PopulateWebProfile((int)SettingsManager.Instance.Settings.ApiGamemode);
				bool scorerank = await user.PopulateScoreRank((int)SettingsManager.Instance.Settings.ApiGamemode);

				TrackerPage trackerPage = MainWindow.FindControl<TrackerPage>("TrackerPage");
				TrackerPageViewModel? context = (TrackerPageViewModel?)trackerPage.DataContext;

				if (context == null)
					throw new NullReferenceException();

				context.Username = user.Username ?? String.Empty;

				//Profile picture
				try
				{
					context.AvatarImage = ImageHelper.GetAvaloniaBitmapFromWeb("https://a.ppy.sh/" + user.ID);
				}
				catch (Exception)
				{
					context.AvatarImage = TrackerPageViewModel.DefaultAvatarImage;
				}

				//Header Image
				if (web)
                {
					string headerUrl = user.WebData.User.Cover.CustomURL;
                    try
                    {
						context.HeaderImage = ImageHelper.GetAvaloniaBitmapFromWeb(headerUrl);
					}
                    catch (Exception)
                    {
						context.HeaderImage = TrackerPageViewModel.DefaultHeaderImage;
                    }
                }
                else
                {
					context.HeaderImage = TrackerPageViewModel.DefaultHeaderImage;
				}

				//Country
				// CultureInfo[] cultures = CultureInfo.GetCultures(CultureTypes.SpecificCultures);
				RegionInfo? region = null;
				try{
					region = new RegionInfo(user.Country.ToUpper());
				}catch(ArgumentException){}
				string? country = null;
				if(region!=null)
					country = region.EnglishName;
				// foreach (CultureInfo culture in cultures)
				// {
				// 	RegionInfo region = new RegionInfo(culture.LCID);
				// 	if (region.TwoLetterISORegionName.ToUpper() == user.Country.ToUpper())
				// 	{
				// 		country = region.EnglishName;
				// 		break;
				// 	}
				// }
				context.CountryName = country ?? "Unknown";

				try
				{
					context.FlagImage = ImageHelper.GetAvaloniaBitmapFromAssets("avares://OsuFarmer/Assets/Images/Flags/"+user.Country.ToUpperInvariant()+".png");
				}
				catch (Exception)
				{
					context.FlagImage = TrackerPageViewModel.DefaultFlagImage;
				}

				//if (web)
				//	trackerUserHeader.Source = user.WebData?.User?.Cover?.Url;
				//else
				//	trackerUserHeader.Source = string.Empty;
			});
		}

		public async Task TrackersApplySession(Session session)
		{
			await Avalonia.Threading.Dispatcher.UIThread.InvokeAsync(() =>
			{
				foreach (TrackerItem item in SettingsManager.Instance.Settings.RunningTrackers)
				{

					double original = Convert.ToDouble(session.Start[item.Property??string.Empty]);
					double current = Convert.ToDouble(session.Latest[item.Property ?? string.Empty]);

					double diff = current - original;

					TrackerItemControl? tracker = FindTrackerItem(item.Property ?? string.Empty);
					TrackerItemControlViewModel? context = (TrackerItemControlViewModel?)tracker.DataContext;

					context.OriginalValue = ""+current;
					context.ChangedValue = ""+diff;

					context.ChangedSign = (diff == 0 ? "" : (
						!context.InvertSign ? 
						(diff>0?TrackerItemControlViewModel.PositiveCharacter:TrackerItemControlViewModel.NegativeCharacter) :
						(diff>0?TrackerItemControlViewModel.NegativeCharacter:TrackerItemControlViewModel.PositiveCharacter)
					));

					context.ChangedSignColor = (diff == 0 ? Brushes.White : (
						!context.InvertSign ?
						(diff > 0 ? TrackerItemControlViewModel.PositiveColor : TrackerItemControlViewModel.NegativeColor) :
						(diff > 0 ? TrackerItemControlViewModel.NegativeColor : TrackerItemControlViewModel.PositiveColor)
					));

					//tracker.SetCurrentValue(current);
					//tracker.SetChangedValue(diff);
				}
			});


		}

		public void SetClockValue(DateTime time)
        {
			Avalonia.Threading.Dispatcher.UIThread.InvokeAsync(() =>
			{
				TrackerPage trackerPage = MainWindow.FindControl<TrackerPage>("TrackerPage");
				TrackerPageViewModel? context = (TrackerPageViewModel?)trackerPage.DataContext;

				context.ClockValue = time.ToString("HH:mm:ss");
			});
		}

		public void SetTrackerProgress(DateTime start, DateTime now, DateTime expected)
        {
			double diff = (expected - start).TotalMilliseconds;
			double perc = (DateTime.Now - start).TotalMilliseconds / diff;

			Avalonia.Threading.Dispatcher.UIThread.InvokeAsync(() =>
			{
				TrackerPage trackerPage = MainWindow.FindControl<TrackerPage>("TrackerPage");
				TrackerPageViewModel? context = (TrackerPageViewModel?)trackerPage.DataContext;

				context.TimerProgress = 1 - perc;
				context.TimerText = string.Format("Next update in {0} seconds", Math.Round((expected - DateTime.Now).TotalSeconds));
			});
		}

		public async Task GenerateTrackerFields(Settings? settings)
		{
			if (settings == null)
				return;

			await Avalonia.Threading.Dispatcher.UIThread.InvokeAsync(async () =>
			{
				TrackerPage trackerPage = MainWindow.FindControl<TrackerPage>("TrackerPage");
				StackPanel trackerItemList = trackerPage.FindControl<StackPanel>("TrackerItemList");
				trackerItemList.Children.Clear();
				if (settings.RunningTrackers != null && settings.RunningTrackers.Count > 0)
				{
					foreach (TrackerItem item in Settings.PrefabTrackers)
					{
						TrackerItemControl c = new TrackerItemControl();
						c.Name = item.Property;

						trackerItemList.Children.Add(c);
						TrackerItemControlViewModel? context = (TrackerItemControlViewModel?)c.DataContext;
						context.Title = item.Name ?? string.Empty;
						context.InvertSign = item.Inverted;
						context.Rounding = item.Rounding;
						//c.SetTitle(item.Name);
						//c.SetChangedValue(0);
						//c.AttachedProperty = item.Property;
						//context.IsVisible = settings.RunningTrackers.Exists(_item => _item.Property == item.Property);
					}
				}
				await UpdateTrackerFields(settings);
			});
		}

		public async Task UpdateTrackerFields(Settings? settings)
		{
			if (settings == null)
				return;

			await Avalonia.Threading.Dispatcher.UIThread.InvokeAsync(() =>
			{
				if (settings.RunningTrackers != null && settings.RunningTrackers.Count > 0)
				{
					foreach (TrackerItem item in Settings.PrefabTrackers)
					{
						TrackerItemControl? control = FindTrackerItem(item.Property);
						control.IsVisible = settings.RunningTrackers.Exists(_item => _item.Property == item.Property); ;
					}
				}
			});
		}

		public void PrefillSettings(Settings? settings)
		{
			if (settings == null)
				return;

			Avalonia.Threading.Dispatcher.UIThread.InvokeAsync(() =>
			{
				SettingsPage settingsPage = MainWindow.FindControl<SettingsPage>("SettingsPage");
				SettingsPageViewModel? context = (SettingsPageViewModel?)settingsPage.DataContext;

				context.APIKey = settings.ApiKey ?? string.Empty;
				context.APIUsername = settings.ApiUsername ?? string.Empty;
				context.APIGamemode = settings.ApiGamemode;
				context.APIUpdateRate = settings.ApiUpdateInterval;
				context.VisualsHeaderEnabled = settings.ShowHeaderImage;
				context.VisualsProgressTimerEnabled = settings.ShowTrackerTimer;
				context.VisualsSmoothProgressTimerEnabled = settings.SmoothTrackerTimer;

				StackPanel settingsTrackerList = settingsPage.FindControl<StackPanel>("settingsTrackerList");

                if (settingsTrackerList.Children.Count == 0)
                {
					foreach (TrackerItem item in Settings.PrefabTrackers)
					{
						TrackerOptionControl optionControl = new TrackerOptionControl();
						TrackerOptionControlViewModel? _context = (TrackerOptionControlViewModel?)optionControl.DataContext;
						optionControl.Name = item.Property ?? string.Empty;
						settingsTrackerList.Children.Add(optionControl);

						_context.Title = item.Name ?? string.Empty;
						_context.AttachedProperty = item.Property ?? string.Empty;
					}
                }

				foreach (TrackerItem item in Settings.PrefabTrackers)
                {
					TrackerOptionControl? optionControl = FindTrackerOption(item.Property);
					TrackerOptionControlViewModel? _context = (TrackerOptionControlViewModel?)optionControl.DataContext;
					_context.IsToggled = settings.RunningTrackers.Exists(_item => _item.Property == item.Property);
				}
			});

		}

		public void PopulateSessions(List<Session> sessions)
        {
			Avalonia.Threading.Dispatcher.UIThread.InvokeAsync(() =>
			{
				SessionsPage sessionsPage = MainWindow.FindControl<SessionsPage>("SessionsPage");
				StackPanel SessionListElement = sessionsPage.FindControl<StackPanel>("SessionListElement");
				SessionListElement.Children.Clear();

				if (sessions != null && sessions.Count > 0)
				{
					foreach (Session session in sessions)
					{
						SessionItemControl control = new SessionItemControl();
						SessionItemControlViewModel? context = (SessionItemControlViewModel?)control.DataContext;
						SessionListElement.Children.Add(control);

						context.Username = session.Start.Username ?? string.Empty;
						context.Filename = session.Name ?? string.Empty;
						context.Fileage = session.LastUpdatedAt.ToString("MM/dd/yyyy h:mm tt");
					}
				}
			});
		}

		public TrackerOptionControl? FindTrackerOption(string? name)
        {
			if (name == null)
				return null;

			SettingsPage settingsPage = MainWindow.FindControl<SettingsPage>("SettingsPage");
			StackPanel settingsTrackerList = settingsPage.FindControl<StackPanel>("settingsTrackerList");

            if (settingsTrackerList.Children.Count > 0)
            {
				foreach (TrackerOptionControl item in settingsTrackerList.Children)
				{
					if (item.Name.Equals(name, StringComparison.InvariantCultureIgnoreCase))
						return item;
				}
			}
			return null;
		}

		private TrackerItemControl? FindTrackerItem(string? name)
		{
			if (name == null)
				return null;

			TrackerPage trackerPage = MainWindow.FindControl<TrackerPage>("TrackerPage");
			StackPanel trackerItemList = trackerPage.FindControl<StackPanel>("TrackerItemList");

			if (trackerItemList.Children.Count > 0)
			{
				foreach (TrackerItemControl item in trackerItemList.Children)
				{
					if (item.Name.Equals(name, StringComparison.InvariantCultureIgnoreCase))
						return item;
				}
			}
			return null;
		}

		public async Task<AlertResult> DisplayAlertAsync(string title, string message, string[]? buttons = null)
		{
			return await Avalonia.Threading.Dispatcher.UIThread.InvokeAsync(async () =>
			{
				await SetLoadState(true);
				AlertWindow alertWindow = new AlertWindow();
				alertWindow.ShowDialog(MainWindow);
				AlertResult res = await alertWindow.Run(title, message, buttons);
				await SetLoadState(false);
				return res;
			});
		}

		public async Task<AlertResult> DisplayInputAlertAsync(string title, string message, bool isPassword = false)
		{
			return await Avalonia.Threading.Dispatcher.UIThread.InvokeAsync(async () =>
			{
				await SetLoadState(true);
				AlertWindow alertWindow = new AlertWindow();
				alertWindow.ShowDialog(MainWindow);
				AlertResult res = await alertWindow.Run(title, message, new string[] { "Continue" }, true, isPassword);
				await SetLoadState(false);
				return res;
			});
		}

		public void ToggleExpansiveMode(bool state)
		{
			Avalonia.Threading.Dispatcher.UIThread.InvokeAsync(() =>
			{
				if (SettingsManager.Instance == null || SettingsManager.Instance.Settings == null)
					return;

				state = SettingsManager.Instance.Settings.UseExpandedTracker && state;

                //TODO implement
                state = false;

                TrackerPage trackerPage = MainWindow.FindControl<TrackerPage>("TrackerPage");
				TrackerPageViewModel? vm = (TrackerPageViewModel?)trackerPage.DataContext;

				vm.ExpansiveModeEnabled = state;
				vm.ThinTrackerWidth = vm.ExpansiveModeEnabled ? UIManager.Instance.MainWindow.MinWidth : double.NaN;
			});
		}

		public void GoToPage(string page)
		{
			Avalonia.Threading.Dispatcher.UIThread.InvokeAsync(() =>
			{
				MainWindow.GoToPage(page);
			});
		}

		public UIManager(MainWindow MainWindow)
        {
			this.MainWindow = MainWindow;

			Register(this);
        }
	}
}
