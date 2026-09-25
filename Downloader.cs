using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;

namespace TinhKiemAuto
{
	// Token: 0x0200008B RID: 139
	internal class Downloader
	{
		// Token: 0x06000615 RID: 1557 RVA: 0x00023178 File Offset: 0x00021378
		private Downloader(ResourceLocation rl, ResourceLocation[] mirrors, string localFile)
		{
			this.threads = new List<Thread>();
			this.resourceLocation = rl;
			if (mirrors == null)
			{
				this.mirrors = new List<ResourceLocation>();
			}
			else
			{
				this.mirrors = new List<ResourceLocation>(mirrors);
			}
			this.localFile = localFile;
			this.extentedProperties = new Dictionary<string, object>();
			this.defaultDownloadProvider = rl.BindProtocolProviderInstance(this);
			this.segmentCalculator = new MinSizeSegmentCalculator();
			this.MirrorSelector = new SequentialMirrorSelector();
		}

		// Token: 0x06000616 RID: 1558 RVA: 0x000231F9 File Offset: 0x000213F9
		public Downloader(ResourceLocation rl, ResourceLocation[] mirrors, string localFile, int segmentCount) : this(rl, mirrors, localFile)
		{
			this.SetState(DownloaderState.NeedToPrepare);
			this.createdDateTime = DateTime.Now;
			this.requestedSegmentCount = segmentCount;
			this.segments = new List<Segment>();
		}

		// Token: 0x06000617 RID: 1559 RVA: 0x0002322C File Offset: 0x0002142C
		public Downloader(ResourceLocation rl, ResourceLocation[] mirrors, string localFile, List<Segment> segments, RemoteFileInfo remoteInfo, int requestedSegmentCount, DateTime createdDateTime) : this(rl, mirrors, localFile)
		{
			if (segments.Count > 0)
			{
				this.SetState(DownloaderState.Prepared);
			}
			else
			{
				this.SetState(DownloaderState.NeedToPrepare);
			}
			this.createdDateTime = createdDateTime;
			this.remoteFileInfo = remoteInfo;
			this.requestedSegmentCount = requestedSegmentCount;
			this.segments = segments;
		}

		// Token: 0x14000003 RID: 3
		// (add) Token: 0x06000618 RID: 1560 RVA: 0x0002327C File Offset: 0x0002147C
		// (remove) Token: 0x06000619 RID: 1561 RVA: 0x000232B4 File Offset: 0x000214B4
		public event EventHandler Ending;

		// Token: 0x14000004 RID: 4
		// (add) Token: 0x0600061A RID: 1562 RVA: 0x000232EC File Offset: 0x000214EC
		// (remove) Token: 0x0600061B RID: 1563 RVA: 0x00023324 File Offset: 0x00021524
		public event EventHandler InfoReceived;

		// Token: 0x14000005 RID: 5
		// (add) Token: 0x0600061C RID: 1564 RVA: 0x0002335C File Offset: 0x0002155C
		// (remove) Token: 0x0600061D RID: 1565 RVA: 0x00023394 File Offset: 0x00021594
		public event EventHandler StateChanged;

		// Token: 0x14000006 RID: 6
		// (add) Token: 0x0600061E RID: 1566 RVA: 0x000233CC File Offset: 0x000215CC
		// (remove) Token: 0x0600061F RID: 1567 RVA: 0x00023404 File Offset: 0x00021604
		public event EventHandler<SegmentEventArgs> RestartingSegment;

		// Token: 0x14000007 RID: 7
		// (add) Token: 0x06000620 RID: 1568 RVA: 0x0002343C File Offset: 0x0002163C
		// (remove) Token: 0x06000621 RID: 1569 RVA: 0x00023474 File Offset: 0x00021674
		public event EventHandler<SegmentEventArgs> SegmentStoped;

		// Token: 0x14000008 RID: 8
		// (add) Token: 0x06000622 RID: 1570 RVA: 0x000234AC File Offset: 0x000216AC
		// (remove) Token: 0x06000623 RID: 1571 RVA: 0x000234E4 File Offset: 0x000216E4
		public event EventHandler<SegmentEventArgs> SegmentStarting;

