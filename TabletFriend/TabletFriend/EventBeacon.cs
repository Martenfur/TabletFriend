using System;
using System.Collections.Generic;

namespace TabletFriend
{
	public static class EventBeacon
	{
		private static Dictionary<string, List<Action<object[]>>> _subscribers
			= new Dictionary<string, List<Action<object[]>>>();


		public static void Subscribe(string tag, Action<object[]> action)
		{
			if (!_subscribers.TryGetValue(tag, out var subscribers))
			{
				subscribers = new List<Action<object[]>>();
				_subscribers.Add(tag, subscribers);
			}
			subscribers.Add(action);
		}


		public static void SendEvent(string tag, params object[] args)
		{
			if (_subscribers.TryGetValue(tag, out var subscribers))
			{
				foreach (var subscriber in subscribers)
				{
					subscriber?.Invoke(args);
				}
			}
		}
	}
}
