using System;
using System.Collections.Generic;
using System.Globalization;
using System.Numerics;
using System.Windows.Media;
using TabletFriend.Data;

namespace TabletFriend.Models
{
	public class ThemeModel
	{
		public const int MinButtonSize = 8;
		public const int MinMargin = 0;

		public int ButtonSize = MinButtonSize;
		public int Margin = MinMargin;

		public double WindowRounding = 2;
		public Color WindowColor = Colors.White;

		public string DefaultStyle;

		public int CellSize => ButtonSize + Margin;

		public ThemeModel(ThemeData data)
		{
			if (data == null)
			{
				return;
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
				WindowColor = Utils.StringToColor(data.WindowColor);
			}

			DefaultStyle = data.DefaultStyle;
		}
	}
}
