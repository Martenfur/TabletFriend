using System;
using System.IO;
using System.Windows;
using TabletFriend.Data;
using TabletFriend.Models;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace TabletFriend
{
	public static class LayoutImporter
	{
		private static readonly IDeserializer _deserializer = new DeserializerBuilder()
			.WithNamingConvention(UnderscoredNamingConvention.Instance)
			.Build();

		public static LayoutModel Import(string path)
		{
			try
			{
				var data = Import<LayoutData>(path);
				if (data.ExternalTheme != null && File.Exists(data.ExternalTheme))
				{
					if (data.Theme == null)
					{
						data.Theme = new ThemeData();
					}
					data.Theme.Merge(Import<ThemeData>(data.ExternalTheme));
				}
				return new LayoutModel(data);
			}
			catch (Exception e)
			{
				// This is causing problems when the file is loaded several times over by the file watcher.
				//MessageBox.Show(
				//	"Cannot load '" + path + "': " + e.Message,
				//	"Load failure!",
				//	MessageBoxButton.OK,
				//	MessageBoxImage.Error
				//);
			}
			return null;
		}


		private static T Import<T>(string path)
		{
			string layout = null;

			if (!File.Exists(path))
			{
				MessageBox.Show(
					"'" + path + "' does not exist!",
					"File not found!",
					MessageBoxButton.OK,
					MessageBoxImage.Error
				);

				return default(T);
			}

			for (var i = 0; i < 32; i += 1)
			{
				try
				{
					layout = File.ReadAllText(path)
						.Replace("\t", "  "); // The thing doesn't like tabs.
					break;
				}
				catch (Exception e)
				{

				}
			}

			var data = _deserializer.Deserialize<T>(layout);
			if (data == null)
			{
				throw new Exception("Failed to parse yaml!");
			}
			return data;
		}
	}
}
