using System.IO;
using System.Windows;
using System.Windows.Controls;

namespace TabletFriend
{
	public class LayoutManager
	{
		private readonly Canvas _canvas;
		private readonly Window _window;

		public LayoutManager(Canvas canvas, Window window)
		{
			_canvas = canvas;
			_window = window;
			EventBeacon.Subscribe("files_changed", OnChanged);
		}

		private void OnChanged(object[] args)
		{
			var sender = args[0];
			var fileSystemArgs = (FileSystemEventArgs)args[1];

			if (fileSystemArgs.FullPath == AppState.CurrentLayoutPath)
			{
				Application.Current.Dispatcher.Invoke(
					delegate
					{
						LoadLayout(fileSystemArgs.FullPath);
					}
				);
			}
		}

		public void LoadLayout(string path)
		{
			if (AppState.CurrentLayout != null)
			{
				AppState.CurrentLayout.Dispose();
			}
			var layout = LayoutImporter.Import(path);
			AppState.CurrentLayout = layout;
			AppState.CurrentLayout.CreateUI(_canvas, _window);
			AppState.CurrentLayoutPath = path;
		}
	}
}
