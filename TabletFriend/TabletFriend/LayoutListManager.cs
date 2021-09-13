using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Controls;

namespace TabletFriend
{
	public class LayoutListManager
	{
		public MenuItem Menu;


		public LayoutListManager()
		{
			Menu = new MenuItem() { Header = "layouts" };

			OnUpdateLayoutList();
			EventBeacon.Subscribe("update_layout_list", OnUpdateLayoutList);
		}


		private void OnUpdateLayoutList(object[] obj = null)
		{
			Application.Current.Dispatcher.Invoke(
				delegate
				{
					Menu.Items.Clear();
					foreach (var layout in AppState.Layouts)
					{
						var item = new MenuItem()
						{
							Header = Path.GetFileNameWithoutExtension(layout),
							DataContext = layout,
							IsCheckable = true,
							IsChecked = layout == AppState.CurrentLayoutPath
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
			foreach(MenuItem otherItem in Menu.Items)
			{
				otherItem.IsChecked = otherItem.DataContext == item.DataContext;
			}
			EventBeacon.SendEvent("change_layout", item.DataContext);
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
