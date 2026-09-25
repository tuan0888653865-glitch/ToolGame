using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.IO.Compression;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using Microsoft.Win32;
using TinhKiemAuto.Models;

namespace TinhKiemAuto
{
	// Token: 0x020000EB RID: 235
	public class TINHKIEM
	{
		// Token: 0x06000C00 RID: 3072 RVA: 0x000221E2 File Offset: 0x000203E2
		public static string FormatMoney(int value)
		{
			if (value == 0)
			{
				return "0";
			}
			return string.Format("{0:#,###}", value);
		}

		// Token: 0x06000C01 RID: 3073 RVA: 0x0004C8C0 File Offset: 0x0004AAC0
		public static void MoveToEnd(TextBox txt)
		{
			txt.Select(txt.Text.Length, 0);
			txt.Focus();
			txt.ScrollToCaret();
		}

		// Token: 0x06000C02 RID: 3074 RVA: 0x0004C8E1 File Offset: 0x0004AAE1
		public static void MoveToBeg(TextBox txt)
		{
			txt.Select(0, 0);
			txt.Focus();
			txt.ScrollToCaret();
		}

		// Token: 0x06000C03 RID: 3075 RVA: 0x0004C8F8 File Offset: 0x0004AAF8
		public static string GetMACAddress()
		{
			NetworkInterface[] allNetworkInterfaces = NetworkInterface.GetAllNetworkInterfaces();
			string text = string.Empty;
			foreach (NetworkInterface networkInterface in allNetworkInterfaces)
			{
				if (text == string.Empty)
				{
					networkInterface.GetIPProperties();
					text = networkInterface.GetPhysicalAddress().ToString();
				}
			}
			return text;
		}

		// Token: 0x06000C04 RID: 3076 RVA: 0x0004C944 File Offset: 0x0004AB44
		public static string HttpUploadFile(string url, string file, string paramName, string contentType, NameValueCollection nvc)
		{
			string str = "---------------------------" + DateTime.Now.Ticks.ToString("x");
			HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
			httpWebRequest.ContentType = "multipart/form-data; boundary=" + str;
			httpWebRequest.Method = "POST";
			httpWebRequest.KeepAlive = true;
			httpWebRequest.ServicePoint.Expect100Continue = false;
			using (Stream requestStream = httpWebRequest.GetRequestStream())
			{
				using (StreamWriter streamWriter = new StreamWriter(requestStream, Encoding.UTF8))
				{
					foreach (object obj in nvc.Keys)
					{
						string text = (string)obj;
						streamWriter.Write("\r\n" + str + "\r\n");
						streamWriter.Write(string.Format("Content-Disposition: form-data; name=\"{0}\"\r\n\r\n{1}", text, nvc[text]));
					}
					streamWriter.Write("\r\n" + str + "\r\n");
					streamWriter.Write(string.Format("Content-Disposition: form-data; name=\"{0}\"; filename=\"{1}\"\r\nContent-Type: {2}\r\n\r\n", paramName, file, contentType));
					using (new FileStream(file, FileMode.Open, FileAccess.Read))
					{
					}
					streamWriter.Write("\r\n--" + str + "--\r\n");
				}
			}
			try
			{
				using (HttpWebResponse httpWebResponse = (HttpWebResponse)httpWebRequest.GetResponse())
				{
					if (httpWebResponse.StatusCode == HttpStatusCode.OK)
					{
						using (Stream responseStream = httpWebResponse.GetResponseStream())
						{
							if (responseStream == null)
							{
								return null;
							}
							using (StreamReader streamReader = new StreamReader(responseStream))
							{
								return streamReader.ReadToEnd();
							}
						}
					}
					throw new ApplicationException("Error while upload files. Server status code: " + httpWebResponse.StatusCode.ToString());
				}
			}
			catch (Exception innerException)
			{
				throw new ApplicationException("Error while uploading file", innerException);
			}
			string result;
			return result;
		}

		// Token: 0x06000C05 RID: 3077 RVA: 0x0004CBB4 File Offset: 0x0004ADB4
		public static void CopyTo(Stream input, Stream output)
		{
			byte[] array = new byte[4096];
			int count;
			while ((count = input.Read(array, 0, array.Length)) != 0)
			{
				output.Write(array, 0, count);
			}
		}

		// Token: 0x06000C06 RID: 3078 RVA: 0x0004CBE8 File Offset: 0x0004ADE8
		public static byte[] ToArr(Stream input)
		{
			int num = Convert.ToInt32(input.Length);
			byte[] array = new byte[num];
			input.Read(array, 0, num);
			return array;
		}

		// Token: 0x06000C07 RID: 3079 RVA: 0x0002225F File Offset: 0x0002045F
		public static int Bool2Int(bool value)
		{
			if (value)
			{
				return 1;
			}
			return 0;
		}

		// Token: 0x06000C08 RID: 3080 RVA: 0x0004CC14 File Offset: 0x0004AE14
		public static uint ComputeStringHash(string string_0)
		{
			uint num = 0U;
			if (string_0 != null)
			{
				num = 2166136261U;
				for (int i = 0; i < string_0.Length; i++)
				{
					num = ((uint)string_0[i] ^ num) * 16777619U;
				}
			}
			return num;
		}

		// Token: 0x06000C09 RID: 3081 RVA: 0x0004CC50 File Offset: 0x0004AE50
		public static Keys String2Key(string key)
		{
			uint num = TINHKIEM.ComputeStringHash(key);
			if (num <= 433243402U)
			{
				if (num <= 215134355U)
				{
					if (num <= 87056132U)
					{
						if (num != 3168037U)
						{
							if (num == 87056132U && key == "Alt 8")
							{
								return Keys.D8;
							}
						}
						else if (key == "Alt 3")
						{
							return Keys.D3;
						}
					}
					else if (num != 103833751U)
					{
						if (num != 198356736U)
						{
							if (num == 215134355U && key == "F8")
							{
								return Keys.F8;
							}
						}
						else if (key == "F9")
						{
							return Keys.F9;
						}
					}
					else if (key == "Alt 9")
					{
						return Keys.D9;
					}
				}
				else if (num <= 382910545U)
				{
					if (num != 332577688U)
					{
						if (num != 366132926U)
						{
							if (num == 382910545U && key == "F2")
							{
								return Keys.F2;
							}
						}
						else if (key == "F3")
						{
							return Keys.F3;
						}
					}
					else if (key == "F1")
					{
						return Keys.F1;
					}
				}
				else if (num != 399688164U)
				{
					if (num != 416465783U)
					{
						if (num == 433243402U && key == "F7")
						{
							return Keys.F7;
						}
					}
					else if (key == "F4")
					{
						return Keys.F4;
					}
				}
				else if (key == "F5")
				{
					return Keys.F5;
				}
			}
			else if (num <= 4180692000U)
			{
				if (num <= 3703400824U)
				{
					if (num != 450021021U)
					{
						if (num == 3703400824U && key == "F10")
						{
							return Keys.F10;
						}
					}
					else if (key == "F6")
					{
						return Keys.F6;
					}
				}
				else if (num != 3720178443U)
				{
					if (num != 3736956062U)
					{
						if (num == 4180692000U && key == "Alt 4")
						{
							return Keys.D4;
						}
					}
					else if (key == "F12")
					{
						return Keys.F12;
					}
				}
				else if (key == "F11")
				{
					return Keys.F11;
				}
			}
			else if (num <= 4231024857U)
			{
				if (num != 4197469619U)
				{
					if (num != 4214247238U)
					{
						if (num == 4231024857U && key == "Alt 7")
						{
							return Keys.D7;
						}
					}
					else if (key == "Alt 6")
					{
						return Keys.D6;
					}
				}
				else if (key == "Alt 5")
				{
					return Keys.D5;
				}
			}
			else if (num != 4247802476U)
			{
				if (num != 4264580095U)
				{
					if (num == 4281357714U && key == "Alt 2")
					{
						return Keys.D2;
					}
				}
				else if (key == "Alt 1")
				{
					return Keys.D1;
				}
			}
			else if (key == "Alt 0")
			{
				return Keys.D0;
			}
			return Keys.F13;
		}

		// Token: 0x06000C0A RID: 3082 RVA: 0x0004CEFC File Offset: 0x0004B0FC
		public static Keys Int2Key(int key)
		{
			switch (key)
			{
			case 0:
				return Keys.F1;
			case 1:
				return Keys.F2;
			case 2:
				return Keys.F3;
			case 3:
				return Keys.F4;
			case 4:
				return Keys.F5;
			case 5:
				return Keys.F6;
			case 6:
				return Keys.F7;
			case 7:
				return Keys.F8;
			case 8:
				return Keys.F9;
			case 9:
				return Keys.F10;
			case 10:
				return Keys.F11;
			case 11:
				return Keys.F12;
			case 12:
				return Keys.D1;
			case 13:
				return Keys.D2;
			case 14:
				return Keys.D3;
			case 15:
				return Keys.D4;
			case 16:
				return Keys.D5;
			case 17:
				return Keys.D6;
			case 18:
				return Keys.D7;
			case 19:
				return Keys.D8;
			case 20:
				return Keys.D9;
			case 21:
				return Keys.D0;
			default:
				return Keys.F13;
			}
		}

		// Token: 0x06000C0B RID: 3083 RVA: 0x0004CFB0 File Offset: 0x0004B1B0
		public static int Key2Int(Keys key)
		{
			switch (key)
			{
			case Keys.D0:
				return 21;
			case Keys.D1:
				return 12;
			case Keys.D2:
				return 13;
			case Keys.D3:
				return 14;
			case Keys.D4:
				return 15;
			case Keys.D5:
				return 16;
			case Keys.D6:
				return 17;
			case Keys.D7:
				return 18;
			case Keys.D8:
				return 19;
			case Keys.D9:
				return 20;
			default:
				switch (key)
				{
				case Keys.F1:
					return 0;
				case Keys.F2:
					return 1;
				case Keys.F3:
					return 2;
				case Keys.F4:
					return 3;
				case Keys.F5:
					return 4;
				case Keys.F6:
					return 5;
				case Keys.F7:
					return 6;
				case Keys.F8:
					return 7;
				case Keys.F9:
					return 8;
				case Keys.F10:
					return 9;
				case Keys.F11:
					return 10;
				case Keys.F12:
					return 11;
				default:
					return 22;
				}
				break;
			}
		}