		// Token: 0x14000009 RID: 9
		// (add) Token: 0x06000624 RID: 1572 RVA: 0x0002351C File Offset: 0x0002171C
		// (remove) Token: 0x06000625 RID: 1573 RVA: 0x00023554 File Offset: 0x00021754
		public event EventHandler<SegmentEventArgs> SegmentStarted;

		// Token: 0x1400000A RID: 10
		// (add) Token: 0x06000626 RID: 1574 RVA: 0x0002358C File Offset: 0x0002178C
		// (remove) Token: 0x06000627 RID: 1575 RVA: 0x000235C4 File Offset: 0x000217C4
		public event EventHandler<SegmentEventArgs> SegmentFailed;

		// Token: 0x1700011C RID: 284
		// (get) Token: 0x06000628 RID: 1576 RVA: 0x000235F9 File Offset: 0x000217F9
		public Dictionary<string, object> ExtendedProperties
		{
			get
			{
				return this.extentedProperties;
			}
		}

		// Token: 0x1700011D RID: 285
		// (get) Token: 0x06000629 RID: 1577 RVA: 0x00023601 File Offset: 0x00021801
		public ResourceLocation ResourceLocation
		{
			get
			{
				return this.resourceLocation;
			}
		}

		// Token: 0x1700011E RID: 286
		// (get) Token: 0x0600062A RID: 1578 RVA: 0x00023609 File Offset: 0x00021809
		public List<ResourceLocation> Mirrors
		{
			get
			{
				return this.mirrors;
			}
		}

		// Token: 0x1700011F RID: 287
		// (get) Token: 0x0600062B RID: 1579 RVA: 0x00023611 File Offset: 0x00021811
		public long FileSize
		{
			get
			{
				if (this.remoteFileInfo == null)
				{
					return 0L;
				}
				return this.remoteFileInfo.FileSize;
			}
		}

		// Token: 0x17000120 RID: 288
		// (get) Token: 0x0600062C RID: 1580 RVA: 0x00023629 File Offset: 0x00021829
		public DateTime CreatedDateTime
		{
			get
			{
				return this.createdDateTime;
			}
		}

		// Token: 0x17000121 RID: 289
		// (get) Token: 0x0600062D RID: 1581 RVA: 0x00023631 File Offset: 0x00021831
		public int RequestedSegments
		{
			get
			{
				return this.requestedSegmentCount;
			}
		}

		// Token: 0x17000122 RID: 290
		// (get) Token: 0x0600062E RID: 1582 RVA: 0x00023639 File Offset: 0x00021839
		public string LocalFile
		{
			get
			{
				return this.localFile;
			}
		}

		// Token: 0x17000123 RID: 291
		// (get) Token: 0x0600062F RID: 1583 RVA: 0x00023641 File Offset: 0x00021841
		public int Percent
		{
			get
			{
				return (int)this.Progress;
			}
		}

		// Token: 0x17000124 RID: 292
		// (get) Token: 0x06000630 RID: 1584 RVA: 0x0002364C File Offset: 0x0002184C
		public double Progress
		{
			get
			{
				int count = this.segments.Count;
				if (count > 0)
				{
					double num = 0.0;
					for (int i = 0; i < count; i++)
					{
						num += this.segments[i].Progress;
					}
					return num / (double)count;
				}
				return 0.0;
			}
		}

		// Token: 0x17000125 RID: 293
		// (get) Token: 0x06000631 RID: 1585 RVA: 0x000236A4 File Offset: 0x000218A4
		public double Rate
		{
			get
			{
				double num = 0.0;
				for (int i = 0; i < this.segments.Count; i++)
				{
					num += this.segments[i].Rate;
				}
				return num;
			}
		}

