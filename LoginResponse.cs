using System;

namespace TinhKiemAuto.Models
{
	// Token: 0x0200012D RID: 301
	public class LoginResponse
	{
		// Token: 0x17000371 RID: 881
		// (get) Token: 0x06000FAB RID: 4011 RVA: 0x00074B17 File Offset: 0x00072D17
		// (set) Token: 0x06000FAC RID: 4012 RVA: 0x00074B1F File Offset: 0x00072D1F
		public string offsetcode { get; set; }

		// Token: 0x17000372 RID: 882
		// (get) Token: 0x06000FAD RID: 4013 RVA: 0x00074B28 File Offset: 0x00072D28
		// (set) Token: 0x06000FAE RID: 4014 RVA: 0x00074B30 File Offset: 0x00072D30
		public string email { get; set; }

		// Token: 0x17000373 RID: 883
		// (get) Token: 0x06000FAF RID: 4015 RVA: 0x00074B39 File Offset: 0x00072D39
		// (set) Token: 0x06000FB0 RID: 4016 RVA: 0x00074B41 File Offset: 0x00072D41
		public string msg { get; set; }

		// Token: 0x17000374 RID: 884
		// (get) Token: 0x06000FB1 RID: 4017 RVA: 0x00074B4A File Offset: 0x00072D4A
		// (set) Token: 0x06000FB2 RID: 4018 RVA: 0x00074B52 File Offset: 0x00072D52
		public int status { get; set; }
	}
}
