using System;

namespace ComponentAce.Compression.Libs.zlib
{
	// Token: 0x0200000B RID: 11
	internal sealed class Inflate
	{
		// Token: 0x06000052 RID: 82 RVA: 0x00006D9C File Offset: 0x00004F9C
		internal int inflateReset(ZStream z)
		{
			int result;
			if (z == null || z.istate == null)
			{
				result = -2;
			}
			else
			{
				z.total_in = (z.total_out = 0L);
				z.msg = null;
				z.istate.mode = ((z.istate.nowrap != 0) ? 7 : 0);
				z.istate.blocks.reset(z, null);
				result = 0;
			}
			return result;
		}

		// Token: 0x06000053 RID: 83 RVA: 0x00006E02 File Offset: 0x00005002
		internal int inflateEnd(ZStream z)
		{
			if (this.blocks != null)
			{
				this.blocks.free(z);
			}
			this.blocks = null;
			return 0;
		}

		// Token: 0x06000054 RID: 84 RVA: 0x00006E20 File Offset: 0x00005020
		internal int inflateInit(ZStream z, int w)
		{
			z.msg = null;
			this.blocks = null;
			this.nowrap = 0;
			if (w < 0)
			{
				w = -w;
				this.nowrap = 1;
			}
			int result;
			if (w < 8 || w > 15)
			{
				this.inflateEnd(z);
				result = -2;
			}
			else
			{
				this.wbits = w;
				z.istate.blocks = new InfBlocks(z, (z.istate.nowrap != 0) ? null : this, 1 << w);
				this.inflateReset(z);
				result = 0;
			}
			return result;
		}