		// Token: 0x06000C0C RID: 3084 RVA: 0x0004D068 File Offset: 0x0004B268
		public static int GetTruyen(string ChuoiTruyen)
		{
			ChuoiTruyen = TINHKIEM.VietLien(ChuoiTruyen);
			if (ChuoiTruyen.Contains("namvuc"))
			{
				return 4;
			}
			if (ChuoiTruyen.Contains("quynhchau"))
			{
				return 4;
			}
			if (ChuoiTruyen.Contains("vodi"))
			{
				return 4;
			}
			if (ChuoiTruyen.Contains("haitacdong"))
			{
				return 4;
			}
			if (ChuoiTruyen.Contains("haitocdong"))
			{
				return 4;
			}
			if (ChuoiTruyen.Contains("mieunhandong"))
			{
				return 4;
			}
			if (ChuoiTruyen.Contains("namchieu"))
			{
				return 5;
			}
			if (ChuoiTruyen.Contains("diemho"))
			{
				return 5;
			}
			if (ChuoiTruyen.Contains("bachsadiemkhanh"))
			{
				return 5;
			}
			if (ChuoiTruyen.Contains("bochsadiemkhanh"))
			{
				return 5;
			}
			if (ChuoiTruyen.Contains("thachlam"))
			{
				return 5;
			}
			if (ChuoiTruyen.Contains("thochlam"))
			{
				return 5;
			}
			if (ChuoiTruyen.Contains("mieucuong"))
			{
				return 5;
			}
			if (ChuoiTruyen.Contains("ngockhe"))
			{
				return 5;
			}
			if (ChuoiTruyen.Contains("truongbachson"))
			{
				return 6;
			}
			if (ChuoiTruyen.Contains("truongbochson"))
			{
				return 6;
			}
			if (ChuoiTruyen.Contains("hoanglongphu"))
			{
				return 6;
			}
			if (ChuoiTruyen.Contains("tuyetlangho"))
			{
				return 6;
			}
			if (ChuoiTruyen.Contains("thaonguyen"))
			{
				return 6;
			}
			if (ChuoiTruyen.Contains("thuykinhho"))
			{
				return 6;
			}
			if (ChuoiTruyen.Contains("lieutay"))
			{
				return 6;
			}
			if (ChuoiTruyen.Contains("tienvuongphan"))
			{
				return 6;
			}
			if (ChuoiTruyen.Contains("nganngaituyetnguyen"))
			{
				return 6;
			}
			return -1;
		}

		// Token: 0x06000C0D RID: 3085 RVA: 0x0004D1D8 File Offset: 0x0004B3D8
		public static int SecDiff(DateTime from, DateTime to)
		{
			return (int)(to - from).TotalSeconds;
		}

		// Token: 0x06000C0E RID: 3086 RVA: 0x0004D1F8 File Offset: 0x0004B3F8
		public static int IsKhoang(string ChuoiTruyen)
		{
			ChuoiTruyen = TINHKIEM.VietLien(ChuoiTruyen);
			if (ChuoiTruyen == "modong")
			{
				return 1;
			}
			if (ChuoiTruyen == "mosat")
			{
				return 2;
			}
			if (ChuoiTruyen == "mobac")
			{
				return 3;
			}
			if (ChuoiTruyen == "mohanthiet")
			{
				return 4;
			}
			if (ChuoiTruyen == "movang")
			{
				return 5;
			}
			if (ChuoiTruyen == "mohuyenthiet")
			{
				return 6;
			}
			if (ChuoiTruyen == "mophale")
			{
				return 7;
			}
			if (ChuoiTruyen == "mophithuy")
			{
				return 8;
			}
			if (ChuoiTruyen == "mochanvu")
			{
				return 9;
			}
			if (ChuoiTruyen == "molonghuyet")
			{
				return 10;
			}
			if (ChuoiTruyen == "mophunghuyet")
			{
				return 11;
			}
			return -1;
		}

		// Token: 0x06000C0F RID: 3087 RVA: 0x0004D2B8 File Offset: 0x0004B4B8
		public static int IsDuoc(string ChuoiTruyen)
		{
			ChuoiTruyen = TINHKIEM.VietLien(ChuoiTruyen);
			if (ChuoiTruyen == "bachanh")
			{
				return 1;
			}
			if (ChuoiTruyen == "bochanh")
			{
				return 1;
			}
			if (ChuoiTruyen == "bohoang")
			{
				return 2;
			}
			if (ChuoiTruyen == "xuyenboi")
			{
				return 3;
			}
			if (ChuoiTruyen == "nguyenho")
			{
				return 4;
			}
			if (ChuoiTruyen == "tyba")
			{
				return 5;
			}
			if (ChuoiTruyen == "camthao")
			{
				return 6;
			}
			if (ChuoiTruyen == "kimnganhoa")
			{
				return 7;
			}
			if (ChuoiTruyen == "hoangcam")
			{
				return 8;
			}
			if (ChuoiTruyen == "cauky")
			{
				return 9;
			}
			if (ChuoiTruyen == "tramhuong")
			{
				return 10;
			}
			if (ChuoiTruyen == "dotrong")
			{
				return 11;
			}
			if (ChuoiTruyen == "thuongthuat")
			{
				return 12;
			}
			if (ChuoiTruyen == "phuclinh")
			{
				return 13;
			}
			if (ChuoiTruyen == "phongphong")
			{
				return 14;
			}
			if (ChuoiTruyen == "huongnhu")
			{
				return 15;
			}
			if (ChuoiTruyen == "hoanglien")
			{
				return 16;
			}
			if (ChuoiTruyen == "duongqui")
			{
				return 17;
			}
			if (ChuoiTruyen == "quetam")
			{
				return 18;
			}
			if (ChuoiTruyen == "huongphu")
			{
				return 19;
			}
			if (ChuoiTruyen == "hoachuong")
			{
				return 20;
			}
			if (ChuoiTruyen == "hoithanthao")
			{
				return 21;
			}
			if (ChuoiTruyen == "thuo")
			{
				return 22;
			}
			if (ChuoiTruyen == "dongtrunghathao")
			{
				return 23;
			}
			if (ChuoiTruyen == "dongtrunghothao")
			{
				return 23;
			}
			if (ChuoiTruyen == "longquitu")
			{
				return 24;
			}
			if (ChuoiTruyen == "tuongboi")
			{
				return 25;
			}
			if (ChuoiTruyen == "nhansam")
			{
				return 26;
			}
			if (ChuoiTruyen == "linhchi")
			{
				return 27;
			}
			if (ChuoiTruyen == "tuanthao")
			{
				return 28;
			}
			if (ChuoiTruyen == "lientu")
			{
				return 29;
			}
			if (ChuoiTruyen == "khomocxuan")
			{
				return 30;
			}
			return -1;
		}

		// Token: 0x06000C10 RID: 3088 RVA: 0x0004D4C8 File Offset: 0x0004B6C8
		public static string MapToString(int mapid)
		{
			if (mapid == 0)
			{
				return "lacduong";
			}
			if (mapid == 1)
			{
				return "tochau";
			}
			if (mapid == 2)
			{
				return "daily";
			}
			if (mapid == 3)
			{
				return "tungson";
			}
			if (mapid == 4)
			{
				return "thaiho";
			}
			if (mapid == 7)
			{
				return "kiemcac";
			}
			if (mapid == 8)
			{
				return "donhoang";
			}
			if (mapid == 18)
			{
				return "nhannam";
			}
			if (mapid == 19)
			{
				return "nhanbac";
			}
			if (mapid == 20)
			{
				return "thaonguyen";
			}
			if (mapid == 21)
			{
				return "lieutay";
			}
			if (mapid == 22)
			{
				return "truongbachson";
			}
			if (mapid == 23)
			{
				return "hoanglongphu";
			}
			if (mapid == 24)
			{
				return "nhihai";
			}
			if (mapid == 25)
			{
				return "thuongson";
			}
			if (mapid == 26)
			{
				return "thachlam";
			}
			if (mapid == 27)
			{
				return "ngockhue";
			}
			if (mapid == 28)
			{
				return "namchieu";
			}
			if (mapid == 29)
			{
				return "mieucuong";
			}
			if (mapid == 30)
			{
				return "tayho";
			}
			if (mapid == 31)
			{
				return "longtuyen";
			}
			if (mapid == 32)
			{
				return "vodi";
			}
			if (mapid == 33)
			{
				return "mailinh";
			}
			if (mapid == 34)
			{
				return "namhai";
			}
			if (mapid == 35)
			{
				return "quynhchau";
			}
			return "khongbiet";
		}

