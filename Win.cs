using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

namespace TinhKiemAuto
{
	// Token: 0x020000F5 RID: 245
	internal class Win
	{
		// Token: 0x06000CC2 RID: 3266
		[DllImport("user32.dll", SetLastError = true)]
		public static extern bool SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int X, int Y, int cx, int cy, Win.SetWindowPosFlags uFlags);

		// Token: 0x06000CC3 RID: 3267 RVA: 0x000529B0 File Offset: 0x00050BB0
		public static IntPtr GetHandle(int processId, string className)
		{
			IntPtr window = Win.GetWindow(Win.GetForegroundWindow(), Win.GetWindow_Cmd.GW_HWNDFIRST);
			StringBuilder stringBuilder = new StringBuilder(100);
			while (window != IntPtr.Zero)
			{
				Win.GetClassName(window, stringBuilder, 100);
				if (stringBuilder.ToString().IndexOf(className) != -1)
				{
					int num = 0;
					Win.GetWindowThreadProcessId(window, out num);
					if (num == processId)
					{
						return window;
					}
				}
				window = Win.GetWindow(window, Win.GetWindow_Cmd.GW_HWNDNEXT);
			}
			return IntPtr.Zero;
		}

		// Token: 0x06000CC4 RID: 3268 RVA: 0x00052A18 File Offset: 0x00050C18
		public static IntPtr GetHandle(int processId, string[] classNames)
		{
			IntPtr window = Win.GetWindow(Win.GetForegroundWindow(), Win.GetWindow_Cmd.GW_HWNDFIRST);
			StringBuilder stringBuilder = new StringBuilder(100);
			while (window != IntPtr.Zero)
			{
				Win.GetClassName(window, stringBuilder, 100);
				foreach (string value in classNames)
				{
					if (stringBuilder.ToString().IndexOf(value) != -1)
					{
						int num = 0;
						Win.GetWindowThreadProcessId(window, out num);
						if (num == processId)
						{
							return window;
						}
					}
				}
				window = Win.GetWindow(window, Win.GetWindow_Cmd.GW_HWNDNEXT);
			}
			return IntPtr.Zero;
		}

		// Token: 0x06000CC5 RID: 3269 RVA: 0x00052A98 File Offset: 0x00050C98
		public static IntPtr GetHandle(string className, string windowText)
		{
			IntPtr window = Win.GetWindow(Win.GetForegroundWindow(), Win.GetWindow_Cmd.GW_HWNDFIRST);
			StringBuilder stringBuilder = new StringBuilder(100);
			StringBuilder stringBuilder2 = new StringBuilder(100);
			while (window != IntPtr.Zero)
			{
				Win.GetClassName(window, stringBuilder, 100);
				Win.GetWindowText(window, stringBuilder2, 100);
				if (stringBuilder.ToString().IndexOf(className) != -1 && stringBuilder2.ToString().IndexOf(windowText) != -1)
				{
					return window;
				}
				window = Win.GetWindow(window, Win.GetWindow_Cmd.GW_HWNDNEXT);
			}
			return IntPtr.Zero;
		}

		// Token: 0x06000CC6 RID: 3270 RVA: 0x00052B14 File Offset: 0x00050D14
		public static HashSet<Process> GetProcessByClassName(string className)
		{
			HashSet<Process> hashSet = new HashSet<Process>();
			IntPtr window = Win.GetWindow(Win.GetForegroundWindow(), Win.GetWindow_Cmd.GW_HWNDFIRST);
			StringBuilder stringBuilder = new StringBuilder(100);
			int processId = 0;
			while (window != IntPtr.Zero)
			{
				Win.GetClassName(window, stringBuilder, 100);
				if (stringBuilder.ToString().IndexOf(className) != -1)
				{
					Win.GetWindowThreadProcessId(window, out processId);
					hashSet.Add(Process.GetProcessById(processId));
				}
				window = Win.GetWindow(window, Win.GetWindow_Cmd.GW_HWNDNEXT);
			}
			return hashSet;
		}

