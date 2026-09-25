using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace TinhKiemAuto
{
	// Token: 0x020000FB RID: 251
	internal partial class Games : Form
	{
		// Token: 0x06000E61 RID: 3681
		[DllImport("user32.dll")]
		public static extern IntPtr SetParent(IntPtr hWndChild, IntPtr hWndNewParent);

		// Token: 0x06000E62 RID: 3682
		[DllImport("user32.dll")]
		public static extern int SetWindowLong(IntPtr hWnd, int nIndex, int dwNewLong);

		// Token: 0x06000E63 RID: 3683
		[DllImport("user32.dll", SetLastError = true)]
		internal static extern bool MoveWindow(IntPtr hWnd, int X, int Y, int nWidth, int nHeight, bool bRepaint);

		// Token: 0x06000E64 RID: 3684
		[DllImport("USER32.DLL")]
		public static extern int GetWindowLong(IntPtr hWnd, int nIndex);

		// Token: 0x1700032C RID: 812
		// (get) Token: 0x06000E65 RID: 3685 RVA: 0x0006DDBC File Offset: 0x0006BFBC
		public static Games Instance
		{
			get
			{
				if (Games.instance == null || Games.instance.IsDisposed)
				{
					Games.instance = new Games();
				}
				return Games.instance;
			}
		}

		// Token: 0x06000E66 RID: 3686 RVA: 0x0006DDE0 File Offset: 0x0006BFE0
		public Games()
		{
			this.InitializeComponent();
			Games.instance = this;
		}

		// Token: 0x06000E67 RID: 3687 RVA: 0x0006DDF4 File Offset: 0x0006BFF4
		private void timer1_Tick(object sender, EventArgs e)
		{
			foreach (KeyValuePair<int, Game> keyValuePair in FrmMain.dicGame)
			{
				if (keyValuePair.Value.Parrent == IntPtr.Zero)
				{
					if (keyValuePair.Value.TLBB.Online)
					{
						this.BindGame(keyValuePair.Value);
					}
				}
				else
				{
					keyValuePair.Value.Parrent = this.tab.SelectedTab.Handle;
				}
			}
			foreach (object obj in this.tab.TabPages)
			{
				TabPage tabPage = (TabPage)obj;
				if (tabPage.Tag != null && ((Game)tabPage.Tag).Parrent == IntPtr.Zero)
				{
					this.tab.TabPages.Remove(tabPage);
				}
			}
		}

		// Token: 0x06000E68 RID: 3688 RVA: 0x0006DF14 File Offset: 0x0006C114
		private void BindGame(Game game)
		{
			int num = Games.GetWindowLong(game.Handle, -16);
			game.style = num;
			num &= -12582913;
			num &= -536870913;
			num &= -65537;
			num &= -131073;
			TabPage tabPage = new TabPage(game.TLBB.Name);
			tabPage.Tag = game;
			this.tab.TabPages.Add(tabPage);
			Win.Active(game.Handle);
			Games.SetParent(game.Handle, tabPage.Handle);
			Games.SetWindowLong(game.Handle, -16, num);
			Games.MoveWindow(game.Handle, 0, 0, tabPage.Width, tabPage.Height, true);
			game.LuaDoString("PushEvent('VIEW_RESOLUTION_CHANGED')");
			game.Parrent = tabPage.Handle;
		}

		// Token: 0x06000E69 RID: 3689
		[DllImport("user32.dll", SetLastError = true)]
		private static extern IntPtr SetFocus(IntPtr hWnd);

		// Token: 0x06000E6A RID: 3690 RVA: 0x0006DFE0 File Offset: 0x0006C1E0
		private void Games_FormClosing(object sender, FormClosingEventArgs e)
		{
			foreach (KeyValuePair<int, Game> keyValuePair in FrmMain.dicGame)
			{
				if (keyValuePair.Value.Parrent != IntPtr.Zero)
				{
					Games.SetParent(keyValuePair.Value.Handle, IntPtr.Zero);
					keyValuePair.Value.Parrent = IntPtr.Zero;
					Games.SetWindowLong(keyValuePair.Value.Handle, -16, keyValuePair.Value.style);
				}
			}
		}

		// Token: 0x06000E6B RID: 3691 RVA: 0x00006740 File Offset: 0x00004940
		private void tab_MouseClick(object sender, MouseEventArgs e)
		{
		}

		// Token: 0x06000E6C RID: 3692 RVA: 0x0006E08C File Offset: 0x0006C28C
		private void Games_ResizeEnd(object sender, EventArgs e)
		{
			this.IsResize = true;
		}

		// Token: 0x06000E6D RID: 3693 RVA: 0x0006E08C File Offset: 0x0006C28C
		private void Games_Resize(object sender, EventArgs e)
		{
			this.IsResize = true;
		}

		// Token: 0x06000E6E RID: 3694 RVA: 0x00006740 File Offset: 0x00004940
		private void Games_SizeChanged(object sender, EventArgs e)
		{
		}

		// Token: 0x06000E6F RID: 3695 RVA: 0x00006740 File Offset: 0x00004940
		private void Games_MouseUp(object sender, MouseEventArgs e)
		{
		}

		// Token: 0x06000E70 RID: 3696 RVA: 0x00006740 File Offset: 0x00004940
		private void Games_MouseEnter(object sender, EventArgs e)
		{
		}

		// Token: 0x06000E71 RID: 3697 RVA: 0x00006740 File Offset: 0x00004940
		private void Games_Validated(object sender, EventArgs e)
		{
		}

		// Token: 0x06000E72 RID: 3698 RVA: 0x0006E098 File Offset: 0x0006C298
		private void ResizeGame()
		{
			if (this.IsResize && this.tab.SelectedTab != null && this.tab.SelectedTab.Tag != null)
			{
				this.IsResize = false;
				Game game = this.tab.SelectedTab.Tag as Game;
				Win.RECT rect;
				Win.GetWindowRect(game.Handle, out rect);
				if (rect.Left + rect.Right != 0 || rect.Left == -32000 || TINHKIEM.NumDiff(rect.Right - rect.Left + 1, this.tab.SelectedTab.Width) > 10)
				{
					Games.MoveWindow(game.Handle, 0, 0, this.tab.SelectedTab.Width, this.tab.SelectedTab.Height, false);
					Games.ForcePaint(game.Handle);
				}
			}
		}

		// Token: 0x06000E73 RID: 3699 RVA: 0x0006E17E File Offset: 0x0006C37E
		private void timer2_Tick(object sender, EventArgs e)
		{
			this.ResizeGame();
		}

		// Token: 0x06000E74 RID: 3700
		[DllImport("User32.dll")]
		public static extern long SendMessage(IntPtr hWnd, uint msg, IntPtr wParam, IntPtr lParam);

		// Token: 0x06000E75 RID: 3701 RVA: 0x0006E186 File Offset: 0x0006C386
		public static void ForcePaint(IntPtr handle)
		{
			Games.SendMessage(handle, 15U, IntPtr.Zero, IntPtr.Zero);
		}

		// Token: 0x06000E76 RID: 3702 RVA: 0x00006740 File Offset: 0x00004940
		private void Games_Load(object sender, EventArgs e)
		{
		}

		// Token: 0x06000E77 RID: 3703 RVA: 0x00006740 File Offset: 0x00004940
		private void Games_Load_1(object sender, EventArgs e)
		{
		}

		// Token: 0x04000BD5 RID: 3029
		public const int WS_BORDER = 8388608;

		// Token: 0x04000BD6 RID: 3030
		public const int WS_DLGFRAME = 4194304;

		// Token: 0x04000BD7 RID: 3031
		public const int WS_CAPTION = 12582912;

		// Token: 0x04000BD8 RID: 3032
		public const int WS_SYSMENU = 524288;

		// Token: 0x04000BD9 RID: 3033
		public const int WS_THICKFRAME = 262144;

		// Token: 0x04000BDA RID: 3034
		public const int WS_MINIMIZE = 536870912;

		// Token: 0x04000BDB RID: 3035
		public const int WS_MAXIMIZEBOX = 65536;

		// Token: 0x04000BDC RID: 3036
		public const int GWL_STYLE = -16;

		// Token: 0x04000BDD RID: 3037
		public const int GWL_EXSTYLE = -20;

		// Token: 0x04000BDE RID: 3038
		public const int WS_EX_DLGMODALFRAME = 1;

		// Token: 0x04000BDF RID: 3039
		public const int SWP_NOMOVE = 2;

		// Token: 0x04000BE0 RID: 3040
		public const int SWP_NOSIZE = 1;

		// Token: 0x04000BE1 RID: 3041
		public const int SWP_FRAMECHANGED = 32;

		// Token: 0x04000BE2 RID: 3042
		public const uint MF_BYPOSITION = 1024U;

		// Token: 0x04000BE3 RID: 3043
		public const uint MF_REMOVE = 4096U;

		// Token: 0x04000BE4 RID: 3044
		public const int WS_MINIMIZEBOX = 131072;

		// Token: 0x04000BE5 RID: 3045
		public static Games instance;

		// Token: 0x04000BE6 RID: 3046
		private bool IsResize;

		// Token: 0x04000BE7 RID: 3047
		private const int WmPaint = 15;
	}
}
