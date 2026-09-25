using System;

namespace TinhKiemAuto
{
	// Token: 0x0200010A RID: 266
	internal class THIEULAM
	{
		// Token: 0x04000C51 RID: 3153
		public static int Id = 9;

		// Token: 0x04000C52 RID: 3154
		public static NPC HuyenTich = new NPC
		{
			Id = 4,
			X = 90,
			Y = 72,
			Map = THIEULAM.Id,
			INFOAIM = "#GThiếu Lâm Tự#RHuyền Tịch#{_INFOAIM90,72,9,Huyền Tịch}"
		};

		// Token: 0x04000C53 RID: 3155
		public static NPC TuePhuong = new NPC
		{
			Id = 9,
			X = 96,
			Y = 82,
			Map = THIEULAM.Id,
			INFOAIM = "#GThiếu Lâm Tự#RTuệ Phương#{_INFOAIM96,82,9,Tuệ Phương}"
		};

		// Token: 0x04000C54 RID: 3156
		public static NPC HuyenNan = new NPC
		{
			Id = 5,
			X = 92,
			Y = 71,
			Map = THIEULAM.Id,
			INFOAIM = "#GThiếu Lâm Tự#RHuyền Nạn#{_INFOAIM92,71,9,Huyền Nạn}"
		};
	}
}
