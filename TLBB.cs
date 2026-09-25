using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;

namespace TinhKiemAuto
{
	// Token: 0x020000EC RID: 236
	public class TLBB
	{
		// Token: 0x170002D1 RID: 721
		// (get) Token: 0x06000C3B RID: 3131 RVA: 0x0004FE01 File Offset: 0x0004E001
		public bool IsRelive
		{
			get
			{
				return this.Memory.Read1Byte(this.Memory.ReadAddress(this.Address.IsRelive)) == 1;
			}
		}

		// Token: 0x170002D2 RID: 722
		// (get) Token: 0x06000C3C RID: 3132 RVA: 0x0004FE27 File Offset: 0x0004E027
		public int Gold
		{
			get
			{
				if (this.Address.GameType == 1)
				{
					this.Address.PlayerGold = 10044;
				}
				return this.Memory.Read(this.Base + this.Address.PlayerGold);
			}
		}

		// Token: 0x170002D3 RID: 723
		// (get) Token: 0x06000C3D RID: 3133 RVA: 0x0004FE64 File Offset: 0x0004E064
		public int MaxODaoCu
		{
			get
			{
				int num = this.Memory.Read(this.Address.ODaoCu);
				if (num == 10141355)
				{
					num = 30;
				}
				else if (num >= 10141020)
				{
					num = num - 10141020 + 20;
				}
				if (num == 0)
				{
					num = 20;
				}
				return num;
			}
		}

		// Token: 0x170002D4 RID: 724
		// (get) Token: 0x06000C3E RID: 3134 RVA: 0x0004FEAF File Offset: 0x0004E0AF
		// (set) Token: 0x06000C3F RID: 3135 RVA: 0x0004FEB7 File Offset: 0x0004E0B7
		public string Img { get; set; }

		// Token: 0x170002D5 RID: 725
		// (get) Token: 0x06000C40 RID: 3136 RVA: 0x0004FEC0 File Offset: 0x0004E0C0
		// (set) Token: 0x06000C41 RID: 3137 RVA: 0x0004FEC8 File Offset: 0x0004E0C8
		private int BaseImg { get; set; }

		// Token: 0x170002D6 RID: 726
		// (get) Token: 0x06000C42 RID: 3138 RVA: 0x0004FED1 File Offset: 0x0004E0D1
		// (set) Token: 0x06000C43 RID: 3139 RVA: 0x0004FED9 File Offset: 0x0004E0D9
		private int BaseAnswer { get; set; }

		// Token: 0x170002D7 RID: 727
		// (get) Token: 0x06000C44 RID: 3140 RVA: 0x0004FEE2 File Offset: 0x0004E0E2
		public string[] Answer
		{
			get
			{
				return this.answer;
			}
		}

		// Token: 0x170002D8 RID: 728
		// (get) Token: 0x06000C45 RID: 3141 RVA: 0x0004FEEA File Offset: 0x0004E0EA
		// (set) Token: 0x06000C46 RID: 3142 RVA: 0x0004FEF2 File Offset: 0x0004E0F2
		public string ImgHash { get; set; }

		// Token: 0x170002D9 RID: 729
		// (get) Token: 0x06000C47 RID: 3143 RVA: 0x0004FEFB File Offset: 0x0004E0FB
		// (set) Token: 0x06000C48 RID: 3144 RVA: 0x0004FF03 File Offset: 0x0004E103
		public string bin { get; set; }

		// Token: 0x170002DA RID: 730
		// (get) Token: 0x06000C49 RID: 3145 RVA: 0x0004FF0C File Offset: 0x0004E10C
		// (set) Token: 0x06000C4A RID: 3146 RVA: 0x0004FF14 File Offset: 0x0004E114
		public string BinEx { get; set; }

