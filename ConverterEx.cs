using System;
using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;

namespace TinhKiemAuto
{
	// Token: 0x02000087 RID: 135
	internal class ConverterEx
	{
		// Token: 0x060005FA RID: 1530 RVA: 0x00021A34 File Offset: 0x0001FC34
		public static string CleanJarVar(string input)
		{
			return "";
		}

		// Token: 0x060005FB RID: 1531 RVA: 0x00021A3B File Offset: 0x0001FC3B
		public static bool HasSpecialChars(string yourString)
		{
			return !Regex.IsMatch(yourString, "^[a-zA-Z0-9]+$");
		}

		// Token: 0x060005FC RID: 1532 RVA: 0x00021A4B File Offset: 0x0001FC4B
		public static float GetDistance(float fromX, float fromY, float toX, float toY)
		{
			return (float)Math.Sqrt(Math.Pow((double)(fromX - toX), 2.0) + Math.Pow((double)(fromY - toY), 2.0));
		}

		// Token: 0x060005FD RID: 1533 RVA: 0x00021A78 File Offset: 0x0001FC78
		public static string ConvertStringToHex(string asciiString)
		{
			string text = "";
			for (int i = 0; i < asciiString.Length; i++)
			{
				text += string.Format("{0:x2}", Convert.ToUInt32(((int)asciiString[i]).ToString()));
			}
			return text;
		}

		// Token: 0x060005FE RID: 1534 RVA: 0x00021AC8 File Offset: 0x0001FCC8
		public static string ConvertHexToString(string HexValue)
		{
			string text = "";
			while (HexValue.Length > 0)
			{
				text += Convert.ToChar(Convert.ToUInt32(HexValue.Substring(0, 2), 16)).ToString();
				HexValue = HexValue.Substring(2, HexValue.Length - 2);
			}
			return text;
		}

		// Token: 0x060005FF RID: 1535 RVA: 0x00021A34 File Offset: 0x0001FC34
		public static string Hex2Bin(string hex)
		{
			return "";
		}

