using Hardcodet.Wpf.TaskbarNotification;
using Microsoft.Win32;
using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Security.Cryptography.Pkcs;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using TabletFriend.Docking;
using TabletFriend.TabletMode;
using WpfAppBar;

namespace TabletFriend
{
	public class TrayManager
	{
		private class TrayCommand : ICommand
		{
			// Why do we need an entire class instead of an event handler? No fucking idea.
			public event EventHandler CanExecuteChanged;
			public bool CanExecute(object parameter) => true;
			public void Execute(object parameter) => EventBeacon.SendEvent("toggle_minimize");
		}


		private TaskbarIcon _icon;
		private MenuItem _autostartMenuItem;
		private MenuItem _autoUpdateMenuItem;
		private MenuItem _autohideMenuItem;
		private readonly string _iconPathBlack = AppState.CurrentDirectory + "/files/icons/tray/tray_black.ico";
		private readonly string _iconPathWhite = AppState.CurrentDirectory + "/files/icons/tray/tray_white.ico";
		private readonly LayoutListManager _layoutList;
		private readonly ThemeListManager _themeList;

		private AppFocusMonitor _focusMonitor;
		private MenuItem _focusedApp;
		public TrayManager(LayoutListManager layoutList, ThemeListManager themeList, AppFocusMonitor focusMonitor)
		{
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
			_icon.LeftClickCommand = new TrayCommand();

			CreateMenu();

			EventBeacon.Subscribe("change_layout", OnUpdateLayoutList);
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

			if (AppState.Settings.AddToAutostart)
			{
				_autostartMenuItem = AddMenuItem("remove from autostart", OnAutostartToggle);
			}
			else
			{
				_autostartMenuItem = AddMenuItem("add to autostart", OnAutostartToggle);
			}

			if (AppState.Settings.UpdateCheckingEnabled)
			{
				_autoUpdateMenuItem = AddMenuItem("don't check for updates", OnAutoUpdateToggle);
			}
			else
			{
				_autoUpdateMenuItem = AddMenuItem("check for updates", OnAutoUpdateToggle);
			}

			if (AppState.Settings.ToolbarAutohideEnabled)
			{
				_autohideMenuItem = AddMenuItem("disable toolbar autohide", OnAutohideToggle);
				ToolbarAutohider.StartWatching();
			}
			else
			{
				_autohideMenuItem = AddMenuItem("enable toolbar autohide", OnAutohideToggle);
				ToolbarAutohider.StopWatching();
			}

			AddMenuItem("open layouts directory...", OnOpenLayoutsDirectory);
			AddMenuItem("about", OnAbout);
			_focusedApp = AddMenuItem("focused app: none");
			_focusMonitor.OnAppChanged += OnAppChanged;
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
			EventBeacon.SendEvent("update_settings");
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
			EventBeacon.SendEvent("update_settings");
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
			EventBeacon.SendEvent("update_settings");
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


		private void OnOpenLayoutsDirectory(object sender, RoutedEventArgs e)
		{
			var startInfo = new ProcessStartInfo()
			{
				Arguments = AppState.LayoutsRoot,
				FileName = "explorer.exe"
			};
			Process.Start(startInfo);
		}


		private void OnQuit(object sender, RoutedEventArgs e)
		{
			_icon.Dispose();
			Application.Current.Shutdown();
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
