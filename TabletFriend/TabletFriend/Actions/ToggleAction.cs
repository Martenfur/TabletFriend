using System.Threading.Tasks;
using System.Windows.Input;
using WindowsInput;
using WindowsInput.Events;
using WindowsInput.Events.Sources;

namespace TabletFriend.Actions
{
	public class ToggleAction : ButtonAction
	{
		private readonly KeyCode _key;

		public IKeyboardEventSource Keyboard { get; }

		private bool _held = false;

		public ToggleAction(KeyCode key)
		{
			_key = key;


			Keyboard = Capture.Global.KeyboardAsync();

			//Capture all events from the keyboard
			Keyboard.KeyEvent += KeyEvent;

		}

		public override async Task Invoke()
		{
			if (!_held)
			{
				await Simulate.Events().Hold(_key).Invoke();
				_held = true;
			}
			else
			{
				await Simulate.Events().Release(_key).Invoke();
				_held = false;
			}
		}

		private void KeyEvent(object sender, EventSourceEventArgs<KeyboardEvent> e)
		{
			if (e.Data?.KeyUp?.Key == _key)
			{
				_held = false;
			}
		}

		public override void Dispose()
		{
			base.Dispose();

			Keyboard.KeyEvent -= KeyEvent;

			Keyboard.Dispose();
		}
	}
}
