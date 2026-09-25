using System;
using System.IO;

namespace ComponentAce.Compression.Libs.zlib
{
	// Token: 0x02000012 RID: 18
	public class ZOutputStream : Stream
	{
		// Token: 0x06000084 RID: 132 RVA: 0x000088BB File Offset: 0x00006ABB
		private void InitBlock()
		{
			this.flush_Renamed_Field = 0;
			this.buf = new byte[this.bufsize];
		}

		// Token: 0x17000004 RID: 4
		// (get) Token: 0x06000085 RID: 133 RVA: 0x000088D5 File Offset: 0x00006AD5
		// (set) Token: 0x06000086 RID: 134 RVA: 0x000088DD File Offset: 0x00006ADD
		public virtual int FlushMode
		{
			get
			{
				return this.flush_Renamed_Field;
			}
			set
			{
				this.flush_Renamed_Field = value;
			}
		}

		// Token: 0x17000005 RID: 5
		// (get) Token: 0x06000087 RID: 135 RVA: 0x000088E6 File Offset: 0x00006AE6
		public virtual long TotalIn
		{
			get
			{
				return this.z.total_in;
			}
		}

		// Token: 0x17000006 RID: 6
		// (get) Token: 0x06000088 RID: 136 RVA: 0x000088F3 File Offset: 0x00006AF3
		public virtual long TotalOut
		{
			get
			{
				return this.z.total_out;
			}
		}

		// Token: 0x06000089 RID: 137 RVA: 0x00008900 File Offset: 0x00006B00
		public ZOutputStream(Stream out_Renamed)
		{
			this.InitBlock();
			this.out_Renamed = out_Renamed;
			this.z.inflateInit();
			this.compress = false;
		}

		// Token: 0x0600008A RID: 138 RVA: 0x00008958 File Offset: 0x00006B58
		public ZOutputStream(Stream out_Renamed, int level)
		{
			this.InitBlock();
			this.out_Renamed = out_Renamed;
			this.z.deflateInit(level);
			this.compress = true;
		}

		// Token: 0x0600008B RID: 139 RVA: 0x000089AE File Offset: 0x00006BAE
		public void WriteByte(int b)
		{
			this.buf1[0] = (byte)b;
			this.Write(this.buf1, 0, 1);
		}

		// Token: 0x0600008C RID: 140 RVA: 0x000089C8 File Offset: 0x00006BC8
		public override void WriteByte(byte b)
		{
			this.WriteByte((int)b);
		}

		// Token: 0x0600008D RID: 141 RVA: 0x000089D4 File Offset: 0x00006BD4
		public override void Write(byte[] b1, int off, int len)
		{
			if (len != 0)
			{
				byte[] array = new byte[b1.Length];
				Array.Copy(b1, 0, array, 0, b1.Length);
				this.z.next_in = array;
				this.z.next_in_index = off;
				this.z.avail_in = len;
				do
				{
					this.z.next_out = this.buf;
					this.z.next_out_index = 0;
					this.z.avail_out = this.bufsize;
					int num;
					if (this.compress)
					{
						num = this.z.deflate(this.flush_Renamed_Field);
					}
					else
					{
						num = this.z.inflate(this.flush_Renamed_Field);
					}
					if (num != 0 && num != 1)
					{
						goto IL_E8;
					}
					this.out_Renamed.Write(this.buf, 0, this.bufsize - this.z.avail_out);
				}
				while (this.z.avail_in > 0 || this.z.avail_out == 0);
				return;
				IL_E8:
				throw new ZStreamException((this.compress ? "de" : "in") + "flating: " + this.z.msg);
			}
		}

