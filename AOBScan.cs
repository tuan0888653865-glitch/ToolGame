using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace TinhKiemAuto
{
	// Token: 0x0200007E RID: 126
	public class AOBScan
	{
		// Token: 0x060004D1 RID: 1233 RVA: 0x0001C260 File Offset: 0x0001A460
		public AOBScan(uint ProcessID)
		{
			this.ProcessID = ProcessID;
		}

		// Token: 0x060004D2 RID: 1234
		[DllImport("kernel32.dll")]
		protected static extern bool ReadProcessMemory(IntPtr hProcess, IntPtr lpBaseAddress, byte[] buffer, uint size, int lpNumberOfBytesRead);

		// Token: 0x060004D3 RID: 1235
		[DllImport("kernel32.dll")]
		protected static extern int VirtualQueryEx(IntPtr hProcess, IntPtr lpAddress, out AOBScan.MEMORY_BASIC_INFORMATION lpBuffer, int dwLength);

		// Token: 0x17000108 RID: 264
		// (get) Token: 0x060004D4 RID: 1236 RVA: 0x0001C26F File Offset: 0x0001A46F
		// (set) Token: 0x060004D5 RID: 1237 RVA: 0x0001C277 File Offset: 0x0001A477
		protected List<AOBScan.MEMORY_BASIC_INFORMATION> MemoryRegion { get; set; }

		// Token: 0x060004D6 RID: 1238 RVA: 0x0001C280 File Offset: 0x0001A480
		protected void MemInfo(IntPtr pHandle)
		{
			IntPtr lpAddress = (IntPtr)0;
			for (;;)
			{
				AOBScan.MEMORY_BASIC_INFORMATION memory_BASIC_INFORMATION = default(AOBScan.MEMORY_BASIC_INFORMATION);
				if (AOBScan.VirtualQueryEx(pHandle, lpAddress, out memory_BASIC_INFORMATION, Marshal.SizeOf(memory_BASIC_INFORMATION)) == 0)
				{
					break;
				}
				if ((memory_BASIC_INFORMATION.State & 4096U) != 0U && (memory_BASIC_INFORMATION.Protect & 256U) == 0U)
				{
					this.MemoryRegion.Add(memory_BASIC_INFORMATION);
				}
				lpAddress = new IntPtr(memory_BASIC_INFORMATION.BaseAddress.ToInt32() + (int)memory_BASIC_INFORMATION.RegionSize);
			}
		}

		// Token: 0x060004D7 RID: 1239 RVA: 0x0001C2F8 File Offset: 0x0001A4F8
		public static bool Compare(byte[] input, byte[] pattern, int index)
		{
			for (int i = 0; i < pattern.Length; i++)
			{
				if (pattern[i] != 255)
				{
					if (pattern[i] == 1)
					{
						if (input[index + i] == 0)
						{
							return false;
						}
					}
					else if (input[index + i] != pattern[i])
					{
						return false;
					}
				}
			}
			return true;
		}

		// Token: 0x060004D8 RID: 1240 RVA: 0x0001C33C File Offset: 0x0001A53C
		protected IntPtr Scan(byte[] sIn, byte[] sFor)
		{
			for (int i = 0; i < sIn.Length - sFor.Length; i++)
			{
				if (AOBScan.Compare(sIn, sFor, i))
				{
					return (IntPtr)i;
				}
			}
			return IntPtr.Zero;
		}

		// Token: 0x060004D9 RID: 1241 RVA: 0x0001C374 File Offset: 0x0001A574
		public IntPtr AobScan(byte[] Pattern, uint startAddress, uint endAddress)
		{
			Process processById = Process.GetProcessById((int)this.ProcessID);
			if (processById.Id == 0)
			{
				return IntPtr.Zero;
			}
			this.MemoryRegion = new List<AOBScan.MEMORY_BASIC_INFORMATION>();
			this.MemInfo(processById.Handle);
			for (int i = 0; i < this.MemoryRegion.Count; i++)
			{
				if ((int)this.MemoryRegion[i].BaseAddress >= (int)startAddress && (int)this.MemoryRegion[i].BaseAddress <= (int)endAddress)
				{
					byte[] array = new byte[this.MemoryRegion[i].RegionSize];
					AOBScan.ReadProcessMemory(processById.Handle, this.MemoryRegion[i].BaseAddress, array, this.MemoryRegion[i].RegionSize, 0);
					IntPtr value = this.Scan(array, Pattern);
					if (value != IntPtr.Zero)
					{
						return new IntPtr(this.MemoryRegion[i].BaseAddress.ToInt32() + value.ToInt32());
					}
				}
			}
			return IntPtr.Zero;
		}

		// Token: 0x060004DA RID: 1242 RVA: 0x0001C490 File Offset: 0x0001A690
		public IntPtr AobScan(byte[] Pattern, int startAddress)
		{
			Process processById = Process.GetProcessById((int)this.ProcessID);
			if (processById.Id == 0)
			{
				return IntPtr.Zero;
			}
			this.MemoryRegion = new List<AOBScan.MEMORY_BASIC_INFORMATION>();
			this.MemInfo(processById.Handle);
			for (int i = 0; i < this.MemoryRegion.Count; i++)
			{
				if ((int)this.MemoryRegion[i].BaseAddress >= startAddress)
				{
					byte[] array = new byte[this.MemoryRegion[i].RegionSize];
					AOBScan.ReadProcessMemory(processById.Handle, this.MemoryRegion[i].BaseAddress, array, this.MemoryRegion[i].RegionSize, 0);
					IntPtr value = this.Scan(array, Pattern);
					if (value != IntPtr.Zero)
					{
						return new IntPtr(this.MemoryRegion[i].BaseAddress.ToInt32() + value.ToInt32());
					}
				}
			}
			return IntPtr.Zero;
		}

		// Token: 0x060004DB RID: 1243 RVA: 0x0001C590 File Offset: 0x0001A790
		public IntPtr AobScan(byte[] Pattern)
		{
			Process processById = Process.GetProcessById((int)this.ProcessID);
			if (processById.Id == 0)
			{
				return IntPtr.Zero;
			}
			this.MemoryRegion = new List<AOBScan.MEMORY_BASIC_INFORMATION>();
			this.MemInfo(processById.Handle);
			for (int i = 0; i < this.MemoryRegion.Count; i++)
			{
				byte[] array = new byte[this.MemoryRegion[i].RegionSize];
				AOBScan.ReadProcessMemory(processById.Handle, this.MemoryRegion[i].BaseAddress, array, this.MemoryRegion[i].RegionSize, 0);
				IntPtr value = this.Scan(array, Pattern);
				if (value != IntPtr.Zero)
				{
					return new IntPtr(this.MemoryRegion[i].BaseAddress.ToInt32() + value.ToInt32());
				}
			}
			return IntPtr.Zero;
		}

		// Token: 0x040003B6 RID: 950
		protected uint ProcessID;

		// Token: 0x02000167 RID: 359
		protected struct MEMORY_BASIC_INFORMATION
		{
			// Token: 0x04000E39 RID: 3641
			public IntPtr BaseAddress;

			// Token: 0x04000E3A RID: 3642
			public IntPtr AllocationBase;

			// Token: 0x04000E3B RID: 3643
			public uint AllocationProtect;

			// Token: 0x04000E3C RID: 3644
			public uint RegionSize;

			// Token: 0x04000E3D RID: 3645
			public uint State;

			// Token: 0x04000E3E RID: 3646
			public uint Protect;

			// Token: 0x04000E3F RID: 3647
			public uint Type;
		}
	}
}
