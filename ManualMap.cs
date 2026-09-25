using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;

namespace TinhKiemAuto
{
	// Token: 0x020000B5 RID: 181
	internal class ManualMap : InjectionMethod
	{
		// Token: 0x060009FB RID: 2555 RVA: 0x00040A0C File Offset: 0x0003EC0C
		private static byte[] ExtractManifest(PortableExecutable image)
		{
			byte[] result = null;
			ResourceWalker resourceWalker = new ResourceWalker(image);
			ResourceWalker.ResourceDirectory resourceDirectory = null;
			int num = 0;
			while (num < resourceWalker.Root.Directories.Length && resourceDirectory == null)
			{
				if ((long)resourceWalker.Root.Directories[num].Id == 24L)
				{
					resourceDirectory = resourceWalker.Root.Directories[num];
				}
				num++;
			}
			if (resourceDirectory != null && resourceDirectory.Directories.Length != 0 && ManualMap.IsManifestResource(resourceDirectory.Directories[0].Id) && resourceDirectory.Directories[0].Files.Length == 1)
			{
				result = resourceDirectory.Directories[0].Files[0].GetData();
			}
			return result;
		}

		// Token: 0x060009FC RID: 2556 RVA: 0x00040AB0 File Offset: 0x0003ECB0
		private static uint FindEntryPoint(IntPtr hProcess, IntPtr hModule)
		{
			if (hProcess.IsNull() || hProcess.Compare(-1L))
			{
				throw new ArgumentException("Invalid process handle.", "hProcess");
			}
			if (hModule.IsNull())
			{
				throw new ArgumentException("Invalid module handle.", "hModule");
			}
			byte[] array = WinAPI.ReadRemoteMemory(hProcess, hModule, (uint)Marshal.SizeOf(typeof(IMAGE_DOS_HEADER)));
			if (array != null)
			{
				ushort num = BitConverter.ToUInt16(array, 0);
				uint num2 = BitConverter.ToUInt32(array, 60);
				if (num == 23117)
				{
					byte[] array2 = WinAPI.ReadRemoteMemory(hProcess, hModule.Add((long)((ulong)num2)), (uint)Marshal.SizeOf(typeof(IMAGE_NT_HEADER32)));
					if (array2 != null && BitConverter.ToUInt32(array2, 0) == 17744U)
					{
						IMAGE_NT_HEADER32 image_NT_HEADER = default(IMAGE_NT_HEADER32);
						using (UnmanagedBuffer unmanagedBuffer = new UnmanagedBuffer(256))
						{
							if (unmanagedBuffer.Translate<IMAGE_NT_HEADER32>(array2, out image_NT_HEADER))
							{
								return image_NT_HEADER.OptionalHeader.AddressOfEntryPoint;
							}
						}
						return 0U;
					}
				}
			}
			return 0U;
		}

		// Token: 0x060009FD RID: 2557 RVA: 0x00040BB0 File Offset: 0x0003EDB0
		private static IntPtr GetRemoteModuleHandle(string module, int processId)
		{
			IntPtr intPtr = IntPtr.Zero;
			Process processById = Process.GetProcessById(processId);
			int num = 0;
			while (num < processById.Modules.Count && intPtr.IsNull())
			{
				if (processById.Modules[num].ModuleName.ToLower() == module.ToLower())
				{
					intPtr = processById.Modules[num].BaseAddress;
				}
				num++;
			}
			return intPtr;
		}

		// Token: 0x060009FE RID: 2558 RVA: 0x00040C20 File Offset: 0x0003EE20
		public override IntPtr Inject(PortableExecutable image, IntPtr hProcess)
		{
			this.ClearErrors();
			IntPtr result;
			try
			{
				result = ManualMap.MapModule(Utils.DeepClone<PortableExecutable>(image), hProcess, true);
			}
			catch (Exception lastError)
			{
				this.SetLastError(lastError);
				result = IntPtr.Zero;
			}
			return result;
		}

