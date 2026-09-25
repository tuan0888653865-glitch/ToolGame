using System;

namespace TinhKiemAuto.Models
{
	// Token: 0x02000136 RID: 310
	public class Scene
	{
		// Token: 0x06000FE5 RID: 4069 RVA: 0x000756BF File Offset: 0x000738BF
		public Scene(int sceneId, string sceneName)
		{
			this.sceneId = sceneId;
			this.sceneName = sceneName;
		}

		// Token: 0x04000CFA RID: 3322
		public readonly int sceneId;

		// Token: 0x04000CFB RID: 3323
		public readonly string sceneName;
	}
}
