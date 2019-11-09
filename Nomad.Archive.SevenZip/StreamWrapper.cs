using System;
using System.IO;
using System.Runtime.InteropServices;

namespace Nomad.Archive.SevenZip
{
	public class StreamWrapper : IDisposable
	{
		protected Stream BaseStream;

		protected StreamWrapper(Stream baseStream)
		{
			this.BaseStream = baseStream;
		}

		public void Dispose()
		{
			this.BaseStream.Close();
		}

		public virtual void Seek(long offset, uint seekOrigin, IntPtr newPosition)
		{
			long num = (long)((uint)this.BaseStream.Seek(offset, (SeekOrigin)seekOrigin));
			if (newPosition != IntPtr.Zero)
			{
				Marshal.WriteInt64(newPosition, num);
			}
		}
	}
}