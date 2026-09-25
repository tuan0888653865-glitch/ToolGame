using System;
using System.Runtime.InteropServices;

namespace TinhKiemAuto
{
	// Token: 0x020000A5 RID: 165
	[Serializable]
	[StructLayout(LayoutKind.Sequential, Pack = 1)]
	public struct IMAGE_OPTIONAL_HEADER32
	{
		// Token: 0x040006E3 RID: 1763
		public ushort Magic;

		// Token: 0x040006E4 RID: 1764
		public byte MajorLinkerVersion;

		// Token: 0x040006E5 RID: 1765
		public byte MinorLinkerVersion;

		// Token: 0x040006E6 RID: 1766
		public uint SizeOfCode;

		// Token: 0x040006E7 RID: 1767
		public uint SizeOfInitializedData;

		// Token: 0x040006E8 RID: 1768
		public uint SizeOfUninitializedData;

		// Token: 0x040006E9 RID: 1769
		public uint AddressOfEntryPoint;

		// Token: 0x040006EA RID: 1770
		public uint BaseOfCode;

		// Token: 0x040006EB RID: 1771
		public uint BaseOfData;

		// Token: 0x040006EC RID: 1772
		public uint ImageBase;

		// Token: 0x040006ED RID: 1773
		public uint SectionAlignment;

		// Token: 0x040006EE RID: 1774
		public uint FileAlignment;

		// Token: 0x040006EF RID: 1775
		public ushort MajorOperatingSystemVersion;

		// Token: 0x040006F0 RID: 1776
		public ushort MinorOperatingSystemVersion;

		// Token: 0x040006F1 RID: 1777
		public ushort MajorImageVersion;

		// Token: 0x040006F2 RID: 1778
		public ushort MinorImageVersion;

		// Token: 0x040006F3 RID: 1779
		public ushort MajorSubsystemVersion;

		// Token: 0x040006F4 RID: 1780
		public ushort MinorSubsystemVersion;

		// Token: 0x040006F5 RID: 1781
		public uint Win32VersionValue;

		// Token: 0x040006F6 RID: 1782
		public uint SizeOfImage;

		// Token: 0x040006F7 RID: 1783
		public uint SizeOfHeaders;

		// Token: 0x040006F8 RID: 1784
		public uint CheckSum;

		// Token: 0x040006F9 RID: 1785
		public ushort Subsystem;

		// Token: 0x040006FA RID: 1786
		public ushort DllCharacteristics;

		// Token: 0x040006FB RID: 1787
		public uint SizeOfStackReserve;

		// Token: 0x040006FC RID: 1788
		public uint SizeOfStackCommit;

		// Token: 0x040006FD RID: 1789
		public uint SizeOfHeapReserve;

		// Token: 0x040006FE RID: 1790
		public uint SizeOfHeapCommit;

		// Token: 0x040006FF RID: 1791
		public uint LoaderFlags;

		// Token: 0x04000700 RID: 1792
		public uint NumberOfRvaAndSizes;

		// Token: 0x04000701 RID: 1793
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]
		public IMAGE_DATA_DIRECTORY[] DataDirectory;
	}
}
