using System;

namespace Nomad.Archive.SevenZip
{
	public enum OperationResult
	{
		kOK,
		kUnSupportedMethod,
		kDataError,
		kCRCError
	}
}