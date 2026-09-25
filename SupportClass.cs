using System;
using System.IO;
using System.Text;

namespace ComponentAce.Compression.Libs.zlib
{
	// Token: 0x0200000E RID: 14
	public class SupportClass
	{
		// Token: 0x06000063 RID: 99 RVA: 0x00007E59 File Offset: 0x00006059
		public static long Identity(long literal)
		{
			return literal;
		}

		// Token: 0x06000064 RID: 100 RVA: 0x00007E59 File Offset: 0x00006059
		public static ulong Identity(ulong literal)
		{
			return literal;
		}

		// Token: 0x06000065 RID: 101 RVA: 0x00007E59 File Offset: 0x00006059
		public static float Identity(float literal)
		{
			return literal;
		}

		// Token: 0x06000066 RID: 102 RVA: 0x00007E59 File Offset: 0x00006059
		public static double Identity(double literal)
		{
			return literal;
		}

		// Token: 0x06000067 RID: 103 RVA: 0x00007E5C File Offset: 0x0000605C
		public static int URShift(int number, int bits)
		{
			int result;
			if (number >= 0)
			{
				result = number >> bits;
			}
			else
			{
				result = (number >> bits) + (2 << ~bits);
			}
			return result;
		}

		// Token: 0x06000068 RID: 104 RVA: 0x00007E86 File Offset: 0x00006086
		public static int URShift(int number, long bits)
		{
			return SupportClass.URShift(number, (int)bits);
		}

		// Token: 0x06000069 RID: 105 RVA: 0x00007E90 File Offset: 0x00006090
		public static long URShift(long number, int bits)
		{
			long result;
			if (number >= 0L)
			{
				result = number >> bits;
			}
			else
			{
				result = (number >> bits) + (2L << ~bits);
			}
			return result;
		}

		// Token: 0x0600006A RID: 106 RVA: 0x00007EBC File Offset: 0x000060BC
		public static long URShift(long number, long bits)
		{
			return SupportClass.URShift(number, (int)bits);
		}

		// Token: 0x0600006B RID: 107 RVA: 0x00007EC8 File Offset: 0x000060C8
		public static int ReadInput(Stream sourceStream, byte[] target, int start, int count)
		{
			int result;
			if (target.Length == 0)
			{
				result = 0;
			}
			else
			{
				byte[] array = new byte[target.Length];
				int num = sourceStream.Read(array, start, count);
				if (num == 0)
				{
					result = -1;
				}
				else
				{
					for (int i = start; i < start + num; i++)
					{
						target[i] = array[i];
					}
					result = num;
				}
			}
			return result;
		}

		// Token: 0x0600006C RID: 108 RVA: 0x00007F10 File Offset: 0x00006110
		public static int ReadInput(TextReader sourceTextReader, byte[] target, int start, int count)
		{
			int result;
			if (target.Length == 0)
			{
				result = 0;
			}
			else
			{
				char[] array = new char[target.Length];
				int num = sourceTextReader.Read(array, start, count);
				if (num == 0)
				{
					result = -1;
				}
				else
				{
					for (int i = start; i < start + num; i++)
					{
						target[i] = (byte)array[i];
					}
					result = num;
				}
			}
			return result;
		}

		// Token: 0x0600006D RID: 109 RVA: 0x00007F57 File Offset: 0x00006157
		public static byte[] ToByteArray(string sourceString)
		{
			return Encoding.UTF8.GetBytes(sourceString);
		}

		// Token: 0x0600006E RID: 110 RVA: 0x00007F64 File Offset: 0x00006164
		public static char[] ToCharArray(byte[] byteArray)
		{
			return Encoding.UTF8.GetChars(byteArray);
		}
	}
}
