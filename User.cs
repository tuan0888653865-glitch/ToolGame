using System;

namespace TinhKiemAuto.Controllers
{
	// Token: 0x02000140 RID: 320
	public class User
	{
		// Token: 0x14000014 RID: 20
		// (add) Token: 0x06001043 RID: 4163 RVA: 0x00076850 File Offset: 0x00074A50
		// (remove) Token: 0x06001044 RID: 4164 RVA: 0x00076888 File Offset: 0x00074A88
		public event EventHandler Updated;

		// Token: 0x17000391 RID: 913
		// (get) Token: 0x06001045 RID: 4165 RVA: 0x000768BD File Offset: 0x00074ABD
		// (set) Token: 0x06001046 RID: 4166 RVA: 0x000768C4 File Offset: 0x00074AC4
		public static bool IsAlarm { get; set; }

		// Token: 0x04000D26 RID: 3366
		public static int TienXu = 50000;

		// Token: 0x04000D27 RID: 3367
		public static int ThoiGian = 50000;

		// Token: 0x04000D29 RID: 3369
		public static string Email = "tlbb@gmail.com";

		// Token: 0x04000D2A RID: 3370
		public static string Pass = "123456";
	}
}
