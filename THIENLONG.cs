using System;

namespace TinhKiemAuto
{
	// Token: 0x02000108 RID: 264
	internal class THIENLONG
	{
		// Token: 0x04000C49 RID: 3145
		public static int Id = 13;

		// Token: 0x04000C4A RID: 3146
		public static NPC BanNhan = new NPC
		{
			Id = 0,
			X = 96,
			Y = 66,
			Map = THIENLONG.Id,
			INFOAIM = "#GThiên Long Tự #RBản Nhân#{_INFOAIM96,66,13,Bản Nhân}"
		};

		// Token: 0x04000C4B RID: 3147
		public static NPC BanPham = new NPC
		{
			Id = 4,
			X = 96,
			Y = 89,
			Map = THIENLONG.Id,
			INFOAIM = "#GThiên Long Tự #RBản Phàm#{_INFOAIM96,89,13,Bản Phàm}"
		};

		// Token: 0x04000C4C RID: 3148
		public static NPC BanQuan = new NPC
		{
			Id = 1,
			X = 98,
			Y = 67,
			Map = THIENLONG.Id,
			INFOAIM = "#GThiên Long Tự #RBản Quán#{_INFOAIM98,67,13,Bản Quán}"
		};
	}
}
