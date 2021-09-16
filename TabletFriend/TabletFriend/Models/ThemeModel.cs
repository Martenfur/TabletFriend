using System.Globalization;
using System.Windows.Media;
using TabletFriend.Data;

namespace TabletFriend.Models
{
	public class ThemeModel
	{
		public const int MinButtonSize = 8;
		public const int MinMargin = 4;

		public int ButtonSize = 48;
		public int Margin = MinMargin;

		public double Rounding = 4;

		public string DefaultStyle = "default";

		public int CellSize => ButtonSize + Margin;

		public Color PrimaryColor = Colors.White;
		public Color SecondaryColor = Colors.White;
		public Color BackgroundColor = Colors.White;

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
		}
	}
}
