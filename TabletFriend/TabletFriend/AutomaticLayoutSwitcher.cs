using System.Collections.Generic;
using System.Windows;

namespace TabletFriend
{
	public class AutomaticLayoutSwitcher
	{
		private AppFocusMonitor _monitor;

		private Dictionary<string, string> _appSpecificLayouts = new Dictionary<string, string>();

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
				if (!string.IsNullOrEmpty(layout.Value.App))
				{
					// TODO: More than one same app crash the app.
					_appSpecificLayouts.Add(layout.Value.App, layout.Key);
				}
			}
		}


		private void OnAppChanged(string app)
		{
			if (Application.Current == null)
			{ 
				// Yep. Can happen.
				return;
			}
			Application.Current.Dispatcher.Invoke(
				delegate
				{
					if (_appSpecificLayouts.TryGetValue(app, out var key))
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
	}
}
