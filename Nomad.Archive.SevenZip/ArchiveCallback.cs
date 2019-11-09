using System;
using System.IO;

namespace Nomad.Archive.SevenZip
{
	internal class ArchiveCallback : IArchiveExtractCallback
	{
		private uint FileNumber;

		private string FileName;

		private OutStreamWrapper FileStream;

		public ArchiveCallback(uint fileNumber, string fileName)
		{
			this.FileNumber = fileNumber;
			this.FileName = fileName;
		}

		public int GetStream(uint index, out ISequentialOutStream outStream, AskMode askExtractMode)
		{
			if (index != this.FileNumber || askExtractMode != AskMode.kExtract)
			{
				outStream = null;
			}
			else
			{
				string directoryName = Path.GetDirectoryName(this.FileName);
				if (!string.IsNullOrEmpty(directoryName))
				{
					Directory.CreateDirectory(directoryName);
				}
				this.FileStream = new OutStreamWrapper(File.Create(this.FileName));
				outStream = this.FileStream;
			}
			return 0;
		}

		public void PrepareOperation(AskMode askExtractMode)
		{
		}

		public void SetCompleted(ref ulong completeValue)
		{
		}

		public void SetOperationResult(OperationResult resultEOperationResult)
		{
			this.FileStream.Dispose();
			Console.WriteLine(resultEOperationResult);
		}

		public void SetTotal(ulong total)
		{
		}
	}
}