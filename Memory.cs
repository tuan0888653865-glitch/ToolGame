using System;
using System.Diagnostics;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Text;

namespace TinhKiemAuto
{
	// Token: 0x020000B8 RID: 184
	public class Memory
	{
		// Token: 0x17000254 RID: 596
		// (get) Token: 0x06000A11 RID: 2577 RVA: 0x000421AD File Offset: 0x000403AD
		public IntPtr Id
		{
			get
			{
				return this.id;
			}
		}

		// Token: 0x17000255 RID: 597
		// (get) Token: 0x06000A12 RID: 2578 RVA: 0x000421B5 File Offset: 0x000403B5
		// (set) Token: 0x06000A13 RID: 2579 RVA: 0x000421BD File Offset: 0x000403BD
		public int ProcessID { get; set; }

		// Token: 0x06000A14 RID: 2580 RVA: 0x000421C6 File Offset: 0x000403C6
		public Memory(int processId)
		{
			this.ProcessID = processId;
			this.id = Memory.OpenProcess(2035711, false, processId);
		}

		// Token: 0x06000A15 RID: 2581 RVA: 0x00007E59 File Offset: 0x00006059
		public static int Char2Byte(char c)
		{
			return (int)c;
		}

		// Token: 0x06000A16 RID: 2582 RVA: 0x000421E8 File Offset: 0x000403E8
		public static int Hex2Int(string hex)
		{
			if (hex == "??")
			{
				return -1;
			}
			int result = -1;
			int.TryParse(hex, NumberStyles.HexNumber, CultureInfo.InvariantCulture, out result);
			return result;
		}

		// Token: 0x06000A17 RID: 2583 RVA: 0x0004221C File Offset: 0x0004041C
		public static byte Hex2Byte(string hex)
		{
			byte result = 0;
			byte.TryParse(hex, NumberStyles.HexNumber, CultureInfo.InvariantCulture, out result);
			return result;
		}

		// Token: 0x06000A18 RID: 2584 RVA: 0x00042240 File Offset: 0x00040440
		public static byte[] Hex2ByteArr(string hex)
		{
			byte[] array = new byte[hex.Length / 2];
			for (int i = 0; i < array.Length; i++)
			{
				array[i] = Memory.Hex2Byte(hex.Substring(i * 2, 2));
			}
			return array;
		}

		// Token: 0x06000A19 RID: 2585 RVA: 0x0004227C File Offset: 0x0004047C
		public int WriteHex(string hex)
		{
			byte[] array = Memory.Hex2ByteArr(hex.Replace(" ", ""));
			int num = Memory.VirtualAllocEx(this.id, 0, array.Length + 16, 4096U, 64U);
			Memory.WriteProcessMemory(this.id, num, array, array.Length, 0);
			return num;
		}

		// Token: 0x06000A1A RID: 2586 RVA: 0x000422CC File Offset: 0x000404CC
		public int GetModuleAddress(string moduleName)
		{
			Process processById = Process.GetProcessById(this.ProcessID);
			for (int i = 0; i < processById.Modules.Count; i++)
			{
				if (processById.Modules[i].ModuleName.ToLower() == moduleName.ToLower())
				{
					return (int)processById.Modules[i].BaseAddress;
				}
			}
			return 0;
		}

		// Token: 0x06000A1B RID: 2587 RVA: 0x00042338 File Offset: 0x00040538
		public string ReverseString(string input)
		{
			string text = "";
			if (input.Length % 2 != 0)
			{
				input = "0" + input;
			}
			for (int i = input.Length / 2 - 1; i >= 0; i--)
			{
				text += input.Substring(i * 2, 2);
			}
			return text;
		}

		// Token: 0x06000A1C RID: 2588 RVA: 0x0004238C File Offset: 0x0004058C
		public static bool Compare(byte[] input, int[] pattern, int index)
		{
			for (int i = 0; i < pattern.Length; i++)
			{
				if (pattern[i] != -1)
				{
					if (pattern[i] == 257)
					{
						if (input[index + i] == 0)
						{
							return false;
						}
					}
					else if ((int)input[index + i] != pattern[i])
					{
						return false;
					}
				}
			}
			return true;
		}

