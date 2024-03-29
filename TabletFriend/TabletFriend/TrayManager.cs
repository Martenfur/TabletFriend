﻿using Hardcodet.Wpf.TaskbarNotification;
using Microsoft.Win32;
using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using TabletFriend.Docking;
using TabletFriend.TabletMode;
using WpfAppBar;

namespace TabletFriend
{
	public class TrayManager
	{
		private TaskbarIcon _icon;
		private MenuItem _autostartMenuItem;
		private MenuItem _autoUpdateMenuItem;
		private MenuItem _autohideMenuItem;
		private MenuItem _perAppLayoutsMenuItem;
		private readonly string _iconPathBlack = AppState.CurrentDirectory + "/files/icons/tray/tray_black.ico";
		private readonly string _iconPathWhite = AppState.CurrentDirectory + "/files/icons/tray/tray_white.ico";
		private readonly MainWindow _window;
		private readonly LayoutListManager _layoutList;
		private readonly ThemeListManager _themeList;

		private AppFocusMonitor _focusMonitor;
		private MenuItem _focusedApp;
		public TrayManager(MainWindow window, LayoutListManager layoutList, ThemeListManager themeList, AppFocusMonitor focusMonitor)
		{
			_window = window;
			_layoutList = layoutList;
			_themeList = themeList;
			_focusMonitor = focusMonitor;

			_icon = new TaskbarIcon();

			if (_isLightTheme)
			{
				_icon.Icon = new Icon(_iconPathBlack);
			}
			else
			{ 
				_icon.Icon = new Icon(_iconPathWhite);
			}

			_icon.Visibility = Visibility.Visible;
			_icon.ContextMenu = new ContextMenu();
			_icon.TrayLeftMouseDown += MouseDown;
			CreateMenu();

			EventBeacon.Subscribe(Events.ChangeLayout, OnUpdateLayoutList);
			EventBeacon.Subscribe(Events.FilesChanged, OnUpdateLayoutList);
		}

		private void MouseDown(object sender, RoutedEventArgs e)
		{
			EventBeacon.SendEvent(Events.ToggleMinimize);
		}

		private void OnUpdateLayoutList(object[] obj = null)
		{
			// Secondary quick access context menu.
			Application.Current.Dispatcher.Invoke(
				() =>
				{
					_icon.ContextMenu.Items.Clear();
					_icon.ContextMenu = new ContextMenu();
					CreateMenu();
				}
			);
		}

		private void CreateMenu()
		{
			
			_icon.ContextMenu.Items.Add(_layoutList.CloneMenu());
			_icon.ContextMenu.Items.Add(_themeList.CloneMenu());

			DockingMenuFactory.CreateDockingMenu(_icon.ContextMenu);

			var settings = new MenuItem() { Header = "settings" };

			if (AppState.Settings.AddToAutostart)
			{
				_autostartMenuItem = AddSubmenuItem(settings, "remove from autostart", OnAutostartToggle);
			}
			else
			{
				_autostartMenuItem = AddSubmenuItem(settings, "add to autostart", OnAutostartToggle);
			}

			if (AppState.Settings.UpdateCheckingEnabled)
			{
				_autoUpdateMenuItem = AddSubmenuItem(settings, "don't check for updates", OnAutoUpdateToggle);
			}
			else
			{
				_autoUpdateMenuItem = AddSubmenuItem(settings, "check for updates", OnAutoUpdateToggle);
			}

			if (AppState.Settings.ToolbarAutohideEnabled)
			{
				_autohideMenuItem = AddSubmenuItem(settings, "disable toolbar autohide", OnAutohideToggle);
				ToolbarAutohider.StartWatching();
			}
			else
			{
				_autohideMenuItem = AddSubmenuItem(settings, "enable toolbar autohide", OnAutohideToggle);
				ToolbarAutohider.StopWatching();
			}

			if (AppState.Settings.PerAppLayoutsEnabled)
			{
				_perAppLayoutsMenuItem = AddSubmenuItem(settings, "disable per-app layouts", OnPerAppLayoutsToggle);
			}
			else
			{
				_perAppLayoutsMenuItem = AddSubmenuItem(settings, "enable per-app layouts", OnPerAppLayoutsToggle);
			}


			AddSubmenuItem(settings, "open files directory...", OnOpenLayoutsDirectory);
			_focusedApp = AddSubmenuItem(settings, "focused app: none");
			_focusMonitor.OnAppChanged += OnAppChanged;
			_icon.ContextMenu.Items.Add(settings);

			_icon.ContextMenu.Items.Add(new Separator());
			AddMenuItem("about", OnAbout);
			AddMenuItem("quit", OnQuit);
		}

