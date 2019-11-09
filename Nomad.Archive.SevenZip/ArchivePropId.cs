using System;

namespace Nomad.Archive.SevenZip
{
	public enum ArchivePropId : uint
	{
		kName,
		kClassID,
		kExtension,
		kAddExtension,
		kUpdate,
		kKeepName,
		kStartSignature,
		kFinishSignature,
		kAssociate
	}
}