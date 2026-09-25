using System;
using System.Collections.Generic;

namespace TinhKiemAuto
{
	// Token: 0x02000080 RID: 128
	internal class Bank
	{
		// Token: 0x060005D9 RID: 1497 RVA: 0x0001EEBA File Offset: 0x0001D0BA
		public Bank(Game game)
		{
			this.game = game;
		}

		// Token: 0x17000109 RID: 265
		// (get) Token: 0x060005DA RID: 1498 RVA: 0x0001EED0 File Offset: 0x0001D0D0
		public string ClearName
		{
			get
			{
				return TINHKIEM.VietLien(this.Name);
			}
		}

		// Token: 0x1700010A RID: 266
		// (get) Token: 0x060005DB RID: 1499 RVA: 0x0001EEDD File Offset: 0x0001D0DD
		public int DiemAddress
		{
			get
			{
				return this.game.Memory.Read(this.Address + 20) + 144;
			}
		}

		// Token: 0x1700010B RID: 267
		// (get) Token: 0x060005DC RID: 1500 RVA: 0x0001EEFE File Offset: 0x0001D0FE
		public bool HaveTheLuc
		{
			get
			{
				return this.DiemType[51] == '1';
			}
		}

		// Token: 0x1700010C RID: 268
		// (get) Token: 0x060005DD RID: 1501 RVA: 0x0001EF14 File Offset: 0x0001D114
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

		// Token: 0x1700010D RID: 269
		// (get) Token: 0x060005DE RID: 1502 RVA: 0x0001EFDC File Offset: 0x0001D1DC
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

		// Token: 0x1700010E RID: 270
		// (get) Token: 0x060005DF RID: 1503 RVA: 0x0001F0B0 File Offset: 0x0001D2B0
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

		// Token: 0x1700010F RID: 271
		// (get) Token: 0x060005E0 RID: 1504 RVA: 0x0001F110 File Offset: 0x0001D310
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

		// Token: 0x17000110 RID: 272
		// (get) Token: 0x060005E1 RID: 1505 RVA: 0x0001F154 File Offset: 0x0001D354
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

		// Token: 0x17000111 RID: 273
		// (get) Token: 0x060005E2 RID: 1506 RVA: 0x0001F198 File Offset: 0x0001D398
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

