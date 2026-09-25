using System;
using System.Drawing;

namespace TinhKiemAuto
{
	// Token: 0x02000097 RID: 151
	internal class GdiMemoryContext : IDisposable
	{
		// Token: 0x06000967 RID: 2407 RVA: 0x0003F288 File Offset: 0x0003D488
		public GdiMemoryContext(Graphics compatibleTo, int width, int height)
		{
			if (compatibleTo == null || width <= 0 || height <= 0)
			{
				throw new ArgumentException("Arguments are unacceptable");
			}
			IntPtr hdc = compatibleTo.GetHdc();
			bool flag = true;
			if (!((this.fDC = NativeMethods.CreateCompatibleDC(hdc)) == IntPtr.Zero))
			{
				if ((this.fBitmap = NativeMethods.CreateCompatibleBitmap(hdc, width, height)) == IntPtr.Zero)
				{
					NativeMethods.DeleteDC(this.fDC);
				}
				else
				{
					this.fStockMonoBmp = NativeMethods.SelectObject(this.fDC, this.fBitmap);
					if (this.fStockMonoBmp == IntPtr.Zero)
					{
						NativeMethods.DeleteObject(this.fBitmap);
						NativeMethods.DeleteDC(this.fDC);
					}
					else
					{
						flag = false;
					}
				}
			}
			compatibleTo.ReleaseHdc(hdc);
			if (flag)
			{
				throw new SystemException("GDI error occured while creating context");
			}
			this.gdiPlusContext = Graphics.FromHdc(this.fDC);
			this.fWidth = width;
			this.fHeight = height;
		}

		// Token: 0x06000968 RID: 2408 RVA: 0x0003F37C File Offset: 0x0003D57C
		~GdiMemoryContext()
		{
			this.Dispose(false);
		}

		// Token: 0x06000969 RID: 2409 RVA: 0x0003F3AC File Offset: 0x0003D5AC
		protected virtual void Dispose(bool disposing)
		{
			if (disposing && this.gdiPlusContext != null)
			{
				this.gdiPlusContext.Dispose();
			}
			NativeMethods.SelectObject(this.fDC, this.fStockMonoBmp);
			NativeMethods.DeleteDC(this.fDC);
			this.fDC = (this.fStockMonoBmp = IntPtr.Zero);
			NativeMethods.DeleteObject(this.fBitmap);
			this.fBitmap = IntPtr.Zero;
		}

		// Token: 0x0600096A RID: 2410 RVA: 0x0003F418 File Offset: 0x0003D618
		public void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		// Token: 0x17000233 RID: 563
		// (get) Token: 0x0600096B RID: 2411 RVA: 0x0003F427 File Offset: 0x0003D627
		public Graphics Graphics
		{
			get
			{
				return this.gdiPlusContext;
			}
		}

		// Token: 0x0600096C RID: 2412 RVA: 0x0003F430 File Offset: 0x0003D630
		public void FlipVertical()
		{
			if (this.fDC != IntPtr.Zero)
			{
				NativeMethods.StretchBlt(this.fDC, 0, this.fHeight - 1, this.fWidth, -this.fHeight, this.fDC, 0, 0, this.fWidth, this.fHeight, 13369376U);
			}
		}

		// Token: 0x17000234 RID: 564
		// (get) Token: 0x0600096D RID: 2413 RVA: 0x0003F48A File Offset: 0x0003D68A
		public int Width
		{
			get
			{
				return this.fWidth;
			}
		}

		// Token: 0x17000235 RID: 565
		// (get) Token: 0x0600096E RID: 2414 RVA: 0x0003F492 File Offset: 0x0003D692
		public int Height
		{
			get
			{
				return this.fHeight;
			}
		}

		// Token: 0x0600096F RID: 2415 RVA: 0x0003F49A File Offset: 0x0003D69A
		public uint GetPixel(int x, int y)
		{
			if (this.fDC != IntPtr.Zero)
			{
				return NativeMethods.GetPixel(this.fDC, x, y);
			}
			throw new ObjectDisposedException(null, "GDI context seems to be disposed.");
		}

		// Token: 0x06000970 RID: 2416 RVA: 0x0003F4C7 File Offset: 0x0003D6C7
		public void SetPixel(int x, int y, uint value)
		{
			if (this.fDC != IntPtr.Zero)
			{
				NativeMethods.SetPixel(this.fDC, x, y, value);
				return;
			}
			throw new ObjectDisposedException(null, "GDI context seems to be disposed.");
		}

		// Token: 0x06000971 RID: 2417 RVA: 0x0003F4F8 File Offset: 0x0003D6F8
		public void DrawContextClipped(Graphics drawTo, Rectangle drawRect)
		{
			if (drawTo != null && !(this.fDC == IntPtr.Zero))
			{
				IntPtr hdc = drawTo.GetHdc();
				if (!(hdc == IntPtr.Zero))
				{
					NativeMethods.BitBlt(hdc, drawRect.Left, drawRect.Top, drawRect.Width, drawRect.Height, this.fDC, 0, 0, 13369376U);
					drawTo.ReleaseHdc(hdc);
				}
			}
		}

		// Token: 0x04000655 RID: 1621
		private IntPtr fDC;

		// Token: 0x04000656 RID: 1622
		private IntPtr fBitmap;

		// Token: 0x04000657 RID: 1623
		private IntPtr fStockMonoBmp;

		// Token: 0x04000658 RID: 1624
		private int fWidth;

		// Token: 0x04000659 RID: 1625
		private int fHeight;

		// Token: 0x0400065A RID: 1626
		private Graphics gdiPlusContext;
	}
}
