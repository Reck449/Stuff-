using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Nomad.Archive.SevenZip
{
	[Guid("23170F69-40C1-278A-0000-000300020000")]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	public interface ISequentialOutStream
	{
		[MethodImpl(MethodImplOptions.PreserveSig)]
		int Write([In] byte[] data, uint size, IntPtr processedSize);
	}
}