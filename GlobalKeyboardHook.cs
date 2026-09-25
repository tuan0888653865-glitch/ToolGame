using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace TinhKiemAuto
{
	// Token: 0x02000099 RID: 153
	internal class GlobalKeyboardHook
	{
		// Token: 0x1400000D RID: 13
		// (add) Token: 0x06000991 RID: 2449 RVA: 0x0003FF8C File Offset: 0x0003E18C
		// (remove) Token: 0x06000992 RID: 2450 RVA: 0x0003FFC4 File Offset: 0x0003E1C4
		public event KeyEventHandler KeyDown;

		// Token: 0x1400000E RID: 14
		// (add) Token: 0x06000993 RID: 2451 RVA: 0x0003FFFC File Offset: 0x0003E1FC
		// (remove) Token: 0x06000994 RID: 2452 RVA: 0x00040034 File Offset: 0x0003E234
		public event KeyEventHandler KeyUp;

		// Token: 0x06000995 RID: 2453 RVA: 0x00040069 File Offset: 0x0003E269
		public GlobalKeyboardHook()
		{
			this.Hook();
		}

		// Token: 0x1400000F RID: 15
		// (add) Token: 0x06000996 RID: 2454 RVA: 0x00040090 File Offset: 0x0003E290
		// (remove) Token: 0x06000997 RID: 2455 RVA: 0x000400C8 File Offset: 0x0003E2C8
		public event EventHandler DisTruct;

		// Token: 0x06000998 RID: 2456 RVA: 0x00040100 File Offset: 0x0003E300
		~GlobalKeyboardHook()
		{
			this.UnHook();
			if (this.DisTruct != null)
			{
				this.DisTruct(null, null);
			}
		}

		// Token: 0x06000999 RID: 2457 RVA: 0x00040144 File Offset: 0x0003E344
		public void Hook()
		{
			IntPtr hInstance = GlobalKeyboardHook.LoadLibrary("User32");
			GlobalKeyboardHook._keyboardHookProc = new GlobalKeyboardHook.keyboardHookProc(this.hookProc);
			this.hhook = GlobalKeyboardHook.SetWindowsHookEx(13, GlobalKeyboardHook._keyboardHookProc, hInstance, 0U);
		}

		// Token: 0x0600099A RID: 2458 RVA: 0x00040181 File Offset: 0x0003E381
		public void UnHook()
		{
			GlobalKeyboardHook.UnhookWindowsHookEx(this.hhook);
		}

		// Token: 0x0600099B RID: 2459 RVA: 0x00040190 File Offset: 0x0003E390
		public int hookProc(int code, int wParam, ref GlobalKeyboardHook.keyboardHookStruct lParam)
		{
			if (code >= 0)
			{
				Keys vkCode = (Keys)lParam.vkCode;
				if (this.HookedKeys.Contains(vkCode))
				{
					KeyEventArgs keyEventArgs = new KeyEventArgs(vkCode);
					if ((wParam == 256 || wParam == 260) && this.KeyDown != null)
					{
						this.KeyDown(this, keyEventArgs);
					}
					else if ((wParam == 257 || wParam == 261) && this.KeyUp != null)
					{
						this.KeyUp(this, keyEventArgs);
					}
					if (keyEventArgs.Handled)
					{
						return 1;
					}
				}
			}
			return GlobalKeyboardHook.CallNextHookEx(this.hhook, code, wParam, ref lParam);
		}

		// Token: 0x0600099C RID: 2460
		[DllImport("user32.dll")]
		private static extern IntPtr SetWindowsHookEx(int idHook, GlobalKeyboardHook.keyboardHookProc callback, IntPtr hInstance, uint threadId);

		// Token: 0x0600099D RID: 2461
		[DllImport("user32.dll")]
		private static extern bool UnhookWindowsHookEx(IntPtr hInstance);

		// Token: 0x0600099E RID: 2462
		[DllImport("user32.dll")]
		private static extern int CallNextHookEx(IntPtr idHook, int nCode, int wParam, ref GlobalKeyboardHook.keyboardHookStruct lParam);

		// Token: 0x0600099F RID: 2463
		[DllImport("kernel32.dll")]
		private static extern IntPtr LoadLibrary(string lpFileName);

		// Token: 0x040006A9 RID: 1705
		public static GlobalKeyboardHook.keyboardHookProc _keyboardHookProc;

		// Token: 0x040006AA RID: 1706
		private const int WH_KEYBOARD_LL = 13;

		// Token: 0x040006AB RID: 1707
		private const int WM_KEYDOWN = 256;

		// Token: 0x040006AC RID: 1708
		private const int WM_KEYUP = 257;

		// Token: 0x040006AD RID: 1709
		private const int WM_SYSKEYDOWN = 260;

		// Token: 0x040006AE RID: 1710
		private const int WM_SYSKEYUP = 261;

		// Token: 0x040006AF RID: 1711
		public List<Keys> HookedKeys = new List<Keys>();

		// Token: 0x040006B0 RID: 1712
		private IntPtr hhook = IntPtr.Zero;

		// Token: 0x0200016D RID: 365
		// (Invoke) Token: 0x06001187 RID: 4487
		public delegate int keyboardHookProc(int code, int wParam, ref GlobalKeyboardHook.keyboardHookStruct lParam);

		// Token: 0x0200016E RID: 366
		public struct keyboardHookStruct
		{
			// Token: 0x04000E51 RID: 3665
			public int vkCode;

			// Token: 0x04000E52 RID: 3666
			public int scanCode;

			// Token: 0x04000E53 RID: 3667
			public int flags;

			// Token: 0x04000E54 RID: 3668
			public int time;

			// Token: 0x04000E55 RID: 3669
			public int dwExtraInfo;
		}
	}
}
