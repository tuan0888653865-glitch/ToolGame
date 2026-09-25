using System;

namespace ComponentAce.Compression.Libs.zlib
{
	// Token: 0x02000013 RID: 19
	public sealed class ZStream
	{
		// Token: 0x0600009B RID: 155 RVA: 0x00008CA4 File Offset: 0x00006EA4
		public int inflateInit()
		{
			return this.inflateInit(ZStream.DEF_WBITS);
		}

		// Token: 0x0600009C RID: 156 RVA: 0x00008CB1 File Offset: 0x00006EB1
		public int inflateInit(int w)
		{
			this.istate = new Inflate();
			return this.istate.inflateInit(this, w);
		}

		// Token: 0x0600009D RID: 157 RVA: 0x00008CCC File Offset: 0x00006ECC
		public int inflate(int f)
		{
			int result;
			if (this.istate == null)
			{
				result = -2;
			}
			else
			{
				result = this.istate.inflate(this, f);
			}
			return result;
		}

		// Token: 0x0600009E RID: 158 RVA: 0x00008CF8 File Offset: 0x00006EF8
		public int inflateEnd()
		{
			int result;
			if (this.istate == null)
			{
				result = -2;
			}
			else
			{
				int num = this.istate.inflateEnd(this);
				this.istate = null;
				result = num;
			}
			return result;
		}

		// Token: 0x0600009F RID: 159 RVA: 0x00008D28 File Offset: 0x00006F28
		public int inflateSync()
		{
			int result;
			if (this.istate == null)
			{
				result = -2;
			}
			else
			{
				result = this.istate.inflateSync(this);
			}
			return result;
		}

		// Token: 0x060000A0 RID: 160 RVA: 0x00008D50 File Offset: 0x00006F50
		public int inflateSetDictionary(byte[] dictionary, int dictLength)
		{
			int result;
			if (this.istate == null)
			{
				result = -2;
			}
			else
			{
				result = this.istate.inflateSetDictionary(this, dictionary, dictLength);
			}
			return result;
		}

		// Token: 0x060000A1 RID: 161 RVA: 0x00008D7A File Offset: 0x00006F7A
		public int deflateInit(int level)
		{
			return this.deflateInit(level, 15);
		}

		// Token: 0x060000A2 RID: 162 RVA: 0x00008D85 File Offset: 0x00006F85
		public int deflateInit(int level, int bits)
		{
			this.dstate = new Deflate();
			return this.dstate.deflateInit(this, level, bits);
		}

		// Token: 0x060000A3 RID: 163 RVA: 0x00008DA0 File Offset: 0x00006FA0
		public int deflate(int flush)
		{
			int result;
			if (this.dstate == null)
			{
				result = -2;
			}
			else
			{
				result = this.dstate.deflate(this, flush);
			}
			return result;
		}

		// Token: 0x060000A4 RID: 164 RVA: 0x00008DCC File Offset: 0x00006FCC
		public int deflateEnd()
		{
			int result;
			if (this.dstate == null)
			{
				result = -2;
			}
			else
			{
				int num = this.dstate.deflateEnd();
				this.dstate = null;
				result = num;
			}
			return result;
		}

		// Token: 0x060000A5 RID: 165 RVA: 0x00008DFC File Offset: 0x00006FFC
		public int deflateParams(int level, int strategy)
		{
			int result;
			if (this.dstate == null)
			{
				result = -2;
			}
			else
			{
				result = this.dstate.deflateParams(this, level, strategy);
			}
			return result;
		}

		// Token: 0x060000A6 RID: 166 RVA: 0x00008E28 File Offset: 0x00007028
		public int deflateSetDictionary(byte[] dictionary, int dictLength)
		{
			int result;
			if (this.dstate == null)
			{
				result = -2;
			}
			else
			{
				result = this.dstate.deflateSetDictionary(this, dictionary, dictLength);
			}
			return result;
		}

