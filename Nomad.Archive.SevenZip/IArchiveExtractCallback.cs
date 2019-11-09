using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Nomad.Archive.SevenZip
{
	[Guid("23170F69-40C1-278A-0000-000600200000")]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	public interface IArchiveExtractCallback
	{
		[MethodImpl(MethodImplOptions.PreserveSig)]
		int GetStream(uint index, out ISequentialOutStream outStream, AskMode askExtractMode);

		void PrepareOperation(AskMode askExtractMode);

		void SetCompleted([In] ref ulong completeValue);

		void SetOperationResult(OperationResult resultEOperationResult);

		void SetTotal(ulong total);
	}
}