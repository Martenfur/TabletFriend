using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using TabletFriend.Models;

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

			EventBeacon.Subscribe(Events.UpdateLayoutList, OnUpdateLayoutList);
		}


		private void OnUpdateLayoutList(object[] obj)
		{
			_appSpecificLayouts.Clear();

			foreach (var layout in AppState.Layouts)
			{
				if (!string.IsNullOrEmpty(layout.Value.App))
				{
					_appSpecificLayouts.Add(layout.Value.App, layout.Key);
				}
			}
		}


		private void OnAppChanged(string app)
		{
			Application.Current.Dispatcher.Invoke(
				delegate
				{
					if (_appSpecificLayouts.TryGetValue(app, out var key))
					{
						EventBeacon.SendEvent(Events.ChangeLayout, key);
					}
				}
			);
		}
	}
}
