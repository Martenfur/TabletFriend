﻿using Hardcodet.Wpf.TaskbarNotification;
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

		public TrayManager(LayoutListManager layoutList)
		{
			_icon = new TaskbarIcon();
			_icon.Visibility = Visibility.Visible;

			_icon.ContextMenu = new ContextMenu();

			_icon.ContextMenu.Items.Add(layoutList.Menu);
			AddMenuItem("open layouts directory...", OnOpenLayoutsDirectory);
			AddMenuItem("quit", OnQuit);
		}



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

	}
}
