using System;
using System.Runtime.InteropServices;
using System.Text;

namespace TinhKiemAuto
{
	// Token: 0x020000C0 RID: 192
	internal class Music
	{
		// Token: 0x1700025C RID: 604
		// (get) Token: 0x06000A86 RID: 2694 RVA: 0x000436EF File Offset: 0x000418EF
		// (set) Token: 0x06000A87 RID: 2695 RVA: 0x000436F6 File Offset: 0x000418F6
		public static string Path
		{
			get
			{
				return Music.path;
			}
			set
			{
				Music.path = value;
			}
		}

		// Token: 0x06000A88 RID: 2696 RVA: 0x000436FE File Offset: 0x000418FE
		private static void Open()
		{
			Music.mciSendString("open \"" + Music.Path + "\" type mpegvideo alias MediaFile", null, 0, IntPtr.Zero);
			Music.isOpen = true;
		}

		// Token: 0x06000A89 RID: 2697 RVA: 0x00043728 File Offset: 0x00041928
		private static void Pause()
		{
			try
			{
				Music.mciSendString("stop MediaFile", null, 0, IntPtr.Zero);
			}
			catch
			{
			}
		}

		// Token: 0x06000A8A RID: 2698 RVA: 0x0004375C File Offset: 0x0004195C
		public static void Play()
		{
			if (Global.Mute)
			{
				return;
			}
			if (!Music.isOpen)
			{
				Music.Open();
				Music.mciSendString("play MediaFile REPEAT", null, 0, IntPtr.Zero);
			}
		}

		// Token: 0x06000A8B RID: 2699 RVA: 0x00043784 File Offset: 0x00041984
		public static void ForcePlay()
		{
			if (!Music.isOpen)
			{
				Music.Open();
				Music.mciSendString("play MediaFile REPEAT", null, 0, IntPtr.Zero);
			}
		}

		// Token: 0x06000A8C RID: 2700 RVA: 0x000437A4 File Offset: 0x000419A4
		public static void Stop()
		{
			Music.mciSendString("close MediaFile", null, 0, IntPtr.Zero);
			Music.isOpen = false;
		}

		// Token: 0x06000A8D RID: 2701
		[DllImport("winmm.dll")]
		private static extern long mciSendString(string stay, StringBuilder strbuilder, int width, IntPtr sign);

		// Token: 0x040007BE RID: 1982
		private static bool isOpen = false;

		// Token: 0x040007BF RID: 1983
		public static string path = "";
	}
}
