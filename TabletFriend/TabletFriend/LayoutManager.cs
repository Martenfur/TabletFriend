using System;
using System.Diagnostics;
using System.IO;
using System.Windows;
using WpfAppBar;

namespace TabletFriend
{
	public class LayoutManager
	{
		public LayoutLoadResult LastLoadResult;

		private readonly MainWindow _window;

		public LayoutManager(MainWindow window)
		{
			_window = window;
			EventBeacon.Subscribe(Events.FilesChanged, OnFilesChanged);
			EventBeacon.Subscribe(Events.ChangeLayout, OnChangeLayout);
		}


		private void OnFilesChanged(object[] args)
		{
			Application.Current.Dispatcher.Invoke(
				delegate
				{
					// Full reload every time.
					LoadLayout(AppState.CurrentLayoutName);
					LastLoadResult = LayoutLoadResult.RequiresRedock;
				}
			);
		}

		private void OnChangeLayout(object[] obj)
		{
			var firstLoad = AppState.CurrentLayout == null;
			var path = (string)obj[0];

			var isManual = true;
			if (obj.Length > 1)
			{
				var method = (LayoutChangeMethod)obj[1];
				if (method == LayoutChangeMethod.Automatic)
				{
					isManual = false;
				}
			}

            if (isManual)
            {
				AppState.LastManuallySetLayout = path;				
			}

			LoadLayout(path);
			if (firstLoad)
			{
				LastLoadResult = LayoutLoadResult.RequiresRedock;
			}

			if (!firstLoad)
			{
				EventBeacon.SendEvent(Events.UpdateSettings);
			}
		}


		public void LoadLayout(string path)
		{
			LastLoadResult = LayoutLoadResult.Default;

			Debug.WriteLine("Loading " + path);
			if (AppState.CurrentLayout != null)
			{
				AppState.CurrentLayout.Dispose();
			}
			var layout = AppState.Layouts[path];
			if (layout == null) 
			{ 
				layout = AppState.Layouts["default"];
			}

			if (layout == null)// || layout == AppState.CurrentLayout)
			{
				return;
			}

			if (AppState.CurrentLayout != null && !AppState.CurrentLayout.IsSameWidth(layout))
			{
				LastLoadResult = LayoutLoadResult.RequiresRedock;
			}
			AppState.CurrentLayout = layout;
			//UiFactory.CreateUi(AppState.CurrentLayout, _window);
			AppState.CurrentLayoutName = Path.GetFileNameWithoutExtension(path);
			EventBeacon.SendEvent(Events.DockingChanged, AppState.Settings.DockingMode);
		}

	}

	public enum LayoutLoadResult
	{
		Default,
		RequiresRedock,
	}
}
