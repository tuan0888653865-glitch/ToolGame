using System;
using System.Management;
using System.Security.Cryptography;
using System.Text;

namespace TinhKiemAuto
{
	// Token: 0x02000090 RID: 144
	internal class FingerPrint
	{
		// Token: 0x06000664 RID: 1636 RVA: 0x0002470C File Offset: 0x0002290C
		public static string Value()
		{
			if (string.IsNullOrEmpty(FingerPrint.fingerPrint))
			{
				FingerPrint.fingerPrint = FingerPrint.GetHash(string.Concat(new string[]
				{
					"CPU >> ",
					FingerPrint.CpuId,
					"\nBIOS >> ",
					FingerPrint.biosId(),
					"\nBASE >> ",
					FingerPrint.baseId()
				}));
			}
			return FingerPrint.fingerPrint;
		}

		// Token: 0x17000131 RID: 305
		// (get) Token: 0x06000665 RID: 1637 RVA: 0x00024770 File Offset: 0x00022970
		public static string Serial
		{
			get
			{
				FingerPrint.serial = Setting.Read("User", "HWID");
				if (FingerPrint.serial.Length == 32)
				{
					return FingerPrint.serial;
				}
				if (FingerPrint.serial == "")
				{
					FingerPrint.serial = FingerPrint.GetHash(FingerPrint.Value() + TINHKIEM.GetMACAddress());
					Setting.Write("User", "HWID", FingerPrint.serial);
				}
				return FingerPrint.serial;
			}
		}

		// Token: 0x17000132 RID: 306
		// (get) Token: 0x06000666 RID: 1638 RVA: 0x000247E8 File Offset: 0x000229E8
		public static string TrueSerial
		{
			get
			{
				Setting.Write("User", "HWID", FingerPrint.GetHash(FingerPrint.Value() + TINHKIEM.GetMACAddress()));
				return FingerPrint.GetHash(FingerPrint.Value() + TINHKIEM.GetMACAddress());
			}
		}

		// Token: 0x06000667 RID: 1639 RVA: 0x00024824 File Offset: 0x00022A24
		private static string Md5(string input)
		{
			byte[] array = MD5.Create().ComputeHash(Encoding.Default.GetBytes(input));
			StringBuilder stringBuilder = new StringBuilder();
			for (int i = 0; i < array.Length; i++)
			{
				stringBuilder.Append(array[i].ToString("x2"));
			}
			return stringBuilder.ToString();
		}

		// Token: 0x06000668 RID: 1640 RVA: 0x00024879 File Offset: 0x00022A79
		public static string macId()
		{
			return FingerPrint.identifier("Win32_NetworkAdapterConfiguration", "MACAddress", "IPEnabled");
		}

		// Token: 0x17000133 RID: 307
		// (get) Token: 0x06000669 RID: 1641 RVA: 0x00024890 File Offset: 0x00022A90
		public static string CpuId
		{
			get
			{
				string text = FingerPrint.identifier("Win32_Processor", "UniqueId");
				if (text == "")
				{
					text = FingerPrint.identifier("Win32_Processor", "ProcessorId");
					if (text == "")
					{
						text = FingerPrint.identifier("Win32_Processor", "Name");
						if (text == "")
						{
							text = FingerPrint.identifier("Win32_Processor", "Manufacturer");
						}
						text += FingerPrint.identifier("Win32_Processor", "MaxClockSpeed");
					}
				}
				return text;
			}
		}

		// Token: 0x0600066A RID: 1642 RVA: 0x0002491C File Offset: 0x00022B1C
		private static string identifier(string wmiClass, string wmiProperty, string wmiMustBeTrue)
		{
			string text = "";
			foreach (ManagementBaseObject managementBaseObject in new ManagementClass(wmiClass).GetInstances())
			{
				ManagementObject managementObject = (ManagementObject)managementBaseObject;
				if (managementObject[wmiMustBeTrue].ToString() == "True" && text == "")
				{
					try
					{
						text = managementObject[wmiProperty].ToString();
						break;
					}
					catch
					{
					}
				}
			}
			return text;
		}

		// Token: 0x0600066B RID: 1643 RVA: 0x000249B8 File Offset: 0x00022BB8
		private static string identifier(string wmiClass, string wmiProperty)
		{
			string text = "";
			foreach (ManagementBaseObject managementBaseObject in new ManagementClass(wmiClass).GetInstances())
			{
				ManagementObject managementObject = (ManagementObject)managementBaseObject;
				if (text == "")
				{
					try
					{
						text = managementObject[wmiProperty].ToString();
						break;
					}
					catch
					{
					}
				}
			}
			return text;
		}

		// Token: 0x0600066C RID: 1644 RVA: 0x00024A3C File Offset: 0x00022C3C
		private static string biosId()
		{
			return string.Concat(new string[]
			{
				FingerPrint.identifier("Win32_BIOS", "Manufacturer"),
				FingerPrint.identifier("Win32_BIOS", "SMBIOSBIOSVersion"),
				FingerPrint.identifier("Win32_BIOS", "IdentificationCode"),
				FingerPrint.identifier("Win32_BIOS", "SerialNumber"),
				FingerPrint.identifier("Win32_BIOS", "ReleaseDate"),
				FingerPrint.identifier("Win32_BIOS", "Version")
			});
		}

		// Token: 0x0600066D RID: 1645 RVA: 0x00024AC0 File Offset: 0x00022CC0
		public static string diskId()
		{
			return FingerPrint.identifier("Win32_DiskDrive", "Model") + FingerPrint.identifier("Win32_DiskDrive", "Manufacturer") + FingerPrint.identifier("Win32_DiskDrive", "Signature") + FingerPrint.identifier("Win32_DiskDrive", "TotalHeads");
		}

		// Token: 0x0600066E RID: 1646 RVA: 0x00024B10 File Offset: 0x00022D10
		private static string baseId()
		{
			return FingerPrint.identifier("Win32_BaseBoard", "Model") + FingerPrint.identifier("Win32_BaseBoard", "Manufacturer") + FingerPrint.identifier("Win32_BaseBoard", "Name") + FingerPrint.identifier("Win32_BaseBoard", "SerialNumber");
		}

		// Token: 0x0600066F RID: 1647 RVA: 0x00024B5E File Offset: 0x00022D5E
		private static string videoId()
		{
			return FingerPrint.identifier("Win32_VideoController", "DriverVersion") + FingerPrint.identifier("Win32_VideoController", "Name");
		}

		// Token: 0x06000670 RID: 1648 RVA: 0x00024B84 File Offset: 0x00022D84
		private static string GetHash(string s)
		{
			HashAlgorithm hashAlgorithm = new MD5CryptoServiceProvider();
			byte[] bytes = new ASCIIEncoding().GetBytes(s);
			return FingerPrint.GetHexString(hashAlgorithm.ComputeHash(bytes));
		}

		// Token: 0x06000671 RID: 1649 RVA: 0x00024BB0 File Offset: 0x00022DB0
		public static string GetHexString(byte[] bt)
		{
			string text = string.Empty;
			foreach (byte b in bt)
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

		// Token: 0x04000424 RID: 1060
		private static string fingerPrint = string.Empty;

		// Token: 0x04000425 RID: 1061
		private static string serial = "";
	}
}
