using System;
using System.IO;
using System.Text;

namespace TinhKiemAuto.Models
{
	// Token: 0x02000130 RID: 304
	public static class LoadFile
	{
		// Token: 0x06000FC8 RID: 4040 RVA: 0x000753F0 File Offset: 0x000735F0
		private static byte[] DecryptABFile(string fileName, string password)
		{
			byte[] data = File.ReadAllBytes(fileName);
			string md5Hash = MD5Util.GetMD5Hash(password);
			return XXTEA.Decrypt(data, md5Hash);
		}

		// Token: 0x06000FC9 RID: 4041 RVA: 0x00075410 File Offset: 0x00073610
		private static byte[] EncryptABFile(byte[] data, string password)
		{
			string md5Hash = MD5Util.GetMD5Hash(password);
			return XXTEA.Encrypt(data, md5Hash);
		}

		// Token: 0x06000FCA RID: 4042 RVA: 0x0007542C File Offset: 0x0007362C
		public static void WriteFileWithEncrypt(string data, string filename)
		{
			string password = "afc6f3d4f1f090792a9fd8fd7146f094";
			try
			{
				byte[] bytes = LoadFile.EncryptABFile(Encoding.UTF8.GetBytes(data), password);
				File.WriteAllBytes(filename, bytes);
			}
			catch
			{
			}
		}

		// Token: 0x06000FCB RID: 4043 RVA: 0x00075470 File Offset: 0x00073670
		public static string LoadFileWithDecrypt(string filename)
		{
			string result = "";
			try
			{
				string password = "afc6f3d4f1f090792a9fd8fd7146f094";
				byte[] bytes = LoadFile.DecryptABFile(filename, password);
				result = Encoding.UTF8.GetString(bytes);
			}
			catch
			{
			}
			return result;
		}
	}
}
