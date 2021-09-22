using MaterialDesignThemes.Wpf;
using System;
using System.Numerics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using TabletFriend.Models;
using WpfAppBar;

namespace TabletFriend
{
	public static class UiFactory
	{
		public static void CreateUi(LayoutModel layout, MainWindow window)
		{
			var theme = layout.Theme;

			window.MainCanvas.Children.Clear();
			window.MainBorder.CornerRadius = new CornerRadius(theme.Rounding);

			var sizes = layout.Buttons.GetSizes();
			var positions = Packer.Pack(sizes, layout.LayoutWidth);

			var size = Packer.GetSize(positions, sizes);

			if (AppState.Settings.DockingMode != DockingMode.None)
			{
				var layoutVertical = size.Y > size.X;
				var dockingVertical = AppState.Settings.DockingMode == DockingMode.Left
					|| AppState.Settings.DockingMode == DockingMode.Right;

				if (layoutVertical != dockingVertical)
				{
					if (layoutVertical)
					{
						// Converting vertical toolbars to horizontal.
						positions = Packer.Pack(sizes, 999);
					}
					else
					{
						// Converting horizontal toolbars to vertical.
						positions = Packer.Pack(sizes, 2);
					}
					size = Packer.GetSize(positions, sizes);
				}
			}

			window.MaxWidth = double.PositiveInfinity;
			window.MaxHeight = double.PositiveInfinity;

			window.Width = size.X * theme.CellSize + theme.Margin;
			window.Height = size.Y * theme.CellSize + theme.Margin;

			window.Opacity = theme.Opacity;

			Application.Current.Resources["PrimaryHueMidBrush"] = new SolidColorBrush(theme.PrimaryColor);
			Application.Current.Resources["PrimaryHueMidForegroundBrush"] = new SolidColorBrush(theme.SecondaryColor);
			Application.Current.Resources["MaterialDesignToolForeground"] = new SolidColorBrush(theme.SecondaryColor);
	
			Application.Current.Resources["MaterialDesignPaper"] = new SolidColorBrush(theme.BackgroundColor);
			Application.Current.Resources["MaterialDesignFont"] = new SolidColorBrush(theme.SecondaryColor);
			Application.Current.Resources["MaterialDesignBody"] = new SolidColorBrush(theme.SecondaryColor);

			window.MainBorder.Background = new SolidColorBrush(theme.BackgroundColor);

			if (window.Width < window.Height)
			{
				window.MaxWidth = window.Width;
			}
			else
			{
				window.MaxHeight = window.Height;
			}

			for (var i = 0; i < positions.Length; i += 1)
			{
				var button = layout.Buttons[i];
				if (button.Spacer)
				{
					continue;
				}
				CreateButton(layout, window, button, positions[i], sizes[i]);
			}
		}

		private static void CreateButton(LayoutModel layout, MainWindow window, ButtonModel button, Vector2 position, Vector2 size)
		{
			var theme = layout.Theme;

			var uiButton = new Button();
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
			}

			var style = button.Style;
			if (style == null)
			{
				style = theme.DefaultStyle;
			}

			if (style == null)
			{
				uiButton.Style = null;
			}
			else
			{
				uiButton.Style = Application.Current.Resources[style] as Style;
			}

			if (button.Action != null)
			{
				uiButton.Click += (e, o) => _ = button.Action.Invoke();
			}

			Canvas.SetTop(uiButton, theme.CellSize * position.Y + theme.Margin);
			Canvas.SetLeft(uiButton, theme.CellSize * position.X + theme.Margin);
			window.MainCanvas.Children.Add(uiButton);
		}
	}
}
