using System;

namespace TinhKiemAuto
{
	// Token: 0x02000104 RID: 260
	internal class MINHGIAO
	{
		// Token: 0x04000C3A RID: 3130
		public static int Id = 11;

		// Token: 0x04000C3B RID: 3131
		public static NPC LaSuTuong = new NPC
		{
			Id = 11,
			X = 108,
			Y = 56,
			Map = MINHGIAO.Id,
			INFOAIM = "#GQuang Minh Điện#RLã Sư Tương#{_INFOAIM108,56,11,Lã Sư Tương}"
		};

		// Token: 0x04000C3C RID: 3132
		public static NPC LamNham = new NPC
		{
			Id = 1,
			X = 98,
			Y = 105,
			Map = MINHGIAO.Id,
			INFOAIM = "#GQuang Minh Điện#RLâm Nham#{_INFOAIM98,105,11,Lâm Nham}"
		};

		// Token: 0x04000C3D RID: 3133
		public static NPC BangVanXuan = new NPC
		{
			Id = 12,
			X = 109,
			Y = 59,
			Map = MINHGIAO.Id,
			INFOAIM = "#GQuang Minh Điện#RBàng Vạn Xuân#{_INFOAIM109,59,11,Bàng Vạn Xuân}"
		};
	}
}
