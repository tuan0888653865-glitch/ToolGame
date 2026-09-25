using System;

namespace ComponentAce.Compression.Libs.zlib
{
	// Token: 0x0200000A RID: 10
	internal sealed class InfCodes
	{
		// Token: 0x0600004C RID: 76 RVA: 0x00005C88 File Offset: 0x00003E88
		internal InfCodes(int bl, int bd, int[] tl, int tl_index, int[] td, int td_index, ZStream z)
		{
			this.mode = 0;
			this.lbits = (byte)bl;
			this.dbits = (byte)bd;
			this.ltree = tl;
			this.ltree_index = tl_index;
			this.dtree = td;
			this.dtree_index = td_index;
		}

		// Token: 0x0600004D RID: 77 RVA: 0x00005CC6 File Offset: 0x00003EC6
		internal InfCodes(int bl, int bd, int[] tl, int[] td, ZStream z)
		{
			this.mode = 0;
			this.lbits = (byte)bl;
			this.dbits = (byte)bd;
			this.ltree = tl;
			this.ltree_index = 0;
			this.dtree = td;
			this.dtree_index = 0;
		}

		// Token: 0x0600004E RID: 78 RVA: 0x00005D04 File Offset: 0x00003F04
		internal int proc(InfBlocks s, ZStream z, int r)
		{
			int num = z.next_in_index;
			int num2 = z.avail_in;
			int num3 = s.bitb;
			int i = s.bitk;
			int num4 = s.write;
			int num5 = (num4 < s.read) ? (s.read - num4 - 1) : (s.end - num4);
			for (;;)
			{
				int num6;
				switch (this.mode)
				{
				case 0:
					if (num5 >= 258 && num2 >= 10)
					{
						s.bitb = num3;
						s.bitk = i;
						z.avail_in = num2;
						z.total_in += (long)(num - z.next_in_index);
						z.next_in_index = num;
						s.write = num4;
						r = this.inflate_fast((int)this.lbits, (int)this.dbits, this.ltree, this.ltree_index, this.dtree, this.dtree_index, s, z);
						num = z.next_in_index;
						num2 = z.avail_in;
						num3 = s.bitb;
						i = s.bitk;
						num4 = s.write;
						num5 = ((num4 < s.read) ? (s.read - num4 - 1) : (s.end - num4));
						if (r != 0)
						{
							this.mode = ((r == 1) ? 7 : 9);
							continue;
						}
					}
					this.need = (int)this.lbits;
					this.tree = this.ltree;
					this.tree_index = this.ltree_index;
					this.mode = 1;
					goto IL_369;
				case 1:
					goto IL_369;
				case 2:
					num6 = this.get_Renamed;
					while (i < num6)
					{
						if (num2 == 0)
						{
							goto IL_75E;
						}
						r = 0;
						num2--;
						num3 |= (int)(z.next_in[num++] & byte.MaxValue) << i;
						i += 8;
					}
					this.len += (num3 & InfCodes.inflate_mask[num6]);
					num3 >>= num6;
					i -= num6;
					this.need = (int)this.dbits;
					this.tree = this.dtree;
					this.tree_index = this.dtree_index;
					this.mode = 3;
					goto IL_473;
				case 3:
					goto IL_473;
				case 4:
					num6 = this.get_Renamed;
					while (i < num6)
					{
						if (num2 == 0)
						{
							goto IL_83E;
						}
						r = 0;
						num2--;
						num3 |= (int)(z.next_in[num++] & byte.MaxValue) << i;
						i += 8;
					}
					this.dist += (num3 & InfCodes.inflate_mask[num6]);
					num3 >>= num6;
					i -= num6;
					this.mode = 5;
					goto IL_548;
				case 5:
					goto IL_548;
				case 6:
					if (num5 == 0)
					{
						if (num4 == s.end && s.read != 0)
						{
							num4 = 0;
							num5 = ((num4 < s.read) ? (s.read - num4 - 1) : (s.end - num4));
						}
						if (num5 == 0)
						{
							s.write = num4;
							r = s.inflate_flush(z, r);
							num4 = s.write;
							num5 = ((num4 < s.read) ? (s.read - num4 - 1) : (s.end - num4));
							if (num4 == s.end && s.read != 0)
							{
								num4 = 0;
								num5 = ((num4 < s.read) ? (s.read - num4 - 1) : (s.end - num4));
							}
							if (num5 == 0)
							{
								goto IL_8C4;
							}
						}
					}
					r = 0;
					s.window[num4++] = (byte)this.lit;
					num5--;
					this.mode = 0;
					continue;
				case 7:
					goto IL_907;
				case 8:
					goto IL_9A3;
				case 9:
					goto IL_9E9;
				}
				break;
				IL_369:
				num6 = this.need;
				while (i < num6)
				{
					if (num2 == 0)
					{
						goto IL_6C1;
					}
					r = 0;
					num2--;
					num3 |= (int)(z.next_in[num++] & byte.MaxValue) << i;
					i += 8;
				}
				int num7 = (this.tree_index + (num3 & InfCodes.inflate_mask[num6])) * 3;
				num3 = SupportClass.URShift(num3, this.tree[num7 + 1]);
				i -= this.tree[num7 + 1];
				int num8 = this.tree[num7];
				if (num8 == 0)
				{
					this.lit = this.tree[num7 + 2];
					this.mode = 6;
					continue;
				}
				if ((num8 & 16) != 0)
				{
					this.get_Renamed = (num8 & 15);
					this.len = this.tree[num7 + 2];
					this.mode = 2;
					continue;
				}
				if ((num8 & 64) == 0)
				{
					this.need = num8;
					this.tree_index = num7 / 3 + this.tree[num7 + 2];
					continue;
				}
				if ((num8 & 32) != 0)
				{
					this.mode = 7;
					continue;
				}
				goto IL_704;
				IL_473:
				num6 = this.need;
				while (i < num6)
				{
					if (num2 == 0)
					{
						goto IL_7A1;
					}
					r = 0;
					num2--;
					num3 |= (int)(z.next_in[num++] & byte.MaxValue) << i;
					i += 8;
				}
				num7 = (this.tree_index + (num3 & InfCodes.inflate_mask[num6])) * 3;
				num3 >>= this.tree[num7 + 1];
				i -= this.tree[num7 + 1];
				num8 = this.tree[num7];
				if ((num8 & 16) != 0)
				{
					this.get_Renamed = (num8 & 15);
					this.dist = this.tree[num7 + 2];
					this.mode = 4;
					continue;
				}
				if ((num8 & 64) == 0)
				{
					this.need = num8;
					this.tree_index = num7 / 3 + this.tree[num7 + 2];
					continue;
				}
				goto IL_7E4;
				IL_548:
				int j;
				for (j = num4 - this.dist; j < 0; j += s.end)
				{
				}
				while (this.len != 0)
				{
					if (num5 == 0)
					{
						if (num4 == s.end && s.read != 0)
						{
							num4 = 0;
							num5 = ((num4 < s.read) ? (s.read - num4 - 1) : (s.end - num4));
						}
						if (num5 == 0)
						{
							s.write = num4;
							r = s.inflate_flush(z, r);
							num4 = s.write;
							num5 = ((num4 < s.read) ? (s.read - num4 - 1) : (s.end - num4));
							if (num4 == s.end && s.read != 0)
							{
								num4 = 0;
								num5 = ((num4 < s.read) ? (s.read - num4 - 1) : (s.end - num4));
							}
							if (num5 == 0)
							{
								goto IL_881;
							}
						}
					}
					s.window[num4++] = s.window[j++];
					num5--;
					if (j == s.end)
					{
						j = 0;
					}
					this.len--;
				}
				this.mode = 0;
			}
			r = -2;
			s.bitb = num3;
			s.bitk = i;
			z.avail_in = num2;
			z.total_in += (long)(num - z.next_in_index);
			z.next_in_index = num;
			s.write = num4;
			return s.inflate_flush(z, r);
			IL_6C1:
			s.bitb = num3;
			s.bitk = i;
			z.avail_in = num2;
			z.total_in += (long)(num - z.next_in_index);
			z.next_in_index = num;
			s.write = num4;
			return s.inflate_flush(z, r);
			IL_704:
			this.mode = 9;
			z.msg = "invalid literal/length code";
			r = -3;
			s.bitb = num3;
			s.bitk = i;
			z.avail_in = num2;
			z.total_in += (long)(num - z.next_in_index);
			z.next_in_index = num;
			s.write = num4;
			return s.inflate_flush(z, r);
			IL_75E:
			s.bitb = num3;
			s.bitk = i;
			z.avail_in = num2;
			z.total_in += (long)(num - z.next_in_index);
			z.next_in_index = num;
			s.write = num4;
			return s.inflate_flush(z, r);
			IL_7A1:
			s.bitb = num3;
			s.bitk = i;
			z.avail_in = num2;
			z.total_in += (long)(num - z.next_in_index);
			z.next_in_index = num;
			s.write = num4;
			return s.inflate_flush(z, r);
			IL_7E4:
			this.mode = 9;
			z.msg = "invalid distance code";
			r = -3;
			s.bitb = num3;
			s.bitk = i;
			z.avail_in = num2;
			z.total_in += (long)(num - z.next_in_index);
			z.next_in_index = num;
			s.write = num4;
			return s.inflate_flush(z, r);
			IL_83E:
			s.bitb = num3;
			s.bitk = i;
			z.avail_in = num2;
			z.total_in += (long)(num - z.next_in_index);
			z.next_in_index = num;
			s.write = num4;
			return s.inflate_flush(z, r);
			IL_881:
			s.bitb = num3;
			s.bitk = i;
			z.avail_in = num2;
			z.total_in += (long)(num - z.next_in_index);
			z.next_in_index = num;
			s.write = num4;
			return s.inflate_flush(z, r);
			IL_8C4:
			s.bitb = num3;
			s.bitk = i;
			z.avail_in = num2;
			z.total_in += (long)(num - z.next_in_index);
			z.next_in_index = num;
			s.write = num4;
			return s.inflate_flush(z, r);
			IL_907:
			if (i > 7)
			{
				i -= 8;
				num2++;
				num--;
			}
			s.write = num4;
			r = s.inflate_flush(z, r);
			num4 = s.write;
			if (num4 >= s.read)
			{
				int end = s.end;
			}
			else
			{
				int read = s.read;
			}
			if (s.read != s.write)
			{
				s.bitb = num3;
				s.bitk = i;
				z.avail_in = num2;
				z.total_in += (long)(num - z.next_in_index);
				z.next_in_index = num;
				s.write = num4;
				return s.inflate_flush(z, r);
			}
			this.mode = 8;
			IL_9A3:
			r = 1;
			s.bitb = num3;
			s.bitk = i;
			z.avail_in = num2;
			z.total_in += (long)(num - z.next_in_index);
			z.next_in_index = num;
			s.write = num4;
			return s.inflate_flush(z, r);
			IL_9E9:
			r = -3;
			s.bitb = num3;
			s.bitk = i;
			z.avail_in = num2;
			z.total_in += (long)(num - z.next_in_index);
			z.next_in_index = num;
			s.write = num4;
			return s.inflate_flush(z, r);
		}

