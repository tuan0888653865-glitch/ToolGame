using System;
using System.Collections.Generic;

namespace TinhKiemAuto.Models
{
	// Token: 0x02000137 RID: 311
	public class SceneInfo
	{
		// Token: 0x06000FE6 RID: 4070 RVA: 0x000756D5 File Offset: 0x000738D5
		public SceneInfo(Scene scene)
		{
			this.scene = scene;
		}

		// Token: 0x06000FE7 RID: 4071 RVA: 0x000756FC File Offset: 0x000738FC
		public void AddNearScene(Scene scene, int portalX, int portalY)
		{
			NearScene item = new NearScene(scene, portalX, portalY);
			this.nearScenes.Add(item);
		}

		// Token: 0x06000FE8 RID: 4072 RVA: 0x0007571E File Offset: 0x0007391E
		public void AddTeleportNPC(TeleportNPC npc)
		{
			if (npc.npcName.CompareTo("N/A") != 0)
			{
				this.teleportNPCs.Add(npc);
			}
		}

		// Token: 0x04000CFC RID: 3324
		public readonly Scene scene;

		// Token: 0x04000CFD RID: 3325
		public readonly List<NearScene> nearScenes = new List<NearScene>();

		// Token: 0x04000CFE RID: 3326
		public readonly List<TeleportNPC> teleportNPCs = new List<TeleportNPC>();
	}
}
