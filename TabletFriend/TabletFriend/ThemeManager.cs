using System.Windows;
using TabletFriend.Models;
using WpfAppBar;

namespace TabletFriend
{
	public class ThemeManager
	{
		/// <summary>
		/// If all else fails, use this theme.
		/// </summary>
		private static ThemeModel _fallbackTheme = new ThemeModel();
		
		public ThemeManager()
		{
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
			
			if (AppState.Themes.Count == 0)
			{
				MessageBox.Show(
					"No themes found!",
					"Load failure!",
					MessageBoxButton.OK,
					MessageBoxImage.Error
				);
				return;
			}
			if (!AppState.Themes.TryGetValue(path, out var theme)) // TODO: fix the check lul.
			{
				if (AppState.Themes.ContainsKey("default"))
				{
					MessageBox.Show(
						"Cannot load '" + path + "'! Trying to fall back to default theme.",
						"Load failure!",
						MessageBoxButton.OK,
						MessageBoxImage.Error
					);
					theme = AppState.Themes["default"];
				}
				else
				{
					MessageBox.Show(
						"No default theme found! Make sure you have a valid theme named 'default.yaml'",
						"Man you really screwed up",
						MessageBoxButton.OK,
						MessageBoxImage.Error
					);

					theme = _fallbackTheme;
				}
			}

			if (theme == null)
			{
				return;
			}

			AppState.CurrentTheme = theme;
			AppState.CurrentThemeName = path;
		}

	}
}
