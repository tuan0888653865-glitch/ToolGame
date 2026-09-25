using System;
using System.Management;
using System.Net.NetworkInformation;
using System.Security.Cryptography;
using System.Text;

namespace TinhKiemAuto
{
	// Token: 0x02000116 RID: 278
	internal class Class95
	{
		// Token: 0x06000ED9 RID: 3801 RVA: 0x00071EF4 File Offset: 0x000700F4
		public static string smethod_0()
		{
			if (string.IsNullOrEmpty(Class95.string_0))
			{
				Class95.string_0 = Class95.smethod_9(string.Concat(new string[]
				{
					"CPU >> ",
					Class95.String_2,
					"\nBIOS >> ",
					Class95.smethod_5(),
					"\nBASE >> ",
					Class95.smethod_7()
				}));
			}
			return Class95.string_0;
		}

		// Token: 0x06000EDA RID: 3802 RVA: 0x00071F58 File Offset: 0x00070158
		public static string GetSING()
		{
			return Class95.smethod_9(DateTime.Now.ToString());
		}

		// Token: 0x1700033C RID: 828
		// (get) Token: 0x06000EDB RID: 3803 RVA: 0x00071F78 File Offset: 0x00070178
		public static string String_0
		{
			get
			{
				string result = "";
				try
				{
					if (Class95.string_1.Length == 32)
					{
						result = Class95.string_1;
					}
					if (Class95.smethod_14(Class95.string_1, ""))
					{
						Class95.string_1 = Class95.smethod_9(Class95.smethod_0() + Class95.smethod_3());
					}
					result = Class95.string_1;
				}
				catch
				{
					result = Class95.smethod_9(Class95.GetHardDiskSerialNo());
				}
				return result;
			}
		}

		// Token: 0x06000EDC RID: 3804 RVA: 0x00071FF0 File Offset: 0x000701F0
		public static string GetHardDiskSerialNo()
		{
			string result = "";
			try
			{
				using (ManagementObjectCollection.ManagementObjectEnumerator enumerator = new ManagementClass("Win32_DiskDrive").GetInstances().GetEnumerator())
				{
					if (enumerator.MoveNext())
					{
						result = Convert.ToString(((ManagementObject)enumerator.Current)["SerialNumber"]);
					}
				}
			}
			catch
			{
				result = Class95.smethod_3();
			}
			return result;
		}

		// Token: 0x06000EDD RID: 3805 RVA: 0x00072074 File Offset: 0x00070274
		public static string smethod_3()
		{
			NetworkInterface[] allNetworkInterfaces = NetworkInterface.GetAllNetworkInterfaces();
			string text = string.Empty;
			foreach (NetworkInterface networkInterface in allNetworkInterfaces)
			{
				if (text == string.Empty)
				{
					text = networkInterface.GetPhysicalAddress().ToString();
				}
			}
			return text;
		}

		// Token: 0x1700033D RID: 829
		// (get) Token: 0x06000EDE RID: 3806 RVA: 0x000720B9 File Offset: 0x000702B9
		public static string String_1
		{
			get
			{
				return Class95.smethod_9(Class95.smethod_0() + Class95.smethod_3());
			}
		}

		// Token: 0x1700033E RID: 830
		// (get) Token: 0x06000EDF RID: 3807 RVA: 0x000720D0 File Offset: 0x000702D0
		public static string String_2
		{
			get
			{
				string text = Class95.smethod_4("Win32_Processor", "UniqueId");
				try
				{
					if (Class95.smethod_14(text, ""))
					{
						text = Class95.smethod_4("Win32_Processor", "ProcessorId");
						if (Class95.smethod_14(text, ""))
						{
							text = Class95.smethod_4("Win32_Processor", "Name");
							if (Class95.smethod_14(text, ""))
							{
								text = Class95.smethod_4("Win32_Processor", "Manufacturer");
							}
							text += Class95.smethod_4("Win32_Processor", "MaxClockSpeed");
						}
					}
				}
				catch
				{
				}
				return text;
			}
		}