		// Token: 0x06000C11 RID: 3089 RVA: 0x0004D5E8 File Offset: 0x0004B7E8
		public static string GetBangXY(string input)
		{
			input = TINHKIEM.VietLien(input);
			if (input.Contains("thaonguyen"))
			{
				if (input.Contains("chinhdong"))
				{
					return "271,191,20," + NPC.ThaoNguyenChinhDong.Id.ToString();
				}
				if (input.Contains("chinhbac"))
				{
					return "0,0,20," + NPC.ThaoNguyenChinhDong.Id.ToString();
				}
				if (input.Contains("chinhtay"))
				{
					return "66,202,20," + NPC.ThaoNguyenChinhTay.Id.ToString();
				}
				if (input.Contains("taynam"))
				{
					return "97,281,20," + NPC.ThaoNguyenTayNam.Id.ToString();
				}
			}
			else if (input.Contains("nhannam") || input.Contains("nhonnam"))
			{
				if (input.Contains("chinhdong"))
				{
					return "283,113,18," + NPC.NhanNamChinhDong.Id.ToString();
				}
				if (input.Contains("chinhbac"))
				{
					return "102,36,18," + NPC.NhanNamChinhBac.Id.ToString();
				}
				if (input.Contains("chinhtay"))
				{
					return "0,0,18," + NPC.NhanNamChinhBac.Id.ToString();
				}
				if (input.Contains("chinhnam"))
				{
					return "72,284,18," + NPC.NhanNamChinhNam.Id.ToString();
				}
			}
			else if (input.Contains("nhanbac") || input.Contains("nhonbac"))
			{
				if (input.Contains("dongbac"))
				{
					return "234,24,19," + NPC.NhanBacDongBac.Id.ToString();
				}
				if (input.Contains("taybac"))
				{
					return "116,29,19," + NPC.NhanBacTayBac.Id.ToString();
				}
				if (input.Contains("chinhtay"))
				{
					return "32,128,19," + NPC.NhanBacChinhTay.Id.ToString();
				}
				if (input.Contains("huongnam"))
				{
					return "0,0,19," + NPC.NhanBacChinhTay.Id.ToString();
				}
			}
			else if (input.Contains("lieutay"))
			{
				if (input.Contains("dongnam"))
				{
					return "277,258,21," + NPC.LieuTayDongNam.Id.ToString();
				}
				if (input.Contains("taybac"))
				{
					return "75,35,21," + NPC.LieuTayTayBac.Id.ToString();
				}
				if (input.Contains("chinhtay"))
				{
					return "40,142,21," + NPC.LieuTayChinhTay.Id.ToString();
				}
				if (input.Contains("huongnam"))
				{
					return "0,0,21," + NPC.LieuTayChinhTay.Id.ToString();
				}
			}
			else if (input.Contains("truongbachson") || input.Contains("truongbochson"))
			{
				if (input.Contains("chinhnam"))
				{
					return "216,282," + NPC.TruongBachSonChinhNam.Id.ToString();
				}
				if (input.Contains("taybac"))
				{
					return "39,63," + NPC.TruongBachSonTayBac.Id.ToString();
				}
				if (input.Contains("chinhdong"))
				{
					return "280,154," + NPC.TruongBachSonChinhDong.Id.ToString();
				}
				if (input.Contains("chinhnam"))
				{
					return "0,0,22," + NPC.TruongBachSonChinhDong.Id.ToString();
				}
			}
			else if (input.Contains("hoanglongphu"))
			{
				if (input.Contains("dongnam"))
				{
					return "253,285,23," + NPC.HoangLongPhuDongNam.Id.ToString();
				}
				if (input.Contains("taybac"))
				{
					return "28,54,23," + NPC.HoangLongPhuTayBac.Id.ToString();
				}
				if (input.Contains("chinhdong"))
				{
					return "290,115,23," + NPC.HoangLongPhuChinhDong.Id.ToString();
				}
				if (input.Contains("huongnam"))
				{
					return "0,0,,23," + NPC.HoangLongPhuChinhDong.Id.ToString();
				}
			}
			else if (input.Contains("nhihai"))
			{
				if (input.Contains("chinhdong"))
				{
					return "285,166,24," + NPC.NhiHaiChinhDong.Id.ToString();
				}
				if (input.Contains("chinhnam"))
				{
					return "173,283,24," + NPC.NhiHaiChinhNam.Id.ToString();
				}
				if (input.Contains("chinhtay"))
				{
					return "34,100,24," + NPC.NhiHaiChinhTay.Id.ToString();
				}
				if (input.Contains("huongnam"))
				{
					return "0,0,24," + NPC.NhiHaiChinhTay.Id.ToString();
				}
			}
			else if (input.Contains("thuongson"))
			{
				if (input.Contains("tay"))
				{
					return "37,172,25," + NPC.ThuongSonTranTay.Id.ToString();
				}
				if (input.Contains("chinhdong"))
				{
					return "294,153,25," + NPC.ThuongSonChinhDong.Id.ToString();
				}
				if (input.Contains("chinhnam"))
				{
					return "146,284,25," + NPC.ThuongSonChinhNam.Id.ToString();
				}
				if (input.Contains("huongnam"))
				{
					return "0,0,25," + NPC.ThuongSonChinhNam.Id.ToString();
				}
			}
			else if (input.Contains("thachlam") || input.Contains("thochlam"))
			{
				if (input.Contains("chinhbac"))
				{
					return "226,36,26," + NPC.ThachLamChinhBac.Id.ToString();
				}
				if (input.Contains("chinhnam"))
				{
					return "278,281,26," + NPC.ThachLamChinhNam.Id.ToString();
				}
				if (input.Contains("chinhtay"))
				{
					return "45,177,26," + NPC.ThachLamChinhTay.Id.ToString();
				}
				if (input.Contains("huongnam"))
				{
					return "0,0,26," + NPC.ThachLamChinhTay.Id.ToString();
				}
			}
			else if (input.Contains("mieucuong"))
			{
				if (input.Contains("dongbac"))
				{
					return "250,45,29," + NPC.MieuCuongDongBac.Id.ToString();
				}
				if (input.Contains("taynam"))
				{
					return "37,251,29," + NPC.MieuCuongTayNam.Id.ToString();
				}
				if (input.Contains("chinhdong"))
				{
					return "281,160,29," + NPC.MieuCuongChinhDong.Id.ToString();
				}
				if (input.Contains("huongnam"))
				{
					return "0,0,29," + NPC.MieuCuongChinhDong.Id.ToString();
				}
			}
			else if (input.Contains("tayho"))
			{
				if (input.Contains("taynam"))
				{
					return "45,267,30," + NPC.TayHoTayNam.Id.ToString();
				}
				if (input.Contains("chinhdong"))
				{
					return "261,231,30," + NPC.TayHoChinhDong.Id.ToString();
				}
				if (input.Contains("chinhtay"))
				{
					return "39,139,30," + NPC.TayHoChinhTay.Id.ToString();
				}
				if (input.Contains("huongnam"))
				{
					return "0,0,30," + NPC.TayHoChinhTay.Id.ToString();
				}
			}
			else if (input.Contains("longtuyen"))
			{
				if (input.Contains("dongnam"))
				{
					return "218,282,31," + NPC.LongTuyenDongNam.Id.ToString();
				}
				if (input.Contains("chinhbac"))
				{
					return "63,33,31," + NPC.LongTuyenChinhBac.Id.ToString();
				}
				if (input.Contains("chinhtay"))
				{
					return "35,121,31," + NPC.LongTuyenChinhTay.Id.ToString();
				}
				if (input.Contains("huongnam"))
				{
					return "0,0,31," + NPC.LongTuyenChinhTay.Id.ToString();
				}
			}
			else if (input.Contains("vodi"))
			{
				if (input.Contains("dongbac"))
				{
					return "254,38,32," + NPC.VoDiDongBac.Id.ToString();
				}
				if (input.Contains("chinhnam"))
				{
					return "113,280,32," + NPC.VoDiChinhNam.Id.ToString();
				}
				if (input.Contains("chinhtay"))
				{
					return "92,172,32," + NPC.VoDiChinhTay.Id.ToString();
				}
				if (input.Contains("huongnam"))
				{
					return "0,0,32," + NPC.VoDiChinhTay.Id.ToString();
				}
			}
			else if (input.Contains("mailinh"))
			{
				if (input.Contains("dongbac"))
				{
					return "271,37,33," + NPC.MaiLinhDongBac.Id.ToString();
				}
				if (input.Contains("taybac"))
				{
					return "31,90,33," + NPC.MaiLinhTayBac.Id.ToString();
				}
				if (input.Contains("chinhdong"))
				{
					return "282,236,33," + NPC.MaiLinhChinhDong.Id.ToString();
				}
				if (input.Contains("huongnam"))
				{
					return "0,0,33," + NPC.MaiLinhChinhDong.Id.ToString();
				}
			}
			else if (input.Contains("namvuc"))
			{
				if (input.Contains("dongbac"))
				{
					return "292,58,34," + NPC.NamVucDongBac.Id.ToString();
				}
				if (input.Contains("taynam"))
				{
					return "108,227,34," + NPC.NamVucTayNam.Id.ToString();
				}
				if (input.Contains("chinhbac"))
				{
					return "134,40,34," + NPC.NamVucChinhBac.Id.ToString();
				}
				if (input.Contains("huongnam"))
				{
					return "0,0,34," + NPC.NamVucChinhBac.Id.ToString();
				}
			}
			else
			{
				if (!input.Contains("quynhchau"))
				{
					return "";
				}
				if (input.Contains("dongc"))
				{
					return "243,150,35," + NPC.QuynhChauChinhDong.Id.ToString();
				}
				if (input.Contains("dongbac"))
				{
					return "273,52,35," + NPC.QuynhChauDongBac.Id.ToString();
				}
				if (input.Contains("tay"))
				{
					return "80,139,35," + NPC.QuynhChauChinhTay.Id.ToString();
				}
				if (input.Contains("nam"))
				{
					return "0,0,35," + NPC.QuynhChauChinhTay.Id.ToString();
				}
			}
			return "";
		}

		// Token: 0x06000C12 RID: 3090 RVA: 0x0004E128 File Offset: 0x0004C328
		public static string GetDuaBangXY(string input)
		{
			input = TINHKIEM.VietLien(input);
			if (input.Contains("thaonguyen") || input.Contains("hoanglongphu") || input.Contains("truongbachson") || input.Contains("truongbochson") || input.Contains("lieutay"))
			{
				return "256,51,20";
			}
			if (input.Contains("nhanbac") || input.Contains("nhonbac") || input.Contains("nhannam") || input.Contains("nhonnam"))
			{
				return "277,54,19";
			}
			if (input.Contains("nhihai") || input.Contains("thuongson"))
			{
				return "77,206,24";
			}
			if (input.Contains("thachlam") || input.Contains("thochlam") || input.Contains("namchieu") || input.Contains("ngockhe"))
			{
				return "73,214,26";
			}
			if (input.Contains("mieucuong"))
			{
				return "195,49,29";
			}
			if (input.Contains("tayho") || input.Contains("longtuyen"))
			{
				return "134,165,30";
			}
			if (input.Contains("vodi"))
			{
				return "69,106,32";
			}
			if (input.Contains("namvuc"))
			{
				return "110,64,34";
			}
			if (input.Contains("quynhchau"))
			{
				return "136,236,35";
			}
			if (input.Contains("thaiho"))
			{
				return "246,146,4";
			}
			return "-1";
		}

