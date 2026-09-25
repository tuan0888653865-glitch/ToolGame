using System;
using System.IO;

namespace TinhKiemAuto
{
	// Token: 0x020000AF RID: 175
	internal interface IProtocolProvider
	{
		// Token: 0x060009D8 RID: 2520
		void Initialize(Downloader downloader);

		// Token: 0x060009D9 RID: 2521
		Stream CreateStream(ResourceLocation rl, long initialPosition, long endPosition);

		// Token: 0x060009DA RID: 2522
		RemoteFileInfo GetFileInfo(ResourceLocation rl, out Stream stream);
	}
}
