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

namespace OsuFarmer.Managers
{
    /// <summary>
    /// This manager handles all communication between UI and logic. Every control update is going through here first.
    /// </summary>
    public class UIManager : Manager<UIManager>
    {
		public MainWindow MainWindow { get; private set; }

		private int _loaders = 0;
		public void SetLoadState(bool state)
        {
			Avalonia.Threading.Dispatcher.UIThread.InvokeAsync(() =>
			{
				_loaders += (state ? 1 : -1);

				Control UILock = MainWindow.FindControl<Control>("UILocker");
                if (_loaders > 0)
					UILock.IsVisible = true;
                else
					UILock.IsVisible = false;
			});
		}

		public async Task ApplySettings(Settings settings)
        {
			await Avalonia.Threading.Dispatcher.UIThread.InvokeAsync(async () =>
			{
				await UpdateTrackerFields(settings);
				TrackerPage trackerPage = MainWindow.FindControl<TrackerPage>("TrackerPage");
				TrackerPageViewModel context = (TrackerPageViewModel)trackerPage.DataContext;

				context.ShowHeader = settings.ShowHeaderImage;
			});
		}

		public async Task TrackersApplyUser(User user)
        {
			await Avalonia.Threading.Dispatcher.UIThread.InvokeAsync(async () =>
			{
				bool web = await user.PopulateWebProfile((int)SettingsManager.Instance.Settings.ApiGamemode);

				TrackerPage trackerPage = MainWindow.FindControl<TrackerPage>("TrackerPage");
				TrackerPageViewModel context = (TrackerPageViewModel)trackerPage.DataContext;
				context.Username = user.Username;

				CultureInfo[] cultures = CultureInfo.GetCultures(CultureTypes.SpecificCultures);
				string? country = null;
				foreach (CultureInfo culture in cultures)
				{
					RegionInfo region = new RegionInfo(culture.LCID);
					if (region.TwoLetterISORegionName.ToUpper() == user.Country.ToUpper())
					{
						country = region.EnglishName;
						break;
					}
				}
				context.CountryName = country ?? "Unknown";
				//if (web)
				//	trackerUserHeader.Source = user.WebData?.User?.Cover?.Url;
				//else
				//	trackerUserHeader.Source = string.Empty;
			});
		}

		public void TrackersApplySession(Session session)
		{
			Avalonia.Threading.Dispatcher.UIThread.InvokeAsync(() =>
			{
				foreach (TrackerItem item in SettingsManager.Instance.Settings.RunningTrackers)
				{

					double original = Convert.ToInt64(session.Start[item.Property]);
					double current = Convert.ToInt64(session.Latest[item.Property]);

					double diff = current - original;

					TrackerItemControl? tracker = FindTrackerItem(item.Property);
					TrackerItemControlViewModel context = (TrackerItemControlViewModel)tracker.DataContext;

					context.OriginalValue = ""+current;
					context.ChangedValue = ""+diff;

					//tracker.SetCurrentValue(current);
					//tracker.SetChangedValue(diff);
				}
			});


		}

		public async Task GenerateTrackerFields(Settings settings)
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
						context.Title = item.Name;
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
				context.VisualsHeaderEnabled = settings.ShowHeaderImage;

				StackPanel settingsTrackerList = settingsPage.FindControl<StackPanel>("settingsTrackerList");

                if (settingsTrackerList.Children.Count == 0)
                {
					foreach (TrackerItem item in Settings.PrefabTrackers)
					{
						TrackerOptionControl optionControl = new TrackerOptionControl();
						TrackerOptionControlViewModel? _context = (TrackerOptionControlViewModel?)optionControl.DataContext;
						optionControl.Name = item.Property;
						settingsTrackerList.Children.Add(optionControl);

						_context.Title = item.Name;
						_context.AttachedProperty = item.Property;
					}
                }

				foreach (TrackerItem item in Settings.PrefabTrackers)
                {
					TrackerOptionControl optionControl = FindTrackerOption(item.Property);
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

						context.Username = session.Start.Username;
						context.Filename = session.Name;
						context.Fileage = session.LastUpdatedAt.ToString("MM/dd/yyyy h:mm tt");
					}
				}
			});
		}

		public TrackerOptionControl? FindTrackerOption(string name)
        {
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

		private TrackerItemControl FindTrackerItem(string name)
		{
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
				SetLoadState(true);
				AlertWindow alertWindow = new AlertWindow();
				AlertResult res = await alertWindow.Run(title, message, buttons);
				SetLoadState(false);
				return res;
			});
		}

		public async Task<AlertResult> DisplayInputAlertAsync(string title, string message, bool isPassword = false)
		{
			return await Avalonia.Threading.Dispatcher.UIThread.InvokeAsync(async () =>
			{
				SetLoadState(true);
				AlertWindow alertWindow = new AlertWindow();
				AlertResult res = await alertWindow.Run(title, message, new string[] { "Continue" }, true, isPassword);
				SetLoadState(false);
				return res;
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
