using System;
using System.Runtime.InteropServices;

namespace TinhKiemAuto
{
	// Token: 0x020000A1 RID: 161
	[Serializable]
	[StructLayout(LayoutKind.Sequential, Pack = 1)]
	public struct IMAGE_DOS_HEADER
	{
		// Token: 0x040006B5 RID: 1717
		public ushort e_magic;

		// Token: 0x040006B6 RID: 1718
		public ushort e_cblp;

		// Token: 0x040006B7 RID: 1719
		public ushort e_cp;

		// Token: 0x040006B8 RID: 1720
		public ushort e_crlc;

		// Token: 0x040006B9 RID: 1721
		public ushort e_cparhdr;

		// Token: 0x040006BA RID: 1722
		public ushort e_minalloc;

		// Token: 0x040006BB RID: 1723
		public ushort e_maxalloc;

		// Token: 0x040006BC RID: 1724
		public ushort e_ss;

		// Token: 0x040006BD RID: 1725
		public ushort e_sp;

		// Token: 0x040006BE RID: 1726
		public ushort e_csum;

		// Token: 0x040006BF RID: 1727
		public ushort e_ip;

		// Token: 0x040006C0 RID: 1728
		public ushort e_cs;

		// Token: 0x040006C1 RID: 1729
		public ushort e_lfarlc;

		// Token: 0x040006C2 RID: 1730
		public ushort e_ovno;

		// Token: 0x040006C3 RID: 1731
		public ushort e_res_0;

		// Token: 0x040006C4 RID: 1732
		public ushort e_res_1;

		// Token: 0x040006C5 RID: 1733
		public ushort e_res_2;

		// Token: 0x040006C6 RID: 1734
		public ushort e_res_3;

		// Token: 0x040006C7 RID: 1735
		public ushort e_oemid;

		// Token: 0x040006C8 RID: 1736
		public ushort e_oeminfo;

		// Token: 0x040006C9 RID: 1737
		public ushort e_res2_0;

		// Token: 0x040006CA RID: 1738
		public ushort e_res2_1;

		// Token: 0x040006CB RID: 1739
		public ushort e_res2_2;

		// Token: 0x040006CC RID: 1740
		public ushort e_res2_3;

		// Token: 0x040006CD RID: 1741
		public ushort e_res2_4;

		// Token: 0x040006CE RID: 1742
		public ushort e_res2_5;

		// Token: 0x040006CF RID: 1743
		public ushort e_res2_6;

		// Token: 0x040006D0 RID: 1744
		public ushort e_res2_7;

		// Token: 0x040006D1 RID: 1745
		public ushort e_res2_8;

		// Token: 0x040006D2 RID: 1746
		public ushort e_res2_9;

		// Token: 0x040006D3 RID: 1747
		public uint e_lfanew;
	}
}
