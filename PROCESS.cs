using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace TinhKiemAuto
{
	// Token: 0x020000CD RID: 205
	internal class PROCESS
	{
		// Token: 0x06000AEE RID: 2798
		[DllImport("kernel32.dll")]
		private static extern IntPtr OpenThread(PROCESS.ThreadAccess dwDesiredAccess, bool bInheritHandle, uint dwThreadId);

		// Token: 0x06000AEF RID: 2799
		[DllImport("kernel32.dll")]
		private static extern bool CloseHandle(IntPtr hObject);

		// Token: 0x06000AF0 RID: 2800
		[DllImport("kernel32.dll")]
		private static extern uint SuspendThread(IntPtr hThread);

		// Token: 0x06000AF1 RID: 2801
		[DllImport("kernel32.dll")]
		private static extern int ResumeThread(IntPtr hThread);

		// Token: 0x06000AF2 RID: 2802 RVA: 0x000488E8 File Offset: 0x00046AE8
		public static void Suspend(int pid)
		{
			Process processById = Process.GetProcessById(pid);
			if (processById.ProcessName == string.Empty)
			{
				return;
			}
			foreach (object obj in processById.Threads)
			{
				ProcessThread processThread = (ProcessThread)obj;
				IntPtr intPtr = PROCESS.OpenThread(PROCESS.ThreadAccess.SUSPEND_RESUME, false, (uint)processThread.Id);
				if (!(intPtr == IntPtr.Zero))
				{
					PROCESS.SuspendThread(intPtr);
					PROCESS.CloseHandle(intPtr);
				}
			}
		}

		// Token: 0x06000AF3 RID: 2803 RVA: 0x00048980 File Offset: 0x00046B80
		public static void Resume(int pid)
		{
			Process processById = Process.GetProcessById(pid);
			if (processById.ProcessName == string.Empty)
			{
				return;
			}
			foreach (object obj in processById.Threads)
			{
				ProcessThread processThread = (ProcessThread)obj;
				IntPtr intPtr = PROCESS.OpenThread(PROCESS.ThreadAccess.SUSPEND_RESUME, false, (uint)processThread.Id);
				if (!(intPtr == IntPtr.Zero))
				{
					int num;
					do
					{
						num = PROCESS.ResumeThread(intPtr);
					}
					while (num > 0);
					PROCESS.CloseHandle(intPtr);
				}
			}
		}

		// Token: 0x0200017B RID: 379
		[Flags]
		public enum ThreadAccess
		{
			// Token: 0x04000E91 RID: 3729
			TERMINATE = 1,
			// Token: 0x04000E92 RID: 3730
			SUSPEND_RESUME = 2,
			// Token: 0x04000E93 RID: 3731
			GET_CONTEXT = 8,
			// Token: 0x04000E94 RID: 3732
			SET_CONTEXT = 16,
			// Token: 0x04000E95 RID: 3733
			SET_INFORMATION = 32,
			// Token: 0x04000E96 RID: 3734
			QUERY_INFORMATION = 64,
			// Token: 0x04000E97 RID: 3735
			SET_THREAD_TOKEN = 128,
			// Token: 0x04000E98 RID: 3736
			IMPERSONATE = 256,
			// Token: 0x04000E99 RID: 3737
			DIRECT_IMPERSONATION = 512
		}
	}
}