		// Token: 0x06000C4B RID: 3147 RVA: 0x0004FF20 File Offset: 0x0004E120
		public void ReadCaptcha()
		{
			try
			{
				if (this.IsReadCaptcha && (this.BaseAnswer == 0 || this.BaseImg == 0))
				{
					return;
				}
				this.bin = "";
				this.BinEx = "";
				if (this.BaseAnswer == 0)
				{
					this.BaseAnswer = (int)this.Game.AOB.AobScan(new byte[]
					{
						65,
						110,
						116,
						105,
						82,
						111,
						98,
						111,
						116,
						95,
						70,
						114,
						97,
						109,
						101,
						0,
						15,
						0,
						0,
						0,
						15,
						0,
						0,
						0,
						0,
						0,
						0,
						0,
						84,
						76,
						66,
						66,
						95,
						77,
						97,
						105,
						110,
						70,
						114,
						97,
						109,
						101,
						48
					}, 1073741824) + 6240;
				}
				this.answer = new string[4];
				int num = this.Memory.ScanString("TLBB_ButtonNULL", this.BaseAnswer, this.BaseAnswer + 16777215, 0) - 444;
				this.answer[0] = ConverterEx.Unicodes[this.Memory.Read(num)].ToString() + ConverterEx.Unicodes[this.Memory.Read(num + 4)].ToString() + ConverterEx.Unicodes[this.Memory.Read(num + 8)].ToString() + ConverterEx.Unicodes[this.Memory.Read(num + 12)].ToString();
				int num2 = this.Memory.ScanString("TLBB_ButtonNULL", num + 511, num + 16777215, 0) - 444;
				this.answer[1] = ConverterEx.Unicodes[this.Memory.Read(num2)].ToString() + ConverterEx.Unicodes[this.Memory.Read(num2 + 4)].ToString() + ConverterEx.Unicodes[this.Memory.Read(num2 + 8)].ToString() + ConverterEx.Unicodes[this.Memory.Read(num2 + 12)].ToString();
				int num3 = this.Memory.ScanString("TLBB_ButtonNULL", num2 + 511, num2 + 16777215, 0) - 444;
				this.answer[2] = ConverterEx.Unicodes[this.Memory.Read(num3)].ToString() + ConverterEx.Unicodes[this.Memory.Read(num3 + 4)].ToString() + ConverterEx.Unicodes[this.Memory.Read(num3 + 8)].ToString() + ConverterEx.Unicodes[this.Memory.Read(num3 + 12)].ToString();
				int num4 = this.Memory.ScanString("TLBB_ButtonNULL", num3 + 511, num3 + 16777215, 0) - 444;
				this.answer[3] = ConverterEx.Unicodes[this.Memory.Read(num4)].ToString() + ConverterEx.Unicodes[this.Memory.Read(num4 + 4)].ToString() + ConverterEx.Unicodes[this.Memory.Read(num4 + 8)].ToString() + ConverterEx.Unicodes[this.Memory.Read(num4 + 12)].ToString();
				try
				{
					this.Img = string.Empty;
					if (this.BaseImg == 0)
					{
						this.BaseImg = (int)this.Game.AOB.AobScan(new byte[]
						{
							1,
							0,
							0,
							0,
							byte.MaxValue,
							160,
							byte.MaxValue,
							160,
							byte.MaxValue,
							160,
							byte.MaxValue,
							160,
							byte.MaxValue,
							160,
							byte.MaxValue,
							160
						}, 0U, 536870911U) + 4;
					}
					int num5 = this.Memory.Read2Byte(this.BaseImg);
					if (num5 == 40960 || num5 == 41215)
					{
						byte[] array = new byte[9218];
						Memory.ReadProcessMemory(this.Memory.Id, this.BaseImg, array, array.Length, 0);
						for (int i = 0; i < 9216; i += 2)
						{
							byte[] array2 = new byte[4];
							array2[0] = array[i];
							array2[1] = array[i + 1];
							num5 = BitConverter.ToInt32(array2, 0);
							if (num5 == 40960)
							{
								this.bin += "0";
								this.BinEx += "0";
							}
							else
							{
								this.bin += "1";
								this.BinEx += "1";
							}
							if (this.bin.Length == 8)
							{
								this.Img += Convert.ToInt32(this.bin, 2).ToString("X2");
								this.bin = "";
							}
						}
					}
					this.Img = string.Concat(new string[]
					{
						this.Img,
						this.answer[0],
						this.answer[1],
						this.answer[2],
						this.answer[3]
					});
					this.ImgHash = TINHKIEM.Hasher.MD5(this.Img);
				}
				catch
				{
				}
			}
			catch
			{
			}
			this.IsReadCaptcha = true;
		}

		// Token: 0x170002DB RID: 731
		// (get) Token: 0x06000C4C RID: 3148 RVA: 0x00050460 File Offset: 0x0004E660
		// (set) Token: 0x06000C4D RID: 3149 RVA: 0x0005053C File Offset: 0x0004E73C
		public Bitmap Captcha
		{
			get
			{
				if (this.captcha == null && this.BinEx != null && this.BinEx != "")
				{
					this.captcha = new Bitmap(128, 36);
					int num = 0;
					string binEx = this.BinEx;
					for (int i = 0; i < binEx.Length; i++)
					{
						if (binEx[i] == '0')
						{
							this.captcha.SetPixel(num / 2 % 128, num / 2 / 128, Color.Black);
						}
						else
						{
							this.captcha.SetPixel(num / 2 % 128, num / 2 / 128, Color.White);
						}
						num += 2;
					}
					this.captcha = new Bitmap(this.captcha, new Size(160, 45));
				}
				return this.captcha;
			}
			set
			{
				this.captcha = value;
			}
		}

		// Token: 0x170002DC RID: 732
		// (get) Token: 0x06000C4E RID: 3150 RVA: 0x00050548 File Offset: 0x0004E748
		public string RaoTxt
		{
			get
			{
				int num = this.Memory.GetModuleAddress("UI_CEGUI.dll");
				if (this.Address.GameType == 1)
				{
					num += 400392;
				}
				else
				{
					num += 226052;
				}
				return this.Memory.ReadString(this.Memory.Read(num));
			}
		}

		// Token: 0x170002DD RID: 733
		// (get) Token: 0x06000C4F RID: 3151 RVA: 0x0005059D File Offset: 0x0004E79D
		public bool IsFresh
		{
			get
			{
				return this.Memory.Read(this.Address.CountDown10Sec) == 1;
			}
		}

		// Token: 0x170002DE RID: 734
		// (get) Token: 0x06000C50 RID: 3152 RVA: 0x000505B8 File Offset: 0x0004E7B8
		public bool IsShopOpen
		{
			get
			{
				return this.Memory.Read(this.Address.IsShopOpen) == 1;
			}
		}

		// Token: 0x170002DF RID: 735
		// (get) Token: 0x06000C51 RID: 3153 RVA: 0x000505D4 File Offset: 0x0004E7D4
		public int MaxONguyenLieu
		{
			get
			{
				int num = this.Memory.Read(this.Address.ONguyenLieu);
				if (num == 10141356)
				{
					num = 30;
				}
				else if (num >= 10141030)
				{
					num = num - 10141030 + 20;
				}
				if (num == 0)
				{
					num = 20;
				}
				return num;
			}
		}

		// Token: 0x170002E0 RID: 736
		// (get) Token: 0x06000C52 RID: 3154 RVA: 0x00050620 File Offset: 0x0004E820
		public bool IsODaoCuFull
		{
			get
			{
				int maxODaoCu = this.MaxODaoCu;
				int num = this.Memory.Read(this.Address.PacketItemBase);
				for (int i = 0; i < maxODaoCu; i++)
				{
					if (this.Memory.Read(num + i * 4) == 0)
					{
						return false;
					}
				}
				return true;
			}
		}

		// Token: 0x170002E1 RID: 737
		// (get) Token: 0x06000C53 RID: 3155 RVA: 0x0005066C File Offset: 0x0004E86C
		public bool IsONguyenLieuFull
		{
			get
			{
				int maxONguyenLieu = this.MaxONguyenLieu;
				int num = this.Memory.Read(this.Address.PacketItemBase);
				for (int i = 30; i < 30 + maxONguyenLieu; i++)
				{
					if (this.Memory.Read(num + i * 4) == 0)
					{
						return false;
					}
				}
				return true;
			}
		}

