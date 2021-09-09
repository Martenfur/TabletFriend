using System;
using System.Threading.Tasks;

namespace TabletFriend.Actions
{
	public abstract class ButtonAction : IDisposable
	{
		public abstract Task Invoke();

		public virtual void Dispose() {}
	}
}
