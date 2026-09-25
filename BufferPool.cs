using System;
using System.Threading;

namespace ProtoBuf
{
	// Token: 0x02000018 RID: 24
	internal class BufferPool
	{
		// Token: 0x060000C2 RID: 194 RVA: 0x00009A44 File Offset: 0x00007C44
		internal static void Flush()
		{
			for (int i = 0; i < BufferPool.pool.Length; i++)
			{
				Interlocked.Exchange(ref BufferPool.pool[i], null);
			}
		}

		// Token: 0x060000C3 RID: 195 RVA: 0x000020C5 File Offset: 0x000002C5
		private BufferPool()
		{
		}

		// Token: 0x060000C4 RID: 196 RVA: 0x00009A78 File Offset: 0x00007C78
		internal static byte[] GetBuffer()
		{
			for (int i = 0; i < BufferPool.pool.Length; i++)
			{
				object obj;
				if ((obj = Interlocked.Exchange(ref BufferPool.pool[i], null)) != null)
				{
					return (byte[])obj;
				}
			}
			return new byte[1024];
		}

		// Token: 0x060000C5 RID: 197 RVA: 0x00009AC0 File Offset: 0x00007CC0
		internal static void ResizeAndFlushLeft(ref byte[] buffer, int toFitAtLeastBytes, int copyFromIndex, int copyBytes)
		{
			int num = buffer.Length * 2;
			if (num < toFitAtLeastBytes)
			{
				num = toFitAtLeastBytes;
			}
			byte[] array = new byte[num];
			if (copyBytes > 0)
			{
				Helpers.BlockCopy(buffer, copyFromIndex, array, 0, copyBytes);
			}
			if (buffer.Length == 1024)
			{
				BufferPool.ReleaseBufferToPool(ref buffer);
			}
			buffer = array;
		}

		// Token: 0x060000C6 RID: 198 RVA: 0x00009B04 File Offset: 0x00007D04
		internal static void ReleaseBufferToPool(ref byte[] buffer)
		{
			if (buffer == null)
			{
				return;
			}
			if (buffer.Length == 1024)
			{
				int num = 0;
				while (num < BufferPool.pool.Length && Interlocked.CompareExchange(ref BufferPool.pool[num], buffer, null) != null)
				{
					num++;
				}
			}
			buffer = null;
		}

		// Token: 0x0400017A RID: 378
		private const int PoolSize = 20;

		// Token: 0x0400017B RID: 379
		internal const int BufferLength = 1024;

		// Token: 0x0400017C RID: 380
		private static readonly object[] pool = new object[20];
	}
}
