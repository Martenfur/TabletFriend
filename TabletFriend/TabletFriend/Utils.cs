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
		public static Vector2[] GetSizes(this List<ButtonModel> buttons, bool isDocked)
		{
			var sizes = new List<Vector2>();

			foreach (var button in buttons)
			{
				if (button.IsVisible(isDocked))
				{
					sizes.Add(button.Size);
				}
			}
			return sizes.ToArray();
		}

		public static bool IsVisible(this ButtonModel button, bool isDocked)
		{
			return button.Visibility == ButtonVisibility.Always
				|| (button.Visibility == ButtonVisibility.Docked && isDocked)
				|| (button.Visibility == ButtonVisibility.Undocked && !isDocked);
		}

		public static Color StringToColor(string hexColor)
		{
			// TODO: Change the format from ARGB to RGBA.
			return (Color)ColorConverter.ConvertFromString(hexColor);
		}
	}
}
