using System;
using System.Runtime.InteropServices;

namespace TinhKiemAuto
{
	// Token: 0x020000EE RID: 238
	[Serializable]
	[StructLayout(LayoutKind.Explicit)]
	public struct U1
	{
		// Token: 0x04000962 RID: 2402
		[FieldOffset(0)]
		public uint AddressOfData;

		// Token: 0x04000963 RID: 2403
		[FieldOffset(0)]
		public uint ForwarderString;

		// Token: 0x04000964 RID: 2404
		[FieldOffset(0)]
		public uint Function;

		// Token: 0x04000965 RID: 2405
		[FieldOffset(0)]
		public uint Ordinal;
	}
}