		// Token: 0x060000A7 RID: 167 RVA: 0x00008E54 File Offset: 0x00007054
		internal void flush_pending()
		{
			int pending = this.dstate.pending;
			if (pending > this.avail_out)
			{
				pending = this.avail_out;
			}
			if (pending != 0)
			{
				if (this.dstate.pending_buf.Length > this.dstate.pending_out && this.next_out.Length > this.next_out_index && this.dstate.pending_buf.Length >= this.dstate.pending_out + pending)
				{
					int num = this.next_out.Length;
					int num2 = this.next_out_index + pending;
				}
				Array.Copy(this.dstate.pending_buf, this.dstate.pending_out, this.next_out, this.next_out_index, pending);
				this.next_out_index += pending;
				this.dstate.pending_out += pending;
				this.total_out += (long)pending;
				this.avail_out -= pending;
				this.dstate.pending -= pending;
				if (this.dstate.pending == 0)
				{
					this.dstate.pending_out = 0;
				}
			}
		}

		// Token: 0x060000A8 RID: 168 RVA: 0x00008F70 File Offset: 0x00007170
		internal int read_buf(byte[] buf, int start, int size)
		{
			int num = this.avail_in;
			if (num > size)
			{
				num = size;
			}
			int result;
			if (num == 0)
			{
				result = 0;
			}
			else
			{
				this.avail_in -= num;
				if (this.dstate.noheader == 0)
				{
					this.adler = this._adler.adler32(this.adler, this.next_in, this.next_in_index, num);
				}
				Array.Copy(this.next_in, this.next_in_index, buf, start, num);
				this.next_in_index += num;
				this.total_in += (long)num;
				result = num;
			}
			return result;
		}

		// Token: 0x060000A9 RID: 169 RVA: 0x00009004 File Offset: 0x00007204
		public void free()
		{
			this.next_in = null;
			this.next_out = null;
			this.msg = null;
			this._adler = null;
		}

		// Token: 0x0400014C RID: 332
		private const int MAX_WBITS = 15;

		// Token: 0x0400014D RID: 333
		private const int Z_NO_FLUSH = 0;

		// Token: 0x0400014E RID: 334
		private const int Z_PARTIAL_FLUSH = 1;

		// Token: 0x0400014F RID: 335
		private const int Z_SYNC_FLUSH = 2;

		// Token: 0x04000150 RID: 336
		private const int Z_FULL_FLUSH = 3;

		// Token: 0x04000151 RID: 337
		private const int Z_FINISH = 4;

		// Token: 0x04000152 RID: 338
		private const int MAX_MEM_LEVEL = 9;

		// Token: 0x04000153 RID: 339
		private const int Z_OK = 0;

		// Token: 0x04000154 RID: 340
		private const int Z_STREAM_END = 1;

		// Token: 0x04000155 RID: 341
		private const int Z_NEED_DICT = 2;

		// Token: 0x04000156 RID: 342
		private const int Z_ERRNO = -1;

		// Token: 0x04000157 RID: 343
		private const int Z_STREAM_ERROR = -2;

		// Token: 0x04000158 RID: 344
		private const int Z_DATA_ERROR = -3;

		// Token: 0x04000159 RID: 345
		private const int Z_MEM_ERROR = -4;

		// Token: 0x0400015A RID: 346
		private const int Z_BUF_ERROR = -5;

		// Token: 0x0400015B RID: 347
		private const int Z_VERSION_ERROR = -6;

		// Token: 0x0400015C RID: 348
		private static readonly int DEF_WBITS = 15;

		// Token: 0x0400015D RID: 349
		public byte[] next_in;

		// Token: 0x0400015E RID: 350
		public int next_in_index;

		// Token: 0x0400015F RID: 351
		public int avail_in;

		// Token: 0x04000160 RID: 352
		public long total_in;

		// Token: 0x04000161 RID: 353
		public byte[] next_out;

		// Token: 0x04000162 RID: 354
		public int next_out_index;

		// Token: 0x04000163 RID: 355
		public int avail_out;

		// Token: 0x04000164 RID: 356
		public long total_out;

		// Token: 0x04000165 RID: 357
		public string msg;

		// Token: 0x04000166 RID: 358
		internal Deflate dstate;

		// Token: 0x04000167 RID: 359
		internal Inflate istate;

		// Token: 0x04000168 RID: 360
		internal int data_type;

		// Token: 0x04000169 RID: 361
		public long adler;

		// Token: 0x0400016A RID: 362
		internal Adler32 _adler = new Adler32();
	}
}