		// Token: 0x17000126 RID: 294
		// (get) Token: 0x06000632 RID: 1586 RVA: 0x000236E8 File Offset: 0x000218E8
		public long Transfered
		{
			get
			{
				long num = 0L;
				for (int i = 0; i < this.segments.Count; i++)
				{
					num += this.segments[i].Transfered;
				}
				return num;
			}
		}

		// Token: 0x17000127 RID: 295
		// (get) Token: 0x06000633 RID: 1587 RVA: 0x00023724 File Offset: 0x00021924
		public TimeSpan Left
		{
			get
			{
				if (this.Rate == 0.0)
				{
					return TimeSpan.MaxValue;
				}
				double num = 0.0;
				for (int i = 0; i < this.segments.Count; i++)
				{
					num += (double)this.segments[i].MissingTransfer;
				}
				return TimeSpan.FromSeconds(num / this.Rate);
			}
		}

		// Token: 0x17000128 RID: 296
		// (get) Token: 0x06000634 RID: 1588 RVA: 0x0002378A File Offset: 0x0002198A
		public List<Segment> Segments
		{
			get
			{
				return this.segments;
			}
		}

		// Token: 0x17000129 RID: 297
		// (get) Token: 0x06000635 RID: 1589 RVA: 0x00023792 File Offset: 0x00021992
		// (set) Token: 0x06000636 RID: 1590 RVA: 0x0002379A File Offset: 0x0002199A
		public Exception LastError
		{
			get
			{
				return this.lastError;
			}
			set
			{
				this.lastError = value;
			}
		}

		// Token: 0x1700012A RID: 298
		// (get) Token: 0x06000637 RID: 1591 RVA: 0x000237A3 File Offset: 0x000219A3
		public DownloaderState State
		{
			get
			{
				return this.state;
			}
		}

		// Token: 0x06000638 RID: 1592 RVA: 0x000237AC File Offset: 0x000219AC
		public bool IsWorking()
		{
			DownloaderState downloaderState = this.State;
			return downloaderState == DownloaderState.Preparing || downloaderState == DownloaderState.WaitingForReconnect || downloaderState == DownloaderState.Working;
		}

		// Token: 0x1700012B RID: 299
		// (get) Token: 0x06000639 RID: 1593 RVA: 0x000237CE File Offset: 0x000219CE
		public RemoteFileInfo RemoteFileInfo
		{
			get
			{
				return this.remoteFileInfo;
			}
		}

		// Token: 0x1700012C RID: 300
		// (get) Token: 0x0600063A RID: 1594 RVA: 0x000237D6 File Offset: 0x000219D6
		// (set) Token: 0x0600063B RID: 1595 RVA: 0x000237DE File Offset: 0x000219DE
		public string StatusMessage
		{
			get
			{
				return this.statusMessage;
			}
			set
			{
				this.statusMessage = value;
			}
		}

		// Token: 0x1700012D RID: 301
		// (get) Token: 0x0600063C RID: 1596 RVA: 0x000237E7 File Offset: 0x000219E7
		// (set) Token: 0x0600063D RID: 1597 RVA: 0x000237EF File Offset: 0x000219EF
		public ISegmentCalculator SegmentCalculator
		{
			get
			{
				return this.segmentCalculator;
			}
			set
			{
				if (value == null)
				{
					throw new ArgumentNullException("value");
				}
				this.segmentCalculator = value;
			}
		}

		// Token: 0x1700012E RID: 302
		// (get) Token: 0x0600063E RID: 1598 RVA: 0x00023806 File Offset: 0x00021A06
		// (set) Token: 0x0600063F RID: 1599 RVA: 0x0002380E File Offset: 0x00021A0E
		public IMirrorSelector MirrorSelector
		{
			get
			{
				return this.mirrorSelector;
			}
			set
			{
				if (value == null)
				{
					throw new ArgumentNullException("value");
				}
				this.mirrorSelector = value;
				this.mirrorSelector.Init(this);
			}
		}

		// Token: 0x06000640 RID: 1600 RVA: 0x00023831 File Offset: 0x00021A31
		private void SetState(DownloaderState value)
		{
			this.state = value;
			this.OnStateChanged();
		}

