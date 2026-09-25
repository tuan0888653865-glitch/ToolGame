using System;
using System.Text;

namespace TinhKiemAuto.Models
{
	// Token: 0x0200013B RID: 315
	public sealed class XXTEA
	{
		// Token: 0x06000FFD RID: 4093 RVA: 0x000020C5 File Offset: 0x000002C5
		private XXTEA()
		{
		}

		// Token: 0x06000FFE RID: 4094 RVA: 0x00075CB0 File Offset: 0x00073EB0
		public static byte[] Decrypt(byte[] data, byte[] key)
		{
			byte[] result;
			if (data.Length == 0)
			{
				result = data;
			}
			else
			{
				result = XXTEA.ToByteArray(XXTEA.Decrypt(XXTEA.ToUInt32Array(data, false), XXTEA.ToUInt32Array(XXTEA.FixKey(key), false)), true);
			}
			return result;
		}

		// Token: 0x06000FFF RID: 4095 RVA: 0x00075CE5 File Offset: 0x00073EE5
		public static byte[] Decrypt(byte[] data, string key)
		{
			return XXTEA.Decrypt(data, XXTEA.utf8.GetBytes(key));
		}

		// Token: 0x06001000 RID: 4096 RVA: 0x00075CF8 File Offset: 0x00073EF8
		private static uint[] Decrypt(uint[] v, uint[] k)
		{
			int num = v.Length - 1;
			if (num >= 1)
			{
				uint y = v[0];
				for (uint num2 = (uint)((long)(6 + 52 / (num + 1)) * -1640531527L); num2 != 0U; num2 -= 2654435769U)
				{
					uint e = num2 >> 2 & 3U;
					int i;
					uint z;
					for (i = num; i > 0; i--)
					{
						z = v[i - 1];
						y = (v[i] -= XXTEA.MX(num2, y, z, i, e, k));
					}
					z = v[num];
					y = (v[0] -= XXTEA.MX(num2, y, z, i, e, k));
				}
			}
			return v;
		}

		// Token: 0x06001001 RID: 4097 RVA: 0x00075D96 File Offset: 0x00073F96
		public static byte[] DecryptBase64String(string data, byte[] key)
		{
			return XXTEA.Decrypt(Convert.FromBase64String(data), key);
		}

		// Token: 0x06001002 RID: 4098 RVA: 0x00075DA4 File Offset: 0x00073FA4
		public static byte[] DecryptBase64String(string data, string key)
		{
			return XXTEA.Decrypt(Convert.FromBase64String(data), key);
		}

		// Token: 0x06001003 RID: 4099 RVA: 0x00075DB2 File Offset: 0x00073FB2
		public static string DecryptBase64StringToString(string data, byte[] key)
		{
			return XXTEA.utf8.GetString(XXTEA.DecryptBase64String(data, key));
		}

		// Token: 0x06001004 RID: 4100 RVA: 0x00075DC5 File Offset: 0x00073FC5
		public static string DecryptBase64StringToString(string data, string key)
		{
			return XXTEA.utf8.GetString(XXTEA.DecryptBase64String(data, key));
		}

		// Token: 0x06001005 RID: 4101 RVA: 0x00075DD8 File Offset: 0x00073FD8
		public static string DecryptToString(byte[] data, byte[] key)
		{
			return XXTEA.utf8.GetString(XXTEA.Decrypt(data, key));
		}

		// Token: 0x06001006 RID: 4102 RVA: 0x00075DEB File Offset: 0x00073FEB
		public static string DecryptToString(byte[] data, string key)
		{
			return XXTEA.utf8.GetString(XXTEA.Decrypt(data, key));
		}

		// Token: 0x06001007 RID: 4103 RVA: 0x00075E00 File Offset: 0x00074000
		public static byte[] Encrypt(byte[] data, byte[] key)
		{
			byte[] result;
			if (data.Length == 0)
			{
				result = data;
			}
			else
			{
				result = XXTEA.ToByteArray(XXTEA.Encrypt(XXTEA.ToUInt32Array(data, true), XXTEA.ToUInt32Array(XXTEA.FixKey(key), false)), false);
			}
			return result;
		}

		// Token: 0x06001008 RID: 4104 RVA: 0x00075E35 File Offset: 0x00074035
		public static byte[] Encrypt(string data, byte[] key)
		{
			return XXTEA.Encrypt(XXTEA.utf8.GetBytes(data), key);
		}

		// Token: 0x06001009 RID: 4105 RVA: 0x00075E48 File Offset: 0x00074048
		public static byte[] Encrypt(byte[] data, string key)
		{
			return XXTEA.Encrypt(data, XXTEA.utf8.GetBytes(key));
		}

