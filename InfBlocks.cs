using System;

namespace ComponentAce.Compression.Libs.zlib
{
	// Token: 0x02000009 RID: 9
	internal sealed class InfBlocks
	{
		// Token: 0x06000044 RID: 68 RVA: 0x00004BCC File Offset: 0x00002DCC
		internal InfBlocks(ZStream z, object checkfn, int w)
		{
			this.hufts = new int[4320];
			this.window = new byte[w];
			this.end = w;
			this.checkfn = checkfn;
			this.mode = 0;
			this.reset(z, null);
		}

		// Token: 0x06000045 RID: 69 RVA: 0x00004C30 File Offset: 0x00002E30
		internal void reset(ZStream z, long[] c)
		{
			if (c != null)
			{
				c[0] = this.check;
			}
			if (this.mode == 4 || this.mode == 5)
			{
				this.blens = null;
			}
			if (this.mode == 6)
			{
				this.codes.free(z);
			}
			this.mode = 0;
			this.bitk = 0;
			this.bitb = 0;
			this.read = (this.write = 0);
			if (this.checkfn != null)
			{
				z.adler = (this.check = z._adler.adler32(0L, null, 0, 0));
			}
		}

		// Token: 0x06000046 RID: 70 RVA: 0x00004CC4 File Offset: 0x00002EC4
		internal int proc(ZStream z, int r)
		{
			int num = z.next_in_index;
			int num2 = z.avail_in;
			int num3 = this.bitb;
			int i = this.bitk;
			int num4 = this.write;
			int num5 = (num4 < this.read) ? (this.read - num4 - 1) : (this.end - num4);
			int num6;
			for (;;)
			{
				int[] array;
				int[] array2;
				switch (this.mode)
				{
				case 0:
					while (i < 3)
					{
						if (num2 == 0)
						{
							goto IL_858;
						}
						r = 0;
						num2--;
						num3 |= (int)(z.next_in[num++] & byte.MaxValue) << i;
						i += 8;
					}
					num6 = (num3 & 7);
					this.last = (num6 & 1);
					switch (SupportClass.URShift(num6, 1))
					{
					case 0:
						num3 = SupportClass.URShift(num3, 3);
						i -= 3;
						num6 = (i & 7);
						num3 = SupportClass.URShift(num3, num6);
						i -= num6;
						this.mode = 1;
						continue;
					case 1:
					{
						array = new int[1];
						array2 = new int[1];
						int[][] array3 = new int[1][];
						int[][] array4 = new int[1][];
						InfTree.inflate_trees_fixed(array, array2, array3, array4, z);
						this.codes = new InfCodes(array[0], array2[0], array3[0], array4[0], z);
						num3 = SupportClass.URShift(num3, 3);
						i -= 3;
						this.mode = 6;
						continue;
					}
					case 2:
						num3 = SupportClass.URShift(num3, 3);
						i -= 3;
						this.mode = 3;
						continue;
					case 3:
						goto IL_89B;
					default:
						continue;
					}
					break;
				case 1:
					while (i < 32)
					{
						if (num2 == 0)
						{
							goto IL_901;
						}
						r = 0;
						num2--;
						num3 |= (int)(z.next_in[num++] & byte.MaxValue) << i;
						i += 8;
					}
					if ((SupportClass.URShift(~num3, 16) & 65535) == (num3 & 65535))
					{
						this.left = (num3 & 65535);
						num3 = (i = 0);
						this.mode = ((this.left != 0) ? 2 : ((this.last != 0) ? 7 : 0));
						continue;
					}
					goto IL_944;
				case 2:
					if (num2 == 0)
					{
						goto IL_99E;
					}
					if (num5 == 0)
					{
						if (num4 == this.end && this.read != 0)
						{
							num4 = 0;
							num5 = ((num4 < this.read) ? (this.read - num4 - 1) : (this.end - num4));
						}
						if (num5 == 0)
						{
							this.write = num4;
							r = this.inflate_flush(z, r);
							num4 = this.write;
							num5 = ((num4 < this.read) ? (this.read - num4 - 1) : (this.end - num4));
							if (num4 == this.end && this.read != 0)
							{
								num4 = 0;
								num5 = ((num4 < this.read) ? (this.read - num4 - 1) : (this.end - num4));
							}
							if (num5 == 0)
							{
								goto IL_9E1;
							}
						}
					}
					r = 0;
					num6 = this.left;
					if (num6 > num2)
					{
						num6 = num2;
					}
					if (num6 > num5)
					{
						num6 = num5;
					}
					Array.Copy(z.next_in, num, this.window, num4, num6);
					num += num6;
					num2 -= num6;
					num4 += num6;
					num5 -= num6;
					if ((this.left -= num6) == 0)
					{
						this.mode = ((this.last != 0) ? 7 : 0);
						continue;
					}
					continue;
				case 3:
					while (i < 14)
					{
						if (num2 == 0)
						{
							goto IL_A24;
						}
						r = 0;
						num2--;
						num3 |= (int)(z.next_in[num++] & byte.MaxValue) << i;
						i += 8;
					}
					num6 = (this.table = (num3 & 16383));
					if ((num6 & 31) <= 29 && (num6 >> 5 & 31) <= 29)
					{
						num6 = 258 + (num6 & 31) + (num6 >> 5 & 31);
						this.blens = new int[num6];
						num3 = SupportClass.URShift(num3, 14);
						i -= 14;
						this.index = 0;
						this.mode = 4;
						goto IL_789;
					}
					goto IL_A67;
				case 4:
					goto IL_789;
				case 5:
					goto IL_4A5;
				case 6:
					goto IL_3E8;
				case 7:
					goto IL_CB2;
				case 8:
					goto IL_D3E;
				case 9:
					goto IL_D84;
				}
				break;
				for (;;)
				{
					IL_4A5:
					num6 = this.table;
					if (this.index >= 258 + (num6 & 31) + (num6 >> 5 & 31))
					{
						break;
					}
					num6 = this.bb[0];
					while (i < num6)
					{
						if (num2 == 0)
						{
							goto IL_B5F;
						}
						r = 0;
						num2--;
						num3 |= (int)(z.next_in[num++] & byte.MaxValue) << i;
						i += 8;
					}
					int num7 = this.tb[0];
					num6 = this.hufts[(this.tb[0] + (num3 & InfBlocks.inflate_mask[num6])) * 3 + 1];
					int num8 = this.hufts[(this.tb[0] + (num3 & InfBlocks.inflate_mask[num6])) * 3 + 2];
					if (num8 < 16)
					{
						num3 = SupportClass.URShift(num3, num6);
						i -= num6;
						int[] array5 = this.blens;
						int num9 = this.index;
						this.index = num9 + 1;
						array5[num9] = num8;
					}
					else
					{
						int num10 = (num8 == 18) ? 7 : (num8 - 14);
						int num11 = (num8 == 18) ? 11 : 3;
						while (i < num6 + num10)
						{
							if (num2 == 0)
							{
								goto IL_BA2;
							}
							r = 0;
							num2--;
							num3 |= (int)(z.next_in[num++] & byte.MaxValue) << i;
							i += 8;
						}
						num3 = SupportClass.URShift(num3, num6);
						i -= num6;
						num11 += (num3 & InfBlocks.inflate_mask[num10]);
						num3 = SupportClass.URShift(num3, num10);
						i -= num10;
						num10 = this.index;
						num6 = this.table;
						if (num10 + num11 > 258 + (num6 & 31) + (num6 >> 5 & 31) || (num8 == 16 && num10 < 1))
						{
							goto IL_BE5;
						}
						num8 = ((num8 == 16) ? this.blens[num10 - 1] : 0);
						do
						{
							this.blens[num10++] = num8;
						}
						while (--num11 != 0);
						this.index = num10;
					}
				}
				this.tb[0] = -1;
				array = new int[1];
				array2 = new int[1];
				int[] array6 = new int[1];
				int[] array7 = new int[1];
				array[0] = 9;
				array2[0] = 6;
				num6 = this.table;
				num6 = InfTree.inflate_trees_dynamic(257 + (num6 & 31), 1 + (num6 >> 5 & 31), this.blens, array, array2, array6, array7, this.hufts, z);
				if (num6 == 0)
				{
					this.codes = new InfCodes(array[0], array2[0], this.hufts, array6[0], this.hufts, array7[0], z);
					this.blens = null;
					this.mode = 6;
					goto IL_3E8;
				}
				goto IL_C46;
				IL_789:
				while (this.index < 4 + SupportClass.URShift(this.table, 10))
				{
					while (i < 3)
					{
						if (num2 == 0)
						{
							goto IL_AC1;
						}
						r = 0;
						num2--;
						num3 |= (int)(z.next_in[num++] & byte.MaxValue) << i;
						i += 8;
					}
					int[] array8 = this.blens;
					int[] array9 = InfBlocks.border;
					int num9 = this.index;
					this.index = num9 + 1;
					array8[array9[num9]] = (num3 & 7);
					num3 = SupportClass.URShift(num3, 3);
					i -= 3;
				}
				while (this.index < 19)
				{
					int[] array10 = this.blens;
					int[] array11 = InfBlocks.border;
					int num9 = this.index;
					this.index = num9 + 1;
					array10[array11[num9]] = 0;
				}
				this.bb[0] = 7;
				num6 = InfTree.inflate_trees_bits(this.blens, this.bb, this.tb, this.hufts, z);
				if (num6 == 0)
				{
					this.index = 0;
					this.mode = 5;
					goto IL_4A5;
				}
				goto IL_B04;
				IL_3E8:
				this.bitb = num3;
				this.bitk = i;
				z.avail_in = num2;
				z.total_in += (long)(num - z.next_in_index);
				z.next_in_index = num;
				this.write = num4;
				if ((r = this.codes.proc(this, z, r)) != 1)
				{
					goto IL_CA2;
				}
				r = 0;
				this.codes.free(z);
				num = z.next_in_index;
				num2 = z.avail_in;
				num3 = this.bitb;
				i = this.bitk;
				num4 = this.write;
				num5 = ((num4 < this.read) ? (this.read - num4 - 1) : (this.end - num4));
				if (this.last != 0)
				{
					goto IL_CAB;
				}
				this.mode = 0;
			}
			r = -2;
			this.bitb = num3;
			this.bitk = i;
			z.avail_in = num2;
			z.total_in += (long)(num - z.next_in_index);
			z.next_in_index = num;
			this.write = num4;
			return this.inflate_flush(z, r);
			IL_858:
			this.bitb = num3;
			this.bitk = i;
			z.avail_in = num2;
			z.total_in += (long)(num - z.next_in_index);
			z.next_in_index = num;
			this.write = num4;
			return this.inflate_flush(z, r);
			IL_89B:
			num3 = SupportClass.URShift(num3, 3);
			i -= 3;
			this.mode = 9;
			z.msg = "invalid block type";
			r = -3;
			this.bitb = num3;
			this.bitk = i;
			z.avail_in = num2;
			z.total_in += (long)(num - z.next_in_index);
			z.next_in_index = num;
			this.write = num4;
			return this.inflate_flush(z, r);
			IL_901:
			this.bitb = num3;
			this.bitk = i;
			z.avail_in = num2;
			z.total_in += (long)(num - z.next_in_index);
			z.next_in_index = num;
			this.write = num4;
			return this.inflate_flush(z, r);
			IL_944:
			this.mode = 9;
			z.msg = "invalid stored block lengths";
			r = -3;
			this.bitb = num3;
			this.bitk = i;
			z.avail_in = num2;
			z.total_in += (long)(num - z.next_in_index);
			z.next_in_index = num;
			this.write = num4;
			return this.inflate_flush(z, r);
			IL_99E:
			this.bitb = num3;
			this.bitk = i;
			z.avail_in = num2;
			z.total_in += (long)(num - z.next_in_index);
			z.next_in_index = num;
			this.write = num4;
			return this.inflate_flush(z, r);
			IL_9E1:
			this.bitb = num3;
			this.bitk = i;
			z.avail_in = num2;
			z.total_in += (long)(num - z.next_in_index);
			z.next_in_index = num;
			this.write = num4;
			return this.inflate_flush(z, r);
			IL_A24:
			this.bitb = num3;
			this.bitk = i;
			z.avail_in = num2;
			z.total_in += (long)(num - z.next_in_index);
			z.next_in_index = num;
			this.write = num4;
			return this.inflate_flush(z, r);
			IL_A67:
			this.mode = 9;
			z.msg = "too many length or distance symbols";
			r = -3;
			this.bitb = num3;
			this.bitk = i;
			z.avail_in = num2;
			z.total_in += (long)(num - z.next_in_index);
			z.next_in_index = num;
			this.write = num4;
			return this.inflate_flush(z, r);
			IL_AC1:
			this.bitb = num3;
			this.bitk = i;
			z.avail_in = num2;
			z.total_in += (long)(num - z.next_in_index);
			z.next_in_index = num;
			this.write = num4;
			return this.inflate_flush(z, r);
			IL_B04:
			r = num6;
			if (r == -3)
			{
				this.blens = null;
				this.mode = 9;
			}
			this.bitb = num3;
			this.bitk = i;
			z.avail_in = num2;
			z.total_in += (long)(num - z.next_in_index);
			z.next_in_index = num;
			this.write = num4;
			return this.inflate_flush(z, r);
			IL_B5F:
			this.bitb = num3;
			this.bitk = i;
			z.avail_in = num2;
			z.total_in += (long)(num - z.next_in_index);
			z.next_in_index = num;
			this.write = num4;
			return this.inflate_flush(z, r);
			IL_BA2:
			this.bitb = num3;
			this.bitk = i;
			z.avail_in = num2;
			z.total_in += (long)(num - z.next_in_index);
			z.next_in_index = num;
			this.write = num4;
			return this.inflate_flush(z, r);
			IL_BE5:
			this.blens = null;
			this.mode = 9;
			z.msg = "invalid bit length repeat";
			r = -3;
			this.bitb = num3;
			this.bitk = i;
			z.avail_in = num2;
			z.total_in += (long)(num - z.next_in_index);
			z.next_in_index = num;
			this.write = num4;
			return this.inflate_flush(z, r);
			IL_C46:
			if (num6 == -3)
			{
				this.blens = null;
				this.mode = 9;
			}
			r = num6;
			this.bitb = num3;
			this.bitk = i;
			z.avail_in = num2;
			z.total_in += (long)(num - z.next_in_index);
			z.next_in_index = num;
			this.write = num4;
			return this.inflate_flush(z, r);
			IL_CA2:
			return this.inflate_flush(z, r);
			IL_CAB:
			this.mode = 7;
			IL_CB2:
			this.write = num4;
			r = this.inflate_flush(z, r);
			num4 = this.write;
			if (num4 >= this.read)
			{
				int num12 = this.end;
			}
			else
			{
				int num13 = this.read;
			}
			if (this.read != this.write)
			{
				this.bitb = num3;
				this.bitk = i;
				z.avail_in = num2;
				z.total_in += (long)(num - z.next_in_index);
				z.next_in_index = num;
				this.write = num4;
				return this.inflate_flush(z, r);
			}
			this.mode = 8;
			IL_D3E:
			r = 1;
			this.bitb = num3;
			this.bitk = i;
			z.avail_in = num2;
			z.total_in += (long)(num - z.next_in_index);
			z.next_in_index = num;
			this.write = num4;
			return this.inflate_flush(z, r);
			IL_D84:
			r = -3;
			this.bitb = num3;
			this.bitk = i;
			z.avail_in = num2;
			z.total_in += (long)(num - z.next_in_index);
			z.next_in_index = num;
			this.write = num4;
			return this.inflate_flush(z, r);
		}

