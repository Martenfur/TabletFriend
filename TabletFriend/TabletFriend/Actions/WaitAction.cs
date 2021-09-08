using System.Threading.Tasks;

namespace TabletFriend.Actions
{
	public class WaitAction : ButtonAction
	{
		private int _delay;

		public WaitAction(int delay)
		{
			_delay = delay;
		}


		public override async Task Invoke()
		{
			await Task.Delay(_delay);
		}
	}
}
