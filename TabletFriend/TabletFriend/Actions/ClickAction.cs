using System.Drawing;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using WindowsInput.Events;

namespace TabletFriend.Actions
{
	public class ClickAction : ButtonAction
	{
		private static System.Drawing.Point? _newClickCoordinates = null;
		private static Rectangle _dragBoxFromMouseDown;

		private bool _initialized = false;
		private readonly int _x;
		private readonly int _y;

		public ClickAction(string cmd)
		{
			// The regular expression extracts the coordinates from the given cmd string.
			// Furthermore it validates the format of the string.
			// The string has to consist of two numbers separated by a comma "," (white spaces are ignored) like e.g
			// "10,20" or " 10,20 " or "10, 20" or "  10  , 20 "
			Regex expression = new Regex(@"^\s*(?<x_coordinate>\d+)\s*,\s*(?<y_coordinate>\d+)\s*$");
			Match match = expression.Match(cmd);
			if (match.Success)
			{
				_x = int.Parse(match.Groups["x_coordinate"].Value);
				_y = int.Parse(match.Groups["y_coordinate"].Value);
				_initialized = true;
			}
		}

		public async override Task Invoke()
		{
			if (_initialized)
			{
				var sim = new EventBuilder();
				sim.MoveTo(_x, _y).Click(ButtonCode.Left);
				await sim.Invoke();
			}
		}

		public static void AddDragAndDropEventHandlers(ButtonBase uiButton, string key)
		{
			// Add event handlers to the click action button
			// to support drag/drop for defining the destination coordinates.
			uiButton.Name = key;
			uiButton.PreviewMouseLeftButtonUp += MouseUp;
			uiButton.PreviewMouseLeftButtonDown += MouseDown;
			uiButton.PreviewMouseMove += MouseMove;
			uiButton.PreviewQueryContinueDrag += QueryContinueDrag;
			uiButton.PreviewGiveFeedback += GiveFeedback;
		}

		private static void MouseDown(object sender, System.Windows.Input.MouseEventArgs e)
		{
			// Remember the point where the mouse down occurred. The DragSize indicates
			// the size that the mouse can move before a drag event should be started.
			System.Drawing.Size dragSize = System.Windows.Forms.SystemInformation.DragSize;
			var x = System.Windows.Forms.Cursor.Position.X;
			var y = System.Windows.Forms.Cursor.Position.Y;

			System.Windows.Point mpos = e.GetPosition(null);
			// Create a rectangle using the DragSize, with the mouse position being
			// at the center of the rectangle.
			_dragBoxFromMouseDown = new Rectangle(
				new System.Drawing.Point(
					x - (dragSize.Width / 2),
					y - (dragSize.Height / 2)),
				dragSize);
		}

		private static void MouseUp(object sender, System.Windows.Input.MouseEventArgs e)
		{
			// Reset the drag rectangle when the mouse button is raised.
			_dragBoxFromMouseDown = Rectangle.Empty;
		}

		private static void MouseMove(object sender, System.Windows.Input.MouseEventArgs e)
		{
			if (e.LeftButton == System.Windows.Input.MouseButtonState.Pressed)
			{
				var x = System.Windows.Forms.Cursor.Position.X;
				var y = System.Windows.Forms.Cursor.Position.Y;

				// If the mouse moves outside the rectangle, start the drag.
				if (_dragBoxFromMouseDown != Rectangle.Empty &&
					!_dragBoxFromMouseDown.Contains(x, y))
				{
					_dragBoxFromMouseDown = Rectangle.Empty;
					{
						DataObject dataObj = new DataObject((sender as Button));
						System.Windows.DragDrop.DoDragDrop((sender as Button), dataObj, DragDropEffects.None);
						if (_newClickCoordinates.HasValue)
						{
							var result = MessageBox.Show(
								$"Save new click destination {_newClickCoordinates.Value}",
								"Mouse Click Action",
								MessageBoxButton.OKCancel);

							if (result == MessageBoxResult.OK)
							{
								var key = (sender as Button).Name;
								LayoutManager.UpdateClickActionCoordinatesInCurrentLayoutFile(key, _newClickCoordinates.Value);
							}

							_newClickCoordinates = null;
						}
					}
				}
			}
		}

		private static void GiveFeedback(object sender, GiveFeedbackEventArgs e)
		{
			System.Windows.Input.Mouse.SetCursor(System.Windows.Input.Cursors.Cross);
			e.Handled = true;
		}

		private static void QueryContinueDrag(object sender, QueryContinueDragEventArgs e)
		{
			if (e.KeyStates == DragDropKeyStates.None)
			{
				var pointOnScreen = System.Windows.Forms.Cursor.Position;
				var pointInMainWindow = Application.Current.MainWindow.PointFromScreen(
					new System.Windows.Point(pointOnScreen.X, pointOnScreen.Y));
				var rs = Application.Current.MainWindow.RenderSize;
				var dropDestinationInsideOfMainWindow =
					(pointInMainWindow.X > 0 && pointInMainWindow.X < rs.Width) &&
					(pointInMainWindow.Y > 0 && pointInMainWindow.Y < rs.Height);

				if (!dropDestinationInsideOfMainWindow)
				{
					_newClickCoordinates = pointOnScreen;
				}
			}
		}
	}
}