		// Token: 0x060009FF RID: 2559 RVA: 0x00040C68 File Offset: 0x0003EE68
		public override IntPtr Inject(string dllPath, IntPtr hProcess)
		{
			this.ClearErrors();
			IntPtr result;
			try
			{
				using (PortableExecutable portableExecutable = new PortableExecutable(dllPath))
				{
					result = this.Inject(portableExecutable, hProcess);
				}
			}
			catch (Exception lastError)
			{
				this.SetLastError(lastError);
				result = IntPtr.Zero;
			}
			return result;
		}

		// Token: 0x06000A00 RID: 2560 RVA: 0x00040CC8 File Offset: 0x0003EEC8
		public override IntPtr[] InjectAll(PortableExecutable[] images, IntPtr hProcess)
		{
			this.ClearErrors();
			return Array.ConvertAll<PortableExecutable, IntPtr>(images, (PortableExecutable pe) => this.Inject(pe, hProcess));
		}

		// Token: 0x06000A01 RID: 2561 RVA: 0x00040D04 File Offset: 0x0003EF04
		public override IntPtr[] InjectAll(string[] dllPaths, IntPtr hProcess)
		{
			this.ClearErrors();
			return Array.ConvertAll<string, IntPtr>(dllPaths, (string dp) => this.Inject(dp, hProcess));
		}

		// Token: 0x06000A02 RID: 2562 RVA: 0x00040D3D File Offset: 0x0003EF3D
		private static bool IsManifestResource(int id)
		{
			return id - 1 <= 2;
		}

		// Token: 0x06000A03 RID: 2563 RVA: 0x00040D48 File Offset: 0x0003EF48
		private static bool LoadDependencies(PortableExecutable image, IntPtr hProcess, int processId)
		{
			List<string> list = new List<string>();
			string empty = string.Empty;
			bool result = false;
			foreach (IMAGE_IMPORT_DESCRIPTOR image_IMPORT_DESCRIPTOR in image.EnumImports())
			{
				if (image.ReadString((long)((ulong)image.GetPtrFromRVA(image_IMPORT_DESCRIPTOR.Name)), SeekOrigin.Begin, out empty, -1, null) && !string.IsNullOrEmpty(empty) && ManualMap.GetRemoteModuleHandle(empty, processId).IsNull())
				{
					list.Add(empty);
				}
			}
			if (list.Count > 0)
			{
				byte[] array = ManualMap.ExtractManifest(image);
				string text = string.Empty;
				if (array == null)
				{
					if (string.IsNullOrEmpty(image.FileLocation) || !File.Exists(Path.Combine(Path.GetDirectoryName(image.FileLocation), Path.GetFileName(image.FileLocation) + ".manifest")))
					{
						IntPtr[] array2 = InjectionMethod.Create(InjectionMethodType.Standard).InjectAll(list.ToArray(), hProcess);
						for (int i = 0; i < array2.Length; i++)
						{
							if (array2[i].IsNull())
							{
								return false;
							}
						}
						return true;
					}
					text = Path.Combine(Path.GetDirectoryName(image.FileLocation), Path.GetFileName(image.FileLocation) + ".manifest");
				}
				else
				{
					text = Utils.WriteTempData(array);
				}
				if (string.IsNullOrEmpty(text))
				{
					return false;
				}
				IntPtr intPtr = WinAPI.VirtualAllocEx(hProcess, IntPtr.Zero, (uint)ManualMap.RESOLVER_STUB.Length, 12288, 64);
				IntPtr lpAddress = WinAPI.CreateRemotePointer(hProcess, Encoding.ASCII.GetBytes(text + "\0"), 4);
				IntPtr lpAddress2 = WinAPI.CreateRemotePointer(hProcess, Encoding.ASCII.GetBytes(string.Join("\0", list.ToArray()) + "\0"), 4);
				if (!intPtr.IsNull())
				{
					byte[] array3 = (byte[])ManualMap.RESOLVER_STUB.Clone();
					uint num = 0U;
					BitConverter.GetBytes(ManualMap.FN_CREATEACTCTXA.Subtract(intPtr.Add(63L)).ToInt32()).CopyTo(array3, 59);
					BitConverter.GetBytes(ManualMap.FN_ACTIVATEACTCTX.Subtract(intPtr.Add(88L)).ToInt32()).CopyTo(array3, 84);
					BitConverter.GetBytes(ManualMap.FN_GETMODULEHANDLEA.Subtract(intPtr.Add(132L)).ToInt32()).CopyTo(array3, 128);
					BitConverter.GetBytes(ManualMap.FN_LOADLIBRARYA.Subtract(intPtr.Add(146L)).ToInt32()).CopyTo(array3, 142);
					BitConverter.GetBytes(ManualMap.FN_DEACTIVATEACTCTX.Subtract(intPtr.Add(200L)).ToInt32()).CopyTo(array3, 196);
					BitConverter.GetBytes(ManualMap.FN_RELEASEACTCTX.Subtract(intPtr.Add(209L)).ToInt32()).CopyTo(array3, 205);
					BitConverter.GetBytes(lpAddress.ToInt32()).CopyTo(array3, 31);
					BitConverter.GetBytes(list.Count).CopyTo(array3, 40);
					BitConverter.GetBytes(lpAddress2.ToInt32()).CopyTo(array3, 49);
					if (WinAPI.WriteProcessMemory(hProcess, intPtr, array3, array3.Length, out num) && (ulong)num == (ulong)((long)array3.Length))
					{
						uint num2 = WinAPI.RunThread(hProcess, intPtr, 0U, 5000);
						result = (num2 != uint.MaxValue && num2 > 0U);
					}
					WinAPI.VirtualFreeEx(hProcess, lpAddress2, 0, 32768);
					WinAPI.VirtualFreeEx(hProcess, lpAddress, 0, 32768);
					WinAPI.VirtualFreeEx(hProcess, intPtr, 0, 32768);
				}
			}
			return result;
		}

