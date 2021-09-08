using System.Threading.Tasks;
using WindowsInput;
using WindowsInput.Events;

namespace TabletFriend.Actions
{
	public class HoldAction : ButtonAction
	{
		private readonly KeyCode[] _keys;

		public HoldAction(KeyCode[] keys)
		{
			_keys = keys;
		}

		public override async Task Invoke() =>
			await Simulate.Events().Hold(_keys).Invoke();
	}
}