		// Token: 0x06000C13 RID: 3091 RVA: 0x0004E29C File Offset: 0x0004C49C
		public static int GetMapId(string input)
		{
			input = TINHKIEM.VietLien(input);
			if (input.Contains("lacduong"))
			{
				return 0;
			}
			if (input.Contains("locduong"))
			{
				return 0;
			}
			if (input.Contains("tochau"))
			{
				return 1;
			}
			if (input.Contains("daily"))
			{
				return 2;
			}
			if (input.Contains("doily"))
			{
				return 2;
			}
			if (input.Contains("doilu"))
			{
				return 2;
			}
			if (input.Contains("tungson"))
			{
				return 3;
			}
			if (input.Contains("thaiho"))
			{
				return 4;
			}
			if (input.Contains("kinhho"))
			{
				return 5;
			}
			if (input.Contains("voluongson"))
			{
				return 6;
			}
			if (input.Contains("kiemcac"))
			{
				return 7;
			}
			if (input.Contains("donhoang"))
			{
				return 8;
			}
			if (input.Contains("thieulamtu"))
			{
				return 9;
			}
			if (input.Contains("caibangtongda"))
			{
				return 10;
			}
			if (input.Contains("quangminhdien"))
			{
				return 11;
			}
			if (input.Contains("vodangson"))
			{
				return 12;
			}
			if (input.Contains("thienlongtu"))
			{
				return 13;
			}
			if (input.Contains("langbadong"))
			{
				return 14;
			}
			if (input.Contains("ngamison"))
			{
				return 15;
			}
			if (input.Contains("tinhtuchai"))
			{
				return 16;
			}
			if (input.Contains("thienson"))
			{
				return 17;
			}
			if (input.Contains("nhannam"))
			{
				return 18;
			}
			if (input.Contains("nhonnam"))
			{
				return 18;
			}
			if (input.Contains("nhanbac"))
			{
				return 19;
			}
			if (input.Contains("nhonbac"))
			{
				return 19;
			}
			if (input.Contains("thaonguyen"))
			{
				return 20;
			}
			if (input.Contains("lieutay"))
			{
				return 21;
			}
			if (input.Contains("truongbachson"))
			{
				return 22;
			}
			if (input.Contains("truongbochson"))
			{
				return 22;
			}
			if (input.Contains("hoanglongphu"))
			{
				return 23;
			}
			if (input.Contains("nhihai"))
			{
				return 24;
			}
			if (input.Contains("thuongson"))
			{
				return 25;
			}
			if (input.Contains("thachlam"))
			{
				return 26;
			}
			if (input.Contains("thochlam"))
			{
				return 26;
			}
			if (input.Contains("ngockhe"))
			{
				return 27;
			}
			if (input.Contains("namchieu"))
			{
				return 28;
			}
			if (input.Contains("mieucuong"))
			{
				return 29;
			}
			if (input.Contains("tayho"))
			{
				return 30;
			}
			if (input.Contains("longtuyen"))
			{
				return 31;
			}
			if (input.Contains("vodi"))
			{
				return 32;
			}
			if (input.Contains("mailinh"))
			{
				return 33;
			}
			if (input.Contains("namvuc"))
			{
				return 34;
			}
			if (input.Contains("namhai"))
			{
				return 34;
			}
			if (input.Contains("quynhchau"))
			{
				return 35;
			}
			if (input.Contains("huyenvudao"))
			{
				return 112;
			}
			if (input.Contains("baotangdongtang1"))
			{
				return 166;
			}
			if (input.Contains("baotangdongtang2"))
			{
				return 169;
			}
			if (input.Contains("nganngaituyetnguyen"))
			{
				return 229;
			}
			if (input.Contains("baotangdongtang3"))
			{
				return 191;
			}
			if (input.Contains("baotangdongtang4"))
			{
				return 192;
			}
			if (input.Contains("baotangdongtang5"))
			{
				return 193;
			}
			if (input.Contains("thaolieutruong"))
			{
				return 199;
			}
			if (input.Contains("mieu nhan dong"))
			{
				return 200;
			}
			if (input.Contains("thanh thu son"))
			{
				return 201;
			}
			if (input.Contains("yenvuongcomotang1"))
			{
				return 202;
			}
			if (input.Contains("yenvuongcomotang2"))
			{
				return 203;
			}
			if (input.Contains("yenvuongcomotang3"))
			{
				return 204;
			}
			if (input.Contains("yenvuongcomotang4"))
			{
				return 205;
			}
			if (input.Contains("yenvuongcomotang5"))
			{
				return 206;
			}
			if (input.Contains("yenvuongcomotang6"))
			{
				return 207;
			}
			if (input.Contains("yenvuongcomotang7"))
			{
				return 208;
			}
			if (input.Contains("yenvuongcomotang8"))
			{
				return 209;
			}
			if (input.Contains("yenvuongcomotang9"))
			{
				return 210;
			}
			if (input.Contains("bentausondong"))
			{
				return 211;
			}
			if (input.Contains("kiemgia"))
			{
				return 212;
			}
			if (input.Contains("manhaidong"))
			{
				return 213;
			}
			if (input.Contains("danhancau"))
			{
				return 214;
			}
			if (input.Contains("ontuyendong"))
			{
				return 215;
			}
			if (input.Contains("hoanglongdong"))
			{
				return 216;
			}
			if (input.Contains("thuykinhho"))
			{
				return 217;
			}
			if (input.Contains("tienvuongphan"))
			{
				return 218;
			}
			if (input.Contains("thienkhanhthudong"))
			{
				return 219;
			}
			if (input.Contains("daohoanguyen"))
			{
				return 220;
			}
			if (input.Contains("haitacdong"))
			{
				return 221;
			}
			if (input.Contains("tuyetlangho"))
			{
				return 222;
			}
			if (input.Contains("diemho"))
			{
				return 235;
			}
			if (input.Contains("bachsadiemkhanh"))
			{
				return 237;
			}
			if (input.Contains("bochsadiemkhanh"))
			{
				return 237;
			}
			if (input.Contains("hoadiemson"))
			{
				return 244;
			}
			if (input.Contains("caoxuong"))
			{
				return 245;
			}
			if (input.Contains("laulan"))
			{
				return 246;
			}
			if (input.Contains("thaplymoc"))
			{
				return 247;
			}
			if (input.Contains("thaplumoc"))
			{
				return 247;
			}
			if (input.Contains("hoadiemcoc"))
			{
				return 251;
			}
			if (input.Contains("caoxuongmecung"))
			{
				return 252;
			}
			if (input.Contains("thapkhaclapmacan"))
			{
				return 253;
			}
			if (input.Contains("daiuyen"))
			{
				return 249;
			}
			if (input.Contains("hanhuyetlinh"))
			{
				return 255;
			}
			if (input.Contains("honhuyetlinh"))
			{
				return 255;
			}
			if (input.Contains("tanhoangdiacungtang1"))
			{
				return 262;
			}
			if (input.Contains("tanhoangdiacungtang2"))
			{
				return 263;
			}
			if (input.Contains("tanhoangdiacungtang3"))
			{
				return 264;
			}
			if (input.Contains("tanhoangdiacungtang4"))
			{
				return 292;
			}
			if (input.Contains("datayho"))
			{
				return 164;
			}
			if (input.Contains("dotayho"))
			{
				return 164;
			}
			if (input.Contains("conlonphucdia"))
			{
				return 254;
			}
			if (input.Contains("conlonson"))
			{
				return 248;
			}
			if (input.Contains("thanhnguyen"))
			{
				return 282;
			}
			if (input.Contains("thanhnguyensondong"))
			{
				return 283;
			}
			if (input.Contains("modungsontrang"))
			{
				return 284;
			}
			if (input.Contains("tatmanhihan"))
			{
				return 250;
			}
			if (input.Contains("thanhhoacung"))
			{
				return 256;
			}
			if (input.Contains("lamhaikhecoc"))
			{
				return 569;
			}
			if (input.Contains("macnamthanhnguyen"))
			{
				return 573;
			}
			if (input.Contains("vongxuyenhoahai"))
			{
				return 574;
			}
			if (input.Contains("thienkynamhoai"))
			{
				return 575;
			}
			if (input.Contains("thongthienthapdiacung"))
			{
				return 295;
			}
			if (input.Contains("thongthienthaptang1"))
			{
				return 296;
			}
			if (input.Contains("thongthienthaptang2"))
			{
				return 297;
			}
			if (input.Contains("thongthienthaptang3"))
			{
				return 298;
			}
			if (input.Contains("dinhthongthienthap"))
			{
				return 299;
			}
			if (input.Contains("phungminhtran"))
			{
				return 580;
			}
			if (input.Contains("laulan"))
			{
				return 246;
			}
			if (input.Contains("thuchacotran"))
			{
				return 260;
			}
			if (input.Contains("denhatkhunghingoitailacduong"))
			{
				return 238;
			}
			if (input.Contains("khunghingoitaidaily"))
			{
				return 240;
			}
			if (input.Contains("khunghingoitaitochau"))
			{
				return 241;
			}
			if (input.Contains("hanngoccoc"))
			{
				return 243;
			}
			if (input.Contains("thuynguyetdongthien"))
			{
				return 613;
			}
			if (input.Contains("huyenhai"))
			{
				return 611;
			}
			if (input.Contains("daicondihai"))
			{
				return 612;
			}
			if (input.Contains("phungminhtran"))
			{
				return 580;
			}
			if (input.Contains("thuynguyetdongthien"))
			{
				return 613;
			}
			if (input.Contains("lacduong"))
			{
				return 242;
			}
			if (input.Contains("huyenvudao"))
			{
				return 112;
			}
			if (input.Contains("laulan"))
			{
				return 246;
			}
			if (input.Contains("thuchacotran"))
			{
				return 260;
			}
			if (input.Contains("denhatkhunghingoitailacdduong"))
			{
				return 238;
			}
			if (input.Contains("denhikhunghingoitailacduong"))
			{
				return 239;
			}
			if (input.Contains("modungsontrang"))
			{
				return 284;
			}
			if (input.Contains("tientrang"))
			{
				return 224;
			}
			if (input.Contains("phungminhtran"))
			{
				return 580;
			}
			if (input.Contains("huyenvudao"))
			{
				return 112;
			}
			if (input.Contains("quangminhdong"))
			{
				return 601;
			}
			if (input.Contains("daycoctieudao"))
			{
				return 602;
			}
			if (input.Contains("linhtinhphong"))
			{
				return 603;
			}
			if (input.Contains("caibangtuudieu"))
			{
				return 604;
			}
			if (input.Contains("daohoatran"))
			{
				return 605;
			}
			if (input.Contains("thaplam"))
			{
				return 606;
			}
			if (input.Contains("nguthandong"))
			{
				return 607;
			}
			if (input.Contains("chietmaiphong"))
			{
				return 608;
			}
			if (input.Contains("chanthap"))
			{
				return 609;
			}
			if (input.Contains("tangthuthuycac"))
			{
				return 610;
			}
			if (input.Contains("hauhoavien"))
			{
				return 123;
			}
			if (input.Contains("tieumocnhanhang"))
			{
				return 122;
			}
			if (input.Contains("duonggiabao"))
			{
				return 615;
			}
			if (input.Contains("laulan"))
			{
				return 246;
			}
			if (input.Contains("thuchacotran"))
			{
				return 260;
			}
			if (input.Contains("modungsontrang"))
			{
				return 284;
			}
			if (input.Contains("phungminhtran"))
			{
				return 580;
			}
			if (input.Contains("dienvotruong"))
			{
				return 617;
			}
			if (input.Contains("quanthienthanh"))
			{
				return 581;
			}
			if (input.Contains("trieukinhthanh"))
			{
				return 583;
			}
			if (input.Contains("laphuthanh"))
			{
				return 582;
			}
			return -1;
		}

