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
		public const int MinButtonSize = 8;
		public const int MinMargin = 0;

		public int LayoutWidth = MinLayoutWidth;
		public int ButtonSize = MinButtonSize;
		public int Margin = MinMargin;

		public double WindowRounding = 2;
		public Color WindowColor = Colors.White;

		public string DefaultStyle;

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

			if (data.WindowRounding != null)
			{
				WindowRounding = double.Parse(data.WindowRounding, CultureInfo.InvariantCulture);
			}

			if (data.WindowColor != null)
			{
				WindowColor = (Color)ColorConverter.ConvertFromString(data.WindowColor);
			}

			DefaultStyle = data.DefaultStyle;

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
