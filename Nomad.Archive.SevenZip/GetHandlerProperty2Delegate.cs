using System;
using System.Runtime.InteropServices;

namespace Nomad.Archive.SevenZip
{
	[UnmanagedFunctionPointer(CallingConvention.StdCall)]
	public delegate int GetHandlerProperty2Delegate(uint formatIndex, ArchivePropId propID, ref PropVariant value);
}