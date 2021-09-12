using Hardcodet.Wpf.TaskbarNotification;
using System;
using System.Diagnostics;
using System.IO;
using System.Windows;
using System.Windows.Controls;

namespace TabletFriend
{
	public class TrayManager
	{
		private TaskbarIcon _icon;

		public event Action<string> OnLayoutChanged;

		public TrayManager()
		{
			EventBeacon.Subscribe("update_layout_list", OnUpdateLayoutList);
			_icon = new TaskbarIcon();
			_icon.Visibility = Visibility.Visible;

			_icon.ContextMenu = new ContextMenu();
			_layoutsMenu = AddMenuItem("layouts");
			OnUpdateLayoutList();

			AddMenuItem("open layouts directory...", OnOpenLayoutsDirectory);
			AddMenuItem("quit", OnQuit);
		}

		private void OnUpdateLayoutList(object[] obj = null)
		{
			Application.Current.Dispatcher.Invoke(
				delegate
				{
					_layoutsMenu.Items.Clear();
					foreach (var layout in AppState.Layouts)
					{
						_layoutsMenu.Items.Add(
							new MenuItem()
							{
								Header = Path.GetFileNameWithoutExtension(layout),
								DataContext = layout,
								IsCheckable = true,
								IsChecked = layout == AppState.CurrentLayoutPath
							}
						);
					}
				}
			);
		}

		private void OnOpenLayoutsDirectory(object sender, RoutedEventArgs e)
		{
			var startInfo = new ProcessStartInfo()
			{
				Arguments = AppState.LayoutRoot,
				FileName = "explorer.exe"
			};
			Process.Start(startInfo);
		}

		private void OnQuit(object sender, RoutedEventArgs e) =>
			Application.Current.Shutdown();

		private MenuItem AddMenuItem(string header, RoutedEventHandler click = null)
		{
			var item = new MenuItem() { Header = header };
			if (click != null)
			{
				item.Click += click;
			}
			_icon.ContextMenu.Items.Add(item);
			return item;
		}

		private MenuItem _layoutsMenu;
		public void AddLayout(string layout, bool current)
		{
			_layoutsMenu.Items.Add(new MenuItem() { Header = layout, IsCheckable = true, IsChecked = current });
		}

		public void ClearLayouts() =>
			_layoutsMenu.Items.Clear();

	}
}