		// Token: 0x170002E2 RID: 738
		// (get) Token: 0x06000C54 RID: 3156 RVA: 0x000506BC File Offset: 0x0004E8BC
		// (set) Token: 0x06000C55 RID: 3157 RVA: 0x000506C4 File Offset: 0x0004E8C4
		public int PlayerState { get; set; }

		// Token: 0x170002E3 RID: 739
		// (get) Token: 0x06000C56 RID: 3158 RVA: 0x000506CD File Offset: 0x0004E8CD
		// (set) Token: 0x06000C57 RID: 3159 RVA: 0x000506D5 File Offset: 0x0004E8D5
		public int MapId { get; set; }

		// Token: 0x170002E4 RID: 740
		// (get) Token: 0x06000C58 RID: 3160 RVA: 0x000506DE File Offset: 0x0004E8DE
		// (set) Token: 0x06000C59 RID: 3161 RVA: 0x000506E6 File Offset: 0x0004E8E6
		public int OnlineTimeSec { get; set; }

		// Token: 0x170002E5 RID: 741
		// (get) Token: 0x06000C5A RID: 3162 RVA: 0x000506EF File Offset: 0x0004E8EF
		public int MenpaiPoint
		{
			get
			{
				return this.Memory.Read(this.Base + this.Address.CharMenpaiPoint);
			}
		}

		// Token: 0x170002E6 RID: 742
		// (get) Token: 0x06000C5B RID: 3163 RVA: 0x0005070E File Offset: 0x0004E90E
		public int SafeTime
		{
			get
			{
				return this.Memory.Read(this.Address.SafeTime);
			}
		}

		// Token: 0x170002E7 RID: 743
		// (get) Token: 0x06000C5C RID: 3164 RVA: 0x00050726 File Offset: 0x0004E926
		public bool IsNexLogin
		{
			get
			{
				return this.Memory.Read(this.Address.IsNexLogin) == 1;
			}
		}

		// Token: 0x170002E8 RID: 744
		// (get) Token: 0x06000C5D RID: 3165 RVA: 0x00050741 File Offset: 0x0004E941
		public bool IsSelectServer
		{
			get
			{
				return this.Memory.Read(this.Address.IsSelectServer) == 1;
			}
		}

		// Token: 0x170002E9 RID: 745
		// (get) Token: 0x06000C5E RID: 3166 RVA: 0x0005075C File Offset: 0x0004E95C
		public bool IsTextCaptcha
		{
			get
			{
				return this.Memory.Read(this.Address.IsTextCaptcha) == 1;
			}
		}

		// Token: 0x170002EA RID: 746
		// (get) Token: 0x06000C5F RID: 3167 RVA: 0x00050777 File Offset: 0x0004E977
		public bool IsLogon
		{
			get
			{
				return this.Memory.Read(this.Address.IsLogon) == 1;
			}
		}

		// Token: 0x170002EB RID: 747
		// (get) Token: 0x06000C60 RID: 3168 RVA: 0x00050792 File Offset: 0x0004E992
		public bool IsSelectServerQuest
		{
			get
			{
				return this.Memory.Read(this.Address.IsLoginMessage) == 1;
			}
		}

		// Token: 0x170002EC RID: 748
		// (get) Token: 0x06000C61 RID: 3169 RVA: 0x000507AD File Offset: 0x0004E9AD
		public bool IsSelectCharacter
		{
			get
			{
				return this.Memory.Read(this.Address.IsSelectCharacter) == 1;
			}
		}

		// Token: 0x170002ED RID: 749
		// (get) Token: 0x06000C62 RID: 3170 RVA: 0x000507C8 File Offset: 0x0004E9C8
		public int FakeMapId
		{
			get
			{
				return this.Memory.Read(this.Address.FakeMapId);
			}
		}

		// Token: 0x170002EE RID: 750
		// (get) Token: 0x06000C63 RID: 3171 RVA: 0x000507E0 File Offset: 0x0004E9E0
		public int HPPercent
		{
			get
			{
				return TLBB.Percent(this.HP, this.MaxHP);
			}
		}

		// Token: 0x170002EF RID: 751
		// (get) Token: 0x06000C64 RID: 3172 RVA: 0x000507F3 File Offset: 0x0004E9F3
		public int MPPercent
		{
			get
			{
				return TLBB.Percent(this.MP, this.MaxMP);
			}
		}

		// Token: 0x170002F0 RID: 752
		// (get) Token: 0x06000C65 RID: 3173 RVA: 0x00050806 File Offset: 0x0004EA06
		public int PetHPPercent
		{
			get
			{
				return TLBB.Percent(this.PetHP, this.PetMaxHP);
			}
		}

		// Token: 0x170002F1 RID: 753
		// (get) Token: 0x06000C66 RID: 3174 RVA: 0x00050819 File Offset: 0x0004EA19
		public bool Busy
		{
			get
			{
				return this.PlayerState > 2 && this.PlayerState < 10;
			}
		}

		// Token: 0x170002F2 RID: 754
		// (get) Token: 0x06000C67 RID: 3175 RVA: 0x00050830 File Offset: 0x0004EA30
		public bool BusyEx
		{
			get
			{
				return this.PlayerState > 0 && this.PlayerState != 2 && this.PlayerState != 7;
			}
		}

		// Token: 0x06000C68 RID: 3176 RVA: 0x00050854 File Offset: 0x0004EA54
		private static int Percent(int min, int max)
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

		// Token: 0x170002F3 RID: 755
		// (get) Token: 0x06000C69 RID: 3177 RVA: 0x0005087F File Offset: 0x0004EA7F
		public int MaxExp
		{
			get
			{
				if (this.Lvl > 0 && this.Lvl < 150)
				{
					return MAXEXP.Lvl[this.Lvl];
				}
				return MAXEXP.Lvl[1];
			}
		}