		// Token: 0x06000047 RID: 71 RVA: 0x00005A9B File Offset: 0x00003C9B
		internal void free(ZStream z)
		{
			this.reset(z, null);
			this.window = null;
			this.hufts = null;
		}

		// Token: 0x06000048 RID: 72 RVA: 0x00005AB3 File Offset: 0x00003CB3
		internal void set_dictionary(byte[] d, int start, int n)
		{
			Array.Copy(d, start, this.window, 0, n);
			this.write = n;
			this.read = n;
		}

		// Token: 0x06000049 RID: 73 RVA: 0x00005AD2 File Offset: 0x00003CD2
		internal int sync_point()
		{
			if (this.mode != 1)
			{
				return 0;
			}
			return 1;
		}

		// Token: 0x0600004A RID: 74 RVA: 0x00005AE0 File Offset: 0x00003CE0
		internal int inflate_flush(ZStream z, int r)
		{
			int num = z.next_out_index;
			int num2 = this.read;
			int num3 = ((num2 <= this.write) ? this.write : this.end) - num2;
			if (num3 > z.avail_out)
			{
				num3 = z.avail_out;
			}
			if (num3 != 0 && r == -5)
			{
				r = 0;
			}
			z.avail_out -= num3;
			z.total_out += (long)num3;
			if (this.checkfn != null)
			{
				z.adler = (this.check = z._adler.adler32(this.check, this.window, num2, num3));
			}
			Array.Copy(this.window, num2, z.next_out, num, num3);
			num += num3;
			num2 += num3;
			if (num2 == this.end)
			{
				num2 = 0;
				if (this.write == this.end)
				{
					this.write = 0;
				}
				num3 = this.write - num2;
				if (num3 > z.avail_out)
				{
					num3 = z.avail_out;
				}
				if (num3 != 0 && r == -5)
				{
					r = 0;
				}
				z.avail_out -= num3;
				z.total_out += (long)num3;
				if (this.checkfn != null)
				{
					z.adler = (this.check = z._adler.adler32(this.check, this.window, num2, num3));
				}
				Array.Copy(this.window, num2, z.next_out, num, num3);
				num += num3;
				num2 += num3;
			}
			z.next_out_index = num;
			this.read = num2;
			return r;
		}

