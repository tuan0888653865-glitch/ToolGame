using System;

namespace ComponentAce.Compression.Libs.zlib
{
	// Token: 0x02000008 RID: 8
	public sealed class Deflate
	{
		// Token: 0x0600001C RID: 28 RVA: 0x000029B8 File Offset: 0x00000BB8
		internal Deflate()
		{
			this.dyn_ltree = new short[Deflate.HEAP_SIZE * 2];
			this.dyn_dtree = new short[122];
			this.bl_tree = new short[78];
		}

		// Token: 0x0600001D RID: 29 RVA: 0x00002A50 File Offset: 0x00000C50
		internal void lm_init()
		{
			this.window_size = 2 * this.w_size;
			this.head[this.hash_size - 1] = 0;
			for (int i = 0; i < this.hash_size - 1; i++)
			{
				this.head[i] = 0;
			}
			this.max_lazy_match = Deflate.config_table[this.level].max_lazy;
			this.good_match = Deflate.config_table[this.level].good_length;
			this.nice_match = Deflate.config_table[this.level].nice_length;
			this.max_chain_length = Deflate.config_table[this.level].max_chain;
			this.strstart = 0;
			this.block_start = 0;
			this.lookahead = 0;
			this.match_length = (this.prev_length = 2);
			this.match_available = 0;
			this.ins_h = 0;
		}

		// Token: 0x0600001E RID: 30 RVA: 0x00002B28 File Offset: 0x00000D28
		internal void tr_init()
		{
			this.l_desc.dyn_tree = this.dyn_ltree;
			this.l_desc.stat_desc = StaticTree.static_l_desc;
			this.d_desc.dyn_tree = this.dyn_dtree;
			this.d_desc.stat_desc = StaticTree.static_d_desc;
			this.bl_desc.dyn_tree = this.bl_tree;
			this.bl_desc.stat_desc = StaticTree.static_bl_desc;
			this.bi_buf = 0;
			this.bi_valid = 0;
			this.last_eob_len = 8;
			this.init_block();
		}

		// Token: 0x0600001F RID: 31 RVA: 0x00002BB4 File Offset: 0x00000DB4
		internal void init_block()
		{
			for (int i = 0; i < Deflate.L_CODES; i++)
			{
				this.dyn_ltree[i * 2] = 0;
			}
			for (int j = 0; j < 30; j++)
			{
				this.dyn_dtree[j * 2] = 0;
			}
			for (int k = 0; k < 19; k++)
			{
				this.bl_tree[k * 2] = 0;
			}
			this.dyn_ltree[512] = 1;
			this.opt_len = (this.static_len = 0);
			this.last_lit = (this.matches = 0);
		}

		// Token: 0x06000020 RID: 32 RVA: 0x00002C3C File Offset: 0x00000E3C
		internal void pqdownheap(short[] tree, int k)
		{
			int num = this.heap[k];
			for (int i = k << 1; i <= this.heap_len; i <<= 1)
			{
				if (i < this.heap_len && Deflate.smaller(tree, this.heap[i + 1], this.heap[i], this.depth))
				{
					i++;
				}
				if (Deflate.smaller(tree, num, this.heap[i], this.depth))
				{
					break;
				}
				this.heap[k] = this.heap[i];
				k = i;
			}
			this.heap[k] = num;
		}

		// Token: 0x06000021 RID: 33 RVA: 0x00002CC5 File Offset: 0x00000EC5
		internal static bool smaller(short[] tree, int n, int m, byte[] depth)
		{
			return tree[n * 2] < tree[m * 2] || (tree[n * 2] == tree[m * 2] && depth[n] <= depth[m]);
		}

		// Token: 0x06000022 RID: 34 RVA: 0x00002CF0 File Offset: 0x00000EF0
		internal void scan_tree(short[] tree, int max_code)
		{
			int num = -1;
			int num2 = (int)tree[1];
			int num3 = 0;
			int num4 = 7;
			int num5 = 4;
			if (num2 == 0)
			{
				num4 = 138;
				num5 = 3;
			}
			tree[(max_code + 1) * 2 + 1] = (short)SupportClass.Identity(65535L);
			for (int i = 0; i <= max_code; i++)
			{
				int num6 = num2;
				num2 = (int)tree[(i + 1) * 2 + 1];
				if (++num3 >= num4 || num6 != num2)
				{
					if (num3 < num5)
					{
						this.bl_tree[num6 * 2] = (short)((int)this.bl_tree[num6 * 2] + num3);
					}
					else if (num6 != 0)
					{
						if (num6 != num)
						{
							short[] array = this.bl_tree;
							int num7 = num6 * 2;
							int num8 = num7;
							array[num8] += 1;
						}
						short[] array2 = this.bl_tree;
						int num9 = 32;
						int num10 = num9;
						array2[num10] += 1;
					}
					else if (num3 <= 10)
					{
						short[] array3 = this.bl_tree;
						int num11 = 34;
						int num12 = num11;
						array3[num12] += 1;
					}
					else
					{
						short[] array4 = this.bl_tree;
						int num13 = 36;
						int num14 = num13;
						array4[num14] += 1;
					}
					num3 = 0;
					num = num6;
					if (num2 == 0)
					{
						num4 = 138;
						num5 = 3;
					}
					else if (num6 == num2)
					{
						num4 = 6;
						num5 = 3;
					}
					else
					{
						num4 = 7;
						num5 = 4;
					}
				}
			}
		}

		// Token: 0x06000023 RID: 35 RVA: 0x00002E14 File Offset: 0x00001014
		internal int build_bl_tree()
		{
			this.scan_tree(this.dyn_ltree, this.l_desc.max_code);
			this.scan_tree(this.dyn_dtree, this.d_desc.max_code);
			this.bl_desc.build_tree(this);
			int num = 18;
			while (num >= 3 && this.bl_tree[(int)(Tree.bl_order[num] * 2 + 1)] == 0)
			{
				num--;
			}
			this.opt_len += 3 * (num + 1) + 5 + 5 + 4;
			return num;
		}

