using System;
using System.IO;

namespace TabletFriend
{
	public class FileManager
	{
		private SoftMade.IO.AdvancedFileSystemWatcher _watcher;

		public FileManager()
		{
			_watcher = new SoftMade.IO.AdvancedFileSystemWatcher();
			_watcher.Path = AppState.FilesRoot;
			_watcher.NotifyFilter = NotifyFilters.LastWrite
				| NotifyFilters.Size
				| NotifyFilters.LastWrite
				| NotifyFilters.CreationTime
				| NotifyFilters.FileName
				| NotifyFilters.DirectoryName;
			
			_watcher.Changed += InternalOnChanged;
			_watcher.Created += InternalOnChanged;
			_watcher.Deleted += InternalOnChanged;
			_watcher.EnableRaisingEvents = true;
			_watcher.IncludeSubdirectories = true;

			RefreshLayoutList();
		}


		private void InternalOnChanged(object sender, SoftMade.IO.FileSystemEventArgs args)
		{
			if (args.ChangeType != SoftMade.IO.WatcherChangeTypes.BeginWrite)
			{
				RefreshLayoutList();
				EventBeacon.SendEvent("files_changed", sender, args);
			}
		}


		public void RefreshLayoutList()
		{
			EventBeacon.SendEvent("update_layout_list");
			AppState.Layouts = Directory.GetFiles(AppState.LayoutRoot, AppState.LayoutExtension);
		}

	}
}
