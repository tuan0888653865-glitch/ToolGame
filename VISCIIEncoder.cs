using System;
using System.Text;

namespace TinhKiemAuto
{
	// Token: 0x020000F4 RID: 244
	public class VISCIIEncoder : Encoder
	{
		// Token: 0x17000317 RID: 791
		// (get) Token: 0x06000CB9 RID: 3257 RVA: 0x000525AD File Offset: 0x000507AD
		// (set) Token: 0x06000CBA RID: 3258 RVA: 0x000525B5 File Offset: 0x000507B5
		protected char HighSurrogate { get; set; }

		// Token: 0x06000CBB RID: 3259 RVA: 0x000525C0 File Offset: 0x000507C0
		static VISCIIEncoder()
		{
			for (int i = 0; i < VISCII.Unicodes.Length; i++)
			{
				VISCIIEncoder.VISCIIs[(int)VISCII.Unicodes[i]] = (byte)i;
			}
		}

		// Token: 0x06000CBC RID: 3260 RVA: 0x00052600 File Offset: 0x00050800
		public override int GetByteCount(char[] chars, int index, int count, bool flush)
		{
			if (chars == null)
			{
				throw new ArgumentNullException("chars");
			}
			if (index < 0 || index > chars.Length)
			{
				throw new ArgumentOutOfRangeException("index");
			}
			if (count < 0)
			{
				throw new ArgumentOutOfRangeException("count");
			}
			if (index + count > chars.Length)
			{
				throw new ArgumentOutOfRangeException("chars");
			}
			EncoderFallbackBuffer encoderFallbackBuffer = null;
			char c = this.HighSurrogate;
			int num = 0;
			int num2 = index + count;
			while (index < num2)
			{
				char c2 = chars[index];
				if (c == '\0')
				{
					goto IL_B9;
				}
				if (encoderFallbackBuffer == null)
				{
					encoderFallbackBuffer = (base.Fallback ?? EncoderFallback.ReplacementFallback).CreateFallbackBuffer();
				}
				if (!char.IsLowSurrogate(c2))
				{
					if (encoderFallbackBuffer.Fallback(c, index - 1))
					{
						VISCIIEncoder.HandleFallbackCount(encoderFallbackBuffer, ref num);
					}
					c = '\0';
					goto IL_B9;
				}
				if (encoderFallbackBuffer.Fallback(c, c2, index - 1))
				{
					VISCIIEncoder.HandleFallbackCount(encoderFallbackBuffer, ref num);
				}
				c = '\0';
				IL_B2:
				index++;
				continue;
				IL_B9:
				if ((int)c2 < VISCIIEncoder.VISCIIs.Length && (VISCIIEncoder.VISCIIs[(int)c2] != 0 || c2 == '\0'))
				{
					num++;
					goto IL_B2;
				}
				if (char.IsHighSurrogate(c2))
				{
					c = c2;
					goto IL_B2;
				}
				if (encoderFallbackBuffer == null)
				{
					encoderFallbackBuffer = (base.Fallback ?? EncoderFallback.ReplacementFallback).CreateFallbackBuffer();
				}
				if (encoderFallbackBuffer.Fallback(c2, index))
				{
					VISCIIEncoder.HandleFallbackCount(encoderFallbackBuffer, ref num);
					goto IL_B2;
				}
				goto IL_B2;
			}
			if (flush && c != '\0')
			{
				if (encoderFallbackBuffer == null)
				{
					encoderFallbackBuffer = (base.Fallback ?? EncoderFallback.ReplacementFallback).CreateFallbackBuffer();
				}
				if (encoderFallbackBuffer.Fallback(c, index - 1))
				{
					VISCIIEncoder.HandleFallbackCount(encoderFallbackBuffer, ref num);
				}
			}
			return num;
		}