		// Token: 0x06000024 RID: 36 RVA: 0x00002E98 File Offset: 0x00001098
		internal void send_all_trees(int lcodes, int dcodes, int blcodes)
		{
			this.send_bits(lcodes - 257, 5);
			this.send_bits(dcodes - 1, 5);
			this.send_bits(blcodes - 4, 4);
			for (int i = 0; i < blcodes; i++)
			{
				this.send_bits((int)this.bl_tree[(int)(Tree.bl_order[i] * 2 + 1)], 3);
			}
			this.send_tree(this.dyn_ltree, lcodes - 1);
			this.send_tree(this.dyn_dtree, dcodes - 1);
		}

		// Token: 0x06000025 RID: 37 RVA: 0x00002F0C File Offset: 0x0000110C
		internal void send_tree(short[] tree, int max_code)
		{
			int num = -1;
			int num2 = (int)tree[1];
			int num3 = 0;
			int num4 = 7;
			int num5 = 4;
			if (num2 == 0)
			{
				num4 = 138;
				num5 = 3;
			}
			for (int i = 0; i <= max_code; i++)
			{
				int num6 = num2;
				num2 = (int)tree[(i + 1) * 2 + 1];
				if (++num3 >= num4 || num6 != num2)
				{
					if (num3 < num5)
					{
						do
						{
							this.send_code(num6, this.bl_tree);
						}
						while (--num3 != 0);
					}
					else if (num6 != 0)
					{
						if (num6 != num)
						{
							this.send_code(num6, this.bl_tree);
							num3--;
						}
						this.send_code(16, this.bl_tree);
						this.send_bits(num3 - 3, 2);
					}
					else if (num3 <= 10)
					{
						this.send_code(17, this.bl_tree);
						this.send_bits(num3 - 3, 3);
					}
					else
					{
						this.send_code(18, this.bl_tree);
						this.send_bits(num3 - 11, 7);
					}
					num3 = 0;
					num = num6;
					if (num2 == 0)
					{
						num4 = 138;
						num5 = 3;
					}
					else if (num6 == num2)
					{
						num4 = 6;
						num5 = 3;
					}
					else
					{
						num4 = 7;
						num5 = 4;
					}
				}
			}
		}

		// Token: 0x06000026 RID: 38 RVA: 0x00003013 File Offset: 0x00001213
		internal void put_byte(byte[] p, int start, int len)
		{
			Array.Copy(p, start, this.pending_buf, this.pending, len);
			this.pending += len;
		}

		// Token: 0x06000027 RID: 39 RVA: 0x00003038 File Offset: 0x00001238
		internal void put_byte(byte c)
		{
			byte[] array = this.pending_buf;
			int num = this.pending;
			this.pending = num + 1;
			array[num] = c;
		}

		// Token: 0x06000028 RID: 40 RVA: 0x0000305E File Offset: 0x0000125E
		internal void put_short(int w)
		{
			this.put_byte((byte)w);
			this.put_byte((byte)SupportClass.URShift(w, 8));
		}

		// Token: 0x06000029 RID: 41 RVA: 0x00003076 File Offset: 0x00001276
		internal void putShortMSB(int b)
		{
			this.put_byte((byte)(b >> 8));
			this.put_byte((byte)b);
		}

		// Token: 0x0600002A RID: 42 RVA: 0x0000308A File Offset: 0x0000128A
		internal void send_code(int c, short[] tree)
		{
			this.send_bits((int)tree[c * 2] & 65535, (int)tree[c * 2 + 1] & 65535);
		}

		// Token: 0x0600002B RID: 43 RVA: 0x000030AC File Offset: 0x000012AC
		internal void send_bits(int value_Renamed, int length)
		{
			if (this.bi_valid > 16 - length)
			{
				this.bi_buf = (short)((ushort)this.bi_buf | (ushort)(value_Renamed << this.bi_valid & 65535));
				this.put_short((int)this.bi_buf);
				this.bi_buf = (short)SupportClass.URShift(value_Renamed, 16 - this.bi_valid);
				this.bi_valid += length - 16;
				return;
			}
			this.bi_buf = (short)((ushort)this.bi_buf | (ushort)(value_Renamed << this.bi_valid & 65535));
			this.bi_valid += length;
		}

		// Token: 0x0600002C RID: 44 RVA: 0x0000314C File Offset: 0x0000134C
		internal void _tr_align()
		{
			this.send_bits(2, 3);
			this.send_code(256, StaticTree.static_ltree);
			this.bi_flush();
			if (1 + this.last_eob_len + 10 - this.bi_valid < 9)
			{
				this.send_bits(2, 3);
				this.send_code(256, StaticTree.static_ltree);
				this.bi_flush();
			}
			this.last_eob_len = 7;
		}

		// Token: 0x0600002D RID: 45 RVA: 0x000031B4 File Offset: 0x000013B4
		internal bool _tr_tally(int dist, int lc)
		{
			this.pending_buf[this.d_buf + this.last_lit * 2] = (byte)SupportClass.URShift(dist, 8);
			this.pending_buf[this.d_buf + this.last_lit * 2 + 1] = (byte)dist;
			this.pending_buf[this.l_buf + this.last_lit] = (byte)lc;
			this.last_lit++;
			if (dist == 0)
			{
				short[] array = this.dyn_ltree;
				int num = lc * 2;
				int num2 = num;
				array[num2] += 1;
			}
			else
			{
				this.matches++;
				dist--;
				short[] array2 = this.dyn_ltree;
				int num3 = ((int)Tree._length_code[lc] + 256 + 1) * 2;
				int num4 = num3;
				array2[num4] += 1;
				short[] array3 = this.dyn_dtree;
				int num5 = Tree.d_code(dist) * 2;
				int num6 = num5;
				array3[num6] += 1;
			}
			if ((this.last_lit & 8191) == 0 && this.level > 2)
			{
				int num7 = this.last_lit * 8;
				int num8 = this.strstart - this.block_start;
				for (int i = 0; i < 30; i++)
				{
					num7 = (int)((long)num7 + (long)this.dyn_dtree[i * 2] * (5L + (long)Tree.extra_dbits[i]));
				}
				num7 = SupportClass.URShift(num7, 3);
				if (this.matches < this.last_lit / 2 && num7 < num8 / 2)
				{
					return true;
				}
			}
			return this.last_lit == this.lit_bufsize - 1;
		}

