using System;
using System.Collections.Generic;
using System.Numerics;
using System.Windows;
using System.Windows.Controls;
using TabletFriend.Data;

namespace TabletFriend.Models
{
	public class LayoutModel : IDisposable
	{
		public const int MinLayoutWidth = 1;
		public const int MinButtonSize = 8;
		public const int MinMargin = 0;

		public int LayoutWidth = MinLayoutWidth;
		public int ButtonSize = MinButtonSize;
		public int Margin = MinMargin;

		public int CellSize => ButtonSize + Margin;

		public List<ButtonModel> Buttons = new List<ButtonModel>();



		public LayoutModel(LayoutData data)
		{
			LayoutWidth = data.LayoutWidth;
			if (LayoutWidth < MinLayoutWidth)
			{
				LayoutWidth = MinLayoutWidth;
			}

			ButtonSize = data.ButtonSize;
			if (ButtonSize < MinButtonSize)
			{
				ButtonSize = MinButtonSize;
			}

			Margin = data.Margin;
			if (Margin < MinMargin)
			{
				Margin = MinMargin;
			}

			foreach (var button in data.Buttons)
			{
				Buttons.Add(new ButtonModel(button.Value));
			}
		}


		public void CreateUI(Canvas canvas, Window window)
		{
			canvas.Children.Clear();

			var sizes = GetSizeArray();
			var positions = Packer.Pack(sizes, LayoutWidth);

			var size = Packer.GetSize(positions, sizes);

			window.MaxWidth = double.PositiveInfinity;
			window.MaxHeight = double.PositiveInfinity;

			window.Width = size.X * CellSize + Margin;
			window.Height = size.Y * CellSize + Margin;

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
				var button = Buttons[i];
				if (button.Spacer)
				{
					continue;
				}

				var uiButton = new Button()
				{
					Width = CellSize * sizes[i].X - Margin,
					Height = CellSize * sizes[i].Y - Margin,
					Content = button.Text,
					Opacity = 0.7
				};
				if (button.Icon != null)
				{
					uiButton.Content = button.Icon;
				}
				if (button.Action != null)
				{
					uiButton.Click += (e, o) => _ = button.Action.Invoke();
				}


				Canvas.SetTop(uiButton, CellSize * positions[i].Y + Margin);
				Canvas.SetLeft(uiButton, CellSize * positions[i].X + Margin);
				canvas.Children.Add(uiButton);
			}
		}

		public void Dispose()
		{
			foreach (var button in Buttons)
			{
				button.Dispose();
			}
		}

		private Vector2[] GetSizeArray()
		{
			var sizes = new Vector2[Buttons.Count];
			var i = 0;
			foreach (var button in Buttons)
			{
				sizes[i] = button.Size;
				i += 1;
			}
			return sizes;
		}
	}
}
