﻿
namespace TabletFriend.Data
{
	public class ThemeData
	{
		public string Rounding;
		public string DefaultStyle;

		public string PrimaryColor;
		public string SecondaryColor;
		public string BackgroundColor;

		public string DefaultFont;
		public int? DefaultFontSize;
		public int? DefaultFontWeight;

		/// <summary>
		/// Merges another theme into the current theme.
		/// </summary>
		public void Merge(ThemeData data)
		{
			Rounding = Rounding ?? data.Rounding;
			DefaultStyle = DefaultStyle ?? data.DefaultStyle;

			PrimaryColor = PrimaryColor ?? data.PrimaryColor;
			SecondaryColor = SecondaryColor ?? data.SecondaryColor;
			BackgroundColor = BackgroundColor ?? data.BackgroundColor;

			DefaultFont = DefaultFont ?? data.DefaultFont;
			DefaultFontSize = DefaultFontSize ?? data.DefaultFontSize;
			DefaultFontWeight = DefaultFontWeight ?? data.DefaultFontWeight;
		}
	}
}