		// Token: 0x06000641 RID: 1601 RVA: 0x00023840 File Offset: 0x00021A40
		private void StartToPrepare()
		{
			this.mainThread = new Thread(new ParameterizedThreadStart(this.StartDownloadThreadProc));
			this.mainThread.IsBackground = true;
			this.mainThread.Start(this.requestedSegmentCount);
		}

		// Token: 0x06000642 RID: 1602 RVA: 0x0002387B File Offset: 0x00021A7B
		private void StartPrepared()
		{
			this.mainThread = new Thread(new ThreadStart(this.RestartDownload));
			this.mainThread.Start();
		}

		// Token: 0x06000643 RID: 1603 RVA: 0x0002389F File Offset: 0x00021A9F
		protected virtual void OnRestartingSegment(Segment segment)
		{
			if (this.RestartingSegment != null)
			{
				this.RestartingSegment(this, new SegmentEventArgs(this, segment));
			}
		}

		// Token: 0x06000644 RID: 1604 RVA: 0x000238BC File Offset: 0x00021ABC
		protected virtual void OnSegmentStoped(Segment segment)
		{
			if (this.SegmentStoped != null)
			{
				this.SegmentStoped(this, new SegmentEventArgs(this, segment));
			}
		}

		// Token: 0x06000645 RID: 1605 RVA: 0x000238D9 File Offset: 0x00021AD9
		protected virtual void OnSegmentFailed(Segment segment)
		{
			if (this.SegmentFailed != null)
			{
				this.SegmentFailed(this, new SegmentEventArgs(this, segment));
			}
		}

		// Token: 0x06000646 RID: 1606 RVA: 0x000238F6 File Offset: 0x00021AF6
		protected virtual void OnSegmentStarting(Segment segment)
		{
			if (this.SegmentStarting != null)
			{
				this.SegmentStarting(this, new SegmentEventArgs(this, segment));
			}
		}

		// Token: 0x06000647 RID: 1607 RVA: 0x00023913 File Offset: 0x00021B13
		protected virtual void OnSegmentStarted(Segment segment)
		{
			if (this.SegmentStarted != null)
			{
				this.SegmentStarted(this, new SegmentEventArgs(this, segment));
			}
		}

		// Token: 0x06000648 RID: 1608 RVA: 0x00023930 File Offset: 0x00021B30
		protected virtual void OnStateChanged()
		{
			if (this.StateChanged != null)
			{
				this.StateChanged(this, EventArgs.Empty);
			}
		}

		// Token: 0x06000649 RID: 1609 RVA: 0x0002394B File Offset: 0x00021B4B
		protected virtual void OnEnding()
		{
			if (this.Ending != null)
			{
				this.Ending(this, EventArgs.Empty);
			}
		}

		// Token: 0x0600064A RID: 1610 RVA: 0x00023966 File Offset: 0x00021B66
		protected virtual void OnInfoReceived()
		{
			if (this.InfoReceived != null)
			{
				this.InfoReceived(this, EventArgs.Empty);
			}
		}

		// Token: 0x0600064B RID: 1611 RVA: 0x00023981 File Offset: 0x00021B81
		public IDisposable LockSegments()
		{
			return new ObjectLocker(this.segments);
		}

		// Token: 0x0600064C RID: 1612 RVA: 0x00023990 File Offset: 0x00021B90
		public void WaitForConclusion()
		{
			if (!this.IsWorking() && this.mainThread != null && this.mainThread.IsAlive)
			{
				this.mainThread.Join(TimeSpan.FromSeconds(1.0));
			}
			while (this.IsWorking())
			{
				Thread.Sleep(100);
			}
		}