		// Token: 0x06000EE0 RID: 3808 RVA: 0x00072170 File Offset: 0x00070370
		private static string smethod_3(string string_2, string string_3, string string_4)
		{
			string text = "";
			foreach (ManagementBaseObject managementBaseObject in new ManagementClass(string_2).GetInstances())
			{
				ManagementObject managementObject = (ManagementObject)managementBaseObject;
				if (Class95.smethod_14(managementObject[string_4].ToString(), "True") && Class95.smethod_14(text, ""))
				{
					try
					{
						text = managementObject[string_3].ToString();
						break;
					}
					catch
					{
					}
				}
			}
			return text;
		}

		// Token: 0x06000EE1 RID: 3809 RVA: 0x0007220C File Offset: 0x0007040C
		private static string smethod_4(string string_2, string string_3)
		{
			string text = "";
			foreach (ManagementBaseObject managementBaseObject in new ManagementClass(string_2).GetInstances())
			{
				ManagementObject managementObject = (ManagementObject)managementBaseObject;
				if (Class95.smethod_14(text, ""))
				{
					try
					{
						text = managementObject[string_3].ToString();
						break;
					}
					catch
					{
					}
				}
			}
			return text;
		}

		// Token: 0x06000EE2 RID: 3810 RVA: 0x00072290 File Offset: 0x00070490
		private static string smethod_5()
		{
			string result;
			try
			{
				result = string.Concat(new string[]
				{
					Class95.smethod_4("Win32_BIOS", "Manufacturer"),
					Class95.smethod_4("Win32_BIOS", "SMBIOSBIOSVersion"),
					Class95.smethod_4("Win32_BIOS", "IdentificationCode"),
					Class95.smethod_4("Win32_BIOS", "SerialNumber"),
					Class95.smethod_4("Win32_BIOS", "ReleaseDate"),
					Class95.smethod_4("Win32_BIOS", "Version")
				});
			}
			catch
			{
				result = "";
			}
			return result;
		}

		// Token: 0x06000EE3 RID: 3811 RVA: 0x00072334 File Offset: 0x00070534
		private static string smethod_7()
		{
			string result;
			try
			{
				result = Class95.smethod_4("Win32_BaseBoard", "Model") + Class95.smethod_4("Win32_BaseBoard", "Manufacturer") + Class95.smethod_4("Win32_BaseBoard", "Name") + Class95.smethod_4("Win32_BaseBoard", "SerialNumber");
			}
			catch
			{
				result = "";
			}
			return result;
		}

		// Token: 0x06000EE4 RID: 3812 RVA: 0x000723A0 File Offset: 0x000705A0
		private static string smethod_9(string string_2)
		{
			HashAlgorithm hashAlgorithm = new MD5CryptoServiceProvider();
			byte[] bytes = new ASCIIEncoding().GetBytes(string_2);
			return Class95.smethod_10(hashAlgorithm.ComputeHash(bytes));
		}

		// Token: 0x06000EE5 RID: 3813 RVA: 0x000723CC File Offset: 0x000705CC
		public static string smethod_10(byte[] byte_0)
		{
			string text = string.Empty;
			foreach (byte b in byte_0)
			{
				int num = (int)(b & 15);
				int num2 = b >> 4 & 15;
				if (num2 > 9)
				{
					text += ((char)(num2 - 10 + 65)).ToString();
				}
				else
				{
					text += num2.ToString();
				}
				if (num > 9)
				{
					text += ((char)(num - 10 + 65)).ToString();
				}
				else
				{
					text += num.ToString();
				}
			}
			return text;
		}

		// Token: 0x06000EE6 RID: 3814 RVA: 0x00072456 File Offset: 0x00070656
		private static bool smethod_14(string string_2, string string_3)
		{
			return string_2 == string_3;
		}

		// Token: 0x04000C8B RID: 3211
		private static string string_0 = string.Empty;

		// Token: 0x04000C8C RID: 3212
		private static string string_1 = "";
	}
}
