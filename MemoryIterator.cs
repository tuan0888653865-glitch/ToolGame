using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;

namespace TinhKiemAuto
{
	// Token: 0x020000BA RID: 186
	[Serializable]
	public class MemoryIterator : ErrorBase, IDisposable
	{
		// Token: 0x06000A61 RID: 2657 RVA: 0x00042FB0 File Offset: 0x000411B0
		public MemoryIterator(byte[] iterable)
		{
			if (iterable == null)
			{
				throw new ArgumentException("Unable to iterate a null reference", "iterable");
			}
			this._base = new MemoryStream(iterable, 0, iterable.Length, true);
			this._ubuffer = new UnmanagedBuffer(256);
		}

		// Token: 0x06000A62 RID: 2658 RVA: 0x00042FEC File Offset: 0x000411EC
		public void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		// Token: 0x06000A63 RID: 2659 RVA: 0x00042FFB File Offset: 0x000411FB
		protected virtual void Dispose(bool disposing)
		{
			if (!this._disposed)
			{
				if (disposing)
				{
					this._ubuffer.Dispose();
					this._base.Dispose();
				}
				this._disposed = true;
			}
		}

		// Token: 0x06000A64 RID: 2660 RVA: 0x00043025 File Offset: 0x00041225
		protected byte[] GetUnderlyingData()
		{
			return this._base.ToArray();
		}

		// Token: 0x06000A65 RID: 2661 RVA: 0x00043032 File Offset: 0x00041232
		public bool Read<TResult>(out TResult result) where TResult : struct
		{
			return this.Read<TResult>(0L, SeekOrigin.Current, out result);
		}

		// Token: 0x06000A66 RID: 2662 RVA: 0x00043040 File Offset: 0x00041240
		public bool Read(long offset, SeekOrigin origin, byte[] buffer)
		{
			if (buffer == null)
			{
				throw new ArgumentNullException("buffer", "Parameter cannot be null");
			}
			try
			{
				this._base.Seek(offset, origin);
				this._base.Read(buffer, 0, buffer.Length);
			}
			catch (Exception lastError)
			{
				this.SetLastError(lastError);
				buffer = null;
			}
			return buffer != null;
		}

		// Token: 0x06000A67 RID: 2663 RVA: 0x000430A4 File Offset: 0x000412A4
		public bool Read<TResult>(long offset, SeekOrigin origin, out TResult result) where TResult : struct
		{
			result = default(TResult);
			bool result2;
			try
			{
				this._base.Seek(offset, origin);
				byte[] array = new byte[Marshal.SizeOf(typeof(TResult))];
				this._base.Read(array, 0, array.Length);
				if (!this._ubuffer.Translate<TResult>(array, out result))
				{
					throw this._ubuffer.GetLastError();
				}
				result2 = true;
			}
			catch (Exception lastError)
			{
				result2 = base.SetLastError(lastError);
			}
			return result2;
		}

		// Token: 0x06000A68 RID: 2664 RVA: 0x00043128 File Offset: 0x00041328
		public bool ReadString(long offset, SeekOrigin origin, out string lpBuffer, int len = -1, Encoding stringEncoding = null)
		{
			lpBuffer = null;
			byte[] array = new byte[(len > 0) ? len : 64];
			if (stringEncoding == null)
			{
				stringEncoding = Encoding.ASCII;
			}
			bool result;
			try
			{
				this._base.Seek(offset, origin);
				StringBuilder stringBuilder = new StringBuilder((len > 0) ? len : 260);
				int num = -1;
				int num2 = 0;
				int num3;
				while (num == -1 && (num3 = this._base.Read(array, 0, array.Length)) > 0)
				{
					stringBuilder.Append(stringEncoding.GetString(array));
					num = stringBuilder.ToString().IndexOf('\0', num2);
					num2 += num3;
					if (len > 0 && num2 >= len)
					{
						break;
					}
				}
				if (num > -1)
				{
					lpBuffer = stringBuilder.ToString().Substring(0, num);
				}
				else if (num2 >= len && len > 0)
				{
					lpBuffer = stringBuilder.ToString().Substring(0, len);
				}
				result = (lpBuffer != null);
			}
			catch (Exception lastError)
			{
				result = this.SetLastError(lastError);
			}
			return result;
		}

		// Token: 0x06000A69 RID: 2665 RVA: 0x00043220 File Offset: 0x00041420
		public long Seek(long offset, SeekOrigin origin)
		{
			return this._base.Seek(offset, origin);
		}

		// Token: 0x06000A6A RID: 2666 RVA: 0x00043230 File Offset: 0x00041430
		public bool Write(long offset, SeekOrigin origin, byte[] data)
		{
			if (data == null)
			{
				throw new ArgumentNullException("Parameter 'data' cannot be null");
			}
			bool result;
			try
			{
				this._base.Seek(offset, origin);
				this._base.Write(data, 0, data.Length);
				result = true;
			}
			catch (Exception lastError)
			{
				result = this.SetLastError(lastError);
			}
			return result;
		}

		// Token: 0x06000A6B RID: 2667 RVA: 0x0004328C File Offset: 0x0004148C
		public bool Write<TSource>(long offset, SeekOrigin origin, TSource data) where TSource : struct
		{
			bool result;
			try
			{
				this._base.Seek(offset, origin);
				byte[] array = null;
				if (!this._ubuffer.Translate<TSource>(data, out array))
				{
					throw this._ubuffer.GetLastError();
				}
				this._base.Write(array, 0, array.Length);
				result = true;
			}
			catch (Exception lastError)
			{
				result = this.SetLastError(lastError);
			}
			return result;
		}

		// Token: 0x040007AA RID: 1962
		private MemoryStream _base;

		// Token: 0x040007AB RID: 1963
		private bool _disposed;

		// Token: 0x040007AC RID: 1964
		private UnmanagedBuffer _ubuffer;
	}
}
