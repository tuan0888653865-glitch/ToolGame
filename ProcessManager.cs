using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace TinhKiemAuto
{
	// Token: 0x020000CE RID: 206
	internal class ProcessManager
	{
		// Token: 0x06000AF5 RID: 2805 RVA: 0x00048A20 File Offset: 0x00046C20
		public static List<Modules> CollectModules(Process process)
		{
			List<Modules> list = new List<Modules>();
			IntPtr[] array = new IntPtr[0];
			int num = 0;
			if (!Native.EnumProcessModulesEx(process.Handle, array, 0, out num, 3U))
			{
				return list;
			}
			int num2 = num / IntPtr.Size;
			array = new IntPtr[num2];
			if (Native.EnumProcessModulesEx(process.Handle, array, num, out num, 3U))
			{
				for (int i = 0; i < num2; i++)
				{
					StringBuilder stringBuilder = new StringBuilder(1024);
					Native.GetModuleFileNameEx(process.Handle, array[i], stringBuilder, (uint)stringBuilder.Capacity);
					string fileName = Path.GetFileName(stringBuilder.ToString());
					Native.ModuleInformation moduleInformation = default(Native.ModuleInformation);
					Native.GetModuleInformation(process.Handle, array[i], out moduleInformation, (uint)(IntPtr.Size * array.Length));
					Modules item = new Modules(fileName, moduleInformation.lpBaseOfDll, moduleInformation.SizeOfImage);
					list.Add(item);
				}
			}
			return list;
		}
	}
}
