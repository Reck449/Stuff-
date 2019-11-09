using System;

namespace Nomad.Archive.SevenZip
{
	public enum ItemPropId : uint
	{
		kpidNoProperty = 0,
		kpidHandlerItemIndex = 2,
		kpidPath = 3,
		kpidName = 4,
		kpidExtension = 5,
		kpidIsFolder = 6,
		kpidSize = 7,
		kpidPackedSize = 8,
		kpidAttributes = 9,
		kpidCreationTime = 10,
		kpidLastAccessTime = 11,
		kpidLastWriteTime = 12,
		kpidSolid = 13,
		kpidCommented = 14,
		kpidEncrypted = 15,
		kpidSplitBefore = 16,
		kpidSplitAfter = 17,
		kpidDictionarySize = 18,
		kpidCRC = 19,
		kpidType = 20,
		kpidIsAnti = 21,
		kpidMethod = 22,
		kpidHostOS = 23,
		kpidFileSystem = 24,
		kpidUser = 25,
		kpidGroup = 26,
		kpidBlock = 27,
		kpidComment = 28,
		kpidPosition = 29,
		kpidPrefix = 30,
		kpidTotalSize = 4352,
		kpidFreeSpace = 4353,
		kpidClusterSize = 4354,
		kpidVolumeName = 4355,
		kpidLocalName = 4608,
		kpidProvider = 4609,
		kpidUserDefined = 65536
	}
}