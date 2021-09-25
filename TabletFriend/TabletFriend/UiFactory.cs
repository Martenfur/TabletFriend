using System;
using System.Collections.Generic;
using System.Drawing;
using System.Numerics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;
using TabletFriend.Actions;
using TabletFriend.Models;
using WpfAppBar;

namespace TabletFriend
{
	public static class UiFactory
	{
		private static System.Drawing.Point? _newClickCoordinates = null;
		private static Rectangle _dragBoxFromMouseDown;

		public static void CreateUi(LayoutModel layout, MainWindow window)
		{
			ToggleManager.ClearButtons();
			var theme = layout.Theme;

			window.MainCanvas.Children.Clear();

			var isDocked = AppState.Settings.DockingMode != DockingMode.None;

			if (!isDocked)
			{
				window.MainBorder.CornerRadius = new CornerRadius(theme.Rounding);
			}
			else
			{
				window.MainBorder.CornerRadius = new CornerRadius(0);
			}
			var sizes = layout.Buttons.GetSizes(isDocked);
			var positions = Packer.Pack(sizes, layout.LayoutWidth);

			var size = Packer.GetSize(positions, sizes);


			var rotateLayout = false;
			var layoutVertical = size.Y > size.X;
			if (AppState.Settings.DockingMode != DockingMode.None)
			{
				var dockingVertical = AppState.Settings.DockingMode == DockingMode.Left
					|| AppState.Settings.DockingMode == DockingMode.Right;

				if (layoutVertical != dockingVertical)
				{
					rotateLayout = true;
				}
			}

			var titlebarHeight = TitlebarManager.GetTitlebarHeight(theme);

			if (rotateLayout)
			{
				window.Height = size.X * theme.CellSize + theme.Margin + titlebarHeight;
				window.Width = size.Y * theme.CellSize + theme.Margin;
			}
			else
			{
				window.Width = size.X * theme.CellSize + theme.Margin;
				window.Height = size.Y * theme.CellSize + theme.Margin + titlebarHeight;
			}

			var offset = Vector2.Zero;
			if (AppState.Settings.DockingMode != DockingMode.None)
			{
				if (AppState.Settings.DockingMode == DockingMode.Top || AppState.Settings.DockingMode == DockingMode.Bottom)
				{
					offset.X = (float)(SystemParameters.PrimaryScreenWidth - window.Width) / 2;
				}
				else
				{
					offset.Y = (float)(SystemParameters.PrimaryScreenHeight - window.Height) / 2;
				}
			}
			else
			{
				offset.Y = (float)titlebarHeight;
			}

			if (AppState.Settings.DockingMode != DockingMode.None)
			{
				window.MinOpacity = theme.MaxOpacity;
			}
			else
			{
				window.MinOpacity = theme.MinOpacity;
			}
			window.MaxOpacity = theme.MaxOpacity;
			window.BeginAnimation(UIElement.OpacityProperty, null);
			window.Opacity = theme.MaxOpacity;
			if (window.IsMouseOver)
			{
				window.BeginAnimation(UIElement.OpacityProperty, window.FadeIn);
			}
			else
			{
				window.BeginAnimation(UIElement.OpacityProperty, window.FadeOut);
			}

			Application.Current.Resources["PrimaryHueMidBrush"] = new SolidColorBrush(theme.PrimaryColor);
			Application.Current.Resources["PrimaryHueMidForegroundBrush"] = new SolidColorBrush(theme.SecondaryColor);
			Application.Current.Resources["MaterialDesignToolForeground"] = new SolidColorBrush(theme.SecondaryColor);

			Application.Current.Resources["MaterialDesignPaper"] = new SolidColorBrush(theme.BackgroundColor);
			Application.Current.Resources["MaterialDesignFont"] = new SolidColorBrush(theme.SecondaryColor);
			Application.Current.Resources["MaterialDesignBody"] = new SolidColorBrush(theme.SecondaryColor);

			window.MainBorder.Background = new SolidColorBrush(theme.BackgroundColor);

			var visibleButtons = new List<ButtonModel>();

			foreach (var button in layout.Buttons)
			{
				if (button.IsVisible(isDocked))
				{
					visibleButtons.Add(button);
				}
			}

			for (var i = 0; i < positions.Length; i += 1)
			{
				var button = visibleButtons[i];


				if (button.Spacer)
				{
					continue;
				}
				var buttonPosition = positions[i];
				var buttonSize = sizes[i];

				if (rotateLayout)
				{
					var buffer = buttonPosition.X;
					buttonPosition.X = buttonPosition.Y;
					buttonPosition.Y = buffer;

					buffer = buttonSize.X;
					buttonSize.X = buttonSize.Y;
					buttonSize.Y = buffer;
				}

				CreateButton(layout, window, button, buttonPosition, buttonSize, offset);
			}


			TitlebarManager.CreateTitlebar(window, theme);
		}