		// Token: 0x0600004F RID: 79 RVA: 0x00006740 File Offset: 0x00004940
		internal void free(ZStream z)
		{
		}

		// Token: 0x06000050 RID: 80 RVA: 0x00006744 File Offset: 0x00004944
		internal int inflate_fast(int bl, int bd, int[] tl, int tl_index, int[] td, int td_index, InfBlocks s, ZStream z)
		{
			int num = z.next_in_index;
			int num2 = z.avail_in;
			int num3 = s.bitb;
			int i = s.bitk;
			int num4 = s.write;
			int num5 = (num4 < s.read) ? (s.read - num4 - 1) : (s.end - num4);
			int num6 = InfCodes.inflate_mask[bl];
			int num7 = InfCodes.inflate_mask[bd];
			int num9;
			int num10;
			for (;;)
			{
				if (i >= 20)
				{
					int num8 = num3 & num6;
					if ((num9 = tl[(tl_index + num8) * 3]) == 0)
					{
						num3 >>= tl[(tl_index + num8) * 3 + 1];
						i -= tl[(tl_index + num8) * 3 + 1];
						s.window[num4++] = (byte)tl[(tl_index + num8) * 3 + 2];
						num5--;
					}
					else
					{
						do
						{
							num3 >>= tl[(tl_index + num8) * 3 + 1];
							i -= tl[(tl_index + num8) * 3 + 1];
							if ((num9 & 16) != 0)
							{
								goto IL_185;
							}
							if ((num9 & 64) != 0)
							{
								goto IL_4D2;
							}
							num8 += tl[(tl_index + num8) * 3 + 2];
							num8 += (num3 & InfCodes.inflate_mask[num9]);
						}
						while ((num9 = tl[(tl_index + num8) * 3]) != 0);
						num3 >>= tl[(tl_index + num8) * 3 + 1];
						i -= tl[(tl_index + num8) * 3 + 1];
						s.window[num4++] = (byte)tl[(tl_index + num8) * 3 + 2];
						num5--;
						goto IL_43D;
						IL_185:
						num9 &= 15;
						num10 = tl[(tl_index + num8) * 3 + 2] + (num3 & InfCodes.inflate_mask[num9]);
						num3 >>= num9;
						for (i -= num9; i < 15; i += 8)
						{
							num2--;
							num3 |= (int)(z.next_in[num++] & byte.MaxValue) << i;
						}
						num8 = (num3 & num7);
						num9 = td[(td_index + num8) * 3];
						for (;;)
						{
							num3 >>= td[(td_index + num8) * 3 + 1];
							i -= td[(td_index + num8) * 3 + 1];
							if ((num9 & 16) != 0)
							{
								break;
							}
							if ((num9 & 64) != 0)
							{
								goto IL_456;
							}
							num8 += td[(td_index + num8) * 3 + 2];
							num8 += (num3 & InfCodes.inflate_mask[num9]);
							num9 = td[(td_index + num8) * 3];
						}
						num9 &= 15;
						while (i < num9)
						{
							num2--;
							num3 |= (int)(z.next_in[num++] & byte.MaxValue) << i;
							i += 8;
						}
						int num11 = td[(td_index + num8) * 3 + 2] + (num3 & InfCodes.inflate_mask[num9]);
						num3 >>= num9;
						i -= num9;
						num5 -= num10;
						int num12;
						if (num4 >= num11)
						{
							num12 = num4 - num11;
							if (num4 - num12 > 0 && 2 > num4 - num12)
							{
								s.window[num4++] = s.window[num12++];
								num10--;
								s.window[num4++] = s.window[num12++];
								num10--;
							}
							else
							{
								Array.Copy(s.window, num12, s.window, num4, 2);
								num4 += 2;
								num12 += 2;
								num10 -= 2;
							}
						}
						else
						{
							num12 = num4 - num11;
							do
							{
								num12 += s.end;
							}
							while (num12 < 0);
							num9 = s.end - num12;
							if (num10 > num9)
							{
								num10 -= num9;
								if (num4 - num12 > 0 && num9 > num4 - num12)
								{
									do
									{
										s.window[num4++] = s.window[num12++];
									}
									while (--num9 != 0);
								}
								else
								{
									Array.Copy(s.window, num12, s.window, num4, num9);
									num4 += num9;
									num12 += num9;
								}
								num12 = 0;
							}
						}
						if (num4 - num12 > 0 && num10 > num4 - num12)
						{
							do
							{
								s.window[num4++] = s.window[num12++];
							}
							while (--num10 != 0);
						}
						else
						{
							Array.Copy(s.window, num12, s.window, num4, num10);
							num4 += num10;
							num12 += num10;
						}
					}
					IL_43D:
					if (num5 < 258)
					{
						break;
					}
					if (num2 < 10)
					{
						break;
					}
				}
				else
				{
					num2--;
					num3 |= (int)(z.next_in[num++] & byte.MaxValue) << i;
					i += 8;
				}
			}
			goto IL_5C4;
			IL_456:
			z.msg = "invalid distance code";
			num10 = z.avail_in - num2;
			num10 = ((i >> 3 < num10) ? (i >> 3) : num10);
			num2 += num10;
			num -= num10;
			i -= num10 << 3;
			s.bitb = num3;
			s.bitk = i;
			z.avail_in = num2;
			z.total_in += (long)(num - z.next_in_index);
			z.next_in_index = num;
			s.write = num4;
			return -3;
			IL_4D2:
			if ((num9 & 32) != 0)
			{
				num10 = z.avail_in - num2;
				num10 = ((i >> 3 < num10) ? (i >> 3) : num10);
				num2 += num10;
				num -= num10;
				i -= num10 << 3;
				s.bitb = num3;
				s.bitk = i;
				z.avail_in = num2;
				z.total_in += (long)(num - z.next_in_index);
				z.next_in_index = num;
				s.write = num4;
				return 1;
			}
			z.msg = "invalid literal/length code";
			num10 = z.avail_in - num2;
			num10 = ((i >> 3 < num10) ? (i >> 3) : num10);
			num2 += num10;
			num -= num10;
			i -= num10 << 3;
			s.bitb = num3;
			s.bitk = i;
			z.avail_in = num2;
			z.total_in += (long)(num - z.next_in_index);
			z.next_in_index = num;
			s.write = num4;
			return -3;
			IL_5C4:
			num10 = z.avail_in - num2;
			num10 = ((i >> 3 < num10) ? (i >> 3) : num10);
			num2 += num10;
			num -= num10;
			i -= num10 << 3;
			s.bitb = num3;
			s.bitk = i;
			z.avail_in = num2;
			z.total_in += (long)(num - z.next_in_index);
			z.next_in_index = num;
			s.write = num4;
			return 0;
		}

