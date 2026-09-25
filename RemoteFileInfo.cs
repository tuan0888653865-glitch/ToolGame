using System;

namespace TinhKiemAuto
{
	// Token: 0x020000D2 RID: 210
	[Serializable]
	internal class RemoteFileInfo
	{
		// Token: 0x17000279 RID: 633
		// (get) Token: 0x06000B13 RID: 2835 RVA: 0x00049307 File Offset: 0x00047507
		// (set) Token: 0x06000B14 RID: 2836 RVA: 0x0004930F File Offset: 0x0004750F
		public string MimeType
		{
			get
			{
				return this.mimeType;
			}
			set
			{
				this.mimeType = value;
			}
		}

		// Token: 0x1700027A RID: 634
		// (get) Token: 0x06000B15 RID: 2837 RVA: 0x00049318 File Offset: 0x00047518
		// (set) Token: 0x06000B16 RID: 2838 RVA: 0x00049320 File Offset: 0x00047520
		public bool AcceptRanges
		{
			get
			{
				return this.acceptRanges;
			}
			set
			{
				this.acceptRanges = value;
			}
		}

		// Token: 0x1700027B RID: 635
		// (get) Token: 0x06000B17 RID: 2839 RVA: 0x00049329 File Offset: 0x00047529
		// (set) Token: 0x06000B18 RID: 2840 RVA: 0x00049331 File Offset: 0x00047531
		public long FileSize
		{
			get
			{
				return this.fileSize;
			}
			set
			{
				this.fileSize = value;
			}
		}

		// Token: 0x1700027C RID: 636
		// (get) Token: 0x06000B19 RID: 2841 RVA: 0x0004933A File Offset: 0x0004753A
		// (set) Token: 0x06000B1A RID: 2842 RVA: 0x00049342 File Offset: 0x00047542
		public DateTime LastModified
		{
			get
			{
				return this.lastModified;
			}
			set
			{
				this.lastModified = value;
			}
		}

		// Token: 0x0400089F RID: 2207
		private bool acceptRanges;

		// Token: 0x040008A0 RID: 2208
		private long fileSize;

		// Token: 0x040008A1 RID: 2209
		private DateTime lastModified = DateTime.MinValue;

		// Token: 0x040008A2 RID: 2210
		private string mimeType;
	}
}
