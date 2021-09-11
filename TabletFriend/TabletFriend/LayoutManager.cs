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
		private LayoutModel _currentLayout;
		private string _currentLayoutPath;

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
			if (e.FullPath == _currentLayoutPath)
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
			if (_currentLayout != null)
			{
				_currentLayout.Dispose();
			}
			var layout = LayoutImporter.Import(path);
			_currentLayout = layout;
			_currentLayout.Create(_canvas, _window);
			_currentLayoutPath = path;
		}
	}
}
