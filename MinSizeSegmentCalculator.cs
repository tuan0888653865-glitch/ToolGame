using System;
using System.Collections.Generic;

namespace TinhKiemAuto
{
	// Token: 0x020000BC RID: 188
	internal class MinSizeSegmentCalculator : ISegmentCalculator
	{
		// Token: 0x06000A6E RID: 2670 RVA: 0x0004334C File Offset: 0x0004154C
		public CalculatedSegment[] GetSegments(int segmentCount, RemoteFileInfo remoteFileInfo)
		{
			long num = (long)DownloadSettings.MinSegmentSize;
			long num2 = remoteFileInfo.FileSize / (long)segmentCount;
			while (segmentCount > 1 && num2 < num)
			{
				segmentCount--;
				num2 = remoteFileInfo.FileSize / (long)segmentCount;
			}
			long num3 = 0L;
			List<CalculatedSegment> list = new List<CalculatedSegment>();
			for (int i = 0; i < segmentCount; i++)
			{
				if (segmentCount - 1 == i)
				{
					list.Add(new CalculatedSegment(num3, remoteFileInfo.FileSize));
				}
				else
				{
					list.Add(new CalculatedSegment(num3, num3 + (long)((int)num2)));
				}
				num3 = list[list.Count - 1].EndPosition;
			}
			return list.ToArray();
		}
	}
}
