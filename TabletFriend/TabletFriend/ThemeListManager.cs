using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Controls;

namespace TabletFriend
{
	public class ThemeListManager
	{
		public MenuItem Menu;


		public ThemeListManager()
		{
			Menu = new MenuItem() { Header = "themes" };

			OnUpdateThemeList();
			EventBeacon.Subscribe(Events.ChangeTheme, OnChangeTheme);
			EventBeacon.Subscribe(Events.UpdateThemeList, OnUpdateThemeList);
		}

		private void OnChangeTheme(object[] obj)
		{
			var path = Path.GetFullPath((string)obj[0]);
			foreach (MenuItem otherItem in Menu.Items)
			{
				otherItem.IsChecked = (string)otherItem.DataContext == path;
			}
		}

		private void OnUpdateThemeList(object[] obj = null)
		{
			Application.Current.Dispatcher.Invoke(
				delegate
				{
					Menu.Items.Clear();
					foreach (var theme in AppState.Themes)
					{
						var item = new MenuItem()
						{
							Header = Path.GetFileNameWithoutExtension(theme).Replace("_", " "),
							DataContext = theme,
							IsCheckable = true,
							IsChecked = theme == AppState.CurrentThemePath
						};
						Menu.Items.Add(item);
						item.Click += OnClick;
					}
				}
			);
		}

		private void OnClick(object sender, RoutedEventArgs e)
		{
			var item = (MenuItem)sender;
			EventBeacon.SendEvent(Events.ChangeTheme, item.DataContext);
			EventBeacon.SendEvent(Events.ChangeLayout, AppState.CurrentLayoutPath);
		}


		public MenuItem CloneMenu()
		{
			var menu = new MenuItem() { Header = "themes" };

			var items = new List<MenuItem>();

			foreach (MenuItem item in Menu.Items)
			{
				var newItem = new MenuItem()
				{
					Header = item.Header,
					DataContext = item.DataContext,
					IsCheckable = item.IsCheckable,
					IsChecked = item.IsChecked,
				};
				menu.Items.Add(newItem);
				newItem.Click += OnClick;
			}

			return menu;
		}
	}
}
