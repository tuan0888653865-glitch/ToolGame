using System;

namespace TinhKiemAuto
{
	// Token: 0x02000105 RID: 261
	internal class MODUNG
	{
		// Token: 0x04000C3E RID: 3134
		public static int Id = 284;

		// Token: 0x04000C3F RID: 3135
		public static NPC MoDungKiet = new NPC
		{
			Id = 13,
			X = 48,
			Y = 144,
			Map = MODUNG.Id,
			INFOAIM = "#GMộ Dung Sơn Trang#RMộ Dung Kiệt#{_INFOAIM48,144,284,Mộ Dung Kiệt}"
		};

		// Token: 0x04000C40 RID: 3136
		public static NPC MoDungThang = new NPC
		{
			Id = 9,
			X = 69,
			Y = 126,
			Map = MODUNG.Id,
			INFOAIM = "#GMộ Dung Sơn Trang#RMộ Dung Thắng#{_INFOAIM69,126,284,Mộ Dung Thắng}"
		};

		// Token: 0x04000C41 RID: 3137
		public static NPC MoDungThanhSon = new NPC
		{
			Id = 14,
			X = 48,
			Y = 135,
			Map = MODUNG.Id,
			INFOAIM = "#GMộ Dung Sơn Trang#RMộ Dung Thanh Sơn#{_INFOAIM48,135,284,Mộ Dung Thanh Sơn}"
		};
	}
}