		// Token: 0x0600002E RID: 46 RVA: 0x0000331C File Offset: 0x0000151C
		internal void compress_block(short[] ltree, short[] dtree)
		{
			int num = 0;
			if (this.last_lit != 0)
			{
				do
				{
					int num2 = ((int)this.pending_buf[this.d_buf + num * 2] << 8 & 65280) | (int)(this.pending_buf[this.d_buf + num * 2 + 1] & byte.MaxValue);
					int num3 = (int)(this.pending_buf[this.l_buf + num] & byte.MaxValue);
					num++;
					if (num2 == 0)
					{
						this.send_code(num3, ltree);
					}
					else
					{
						int num4 = (int)Tree._length_code[num3];
						this.send_code(num4 + 256 + 1, ltree);
						int num5 = Tree.extra_lbits[num4];
						if (num5 != 0)
						{
							num3 -= Tree.base_length[num4];
							this.send_bits(num3, num5);
						}
						num2--;
						num4 = Tree.d_code(num2);
						this.send_code(num4, dtree);
						num5 = Tree.extra_dbits[num4];
						if (num5 != 0)
						{
							num2 -= Tree.base_dist[num4];
							this.send_bits(num2, num5);
						}
					}
				}
				while (num < this.last_lit);
			}
			this.send_code(256, ltree);
			this.last_eob_len = (int)ltree[513];
		}

		// Token: 0x0600002F RID: 47 RVA: 0x00003424 File Offset: 0x00001624
		internal void set_data_type()
		{
			int i = 0;
			int num = 0;
			int num2 = 0;
			while (i < 7)
			{
				num2 += (int)this.dyn_ltree[i * 2];
				i++;
			}
			while (i < 128)
			{
				num += (int)this.dyn_ltree[i * 2];
				i++;
			}
			while (i < 256)
			{
				num2 += (int)this.dyn_ltree[i * 2];
				i++;
			}
			this.data_type = ((num2 > SupportClass.URShift(num, 2)) ? 0 : 1);
		}

		// Token: 0x06000030 RID: 48 RVA: 0x0000349C File Offset: 0x0000169C
		internal void bi_flush()
		{
			if (this.bi_valid == 16)
			{
				this.put_short((int)this.bi_buf);
				this.bi_buf = 0;
				this.bi_valid = 0;
				return;
			}
			if (this.bi_valid >= 8)
			{
				this.put_byte((byte)this.bi_buf);
				this.bi_buf = (short)SupportClass.URShift((int)this.bi_buf, 8);
				this.bi_valid -= 8;
			}
		}

		// Token: 0x06000031 RID: 49 RVA: 0x00003505 File Offset: 0x00001705
		internal void bi_windup()
		{
			if (this.bi_valid > 8)
			{
				this.put_short((int)this.bi_buf);
			}
			else if (this.bi_valid > 0)
			{
				this.put_byte((byte)this.bi_buf);
			}
			this.bi_buf = 0;
			this.bi_valid = 0;
		}

		// Token: 0x06000032 RID: 50 RVA: 0x00003542 File Offset: 0x00001742
		internal void copy_block(int buf, int len, bool header)
		{
			this.bi_windup();
			this.last_eob_len = 8;
			if (header)
			{
				this.put_short((int)((short)len));
				this.put_short((int)(~(short)len));
			}
			this.put_byte(this.window, buf, len);
		}

		// Token: 0x06000033 RID: 51 RVA: 0x00003574 File Offset: 0x00001774
		internal void flush_block_only(bool eof)
		{
			this._tr_flush_block((this.block_start >= 0) ? this.block_start : -1, this.strstart - this.block_start, eof);
			this.block_start = this.strstart;
			this.strm.flush_pending();
		}

		// Token: 0x06000034 RID: 52 RVA: 0x000035B4 File Offset: 0x000017B4
		internal int deflate_stored(int flush)
		{
			int num = 65535;
			if (num > this.pending_buf_size - 5)
			{
				num = this.pending_buf_size - 5;
			}
			for (;;)
			{
				if (this.lookahead <= 1)
				{
					this.fill_window();
					if (this.lookahead == 0 && flush == 0)
					{
						return 0;
					}
					if (this.lookahead == 0)
					{
						goto IL_D8;
					}
				}
				this.strstart += this.lookahead;
				this.lookahead = 0;
				int num2 = this.block_start + num;
				if (this.strstart == 0 || this.strstart >= num2)
				{
					this.lookahead = this.strstart - num2;
					this.strstart = num2;
					this.flush_block_only(false);
					if (this.strm.avail_out == 0)
					{
						return 0;
					}
				}
				if (this.strstart - this.block_start >= this.w_size - Deflate.MIN_LOOKAHEAD)
				{
					this.flush_block_only(false);
					if (this.strm.avail_out == 0)
					{
						break;
					}
				}
			}
			return 0;
			IL_D8:
			this.flush_block_only(flush == 4);
			if (this.strm.avail_out == 0)
			{
				if (flush != 4)
				{
					return 0;
				}
				return 2;
			}
			else
			{
				if (flush != 4)
				{
					return 1;
				}
				return 3;
			}
			return 0;
		}

		// Token: 0x06000035 RID: 53 RVA: 0x000036C3 File Offset: 0x000018C3
		internal void _tr_stored_block(int buf, int stored_len, bool eof)
		{
			this.send_bits(eof ? 1 : 0, 3);
			this.copy_block(buf, stored_len, true);
		}

		// Token: 0x06000036 RID: 54 RVA: 0x000036DC File Offset: 0x000018DC
		internal void _tr_flush_block(int buf, int stored_len, bool eof)
		{
			int num = 0;
			int num2;
			int num3;
			if (this.level > 0)
			{
				if (this.data_type == 2)
				{
					this.set_data_type();
				}
				this.l_desc.build_tree(this);
				this.d_desc.build_tree(this);
				num = this.build_bl_tree();
				num2 = SupportClass.URShift(this.opt_len + 3 + 7, 3);
				num3 = SupportClass.URShift(this.static_len + 3 + 7, 3);
				if (num3 <= num2)
				{
					num2 = num3;
				}
			}
			else
			{
				num2 = (num3 = stored_len + 5);
			}
			if (stored_len + 4 <= num2 && buf != -1)
			{
				this._tr_stored_block(buf, stored_len, eof);
			}
			else if (num3 == num2)
			{
				this.send_bits(2 + (eof ? 1 : 0), 3);
				this.compress_block(StaticTree.static_ltree, StaticTree.static_dtree);
			}
			else
			{
				this.send_bits(4 + (eof ? 1 : 0), 3);
				this.send_all_trees(this.l_desc.max_code + 1, this.d_desc.max_code + 1, num + 1);
				this.compress_block(this.dyn_ltree, this.dyn_dtree);
			}
			this.init_block();
			if (eof)
			{
				this.bi_windup();
			}
		}

