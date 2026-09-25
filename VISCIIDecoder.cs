using System;
using System.Text;

namespace TinhKiemAuto
{
	// Token: 0x020000F3 RID: 243
	public class VISCIIDecoder : Decoder
	{
		// Token: 0x06000CB5 RID: 3253 RVA: 0x00052484 File Offset: 0x00050684
		public override int GetCharCount(byte[] bytes, int index, int count)
		{
			if (bytes == null)
			{
				throw new ArgumentNullException("bytes");
			}
			if (index < 0 || index > bytes.Length)
			{
				throw new ArgumentOutOfRangeException("index");
			}
			if (count < 0)
			{
				throw new ArgumentOutOfRangeException("count");
			}
			if (index + count > bytes.Length)
			{
				throw new ArgumentOutOfRangeException("bytes");
			}
			return count;
		}

		// Token: 0x06000CB6 RID: 3254 RVA: 0x000524D8 File Offset: 0x000506D8
		public override int GetChars(byte[] bytes, int byteIndex, int byteCount, char[] chars, int charIndex)
		{
			if (bytes == null)
			{
				throw new ArgumentNullException("bytes");
			}
			if (byteIndex < 0 || byteIndex > bytes.Length)
			{
				throw new ArgumentOutOfRangeException("byteIndex");
			}
			if (byteCount < 0)
			{
				throw new ArgumentOutOfRangeException("byteCount");
			}
			if (byteIndex + byteCount > bytes.Length)
			{
				throw new ArgumentOutOfRangeException("bytes");
			}
			if (chars == null)
			{
				throw new ArgumentNullException("chars");
			}
			if (charIndex < 0 || charIndex > chars.Length)
			{
				throw new ArgumentOutOfRangeException("charIndex");
			}
			int num = byteCount + byteIndex;
			int num2 = charIndex;
			while (byteIndex < num)
			{
				byte b = bytes[byteIndex];
				if (num2 == chars.Length)
				{
					throw new ArgumentException("chars");
				}
				chars[num2] = ((b >= 31 && b <= 127) ? ((char)b) : VISCIIDecoder.Unicodes[(int)b]);
				num2++;
				byteIndex++;
			}
			return num2 - charIndex;
		}

		// Token: 0x04000A0D RID: 2573
		private static readonly char[] Unicodes = VISCII.Unicodes;
	}
}
