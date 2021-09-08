using System.Diagnostics;
using System.Threading.Tasks;

namespace TabletFriend.Actions
{
	public class CmdAction : ButtonAction
	{
		private readonly string _cmd;

		public CmdAction(string cmd)
		{
			_cmd = cmd;
		}

		public async override Task Invoke()
		{
			var process = new Process();
			var startInfo = new ProcessStartInfo();
			startInfo.WindowStyle = ProcessWindowStyle.Hidden;
			startInfo.FileName = "cmd.exe";
			startInfo.Arguments = "/C " + _cmd;
			process.StartInfo = startInfo;
			process.Start();
		}
	}
}