		// Token: 0x06000037 RID: 55 RVA: 0x000037E4 File Offset: 0x000019E4
		internal void fill_window()
		{
			do
			{
				int num = this.window_size - this.lookahead - this.strstart;
				int num2;
				if (num == 0 && this.strstart == 0 && this.lookahead == 0)
				{
					num = this.w_size;
				}
				else if (num == -1)
				{
					num--;
				}
				else if (this.strstart >= this.w_size + this.w_size - Deflate.MIN_LOOKAHEAD)
				{
					Array.Copy(this.window, this.w_size, this.window, 0, this.w_size);
					this.match_start -= this.w_size;
					this.strstart -= this.w_size;
					this.block_start -= this.w_size;
					num2 = this.hash_size;
					int num3 = num2;
					do
					{
						int num4 = (int)this.head[--num3] & 65535;
						this.head[num3] = (short)((num4 >= this.w_size) ? (num4 - this.w_size) : 0);
					}
					while (--num2 != 0);
					num2 = this.w_size;
					num3 = num2;
					do
					{
						int num5 = (int)this.prev[--num3] & 65535;
						this.prev[num3] = (short)((num5 >= this.w_size) ? (num5 - this.w_size) : 0);
					}
					while (--num2 != 0);
					num += this.w_size;
				}
				if (this.strm.avail_in == 0)
				{
					break;
				}
				num2 = this.strm.read_buf(this.window, this.strstart + this.lookahead, num);
				this.lookahead += num2;
				if (this.lookahead >= 3)
				{
					this.ins_h = (int)(this.window[this.strstart] & byte.MaxValue);
					this.ins_h = ((this.ins_h << this.hash_shift ^ (int)(this.window[this.strstart + 1] & byte.MaxValue)) & this.hash_mask);
				}
			}
			while (this.lookahead < Deflate.MIN_LOOKAHEAD && this.strm.avail_in != 0);
		}

		// Token: 0x06000038 RID: 56 RVA: 0x000039E4 File Offset: 0x00001BE4
		internal int deflate_fast(int flush)
		{
			int num = 0;
			for (;;)
			{
				if (this.lookahead < Deflate.MIN_LOOKAHEAD)
				{
					this.fill_window();
					if (this.lookahead < Deflate.MIN_LOOKAHEAD && flush == 0)
					{
						return 0;
					}
					if (this.lookahead == 0)
					{
						goto IL_2C7;
					}
				}
				if (this.lookahead >= 3)
				{
					this.ins_h = ((this.ins_h << this.hash_shift ^ (int)(this.window[this.strstart + 2] & byte.MaxValue)) & this.hash_mask);
					num = ((int)this.head[this.ins_h] & 65535);
					this.prev[this.strstart & this.w_mask] = this.head[this.ins_h];
					this.head[this.ins_h] = (short)this.strstart;
				}
				if ((long)num != 0L && (this.strstart - num & 65535) <= this.w_size - Deflate.MIN_LOOKAHEAD && this.strategy != 2)
				{
					this.match_length = this.longest_match(num);
				}
				bool flag;
				if (this.match_length >= 3)
				{
					flag = this._tr_tally(this.strstart - this.match_start, this.match_length - 3);
					this.lookahead -= this.match_length;
					if (this.match_length <= this.max_lazy_match && this.lookahead >= 3)
					{
						this.match_length--;
						int num2;
						do
						{
							this.strstart++;
							this.ins_h = ((this.ins_h << this.hash_shift ^ (int)(this.window[this.strstart + 2] & byte.MaxValue)) & this.hash_mask);
							num = ((int)this.head[this.ins_h] & 65535);
							this.prev[this.strstart & this.w_mask] = this.head[this.ins_h];
							this.head[this.ins_h] = (short)this.strstart;
							num2 = this.match_length - 1;
							this.match_length = num2;
						}
						while (num2 != 0);
						this.strstart++;
					}
					else
					{
						this.strstart += this.match_length;
						this.match_length = 0;
						this.ins_h = (int)(this.window[this.strstart] & byte.MaxValue);
						this.ins_h = ((this.ins_h << this.hash_shift ^ (int)(this.window[this.strstart + 1] & byte.MaxValue)) & this.hash_mask);
					}
				}
				else
				{
					flag = this._tr_tally(0, (int)(this.window[this.strstart] & byte.MaxValue));
					this.lookahead--;
					this.strstart++;
				}
				if (flag)
				{
					this.flush_block_only(false);
					if (this.strm.avail_out == 0)
					{
						break;
					}
				}
			}
			return 0;
			IL_2C7:
			this.flush_block_only(flush == 4);
			if (this.strm.avail_out != 0)
			{
				if (flush != 4)
				{
					return 1;
				}
				return 3;
			}
			else
			{
				if (flush == 4)
				{
					return 2;
				}
				return 0;
			}
		}

