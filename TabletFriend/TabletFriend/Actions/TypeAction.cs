using System.Threading.Tasks;
using WindowsInput;

namespace TabletFriend.Actions
{
	public class TypeAction : ButtonAction
	{
		public readonly string _text;

		public TypeAction(string text)
		{
			_text = text;
		}

		public override async Task Invoke()
		{
			await Simulate.Events().Click(_text).Invoke();
		}
	}
}
