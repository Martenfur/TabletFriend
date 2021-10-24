using System.Threading.Tasks;

namespace TabletFriend.Actions
{
	public class HideAction : ButtonAction
	{
		public override async Task Invoke() =>
			EventBeacon.SendEvent("toggle_minimize");
	}
}