		// Token: 0x06000A1D RID: 2589 RVA: 0x000423D0 File Offset: 0x000405D0
		public int Scan(string hex, string moduleName)
		{
			Process processById = Process.GetProcessById(this.ProcessID);
			for (int i = 0; i < processById.Modules.Count; i++)
			{
				if (processById.Modules[i].ModuleName.ToLower() == moduleName.ToLower())
				{
					return this.Scan(hex, (int)processById.Modules[i].BaseAddress, (int)processById.Modules[i].BaseAddress + processById.Modules[i].ModuleMemorySize, 0);
				}
			}
			return 0;
		}

		// Token: 0x06000A1E RID: 2590 RVA: 0x0004246C File Offset: 0x0004066C
		public int Scan(string hex, string moduleName, int index)
		{
			Process processById = Process.GetProcessById(this.ProcessID);
			for (int i = 0; i < processById.Modules.Count; i++)
			{
				if (processById.Modules[i].ModuleName.ToLower() == moduleName.ToLower())
				{
					return this.Scan(hex, (int)processById.Modules[i].BaseAddress, (int)processById.Modules[i].BaseAddress + processById.Modules[i].ModuleMemorySize, index);
				}
			}
			return 0;
		}

		// Token: 0x06000A1F RID: 2591 RVA: 0x00042506 File Offset: 0x00040706
		public int Scan(string hex)
		{
			return this.Scan(hex, -1, -1, 0);
		}

		// Token: 0x06000A20 RID: 2592 RVA: 0x00042512 File Offset: 0x00040712
		public int Scan(string hex, int index)
		{
			return this.Scan(hex, -1, -1, index);
		}

		// Token: 0x06000A21 RID: 2593 RVA: 0x0004251E File Offset: 0x0004071E
		public int ScanString(string s, int startAdd, int endAdd, int index)
		{
			return this.Scan(ConverterEx.String2Hex(s), startAdd, endAdd, index);
		}

		// Token: 0x06000A22 RID: 2594 RVA: 0x00042530 File Offset: 0x00040730
		public int ScanString(string s)
		{
			return this.Scan(ConverterEx.String2Hex(s));
		}

		// Token: 0x06000A23 RID: 2595 RVA: 0x00042540 File Offset: 0x00040740
		public int Scan(string hex, int startAddress, int endAddress, int index)
		{
			int num = 0;
			int result = 0;
			try
			{
				if (startAddress == -1)
				{
					startAddress = (int)Process.GetProcessById(this.ProcessID).MainModule.BaseAddress;
				}
				if (endAddress == -1)
				{
					endAddress = startAddress + Process.GetProcessById(this.ProcessID).MainModule.ModuleMemorySize;
				}
			}
			catch
			{
				startAddress = 262144;
				endAddress = 8978431;
			}
			if (startAddress == 0 && endAddress == 0)
			{
				startAddress = 0;
				endAddress = int.MaxValue;
			}
			int[] array = ConverterEx.Hex2IntArr(hex);
			byte[] array2 = new byte[Memory.BufferSize + array.Length];
			int num2 = (endAddress - startAddress) / Memory.BufferSize;
			int num3 = (endAddress - startAddress) % Memory.BufferSize;
			for (int i = 0; i < num2; i++)
			{
				Memory.ReadProcessMemory(this.Id, startAddress + i * Memory.BufferSize, array2, array2.Length, out this.BytesCount);
				for (int j = 0; j < Memory.BufferSize; j++)
				{
					if (Memory.Compare(array2, array, j))
					{
						int num4 = j + i * Memory.BufferSize + startAddress;
						if (num++ >= index)
						{
							return num4;
						}
						result = num4;
					}
				}
			}
			if (num3 > 0)
			{
				array2 = new byte[num3 + array.Length];
				Memory.ReadProcessMemory(this.Id, startAddress + num2 * Memory.BufferSize, array2, array2.Length, 0);
				for (int k = 0; k < num3; k++)
				{
					if (Memory.Compare(array2, array, k))
					{
						int num5 = k + startAddress + num2 * Memory.BufferSize;
						if (num++ >= index)
						{
							return num5;
						}
						result = num5;
					}
				}
			}
			return result;
		}

