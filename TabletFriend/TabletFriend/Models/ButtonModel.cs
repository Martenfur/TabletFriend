using System;
using System.IO;
using System.Numerics;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using TabletFriend.Actions;
using TabletFriend.Data;

namespace TabletFriend.Models
{
	public class ButtonModel : IDisposable
	{
		public ButtonAction Action;
		
		public string Text;
		public Image Icon;

		public Vector2 Position = Vector2.Zero;
		public Vector2 Size = Vector2.One;


		/// <summary>
		/// If true, the button is considered a spacer. All actions will be ignored.
		/// </summary>
		public bool Spacer;

		public ButtonModel(ButtonData data)
		{
			Text = data.Text ?? "";
			if (!string.IsNullOrEmpty(data.Icon) && File.Exists(data.Icon))
			{
				Icon = new Image();
				
				Icon.Source = new BitmapImage(new Uri(Path.Combine(Environment.CurrentDirectory, data.Icon)));
				Icon.Stretch = (Stretch)data.IconStretch;
			}
			
			if (data.Size != null)
			{
				Size = ModelConverter.ConvertVector2(data.Size);
			}

			Spacer = data.Spacer;

			if (data.Actions == null || data.Actions.Length == 0)
			{
				Action = ButtonActionResolver.Resolve(data.Action);
			}
			else
			{ 
				Action = new BatchAction(ButtonActionResolver.Resolve(data.Actions));
			}
		}

		public void Dispose()
		{
			Action?.Dispose();
			// TODO: Put icon dispose here also.
		}
	}
}
