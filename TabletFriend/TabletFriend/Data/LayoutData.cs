using System.Collections.Generic;

namespace TabletFriend.Data
{
	public class LayoutData
	{
		public int LayoutWidth;
		public int ButtonSize;
		public int Margin;
		public string WindowRounding;
		public string WindowColor;

		public Dictionary<string, ButtonData> Buttons;
	}
}
