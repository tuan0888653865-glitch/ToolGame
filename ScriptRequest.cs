using System;

namespace TinhKiemAuto.Models
{
	// Token: 0x02000128 RID: 296
	public class ScriptRequest
	{
		// Token: 0x1700035E RID: 862
		// (get) Token: 0x06000F80 RID: 3968 RVA: 0x000749D4 File Offset: 0x00072BD4
		// (set) Token: 0x06000F81 RID: 3969 RVA: 0x000749DC File Offset: 0x00072BDC
		public string cmd { get; set; }

		// Token: 0x1700035F RID: 863
		// (get) Token: 0x06000F82 RID: 3970 RVA: 0x000749E5 File Offset: 0x00072BE5
		// (set) Token: 0x06000F83 RID: 3971 RVA: 0x000749ED File Offset: 0x00072BED
		public string serial { get; set; }

		// Token: 0x17000360 RID: 864
		// (get) Token: 0x06000F84 RID: 3972 RVA: 0x000749F6 File Offset: 0x00072BF6
		// (set) Token: 0x06000F85 RID: 3973 RVA: 0x000749FE File Offset: 0x00072BFE
		public string version { get; set; }

		// Token: 0x17000361 RID: 865
		// (get) Token: 0x06000F86 RID: 3974 RVA: 0x00074A07 File Offset: 0x00072C07
		// (set) Token: 0x06000F87 RID: 3975 RVA: 0x00074A0F File Offset: 0x00072C0F
		public string md { get; set; }
	}
}
