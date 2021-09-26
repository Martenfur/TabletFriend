using System.Numerics;

namespace TabletFriend.Models
{
	public static class ModelConverter
	{
		public static Vector2 ConvertVector2(string str)
		{
			var parts = str.Split(",");
			return new Vector2(int.Parse(parts[0]), int.Parse(parts[1]));
		}
	}
}