		// Token: 0x06000A04 RID: 2564 RVA: 0x000410EC File Offset: 0x0003F2EC
		private static IntPtr MapModule(PortableExecutable image, IntPtr hProcess, bool preserveHeaders = false)
		{
			if (hProcess.IsNull() || hProcess.Compare(-1L))
			{
				throw new ArgumentException("Invalid process handle.", "hProcess");
			}
			if (image == null)
			{
				throw new ArgumentException("Cannot map a non-existant PE Image.", "image");
			}
			int processId = WinAPI.GetProcessId(hProcess);
			if (processId == 0)
			{
				throw new ArgumentException("Provided handle doesn't have sufficient permissions to inject", "hProcess");
			}
			IntPtr intPtr = IntPtr.Zero;
			IntPtr intPtr2 = IntPtr.Zero;
			uint num = 0U;
			try
			{
				intPtr = WinAPI.VirtualAllocEx(hProcess, IntPtr.Zero, image.NTHeader.OptionalHeader.SizeOfImage, 12288, 4);
				if (intPtr.IsNull())
				{
					throw new InvalidOperationException("Unable to allocate memory in the remote process.");
				}
				ManualMap.PatchRelocations(image, intPtr);
				ManualMap.LoadDependencies(image, hProcess, processId);
				ManualMap.PatchImports(image, hProcess, processId);
				if (preserveHeaders)
				{
					byte[] array = new byte[(ulong)image.DOSHeader.e_lfanew + (ulong)((long)Marshal.SizeOf(typeof(IMAGE_FILE_HEADER))) + 4UL + (ulong)image.NTHeader.FileHeader.SizeOfOptionalHeader];
					if (image.Read(0L, SeekOrigin.Begin, array))
					{
						WinAPI.WriteProcessMemory(hProcess, intPtr, array, array.Length, out num);
					}
				}
				ManualMap.MapSections(image, hProcess, intPtr);
				if (image.NTHeader.OptionalHeader.AddressOfEntryPoint <= 0U)
				{
					return intPtr;
				}
				byte[] array2 = (byte[])ManualMap.DLLMAIN_STUB.Clone();
				BitConverter.GetBytes(intPtr.ToInt32()).CopyTo(array2, 11);
				intPtr2 = WinAPI.VirtualAllocEx(hProcess, IntPtr.Zero, (uint)ManualMap.DLLMAIN_STUB.Length, 12288, 64);
				if (intPtr2.IsNull() || !WinAPI.WriteProcessMemory(hProcess, intPtr2, array2, array2.Length, out num) || (ulong)num != (ulong)((long)array2.Length))
				{
					throw new InvalidOperationException("Unable to write stub to the remote process.");
				}
				IntPtr intPtr3 = WinAPI.CreateRemoteThread(hProcess, 0, 0, intPtr2, (uint)intPtr.Add((long)((ulong)image.NTHeader.OptionalHeader.AddressOfEntryPoint)).ToInt32(), 0, 0);
				if ((ulong)WinAPI.WaitForSingleObject(intPtr3, 5000) != 0UL)
				{
					return intPtr;
				}
				WinAPI.GetExitCodeThread(intPtr3, out num);
				if (num == 0U)
				{
					WinAPI.VirtualFreeEx(hProcess, intPtr, 0, 32768);
					throw new Exception("Entry method of module reported a failure " + Marshal.GetLastWin32Error().ToString());
				}
				WinAPI.VirtualFreeEx(hProcess, intPtr2, 0, 32768);
				WinAPI.CloseHandle(intPtr3);
			}
			catch (Exception ex)
			{
				if (!intPtr.IsNull())
				{
					WinAPI.VirtualFreeEx(hProcess, intPtr, 0, 32768);
				}
				if (!intPtr2.IsNull())
				{
					WinAPI.VirtualFreeEx(hProcess, intPtr, 0, 32768);
				}
				intPtr = IntPtr.Zero;
				throw ex;
			}
			return intPtr;
		}

