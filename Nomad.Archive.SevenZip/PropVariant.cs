using System;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;

namespace Nomad.Archive.SevenZip
{
	[StructLayout(LayoutKind.Explicit)]
	public struct PropVariant
	{
		[FieldOffset(0)]
		public ushort vt;

		[FieldOffset(8)]
		public IntPtr pointerValue;

		[FieldOffset(8)]
		public byte byteValue;

		[FieldOffset(8)]
		public long longValue;

		[FieldOffset(8)]
		public System.Runtime.InteropServices.ComTypes.FILETIME filetime;

		public VarEnum VarType
		{
			get
			{
				return (VarEnum)this.vt;
			}
		}

		public void Clear()
		{
			int num;
			VarEnum varType = this.VarType;
			switch (varType)
			{
				case VarEnum.VT_EMPTY:
				{
					return;
				}
				case VarEnum.VT_NULL:
				case VarEnum.VT_I2:
				case VarEnum.VT_I4:
				case VarEnum.VT_R4:
				case VarEnum.VT_R8:
				case VarEnum.VT_CY:
				case VarEnum.VT_DATE:
				case VarEnum.VT_ERROR:
				case VarEnum.VT_BOOL:
				case VarEnum.VT_I1:
				case VarEnum.VT_UI1:
				case VarEnum.VT_UI2:
				case VarEnum.VT_UI4:
				case VarEnum.VT_I8:
				case VarEnum.VT_UI8:
				case VarEnum.VT_INT:
				case VarEnum.VT_UINT:
				case VarEnum.VT_HRESULT:
				{
					this.vt = 0;
					return;
				}
				case VarEnum.VT_BSTR:
				case VarEnum.VT_DISPATCH:
				case VarEnum.VT_VARIANT:
				case VarEnum.VT_UNKNOWN:
				case VarEnum.VT_DECIMAL:
				case VarEnum.VT_NULL | VarEnum.VT_I2 | VarEnum.VT_I4 | VarEnum.VT_R4 | VarEnum.VT_R8 | VarEnum.VT_CY | VarEnum.VT_DATE | VarEnum.VT_BSTR | VarEnum.VT_DISPATCH | VarEnum.VT_ERROR | VarEnum.VT_BOOL | VarEnum.VT_VARIANT | VarEnum.VT_UNKNOWN | VarEnum.VT_DECIMAL:
				case VarEnum.VT_VOID:
				{
					num = PropVariant.PropVariantClear(this);
					return;
				}
				default:
				{
					if (varType != VarEnum.VT_FILETIME)
					{
						num = PropVariant.PropVariantClear(this);
						return;
					}
					else
					{
						this.vt = 0;
						return;
					}
				}
			}
		}

		public object GetObject()
		{
			object objectForNativeVariant;
			VarEnum varType = this.VarType;
			if (varType == VarEnum.VT_EMPTY)
			{
				return null;
			}
			if (varType == VarEnum.VT_FILETIME)
			{
				return DateTime.FromFileTime(this.longValue);
			}
			GCHandle gCHandle = GCHandle.Alloc(this, GCHandleType.Pinned);
			try
			{
				objectForNativeVariant = Marshal.GetObjectForNativeVariant(gCHandle.AddrOfPinnedObject());
			}
			finally
			{
				gCHandle.Free();
			}
			return objectForNativeVariant;
		}

		[DllImport("ole32.dll", CharSet=CharSet.None, ExactSpelling=false)]
		private static extern int PropVariantClear(ref PropVariant pvar);
	}
}