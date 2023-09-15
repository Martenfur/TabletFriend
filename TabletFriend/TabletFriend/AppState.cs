using System;
using System.IO;
using TabletFriend.Models;

namespace TabletFriend
{
	public static class AppState
	{
		public static readonly string CurrentDirectory = AppDomain.CurrentDomain.BaseDirectory;

		public static readonly string FilesRoot = Path.Combine(CurrentDirectory, "files");
		public static readonly string LayoutsRoot = Path.Combine(CurrentDirectory, "files\\layouts");
		public static readonly string ThemesRoot = Path.Combine(CurrentDirectory, "files\\themes");

		public const string LayoutExtension = "*.yaml";

		public static string[] Layouts;
		public static string[] Themes;

		public static LayoutModel CurrentLayout;
		public static string CurrentLayoutPath;

		public static ThemeModel CurrentTheme;
		public static string CurrentThemePath;

		public static Settings Settings;
	}
}
