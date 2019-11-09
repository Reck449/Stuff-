using System;
using System.Runtime.InteropServices;

namespace Nomad.Archive.SevenZip
{
	[Guid("23170F69-40C1-278A-0000-000600100000")]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	public interface IArchiveOpenCallback
	{
		void SetCompleted(IntPtr files, IntPtr bytes);

		void SetTotal(IntPtr files, IntPtr bytes);
	}
}