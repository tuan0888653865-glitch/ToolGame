using System;

namespace ProtoBuf.Serializers
{
	// Token: 0x0200003F RID: 63
	internal class CRC32
	{
		// Token: 0x06000202 RID: 514 RVA: 0x0000D704 File Offset: 0x0000B904
		private static uint[] makeCrcTable()
		{
			uint[] array = new uint[256];
			for (int i = 0; i < 256; i++)
			{
				uint num = (uint)i;
				int num2 = 8;
				while (--num2 >= 0)
				{
					if ((num & 1U) != 0U)
					{
						num = (3988292384U ^ num >> 1);
					}
					else
					{
						num >>= 1;
					}
				}
				array[i] = num;
			}
			return array;
		}

		// Token: 0x06000203 RID: 515 RVA: 0x0000D754 File Offset: 0x0000B954
		public uint getValue()
		{
			return this.crc & uint.MaxValue;
		}

		// Token: 0x06000204 RID: 516 RVA: 0x0000D75E File Offset: 0x0000B95E
		public void reset()
		{
			this.crc = 0U;
		}

		// Token: 0x06000205 RID: 517 RVA: 0x0000D768 File Offset: 0x0000B968
		public void update(byte[] buf)
		{
			uint num = 0U;
			int num2 = buf.Length;
			uint num3 = ~this.crc;
			while (--num2 >= 0)
			{
				num3 = (CRC32.crcTable[(int)((uint)((UIntPtr)((num3 ^ (uint)buf[(int)((uint)((UIntPtr)(num++)))]) & 255U)))] ^ num3 >> 8);
			}
			this.crc = ~num3;
		}

		// Token: 0x06000206 RID: 518 RVA: 0x0000D7C4 File Offset: 0x0000B9C4
		public void update(byte[] buf, int off, int len)
		{
			uint num = ~this.crc;
			while (--len >= 0)
			{
				num = (CRC32.crcTable[(int)((uint)((UIntPtr)((num ^ (uint)buf[off++]) & 255U)))] ^ num >> 8);
			}
			this.crc = ~num;
		}

		// Token: 0x0400021F RID: 543
		private uint crc;

		// Token: 0x04000220 RID: 544
		private static uint[] crcTable = CRC32.makeCrcTable();
	}
}
