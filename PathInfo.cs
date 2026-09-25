using System;

namespace TinhKiemAuto.Models
{
	// Token: 0x02000135 RID: 309
	public class PathInfo
	{
		// Token: 0x06000FE4 RID: 4068 RVA: 0x00075692 File Offset: 0x00073892
		public PathInfo(bool isNPC, int x, int y, int idNext, string npcname)
		{
			this.isNPC = isNPC;
			this.x = x;
			this.y = y;
			this.idNext = idNext;
			this.NpcName = npcname;
		}

		// Token: 0x04000CF5 RID: 3317
		public readonly bool isNPC;

		// Token: 0x04000CF6 RID: 3318
		public readonly int x;

		// Token: 0x04000CF7 RID: 3319
		public readonly int y;

		// Token: 0x04000CF8 RID: 3320
		public readonly int idNext;

		// Token: 0x04000CF9 RID: 3321
		public readonly string NpcName;
	}
}