		// Token: 0x06000039 RID: 57 RVA: 0x00003CE0 File Offset: 0x00001EE0
		internal int deflate_slow(int flush)
		{
			int num = 0;
			for (;;)
			{
				if (this.lookahead < Deflate.MIN_LOOKAHEAD)
				{
					this.fill_window();
					if (this.lookahead < Deflate.MIN_LOOKAHEAD && flush == 0)
					{
						return 0;
					}
					if (this.lookahead == 0)
					{
						goto IL_327;
					}
				}
				if (this.lookahead >= 3)
				{
					this.ins_h = ((this.ins_h << this.hash_shift ^ (int)(this.window[this.strstart + 2] & byte.MaxValue)) & this.hash_mask);
					num = ((int)this.head[this.ins_h] & 65535);
					this.prev[this.strstart & this.w_mask] = this.head[this.ins_h];
					this.head[this.ins_h] = (short)this.strstart;
				}
				this.prev_length = this.match_length;
				this.prev_match = this.match_start;
				this.match_length = 2;
				if (num != 0 && this.prev_length < this.max_lazy_match && (this.strstart - num & 65535) <= this.w_size - Deflate.MIN_LOOKAHEAD)
				{
					if (this.strategy != 2)
					{
						this.match_length = this.longest_match(num);
					}
					if (this.match_length <= 5 && (this.strategy == 1 || (this.match_length == 3 && this.strstart - this.match_start > 4096)))
					{
						this.match_length = 2;
					}
				}
				if (this.prev_length >= 3 && this.match_length <= this.prev_length)
				{
					int num2 = this.strstart + this.lookahead - 3;
					bool flag = this._tr_tally(this.strstart - 1 - this.prev_match, this.prev_length - 3);
					this.lookahead -= this.prev_length - 1;
					this.prev_length -= 2;
					int num3;
					do
					{
						num3 = this.strstart + 1;
						this.strstart = num3;
						if (num3 <= num2)
						{
							this.ins_h = ((this.ins_h << this.hash_shift ^ (int)(this.window[this.strstart + 2] & byte.MaxValue)) & this.hash_mask);
							num = ((int)this.head[this.ins_h] & 65535);
							this.prev[this.strstart & this.w_mask] = this.head[this.ins_h];
							this.head[this.ins_h] = (short)this.strstart;
						}
						num3 = this.prev_length - 1;
						this.prev_length = num3;
					}
					while (num3 != 0);
					this.match_available = 0;
					this.match_length = 2;
					this.strstart++;
					if (flag)
					{
						this.flush_block_only(false);
						if (this.strm.avail_out == 0)
						{
							break;
						}
					}
				}
				else if (this.match_available != 0)
				{
					if (this._tr_tally(0, (int)(this.window[this.strstart - 1] & 255)))
					{
						this.flush_block_only(false);
					}
					this.strstart++;
					this.lookahead--;
					if (this.strm.avail_out == 0)
					{
						return 0;
					}
				}
				else
				{
					this.match_available = 1;
					this.strstart++;
					this.lookahead--;
				}
			}
			return 0;
			IL_327:
			if (this.match_available != 0)
			{
				this._tr_tally(0, (int)(this.window[this.strstart - 1] & byte.MaxValue));
				this.match_available = 0;
			}
			this.flush_block_only(flush == 4);
			if (this.strm.avail_out != 0)
			{
				if (flush != 4)
				{
					return 1;
				}
				return 3;
			}
			else
			{
				if (flush == 4)
				{
					return 2;
				}
				return 0;
			}
		}

		// Token: 0x0600003A RID: 58 RVA: 0x0000406C File Offset: 0x0000226C
		internal int longest_match(int cur_match)
		{
			int num = this.max_chain_length;
			int num2 = this.strstart;
			int num3 = this.prev_length;
			int num4 = (this.strstart > this.w_size - Deflate.MIN_LOOKAHEAD) ? (this.strstart - (this.w_size - Deflate.MIN_LOOKAHEAD)) : 0;
			int num5 = this.nice_match;
			int num6 = this.w_mask;
			int num7 = this.strstart + 258;
			byte b = this.window[num2 + num3 - 1];
			byte b2 = this.window[num2 + num3];
			if (this.prev_length >= this.good_match)
			{
				num >>= 2;
			}
			if (num5 > this.lookahead)
			{
				num5 = this.lookahead;
			}
			do
			{
				int num8 = cur_match;
				if (this.window[num8 + num3] == b2 && this.window[num8 + num3 - 1] == b && this.window[num8] == this.window[num2] && this.window[++num8] == this.window[num2 + 1])
				{
					num2 += 2;
					num8++;
					while (this.window[++num2] == this.window[++num8] && this.window[++num2] == this.window[++num8] && this.window[++num2] == this.window[++num8] && this.window[++num2] == this.window[++num8] && this.window[++num2] == this.window[++num8] && this.window[++num2] == this.window[++num8] && this.window[++num2] == this.window[++num8] && this.window[++num2] == this.window[++num8] && num2 < num7)
					{
					}
					int num9 = 258 - (num7 - num2);
					num2 = num7 - 258;
					if (num9 > num3)
					{
						this.match_start = cur_match;
						num3 = num9;
						if (num9 >= num5)
						{
							break;
						}
						b = this.window[num2 + num3 - 1];
						b2 = this.window[num2 + num3];
					}
				}
			}
			while ((cur_match = ((int)this.prev[cur_match & num6] & 65535)) > num4 && --num != 0);
			int result;
			if (num3 <= this.lookahead)
			{
				result = num3;
			}
			else
			{
				result = this.lookahead;
			}
			return result;
		}

		// Token: 0x0600003B RID: 59 RVA: 0x000042E9 File Offset: 0x000024E9
		internal int deflateInit(ZStream strm, int level, int bits)
		{
			return this.deflateInit2(strm, level, 8, bits, 8, 0);
		}

		// Token: 0x0600003C RID: 60 RVA: 0x000042F7 File Offset: 0x000024F7
		internal int deflateInit(ZStream strm, int level)
		{
			return this.deflateInit(strm, level, 15);
		}

		// Token: 0x0600003D RID: 61 RVA: 0x00004304 File Offset: 0x00002504
		internal int deflateInit2(ZStream strm, int level, int method, int windowBits, int memLevel, int strategy)
		{
			int num = 0;
			strm.msg = null;
			if (level == -1)
			{
				level = 6;
			}
			if (windowBits < 0)
			{
				num = 1;
				windowBits = -windowBits;
			}
			int result;
			if (memLevel < 1 || memLevel > 9 || method != 8 || windowBits < 9 || windowBits > 15 || level < 0 || level > 9 || strategy < 0 || strategy > 2)
			{
				result = -2;
			}
			else
			{
				strm.dstate = this;
				this.noheader = num;
				this.w_bits = windowBits;
				this.w_size = 1 << this.w_bits;
				this.w_mask = this.w_size - 1;
				this.hash_bits = memLevel + 7;
				this.hash_size = 1 << this.hash_bits;
				this.hash_mask = this.hash_size - 1;
				this.hash_shift = (this.hash_bits + 3 - 1) / 3;
				this.window = new byte[this.w_size * 2];
				this.prev = new short[this.w_size];
				this.head = new short[this.hash_size];
				this.lit_bufsize = 1 << memLevel + 6;
				this.pending_buf = new byte[this.lit_bufsize * 4];
				this.pending_buf_size = this.lit_bufsize * 4;
				this.d_buf = this.lit_bufsize;
				this.l_buf = 3 * this.lit_bufsize;
				this.level = level;
				this.strategy = strategy;
				this.method = (byte)method;
				result = this.deflateReset(strm);
			}
			return result;
		}

