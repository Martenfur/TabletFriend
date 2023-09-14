using MaterialDesignThemes.Wpf;
using System;
using System.Collections.Generic;
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
		public static void CreateUi(LayoutModel layout, MainWindow window)
		{
			ToggleManager.ClearButtons();
			var theme = AppState.CurrentTheme;

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

			var titlebarHeight = TitlebarManager.GetTitlebarHeight(layout);

			if (rotateLayout)
			{
				window.Height = size.X * layout.CellSize + layout.Margin + titlebarHeight;
				window.Width = size.Y * layout.CellSize + layout.Margin;
			}
			else
			{
				window.Width = size.X * layout.CellSize + layout.Margin;
				window.Height = size.Y * layout.CellSize + layout.Margin + titlebarHeight;
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


			TitlebarManager.CreateTitlebar(window, theme, layout);
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
			var theme = AppState.CurrentTheme;

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
			uiButton.Width = layout.CellSize * size.X - layout.Margin;
			uiButton.Height = layout.CellSize * size.Y - layout.Margin;

			var font = button.Font;
			if (font == null)
			{
				font = AppState.CurrentTheme.DefaultFont;
			}
			var fontSize = button.FontSize;
			if (fontSize == 0)
			{
				fontSize = AppState.CurrentTheme.DefaultFontSize;
			}
			var fontWeight = button.FontWeight;
			if (fontWeight == 0)
			{
				fontWeight = AppState.CurrentTheme.DefaultFontWeight;
			}

			var text = new TextBlock();
			text.Text = button.Text;
			if (fontSize > 0)
			{
				text.FontSize = fontSize;
			}
			if (font != null)
			{
				text.FontFamily = new FontFamily(font);
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
			}

			Canvas.SetTop(uiButton, layout.CellSize * position.Y + layout.Margin + offset.Y);
			Canvas.SetLeft(uiButton, layout.CellSize * position.X + layout.Margin + offset.X);
			window.MainCanvas.Children.Add(uiButton);
		}
	}
}
