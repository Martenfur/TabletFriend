using System.Globalization;
using System.Windows.Media;
using TabletFriend.Data;

namespace TabletFriend.Models
{
	public class ThemeModel
	{
		public const int MinButtonSize = 8;
		public const int MinMargin = 0;

		public int ButtonSize = 32;
		public int Margin = 8;

		public double Rounding = 16;
		
		public double MinOpacity = 0.2;
		public double MaxOpacity = 0.9;

		public string DefaultStyle = "default";

		public int CellSize => ButtonSize + Margin;

		public Color PrimaryColor = Utils.StringToColor("#847979");
		public Color SecondaryColor = Utils.StringToColor("#f7f0f5");
		public Color BackgroundColor = Utils.StringToColor("#323031");

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
			if (data.MinOpacity != null)
			{
				MinOpacity = double.Parse(data.MinOpacity, CultureInfo.InvariantCulture);
			}
			if (data.MaxOpacity != null)
			{
				MaxOpacity = double.Parse(data.MaxOpacity, CultureInfo.InvariantCulture);
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
