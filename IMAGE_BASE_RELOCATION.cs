using System;
using System.Runtime.InteropServices;

namespace TinhKiemAuto
{
	// Token: 0x0200009F RID: 159
	[Serializable]
	[StructLayout(LayoutKind.Sequential, Pack = 1)]
	public struct IMAGE_BASE_RELOCATION
	{
		// Token: 0x040006B1 RID: 1713
		public uint VirtualAddress;

		// Token: 0x040006B2 RID: 1714
		public uint SizeOfBlock;
	}
}
