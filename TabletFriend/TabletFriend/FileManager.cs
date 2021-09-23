using System;
using System.Diagnostics;
using System.IO;
using System.Timers;

namespace TabletFriend
{
	public class FileManager
	{
		private FileSystemWatcher _watcher;

		public FileManager()
		{
			_watcher = new FileSystemWatcher();
			_watcher.Path = AppState.FilesRoot;
			_watcher.NotifyFilter = NotifyFilters.FileName 
				| NotifyFilters.DirectoryName
				| NotifyFilters.Attributes
				| NotifyFilters.Size
				| NotifyFilters.LastWrite
				| NotifyFilters.LastAccess
				| NotifyFilters.CreationTime
				| NotifyFilters.Security;


			_watcher.Changed += OnChanged;
			_watcher.Created += OnChanged;
			_watcher.Deleted += OnChanged;
			_watcher.EnableRaisingEvents = true;
			_watcher.IncludeSubdirectories = true;

			RefreshLayoutList();
		}

		private void OnElapsed(object sender, ElapsedEventArgs e)
		{
			Debug.WriteLine("UNBLOCKED");
		}

		private void OnChanged(object sender, FileSystemEventArgs args)
		{
			RefreshLayoutList();
			EventBeacon.SendEvent("files_changed", sender, args);
		}


		private void RefreshLayoutList()
		{
			AppState.Layouts = Directory.GetFiles(AppState.LayoutRoot, AppState.LayoutExtension);
			EventBeacon.SendEvent("update_layout_list");
		}
	}
}
