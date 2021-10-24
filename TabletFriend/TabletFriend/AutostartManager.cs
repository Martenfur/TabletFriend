using Microsoft.Win32;
using System.IO;
using System.Reflection;

namespace TabletFriend
{
	public class AutostartManager
	{
		private const string _key = "TabletFriend";

		private static readonly string _appPath =
			'"' + Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, "TabletFriend.exe") + '"';

		public static bool IsAutostartSet => (string)GetKey().GetValue(_key) == _appPath;

		public static void SetAutostart()
		{
			if (!IsAutostartSet)
			{
				GetKey().SetValue(_key, _appPath);
			}
		}

		public static void ResetAutostart()
		{
			if (IsAutostartSet)
			{
				GetKey().DeleteValue(_key);
			}
		}

		private static RegistryKey GetKey() =>
			Registry.CurrentUser.OpenSubKey("Software\\Microsoft\\Windows\\CurrentVersion\\Run", true);
	}
}
