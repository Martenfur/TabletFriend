using System.Threading.Tasks;
using WindowsInput;
using WindowsInput.Events;

namespace TabletFriend.Actions
{
	public class ReleaseAction : ButtonAction
	{
		private readonly KeyCode[] _keys;

		public ReleaseAction(KeyCode[] keys)
		{
			_keys = keys;
		}

		public override async Task Invoke() =>
			await Simulate.Events().Release(_keys).Invoke();
	}
}
