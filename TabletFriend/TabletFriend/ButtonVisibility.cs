
namespace TabletFriend
{
	public enum ButtonVisibility
	{
		/// <summary>
		/// The button will always be present.
		/// </summary>
		Always = 0,

		/// <summary>
		/// The button will only be present in docked mode.
		/// </summary>
		Docked = 1,

		/// <summary>
		/// The button will only be present in undocked mode.
		/// </summary>
		Undocked = 2,

		// Naming conventions are broken because these values
		// are parsed from yaml which uses snake_case.

		/// <summary>
		/// The button will only be present when docked left.
		/// </summary>
		Docked_Left = 3,

		/// <summary>
		/// The button will only be present when docked right.
		/// </summary>
		Docked_Right = 4,
		
		/// <summary>
		/// The button will only be present when docked top.
		/// </summary>
		Docked_Top = 5,
	}
}
