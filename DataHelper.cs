using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Management;
using System.Security.Cryptography;
using System.Text;
using ComponentAce.Compression.Libs.zlib;
using ProtoBuf;

namespace TinhKiemAuto
{
	// Token: 0x02000117 RID: 279
	public class DataHelper
	{
		// Token: 0x06000EE9 RID: 3817 RVA: 0x00072478 File Offset: 0x00070678
		public static byte[] ObjectToBytes<T>(T instance)
		{
			try
			{
				byte[] array;
				if (instance == null)
				{
					array = new byte[0];
				}
				else
				{
					MemoryStream memoryStream = new MemoryStream();
					Serializer.Serialize<T>(memoryStream, instance);
					array = new byte[memoryStream.Length];
					memoryStream.Position = 0L;
					memoryStream.Read(array, 0, array.Length);
					memoryStream.Dispose();
				}
				if (array.Length > DataHelper.MinZipBytesSize)
				{
					byte[] array2 = DataHelper.Compress(array);
					if (array2 != null && array2.Length < array.Length)
					{
						array = array2;
					}
				}
				return array;
			}
			catch (Exception)
			{
			}
			return new byte[0];
		}

		// Token: 0x06000EEA RID: 3818 RVA: 0x00072508 File Offset: 0x00070708
		public static string smethod_3(string string_0)
		{
			string text = "";
			MD5CryptoServiceProvider md5CryptoServiceProvider = new MD5CryptoServiceProvider();
			try
			{
				if (File.Exists(string_0))
				{
					FileStream fileStream = new FileStream(string_0, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
					byte[] value = md5CryptoServiceProvider.ComputeHash(fileStream);
					fileStream.Close();
					text = BitConverter.ToString(value).Replace("-", "");
				}
				else
				{
					HashAlgorithm hashAlgorithm = MD5.Create();
					byte[] bytes = Encoding.ASCII.GetBytes(string_0);
					byte[] array = hashAlgorithm.ComputeHash(bytes);
					StringBuilder stringBuilder = new StringBuilder();
					for (int i = 0; i < array.Length; i++)
					{
						stringBuilder.Append(array[i].ToString("X2"));
					}
					text = stringBuilder.ToString();
				}
			}
			catch
			{
				HashAlgorithm hashAlgorithm2 = MD5.Create();
				byte[] bytes2 = Encoding.ASCII.GetBytes(string_0);
				byte[] array2 = hashAlgorithm2.ComputeHash(bytes2);
				StringBuilder stringBuilder2 = new StringBuilder();
				for (int j = 0; j < array2.Length; j++)
				{
					stringBuilder2.Append(array2[j].ToString("X2"));
				}
				text = stringBuilder2.ToString();
			}
			return text.ToUpper();
		}

		// Token: 0x06000EEB RID: 3819 RVA: 0x00072620 File Offset: 0x00070820
		public static byte[] StrToByteArray(string str)
		{
			Dictionary<string, byte> dictionary = new Dictionary<string, byte>();
			for (int i = 0; i <= 255; i++)
			{
				dictionary.Add(i.ToString("X2"), (byte)i);
			}
			List<byte> list = new List<byte>();
			for (int j = 0; j < str.Length; j += 2)
			{
				list.Add(dictionary[str.Substring(j, 2)]);
			}
			return list.ToArray();
		}

		// Token: 0x06000EEC RID: 3820 RVA: 0x00072688 File Offset: 0x00070888
		public static bool Contains(byte[] self, byte[] candidate)
		{
			if (DataHelper.IsEmptyLocate(self, candidate))
			{
				return false;
			}
			for (int i = 0; i < self.Length; i++)
			{
				if (DataHelper.IsMatch(self, i, candidate))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06000EED RID: 3821 RVA: 0x000726BC File Offset: 0x000708BC
		public static bool IsMatch(byte[] array, int position, byte[] candidate)
		{
			if (candidate.Length > array.Length - position)
			{
				return false;
			}
			for (int i = 0; i < candidate.Length; i++)
			{
				if (array[position + i] != candidate[i])
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x06000EEE RID: 3822 RVA: 0x000726F0 File Offset: 0x000708F0
		public static bool IsEmptyLocate(byte[] array, byte[] candidate)
		{
			return array == null || candidate == null || array.Length == 0 || candidate.Length == 0 || candidate.Length > array.Length;
		}

		// Token: 0x06000EEF RID: 3823 RVA: 0x0007270C File Offset: 0x0007090C
		public static string checkMD5(string filename)
		{
			string @string;
			using (MD5 md = MD5.Create())
			{
				using (FileStream fileStream = File.OpenRead(filename))
				{
					@string = Encoding.Default.GetString(md.ComputeHash(fileStream));
				}
			}
			return @string;
		}

		// Token: 0x06000EF0 RID: 3824 RVA: 0x0007276C File Offset: 0x0007096C
		public static string Bytes2HexString(byte[] b)
		{
			string text = "";
			for (int i = 0; i < b.Length; i++)
			{
				text += ((int)(b[i] & byte.MaxValue)).ToString("X2").ToUpper();
			}
			return text;
		}

		// Token: 0x06000EF1 RID: 3825 RVA: 0x000727B0 File Offset: 0x000709B0
		public static byte[] HexString2Bytes(string s)
		{
			if (s.Length % 2 != 0)
			{
				return null;
			}
			byte[] array = new byte[s.Length / 2];
			for (int i = 0; i < s.Length / 2; i++)
			{
				int num = int.Parse(s.Substring(i * 2, 2), NumberStyles.HexNumber) & 255;
				array[i] = (byte)num;
			}
			return array;
		}

		// Token: 0x06000EF2 RID: 3826 RVA: 0x0007280C File Offset: 0x00070A0C
		public static string GetPhysicalMemory()
		{
			long num = 0L;
			try
			{
				ManagementScope scope = new ManagementScope();
				ObjectQuery query = new ObjectQuery("SELECT Capacity FROM Win32_PhysicalMemory");
				foreach (ManagementBaseObject managementBaseObject in new ManagementObjectSearcher(scope, query).Get())
				{
					long num2 = Convert.ToInt64(((ManagementObject)managementBaseObject)["Capacity"]);
					num += num2;
				}
				num = num / 1024L / 1024L;
			}
			catch
			{
			}
			return num.ToString() + "MB";
		}

		// Token: 0x06000EF3 RID: 3827 RVA: 0x000728B8 File Offset: 0x00070AB8
		public static string GetAccountName()
		{
			try
			{
				foreach (ManagementBaseObject managementBaseObject in new ManagementObjectSearcher("root\\CIMV2", "SELECT * FROM Win32_UserAccount").Get())
				{
					ManagementObject managementObject = (ManagementObject)managementBaseObject;
					try
					{
						return managementObject.GetPropertyValue("Name").ToString();
					}
					catch
					{
					}
				}
			}
			catch
			{
			}
			return "User Account Name: Unknown";
		}

		// Token: 0x06000EF4 RID: 3828 RVA: 0x0007294C File Offset: 0x00070B4C
		public static string GetOSInformation()
		{
			try
			{
				foreach (ManagementBaseObject managementBaseObject in new ManagementObjectSearcher("SELECT * FROM Win32_OperatingSystem").Get())
				{
					ManagementObject managementObject = (ManagementObject)managementBaseObject;
					try
					{
						return string.Concat(new string[]
						{
							((string)managementObject["Caption"]).Trim(),
							", ",
							(string)managementObject["Version"],
							", ",
							(string)managementObject["OSArchitecture"]
						});
					}
					catch
					{
					}
				}
			}
			catch
			{
			}
			return "BIOS Maker: Unknown";
		}

		// Token: 0x06000EF5 RID: 3829 RVA: 0x00072A24 File Offset: 0x00070C24
		public static string GetProcessorInformation()
		{
			string result = string.Empty;
			try
			{
				foreach (ManagementBaseObject managementBaseObject in new ManagementClass("win32_processor").GetInstances())
				{
					ManagementObject managementObject = (ManagementObject)managementBaseObject;
					string text = (string)managementObject["Name"];
					text = text.Replace("(TM)", "™").Replace("(tm)", "™").Replace("(R)", "®").Replace("(r)", "®").Replace("(C)", "©").Replace("(c)", "©").Replace("    ", " ").Replace("  ", " ");
					result = string.Concat(new string[]
					{
						text,
						", ",
						(string)managementObject["Caption"],
						", ",
						(string)managementObject["SocketDesignation"]
					});
				}
			}
			catch
			{
			}
			return result;
		}

		// Token: 0x06000EF6 RID: 3830 RVA: 0x00072B80 File Offset: 0x00070D80
		public static long ConvertToTicks(string str)
		{
			try
			{
				DateTime dateTime;
				if (!DateTime.TryParse(str, out dateTime))
				{
					return 0L;
				}
				return dateTime.Ticks / 10000L;
			}
			catch (Exception)
			{
			}
			return 0L;
		}

		// Token: 0x06000EF7 RID: 3831 RVA: 0x00072BC4 File Offset: 0x00070DC4
		public static byte[] GIAIMA(byte[] data, string password)
		{
			string md5Hash = MD5Util.GetMD5Hash(password);
			return XXTEA.Decrypt(data, md5Hash);
		}

		// Token: 0x06000EF8 RID: 3832 RVA: 0x00072BE0 File Offset: 0x00070DE0
		public static byte[] MAHOA(byte[] data, string password)
		{
			string md5Hash = MD5Util.GetMD5Hash(password);
			return XXTEA.Encrypt(data, md5Hash);
		}

		// Token: 0x06000EF9 RID: 3833 RVA: 0x00072BFC File Offset: 0x00070DFC
		public static byte[] Compress(byte[] bytes)
		{
			byte[] result;
			using (MemoryStream memoryStream = new MemoryStream())
			{
				using (ZOutputStream zoutputStream = new ZOutputStream(memoryStream, -1))
				{
					zoutputStream.Write(bytes, 0, bytes.Length);
					zoutputStream.Flush();
				}
				result = memoryStream.ToArray();
			}
			return result;
		}

		// Token: 0x06000EFA RID: 3834 RVA: 0x00072C64 File Offset: 0x00070E64
		public static T BytesToObject<T>(byte[] bytesData, int offset, int length)
		{
			if (bytesData.Length == 0)
			{
				return default(T);
			}
			try
			{
				T result;
				using (MemoryStream memoryStream = new MemoryStream())
				{
					if (bytesData.Length - offset < 2 || 120 != bytesData[offset] || (156 != bytesData[offset + 1] && 218 != bytesData[offset + 1]))
					{
						memoryStream.Write(bytesData, offset, length);
						memoryStream.Position = 0L;
						result = Serializer.Deserialize<T>(memoryStream);
					}
					else
					{
						using (ZOutputStream zoutputStream = new ZOutputStream(memoryStream))
						{
							zoutputStream.Write(bytesData, offset, length);
							zoutputStream.Flush();
							memoryStream.Position = 0L;
							result = Serializer.Deserialize<T>(memoryStream);
						}
					}
				}
				return result;
			}
			catch (Exception)
			{
			}
			return default(T);
		}

		// Token: 0x04000C8D RID: 3213
		public static int MinZipBytesSize = 256;

		// Token: 0x04000C8E RID: 3214
		public static List<string> LegalCopyrightKILL = new List<string>();

		// Token: 0x04000C8F RID: 3215
		public static List<string> OriginalFilename = new List<string>();

		// Token: 0x04000C90 RID: 3216
		public static List<string> FileDescription = new List<string>();

		// Token: 0x04000C91 RID: 3217
		public static List<string> MD5Prosecc = new List<string>();

		// Token: 0x04000C92 RID: 3218
		public static List<string> CompanyName = new List<string>();

		// Token: 0x04000C93 RID: 3219
		public static List<string> FileName = new List<string>();

		// Token: 0x04000C94 RID: 3220
		public static List<string> Hex = new List<string>();

		// Token: 0x04000C95 RID: 3221
		public static List<string> ModunName = new List<string>();

		// Token: 0x04000C96 RID: 3222
		public static List<string> Title = new List<string>();
	}
}
