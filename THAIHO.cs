using System;

namespace TinhKiemAuto
{
	// Token: 0x02000107 RID: 263
	internal class THAIHO
	{
		// Token: 0x04000C46 RID: 3142
		public static int Id = 4;

		// Token: 0x04000C47 RID: 3143
		public static NPC HoDienKhanh = new NPC
		{
			Id = 13,
			X = 67,
			Y = 77,
			Map = THAIHO.Id,
			INFOAIM = "#GThái Hồ#RHô Diên Khánh #{_INFOAIM67,77,4,Hô Diên Khánh }"
		};

		// Token: 0x04000C48 RID: 3144
		public static NPC LyCuong = new NPC
		{
			Id = 0,
			X = 70,
			Y = 119,
			Map = THAIHO.Id,
			INFOAIM = "#GThái Hồ#RLý Cương#{_INFOAIM70,119,4,Lý Cương}"
		};
	}
}
