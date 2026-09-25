using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.Resources;
using System.Runtime.CompilerServices;

namespace TinhKiemAuto.Properties
{
	// Token: 0x0200011D RID: 285
	[GeneratedCode("System.Resources.Tools.StronglyTypedResourceBuilder", "4.0.0.0")]
	[DebuggerNonUserCode]
	[CompilerGenerated]
	internal class Resources
	{
		// Token: 0x06000F3A RID: 3898 RVA: 0x000020C5 File Offset: 0x000002C5
		internal Resources()
		{
		}

		// Token: 0x1700033F RID: 831
		// (get) Token: 0x06000F3B RID: 3899 RVA: 0x0007411E File Offset: 0x0007231E
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		internal static ResourceManager ResourceManager
		{
			get
			{
				if (Resources.resourceMan == null)
				{
					Resources.resourceMan = new ResourceManager("TinhKiemAuto.Properties.Resources", typeof(Resources).Assembly);
				}
				return Resources.resourceMan;
			}
		}

		// Token: 0x17000340 RID: 832
		// (get) Token: 0x06000F3C RID: 3900 RVA: 0x0007414A File Offset: 0x0007234A
		// (set) Token: 0x06000F3D RID: 3901 RVA: 0x00074151 File Offset: 0x00072351
		[EditorBrowsable(EditorBrowsableState.Advanced)]
		internal static CultureInfo Culture
		{
			get
			{
				return Resources.resourceCulture;
			}
			set
			{
				Resources.resourceCulture = value;
			}
		}

		// Token: 0x17000341 RID: 833
		// (get) Token: 0x06000F3E RID: 3902 RVA: 0x00074159 File Offset: 0x00072359
		internal static Bitmap Cancel_50px
		{
			get
			{
				return (Bitmap)Resources.ResourceManager.GetObject("Cancel_50px", Resources.resourceCulture);
			}
		}

		// Token: 0x17000342 RID: 834
		// (get) Token: 0x06000F3F RID: 3903 RVA: 0x00074174 File Offset: 0x00072374
		internal static Bitmap List_50px
		{
			get
			{
				return (Bitmap)Resources.ResourceManager.GetObject("List_50px", Resources.resourceCulture);
			}
		}

		// Token: 0x17000343 RID: 835
		// (get) Token: 0x06000F40 RID: 3904 RVA: 0x0007418F File Offset: 0x0007238F
		internal static Bitmap Ok_50px
		{
			get
			{
				return (Bitmap)Resources.ResourceManager.GetObject("Ok_50px", Resources.resourceCulture);
			}
		}

		// Token: 0x17000344 RID: 836
		// (get) Token: 0x06000F41 RID: 3905 RVA: 0x000741AA File Offset: 0x000723AA
		internal static Bitmap Settings_48px
		{
			get
			{
				return (Bitmap)Resources.ResourceManager.GetObject("Settings_48px", Resources.resourceCulture);
			}
		}

		// Token: 0x04000CAD RID: 3245
		private static ResourceManager resourceMan;

		// Token: 0x04000CAE RID: 3246
		private static CultureInfo resourceCulture;
	}
}
