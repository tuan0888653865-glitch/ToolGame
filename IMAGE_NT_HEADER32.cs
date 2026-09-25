using System;
using System.Runtime.InteropServices;

namespace TinhKiemAuto
{
	// Token: 0x020000A4 RID: 164
	[Serializable]
	[StructLayout(LayoutKind.Sequential, Pack = 1)]
	public struct IMAGE_NT_HEADER32
	{
		// Token: 0x040006E0 RID: 1760
		public int Signature;

		// Token: 0x040006E1 RID: 1761
		public IMAGE_FILE_HEADER FileHeader;

		// Token: 0x040006E2 RID: 1762
		public IMAGE_OPTIONAL_HEADER32 OptionalHeader;
	}
}
