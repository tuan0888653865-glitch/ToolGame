using System;
using System.Collections.Generic;

namespace TinhKiemAuto
{
	// Token: 0x020000DD RID: 221
	internal class SequentialMirrorSelector : IMirrorSelector
	{
		// Token: 0x06000B95 RID: 2965 RVA: 0x0004AA56 File Offset: 0x00048C56
		public void Init(Downloader downloader)
		{
			this.queryMirrorCount = 0;
			this.downloader = downloader;
		}

		// Token: 0x06000B96 RID: 2966 RVA: 0x0004AA68 File Offset: 0x00048C68
		public ResourceLocation GetNextResourceLocation()
		{
			if (this.downloader.Mirrors == null || this.downloader.Mirrors.Count == 0)
			{
				return this.downloader.ResourceLocation;
			}
			List<ResourceLocation> mirrors = this.downloader.Mirrors;
			ResourceLocation result;
			lock (mirrors)
			{
				if (this.queryMirrorCount >= this.downloader.Mirrors.Count)
				{
					this.queryMirrorCount = 0;
					result = this.downloader.ResourceLocation;
				}
				else
				{
					List<ResourceLocation> mirrors2 = this.downloader.Mirrors;
					int num = this.queryMirrorCount;
					this.queryMirrorCount = num + 1;
					result = mirrors2[num];
				}
			}
			return result;
		}

		// Token: 0x040008C7 RID: 2247
		private Downloader downloader;

		// Token: 0x040008C8 RID: 2248
		private int queryMirrorCount;
	}
}
