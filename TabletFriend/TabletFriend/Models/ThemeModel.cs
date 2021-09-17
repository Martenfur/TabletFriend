using System;
using System.Globalization;
using System.Windows.Media;
using TabletFriend.Data;

namespace TabletFriend.Models
{
	public class ThemeModel
	{
		public const int MinButtonSize = 8;
		public const int MinMargin = 0;

		public int ButtonSize = 48;
		public int Margin = 4;

		public double Rounding = 4;

		public string DefaultStyle = "default";

		public int CellSize => ButtonSize + Margin;

		public Color PrimaryColor = Colors.White;
		public Color SecondaryColor = Colors.White;
		public Color BackgroundColor = Colors.White;

		public string DefaultFont;
		public int DefaultFontSize;
		public int DefaultFontWeight;

		public ThemeModel(ThemeData data)
		{
			if (data == null)
			{
				return;
			}

			if (data.ButtonSize != null)
			{
				ButtonSize = data.ButtonSize.Value;
			}
			if (ButtonSize < MinButtonSize)
			{
				ButtonSize = MinButtonSize;
			}

			if (data.Margin != null)
			{
				Margin = data.Margin.Value;
			}
			if (Margin < MinMargin)
			{
				Margin = MinMargin;
			}

			if (data.Rounding != null)
			{
				Rounding = double.Parse(data.Rounding, CultureInfo.InvariantCulture);
			}

			if (data.PrimaryColor != null)
			{
				PrimaryColor = Utils.StringToColor(data.PrimaryColor);
			}
			if (data.SecondaryColor != null)
			{
				SecondaryColor = Utils.StringToColor(data.SecondaryColor);
			}
			if (data.BackgroundColor != null)
			{
				BackgroundColor = Utils.StringToColor(data.BackgroundColor);
			}

			DefaultStyle = data.DefaultStyle;

			DefaultFont = data.DefaultFont;
			if (data.DefaultFontSize != null)
			{
				DefaultFontSize = data.DefaultFontSize.Value;
			}
			if (data.DefaultFontWeight != null)
			{
				DefaultFontWeight = data.DefaultFontWeight.Value;
			}
		}
	}
}
