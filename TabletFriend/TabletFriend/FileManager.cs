using System;
using System.IO;

namespace TabletFriend
{
	public class FileManager
	{

		private const string _layoutExtension = "*.yaml";
		private FileSystemWatcher _watcher;

		public FileManager()
		{
			_watcher = new FileSystemWatcher(AppState.LayoutRoot);
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


		private void InternalOnChanged(object sender, FileSystemEventArgs args)
		{
			RefreshLayoutList();
			EventBeacon.SendEvent("files_changed", sender, args);
		}


		public void RefreshLayoutList() =>
			AppState.Layouts = Directory.GetFiles(AppState.LayoutRoot, _layoutExtension);

	}
}
