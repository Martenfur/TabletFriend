using System.Collections.Generic;
using System.Numerics;
using System.Windows;
using System.Windows.Controls;
using TabletFriend.Data;

namespace TabletFriend.Models
{
	public class LayoutModel
	{
		public const int MinLayoutWidth = 1;
		public const int MinButtonSize = 8;

		public int LayoutWidth = MinLayoutWidth;
		public int ButtonSize = MinButtonSize;

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

			foreach(var button in data.Buttons)
			{
				Buttons.Add(new ButtonModel(button.Value));
			}
		}


		public void Create(Canvas canvas, Window window)
		{
			var sizes = GetSizeArray();
			var positions = Packer.Pack(sizes, LayoutWidth);

			var size = Packer.GetSize(positions, sizes);


			window.Width = size.X * ButtonSize;
			window.Height = size.Y * ButtonSize;

			for (var i = 0; i < positions.Length; i += 1)
			{
				if (Buttons[i].Spacer)
				{
					continue;
				}

				var button = new Button()
				{
					Width = ButtonSize * sizes[i].X,
					Height = ButtonSize * sizes[i].Y,
					Content = Buttons[i].Text,
				};

				Canvas.SetTop(button, ButtonSize * positions[i].Y);
				Canvas.SetLeft(button, ButtonSize * positions[i].X);
				canvas.Children.Add(button);
			}
		}


		private Vector2[] GetSizeArray()
		{
			var sizes = new Vector2[Buttons.Count];
			var i = 0;
			foreach(var button in Buttons)
			{
				sizes[i] = button.Size;
				i += 1;
			}
			return sizes;
		}
	}
}
