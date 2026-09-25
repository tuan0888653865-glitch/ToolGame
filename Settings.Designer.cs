using System;
using System.CodeDom.Compiler;
using System.Configuration;
using System.Runtime.CompilerServices;

namespace TinhKiemAuto.Properties
{
	// Token: 0x0200011E RID: 286
	[CompilerGenerated]
	[GeneratedCode("Microsoft.VisualStudio.Editors.SettingsDesigner.SettingsSingleFileGenerator", "11.0.0.0")]
	internal sealed partial class Settings : ApplicationSettingsBase
	{
		// Token: 0x17000345 RID: 837
		// (get) Token: 0x06000F42 RID: 3906 RVA: 0x000741C5 File Offset: 0x000723C5
		public static Settings Default
		{
			get
			{
				return Settings.defaultInstance;
			}
		}

		// Token: 0x04000CAF RID: 3247
		private static Settings defaultInstance = (Settings)SettingsBase.Synchronized(new Settings());
	}
}
