using System;

namespace TinhKiemAuto
{
	// Token: 0x0200010B RID: 267
	internal class THUCHACOTRAN
	{
		// Token: 0x04000C55 RID: 3157
		public static int Id = 260;

		// Token: 0x04000C56 RID: 3158
		public static NPC LyDa = new NPC
		{
			Id = 55,
			X = 150,
			Y = 152,
			Map = THUCHACOTRAN.Id,
			INFOAIM = "#GThúc Hà Cổ Trấn#RLý Dã#{_INFOAIM150,152,260,Lý Dã}"
		};

		// Token: 0x04000C57 RID: 3159
		public static NPC ThuongKho = new NPC
		{
			Id = 10,
			X = 200,
			Y = 253,
			Map = THUCHACOTRAN.Id,
			INFOAIM = "#GThúc Hà Cổ Trấn#RHầu bàn Lưu#{_INFOAIM200,253,260,Hầu bàn Lưu}"
		};

		// Token: 0x04000C58 RID: 3160
		public static NPC ThuongKhoTinhKiem = new NPC
		{
			Id = 10,
			X = 200,
			Y = 253,
			Map = THUCHACOTRAN.Id,
			INFOAIM = "#GThúc Hà Cổ Trấn#RLưu tiểu nhị#{_INFOAIM200,253,260,Lưu tiểu nhị}"
		};
	}
}
