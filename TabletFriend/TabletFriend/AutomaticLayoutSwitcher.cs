using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Windows;

namespace TabletFriend
{
	public class AutomaticLayoutSwitcher
	{
		private AppFocusMonitor _monitor;

		private Dictionary<string, string> _appSpecificLayouts = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

		public AutomaticLayoutSwitcher(AppFocusMonitor monitor)
		{
			_monitor = monitor;
			_monitor.OnAppChanged += OnAppChanged;

			OnUpdateLayoutList();
			EventBeacon.Subscribe(Events.UpdateLayoutList, OnUpdateLayoutList);
		}


		private void OnUpdateLayoutList(params object[] obj)
		{
			_appSpecificLayouts.Clear();

			foreach (var layout in AppState.Layouts)
			{
				if (string.IsNullOrEmpty(layout.Value.App))
				{
					continue;
				}
				var appRegex = WildCardToRegular(layout.Value.App);
				if (!_appSpecificLayouts.ContainsKey(appRegex))
				{
					_appSpecificLayouts.Add(appRegex, layout.Key);
				}
			}
		}


		private void OnAppChanged(string app)
		{
			if (!AppState.Settings.PerAppLayoutsEnabled)
			{
				return;
			}

			if (Application.Current == null)
			{ 
				// Yep. Can happen.
				return;
			}
			Application.Current.Dispatcher.Invoke(
				delegate
				{
					if (MatchesApp(app, out var key))
					{
						EventBeacon.SendEvent(Events.ChangeLayout, key, LayoutChangeMethod.Automatic);
					}
					else
					{
						if (AppState.LastManuallySetLayout != null)
						{
							EventBeacon.SendEvent(Events.ChangeLayout, AppState.LastManuallySetLayout, LayoutChangeMethod.Automatic);
						}
					}
				}
			);
		}

		private bool MatchesApp(string app, out string layout)
		{
			foreach(var regex in _appSpecificLayouts)
			{
				if (Regex.IsMatch(app, regex.Key, RegexOptions.IgnoreCase))
				{
					layout = regex.Value;
					return true;
				}
			}
			layout = null;
			return false;
		}

		private static string WildCardToRegular(string value) =>
			 "^" + Regex.Escape(value).Replace("\\*", ".*") + "$";
	}
}
