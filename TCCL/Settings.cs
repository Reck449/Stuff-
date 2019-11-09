using System;
using System.IO;

namespace TCCL
{
	public static class Settings
	{
		private static string oldconfig;

		public static string config;

		private static StreamReader sr;

		private static StreamWriter sw;

		private static bool bShowGui;

		private static bool bAutoUpdate;

		private static bool bSkipSplash;

		public static bool AutoUpdate
		{
			get
			{
				return Settings.bAutoUpdate;
			}
			set
			{
				Settings.bAutoUpdate = value;
			}
		}

		public static bool ShowGui
		{
			get
			{
				return Settings.bShowGui;
			}
			set
			{
				Settings.bShowGui = value;
			}
		}

		public static bool SkipSplash
		{
			get
			{
				return Settings.bSkipSplash;
			}
			set
			{
				Settings.bSkipSplash = value;
			}
		}

		static Settings()
		{
			Settings.oldconfig = string.Concat(Helper.StartUpPath, "\\TerrariaCustomContentLoader.cfg");
			Settings.config = string.Concat(Helper.StartUpPath, "\\TCCL.cfg");
			Settings.bShowGui = true;
			Settings.bAutoUpdate = false;
			Settings.bSkipSplash = false;
		}

		private static bool GetBoolValue(string str)
		{
			if (str.ToLower().Contains("true"))
			{
				return true;
			}
			return false;
		}

		public static void Read()
		{
			if (File.Exists(Settings.oldconfig))
			{
				try
				{
					File.Move(Settings.oldconfig, Settings.config);
				}
				catch (Exception exception)
				{
				}
			}
			if (!File.Exists(Settings.config))
			{
				return;
			}
			Settings.sr = new StreamReader(Settings.config);
			try
			{
				try
				{
					if (!Settings.sr.EndOfStream)
					{
						Settings.ShowGui = Settings.GetBoolValue(Settings.sr.ReadLine());
					}
					if (!Settings.sr.EndOfStream)
					{
						Settings.SkipSplash = Settings.GetBoolValue(Settings.sr.ReadLine());
					}
					if (!Settings.sr.EndOfStream)
					{
						Settings.AutoUpdate = Settings.GetBoolValue(Settings.sr.ReadLine());
					}
				}
				catch (Exception exception1)
				{
					Log.Write(exception1);
				}
			}
			finally
			{
				if (Settings.sr != null)
				{
					Settings.sr.Close();
				}
			}
		}

		public static void Write()
		{
			Settings.sw = new StreamWriter(Settings.config);
			try
			{
				try
				{
					StreamWriter streamWriter = Settings.sw;
					bool showGui = Settings.ShowGui;
					streamWriter.WriteLine(string.Concat("ShowGui = ", showGui.ToString()));
					StreamWriter streamWriter1 = Settings.sw;
					bool skipSplash = Settings.SkipSplash;
					streamWriter1.WriteLine(string.Concat("Skip Splash = ", skipSplash.ToString()));
				}
				catch (Exception exception)
				{
					Log.Write(exception);
				}
			}
			finally
			{
				Settings.sw.Close();
			}
		}
	}
}