		// Token: 0x0600064D RID: 1613 RVA: 0x000239E8 File Offset: 0x00021BE8
		public void Pause()
		{
			if (this.state == DownloaderState.Preparing || this.state == DownloaderState.WaitingForReconnect)
			{
				this.Segments.Clear();
				this.mainThread.Abort();
				this.mainThread = null;
				this.SetState(DownloaderState.NeedToPrepare);
				return;
			}
			if (this.state == DownloaderState.Working)
			{
				this.SetState(DownloaderState.Pausing);
				while (!this.AllWorkersStopped(5))
				{
				}
				List<Thread> obj = this.threads;
				lock (obj)
				{
					this.threads.Clear();
				}
				this.mainThread.Abort();
				this.mainThread = null;
				if (this.RemoteFileInfo != null && !this.RemoteFileInfo.AcceptRanges)
				{
					this.Segments[0].StartPosition = 0L;
				}
				this.SetState(DownloaderState.Paused);
			}
		}

		// Token: 0x0600064E RID: 1614 RVA: 0x00023AB8 File Offset: 0x00021CB8
		public void Start()
		{
			if (this.state == DownloaderState.NeedToPrepare)
			{
				this.SetState(DownloaderState.Preparing);
				this.StartToPrepare();
				return;
			}
			if (this.state != DownloaderState.Preparing && this.state != DownloaderState.Pausing && this.state != DownloaderState.Working && this.state != DownloaderState.WaitingForReconnect)
			{
				this.SetState(DownloaderState.Preparing);
				this.StartPrepared();
			}
		}

		// Token: 0x0600064F RID: 1615 RVA: 0x00023B0C File Offset: 0x00021D0C
		private void AllocLocalFile()
		{
			FileInfo fileInfo = new FileInfo(this.LocalFile);
			if (!Directory.Exists(fileInfo.DirectoryName))
			{
				Directory.CreateDirectory(fileInfo.DirectoryName);
			}
			if (fileInfo.Exists)
			{
				int num = 1;
				string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(this.LocalFile);
				string extension = Path.GetExtension(this.LocalFile);
				string path;
				do
				{
					path = PathHelper.GetWithBackslash(fileInfo.DirectoryName) + fileNameWithoutExtension + string.Format("({0})", num++) + extension;
				}
				while (File.Exists(path));
				this.localFile = path;
			}
			using (FileStream fileStream = new FileStream(this.LocalFile, FileMode.Create, FileAccess.Write))
			{
				fileStream.SetLength(Math.Max(this.FileSize, 0L));
			}
		}

		// Token: 0x06000650 RID: 1616 RVA: 0x00023BDC File Offset: 0x00021DDC
		private void StartDownloadThreadProc(object objSegmentCount)
		{
			this.SetState(DownloaderState.Preparing);
			int segmentCount = Math.Min((int)objSegmentCount, DownloadSettings.MaxSegments);
			Stream inputStream = null;
			int num = 0;
			for (;;)
			{
				this.lastError = null;
				if (this.state != DownloaderState.Pausing)
				{
					this.SetState(DownloaderState.Preparing);
					num++;
					try
					{
						this.remoteFileInfo = this.defaultDownloadProvider.GetFileInfo(this.ResourceLocation, out inputStream);
						goto IL_96;
					}
					catch (ThreadAbortException)
					{
						this.SetState(DownloaderState.NeedToPrepare);
						return;
					}
					catch (Exception ex)
					{
						this.lastError = ex;
						if (num < DownloadSettings.MaxRetries)
						{
							this.SetState(DownloaderState.WaitingForReconnect);
							Thread.Sleep(TimeSpan.FromSeconds((double)DownloadSettings.RetryDelay));
							continue;
						}
						this.SetState(DownloaderState.NeedToPrepare);
						return;
					}
					break;
				}
				break;
			}
			this.SetState(DownloaderState.NeedToPrepare);
			return;
			IL_96:
			try
			{
				this.lastError = null;
				this.StartSegments(segmentCount, inputStream);
			}
			catch (ThreadAbortException)
			{
				throw;
			}
			catch (Exception ex2)
			{
				this.lastError = ex2;
				this.SetState(DownloaderState.EndedWithError);
			}
		}

