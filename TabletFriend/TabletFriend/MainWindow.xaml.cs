using System;
using System.Collections.Generic;
using System.Numerics;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Interop;
using WindowsInput.Events;

namespace TabletFriend
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		public MainWindow()
		{
			Topmost = true;
			InitializeComponent();
			MouseDown += OnMouseDown;
		
			LoadYaml();
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


		private const int GWL_EXSTYLE = -20;
		private const int WS_EX_NOACTIVATE = 0x08000000;

		[DllImport("user32.dll")]
		public static extern IntPtr SetWindowLong(IntPtr hWnd, int nIndex, int dwNewLong);

		[DllImport("user32.dll")]
		public static extern int GetWindowLong(IntPtr hWnd, int nIndex);

		private void cock_Click(object sender, RoutedEventArgs e)
		{
			WindowsInput.Simulate.Events().Hold(KeyCode.Shift).Invoke();
		}


		private void CreateButtons(Vector2[] vs)
		{
			var pos = Packer.Pack(vs, 3);

			var size = Packer.GetSize(pos, vs);
			Width = size.X * buttonSize;
			Height = size.Y * buttonSize;

			for (var i = 0; i < pos.Length; i += 1)
			{
				var b = new Button()
				{
					Width = buttonSize * vs[i].X,
					Height = buttonSize * vs[i].Y,
				};

				Canvas.SetTop(b, buttonSize * pos[i].Y);
				Canvas.SetLeft(b, buttonSize * pos[i].X);
				Stacke.Children.Add(b);
			}
		}

		private int buttonSize = 64;

		private void LoadYaml()
		{
			var layout = LayoutImporter.Import("layouts/test_layout.yaml");
			buttonSize = layout.ButtonSize;
			var list = new List<Vector2>();
			foreach(var button in layout.Buttons)
			{
				list.Add(button.Value.Size);
			}

			CreateButtons(list.ToArray());
		}
	}
}
