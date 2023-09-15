using System.Globalization;
using System.Windows.Media;
using TabletFriend.Data;

namespace TabletFriend.Models
{
	public class ThemeModel
	{
		
		public double Rounding = 16;
		
		public string DefaultStyle = "default";

		
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
