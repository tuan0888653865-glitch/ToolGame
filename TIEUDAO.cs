using System;

namespace TinhKiemAuto
{
	// Token: 0x0200010C RID: 268
	internal class TIEUDAO
	{
		// Token: 0x04000C59 RID: 3161
		public static int Id = 14;

		// Token: 0x04000C5A RID: 3162
		public static NPC ToTinhHa = new NPC
		{
			Id = 0,
			X = 126,
			Y = 145,
			Map = TIEUDAO.Id,
			INFOAIM = "#GLăng Ba Động #RTô Tinh Hà#{_INFOAIM126,145,14,Tô Tinh Hà}"
		};

		// Token: 0x04000C5B RID: 3163
		public static NPC TanQuan = new NPC
		{
			Id = 13,
			X = 119,
			Y = 152,
			Map = TIEUDAO.Id,
			INFOAIM = "#GLăng Ba Động #RTần Quán#{_INFOAIM119,152,14,Tần Quán}"
		};

		// Token: 0x04000C5C RID: 3164
		public static NPC KhangQuangLang = new NPC
		{
			Id = 1,
			X = 125,
			Y = 142,
			Map = TIEUDAO.Id,
			INFOAIM = "#GLăng Ba Động #RKhang Quảng Lăng#{_INFOAIM125,142,14,Khang Quảng Lăng}"
		};
	}
}