		// Token: 0x0600003E RID: 62 RVA: 0x00004474 File Offset: 0x00002674
		internal int deflateReset(ZStream strm)
		{
			strm.total_in = (strm.total_out = 0L);
			strm.msg = null;
			strm.data_type = 2;
			this.pending = 0;
			this.pending_out = 0;
			if (this.noheader < 0)
			{
				this.noheader = 0;
			}
			this.status = ((this.noheader != 0) ? 113 : 42);
			strm.adler = strm._adler.adler32(0L, null, 0, 0);
			this.last_flush = 0;
			this.tr_init();
			this.lm_init();
			return 0;
		}

		// Token: 0x0600003F RID: 63 RVA: 0x000044FC File Offset: 0x000026FC
		internal int deflateEnd()
		{
			int result;
			if (this.status != 42 && this.status != 113 && this.status != 666)
			{
				result = -2;
			}
			else
			{
				this.pending_buf = null;
				this.head = null;
				this.prev = null;
				this.window = null;
				result = ((this.status == 113) ? -3 : 0);
			}
			return result;
		}

		// Token: 0x06000040 RID: 64 RVA: 0x0000455C File Offset: 0x0000275C
		internal int deflateParams(ZStream strm, int _level, int _strategy)
		{
			int num = 0;
			if (_level == -1)
			{
				_level = 6;
			}
			int result;
			if (_level < 0 || _level > 9 || _strategy < 0 || _strategy > 2)
			{
				result = -2;
			}
			else
			{
				if (Deflate.config_table[this.level].func != Deflate.config_table[_level].func && strm.total_in != 0L)
				{
					num = strm.deflate(1);
				}
				if (this.level != _level)
				{
					this.level = _level;
					this.max_lazy_match = Deflate.config_table[this.level].max_lazy;
					this.good_match = Deflate.config_table[this.level].good_length;
					this.nice_match = Deflate.config_table[this.level].nice_length;
					this.max_chain_length = Deflate.config_table[this.level].max_chain;
				}
				this.strategy = _strategy;
				result = num;
			}
			return result;
		}

		// Token: 0x06000041 RID: 65 RVA: 0x00004630 File Offset: 0x00002830
		internal int deflateSetDictionary(ZStream strm, byte[] dictionary, int dictLength)
		{
			int num = dictLength;
			int sourceIndex = 0;
			int result;
			if (dictionary == null || this.status != 42)
			{
				result = -2;
			}
			else
			{
				strm.adler = strm._adler.adler32(strm.adler, dictionary, 0, dictLength);
				if (num < 3)
				{
					result = 0;
				}
				else
				{
					if (num > this.w_size - Deflate.MIN_LOOKAHEAD)
					{
						num = this.w_size - Deflate.MIN_LOOKAHEAD;
						sourceIndex = dictLength - num;
					}
					Array.Copy(dictionary, sourceIndex, this.window, 0, num);
					this.strstart = num;
					this.block_start = num;
					this.ins_h = (int)(this.window[0] & byte.MaxValue);
					this.ins_h = ((this.ins_h << this.hash_shift ^ (int)(this.window[1] & byte.MaxValue)) & this.hash_mask);
					for (int i = 0; i <= num - 3; i++)
					{
						this.ins_h = ((this.ins_h << this.hash_shift ^ (int)(this.window[i + 2] & byte.MaxValue)) & this.hash_mask);
						this.prev[i & this.w_mask] = this.head[this.ins_h];
						this.head[this.ins_h] = (short)i;
					}
					result = 0;
				}
			}
			return result;
		}

		// Token: 0x06000042 RID: 66 RVA: 0x00004764 File Offset: 0x00002964
		internal int deflate(ZStream strm, int flush)
		{
			int result;
			if (flush > 4 || flush < 0)
			{
				result = -2;
			}
			else if (strm.next_out == null || (strm.next_in == null && strm.avail_in != 0) || (this.status == 666 && flush != 4))
			{
				strm.msg = Deflate.z_errmsg[4];
				result = -2;
			}
			else if (strm.avail_out == 0)
			{
				strm.msg = Deflate.z_errmsg[7];
				result = -5;
			}
			else
			{
				this.strm = strm;
				int num = this.last_flush;
				this.last_flush = flush;
				if (this.status == 42)
				{
					int num2 = 8 + (this.w_bits - 8 << 4) << 8;
					int num3 = (this.level - 1 & 255) >> 1;
					if (num3 > 3)
					{
						num3 = 3;
					}
					num2 |= num3 << 6;
					if (this.strstart != 0)
					{
						num2 |= 32;
					}
					num2 += 31 - num2 % 31;
					this.status = 113;
					this.putShortMSB(num2);
					if (this.strstart != 0)
					{
						this.putShortMSB((int)SupportClass.URShift(strm.adler, 16));
						this.putShortMSB((int)(strm.adler & 65535L));
					}
					strm.adler = strm._adler.adler32(0L, null, 0, 0);
				}
				if (this.pending != 0)
				{
					strm.flush_pending();
					if (strm.avail_out == 0)
					{
						this.last_flush = -1;
						return 0;
					}
				}
				else if (strm.avail_in == 0 && flush <= num && flush != 4)
				{
					strm.msg = Deflate.z_errmsg[7];
					return -5;
				}
				if (this.status == 666 && strm.avail_in != 0)
				{
					strm.msg = Deflate.z_errmsg[7];
					result = -5;
				}
				else
				{
					if (strm.avail_in != 0 || this.lookahead != 0 || (flush != 0 && this.status != 666))
					{
						int num4 = -1;
						switch (Deflate.config_table[this.level].func)
						{
						case 0:
							num4 = this.deflate_stored(flush);
							break;
						case 1:
							num4 = this.deflate_fast(flush);
							break;
						case 2:
							num4 = this.deflate_slow(flush);
							break;
						}
						if (num4 == 2 || num4 == 3)
						{
							this.status = 666;
						}
						if (num4 == 0 || num4 == 2)
						{
							if (strm.avail_out == 0)
							{
								this.last_flush = -1;
							}
							return 0;
						}
						if (num4 == 1)
						{
							if (flush == 1)
							{
								this._tr_align();
							}
							else
							{
								this._tr_stored_block(0, 0, false);
								if (flush == 3)
								{
									for (int i = 0; i < this.hash_size; i++)
									{
										this.head[i] = 0;
									}
								}
							}
							strm.flush_pending();
							if (strm.avail_out == 0)
							{
								this.last_flush = -1;
								return 0;
							}
						}
					}
					if (flush != 4)
					{
						result = 0;
					}
					else if (this.noheader != 0)
					{
						result = 1;
					}
					else
					{
						this.putShortMSB((int)SupportClass.URShift(strm.adler, 16));
						this.putShortMSB((int)(strm.adler & 65535L));
						strm.flush_pending();
						this.noheader = -1;
						result = ((this.pending != 0) ? 0 : 1);
					}
				}
			}
			return result;
		}

