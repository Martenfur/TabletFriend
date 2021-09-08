using System.Numerics;
using TabletFriend.Data;

namespace TabletFriend.Models
{
	public class ButtonModel
	{
		public string[] Actions;
		
		public string Text;
		public string Icon;

		public Vector2 Position = Vector2.Zero;
		public Vector2 Size = Vector2.One;

		/// <summary>
		/// If true, the button is considered a spacer. All actions will be ignored.
		/// </summary>
		public bool Spacer;

		public ButtonModel(ButtonData data)
		{
			Text = data.Text ?? "";
			Icon = data.Icon;
			
			if (data.Size != null)
			{
				Size = ModelConverter.ConvertVector2(data.Size);
			}

			Spacer = data.Spacer;
		}
	}
}