		// Token: 0x040000A0 RID: 160
		private const int Z_OK = 0;

		// Token: 0x040000A1 RID: 161
		private const int Z_STREAM_END = 1;

		// Token: 0x040000A2 RID: 162
		private const int Z_NEED_DICT = 2;

		// Token: 0x040000A3 RID: 163
		private const int Z_ERRNO = -1;

		// Token: 0x040000A4 RID: 164
		private const int Z_STREAM_ERROR = -2;

		// Token: 0x040000A5 RID: 165
		private const int Z_DATA_ERROR = -3;

		// Token: 0x040000A6 RID: 166
		private const int Z_MEM_ERROR = -4;

		// Token: 0x040000A7 RID: 167
		private const int Z_BUF_ERROR = -5;

		// Token: 0x040000A8 RID: 168
		private const int Z_VERSION_ERROR = -6;

		// Token: 0x040000A9 RID: 169
		private const int START = 0;

		// Token: 0x040000AA RID: 170
		private const int LEN = 1;

		// Token: 0x040000AB RID: 171
		private const int LENEXT = 2;

		// Token: 0x040000AC RID: 172
		private const int DIST = 3;

		// Token: 0x040000AD RID: 173
		private const int DISTEXT = 4;

		// Token: 0x040000AE RID: 174
		private const int COPY = 5;

