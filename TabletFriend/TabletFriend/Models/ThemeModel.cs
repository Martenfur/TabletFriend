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

		public double WindowRounding = 4;
		public Color WindowColor = Colors.White;

		public string DefaultStyle = "default";

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
