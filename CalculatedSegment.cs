using System;

namespace TinhKiemAuto
{
	// Token: 0x02000083 RID: 131
	[Serializable]
	public struct CalculatedSegment
	{
		// Token: 0x17000118 RID: 280
		// (get) Token: 0x060005F2 RID: 1522 RVA: 0x000219FA File Offset: 0x0001FBFA
		public long StartPosition
		{
			get
			{
				return this.startPosition;
			}
		}

		// Token: 0x17000119 RID: 281
		// (get) Token: 0x060005F3 RID: 1523 RVA: 0x00021A02 File Offset: 0x0001FC02
		public long EndPosition
		{
			get
			{
				return this.endPosition;
			}
		}

		// Token: 0x060005F4 RID: 1524 RVA: 0x00021A0A File Offset: 0x0001FC0A
		public CalculatedSegment(long startPos, long endPos)
		{
			this.endPosition = endPos;
			this.startPosition = startPos;
		}

		// Token: 0x040003D4 RID: 980
		private long startPosition;

		// Token: 0x040003D5 RID: 981
		private long endPosition;
	}
}
