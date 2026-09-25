using System;
using System.Runtime.InteropServices;
using System.Text;

namespace TinhKiemAuto
{
	// Token: 0x020000F7 RID: 247
	public static class WinAPI
	{
		// Token: 0x06000CEB RID: 3307
		[DllImport("kernel32.dll", SetLastError = true)]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool CloseHandle(IntPtr handle);

		// Token: 0x06000CEC RID: 3308 RVA: 0x000531F8 File Offset: 0x000513F8
		public static IntPtr CreateRemotePointer(IntPtr hProcess, byte[] pData, int flProtect)
		{
			IntPtr intPtr = IntPtr.Zero;
			if (pData != null && hProcess != IntPtr.Zero)
			{
				intPtr = WinAPI.VirtualAllocEx(hProcess, IntPtr.Zero, (uint)pData.Length, 12288, flProtect);
				uint num = 0U;
				if (intPtr != IntPtr.Zero && WinAPI.WriteProcessMemory(hProcess, intPtr, pData, pData.Length, out num) && (ulong)num == (ulong)((long)pData.Length))
				{
					return intPtr;
				}
				if (intPtr != IntPtr.Zero)
				{
					WinAPI.VirtualFreeEx(hProcess, intPtr, 0, 32768);
					intPtr = IntPtr.Zero;
				}
			}
			return intPtr;
		}

		// Token: 0x06000CED RID: 3309
		[DllImport("kernel32.dll", SetLastError = true)]
		public static extern IntPtr CreateRemoteThread(IntPtr hProcess, int lpThreadAttributes, int dwStackSize, IntPtr lpStartAddress, uint lpParameter, int dwCreationFlags, int lpThreadId);

		// Token: 0x06000CEE RID: 3310
		[DllImport("kernel32.dll", SetLastError = true)]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool GetExitCodeThread(IntPtr hThread, out uint lpExitCode);

		// Token: 0x06000CEF RID: 3311 RVA: 0x0005327C File Offset: 0x0005147C
		public static uint GetLastErrorEx(IntPtr hProcess)
		{
			IntPtr procAddress = WinAPI.GetProcAddress(WinAPI.GetModuleHandleA("kernel32.dll"), "GetLastError");
			return WinAPI.RunThread(hProcess, procAddress, 0U, 1000);
		}

		// Token: 0x06000CF0 RID: 3312
		[DllImport("kernel32.dll", SetLastError = true)]
		public static extern IntPtr GetModuleHandleA(string lpModuleName);

		// Token: 0x06000CF1 RID: 3313 RVA: 0x000532AC File Offset: 0x000514AC
		public static IntPtr GetModuleHandleEx(IntPtr hProcess, string lpModuleName)
		{
			IntPtr procAddress = WinAPI.GetProcAddress(WinAPI.GetModuleHandleA("kernel32.dll"), "GetModuleHandleW");
			IntPtr result = IntPtr.Zero;
			if (!procAddress.IsNull())
			{
				IntPtr intPtr = WinAPI.CreateRemotePointer(hProcess, Encoding.Unicode.GetBytes(lpModuleName + "\0"), 4);
				if (!intPtr.IsNull())
				{
					result = Win32Ptr.Create((long)((ulong)WinAPI.RunThread(hProcess, procAddress, (uint)intPtr.ToInt32(), 1000)));
					WinAPI.VirtualFreeEx(hProcess, intPtr, 0, 32768);
				}
			}
			return result;
		}

		// Token: 0x06000CF2 RID: 3314
		[DllImport("kernel32.dll")]
		public static extern IntPtr LoadLibrary(string dllToLoad);

		// Token: 0x06000CF3 RID: 3315
		[DllImport("kernel32.dll", CharSet = CharSet.Ansi, SetLastError = true)]
		public static extern IntPtr GetProcAddress(IntPtr hModule, string lpProcName);

		// Token: 0x06000CF4 RID: 3316
		[DllImport("kernel32.dll", CharSet = CharSet.Ansi, SetLastError = true)]
		public static extern IntPtr GetProcAddress(IntPtr hModule, uint lpProcName);