		// Token: 0x06000A24 RID: 2596 RVA: 0x000426C4 File Offset: 0x000408C4
		public int[] ToArr(int address, int offset)
		{
			return new int[]
			{
				address,
				offset
			};
		}

		// Token: 0x06000A25 RID: 2597 RVA: 0x000426D4 File Offset: 0x000408D4
		public int[] ToArr(int address, int[] offset)
		{
			int[] array = new int[offset.Length + 1];
			array[0] = address;
			offset.CopyTo(array, 1);
			return array;
		}

		// Token: 0x06000A26 RID: 2598 RVA: 0x000426FC File Offset: 0x000408FC
		public int Read(int address)
		{
			byte[] array = new byte[4];
			Memory.ReadProcessMemory(this.id, address, array, 4, 0);
			return BitConverter.ToInt32(array, 0);
		}

		// Token: 0x06000A27 RID: 2599 RVA: 0x00042728 File Offset: 0x00040928
		public int Read1Byte(int address)
		{
			byte[] array = new byte[4];
			Memory.ReadProcessMemory(this.id, address, array, 1, 0);
			return BitConverter.ToInt32(array, 0);
		}

		// Token: 0x06000A28 RID: 2600 RVA: 0x00042754 File Offset: 0x00040954
		public int Read1Byte(int address, int offset)
		{
			address = this.ReadAddress(address, offset);
			byte[] array = new byte[4];
			Memory.ReadProcessMemory(this.id, address, array, 1, 0);
			return BitConverter.ToInt32(array, 0);
		}

		// Token: 0x06000A29 RID: 2601 RVA: 0x0004278C File Offset: 0x0004098C
		public int Read2Byte(int address)
		{
			byte[] array = new byte[4];
			Memory.ReadProcessMemory(this.id, address, array, 2, 0);
			return BitConverter.ToInt32(array, 0);
		}

		// Token: 0x06000A2A RID: 2602 RVA: 0x000427B8 File Offset: 0x000409B8
		public int Read2Byte(int address, int offset)
		{
			address = this.ReadAddress(address, offset);
			byte[] array = new byte[4];
			Memory.ReadProcessMemory(this.id, address, array, 2, 0);
			return BitConverter.ToInt32(array, 0);
		}

		// Token: 0x06000A2B RID: 2603 RVA: 0x000427F0 File Offset: 0x000409F0
		public ulong Read8Byte(int address)
		{
			byte[] array = new byte[8];
			Memory.ReadProcessMemory(this.id, address, array, 8, 0);
			return BitConverter.ToUInt64(array, 0);
		}

		// Token: 0x06000A2C RID: 2604 RVA: 0x0004281C File Offset: 0x00040A1C
		public ulong Read8Byte(int[] offsets)
		{
			byte[] array = new byte[8];
			int lpBaseAddress = this.ReadAddress(offsets);
			Memory.ReadProcessMemory(this.id, lpBaseAddress, array, 8, 0);
			return BitConverter.ToUInt64(array, 0);
		}

		// Token: 0x06000A2D RID: 2605 RVA: 0x0004284F File Offset: 0x00040A4F
		public int Read(int address, int offset)
		{
			address = this.Read(address);
			address = this.Read(address + offset);
			return address;
		}

		// Token: 0x06000A2E RID: 2606 RVA: 0x00042866 File Offset: 0x00040A66
		public int Read(int address, int[] offsets)
		{
			return this.Read(this.ToArr(address, offsets));
		}

		// Token: 0x06000A2F RID: 2607 RVA: 0x00042876 File Offset: 0x00040A76
		public int ReadPointer(int address, int[] offsets)
		{
			offsets[0] = address + offsets[0];
			return this.Read(offsets);
		}

		// Token: 0x06000A30 RID: 2608 RVA: 0x00042888 File Offset: 0x00040A88
		public int Read(int[] pointer, int offset)
		{
			int num = this.Read(pointer);
			return this.Read(num + offset);
		}

