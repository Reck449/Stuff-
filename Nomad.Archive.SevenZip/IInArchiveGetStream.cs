using System;
using System.Runtime.InteropServices;

namespace Nomad.Archive.SevenZip
{
	[Guid("23170F69-40C1-278A-0000-000600400000")]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	public interface IInArchiveGetStream
	{
		ISequentialInStream GetStream(uint index);
	}
}