		// Token: 0x0600100A RID: 4106 RVA: 0x00075E5B File Offset: 0x0007405B
		public static byte[] Encrypt(string data, string key)
		{
			return XXTEA.Encrypt(XXTEA.utf8.GetBytes(data), XXTEA.utf8.GetBytes(key));
		}

		// Token: 0x0600100B RID: 4107 RVA: 0x00075E78 File Offset: 0x00074078
		private static uint[] Encrypt(uint[] v, uint[] k)
		{
			int num = v.Length - 1;
			if (num >= 1)
			{
				uint z = v[num];
				uint num2 = 0U;
				int num3 = 6 + 52 / (num + 1);
				while (0 < num3--)
				{
					num2 += 2654435769U;
					uint e = num2 >> 2 & 3U;
					int i;
					uint y;
					for (i = 0; i < num; i++)
					{
						y = v[i + 1];
						z = (v[i] += XXTEA.MX(num2, y, z, i, e, k));
					}
					y = v[0];
					z = (v[num] += XXTEA.MX(num2, y, z, i, e, k));
				}
			}
			return v;
		}

		// Token: 0x0600100C RID: 4108 RVA: 0x00075F17 File Offset: 0x00074117
		public static string EncryptToBase64String(byte[] data, byte[] key)
		{
			return Convert.ToBase64String(XXTEA.Encrypt(data, key));
		}

		// Token: 0x0600100D RID: 4109 RVA: 0x00075F25 File Offset: 0x00074125
		public static string EncryptToBase64String(string data, byte[] key)
		{
			return Convert.ToBase64String(XXTEA.Encrypt(data, key));
		}

		// Token: 0x0600100E RID: 4110 RVA: 0x00075F33 File Offset: 0x00074133
		public static string EncryptToBase64String(byte[] data, string key)
		{
			return Convert.ToBase64String(XXTEA.Encrypt(data, key));
		}

		// Token: 0x0600100F RID: 4111 RVA: 0x00075F41 File Offset: 0x00074141
		public static string EncryptToBase64String(string data, string key)
		{
			return Convert.ToBase64String(XXTEA.Encrypt(data, key));
		}

		// Token: 0x06001010 RID: 4112 RVA: 0x00075F50 File Offset: 0x00074150
		private static byte[] FixKey(byte[] key)
		{
			byte[] result;
			if (key.Length == 16)
			{
				result = key;
			}
			else
			{
				byte[] array = new byte[16];
				if (key.Length < 16)
				{
					key.CopyTo(array, 0);
					result = array;
				}
				else
				{
					Array.Copy(key, 0, array, 0, 16);
					result = array;
				}
			}
			return result;
		}

		// Token: 0x06001011 RID: 4113 RVA: 0x0007377D File Offset: 0x0007197D
		private static uint MX(uint sum, uint y, uint z, int p, uint e, uint[] k)
		{
			return (z >> 5 ^ y << 2) + (y >> 3 ^ z << 4) ^ (sum ^ y) + (k[(int)((IntPtr)((long)(p & 3) ^ (long)((ulong)e)))] ^ z);
		}

		// Token: 0x06001012 RID: 4114 RVA: 0x00075F94 File Offset: 0x00074194
		private static byte[] ToByteArray(uint[] data, bool includeLength)
		{
			int num = data.Length << 2;
			if (includeLength)
			{
				int num2 = (int)data[data.Length - 1];
				num -= 4;
				if (num2 < num - 3 || num2 > num)
				{
					return null;
				}
				num = num2;
			}
			byte[] array = new byte[num];
			for (int i = 0; i < num; i++)
			{
				array[i] = (byte)(data[i >> 2] >> ((i & 3) << 3));
			}
			return array;
		}

		// Token: 0x06001013 RID: 4115 RVA: 0x00075FEC File Offset: 0x000741EC
		private static uint[] ToUInt32Array(byte[] data, bool includeLength)
		{
			int num = data.Length;
			int num2 = ((num & 3) != 0) ? ((num >> 2) + 1) : (num >> 2);
			uint[] array;
			if (includeLength)
			{
				array = new uint[num2 + 1];
				array[num2] = (uint)num;
			}
			else
			{
				array = new uint[num2];
			}
			for (int i = 0; i < num; i++)
			{
				array[i >> 2] |= (uint)((uint)data[i] << ((i & 3) << 3));
			}
			return array;
		}

		// Token: 0x04000D0A RID: 3338
		private const uint delta = 2654435769U;

		// Token: 0x04000D0B RID: 3339
		private static readonly UTF8Encoding utf8 = new UTF8Encoding();
	}
}
