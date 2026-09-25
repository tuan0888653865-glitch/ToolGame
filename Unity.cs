using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using Newtonsoft.Json;

namespace TinhKiemAuto.Models
{
	// Token: 0x0200012F RID: 303
	public class Unity
	{
		// Token: 0x06000FB9 RID: 4025 RVA: 0x00074B80 File Offset: 0x00072D80
		public static bool DangOMapVutRac(int MapID)
		{
			using (List<NPC>.Enumerator enumerator = Unity.VatPhamRac.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (enumerator.Current.Map == MapID)
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x06000FBA RID: 4026 RVA: 0x00074BDC File Offset: 0x00072DDC
		public static NPC GETNPCTRILIEU(int MapID)
		{
			foreach (NPC npc in Unity.ListTriLieu)
			{
				if (npc.Map == MapID)
				{
					return npc;
				}
			}
			return null;
		}

		// Token: 0x06000FBB RID: 4027 RVA: 0x00074C38 File Offset: 0x00072E38
		public static NPC GETNPCVUTRAC(int MapID)
		{
			foreach (NPC npc in Unity.VatPhamRac)
			{
				if (npc.Map == MapID)
				{
					return npc;
				}
			}
			return null;
		}

		// Token: 0x06000FBC RID: 4028 RVA: 0x00074C94 File Offset: 0x00072E94
		public static bool DangOMapTriLieuHienTai(int MapID)
		{
			using (List<NPC>.Enumerator enumerator = Unity.ListTriLieu.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (enumerator.Current.Map == MapID)
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x06000FBD RID: 4029 RVA: 0x00074CF0 File Offset: 0x00072EF0
		public static List<ServerList> GetDanhSach()
		{
			return new List<ServerList>
			{
				new ServerList
				{
					ServerID = 1,
					ServerName = "Nhất Kiếm"
				},
				new ServerList
				{
					ServerID = 2,
					ServerName = "Tái Chiến"
				},
				new ServerList
				{
					ServerID = 3,
					ServerName = "Nhị Kiếm"
				},
				new ServerList
				{
					ServerID = 4,
					ServerName = "Tam Kiếm"
				},
				new ServerList
				{
					ServerID = 5,
					ServerName = "Tứ Kiếm"
				},
				new ServerList
				{
					ServerID = 6,
					ServerName = "Tiếu Ngạo"
				},
				new ServerList
				{
					ServerID = 7,
					ServerName = "Long Kiếm"
				},
				new ServerList
				{
					ServerID = 9,
					ServerName = "Song Kiếm"
				},
				new ServerList
				{
					ServerID = 10,
					ServerName = "Du Kiếm"
				},
				new ServerList
				{
					ServerID = 11,
					ServerName = "Ảnh Kiếm"
				}
			};
		}

		// Token: 0x06000FBE RID: 4030 RVA: 0x00074E54 File Offset: 0x00073054
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

		// Token: 0x06000FBF RID: 4031 RVA: 0x000751AC File Offset: 0x000733AC
		public static bool IsMessengerBox(int MapID, int x, int y)
		{
			bool result = false;
			int num = 0;
			if (MapID != 1)
			{
				if (MapID != 19)
				{
					switch (MapID)
					{
					case 244:
						if (x == 29 + num && y == 103 + num)
						{
							result = true;
						}
						break;
					case 245:
						if (x == 72 + num && y == 144 + num)
						{
							result = true;
						}
						break;
					case 246:
						if (x == 21 + num && y == 143 + num)
						{
							result = true;
						}
						break;
					case 247:
						if (x == 104 + num && y == 215 + num)
						{
							result = true;
						}
						break;
					case 249:
						if (x == 20 + num && y == 210 + num)
						{
							result = true;
						}
						break;
					}
				}
				else if (x == 139 + num && y == 259 + num)
				{
					result = true;
				}
			}
			else if (x == 65 + num && y == 270 + num)
			{
				result = true;
			}
			return result;
		}

		// Token: 0x06000FC0 RID: 4032 RVA: 0x00075284 File Offset: 0x00073484
		public static string Base64Encode(string plainText)
		{
			return Convert.ToBase64String(Encoding.UTF8.GetBytes(plainText));
		}

		// Token: 0x06000FC1 RID: 4033 RVA: 0x00075298 File Offset: 0x00073498
		public static string Base64Decode(string base64EncodedData)
		{
			byte[] bytes = Convert.FromBase64String(base64EncodedData);
			return Encoding.UTF8.GetString(bytes);
		}

		// Token: 0x06000FC2 RID: 4034 RVA: 0x000752B8 File Offset: 0x000734B8
		public static object ByteArrayToObject(byte[] arrBytes)
		{
			object result;
			using (MemoryStream memoryStream = new MemoryStream())
			{
				BinaryFormatter binaryFormatter = new BinaryFormatter();
				memoryStream.Write(arrBytes, 0, arrBytes.Length);
				memoryStream.Seek(0L, SeekOrigin.Begin);
				result = binaryFormatter.Deserialize(memoryStream);
			}
			return result;
		}

		// Token: 0x06000FC3 RID: 4035 RVA: 0x0007530C File Offset: 0x0007350C
		public static byte[] ObjectToByteArray(object obj)
		{
			BinaryFormatter binaryFormatter = new BinaryFormatter();
			byte[] result;
			using (MemoryStream memoryStream = new MemoryStream())
			{
				binaryFormatter.Serialize(memoryStream, obj);
				result = memoryStream.ToArray();
			}
			return result;
		}

		// Token: 0x06000FC4 RID: 4036 RVA: 0x00075354 File Offset: 0x00073554
		public static bool IsNumeric(object Expression)
		{
			double num;
			return double.TryParse(Convert.ToString(Expression), NumberStyles.Any, NumberFormatInfo.InvariantInfo, out num);
		}

		// Token: 0x06000FC5 RID: 4037 RVA: 0x00075378 File Offset: 0x00073578
		public static T Deserialize<T>(string json)
		{
			return JsonConvert.DeserializeObject<T>(json);
		}

		// Token: 0x04000CE1 RID: 3297
		public static List<NPC> ListTriLieu = new List<NPC>
		{
			NPC.LONGBATHIEN,
			NPC.DOTHANHDANG,
			NPC.BINHSANHAN
		};

		// Token: 0x04000CE2 RID: 3298
		public static List<NPC> VatPhamRac = new List<NPC>
		{
			NPC.VANDIEUDIEU,
			NPC.TONTUVU,
			NPC.TRUONGTHIENTHIEN,
			NPC.DONGHOAKIM
		};
	}
}
