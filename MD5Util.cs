using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace TinhKiemAuto
{
	// Token: 0x02000118 RID: 280
	public class MD5Util
	{
		// Token: 0x06000EFD RID: 3837 RVA: 0x00072DB4 File Offset: 0x00070FB4
		public static string GetFileMD5(string fileName)
		{
			string result = string.Empty;
			if (File.Exists(fileName))
			{
				using (FileStream fileStream = new FileStream(fileName, FileMode.Open, FileAccess.Read))
				{
					result = MD5Util.GetMD5Hash(fileStream);
				}
			}
			return result;
		}

		// Token: 0x06000EFE RID: 3838 RVA: 0x00072DFC File Offset: 0x00070FFC
		public static string GetMD5Hash(Stream stream)
		{
			byte[] array = MD5.Create().ComputeHash(stream);
			StringBuilder stringBuilder = new StringBuilder();
			for (int i = 0; i < array.Length; i++)
			{
				stringBuilder.Append(array[i].ToString("x2"));
			}
			return stringBuilder.ToString();
		}

		// Token: 0x06000EFF RID: 3839 RVA: 0x00072E48 File Offset: 0x00071048
		public static string GetMD5Hash(string input)
		{
			byte[] array = MD5.Create().ComputeHash(Encoding.UTF8.GetBytes(input));
			StringBuilder stringBuilder = new StringBuilder();
			for (int i = 0; i < array.Length; i++)
			{
				stringBuilder.Append(array[i].ToString("x2"));
			}
			return stringBuilder.ToString();
		}

		// Token: 0x06000F00 RID: 3840 RVA: 0x00072EA0 File Offset: 0x000710A0
		public static bool VerfyMd5Hash(string input, string hash)
		{
			string md5Hash = MD5Util.GetMD5Hash(input);
			StringComparer ordinalIgnoreCase = StringComparer.OrdinalIgnoreCase;
			return ordinalIgnoreCase.Compare(md5Hash, hash) == 0;
		}
	}
}