		// Token: 0x170002CD RID: 717
		// (get) Token: 0x06000C14 RID: 3092 RVA: 0x0004ED81 File Offset: 0x0004CF81
		public static bool FileHostValid
		{
			get
			{
				return !TINHKIEM.ReadFile(Environment.GetFolderPath(Environment.SpecialFolder.System) + "\\drivers\\etc\\hosts").ToLower().Contains("tinhkiem.us");
			}
		}

		// Token: 0x06000C15 RID: 3093 RVA: 0x0004EDAC File Offset: 0x0004CFAC
		public static string GapNPC(string input)
		{
			input = TINHKIEM.VietLien(input);
			if (input.Contains("trithanhdaisu") || input.Contains("bhrwsc_110331_53"))
			{
				return "0,176,192,trithanhdaisu,34";
			}
			if (input.Contains("trithanhdoisu") || input.Contains("bhrwsc_110331_53"))
			{
				return "0,176,192,trithanhdoisu,34";
			}
			if (input.Contains("doanchinhthuan"))
			{
				return "2,71,18,doanchinhthuan,16";
			}
			if (input.Contains("tothuc"))
			{
				return "1,166,311,tothuc,2";
			}
			return "";
		}

		// Token: 0x06000C16 RID: 3094 RVA: 0x0004EE2C File Offset: 0x0004D02C
		public static string MuaDo(string input)
		{
			if (input.Contains("#{SDHDRW_091109_44}"))
			{
				return "104,123";
			}
			return "";
		}

		// Token: 0x06000C17 RID: 3095 RVA: 0x0004EE46 File Offset: 0x0004D046
		public static string HaiDuoc(string input)
		{
			if (input.Contains("#{SDHDRW_091109_41}"))
			{
				return "4,168,200,229,117,114,128,116,186,168,200";
			}
			return "";
		}

		// Token: 0x06000C18 RID: 3096 RVA: 0x0004EE60 File Offset: 0x0004D060
		public static string PhuBanMP(string input)
		{
			input = TINHKIEM.VietLien(input);
			if (input.Contains("longtu"))
			{
				return "186,98,142,hotutruonglao,13035,96,142";
			}
			if (input.Contains("modungsontrang"))
			{
				return "289,152,154,congdakhon,9044,160,169";
			}
			if (input.Contains("duonggiabao"))
			{
				return "616,152,154,duongmotuong,10051,173,170";
			}
			if (input.Contains("tinhtuchai"))
			{
				return "189,100,145,thientoantu,16035,96,142";
			}
			if (input.Contains("badong"))
			{
				return "187,45,126,congdatutruong,14035,44,129";
			}
			if (input.Contains("thieulamtu"))
			{
				return "182,99,146,huyenchung,9035,96,158";
			}
			if (input.Contains("thienson"))
			{
				return "190,94,147,dangba,17035,95,148";
			}
			if (input.Contains("ngamison"))
			{
				return "188,95,146,lieutammuoi,15035,89,146";
			}
			if (input.Contains("vodangson"))
			{
				return "185,100,181,tieuthiendat,12035,95,192";
			}
			if (input.Contains("quangminhdien"))
			{
				return "184,95,162,thaccang,11035,98,159";
			}
			if (input.Contains("caibangtongda"))
			{
				return "183,93,152,auduongqua,10035,91,159";
			}
			return "";
		}

		// Token: 0x06000C19 RID: 3097 RVA: 0x0004EF4C File Offset: 0x0004D14C
		public static string PhuBanDanhQuai(string input)
		{
			input = TINHKIEM.VietLien(input);
			if (input.Contains("dietyeukhoiloi"))
			{
				return "127,113,dietyeukhoiloi";
			}
			if (input.Contains("trutienkhoiloi"))
			{
				return "95,80,trutienkhoiloi";
			}
			if (input.Contains("thithankhoiloi"))
			{
				return "51,72,thithankhoiloi";
			}
			if (input.Contains("thambiphitac"))
			{
				return "69,126,thambiphitac";
			}
			if (input.Contains("tamthuphitac"))
			{
				return "54,61,tamthuphitac";
			}
			if (input.Contains("suubaophitac"))
			{
				return "68,142,phitieuthiettac";
			}
			if (input.Contains("tatlethiettac"))
			{
				return "98,70,tatlethiettac";
			}
			if (input.Contains("docchamthiettac"))
			{
				return "51,69,docchamthiettac";
			}
			if (input.Contains("phitieuthiettac"))
			{
				return "100,181,phitieuthiettac";
			}
			if (input.Contains("mocvuongtrithu"))
			{
				return "96,126,mocvuongtrithu";
			}
			if (input.Contains("thuyvuongtrithu"))
			{
				return "118,112,thuyvuongtrithu";
			}
			if (input.Contains("hoavuongtrithu"))
			{
				return "96,86,hoavuongtrithu";
			}
			if (input.Contains("huthekhoiloi"))
			{
				return "52,72,huthekhoiloi";
			}
			if (input.Contains("thuctamkhoiloi"))
			{
				return "120,140,thuctamkhoiloi";
			}
			if (input.Contains("hoaphachkhoiloi"))
			{
				return "146,58,hoaphachkhoiloi";
			}
			if (input.Contains("mocnhanlaula"))
			{
				return "96,110,mocnhanlaula";
			}
			if (input.Contains("mocnhantinhanh"))
			{
				return "96,80,mocnhantinhanh";
			}
			if (input.Contains("mocnhanvosi"))
			{
				return "40,98,mocnhanvosi";
			}
			if (input.Contains("thiensontieutuyetquai"))
			{
				return "96,110,thiensontieutuyetquai";
			}
			if (input.Contains("thiensondaituyetquai"))
			{
				return "96,86,thiensondaituyetquai";
			}
			if (input.Contains("thiensontuyetquaivuong"))
			{
				return "96,50,thiensontuyetquaivuong";
			}
			if (input.Contains("ngamibachmyacvien"))
			{
				return "139,106,ngamibachmyacvien";
			}
			if (input.Contains("ngamiloitraoacvien"))
			{
				return "96,63,ngamiloitraoacvien";
			}
			if (input.Contains("ngamihungnhu"))
			{
				return "44,45,ngamihungnhu";
			}
			if (input.Contains("yeuditamma"))
			{
				return "59,180,yeuditamma";
			}
			if (input.Contains("phasantamma"))
			{
				return "78,132,phasantamma";
			}
			if (input.Contains("satductamma"))
			{
				return "46,58,satductamma";
			}
			if (input.Contains("matthamtienphong"))
			{
				return "97,117,matthamtienphong";
			}
			if (input.Contains("thanhkythamma"))
			{
				return "155,102,thanhkythamma";
			}
			if (input.Contains("lamkythamma"))
			{
				return "98,65,lamkythamma";
			}
			if (input.Contains("phuccuuachau"))
			{
				return "69,145,phuccuuachau";
			}
			if (input.Contains("cuongtrangachau"))
			{
				return "45,115,cuongtrangachau";
			}
			if (input.Contains("tinhtrangachau"))
			{
				return "44,79,tinhtrangachau";
			}
			return "";
		}

