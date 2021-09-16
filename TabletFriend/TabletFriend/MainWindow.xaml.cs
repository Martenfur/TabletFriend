using System;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Input;
using System.Windows.Interop;

namespace TabletFriend
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		private LayoutManager _layout;
		private LayoutListManager _layoutList;
		private TrayManager _tray;
		private FileManager _file;

		public MainWindow()
		{
			Topmost = true;
			InitializeComponent();
			MouseDown += OnMouseDown;

			_file = new FileManager();

			_layout = new LayoutManager(this);
			_layout.LoadLayout(AppState.Layouts[0]);
			_layoutList = new LayoutListManager();
			ContextMenu = new System.Windows.Controls.ContextMenu();

			OnUpdateLayoutList();

			_tray = new TrayManager(_layoutList);

			EventBeacon.Subscribe("toggle_minimize", OnToggleMinimize);
			EventBeacon.Subscribe("update_layout_list", OnUpdateLayoutList);
			EventBeacon.Subscribe("change_layout", OnUpdateLayoutList);
		}

		private void OnUpdateLayoutList(object[] obj = null)
		{
			Application.Current.Dispatcher.Invoke(
				() =>
				{
					ContextMenu.Items.Clear();
					var items = _layoutList.CloneMenu();
					foreach (var item in items)
					{
						ContextMenu.Items.Add(item);
					}
				}
			);
		}

		private void OnMouseDown(object sender, MouseButtonEventArgs e)
		{
			if (e.ChangedButton == MouseButton.Left)
			{
				DragMove();
			}
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
		}


		private void OnToggleMinimize(object[] obj)
		{
			// Regular minimize doesn't work without the taaskbar icon.
			// The window just derps out and stays at the bottom left corner.
			// There are workarounds, btu they make an icon flash in the taskbar
			// for a split second. This is the best solution I found.
			if (Visibility == Visibility.Collapsed)
			{
				Visibility = Visibility.Visible;
			}
			else
			{
				Visibility = Visibility.Collapsed;
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
