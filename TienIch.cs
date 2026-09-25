using System;
using System.Collections.Generic;
using System.Drawing;
using System.Security.Cryptography;
using System.Text;

namespace TinhKiemAuto.Models
{
	// Token: 0x02000139 RID: 313
	internal class TienIch
	{
		// Token: 0x1700037F RID: 895
		// (get) Token: 0x06000FEA RID: 4074 RVA: 0x00075766 File Offset: 0x00073966
		// (set) Token: 0x06000FEB RID: 4075 RVA: 0x0007576D File Offset: 0x0007396D
		public static string LangName { get; set; }

		// Token: 0x17000380 RID: 896
		// (get) Token: 0x06000FEC RID: 4076 RVA: 0x00075775 File Offset: 0x00073975
		// (set) Token: 0x06000FED RID: 4077 RVA: 0x0007577C File Offset: 0x0007397C
		public static string ServerName { get; set; }

		// Token: 0x17000381 RID: 897
		// (get) Token: 0x06000FEE RID: 4078 RVA: 0x00075784 File Offset: 0x00073984
		// (set) Token: 0x06000FEF RID: 4079 RVA: 0x0007578B File Offset: 0x0007398B
		public static int ServerID { get; set; }

		// Token: 0x06000FF0 RID: 4080 RVA: 0x00075793 File Offset: 0x00073993
		public static int GameCount()
		{
			return FrmMain.GameCount;
		}

		// Token: 0x06000FF1 RID: 4081 RVA: 0x0007579C File Offset: 0x0007399C
		public static int GetTime()
		{
			return (int)(DateTime.Now - new DateTime(1970, 1, 1)).TotalSeconds;
		}

		// Token: 0x06000FF2 RID: 4082 RVA: 0x000757C8 File Offset: 0x000739C8
		public static string GetMD5(string input)
		{
			string result = "";
			try
			{
				HashAlgorithm hashAlgorithm = MD5.Create();
				byte[] bytes = Encoding.ASCII.GetBytes(input);
				byte[] array = hashAlgorithm.ComputeHash(bytes);
				StringBuilder stringBuilder = new StringBuilder();
				for (int i = 0; i < array.Length; i++)
				{
					stringBuilder.Append(array[i].ToString("x2"));
				}
				result = stringBuilder.ToString();
			}
			catch
			{
				result = "UnknowMD5";
			}
			return result;
		}

		// Token: 0x06000FF3 RID: 4083 RVA: 0x00075848 File Offset: 0x00073A48
		public static string Genkey(string time)
		{
			return TienIch.GetMD5("thanh06ht" + time);
		}

		// Token: 0x06000FF4 RID: 4084 RVA: 0x0007585C File Offset: 0x00073A5C
		public static bool IsMessengerBox(int MapID, int x, int y)
		{
			bool result = false;
			if (MapID != 19)
			{
				switch (MapID)
				{
				case 244:
					if (x == 26 && y == 103)
					{
						result = true;
					}
					break;
				case 245:
					if (x == 72 && y == 144)
					{
						result = true;
					}
					break;
				case 246:
					if (x == 21 && y == 143)
					{
						result = true;
					}
					break;
				case 249:
					if (x == 20 && y == 210)
					{
						result = true;
					}
					break;
				}
			}
			else if (x == 139 && y == 259)
			{
				result = true;
			}
			return result;
		}

		// Token: 0x06000FF5 RID: 4085 RVA: 0x000758EC File Offset: 0x00073AEC
		public List<Point> GetDanhSach()
		{
			List<Point> list = new List<Point>();
			Point item = new Point(20, 220);
			list.Add(item);
			return list;
		}

		// Token: 0x06000FF6 RID: 4086 RVA: 0x00075914 File Offset: 0x00073B14
		public static int GetFakeMapID(int MapID)
		{
			if (MapID <= 166)
			{
				if (MapID == 112)
				{
					return 39;
				}
				if (MapID == 164)
				{
					return 121;
				}
				if (MapID == 166)
				{
					return 123;
				}
			}
			else
			{
				if (MapID == 169)
				{
					return 126;
				}
				switch (MapID)
				{
				case 191:
					return 148;
				case 192:
					return 149;
				case 193:
					return 150;
				case 194:
				case 195:
				case 196:
				case 197:
				case 198:
				case 212:
				case 223:
				case 224:
				case 225:
				case 226:
				case 227:
				case 228:
				case 230:
				case 231:
				case 232:
				case 233:
				case 234:
				case 236:
				case 238:
				case 239:
				case 240:
				case 241:
				case 242:
				case 243:
				case 248:
				case 250:
				case 254:
				case 256:
				case 257:
				case 258:
				case 259:
				case 261:
					break;
				case 199:
					return 156;
				case 200:
					return 157;
				case 201:
					return 158;
				case 202:
					return 159;
				case 203:
					return 160;
				case 204:
					return 161;
				case 205:
					return 162;
				case 206:
					return 163;
				case 207:
					return 164;
				case 208:
					return 165;
				case 209:
					return 166;
				case 210:
					return 167;
				case 211:
					return 168;
				case 213:
					return 170;
				case 214:
					return 171;
				case 215:
					return 172;
				case 216:
					return 173;
				case 217:
					return 174;
				case 218:
					return 175;
				case 219:
					return 176;
				case 220:
					return 177;
				case 221:
					return 178;
				case 222:
					return 179;
				case 229:
					return 188;
				case 235:
					return 415;
				case 237:
					return 517;
				case 244:
					return 423;
				case 245:
					return 424;
				case 246:
					return 186;
				case 247:
					return 425;
				case 249:
					return 431;
				case 251:
					return 519;
				case 252:
					return 520;
				case 253:
					return 427;
				case 255:
					return 432;
				case 260:
					return 420;
				case 262:
					return 400;
				case 263:
					return 401;
				case 264:
					return 402;
				default:
					if (MapID == 284)
					{
						return 435;
					}
					break;
				}
			}
			return MapID;
		}

		// Token: 0x06000FF7 RID: 4087 RVA: 0x00075284 File Offset: 0x00073484
		public static string Base64Encode(string plainText)
		{
			return Convert.ToBase64String(Encoding.UTF8.GetBytes(plainText));
		}

		// Token: 0x06000FF8 RID: 4088 RVA: 0x00075C6C File Offset: 0x00073E6C
		public static string Base64Decode(string base64EncodedData)
		{
			byte[] bytes = Convert.FromBase64String(base64EncodedData);
			return Encoding.UTF8.GetString(bytes);
		}

		// Token: 0x04000D03 RID: 3331
		public static int CountMSG = 0;

		// Token: 0x04000D07 RID: 3335
		public static string domain = "http://update.chickenauto.com/";
	}
}
