using System;

namespace TinhKiemAuto.Models
{
	// Token: 0x02000132 RID: 306
	public class NearScene
	{
		// Token: 0x06000FD1 RID: 4049 RVA: 0x000755C5 File Offset: 0x000737C5
		public NearScene(Scene scene, int portalX, int portalY)
		{
			this.scene = scene;
			this.nearScenePortalX = portalX;
			this.nearScenePortalY = portalY;
		}

		// Token: 0x04000CE3 RID: 3299
		public readonly Scene scene;

		// Token: 0x04000CE4 RID: 3300
		public readonly int nearScenePortalX;

		// Token: 0x04000CE5 RID: 3301
		public readonly int nearScenePortalY;
	}
}
