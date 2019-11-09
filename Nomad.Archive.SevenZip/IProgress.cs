using System;
using System.Runtime.InteropServices;

namespace Nomad.Archive.SevenZip
{
	[Guid("23170F69-40C1-278A-0000-000000050000")]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	public interface IProgress
	{
		void SetCompleted([In] ref ulong completeValue);

		void SetTotal(ulong total);
	}
}