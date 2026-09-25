using System;
using System.Text;

namespace TinhKiemAuto
{
	// Token: 0x020000F2 RID: 242
	internal class VISCII : Encoding
	{
		// Token: 0x17000312 RID: 786
		// (get) Token: 0x06000CA5 RID: 3237 RVA: 0x00052354 File Offset: 0x00050554
		protected VISCIIDecoder Decoder
		{
			get
			{
				VISCIIDecoder visciidecoder = this.decoder;
				if (visciidecoder == null)
				{
					visciidecoder = (this.decoder = new VISCIIDecoder());
				}
				DecoderFallback decoderFallback = base.DecoderFallback;
				if (decoderFallback != null && decoderFallback != visciidecoder.Fallback)
				{
					visciidecoder.Fallback = decoderFallback;
				}
				return visciidecoder;
			}
		}

		// Token: 0x17000313 RID: 787
		// (get) Token: 0x06000CA6 RID: 3238 RVA: 0x00052398 File Offset: 0x00050598
		protected VISCIIEncoder Encoder
		{
			get
			{
				VISCIIEncoder visciiencoder = this.encoder;
				if (visciiencoder == null)
				{
					visciiencoder = (this.encoder = new VISCIIEncoder());
				}
				EncoderFallback encoderFallback = base.EncoderFallback;
				if (encoderFallback != null && encoderFallback != visciiencoder.Fallback)
				{
					visciiencoder.Fallback = encoderFallback;
				}
				return visciiencoder;
			}
		}

		// Token: 0x17000314 RID: 788
		// (get) Token: 0x06000CA7 RID: 3239 RVA: 0x000523D9 File Offset: 0x000505D9
		public override string BodyName
		{
			get
			{
				return "viscii-simple";
			}
		}

		// Token: 0x17000315 RID: 789
		// (get) Token: 0x06000CA8 RID: 3240 RVA: 0x000523E0 File Offset: 0x000505E0
		public override string EncodingName
		{
			get
			{
				return this.BodyName;
			}
		}

		// Token: 0x17000316 RID: 790
		// (get) Token: 0x06000CA9 RID: 3241 RVA: 0x0000D470 File Offset: 0x0000B670
		public override bool IsSingleByte
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06000CAA RID: 3242 RVA: 0x000523E8 File Offset: 0x000505E8
		public override object Clone()
		{
			VISCII viscii = (VISCII)base.Clone();
			viscii.decoder = null;
			viscii.encoder = null;
			return viscii;
		}

		// Token: 0x06000CAB RID: 3243 RVA: 0x00052403 File Offset: 0x00050603
		public override Decoder GetDecoder()
		{
			return new VISCIIDecoder();
		}

		// Token: 0x06000CAC RID: 3244 RVA: 0x0005240A File Offset: 0x0005060A
		public override Encoder GetEncoder()
		{
			return new VISCIIEncoder();
		}

		// Token: 0x06000CAD RID: 3245 RVA: 0x00052411 File Offset: 0x00050611
		public override int GetByteCount(char[] chars, int index, int count)
		{
			return this.Encoder.GetByteCount(chars, index, count, true);
		}

		// Token: 0x06000CAE RID: 3246 RVA: 0x00052422 File Offset: 0x00050622
		public override int GetBytes(char[] chars, int charIndex, int charCount, byte[] bytes, int byteIndex)
		{
			return this.Encoder.GetBytes(chars, charIndex, charCount, bytes, byteIndex, true);
		}

		// Token: 0x06000CAF RID: 3247 RVA: 0x00052437 File Offset: 0x00050637
		public override int GetCharCount(byte[] bytes, int index, int count)
		{
			return this.Decoder.GetCharCount(bytes, index, count, true);
		}

		// Token: 0x06000CB0 RID: 3248 RVA: 0x00052448 File Offset: 0x00050648
		public override int GetChars(byte[] bytes, int byteIndex, int byteCount, char[] chars, int charIndex)
		{
			return this.Decoder.GetChars(bytes, byteIndex, byteCount, chars, charIndex, true);
		}

		// Token: 0x06000CB1 RID: 3249 RVA: 0x00014A6F File Offset: 0x00012C6F
		public override int GetMaxByteCount(int charCount)
		{
			return charCount;
		}

		// Token: 0x06000CB2 RID: 3250 RVA: 0x00014A6F File Offset: 0x00012C6F
		public override int GetMaxCharCount(int byteCount)
		{
			return byteCount;
		}

