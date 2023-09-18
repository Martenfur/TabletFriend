using System.Threading.Tasks;

namespace TabletFriend.Actions
{
	public class HideAction : ButtonAction
	{
		public override Task Invoke()
		{
			EventBeacon.SendEvent(Events.ToggleMinimize);
			return Task.CompletedTask;
		}
	}
}
