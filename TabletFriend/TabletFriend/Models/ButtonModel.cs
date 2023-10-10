using MaterialDesignThemes.Wpf;
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

		public string Text = "";
		public object Icon;

		public Vector2 Position = Vector2.Zero;
		public Vector2 Size = Vector2.One;

		public string Style;

		public string Font;
		public int FontSize;
		public int FontWeight;

		/// <summary>
		/// If true, the button is considered a spacer. All actions will be ignored.
		/// </summary>
		public bool Spacer;

		public ButtonVisibility Visibility;

		public ButtonModel() { }

		public ButtonModel(ButtonData data)
		{
			if (data == null)
			{
				return;
			}
			Text = (data.Text ?? "").Replace("\\n", Environment.NewLine);

			if (!string.IsNullOrEmpty(data.Icon))
			{
				if (!string.IsNullOrEmpty(Path.GetExtension(data.Icon)) && File.Exists(data.Icon))
				{
					var ico = new Image();

					ico.Source = new BitmapImage(new Uri(Path.Combine(AppState.CurrentDirectory, data.Icon)));
					ico.Stretch = (Stretch)data.IconStretch;

					Icon = ico;
				}
				else
				{
					var iconName = data.Icon.Replace("_", "").Replace("-", ""); // Sanitizing the icon.
					if (Enum.TryParse<PackIconKind>(iconName, true, out var kind))
					{
						var ico = new PackIcon();
						ico.Kind = kind;

						Icon = ico;
					}
				}
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

			Style = data.Style;

			Font = data.Font;
			FontSize = data.FontSize;
			FontWeight = data.FontWeight;

			Visibility = data.Visibility;
		}

		public void Dispose()
		{
			Action?.Dispose();
		}
	}
}
