using System;

namespace TinhKiemAuto
{
	// Token: 0x02000101 RID: 257
	internal class DUONGMON
	{
		// Token: 0x04000C26 RID: 3110
		public static int Id = 615;

		// Token: 0x04000C27 RID: 3111
		public static NPC DuongXichPhong = new NPC
		{
			Id = 0,
			X = 78,
			Y = 35,
			Map = DUONGMON.Id,
			INFOAIM = "#GĐường Gia Bảo#RĐường Xích Phong#{_INFOAIM78,35,615,Đường Xích Phong}"
		};

		// Token: 0x04000C28 RID: 3112
		public static NPC DuongThanhThu = new NPC
		{
			Id = 2,
			X = 100,
			Y = 64,
			Map = DUONGMON.Id,
			INFOAIM = "#GĐường Gia Bảo#RĐường Thanh Thu#{_INFOAIM100,64,615,Đường Thanh Thu}"
		};

		// Token: 0x04000C29 RID: 3113
		public static NPC DuongNhacXung = new NPC
		{
			Id = 1,
			X = 38,
			Y = 75,
			Map = DUONGMON.Id,
			INFOAIM = "#GĐường Gia Bảo#RĐường Nhạc Xung#{_INFOAIM38,75,615,Đường Nhạc Xung}"
		};
	}
}
