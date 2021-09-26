using System.Threading.Tasks;

namespace TabletFriend.Actions
{
	public class BatchAction : ButtonAction
	{
		private ButtonAction[] _actions;

		public BatchAction(ButtonAction[] actions)
		{
			_actions = actions;
		}

		public override async Task Invoke()
		{
			for (var i = 0; i < _actions.Length; i += 1)
			{
				await _actions[i].Invoke();
			}
		}

		public override void Dispose()
		{
			base.Dispose();
			for (var i = 0; i < _actions.Length; i += 1)
			{
				_actions[i].Dispose();
			}
		}
	}
}
