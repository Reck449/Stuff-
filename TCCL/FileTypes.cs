using System;

namespace TCCL
{
	public sealed class FileTypes
	{
		public readonly static string png;

		public readonly static string wav;

		public readonly static string xwb;

		public readonly static string cs;

		public readonly static string txt;

		public readonly static string xnb;

		static FileTypes()
		{
			FileTypes.png = "png";
			FileTypes.wav = "wav";
			FileTypes.xwb = "xwb";
			FileTypes.cs = "cs";
			FileTypes.txt = "txt";
			FileTypes.xnb = "xnb";
		}

		private FileTypes()
		{
		}
	}
}