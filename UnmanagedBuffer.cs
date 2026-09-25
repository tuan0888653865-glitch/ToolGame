using System;
using System.Runtime.InteropServices;

namespace TinhKiemAuto
{
	// Token: 0x020000EF RID: 239
	[Serializable]
	public class UnmanagedBuffer : ErrorBase, IDisposable
	{
		// Token: 0x06000C91 RID: 3217 RVA: 0x00051E87 File Offset: 0x00050087
		public UnmanagedBuffer(int cbneeded)
		{
			if (cbneeded > 0)
			{
				this.Pointer = Marshal.AllocHGlobal(cbneeded);
				this.Size = cbneeded;
				return;
			}
			this.Pointer = IntPtr.Zero;
			this.Size = 0;
		}

		// Token: 0x06000C92 RID: 3218 RVA: 0x00051EBC File Offset: 0x000500BC
		private bool Alloc(int cb)
		{
			bool result;
			try
			{
				if (cb > this.Size)
				{
					this.Pointer = ((this.Pointer == IntPtr.Zero) ? Marshal.AllocHGlobal(cb) : Marshal.ReAllocHGlobal(this.Pointer, new IntPtr(cb)));
					this.Size = cb;
				}
				result = true;
			}
			catch (Exception lastError)
			{
				result = this.SetLastError(lastError);
			}
			return result;
		}

		// Token: 0x06000C93 RID: 3219 RVA: 0x00051F2C File Offset: 0x0005012C
		public bool Commit<T>(T data) where T : struct
		{
			bool result;
			try
			{
				if (this.Alloc(Marshal.SizeOf(typeof(T))))
				{
					Marshal.StructureToPtr(data, this.Pointer, false);
					result = true;
				}
				else
				{
					result = false;
				}
			}
			catch (Exception lastError)
			{
				result = this.SetLastError(lastError);
			}
			return result;
		}

		// Token: 0x06000C94 RID: 3220 RVA: 0x00051F88 File Offset: 0x00050188
		public bool Commit(byte[] data, int index, int count)
		{
			if (data != null && this.Alloc(count))
			{
				Marshal.Copy(data, index, this.Pointer, count);
				return true;
			}
			if (data == null)
			{
				this.SetLastError(new ArgumentException("Attempting to commit a null reference", "data"));
			}
			return false;
		}

		// Token: 0x06000C95 RID: 3221 RVA: 0x00051FC0 File Offset: 0x000501C0
		public void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		// Token: 0x06000C96 RID: 3222 RVA: 0x00051FCF File Offset: 0x000501CF
		private void Dispose(bool disposing)
		{
			if (!this._disposed)
			{
				if (disposing)
				{
					this.Resize(0);
				}
				this._disposed = true;
			}
		}

		// Token: 0x06000C97 RID: 3223 RVA: 0x00051FEC File Offset: 0x000501EC
		public byte[] Read(int count)
		{
			byte[] result;
			try
			{
				if (count > this.Size || count <= 0)
				{
					throw new ArgumentException("There is either not enough memory allocated to read 'count' bytes, or 'count' is negative (" + count.ToString() + ")", "count");
				}
				byte[] array = new byte[count];
				Marshal.Copy(this.Pointer, array, 0, count);
				result = array;
			}
			catch (Exception lastError)
			{
				this.SetLastError(lastError);
				result = null;
			}
			return result;
		}

		// Token: 0x06000C98 RID: 3224 RVA: 0x00052060 File Offset: 0x00050260
		public bool Read<TResult>(out TResult data) where TResult : struct
		{
			data = default(TResult);
			bool result;
			try
			{
				if (this.Size < Marshal.SizeOf(typeof(TResult)))
				{
					throw new InvalidCastException("Not enough unmanaged memory is allocated to contain this structure type.");
				}
				data = (TResult)((object)Marshal.PtrToStructure(this.Pointer, typeof(TResult)));
				result = true;
			}
			catch (Exception lastError)
			{
				result = this.SetLastError(lastError);
			}
			return result;
		}

		// Token: 0x06000C99 RID: 3225 RVA: 0x000520D8 File Offset: 0x000502D8
		public bool Resize(int size)
		{
			if (size < 0)
			{
				return this.SetLastError(new ArgumentException("Attempting to resize to less than zero bytes of memory", "size"));
			}
			if (size == this.Size)
			{
				return true;
			}
			if (size > this.Size)
			{
				return this.Alloc(size);
			}
			bool result;
			try
			{
				if (size == 0)
				{
					Marshal.FreeHGlobal(this.Pointer);
					this.Pointer = IntPtr.Zero;
				}
				else if (size > 0)
				{
					this.Pointer = Marshal.ReAllocHGlobal(this.Pointer, new IntPtr(size));
				}
				this.Size = size;
				result = true;
			}
			catch (Exception lastError)
			{
				result = this.SetLastError(lastError);
			}
			return result;
		}

		// Token: 0x06000C9A RID: 3226 RVA: 0x0005217C File Offset: 0x0005037C
		public bool SafeDecommit<T>() where T : struct
		{
			bool result;
			try
			{
				if (this.Size < Marshal.SizeOf(typeof(T)))
				{
					throw new InvalidCastException("Not enough unmanaged memory is allocated to contain this structure type.");
				}
				Marshal.DestroyStructure(this.Pointer, typeof(T));
				result = true;
			}
			catch (Exception lastError)
			{
				result = this.SetLastError(lastError);
			}
			return result;
		}

		// Token: 0x06000C9B RID: 3227 RVA: 0x000521E0 File Offset: 0x000503E0
		public bool Translate<TSource>(TSource data, out byte[] buffer) where TSource : struct
		{
			buffer = null;
			if (this.Commit<TSource>(data))
			{
				buffer = this.Read(Marshal.SizeOf(typeof(TSource)));
				this.SafeDecommit<TSource>();
			}
			return buffer != null;
		}

		// Token: 0x06000C9C RID: 3228 RVA: 0x00052211 File Offset: 0x00050411
		public bool Translate<TResult>(byte[] buffer, out TResult result) where TResult : struct
		{
			result = default(TResult);
			if (buffer == null)
			{
				return this.SetLastError(new ArgumentException("Attempted to translate a null reference to a structure.", "buffer"));
			}
			return this.Commit(buffer, 0, buffer.Length) && this.Read<TResult>(out result);
		}

		// Token: 0x06000C9D RID: 3229 RVA: 0x00052249 File Offset: 0x00050449
		public bool Translate<TSource, TResult>(TSource data, out TResult result) where TSource : struct where TResult : struct
		{
			result = default(TResult);
			return this.Commit<TSource>(data) && this.Read<TResult>(out result) && this.SafeDecommit<TSource>();
		}

		// Token: 0x17000310 RID: 784
		// (get) Token: 0x06000C9E RID: 3230 RVA: 0x0005226C File Offset: 0x0005046C
		// (set) Token: 0x06000C9F RID: 3231 RVA: 0x00052274 File Offset: 0x00050474
		public IntPtr Pointer { get; private set; }

		// Token: 0x17000311 RID: 785
		// (get) Token: 0x06000CA0 RID: 3232 RVA: 0x0005227D File Offset: 0x0005047D
		// (set) Token: 0x06000CA1 RID: 3233 RVA: 0x00052285 File Offset: 0x00050485
		public int Size { get; private set; }

		// Token: 0x04000968 RID: 2408
		private bool _disposed;
	}
}
