using System;
using System.Threading;

namespace TinhKiemAuto
{
	// Token: 0x020000D1 RID: 209
	internal class ReaderWriterObjectLocker
	{
		// Token: 0x06000B10 RID: 2832 RVA: 0x000492B4 File Offset: 0x000474B4
		public ReaderWriterObjectLocker()
		{
			this.locker = new ReaderWriterLock();
			this.writerReleaser = new ReaderWriterObjectLocker.WriterReleaser(this);
			this.readerReleaser = new ReaderWriterObjectLocker.ReaderReleaser(this);
		}

		// Token: 0x06000B11 RID: 2833 RVA: 0x000492DF File Offset: 0x000474DF
		public IDisposable LockForRead()
		{
			this.locker.AcquireReaderLock(-1);
			return this.readerReleaser;
		}

		// Token: 0x06000B12 RID: 2834 RVA: 0x000492F3 File Offset: 0x000474F3
		public IDisposable LockForWrite()
		{
			this.locker.AcquireWriterLock(-1);
			return this.writerReleaser;
		}

		// Token: 0x0400089C RID: 2204
		private ReaderWriterLock locker;

		// Token: 0x0400089D RID: 2205
		private IDisposable writerReleaser;

		// Token: 0x0400089E RID: 2206
		private IDisposable readerReleaser;

		// Token: 0x0200017C RID: 380
		private class BaseReleaser
		{
			// Token: 0x060011A2 RID: 4514 RVA: 0x00077DB0 File Offset: 0x00075FB0
			public BaseReleaser(ReaderWriterObjectLocker locker)
			{
				this.locker = locker;
			}

			// Token: 0x04000E9A RID: 3738
			protected ReaderWriterObjectLocker locker;
		}

		// Token: 0x0200017D RID: 381
		private class ReaderReleaser : ReaderWriterObjectLocker.BaseReleaser, IDisposable
		{
			// Token: 0x060011A3 RID: 4515 RVA: 0x00077DBF File Offset: 0x00075FBF
			public ReaderReleaser(ReaderWriterObjectLocker locker) : base(locker)
			{
			}

			// Token: 0x060011A4 RID: 4516 RVA: 0x00077DC8 File Offset: 0x00075FC8
			public void Dispose()
			{
				this.locker.locker.ReleaseReaderLock();
			}
		}

		// Token: 0x0200017E RID: 382
		private class WriterReleaser : ReaderWriterObjectLocker.BaseReleaser, IDisposable
		{
			// Token: 0x060011A5 RID: 4517 RVA: 0x00077DBF File Offset: 0x00075FBF
			public WriterReleaser(ReaderWriterObjectLocker locker) : base(locker)
			{
			}

			// Token: 0x060011A6 RID: 4518 RVA: 0x00077DDA File Offset: 0x00075FDA
			public void Dispose()
			{
				this.locker.locker.ReleaseWriterLock();
			}
		}
	}
}