		// Token: 0x170002F4 RID: 756
		// (get) Token: 0x06000C6A RID: 3178 RVA: 0x000508AB File Offset: 0x0004EAAB
		public float ExpPercent
		{
			get
			{
				return (float)this.Exp * 100f / (float)this.MaxExp;
			}
		}

		// Token: 0x170002F5 RID: 757
		// (get) Token: 0x06000C6B RID: 3179 RVA: 0x000508C2 File Offset: 0x0004EAC2
		public bool IsNoi
		{
			get
			{
				return this.Menpai >= 4 && this.Menpai != 8 && this.Menpai != 32;
			}
		}

		// Token: 0x170002F6 RID: 758
		// (get) Token: 0x06000C6C RID: 3180 RVA: 0x000508E8 File Offset: 0x0004EAE8
		public bool IsRide
		{
			get
			{
				this.Base = this.Memory.Read(this.Address.CharBase);
				return this.Memory.Read(this.Base + this.Address.ObjectRide) != -1;
			}
		}

		// Token: 0x170002F7 RID: 759
		// (get) Token: 0x06000C6D RID: 3181 RVA: 0x00050934 File Offset: 0x0004EB34
		public bool IsLeader
		{
			get
			{
				return this.Online && this.Id.Contains(this.KeyId);
			}
		}

		// Token: 0x170002F8 RID: 760
		// (get) Token: 0x06000C6E RID: 3182 RVA: 0x00050951 File Offset: 0x0004EB51
		// (set) Token: 0x06000C6F RID: 3183 RVA: 0x00050959 File Offset: 0x0004EB59
		public bool IsOnline { get; set; }

		// Token: 0x170002F9 RID: 761
		// (get) Token: 0x06000C70 RID: 3184 RVA: 0x00050962 File Offset: 0x0004EB62
		public int CEGUIBaseAddress
		{
			get
			{
				if (this.cEGUIBaseAddress == 0)
				{
					this.cEGUIBaseAddress = this.Memory.GetModuleAddress("CEGUIBase.dll");
				}
				return this.cEGUIBaseAddress;
			}
		}

		// Token: 0x170002FA RID: 762
		// (get) Token: 0x06000C71 RID: 3185 RVA: 0x00050988 File Offset: 0x0004EB88
		public int FreshmanWatchTime
		{
			get
			{
				return this.Memory.Read(new int[]
				{
					this.CEGUIBaseAddress + this.Address.FreshmanWatchTime[0],
					this.Address.FreshmanWatchTime[1],
					this.Address.FreshmanWatchTime[2],
					this.Address.FreshmanWatchTime[3],
					this.Address.FreshmanWatchTime[4],
					this.Address.FreshmanWatchTime[5]
				});
			}
		}

		// Token: 0x170002FB RID: 763
		// (get) Token: 0x06000C72 RID: 3186 RVA: 0x00050A10 File Offset: 0x0004EC10
		public bool Online
		{
			get
			{
				if (this.Id.Contains("00000000") || this.Id.Contains("FFFFFFFF") || this.Name == "ĐăngNhập" || this.Lvl < 1)
				{
					return false;
				}
				this.IsOnline = true;
				return true;
			}
		}

		// Token: 0x170002FC RID: 764
		// (get) Token: 0x06000C73 RID: 3187 RVA: 0x00050A68 File Offset: 0x0004EC68
		public int PetCount
		{
			get
			{
				int num = 0;
				int num2 = this.Memory.Read(this.Address.PetBase);
				int num3 = 0;
				while (num3 < 20 && this.Memory.Read(num2 + this.Address.PetDataSize * num3 + this.Address.PetId) != 0 && this.Memory.Read(num2 + this.Address.PetDataSize * num3 + this.Address.PetMaxHP) > 0)
				{
					num++;
					num3++;
				}
				return num;
			}
		}

		// Token: 0x170002FD RID: 765
		// (get) Token: 0x06000C74 RID: 3188 RVA: 0x00050AF4 File Offset: 0x0004ECF4
		public bool IsBienThan
		{
			get
			{
				if (this.Address.GameType == 1)
				{
					return this.Memory.Read(this.Base + 156) != -1;
				}
				return this.Memory.Read(this.Address.LuyenKimBase, this.Address.BienThan) == 1;
			}
		}

		// Token: 0x170002FE RID: 766
		// (get) Token: 0x06000C75 RID: 3189 RVA: 0x00050B51 File Offset: 0x0004ED51
		public bool IsTogleMission
		{
			get
			{
				return this.Memory.Read(this.Address.IsTogleMission) == 1;
			}
		}

		// Token: 0x170002FF RID: 767
		// (get) Token: 0x06000C76 RID: 3190 RVA: 0x00050B6C File Offset: 0x0004ED6C
		public bool IsToggleYuanbaoShop
		{
			get
			{
				return this.Memory.Read(new int[]
				{
					6563448,
					0,
					12,
					100
				}) == 1;
			}
		}

		// Token: 0x17000300 RID: 768
		// (get) Token: 0x06000C77 RID: 3191 RVA: 0x00050B8D File Offset: 0x0004ED8D
		public bool IsQuestOpen
		{
			get
			{
				return this.Memory.Read(this.Address.QuestInfo) == 1;
			}
		}

		// Token: 0x17000301 RID: 769
		// (get) Token: 0x06000C78 RID: 3192 RVA: 0x00050BA8 File Offset: 0x0004EDA8
		public bool HaveRide
		{
			get
			{
				bool flag = this.Memory.Read(this.Address.HaveRide1) != 0;
				if (this.Address.GameType == 1)
				{
					int num = 0;
					while (!flag)
					{
						this.Address.HaveRide2[2] = num;
						flag = (this.Memory.Read(this.Address.HaveRide2) != 0);
						num += 4;
						if (num > 12)
						{
							break;
						}
					}
				}
				return flag;
			}
		}

