using System.Text.RegularExpressions;
using System.Threading.Tasks;
using WindowsInput.Events;

namespace TabletFriend.Actions
{
	public class ClickAction : ButtonAction
	{
		private bool _initialized = false;
		private readonly int _x;
		private readonly int _y;

		public ClickAction(string cmd)
		{
			Regex expression = new Regex(@"(?<x_coordinate>\d+)\s*,\s*(?<y_coordinate>\d+)");
			Match match = expression.Match(cmd);
			if (match.Success)
			{
				_x = int.Parse(match.Groups["x_coordinate"].Value);
				_y = int.Parse(match.Groups["y_coordinate"].Value);
				_initialized = true;
			}
		}

		public async override Task Invoke()
		{
			if (_initialized)
			{
				var sim = new EventBuilder();
				sim.MoveTo(_x, _y).Click(ButtonCode.Left);
				await sim.Invoke();
			}
		}
	}
}
