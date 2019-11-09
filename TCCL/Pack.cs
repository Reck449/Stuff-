using System;
using System.IO;
using System.Text;

namespace TCCL
{
	public class Pack : IComparable
	{
		public string sName = "";

		public string sDescription = "Insert a small Description here";

		public string sAuthor = "Insert Author Name here";

		public string sHomepage = "Insert an Url here";

		public string sHomepageShortName = "Insert short name of your Homepage here";

		public bool bLoadPngs;

		public bool bLoadFonts;

		public bool bLoadSounds;

		public bool bLoadMusic;

		public bool bLoadScripts;

		public int priority;

		private static string config;

		private static StreamReader sr;

		private static StreamWriter sw;

		static Pack()
		{
			Pack.config = "pack.txt";
		}

		public Pack()
		{
		}

		public int CompareTo(object o)
		{
			if (!(o is Pack))
			{
				return 0;
			}
			return this.priority - ((Pack)o).priority;
		}

		private static bool GetBoolValue(string str)
		{
			if (str.ToLower().Contains("true"))
			{
				return true;
			}
			return false;
		}

		private string GetInfo(string str)
		{
			if (str.Contains("Description = "))
			{
				return str.Replace("Description = ", "");
			}
			if (str.Contains("Author = "))
			{
				return str.Replace("Author = ", "");
			}
			if (str.Contains("Homepage = "))
			{
				return str.Replace("Homepage = ", "");
			}
			if (!str.Contains("Homepage Shortname = "))
			{
				return str;
			}
			return str.Replace("Homepage Shortname = ", "");
		}

		private static int GetIntValue(string str)
		{
			if (!str.Contains("Priority = "))
			{
				return -1;
			}
			return Convert.ToInt32(str.Replace("Priority = ", ""));
		}

		public void Read()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append(Helper.StartUpPath);
			stringBuilder.Append("\\Custom Content\\");
			stringBuilder.Append(this.sName);
			stringBuilder.Append("\\");
			stringBuilder.Append(Pack.config);
			if (!File.Exists(stringBuilder.ToString()))
			{
				return;
			}
			Pack.sr = new StreamReader(stringBuilder.ToString());
			try
			{
				try
				{
					this.sDescription = this.GetInfo(Pack.sr.ReadLine());
					this.sAuthor = this.GetInfo(Pack.sr.ReadLine());
					this.sHomepage = this.GetInfo(Pack.sr.ReadLine());
					this.sHomepageShortName = this.GetInfo(Pack.sr.ReadLine());
					this.bLoadPngs = Pack.GetBoolValue(Pack.sr.ReadLine());
					this.bLoadFonts = Pack.GetBoolValue(Pack.sr.ReadLine());
					this.bLoadSounds = Pack.GetBoolValue(Pack.sr.ReadLine());
					this.bLoadMusic = Pack.GetBoolValue(Pack.sr.ReadLine());
					this.bLoadScripts = Pack.GetBoolValue(Pack.sr.ReadLine());
					if (!Pack.sr.EndOfStream)
					{
						this.priority = Pack.GetIntValue(Pack.sr.ReadLine());
					}
				}
				catch (Exception exception)
				{
					Log.Write(exception);
				}
			}
			finally
			{
				if (Pack.sr != null)
				{
					Pack.sr.Close();
				}
			}
		}

		public void Write()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append(Helper.StartUpPath);
			stringBuilder.Append("\\Custom Content\\");
			stringBuilder.Append(this.sName);
			stringBuilder.Append("\\");
			stringBuilder.Append(Pack.config);
			Pack.sw = new StreamWriter(stringBuilder.ToString());
			try
			{
				try
				{
					Pack.sw.WriteLine(string.Concat("Description = ", this.sDescription));
					Pack.sw.WriteLine(string.Concat("Author = ", this.sAuthor));
					Pack.sw.WriteLine(string.Concat("Homepage = ", this.sHomepage));
					Pack.sw.WriteLine(string.Concat("Homepage Shortname = ", this.sHomepageShortName));
					Pack.sw.WriteLine(string.Concat("Load Pngs = ", this.bLoadPngs.ToString()));
					Pack.sw.WriteLine(string.Concat("Load Fonts = ", this.bLoadFonts.ToString()));
					Pack.sw.WriteLine(string.Concat("Load Sounds = ", this.bLoadSounds.ToString()));
					Pack.sw.WriteLine(string.Concat("Load Music = ", this.bLoadMusic.ToString()));
					Pack.sw.WriteLine(string.Concat("Load Scripts = ", this.bLoadScripts.ToString()));
					Pack.sw.WriteLine(string.Concat("Priority = ", this.priority.ToString()));
				}
				catch (Exception exception)
				{
					Log.Write(exception);
				}
			}
			finally
			{
				Pack.sw.Close();
			}
		}
	}
}