		// Token: 0x06000CF5 RID: 3317 RVA: 0x0005332C File Offset: 0x0005152C
		public static IntPtr GetProcAddressEx(IntPtr hProc, IntPtr hModule, object lpProcName)
		{
			IntPtr result = IntPtr.Zero;
			byte[] array = WinAPI.ReadRemoteMemory(hProc, hModule, 64U);
			if (array == null || BitConverter.ToUInt16(array, 0) != 23117)
			{
				return result;
			}
			uint num = BitConverter.ToUInt32(array, 60);
			if (num <= 0U)
			{
				return result;
			}
			byte[] array2 = WinAPI.ReadRemoteMemory(hProc, hModule.Add((long)((ulong)num)), 264U);
			if (array2 == null || BitConverter.ToUInt32(array2, 0) != 17744U)
			{
				return result;
			}
			uint num2 = BitConverter.ToUInt32(array2, 120);
			uint num3 = BitConverter.ToUInt32(array2, 124);
			if (num2 <= 0U || num3 <= 0U)
			{
				return result;
			}
			byte[] array3 = WinAPI.ReadRemoteMemory(hProc, hModule.Add((long)((ulong)num2)), 40U);
			uint num4 = BitConverter.ToUInt32(array3, 28);
			uint num5 = BitConverter.ToUInt32(array3, 36);
			uint num6 = BitConverter.ToUInt32(array3, 20);
			int num7 = -1;
			if (num4 <= 0U || num5 <= 0U)
			{
				return result;
			}
			if (lpProcName.GetType().Equals(typeof(string)))
			{
				int num8 = WinAPI.SearchExports(hProc, hModule, array3, (string)lpProcName);
				if (num8 > -1)
				{
					byte[] array4 = WinAPI.ReadRemoteMemory(hProc, hModule.Add((long)((ulong)num5 + (ulong)((ulong)((long)num8) << 1))), 2U);
					num7 = ((array4 == null) ? -1 : ((int)BitConverter.ToUInt16(array4, 0)));
				}
			}
			else if (lpProcName.GetType().Equals(typeof(short)) || lpProcName.GetType().Equals(typeof(ushort)))
			{
				num7 = int.Parse(lpProcName.ToString());
			}
			if (num7 <= -1 || (long)num7 >= (long)((ulong)num6))
			{
				return result;
			}
			byte[] array5 = WinAPI.ReadRemoteMemory(hProc, hModule.Add((long)((ulong)num4 + (ulong)((ulong)((long)num7) << 2))), 4U);
			if (array5 == null)
			{
				return result;
			}
			uint num9 = BitConverter.ToUInt32(array5, 0);
			if (num9 >= num2 && num9 < num2 + num3)
			{
				string text = WinAPI.ReadRemoteString(hProc, hModule.Add((long)((ulong)num9)), null);
				if (!string.IsNullOrEmpty(text) && text.Contains("."))
				{
					result = WinAPI.GetProcAddressEx(hProc, WinAPI.GetModuleHandleEx(hProc, text.Split(new char[]
					{
						'.'
					})[0]), text.Split(new char[]
					{
						'.'
					})[1]);
				}
				return result;
			}
			return hModule.Add((long)((ulong)num9));
		}

		// Token: 0x06000CF6 RID: 3318
		[DllImport("kernel32.dll", SetLastError = true)]
		public static extern int GetProcessId(IntPtr hProcess);

		// Token: 0x06000CF7 RID: 3319
		[DllImport("kernel32.dll", SetLastError = true)]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool GetThreadContext(IntPtr hThread, ref WinAPI.CONTEXT pContext);

		// Token: 0x06000CF8 RID: 3320
		[DllImport("kernel32.dll", SetLastError = true)]
		public static extern IntPtr OpenProcess(uint dwDesiredAccess, bool bInheritHandle, int dwProcessId);

		// Token: 0x06000CF9 RID: 3321
		[DllImport("kernel32.dll", SetLastError = true)]
		public static extern IntPtr OpenThread(uint dwDesiredAccess, bool bInheritHandle, int dwThreadId);

		// Token: 0x06000CFA RID: 3322
		[DllImport("kernel32.dll", SetLastError = true)]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool ReadProcessMemory(IntPtr hProcess, IntPtr lpBaseAddress, byte[] lpBuffer, int dwSize, out uint lpNumberOfBytesRead);

