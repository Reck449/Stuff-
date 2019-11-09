using System;
using System.IO;

namespace Nomad.Archive.SevenZip
{
	public class InStreamWrapper : StreamWrapper, ISequentialInStream, IInStream
	{
		public InStreamWrapper(Stream baseStream) : base(baseStream)
		{
		}

		public uint Read(byte[] data, uint size)
		{
			return (uint)this.BaseStream.Read(data, 0, (int)size);
		}
	}
}