		// Token: 0x040000AF RID: 175
		private const int LIT = 6;

		// Token: 0x040000B0 RID: 176
		private const int WASH = 7;

		// Token: 0x040000B1 RID: 177
		private const int END = 8;

		// Token: 0x040000B2 RID: 178
		private const int BADCODE = 9;

		// Token: 0x040000B3 RID: 179
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

		// Token: 0x040000B4 RID: 180
		internal int mode;

		// Token: 0x040000B5 RID: 181
		internal int len;

		// Token: 0x040000B6 RID: 182
		internal int[] tree;

		// Token: 0x040000B7 RID: 183
		internal int tree_index;

		// Token: 0x040000B8 RID: 184
		internal int need;

		// Token: 0x040000B9 RID: 185
		internal int lit;

		// Token: 0x040000BA RID: 186
		internal int get_Renamed;

		// Token: 0x040000BB RID: 187
		internal int dist;

		// Token: 0x040000BC RID: 188
		internal byte lbits;

		// Token: 0x040000BD RID: 189
		internal byte dbits;

		// Token: 0x040000BE RID: 190
		internal int[] ltree;

		// Token: 0x040000BF RID: 191
		internal int ltree_index;

		// Token: 0x040000C0 RID: 192
		internal int[] dtree;

		// Token: 0x040000C1 RID: 193
		internal int dtree_index;
	}
}
