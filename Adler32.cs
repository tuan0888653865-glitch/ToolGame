using System;

namespace ComponentAce.Compression.Libs.zlib
{
	// Token: 0x02000007 RID: 7
	internal sealed class Adler32
	{
		// Token: 0x0600001A RID: 26 RVA: 0x000027B8 File Offset: 0x000009B8
		internal long adler32(long adler, byte[] buf, int index, int len)
		{
			long result;
			if (buf == null)
			{
				result = 1L;
			}
			else
			{
				long num = adler & 65535L;
				long num2 = adler >> 16 & 65535L;
				while (len > 0)
				{
					int i = (len < 5552) ? len : 5552;
					len -= i;
					while (i >= 16)
					{
						num += (long)(buf[index++] & byte.MaxValue);
						num2 += num;
						num += (long)(buf[index++] & byte.MaxValue);
						num2 += num;
						num += (long)(buf[index++] & byte.MaxValue);
						num2 += num;
						num += (long)(buf[index++] & byte.MaxValue);
						num2 += num;
						num += (long)(buf[index++] & byte.MaxValue);
						num2 += num;
						num += (long)(buf[index++] & byte.MaxValue);
						num2 += num;
						num += (long)(buf[index++] & byte.MaxValue);
						num2 += num;
						num += (long)(buf[index++] & byte.MaxValue);
						num2 += num;
						num += (long)(buf[index++] & byte.MaxValue);
						num2 += num;
						num += (long)(buf[index++] & byte.MaxValue);
						num2 += num;
						num += (long)(buf[index++] & byte.MaxValue);
						num2 += num;
						num += (long)(buf[index++] & byte.MaxValue);
						num2 += num;
						num += (long)(buf[index++] & byte.MaxValue);
						num2 += num;
						num += (long)(buf[index++] & byte.MaxValue);
						num2 += num;
						num += (long)(buf[index++] & byte.MaxValue);
						num2 += num;
						num += (long)(buf[index++] & byte.MaxValue);
						num2 += num;
						i -= 16;
					}
					if (i != 0)
					{
						do
						{
							num += (long)(buf[index++] & byte.MaxValue);
							num2 += num;
						}
						while (--i != 0);
					}
					num %= 65521L;
					num2 %= 65521L;
				}
				result = (num2 << 16 | num);
			}
			return result;
		}

		// Token: 0x04000005 RID: 5
		private const int BASE = 65521;

		// Token: 0x04000006 RID: 6
		private const int NMAX = 5552;
	}
}
