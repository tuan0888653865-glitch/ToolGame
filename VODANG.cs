using System;

namespace TinhKiemAuto
{
	// Token: 0x02000110 RID: 272
	internal class VODANG
	{
		// Token: 0x04000C6B RID: 3179
		public static int Id = 12;

		// Token: 0x04000C6C RID: 3180
		public static NPC TruongHuyenTo = new NPC
		{
			Id = 0,
			X = 78,
			Y = 86,
			Map = VODANG.Id,
			INFOAIM = "#GVõ Đang Sơn#RTrương Huyền Tố#{_INFOAIM78,86,12,Trương Huyền Tố}"
		};

		// Token: 0x04000C6D RID: 3181
		public static NPC TruongTrungHanh = new NPC
		{
			Id = 9,
			X = 78,
			Y = 95,
			Map = VODANG.Id,
			INFOAIM = "#GVõ Đang Sơn#RTrương Trung Hành#{_INFOAIM78,95,12,Trương Trung Hành}"
		};

		// Token: 0x04000C6E RID: 3182
		public static NPC DuVienSon = new NPC
		{
			Id = 1,
			X = 83,
			Y = 85,
			Map = VODANG.Id,
			INFOAIM = "#GVõ Đang Sơn#RDu Viễn Sơn#{_INFOAIM83,85,12,Du Viễn Sơn}"
		};
	}
}