		// Token: 0x04000078 RID: 120
		private const int MANY = 1440;

		// Token: 0x04000079 RID: 121
		private const int Z_OK = 0;

		// Token: 0x0400007A RID: 122
		private const int Z_STREAM_END = 1;

		// Token: 0x0400007B RID: 123
		private const int Z_NEED_DICT = 2;

		// Token: 0x0400007C RID: 124
		private const int Z_ERRNO = -1;

		// Token: 0x0400007D RID: 125
		private const int Z_STREAM_ERROR = -2;

		// Token: 0x0400007E RID: 126
		private const int Z_DATA_ERROR = -3;

		// Token: 0x0400007F RID: 127
		private const int Z_MEM_ERROR = -4;

		// Token: 0x04000080 RID: 128
		private const int Z_BUF_ERROR = -5;

		// Token: 0x04000081 RID: 129
		private const int Z_VERSION_ERROR = -6;

		// Token: 0x04000082 RID: 130
		private const int TYPE = 0;

		// Token: 0x04000083 RID: 131
		private const int LENS = 1;

		// Token: 0x04000084 RID: 132
		private const int STORED = 2;

		// Token: 0x04000085 RID: 133
		private const int TABLE = 3;

		// Token: 0x04000086 RID: 134
		private const int BTREE = 4;

