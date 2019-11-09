using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Reflection;
using System.Windows.Forms;

namespace TCCL
{
	public sealed class Update
	{
		public Update()
		{
		}

		public static bool CheckForUpdate()
		{
			bool flag;
			try
			{
				Stream stream = (new WebClient()).OpenRead("http://dl.dropbox.com/u/24414601/Terraria/TCCL%20Install/version.txt");
				string str = (new StreamReader(stream)).ReadLine();
				stream.Close();
				AssemblyName name = Assembly.GetExecutingAssembly().GetName();
				string str1 = name.Version.ToString();
				if (Update.GetVersion(str) <= Update.GetVersion(str1))
				{
					return false;
				}
				else
				{
					flag = (MessageBox.Show(string.Concat("New Version (", str, ") found. \nDownload Update?"), Application.ProductName, MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes ? false : true);
				}
			}
			catch (Exception exception)
			{
				TCCL.Log.Write(exception);
				return false;
			}
			return flag;
		}

		public static bool DownloadUpdate()
		{
			bool flag;
			WebClient webClient = new WebClient();
			try
			{
				webClient.DownloadFile("http://dl.dropbox.com/u/24414601/Terraria/TCCL%20Install/TCCLInstaller.exe", Path.Combine(Path.GetTempPath(), "TCCLInstaller.exe"));
				Process.Start(Path.Combine(Path.GetTempPath(), "TCCLInstaller.exe"));
				flag = true;
			}
			catch (Exception exception1)
			{
				Exception exception = exception1;
				TCCL.Log.Write(exception);
				MessageBox.Show(string.Concat("Download failed\n", exception.Message), "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);
				flag = false;
			}
			return flag;
		}

		private static int GetVersion(string v)
		{
			return Convert.ToInt32(v.Replace(".", ""));
		}
	}
}