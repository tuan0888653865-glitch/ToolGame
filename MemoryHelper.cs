using System;
using System.Diagnostics;

namespace TinhKiemAuto
{
	// Token: 0x020000B9 RID: 185
	internal class MemoryHelper
	{
		// Token: 0x06000A5E RID: 2654 RVA: 0x00042F08 File Offset: 0x00041108
		public static void ReduceMemory(int processId)
		{
			try
			{
				Process processById = Process.GetProcessById(processId);
				if (MemoryHelper._Toggle)
				{
					processById.MaxWorkingSet = (IntPtr)((int)processById.MaxWorkingSet - 1);
					processById.MinWorkingSet = (IntPtr)((int)processById.MinWorkingSet - 1);
				}
				else
				{
					processById.MaxWorkingSet = (IntPtr)((int)processById.MaxWorkingSet + 1);
					processById.MinWorkingSet = (IntPtr)((int)processById.MinWorkingSet + 1);
				}
				MemoryHelper._Toggle = !MemoryHelper._Toggle;
			}
			catch
			{
			}
		}

		// Token: 0x040007A9 RID: 1961
		private static bool _Toggle = true;
	}
}
