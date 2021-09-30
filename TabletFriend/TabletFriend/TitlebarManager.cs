using MaterialDesignThemes.Wpf;
using System.Windows;
using System.Windows.Controls;
using TabletFriend.Models;
using WpfAppBar;

namespace TabletFriend
{
	public static class TitlebarManager
	{

		private const PackIconKind _defaultIcon = PackIconKind.RhombusMedium;
		private const PackIconKind _minimizedIcon = PackIconKind.CircleMedium;
		private const double _baseTitlebarHeight = 12;

		private static bool _minimized = false;

		private static PackIcon _ico;
		private static MainWindow _window;
		private static ThemeModel _theme;

		private static double _maximizedWindowHeight;

		public static double GetTitlebarHeight(ThemeModel theme)
		{
			if (AppState.Settings.DockingMode == DockingMode.None)
			{
				return _baseTitlebarHeight + theme.Margin * 2;
			}
			return 0;
		}

		public static void CreateTitlebar(MainWindow window, ThemeModel theme)
		{
			_minimized = false;
			_window = window;
			_theme = theme;

			if (AppState.Settings.DockingMode != DockingMode.None)
			{
				return;
			}

			_maximizedWindowHeight = _window.Height;

			var uiButton = new Button();

			uiButton.Width = 32;
			uiButton.Height = GetTitlebarHeight(theme);

			uiButton.Style = Application.Current.Resources["shy"] as Style;
			_ico = new PackIcon();
			_ico.Kind = _defaultIcon;

			uiButton.Content = _ico;
			uiButton.Click += OnClick;

			Canvas.SetTop(uiButton, 0);
			Canvas.SetLeft(uiButton, (_window.Width - uiButton.Width) / 2);
			_window.MainCanvas.Children.Add(uiButton);
		}

		private static void OnClick(object sender, RoutedEventArgs e)
		{
			if (_minimized)
			{
				_window.Height = _maximizedWindowHeight;
				_ico.Kind = _defaultIcon;
			}
			else
			{
				_window.Height = GetTitlebarHeight(_theme);
				_ico.Kind = _minimizedIcon;
			}
			_minimized = !_minimized;
		}
	}
}
