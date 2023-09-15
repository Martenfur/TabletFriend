using System.Threading.Tasks;

namespace TabletFriend.Actions
{
	public class LayoutAction : ButtonAction
	{
		private readonly string _layout;

		public LayoutAction(string layout)
		{
			_layout = layout;
		}

		public override Task Invoke()
		{
			EventBeacon.SendEvent("change_layout", _layout);
			return Task.CompletedTask;
		}
	}
}