		// Token: 0x04000A0A RID: 2570
		public static readonly char[] Unicodes = new char[]
		{
			'\0',
			'\u0001',
			'Ẳ',
			'\u0003',
			'\u0004',
			'Ẵ',
			'Ẫ',
			'\a',
			'\b',
			'\t',
			'\n',
			'\v',
			'\f',
			'\r',
			'\u000e',
			'\u000f',
			'\u0010',
			'\u0011',
			'\u0012',
			'\u0013',
			'Ỷ',
			'\u0015',
			'\u0016',
			'\u0017',
			'\u0018',
			'Ỹ',
			'\u001a',
			'\u001b',
			'\u001c',
			'\u001d',
			'Ỵ',
			'\u001f',
			' ',
			'!',
			'"',
			'#',
			'$',
			'%',
			'&',
			'\'',
			'(',
			')',
			'*',
			'+',
			',',
			'-',
			'.',
			'/',
			'0',
			'1',
			'2',
			'3',
			'4',
			'5',
			'6',
			'7',
			'8',
			'9',
			':',
			';',
			'<',
			'=',
			'>',
			'?',
			'@',
			'A',
			'B',
			'C',
			'D',
			'E',
			'F',
			'G',
			'H',
			'I',
			'J',
			'K',
			'L',
			'M',
			'N',
			'O',
			'P',
			'Q',
			'R',
			'S',
			'T',
			'U',
			'V',
			'W',
			'X',
			'Y',
			'Z',
			'[',
			'\\',
			']',
			'^',
			'_',
			'`',
			'a',
			'b',
			'c',
			'd',
			'e',
			'f',
			'g',
			'h',
			'i',
			'j',
			'k',
			'l',
			'm',
			'n',
			'o',
			'p',
			'q',
			'r',
			's',
			't',
			'u',
			'v',
			'w',
			'x',
			'y',
			'z',
			'{',
			'|',
			'}',
			'~',
			'\u007f',
			'Ạ',
			'Ắ',
			'Ằ',
			'Ặ',
			'Ấ',
			'Ầ',
			'Ẩ',
			'Ậ',
			'Ẽ',
			'Ẹ',
			'Ế',
			'Ề',
			'Ể',
			'Ễ',
			'Ệ',
			'Ố',
			'Ồ',
			'Ổ',
			'Ỗ',
			'Ộ',
			'Ợ',
			'Ớ',
			'Ờ',
			'Ở',
			'Ị',
			'Ỏ',
			'Ọ',
			'Ỉ',
			'Ủ',
			'Ũ',
			'Ụ',
			'Ỳ',
			'Õ',
			'ắ',
			'ằ',
			'ặ',
			'ấ',
			'ầ',
			'ẩ',
			'ậ',
			'ẽ',
			'ẹ',
			'ế',
			'ề',
			'ể',
			'ễ',
			'ệ',
			'ố',
			'ồ',
			'ổ',
			'ỗ',
			'Ỡ',
			'Ơ',
			'ộ',
			'ờ',
			'ở',
			'ị',
			'Ự',
			'Ứ',
			'Ừ',
			'Ử',
			'ơ',
			'ớ',
			'Ư',
			'À',
			'Á',
			'Â',
			'Ã',
			'Ả',
			'Ă',
			'ẳ',
			'ẵ',
			'È',
			'É',
			'Ê',
			'Ẻ',
			'Ì',
			'Í',
			'Ĩ',
			'ỳ',
			'Đ',
			'ứ',
			'Ò',
			'Ó',
			'Ô',
			'ạ',
			'ỷ',
			'ừ',
			'ử',
			'Ù',
			'Ú',
			'ỹ',
			'ỵ',
			'Ý',
			'ỡ',
			'ư',
			'à',
			'á',
			'â',
			'ã',
			'ả',
			'ă',
			'ữ',
			'ẫ',
			'è',
			'é',
			'ê',
			'ẻ',
			'ì',
			'í',
			'ĩ',
			'ỉ',
			'đ',
			'ự',
			'ò',
			'ó',
			'ô',
			'õ',
			'ỏ',
			'ọ',
			'ụ',
			'ù',
			'ú',
			'ũ',
			'ủ',
			'ý',
			'ợ',
			'Ữ'
		};

		// Token: 0x04000A0B RID: 2571
		private VISCIIDecoder decoder;

		// Token: 0x04000A0C RID: 2572
		private VISCIIEncoder encoder;
	}
}