		// Token: 0x06000A05 RID: 2565 RVA: 0x00041378 File Offset: 0x0003F578
		private static void MapSections(PortableExecutable image, IntPtr hProcess, IntPtr pModule)
		{
			foreach (IMAGE_SECTION_HEADER image_SECTION_HEADER in image.EnumSectionHeaders())
			{
				byte[] array = new byte[image_SECTION_HEADER.SizeOfRawData];
				if (!image.Read((long)((ulong)image_SECTION_HEADER.PointerToRawData), SeekOrigin.Begin, array))
				{
					throw image.GetLastError();
				}
				if ((image_SECTION_HEADER.Characteristics & 33554432U) == 0U)
				{
					uint num;
					WinAPI.WriteProcessMemory(hProcess, pModule.Add((long)((ulong)image_SECTION_HEADER.VirtualAddress)), array, array.Length, out num);
					IntPtr lpAddress = pModule.Add((long)((ulong)image_SECTION_HEADER.VirtualAddress));
					WinAPI.VirtualProtectEx(hProcess, lpAddress, image_SECTION_HEADER.SizeOfRawData, image_SECTION_HEADER.Characteristics & 16777215U, out num);
				}
			}
		}

		// Token: 0x06000A06 RID: 2566 RVA: 0x0004143C File Offset: 0x0003F63C
		private static void PatchImports(PortableExecutable image, IntPtr hProcess, int processId)
		{
			string empty = string.Empty;
			string empty2 = string.Empty;
			foreach (IMAGE_IMPORT_DESCRIPTOR image_IMPORT_DESCRIPTOR in image.EnumImports())
			{
				if (image.ReadString((long)((ulong)image.GetPtrFromRVA(image_IMPORT_DESCRIPTOR.Name)), SeekOrigin.Begin, out empty, -1, null))
				{
					IntPtr intPtr = IntPtr.Zero;
					intPtr = ManualMap.GetRemoteModuleHandle(empty, processId);
					if (intPtr.IsNull())
					{
						throw new FileNotFoundException(string.Format("Unable to load dependent module '{0}'.", empty));
					}
					uint num = image.GetPtrFromRVA(image_IMPORT_DESCRIPTOR.FirstThunkPtr);
					uint num2 = (uint)Marshal.SizeOf(typeof(IMAGE_THUNK_DATA));
					IMAGE_THUNK_DATA image_THUNK_DATA;
					while (image.Read<IMAGE_THUNK_DATA>((long)((ulong)num), SeekOrigin.Begin, out image_THUNK_DATA) && image_THUNK_DATA.u1.AddressOfData > 0U)
					{
						IntPtr intPtr2 = IntPtr.Zero;
						object obj;
						if ((image_THUNK_DATA.u1.Ordinal & 2147483648U) == 0U)
						{
							if (!image.ReadString((long)((ulong)(image.GetPtrFromRVA(image_THUNK_DATA.u1.AddressOfData) + 2U)), SeekOrigin.Begin, out empty2, -1, null))
							{
								throw image.GetLastError();
							}
							obj = empty2;
						}
						else
						{
							obj = (ushort)(image_THUNK_DATA.u1.Ordinal & 65535U);
						}
						if (!(intPtr2 = WinAPI.GetModuleHandleA(empty)).IsNull())
						{
							IntPtr ptr = obj.GetType().Equals(typeof(string)) ? WinAPI.GetProcAddress(intPtr2, (string)obj) : WinAPI.GetProcAddress(intPtr2, (uint)((ushort)obj & ushort.MaxValue));
							if (!ptr.IsNull())
							{
								intPtr2 = intPtr.Add((long)ptr.Subtract((long)intPtr2.ToInt32()).ToInt32());
							}
						}
						else
						{
							intPtr2 = WinAPI.GetProcAddressEx(hProcess, intPtr, obj);
						}
						if (intPtr2.IsNull())
						{
							throw new EntryPointNotFoundException(string.Format("Unable to locate imported function '{0}' from module '{1}' in the remote process.", empty2, empty));
						}
						image.Write<int>((long)((ulong)num), SeekOrigin.Begin, intPtr2.ToInt32());
						num += num2;
					}
				}
			}
		}

