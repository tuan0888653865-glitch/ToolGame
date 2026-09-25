using System;

namespace TinhKiemAuto
{
	// Token: 0x0200008C RID: 140
	internal class DownloaderEventArgs : EventArgs
	{
		// Token: 0x06000659 RID: 1625 RVA: 0x00024630 File Offset: 0x00022830
		public DownloaderEventArgs(Downloader download)
		{
			this.downloader = download;
		}

		// Token: 0x0600065A RID: 1626 RVA: 0x0002463F File Offset: 0x0002283F
		public DownloaderEventArgs(Downloader download, bool willStart) : this(download)
		{
			this.willStart = willStart;
		}

		// Token: 0x1700012F RID: 303
		// (get) Token: 0x0600065B RID: 1627 RVA: 0x0002464F File Offset: 0x0002284F
		public Downloader Downloader
		{
			get
			{
				return this.downloader;
			}
		}

		// Token: 0x17000130 RID: 304
		// (get) Token: 0x0600065C RID: 1628 RVA: 0x00024657 File Offset: 0x00022857
		public bool WillStart
		{
			get
			{
				return this.willStart;
			}
		}

		// Token: 0x0400040A RID: 1034
		private Downloader downloader;

		// Token: 0x0400040B RID: 1035
		private bool willStart;
	}
}
