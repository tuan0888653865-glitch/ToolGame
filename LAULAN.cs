using System;

namespace TinhKiemAuto
{
	// Token: 0x02000103 RID: 259
	internal class LAULAN
	{
		// Token: 0x04000C36 RID: 3126
		public static int Id = 246;

		// Token: 0x04000C37 RID: 3127
		public static NPC KimCuuLinh = new NPC
		{
			Id = 70,
			X = 162,
			Y = 75,
			Map = LAULAN.Id,
			INFOAIM = "#GLâu Lan#RKim Cửu Linh#{_INFOAIM162,75,246,Kim Cửu Linh}"
		};

		// Token: 0x04000C38 RID: 3128
		public static NPC ThuongKho = new NPC
		{
			Id = 7,
			X = 207,
			Y = 122,
			Map = LAULAN.Id,
			INFOAIM = "#GLâu Lan#RHầu bàn Tống#{_INFOAIM207,122,246,Hầu bàn Tống}"
		};

		// Token: 0x04000C39 RID: 3129
		public static NPC HaDuyet = new NPC
		{
			Id = 71,
			X = 295,
			Y = 68,
			Map = LAULAN.Id,
			INFOAIM = "#GLâu Lan#RHà Duyệt#{_INFOAIM295,68,246,Hà Duyệt}"
		};
	}
}
