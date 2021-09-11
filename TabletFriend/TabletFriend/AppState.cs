using System;
using System.IO;
using TabletFriend.Models;

namespace TabletFriend
{
	public static class AppState
	{
		public static readonly string LayoutRoot = Path.Combine(Environment.CurrentDirectory, "layouts");

		public static string[] Layouts;

		public static LayoutModel CurrentLayout;

		public static string CurrentLayoutPath;
	}
}
