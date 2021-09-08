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
		private const string _holdKeyword = "hold ";
		private const string _releaseKeyword = "release ";


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
				return ResolveCmdAction(actionString.Substring(_cmdKeyword.Length));
			}
			if (actionString.StartsWith(_waitKeyword))
			{
				return ResolveWaitAction(actionString.Substring(_waitKeyword.Length));
			}
			if (actionString.StartsWith(_holdKeyword))
			{
				return ResolveHoldAction(actionString.Substring(_holdKeyword.Length));
			}
			if (actionString.StartsWith(_releaseKeyword))
			{
				return ResolveReleaseAction(actionString.Substring(_releaseKeyword.Length));
			}

			return new KeyAction(StringToKeyCode(actionString));
		}

		private static ButtonAction ResolveHoldAction(string actionString) =>
			new HoldAction(StringToKeyCode(actionString));
		
		private static ButtonAction ResolveReleaseAction(string actionString) =>
			new ReleaseAction(StringToKeyCode(actionString));

		private static ButtonAction ResolveTypeAction(string actionString) =>
			new TypeAction(actionString.Trim());
		
		private static ButtonAction ResolveCmdAction(string actionString) =>
			new CmdAction(actionString.Trim());

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
