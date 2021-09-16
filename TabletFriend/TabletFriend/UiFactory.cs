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
			window.MainCanvas.Children.Clear();
			window.MainBorder.CornerRadius = new CornerRadius(layout.WindowRounding);

			var sizes = layout.Buttons.GetSizes();
			var positions = Packer.Pack(sizes, layout.LayoutWidth);

			var size = Packer.GetSize(positions, sizes);

			window.MaxWidth = double.PositiveInfinity;
			window.MaxHeight = double.PositiveInfinity;

			window.Width = size.X * layout.CellSize + layout.Margin;
			window.Height = size.Y * layout.CellSize + layout.Margin;

			window.MainBorder.Background = new SolidColorBrush(layout.WindowColor);

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
			var uiButton = new Button();
			uiButton.Width = layout.CellSize * size.X - layout.Margin;
			uiButton.Height = layout.CellSize * size.Y - layout.Margin;
			uiButton.Content = button.Text;

			var style = button.Style;
			if (style == null)
			{
				style = layout.DefaultStyle;
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

			Canvas.SetTop(uiButton, layout.CellSize * position.Y + layout.Margin);
			Canvas.SetLeft(uiButton, layout.CellSize * position.X + layout.Margin);
			window.MainCanvas.Children.Add(uiButton);
		}
	}
}
