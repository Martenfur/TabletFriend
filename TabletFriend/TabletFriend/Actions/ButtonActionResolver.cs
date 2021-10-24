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
		private const string _layoutKeyword = "layout ";
		private const string _repeatKeyword = "repeat ";
		private const string _hideKeyword = "hide";


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
				return ResolveToggleAction(actionString.Substring(_toggleKeyword.Length));
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
			if (actionString.StartsWith(_layoutKeyword))
			{
				return ResolveLayoutAction(actionString.Substring(_layoutKeyword.Length));
			}
			if (actionString.StartsWith(_repeatKeyword))
			{
				return ResolveRepeatAction(actionString.Substring(_repeatKeyword.Length));
			}
			if (actionString.StartsWith(_hideKeyword))
			{
				return ResolveHideAction();
			}

			return new KeyAction(StringToKeyCode(actionString));
		}

		private static ButtonAction ResolveToggleAction(string actionString) =>
			new ToggleAction(StringToKeyCode(actionString)[0]);

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

		private static ButtonAction ResolveLayoutAction(string actionString) =>
			new LayoutAction(actionString);
		
		private static ButtonAction ResolveRepeatAction(string actionString) =>
			new RepeatAction(StringToKeyCode(actionString));
		
		private static ButtonAction ResolveHideAction() =>
			new HideAction();


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
			try
			{
				var args = keyString.Replace(" ", "").Replace("_", "").Split("+");
				var keysList = new List<KeyCode>();

				foreach (var arg in args)
				{
					keysList.Add(Enum.Parse<KeyCode>(Translate(arg), true));
				}

				return keysList.ToArray();
			}
			catch (Exception e)
			{
				throw new FormatException(
					"Error parsing '" + keyString + "'. Command or key combination may be invalid. "
					+ Environment.NewLine + e.Message
				);
			}
		}

		private static string Translate(string inputKey)
		{
			if (_translationTable.TryGetValue(inputKey, out var outputKey))
			{
				return outputKey;
			}
			return inputKey;
		}

		/// <summary>
		/// Some enum entries don't look very pretty, so we translate them.
		/// </summary>
		private static Dictionary<string, string> _translationTable = new Dictionary<string, string>(StringComparer.InvariantCultureIgnoreCase)
		{
			{ "Windows", nameof(KeyCode.LWin) },
			{ "Win", nameof(KeyCode.LWin) },
			{ "Shift", nameof(KeyCode.LShift) },
			{ "Ctrl", nameof(KeyCode.LControl) },
			{ "Alt", nameof(KeyCode.LAlt) },
			{ "0", nameof(KeyCode.D0) },
			{ "1", nameof(KeyCode.D1) },
			{ "2", nameof(KeyCode.D2) },
			{ "3", nameof(KeyCode.D3) },
			{ "4", nameof(KeyCode.D4) },
			{ "5", nameof(KeyCode.D5) },
			{ "6", nameof(KeyCode.D6) },
			{ "7", nameof(KeyCode.D7) },
			{ "8", nameof(KeyCode.D8) },
			{ "9", nameof(KeyCode.D9) },
		};
	}
}
