using Microsoft.Win32.SafeHandles;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.ConstrainedExecution;
using System.Runtime.InteropServices;
using System.Security;

namespace Nomad.Archive.SevenZip
{
	public class SevenZipFormat : IDisposable
	{
		private const string Kernel32Dll = "kernel32.dll";

		private SevenZipFormat.SafeLibraryHandle LibHandle;

		private static Dictionary<KnownSevenZipFormat, Guid> FFormatClassMap;

		private static Dictionary<KnownSevenZipFormat, Guid> FormatClassMap
		{
			get
			{
				if (SevenZipFormat.FFormatClassMap == null)
				{
					SevenZipFormat.FFormatClassMap = new Dictionary<KnownSevenZipFormat, Guid>()
					{
						{ KnownSevenZipFormat.SevenZip, new Guid("23170f69-40c1-278a-1000-000110070000") },
						{ KnownSevenZipFormat.Arj, new Guid("23170f69-40c1-278a-1000-000110040000") },
						{ KnownSevenZipFormat.BZip2, new Guid("23170f69-40c1-278a-1000-000110020000") },
						{ KnownSevenZipFormat.Cab, new Guid("23170f69-40c1-278a-1000-000110080000") },
						{ KnownSevenZipFormat.Chm, new Guid("23170f69-40c1-278a-1000-000110e90000") },
						{ KnownSevenZipFormat.Compound, new Guid("23170f69-40c1-278a-1000-000110e50000") },
						{ KnownSevenZipFormat.Cpio, new Guid("23170f69-40c1-278a-1000-000110ed0000") },
						{ KnownSevenZipFormat.Deb, new Guid("23170f69-40c1-278a-1000-000110ec0000") },
						{ KnownSevenZipFormat.GZip, new Guid("23170f69-40c1-278a-1000-000110ef0000") },
						{ KnownSevenZipFormat.Iso, new Guid("23170f69-40c1-278a-1000-000110e70000") },
						{ KnownSevenZipFormat.Lzh, new Guid("23170f69-40c1-278a-1000-000110060000") },
						{ KnownSevenZipFormat.Lzma, new Guid("23170f69-40c1-278a-1000-0001100a0000") },
						{ KnownSevenZipFormat.Nsis, new Guid("23170f69-40c1-278a-1000-000110090000") },
						{ KnownSevenZipFormat.Rar, new Guid("23170f69-40c1-278a-1000-000110030000") },
						{ KnownSevenZipFormat.Rpm, new Guid("23170f69-40c1-278a-1000-000110eb0000") },
						{ KnownSevenZipFormat.Split, new Guid("23170f69-40c1-278a-1000-000110ea0000") },
						{ KnownSevenZipFormat.Tar, new Guid("23170f69-40c1-278a-1000-000110ee0000") },
						{ KnownSevenZipFormat.Wim, new Guid("23170f69-40c1-278a-1000-000110e60000") },
						{ KnownSevenZipFormat.Z, new Guid("23170f69-40c1-278a-1000-000110050000") },
						{ KnownSevenZipFormat.Zip, new Guid("23170f69-40c1-278a-1000-000110010000") }
					};
				}
				return SevenZipFormat.FFormatClassMap;
			}
		}

		public SevenZipFormat(string sevenZipLibPath)
		{
			this.LibHandle = SevenZipFormat.LoadLibrary(sevenZipLibPath);
			if (this.LibHandle.IsInvalid)
			{
				throw new Win32Exception();
			}
			if (SevenZipFormat.GetProcAddress(this.LibHandle, "GetHandlerProperty") == IntPtr.Zero)
			{
				this.LibHandle.Close();
				throw new ArgumentException();
			}
		}

		public IInArchive CreateInArchive(Guid classId)
		{
			object obj = null;
			if (this.LibHandle == null)
			{
				throw new ObjectDisposedException("SevenZipFormat");
			}
			CreateObjectDelegate delegateForFunctionPointer = (CreateObjectDelegate)Marshal.GetDelegateForFunctionPointer(SevenZipFormat.GetProcAddress(this.LibHandle, "CreateObject"), typeof(CreateObjectDelegate));
			if (delegateForFunctionPointer == null)
			{
				return null;
			}
			Guid gUID = typeof(IInArchive).GUID;
			delegateForFunctionPointer(ref classId, ref gUID, out obj);
			return obj as IInArchive;
		}

		protected void Dispose(bool disposing)
		{
			if (this.LibHandle != null && !this.LibHandle.IsClosed)
			{
				this.LibHandle.Close();
			}
			this.LibHandle = null;
		}

		public void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		~SevenZipFormat()
		{
			this.Dispose(false);
		}

		public static Guid GetClassIdFromKnownFormat(KnownSevenZipFormat format)
		{
			Guid guid;
			if (SevenZipFormat.FormatClassMap.TryGetValue(format, out guid))
			{
				return guid;
			}
			return Guid.Empty;
		}

		[DllImport("kernel32.dll", CharSet=CharSet.Ansi, ExactSpelling=false, SetLastError=true)]
		private static extern IntPtr GetProcAddress(SevenZipFormat.SafeLibraryHandle hModule, string procName);

		[DllImport("kernel32.dll", CharSet=CharSet.Auto, ExactSpelling=false, SetLastError=true)]
		private static extern SevenZipFormat.SafeLibraryHandle LoadLibrary(string lpFileName);

		private sealed class SafeLibraryHandle : SafeHandleZeroOrMinusOneIsInvalid
		{
			public SafeLibraryHandle() : base(true)
			{
			}

			[DllImport("kernel32.dll", CharSet=CharSet.None, ExactSpelling=false)]
			[SuppressUnmanagedCodeSecurity]
			private static extern bool FreeLibrary(IntPtr hModule);

			[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
			protected override bool ReleaseHandle()
			{
				return SevenZipFormat.SafeLibraryHandle.FreeLibrary(this.handle);
			}
		}
	}
}