		// Token: 0x06000651 RID: 1617 RVA: 0x00023CDC File Offset: 0x00021EDC
		private void StartSegments(int segmentCount, Stream inputStream)
		{
			this.OnInfoReceived();
			this.AllocLocalFile();
			CalculatedSegment[] array;
			if (!this.remoteFileInfo.AcceptRanges)
			{
				array = new CalculatedSegment[]
				{
					new CalculatedSegment(0L, this.remoteFileInfo.FileSize)
				};
			}
			else
			{
				array = this.SegmentCalculator.GetSegments(segmentCount, this.remoteFileInfo);
			}
			List<Thread> obj = this.threads;
			lock (obj)
			{
				this.threads.Clear();
			}
			List<Segment> obj2 = this.segments;
			lock (obj2)
			{
				this.segments.Clear();
			}
			for (int i = 0; i < array.Length; i++)
			{
				Segment segment = new Segment();
				if (i == 0)
				{
					segment.InputStream = inputStream;
				}
				segment.Index = i;
				segment.InitialStartPosition = array[i].StartPosition;
				segment.StartPosition = array[i].StartPosition;
				segment.EndPosition = array[i].EndPosition;
				this.segments.Add(segment);
			}
			this.RunSegments();
		}

		// Token: 0x06000652 RID: 1618 RVA: 0x00023E0C File Offset: 0x0002200C
		private void RestartDownload()
		{
			int num = 0;
			Stream stream;
			RemoteFileInfo fileInfo;
			try
			{
				for (;;)
				{
					this.lastError = null;
					this.SetState(DownloaderState.Preparing);
					num++;
					try
					{
						fileInfo = this.defaultDownloadProvider.GetFileInfo(this.ResourceLocation, out stream);
						break;
					}
					catch (Exception ex)
					{
						this.lastError = ex;
						if (num >= DownloadSettings.MaxRetries)
						{
							return;
						}
						this.SetState(DownloaderState.WaitingForReconnect);
						Thread.Sleep(TimeSpan.FromSeconds((double)DownloadSettings.RetryDelay));
					}
				}
			}
			finally
			{
				this.SetState(DownloaderState.Prepared);
			}
			try
			{
				if (!fileInfo.AcceptRanges || fileInfo.LastModified > this.RemoteFileInfo.LastModified || fileInfo.FileSize != this.RemoteFileInfo.FileSize)
				{
					this.remoteFileInfo = fileInfo;
					this.StartSegments(this.RequestedSegments, stream);
				}
				else
				{
					if (stream != null)
					{
						stream.Dispose();
					}
					this.RunSegments();
				}
			}
			catch (ThreadAbortException)
			{
				throw;
			}
			catch (Exception ex2)
			{
				this.lastError = ex2;
				this.SetState(DownloaderState.EndedWithError);
			}
		}

		// Token: 0x06000653 RID: 1619 RVA: 0x00023F20 File Offset: 0x00022120
		private void RunSegments()
		{
			this.SetState(DownloaderState.Working);
			using (FileStream fileStream = new FileStream(this.LocalFile, FileMode.Open, FileAccess.Write))
			{
				for (int i = 0; i < this.Segments.Count; i++)
				{
					this.Segments[i].OutputStream = fileStream;
					this.StartSegment(this.Segments[i]);
				}
				while (!this.AllWorkersStopped(1000) || this.RestartFailedSegments())
				{
				}
			}
			for (int j = 0; j < this.Segments.Count; j++)
			{
				if (this.Segments[j].State == SegmentState.Error)
				{
					this.SetState(DownloaderState.EndedWithError);
					return;
				}
			}
			if (this.State != DownloaderState.Pausing)
			{
				this.OnEnding();
			}
			this.SetState(DownloaderState.Ended);
		}

