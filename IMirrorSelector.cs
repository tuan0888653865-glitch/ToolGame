using System;

namespace TinhKiemAuto
{
	// Token: 0x020000AB RID: 171
	internal interface IMirrorSelector
	{
		// Token: 0x060009B9 RID: 2489
		void Init(Downloader downloader);

		// Token: 0x060009BA RID: 2490
		ResourceLocation GetNextResourceLocation();
	}
}