		// Token: 0x06000A07 RID: 2567 RVA: 0x0004164C File Offset: 0x0003F84C
		private static void PatchRelocations(PortableExecutable image, IntPtr pAlloc)
		{
			IMAGE_DATA_DIRECTORY image_DATA_DIRECTORY = image.NTHeader.OptionalHeader.DataDirectory[5];
			if (image_DATA_DIRECTORY.Size > 0U)
			{
				uint num = 0U;
				uint num2 = (uint)(pAlloc.ToInt32() - (int)image.NTHeader.OptionalHeader.ImageBase);
				uint num3 = image.GetPtrFromRVA(image_DATA_DIRECTORY.VirtualAddress);
				uint num4 = (uint)Marshal.SizeOf(typeof(IMAGE_BASE_RELOCATION));
				IMAGE_BASE_RELOCATION image_BASE_RELOCATION;
				while (num < image_DATA_DIRECTORY.Size && image.Read<IMAGE_BASE_RELOCATION>((long)((ulong)num3), SeekOrigin.Begin, out image_BASE_RELOCATION))
				{
					int num5 = (int)((image_BASE_RELOCATION.SizeOfBlock - num4) / 2U);
					uint ptrFromRVA = image.GetPtrFromRVA(image_BASE_RELOCATION.VirtualAddress);
					for (int i = 0; i < num5; i++)
					{
						ushort num6;
						if (image.Read<ushort>((long)((ulong)(num3 + num4) + (ulong)((ulong)((long)i) << 1)), SeekOrigin.Begin, out num6) && (num6 >> 12 & 3) != 0)
						{
							uint num7 = ptrFromRVA + (uint)(num6 & 4095);
							uint num8;
							if (!image.Read<uint>((long)((ulong)num7), SeekOrigin.Begin, out num8))
							{
								throw image.GetLastError();
							}
							image.Write<uint>(-4L, SeekOrigin.Current, num8 + num2);
						}
					}
					num += image_BASE_RELOCATION.SizeOfBlock;
					num3 += image_BASE_RELOCATION.SizeOfBlock;
				}
			}
		}

