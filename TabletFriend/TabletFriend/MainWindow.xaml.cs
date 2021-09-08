using System;
using System.Collections.Generic;
using System.Numerics;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Interop;
using TabletFriend.Data;
using WindowsInput.Events;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace TabletFriend
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{

		static string y = @"button:
  action: key A+Ctrl
  actions:
    - key A
    - wait 1000
  name: Button
  icon: icons/cut.svg
  position: 0,1
  size: 1,1
";
		public MainWindow()
		{
			var deserializer = new DeserializerBuilder()
				.WithNamingConvention(UnderscoredNamingConvention.Instance)
				.Build();

			//yml contains a string containing your YAML
			//var p = deserializer.Deserialize<Dictionary<string, ButtonData>>(y);
			Topmost = true;
			InitializeComponent();
			MouseDown += OnMouseDown;


			var vs = new Vector2[]
			{ 
				new Vector2(1, 1),	
				new Vector2(2, 1),	
				new Vector2(1, 1),	
				new Vector2(1, 1),	
				new Vector2(1, 2),	
				new Vector2(2, 2),	
			};

			var pos = LayoutCreator.Pack(vs, 3);

			var size = LayoutCreator.GetSize(pos, vs);
			Width = size.X * 64;
			Height = size.Y * 64;

			for(var i = 0; i < pos.Length; i += 1)
			{
				var b = new Button() 
				{ 
					Width = 64 * vs[i].X,
					Height = 64 * vs[i].Y,
				};
				Canvas.SetTop(b, 64 * pos[i].Y);
				Canvas.SetLeft(b, 64 * pos[i].X);
				Stacke.Children.Add(b);
			}
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
	}
}