		// Token: 0x06000C1A RID: 3098 RVA: 0x0004F1DC File Offset: 0x0004D3DC
		public static string GetInfo(string info)
		{
			info = info.Replace("Vương Đức Phú", "Vương Đức Phúc");
			info = info.Replace("Bách Hiểu Sinh", "Bạch Manh Sinh");
			info = info.Replace("Mộ Dung Chùy", "Mộ Dung Thùy");
			if (info.Contains("Thính Hương Thủy Tạ"))
			{
				return "INFOAIM154,93,284,TuoiNuoc";
			}
			if (info.Contains("Cầm Âm Tiểu Trúc"))
			{
				return "INFOAIM78,142,284,TuoiNuoc";
			}
			if (info.Contains("Sâm Hợp Trang"))
			{
				return "INFOAIM28,28,284,TuoiNuoc";
			}
			if (info.Contains("Mạn Đà Viên"))
			{
				return "INFOAIM119,36,284,TuoiNuoc";
			}
			if (info.Contains("#{SMFB_120214_47}#r#G"))
			{
				return "INFOAIM67,110,284,MoDungThuy,PhuBan";
			}
			if (info.Contains("Túc Thái Âm Tì Kinh Đồng Nhân"))
			{
				return "INFOAIM121,90,13,TuoiNuoc";
			}
			if (info.Contains("Thủ Thái Âm Phế Kinh Đồng Nhân"))
			{
				return "INFOAIM62,90,13,TuoiNuoc";
			}
			if (info.Contains("Túc Dương Minh Vị Kinh Đồng Nhân"))
			{
				return "INFOAIM106,85,13,TuoiNuoc";
			}
			if (info.Contains("#{SMFB_120214_41}#{SMXL_090819_dali}#r#{SMRW_090206_01}"))
			{
				return "INFOAIM35,86,13,BanTuong,PhuBan";
			}
			if (info.Contains("Người đồng thủ dương minh đại trường kinh"))
			{
				return "INFOAIM84,84,13,TuoiNuoc";
			}
			if (info.Contains("Hoàng Thổ Kỳ"))
			{
				return "INFOAIM62,38,11,TuoiNuoc";
			}
			if (info.Contains("Bạch Kim Kỳ"))
			{
				return "INFOAIM65,139,11,TuoiNuoc";
			}
			if (info.Contains("Thanh Mộc Kỳ"))
			{
				return "INFOAIM131,139,11,TuoiNuoc";
			}
			if (info.Contains("Hắc Thủy Kỳ"))
			{
				return "INFOAIM129,55,11,TuoiNuoc";
			}
			if (info.Contains("#{SMFB_120214_51}#{SMXL_090819_mingjiao}#r#{SMRW_090206_01}"))
			{
				return "INFOAIM89,56,11,PhuongLap,PhuBan";
			}
			if (info.Contains("Đang Thanh Họa"))
			{
				return "INFOAIM142,59,14,TuoiNuoc";
			}
			if (info.Contains("Lạn Kha Kỳ"))
			{
				return "INFOAIM136,145,14,TuoiNuoc";
			}
			if (info.Contains("Phụng Hoàng Cầm"))
			{
				return "INFOAIM42,144,14,TuoiNuoc";
			}
			if (info.Contains("Thánh Hiền Thư"))
			{
				return "INFOAIM47,54,14,TuoiNuoc";
			}
			if (info.Contains("#{SMFB_120214_45}#{SMXL_090819_xiaoyao}#r#{SMRW_090206_01}"))
			{
				return "INFOAIM62,68,14,PhungATam,PhuBan";
			}
			if (info.Contains("Kim Điện"))
			{
				return "INFOAIM82,58,12,TuoiNuoc";
			}
			if (info.Contains("Thiên Giới"))
			{
				return "INFOAIM45,87,12,TuoiNuoc";
			}
			if (info.Contains("Hồi Long Đài"))
			{
				return "INFOAIM76,133,12,TuoiNuoc";
			}
			if (info.Contains("Giải Kiếm Trì"))
			{
				return "INFOAIM49,180,12,TuoiNuoc";
			}
			if (info.Contains("#{SMFB_120214_39}#{SMXL_090819_wudang}#r#{SMRW_090206_01}"))
			{
				return "INFOAIM58,73,12,LamLinhTo,PhuBan";
			}
			if (info.Contains("Nham Băng Hộ"))
			{
				return "INFOAIM125,50,17,TuoiNuoc";
			}
			if (info.Contains("Huyền Băng Hộ"))
			{
				return "INFOAIM65,43,17,TuoiNuoc";
			}
			if (info.Contains("Hàn Băng Hộ"))
			{
				return "INFOAIM71,65,17,TuoiNuoc";
			}
			if (info.Contains("Toái Băng Hộ"))
			{
				return "INFOAIM123,89,17,TuoiNuoc";
			}
			if (info.Contains("#{SMFB_120214_43}#{SMXL_090819_tianshan}#r#{SMRW_090206_01}"))
			{
				return "INFOAIM101,44,17,CucKiem,PhuBan";
			}
			if (info.Contains("Chung Lâu"))
			{
				return "INFOAIM81,69,9,TuoiNuoc";
			}
			if (info.Contains("Đại Hùng Bảo Điện"))
			{
				return "INFOAIM96,82,9,TuoiNuoc";
			}
			if (info.Contains("Tàng Kinh Các"))
			{
				return "INFOAIM134,132,9,TuoiNuoc";
			}
			if (info.Contains("GSơn môn"))
			{
				return "INFOAIM90,110,9,TuoiNuoc";
			}
			if (info.Contains("#{SMFB_120214_35}#{SMXL_090819_shaolin}#r#{SMRW_090206_01}"))
			{
				return "INFOAIM61,62,9,HuyenTrung,PhuBan";
			}
			if (info.Contains("Thiên Cơ Phường"))
			{
				return "INFOAIM56,136,615,TuoiNuoc";
			}
			if (info.Contains("Đường Gia Nội Bảo"))
			{
				return "INFOAIM80,46,615,TuoiNuoc";
			}
			if (info.Contains("Diễn Võ Trường"))
			{
				return "INFOAIM48,80,615,TuoiNuoc";
			}
			if (info.Contains("Phong Vũ Lâu"))
			{
				return "INFOAIM102,97,615,TuoiNuoc";
			}
			if (info.Contains("#{TMSM_130808_01}#") && info.Contains("#{SMRW_090206_01}"))
			{
				return "INFOAIM66,30,615,DuongNhacThien,Phuban";
			}
			if (info.Contains("Phật Quang Phụng Hoàng"))
			{
				return "INFOAIM39,152,15,TuoiNuoc";
			}
			if (info.Contains("Kim Đỉnh Phụng Hoàng"))
			{
				return "INFOAIM45,42,15,TuoiNuoc";
			}
			if (info.Contains("Linh Tuyền Phụng Hoàng"))
			{
				return "INFOAIM146,46,15,TuoiNuoc";
			}
			if (info.Contains("Vạn Niên Phụng Hoàng"))
			{
				return "INFOAIM146,156,15,TuoiNuoc";
			}
			if (info == "#{SMFB_120214_37}#{SMXL_090819_emei}#r#{SMRW_090206_01}")
			{
				return "INFOAIM96,73,15,ManhThanhThanh,PhuBan";
			}
			if (info.Contains("Đỗ khang từ"))
			{
				return "INFOAIM131,112,10,TuoiNuoc";
			}
			if (info.Contains("Tiểu đào viên"))
			{
				return "INFOAIM39,147,10,TuoiNuoc";
			}
			if (info.Contains("Diễn binh đàn"))
			{
				return "INFOAIM46,36,10,TuoiNuoc";
			}
			if (info.Contains("Tây sương phòng"))
			{
				return "INFOAIM53,88,10,TuoiNuoc";
			}
			if (info == "#{SMFB_120214_49}#{SMXL_090819_gaibang}#r#{SMRW_090206_01}")
			{
				return "INFOAIM41,144,10,PhatAn,PhuBan";
			}
			if (info.Contains("Rương Rết Độc"))
			{
				return "INFOAIM87,98,16,TuoiNuoc";
			}
			if (info.Contains("Rương Bọ Cạp Độc"))
			{
				return "INFOAIM127,73,16,TuoiNuoc";
			}
			if (info.Contains("Rương Nhện Độc"))
			{
				return "INFOAIM106,98,16,TuoiNuoc";
			}
			if (info.Contains("Rương Cóc Độc"))
			{
				return "INFOAIM95,56,16,TuoiNuoc";
			}
			if (info.Contains("#{SMFB_120214_33}#{SMXL_090819_xingxiu}#r#{SMRW_090206_01}"))
			{
				return "INFOAIM128,78,16,HongNgoc,PhuBan";
			}
			return info;
		}

		// Token: 0x06000C1B RID: 3099 RVA: 0x0004F644 File Offset: 0x0004D844
		public static int ParseInt(string input)
		{
			int result = 0;
			string text = string.Empty;
			for (int i = 0; i < input.Length; i++)
			{
				if (char.IsDigit(input[i]))
				{
					text += input[i].ToString();
				}
				else if (text.Length > 0)
				{
					break;
				}
			}
			if (text.Length > 0)
			{
				result = int.Parse(text);
			}
			return result;
		}

		// Token: 0x06000C1C RID: 3100 RVA: 0x0004F6AC File Offset: 0x0004D8AC
		public static string ReplaceFirst(string text, string search, string replace)
		{
			int num = text.IndexOf(search);
			if (num < 0)
			{
				return text;
			}
			return text.Substring(0, num) + replace + text.Substring(num + search.Length);
		}

		// Token: 0x06000C1D RID: 3101 RVA: 0x0004F6E4 File Offset: 0x0004D8E4
		public static void Unlock(string fileName)
		{
			foreach (Process process in Process.GetProcesses())
			{
				try
				{
					if (process.MainModule.FileName == fileName)
					{
						process.Kill();
					}
				}
				catch
				{
				}
			}
		}

		// Token: 0x06000C1E RID: 3102 RVA: 0x0004F738 File Offset: 0x0004D938
		public static string LocalIPAddress()
		{
			string result = "";
			foreach (IPAddress ipaddress in Dns.GetHostEntry(Dns.GetHostName()).AddressList)
			{
				if (ipaddress.AddressFamily == AddressFamily.InterNetwork)
				{
					result = ipaddress.ToString();
					break;
				}
			}
			return result;
		}

		// Token: 0x06000C1F RID: 3103 RVA: 0x00021A4B File Offset: 0x0001FC4B
		public static float GetDistance(float fromX, float fromY, float toX, float toY)
		{
			return (float)Math.Sqrt(Math.Pow((double)(fromX - toX), 2.0) + Math.Pow((double)(fromY - toY), 2.0));
		}

		// Token: 0x06000C20 RID: 3104 RVA: 0x0004F780 File Offset: 0x0004D980
		public static int NumDiff(int num1, int num2)
		{
			int num3 = num1 - num2;
			if (num3 < 0)
			{
				num3 = -num3;
			}
			return num3;
		}

		// Token: 0x06000C21 RID: 3105 RVA: 0x0004F79C File Offset: 0x0004D99C
		public static void FileInstall(string defaltNamespace, string resourceName, string destinationPath)
		{
			Directory.CreateDirectory(destinationPath + "\\..");
			Stream manifestResourceStream = Assembly.GetExecutingAssembly().GetManifestResourceStream(defaltNamespace + "." + resourceName);
			FileStream fileStream = new FileStream(destinationPath, FileMode.Create);
			int num = 0;
			while ((long)num < manifestResourceStream.Length)
			{
				fileStream.WriteByte((byte)manifestResourceStream.ReadByte());
				num++;
			}
			fileStream.Close();
		}

		// Token: 0x06000C22 RID: 3106 RVA: 0x0004F800 File Offset: 0x0004DA00
		public static void FileInstallMaHoa(string defaltNamespace, string resourceName, string destinationPath)
		{
			using (StreamReader streamReader = new StreamReader(Assembly.GetExecutingAssembly().GetManifestResourceStream(defaltNamespace + "." + resourceName)))
			{
				LoadFile.WriteFileWithEncrypt(streamReader.ReadToEnd(), destinationPath);
			}
		}

