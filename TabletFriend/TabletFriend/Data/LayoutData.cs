using System.Collections.Generic;

namespace TabletFriend.Data
{
	public class LayoutData
	{
		public int LayoutWidth;

		public int? ButtonSize;
		public int? Margin;
		public string MinOpacity;
		public string MaxOpacity;

		public string App;

		public Dictionary<string, ButtonData> Buttons;
	}
}
