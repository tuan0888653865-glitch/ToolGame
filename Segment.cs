using System;
using System.IO;

namespace TinhKiemAuto
{
	// Token: 0x020000DA RID: 218
	internal class Segment
	{
		// Token: 0x170002A2 RID: 674
		// (get) Token: 0x06000B74 RID: 2932 RVA: 0x0004A753 File Offset: 0x00048953
		// (set) Token: 0x06000B75 RID: 2933 RVA: 0x0004A75B File Offset: 0x0004895B
		public int CurrentTry
		{
			get
			{
				return this.currentTry;
			}
			set
			{
				this.currentTry = value;
			}
		}

		// Token: 0x170002A3 RID: 675
		// (get) Token: 0x06000B76 RID: 2934 RVA: 0x0004A764 File Offset: 0x00048964
		// (set) Token: 0x06000B77 RID: 2935 RVA: 0x0004A76C File Offset: 0x0004896C
		public SegmentState State
		{
			get
			{
				return this.state;
			}
			set
			{
				this.state = value;
				switch (this.state)
				{
				case SegmentState.Connecting:
				case SegmentState.Paused:
				case SegmentState.Finished:
				case SegmentState.Error:
					this.rate = 0.0;
					this.left = TimeSpan.Zero;
					return;
				case SegmentState.Downloading:
					this.BeginWork();
					return;
				default:
					return;
				}
			}
		}

		// Token: 0x170002A4 RID: 676
		// (get) Token: 0x06000B78 RID: 2936 RVA: 0x0004A7C5 File Offset: 0x000489C5
		public DateTime LastErrorDateTime
		{
			get
			{
				return this.lastErrorDateTime;
			}
		}

		// Token: 0x170002A5 RID: 677
		// (get) Token: 0x06000B79 RID: 2937 RVA: 0x0004A7CD File Offset: 0x000489CD
		// (set) Token: 0x06000B7A RID: 2938 RVA: 0x0004A7D5 File Offset: 0x000489D5
		public Exception LastError
		{
			get
			{
				return this.lastError;
			}
			set
			{
				if (value != null)
				{
					this.lastErrorDateTime = DateTime.Now;
				}
				else
				{
					this.lastErrorDateTime = DateTime.MinValue;
				}
				this.lastError = value;
			}
		}

		// Token: 0x170002A6 RID: 678
		// (get) Token: 0x06000B7B RID: 2939 RVA: 0x0004A7F9 File Offset: 0x000489F9
		// (set) Token: 0x06000B7C RID: 2940 RVA: 0x0004A801 File Offset: 0x00048A01
		public int Index
		{
			get
			{
				return this.index;
			}
			set
			{
				this.index = value;
			}
		}

		// Token: 0x170002A7 RID: 679
		// (get) Token: 0x06000B7D RID: 2941 RVA: 0x0004A80A File Offset: 0x00048A0A
		// (set) Token: 0x06000B7E RID: 2942 RVA: 0x0004A812 File Offset: 0x00048A12
		public long InitialStartPosition
		{
			get
			{
				return this.initialStartPosition;
			}
			set
			{
				this.initialStartPosition = value;
			}
		}

		// Token: 0x170002A8 RID: 680
		// (get) Token: 0x06000B7F RID: 2943 RVA: 0x0004A81B File Offset: 0x00048A1B
		// (set) Token: 0x06000B80 RID: 2944 RVA: 0x0004A823 File Offset: 0x00048A23
		public long StartPosition
		{
			get
			{
				return this.startPosition;
			}
			set
			{
				this.startPosition = value;
			}
		}

		// Token: 0x170002A9 RID: 681
		// (get) Token: 0x06000B81 RID: 2945 RVA: 0x0004A82C File Offset: 0x00048A2C
		public long Transfered
		{
			get
			{
				return this.StartPosition - this.InitialStartPosition;
			}
		}

		// Token: 0x170002AA RID: 682
		// (get) Token: 0x06000B82 RID: 2946 RVA: 0x0004A83B File Offset: 0x00048A3B
		public long TotalToTransfer
		{
			get
			{
				if (this.EndPosition > 0L)
				{
					return this.EndPosition - this.InitialStartPosition;
				}
				return 0L;
			}
		}

		// Token: 0x170002AB RID: 683
		// (get) Token: 0x06000B83 RID: 2947 RVA: 0x0004A857 File Offset: 0x00048A57
		public long MissingTransfer
		{
			get
			{
				if (this.EndPosition > 0L)
				{
					return this.EndPosition - this.StartPosition;
				}
				return 0L;
			}
		}

		// Token: 0x170002AC RID: 684
		// (get) Token: 0x06000B84 RID: 2948 RVA: 0x0004A873 File Offset: 0x00048A73
		public double Progress
		{
			get
			{
				if (this.EndPosition > 0L)
				{
					return (double)this.Transfered / (double)this.TotalToTransfer * 100.0;
				}
				return 0.0;
			}
		}

