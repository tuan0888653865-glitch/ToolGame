using System;

namespace TinhKiemAuto
{
	// Token: 0x020000DB RID: 219
	internal class SegmentEventArgs : DownloaderEventArgs
	{
		// Token: 0x06000B92 RID: 2962 RVA: 0x0004AA35 File Offset: 0x00048C35
		public SegmentEventArgs(Downloader d, Segment segment) : base(d)
		{
			this.segment = segment;
		}

		// Token: 0x170002B3 RID: 691
		// (get) Token: 0x06000B93 RID: 2963 RVA: 0x0004AA45 File Offset: 0x00048C45
		// (set) Token: 0x06000B94 RID: 2964 RVA: 0x0004AA4D File Offset: 0x00048C4D
		public Segment Segment
		{
			get
			{
				return this.segment;
			}
			set
			{
				this.segment = value;
			}
		}

		// Token: 0x040008BF RID: 2239
		private Segment segment;
	}
}
