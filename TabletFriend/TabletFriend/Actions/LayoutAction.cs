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
			EventBeacon.SendEvent(Events.ChangeLayout, _layout);
			return Task.CompletedTask;
		}
	}
}
