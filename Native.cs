using System;
using System.Runtime.InteropServices;
using System.Text;

namespace TinhKiemAuto
{
	// Token: 0x020000C1 RID: 193
	public class Native
	{
		// Token: 0x06000A90 RID: 2704
		[DllImport("psapi.dll")]
		public static extern bool EnumProcessModulesEx(IntPtr hProcess, [MarshalAs(UnmanagedType.LPArray, ArraySubType = UnmanagedType.U4)] [In] [Out] IntPtr[] lphModule, int cb, [MarshalAs(UnmanagedType.U4)] out int lpcbNeeded, uint dwFilterFlag);

		// Token: 0x06000A91 RID: 2705
		[DllImport("psapi.dll")]
		public static extern uint GetModuleFileNameEx(IntPtr hProcess, IntPtr hModule, [Out] StringBuilder lpBaseName, [MarshalAs(UnmanagedType.U4)] [In] uint nSize);

		// Token: 0x06000A92 RID: 2706
		[DllImport("psapi.dll", SetLastError = true)]
		public static extern bool GetModuleInformation(IntPtr hProcess, IntPtr hModule, out Native.ModuleInformation lpmodinfo, uint cb);

		// Token: 0x02000173 RID: 371
		public struct ModuleInformation
		{
			// Token: 0x04000E6F RID: 3695
			public IntPtr lpBaseOfDll;

			// Token: 0x04000E70 RID: 3696
			public uint SizeOfImage;

			// Token: 0x04000E71 RID: 3697
			public IntPtr EntryPoint;
		}

		// Token: 0x02000174 RID: 372
		internal enum ModuleFilter
		{
			// Token: 0x04000E73 RID: 3699
			ListModulesDefault,
			// Token: 0x04000E74 RID: 3700
			ListModules32Bit,
			// Token: 0x04000E75 RID: 3701
			ListModules64Bit,
			// Token: 0x04000E76 RID: 3702
			ListModulesAll
		}
	}
}
