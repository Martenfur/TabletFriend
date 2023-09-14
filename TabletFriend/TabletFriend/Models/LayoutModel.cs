using System;
using System.Collections.Generic;
using TabletFriend.Data;

namespace TabletFriend.Models
{
	public class LayoutModel : IDisposable
	{
		public const int MinLayoutWidth = 1;

		public int ButtonSize = 32;
		public int Margin = 8;

		public int LayoutWidth = MinLayoutWidth;

		public List<ButtonModel> Buttons = new List<ButtonModel>();

		public const int MinButtonSize = 8;
		public const int MinMargin = 0;

		public int CellSize => ButtonSize + Margin;

		public LayoutModel(LayoutData data)
		{
			ButtonSize = Math.Max(data.ButtonSize ?? ButtonSize, MinButtonSize);
			Margin = Math.Max(data.Margin ?? Margin, MinMargin);
			LayoutWidth = Math.Max(data.LayoutWidth, MinLayoutWidth);
			
			foreach (var button in data.Buttons)
			{
				Buttons.Add(new ButtonModel(button.Value));
			}
		}


		public void Dispose()
		{
			foreach (var button in Buttons)
			{
				button.Dispose();
			}
		}

	}
}
