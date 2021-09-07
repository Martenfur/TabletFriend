using System;
using System.Numerics;

namespace TabletFriend
{
	public class LayoutCreator
	{
		/// <summary>
		/// Packs rectangle of given sizes into a column of given width. Returns an array of rectangle positions.
		/// </summary>
		public static Vector2[] Pack(Vector2[] sizes, int width)
		{
			var height = 0;
			foreach (var size in sizes)
			{
				height += (int)size.Y;
				if (size.X > width)
				{
					width = (int)size.X;
				}
			}
			var grid = new int[width, height];

			var positions = new Vector2[sizes.Length];

			var cursor = Vector2.Zero;

			for (var i = 0; i < sizes.Length; i += 1)
			{
				while (CheckCollision(cursor, sizes[i], grid))
				{
					cursor.X += 1;
					if (cursor.X + sizes[i].X > width)
					{
						cursor.X = 0;
						cursor.Y += 1;
					}
				}
				Fill(cursor, sizes[i], grid, i + 1);
				positions[i] = cursor;
			}

			// TODO: Remove. Debug only.
			var res = "";
			for (var y = 0; y < grid.GetLength(1); y += 1)
			{
				for (var x = 0; x < grid.GetLength(0); x += 1)
				{
					res += grid[(int)x, y] + ",";
				}
				res += Environment.NewLine;
			}


			return positions;
		}

		
		private static bool CheckCollision(Vector2 position, Vector2 size, int[,] grid)
		{
			if (position.X + size.X > grid.GetLength(0))
			{
				return true;
			}
			for (var y = position.Y; y < position.Y + size.Y; y += 1)
			{
				for (var x = position.X; x < position.X + size.X; x += 1)
				{
					if (grid[(int)x, (int)y] != 0)
					{
						return true;
					}
				}
			}

			return false;
		}

		private static void Fill(Vector2 position, Vector2 size, int[,] grid, int i)
		{
			for (var y = position.Y; y < position.Y + size.Y; y += 1)
			{
				for (var x = position.X; x < position.X + size.X; x += 1)
				{
					grid[(int)x, (int)y] = i;
				}
			}
		}
	}
}
