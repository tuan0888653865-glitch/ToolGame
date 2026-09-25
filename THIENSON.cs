using System;

namespace TinhKiemAuto
{
	// Token: 0x02000109 RID: 265
	internal class THIENSON
	{
		// Token: 0x04000C4D RID: 3149
		public static int Id = 17;

		// Token: 0x04000C4E RID: 3150
		public static NPC MaiKiem = new NPC
		{
			Id = 0,
			X = 92,
			Y = 45,
			Map = THIENSON.Id,
			INFOAIM = "#GThiên Sơn#RMai Kiếm#{_INFOAIM92,45,17,Mai Kiếm}"
		};

		// Token: 0x04000C4F RID: 3151
		public static NPC PhuManNghi = new NPC
		{
			Id = 2,
			X = 95,
			Y = 61,
			Map = THIENSON.Id,
			INFOAIM = "#GThiên Sơn#RPhù Mẫn Nghi#{_INFOAIM95,61,17,Phù Mẫn Nghi}"
		};

		// Token: 0x04000C50 RID: 3152
		public static NPC LanKiem = new NPC
		{
			Id = 13,
			X = 89,
			Y = 45,
			Map = THIENSON.Id,
			INFOAIM = "#GThiên Sơn#RLan Kiếm#{_INFOAIM89,45,17,Lan Kiếm}"
		};
	}
}
