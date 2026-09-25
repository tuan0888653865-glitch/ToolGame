using System;

namespace TinhKiemAuto
{
	// Token: 0x0200010D RID: 269
	internal class TINHTUC
	{
		// Token: 0x04000C5D RID: 3165
		public static int Id = 16;

		// Token: 0x04000C5E RID: 3166
		public static NPC HanTheTrung = new NPC
		{
			Id = 1,
			X = 96,
			Y = 75,
			Map = TINHTUC.Id,
			INFOAIM = "#GTinh Túc Hải #RHàn Thế Trung#{_INFOAIM96,75,16,Hàn Thế Trung}"
		};

		// Token: 0x04000C5F RID: 3167
		public static NPC VuongNgan = new NPC
		{
			Id = 9,
			X = 96,
			Y = 93,
			Map = TINHTUC.Id,
			INFOAIM = "#GTinh Túc Hải #RVương Ngạn#{_INFOAIM96,93,16,Vương Ngạn}"
		};

		// Token: 0x04000C60 RID: 3168
		public static NPC ThiToan = new NPC
		{
			Id = 6,
			X = 87,
			Y = 70,
			Map = TINHTUC.Id,
			INFOAIM = "#GTinh Túc Hải #RThi Toàn#{_INFOAIM87,70,16,Thi Toàn}"
		};
	}
}
