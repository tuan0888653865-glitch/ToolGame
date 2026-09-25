using System;
using System.Runtime.InteropServices;

namespace TinhKiemAuto
{
	// Token: 0x020000A2 RID: 162
	[Serializable]
	[StructLayout(LayoutKind.Sequential, Pack = 1)]
	public struct IMAGE_FILE_HEADER
	{
		// Token: 0x040006D4 RID: 1748
		public ushort Machine;

		// Token: 0x040006D5 RID: 1749
		public ushort NumberOfSections;

		// Token: 0x040006D6 RID: 1750
		public uint TimeDateStamp;

		// Token: 0x040006D7 RID: 1751
		public uint PointerToSymbolTable;

		// Token: 0x040006D8 RID: 1752
		public uint NumberOfSymbols;

		// Token: 0x040006D9 RID: 1753
		public ushort SizeOfOptionalHeader;

		// Token: 0x040006DA RID: 1754
		public ushort Characteristics;
	}
}
