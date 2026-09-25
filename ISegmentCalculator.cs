using System;

namespace TinhKiemAuto
{
	// Token: 0x020000B0 RID: 176
	internal interface ISegmentCalculator
	{
		// Token: 0x060009DB RID: 2523
		CalculatedSegment[] GetSegments(int segmentCount, RemoteFileInfo fileSize);
	}
}
