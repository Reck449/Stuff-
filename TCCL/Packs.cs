using System;
using System.Collections.Generic;

namespace TCCL
{
	public static class Packs
	{
		private static List<Pack> pack;

		public static int Count
		{
			get
			{
				return Packs.pack.Count;
			}
		}

		static Packs()
		{
			Packs.pack = new List<Pack>();
		}

		public static void Add(Pack p)
		{
			Packs.pack.Add(p);
		}

		public static void Clear()
		{
			Packs.pack.Clear();
		}

		public static string GetName(int i)
		{
			return Packs.pack[i].sName;
		}

		public static Pack GetPack(int i)
		{
			return Packs.pack[i];
		}

		public static List<Pack> GetPackArray()
		{
			return Packs.pack;
		}

		public static bool LoadFonts(int i)
		{
			return Packs.pack[i].bLoadFonts;
		}

		public static bool LoadMusic(int i)
		{
			return Packs.pack[i].bLoadMusic;
		}

		public static bool LoadPngs(int i)
		{
			return Packs.pack[i].bLoadPngs;
		}

		public static bool LoadScripts(int i)
		{
			return Packs.pack[i].bLoadScripts;
		}

		public static bool LoadSounds(int i)
		{
			return Packs.pack[i].bLoadSounds;
		}

		public static void MoveDown(string name)
		{
			for (int i = 0; i < Packs.pack.Count; i++)
			{
				if (Packs.pack[i].sName == name)
				{
					Packs.MoveDown(i);
					return;
				}
			}
		}

		public static void MoveDown(int i)
		{
			int num = i + 1;
			if (num == Packs.pack.Count)
			{
				return;
			}
			Pack item = Packs.pack[i];
			Packs.pack[i] = Packs.pack[num];
			Packs.pack[num] = item;
		}

		public static void MoveUp(int i)
		{
			int num = i - 1;
			if (num == -1)
			{
				return;
			}
			Pack item = Packs.pack[i];
			Packs.pack[i] = Packs.pack[num];
			Packs.pack[num] = item;
		}

		public static void MoveUp(string name)
		{
			for (int i = 0; i < Packs.pack.Count; i++)
			{
				if (Packs.pack[i].sName == name)
				{
					Packs.MoveUp(i);
					return;
				}
			}
		}

		public static void RemoveAt(int i)
		{
			if (Packs.pack.Count > 0)
			{
				Packs.pack.RemoveAt(i);
			}
		}

		public static void SetPackArray(List<Pack> packs)
		{
			Packs.pack = packs;
		}

		public static void Write()
		{
			for (int i = 0; i < Packs.pack.Count; i++)
			{
				Packs.pack[i].Write();
			}
		}
	}
}