		// Token: 0x06000654 RID: 1620 RVA: 0x00023FF8 File Offset: 0x000221F8
		private bool RestartFailedSegments()
		{
			bool result = false;
			double num = 0.0;
			for (int i = 0; i < this.Segments.Count; i++)
			{
				if (this.Segments[i].State == SegmentState.Error && this.Segments[i].LastErrorDateTime != DateTime.MinValue && (DownloadSettings.MaxRetries == 0 || this.Segments[i].CurrentTry < DownloadSettings.MaxRetries))
				{
					result = true;
					TimeSpan timeSpan = DateTime.Now - this.Segments[i].LastErrorDateTime;
					if (timeSpan.TotalSeconds >= (double)DownloadSettings.RetryDelay)
					{
						Segment segment = this.Segments[i];
						int currentTry = segment.CurrentTry;
						segment.CurrentTry = currentTry + 1;
						this.StartSegment(this.Segments[i]);
						this.OnRestartingSegment(this.Segments[i]);
					}
					else
					{
						num = Math.Max(num, (double)(DownloadSettings.RetryDelay * 1000) - timeSpan.TotalMilliseconds);
					}
				}
			}
			Thread.Sleep((int)num);
			return result;
		}

		// Token: 0x06000655 RID: 1621 RVA: 0x0002411C File Offset: 0x0002231C
		private void StartSegment(Segment newSegment)
		{
			Thread thread = new Thread(new ParameterizedThreadStart(this.SegmentThreadProc));
			thread.IsBackground = true;
			thread.Start(newSegment);
			List<Thread> obj = this.threads;
			lock (obj)
			{
				this.threads.Add(thread);
			}
		}

		// Token: 0x06000656 RID: 1622 RVA: 0x0002417C File Offset: 0x0002237C
		private bool AllWorkersStopped(int timeOut)
		{
			bool flag = true;
			List<Thread> obj = this.threads;
			Thread[] array;
			lock (obj)
			{
				array = this.threads.ToArray();
			}
			foreach (Thread thread in array)
			{
				bool flag2 = thread.Join(timeOut);
				flag = (flag && flag2);
				if (flag2)
				{
					obj = this.threads;
					lock (obj)
					{
						this.threads.Remove(thread);
					}
				}
			}
			return flag;
		}

		// Token: 0x06000657 RID: 1623 RVA: 0x0002421C File Offset: 0x0002241C
		private void SegmentThreadProc(object objSegment)
		{
			Segment segment = (Segment)objSegment;
			segment.LastError = null;
			try
			{
				if (segment.EndPosition > 0L && segment.StartPosition >= segment.EndPosition)
				{
					segment.State = SegmentState.Finished;
					this.OnSegmentStoped(segment);
				}
				else
				{
					int num = 8192;
					byte[] buffer = new byte[num];
					segment.State = SegmentState.Connecting;
					this.OnSegmentStarting(segment);
					if (segment.InputStream == null)
					{
						ResourceLocation nextResourceLocation = this.MirrorSelector.GetNextResourceLocation();
						IProtocolProvider protocolProvider = nextResourceLocation.BindProtocolProviderInstance(this);
						while (nextResourceLocation != this.ResourceLocation)
						{
							Stream stream;
							RemoteFileInfo fileInfo = protocolProvider.GetFileInfo(nextResourceLocation, out stream);
							if (stream != null)
							{
								stream.Dispose();
							}
							if (fileInfo.FileSize == this.remoteFileInfo.FileSize && fileInfo.AcceptRanges == this.remoteFileInfo.AcceptRanges)
							{
								break;
							}
							List<ResourceLocation> obj = this.mirrors;
							lock (obj)
							{
								this.mirrors.Remove(nextResourceLocation);
							}
							nextResourceLocation = this.MirrorSelector.GetNextResourceLocation();
							protocolProvider = nextResourceLocation.BindProtocolProviderInstance(this);
						}
						segment.InputStream = protocolProvider.CreateStream(nextResourceLocation, segment.StartPosition, segment.EndPosition);
						segment.CurrentURL = nextResourceLocation.URL;
					}
					else
					{
						segment.CurrentURL = this.resourceLocation.URL;
					}
					using (segment.InputStream)
					{
						this.OnSegmentStarted(segment);
						segment.State = SegmentState.Downloading;
						segment.CurrentTry = 0;
						long num2;
						do
						{
							num2 = (long)segment.InputStream.Read(buffer, 0, num);
							if (segment.EndPosition > 0L && segment.StartPosition + num2 > segment.EndPosition)
							{
								num2 = segment.EndPosition - segment.StartPosition;
								if (num2 <= 0L)
								{
									goto IL_201;
								}
							}
							Stream outputStream = segment.OutputStream;
							lock (outputStream)
							{
								segment.OutputStream.Position = segment.StartPosition;
								segment.OutputStream.Write(buffer, 0, (int)num2);
							}
							segment.IncreaseStartPosition(num2);
							if (segment.EndPosition > 0L && segment.StartPosition >= segment.EndPosition)
							{
								goto IL_20F;
							}
							if (this.state == DownloaderState.Pausing)
							{
								goto IL_21D;
							}
						}
						while (num2 > 0L);
						goto IL_224;
						IL_201:
						segment.StartPosition = segment.EndPosition;
						goto IL_224;
						IL_20F:
						segment.StartPosition = segment.EndPosition;
						goto IL_224;
						IL_21D:
						segment.State = SegmentState.Paused;
						IL_224:
						if (segment.State == SegmentState.Downloading)
						{
							segment.State = SegmentState.Finished;
							this.AddNewSegmentIfNeeded();
						}
					}
					this.OnSegmentStoped(segment);
				}
			}
			catch (Exception ex)
			{
				segment.State = SegmentState.Error;
				segment.LastError = ex;
				this.OnSegmentFailed(segment);
			}
			finally
			{
				segment.InputStream = null;
			}
		}

