using System;
using System.Collections.Generic;
using WindowsInput.Events;

namespace TabletFriend.Actions
{
	public static class ButtonActionResolver
	{
		private const string _typeKeyword = "type ";
		private const string _toggleKeyword = "toggle ";
		private const string _cmdKeyword = "cmd ";
		private const string _waitKeyword = "wait ";


		public static ButtonAction Resolve(string actionString)
		{
			if (string.IsNullOrEmpty(actionString))
			{
				return null;
			}
			actionString = actionString.Trim();

			if (actionString.StartsWith(_typeKeyword))
			{
				return ResolveTypeAction(actionString.Substring(_typeKeyword.Length));
			}
			if (actionString.StartsWith(_toggleKeyword))
			{
				return null;
			}
			if (actionString.StartsWith(_cmdKeyword))
			{
				return null;
			}
			if (actionString.StartsWith(_waitKeyword))
			{
				return ResolveWaitAction(actionString.Substring(_waitKeyword.Length));
			}

			return new KeyAction(StringToKeyCode(actionString));
		}

		private static ButtonAction ResolveTypeAction(string actionString) =>
			new TypeAction(actionString.Trim());

		private static ButtonAction ResolveWaitAction(string actionString) =>
			new WaitAction(int.Parse(actionString));

		public static ButtonAction[] Resolve(string[] actionStrings)
		{
			var actions = new List<ButtonAction>();

			foreach (var actionString in actionStrings)
			{
				var action = Resolve(actionString);
				if (action != null)
				{
					actions.Add(action);
				}
			}

			return actions.ToArray();
		}


		private static KeyCode[] StringToKeyCode(string keyString)
		{
			var args = keyString.Split("+");
			var keysList = new List<KeyCode>();

			foreach (var arg in args)
			{
				keysList.Add(Enum.Parse<KeyCode>(arg));
			}

			return keysList.ToArray();
		}
	}
}
