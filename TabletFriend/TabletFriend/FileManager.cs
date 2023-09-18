using System.IO;

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

			RefreshLists();
		}


		private void OnChanged(object sender, FileSystemEventArgs args)
		{
			RefreshLists();
			EventBeacon.SendEvent(Events.FilesChanged, sender, args);
		}


		private void RefreshLists()
		{
			AppState.Layouts = Directory.GetFiles(AppState.LayoutsRoot, AppState.LayoutExtension);
			AppState.Themes = Directory.GetFiles(AppState.ThemesRoot, AppState.LayoutExtension);
			EventBeacon.SendEvent(Events.UpdateThemeList);
			EventBeacon.SendEvent(Events.UpdateLayoutList);
		}
	}
}
