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
				var button = Buttons[i];
				if (button.Spacer)
				{
					continue;
				}

				var uiButton = new Button()
				{
					Width = ButtonSize * sizes[i].X,
					Height = ButtonSize * sizes[i].Y,
					Content = button.Text,
				};
				if (button.Action != null)
				{
					uiButton.Click += (e, o) => _ = button.Action.Invoke();
				}


				Canvas.SetTop(uiButton, ButtonSize * positions[i].Y);
				Canvas.SetLeft(uiButton, ButtonSize * positions[i].X);
				canvas.Children.Add(uiButton);
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
