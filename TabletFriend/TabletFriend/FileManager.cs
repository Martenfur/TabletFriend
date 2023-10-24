using System.IO;
using System.Windows;

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
			Application.Current.Dispatcher.Invoke(
				delegate
				{
					var layouts = Importer.ImportLayouts();
					if (AppState.Layouts == null || layouts.Count > 0)
					{
						AppState.Layouts = layouts;
					}
					var themes = Importer.ImportThemes();
					if (AppState.Themes == null || themes.Count > 0)
					{
						AppState.Themes = themes;
					}
				}
			);
			EventBeacon.SendEvent(Events.UpdateThemeList);
			EventBeacon.SendEvent(Events.UpdateLayoutList);
		}
	}
}