		// Token: 0x170002AD RID: 685
		// (get) Token: 0x06000B85 RID: 2949 RVA: 0x0004A8A2 File Offset: 0x00048AA2
		// (set) Token: 0x06000B86 RID: 2950 RVA: 0x0004A8AA File Offset: 0x00048AAA
		public long EndPosition
		{
			get
			{
				return this.endPosition;
			}
			set
			{
				this.endPosition = value;
			}
		}

		// Token: 0x170002AE RID: 686
		// (get) Token: 0x06000B87 RID: 2951 RVA: 0x0004A8B3 File Offset: 0x00048AB3
		// (set) Token: 0x06000B88 RID: 2952 RVA: 0x0004A8BB File Offset: 0x00048ABB
		public Stream OutputStream
		{
			get
			{
				return this.outputStream;
			}
			set
			{
				this.outputStream = value;
			}
		}

		// Token: 0x170002AF RID: 687
		// (get) Token: 0x06000B89 RID: 2953 RVA: 0x0004A8C4 File Offset: 0x00048AC4
		// (set) Token: 0x06000B8A RID: 2954 RVA: 0x0004A8CC File Offset: 0x00048ACC
		public Stream InputStream
		{
			get
			{
				return this.inputStream;
			}
			set
			{
				this.inputStream = value;
			}
		}

		// Token: 0x170002B0 RID: 688
		// (get) Token: 0x06000B8B RID: 2955 RVA: 0x0004A8D5 File Offset: 0x00048AD5
		// (set) Token: 0x06000B8C RID: 2956 RVA: 0x0004A8DD File Offset: 0x00048ADD
		public string CurrentURL
		{
			get
			{
				return this.currentURL;
			}
			set
			{
				this.currentURL = value;
			}
		}

		// Token: 0x170002B1 RID: 689
		// (get) Token: 0x06000B8D RID: 2957 RVA: 0x0004A8E6 File Offset: 0x00048AE6
		public double Rate
		{
			get
			{
				if (this.State == SegmentState.Downloading)
				{
					this.IncreaseStartPosition(0L);
					return this.rate;
				}
				return 0.0;
			}
		}

		// Token: 0x170002B2 RID: 690
		// (get) Token: 0x06000B8E RID: 2958 RVA: 0x0004A909 File Offset: 0x00048B09
		public TimeSpan Left
		{
			get
			{
				return this.left;
			}
		}

		// Token: 0x06000B8F RID: 2959 RVA: 0x0004A911 File Offset: 0x00048B11
		public void BeginWork()
		{
			this.start = this.startPosition;
			this.lastReception = DateTime.Now;
			this.started = true;
		}

		// Token: 0x06000B90 RID: 2960 RVA: 0x0004A934 File Offset: 0x00048B34
		public void IncreaseStartPosition(long size)
		{
			lock (this)
			{
				DateTime now = DateTime.Now;
				this.startPosition += size;
				if (this.started)
				{
					TimeSpan timeSpan = now - this.lastReception;
					if (timeSpan.TotalSeconds != 0.0)
					{
						this.rate = (double)(this.startPosition - this.start) / timeSpan.TotalSeconds;
						if (this.rate > 0.0)
						{
							this.left = TimeSpan.FromSeconds((double)this.MissingTransfer / this.rate);
						}
						else
						{
							this.left = TimeSpan.MaxValue;
						}
					}
				}
				else
				{
					this.start = this.startPosition;
					this.lastReception = now;
					this.started = true;
				}
			}
		}

		// Token: 0x040008AF RID: 2223
		private long startPosition;

		// Token: 0x040008B0 RID: 2224
		private int index;

		// Token: 0x040008B1 RID: 2225
		private string currentURL;

		// Token: 0x040008B2 RID: 2226
		private long initialStartPosition;

		// Token: 0x040008B3 RID: 2227
		private long endPosition;

		// Token: 0x040008B4 RID: 2228
		private Stream outputStream;

		// Token: 0x040008B5 RID: 2229
		private Stream inputStream;

		// Token: 0x040008B6 RID: 2230
		private Exception lastError;

		// Token: 0x040008B7 RID: 2231
		private SegmentState state;

		// Token: 0x040008B8 RID: 2232
		private bool started;

		// Token: 0x040008B9 RID: 2233
		private DateTime lastReception = DateTime.MinValue;

		// Token: 0x040008BA RID: 2234
		private DateTime lastErrorDateTime = DateTime.MinValue;

		// Token: 0x040008BB RID: 2235
		private double rate;

		// Token: 0x040008BC RID: 2236
		private long start;

		// Token: 0x040008BD RID: 2237
		private TimeSpan left = TimeSpan.Zero;

		// Token: 0x040008BE RID: 2238
		private int currentTry;
	}
}
