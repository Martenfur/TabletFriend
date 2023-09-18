﻿using System;
using System.IO;
using System.Threading;
using System.Windows;
using TabletFriend.Data;
using TabletFriend.Models;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace TabletFriend
{
	public static class Importer
	{
		private static readonly IDeserializer _deserializer = new DeserializerBuilder()
			.WithNamingConvention(UnderscoredNamingConvention.Instance)
			.IgnoreUnmatchedProperties()
			.Build();

		public static LayoutModel ImportLayout(string path)
		{
			try
			{
				var data = Import<LayoutData>(path);
				
				if (data == null)
				{
					throw new Exception("Layout import failed!");
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

		public static ThemeModel ImportTheme(string path)
		{
			try
			{
				var data = Import<ThemeData>(path);

				return new ThemeModel(data);
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
				catch
				{
					Thread.Sleep(100);
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
