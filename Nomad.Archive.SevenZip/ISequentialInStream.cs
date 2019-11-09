using System;
using System.Runtime.InteropServices;

namespace Nomad.Archive.SevenZip
{
	[Guid("23170F69-40C1-278A-0000-000300010000")]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	public interface ISequentialInStream
	{
		uint Read([Out] byte[] data, uint size);
	}
}