		// Token: 0x06000CC7 RID: 3271 RVA: 0x00052B84 File Offset: 0x00050D84
		public static void Active(Form form)
		{
			form.Show();
			Win.Active(form.Handle);
			form.Activate();
			form.Refresh();
		}

		// Token: 0x06000CC8 RID: 3272 RVA: 0x00052BA4 File Offset: 0x00050DA4
		public static void Active(IntPtr handle)
		{
			Win.RECT rect;
			Win.GetWindowRect(handle, out rect);
			bool flag = rect.Left == -32000;
			if (!Win.IsWindowVisible(handle))
			{
				Win.ShowWindow(handle, Win.WindowShowStyle.Show);
			}
			if (flag)
			{
				Win.ShowWindow(handle, Win.WindowShowStyle.Restore);
			}
			if (Win.GetForegroundWindow() != handle)
			{
				Win.SetForegroundWindow(handle);
			}
		}

		// Token: 0x06000CC9 RID: 3273 RVA: 0x00052BF6 File Offset: 0x00050DF6
		public static bool IsHideOrMini(IntPtr handle)
		{
			return Win.IsWindowVisible(handle) || Win.IsWindowMini(handle);
		}

		// Token: 0x06000CCA RID: 3274 RVA: 0x00052C08 File Offset: 0x00050E08
		public static bool IsWindowMini(IntPtr handle)
		{
			Win.RECT rect;
			Win.GetWindowRect(handle, out rect);
			return rect.Left == -32000;
		}

		// Token: 0x06000CCB RID: 3275 RVA: 0x00052C2B File Offset: 0x00050E2B
		public static void Hide(IntPtr handle)
		{
			Win.ShowWindow(handle, Win.WindowShowStyle.Hide);
		}

		// Token: 0x17000318 RID: 792
		// (get) Token: 0x06000CCC RID: 3276 RVA: 0x00052C38 File Offset: 0x00050E38
		public static int ScreenWidth
		{
			get
			{
				return Screen.PrimaryScreen.Bounds.Width;
			}
		}

		// Token: 0x17000319 RID: 793
		// (get) Token: 0x06000CCD RID: 3277 RVA: 0x00052C58 File Offset: 0x00050E58
		public static int ScreenHight
		{
			get
			{
				return Screen.PrimaryScreen.Bounds.Height - (Screen.PrimaryScreen.Bounds.Bottom - Screen.PrimaryScreen.WorkingArea.Bottom);
			}
		}

		// Token: 0x06000CCE RID: 3278 RVA: 0x00052CA0 File Offset: 0x00050EA0
		public static void MoveEx(Form form, Win.WindowLocation location, int width, int height)
		{
			int width2 = Screen.PrimaryScreen.Bounds.Width;
			int height2 = Screen.PrimaryScreen.Bounds.Height;
			switch (location)
			{
			case Win.WindowLocation.TopLeft:
				form.Location = new Point(0, 0);
				return;
			case Win.WindowLocation.TopRight:
				form.Location = new Point(width2 - width, 0);
				return;
			case Win.WindowLocation.BottomRight:
				form.Location = new Point(width2 - width, height2 - height - (Screen.PrimaryScreen.Bounds.Bottom - Screen.PrimaryScreen.WorkingArea.Bottom));
				return;
			case Win.WindowLocation.BottomLeft:
				form.Location = new Point(0, height2 - height - (Screen.PrimaryScreen.Bounds.Bottom - Screen.PrimaryScreen.WorkingArea.Bottom));
				return;
			case Win.WindowLocation.Center:
				form.Location = new Point((width2 - width) / 2, (height2 - height - (Screen.PrimaryScreen.Bounds.Bottom - Screen.PrimaryScreen.WorkingArea.Bottom)) / 2);
				return;
			case Win.WindowLocation.None:
				form.Location = new Point(-1000, -1000);
				return;
			case Win.WindowLocation.TopCenter:
				form.Location = new Point((width2 - width) / 2, 0);
				return;
			case Win.WindowLocation.RightCenter:
				form.Location = new Point(width2 - width, (height2 - height - (Screen.PrimaryScreen.Bounds.Bottom - Screen.PrimaryScreen.WorkingArea.Bottom)) / 2);
				return;
			case Win.WindowLocation.BottomCenter:
				form.Location = new Point((width2 - width) / 2, height2 - height - (Screen.PrimaryScreen.Bounds.Bottom - Screen.PrimaryScreen.WorkingArea.Bottom));
				return;
			case Win.WindowLocation.LeftCenter:
				form.Location = new Point(0, (height2 - height - (Screen.PrimaryScreen.Bounds.Bottom - Screen.PrimaryScreen.WorkingArea.Bottom)) / 2);
				return;
			default:
				return;
			}
		}

