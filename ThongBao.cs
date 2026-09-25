using System;

namespace TinhKiemAuto.Models
{
	// Token: 0x02000125 RID: 293
	public class ThongBao
	{
		// Token: 0x17000357 RID: 855
		// (get) Token: 0x06000F6F RID: 3951 RVA: 0x0007495D File Offset: 0x00072B5D
		// (set) Token: 0x06000F70 RID: 3952 RVA: 0x00074965 File Offset: 0x00072B65
		public string tideu { get; set; }

		// Token: 0x17000358 RID: 856
		// (get) Token: 0x06000F71 RID: 3953 RVA: 0x0007496E File Offset: 0x00072B6E
		// (set) Token: 0x06000F72 RID: 3954 RVA: 0x00074976 File Offset: 0x00072B76
		public string noidung { get; set; }

		// Token: 0x17000359 RID: 857
		// (get) Token: 0x06000F73 RID: 3955 RVA: 0x0007497F File Offset: 0x00072B7F
		// (set) Token: 0x06000F74 RID: 3956 RVA: 0x00074987 File Offset: 0x00072B87
		public CanhBao.Kieu Type { get; set; }
	}
}
