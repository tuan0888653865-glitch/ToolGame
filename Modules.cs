using System;

namespace TinhKiemAuto
{
	// Token: 0x020000BF RID: 191
	public class Modules
	{
		// Token: 0x06000A7F RID: 2687 RVA: 0x0004369F File Offset: 0x0004189F
		public Modules(string moduleName, IntPtr baseAddress, uint size)
		{
			this.ModuleName = moduleName;
			this.BaseAddress = baseAddress;
			this.Size = size;
		}

		// Token: 0x17000259 RID: 601
		// (get) Token: 0x06000A80 RID: 2688 RVA: 0x000436BC File Offset: 0x000418BC
		// (set) Token: 0x06000A81 RID: 2689 RVA: 0x000436C4 File Offset: 0x000418C4
		public string ModuleName { get; set; }

		// Token: 0x1700025A RID: 602
		// (get) Token: 0x06000A82 RID: 2690 RVA: 0x000436CD File Offset: 0x000418CD
		// (set) Token: 0x06000A83 RID: 2691 RVA: 0x000436D5 File Offset: 0x000418D5
		public IntPtr BaseAddress { get; set; }

		// Token: 0x1700025B RID: 603
		// (get) Token: 0x06000A84 RID: 2692 RVA: 0x000436DE File Offset: 0x000418DE
		// (set) Token: 0x06000A85 RID: 2693 RVA: 0x000436E6 File Offset: 0x000418E6
		public uint Size { get; set; }
	}
}
