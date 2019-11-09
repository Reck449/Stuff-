using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

namespace TCCL
{
	public sealed class Helper
	{
		public static bool Redraw;

		public static bool updateList;

		public static int what;

		public static string StartUpPath;

		public static bool isLoading;

		public static List<Pack> tempPacks;

		static Helper()
		{
			Helper.Redraw = false;
			Helper.updateList = false;
			Helper.what = -1;
			Helper.StartUpPath = "";
			Helper.isLoading = true;
		}

		private Helper()
		{
		}

		public static Texture2D LoadTextureFromRessource(Game game, Bitmap b)
		{
			MemoryStream memoryStream = new MemoryStream();
			b.Save(memoryStream, ImageFormat.Png);
			memoryStream.Position = (long)0;
			return Texture2D.FromStream(game.GraphicsDevice, memoryStream);
		}

		public static bool ProcessRunning(string processName)
		{
			Process[] processes = Process.GetProcesses();
			for (int i = 0; i < (int)processes.Length; i++)
			{
				if (processes[i].ProcessName.ToLower().Contains(processName.ToLower()))
				{
					return true;
				}
			}
			return false;
		}

		public static void RunTetrarria()
		{
			if (File.Exists(string.Concat(Directory.GetCurrentDirectory(), "\\Tetrarria.exe")))
			{
				Process.Start(string.Concat(Directory.GetCurrentDirectory(), "\\Tetrarria.exe"));
			}
		}
	}
}