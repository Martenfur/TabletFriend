using System.Threading.Tasks;
using WindowsInput;
using WindowsInput.Events;

namespace TabletFriend.Actions
{
	public class KeyAction : ButtonAction
	{
		private readonly KeyCode[] _keys;

		public KeyAction(KeyCode[] keys)
		{
			_keys = keys;
		}

		public override async Task Invoke() =>
			await Simulate.Events().ClickChord(_keys).Invoke();
	}
}
