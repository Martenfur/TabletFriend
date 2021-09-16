using System.Numerics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using TabletFriend.Models;

namespace TabletFriend
{
	public static class UiFactory
	{
		public static void CreateUI(LayoutModel layout, MainWindow window)
		{
			var theme = layout.Theme;

			window.MainCanvas.Children.Clear();
			window.MainBorder.CornerRadius = new CornerRadius(theme.WindowRounding);

			var sizes = layout.Buttons.GetSizes();
			var positions = Packer.Pack(sizes, layout.LayoutWidth);

			var size = Packer.GetSize(positions, sizes);

			window.MaxWidth = double.PositiveInfinity;
			window.MaxHeight = double.PositiveInfinity;

			window.Width = size.X * theme.CellSize + theme.Margin;
			window.Height = size.Y * theme.CellSize + theme.Margin;

			window.MainBorder.Background = new SolidColorBrush(theme.WindowColor);

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
				CreateUIButton(layout, window, button, positions[i], sizes[i]);
			}
		}

		public static void CreateUIButton(LayoutModel layout, MainWindow window, ButtonModel button, Vector2 position, Vector2 size)
		{
			var theme = layout.Theme;

			var uiButton = new Button();
			uiButton.Width = theme.CellSize * size.X - theme.Margin;
			uiButton.Height = theme.CellSize * size.Y - theme.Margin;
			uiButton.Content = button.Text;

			var style = button.Style;
			if (style == null)
			{
				style = theme.DefaultStyle;
			}
			style = Utils.TranslateFriendlyStyleName(style);

			if (style == null)
			{
				uiButton.Style = null;
			}
			else
			{
				uiButton.Style = Application.Current.Resources[style] as Style;
			}

			if (button.Icon != null)
			{
				uiButton.Content = button.Icon;
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
