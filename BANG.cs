using System;

namespace TinhKiemAuto
{
	// Token: 0x020000FE RID: 254
	internal class BANG
	{
		// Token: 0x04000C04 RID: 3076
		public static int Id = -1;

		// Token: 0x04000C05 RID: 3077
		public static NPC TienViNhat = new NPC
		{
			Id = 5,
			X = 149,
			Y = 56,
			Map = BANG.Id,
			INFOAIM = "#GLongHổĐịa#RTiền Vi Nhất#{_INFOAIM149,56,540,Tiền Vi Nhất}"
		};

		// Token: 0x04000C06 RID: 3078
		public static NPC DongPhuDung = new NPC
		{
			Id = 6,
			X = 148,
			Y = 96,
			Map = BANG.Id,
			INFOAIM = "#GNamDu#RĐông Phù Dung#{_INFOAIM148,96,525,Đông Phù Dung}"
		};

		// Token: 0x04000C07 RID: 3079
		public static NPC DoiTamKim = new NPC
		{
			Id = 13,
			X = 66,
			Y = 135,
			Map = BANG.Id
		};

		// Token: 0x04000C08 RID: 3080
		public static NPC TrinhVoDanh = new NPC
		{
			Id = 2,
			X = 100,
			Y = 55,
			Map = BANG.Id
		};
	}
}
