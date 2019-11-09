using System;
using System.Runtime.InteropServices;

namespace Nomad.Archive.SevenZip
{
	[UnmanagedFunctionPointer(CallingConvention.StdCall)]
	public delegate int CreateObjectDelegate([In] ref Guid classID, [In] ref Guid interfaceID, out object outObject);
}