		// Token: 0x06000C79 RID: 3193 RVA: 0x00050C18 File Offset: 0x0004EE18
		public string MemPhaiNameByLua()
		{
			switch (this.MenPhaiLuaEX)
			{
			case 0:
				if (this.Name != "ĐăngNhập")
				{
					return "Thiếu Lâm";
				}
				break;
			case 1:
				return "Minh Giáo";
			case 2:
				return "Cái Bang";
			case 3:
				return "Võ Đang";
			case 4:
				return "Nga My";
			case 5:
				return "Tinh Túc";
			case 6:
				return "Thiên Long";
			case 7:
				return "Thiên Sơn";
			case 8:
				return "Tiêu Dao";
			case 9:
				return "Tân Thủ";
			case 10:
				return "Mộ Dung";
			}
			return "Không Có";
		}

		// Token: 0x17000302 RID: 770
		// (get) Token: 0x06000C7A RID: 3194 RVA: 0x00050CB9 File Offset: 0x0004EEB9
		public string MenpaiName
		{
			get
			{
				return this.MemPhaiNameByLua();
			}
		}

		// Token: 0x17000303 RID: 771
		// (get) Token: 0x06000C7B RID: 3195 RVA: 0x00050CC4 File Offset: 0x0004EEC4
		public NPC NPCBaiSu
		{
			get
			{
				if (this.Menpai == MENPAI.ThieuLam)
				{
					return THIEULAM.HuyenTich;
				}
				if (this.Menpai == MENPAI.MinhGiao)
				{
					return MINHGIAO.LaSuTuong;
				}
				if (this.Menpai == MENPAI.CaiBang)
				{
					return CAIBANG.TranCoNhan;
				}
				if (this.Menpai == MENPAI.VoDang)
				{
					return VODANG.TruongHuyenTo;
				}
				if (this.Menpai == MENPAI.NgaMy)
				{
					return NGAMY.LyThapNhiNuong;
				}
				if (this.Menpai == MENPAI.TinhTuc)
				{
					return TINHTUC.HanTheTrung;
				}
				if (this.Menpai == MENPAI.ThienLong)
				{
					return THIENLONG.BanNhan;
				}
				if (this.Menpai == MENPAI.ThienSon)
				{
					return THIENSON.MaiKiem;
				}
				if (this.Menpai == MENPAI.TieuDao)
				{
					return TIEUDAO.ToTinhHa;
				}
				if (this.Menpai == MENPAI.MoDung)
				{
					return MODUNG.MoDungKiet;
				}
				if (this.Menpai == MENPAI.DuongMon)
				{
					return DUONGMON.DuongXichPhong;
				}
				return null;
			}
		}

		// Token: 0x17000304 RID: 772
		// (get) Token: 0x06000C7C RID: 3196 RVA: 0x00050DA4 File Offset: 0x0004EFA4
		public NPC NPCThuongKho
		{
			get
			{
				if (this.Address.GameType == 1)
				{
					if (this.MapId == DAILY.Id)
					{
						return DAILY.ThuongKho;
					}
					if (this.MapId == TOCHAU.Id)
					{
						return TOCHAU.ThuongKho;
					}
					if (this.MapId == LACDUONG.Id)
					{
						return LACDUONG.ThuongKho;
					}
					if (this.MapId == LAULAN.Id)
					{
						return LAULAN.ThuongKho;
					}
					if (this.MapId == THUCHACOTRAN.Id)
					{
						return THUCHACOTRAN.ThuongKho;
					}
					if (this.MapId == PHUNGMINHTRAN.Id)
					{
						return PHUNGMINHTRAN.ThuongKho;
					}
					return LACDUONG.ThuongKho;
				}
				else
				{
					if (this.MapId == DAILY.Id)
					{
						return DAILY.ThuongKhoTinhKiem;
					}
					if (this.MapId == TOCHAU.Id)
					{
						return TOCHAU.ThuongKhoTinhKiem;
					}
					if (this.MapId == LACDUONG.Id)
					{
						return LACDUONG.ThuongKhoTinhKiem;
					}
					if (this.MapId == LAULAN.Id)
					{
						return LAULAN.ThuongKho;
					}
					if (this.MapId == THUCHACOTRAN.Id)
					{
						return THUCHACOTRAN.ThuongKhoTinhKiem;
					}
					if (this.MapId == PHUNGMINHTRAN.Id)
					{
						return PHUNGMINHTRAN.ThuongKho;
					}
					return LACDUONG.ThuongKhoTinhKiem;
				}
			}
		}

		// Token: 0x17000305 RID: 773
		// (get) Token: 0x06000C7D RID: 3197 RVA: 0x00050EAE File Offset: 0x0004F0AE
		public bool IsBankOpen
		{
			get
			{
				return this.Memory.Read(this.Address.IsBankOpen) == 1;
			}
		}

		// Token: 0x17000306 RID: 774
		// (get) Token: 0x06000C7E RID: 3198 RVA: 0x00050ECC File Offset: 0x0004F0CC
		public NPC NPCBaiSuDaiLy
		{
			get
			{
				if (this.Menpai == MENPAI.DuongMon)
				{
					return DAILY.DuongDuc;
				}
				if (this.Menpai == MENPAI.MinhGiao)
				{
					return DAILY.ThachBao;
				}
				if (this.Menpai == MENPAI.ThienSon)
				{
					return DAILY.TrinhThanhSuong;
				}
				if (this.Menpai == MENPAI.TinhTuc)
				{
					return DAILY.HaiPhongTu;
				}
				if (this.Menpai == MENPAI.ThienLong)
				{
					return DAILY.PhaTham;
				}
				if (this.Menpai == MENPAI.TieuDao)
				{
					return DAILY.DamDaiTuVu;
				}
				if (this.Menpai == MENPAI.MoDung)
				{
					return DAILY.MoDungTruyen;
				}
				if (this.Menpai == MENPAI.NgaMy)
				{
					return DAILY.LoTamNuong;
				}
				if (this.Menpai == MENPAI.CaiBang)
				{
					return DAILY.GianNinh;
				}
				if (this.Menpai == MENPAI.ThieuLam)
				{
					return DAILY.TueDich;
				}
				if (this.Menpai == MENPAI.VoDang)
				{
					return DAILY.TruongHoach;
				}
				return null;
			}
		}

