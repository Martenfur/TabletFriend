using System.IO;
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
			var layout = File.ReadAllText(path)
				.Replace("	", "  "); // The thing doesn't like tabs.

			var data = _deserializer.Deserialize<LayoutData>(layout);
			return new LayoutModel(data);
		}
	}
}
