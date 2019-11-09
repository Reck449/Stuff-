using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Nomad.Archive.SevenZip
{
	[Guid("23170F69-40C1-278A-0000-000600600000")]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	public interface IInArchive
	{
		void Close();

		[MethodImpl(MethodImplOptions.PreserveSig)]
		int Extract(uint[] indices, uint numItems, int testMode, IArchiveExtractCallback extractCallback);

		void GetArchiveProperty(uint propID, ref PropVariant value);

		void GetArchivePropertyInfo(uint index, string name, ref uint propID, ref ushort varType);

		uint GetNumberOfArchiveProperties();

		uint GetNumberOfItems();

		uint GetNumberOfProperties();

		void GetProperty(uint index, ItemPropId propID, ref PropVariant value);

		void GetPropertyInfo(uint index, out string name, out ItemPropId propID, out ushort varType);

		[MethodImpl(MethodImplOptions.PreserveSig)]
		int Open(IInStream stream, [In] ref ulong maxCheckStartPosition, IArchiveOpenCallback openArchiveCallback);
	}
}