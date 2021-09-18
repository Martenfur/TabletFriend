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

		public int ButtonSize = 40;
		public int Margin = 8;

		public double Rounding = 8;
		
		public double Opacity = 0.8;

		public string DefaultStyle = "default";

		public int CellSize => ButtonSize + Margin;

		public Color PrimaryColor = Utils.StringToColor("#f48fb1");
		public Color SecondaryColor = Utils.StringToColor("#fff0ff");
		public Color BackgroundColor = Utils.StringToColor("#212531");

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
			if (data.Opacity != null)
			{
				Opacity = double.Parse(data.Opacity, CultureInfo.InvariantCulture);
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

			if (data.DefaultStyle != null)
			{
				DefaultStyle = data.DefaultStyle;
			}

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
