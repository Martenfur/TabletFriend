using System.Collections.Generic;
using System.Numerics;
using System.Windows.Media;
using TabletFriend.Models;
using WpfAppBar;

namespace TabletFriend
{
	public static class Utils
	{
		/// <summary>
		/// Returns the size array of button models.
		/// </summary>
		public static Vector2[] GetSizes(this List<ButtonModel> buttons, DockingMode docking)
		{
			var sizes = new List<Vector2>();

			foreach (var button in buttons)
			{
				if (button.IsVisible(docking))
				{
					sizes.Add(button.Size);
				}
			}
			return sizes.ToArray();
		}

		public static bool IsVisible(this ButtonModel button, DockingMode docking)
		{
			return button.Visibility == ButtonVisibility.Always
				|| (button.Visibility == ButtonVisibility.Docked && docking != DockingMode.None)
				|| (button.Visibility == ButtonVisibility.Undocked && docking == DockingMode.None)
				|| (button.Visibility == ButtonVisibility.Docked_Left && docking == DockingMode.Left)
				|| (button.Visibility == ButtonVisibility.Docked_Right && docking == DockingMode.Right)
				|| (button.Visibility == ButtonVisibility.Docked_Top && docking == DockingMode.Top);
		}

		public static Color StringToColor(string hexColor)
		{
			// TODO: Change the format from ARGB to RGBA.
			return (Color)ColorConverter.ConvertFromString(hexColor);
		}
	}
}