		// Token: 0x06000600 RID: 1536 RVA: 0x00021B1C File Offset: 0x0001FD1C
		public static string Hex2String(string input)
		{
			input = ConverterEx.ReplaceInsensitive(input, "\\x20", " ");
			input = ConverterEx.ReplaceInsensitive(input, "\\x21", "!");
			input = ConverterEx.ReplaceInsensitive(input, "\\x22", "\"");
			input = ConverterEx.ReplaceInsensitive(input, "\\x23", "#");
			input = ConverterEx.ReplaceInsensitive(input, "\\x24", "$");
			input = ConverterEx.ReplaceInsensitive(input, "\\x25", "%");
			input = ConverterEx.ReplaceInsensitive(input, "\\x26", "&");
			input = ConverterEx.ReplaceInsensitive(input, "\\x27", "'");
			input = ConverterEx.ReplaceInsensitive(input, "\\x28", "(");
			input = ConverterEx.ReplaceInsensitive(input, "\\x29", ")");
			input = ConverterEx.ReplaceInsensitive(input, "\\x2A", "*");
			input = ConverterEx.ReplaceInsensitive(input, "\\x2B", "+");
			input = ConverterEx.ReplaceInsensitive(input, "\\x2C", ",");
			input = ConverterEx.ReplaceInsensitive(input, "\\x2D", "-");
			input = ConverterEx.ReplaceInsensitive(input, "\\x2E", ".");
			input = ConverterEx.ReplaceInsensitive(input, "\\x2F", "/");
			input = ConverterEx.ReplaceInsensitive(input, "\\x30", "0");
			input = ConverterEx.ReplaceInsensitive(input, "\\x31", "1");
			input = ConverterEx.ReplaceInsensitive(input, "\\x32", "2");
			input = ConverterEx.ReplaceInsensitive(input, "\\x33", "3");
			input = ConverterEx.ReplaceInsensitive(input, "\\x34", "4");
			input = ConverterEx.ReplaceInsensitive(input, "\\x35", "5");
			input = ConverterEx.ReplaceInsensitive(input, "\\x36", "6");
			input = ConverterEx.ReplaceInsensitive(input, "\\x37", "7");
			input = ConverterEx.ReplaceInsensitive(input, "\\x38", "8");
			input = ConverterEx.ReplaceInsensitive(input, "\\x39", "9");
			input = ConverterEx.ReplaceInsensitive(input, "\\x3A", ":");
			input = ConverterEx.ReplaceInsensitive(input, "\\x3B", ";");
			input = ConverterEx.ReplaceInsensitive(input, "\\x3C", "<");
			input = ConverterEx.ReplaceInsensitive(input, "\\x3D", "=");
			input = ConverterEx.ReplaceInsensitive(input, "\\x3E", ">");
			input = ConverterEx.ReplaceInsensitive(input, "\\x3F", "?");
			input = ConverterEx.ReplaceInsensitive(input, "\\x40", "@");
			input = ConverterEx.ReplaceInsensitive(input, "\\x41", "A");
			input = ConverterEx.ReplaceInsensitive(input, "\\x42", "B");
			input = ConverterEx.ReplaceInsensitive(input, "\\x43", "C");
			input = ConverterEx.ReplaceInsensitive(input, "\\x44", "D");
			input = ConverterEx.ReplaceInsensitive(input, "\\x45", "E");
			input = ConverterEx.ReplaceInsensitive(input, "\\x46", "F");
			input = ConverterEx.ReplaceInsensitive(input, "\\x47", "G");
			input = ConverterEx.ReplaceInsensitive(input, "\\x48", "H");
			input = ConverterEx.ReplaceInsensitive(input, "\\x49", "I");
			input = ConverterEx.ReplaceInsensitive(input, "\\x4A", "J");
			input = ConverterEx.ReplaceInsensitive(input, "\\x4B", "K");
			input = ConverterEx.ReplaceInsensitive(input, "\\x4C", "L");
			input = ConverterEx.ReplaceInsensitive(input, "\\x4D", "M");
			input = ConverterEx.ReplaceInsensitive(input, "\\x4E", "N");
			input = ConverterEx.ReplaceInsensitive(input, "\\x4F", "O");
			input = ConverterEx.ReplaceInsensitive(input, "\\x50", "P");
			input = ConverterEx.ReplaceInsensitive(input, "\\x51", "Q");
			input = ConverterEx.ReplaceInsensitive(input, "\\x52", "R");
			input = ConverterEx.ReplaceInsensitive(input, "\\x53", "S");
			input = ConverterEx.ReplaceInsensitive(input, "\\x54", "T");
			input = ConverterEx.ReplaceInsensitive(input, "\\x55", "U");
			input = ConverterEx.ReplaceInsensitive(input, "\\x56", "V");
			input = ConverterEx.ReplaceInsensitive(input, "\\x57", "W");
			input = ConverterEx.ReplaceInsensitive(input, "\\x58", "X");
			input = ConverterEx.ReplaceInsensitive(input, "\\x59", "Y");
			input = ConverterEx.ReplaceInsensitive(input, "\\x5A", "Z");
			input = ConverterEx.ReplaceInsensitive(input, "\\x5B", "[");
			input = ConverterEx.ReplaceInsensitive(input, "\\x5C", "\\");
			input = ConverterEx.ReplaceInsensitive(input, "\\x5D", "]");
			input = ConverterEx.ReplaceInsensitive(input, "\\x5E", "^");
			input = ConverterEx.ReplaceInsensitive(input, "\\x5F", "_");
			input = ConverterEx.ReplaceInsensitive(input, "\\x60", "`");
			input = ConverterEx.ReplaceInsensitive(input, "\\x61", "a");
			input = ConverterEx.ReplaceInsensitive(input, "\\x62", "b");
			input = ConverterEx.ReplaceInsensitive(input, "\\x63", "c");
			input = ConverterEx.ReplaceInsensitive(input, "\\x64", "d");
			input = ConverterEx.ReplaceInsensitive(input, "\\x65", "e");
			input = ConverterEx.ReplaceInsensitive(input, "\\x66", "f");
			input = ConverterEx.ReplaceInsensitive(input, "\\x67", "g");
			input = ConverterEx.ReplaceInsensitive(input, "\\x68", "h");
			input = ConverterEx.ReplaceInsensitive(input, "\\x69", "i");
			input = ConverterEx.ReplaceInsensitive(input, "\\x6A", "j");
			input = ConverterEx.ReplaceInsensitive(input, "\\x6B", "k");
			input = ConverterEx.ReplaceInsensitive(input, "\\x6C", "l");
			input = ConverterEx.ReplaceInsensitive(input, "\\x6D", "m");
			input = ConverterEx.ReplaceInsensitive(input, "\\x6E", "n");
			input = ConverterEx.ReplaceInsensitive(input, "\\x6F", "o");
			input = ConverterEx.ReplaceInsensitive(input, "\\x70", "p");
			input = ConverterEx.ReplaceInsensitive(input, "\\x71", "q");
			input = ConverterEx.ReplaceInsensitive(input, "\\x72", "r");
			input = ConverterEx.ReplaceInsensitive(input, "\\x73", "s");
			input = ConverterEx.ReplaceInsensitive(input, "\\x74", "t");
			input = ConverterEx.ReplaceInsensitive(input, "\\x75", "u");
			input = ConverterEx.ReplaceInsensitive(input, "\\x76", "v");
			input = ConverterEx.ReplaceInsensitive(input, "\\x77", "w");
			input = ConverterEx.ReplaceInsensitive(input, "\\x78", "x");
			input = ConverterEx.ReplaceInsensitive(input, "\\x79", "y");
			input = ConverterEx.ReplaceInsensitive(input, "\\x7A", "z");
			input = ConverterEx.ReplaceInsensitive(input, "\\x7B", "{");
			input = ConverterEx.ReplaceInsensitive(input, "\\x7C", "|");
			input = ConverterEx.ReplaceInsensitive(input, "\\x7D", "}");
			input = ConverterEx.ReplaceInsensitive(input, "\\x7E", "~");
			return input;
		}

