using System.Threading.Tasks;
using WindowsInput;
using WindowsInput.Events;

namespace TabletFriend.Actions
{
	public class KeyAction : ButtonAction
	{
		public readonly KeyCode[] Keys;

		public KeyAction(KeyCode[] keys)
		{
			Keys = keys;
		}

		public override async Task Invoke()
		{
			await Simulate.Events().ClickChord(Keys).Invoke();
		}
	}
}
