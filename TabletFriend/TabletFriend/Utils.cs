using System.Collections.Generic;
using System.Numerics;
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


		private static readonly Dictionary<string, string> _styleTranslationTable = new Dictionary<string, string>()
		{
			{ "none", null },
			{ "default", "MaterialDesignFlatButton" },
			{ "accent", "MaterialDesignRaisedAccentButton" },
			{ "circular_accent", "MaterialDesignFloatingActionMiniButton" },
			{ "outlined", "MaterialDesignOutlinedButton" },
			{ "circular", "MaterialDesignIconButton" },
			{ "shy", "MaterialDesignToolButton" },
		};

		/// <summary>
		/// Translates friendly style name to the one used by msterisl design internally.
		/// </summary>
		public static string TranslateFriendlyStyleName(string style)
		{
			if (style != null && _styleTranslationTable.TryGetValue(style, out var styleName))
			{
				return styleName;
			}
			return style;
		}
	}
}
