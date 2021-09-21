using System;
using System.IO;
using System.Windows;
using WpfAppBar;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace TabletFriend
{
	public class Settings
	{
		public bool AddToAutostart = true;
		public double WindowX = 0;
		public double WindowY = 0;
		public string Layout = "files/layouts/a_toolbar.yaml";
		public DockingMode DockingMode = DockingMode.None;

		private string FullLayoutPath => Path.Combine(AppState.CurrentDirectory, Layout);

		// TODO: Add docking mode saving.

		public Settings()
		{
			EventBeacon.Subscribe("update_settings", OnUpdateSettings);
		}

		public void Apply()
		{
			if (AppState.CurrentLayout == null || FullLayoutPath != AppState.CurrentLayoutPath)
			{
				EventBeacon.SendEvent("change_layout", FullLayoutPath);
			}
			Application.Current.MainWindow.Left = WindowX;
			Application.Current.MainWindow.Top = WindowY;
			
			EventBeacon.SendEvent("docking_changed", DockingMode);
		}


		private void OnUpdateSettings(object[] obj)
		{
			Layout = Path.GetRelativePath(AppState.CurrentDirectory, AppState.CurrentLayoutPath);
			if (!double.IsNaN(Application.Current.MainWindow.Left))
			{
				WindowX = Application.Current.MainWindow.Left;
			}
			if (!double.IsNaN(Application.Current.MainWindow.Top))
			{
				WindowY = Application.Current.MainWindow.Top;
			}
			Save();
		}

		public void Save()
		{
			var serializer = new SerializerBuilder()
				.WithNamingConvention(UnderscoredNamingConvention.Instance)
				.Build();

			File.WriteAllText(SettingsPath, serializer.Serialize(this));
		}


		public static readonly string SettingsPath =
			Path.Combine(AppState.CurrentDirectory, "settings.yaml");

		public static void Load()
		{
			var deserializer = new DeserializerBuilder()
				.WithNamingConvention(UnderscoredNamingConvention.Instance)
				.Build();

			try
			{
				string text = null;
				for (var i = 0; i < 32; i += 1)
				{
					try
					{
						text = File.ReadAllText(SettingsPath)
							.Replace("\t", "  "); // The thing doesn't like tabs.
						break;
					}
					catch (Exception e)
					{

					}
				}

				AppState.Settings = deserializer.Deserialize<Settings>(text);
			}
			catch
			{
				AppState.Settings = new Settings();
			}
			AppState.Settings.Apply();
		}
	}
}
