using System;
using System.Runtime.InteropServices;
using System.Text;

namespace TinhKiemAuto
{
	// Token: 0x020000A9 RID: 169
	[Serializable]
	[StructLayout(LayoutKind.Sequential, Pack = 1)]
	public struct IMAGE_SECTION_HEADER
	{
		// Token: 0x060009B8 RID: 2488 RVA: 0x000402B4 File Offset: 0x0003E4B4
		public override string ToString()
		{
			string text = Encoding.UTF8.GetString(this.Name);
			if (text.Contains("\0"))
			{
				text = text.Substring(0, text.IndexOf("\0"));
			}
			return text;
		}

		// Token: 0x04000710 RID: 1808
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
		public byte[] Name;

		// Token: 0x04000711 RID: 1809
		public uint VirtualSize;

		// Token: 0x04000712 RID: 1810
		public uint VirtualAddress;

		// Token: 0x04000713 RID: 1811
		public uint SizeOfRawData;

		// Token: 0x04000714 RID: 1812
		public uint PointerToRawData;

		// Token: 0x04000715 RID: 1813
		public uint PointerToRelocations;

		// Token: 0x04000716 RID: 1814
		public uint PointerToLineNumbers;

		// Token: 0x04000717 RID: 1815
		public ushort NumberOfRelocations;

		// Token: 0x04000718 RID: 1816
		public ushort NumberOfLineNumbers;

		// Token: 0x04000719 RID: 1817
		public uint Characteristics;
	}
}