		// Token: 0x06000A08 RID: 2568 RVA: 0x00041768 File Offset: 0x0003F968
		public override bool Unload(IntPtr hModule, IntPtr hProcess)
		{
			this.ClearErrors();
			if (hModule.IsNull())
			{
				throw new ArgumentNullException("hModule", "Invalid module handle");
			}
			if (hProcess.IsNull() || hProcess.Compare(-1L))
			{
				throw new ArgumentException("Invalid process handle.", "hProcess");
			}
			IntPtr intPtr = IntPtr.Zero;
			uint num = 0U;
			bool result;
			try
			{
				uint num2 = ManualMap.FindEntryPoint(hProcess, hModule);
				if (num2 != 0U)
				{
					byte[] array = (byte[])ManualMap.DLLMAIN_STUB.Clone();
					BitConverter.GetBytes(hModule.ToInt32()).CopyTo(array, 11);
					BitConverter.GetBytes(0U).CopyTo(array, 6);
					BitConverter.GetBytes(1000U).CopyTo(array, 1);
					intPtr = WinAPI.VirtualAllocEx(hProcess, IntPtr.Zero, (uint)ManualMap.DLLMAIN_STUB.Length, 12288, 64);
					if (intPtr.IsNull() || !WinAPI.WriteProcessMemory(hProcess, intPtr, array, array.Length, out num) || (ulong)num != (ulong)((long)array.Length))
					{
						throw new InvalidOperationException("Unable to write stub to the remote process.");
					}
					IntPtr intPtr2 = WinAPI.CreateRemoteThread(hProcess, 0, 0, intPtr, (uint)hModule.Add((long)((ulong)num2)).ToInt32(), 0, 0);
					if ((ulong)WinAPI.WaitForSingleObject(intPtr2, 5000) == 0UL)
					{
						WinAPI.VirtualFreeEx(hProcess, intPtr, 0, 32768);
						WinAPI.CloseHandle(intPtr2);
						result = WinAPI.VirtualFreeEx(hProcess, hModule, 0, 32768);
					}
					else
					{
						result = false;
					}
				}
				else
				{
					result = WinAPI.VirtualFreeEx(hProcess, hModule, 0, 32768);
				}
			}
			catch (Exception lastError)
			{
				this.SetLastError(lastError);
				result = false;
			}
			return result;
		}

		// Token: 0x06000A09 RID: 2569 RVA: 0x000418EC File Offset: 0x0003FAEC
		public override bool[] UnloadAll(IntPtr[] hModules, IntPtr hProcess)
		{
			this.ClearErrors();
			if (hModules == null)
			{
				throw new ArgumentNullException("hModules", "Parameter cannot be null.");
			}
			if (hProcess.IsNull() || hProcess.Compare(-1L))
			{
				throw new ArgumentOutOfRangeException("hProcess", "Invalid process handle specified.");
			}
			bool[] result;
			try
			{
				bool[] array = new bool[hModules.Length];
				for (int i = 0; i < hModules.Length; i++)
				{
					array[i] = this.Unload(hModules[i], hProcess);
				}
				result = array;
			}
			catch (Exception lastError)
			{
				this.SetLastError(lastError);
				result = null;
			}
			return result;
		}

		// Token: 0x04000741 RID: 1857
		private static readonly byte[] DLLMAIN_STUB = new byte[]
		{
			104,
			0,
			0,
			0,
			0,
			104,
			1,
			0,
			0,
			0,
			104,
			0,
			0,
			0,
			0,
			byte.MaxValue,
			84,
			36,
			16,
			195
		};

