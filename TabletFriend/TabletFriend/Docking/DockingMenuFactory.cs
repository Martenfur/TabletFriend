using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using WpfAppBar;

namespace TabletFriend.Docking
{
	public static class DockingMenuFactory
	{
		public static void CreateDockingMenu(ContextMenu menu)
		{
			var docking = new MenuItem() { Header = "docking" };

			var item = new MenuItem() { Header = "none" };
			item.Click += (sender, e) => OnDocking(DockingMode.None);
			docking.Items.Add(item);

			item = new MenuItem() { Header = "left" };
			item.Click += (sender, e) => OnDocking(DockingMode.Left);
			docking.Items.Add(item);

			item = new MenuItem() { Header = "top" };
			item.Click += (sender, e) => OnDocking(DockingMode.Top);
			docking.Items.Add(item);

			item = new MenuItem() { Header = "right" };
			item.Click += (sender, e) => OnDocking(DockingMode.Right);
			docking.Items.Add(item);

			// Bottom docking is broken as fuck. Maybe will fix it someday.
			//item = new MenuItem() {Header = "bottom"};
			//item.Click += (sender, e) => OnDocking(ABEdge.Bottom);
			//menu.Items.Add(item);

			menu.Items.Add(docking);
		}

		private static void OnDocking(DockingMode side) =>
			EventBeacon.SendEvent(Events.DockingChanged, side);
	}
}