		// Token: 0x17000307 RID: 775
		// (get) Token: 0x06000C7F RID: 3199 RVA: 0x00050FAC File Offset: 0x0004F1AC
		public NPC NPCTamPhap
		{
			get
			{
				if (this.Menpai == MENPAI.ThieuLam)
				{
					return THIEULAM.HuyenNan;
				}
				if (this.Menpai == MENPAI.MinhGiao)
				{
					return MINHGIAO.BangVanXuan;
				}
				if (this.Menpai == MENPAI.CaiBang)
				{
					return CAIBANG.HeTamKi;
				}
				if (this.Menpai == MENPAI.VoDang)
				{
					return VODANG.DuVienSon;
				}
				if (this.Menpai == MENPAI.NgaMy)
				{
					return NGAMY.ThoiLucHoa;
				}
				if (this.Menpai == MENPAI.TinhTuc)
				{
					return TINHTUC.ThiToan;
				}
				if (this.Menpai == MENPAI.ThienLong)
				{
					return THIENLONG.BanQuan;
				}
				if (this.Menpai == MENPAI.ThienSon)
				{
					return THIENSON.LanKiem;
				}
				if (this.Menpai == MENPAI.TieuDao)
				{
					return TIEUDAO.KhangQuangLang;
				}
				if (this.Menpai == MENPAI.MoDung)
				{
					return MODUNG.MoDungThanhSon;
				}
				if (this.Menpai == MENPAI.DuongMon)
				{
					return DUONGMON.DuongNhacXung;
				}
				return null;
			}
		}

		// Token: 0x17000308 RID: 776
		// (get) Token: 0x06000C80 RID: 3200 RVA: 0x0005108C File Offset: 0x0004F28C
		public int MapAcBa
		{
			get
			{
				if (this.Menpai == MENPAI.ThieuLam)
				{
					return MAP.ThieuLamAcBa;
				}
				if (this.Menpai == MENPAI.MinhGiao)
				{
					return MAP.MinhGiaoAcBa;
				}
				if (this.Menpai == MENPAI.CaiBang)
				{
					return MAP.CaiBangAcBa;
				}
				if (this.Menpai == MENPAI.VoDang)
				{
					return MAP.VoDangAcBa;
				}
				if (this.Menpai == MENPAI.NgaMy)
				{
					return MAP.NgaMyAcBa;
				}
				if (this.Menpai == MENPAI.TinhTuc)
				{
					return MAP.TinhTucAcBa;
				}
				if (this.Menpai == MENPAI.ThienLong)
				{
					return MAP.ThienLongAcBa;
				}
				if (this.Menpai == MENPAI.ThienSon)
				{
					return MAP.ThienSonAcBa;
				}
				if (this.Menpai == MENPAI.TieuDao)
				{
					return MAP.TieuDaoAcBa;
				}
				if (this.Menpai == MENPAI.MoDung)
				{
					return MAP.MoDungAcBa;
				}
				if (this.Menpai == MENPAI.DuongMon)
				{
					return MAP.DuongMonAcBa;
				}
				return -1;
			}
		}

		// Token: 0x17000309 RID: 777
		// (get) Token: 0x06000C81 RID: 3201 RVA: 0x0005116C File Offset: 0x0004F36C
		public int MapMonPhai
		{
			get
			{
				if (this.Menpai == MENPAI.ThieuLam)
				{
					return THIEULAM.Id;
				}
				if (this.Menpai == MENPAI.MinhGiao)
				{
					return MINHGIAO.Id;
				}
				if (this.Menpai == MENPAI.CaiBang)
				{
					return CAIBANG.Id;
				}
				if (this.Menpai == MENPAI.VoDang)
				{
					return VODANG.Id;
				}
				if (this.Menpai == MENPAI.NgaMy)
				{
					return NGAMY.Id;
				}
				if (this.Menpai == MENPAI.TinhTuc)
				{
					return TINHTUC.Id;
				}
				if (this.Menpai == MENPAI.ThienLong)
				{
					return THIENLONG.Id;
				}
				if (this.Menpai == MENPAI.ThienSon)
				{
					return THIENSON.Id;
				}
				if (this.Menpai == MENPAI.TieuDao)
				{
					return TIEUDAO.Id;
				}
				if (this.Menpai == MENPAI.MoDung)
				{
					return MODUNG.Id;
				}
				if (this.Menpai == MENPAI.DuongMon)
				{
					return DUONGMON.Id;
				}
				return -1;
			}
		}

		// Token: 0x06000C82 RID: 3202 RVA: 0x0005124B File Offset: 0x0004F44B
		public string GetMonPhaiName()
		{
			this.Game.LuaDoOneLineString("local menpai = Player:GetData(\"MEMPAI\"); return menpai;");
			return this.Game.LuaToString();
		}

		// Token: 0x06000C83 RID: 3203 RVA: 0x00051268 File Offset: 0x0004F468
		public static bool IsVIPItem(int item)
		{
			return item != 0 && (item == 30008034 || item == 30000000 || (item >= 20109001 && item <= 20109015) || item == 20109101 || item == 20109102 || item == 30008053 || item == 30103042 || item == 10141153 || item == 10157001 || item == 10157002 || item == 10156001 || item == 10156002 || item == 10156003 || item == 10156004 || (!item.ToString().StartsWith("101") && !item.ToString().StartsWith("102") && !item.ToString().StartsWith("103") && !item.ToString().StartsWith("104") && !item.ToString().StartsWith("201") && !item.ToString().StartsWith("300") && !item.ToString().StartsWith("301")));
		}

		// Token: 0x06000C84 RID: 3204 RVA: 0x000513A9 File Offset: 0x0004F5A9
		public TLBB(Game game)
		{
			this.Game = game;
			this.Address = game.Address;
			this.Memory = game.Memory;
			this.Read();
		}

