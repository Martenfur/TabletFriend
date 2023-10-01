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
			EventBeacon.Subscribe(Events.FilesChanged, OnFilesChanged);
			EventBeacon.Subscribe(Events.ChangeTheme, OnChangeTheme);
		}


		private void OnFilesChanged(object[] args)
		{
			Application.Current.Dispatcher.Invoke(
				delegate
				{
					LoadTheme(AppState.CurrentThemeName);
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
				EventBeacon.SendEvent(Events.UpdateSettings);
			}
		}


		public void LoadTheme(string path)
		{
			var theme = AppState.Themes[path];

			if (theme == null)
			{
				return;
			}

			AppState.CurrentTheme = theme;
			AppState.CurrentThemeName = path;
		}

	}
}
