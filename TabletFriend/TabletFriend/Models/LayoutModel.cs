using System;
using System.Collections.Generic;
using System.Globalization;
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
		public double MinOpacity = 0.2;
		public double MaxOpacity = 0.9;

		public int CellSize => ButtonSize + Margin;
		
		public string App;

		public LayoutModel(LayoutData data)
		{
			ButtonSize = Math.Max(data.ButtonSize ?? ButtonSize, MinButtonSize);
			Margin = Math.Max(data.Margin ?? Margin, MinMargin);
			LayoutWidth = Math.Max(data.LayoutWidth, MinLayoutWidth);
			App = data.App;

			if (data.MinOpacity != null)
			{
				MinOpacity = double.Parse(data.MinOpacity, CultureInfo.InvariantCulture);
			}
			if (data.MaxOpacity != null)
			{
				MaxOpacity = double.Parse(data.MaxOpacity, CultureInfo.InvariantCulture);
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