		// Token: 0x04000742 RID: 1858
		private static readonly IntPtr FN_ACTIVATEACTCTX = WinAPI.GetProcAddress(ManualMap.H_KERNEL32, "ActivateActCtx");

		// Token: 0x04000743 RID: 1859
		private static readonly IntPtr FN_CREATEACTCTXA = WinAPI.GetProcAddress(ManualMap.H_KERNEL32, "CreateActCtxA");

		// Token: 0x04000744 RID: 1860
		private static readonly IntPtr FN_DEACTIVATEACTCTX = WinAPI.GetProcAddress(ManualMap.H_KERNEL32, "DeactivateActCtx");

		// Token: 0x04000745 RID: 1861
		private static readonly IntPtr FN_GETMODULEHANDLEA = WinAPI.GetProcAddress(ManualMap.H_KERNEL32, "GetModuleHandleA");

		// Token: 0x04000746 RID: 1862
		private static readonly IntPtr FN_LOADLIBRARYA = WinAPI.GetProcAddress(ManualMap.H_KERNEL32, "LoadLibraryA");

		// Token: 0x04000747 RID: 1863
		private static readonly IntPtr FN_RELEASEACTCTX = WinAPI.GetProcAddress(ManualMap.H_KERNEL32, "ReleaseActCtx");

		// Token: 0x04000748 RID: 1864
		private static readonly IntPtr H_KERNEL32 = WinAPI.GetModuleHandleA("KERNEL32.dll");

		// Token: 0x04000749 RID: 1865
		private static readonly byte[] RESOLVER_STUB = new byte[]
		{
			85,
			139,
			236,
			131,
			236,
			60,
			139,
			204,
			139,
			209,
			131,
			194,
			60,
			199,
			1,
			0,
			0,
			0,
			0,
			131,
			193,
			4,
			59,
			202,
			126,
			243,
			198,
			4,
			36,
			32,
			185,
			0,
			0,
			0,
			0,
			137,
			76,
			36,
			8,
			185,
			0,
			0,
			0,
			0,
			137,
			76,
			36,
			40,
			185,
			0,
			0,
			0,
			0,
			137,
			76,
			36,
			44,
			84,
			232,
			0,
			0,
			0,
			0,
			131,
			56,
			byte.MaxValue,
			15,
			132,
			137,
			0,
			0,
			0,
			137,
			68,
			36,
			48,
			139,
			204,
			131,
			193,
			32,
			81,
			80,
			232,
			0,
			0,
			0,
			0,
			131,
			248,
			0,
			116,
			107,
			198,
			68,
			36,
			36,
			1,
			139,
			76,
			36,
			40,
			131,
			249,
			0,
			126,
			62,
			131,
			233,
			1,
			137,
			76,
			36,
			40,
			139,
			76,
			36,
			36,
			131,
			249,
			0,
			116,
			46,
			byte.MaxValue,
			116,
			36,
			44,
			232,
			0,
			0,
			0,
			0,
			131,
			248,
			0,
			117,
			9,
			byte.MaxValue,
			116,
			36,
			44,
			232,
			0,
			0,
			0,
			0,
			137,
			68,
			36,
			36,
			139,
			76,
			36,
			44,
			138,
			1,
			131,
			193,
			1,
			60,
			0,
			117,
			247,
			137,
			76,
			36,
			44,
			235,
			185,
			139,
			68,
			36,
			36,
			185,
			1,
			0,
			0,
			0,
			35,
			193,
			137,
			76,
			36,
			36,
			131,
			249,
			0,
			117,
			20,
			byte.MaxValue,
			116,
			36,
			32,
			106,
			0,
			232,
			0,
			0,
			0,
			0,
			byte.MaxValue,
			116,
			36,
			48,
			232,
			0,
			0,
			0,
			0,
			139,
			68,
			36,
			36,
			139,
			229,
			93,
			195
		};
	}
}