		// Token: 0x06000CBD RID: 3261 RVA: 0x0005275C File Offset: 0x0005095C
		public override int GetBytes(char[] chars, int charIndex, int charCount, byte[] bytes, int byteIndex, bool flush)
		{
			if (chars == null)
			{
				throw new ArgumentNullException("chars");
			}
			if (charIndex < 0 || charIndex > chars.Length)
			{
				throw new ArgumentOutOfRangeException("charIndex");
			}
			if (charCount < 0)
			{
				throw new ArgumentOutOfRangeException("charCount");
			}
			if (charIndex + charCount > chars.Length)
			{
				throw new ArgumentOutOfRangeException("chars");
			}
			if (bytes == null)
			{
				throw new ArgumentNullException("bytes");
			}
			if (byteIndex < 0 || byteIndex > bytes.Length)
			{
				throw new ArgumentOutOfRangeException("byteIndex");
			}
			EncoderFallbackBuffer encoderFallbackBuffer = null;
			char c = this.HighSurrogate;
			int num = charIndex + charCount;
			int num2 = byteIndex;
			while (charIndex < num)
			{
				char c2 = chars[charIndex];
				if (c == '\0')
				{
					goto IL_E5;
				}
				if (encoderFallbackBuffer == null)
				{
					encoderFallbackBuffer = (base.Fallback ?? EncoderFallback.ReplacementFallback).CreateFallbackBuffer();
				}
				if (!char.IsLowSurrogate(c2))
				{
					if (encoderFallbackBuffer.Fallback(c, charIndex - 1))
					{
						VISCIIEncoder.HandleFallbackWrite(encoderFallbackBuffer, bytes, ref num2);
					}
					c = '\0';
					goto IL_E5;
				}
				if (encoderFallbackBuffer.Fallback(c, c2, charIndex - 1))
				{
					VISCIIEncoder.HandleFallbackWrite(encoderFallbackBuffer, bytes, ref num2);
				}
				c = '\0';
				IL_DE:
				charIndex++;
				continue;
				IL_E5:
				byte b;
				if ((int)c2 < VISCIIEncoder.VISCIIs.Length && ((b = VISCIIEncoder.VISCIIs[(int)c2]) != 0 || c2 == '\0'))
				{
					VISCIIEncoder.WriteByte(bytes, num2, b);
					num2++;
					goto IL_DE;
				}
				if (char.IsHighSurrogate(c2))
				{
					c = c2;
					goto IL_DE;
				}
				if (encoderFallbackBuffer == null)
				{
					encoderFallbackBuffer = (base.Fallback ?? EncoderFallback.ReplacementFallback).CreateFallbackBuffer();
				}
				if (encoderFallbackBuffer.Fallback(c2, charIndex))
				{
					VISCIIEncoder.HandleFallbackWrite(encoderFallbackBuffer, bytes, ref num2);
					goto IL_DE;
				}
				goto IL_DE;
			}
			if (flush)
			{
				if (c != '\0')
				{
					if (encoderFallbackBuffer == null)
					{
						encoderFallbackBuffer = (base.Fallback ?? EncoderFallback.ReplacementFallback).CreateFallbackBuffer();
					}
					if (encoderFallbackBuffer.Fallback(c, charIndex - 1))
					{
						VISCIIEncoder.HandleFallbackWrite(encoderFallbackBuffer, bytes, ref num2);
					}
				}
			}
			else
			{
				this.HighSurrogate = c;
			}
			return num2 - byteIndex;
		}

		// Token: 0x06000CBE RID: 3262 RVA: 0x00052900 File Offset: 0x00050B00
		protected static void HandleFallbackCount(EncoderFallbackBuffer fallbackBuffer, ref int count)
		{
			while (fallbackBuffer.Remaining > 0)
			{
				char nextChar = fallbackBuffer.GetNextChar();
				if ((int)nextChar >= VISCIIEncoder.VISCIIs.Length || (VISCIIEncoder.VISCIIs[(int)nextChar] == 0 && nextChar != '\0'))
				{
					throw new EncoderFallbackException();
				}
				count++;
			}
		}

		// Token: 0x06000CBF RID: 3263 RVA: 0x00052944 File Offset: 0x00050B44
		protected static void HandleFallbackWrite(EncoderFallbackBuffer fallbackBuffer, byte[] bytes, ref int byteIndex)
		{
			while (fallbackBuffer.Remaining > 0)
			{
				char nextChar = fallbackBuffer.GetNextChar();
				byte b;
				if ((int)nextChar >= VISCIIEncoder.VISCIIs.Length || ((b = VISCIIEncoder.VISCIIs[(int)nextChar]) == 0 && nextChar != '\0'))
				{
					throw new EncoderFallbackException();
				}
				VISCIIEncoder.WriteByte(bytes, byteIndex, b);
				byteIndex++;
			}
		}

		// Token: 0x06000CC0 RID: 3264 RVA: 0x00052990 File Offset: 0x00050B90
		protected static void WriteByte(byte[] bytes, int byteIndex, byte b)
		{
			if (byteIndex == bytes.Length)
			{
				throw new ArgumentException("bytes");
			}
			bytes[byteIndex] = b;
		}

		// Token: 0x04000A0F RID: 2575
		private static readonly byte[] VISCIIs = new byte[7930];
	}
}
