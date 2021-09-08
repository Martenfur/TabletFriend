using System.Collections.Generic;
using TabletFriend.Data;

namespace TabletFriend.Models
{
	public class LayoutModel
	{
		public const int MinLayoutWidth = 1;
		public const int MinButtonSize = 8;

		public int LayoutWidth = MinLayoutWidth;
		public int ButtonSize = MinButtonSize;

		public Dictionary<string, ButtonModel> Buttons = new Dictionary<string, ButtonModel>();

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
				Buttons.Add(button.Key, new ButtonModel(button.Value));
			}
		}
	}
}
