using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using TinhKiemAuto.Models;

namespace TinhKiemAuto
{
	// Token: 0x020000DE RID: 222
	internal class Setting
	{
		// Token: 0x170002B4 RID: 692
		// (get) Token: 0x06000B98 RID: 2968 RVA: 0x0004AB1C File Offset: 0x00048D1C
		// (set) Token: 0x06000B99 RID: 2969 RVA: 0x0004AB32 File Offset: 0x00048D32
		public static string BoQua
		{
			get
			{
				return LoadFile.LoadFileWithDecrypt(Global.DataPath + "\\BoQua.dat");
			}
			set
			{
				LoadFile.WriteFileWithEncrypt(value, Global.DataPath + "\\BoQua.dat");
			}
		}

		// Token: 0x170002B5 RID: 693
		// (get) Token: 0x06000B9A RID: 2970 RVA: 0x0004AB49 File Offset: 0x00048D49
		// (set) Token: 0x06000B9B RID: 2971 RVA: 0x0004AB5F File Offset: 0x00048D5F
		public static string DropName
		{
			get
			{
				return LoadFile.LoadFileWithDecrypt(Global.DataPath + "\\DropName.dat");
			}
			set
			{
				LoadFile.WriteFileWithEncrypt(value, Global.DataPath + "\\DropName.dat");
			}
		}

		// Token: 0x170002B6 RID: 694
		// (get) Token: 0x06000B9C RID: 2972 RVA: 0x0004AB76 File Offset: 0x00048D76
		// (set) Token: 0x06000B9D RID: 2973 RVA: 0x0004AB8C File Offset: 0x00048D8C
		public static string DropType
		{
			get
			{
				return LoadFile.LoadFileWithDecrypt(Global.DataPath + "\\DropType.dat");
			}
			set
			{
				LoadFile.WriteFileWithEncrypt(value, Global.DataPath + "\\DropType.dat");
			}
		}

		// Token: 0x170002B7 RID: 695
		// (get) Token: 0x06000B9E RID: 2974 RVA: 0x0004ABA3 File Offset: 0x00048DA3
		// (set) Token: 0x06000B9F RID: 2975 RVA: 0x0004ABB9 File Offset: 0x00048DB9
		public static string AutoEat
		{
			get
			{
				return LoadFile.LoadFileWithDecrypt(Global.DataPath + "\\AutoEat.dat");
			}
			set
			{
				LoadFile.WriteFileWithEncrypt(value, Global.DataPath + "\\AutoEat.dat");
			}
		}

		// Token: 0x06000BA0 RID: 2976 RVA: 0x0004ABD0 File Offset: 0x00048DD0
		public static void SetValue(string Name, string value)
		{
			LoadFile.WriteFileWithEncrypt(value, Global.DataPath + "\\" + Name + ".dat");
		}

		// Token: 0x06000BA1 RID: 2977 RVA: 0x0004ABED File Offset: 0x00048DED
		public static string GetValue(string Name)
		{
			return LoadFile.LoadFileWithDecrypt(Global.DataPath + "\\" + Name + ".dat");
		}

		// Token: 0x170002B8 RID: 696
		// (get) Token: 0x06000BA2 RID: 2978 RVA: 0x0004AC09 File Offset: 0x00048E09
		// (set) Token: 0x06000BA3 RID: 2979 RVA: 0x0004AC1F File Offset: 0x00048E1F
		public static string SellName
		{
			get
			{
				return LoadFile.LoadFileWithDecrypt(Global.DataPath + "\\SellName.dat");
			}
			set
			{
				LoadFile.WriteFileWithEncrypt(value, Global.DataPath + "\\SellName.dat");
			}
		}

		// Token: 0x170002B9 RID: 697
		// (get) Token: 0x06000BA4 RID: 2980 RVA: 0x0004AC36 File Offset: 0x00048E36
		// (set) Token: 0x06000BA5 RID: 2981 RVA: 0x0004AC4C File Offset: 0x00048E4C
		public static string SellType
		{
			get
			{
				return LoadFile.LoadFileWithDecrypt(Global.DataPath + "\\SellType.dat");
			}
			set
			{
				LoadFile.WriteFileWithEncrypt(value, Global.DataPath + "\\SellType.dat");
			}
		}

		// Token: 0x06000BA6 RID: 2982 RVA: 0x0004AC63 File Offset: 0x00048E63
		public static void Write(string section, string key, string value)
		{
			Setting.Ini.Write(section, key, value);
		}

