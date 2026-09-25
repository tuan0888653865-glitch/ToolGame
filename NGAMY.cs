using System;

namespace TinhKiemAuto
{
	// Token: 0x02000106 RID: 262
	internal class NGAMY
	{
		// Token: 0x04000C42 RID: 3138
		public static int Id = 15;

		// Token: 0x04000C43 RID: 3139
		public static NPC LyThapNhiNuong = new NPC
		{
			Id = 1,
			X = 96,
			Y = 52,
			Map = NGAMY.Id,
			INFOAIM = "#GNga Mi Sơn#RLý Thập Nhị Nương#{_INFOAIM96,52,15,Lý Thập Nhị Nương}"
		};

		// Token: 0x04000C44 RID: 3140
		public static NPC ManhLong = new NPC
		{
			Id = 7,
			X = 96,
			Y = 87,
			Map = NGAMY.Id,
			INFOAIM = "#GNga Mi Sơn#RMãnh Long#{_INFOAIM96,87,15,Mãnh Long}"
		};

		// Token: 0x04000C45 RID: 3141
		public static NPC ThoiLucHoa = new NPC
		{
			Id = 2,
			X = 98,
			Y = 52,
			Map = NGAMY.Id,
			INFOAIM = "#GNga Mi Sơn#RThôi Lục Hoa#{_INFOAIM98,52,15,Thôi Lục Hoa}"
		};
	}
}