		// Token: 0x06000055 RID: 85 RVA: 0x00006EA4 File Offset: 0x000050A4
		internal int inflate(ZStream z, int f)
		{
			int result;
			if (z == null || z.istate == null || z.next_in == null)
			{
				result = -2;
			}
			else
			{
				f = ((f == 4) ? -5 : 0);
				int num = -5;
				int next_in_index2;
				for (;;)
				{
					switch (z.istate.mode)
					{
					case 0:
					{
						if (z.avail_in == 0)
						{
							return num;
						}
						num = f;
						z.avail_in--;
						z.total_in += 1L;
						Inflate istate = z.istate;
						byte[] next_in = z.next_in;
						int next_in_index = z.next_in_index;
						z.next_in_index = next_in_index + 1;
						if (((istate.method = next_in[next_in_index]) & 15) != 8)
						{
							z.istate.mode = 13;
							z.msg = "unknown compression method";
							z.istate.marker = 5;
							continue;
						}
						if ((z.istate.method >> 4) + 8 > z.istate.wbits)
						{
							z.istate.mode = 13;
							z.msg = "invalid window size";
							z.istate.marker = 5;
							continue;
						}
						z.istate.mode = 1;
						goto IL_1E2;
					}
					case 1:
						goto IL_1E2;
					case 2:
						goto IL_49B;
					case 3:
						goto IL_505;
					case 4:
						goto IL_577;
					case 5:
						goto IL_5E8;
					case 6:
						goto IL_664;
					case 7:
						num = z.istate.blocks.proc(z, num);
						if (num == -3)
						{
							z.istate.mode = 13;
							z.istate.marker = 0;
							continue;
						}
						if (num == 0)
						{
							num = f;
						}
						if (num != 1)
						{
							return num;
						}
						num = f;
						z.istate.blocks.reset(z, z.istate.was);
						if (z.istate.nowrap != 0)
						{
							z.istate.mode = 12;
							continue;
						}
						z.istate.mode = 8;
						goto IL_413;
					case 8:
						goto IL_413;
					case 9:
						goto IL_39C;
					case 10:
						goto IL_326;
					case 11:
						goto IL_282;
					case 12:
						return 1;
					case 13:
						goto IL_6B2;
					}
					break;
					IL_1E2:
					if (z.avail_in == 0)
					{
						return num;
					}
					num = f;
					z.avail_in--;
					z.total_in += 1L;
					byte[] next_in2 = z.next_in;
					next_in_index2 = z.next_in_index;
					z.next_in_index = next_in_index2 + 1;
					int num2 = next_in2[next_in_index2] & 255;
					if (((z.istate.method << 8) + num2) % 31 != 0)
					{
						z.istate.mode = 13;
						z.msg = "incorrect header check";
						z.istate.marker = 5;
						continue;
					}
					if ((num2 & 32) == 0)
					{
						z.istate.mode = 7;
						continue;
					}
					goto IL_48F;
					IL_282:
					if (z.avail_in == 0)
					{
						return num;
					}
					num = f;
					z.avail_in--;
					z.total_in += 1L;
					Inflate istate2 = z.istate;
					long num3 = istate2.need;
					byte[] next_in3 = z.next_in;
					next_in_index2 = z.next_in_index;
					z.next_in_index = next_in_index2 + 1;
					istate2.need = num3 + (long)(next_in3[next_in_index2] & 255UL);
					if ((int)z.istate.was[0] != (int)z.istate.need)
					{
						z.istate.mode = 13;
						z.msg = "incorrect data check";
						z.istate.marker = 5;
						continue;
					}
					goto IL_6A1;
					IL_326:
					if (z.avail_in != 0)
					{
						num = f;
						z.avail_in--;
						z.total_in += 1L;
						Inflate istate3 = z.istate;
						long num4 = istate3.need;
						byte[] next_in4 = z.next_in;
						next_in_index2 = z.next_in_index;
						z.next_in_index = next_in_index2 + 1;
						istate3.need = num4 + ((long)(next_in4[next_in_index2] & 255) << 8 & 65280L);
						z.istate.mode = 11;
						goto IL_282;
					}
					return num;
					IL_39C:
					if (z.avail_in != 0)
					{
						num = f;
						z.avail_in--;
						z.total_in += 1L;
						Inflate istate4 = z.istate;
						long num5 = istate4.need;
						byte[] next_in5 = z.next_in;
						next_in_index2 = z.next_in_index;
						z.next_in_index = next_in_index2 + 1;
						istate4.need = num5 + ((long)(next_in5[next_in_index2] & 255) << 16 & 16711680L);
						z.istate.mode = 10;
						goto IL_326;
					}
					return num;
					IL_413:
					if (z.avail_in != 0)
					{
						num = f;
						z.avail_in--;
						z.total_in += 1L;
						Inflate istate5 = z.istate;
						byte[] next_in6 = z.next_in;
						next_in_index2 = z.next_in_index;
						z.next_in_index = next_in_index2 + 1;
						istate5.need = (long)((next_in6[next_in_index2] & 255) << 24 & -16777216);
						z.istate.mode = 9;
						goto IL_39C;
					}
					return num;
				}
				return -2;
				IL_48F:
				z.istate.mode = 2;
				IL_49B:
				if (z.avail_in == 0)
				{
					return num;
				}
				num = f;
				z.avail_in--;
				z.total_in += 1L;
				Inflate istate6 = z.istate;
				byte[] next_in7 = z.next_in;
				next_in_index2 = z.next_in_index;
				z.next_in_index = next_in_index2 + 1;
				istate6.need = (long)((next_in7[next_in_index2] & 255) << 24 & -16777216);
				z.istate.mode = 3;
				IL_505:
				if (z.avail_in == 0)
				{
					return num;
				}
				num = f;
				z.avail_in--;
				z.total_in += 1L;
				Inflate istate7 = z.istate;
				long num6 = istate7.need;
				byte[] next_in8 = z.next_in;
				next_in_index2 = z.next_in_index;
				z.next_in_index = next_in_index2 + 1;
				istate7.need = num6 + ((long)(next_in8[next_in_index2] & 255) << 16 & 16711680L);
				z.istate.mode = 4;
				IL_577:
				if (z.avail_in == 0)
				{
					return num;
				}
				num = f;
				z.avail_in--;
				z.total_in += 1L;
				Inflate istate8 = z.istate;
				long num7 = istate8.need;
				byte[] next_in9 = z.next_in;
				next_in_index2 = z.next_in_index;
				z.next_in_index = next_in_index2 + 1;
				istate8.need = num7 + ((long)(next_in9[next_in_index2] & 255) << 8 & 65280L);
				z.istate.mode = 5;
				IL_5E8:
				if (z.avail_in == 0)
				{
					return num;
				}
				z.avail_in--;
				z.total_in += 1L;
				Inflate istate9 = z.istate;
				long num8 = istate9.need;
				byte[] next_in10 = z.next_in;
				next_in_index2 = z.next_in_index;
				z.next_in_index = next_in_index2 + 1;
				istate9.need = num8 + (long)(next_in10[next_in_index2] & 255UL);
				z.adler = z.istate.need;
				z.istate.mode = 6;
				return 2;
				IL_664:
				z.istate.mode = 13;
				z.msg = "need dictionary";
				z.istate.marker = 0;
				return -2;
				IL_6A1:
				z.istate.mode = 12;
				return 1;
				IL_6B2:
				result = -3;
			}
			return result;
		}

		// Token: 0x06000056 RID: 86 RVA: 0x00007568 File Offset: 0x00005768
		internal int inflateSetDictionary(ZStream z, byte[] dictionary, int dictLength)
		{
			int start = 0;
			int num = dictLength;
			int result;
			if (z == null || z.istate == null || z.istate.mode != 6)
			{
				result = -2;
			}
			else if (z._adler.adler32(1L, dictionary, 0, dictLength) != z.adler)
			{
				result = -3;
			}
			else
			{
				z.adler = z._adler.adler32(0L, null, 0, 0);
				if (num >= 1 << z.istate.wbits)
				{
					num = (1 << z.istate.wbits) - 1;
					start = dictLength - num;
				}
				z.istate.blocks.set_dictionary(dictionary, start, num);
				z.istate.mode = 7;
				result = 0;
			}
			return result;
		}