		// Token: 0x06000043 RID: 67 RVA: 0x00004A54 File Offset: 0x00002C54
		static Deflate()
		{
			Deflate.config_table[0] = new Deflate.Config(0, 0, 0, 0, 0);
			Deflate.config_table[1] = new Deflate.Config(4, 4, 8, 4, 1);
			Deflate.config_table[2] = new Deflate.Config(4, 5, 16, 8, 1);
			Deflate.config_table[3] = new Deflate.Config(4, 6, 32, 32, 1);
			Deflate.config_table[4] = new Deflate.Config(4, 4, 16, 16, 2);
			Deflate.config_table[5] = new Deflate.Config(8, 16, 32, 32, 2);
			Deflate.config_table[6] = new Deflate.Config(8, 16, 128, 128, 2);
			Deflate.config_table[7] = new Deflate.Config(8, 32, 128, 256, 2);
			Deflate.config_table[8] = new Deflate.Config(32, 128, 258, 1024, 2);
			Deflate.config_table[9] = new Deflate.Config(32, 258, 258, 4096, 2);
		}

		// Token: 0x04000007 RID: 7
		private const int MAX_MEM_LEVEL = 9;

		// Token: 0x04000008 RID: 8
		private const int Z_DEFAULT_COMPRESSION = -1;

		// Token: 0x04000009 RID: 9
		private const int MAX_WBITS = 15;

		// Token: 0x0400000A RID: 10
		private const int DEF_MEM_LEVEL = 8;

		// Token: 0x0400000B RID: 11
		private const int STORED = 0;

		// Token: 0x0400000C RID: 12
		private const int FAST = 1;

		// Token: 0x0400000D RID: 13
		private const int SLOW = 2;

		// Token: 0x0400000E RID: 14
		private const int NeedMore = 0;

		// Token: 0x0400000F RID: 15
		private const int BlockDone = 1;

		// Token: 0x04000010 RID: 16
		private const int FinishStarted = 2;

		// Token: 0x04000011 RID: 17
		private const int FinishDone = 3;

		// Token: 0x04000012 RID: 18
		private const int PRESET_DICT = 32;

		// Token: 0x04000013 RID: 19
		private const int Z_FILTERED = 1;

		// Token: 0x04000014 RID: 20
		private const int Z_HUFFMAN_ONLY = 2;

		// Token: 0x04000015 RID: 21
		private const int Z_DEFAULT_STRATEGY = 0;

		// Token: 0x04000016 RID: 22
		private const int Z_NO_FLUSH = 0;

		// Token: 0x04000017 RID: 23
		private const int Z_PARTIAL_FLUSH = 1;

		// Token: 0x04000018 RID: 24
		private const int Z_SYNC_FLUSH = 2;

		// Token: 0x04000019 RID: 25
		private const int Z_FULL_FLUSH = 3;

		// Token: 0x0400001A RID: 26
		private const int Z_FINISH = 4;

		// Token: 0x0400001B RID: 27
		private const int Z_OK = 0;

		// Token: 0x0400001C RID: 28
		private const int Z_STREAM_END = 1;

		// Token: 0x0400001D RID: 29
		private const int Z_NEED_DICT = 2;

		// Token: 0x0400001E RID: 30
		private const int Z_ERRNO = -1;

		// Token: 0x0400001F RID: 31
		private const int Z_STREAM_ERROR = -2;

		// Token: 0x04000020 RID: 32
		private const int Z_DATA_ERROR = -3;

		// Token: 0x04000021 RID: 33
		private const int Z_MEM_ERROR = -4;

		// Token: 0x04000022 RID: 34
		private const int Z_BUF_ERROR = -5;

		// Token: 0x04000023 RID: 35
		private const int Z_VERSION_ERROR = -6;

		// Token: 0x04000024 RID: 36
		private const int INIT_STATE = 42;

		// Token: 0x04000025 RID: 37
		private const int BUSY_STATE = 113;

		// Token: 0x04000026 RID: 38
		private const int FINISH_STATE = 666;

		// Token: 0x04000027 RID: 39
		private const int Z_DEFLATED = 8;

		// Token: 0x04000028 RID: 40
		private const int STORED_BLOCK = 0;

		// Token: 0x04000029 RID: 41
		private const int STATIC_TREES = 1;

		// Token: 0x0400002A RID: 42
		private const int DYN_TREES = 2;

		// Token: 0x0400002B RID: 43
		private const int Z_BINARY = 0;

		// Token: 0x0400002C RID: 44
		private const int Z_ASCII = 1;

		// Token: 0x0400002D RID: 45
		private const int Z_UNKNOWN = 2;

		// Token: 0x0400002E RID: 46
		private const int Buf_size = 16;

		// Token: 0x0400002F RID: 47
		private const int REP_3_6 = 16;

		// Token: 0x04000030 RID: 48
		private const int REPZ_3_10 = 17;

		// Token: 0x04000031 RID: 49
		private const int REPZ_11_138 = 18;

		// Token: 0x04000032 RID: 50
		private const int MIN_MATCH = 3;

		// Token: 0x04000033 RID: 51
		private const int MAX_MATCH = 258;

