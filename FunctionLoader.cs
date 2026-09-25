using System;
using System.Runtime.InteropServices;

// Token: 0x02000004 RID: 4
internal class FunctionLoader
{
	// Token: 0x06000006 RID: 6
	[DllImport("Kernel32.dll")]
	private static extern IntPtr LoadLibrary(string path);

	// Token: 0x06000007 RID: 7
	[DllImport("Kernel32.dll")]
	private static extern IntPtr GetProcAddress(IntPtr hModule, string procName);

	// Token: 0x06000008 RID: 8 RVA: 0x000020A8 File Offset: 0x000002A8
	public static Delegate LoadFunction<T>(string dllPath, string functionName)
	{
		return Marshal.GetDelegateForFunctionPointer(FunctionLoader.GetProcAddress(FunctionLoader.LoadLibrary(dllPath), functionName), typeof(T));
	}
}
