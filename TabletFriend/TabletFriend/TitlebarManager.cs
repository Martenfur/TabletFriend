using MaterialDesignThemes.Wpf;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using TabletFriend.Models;
using WpfAppBar;

namespace TabletFriend
{
	public static class TitlebarManager
	{

		private const PackIconKind _defaultIcon = PackIconKind.RhombusMediumOutline;
		private const PackIconKind _minimizedIcon = PackIconKind.RhombusMedium;
		private const double _baseTitlebarHeight = 12;

		private static bool _minimizedMode = false;
		private static bool _minimized = false;
		public static bool Minimized => _minimized;

		private static PackIcon _ico;
		private static MainWindow _window;
		private static ThemeModel _theme;
		private static LayoutModel _layout;

		private static double _maximizedWindowHeight;

		/// <summary>
		/// Blocks the first minimize to make the feel a little bit nicer.
		/// </summary>
		private static bool _grace = false;
		public static double GetTitlebarHeight(LayoutModel layout)
		{
			if (AppState.Settings.DockingMode == DockingMode.None)
			{
				return _baseTitlebarHeight + layout.Margin * 2;
			}
			return 0;
		}

		public static void CreateTitlebar(MainWindow window, ThemeModel theme, LayoutModel layout, double maximizedWindowHeight, bool minimized)
		{
			_minimized = minimized;

			_window = window;
			_theme = theme;
			_layout = layout;
			_maximizedWindowHeight = maximizedWindowHeight;

			_window.MouseEnter -= OnMouseEnter;
			_window.MouseLeave -= OnMouseLeave;

			if (AppState.Settings.DockingMode != DockingMode.None)
			{
				return;
			}


			CreateButton();

			_window.MouseEnter += OnMouseEnter;
			_window.MouseLeave += OnMouseLeave;

			_grace = true;

			if (_minimized)
			{
				_window.Height = GetTitlebarHeight(_layout);
			}
		}


		private static void CreateButton()
		{
			var uiButton = new Button();

			uiButton.Width = 32;
			uiButton.Height = GetTitlebarHeight(_layout);

			uiButton.Style = Application.Current.Resources["shy"] as Style;
			_ico = new PackIcon();
			if (_minimizedMode)
			{
				_ico.Kind = _minimizedIcon;
			}
			else
			{
				_ico.Kind = _defaultIcon;
			}
			uiButton.Content = _ico;
			uiButton.Click += OnClick;

			Canvas.SetTop(uiButton, 0);
			Canvas.SetLeft(uiButton, (_window.Width - uiButton.Width) / 2);
			_window.MainCanvas.Children.Add(uiButton);

		}

		private static void OnMouseLeave(object sender, MouseEventArgs e)
		{
			if (_minimizedMode && !_grace)
			{
				Minimize();
			}
			_grace = false;
		}

		private static void OnMouseEnter(object sender, MouseEventArgs e)
		{
			if (_minimizedMode)
			{
				Maximize();
			}
			_grace = false;
		}

		private static void OnClick(object sender, RoutedEventArgs e)
		{
			if (_minimizedMode)
			{
				Maximize();
				_ico.Kind = _defaultIcon;
			}
			else
			{
				if (!_window.IsMouseOver)
				{
					Minimize();
				}
				_ico.Kind = _minimizedIcon;
			}
			_minimizedMode = !_minimizedMode;
		}

		public static void Minimize()
		{
			if (!_minimized)
			{
				_minimized = true;
				_maximizedWindowHeight = _window.Height;
				_window.Height = GetTitlebarHeight(_layout);
			}
		}

		public static void Maximize()
		{
			if (_minimized)
			{
				_minimized = false;
				_window.Height = _maximizedWindowHeight;
			}
		}


	}
}
