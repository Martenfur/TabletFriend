using System;
using System.IO;

namespace TabletFriend
{
	public class FileManager
    {
        public readonly string LayoutRoot = Path.Combine(Environment.CurrentDirectory, "layouts");
		
		public string[] Layouts;

		private const string _layoutExtension = "*.yaml";
		private FileSystemWatcher _watcher;

		public event FileSystemEventHandler OnChanged;

		public FileManager()
		{
			_watcher = new FileSystemWatcher(LayoutRoot);
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
			OnChanged?.Invoke(sender, args);
		}


		public void RefreshLayoutList() =>
			Layouts = Directory.GetFiles(LayoutRoot, _layoutExtension);


	}
}
