using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;

namespace TinhKiemAuto
{
	// Token: 0x020000AC RID: 172
	internal class IniFileEx
	{
		// Token: 0x060009BB RID: 2491
		[DllImport("kernel32", CharSet = CharSet.Unicode)]
		private static extern int GetPrivateProfileString(string section, string key, string defaultValue, StringBuilder value, int size, string filePath);

		// Token: 0x060009BC RID: 2492
		[DllImport("kernel32.dll", CharSet = CharSet.Unicode)]
		private static extern int GetPrivateProfileString(string section, string key, string defaultValue, [In] [Out] char[] value, int size, string filePath);

		// Token: 0x060009BD RID: 2493
		[DllImport("kernel32.dll", CharSet = CharSet.Auto)]
		private static extern int GetPrivateProfileSection(string section, IntPtr keyValue, int size, string filePath);

		// Token: 0x060009BE RID: 2494
		[DllImport("kernel32", CharSet = CharSet.Unicode, SetLastError = true)]
		[return: MarshalAs(UnmanagedType.Bool)]
		private static extern bool WritePrivateProfileString(string section, string key, string value, string filePath);

		// Token: 0x060009BF RID: 2495 RVA: 0x000402F4 File Offset: 0x0003E4F4
		public static string ReadValue(string section, string key, string filePath, string defaultValue = "")
		{
			StringBuilder stringBuilder = new StringBuilder(IniFileEx.capacity);
			IniFileEx.GetPrivateProfileString(section, key, defaultValue, stringBuilder, stringBuilder.Capacity, filePath);
			return stringBuilder.ToString();
		}

		// Token: 0x060009C0 RID: 2496 RVA: 0x00040324 File Offset: 0x0003E524
		public static string[] ReadSections(string filePath)
		{
			char[] value;
			int privateProfileString;
			for (;;)
			{
				value = new char[IniFileEx.capacity];
				privateProfileString = IniFileEx.GetPrivateProfileString(null, null, "", value, IniFileEx.capacity, filePath);
				if (privateProfileString == 0)
				{
					break;
				}
				if (privateProfileString < IniFileEx.capacity - 2)
				{
					goto IL_3C;
				}
				IniFileEx.capacity *= 2;
			}
			return null;
			IL_3C:
			return new string(value, 0, privateProfileString).Split(new char[1], StringSplitOptions.RemoveEmptyEntries);
		}

		// Token: 0x060009C1 RID: 2497 RVA: 0x00040384 File Offset: 0x0003E584
		public static string[] ReadKeys(string section, string filePath)
		{
			char[] value;
			int privateProfileString;
			for (;;)
			{
				value = new char[IniFileEx.capacity];
				privateProfileString = IniFileEx.GetPrivateProfileString(section, null, "", value, IniFileEx.capacity, filePath);
				if (privateProfileString == 0)
				{
					break;
				}
				if (privateProfileString < IniFileEx.capacity - 2)
				{
					goto IL_3C;
				}
				IniFileEx.capacity *= 2;
			}
			return null;
			IL_3C:
			return new string(value, 0, privateProfileString).Split(new char[1], StringSplitOptions.RemoveEmptyEntries);
		}

		// Token: 0x060009C2 RID: 2498 RVA: 0x000403E4 File Offset: 0x0003E5E4
		public static string[] ReadKeyValuePairs(string section, string filePath)
		{
			IntPtr intPtr;
			int privateProfileSection;
			for (;;)
			{
				intPtr = Marshal.AllocCoTaskMem(IniFileEx.capacity * 2);
				privateProfileSection = IniFileEx.GetPrivateProfileSection(section, intPtr, IniFileEx.capacity, filePath);
				if (privateProfileSection == 0)
				{
					break;
				}
				if (privateProfileSection < IniFileEx.capacity - 2)
				{
					goto IL_44;
				}
				Marshal.FreeCoTaskMem(intPtr);
				IniFileEx.capacity *= 2;
			}
			Marshal.FreeCoTaskMem(intPtr);
			return null;
			IL_44:
			string text = Marshal.PtrToStringAuto(intPtr, privateProfileSection - 1);
			Marshal.FreeCoTaskMem(intPtr);
			return text.Split(new char[1]);
		}

		// Token: 0x060009C3 RID: 2499 RVA: 0x0004044F File Offset: 0x0003E64F
		public static bool WriteValue(string section, string key, string value, string filePath)
		{
			if (!File.Exists(filePath))
			{
				TINHKIEM.CreateFile(filePath, "");
			}
			return IniFileEx.WritePrivateProfileString(section, key, value, filePath);
		}

		// Token: 0x060009C4 RID: 2500 RVA: 0x0004046D File Offset: 0x0003E66D
		public static bool DeleteSection(string section, string filepath)
		{
			return IniFileEx.WritePrivateProfileString(section, null, null, filepath);
		}

		// Token: 0x060009C5 RID: 2501 RVA: 0x00040478 File Offset: 0x0003E678
		public static bool DeleteKey(string section, string key, string filepath)
		{
			return IniFileEx.WritePrivateProfileString(section, key, null, filepath);
		}

		// Token: 0x0400071B RID: 1819
		public static int capacity = 512;
	}
}