		// Token: 0x06000BA7 RID: 2983 RVA: 0x0004AC72 File Offset: 0x00048E72
		public static string Read(string section, string key)
		{
			return Setting.Ini.Read(section, key);
		}

		// Token: 0x06000BA8 RID: 2984 RVA: 0x0004AC80 File Offset: 0x00048E80
		public static int[] LoadSettingOffline(string name)
		{
			return Setting.String2Arr(LoadFile.LoadFileWithDecrypt(Global.DataPath + "\\" + name));
		}

		// Token: 0x06000BA9 RID: 2985 RVA: 0x0004AC9C File Offset: 0x00048E9C
		public static string LoadStringOffline(string name)
		{
			return LoadFile.LoadFileWithDecrypt(Global.DataPath + "\\" + name);
		}

		// Token: 0x06000BAA RID: 2986 RVA: 0x0004ACB3 File Offset: 0x00048EB3
		public static void SaveSettingOffline(string name, string value)
		{
			LoadFile.WriteFileWithEncrypt(value, Global.DataPath + "\\" + name);
		}

		// Token: 0x06000BAB RID: 2987 RVA: 0x0004ACCB File Offset: 0x00048ECB
		public static void SaveMAP(string name, string value)
		{
			Setting.MapIni.Write("MAP", name, value);
		}

		// Token: 0x06000BAC RID: 2988 RVA: 0x0004ACDE File Offset: 0x00048EDE
		public static string LoadMAP(string name)
		{
			name = Setting.MapIni.Read("MAP", name);
			return name;
		}

		// Token: 0x06000BAD RID: 2989 RVA: 0x0004ACF3 File Offset: 0x00048EF3
		public static void SaveWAY(string name, string value)
		{
			Setting.Ini.Write("WAY", name, value);
		}

		// Token: 0x06000BAE RID: 2990 RVA: 0x0004AD06 File Offset: 0x00048F06
		public static string LoadWAY(string name)
		{
			name = Setting.Ini.Read("WAY", name);
			return name;
		}

		// Token: 0x06000BAF RID: 2991 RVA: 0x0004AD1C File Offset: 0x00048F1C
		public static int[] String2Arr(string str)
		{
			string[] array = str.Split(new char[]
			{
				','
			});
			int[] array2 = new int[array.Length];
			for (int i = 0; i < array2.Length; i++)
			{
				array2[i] = Setting.String2Int(array[i]);
			}
			return array2;
		}

		// Token: 0x06000BB0 RID: 2992 RVA: 0x0004AD60 File Offset: 0x00048F60
		public static int String2Int(string input)
		{
			int result = 0;
			int.TryParse(input, out result);
			return result;
		}

		// Token: 0x06000BB1 RID: 2993 RVA: 0x0004AD7C File Offset: 0x00048F7C
		public static void LoadBUff()
		{
			if (File.Exists(Global.DataPath + "\\BuffNM.dat"))
			{
				try
				{
					Setting.BuffValue = JsonConvert.DeserializeObject<List<BuffPramenter>>(LoadFile.LoadFileWithDecrypt(Global.DataPath + "\\BuffNM.dat"));
				}
				catch
				{
					Setting.BuffValue = new List<BuffPramenter>();
				}
			}
		}

		// Token: 0x06000BB2 RID: 2994 RVA: 0x0004ADDC File Offset: 0x00048FDC
		public static bool CheckBuff(string PlayID)
		{
			return Setting.BuffValue.Find((BuffPramenter x) => TINHKIEM.VietLien(x.IDnguoichoi) == TINHKIEM.VietLien(PlayID)) != null;
		}

		// Token: 0x06000BB3 RID: 2995 RVA: 0x0004AE11 File Offset: 0x00049011
		public static void SaveBuff()
		{
			LoadFile.WriteFileWithEncrypt(JsonConvert.SerializeObject(Setting.BuffValue), Global.DataPath + "\\BuffNM.dat");
		}

		// Token: 0x040008C9 RID: 2249
		public static IniFile Ini = new IniFile(Global.DataPath + "\\Setting.dat");

		// Token: 0x040008CA RID: 2250
		public static IniFile MapIni = new IniFile(Global.DataPath + "\\MapPath.dat");

		// Token: 0x040008CB RID: 2251
		public static List<BuffPramenter> BuffValue = new List<BuffPramenter>();

		// Token: 0x040008CC RID: 2252
		public static string IdLocDo = string.Empty;

		// Token: 0x040008CD RID: 2253
		public static string KoKhaiThac = string.Empty;

		// Token: 0x040008CE RID: 2254
		public static string Leader = string.Empty;
	}
}
