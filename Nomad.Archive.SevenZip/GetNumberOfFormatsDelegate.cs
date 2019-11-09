using System;
using System.Runtime.InteropServices;

namespace Nomad.Archive.SevenZip
{
	[UnmanagedFunctionPointer(CallingConvention.StdCall)]
	public delegate int GetNumberOfFormatsDelegate(out uint numFormats);
}