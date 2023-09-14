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
			EventBeacon.Subscribe("change_theme", OnChangeTheme);
			EventBeacon.Subscribe("update_theme_list", OnUpdateThemeList);
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
							IsChecked = theme == AppState.CurrentLayoutPath
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
			EventBeacon.SendEvent("change_theme", item.DataContext);
			EventBeacon.SendEvent("change_layout", AppState.CurrentLayoutPath);
		}

		
		public MenuItem[] CloneMenu()
		{
			var items = new List<MenuItem>();

			foreach(MenuItem item in Menu.Items)
			{
				var newItem = new MenuItem()
				{
					Header = item.Header,
					DataContext = item.DataContext,
					IsCheckable = item.IsCheckable,
					IsChecked = item.IsChecked,
				};
				items.Add(newItem);
				newItem.Click += OnClick;
			}

			return items.ToArray();
		}
	}
}
