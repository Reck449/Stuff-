using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.Resources;
using System.Runtime.CompilerServices;

namespace TCCL
{
	[CompilerGenerated]
	[DebuggerNonUserCode]
	[GeneratedCode("System.Resources.Tools.StronglyTypedResourceBuilder", "4.0.0.0")]
	internal class Resource1
	{
		private static System.Resources.ResourceManager resourceMan;

		private static CultureInfo resourceCulture;

		[EditorBrowsable(EditorBrowsableState.Advanced)]
		internal static CultureInfo Culture
		{
			get
			{
				return Resource1.resourceCulture;
			}
			set
			{
				Resource1.resourceCulture = value;
			}
		}

		internal static Bitmap icon
		{
			get
			{
				return (Bitmap)Resource1.ResourceManager.GetObject("icon", Resource1.resourceCulture);
			}
		}

		internal static Bitmap logo
		{
			get
			{
				return (Bitmap)Resource1.ResourceManager.GetObject("logo", Resource1.resourceCulture);
			}
		}

		internal static Bitmap reload
		{
			get
			{
				return (Bitmap)Resource1.ResourceManager.GetObject("reload", Resource1.resourceCulture);
			}
		}

		[EditorBrowsable(EditorBrowsableState.Advanced)]
		internal static System.Resources.ResourceManager ResourceManager
		{
			get
			{
				if (object.ReferenceEquals(Resource1.resourceMan, null))
				{
					Resource1.resourceMan = new System.Resources.ResourceManager("TCCL.Resource1", typeof(Resource1).Assembly);
				}
				return Resource1.resourceMan;
			}
		}

		internal static Bitmap small_down_arrow
		{
			get
			{
				return (Bitmap)Resource1.ResourceManager.GetObject("small_down_arrow", Resource1.resourceCulture);
			}
		}

		internal static Bitmap tccl
		{
			get
			{
				return (Bitmap)Resource1.ResourceManager.GetObject("tccl", Resource1.resourceCulture);
			}
		}

		internal static Bitmap UpArrow
		{
			get
			{
				return (Bitmap)Resource1.ResourceManager.GetObject("UpArrow", Resource1.resourceCulture);
			}
		}

		internal Resource1()
		{
		}
	}
}