		private static void CreateButton(
			LayoutModel layout,
			MainWindow window,
			ButtonModel button,
			Vector2 position,
			Vector2 size,
			Vector2 offset
		)
		{
			var theme = layout.Theme;

			ButtonBase uiButton;
			var isToggle = button.Action is ToggleAction;
			var isRepeat = button.Action is RepeatAction;

			if (isToggle)
			{
				uiButton = new ToggleButton();
			}
			else
			{
				if (isRepeat)
				{
					uiButton = new RepeatButton();
					Stylus.SetIsPressAndHoldEnabled(uiButton, false);
				}
				else
				{
					uiButton = new Button();
				}
			}
			uiButton.Width = theme.CellSize * size.X - theme.Margin;
			uiButton.Height = theme.CellSize * size.Y - theme.Margin;

			var font = button.Font;
			if (font == null)
			{
				font = layout.Theme.DefaultFont;
			}
			var fontSize = button.FontSize;
			if (fontSize == 0)
			{
				fontSize = layout.Theme.DefaultFontSize;
			}
			var fontWeight = button.FontWeight;
			if (fontWeight == 0)
			{
				fontWeight = layout.Theme.DefaultFontWeight;
			}

			var text = new TextBlock();
			text.Text = button.Text;
			if (fontSize > 0)
			{
				text.FontSize = fontSize;
			}
			if (font != null)
			{
				text.FontFamily = new System.Windows.Media.FontFamily(font);
			}
			if (fontWeight > 0)
			{
				text.FontWeight = FontWeight.FromOpenTypeWeight(Math.Min(999, fontWeight));
			}

			uiButton.Content = text;

			if (button.Icon != null)
			{
				uiButton.Content = button.Icon;
				if (!string.IsNullOrEmpty(button.Text))
				{
					uiButton.ToolTip = new ToolTip()
					{
						Style = Application.Current.Resources["tool_tip"] as Style,
						Content = button.Text,
						HasDropShadow = true,
					};
				}
			}

			var style = button.Style;
			if (style == null)
			{
				style = theme.DefaultStyle;
			}

			if (isToggle)
			{
				uiButton.Style = Application.Current.Resources["toggle"] as Style;

				var key = ((ToggleAction)button.Action).Key;
				var toggle = (ToggleButton)uiButton;
				if (ToggleManager.IsHeld(key))
				{
					toggle.IsChecked = true;
				}
				ToggleManager.AddButton(key, toggle);
			}
			else
			{
				if (style == null)
				{
					uiButton.Style = null;
				}
				else
				{
					uiButton.Style = Application.Current.Resources[style] as Style;
				}
			}

			if (button.Action != null)
			{
				uiButton.Click += (e, o) => _ = button.Action.Invoke();
				if (button.Action is ClickAction)
				{
					uiButton.Name = button.Key;
					uiButton.PreviewMouseLeftButtonUp += ClickActionButton_MouseUp;
					uiButton.PreviewMouseLeftButtonDown += ClickActionButton_MouseDown;
					uiButton.PreviewMouseMove += ClickActionButton_MouseMove;
					uiButton.PreviewQueryContinueDrag += ClickActionButton_QueryContinueDrag;
					uiButton.PreviewGiveFeedback += ClickActionButton_GiveFeedback;
				}
			}

			Canvas.SetTop(uiButton, theme.CellSize * position.Y + theme.Margin + offset.Y);
			Canvas.SetLeft(uiButton, theme.CellSize * position.X + theme.Margin + offset.X);
			window.MainCanvas.Children.Add(uiButton);
		}

		private static void ClickActionButton_MouseDown(object sender, System.Windows.Input.MouseEventArgs e)
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

        private static void ClickActionButton_MouseUp(object sender, System.Windows.Input.MouseEventArgs e)
		{
			// Reset the drag rectangle when the mouse button is raised.
			_dragBoxFromMouseDown = Rectangle.Empty;
		}

		private static void ClickActionButton_MouseMove(object sender, System.Windows.Input.MouseEventArgs e)
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
						DragDrop.DoDragDrop((sender as Button), dataObj, DragDropEffects.None);
						if(_newClickCoordinates.HasValue)
                        {
							var result = MessageBox.Show(
								$"Save new click destination {_newClickCoordinates.Value}",
								"Mouse Click Action",
								MessageBoxButton.OKCancel);

							if(result == MessageBoxResult.OK)
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

		private static void ClickActionButton_GiveFeedback(object sender, GiveFeedbackEventArgs e)
		{
			System.Windows.Input.Mouse.SetCursor(System.Windows.Input.Cursors.Cross);
			e.Handled = true;
		}

		private static void ClickActionButton_QueryContinueDrag(object sender, QueryContinueDragEventArgs e)
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