		// Token: 0x1700030A RID: 778
		// (get) Token: 0x06000C85 RID: 3205 RVA: 0x000513D6 File Offset: 0x0004F5D6
		public int X2TimeSec
		{
			get
			{
				return this.Memory.Read(this.Address.X2);
			}
		}

		// Token: 0x1700030B RID: 779
		// (get) Token: 0x06000C86 RID: 3206 RVA: 0x000513F0 File Offset: 0x0004F5F0
		public Dictionary<int, string> DicPet
		{
			get
			{
				Dictionary<int, string> dictionary = new Dictionary<int, string>();
				this.PetBase = this.Memory.Read(this.Address.PetBase);
				for (int i = 0; i < 20; i++)
				{
					int num = this.Memory.Read(this.PetBase + this.Address.PetDataSize * i + this.Address.PetId);
					string value;
					if (this.Address.GameType != 1)
					{
						value = this.Memory._ReadString(this.PetBase + this.Address.PetDataSize * i + 28);
					}
					else
					{
						value = this.Memory._ReadString(this.PetBase + this.Address.PetDataSize * i + 36);
					}
					if (num == 0)
					{
						break;
					}
					if (!dictionary.ContainsKey(num))
					{
						dictionary.Add(num, value);
					}
				}
				return dictionary;
			}
		}

		// Token: 0x06000C87 RID: 3207 RVA: 0x000514CC File Offset: 0x0004F6CC
		public void Read()
		{
			this.Base = this.Memory.Read(this.Address.CharBase);
			if (this.Address.GameType == 1)
			{
				this.Id = this.Memory.Read8Byte(this.Base + this.Address.CharId).ToString("X8");
			}
			else
			{
				this.Id = this.Memory.Read(this.Base + this.Address.CharId).ToString("X8");
			}
			this.Name = this.Memory.ReadShortString(this.Base + this.Address.CharName);
			if (this.Name == "")
			{
				this.Name = "ĐăngNhập";
			}
			this.MenPhaiLuaEX = this.Memory.Read(this.Base + 168);
			this.Lvl = this.Memory.Read(this.Base + this.Address.CharLvl);
			this.Rage = this.Memory.Read(this.Base + this.Address.CharRage);
			this.IsFollow = (this.Memory.Read(this.Base + this.Address.CharIsFollow) == 1);
			this.PetId = this.Memory.Read(this.Base + this.Address.CharCurPetId);
			this.HP = this.Memory.Read(this.Base + this.Address.CharCurHP);
			this.MaxHP = this.Memory.Read(this.Base + this.Address.CharMaxHP);
			this.MP = this.Memory.Read(this.Base + this.Address.CharCurMP);
			this.Exp = this.Memory.Read(this.Base + this.Address.CharExp);
			this.MaxMP = this.Memory.Read(this.Base + this.Address.CharMaxMP);
			this.GuildName = this.Memory.ReadString(this.Base + this.Address.CharGuildName);
			this.GuildId = this.Memory.Read(this.Base + this.Address.CharGuildID);
			if (this.PetId != 0)
			{
				this.PetBase = this.Memory.Read(this.Address.PetBase);
				int i = 0;
				while (i < 20)
				{
					if (this.Memory.Read(this.PetBase + this.Address.PetDataSize * i + this.Address.PetId) == this.PetId)
					{
						this.PetHP = this.Memory.Read(this.PetBase + this.Address.PetDataSize * i + this.Address.PetCurHP);
						this.PetMaxHP = this.Memory.Read(this.PetBase + this.Address.PetDataSize * i + this.Address.PetMaxHP);
						this.PetEnjoy = this.Memory.Read(this.PetBase + this.Address.PetDataSize * i + this.Address.PetEnjoy);
						this.PetLvl = this.Memory.Read(this.PetBase + this.Address.PetDataSize * i + this.Address.PetLvl);
						if (this.Address.GameType == 1)
						{
							this.PetName = this.Memory._ReadString(this.PetBase + this.Address.PetDataSize * i + 36);
							break;
						}
						this.PetName = this.Memory._ReadString(this.PetBase + this.Address.PetDataSize * i + 28);
						break;
					}
					else
					{
						i++;
					}
				}
			}
			else
			{
				this.PetHP = (this.PetMaxHP = (this.PetEnjoy = 0));
			}
			bool isCaptcha = this.IsCaptcha;
			this.IsCaptcha = (this.Memory.Read(this.Address.IsCaptcha) == 1);
			if (this.IsCaptcha && (this.BinEx == null || this.BinEx == ""))
			{
				if (this.Game.swCaptchaTime == null)
				{
					this.Game.swCaptchaTime = Stopwatch.StartNew();
				}
				if (this.Game.swCaptchaTime.Elapsed.TotalSeconds >= 2.0 && !this.IsRead)
				{
					this.IsRead = true;
				}
			}
			if (!this.IsCaptcha)
			{
				this.IsRead = false;
			}
			this.IsPk = (this.Memory.Read(this.Address.IsPK) == 1);
			this.Disconnected = (this.Memory.Read(this.Address.Disconnected) == 1);
			if (this.Address.GameType == 1)
			{
				this.KeyId = this.Memory.Read8Byte(this.Address.KeyId).ToString("X8");
			}
			else
			{
				this.KeyId = this.Memory.Read(this.Address.KeyId).ToString("X8");
			}
			this.MapName = this.Memory._ReadString(this.Address.MapName);
			if (this.Address.GameType == 1)
			{
				this.Address.SkillPetBase[1] = 104;
			}
			this.SkillPetType = this.Memory._ReadString(this.Memory.Read(this.Address.SkillPetBase), 40) + this.Memory._ReadString(this.Memory.Read(this.Address.SkillPetBase) + 4, 40);
			this.OnlineTime = this.Memory.Read(this.Address.OnlineTime) / 60000;
			this.OnlineTimeSec = this.Memory.Read(this.Address.OnlineTime) / 1000;
			this.DelayBase = this.Memory.Read(this.Address.SkillDelayBase);
			this.MapId = this.Memory.Read(this.Address.MapId);
		}

