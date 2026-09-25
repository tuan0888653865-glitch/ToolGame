using System;
using System.IO;

namespace ProtoBuf
{
	// Token: 0x02000017 RID: 23
	public sealed class BufferExtension : IExtension
	{
		// Token: 0x060000BC RID: 188 RVA: 0x00009953 File Offset: 0x00007B53
		int IExtension.GetLength()
		{
			if (this.buffer != null)
			{
				return this.buffer.Length;
			}
			return 0;
		}

		// Token: 0x060000BD RID: 189 RVA: 0x00009967 File Offset: 0x00007B67
		Stream IExtension.BeginAppend()
		{
			return new MemoryStream();
		}

		// Token: 0x060000BE RID: 190 RVA: 0x00009970 File Offset: 0x00007B70
		void IExtension.EndAppend(Stream stream, bool commit)
		{
			try
			{
				int num;
				if (commit && (num = (int)stream.Length) > 0)
				{
					MemoryStream memoryStream = (MemoryStream)stream;
					if (this.buffer == null)
					{
						this.buffer = memoryStream.ToArray();
					}
					else
					{
						int num2 = this.buffer.Length;
						byte[] to = new byte[num2 + num];
						Helpers.BlockCopy(this.buffer, 0, to, 0, num2);
						Helpers.BlockCopy(memoryStream.GetBuffer(), 0, to, num2, num);
						this.buffer = to;
					}
				}
			}
			finally
			{
				if (stream != null)
				{
					((IDisposable)stream).Dispose();
				}
			}
		}

		// Token: 0x060000BF RID: 191 RVA: 0x000099FC File Offset: 0x00007BFC
		Stream IExtension.BeginQuery()
		{
			if (this.buffer != null)
			{
				return new MemoryStream(this.buffer);
			}
			return Stream.Null;
		}

		// Token: 0x060000C0 RID: 192 RVA: 0x00009A18 File Offset: 0x00007C18
		void IExtension.EndQuery(Stream stream)
		{
			try
			{
			}
			finally
			{
				if (stream != null)
				{
					((IDisposable)stream).Dispose();
				}
			}
		}

		// Token: 0x04000179 RID: 377
		private byte[] buffer;
	}
}
