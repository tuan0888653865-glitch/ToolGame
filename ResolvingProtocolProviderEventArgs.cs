using System;

namespace TinhKiemAuto
{
	// Token: 0x020000D3 RID: 211
	internal class ResolvingProtocolProviderEventArgs : EventArgs
	{
		// Token: 0x06000B1C RID: 2844 RVA: 0x0004935E File Offset: 0x0004755E
		public ResolvingProtocolProviderEventArgs(IProtocolProvider provider, string url)
		{
			this.url = url;
			this.provider = provider;
		}

		// Token: 0x1700027D RID: 637
		// (get) Token: 0x06000B1D RID: 2845 RVA: 0x00049374 File Offset: 0x00047574
		public string URL
		{
			get
			{
				return this.url;
			}
		}

		// Token: 0x1700027E RID: 638
		// (get) Token: 0x06000B1E RID: 2846 RVA: 0x0004937C File Offset: 0x0004757C
		// (set) Token: 0x06000B1F RID: 2847 RVA: 0x00049384 File Offset: 0x00047584
		public IProtocolProvider ProtocolProvider
		{
			get
			{
				return this.provider;
			}
			set
			{
				this.provider = value;
			}
		}

		// Token: 0x040008A3 RID: 2211
		private IProtocolProvider provider;

		// Token: 0x040008A4 RID: 2212
		private string url;
	}
}
