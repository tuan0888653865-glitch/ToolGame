using System;
using System.Runtime.InteropServices;

namespace TinhKiemAuto
{
	// Token: 0x020000A8 RID: 168
	[Serializable]
	[StructLayout(LayoutKind.Explicit)]
	public struct IMAGE_RESOURCE_DIRECTORY_ENTRY
	{
		// Token: 0x0400070C RID: 1804
		[FieldOffset(4)]
		public uint DataEntryRva;

		// Token: 0x0400070D RID: 1805
		[FieldOffset(0)]
		public uint IntegerId;

		// Token: 0x0400070E RID: 1806
		[FieldOffset(0)]
		public uint NameRva;

		// Token: 0x0400070F RID: 1807
		[FieldOffset(4)]
		public uint SubdirectoryRva;
	}
}
