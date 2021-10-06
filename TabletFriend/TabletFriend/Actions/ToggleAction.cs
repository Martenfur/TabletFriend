using System.Threading.Tasks;
using WindowsInput.Events;

namespace TabletFriend.Actions
{
	public class ToggleAction : ButtonAction
	{
		public readonly KeyCode Key;

		public ToggleAction(KeyCode key)
		{
			Key = key;
		}
		
		
		public override async Task Invoke()
		{
			await ToggleManager.Toggle(Key);
		}

		public override void Dispose()
		{
			base.Dispose();

		}
	}
}
