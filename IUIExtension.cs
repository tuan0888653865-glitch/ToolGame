using System;
using System.Windows.Forms;

namespace TinhKiemAuto
{
	// Token: 0x020000B1 RID: 177
	internal interface IUIExtension
	{
		// Token: 0x060009DC RID: 2524
		Control[] CreateSettingsView();

		// Token: 0x060009DD RID: 2525
		void PersistSettings(Control[] settingsView);
	}
}