		// Token: 0x1700030C RID: 780
		// (get) Token: 0x06000C88 RID: 3208 RVA: 0x00051B45 File Offset: 0x0004FD45
		// (set) Token: 0x06000C89 RID: 3209 RVA: 0x00051B4D File Offset: 0x0004FD4D
		public bool IsRead { get; set; }

		// Token: 0x1700030D RID: 781
		// (get) Token: 0x06000C8A RID: 3210 RVA: 0x00051B56 File Offset: 0x0004FD56
		// (set) Token: 0x06000C8B RID: 3211 RVA: 0x00051B5E File Offset: 0x0004FD5E
		public string PetName { get; set; }

		// Token: 0x1700030E RID: 782
		// (get) Token: 0x06000C8C RID: 3212 RVA: 0x00051B67 File Offset: 0x0004FD67
		public bool Valid
		{
			get
			{
				return this.IsNexLogin || this.IsSelectServer || this.IsLogon || this.IsSelectCharacter || this.IsOnline;
			}
		}

		// Token: 0x06000C8D RID: 3213 RVA: 0x00051B94 File Offset: 0x0004FD94
		public override string ToString()
		{
			string text = string.Empty;
			text = text + "InfoAddress: " + this.Base.ToString("X8");
			text += "\r\n";
			text += this.SkillPetType;
			text += "\r\n";
			text += this.Game.TLBB.Menpai.ToString();
			text += "\r\n";
			text = text + "Menpai Point: " + this.Game.TLBB.MenpaiPoint.ToString();
			text += "\r\n";
			text += this.Game.ListHoaTruongThanh.Count.ToString();
			text += "\r\n";
			text = string.Concat(new object[]
			{
				text,
				this.Game.TrongHoaThuHoachX,
				",",
				this.Game.TrongHoaThuHoachY
			});
			text += "\r\n";
			text = text + "hoanhy: " + this.PetEnjoy.ToString();
			text += "\r\n";
			text = text + "isquest: " + this.IsQuestOpen.ToString();
			text += "\r\n";
			text = text + "handle: " + this.Game.Handle.ToString("X8");
			text += "\r\n";
			text = text + "recvdata: " + this.Game.RecvData.ToString("X8");
			text += "\r\n";
			if (this.Game.ScriptCoBan != null)
			{
				text = text + "Script: " + this.Game.ScriptCoBan.Name;
			}
			text += "\r\n";
			text = text + this.MaxODaoCu.ToString() + ".";
			text = text + "\r\n" + this.BaseImg.ToString("X8");
			text = text + "\r\n" + this.Game.ExpStart.ToString();
			return text + "\r\nmap" + this.MapId.ToString();
		}

		// Token: 0x1700030F RID: 783
		// (get) Token: 0x06000C8E RID: 3214 RVA: 0x00051E04 File Offset: 0x00050004
		public int Menpai
		{
			get
			{
				int result = -1;
				switch (this.MenPhaiLuaEX)
				{
				case 0:
					result = 1;
					break;
				case 1:
					result = 2;
					break;
				case 2:
					result = 3;
					break;
				case 3:
					result = 4;
					break;
				case 4:
					result = 5;
					break;
				case 5:
					result = 6;
					break;
				case 6:
					result = 7;
					break;
				case 7:
					result = 8;
					break;
				case 8:
					result = 9;
					break;
				case 9:
					result = -1;
					break;
				case 10:
					result = 32;
					break;
				}
				return result;
			}
		}

		// Token: 0x0400093E RID: 2366
		public Game Game;

		// Token: 0x0400093F RID: 2367
		public Memory Memory;

		// Token: 0x04000940 RID: 2368
		public Address Address;

		// Token: 0x04000941 RID: 2369
		public int Base;

		// Token: 0x04000942 RID: 2370
		public string Id;

		// Token: 0x04000943 RID: 2371
		public string Name;

		// Token: 0x04000944 RID: 2372
		public int MenPhaiLuaEX;

		// Token: 0x04000945 RID: 2373
		public int Lvl;

		// Token: 0x04000946 RID: 2374
		public int Rage;

		// Token: 0x04000947 RID: 2375
		public bool IsFollow;

		// Token: 0x04000948 RID: 2376
		public int PetId;

		// Token: 0x04000949 RID: 2377
		public int HP;

		// Token: 0x0400094A RID: 2378
		public int MP;

		// Token: 0x0400094B RID: 2379
		public int Exp;

		// Token: 0x0400094C RID: 2380
		public int MaxHP;

		// Token: 0x0400094D RID: 2381
		public int MaxMP;

		// Token: 0x0400094E RID: 2382
		public int PetBase;

		// Token: 0x0400094F RID: 2383
		public int PetHP;

		// Token: 0x04000950 RID: 2384
		public int PetMaxHP;

		// Token: 0x04000951 RID: 2385
		public int PetEnjoy;

		// Token: 0x04000952 RID: 2386
		public int PetLvl;

		// Token: 0x04000953 RID: 2387
		public string[] answer;

		// Token: 0x04000954 RID: 2388
		private Bitmap captcha;

		// Token: 0x04000955 RID: 2389
		public bool IsCaptcha;

		// Token: 0x04000956 RID: 2390
		public bool IsPk;

		// Token: 0x04000957 RID: 2391
		public bool Disconnected;

		// Token: 0x04000958 RID: 2392
		public string KeyId;

		// Token: 0x04000959 RID: 2393
		public string MapName;

		// Token: 0x0400095A RID: 2394
		public string SkillPetType;

		// Token: 0x0400095B RID: 2395
		public int OnlineTime;

		// Token: 0x0400095C RID: 2396
		public int DelayBase;

		// Token: 0x0400095D RID: 2397
		public string GuildName;

		// Token: 0x0400095E RID: 2398
		public int GuildId;

		// Token: 0x0400095F RID: 2399
		private int cEGUIBaseAddress;

		// Token: 0x04000960 RID: 2400
		public bool IsReadCaptcha;

		// Token: 0x04000961 RID: 2401
		public static float RunSpeed = 3.5f;
	}
}
