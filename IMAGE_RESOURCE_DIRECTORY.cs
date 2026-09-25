using System;

namespace TinhKiemAuto
{
	// Token: 0x020000A7 RID: 167
	[Serializable]
	public struct IMAGE_RESOURCE_DIRECTORY
	{
		// Token: 0x04000706 RID: 1798
		public uint Characteristics;

		// Token: 0x04000707 RID: 1799
		public uint TimeDateStamp;

		// Token: 0x04000708 RID: 1800
		public ushort MajorVersion;

		// Token: 0x04000709 RID: 1801
		public ushort MinorVersion;

		// Token: 0x0400070A RID: 1802
		public ushort NumberOfNamedEntries;

		// Token: 0x0400070B RID: 1803
		public ushort NumberOfIdEntries;
	}
}
