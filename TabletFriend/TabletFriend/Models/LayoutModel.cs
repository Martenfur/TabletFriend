using System;
using System.Collections.Generic;
using System.Globalization;
using System.Numerics;
using System.Windows.Media;
using TabletFriend.Data;

namespace TabletFriend.Models
{
	public class LayoutModel : IDisposable
	{
		public const int MinLayoutWidth = 1;
		public int LayoutWidth = MinLayoutWidth;

		public ThemeModel Theme;

		public List<ButtonModel> Buttons = new List<ButtonModel>();


		public LayoutModel(LayoutData data)
		{
			LayoutWidth = data.LayoutWidth;
			if (LayoutWidth < MinLayoutWidth)
			{
				LayoutWidth = MinLayoutWidth;
			}

			Theme = new ThemeModel(data.Theme);
			
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