		private void OnAppChanged(string app)
		{
			_focusedApp.Dispatcher.Invoke(
				() =>
				{
					_focusedApp.Header = "focused app: " + app;
				}
			);	
		}

		private void OnAutostartToggle(object sender, RoutedEventArgs e)
		{
			AppState.Settings.AddToAutostart = !AppState.Settings.AddToAutostart;

			if (AppState.Settings.AddToAutostart)
			{
				AutostartManager.SetAutostart();
				_autostartMenuItem.Header = "remove from autostart";
			}
			else
			{
				AutostartManager.ResetAutostart();
				_autostartMenuItem.Header = "add to autostart";
			}
			EventBeacon.SendEvent(Events.UpdateSettings);
		}

		private void OnAutoUpdateToggle(object sender, RoutedEventArgs e)
		{
			AppState.Settings.UpdateCheckingEnabled = !AppState.Settings.UpdateCheckingEnabled;

			if (AppState.Settings.UpdateCheckingEnabled)
			{
				_autoUpdateMenuItem.Header = "don't check for updates";
			}
			else
			{
				_autoUpdateMenuItem.Header = "check for updates";
			}
			EventBeacon.SendEvent(Events.UpdateSettings);
		}

		private void OnAutohideToggle(object sender, RoutedEventArgs e)
		{
			AppState.Settings.ToolbarAutohideEnabled = !AppState.Settings.ToolbarAutohideEnabled;

			if (AppState.Settings.ToolbarAutohideEnabled)
			{
				_autohideMenuItem.Header = "disable toolbar autohide";
				ToolbarAutohider.StartWatching();
			}
			else
			{
				_autohideMenuItem.Header = "enable toolbar autohide";
				ToolbarAutohider.StopWatching();
			}
			EventBeacon.SendEvent(Events.UpdateSettings);
		}

		private void OnPerAppLayoutsToggle(object sender, RoutedEventArgs e)
		{
			AppState.Settings.PerAppLayoutsEnabled = !AppState.Settings.PerAppLayoutsEnabled;

			if (AppState.Settings.PerAppLayoutsEnabled)
			{
				_perAppLayoutsMenuItem.Header = "disable per-app layouts";
			}
			else
			{
				_perAppLayoutsMenuItem.Header = "enable per-app layouts";
			}
			EventBeacon.SendEvent(Events.UpdateSettings);
		}


		private void OnAbout(object sender, RoutedEventArgs e)
		{
			var startInfo = new ProcessStartInfo()
			{
				Arguments = AppState.LayoutsRoot,
				FileName = "http://github.com/Martenfur/TabletFriend",
				UseShellExecute = true,
			};
			Process.Start(startInfo);
		}

		private MenuItem AddMenuItem(string header, RoutedEventHandler click = null)
		{
			var item = new MenuItem() { Header = header };
			if (click != null)
			{
				item.Click += click;
			}
			else
			{
				item.IsEnabled = false;
			}
			_icon.ContextMenu.Items.Add(item);
			return item;
		}

		private MenuItem AddSubmenuItem(MenuItem menu, string header, RoutedEventHandler click = null)
		{
			var item = new MenuItem() { Header = header };
			if (click != null)
			{
				item.Click += click;
			}
			else
			{
				item.IsEnabled = false;
			}
			menu.Items.Add(item);
			return item;
		}

		private void OnOpenLayoutsDirectory(object sender, RoutedEventArgs e)
		{
			var startInfo = new ProcessStartInfo()
			{
				Arguments = Path.Combine(AppState.CurrentDirectory, "files"),
				FileName = "explorer.exe"
			};
			Process.Start(startInfo);
		}


		private void OnQuit(object sender, RoutedEventArgs e)
		{
			AppBarFunctions.SetAppBar(_window, DockingMode.None);
			_icon.Dispose();
			EventBeacon.SendEvent(Events.UpdateSettings);
			Process.GetCurrentProcess().Kill();
		}

		private bool _isLightTheme
		{
			get
			{
				var key = Registry.GetValue(
					@"HKEY_CURRENT_USER\SOFTWARE\Microsoft\Windows\CurrentVersion\Themes\Personalize", 
					"SystemUsesLightTheme", 
					0
				);
				return !(key == null || (int)key == 0);
			}
		}

	}
}
