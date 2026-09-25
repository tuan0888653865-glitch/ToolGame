using System;

namespace TinhKiemAuto
{
	// Token: 0x020000FF RID: 255
	internal class CAIBANG
	{
		// Token: 0x04000C09 RID: 3081
		public static int Id = 10;

		// Token: 0x04000C0A RID: 3082
		public static NPC TranCoNhan = new NPC
		{
			Id = 23,
			X = 92,
			Y = 99,
			Map = CAIBANG.Id,
			INFOAIM = "#GCái Bang Tổng Đà #RTrần Cô Nhạn#{_INFOAIM92,99,10,Trần Cô Nhạn}"
		};

		// Token: 0x04000C0B RID: 3083
		public static NPC HongThong = new NPC
		{
			Id = 16,
			X = 92,
			Y = 77,
			Map = CAIBANG.Id,
			INFOAIM = "#GCái Bang Tổng Đà #RHồng Thông#{_INFOAIM92,77,10,Hồng Thông}"
		};

		// Token: 0x04000C0C RID: 3084
		public static NPC HeTamKi = new NPC
		{
			Id = 0,
			X = 94,
			Y = 99,
			Map = CAIBANG.Id,
			INFOAIM = "#GCái Bang Tổng Đà #RHề Tam Kì#{_INFOAIM94,99,10,Hề Tam Kì}"
		};
	}
}