		// Token: 0x06000CCF RID: 3279 RVA: 0x00052EA0 File Offset: 0x000510A0
		public static void Move(Form form, Win.WindowLocation location)
		{
			int width = Screen.PrimaryScreen.Bounds.Width;
			int height = Screen.PrimaryScreen.Bounds.Height;
			switch (location)
			{
			case Win.WindowLocation.TopLeft:
				form.Location = new Point(0, 0);
				return;
			case Win.WindowLocation.TopRight:
				form.Location = new Point(width - form.Width, 0);
				return;
			case Win.WindowLocation.BottomRight:
				form.Location = new Point(width - form.Width, height - form.Height - (Screen.PrimaryScreen.Bounds.Bottom - Screen.PrimaryScreen.WorkingArea.Bottom));
				return;
			case Win.WindowLocation.BottomLeft:
				form.Location = new Point(0, height - form.Height - (Screen.PrimaryScreen.Bounds.Bottom - Screen.PrimaryScreen.WorkingArea.Bottom));
				return;
			case Win.WindowLocation.Center:
				form.Location = new Point((width - form.Width) / 2, (height - form.Height - (Screen.PrimaryScreen.Bounds.Bottom - Screen.PrimaryScreen.WorkingArea.Bottom)) / 2);
				return;
			case Win.WindowLocation.None:
				form.Location = new Point(-1000, -1000);
				return;
			case Win.WindowLocation.TopCenter:
				form.Location = new Point((width - form.Width) / 2, 0);
				return;
			case Win.WindowLocation.RightCenter:
				form.Location = new Point(width - form.Width, (height - form.Height - (Screen.PrimaryScreen.Bounds.Bottom - Screen.PrimaryScreen.WorkingArea.Bottom)) / 2);
				return;
			case Win.WindowLocation.BottomCenter:
				form.Location = new Point((width - form.Width) / 2, height - form.Height - (Screen.PrimaryScreen.Bounds.Bottom - Screen.PrimaryScreen.WorkingArea.Bottom));
				return;
			case Win.WindowLocation.LeftCenter:
				form.Location = new Point(0, (height - form.Height - (Screen.PrimaryScreen.Bounds.Bottom - Screen.PrimaryScreen.WorkingArea.Bottom)) / 2);
				return;
			default:
				return;
			}
		}

		// Token: 0x06000CD0 RID: 3280
		[DllImport("user32.dll", SetLastError = true)]
		public static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

		// Token: 0x06000CD1 RID: 3281
		[DllImport("user32.dll")]
		public static extern IntPtr SetParent(IntPtr hWndChild, IntPtr hWndNewParent);

		// Token: 0x06000CD2 RID: 3282
		[DllImport("user32.dll")]
		public static extern bool ShowWindow(IntPtr hWnd, Win.WindowShowStyle nCmdShow);

		// Token: 0x06000CD3 RID: 3283
		[DllImport("user32.dll")]
		public static extern bool PostMessage(IntPtr hWnd, int Msg, int wParam, int lParam);

