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

		private const string _layoutExtension = "*.yaml";

		private readonly Canvas _canvas;
		private readonly Window _window;

		private string _layoutRoot = Path.Combine(Environment.CurrentDirectory, "layouts");

		public string[] _layouts;

		private FileSystemWatcher _watcher;

		public LayoutManager(Canvas canvas, Window window)
		{
			_watcher = new FileSystemWatcher(_layoutRoot);
			_watcher.NotifyFilter = NotifyFilters.LastWrite
				| NotifyFilters.Size
				| NotifyFilters.LastWrite
				| NotifyFilters.CreationTime
				| NotifyFilters.FileName
				| NotifyFilters.DirectoryName;
			_watcher.Changed += OnChanged;
			_watcher.Created += OnChanged;
			_watcher.Deleted += OnChanged;
			_watcher.EnableRaisingEvents = true;
			_watcher.IncludeSubdirectories = true;

			RefreshLayoutList();
			_canvas = canvas;
			_window = window;
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


		private void RefreshLayoutList() =>
			_layouts = Directory.GetFiles(_layoutRoot, _layoutExtension);
	}
}