		// Token: 0x06000601 RID: 1537 RVA: 0x000221D8 File Offset: 0x000203D8
		public static string ReplaceInsensitive(string str, string from, string to)
		{
			return str.Replace(from, to);
		}

		// Token: 0x06000602 RID: 1538 RVA: 0x000221E2 File Offset: 0x000203E2
		public static string FormatMoney(int value)
		{
			if (value == 0)
			{
				return "0";
			}
			return string.Format("{0:#,###}", value);
		}

		// Token: 0x06000603 RID: 1539 RVA: 0x000221FD File Offset: 0x000203FD
		public static int Float2Int(float value)
		{
			return BitConverter.ToInt32(BitConverter.GetBytes(value), 0);
		}

		// Token: 0x06000604 RID: 1540 RVA: 0x0002220C File Offset: 0x0002040C
		public static int[] ToArr(int address, int[] offset)
		{
			int[] array = new int[offset.Length + 1];
			array[0] = address;
			offset.CopyTo(array, 1);
			return array;
		}

		// Token: 0x06000605 RID: 1541 RVA: 0x00022234 File Offset: 0x00020434
		public static int Percent(int min, int max)
		{
			if (max == 0)
			{
				return 0;
			}
			int num = min * 100 / max;
			if (num == 0 && min > 0)
			{
				return 1;
			}
			if (num >= 100)
			{
				return 99;
			}
			return num;
		}

		// Token: 0x06000606 RID: 1542 RVA: 0x0002225F File Offset: 0x0002045F
		public static int Bool2Int(bool value)
		{
			if (value)
			{
				return 1;
			}
			return 0;
		}

		// Token: 0x06000607 RID: 1543 RVA: 0x00022268 File Offset: 0x00020468
		public static string String2Hex(string s)
		{
			string text = "";
			for (int i = 0; i < s.Length; i++)
			{
				text += ConverterEx.Char2Int(s[i]).ToString("X2");
			}
			return text;
		}

		// Token: 0x06000608 RID: 1544 RVA: 0x00007E59 File Offset: 0x00006059
		public static int Char2Int(char c)
		{
			return (int)c;
		}

		// Token: 0x06000609 RID: 1545 RVA: 0x000222B0 File Offset: 0x000204B0
		public static int Hex2Int(string hex)
		{
			if (hex.Contains("?"))
			{
				return -1;
			}
			if (hex.Contains("#"))
			{
				return 257;
			}
			int result = -1;
			int.TryParse(hex, NumberStyles.HexNumber, CultureInfo.InvariantCulture, out result);
			return result;
		}