		// Token: 0x06000057 RID: 87 RVA: 0x00007618 File Offset: 0x00005818
		internal int inflateSync(ZStream z)
		{
			int result;
			if (z == null || z.istate == null)
			{
				result = -2;
			}
			else
			{
				if (z.istate.mode != 13)
				{
					z.istate.mode = 13;
					z.istate.marker = 0;
				}
				int num;
				if ((num = z.avail_in) == 0)
				{
					result = -5;
				}
				else
				{
					int num2 = z.next_in_index;
					int num3 = z.istate.marker;
					while (num != 0 && num3 < 4)
					{
						if (z.next_in[num2] == Inflate.mark[num3])
						{
							num3++;
						}
						else if (z.next_in[num2] != 0)
						{
							num3 = 0;
						}
						else
						{
							num3 = 4 - num3;
						}
						num2++;
						num--;
					}
					z.total_in += (long)(num2 - z.next_in_index);
					z.next_in_index = num2;
					z.avail_in = num;
					z.istate.marker = num3;
					if (num3 != 4)
					{
						result = -3;
					}
					else
					{
						long total_in = z.total_in;
						long total_out = z.total_out;
						this.inflateReset(z);
						z.total_in = total_in;
						z.total_out = total_out;
						z.istate.mode = 7;
						result = 0;
					}
				}
			}
			return result;
		}

		// Token: 0x06000058 RID: 88 RVA: 0x00007730 File Offset: 0x00005930
		internal int inflateSyncPoint(ZStream z)
		{
			int result;
			if (z == null || z.istate == null || z.istate.blocks == null)
			{
				result = -2;
			}
			else
			{
				result = z.istate.blocks.sync_point();
			}
			return result;
		}

		// Token: 0x040000C2 RID: 194
		private const int MAX_WBITS = 15;

		// Token: 0x040000C3 RID: 195
		private const int PRESET_DICT = 32;

		// Token: 0x040000C4 RID: 196
		internal const int Z_NO_FLUSH = 0;

		// Token: 0x040000C5 RID: 197
		internal const int Z_PARTIAL_FLUSH = 1;

		// Token: 0x040000C6 RID: 198
		internal const int Z_SYNC_FLUSH = 2;

		// Token: 0x040000C7 RID: 199
		internal const int Z_FULL_FLUSH = 3;

		// Token: 0x040000C8 RID: 200
		internal const int Z_FINISH = 4;

		// Token: 0x040000C9 RID: 201
		private const int Z_DEFLATED = 8;

		// Token: 0x040000CA RID: 202
		private const int Z_OK = 0;

		// Token: 0x040000CB RID: 203
		private const int Z_STREAM_END = 1;

		// Token: 0x040000CC RID: 204
		private const int Z_NEED_DICT = 2;

		// Token: 0x040000CD RID: 205
		private const int Z_ERRNO = -1;

		// Token: 0x040000CE RID: 206
		private const int Z_STREAM_ERROR = -2;

		// Token: 0x040000CF RID: 207
		private const int Z_DATA_ERROR = -3;

		// Token: 0x040000D0 RID: 208
		private const int Z_MEM_ERROR = -4;

		// Token: 0x040000D1 RID: 209
		private const int Z_BUF_ERROR = -5;

		// Token: 0x040000D2 RID: 210
		private const int Z_VERSION_ERROR = -6;

		// Token: 0x040000D3 RID: 211
		private const int METHOD = 0;

		// Token: 0x040000D4 RID: 212
		private const int FLAG = 1;

		// Token: 0x040000D5 RID: 213
		private const int DICT4 = 2;

		// Token: 0x040000D6 RID: 214
		private const int DICT3 = 3;

		// Token: 0x040000D7 RID: 215
		private const int DICT2 = 4;

		// Token: 0x040000D8 RID: 216
		private const int DICT1 = 5;

		// Token: 0x040000D9 RID: 217
		private const int DICT0 = 6;

		// Token: 0x040000DA RID: 218
		private const int BLOCKS = 7;

		// Token: 0x040000DB RID: 219
		private const int CHECK4 = 8;

		// Token: 0x040000DC RID: 220
		private const int CHECK3 = 9;

		// Token: 0x040000DD RID: 221
		private const int CHECK2 = 10;

		// Token: 0x040000DE RID: 222
		private const int CHECK1 = 11;

		// Token: 0x040000DF RID: 223
		private const int DONE = 12;

		// Token: 0x040000E0 RID: 224
		private const int BAD = 13;

		// Token: 0x040000E1 RID: 225
		internal int mode;

		// Token: 0x040000E2 RID: 226
		internal int method;

		// Token: 0x040000E3 RID: 227
		internal long[] was = new long[1];

		// Token: 0x040000E4 RID: 228
		internal long need;

		// Token: 0x040000E5 RID: 229
		internal int marker;

		// Token: 0x040000E6 RID: 230
		internal int nowrap;

		// Token: 0x040000E7 RID: 231
		internal int wbits;

		// Token: 0x040000E8 RID: 232
		internal InfBlocks blocks;

		// Token: 0x040000E9 RID: 233
		private static byte[] mark = new byte[]
		{
			0,
			0,
			(byte)SupportClass.Identity(255L),
			(byte)SupportClass.Identity(255L)
		};
	}
}