		// Token: 0x06000658 RID: 1624 RVA: 0x00024518 File Offset: 0x00022718
		private void AddNewSegmentIfNeeded()
		{
			List<Segment> obj = this.segments;
			lock (obj)
			{
				for (int i = 0; i < this.segments.Count; i++)
				{
					Segment segment = this.segments[i];
					if (segment.State == SegmentState.Downloading && segment.Left.TotalSeconds > (double)DownloadSettings.MinSegmentLeftToStartNewSegment && segment.MissingTransfer / 2L >= (long)DownloadSettings.MinSegmentSize)
					{
						long num = segment.MissingTransfer / 2L;
						Segment segment2 = new Segment();
						segment2.Index = this.segments.Count;
						segment2.StartPosition = segment.StartPosition + num;
						segment2.InitialStartPosition = segment2.StartPosition;
						segment2.EndPosition = segment.EndPosition;
						segment2.OutputStream = segment.OutputStream;
						segment.EndPosition -= num;
						this.segments.Add(segment2);
						this.StartSegment(segment2);
						break;
					}
				}
			}
		}

		// Token: 0x040003FA RID: 1018
		private string localFile;

		// Token: 0x040003FB RID: 1019
		private int requestedSegmentCount;

		// Token: 0x040003FC RID: 1020
		private ResourceLocation resourceLocation;

		// Token: 0x040003FD RID: 1021
		private List<ResourceLocation> mirrors;

		// Token: 0x040003FE RID: 1022
		private List<Segment> segments;

		// Token: 0x040003FF RID: 1023
		private Thread mainThread;

		// Token: 0x04000400 RID: 1024
		private List<Thread> threads;

		// Token: 0x04000401 RID: 1025
		private RemoteFileInfo remoteFileInfo;

		// Token: 0x04000402 RID: 1026
		private DownloaderState state;

		// Token: 0x04000403 RID: 1027
		private DateTime createdDateTime;

		// Token: 0x04000404 RID: 1028
		private Exception lastError;

		// Token: 0x04000405 RID: 1029
		private Dictionary<string, object> extentedProperties = new Dictionary<string, object>();

		// Token: 0x04000406 RID: 1030
		private IProtocolProvider defaultDownloadProvider;

		// Token: 0x04000407 RID: 1031
		private ISegmentCalculator segmentCalculator;

		// Token: 0x04000408 RID: 1032
		private IMirrorSelector mirrorSelector;

		// Token: 0x04000409 RID: 1033
		private string statusMessage;
	}
}
