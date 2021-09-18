using System.Windows;

namespace TabletFriend
{
	public class LayoutManager
	{
		private readonly MainWindow _window;

		public LayoutManager(MainWindow window)
		{
			_window = window;
			EventBeacon.Subscribe("files_changed", OnFilesChanged);
			EventBeacon.Subscribe("change_layout", OnChangeLayout);
		}


		private void OnFilesChanged(object[] args)
		{
			Application.Current.Dispatcher.Invoke(
				delegate
				{
					LoadLayout(AppState.CurrentLayoutPath);
				}
			);
		}

		private void OnChangeLayout(object[] obj)
		{
			var path = (string)obj[0];
			LoadLayout(path);
			EventBeacon.SendEvent("update_settings");
		}


		public void LoadLayout(string path)
		{
			if (AppState.CurrentLayout != null)
			{
				AppState.CurrentLayout.Dispose();
			}
			var layout = LayoutImporter.Import(path);

			if (layout == null)
			{
				return;
			}

			AppState.CurrentLayout = layout;
			UiFactory.CreateUi(AppState.CurrentLayout, _window);
			AppState.CurrentLayoutPath = path;
		}

	}
}
