using System;
using System.Runtime.InteropServices;

namespace Nomad.Archive.SevenZip
{
	[UnmanagedFunctionPointer(CallingConvention.StdCall)]
	public delegate int GetHandlerPropertyDelegate(ArchivePropId propID, ref PropVariant value);
}