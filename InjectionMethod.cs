using System;

namespace TinhKiemAuto
{
	// Token: 0x020000AD RID: 173
	public abstract class InjectionMethod : ErrorBase
	{
		// Token: 0x060009C8 RID: 2504 RVA: 0x00040490 File Offset: 0x0003E690
		public static InjectionMethod Create(InjectionMethodType type)
		{
			InjectionMethod injectionMethod;
			switch (type)
			{
			case InjectionMethodType.Standard:
				injectionMethod = new CRTInjection();
				break;
			case InjectionMethodType.ThreadHijack:
				injectionMethod = new ThreadHijack();
				break;
			case InjectionMethodType.ManualMap:
				injectionMethod = new ManualMap();
				break;
			default:
				return null;
			}
			if (injectionMethod != null)
			{
				injectionMethod.Type = type;
			}
			return injectionMethod;
		}

		// Token: 0x060009C9 RID: 2505 RVA: 0x000404D8 File Offset: 0x0003E6D8
		public virtual IntPtr Inject(PortableExecutable image, int processId)
		{
			this.ClearErrors();
			IntPtr intPtr = WinAPI.OpenProcess(1082U, false, processId);
			IntPtr result = this.Inject(image, intPtr);
			WinAPI.CloseHandle(intPtr);
			return result;
		}

		// Token: 0x060009CA RID: 2506
		public abstract IntPtr Inject(PortableExecutable image, IntPtr hProcess);

		// Token: 0x060009CB RID: 2507 RVA: 0x00040508 File Offset: 0x0003E708
		public virtual IntPtr Inject(string dllPath, int processId)
		{
			this.ClearErrors();
			IntPtr intPtr = WinAPI.OpenProcess(1082U, false, processId);
			IntPtr result = this.Inject(dllPath, intPtr);
			WinAPI.CloseHandle(intPtr);
			return result;
		}

		// Token: 0x060009CC RID: 2508
		public abstract IntPtr Inject(string dllPath, IntPtr hProcess);

		// Token: 0x060009CD RID: 2509 RVA: 0x00040538 File Offset: 0x0003E738
		public virtual IntPtr[] InjectAll(PortableExecutable[] images, int processId)
		{
			this.ClearErrors();
			IntPtr intPtr = WinAPI.OpenProcess(1082U, false, processId);
			IntPtr[] result = this.InjectAll(images, intPtr);
			WinAPI.CloseHandle(intPtr);
			return result;
		}

		// Token: 0x060009CE RID: 2510
		public abstract IntPtr[] InjectAll(PortableExecutable[] images, IntPtr hProcess);

		// Token: 0x060009CF RID: 2511 RVA: 0x00040568 File Offset: 0x0003E768
		public virtual IntPtr[] InjectAll(string[] dllPaths, int processId)
		{
			this.ClearErrors();
			IntPtr intPtr = WinAPI.OpenProcess(1082U, false, processId);
			IntPtr[] result = this.InjectAll(dllPaths, intPtr);
			WinAPI.CloseHandle(intPtr);
			return result;
		}

		// Token: 0x060009D0 RID: 2512
		public abstract IntPtr[] InjectAll(string[] dllPaths, IntPtr hProcess);

		// Token: 0x060009D1 RID: 2513 RVA: 0x00040598 File Offset: 0x0003E798
		public virtual bool Unload(IntPtr hModule, int processId)
		{
			this.ClearErrors();
			IntPtr intPtr = WinAPI.OpenProcess(1082U, false, processId);
			bool result = this.Unload(hModule, intPtr);
			WinAPI.CloseHandle(intPtr);
			return result;
		}

		// Token: 0x060009D2 RID: 2514
		public abstract bool Unload(IntPtr hModule, IntPtr hProcess);

		// Token: 0x060009D3 RID: 2515 RVA: 0x000405C8 File Offset: 0x0003E7C8
		public virtual bool[] UnloadAll(IntPtr[] hModules, int processId)
		{
			this.ClearErrors();
			IntPtr intPtr = WinAPI.OpenProcess(1082U, false, processId);
			bool[] result = this.UnloadAll(hModules, intPtr);
			WinAPI.CloseHandle(intPtr);
			return result;
		}

		// Token: 0x060009D4 RID: 2516
		public abstract bool[] UnloadAll(IntPtr[] hModules, IntPtr hProcess);

		// Token: 0x17000252 RID: 594
		// (get) Token: 0x060009D5 RID: 2517 RVA: 0x000405F7 File Offset: 0x0003E7F7
		// (set) Token: 0x060009D6 RID: 2518 RVA: 0x000405FF File Offset: 0x0003E7FF
		public InjectionMethodType Type { get; protected set; }
	}
}
