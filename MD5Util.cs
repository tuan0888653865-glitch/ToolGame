using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace TinhKiemAuto.Models
{
	// Token: 0x02000131 RID: 305
	public class MD5Util
	{
		// Token: 0x06000FCC RID: 4044 RVA: 0x000754B4 File Offset: 0x000736B4
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

		// Token: 0x06000FCD RID: 4045 RVA: 0x000754FC File Offset: 0x000736FC
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

		// Token: 0x06000FCE RID: 4046 RVA: 0x00075548 File Offset: 0x00073748
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

		// Token: 0x06000FCF RID: 4047 RVA: 0x000755A0 File Offset: 0x000737A0
		public static bool VerfyMd5Hash(string input, string hash)
		{
			string md5Hash = MD5Util.GetMD5Hash(input);
			StringComparer ordinalIgnoreCase = StringComparer.OrdinalIgnoreCase;
			return ordinalIgnoreCase.Compare(md5Hash, hash) == 0;
		}
	}
}
