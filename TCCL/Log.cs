using System;
using System.IO;

namespace TCCL
{
	public static class Log
	{
		private static string logFile;

		private static StreamWriter sw;

		static Log()
		{
			Log.logFile = string.Concat(Helper.StartUpPath, "\\TCCL.log");
		}

		public static void Close()
		{
			if (Log.sw != null)
			{
				Log.sw.Close();
			}
		}

		public static void Open()
		{
			Log.sw = new StreamWriter(Log.logFile, true);
		}

		public static void Write(Exception e)
		{
			try
			{
				StreamWriter streamWriter = Log.sw;
				string shortDateString = DateTime.Now.ToShortDateString();
				DateTime now = DateTime.Now;
				streamWriter.WriteLine(string.Concat(shortDateString, " | ", now.ToShortTimeString()));
				Log.sw.WriteLine(e.Message);
				Log.sw.WriteLine();
			}
			catch (Exception exception)
			{
			}
		}

		public static void Write(string s)
		{
			try
			{
				StreamWriter streamWriter = Log.sw;
				string shortDateString = DateTime.Now.ToShortDateString();
				DateTime now = DateTime.Now;
				streamWriter.WriteLine(string.Concat(shortDateString, " | ", now.ToShortTimeString()));
				Log.sw.WriteLine(s);
				Log.sw.WriteLine();
			}
			catch (Exception exception)
			{
			}
		}
	}
}