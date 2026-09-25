using System;
using System.Runtime.InteropServices;

namespace TinhKiemAuto
{
	// Token: 0x0200009A RID: 154
	internal class IdleTimeFinder
	{
		// Token: 0x060009A0 RID: 2464
		[DllImport("User32.dll")]
		private static extern bool GetLastInputInfo(ref LASTINPUTINFO plii);

		// Token: 0x060009A1 RID: 2465
		[DllImport("Kernel32.dll")]
		private static extern uint GetLastError();

		// Token: 0x060009A2 RID: 2466 RVA: 0x00040224 File Offset: 0x0003E424
		public static uint GetIdleTime()
		{
			LASTINPUTINFO lastinputinfo = default(LASTINPUTINFO);
			lastinputinfo.cbSize = (uint)Marshal.SizeOf(lastinputinfo);
			IdleTimeFinder.GetLastInputInfo(ref lastinputinfo);
			return (uint)((Environment.TickCount - (int)lastinputinfo.dwTime) / 1000);
		}

		// Token: 0x060009A3 RID: 2467 RVA: 0x00040268 File Offset: 0x0003E468
		public static long GetLastInputTime()
		{
			LASTINPUTINFO lastinputinfo = default(LASTINPUTINFO);
			lastinputinfo.cbSize = (uint)Marshal.SizeOf(lastinputinfo);
			if (!IdleTimeFinder.GetLastInputInfo(ref lastinputinfo))
			{
				throw new Exception(IdleTimeFinder.GetLastError().ToString());
			}
			return (long)((ulong)lastinputinfo.dwTime);
		}
	}
}
