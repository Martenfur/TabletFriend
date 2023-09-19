using System;
using System.Collections.Generic;
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

		public const string ConfigExtension = "*.yaml";

		public static Dictionary<string, LayoutModel> Layouts;
		public static Dictionary<string, ThemeModel> Themes;

		public static LayoutModel CurrentLayout;
		public static string CurrentLayoutName;

		public static ThemeModel CurrentTheme;
		public static string CurrentThemeName;

		public static Settings Settings;
	}
}