		// Token: 0x04000087 RID: 135
		private const int DTREE = 5;

		// Token: 0x04000088 RID: 136
		private const int CODES = 6;

		// Token: 0x04000089 RID: 137
		private const int DRY = 7;

		// Token: 0x0400008A RID: 138
		private const int DONE = 8;

		// Token: 0x0400008B RID: 139
		private const int BAD = 9;

		// Token: 0x0400008C RID: 140
		private static readonly int[] inflate_mask = new int[]
		{
			0,
			1,
			3,
			7,
			15,
			31,
			63,
			127,
			255,
			511,
			1023,
			2047,
			4095,
			8191,
			16383,
			32767,
			65535
		};

		// Token: 0x0400008D RID: 141
		internal static readonly int[] border = new int[]
		{
			16,
			17,
			18,
			0,
			8,
			7,
			9,
			6,
			10,
			5,
			11,
			4,
			12,
			3,
			13,
			2,
			14,
			1,
			15
		};

		// Token: 0x0400008E RID: 142
		internal int mode;

		// Token: 0x0400008F RID: 143
		internal int left;

		// Token: 0x04000090 RID: 144
		internal int table;

		// Token: 0x04000091 RID: 145
		internal int index;

		// Token: 0x04000092 RID: 146
		internal int[] blens;

		// Token: 0x04000093 RID: 147
		internal int[] bb = new int[1];

		// Token: 0x04000094 RID: 148
		internal int[] tb = new int[1];

		// Token: 0x04000095 RID: 149
		internal InfCodes codes;

		// Token: 0x04000096 RID: 150
		internal int last;

		// Token: 0x04000097 RID: 151
		internal int bitk;

		// Token: 0x04000098 RID: 152
		internal int bitb;

		// Token: 0x04000099 RID: 153
		internal int[] hufts;

		// Token: 0x0400009A RID: 154
		internal byte[] window;

		// Token: 0x0400009B RID: 155
		internal int end;

		// Token: 0x0400009C RID: 156
		internal int read;

		// Token: 0x0400009D RID: 157
		internal int write;

		// Token: 0x0400009E RID: 158
		internal object checkfn;

		// Token: 0x0400009F RID: 159
		internal long check;
	}
}