		// Token: 0x06000CFB RID: 3323 RVA: 0x00053538 File Offset: 0x00051738
		public static byte[] ReadRemoteMemory(IntPtr hProc, IntPtr address, uint len)
		{
			byte[] array = new byte[len];
			uint num = 0U;
			if (!WinAPI.ReadProcessMemory(hProc, address, array, array.Length, out num) || num != len)
			{
				array = null;
			}
			return array;
		}

		// Token: 0x06000CFC RID: 3324 RVA: 0x00053564 File Offset: 0x00051764
		public static IntPtr ReadRemotePointer(IntPtr hProcess, IntPtr pData)
		{
			IntPtr zero = IntPtr.Zero;
			if (!hProcess.IsNull() && !pData.IsNull())
			{
				byte[] array = WinAPI.ReadRemoteMemory(hProcess, pData, (uint)IntPtr.Size);
				if (array != null)
				{
					zero = new IntPtr(BitConverter.ToInt32(array, 0));
				}
			}
			return zero;
		}

		// Token: 0x06000CFD RID: 3325 RVA: 0x000535A8 File Offset: 0x000517A8
		public static string ReadRemoteString(IntPtr hProcess, IntPtr lpAddress, Encoding encoding = null)
		{
			if (encoding == null)
			{
				encoding = Encoding.ASCII;
			}
			StringBuilder stringBuilder = new StringBuilder();
			byte[] array = new byte[256];
			uint num = 0U;
			int num2 = -1;
			while (num2 < 0 && WinAPI.ReadProcessMemory(hProcess, lpAddress, array, array.Length, out num) && num > 0U)
			{
				lpAddress = lpAddress.Add((long)((ulong)num));
				int length = stringBuilder.Length;
				stringBuilder.Append(encoding.GetString(array, 0, (int)num));
				num2 = stringBuilder.ToString().IndexOf('\0', length);
			}
			return stringBuilder.ToString().Substring(0, num2);
		}

		// Token: 0x06000CFE RID: 3326
		[DllImport("kernel32.dll", SetLastError = true)]
		public static extern uint ResumeThread(IntPtr hThread);

		// Token: 0x06000CFF RID: 3327 RVA: 0x0005362C File Offset: 0x0005182C
		public static uint RunThread(IntPtr hProcess, IntPtr lpStartAddress, uint lpParam, int timeout = 1000)
		{
			uint maxValue = uint.MaxValue;
			IntPtr intPtr = WinAPI.CreateRemoteThread(hProcess, 0, 0, lpStartAddress, lpParam, 0, 0);
			if (intPtr != IntPtr.Zero && (ulong)WinAPI.WaitForSingleObject(intPtr, timeout) == 0UL)
			{
				WinAPI.GetExitCodeThread(intPtr, out maxValue);
			}
			return maxValue;
		}

		// Token: 0x06000D00 RID: 3328 RVA: 0x0005366C File Offset: 0x0005186C
		private static int SearchExports(IntPtr hProcess, IntPtr hModule, byte[] exports, string name)
		{
			uint num = BitConverter.ToUInt32(exports, 24);
			uint num2 = BitConverter.ToUInt32(exports, 32);
			int num3 = -1;
			if (num > 0U && num2 > 0U)
			{
				byte[] array = WinAPI.ReadRemoteMemory(hProcess, hModule.Add((long)((ulong)num2)), num << 2);
				if (array == null)
				{
					return num3;
				}
				uint[] array2 = new uint[num];
				for (int i = 0; i < array2.Length; i++)
				{
					array2[i] = BitConverter.ToUInt32(array, i << 2);
				}
				int num4 = 0;
				int num5 = array2.Length - 1;
				string text = string.Empty;
				while (num4 >= 0 && num4 <= num5 && num3 == -1)
				{
					int num6 = (num4 + num5) / 2;
					text = WinAPI.ReadRemoteString(hProcess, hModule.Add((long)((ulong)array2[num6])), null);
					if (text.Equals(name))
					{
						num3 = num6;
					}
					else if (string.CompareOrdinal(text, name) < 0)
					{
						num4 = num6 - 1;
					}
					else
					{
						num5 = num6 + 1;
					}
				}
			}
			return num3;
		}

		// Token: 0x06000D01 RID: 3329
		[DllImport("kernel32.dll", SetLastError = true)]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool SetThreadContext(IntPtr hThread, ref WinAPI.CONTEXT pContext);

		// Token: 0x06000D02 RID: 3330
		[DllImport("kernel32.dll", SetLastError = true)]
		public static extern uint SuspendThread(IntPtr hThread);

