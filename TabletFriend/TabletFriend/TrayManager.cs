using Hardcodet.Wpf.TaskbarNotification;
using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;

namespace TabletFriend
{
	public class TrayManager
	{
		private TaskbarIcon _icon;

		public event Action<string> OnLayoutChanged;

		private FileManager _file;

		public TrayManager(FileManager file)
		{
			_file = file;
			//file.OnChanged += OnChanged;

			_icon = new TaskbarIcon();
			_icon.Visibility = Visibility.Visible;
			
			_icon.ContextMenu = new ContextMenu();
			_layoutsMenu = AddMenuItem("layouts");
			AddMenuItem("open layouts directory...", OnOpenLayoutsDirectory);
			AddMenuItem("quit", OnQuit);
		}

		private void OnOpenLayoutsDirectory(object sender, RoutedEventArgs e)
		{
			var startInfo = new ProcessStartInfo()
			{
				Arguments = _file.LayoutRoot,
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
