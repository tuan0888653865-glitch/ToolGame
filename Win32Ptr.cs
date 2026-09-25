using System;

namespace TinhKiemAuto
{
	// Token: 0x020000F6 RID: 246
	public static class Win32Ptr
	{
		// Token: 0x06000CE3 RID: 3299 RVA: 0x00053177 File Offset: 0x00051377
		public static IntPtr Add(this IntPtr ptr, long val)
		{
			return new IntPtr(ptr.ToInt32() + (int)val);
		}

		// Token: 0x06000CE4 RID: 3300 RVA: 0x00053188 File Offset: 0x00051388
		public static IntPtr Add(this IntPtr ptr, IntPtr val)
		{
			return new IntPtr(ptr.ToInt32() + val.ToInt32());
		}

		// Token: 0x06000CE5 RID: 3301 RVA: 0x0005319E File Offset: 0x0005139E
		public static bool Compare(this IntPtr ptr, long value)
		{
			return ptr.ToInt64() == value;
		}

		// Token: 0x06000CE6 RID: 3302 RVA: 0x000531AA File Offset: 0x000513AA
		public static IntPtr Create(long value)
		{
			return new IntPtr((int)value);
		}

		// Token: 0x06000CE7 RID: 3303 RVA: 0x000531B3 File Offset: 0x000513B3
		public static bool IsNull(this IntPtr ptr)
		{
			return ptr == IntPtr.Zero;
		}

		// Token: 0x06000CE8 RID: 3304 RVA: 0x000531C0 File Offset: 0x000513C0
		public static bool IsNull(this UIntPtr ptr)
		{
			return ptr == UIntPtr.Zero;
		}

		// Token: 0x06000CE9 RID: 3305 RVA: 0x000531CD File Offset: 0x000513CD
		public static IntPtr Subtract(this IntPtr ptr, long val)
		{
			return new IntPtr((int)(ptr.ToInt64() - val));
		}

		// Token: 0x06000CEA RID: 3306 RVA: 0x000531DE File Offset: 0x000513DE
		public static IntPtr Subtract(this IntPtr ptr, IntPtr val)
		{
			return new IntPtr((int)(ptr.ToInt64() - val.ToInt64()));
		}
	}
}
