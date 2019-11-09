using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Nomad.Archive.SevenZip
{
	[Guid("23170F69-40C1-278A-0000-000500100000")]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	public interface ICryptoGetTextPassword
	{
		[MethodImpl(MethodImplOptions.PreserveSig)]
		int CryptoGetTextPassword(out string password);
	}
}