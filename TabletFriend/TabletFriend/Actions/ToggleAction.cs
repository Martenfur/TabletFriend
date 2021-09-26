using System.Threading;
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

		private long _inputLocked;

		public override async Task Invoke()
		{
			Interlocked.Exchange(ref _inputLocked, 1);

			if (!_held)
			{
				await Simulate.Events().Hold(_key).Invoke();
			}
			else
			{
				await Simulate.Events().Release(_key).Invoke();
			}
			_held = !_held;

			Interlocked.Exchange(ref _inputLocked, 0);
		}

		private void KeyEvent(object sender, EventSourceEventArgs<KeyboardEvent> e)
		{
			if (Interlocked.Read(ref _inputLocked) == 0 && e.Data?.KeyUp?.Key == _key)
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
