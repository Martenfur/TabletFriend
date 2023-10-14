using Microsoft.Win32;
using System;
using System.ComponentModel;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Interop;
using TabletFriend.Docking;
using TabletFriend.TabletMode;
using WpfAppBar;

namespace TabletFriend
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window, INotifyPropertyChanged
	{
		private LayoutManager _layout;
		private ThemeManager _theme;
		private LayoutListManager _layoutList;
		private ThemeListManager _themeList;
		private AutomaticLayoutSwitcher _layoutSwitcher;
		private TrayManager _tray;
		private FileManager _file;

		public event PropertyChangedEventHandler PropertyChanged;

		private void OnPropertyChanged(string property)
		{
			// i hate bizdev i hate bizdev i hate bizdev i hate bizdev i hate bizdev i hate bizdev i hate bizdev i hate bizdev i hate bizdev i hate bizdev i hate bizdev i hate bizdev i hate bizdev i hate bizdev
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));
		}

		public MainWindow()
		{
			var focusMonitor = new AppFocusMonitor(); // Has to be at the very top or else it hangs on starup. Why? No idea. 

			SystemEvents.DisplaySettingsChanged += OnSizeChanged;

			Directory.SetCurrentDirectory(AppState.CurrentDirectory);

			Topmost = true;
			InitializeComponent();
			MouseDown += OnMouseDown;

			_file = new FileManager();

			ToggleManager.Init();

			_theme = new ThemeManager();
			_layout = new LayoutManager();
			Settings.Load();

			Installer.TryInstall();

			_ = UpdateChecker.Check();


			_layoutList = new LayoutListManager();
			_themeList = new ThemeListManager();
			ContextMenu = new System.Windows.Controls.ContextMenu();

			OnUpdateLayoutList();


			_layoutSwitcher = new AutomaticLayoutSwitcher(focusMonitor);
			_tray = new TrayManager(this, _layoutList, _themeList, focusMonitor);


			if (AppState.Settings.AddToAutostart)
			{
				AutostartManager.SetAutostart();
			}
			else
			{
				AutostartManager.ResetAutostart();
			}

			
			EventBeacon.Subscribe(Events.ToggleMinimize, OnToggleMinimize);
			EventBeacon.Subscribe(Events.Maximize, OnMaximize);
			EventBeacon.Subscribe(Events.Minimize, OnMinimize);
			EventBeacon.Subscribe(Events.UpdateLayoutList, OnUpdateLayoutList);
			EventBeacon.Subscribe(Events.ChangeLayout, OnUpdateLayoutList);
			EventBeacon.Subscribe(Events.DockingChanged, OnDockingChanged);
		}


		private void OnSizeChanged(object sender, EventArgs eventArgs)
		{
			UiFactory.CreateUi(AppState.CurrentLayout, this);
		}


		private double _maxOpacity;
		public double MaxOpacity
		{
			get => _maxOpacity;
			set
			{
				_maxOpacity = value;
				OnPropertyChanged(nameof(MaxOpacity));
			}
		}


		private double _minOpacity;
		public double MinOpacity
		{
			get => _minOpacity;
			set
			{
				_minOpacity = value;
				OnPropertyChanged(nameof(MinOpacity));
			}
		}


		private void OnUpdateLayoutList(object[] obj = null)
		{
			// Secondary quick access context menu.
			Application.Current.Dispatcher.Invoke(
				() =>
				{
					ContextMenu.Items.Clear();
					DockingMenuFactory.CreateDockingMenu(ContextMenu);

					ContextMenu.Items.Add(new Separator());
					var items = _layoutList.GetClonedItems();
					foreach (var item in items)
					{
						ContextMenu.Items.Add(item);
					}
				}
			);
		}

		private void OnMouseDown(object sender, MouseButtonEventArgs e)
		{
			if (
				e.ChangedButton == MouseButton.Left
				&& AppState.Settings.DockingMode == DockingMode.None
			)
			{
				DragMove();
			}
		}

		private void OnDockingChanged(params object[] args)
		{
			var side = (DockingMode)args[0];

			if (side != DockingMode.None && side != AppState.Settings.DockingMode)
			{
				AppBarFunctions.SetAppBar(this, DockingMode.None);
			}
			
			AppState.Settings.DockingMode = side;

			UiFactory.CreateUi(AppState.CurrentLayout, this);
			
			if (Visibility == Visibility.Visible)
			{
				AppBarFunctions.SetAppBar(this, side, _layout.LastLoadResult == LayoutLoadResult.RequiresRedock);
			}

			if (side != DockingMode.None)
			{
				MinOpacity = AppState.CurrentLayout.MaxOpacity;
				MaxOpacity = AppState.CurrentLayout.MaxOpacity;
				BeginAnimation(OpacityProperty, null);
				Opacity = AppState.CurrentLayout.MaxOpacity;
			}
			else
			{
				MinOpacity = AppState.CurrentLayout.MinOpacity;
				MaxOpacity = AppState.CurrentLayout.MaxOpacity;
				BeginAnimation(OpacityProperty, null);
				Opacity = AppState.CurrentLayout.MaxOpacity;
				BeginAnimation(OpacityProperty, FadeOut);
			}


			EventBeacon.SendEvent(Events.UpdateSettings);
		}


		protected override void OnSourceInitialized(EventArgs e)
		{
			base.OnSourceInitialized(e);

			// Makes it so the window doesn't hold focus.
			var helper = new WindowInteropHelper(this);
			SetWindowLong(
				helper.Handle,
				GWL_EXSTYLE,
				GetWindowLong(helper.Handle, GWL_EXSTYLE) | WS_EX_NOACTIVATE
			);

			EventBeacon.SendEvent(Events.DockingChanged, AppState.Settings.DockingMode);
		}


		private void OnToggleMinimize(object[] obj)
		{
			// Regular minimize doesn't work without the taskbar icon.
			// The window just derps out and stays at the bottom left corner.
			// There are workarounds, but they make an icon flash in the taskbar
			// for a split second. This is the best solution I found.
			if (Visibility == Visibility.Collapsed || Visibility == Visibility.Hidden)
			{
				Visibility = Visibility.Visible;
				AppBarFunctions.SetAppBar(this, AppState.Settings.DockingMode);
			}
			else
			{
				AppBarFunctions.SetAppBar(this, DockingMode.None);
				Visibility = Visibility.Hidden;
			}
		}

		private void OnMinimize(object[] obj)
		{
			if (Visibility == Visibility.Visible)
			{
				AppBarFunctions.SetAppBar(this, DockingMode.None);
				Visibility = Visibility.Hidden;
			}
		}

		private void OnMaximize(object[] obj)
		{
			if (Visibility == Visibility.Collapsed || Visibility == Visibility.Hidden)
			{
				Visibility = Visibility.Visible;
				AppBarFunctions.SetAppBar(this, AppState.Settings.DockingMode);
			}
		}


		private const int GWL_EXSTYLE = -20;
		private const int WS_EX_NOACTIVATE = 0x08000000;

		[DllImport("user32.dll")]
		public static extern IntPtr SetWindowLong(IntPtr hWnd, int nIndex, int dwNewLong);

		[DllImport("user32.dll")]
		public static extern int GetWindowLong(IntPtr hWnd, int nIndex);
	}
}