		// Token: 0x04000034 RID: 52
		private const int MAX_BITS = 15;

		// Token: 0x04000035 RID: 53
		private const int D_CODES = 30;

		// Token: 0x04000036 RID: 54
		private const int BL_CODES = 19;

		// Token: 0x04000037 RID: 55
		private const int LENGTH_CODES = 29;

		// Token: 0x04000038 RID: 56
		private const int LITERALS = 256;

		// Token: 0x04000039 RID: 57
		private const int END_BLOCK = 256;

		// Token: 0x0400003A RID: 58
		private static Deflate.Config[] config_table = new Deflate.Config[10];

		// Token: 0x0400003B RID: 59
		private static readonly string[] z_errmsg = new string[]
		{
			"need dictionary",
			"stream end",
			"",
			"file error",
			"stream error",
			"data error",
			"insufficient memory",
			"buffer error",
			"incompatible version",
			""
		};

		// Token: 0x0400003C RID: 60
		private static readonly int MIN_LOOKAHEAD = 262;

		// Token: 0x0400003D RID: 61
		private static readonly int L_CODES = 286;

		// Token: 0x0400003E RID: 62
		private static readonly int HEAP_SIZE = 2 * Deflate.L_CODES + 1;

		// Token: 0x0400003F RID: 63
		internal ZStream strm;

		// Token: 0x04000040 RID: 64
		internal int status;

		// Token: 0x04000041 RID: 65
		internal byte[] pending_buf;

		// Token: 0x04000042 RID: 66
		internal int pending_buf_size;

		// Token: 0x04000043 RID: 67
		internal int pending_out;

		// Token: 0x04000044 RID: 68
		internal int pending;

		// Token: 0x04000045 RID: 69
		internal int noheader;

		// Token: 0x04000046 RID: 70
		internal byte data_type;

		// Token: 0x04000047 RID: 71
		internal byte method;

		// Token: 0x04000048 RID: 72
		internal int last_flush;

		// Token: 0x04000049 RID: 73
		internal int w_size;

		// Token: 0x0400004A RID: 74
		internal int w_bits;

		// Token: 0x0400004B RID: 75
		internal int w_mask;

		// Token: 0x0400004C RID: 76
		internal byte[] window;

		// Token: 0x0400004D RID: 77
		internal int window_size;

		// Token: 0x0400004E RID: 78
		internal short[] prev;

		// Token: 0x0400004F RID: 79
		internal short[] head;

		// Token: 0x04000050 RID: 80
		internal int ins_h;

		// Token: 0x04000051 RID: 81
		internal int hash_size;

		// Token: 0x04000052 RID: 82
		internal int hash_bits;

		// Token: 0x04000053 RID: 83
		internal int hash_mask;

		// Token: 0x04000054 RID: 84
		internal int hash_shift;

		// Token: 0x04000055 RID: 85
		internal int block_start;

		// Token: 0x04000056 RID: 86
		internal int match_length;

		// Token: 0x04000057 RID: 87
		internal int prev_match;

		// Token: 0x04000058 RID: 88
		internal int match_available;

		// Token: 0x04000059 RID: 89
		internal int strstart;

		// Token: 0x0400005A RID: 90
		internal int match_start;

		// Token: 0x0400005B RID: 91
		internal int lookahead;

		// Token: 0x0400005C RID: 92
		internal int prev_length;

		// Token: 0x0400005D RID: 93
		internal int max_chain_length;

		// Token: 0x0400005E RID: 94
		internal int max_lazy_match;

		// Token: 0x0400005F RID: 95
		internal int level;

		// Token: 0x04000060 RID: 96
		internal int strategy;

		// Token: 0x04000061 RID: 97
		internal int good_match;

		// Token: 0x04000062 RID: 98
		internal int nice_match;

		// Token: 0x04000063 RID: 99
		internal short[] dyn_ltree;

		// Token: 0x04000064 RID: 100
		internal short[] dyn_dtree;

		// Token: 0x04000065 RID: 101
		internal short[] bl_tree;

		// Token: 0x04000066 RID: 102
		internal Tree l_desc = new Tree();

		// Token: 0x04000067 RID: 103
		internal Tree d_desc = new Tree();

		// Token: 0x04000068 RID: 104
		internal Tree bl_desc = new Tree();

		// Token: 0x04000069 RID: 105
		internal short[] bl_count = new short[16];

		// Token: 0x0400006A RID: 106
		internal int[] heap = new int[2 * Deflate.L_CODES + 1];

		// Token: 0x0400006B RID: 107
		internal int heap_len;

		// Token: 0x0400006C RID: 108
		internal int heap_max;

		// Token: 0x0400006D RID: 109
		internal byte[] depth = new byte[2 * Deflate.L_CODES + 1];

		// Token: 0x0400006E RID: 110
		internal int l_buf;

		// Token: 0x0400006F RID: 111
		internal int lit_bufsize;

		// Token: 0x04000070 RID: 112
		internal int last_lit;

		// Token: 0x04000071 RID: 113
		internal int d_buf;

		// Token: 0x04000072 RID: 114
		internal int opt_len;

		// Token: 0x04000073 RID: 115
		internal int static_len;

		// Token: 0x04000074 RID: 116
		internal int matches;

		// Token: 0x04000075 RID: 117
		internal int last_eob_len;

		// Token: 0x04000076 RID: 118
		internal short bi_buf;

		// Token: 0x04000077 RID: 119
		internal int bi_valid;

		// Token: 0x0200014D RID: 333
		internal class Config
		{
			// Token: 0x0600111D RID: 4381 RVA: 0x00076FD4 File Offset: 0x000751D4
			internal Config(int good_length, int max_lazy, int nice_length, int max_chain, int func)
			{
				this.good_length = good_length;
				this.max_lazy = max_lazy;
				this.nice_length = nice_length;
				this.max_chain = max_chain;
				this.func = func;
			}

			// Token: 0x04000DE5 RID: 3557
			internal int good_length;

			// Token: 0x04000DE6 RID: 3558
			internal int max_lazy;

			// Token: 0x04000DE7 RID: 3559
			internal int nice_length;

			// Token: 0x04000DE8 RID: 3560
			internal int max_chain;

			// Token: 0x04000DE9 RID: 3561
			internal int func;
		}
	}
}
