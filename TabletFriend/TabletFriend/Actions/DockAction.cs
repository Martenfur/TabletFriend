using System.Threading.Tasks;
using WpfAppBar;

namespace TabletFriend.Actions
{
	public class DockAction : ButtonAction
	{
		private DockingMode _side = DockingMode.None;
		public DockAction(string side)
		{
			side = side.Trim();
			if (side == "left")
				_side = DockingMode.Left;
			if (side == "right")
				_side = DockingMode.Right;
			if (side == "top")
				_side = DockingMode.Top;
		}

		public override Task Invoke()
		{
			EventBeacon.SendEvent(Events.DockingChanged, _side);
			return Task.CompletedTask;
		}
	}
}
