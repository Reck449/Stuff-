using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Nomad.Archive.SevenZip
{
	[Guid("23170F69-40C1-278A-0000-000600300000")]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	public interface IArchiveOpenVolumeCallback
	{
		void GetProperty(ItemPropId propID, IntPtr value);

		[MethodImpl(MethodImplOptions.PreserveSig)]
		int GetStream(string name, out IInStream inStream);
	}
}