		// Token: 0x06000CD4 RID: 3284
		[DllImport("user32.dll")]
		public static extern IntPtr SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);

		// Token: 0x06000CD5 RID: 3285
		[DllImport("user32.dll")]
		private static extern bool BringWindowToTop(IntPtr hWnd);

		// Token: 0x06000CD6 RID: 3286
		[DllImport("user32.dll")]
		public static extern bool IsWindowVisible(IntPtr hWnd);

		// Token: 0x06000CD7 RID: 3287
		[DllImport("user32.dll")]
		public static extern bool GetWindowRect(IntPtr hwnd, out Win.RECT lpRect);

		// Token: 0x06000CD8 RID: 3288
		[DllImport("user32.dll", SetLastError = true)]
		private static extern IntPtr GetWindow(IntPtr hWnd, Win.GetWindow_Cmd uCmd);

		// Token: 0x06000CD9 RID: 3289
		[DllImport("user32.dll")]
		public static extern int SetWindowText(IntPtr hWnd, string text);

		// Token: 0x06000CDA RID: 3290
		[DllImport("user32.dll")]
		private static extern uint GetWindowThreadProcessId(IntPtr hWnd, out int lpdwProcessId);

		// Token: 0x06000CDB RID: 3291
		[DllImport("user32.dll")]
		public static extern IntPtr GetForegroundWindow();

		// Token: 0x06000CDC RID: 3292
		[DllImport("user32.dll")]
		private static extern int GetClassName(IntPtr hWnd, StringBuilder lpClassName, int nMaxCount);

		// Token: 0x06000CDD RID: 3293
		[DllImport("user32.dll")]
		private static extern int GetWindowText(IntPtr hWnd, StringBuilder lpString, int nMaxCount);

		// Token: 0x06000CDE RID: 3294
		[DllImport("user32.dll")]
		private static extern bool SetForegroundWindow(IntPtr hWnd);

		// Token: 0x06000CDF RID: 3295
		[DllImport("user32.dll")]
		public static extern bool EnableWindow(IntPtr hwnd, bool enabled);

		// Token: 0x06000CE0 RID: 3296
		[DllImport("user32.dll")]
		public static extern bool ShowWindowAsync(IntPtr hWnd, Win.WindowShowStyle nCmdShow);

		// Token: 0x04000A10 RID: 2576
		public static string[] WndClassNames = new string[]
		{
			"TianLongBaBu WndClass",
			"ThienLongHub WndClass",
			"ThienLongPri WndClass",
			"ThienLongTK2 WndClass",
			"TLBBTinhKiem2WndClass",
			"ThienLong KN WndClass",
			"TLBBPhatKhin WndClass",
			"#32770"
		};

		// Token: 0x04000A11 RID: 2577
		public static string[] GameExeProcessNames = new string[]
		{
			"Game.exe",
			"Plugin_OgreManager.dll",
			"Plugin_Khin.dll",
			"Plugin_Game.dll",
			"Plugin_TinhKiem2.dll",
			"TLBB (32 bit)",
			"tConfig.exe"
		};

		// Token: 0x02000188 RID: 392
		public enum SpecialWindowHandles
		{
			// Token: 0x04000EBC RID: 3772
			HWND_TOP,
			// Token: 0x04000EBD RID: 3773
			HWND_BOTTOM,
			// Token: 0x04000EBE RID: 3774
			HWND_TOPMOST = -1,
			// Token: 0x04000EBF RID: 3775
			HWND_NOTOPMOST = -2
		}

		// Token: 0x02000189 RID: 393
		[Flags]
		public enum SetWindowPosFlags : uint
		{
			// Token: 0x04000EC1 RID: 3777
			SWP_ASYNCWINDOWPOS = 16384U,
			// Token: 0x04000EC2 RID: 3778
			SWP_DEFERERASE = 8192U,
			// Token: 0x04000EC3 RID: 3779
			SWP_DRAWFRAME = 32U,
			// Token: 0x04000EC4 RID: 3780
			SWP_FRAMECHANGED = 32U,
			// Token: 0x04000EC5 RID: 3781
			SWP_HIDEWINDOW = 128U,
			// Token: 0x04000EC6 RID: 3782
			SWP_NOACTIVATE = 16U,
			// Token: 0x04000EC7 RID: 3783
			SWP_NOCOPYBITS = 256U,
			// Token: 0x04000EC8 RID: 3784
			SWP_NOMOVE = 2U,
			// Token: 0x04000EC9 RID: 3785
			SWP_NOOWNERZORDER = 512U,
			// Token: 0x04000ECA RID: 3786
			SWP_NOREDRAW = 8U,
			// Token: 0x04000ECB RID: 3787
			SWP_NOREPOSITION = 512U,
			// Token: 0x04000ECC RID: 3788
			SWP_NOSENDCHANGING = 1024U,
			// Token: 0x04000ECD RID: 3789
			SWP_NOSIZE = 1U,
			// Token: 0x04000ECE RID: 3790
			SWP_NOZORDER = 4U,
			// Token: 0x04000ECF RID: 3791
			SWP_SHOWWINDOW = 64U
		}

		// Token: 0x0200018A RID: 394
		public enum WindowLocation : byte
		{
			// Token: 0x04000ED1 RID: 3793
			TopLeft,
			// Token: 0x04000ED2 RID: 3794
			TopRight,
			// Token: 0x04000ED3 RID: 3795
			BottomRight,
			// Token: 0x04000ED4 RID: 3796
			BottomLeft,
			// Token: 0x04000ED5 RID: 3797
			Center,
			// Token: 0x04000ED6 RID: 3798
			None,
			// Token: 0x04000ED7 RID: 3799
			TopCenter,
			// Token: 0x04000ED8 RID: 3800
			RightCenter,
			// Token: 0x04000ED9 RID: 3801
			BottomCenter,
			// Token: 0x04000EDA RID: 3802
			LeftCenter
		}

		// Token: 0x0200018B RID: 395
		public enum WindowShowStyle : uint
		{
			// Token: 0x04000EDC RID: 3804
			Hide,
			// Token: 0x04000EDD RID: 3805
			ShowNormal,
			// Token: 0x04000EDE RID: 3806
			ShowMinimized,
			// Token: 0x04000EDF RID: 3807
			ShowMaximized,
			// Token: 0x04000EE0 RID: 3808
			Maximize = 3U,
			// Token: 0x04000EE1 RID: 3809
			ShowNormalNoActivate,
			// Token: 0x04000EE2 RID: 3810
			Show,
			// Token: 0x04000EE3 RID: 3811
			Minimize,
			// Token: 0x04000EE4 RID: 3812
			ShowMinNoActivate,
			// Token: 0x04000EE5 RID: 3813
			ShowNoActivate,
			// Token: 0x04000EE6 RID: 3814
			Restore,
			// Token: 0x04000EE7 RID: 3815
			ShowDefault,
			// Token: 0x04000EE8 RID: 3816
			ForceMinimized
		}

		// Token: 0x0200018C RID: 396
		public struct RECT
		{
			// Token: 0x04000EE9 RID: 3817
			public int Left;

			// Token: 0x04000EEA RID: 3818
			public int Top;

			// Token: 0x04000EEB RID: 3819
			public int Right;

			// Token: 0x04000EEC RID: 3820
			public int Bottom;
		}

		// Token: 0x0200018D RID: 397
		private enum GetWindow_Cmd : uint
		{
			// Token: 0x04000EEE RID: 3822
			GW_HWNDFIRST,
			// Token: 0x04000EEF RID: 3823
			GW_HWNDLAST,
			// Token: 0x04000EF0 RID: 3824
			GW_HWNDNEXT,
			// Token: 0x04000EF1 RID: 3825
			GW_HWNDPREV,
			// Token: 0x04000EF2 RID: 3826
			GW_OWNER,
			// Token: 0x04000EF3 RID: 3827
			GW_CHILD,
			// Token: 0x04000EF4 RID: 3828
			GW_ENABLEDPOPUP
		}
	}
}
