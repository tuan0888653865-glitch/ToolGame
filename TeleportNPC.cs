using System;

namespace TinhKiemAuto.Models
{
	// Token: 0x02000138 RID: 312
	public class TeleportNPC
	{
		// Token: 0x06000FE9 RID: 4073 RVA: 0x00075741 File Offset: 0x00073941
		public TeleportNPC(string npcName, int teleportationCost, int posX, int posY)
		{
			this.npcName = npcName;
			this.teleportationCost = teleportationCost;
			this.posX = posX;
			this.posY = posY;
		}

		// Token: 0x04000CFF RID: 3327
		public readonly string npcName;

		// Token: 0x04000D00 RID: 3328
		public readonly int teleportationCost;

		// Token: 0x04000D01 RID: 3329
		public readonly int posX;

		// Token: 0x04000D02 RID: 3330
		public readonly int posY;
	}
}