		// Token: 0x0600008E RID: 142 RVA: 0x00008AF8 File Offset: 0x00006CF8
		public virtual void finish()
		{
			do
			{
				this.z.next_out = this.buf;
				this.z.next_out_index = 0;
				this.z.avail_out = this.bufsize;
				int num;
				if (this.compress)
				{
					num = this.z.deflate(4);
				}
				else
				{
					num = this.z.inflate(4);
				}
				if (num != 1 && num != 0)
				{
					goto IL_B5;
				}
				if (this.bufsize - this.z.avail_out > 0)
				{
					this.out_Renamed.Write(this.buf, 0, this.bufsize - this.z.avail_out);
				}
			}
			while (this.z.avail_in > 0 || this.z.avail_out == 0);
			try
			{
				this.Flush();
			}
			catch
			{
			}
			return;
			IL_B5:
			throw new ZStreamException((this.compress ? "de" : "in") + "flating: " + this.z.msg);
		}

		// Token: 0x0600008F RID: 143 RVA: 0x00008C08 File Offset: 0x00006E08
		public virtual void end()
		{
			if (this.compress)
			{
				this.z.deflateEnd();
			}
			else
			{
				this.z.inflateEnd();
			}
			this.z.free();
			this.z = null;
		}

		// Token: 0x06000090 RID: 144 RVA: 0x00008C40 File Offset: 0x00006E40
		public override void Close()
		{
			try
			{
				this.finish();
			}
			catch
			{
			}
			finally
			{
				this.end();
				this.out_Renamed.Close();
				this.out_Renamed = null;
			}
		}

		// Token: 0x06000091 RID: 145 RVA: 0x00008C90 File Offset: 0x00006E90
		public override void Flush()
		{
			this.out_Renamed.Flush();
		}

		// Token: 0x06000092 RID: 146 RVA: 0x00008C9D File Offset: 0x00006E9D
		public override int Read(byte[] buffer, int offset, int count)
		{
			return 0;
		}

		// Token: 0x06000093 RID: 147 RVA: 0x00006740 File Offset: 0x00004940
		public override void SetLength(long value)
		{
		}

		// Token: 0x06000094 RID: 148 RVA: 0x00008CA0 File Offset: 0x00006EA0
		public override long Seek(long offset, SeekOrigin origin)
		{
			return 0L;
		}

		// Token: 0x17000007 RID: 7
		// (get) Token: 0x06000095 RID: 149 RVA: 0x00008C9D File Offset: 0x00006E9D
		public override bool CanRead
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000008 RID: 8
		// (get) Token: 0x06000096 RID: 150 RVA: 0x00008C9D File Offset: 0x00006E9D
		public override bool CanSeek
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000009 RID: 9
		// (get) Token: 0x06000097 RID: 151 RVA: 0x00008C9D File Offset: 0x00006E9D
		public override bool CanWrite
		{
			get
			{
				return false;
			}
		}

		// Token: 0x1700000A RID: 10
		// (get) Token: 0x06000098 RID: 152 RVA: 0x00008CA0 File Offset: 0x00006EA0
		public override long Length
		{
			get
			{
				return 0L;
			}
		}

		// Token: 0x1700000B RID: 11
		// (get) Token: 0x06000099 RID: 153 RVA: 0x00008CA0 File Offset: 0x00006EA0
		// (set) Token: 0x0600009A RID: 154 RVA: 0x00006740 File Offset: 0x00004940
		public override long Position
		{
			get
			{
				return 0L;
			}
			set
			{
			}
		}

		// Token: 0x04000145 RID: 325
		protected internal ZStream z = new ZStream();

		// Token: 0x04000146 RID: 326
		protected internal int bufsize = 4096;

		// Token: 0x04000147 RID: 327
		protected internal int flush_Renamed_Field;

		// Token: 0x04000148 RID: 328
		protected internal byte[] buf;

		// Token: 0x04000149 RID: 329
		protected internal byte[] buf1 = new byte[1];

		// Token: 0x0400014A RID: 330
		protected internal bool compress;

		// Token: 0x0400014B RID: 331
		private Stream out_Renamed;
	}
}
