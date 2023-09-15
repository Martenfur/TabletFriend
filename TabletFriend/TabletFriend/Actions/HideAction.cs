using System.Threading.Tasks;

namespace TabletFriend.Actions
{
	public class HideAction : ButtonAction
	{
		public override Task Invoke()
		{
			EventBeacon.SendEvent("toggle_minimize");
			return Task.CompletedTask;
		}
	}
}