		// Token: 0x0600060A RID: 1546 RVA: 0x000222F8 File Offset: 0x000204F8
		public static int[] Hex2IntArr(string hex)
		{
			hex = hex.Replace(" ", "");
			if (hex.Length % 2 != 0)
			{
				hex += "0";
			}
			int[] array = new int[hex.Length / 2];
			for (int i = 0; i < array.Length; i++)
			{
				array[i] = ConverterEx.Hex2Int(hex.Substring(i * 2, 2));
			}
			return array;
		}

		// Token: 0x0600060B RID: 1547 RVA: 0x00022360 File Offset: 0x00020560
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
					stringBuilder.Append(ConverterEx.Unicodes[(int)c]);
				}
				else
				{
					stringBuilder.Append(c);
				}
			}
			return stringBuilder.ToString();
		}

		// Token: 0x0600060C RID: 1548 RVA: 0x000223B0 File Offset: 0x000205B0
		public static string VISCII2UnicodeEx(byte[] input)
		{
			StringBuilder stringBuilder = new StringBuilder();
			foreach (char c in input)
			{
				if (c != '\0')
				{
					if (c < 'Ā')
					{
						stringBuilder.Append(ConverterEx.Unicodes[(int)c]);
					}
					else
					{
						stringBuilder.Append("?");
					}
				}
			}
			return stringBuilder.ToString();
		}

		// Token: 0x0600060D RID: 1549 RVA: 0x00022404 File Offset: 0x00020604
		public static string Unicode2VISCII(string input)
		{
			StringBuilder stringBuilder = new StringBuilder();
			for (int i = 0; i < input.Length; i++)
			{
				if (input[i] >= 'ÿ')
				{
					stringBuilder.Append(ConverterEx.Unicode2VISCII(input[i]));
				}
				else
				{
					stringBuilder.Append(input[i]);
				}
			}
			return stringBuilder.ToString();
		}

		// Token: 0x0600060E RID: 1550 RVA: 0x00022460 File Offset: 0x00020660
		public static char Unicode2VISCII(char c)
		{
			for (int i = 0; i < 256; i++)
			{
				if (ConverterEx.Unicodes[i] == c)
				{
					return (char)i;
				}
			}
			return '?';
		}

		// Token: 0x040003DF RID: 991
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

		// Token: 0x040003E0 RID: 992
		public static readonly string[] UnicodesStr = new string[]
		{
			"\\u0000",
			"\\u0001",
			"\\u1EB2",
			"\\u0003",
			"\\u0004",
			"\\u1EB4",
			"\\u1EAA",
			"\\u0007",
			"\\u0008",
			"\\u0009",
			"\\u000A",
			"\\u000B",
			"\\u000C",
			"\\u000D",
			"\\u000E",
			"\\u000F",
			"\\u0010",
			"\\u0011",
			"\\u0012",
			"\\u0013",
			"\\u1EF6",
			"\\u0015",
			"\\u0016",
			"\\u0017",
			"\\u0018",
			"\\u1EF8",
			"\\u001A",
			"\\u001B",
			"\\u001C",
			"\\u001D",
			"\\u1EF4",
			"\\u001F",
			"\\u0020",
			"\\u0021",
			"\\u0022",
			"\\u0023",
			"\\u0024",
			"\\u0025",
			"\\u0026",
			"\\u0027",
			"\\u0028",
			"\\u0029",
			"\\u002A",
			"\\u002B",
			"\\u002C",
			"\\u002D",
			"\\u002E",
			"\\u002F",
			"\\u0030",
			"\\u0031",
			"\\u0032",
			"\\u0033",
			"\\u0034",
			"\\u0035",
			"\\u0036",
			"\\u0037",
			"\\u0038",
			"\\u0039",
			"\\u003A",
			"\\u003B",
			"\\u003C",
			"\\u003D",
			"\\u003E",
			"\\u003F",
			"\\u0040",
			"\\u0041",
			"\\u0042",
			"\\u0043",
			"\\u0044",
			"\\u0045",
			"\\u0046",
			"\\u0047",
			"\\u0048",
			"\\u0049",
			"\\u004A",
			"\\u004B",
			"\\u004C",
			"\\u004D",
			"\\u004E",
			"\\u004F",
			"\\u0050",
			"\\u0051",
			"\\u0052",
			"\\u0053",
			"\\u0054",
			"\\u0055",
			"\\u0056",
			"\\u0057",
			"\\u0058",
			"\\u0059",
			"\\u005A",
			"\\u005B",
			"\\u005C",
			"\\u005D",
			"\\u005E",
			"\\u005F",
			"\\u0060",
			"\\u0061",
			"\\u0062",
			"\\u0063",
			"\\u0064",
			"\\u0065",
			"\\u0066",
			"\\u0067",
			"\\u0068",
			"\\u0069",
			"\\u006A",
			"\\u006B",
			"\\u006C",
			"\\u006D",
			"\\u006E",
			"\\u006F",
			"\\u0070",
			"\\u0071",
			"\\u0072",
			"\\u0073",
			"\\u0074",
			"\\u0075",
			"\\u0076",
			"\\u0077",
			"\\u0078",
			"\\u0079",
			"\\u007A",
			"\\u007B",
			"\\u007C",
			"\\u007D",
			"\\u007E",
			"\\u007F",
			"\\u1EA0",
			"\\u1EAE",
			"\\u1EB0",
			"\\u1EB6",
			"\\u1EA4",
			"\\u1EA6",
			"\\u1EA8",
			"\\u1EAC",
			"\\u1EBC",
			"\\u1EB8",
			"\\u1EBE",
			"\\u1EC0",
			"\\u1EC2",
			"\\u1EC4",
			"\\u1EC6",
			"\\u1ED0",
			"\\u1ED2",
			"\\u1ED4",
			"\\u1ED6",
			"\\u1ED8",
			"\\u1EE2",
			"\\u1EDA",
			"\\u1EDC",
			"\\u1EDE",
			"\\u1ECA",
			"\\u1ECE",
			"\\u1ECC",
			"\\u1EC8",
			"\\u1EE6",
			"\\u0168",
			"\\u1EE4",
			"\\u1EF2",
			"\\u00D5",
			"\\u1EAF",
			"\\u1EB1",
			"\\u1EB7",
			"\\u1EA5",
			"\\u1EA7",
			"\\u1EA9",
			"\\u1EAD",
			"\\u1EBD",
			"\\u1EB9",
			"\\u1EBF",
			"\\u1EC1",
			"\\u1EC3",
			"\\u1EC5",
			"\\u1EC7",
			"\\u1ED1",
			"\\u1ED3",
			"\\u1ED5",
			"\\u1ED7",
			"\\u1EE0",
			"\\u01A0",
			"\\u1ED9",
			"\\u1EDD",
			"\\u1EDF",
			"\\u1ECB",
			"\\u1EF0",
			"\\u1EE8",
			"\\u1EEA",
			"\\u1EEC",
			"\\u01A1",
			"\\u1EDB",
			"\\u01AF",
			"\\u00C0",
			"\\u00C1",
			"\\u00C2",
			"\\u00C3",
			"\\u1EA2",
			"\\u0102",
			"\\u1EB3",
			"\\u1EB5",
			"\\u00C8",
			"\\u00C9",
			"\\u00CA",
			"\\u1EBA",
			"\\u00CC",
			"\\u00CD",
			"\\u0128",
			"\\u1EF3",
			"\\u0110",
			"\\u1EE9",
			"\\u00D2",
			"\\u00D3",
			"\\u00D4",
			"\\u1EA1",
			"\\u1EF7",
			"\\u1EEB",
			"\\u1EED",
			"\\u00D9",
			"\\u00DA",
			"\\u1EF9",
			"\\u1EF5",
			"\\u00DD",
			"\\u1EE1",
			"\\u01B0",
			"\\u00E0",
			"\\u00E1",
			"\\u00E2",
			"\\u00E3",
			"\\u1EA3",
			"\\u0103",
			"\\u1EEF",
			"\\u1EAB",
			"\\u00E8",
			"\\u00E9",
			"\\u00EA",
			"\\u1EBB",
			"\\u00EC",
			"\\u00ED",
			"\\u0129",
			"\\u1EC9",
			"\\u0111",
			"\\u1EF1",
			"\\u00F2",
			"\\u00F3",
			"\\u00F4",
			"\\u00F5",
			"\\u1ECF",
			"\\u1ECD",
			"\\u1EE5",
			"\\u00F9",
			"\\u00FA",
			"\\u0169",
			"\\u1EE7",
			"\\u00FD",
			"\\u1EE3",
			"\\u1EEE"
		};
	}
}