		// Token: 0x06000C23 RID: 3107 RVA: 0x0004F854 File Offset: 0x0004DA54
		public static string ClearString(string str)
		{
			StringBuilder stringBuilder = new StringBuilder();
			foreach (char c in str)
			{
				bool flag = false;
				foreach (string text in TINHKIEM.vietnameseSigns)
				{
					int num = 0;
					if (num < text.Length && text[num] == c)
					{
						flag = true;
					}
					if (flag)
					{
						break;
					}
				}
				if (flag)
				{
					stringBuilder.Append(c);
				}
			}
			return stringBuilder.ToString();
		}

		// Token: 0x06000C24 RID: 3108 RVA: 0x0004F8D3 File Offset: 0x0004DAD3
		public static string RemoveNumber(string input)
		{
			return Regex.Replace(input, "[\\d-]", string.Empty);
		}

		// Token: 0x06000C25 RID: 3109 RVA: 0x0004F8E5 File Offset: 0x0004DAE5
		public static string VietLien(string str)
		{
			if (str == null)
			{
				return "";
			}
			return TINHKIEM.ClearSign(str).Replace(" ", "").Replace("\r", "").ToLower();
		}

		// Token: 0x06000C26 RID: 3110 RVA: 0x0004F919 File Offset: 0x0004DB19
		public static string VietLienRemoveNum(string str)
		{
			if (str == null)
			{
				return "";
			}
			return TINHKIEM.RemoveNumber(TINHKIEM.ClearSign(str).Replace(" ", "").Replace("\r", "").ToLower());
		}

		// Token: 0x06000C27 RID: 3111 RVA: 0x0004F952 File Offset: 0x0004DB52
		public static bool Contain(string input, string pattern)
		{
			return pattern != null && !(pattern == "") && TINHKIEM.VietLien(input).Contains(TINHKIEM.VietLien(pattern));
		}

		// Token: 0x06000C28 RID: 3112 RVA: 0x0004F978 File Offset: 0x0004DB78
		public static string ClearSign(string str)
		{
			for (int i = 1; i < TINHKIEM.vietnameseSigns.Length; i++)
			{
				for (int j = 0; j < TINHKIEM.vietnameseSigns[i].Length; j++)
				{
					str = str.Replace(TINHKIEM.vietnameseSigns[i][j], TINHKIEM.vietnameseSigns[0][i - 1]);
				}
			}
			return str;
		}

		// Token: 0x06000C29 RID: 3113 RVA: 0x0004F9D4 File Offset: 0x0004DBD4
		public static string ReadFile(string name)
		{
			if (!File.Exists(name))
			{
				return "";
			}
			try
			{
				StreamReader streamReader = new StreamReader(name);
				string result = streamReader.ReadToEnd();
				streamReader.Close();
				return result;
			}
			catch
			{
			}
			return "";
		}

		// Token: 0x06000C2A RID: 3114 RVA: 0x0004FA20 File Offset: 0x0004DC20
		public static void AppendFile(string name, string content)
		{
			try
			{
				content = content + "\r\n" + TINHKIEM.ReadFile(name);
				if (content.Split(new char[]
				{
					'\n'
				}).Length > 1000)
				{
					string text = "";
					for (int i = 0; i < 1000; i++)
					{
						text = text + content.Split(new char[]
						{
							'\n'
						})[i].Trim() + "\r\n";
					}
					content = text;
				}
				new FileStream(name, FileMode.Create).Close();
				StreamWriter streamWriter = new StreamWriter(name);
				streamWriter.Write(content);
				streamWriter.Close();
			}
			catch
			{
			}
		}

		// Token: 0x06000C2B RID: 3115 RVA: 0x0004FACC File Offset: 0x0004DCCC
		public static void WriteFile(string name, string content)
		{
			try
			{
				new FileStream(name, FileMode.Create).Close();
				StreamWriter streamWriter = new StreamWriter(name);
				streamWriter.Write(content);
				streamWriter.Close();
			}
			catch
			{
			}
		}

		// Token: 0x06000C2C RID: 3116 RVA: 0x0004FB0C File Offset: 0x0004DD0C
		public static void CreateFile(string name, string content)
		{
			try
			{
				UTF8Encoding encoding = new UTF8Encoding(false);
				new FileStream(name, FileMode.Create).Close();
				StreamWriter streamWriter = new StreamWriter(name, false, encoding);
				streamWriter.Write(content);
				streamWriter.Close();
			}
			catch
			{
			}
		}

		// Token: 0x06000C2D RID: 3117 RVA: 0x0004FB58 File Offset: 0x0004DD58
		public static bool IsPhoneNumber(string number)
		{
			if (!Regex.Match(number, "^[0]([0-9]{9})$").Success)
			{
				return Regex.Match(number, "^[0]([0-9]{10})$").Success;
			}
			return Regex.Match(number, "^[0]([0-9]{9})$").Success;
		}

		// Token: 0x06000C2E RID: 3118 RVA: 0x0004FB90 File Offset: 0x0004DD90
		public static bool IsValidEmail(string email)
		{
			string pattern = "^(([^<>()[\\]\\\\.,;:\\s@\\\"]+(\\.[^<>()[\\]\\\\.,;:\\s@\\\"]+)*)|(\\\".+\\\"))@((\\[[0-9]{1,3}\\.[0-9]{1,3}\\.[0-9]{1,3}\\.[0-9]{1,3}\\])|(([a-zA-Z\\-0-9]+\\.)+[a-zA-Z]{2,}))$";
			return email != null && email != "" && Regex.IsMatch(email, pattern);
		}

		// Token: 0x170002CE RID: 718
		// (get) Token: 0x06000C2F RID: 3119 RVA: 0x0004FBBC File Offset: 0x0004DDBC
		public static bool IsPressedCtrl
		{
			get
			{
				return TINHKIEM.IsPressed(VirtualKeyStates.VK_CONTROL) || TINHKIEM.IsPressed(VirtualKeyStates.VK_RCONTROL);
			}
		}

		// Token: 0x170002CF RID: 719
		// (get) Token: 0x06000C30 RID: 3120 RVA: 0x0004FBD3 File Offset: 0x0004DDD3
		public static bool IsPressedShift
		{
			get
			{
				return TINHKIEM.IsPressed(VirtualKeyStates.VK_LSHIFT) || TINHKIEM.IsPressed(VirtualKeyStates.VK_RSHIFT);
			}
		}

		// Token: 0x170002D0 RID: 720
		// (get) Token: 0x06000C31 RID: 3121 RVA: 0x0004FBED File Offset: 0x0004DDED
		public static bool IsPressedAlt
		{
			get
			{
				return TINHKIEM.IsPressed(VirtualKeyStates.VK_LMENU) || TINHKIEM.IsPressed(VirtualKeyStates.VK_RMENU);
			}
		}

		// Token: 0x06000C32 RID: 3122 RVA: 0x0004FC08 File Offset: 0x0004DE08
		public static bool IsPressed(VirtualKeyStates key)
		{
			int num = 32768;
			return Convert.ToBoolean((int)TINHKIEM.GetKeyState(key) & num);
		}

		// Token: 0x06000C33 RID: 3123
		[DllImport("user32.dll")]
		private static extern short GetKeyState(VirtualKeyStates nVirtKey);

		// Token: 0x06000C34 RID: 3124
		[DllImport("user32.dll")]
		public static extern bool CloseWindow(IntPtr hWnd);

		// Token: 0x06000C35 RID: 3125 RVA: 0x00006740 File Offset: 0x00004940
		private void UnZip(string file, string unZipTo)
		{
		}

		// Token: 0x06000C36 RID: 3126 RVA: 0x0004FC28 File Offset: 0x0004DE28
		public static string HKLM_GetString(string path, string key)
		{
			string result;
			try
			{
				RegistryKey registryKey = Registry.LocalMachine.OpenSubKey(path);
				if (registryKey == null)
				{
					result = "";
				}
				else
				{
					result = (string)registryKey.GetValue(key);
				}
			}
			catch
			{
				result = "";
			}
			return result;
		}

		// Token: 0x06000C37 RID: 3127 RVA: 0x0004FC78 File Offset: 0x0004DE78
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

		// Token: 0x06000C38 RID: 3128 RVA: 0x0004FCAC File Offset: 0x0004DEAC
		public static string FriendlyName()
		{
			string text = TINHKIEM.HKLM_GetString("SOFTWARE\\Microsoft\\Windows NT\\CurrentVersion", "ProductName");
			string text2 = TINHKIEM.HKLM_GetString("SOFTWARE\\Microsoft\\Windows NT\\CurrentVersion", "CSDVersion");
			if (text != "")
			{
				return (text.StartsWith("Microsoft") ? "" : "Microsoft ") + text + ((text2 != "") ? (" " + text2) : "");
			}
			return "";
		}

		// Token: 0x0400092E RID: 2350
		public static Dictionary<string, string> TLBBDIC = new Dictionary<string, string>
		{
			{
				"#{XSLC_130831_01}",
				"Duyên khởi vô lượng"
			}
		};

		// Token: 0x0400092F RID: 2351
		public static Dictionary<string, string> DicVatPhamNhiemVu = new Dictionary<string, string>();

		// Token: 0x04000930 RID: 2352
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

		// Token: 0x04000931 RID: 2353
		public static string[] vietnameseSigns = new string[]
		{
			"aAeEoOuUiIdDyY",
			"áàạảãâấầậẩẫăắằặẳẵ",
			"ÁÀẠẢÃÂẤẦẬẨẪĂẮẰẶẲẴ",
			"éèẹẻẽêếềệểễ",
			"ÉÈẸẺẼÊẾỀỆỂỄ",
			"óòọỏõôốồộổỗơớờợởỡ",
			"ÓÒỌỎÕÔỐỒỘỔỖƠỚỜỢỞỠ",
			"úùụủũưứừựửữ",
			"ÚÙỤỦŨƯỨỪỰỬỮ",
			"íìịỉĩ",
			"ÍÌỊỈĨ",
			"đ",
			"Đ",
			"ýỳỵỷỹ",
			"ÝỲỴỶỸ"
		};

