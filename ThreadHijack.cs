using System;
using System.Diagnostics;
using System.Threading;

namespace TinhKiemAuto
{
	// Token: 0x020000E9 RID: 233
	internal class ThreadHijack : StandardInjectionMethod
	{
		// Token: 0x06000BFA RID: 3066 RVA: 0x0004C53C File Offset: 0x0004A73C
		public override IntPtr Inject(string dllPath, IntPtr hProcess)
		{
			this.ClearErrors();
			IntPtr[] array = this.InjectAll(new string[]
			{
				dllPath
			}, hProcess);
			if (array != null && array[0].IsNull() && this.GetLastError() == null)
			{
				this.SetLastError(new Exception("Module's entry point function reported a failure"));
			}
			if (array == null || array.Length == 0)
			{
				return IntPtr.Zero;
			}
			return array[0];
		}

		// Token: 0x06000BFB RID: 3067 RVA: 0x0004C598 File Offset: 0x0004A798
		public override IntPtr[] InjectAll(string[] dllPaths, IntPtr hProcess)
		{
			this.ClearErrors();
			IntPtr[] result;
			try
			{
				if (hProcess.IsNull() || hProcess.Compare(-1L))
				{
					throw new ArgumentException("Invalid process handle.", "hProcess");
				}
				int processId = WinAPI.GetProcessId(hProcess);
				if (processId == 0)
				{
					throw new ArgumentException("Provided handle doesn't have sufficient permissions to inject", "hProcess");
				}
				Process processById = Process.GetProcessById(processId);
				if (processById.Threads.Count == 0)
				{
					throw new Exception("Target process has no targetable threads to hijack.");
				}
				ProcessThread processThread = ThreadHijack.SelectOptimalThread(processById);
				IntPtr intPtr = WinAPI.OpenThread(26U, false, processThread.Id);
				if (intPtr.IsNull() || intPtr.Compare(-1L))
				{
					throw new Exception("Unable to obtain a handle for the remote thread.");
				}
				IntPtr zero = IntPtr.Zero;
				IntPtr zero2 = IntPtr.Zero;
				IntPtr intPtr2 = this.CreateMultiLoadStub(dllPaths, hProcess, out zero, 1U);
				IntPtr[] array = null;
				if (!intPtr2.IsNull())
				{
					if (WinAPI.SuspendThread(intPtr) == 4294967295U)
					{
						throw new Exception("Unable to suspend the remote thread");
					}
					try
					{
						uint num = 0U;
						WinAPI.CONTEXT context = new WinAPI.CONTEXT
						{
							ContextFlags = 65537U
						};
						if (!WinAPI.GetThreadContext(intPtr, ref context))
						{
							throw new InvalidOperationException("Cannot get the remote thread's context");
						}
						byte[] redirect_STUB = ThreadHijack.REDIRECT_STUB;
						IntPtr intPtr3 = WinAPI.VirtualAllocEx(hProcess, IntPtr.Zero, (uint)redirect_STUB.Length, 12288, 64);
						if (intPtr3.IsNull())
						{
							throw new InvalidOperationException("Unable to allocate memory in the remote process.");
						}
						BitConverter.GetBytes(intPtr2.Subtract(intPtr3.Add(7L)).ToInt32()).CopyTo(redirect_STUB, 3);
						BitConverter.GetBytes(context.Eip - (uint)intPtr3.Add((long)redirect_STUB.Length).ToInt32()).CopyTo(redirect_STUB, redirect_STUB.Length - 4);
						if (!WinAPI.WriteProcessMemory(hProcess, intPtr3, redirect_STUB, redirect_STUB.Length, out num) || (ulong)num != (ulong)((long)redirect_STUB.Length))
						{
							throw new InvalidOperationException("Unable to write stub to the remote process.");
						}
						context.Eip = (uint)intPtr3.ToInt32();
						WinAPI.SetThreadContext(intPtr, ref context);
					}
					catch (Exception lastError)
					{
						this.SetLastError(lastError);
						array = null;
						WinAPI.VirtualFreeEx(hProcess, zero, 0, 32768);
						WinAPI.VirtualFreeEx(hProcess, intPtr2, 0, 32768);
						WinAPI.VirtualFreeEx(hProcess, zero2, 0, 32768);
					}
					WinAPI.ResumeThread(intPtr);
					if (this.GetLastError() == null)
					{
						Thread.Sleep(100);
						array = new IntPtr[dllPaths.Length];
						byte[] array2 = WinAPI.ReadRemoteMemory(hProcess, zero, (uint)((uint)dllPaths.Length << 2));
						if (array2 != null)
						{
							for (int i = 0; i < array.Length; i++)
							{
								array[i] = Win32Ptr.Create((long)BitConverter.ToInt32(array2, i << 2));
							}
						}
					}
					WinAPI.CloseHandle(intPtr);
				}
				result = array;
			}
			catch (Exception lastError2)
			{
				this.SetLastError(lastError2);
				result = null;
			}
			return result;
		}

		// Token: 0x06000BFC RID: 3068 RVA: 0x0004C854 File Offset: 0x0004AA54
		private static ProcessThread SelectOptimalThread(Process target)
		{
			return target.Threads[0];
		}

		// Token: 0x0400092D RID: 2349
		private static readonly byte[] REDIRECT_STUB = new byte[]
		{
			156,
			96,
			232,
			0,
			0,
			0,
			0,
			97,
			157,
			233,
			0,
			0,
			0,
			0
		};
	}
}
