using System;
using System.Collections.Generic;
using TabletFriend.Data;

namespace TabletFriend.Models
{
	public class LayoutModel : IDisposable
	{
		public const int MinLayoutWidth = 1;
		public int LayoutWidth = MinLayoutWidth;

		public List<ButtonModel> Buttons = new List<ButtonModel>();


		public LayoutModel(LayoutData data)
		{
			LayoutWidth = data.LayoutWidth;
			if (LayoutWidth < MinLayoutWidth)
			{
				LayoutWidth = MinLayoutWidth;
			}

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