		// Token: 0x06000D03 RID: 3331
		[DllImport("kernel32.dll", SetLastError = true)]
		public static extern IntPtr VirtualAllocEx(IntPtr hProcess, IntPtr lpAddress, uint dwSize, int flAllocationType, int flProtect);

		// Token: 0x06000D04 RID: 3332
		[DllImport("kernel32.dll", SetLastError = true)]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool VirtualFreeEx(IntPtr hProcess, IntPtr lpAddress, int dwSize, int dwFreeType);

		// Token: 0x06000D05 RID: 3333
		[DllImport("kernel32.dll", SetLastError = true)]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool VirtualProtectEx(IntPtr hProcess, IntPtr lpAddress, uint dwSize, uint flNewProtect, out uint flOldProtect);

		// Token: 0x06000D06 RID: 3334
		[DllImport("kernel32.dll", SetLastError = true)]
		public static extern uint WaitForSingleObject(IntPtr hObject, int dwTimeout);

		// Token: 0x06000D07 RID: 3335
		[DllImport("kernel32.dll", SetLastError = true)]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool WriteProcessMemory(IntPtr hProcess, IntPtr lpAddress, byte[] lpBuffer, int dwSize, out uint lpNumberOfBytesRead);

		// Token: 0x0200018E RID: 398
		public struct CONTEXT
		{
			// Token: 0x04000EF5 RID: 3829
			public uint ContextFlags;

			// Token: 0x04000EF6 RID: 3830
			public uint Dr0;

			// Token: 0x04000EF7 RID: 3831
			public uint Dr1;

			// Token: 0x04000EF8 RID: 3832
			public uint Dr2;

			// Token: 0x04000EF9 RID: 3833
			public uint Dr3;

			// Token: 0x04000EFA RID: 3834
			public uint Dr6;

			// Token: 0x04000EFB RID: 3835
			public uint Dr7;

			// Token: 0x04000EFC RID: 3836
			public WinAPI.FLOATING_SAVE_AREA FloatSave;

			// Token: 0x04000EFD RID: 3837
			public uint SegGs;

			// Token: 0x04000EFE RID: 3838
			public uint SegFs;

			// Token: 0x04000EFF RID: 3839
			public uint SegEs;

			// Token: 0x04000F00 RID: 3840
			public uint SegDs;

			// Token: 0x04000F01 RID: 3841
			public uint Edi;

			// Token: 0x04000F02 RID: 3842
			public uint Esi;

			// Token: 0x04000F03 RID: 3843
			public uint Ebx;

			// Token: 0x04000F04 RID: 3844
			public uint Edx;

			// Token: 0x04000F05 RID: 3845
			public uint Ecx;

			// Token: 0x04000F06 RID: 3846
			public uint Eax;

			// Token: 0x04000F07 RID: 3847
			public uint Ebp;

			// Token: 0x04000F08 RID: 3848
			public uint Eip;

			// Token: 0x04000F09 RID: 3849
			public uint SegCs;

			// Token: 0x04000F0A RID: 3850
			public uint EFlags;

			// Token: 0x04000F0B RID: 3851
			public uint Esp;

			// Token: 0x04000F0C RID: 3852
			public uint SegSs;

			// Token: 0x04000F0D RID: 3853
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 512)]
			public byte[] ExtendedRegisters;
		}

		// Token: 0x0200018F RID: 399
		public struct FLOATING_SAVE_AREA
		{
			// Token: 0x04000F0E RID: 3854
			public uint ControlWord;

			// Token: 0x04000F0F RID: 3855
			public uint StatusWord;

			// Token: 0x04000F10 RID: 3856
			public uint TagWord;

			// Token: 0x04000F11 RID: 3857
			public uint ErrorOffset;

			// Token: 0x04000F12 RID: 3858
			public uint ErrorSelector;

			// Token: 0x04000F13 RID: 3859
			public uint DataOffset;

			// Token: 0x04000F14 RID: 3860
			public uint DataSelector;

			// Token: 0x04000F15 RID: 3861
			[MarshalAs(UnmanagedType.ByValArray, SizeConst = 80)]
			public byte[] RegisterArea;

			// Token: 0x04000F16 RID: 3862
			public uint Cr0NpxState;
		}
	}
}
