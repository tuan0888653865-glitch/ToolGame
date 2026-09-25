using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace TinhKiemAuto
{
	// Token: 0x020000C5 RID: 197
	public class PacketItem
	{
		// Token: 0x06000AA3 RID: 2723 RVA: 0x00044CB5 File Offset: 0x00042EB5
		public PacketItem(Game game)
		{
			this.game = game;
		}

		// Token: 0x1700025D RID: 605
		// (get) Token: 0x06000AA4 RID: 2724 RVA: 0x00044CCB File Offset: 0x00042ECB
		public string ClearName
		{
			get
			{
				return TINHKIEM.VietLien(this.Name);
			}
		}

		// Token: 0x1700025E RID: 606
		// (get) Token: 0x06000AA5 RID: 2725 RVA: 0x00044CD8 File Offset: 0x00042ED8
		public int DiemAddress
		{
			get
			{
				return this.game.Memory.Read(this.Address + 20) + 144;
			}
		}

		// Token: 0x1700025F RID: 607
		// (get) Token: 0x06000AA6 RID: 2726 RVA: 0x00044CF9 File Offset: 0x00042EF9
		public bool HaveTheLuc
		{
			get
			{
				return this.DiemType[51] == '1';
			}
		}

		// Token: 0x06000AA7 RID: 2727 RVA: 0x00044D0C File Offset: 0x00042F0C
		public int ReadNgocID1()
		{
			return this.game.Memory.Read(this.game.Memory.Read(this.Address + 20) + 120);
		}

		// Token: 0x06000AA8 RID: 2728 RVA: 0x00044D3A File Offset: 0x00042F3A
		public int ReadNgocID2()
		{
			return this.game.Memory.Read(this.game.Memory.Read(this.Address + 20) + 124);
		}

		// Token: 0x06000AA9 RID: 2729 RVA: 0x00044D68 File Offset: 0x00042F68
		public int ReadNgocID3()
		{
			return this.game.Memory.Read(this.game.Memory.Read(this.Address + 20) + 128);
		}

		// Token: 0x06000AAA RID: 2730 RVA: 0x00044D99 File Offset: 0x00042F99
		public int ReadNgocID4()
		{
			return this.game.Memory.Read(this.game.Memory.Read(this.Address + 20) + 132);
		}

		// Token: 0x17000260 RID: 608
		// (get) Token: 0x06000AAB RID: 2731 RVA: 0x00044DCC File Offset: 0x00042FCC
		public string DiemType
		{
			get
			{
				string text = "";
				for (int i = 0; i < 2; i++)
				{
					int value;
					if (this.game.Address.GameType == 1)
					{
						value = this.game.Memory.Read(this.game.Memory.Read(this.Address + 20) + 84 + i * 4);
					}
					else
					{
						value = this.game.Memory.Read(this.game.Memory.Read(this.Address + 20) + 112 + i * 4);
					}
					string text2 = Convert.ToString(value, 2);
					while (text2.Length < 32)
					{
						text2 = "0" + text2;
					}
					text += text2;
				}
				return text;
			}
		}

		// Token: 0x17000261 RID: 609
		// (get) Token: 0x06000AAC RID: 2732 RVA: 0x00044E94 File Offset: 0x00043094
		public bool IsHaveLongVan
		{
			get
			{
				string text = this.game.Memory.ReadString(this.game.Memory.Read(this.Address + 20) + 16);
				return text.Replace(this.game.TLBB.Name, "").Contains("*") || text.Replace(this.game.TLBB.Name, "").Contains("~");
			}
		}

		// Token: 0x17000262 RID: 610
		// (get) Token: 0x06000AAD RID: 2733 RVA: 0x00044F20 File Offset: 0x00043120
		public int TheLuc
		{
			get
			{
				int num = 0;
				for (int i = 0; i < 32; i++)
				{
					if (this.DiemType[i] == '1')
					{
						num++;
					}
				}
				for (int j = 52; j < 64; j++)
				{
					if (this.DiemType[j] == '1')
					{
						num++;
					}
				}
				if (!this.HaveTheLuc)
				{
					return 0;
				}
				if (this.game.Address.GameType == 1)
				{
					return this.game.Memory.Read2Byte(this.game.Memory.Read(this.Address + 20) + (110 + num * 2));
				}
				return this.game.Memory.Read2Byte(this.game.Memory.Read(this.Address + 20) + (138 + num * 2));
			}
		}

		// Token: 0x17000263 RID: 611
		// (get) Token: 0x06000AAE RID: 2734 RVA: 0x00044FF4 File Offset: 0x000431F4
		public int Diem
		{
			get
			{
				int num = 0;
				for (int i = 0; i < 4; i++)
				{
					num = this.game.Memory.Read2Byte(this.game.Memory.Read(this.Address + 20) + (144 + i * 2));
					if (num > 0 && num < 200)
					{
						return num;
					}
				}
				return num;
			}
		}

		// Token: 0x17000264 RID: 612
		// (get) Token: 0x06000AAF RID: 2735 RVA: 0x00045054 File Offset: 0x00043254
		public int Diem1
		{
			get
			{
				int num = this.game.Memory.Read2Byte(this.game.Memory.Read(this.Address + 20), 144);
				if (num >= 120)
				{
					return 0;
				}
				return num;
			}
		}

		// Token: 0x17000265 RID: 613
		// (get) Token: 0x06000AB0 RID: 2736 RVA: 0x00045098 File Offset: 0x00043298
		public int Diem2
		{
			get
			{
				int num = this.game.Memory.Read2Byte(this.game.Memory.Read(this.Address + 20), 146);
				if (num >= 120)
				{
					return 0;
				}
				return num;
			}
		}

		// Token: 0x17000266 RID: 614
		// (get) Token: 0x06000AB1 RID: 2737 RVA: 0x000450DC File Offset: 0x000432DC
		public int Diem3
		{
			get
			{
				int num = this.game.Memory.Read2Byte(this.game.Memory.Read(this.Address + 20), 148);
				if (num >= 120)
				{
					return 0;
				}
				return num;
			}
		}

		// Token: 0x17000267 RID: 615
		// (get) Token: 0x06000AB2 RID: 2738 RVA: 0x00045120 File Offset: 0x00043320
		public bool IsHaveNgoc
		{
			get
			{
				bool result = false;
				if ((this.ReadNgocID1() >= 50101001 && this.ReadNgocID1() <= 50921409) || (this.ReadNgocID2() >= 50101001 && this.ReadNgocID2() <= 50921409) || (this.ReadNgocID3() >= 50101001 && this.ReadNgocID3() <= 50921409) || (this.ReadNgocID4() >= 50101001 && this.ReadNgocID4() <= 50921409))
				{
					result = true;
				}
				return result;
			}
		}

		// Token: 0x17000268 RID: 616
		// (get) Token: 0x06000AB3 RID: 2739 RVA: 0x0004519C File Offset: 0x0004339C
		public string Info
		{
			get
			{
				string text = string.Empty;
				text = text + "Address: " + this.Address.ToString("X8");
				text += "\r\n";
				text = text + "Class: " + this.Class.ToString("X8");
				text += "\r\n";
				text = text + "Num: " + this.Count.ToString();
				text += "\r\n";
				text = text + "Index: " + this.Index.ToString();
				text += "\r\n";
				text = text + "TypeName: " + this.TypeName;
				text += "\r\n";
				text = text + "Star: " + this.Star.ToString();
				text += "\r\n";
				text = text + "star address: " + this.game.Memory.ReadAddress(this.Address + 20, 78).ToString();
				text += "\r\n";
				text = text + "Line: " + this.Line.ToString();
				text += "\r\n";
				text = text + "Type: " + this.Type;
				text += "\r\n";
				text = text + "MapId: " + this.MapId.ToString();
				text += "\r\n";
				text = string.Concat(new object[]
				{
					text,
					"XY: ",
					this.X,
					",",
					this.Y
				});
				text += "\r\n";
				text = text + "InfoAddress: " + this.game.Memory.Read(this.Address + 20).ToString("X8");
				text += "\r\n";
				text = text + "DieuVanAddress: " + (this.game.Memory.Read(this.Address + 20) + 16).ToString("X8");
				text += "\r\n";
				text = text + "StringDieuVan: " + this.game.Memory.ReadString(this.game.Memory.Read(this.Address + 20) + 16);
				text += "\r\n";
				text = text + "HaveLongVan-DieuVan: " + this.IsHaveLongVan.ToString();
				text += "\r\n";
				text = text + "NGOCID1: " + this.ReadNgocID1().ToString();
				text += "\r\n";
				text = text + "NGOCID2: " + this.ReadNgocID2().ToString();
				text += "\r\n";
				text = text + "NGOCID3: " + this.ReadNgocID3().ToString();
				text += "\r\n";
				text = text + "NGOCID4: " + this.ReadNgocID4().ToString();
				text += "\r\n";
				text = text + "IsHaveNgoc: " + this.IsHaveNgoc.ToString();
				text += "\r\n";
				text = text + "TheLuc: " + this.Diem.ToString();
				text += "\r\n";
				text = text + "DiemType: " + this.DiemType;
				text += "\r\n";
				text = text + "PacketId: " + this.PacketId.ToString();
				text += "\r\n";
				text = text + "GiamDinh: " + this.game.Memory.Read(this.game.Memory.Read(this.Address + 20) + 13).ToString("X8");
				text += "\r\n";
				text = text + "DiemAddress: " + this.DiemAddress.ToString("X8");
				text += "\r\n";
				text = text + "HaveTheLuc: " + this.HaveTheLuc.ToString();
				text += "\r\n";
				text = text + "TheLuc: " + this.TheLuc.ToString();
				text += "\r\n";
				text = text + "Lvl: " + this.Lvl.ToString();
				text += "\r\n";
				text = text + "IsCoDinh: " + this.IsCoDinh.ToString();
				text += "\r\n";
				return text + "Name: " + this.Name;
			}
		}

		// Token: 0x17000269 RID: 617
		// (get) Token: 0x06000AB4 RID: 2740 RVA: 0x000456A0 File Offset: 0x000438A0
		// (set) Token: 0x06000AB5 RID: 2741 RVA: 0x000456CC File Offset: 0x000438CC
		public int SplitIndex
		{
			get
			{
				return this.game.Memory.Read(this.game.Address.PacketItemBase[0], 856396);
			}
			set
			{
				this.game.Memory.Write(this.game.Memory.ReadAddress(this.game.Address.PacketItemBase[0], 856392), 2);
				this.game.Memory.Write(this.game.Memory.ReadAddress(this.game.Address.PacketItemBase[0], 856396), value);
			}
		}

		// Token: 0x06000AB6 RID: 2742 RVA: 0x0004574C File Offset: 0x0004394C
		public void GiamDinh()
		{
			if (this.game.Memory.Read(this.game.Memory.Read(this.Address + 20) + 13).ToString("X8").EndsWith("000010") || this.game.Memory.Read(this.game.Memory.Read(this.Address + 20) + 13).ToString("X8").EndsWith("000011"))
			{
				this.game.Memory.Write(this.game.Memory.Read(this.Address + 20) + 13, 50);
			}
		}

		// Token: 0x1700026A RID: 618
		// (get) Token: 0x06000AB7 RID: 2743 RVA: 0x00045811 File Offset: 0x00043A11
		// (set) Token: 0x06000AB8 RID: 2744 RVA: 0x00045819 File Offset: 0x00043A19
		public int Lvl { get; set; }

		// Token: 0x1700026B RID: 619
		// (get) Token: 0x06000AB9 RID: 2745 RVA: 0x00045824 File Offset: 0x00043A24
		public List<int> GetListIndex
		{
			get
			{
				List<int> list = new List<int>();
				foreach (PacketItem packetItem in this.DaoCu)
				{
					list.Add(packetItem.Index);
				}
				return list;
			}
		}

		// Token: 0x06000ABA RID: 2746 RVA: 0x00045884 File Offset: 0x00043A84
		public static List<PacketItem> EnumTrangBi(Game game)
		{
			List<PacketItem> list = new List<PacketItem>();
			int num = game.Memory.Read(new int[]
			{
				game.Address.HaveRide1[0],
				game.Address.HaveRide1[1]
			});
			for (int i = 0; i < 80; i++)
			{
				if (game.Memory.Read(num + i * 4) != 0)
				{
					PacketItem packetItem = new PacketItem(game);
					packetItem.Address = game.Memory.Read(num + i * 4);
					packetItem.Class = game.Memory.Read(packetItem.Address);
					packetItem.PacketId = game.Memory.Read(packetItem.Address + 4);
					if (packetItem.Class == game.Address.PacketType1 || packetItem.Class == game.Address.PacketType5)
					{
						packetItem.Name = game.Memory.ReadString(game.Memory.Read(packetItem.Address + 40, 40));
						packetItem.Count = 1;
						packetItem.TypeName = game.Memory.ReadString(game.Memory.Read(packetItem.Address + 40, 88));
						packetItem.Type = game.Memory.ReadString(game.Memory.Read(packetItem.Address + 40, 84));
					}
					else if (packetItem.Class == game.Address.PacketType2)
					{
						packetItem.Name = game.Memory.ReadString(game.Memory.Read(packetItem.Address + 40, 24));
						packetItem.Count = game.Memory.Read1Byte(packetItem.Address + 20, 60);
						if (game.Address.GameType == 2)
						{
							packetItem.Count = game.Memory.Read1Byte(packetItem.Address + 20, 88);
						}
						packetItem.TypeName = game.Memory.ReadString(game.Memory.Read(packetItem.Address + 40, 80));
						packetItem.Type = game.Memory.ReadString(game.Memory.Read(packetItem.Address + 40, 20));
					}
					else if (packetItem.Class == game.Address.PacketType3)
					{
						packetItem.Name = game.Memory.ReadString(game.Memory.Read(packetItem.Address + 40, 28));
						packetItem.Count = 1;
						packetItem.TypeName = game.Memory.ReadString(game.Memory.Read(packetItem.Address + 40, 304));
						packetItem.Type = game.Memory.ReadString(game.Memory.Read(packetItem.Address + 40, 20));
					}
					else if (packetItem.Class == game.Address.PacketType4)
					{
						packetItem.Name = game.Memory.ReadString(game.Memory.Read(packetItem.Address + 40, 40));
						packetItem.Count = 1;
						packetItem.TypeName = game.Memory.ReadString(game.Memory.Read(packetItem.Address + 40, 76));
						packetItem.Type = game.Memory.ReadString(game.Memory.Read(packetItem.Address + 40, 20));
					}
					else
					{
						packetItem.Name = game.Memory.ReadString(game.Memory.Read(packetItem.Address + 40, 88));
						packetItem.Count = 1;
						packetItem.TypeName = game.Memory.ReadString(game.Memory.Read(packetItem.Address + 40, 80));
						packetItem.Type = game.Memory.ReadString(game.Memory.Read(packetItem.Address + 40, 20));
					}
					if (packetItem.Class == game.Address.PacketType6)
					{
						packetItem.Name = game.Memory.ReadString(game.Memory.Read(packetItem.Address + 40, 44));
						packetItem.Count = 1;
						packetItem.TypeName = game.Memory.ReadString(game.Memory.Read(packetItem.Address + 40, 104));
						packetItem.Type = game.Memory.ReadString(game.Memory.Read(packetItem.Address + 40, 40));
					}
					packetItem.Index = game.Memory.Read(packetItem.Address + 16);
					if (game.Address.GameType == 1)
					{
						packetItem.InfoBase = 66;
					}
					else
					{
						packetItem.InfoBase = 94;
					}
					packetItem.Line = game.Memory.Read2Byte(packetItem.Address + 20, packetItem.InfoBase);
					packetItem.Star = game.Memory.Read2Byte(packetItem.Address + 20, packetItem.InfoBase + 12);
					if (packetItem.Type == "Cloth2_9" || packetItem.Type.Contains("Charm"))
					{
						if (game.Address.GameType == 1)
						{
							packetItem.MapId = game.Memory.Read2Byte(game.Memory.ReadAddress(new int[]
							{
								packetItem.Address + 20,
								52
							}));
							packetItem.X = game.Memory.Read2Byte(packetItem.Address + 20, 54);
							packetItem.Y = game.Memory.Read2Byte(packetItem.Address + 20, 56);
						}
						else
						{
							packetItem.MapId = game.Memory.Read2Byte(game.Memory.ReadAddress(new int[]
							{
								packetItem.Address + 20,
								80
							}));
							packetItem.X = game.Memory.Read2Byte(packetItem.Address + 20, 82);
							packetItem.Y = game.Memory.Read2Byte(packetItem.Address + 20, 84);
						}
					}
					packetItem.Lvl = game.Memory.Read(packetItem.Address + 40, 44);
					list.Add(packetItem);
				}
			}
			return list;
		}

		// Token: 0x1700026C RID: 620
		// (get) Token: 0x06000ABB RID: 2747 RVA: 0x00045EA2 File Offset: 0x000440A2
		public bool IsCoDinh
		{
			get
			{
				return this.game.Memory.Read1Byte(this.game.Memory.Read(this.Address + 20) + 13) == 1;
			}
		}

		// Token: 0x06000ABC RID: 2748 RVA: 0x00045ED3 File Offset: 0x000440D3
		public void Use(int index)
		{
			this.game.LuaDoOneLineString("PlayerPackage:UseItem(" + index.ToString() + ");");
		}

		// Token: 0x06000ABD RID: 2749 RVA: 0x00045EF8 File Offset: 0x000440F8
		public static List<PacketItem> Enum(Game game)
		{
			List<PacketItem> list = new List<PacketItem>();
			int num = game.Memory.Read(game.Address.PacketItemBase);
			for (int i = 0; i < 80; i++)
			{
				if (game.Memory.Read(num + i * 4) != 0)
				{
					PacketItem packetItem = new PacketItem(game);
					packetItem.Address = game.Memory.Read(num + i * 4);
					packetItem.Class = game.Memory.Read(packetItem.Address);
					packetItem.PacketId = game.Memory.Read(packetItem.Address + 4);
					if (packetItem.Class == game.Address.PacketType1 || packetItem.Class == game.Address.PacketType5)
					{
						packetItem.Name = game.Memory.ReadString(game.Memory.Read(packetItem.Address + 40, 40));
						packetItem.Count = 1;
						packetItem.TypeName = game.Memory.ReadString(game.Memory.Read(packetItem.Address + 40, 88));
						packetItem.Type = game.Memory.ReadString(game.Memory.Read(packetItem.Address + 40, 84));
					}
					else if (packetItem.Class == game.Address.PacketType2)
					{
						packetItem.Name = game.Memory.ReadString(game.Memory.Read(packetItem.Address + 40, 24));
						packetItem.Count = game.Memory.Read1Byte(packetItem.Address + 20, 60);
						if (game.Address.GameType == 2)
						{
							packetItem.Count = game.Memory.Read1Byte(packetItem.Address + 20, 88);
						}
						packetItem.TypeName = game.Memory.ReadString(game.Memory.Read(packetItem.Address + 40, 80));
						packetItem.Type = game.Memory.ReadString(game.Memory.Read(packetItem.Address + 40, 20));
					}
					else if (packetItem.Class == game.Address.PacketType3)
					{
						packetItem.Name = game.Memory.ReadString(game.Memory.Read(packetItem.Address + 40, 28));
						packetItem.Count = 1;
						packetItem.TypeName = game.Memory.ReadString(game.Memory.Read(packetItem.Address + 40, 304));
						packetItem.Type = game.Memory.ReadString(game.Memory.Read(packetItem.Address + 40, 20));
					}
					else if (packetItem.Class == game.Address.PacketType4)
					{
						packetItem.Name = game.Memory.ReadString(game.Memory.Read(packetItem.Address + 40, 40));
						packetItem.Count = 1;
						packetItem.TypeName = game.Memory.ReadString(game.Memory.Read(packetItem.Address + 40, 76));
						packetItem.Type = game.Memory.ReadString(game.Memory.Read(packetItem.Address + 40, 20));
					}
					else
					{
						packetItem.Name = game.Memory.ReadString(game.Memory.Read(packetItem.Address + 40, 88));
						packetItem.Count = 1;
						packetItem.TypeName = game.Memory.ReadString(game.Memory.Read(packetItem.Address + 40, 80));
						packetItem.Type = game.Memory.ReadString(game.Memory.Read(packetItem.Address + 40, 20));
					}
					if (packetItem.Class == game.Address.PacketType6)
					{
						packetItem.Name = game.Memory.ReadString(game.Memory.Read(packetItem.Address + 40, 44));
						packetItem.Count = 1;
						packetItem.TypeName = game.Memory.ReadString(game.Memory.Read(packetItem.Address + 40, 104));
						packetItem.Type = game.Memory.ReadString(game.Memory.Read(packetItem.Address + 40, 40));
					}
					packetItem.Index = game.Memory.Read(packetItem.Address + 16);
					if (game.Address.GameType == 1)
					{
						packetItem.InfoBase = 66;
					}
					else
					{
						packetItem.InfoBase = 94;
					}
					packetItem.Line = game.Memory.Read2Byte(packetItem.Address + 20, packetItem.InfoBase);
					packetItem.Star = game.Memory.Read2Byte(packetItem.Address + 20, packetItem.InfoBase + 12);
					if (packetItem.Type == "Cloth2_9" || packetItem.Type.Contains("Charm"))
					{
						if (game.Address.GameType == 1)
						{
							packetItem.MapId = game.Memory.Read2Byte(game.Memory.ReadAddress(new int[]
							{
								packetItem.Address + 20,
								52
							}));
							packetItem.X = game.Memory.Read2Byte(packetItem.Address + 20, 54);
							packetItem.Y = game.Memory.Read2Byte(packetItem.Address + 20, 56);
						}
						else
						{
							packetItem.MapId = game.Memory.Read2Byte(game.Memory.ReadAddress(new int[]
							{
								packetItem.Address + 20,
								80
							}));
							packetItem.X = game.Memory.Read2Byte(packetItem.Address + 20, 82);
							packetItem.Y = game.Memory.Read2Byte(packetItem.Address + 20, 84);
						}
					}
					packetItem.Lvl = game.Memory.Read(packetItem.Address + 40, 44);
					list.Add(packetItem);
				}
			}
			return list;
		}

		// Token: 0x06000ABE RID: 2750 RVA: 0x000464FC File Offset: 0x000446FC
		public static List<PacketItem> EnumDrop(Game game)
		{
			List<PacketItem> list = new List<PacketItem>();
			int num = game.Memory.Read(game.Address.PacketItemBase);
			for (int i = 0; i < 80; i++)
			{
				if (game.Memory.Read(num + i * 4) != 0)
				{
					PacketItem packetItem = new PacketItem(game);
					packetItem.Address = game.Memory.Read(num + i * 4);
					packetItem.Class = game.Memory.Read(packetItem.Address);
					packetItem.PacketId = game.Memory.Read(packetItem.Address + 4);
					if (packetItem.Class == game.Address.PacketType1 || packetItem.Class == game.Address.PacketType5)
					{
						packetItem.Name = game.Memory.ReadString(game.Memory.Read(packetItem.Address + 40, 40));
						packetItem.Count = 1;
						packetItem.TypeName = game.Memory.ReadString(game.Memory.Read(packetItem.Address + 40, 88));
						packetItem.Type = game.Memory.ReadString(game.Memory.Read(packetItem.Address + 40, 84));
					}
					else if (packetItem.Class == game.Address.PacketType2)
					{
						packetItem.Name = game.Memory.ReadString(game.Memory.Read(packetItem.Address + 40, 24));
						packetItem.Count = game.Memory.Read1Byte(packetItem.Address + 20, 60);
						if (game.Address.GameType == 2)
						{
							packetItem.Count = game.Memory.Read1Byte(packetItem.Address + 20, 88);
						}
						packetItem.TypeName = game.Memory.ReadString(game.Memory.Read(packetItem.Address + 40, 80));
						packetItem.Type = game.Memory.ReadString(game.Memory.Read(packetItem.Address + 40, 20));
					}
					else if (packetItem.Class == game.Address.PacketType3)
					{
						packetItem.Name = game.Memory.ReadString(game.Memory.Read(packetItem.Address + 40, 28));
						packetItem.Count = 1;
						packetItem.TypeName = game.Memory.ReadString(game.Memory.Read(packetItem.Address + 40, 304));
						packetItem.Type = game.Memory.ReadString(game.Memory.Read(packetItem.Address + 40, 20));
					}
					else if (packetItem.Class == game.Address.PacketType4)
					{
						packetItem.Name = game.Memory.ReadString(game.Memory.Read(packetItem.Address + 40, 40));
						packetItem.Count = 1;
						packetItem.TypeName = game.Memory.ReadString(game.Memory.Read(packetItem.Address + 40, 76));
						packetItem.Type = game.Memory.ReadString(game.Memory.Read(packetItem.Address + 40, 20));
					}
					else
					{
						packetItem.Name = game.Memory.ReadString(game.Memory.Read(packetItem.Address + 40, 88));
						packetItem.Count = 1;
						packetItem.TypeName = game.Memory.ReadString(game.Memory.Read(packetItem.Address + 40, 80));
						packetItem.Type = game.Memory.ReadString(game.Memory.Read(packetItem.Address + 40, 20));
					}
					if (packetItem.Class == game.Address.PacketType6)
					{
						packetItem.Name = game.Memory.ReadString(game.Memory.Read(packetItem.Address + 40, 44));
						packetItem.Count = 1;
						packetItem.TypeName = game.Memory.ReadString(game.Memory.Read(packetItem.Address + 40, 104));
						packetItem.Type = game.Memory.ReadString(game.Memory.Read(packetItem.Address + 40, 40));
					}
					packetItem.Index = game.Memory.Read(packetItem.Address + 16);
					if (game.Address.GameType == 1)
					{
						packetItem.InfoBase = 66;
					}
					else
					{
						packetItem.InfoBase = 94;
					}
					packetItem.Line = game.Memory.Read2Byte(packetItem.Address + 20, packetItem.InfoBase);
					packetItem.Star = game.Memory.Read2Byte(packetItem.Address + 20, packetItem.InfoBase + 12);
					if (packetItem.Type == "Cloth2_9" || packetItem.Type.Contains("Charm"))
					{
						if (game.Address.GameType == 1)
						{
							packetItem.MapId = game.Memory.Read2Byte(game.Memory.ReadAddress(new int[]
							{
								packetItem.Address + 20,
								52
							}));
							packetItem.X = game.Memory.Read2Byte(packetItem.Address + 20, 54);
							packetItem.Y = game.Memory.Read2Byte(packetItem.Address + 20, 56);
						}
						else
						{
							packetItem.MapId = game.Memory.Read2Byte(game.Memory.ReadAddress(new int[]
							{
								packetItem.Address + 20,
								80
							}));
							packetItem.X = game.Memory.Read2Byte(packetItem.Address + 20, 82);
							packetItem.Y = game.Memory.Read2Byte(packetItem.Address + 20, 84);
						}
					}
					packetItem.Lvl = game.Memory.Read(packetItem.Address + 40, 44);
					if (!(TINHKIEM.VietLien(packetItem.TypeName) == "daocunhiemvu"))
					{
						list.Add(packetItem);
					}
				}
			}
			return list;
		}

		// Token: 0x1700026D RID: 621
		// (get) Token: 0x06000ABF RID: 2751 RVA: 0x00046B18 File Offset: 0x00044D18
		public List<PacketItem> DaoCu
		{
			get
			{
				List<PacketItem> list = new List<PacketItem>();
				int num = this.game.Memory.Read(this.game.Address.PacketItemBase);
				for (int i = 0; i < 30; i++)
				{
					if (this.game.Memory.Read(num + i * 4) != 0)
					{
						PacketItem packetItem = new PacketItem(this.game);
						packetItem.Address = this.game.Memory.Read(num + i * 4);
						packetItem.Class = this.game.Memory.Read(packetItem.Address);
						packetItem.PacketId = this.game.Memory.Read(packetItem.Address + 4);
						if (packetItem.Class == this.game.Address.PacketType1 || packetItem.Class == this.game.Address.PacketType5)
						{
							packetItem.Name = this.game.Memory.ReadString(this.game.Memory.Read(packetItem.Address + 40, 40));
							packetItem.Count = 1;
							packetItem.TypeName = this.game.Memory.ReadString(this.game.Memory.Read(packetItem.Address + 40, 88));
							packetItem.Type = this.game.Memory.ReadString(this.game.Memory.Read(packetItem.Address + 40, 84));
						}
						else if (packetItem.Class == this.game.Address.PacketType2)
						{
							packetItem.Name = this.game.Memory.ReadString(this.game.Memory.Read(packetItem.Address + 40, 24));
							packetItem.Count = this.game.Memory.Read1Byte(packetItem.Address + 20, 60);
							if (this.game.Address.GameType == 2)
							{
								packetItem.Count = this.game.Memory.Read1Byte(packetItem.Address + 20, 88);
							}
							packetItem.TypeName = this.game.Memory.ReadString(this.game.Memory.Read(packetItem.Address + 40, 80));
							packetItem.Type = this.game.Memory.ReadString(this.game.Memory.Read(packetItem.Address + 40, 20));
						}
						else if (packetItem.Class == this.game.Address.PacketType3)
						{
							packetItem.Name = this.game.Memory.ReadString(this.game.Memory.Read(packetItem.Address + 40, 28));
							packetItem.Count = 1;
							packetItem.TypeName = this.game.Memory.ReadString(this.game.Memory.Read(packetItem.Address + 40, 304));
							packetItem.Type = this.game.Memory.ReadString(this.game.Memory.Read(packetItem.Address + 40, 20));
						}
						else if (packetItem.Class == this.game.Address.PacketType4)
						{
							packetItem.Name = this.game.Memory.ReadString(this.game.Memory.Read(packetItem.Address + 40, 40));
							packetItem.Count = 1;
							packetItem.TypeName = this.game.Memory.ReadString(this.game.Memory.Read(packetItem.Address + 40, 76));
							packetItem.Type = this.game.Memory.ReadString(this.game.Memory.Read(packetItem.Address + 40, 20));
						}
						else
						{
							packetItem.Name = this.game.Memory.ReadString(this.game.Memory.Read(packetItem.Address + 40, 88));
							packetItem.Count = 1;
							packetItem.TypeName = this.game.Memory.ReadString(this.game.Memory.Read(packetItem.Address + 40, 80));
							packetItem.Type = this.game.Memory.ReadString(this.game.Memory.Read(packetItem.Address + 40, 20));
						}
						if (packetItem.Class == this.game.Address.PacketType6)
						{
							packetItem.Name = this.game.Memory.ReadString(this.game.Memory.Read(packetItem.Address + 40, 44));
							packetItem.Count = 1;
							packetItem.TypeName = this.game.Memory.ReadString(this.game.Memory.Read(packetItem.Address + 40, 104));
							packetItem.Type = this.game.Memory.ReadString(this.game.Memory.Read(packetItem.Address + 40, 40));
						}
						packetItem.Index = this.game.Memory.Read(packetItem.Address + 16);
						if (this.game.Address.GameType == 1)
						{
							packetItem.InfoBase = 66;
						}
						else
						{
							packetItem.InfoBase = 94;
						}
						packetItem.Line = this.game.Memory.Read2Byte(packetItem.Address + 20, packetItem.InfoBase);
						packetItem.Star = this.game.Memory.Read2Byte(packetItem.Address + 20, packetItem.InfoBase + 12);
						if (packetItem.Type == "Cloth2_9" || packetItem.Type.Contains("Charm"))
						{
							if (this.game.Address.GameType == 1)
							{
								packetItem.MapId = this.game.Memory.Read2Byte(this.game.Memory.ReadAddress(new int[]
								{
									packetItem.Address + 20,
									52
								}));
								packetItem.X = this.game.Memory.Read2Byte(packetItem.Address + 20, 54);
								packetItem.Y = this.game.Memory.Read2Byte(packetItem.Address + 20, 56);
							}
							else
							{
								packetItem.MapId = this.game.Memory.Read2Byte(this.game.Memory.ReadAddress(new int[]
								{
									packetItem.Address + 20,
									80
								}));
								packetItem.X = this.game.Memory.Read2Byte(packetItem.Address + 20, 82);
								packetItem.Y = this.game.Memory.Read2Byte(packetItem.Address + 20, 84);
							}
						}
						packetItem.Lvl = this.game.Memory.Read(packetItem.Address + 40, 44);
						if (!(TINHKIEM.VietLien(packetItem.TypeName) == "daocunhiemvu"))
						{
							list.Add(packetItem);
						}
					}
				}
				return list;
			}
		}

		// Token: 0x1700026E RID: 622
		// (get) Token: 0x06000AC0 RID: 2752 RVA: 0x00047288 File Offset: 0x00045488
		public List<PacketItem> NguyenLieu
		{
			get
			{
				List<PacketItem> list = new List<PacketItem>();
				int num = this.game.Memory.Read(this.game.Address.PacketItemBase);
				for (int i = 30; i < 60; i++)
				{
					if (this.game.Memory.Read(num + i * 4) != 0)
					{
						PacketItem packetItem = new PacketItem(this.game);
						packetItem.Address = this.game.Memory.Read(num + i * 4);
						packetItem.Class = this.game.Memory.Read(packetItem.Address);
						packetItem.PacketId = this.game.Memory.Read(packetItem.Address + 4);
						if (packetItem.Class == this.game.Address.PacketType1 || packetItem.Class == this.game.Address.PacketType5)
						{
							packetItem.Name = this.game.Memory.ReadString(this.game.Memory.Read(packetItem.Address + 40, 40));
							packetItem.Count = 1;
							packetItem.TypeName = this.game.Memory.ReadString(this.game.Memory.Read(packetItem.Address + 40, 88));
							packetItem.Type = this.game.Memory.ReadString(this.game.Memory.Read(packetItem.Address + 40, 84));
						}
						else if (packetItem.Class == this.game.Address.PacketType2)
						{
							packetItem.Name = this.game.Memory.ReadString(this.game.Memory.Read(packetItem.Address + 40, 24));
							packetItem.Count = this.game.Memory.Read1Byte(packetItem.Address + 20, 60);
							if (this.game.Address.GameType == 2)
							{
								packetItem.Count = this.game.Memory.Read1Byte(packetItem.Address + 20, 88);
							}
							packetItem.TypeName = this.game.Memory.ReadString(this.game.Memory.Read(packetItem.Address + 40, 80));
							packetItem.Type = this.game.Memory.ReadString(this.game.Memory.Read(packetItem.Address + 40, 20));
						}
						else if (packetItem.Class == this.game.Address.PacketType3)
						{
							packetItem.Name = this.game.Memory.ReadString(this.game.Memory.Read(packetItem.Address + 40, 28));
							packetItem.Count = 1;
							packetItem.TypeName = this.game.Memory.ReadString(this.game.Memory.Read(packetItem.Address + 40, 304));
							packetItem.Type = this.game.Memory.ReadString(this.game.Memory.Read(packetItem.Address + 40, 20));
						}
						else if (packetItem.Class == this.game.Address.PacketType4)
						{
							packetItem.Name = this.game.Memory.ReadString(this.game.Memory.Read(packetItem.Address + 40, 40));
							packetItem.Count = 1;
							packetItem.TypeName = this.game.Memory.ReadString(this.game.Memory.Read(packetItem.Address + 40, 76));
							packetItem.Type = this.game.Memory.ReadString(this.game.Memory.Read(packetItem.Address + 40, 20));
						}
						else
						{
							packetItem.Name = this.game.Memory.ReadString(this.game.Memory.Read(packetItem.Address + 40, 88));
							packetItem.Count = 1;
							packetItem.TypeName = this.game.Memory.ReadString(this.game.Memory.Read(packetItem.Address + 40, 80));
							packetItem.Type = this.game.Memory.ReadString(this.game.Memory.Read(packetItem.Address + 40, 20));
						}
						if (packetItem.Class == this.game.Address.PacketType6)
						{
							packetItem.Name = this.game.Memory.ReadString(this.game.Memory.Read(packetItem.Address + 40, 44));
							packetItem.Count = 1;
							packetItem.TypeName = this.game.Memory.ReadString(this.game.Memory.Read(packetItem.Address + 40, 104));
							packetItem.Type = this.game.Memory.ReadString(this.game.Memory.Read(packetItem.Address + 40, 40));
						}
						packetItem.Index = this.game.Memory.Read(packetItem.Address + 16);
						if (this.game.Address.GameType == 1)
						{
							packetItem.InfoBase = 66;
						}
						else
						{
							packetItem.InfoBase = 94;
						}
						packetItem.Line = this.game.Memory.Read2Byte(packetItem.Address + 20, packetItem.InfoBase);
						packetItem.Star = this.game.Memory.Read2Byte(packetItem.Address + 20, packetItem.InfoBase + 12);
						if (packetItem.Type == "Cloth2_9" || packetItem.Type.Contains("Charm"))
						{
							if (this.game.Address.GameType == 1)
							{
								packetItem.MapId = this.game.Memory.Read2Byte(this.game.Memory.ReadAddress(new int[]
								{
									packetItem.Address + 20,
									52
								}));
								packetItem.X = this.game.Memory.Read2Byte(packetItem.Address + 20, 54);
								packetItem.Y = this.game.Memory.Read2Byte(packetItem.Address + 20, 56);
							}
							else
							{
								packetItem.MapId = this.game.Memory.Read2Byte(this.game.Memory.ReadAddress(new int[]
								{
									packetItem.Address + 20,
									80
								}));
								packetItem.X = this.game.Memory.Read2Byte(packetItem.Address + 20, 82);
								packetItem.Y = this.game.Memory.Read2Byte(packetItem.Address + 20, 84);
							}
						}
						packetItem.Lvl = this.game.Memory.Read(packetItem.Address + 40, 44);
						if (!(TINHKIEM.VietLien(packetItem.TypeName) == "daocunhiemvu"))
						{
							list.Add(packetItem);
						}
					}
				}
				return list;
			}
		}

		// Token: 0x1700026F RID: 623
		// (get) Token: 0x06000AC1 RID: 2753 RVA: 0x000479FC File Offset: 0x00045BFC
		public int GetNumberFromString
		{
			get
			{
				int result = 0;
				try
				{
					result = int.Parse(Regex.Match(this.Name, "\\d+").Value);
				}
				catch
				{
				}
				return result;
			}
		}

		// Token: 0x04000840 RID: 2112
		private Game game;

		// Token: 0x04000841 RID: 2113
		public int Address;

		// Token: 0x04000842 RID: 2114
		public int Class;

		// Token: 0x04000843 RID: 2115
		public int PacketId;

		// Token: 0x04000844 RID: 2116
		public string Name;

		// Token: 0x04000845 RID: 2117
		public int Count;

		// Token: 0x04000846 RID: 2118
		public int Index;

		// Token: 0x04000847 RID: 2119
		public string TypeName;

		// Token: 0x04000848 RID: 2120
		public int Star;

		// Token: 0x04000849 RID: 2121
		public int Line;

		// Token: 0x0400084A RID: 2122
		public string Type;

		// Token: 0x0400084B RID: 2123
		public int MapId = -1;

		// Token: 0x0400084C RID: 2124
		public int X;

		// Token: 0x0400084D RID: 2125
		public int Y;

		// Token: 0x0400084E RID: 2126
		public int InfoBase;
	}
}