		// Token: 0x06000A31 RID: 2609 RVA: 0x000428A8 File Offset: 0x00040AA8
		public int Read(int[] offsets)
		{
			int num = this.Read(offsets[0]);
			for (int i = 1; i < offsets.Length; i++)
			{
				num = this.Read(num + offsets[i]);
			}
			return num;
		}

		// Token: 0x06000A32 RID: 2610 RVA: 0x000428DC File Offset: 0x00040ADC
		public bool IsRead(int[] offsets)
		{
			byte[] array = new byte[4];
			int num;
			Memory.ReadProcessMemory(this.id, offsets[0], array, 4, out num);
			int num2 = BitConverter.ToInt32(array, 0);
			if (num == 0)
			{
				return false;
			}
			for (int i = 1; i < offsets.Length - 1; i++)
			{
				num = 0;
				Memory.ReadProcessMemory(this.id, num2 + offsets[i], array, 4, out num);
				if (num == 0)
				{
					return false;
				}
				num2 = BitConverter.ToInt32(array, 0);
			}
			if (offsets.Length > 1)
			{
				num = 0;
				Memory.ReadProcessMemory(this.id, num2 + offsets[offsets.Length - 1], array, 4, out num);
				if (num == 0)
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x06000A33 RID: 2611 RVA: 0x0004296C File Offset: 0x00040B6C
		public float ReadFloat(int address)
		{
			byte[] array = new byte[4];
			Memory.ReadProcessMemory(this.id, address, array, 4, 0);
			return BitConverter.ToSingle(array, 0);
		}

		// Token: 0x06000A34 RID: 2612 RVA: 0x00042997 File Offset: 0x00040B97
		public float ReadFloat(int address, int offset)
		{
			return this.ReadFloat(this.Read(address) + offset);
		}

		// Token: 0x06000A35 RID: 2613 RVA: 0x000429A8 File Offset: 0x00040BA8
		public float ReadFloat(int[] offsets)
		{
			return this.ReadFloat(this.ReadAddress(offsets));
		}

		// Token: 0x06000A36 RID: 2614 RVA: 0x000429B8 File Offset: 0x00040BB8
		public string ReadString(int address)
		{
			byte[] array = new byte[500];
			Memory.ReadProcessMemory(this.id, address, array, 500, 0);
			return Memory.VISCII2Unicode(array);
		}

		// Token: 0x06000A37 RID: 2615 RVA: 0x000429EC File Offset: 0x00040BEC
		public string ReadStringWithLength(int address, int length)
		{
			byte[] array = new byte[length];
			Memory.ReadProcessMemory(this.id, address, array, length, 0);
			return Memory.VISCII2Unicode(array);
		}

		// Token: 0x06000A38 RID: 2616 RVA: 0x00042A18 File Offset: 0x00040C18
		public string ReadCodeHanler(int address, int length)
		{
			int num = (int)Process.GetProcessById(this.ProcessID).MainModule.BaseAddress;
			byte[] array = new byte[length];
			Memory.ReadProcessMemory(this.id, num + address, array, length, 0);
			return Encoding.Default.GetString(array);
		}

		// Token: 0x06000A39 RID: 2617 RVA: 0x00042A64 File Offset: 0x00040C64
		public string ReadShortString(int address)
		{
			byte[] array = new byte[60];
			Memory.ReadProcessMemory(this.id, address, array, 60, 0);
			return Memory.VISCII2Unicode(array);
		}

		// Token: 0x06000A3A RID: 2618 RVA: 0x00042A90 File Offset: 0x00040C90
		public string ReadStringEx(int address)
		{
			byte[] array = new byte[20248];
			Memory.ReadProcessMemory(this.id, address, array, array.Length, 0);
			return Memory.VISCII2Unicode(array);
		}

		// Token: 0x06000A3B RID: 2619 RVA: 0x00042AC0 File Offset: 0x00040CC0
		public string ReadString(int address, int offset)
		{
			return this.ReadString(this.Read(address) + offset);
		}

		// Token: 0x06000A3C RID: 2620 RVA: 0x00042AD1 File Offset: 0x00040CD1
		public string _ReadString(int address)
		{
			if (this.Read(address + 20) == 15)
			{
				return this.ReadShortString(address);
			}
			return this.ReadShortString(this.Read(address));
		}

		// Token: 0x06000A3D RID: 2621 RVA: 0x00042AF6 File Offset: 0x00040CF6
		public string _ReadString(int address, int offset)
		{
			address = this.Read(address) + offset;
			if (this.Read(address + 20) == 15)
			{
				return this.ReadString(address);
			}
			return this.ReadString(this.Read(address));
		}

		// Token: 0x06000A3E RID: 2622 RVA: 0x00042B28 File Offset: 0x00040D28
		public string _ReadString(int[] offsets)
		{
			int num = this.ReadAddress(offsets);
			if (this.Read(num + 20) == 15)
			{
				return this.ReadString(num);
			}
			return this.ReadString(this.Read(num));
		}

		// Token: 0x06000A3F RID: 2623 RVA: 0x00042B60 File Offset: 0x00040D60
		public string ReadString(int[] offsets)
		{
			return this.ReadString(this.ReadAddress(offsets));
		}

		// Token: 0x06000A40 RID: 2624 RVA: 0x00042B70 File Offset: 0x00040D70
		public void Write(int address, int value)
		{
			byte[] bytes = BitConverter.GetBytes(value);
			Memory.WriteProcessMemory(this.id, address, bytes, 4, 0);
		}

		// Token: 0x06000A41 RID: 2625 RVA: 0x00042B94 File Offset: 0x00040D94
		public void Write(int address, uint value, int length)
		{
			byte[] bytes = BitConverter.GetBytes(value);
			Memory.WriteProcessMemory(this.id, address, bytes, length, 0);
		}

		// Token: 0x06000A42 RID: 2626 RVA: 0x00042BB8 File Offset: 0x00040DB8
		public void Write(int[] offsets, int value)
		{
			this.Write(this.ReadAddress(offsets), value);
		}

		// Token: 0x06000A43 RID: 2627 RVA: 0x00042BC8 File Offset: 0x00040DC8
		public int ReadAddress(int address, int[] offset)
		{
			offset[0] = this.Read(address) + offset[0];
			return this.ReadAddress(offset);
		}

		// Token: 0x06000A44 RID: 2628 RVA: 0x00042BDF File Offset: 0x00040DDF
		public int ReadAddress(int address, int offset)
		{
			return this.ReadAddress(new int[]
			{
				address,
				offset
			});
		}

		// Token: 0x06000A45 RID: 2629 RVA: 0x00042BF8 File Offset: 0x00040DF8
		public int ReadAddress(int[] offsets)
		{
			int num = this.Read(offsets[0]);
			for (int i = 1; i < offsets.Length - 1; i++)
			{
				num = this.Read(num + offsets[i]);
			}
			if (offsets.Length == 1)
			{
				return num;
			}
			return num + offsets[offsets.Length - 1];
		}

		// Token: 0x06000A46 RID: 2630 RVA: 0x00042C3C File Offset: 0x00040E3C
		public static string VISCII2Unicode(string input)
		{
			StringBuilder stringBuilder = new StringBuilder();
			foreach (char c in input)
			{
				if (c == '\0')
				{
					break;
				}
				stringBuilder.Append(Memory.VISCII2Unicode((int)c));
			}
			return stringBuilder.ToString();
		}

		// Token: 0x06000A47 RID: 2631 RVA: 0x00042C80 File Offset: 0x00040E80
		public static string VISCII2Unicode(int input)
		{
			if (input < 256)
			{
				return TINHKIEM.Unicodes[input].ToString();
			}
			return ((char)input).ToString();
		}

		// Token: 0x06000A48 RID: 2632 RVA: 0x00042CB0 File Offset: 0x00040EB0
		public static string VISCII2Unicode(byte[] input)
		{
			StringBuilder stringBuilder = new StringBuilder();
			foreach (char c in input)
			{
				if (c == '\0')
				{
					break;
				}
				if (c < 'Ā')
				{
					stringBuilder.Append(TINHKIEM.Unicodes[(int)c]);
				}
				else
				{
					stringBuilder.Append(c);
				}
			}
			return stringBuilder.ToString();
		}

		// Token: 0x06000A49 RID: 2633 RVA: 0x00042D00 File Offset: 0x00040F00
		public static byte[] Unicode2VISCII(byte[] input)
		{
			for (int i = 0; i < input.Length; i++)
			{
				input[i] = Memory.Unicode2VISCII((int)input[i]);
			}
			return input;
		}

		// Token: 0x06000A4A RID: 2634 RVA: 0x00042D28 File Offset: 0x00040F28
		public static byte Unicode2VISCII(int input)
		{
			for (int i = 0; i < 256; i++)
			{
				if ((int)TINHKIEM.Unicodes[i] == input)
				{
					return (byte)i;
				}
			}
			return 63;
		}

		// Token: 0x06000A4B RID: 2635 RVA: 0x000221FD File Offset: 0x000203FD
		public static int Float2Int(float value)
		{
			return BitConverter.ToInt32(BitConverter.GetBytes(value), 0);
		}

		// Token: 0x06000A4C RID: 2636 RVA: 0x00042D54 File Offset: 0x00040F54
		public int VirtualAllocEx(int length)
		{
			return Memory.VirtualAllocEx(this.id, 0, length + 16, 4096U, 64U);
		}

		// Token: 0x06000A4D RID: 2637 RVA: 0x00042D70 File Offset: 0x00040F70
		public int WriteString(string str)
		{
			byte[] bytes = Encoding.Default.GetBytes(str);
			int num = Memory.VirtualAllocEx(this.id, 0, bytes.Length + 16, 4096U, 64U);
			Memory.WriteProcessMemory(this.id, num, bytes, bytes.Length, 0);
			return num;
		}

		// Token: 0x06000A4E RID: 2638 RVA: 0x00042DB8 File Offset: 0x00040FB8
		public int WriteString(string str, int address)
		{
			byte[] bytes = Encoding.Default.GetBytes(str);
			Memory.WriteProcessMemory(this.id, address, bytes, bytes.Length, 0);
			return address;
		}

		// Token: 0x06000A4F RID: 2639 RVA: 0x00042DE4 File Offset: 0x00040FE4
		public int WriteUnicodeString(string str, int address)
		{
			str = ConverterEx.Unicode2VISCII(str);
			byte[] bytes = Encoding.Default.GetBytes(str);
			Memory.WriteProcessMemory(this.id, address, bytes, bytes.Length, 0);
			return address;
		}

		// Token: 0x06000A50 RID: 2640 RVA: 0x00042E18 File Offset: 0x00041018
		public int WriteUnicodeString(string str)
		{
			byte[] array = new byte[4];
			str = ConverterEx.Unicode2VISCII(str);
			array = Encoding.Default.GetBytes(str);
			int num = Memory.VirtualAllocEx(this.id, 0, array.Length + 16, 4096U, 64U);
			Memory.WriteProcessMemory(this.id, num, array, array.Length, 0);
			return num;
		}

		// Token: 0x06000A51 RID: 2641 RVA: 0x00042E70 File Offset: 0x00041070
		private static byte[] GetBytes(string str)
		{
			byte[] array = new byte[str.Length * 2];
			Buffer.BlockCopy(str.ToCharArray(), 0, array, 0, array.Length);
			return array;
		}

		// Token: 0x06000A52 RID: 2642 RVA: 0x00042E9D File Offset: 0x0004109D
		public void FreeMem(int address, int size)
		{
			Memory.VirtualFreeEx(this.Id, address, size, 32768);
		}

		// Token: 0x06000A53 RID: 2643 RVA: 0x00042EB2 File Offset: 0x000410B2
		public void TrimMem()
		{
			Memory.SetProcessWorkingSetSize(Process.GetProcessById(this.ProcessID).Handle, -1, -1);
		}

		// Token: 0x06000A54 RID: 2644
		[DllImport("kernel32.dll")]
		public static extern IntPtr OpenProcess(int dwDesiredAccess, bool bInheritHandle, int dwProcessId);

		// Token: 0x06000A55 RID: 2645
		[DllImport("kernel32.dll")]
		public static extern bool ReadProcessMemory(IntPtr hProcess, int lpBaseAddress, byte[] lpBuffer, int dwSize, int lpNumberOfBytesRead);

		// Token: 0x06000A56 RID: 2646
		[DllImport("kernel32.dll")]
		public static extern bool ReadProcessMemory(IntPtr hProcess, int lpBaseAddress, byte[] lpBuffer, int dwSize, out int lpNumberOfBytesRead);

		// Token: 0x06000A57 RID: 2647
		[DllImport("kernel32.dll")]
		public static extern bool WriteProcessMemory(IntPtr hProcess, int lpBaseAddress, byte[] lpBuffer, int nSize, int lpNumberOfBytesWritten);

		// Token: 0x06000A58 RID: 2648
		[DllImport("kernel32.dll", ExactSpelling = true, SetLastError = true)]
		public static extern int VirtualAllocEx(IntPtr hProcess, int lpAddress, int dwSize, uint flAllocationType, uint flProtect);

		// Token: 0x06000A59 RID: 2649
		[DllImport("kernel32.dll")]
		public static extern bool VirtualFreeEx(IntPtr hProcess, int lpAddress, int dwSize, int dwFreeType);

		// Token: 0x06000A5A RID: 2650
		[DllImport("kernel32.dll", SetLastError = true)]
		public static extern bool VirtualProtect(int lpAddress, uint dwSize, uint flNewProtect, int lpflOldProtect);

		// Token: 0x06000A5B RID: 2651
		[DllImport("kernel32.dll")]
		private static extern bool SetProcessWorkingSetSize(IntPtr hProcess, int dwMinimumWorkingSetSize, int dwMaximumWorkingSetSize);

		// Token: 0x06000A5C RID: 2652
		[DllImport("kernel32.dll")]
		public static extern bool FlushInstructionCache(int hProcess, int lpBaseAddress, int dwSize);

		// Token: 0x040007A1 RID: 1953
		private IntPtr id;

		// Token: 0x040007A2 RID: 1954
		public int BytesCount;

		// Token: 0x040007A3 RID: 1955
		public static int BufferSize = 20248;

		// Token: 0x040007A4 RID: 1956
		public static int Decommit = 16384;

		// Token: 0x040007A5 RID: 1957
		public static int Release = 32768;

		// Token: 0x040007A6 RID: 1958
		public static int MEM_RESERVE = 8192;

		// Token: 0x040007A7 RID: 1959
		public static int MEM_COMMIT = 4096;

		// Token: 0x040007A8 RID: 1960
		public static int PAGE_READWRITE = 4;

		// Token: 0x02000172 RID: 370
		public enum Protection
		{
			// Token: 0x04000E64 RID: 3684
			PAGE_NOACCESS = 1,
			// Token: 0x04000E65 RID: 3685
			PAGE_READONLY,
			// Token: 0x04000E66 RID: 3686
			PAGE_READWRITE = 4,
			// Token: 0x04000E67 RID: 3687
			PAGE_WRITECOPY = 8,
			// Token: 0x04000E68 RID: 3688
			PAGE_EXECUTE = 16,
			// Token: 0x04000E69 RID: 3689
			PAGE_EXECUTE_READ = 32,
			// Token: 0x04000E6A RID: 3690
			PAGE_EXECUTE_READWRITE = 64,
			// Token: 0x04000E6B RID: 3691
			PAGE_EXECUTE_WRITECOPY = 128,
			// Token: 0x04000E6C RID: 3692
			PAGE_GUARD = 256,
			// Token: 0x04000E6D RID: 3693
			PAGE_NOCACHE = 512,
			// Token: 0x04000E6E RID: 3694
			PAGE_WRITECOMBINE = 1024
		}
	}
}
