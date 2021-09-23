using System;
using System.Diagnostics;
using System.IO;
using System.Windows;

namespace TabletFriend
{
	public static class Installer
	{
		private static readonly string _preferredDirectory =
			Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "TabletFriend");

		public static void TryInstall()
		{
			if (!AppState.Settings.FirstLaunch)
			{
				return;
			}
			if (AppState.CurrentDirectory.TrimEnd('\\') == _preferredDirectory.TrimEnd('\\'))
			{
				return;
			}
			var result = MessageBox.Show(
				"Welcome to Tablet Friend! It seems that you are running from a regular directory. "
				+ "Would you like Tablet Friend to move itself to AppData?",
				"Hi!",
				MessageBoxButton.YesNo,
				MessageBoxImage.Question
			);
			if (result == MessageBoxResult.Yes)
			{
				DirectoryCopy(AppState.CurrentDirectory, _preferredDirectory);
				Process.Start(Path.Combine(_preferredDirectory, "TabletFriend.exe"));
				Application.Current.Shutdown();
			}
		}


		private static void DirectoryCopy(string sourceDirName, string destDirName)
		{
			// Get the subdirectories for the specified directory.
			DirectoryInfo dir = new DirectoryInfo(sourceDirName);

			if (!dir.Exists)
			{
				throw new DirectoryNotFoundException(
					"Source directory does not exist or could not be found: "
					+ sourceDirName);
			}

			DirectoryInfo[] dirs = dir.GetDirectories();

			// If the destination directory doesn't exist, create it.       
			Directory.CreateDirectory(destDirName);

			// Get the files in the directory and copy them to the new location.
			FileInfo[] files = dir.GetFiles();
			foreach (FileInfo file in files)
			{
				string tempPath = Path.Combine(destDirName, file.Name);
				file.CopyTo(tempPath, true);
			}


			foreach (DirectoryInfo subdir in dirs)
			{
				string tempPath = Path.Combine(destDirName, subdir.Name);
				DirectoryCopy(subdir.FullName, tempPath);
			}
		}
	}
}
