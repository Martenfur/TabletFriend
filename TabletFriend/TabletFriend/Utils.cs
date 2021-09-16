using System.Collections.Generic;
using System.Numerics;
using System.Windows.Media;
using TabletFriend.Models;

namespace TabletFriend
{
	public static class Utils
	{
		/// <summary>
		/// Returns the size array of button models.
		/// </summary>
		public static Vector2[] GetSizes(this List<ButtonModel> buttons)
		{
			var sizes = new Vector2[buttons.Count];
			var i = 0;
			foreach (var button in buttons)
			{
				sizes[i] = button.Size;
				i += 1;
			}
			return sizes;
		}


		public static Color StringToColor(string hexColor)
		{
			// TODO: Change the format from ARGB to RGBA.
			return (Color)ColorConverter.ConvertFromString(hexColor);
		}
	}
}