		// Token: 0x17000112 RID: 274
		// (get) Token: 0x060005E3 RID: 1507 RVA: 0x0001F1DC File Offset: 0x0001D3DC
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
				return text + "IsCoDinh: " + this.IsCoDinh.ToString();
			}
		}

		// Token: 0x17000113 RID: 275
		// (get) Token: 0x060005E4 RID: 1508 RVA: 0x0001F51E File Offset: 0x0001D71E
		// (set) Token: 0x060005E5 RID: 1509 RVA: 0x0001F548 File Offset: 0x0001D748
		public int SplitIndex
		{
			get
			{
				return this.game.Memory.Read(this.game.Address.BankBase[0], 856396);
			}
			set
			{
				this.game.Memory.Write(this.game.Memory.ReadAddress(this.game.Address.BankBase[0], 856392), 2);
				this.game.Memory.Write(this.game.Memory.ReadAddress(this.game.Address.BankBase[0], 856396), value);
			}
		}

		// Token: 0x060005E6 RID: 1510 RVA: 0x0001F5C8 File Offset: 0x0001D7C8
		public void GiamDinh()
		{
			if (this.game.Memory.Read(this.game.Memory.Read(this.Address + 20) + 13).ToString("X8").EndsWith("000010") || this.game.Memory.Read(this.game.Memory.Read(this.Address + 20) + 13).ToString("X8").EndsWith("000011"))
			{
				this.game.Memory.Write(this.game.Memory.Read(this.Address + 20) + 13, 50);
			}
		}

		// Token: 0x17000114 RID: 276
		// (get) Token: 0x060005E7 RID: 1511 RVA: 0x0001F68D File Offset: 0x0001D88D
		// (set) Token: 0x060005E8 RID: 1512 RVA: 0x0001F695 File Offset: 0x0001D895
		public int Lvl { get; set; }

		// Token: 0x060005E9 RID: 1513 RVA: 0x0001F6A0 File Offset: 0x0001D8A0
		public static List<Bank> EnumTrangBi(Game game)
		{
			List<Bank> list = new List<Bank>();
			int num = game.Memory.Read(new int[]
			{
				game.Address.HaveRide1[0],
				game.Address.HaveRide1[1]
			});
			for (int i = 0; i < 80; i++)
			{
				if (game.Memory.Read(num + i * 4) != 0)
				{
					Bank bank = new Bank(game);
					bank.Address = game.Memory.Read(num + i * 4);
					bank.Class = game.Memory.Read(bank.Address);
					bank.PacketId = game.Memory.Read(bank.Address + 4);
					if (bank.Class == game.Address.PacketType1 || bank.Class == game.Address.PacketType5)
					{
						bank.Name = game.Memory.ReadString(game.Memory.Read(bank.Address + 40, 40));
						bank.Count = 1;
						bank.TypeName = game.Memory.ReadString(game.Memory.Read(bank.Address + 40, 88));
						bank.Type = game.Memory.ReadString(game.Memory.Read(bank.Address + 40, 84));
					}
					else if (bank.Class == game.Address.PacketType2)
					{
						bank.Name = game.Memory.ReadString(game.Memory.Read(bank.Address + 40, 24));
						bank.Count = game.Memory.Read1Byte(bank.Address + 20, 60);
						if (game.Address.GameType == 2)
						{
							bank.Count = game.Memory.Read1Byte(bank.Address + 20, 88);
						}
						bank.TypeName = game.Memory.ReadString(game.Memory.Read(bank.Address + 40, 80));
						bank.Type = game.Memory.ReadString(game.Memory.Read(bank.Address + 40, 20));
					}
					else if (bank.Class == game.Address.PacketType3)
					{
						bank.Name = game.Memory.ReadString(game.Memory.Read(bank.Address + 40, 28));
						bank.Count = 1;
						bank.TypeName = game.Memory.ReadString(game.Memory.Read(bank.Address + 40, 304));
						bank.Type = game.Memory.ReadString(game.Memory.Read(bank.Address + 40, 20));
					}
					else if (bank.Class == game.Address.PacketType4)
					{
						bank.Name = game.Memory.ReadString(game.Memory.Read(bank.Address + 40, 40));
						bank.Count = 1;
						bank.TypeName = game.Memory.ReadString(game.Memory.Read(bank.Address + 40, 76));
						bank.Type = game.Memory.ReadString(game.Memory.Read(bank.Address + 40, 20));
					}
					else
					{
						bank.Name = game.Memory.ReadString(game.Memory.Read(bank.Address + 40, 88));
						bank.Count = 1;
						bank.TypeName = game.Memory.ReadString(game.Memory.Read(bank.Address + 40, 80));
						bank.Type = game.Memory.ReadString(game.Memory.Read(bank.Address + 40, 20));
					}
					if (bank.Class == game.Address.PacketType6)
					{
						bank.Name = game.Memory.ReadString(game.Memory.Read(bank.Address + 40, 44));
						bank.Count = 1;
						bank.TypeName = game.Memory.ReadString(game.Memory.Read(bank.Address + 40, 104));
						bank.Type = game.Memory.ReadString(game.Memory.Read(bank.Address + 40, 40));
					}
					bank.Index = game.Memory.Read(bank.Address + 16);
					if (game.Address.GameType == 1)
					{
						bank.InfoBase = 66;
					}
					else
					{
						bank.InfoBase = 94;
					}
					bank.Line = game.Memory.Read2Byte(bank.Address + 20, bank.InfoBase);
					bank.Star = game.Memory.Read2Byte(bank.Address + 20, bank.InfoBase + 12);
					if (bank.Type == "Cloth2_9" || bank.Type.Contains("Charm"))
					{
						if (game.Address.GameType == 1)
						{
							bank.MapId = game.Memory.Read2Byte(game.Memory.ReadAddress(new int[]
							{
								bank.Address + 20,
								52
							}));
							bank.X = game.Memory.Read2Byte(bank.Address + 20, 54);
							bank.Y = game.Memory.Read2Byte(bank.Address + 20, 56);
						}
						else
						{
							bank.MapId = game.Memory.Read2Byte(game.Memory.ReadAddress(new int[]
							{
								bank.Address + 20,
								80
							}));
							bank.X = game.Memory.Read2Byte(bank.Address + 20, 82);
							bank.Y = game.Memory.Read2Byte(bank.Address + 20, 84);
						}
					}
					bank.Lvl = game.Memory.Read(bank.Address + 40, 44);
					list.Add(bank);
				}
			}
			return list;
		}

		// Token: 0x17000115 RID: 277
		// (get) Token: 0x060005EA RID: 1514 RVA: 0x0001FCBE File Offset: 0x0001DEBE
		public bool IsCoDinh
		{
			get
			{
				return this.game.Memory.Read1Byte(this.game.Memory.Read(this.Address + 20) + 13) == 1;
			}
		}

		// Token: 0x060005EB RID: 1515 RVA: 0x0001FCF0 File Offset: 0x0001DEF0
		public List<Bank> Enum()
		{
			List<Bank> list = new List<Bank>();
			int num = this.game.Memory.Read(this.game.Address.BankBase);
			for (int i = 0; i < 20; i++)
			{
				if (this.game.Memory.Read(num + i * 4) != 0)
				{
					Bank bank = new Bank(this.game);
					bank.Address = this.game.Memory.Read(num + i * 4);
					bank.Class = this.game.Memory.Read(bank.Address);
					bank.PacketId = this.game.Memory.Read(bank.Address + 4);
					if (bank.Class == this.game.Address.PacketType1 || bank.Class == this.game.Address.PacketType5)
					{
						bank.Name = this.game.Memory.ReadString(this.game.Memory.Read(bank.Address + 40, 40));
						bank.Count = 1;
						bank.TypeName = this.game.Memory.ReadString(this.game.Memory.Read(bank.Address + 40, 88));
						bank.Type = this.game.Memory.ReadString(this.game.Memory.Read(bank.Address + 40, 84));
					}
					else if (bank.Class == this.game.Address.PacketType2)
					{
						bank.Name = this.game.Memory.ReadString(this.game.Memory.Read(bank.Address + 40, 24));
						bank.Count = this.game.Memory.Read1Byte(bank.Address + 20, 60);
						if (this.game.Address.GameType == 2)
						{
							bank.Count = this.game.Memory.Read1Byte(bank.Address + 20, 88);
						}
						bank.TypeName = this.game.Memory.ReadString(this.game.Memory.Read(bank.Address + 40, 80));
						bank.Type = this.game.Memory.ReadString(this.game.Memory.Read(bank.Address + 40, 20));
					}
					else if (bank.Class == this.game.Address.PacketType3)
					{
						bank.Name = this.game.Memory.ReadString(this.game.Memory.Read(bank.Address + 40, 28));
						bank.Count = 1;
						bank.TypeName = this.game.Memory.ReadString(this.game.Memory.Read(bank.Address + 40, 304));
						bank.Type = this.game.Memory.ReadString(this.game.Memory.Read(bank.Address + 40, 20));
					}
					else if (bank.Class == this.game.Address.PacketType4)
					{
						bank.Name = this.game.Memory.ReadString(this.game.Memory.Read(bank.Address + 40, 40));
						bank.Count = 1;
						bank.TypeName = this.game.Memory.ReadString(this.game.Memory.Read(bank.Address + 40, 76));
						bank.Type = this.game.Memory.ReadString(this.game.Memory.Read(bank.Address + 40, 20));
					}
					else
					{
						bank.Name = this.game.Memory.ReadString(this.game.Memory.Read(bank.Address + 40, 88));
						bank.Count = 1;
						bank.TypeName = this.game.Memory.ReadString(this.game.Memory.Read(bank.Address + 40, 80));
						bank.Type = this.game.Memory.ReadString(this.game.Memory.Read(bank.Address + 40, 20));
					}
					if (bank.Class == this.game.Address.PacketType6)
					{
						bank.Name = this.game.Memory.ReadString(this.game.Memory.Read(bank.Address + 40, 44));
						bank.Count = 1;
						bank.TypeName = this.game.Memory.ReadString(this.game.Memory.Read(bank.Address + 40, 104));
						bank.Type = this.game.Memory.ReadString(this.game.Memory.Read(bank.Address + 40, 40));
					}
					bank.Index = this.game.Memory.Read(bank.Address + 16);
					if (this.game.Address.GameType == 1)
					{
						bank.InfoBase = 66;
					}
					else
					{
						bank.InfoBase = 94;
					}
					bank.Line = this.game.Memory.Read2Byte(bank.Address + 20, bank.InfoBase);
					bank.Star = this.game.Memory.Read2Byte(bank.Address + 20, bank.InfoBase + 12);
					if (bank.Type == "Cloth2_9" || bank.Type.Contains("Charm"))
					{
						if (this.game.Address.GameType == 1)
						{
							bank.MapId = this.game.Memory.Read2Byte(this.game.Memory.ReadAddress(new int[]
							{
								bank.Address + 20,
								52
							}));
							bank.X = this.game.Memory.Read2Byte(bank.Address + 20, 54);
							bank.Y = this.game.Memory.Read2Byte(bank.Address + 20, 56);
						}
						else
						{
							bank.MapId = this.game.Memory.Read2Byte(this.game.Memory.ReadAddress(new int[]
							{
								bank.Address + 20,
								80
							}));
							bank.X = this.game.Memory.Read2Byte(bank.Address + 20, 82);
							bank.Y = this.game.Memory.Read2Byte(bank.Address + 20, 84);
						}
					}
					bank.Lvl = this.game.Memory.Read(bank.Address + 40, 44);
					list.Add(bank);
				}
			}
			return list;
		}

		// Token: 0x060005EC RID: 1516 RVA: 0x0002044C File Offset: 0x0001E64C
		public static List<Bank> EnumDrop(Game game)
		{
			List<Bank> list = new List<Bank>();
			int num = game.Memory.Read(game.Address.BankBase);
			for (int i = 0; i < 80; i++)
			{
				if (game.Memory.Read(num + i * 4) != 0)
				{
					Bank bank = new Bank(game);
					bank.Address = game.Memory.Read(num + i * 4);
					bank.Class = game.Memory.Read(bank.Address);
					bank.PacketId = game.Memory.Read(bank.Address + 4);
					if (bank.Class == game.Address.PacketType1 || bank.Class == game.Address.PacketType5)
					{
						bank.Name = game.Memory.ReadString(game.Memory.Read(bank.Address + 40, 40));
						bank.Count = 1;
						bank.TypeName = game.Memory.ReadString(game.Memory.Read(bank.Address + 40, 88));
						bank.Type = game.Memory.ReadString(game.Memory.Read(bank.Address + 40, 84));
					}
					else if (bank.Class == game.Address.PacketType2)
					{
						bank.Name = game.Memory.ReadString(game.Memory.Read(bank.Address + 40, 24));
						bank.Count = game.Memory.Read1Byte(bank.Address + 20, 60);
						if (game.Address.GameType == 2)
						{
							bank.Count = game.Memory.Read1Byte(bank.Address + 20, 88);
						}
						bank.TypeName = game.Memory.ReadString(game.Memory.Read(bank.Address + 40, 80));
						bank.Type = game.Memory.ReadString(game.Memory.Read(bank.Address + 40, 20));
					}
					else if (bank.Class == game.Address.PacketType3)
					{
						bank.Name = game.Memory.ReadString(game.Memory.Read(bank.Address + 40, 28));
						bank.Count = 1;
						bank.TypeName = game.Memory.ReadString(game.Memory.Read(bank.Address + 40, 304));
						bank.Type = game.Memory.ReadString(game.Memory.Read(bank.Address + 40, 20));
					}
					else if (bank.Class == game.Address.PacketType4)
					{
						bank.Name = game.Memory.ReadString(game.Memory.Read(bank.Address + 40, 40));
						bank.Count = 1;
						bank.TypeName = game.Memory.ReadString(game.Memory.Read(bank.Address + 40, 76));
						bank.Type = game.Memory.ReadString(game.Memory.Read(bank.Address + 40, 20));
					}
					else
					{
						bank.Name = game.Memory.ReadString(game.Memory.Read(bank.Address + 40, 88));
						bank.Count = 1;
						bank.TypeName = game.Memory.ReadString(game.Memory.Read(bank.Address + 40, 80));
						bank.Type = game.Memory.ReadString(game.Memory.Read(bank.Address + 40, 20));
					}
					if (bank.Class == game.Address.PacketType6)
					{
						bank.Name = game.Memory.ReadString(game.Memory.Read(bank.Address + 40, 44));
						bank.Count = 1;
						bank.TypeName = game.Memory.ReadString(game.Memory.Read(bank.Address + 40, 104));
						bank.Type = game.Memory.ReadString(game.Memory.Read(bank.Address + 40, 40));
					}
					bank.Index = game.Memory.Read(bank.Address + 16);
					if (game.Address.GameType == 1)
					{
						bank.InfoBase = 66;
					}
					else
					{
						bank.InfoBase = 94;
					}
					bank.Line = game.Memory.Read2Byte(bank.Address + 20, bank.InfoBase);
					bank.Star = game.Memory.Read2Byte(bank.Address + 20, bank.InfoBase + 12);
					if (bank.Type == "Cloth2_9" || bank.Type.Contains("Charm"))
					{
						if (game.Address.GameType == 1)
						{
							bank.MapId = game.Memory.Read2Byte(game.Memory.ReadAddress(new int[]
							{
								bank.Address + 20,
								52
							}));
							bank.X = game.Memory.Read2Byte(bank.Address + 20, 54);
							bank.Y = game.Memory.Read2Byte(bank.Address + 20, 56);
						}
						else
						{
							bank.MapId = game.Memory.Read2Byte(game.Memory.ReadAddress(new int[]
							{
								bank.Address + 20,
								80
							}));
							bank.X = game.Memory.Read2Byte(bank.Address + 20, 82);
							bank.Y = game.Memory.Read2Byte(bank.Address + 20, 84);
						}
					}
					bank.Lvl = game.Memory.Read(bank.Address + 40, 44);
					if (!(TINHKIEM.VietLien(bank.TypeName) == "daocunhiemvu"))
					{
						list.Add(bank);
					}
				}
			}
			return list;
		}

		// Token: 0x17000116 RID: 278
		// (get) Token: 0x060005ED RID: 1517 RVA: 0x00020A68 File Offset: 0x0001EC68
		public List<Bank> DaoCu
		{
			get
			{
				List<Bank> list = new List<Bank>();
				int num = this.game.Memory.Read(this.game.Address.BankBase);
				for (int i = 0; i < 30; i++)
				{
					if (this.game.Memory.Read(num + i * 4) != 0)
					{
						Bank bank = new Bank(this.game);
						bank.Address = this.game.Memory.Read(num + i * 4);
						bank.Class = this.game.Memory.Read(bank.Address);
						bank.PacketId = this.game.Memory.Read(bank.Address + 4);
						if (bank.Class == this.game.Address.PacketType1 || bank.Class == this.game.Address.PacketType5)
						{
							bank.Name = this.game.Memory.ReadString(this.game.Memory.Read(bank.Address + 40, 40));
							bank.Count = 1;
							bank.TypeName = this.game.Memory.ReadString(this.game.Memory.Read(bank.Address + 40, 88));
							bank.Type = this.game.Memory.ReadString(this.game.Memory.Read(bank.Address + 40, 84));
						}
						else if (bank.Class == this.game.Address.PacketType2)
						{
							bank.Name = this.game.Memory.ReadString(this.game.Memory.Read(bank.Address + 40, 24));
							bank.Count = this.game.Memory.Read1Byte(bank.Address + 20, 60);
							if (this.game.Address.GameType == 2)
							{
								bank.Count = this.game.Memory.Read1Byte(bank.Address + 20, 88);
							}
							bank.TypeName = this.game.Memory.ReadString(this.game.Memory.Read(bank.Address + 40, 80));
							bank.Type = this.game.Memory.ReadString(this.game.Memory.Read(bank.Address + 40, 20));
						}
						else if (bank.Class == this.game.Address.PacketType3)
						{
							bank.Name = this.game.Memory.ReadString(this.game.Memory.Read(bank.Address + 40, 28));
							bank.Count = 1;
							bank.TypeName = this.game.Memory.ReadString(this.game.Memory.Read(bank.Address + 40, 304));
							bank.Type = this.game.Memory.ReadString(this.game.Memory.Read(bank.Address + 40, 20));
						}
						else if (bank.Class == this.game.Address.PacketType4)
						{
							bank.Name = this.game.Memory.ReadString(this.game.Memory.Read(bank.Address + 40, 40));
							bank.Count = 1;
							bank.TypeName = this.game.Memory.ReadString(this.game.Memory.Read(bank.Address + 40, 76));
							bank.Type = this.game.Memory.ReadString(this.game.Memory.Read(bank.Address + 40, 20));
						}
						else
						{
							bank.Name = this.game.Memory.ReadString(this.game.Memory.Read(bank.Address + 40, 88));
							bank.Count = 1;
							bank.TypeName = this.game.Memory.ReadString(this.game.Memory.Read(bank.Address + 40, 80));
							bank.Type = this.game.Memory.ReadString(this.game.Memory.Read(bank.Address + 40, 20));
						}
						if (bank.Class == this.game.Address.PacketType6)
						{
							bank.Name = this.game.Memory.ReadString(this.game.Memory.Read(bank.Address + 40, 44));
							bank.Count = 1;
							bank.TypeName = this.game.Memory.ReadString(this.game.Memory.Read(bank.Address + 40, 104));
							bank.Type = this.game.Memory.ReadString(this.game.Memory.Read(bank.Address + 40, 40));
						}
						bank.Index = this.game.Memory.Read(bank.Address + 16);
						if (this.game.Address.GameType == 1)
						{
							bank.InfoBase = 66;
						}
						else
						{
							bank.InfoBase = 94;
						}
						bank.Line = this.game.Memory.Read2Byte(bank.Address + 20, bank.InfoBase);
						bank.Star = this.game.Memory.Read2Byte(bank.Address + 20, bank.InfoBase + 12);
						if (bank.Type == "Cloth2_9" || bank.Type.Contains("Charm"))
						{
							if (this.game.Address.GameType == 1)
							{
								bank.MapId = this.game.Memory.Read2Byte(this.game.Memory.ReadAddress(new int[]
								{
									bank.Address + 20,
									52
								}));
								bank.X = this.game.Memory.Read2Byte(bank.Address + 20, 54);
								bank.Y = this.game.Memory.Read2Byte(bank.Address + 20, 56);
							}
							else
							{
								bank.MapId = this.game.Memory.Read2Byte(this.game.Memory.ReadAddress(new int[]
								{
									bank.Address + 20,
									80
								}));
								bank.X = this.game.Memory.Read2Byte(bank.Address + 20, 82);
								bank.Y = this.game.Memory.Read2Byte(bank.Address + 20, 84);
							}
						}
						bank.Lvl = this.game.Memory.Read(bank.Address + 40, 44);
						if (!(TINHKIEM.VietLien(bank.TypeName) == "daocunhiemvu"))
						{
							list.Add(bank);
						}
					}
				}
				return list;
			}
		}

		// Token: 0x17000117 RID: 279
		// (get) Token: 0x060005EE RID: 1518 RVA: 0x000211D8 File Offset: 0x0001F3D8
		public List<Bank> NguyenLieu
		{
			get
			{
				List<Bank> list = new List<Bank>();
				int num = this.game.Memory.Read(this.game.Address.BankBase);
				for (int i = 30; i < 60; i++)
				{
					if (this.game.Memory.Read(num + i * 4) != 0)
					{
						Bank bank = new Bank(this.game);
						bank.Address = this.game.Memory.Read(num + i * 4);
						bank.Class = this.game.Memory.Read(bank.Address);
						bank.PacketId = this.game.Memory.Read(bank.Address + 4);
						if (bank.Class == this.game.Address.PacketType1 || bank.Class == this.game.Address.PacketType5)
						{
							bank.Name = this.game.Memory.ReadString(this.game.Memory.Read(bank.Address + 40, 40));
							bank.Count = 1;
							bank.TypeName = this.game.Memory.ReadString(this.game.Memory.Read(bank.Address + 40, 88));
							bank.Type = this.game.Memory.ReadString(this.game.Memory.Read(bank.Address + 40, 84));
						}
						else if (bank.Class == this.game.Address.PacketType2)
						{
							bank.Name = this.game.Memory.ReadString(this.game.Memory.Read(bank.Address + 40, 24));
							bank.Count = this.game.Memory.Read1Byte(bank.Address + 20, 60);
							if (this.game.Address.GameType == 2)
							{
								bank.Count = this.game.Memory.Read1Byte(bank.Address + 20, 88);
							}
							bank.TypeName = this.game.Memory.ReadString(this.game.Memory.Read(bank.Address + 40, 80));
							bank.Type = this.game.Memory.ReadString(this.game.Memory.Read(bank.Address + 40, 20));
						}
						else if (bank.Class == this.game.Address.PacketType3)
						{
							bank.Name = this.game.Memory.ReadString(this.game.Memory.Read(bank.Address + 40, 28));
							bank.Count = 1;
							bank.TypeName = this.game.Memory.ReadString(this.game.Memory.Read(bank.Address + 40, 304));
							bank.Type = this.game.Memory.ReadString(this.game.Memory.Read(bank.Address + 40, 20));
						}
						else if (bank.Class == this.game.Address.PacketType4)
						{
							bank.Name = this.game.Memory.ReadString(this.game.Memory.Read(bank.Address + 40, 40));
							bank.Count = 1;
							bank.TypeName = this.game.Memory.ReadString(this.game.Memory.Read(bank.Address + 40, 76));
							bank.Type = this.game.Memory.ReadString(this.game.Memory.Read(bank.Address + 40, 20));
						}
						else
						{
							bank.Name = this.game.Memory.ReadString(this.game.Memory.Read(bank.Address + 40, 88));
							bank.Count = 1;
							bank.TypeName = this.game.Memory.ReadString(this.game.Memory.Read(bank.Address + 40, 80));
							bank.Type = this.game.Memory.ReadString(this.game.Memory.Read(bank.Address + 40, 20));
						}
						if (bank.Class == this.game.Address.PacketType6)
						{
							bank.Name = this.game.Memory.ReadString(this.game.Memory.Read(bank.Address + 40, 44));
							bank.Count = 1;
							bank.TypeName = this.game.Memory.ReadString(this.game.Memory.Read(bank.Address + 40, 104));
							bank.Type = this.game.Memory.ReadString(this.game.Memory.Read(bank.Address + 40, 40));
						}
						bank.Index = this.game.Memory.Read(bank.Address + 16);
						if (this.game.Address.GameType == 1)
						{
							bank.InfoBase = 66;
						}
						else
						{
							bank.InfoBase = 94;
						}
						bank.Line = this.game.Memory.Read2Byte(bank.Address + 20, bank.InfoBase);
						bank.Star = this.game.Memory.Read2Byte(bank.Address + 20, bank.InfoBase + 12);
						if (bank.Type == "Cloth2_9" || bank.Type.Contains("Charm"))
						{
							if (this.game.Address.GameType == 1)
							{
								bank.MapId = this.game.Memory.Read2Byte(this.game.Memory.ReadAddress(new int[]
								{
									bank.Address + 20,
									52
								}));
								bank.X = this.game.Memory.Read2Byte(bank.Address + 20, 54);
								bank.Y = this.game.Memory.Read2Byte(bank.Address + 20, 56);
							}
							else
							{
								bank.MapId = this.game.Memory.Read2Byte(this.game.Memory.ReadAddress(new int[]
								{
									bank.Address + 20,
									80
								}));
								bank.X = this.game.Memory.Read2Byte(bank.Address + 20, 82);
								bank.Y = this.game.Memory.Read2Byte(bank.Address + 20, 84);
							}
						}
						bank.Lvl = this.game.Memory.Read(bank.Address + 40, 44);
						if (!(TINHKIEM.VietLien(bank.TypeName) == "daocunhiemvu"))
						{
							list.Add(bank);
						}
					}
				}
				return list;
			}
		}

		// Token: 0x040003BC RID: 956
		private Game game;

		// Token: 0x040003BD RID: 957
		public int Address;

		// Token: 0x040003BE RID: 958
		public int Class;

		// Token: 0x040003BF RID: 959
		public int PacketId;

		// Token: 0x040003C0 RID: 960
		public string Name;

		// Token: 0x040003C1 RID: 961
		public int Count;

		// Token: 0x040003C2 RID: 962
		public int Index;

		// Token: 0x040003C3 RID: 963
		public string TypeName;

		// Token: 0x040003C4 RID: 964
		public int Star;

		// Token: 0x040003C5 RID: 965
		public int Line;

		// Token: 0x040003C6 RID: 966
		public string Type;

		// Token: 0x040003C7 RID: 967
		public int MapId = -1;

		// Token: 0x040003C8 RID: 968
		public int X;

		// Token: 0x040003C9 RID: 969
		public int Y;

		// Token: 0x040003CA RID: 970
		public int InfoBase;
	}
}
