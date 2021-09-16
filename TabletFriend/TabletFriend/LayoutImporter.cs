﻿using System;
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
			string layout = null;

			if (!File.Exists(path))
			{
				MessageBox.Show(
					"Layout file '" + path + "' does not exist!",
					"Layout not found!",
					MessageBoxButton.OK,
					MessageBoxImage.Error
				);

				return null;
			}

			for (var i = 0; i < 10; i += 1)
			{
				try
				{
					layout = File.ReadAllText(path)
						.Replace("	", "  "); // The thing doesn't like tabs.
					break;
				}
				catch
				{ }
			}

			try
			{
				var data = _deserializer.Deserialize<LayoutData>(layout);
				if (data == null)
				{
					throw new Exception("Failed to parse yaml!");
				}
				return new LayoutModel(data);
			}
			catch (Exception e)
			{
				MessageBox.Show(
					"Cannot load '" + path + "': " + e.Message,
					"Load failure!",
					MessageBoxButton.OK,
					MessageBoxImage.Error
				);
			}
			return null;
		}
	}
}
