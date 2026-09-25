using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.Resources;
using System.Runtime.CompilerServices;

namespace TinhKiemAuto
{
	// Token: 0x020000FD RID: 253
	[GeneratedCode("System.Resources.Tools.StronglyTypedResourceBuilder", "4.0.0.0")]
	[DebuggerNonUserCode]
	[CompilerGenerated]
	public class ImageResource
	{
		// Token: 0x06000E80 RID: 3712 RVA: 0x000020C5 File Offset: 0x000002C5
		internal ImageResource()
		{
		}

		// Token: 0x1700032D RID: 813
		// (get) Token: 0x06000E81 RID: 3713 RVA: 0x0006F384 File Offset: 0x0006D584
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		public static ResourceManager ResourceManager
		{
			get
			{
				if (ImageResource.resourceMan == null)
				{
					ImageResource.resourceMan = new ResourceManager("TinhKiemAuto.ImageResource", typeof(ImageResource).Assembly);
				}
				return ImageResource.resourceMan;
			}
		}

		// Token: 0x1700032E RID: 814
		// (get) Token: 0x06000E82 RID: 3714 RVA: 0x0006F3B0 File Offset: 0x0006D5B0
		// (set) Token: 0x06000E83 RID: 3715 RVA: 0x0006F3B7 File Offset: 0x0006D5B7
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		public static CultureInfo Culture
		{
			get
			{
				return ImageResource.resourceCulture;
			}
			set
			{
				ImageResource.resourceCulture = value;
			}
		}

		// Token: 0x1700032F RID: 815
		// (get) Token: 0x06000E84 RID: 3716 RVA: 0x0006F3BF File Offset: 0x0006D5BF
		public static Bitmap captcha
		{
			get
			{
				return (Bitmap)ImageResource.ResourceManager.GetObject("captcha", ImageResource.resourceCulture);
			}
		}

		// Token: 0x17000330 RID: 816
		// (get) Token: 0x06000E85 RID: 3717 RVA: 0x0006F3DA File Offset: 0x0006D5DA
		public static string Fix2D
		{
			get
			{
				return ImageResource.ResourceManager.GetString("Fix2D", ImageResource.resourceCulture);
			}
		}

		// Token: 0x17000331 RID: 817
		// (get) Token: 0x06000E86 RID: 3718 RVA: 0x0006F3F0 File Offset: 0x0006D5F0
		public static string Fix3D
		{
			get
			{
				return ImageResource.ResourceManager.GetString("Fix3D", ImageResource.resourceCulture);
			}
		}

		// Token: 0x17000332 RID: 818
		// (get) Token: 0x06000E87 RID: 3719 RVA: 0x0006F406 File Offset: 0x0006D606
		public static string FixDG
		{
			get
			{
				return ImageResource.ResourceManager.GetString("FixDG", ImageResource.resourceCulture);
			}
		}

		// Token: 0x17000333 RID: 819
		// (get) Token: 0x06000E88 RID: 3720 RVA: 0x0006F41C File Offset: 0x0006D61C
		public static Bitmap loading
		{
			get
			{
				return (Bitmap)ImageResource.ResourceManager.GetObject("loading", ImageResource.resourceCulture);
			}
		}

		// Token: 0x17000334 RID: 820
		// (get) Token: 0x06000E89 RID: 3721 RVA: 0x0006F437 File Offset: 0x0006D637
		public static string Lua
		{
			get
			{
				return ImageResource.ResourceManager.GetString("Lua", ImageResource.resourceCulture);
			}
		}

		// Token: 0x17000335 RID: 821
		// (get) Token: 0x06000E8A RID: 3722 RVA: 0x0006F44D File Offset: 0x0006D64D
		public static string LuaEx
		{
			get
			{
				return ImageResource.ResourceManager.GetString("LuaEx", ImageResource.resourceCulture);
			}
		}

		// Token: 0x17000336 RID: 822
		// (get) Token: 0x06000E8B RID: 3723 RVA: 0x0006F463 File Offset: 0x0006D663
		public static Bitmap refresh
		{
			get
			{
				return (Bitmap)ImageResource.ResourceManager.GetObject("refresh", ImageResource.resourceCulture);
			}
		}

		// Token: 0x17000337 RID: 823
		// (get) Token: 0x06000E8C RID: 3724 RVA: 0x0006F47E File Offset: 0x0006D67E
		public static Bitmap sound
		{
			get
			{
				return (Bitmap)ImageResource.ResourceManager.GetObject("sound", ImageResource.resourceCulture);
			}
		}

		// Token: 0x04000C02 RID: 3074
		private static ResourceManager resourceMan;

		// Token: 0x04000C03 RID: 3075
		private static CultureInfo resourceCulture;
	}
}