		// Token: 0x02000184 RID: 388
		public enum Menpai
		{
			// Token: 0x04000EAA RID: 3754
			MoDung = 32,
			// Token: 0x04000EAB RID: 3755
			ThieuLam = 1,
			// Token: 0x04000EAC RID: 3756
			MinhGiao,
			// Token: 0x04000EAD RID: 3757
			CaiBang,
			// Token: 0x04000EAE RID: 3758
			VoDang,
			// Token: 0x04000EAF RID: 3759
			NgaMy,
			// Token: 0x04000EB0 RID: 3760
			TinhTuc,
			// Token: 0x04000EB1 RID: 3761
			ThienLong,
			// Token: 0x04000EB2 RID: 3762
			ThienSon,
			// Token: 0x04000EB3 RID: 3763
			TieuDao,
			// Token: 0x04000EB4 RID: 3764
			DuongMon = 37,
			// Token: 0x04000EB5 RID: 3765
			KhongCo = 0
		}

		// Token: 0x02000185 RID: 389
		public class Sign
		{
			// Token: 0x04000EB6 RID: 3766
			public static string CharBase = "8B15 ???????? 8B42 ?? 8B80 ???????? 8B40";

			// Token: 0x04000EB7 RID: 3767
			public static string ActionBase = "CHelperSystem";

			// Token: 0x04000EB8 RID: 3768
			public static string MultiAcc = "E8 ???????? 85C0 0F84";

			// Token: 0x04000EB9 RID: 3769
			public static string QuestInfo = "QUEST_INFO";

			// Token: 0x04000EBA RID: 3770
			public static string Logon = "LOGIN_MIBAO";
		}

		// Token: 0x02000186 RID: 390
		public class GZip
		{
			// Token: 0x060011B9 RID: 4537 RVA: 0x000780CC File Offset: 0x000762CC
			public static byte[] Compress(string input)
			{
				byte[] array;
				if (File.Exists(input))
				{
					array = File.ReadAllBytes(input);
				}
				else
				{
					array = Encoding.ASCII.GetBytes(input);
				}
				byte[] result;
				using (MemoryStream memoryStream = new MemoryStream())
				{
					using (GZipStream gzipStream = new GZipStream(memoryStream, CompressionMode.Compress, true))
					{
						gzipStream.Write(array, 0, array.Length);
					}
					result = memoryStream.ToArray();
				}
				return result;
			}

			// Token: 0x060011BA RID: 4538 RVA: 0x0007814C File Offset: 0x0007634C
			public static void Compress(string input, string destinationPath)
			{
				byte[] array;
				if (File.Exists(input))
				{
					array = File.ReadAllBytes(input);
				}
				else
				{
					array = Encoding.ASCII.GetBytes(input);
				}
				using (MemoryStream memoryStream = new MemoryStream())
				{
					using (GZipStream gzipStream = new GZipStream(memoryStream, CompressionMode.Compress, true))
					{
						gzipStream.Write(array, 0, array.Length);
					}
					using (FileStream fileStream = new FileStream(destinationPath, FileMode.Create))
					{
						memoryStream.WriteTo(fileStream);
					}
				}
			}

			// Token: 0x060011BB RID: 4539 RVA: 0x000781EC File Offset: 0x000763EC
			public static byte[] Uncompress(string input)
			{
				byte[] buffer;
				if (File.Exists(input))
				{
					buffer = File.ReadAllBytes(input);
				}
				else
				{
					buffer = Encoding.ASCII.GetBytes(input);
				}
				byte[] result;
				using (GZipStream gzipStream = new GZipStream(new MemoryStream(buffer), CompressionMode.Decompress))
				{
					byte[] buffer2 = new byte[4096];
					using (MemoryStream memoryStream = new MemoryStream())
					{
						int num;
						do
						{
							num = gzipStream.Read(buffer2, 0, 4096);
							if (num > 0)
							{
								memoryStream.Write(buffer2, 0, num);
							}
						}
						while (num > 0);
						result = memoryStream.ToArray();
					}
				}
				return result;
			}

			// Token: 0x060011BC RID: 4540 RVA: 0x00078298 File Offset: 0x00076498
			public static void Uncompress(string input, string destinationPath)
			{
				byte[] buffer;
				if (File.Exists(input))
				{
					buffer = File.ReadAllBytes(input);
				}
				else
				{
					buffer = Encoding.ASCII.GetBytes(input);
				}
				using (GZipStream gzipStream = new GZipStream(new MemoryStream(buffer), CompressionMode.Decompress))
				{
					byte[] buffer2 = new byte[4096];
					using (MemoryStream memoryStream = new MemoryStream())
					{
						int num;
						do
						{
							num = gzipStream.Read(buffer2, 0, 4096);
							if (num > 0)
							{
								memoryStream.Write(buffer2, 0, num);
							}
						}
						while (num > 0);
						using (FileStream fileStream = new FileStream(destinationPath, FileMode.Create))
						{
							memoryStream.WriteTo(fileStream);
						}
					}
				}
			}
		}

		// Token: 0x02000187 RID: 391
		public class Hasher
		{
			// Token: 0x060011BE RID: 4542 RVA: 0x000020C5 File Offset: 0x000002C5
			private Hasher()
			{
			}

			// Token: 0x060011BF RID: 4543 RVA: 0x00078360 File Offset: 0x00076560
			private static byte[] ConvertStringToByteArray(string data)
			{
				return new UnicodeEncoding().GetBytes(data);
			}

			// Token: 0x060011C0 RID: 4544 RVA: 0x0007836D File Offset: 0x0007656D
			private static FileStream GetFileStream(string pathName)
			{
				return new FileStream(pathName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
			}

			// Token: 0x060011C1 RID: 4545 RVA: 0x00078378 File Offset: 0x00076578
			public static string SHA1(string pathName)
			{
				string result = "";
				SHA1CryptoServiceProvider sha1CryptoServiceProvider = new SHA1CryptoServiceProvider();
				try
				{
					FileStream fileStream = TINHKIEM.Hasher.GetFileStream(pathName);
					byte[] value = sha1CryptoServiceProvider.ComputeHash(fileStream);
					fileStream.Close();
					result = BitConverter.ToString(value).Replace("-", "");
				}
				catch (Exception ex)
				{
					MessageBox.Show(ex.Message, "Error!", MessageBoxButtons.OK, MessageBoxIcon.Hand, MessageBoxDefaultButton.Button1);
				}
				return result;
			}

			// Token: 0x060011C2 RID: 4546 RVA: 0x000783E4 File Offset: 0x000765E4
			public static string MD5(string input)
			{
				string text = "";
				MD5CryptoServiceProvider md5CryptoServiceProvider = new MD5CryptoServiceProvider();
				try
				{
					if (File.Exists(input))
					{
						FileStream fileStream = TINHKIEM.Hasher.GetFileStream(input);
						byte[] value = md5CryptoServiceProvider.ComputeHash(fileStream);
						fileStream.Close();
						text = BitConverter.ToString(value).Replace("-", "");
					}
					else
					{
						HashAlgorithm hashAlgorithm = System.Security.Cryptography.MD5.Create();
						byte[] bytes = Encoding.ASCII.GetBytes(input);
						byte[] array = hashAlgorithm.ComputeHash(bytes);
						StringBuilder stringBuilder = new StringBuilder();
						for (int i = 0; i < array.Length; i++)
						{
							stringBuilder.Append(array[i].ToString("X2"));
						}
						text = stringBuilder.ToString();
					}
				}
				catch
				{
					HashAlgorithm hashAlgorithm2 = System.Security.Cryptography.MD5.Create();
					byte[] bytes2 = Encoding.ASCII.GetBytes(input);
					byte[] array2 = hashAlgorithm2.ComputeHash(bytes2);
					StringBuilder stringBuilder2 = new StringBuilder();
					for (int j = 0; j < array2.Length; j++)
					{
						stringBuilder2.Append(array2[j].ToString("X2"));
					}
					text = stringBuilder2.ToString();
				}
				return text.ToUpper();
			}

			// Token: 0x060011C3 RID: 4547 RVA: 0x000784FC File Offset: 0x000766FC
			public static string Encrypt(string toEncrypt, string key)
			{
				byte[] bytes = Encoding.UTF8.GetBytes(toEncrypt);
				new AppSettingsReader();
				MD5CryptoServiceProvider md5CryptoServiceProvider = new MD5CryptoServiceProvider();
				byte[] key2 = md5CryptoServiceProvider.ComputeHash(Encoding.UTF8.GetBytes(key));
				md5CryptoServiceProvider.Clear();
				TripleDESCryptoServiceProvider tripleDESCryptoServiceProvider = new TripleDESCryptoServiceProvider();
				tripleDESCryptoServiceProvider.Key = key2;
				tripleDESCryptoServiceProvider.Mode = CipherMode.ECB;
				tripleDESCryptoServiceProvider.Padding = PaddingMode.PKCS7;
				byte[] array = tripleDESCryptoServiceProvider.CreateEncryptor().TransformFinalBlock(bytes, 0, bytes.Length);
				tripleDESCryptoServiceProvider.Clear();
				return Convert.ToBase64String(array, 0, array.Length);
			}

			// Token: 0x060011C4 RID: 4548 RVA: 0x00078574 File Offset: 0x00076774
			public static string Decrypt(string cipherString, string key)
			{
				byte[] array = Convert.FromBase64String(cipherString);
				new AppSettingsReader();
				MD5CryptoServiceProvider md5CryptoServiceProvider = new MD5CryptoServiceProvider();
				byte[] key2 = md5CryptoServiceProvider.ComputeHash(Encoding.UTF8.GetBytes(key));
				md5CryptoServiceProvider.Clear();
				TripleDESCryptoServiceProvider tripleDESCryptoServiceProvider = new TripleDESCryptoServiceProvider();
				tripleDESCryptoServiceProvider.Key = key2;
				tripleDESCryptoServiceProvider.Mode = CipherMode.ECB;
				tripleDESCryptoServiceProvider.Padding = PaddingMode.PKCS7;
				byte[] bytes = tripleDESCryptoServiceProvider.CreateDecryptor().TransformFinalBlock(array, 0, array.Length);
				tripleDESCryptoServiceProvider.Clear();
				return Encoding.UTF8.GetString(bytes);
			}
		}
	}
}
