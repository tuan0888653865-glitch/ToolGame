using System;

namespace TinhKiemAuto
{
	// Token: 0x0200009B RID: 155
	internal interface IExtension
	{
		// Token: 0x17000249 RID: 585
		// (get) Token: 0x060009A5 RID: 2469
		string Name { get; }

		// Token: 0x1700024A RID: 586
		// (get) Token: 0x060009A6 RID: 2470
		IUIExtension UIExtension { get; }
	}
}
