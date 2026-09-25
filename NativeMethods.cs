using System;
using System.Runtime.InteropServices;
using System.Text;

namespace TinhKiemAuto
{
	// Token: 0x020000C2 RID: 194
	internal static class NativeMethods
	{
		// Token: 0x06000A94 RID: 2708
		[DllImport("gdi32.dll", CallingConvention = CallingConvention.StdCall)]
		public static extern IntPtr CreateCompatibleDC(IntPtr hdc);

		// Token: 0x06000A95 RID: 2709
		[DllImport("gdi32.dll", CallingConvention = CallingConvention.StdCall)]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool DeleteDC(IntPtr hdc);

		// Token: 0x06000A96 RID: 2710
		[DllImport("gdi32.dll", CallingConvention = CallingConvention.StdCall)]
		public static extern IntPtr CreateCompatibleBitmap(IntPtr hdc, int nWidth, int nHeight);

		// Token: 0x06000A97 RID: 2711
		[DllImport("gdi32.dll", CallingConvention = CallingConvention.StdCall)]
		public static extern IntPtr SelectObject(IntPtr hdc, IntPtr hgdiobj);

		// Token: 0x06000A98 RID: 2712
		[DllImport("gdi32.dll", CallingConvention = CallingConvention.StdCall)]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool DeleteObject(IntPtr hObject);

		// Token: 0x06000A99 RID: 2713
		[DllImport("gdi32.dll", CallingConvention = CallingConvention.StdCall)]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool StretchBlt(IntPtr hdcDest, int nXOriginDest, int nYOriginDest, int nWidthDest, int nHeightDest, IntPtr hdcSrc, int nXOriginSrc, int nYOriginSrc, int nWidthSrc, int nHeightSrc, uint dwRop);

		// Token: 0x06000A9A RID: 2714
		[DllImport("gdi32.dll", CallingConvention = CallingConvention.StdCall)]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool BitBlt(IntPtr hdcDest, int nXDest, int nYDest, int nWidth, int nHeight, IntPtr hdcSrc, int nXSrc, int nYSrc, uint dwRop);

		// Token: 0x06000A9B RID: 2715
		[DllImport("gdi32.dll", CallingConvention = CallingConvention.StdCall)]
		public static extern uint GetPixel(IntPtr hdc, int nXPos, int nYPos);

		// Token: 0x06000A9C RID: 2716
		[DllImport("gdi32.dll", CallingConvention = CallingConvention.StdCall)]
		public static extern uint SetPixel(IntPtr hdc, int X, int Y, uint crColor);

		// Token: 0x06000A9D RID: 2717
		[DllImport("user32.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Unicode, EntryPoint = "SendMessageW")]
		public static extern IntPtr SendMessage(IntPtr hWnd, uint Msg, IntPtr wParam, IntPtr lParam);

		// Token: 0x06000A9E RID: 2718
		[DllImport("user32.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Unicode, EntryPoint = "RealGetWindowClassW")]
		public static extern uint RealGetWindowClass(IntPtr hWnd, StringBuilder ClassName, uint ClassNameMax);

		// Token: 0x040007C0 RID: 1984
		public const uint SRCCOPY = 13369376U;

		// Token: 0x040007C1 RID: 1985
		public const int TCM_HITTEST = 4877;

		// Token: 0x040007C2 RID: 1986
		public const int WM_SETFONT = 48;

		// Token: 0x040007C3 RID: 1987
		public const int WM_THEMECHANGED = 794;

		// Token: 0x040007C4 RID: 1988
		public const int WM_DESTROY = 2;

		// Token: 0x040007C5 RID: 1989
		public const int WM_NCDESTROY = 130;

		// Token: 0x040007C6 RID: 1990
		public const int WM_WINDOWPOSCHANGING = 70;

		// Token: 0x040007C7 RID: 1991
		public const int WM_PARENTNOTIFY = 528;

		// Token: 0x040007C8 RID: 1992
		public const int WM_CREATE = 1;

		// Token: 0x040007C9 RID: 1993
		public const int WM_MOUSEMOVE = 512;

		// Token: 0x040007CA RID: 1994
		public const int WM_LBUTTONDOWN = 513;

		// Token: 0x02000175 RID: 373
		public struct POINT
		{
			// Token: 0x04000E77 RID: 3703
			public int x;

			// Token: 0x04000E78 RID: 3704
			public int y;
		}

		// Token: 0x02000176 RID: 374
		public struct TCHITTESTINFO
		{
			// Token: 0x04000E79 RID: 3705
			public NativeMethods.POINT pt;

			// Token: 0x04000E7A RID: 3706
			public uint flags;
		}

		// Token: 0x02000177 RID: 375
		public struct WINDOWPOS
		{
			// Token: 0x04000E7B RID: 3707
			public IntPtr hwnd;

			// Token: 0x04000E7C RID: 3708
			public IntPtr hwndInsertAfter;

			// Token: 0x04000E7D RID: 3709
			public int x;

			// Token: 0x04000E7E RID: 3710
			public int y;

			// Token: 0x04000E7F RID: 3711
			public int cx;

			// Token: 0x04000E80 RID: 3712
			public int cy;

			// Token: 0x04000E81 RID: 3713
			public int flags;
		}
	}
}
