using System;
using System.Runtime.InteropServices;

namespace TinhKiemAuto
{
	// Token: 0x020000A3 RID: 163
	[Serializable]
	[StructLayout(LayoutKind.Sequential, Pack = 1)]
	public struct IMAGE_IMPORT_DESCRIPTOR
	{
		// Token: 0x040006DB RID: 1755
		public uint OriginalFirstThunk;

		// Token: 0x040006DC RID: 1756
		public uint TimeDateStamp;

		// Token: 0x040006DD RID: 1757
		public uint ForwarderChain;

		// Token: 0x040006DE RID: 1758
		public uint Name;

		// Token: 0x040006DF RID: 1759
		public uint FirstThunkPtr;
	}
}
