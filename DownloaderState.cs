using System;

namespace TinhKiemAuto
{
	// Token: 0x0200008D RID: 141
	public enum DownloaderState : byte
	{
		// Token: 0x0400040D RID: 1037
		NeedToPrepare,
		// Token: 0x0400040E RID: 1038
		Preparing,
		// Token: 0x0400040F RID: 1039
		WaitingForReconnect,
		// Token: 0x04000410 RID: 1040
		Prepared,
		// Token: 0x04000411 RID: 1041
		Working,
		// Token: 0x04000412 RID: 1042
		Pausing,
		// Token: 0x04000413 RID: 1043
		Paused,
		// Token: 0x04000414 RID: 1044
		Ended,
		// Token: 0x04000415 RID: 1045
		EndedWithError
	}
}
