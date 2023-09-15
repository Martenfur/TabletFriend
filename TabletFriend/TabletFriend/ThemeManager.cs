using System.Windows;
using WpfAppBar;

namespace TabletFriend
{
	public class ThemeManager
	{
		private readonly MainWindow _window;

		public ThemeManager(MainWindow window)
		{
			_window = window;
			EventBeacon.Subscribe("files_changed", OnFilesChanged);
			EventBeacon.Subscribe("change_theme", OnChangeTheme);
		}


		private void OnFilesChanged(object[] args)
		{
			Application.Current.Dispatcher.Invoke(
				delegate
				{
					LoadTheme(AppState.CurrentThemePath);
				}
			);
		}

		private void OnChangeTheme(object[] obj)
		{
			var firstLoad = AppState.CurrentTheme == null;
			var path = (string)obj[0];

			LoadTheme(path);
			if (!firstLoad)
			{
				EventBeacon.SendEvent("update_settings");
			}
		}


		public void LoadTheme(string path)
		{
			var theme = Importer.ImportTheme(path);

			if (theme == null)
			{
				return;
			}

			AppState.CurrentTheme = theme;
			AppState.CurrentThemePath = path;
		}

	}
}
