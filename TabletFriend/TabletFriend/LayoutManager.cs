using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using TabletFriend.Models;

namespace TabletFriend
{
	public class LayoutManager
	{
		private readonly Canvas _canvas;
		private readonly Window _window;

		public LayoutManager(Canvas canvas, Window window, FileManager file)
		{
			_canvas = canvas;
			_window = window;
			file.OnChanged += OnChanged;
		}

		private void OnChanged(object sender, FileSystemEventArgs e)
		{
			if (e.FullPath == AppState.CurrentLayoutPath)
			{
				Application.Current.Dispatcher.Invoke(
					delegate
					{
						LoadLayout(e.FullPath);
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
