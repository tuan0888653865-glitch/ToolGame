using System;
using System.IO;

namespace ComponentAce.Compression.Libs.zlib
{
	// Token: 0x02000010 RID: 16
	public class ZInputStream : BinaryReader
	{
		// Token: 0x06000077 RID: 119 RVA: 0x000085A7 File Offset: 0x000067A7
		internal void InitBlock()
		{
			this.flush = 0;
			this.buf = new byte[this.bufsize];
		}

		// Token: 0x17000001 RID: 1
		// (get) Token: 0x06000078 RID: 120 RVA: 0x000085C1 File Offset: 0x000067C1
		// (set) Token: 0x06000079 RID: 121 RVA: 0x000085C9 File Offset: 0x000067C9
		public virtual int FlushMode
		{
			get
			{
				return this.flush;
			}
			set
			{
				this.flush = value;
			}
		}

		// Token: 0x17000002 RID: 2
		// (get) Token: 0x0600007A RID: 122 RVA: 0x000085D2 File Offset: 0x000067D2
		public virtual long TotalIn
		{
			get
			{
				return this.z.total_in;
			}
		}

		// Token: 0x17000003 RID: 3
		// (get) Token: 0x0600007B RID: 123 RVA: 0x000085DF File Offset: 0x000067DF
		public virtual long TotalOut
		{
			get
			{
				return this.z.total_out;
			}
		}

		// Token: 0x0600007C RID: 124 RVA: 0x000085EC File Offset: 0x000067EC
		public ZInputStream(Stream in_Renamed) : base(in_Renamed)
		{
			this.InitBlock();
			this.in_Renamed = in_Renamed;
			this.z.inflateInit();
			this.compress = false;
			this.z.next_in = this.buf;
			this.z.next_in_index = 0;
			this.z.avail_in = 0;
		}

		// Token: 0x0600007D RID: 125 RVA: 0x0000866C File Offset: 0x0000686C
		public ZInputStream(Stream in_Renamed, int level) : base(in_Renamed)
		{
			this.InitBlock();
			this.in_Renamed = in_Renamed;
			this.z.deflateInit(level);
			this.compress = true;
			this.z.next_in = this.buf;
			this.z.next_in_index = 0;
			this.z.avail_in = 0;
		}

		// Token: 0x0600007E RID: 126 RVA: 0x000086EC File Offset: 0x000068EC
		public override int Read()
		{
			int result;
			if (this.read(this.buf1, 0, 1) == -1)
			{
				result = -1;
			}
			else
			{
				result = (int)(this.buf1[0] & byte.MaxValue);
			}
			return result;
		}

		// Token: 0x0600007F RID: 127 RVA: 0x00008720 File Offset: 0x00006920
		public int read(byte[] b, int off, int len)
		{
			int result;
			if (len == 0)
			{
				result = 0;
			}
			else
			{
				this.z.next_out = b;
				this.z.next_out_index = off;
				this.z.avail_out = len;
				int num;
				do
				{
					if (this.z.avail_in == 0 && !this.nomoreinput)
					{
						this.z.next_in_index = 0;
						this.z.avail_in = SupportClass.ReadInput(this.in_Renamed, this.buf, 0, this.bufsize);
						if (this.z.avail_in == -1)
						{
							this.z.avail_in = 0;
							this.nomoreinput = true;
						}
					}
					if (this.compress)
					{
						num = this.z.deflate(this.flush);
					}
					else
					{
						num = this.z.inflate(this.flush);
					}
					if (this.nomoreinput && num == -5)
					{
						return -1;
					}
					if (num != 0 && num != 1)
					{
						goto IL_105;
					}
					if (this.nomoreinput && this.z.avail_out == len)
					{
						return -1;
					}
					if (this.z.avail_out != len)
					{
						break;
					}
				}
				while (num == 0);
				goto IL_138;
				IL_105:
				throw new ZStreamException((this.compress ? "de" : "in") + "flating: " + this.z.msg);
				IL_138:
				result = len - this.z.avail_out;
			}
			return result;
		}

		// Token: 0x06000080 RID: 128 RVA: 0x00008874 File Offset: 0x00006A74
		public long skip(long n)
		{
			int num = 512;
			if (n < (long)num)
			{
				num = (int)n;
			}
			byte[] array = new byte[num];
			return (long)SupportClass.ReadInput(this.BaseStream, array, 0, array.Length);
		}

		// Token: 0x06000081 RID: 129 RVA: 0x000088A7 File Offset: 0x00006AA7
		public override void Close()
		{
			this.in_Renamed.Close();
		}

		// Token: 0x04000127 RID: 295
		protected ZStream z = new ZStream();

		// Token: 0x04000128 RID: 296
		protected int bufsize = 512;

		// Token: 0x04000129 RID: 297
		protected int flush;

		// Token: 0x0400012A RID: 298
		protected byte[] buf;

		// Token: 0x0400012B RID: 299
		protected byte[] buf1 = new byte[1];

		// Token: 0x0400012C RID: 300
		protected bool compress;

		// Token: 0x0400012D RID: 301
		internal Stream in_Renamed;

		// Token: 0x0400012E RID: 302
		internal bool nomoreinput;
	}
}
