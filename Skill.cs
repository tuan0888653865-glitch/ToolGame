using System;
using System.Collections.Generic;
using TinhKiemAuto.Controllers;

namespace TinhKiemAuto
{
	// Token: 0x020000E2 RID: 226
	public class Skill
	{
		// Token: 0x170002BF RID: 703
		// (get) Token: 0x06000BC6 RID: 3014 RVA: 0x0004B1FD File Offset: 0x000493FD
		// (set) Token: 0x06000BC7 RID: 3015 RVA: 0x0004B205 File Offset: 0x00049405
		public bool UsePK { get; set; }

		// Token: 0x170002C0 RID: 704
		// (get) Token: 0x06000BC8 RID: 3016 RVA: 0x0004B20E File Offset: 0x0004940E
		// (set) Token: 0x06000BC9 RID: 3017 RVA: 0x0004B216 File Offset: 0x00049416
		public bool UserBuff { get; set; }

		// Token: 0x170002C1 RID: 705
		// (get) Token: 0x06000BCA RID: 3018 RVA: 0x0004B21F File Offset: 0x0004941F
		// (set) Token: 0x06000BCB RID: 3019 RVA: 0x0004B227 File Offset: 0x00049427
		public bool Use
		{
			get
			{
				return this.use;
			}
			set
			{
				this.use = value;
			}
		}

		// Token: 0x06000BCC RID: 3020 RVA: 0x0004B230 File Offset: 0x00049430
		public static bool IsBuffSkill(int ID)
		{
			SkillModel skillByID = SkillData.GetSkillByID(ID);
			return skillByID != null && !(skillByID.skillTargetType == "Enemy");
		}

		// Token: 0x06000BCD RID: 3021 RVA: 0x0004B25E File Offset: 0x0004945E
		public Skill(Game game)
		{
			this.game = game;
		}

		// Token: 0x06000BCE RID: 3022 RVA: 0x0004B278 File Offset: 0x00049478
		public static bool IsBase(int id)
		{
			return "-311-341-371-281-401-431-461-491-521-760-2900-;".Contains("-" + id.ToString() + "-");
		}

		// Token: 0x06000BCF RID: 3023 RVA: 0x0004B29A File Offset: 0x0004949A
		public static bool IsBand(int id)
		{
			return "-0-1-22-21-35-34-37-245-;".Contains("-" + id.ToString() + "-");
		}

		// Token: 0x06000BD0 RID: 3024 RVA: 0x0004B2BC File Offset: 0x000494BC
		public override string ToString()
		{
			return string.Concat(new string[]
			{
				"Address: ",
				this.Address.ToString("X8"),
				"\r\nDelayOffset: ",
				this.DelayOffset.ToString("X8"),
				"\r\n\r\n\r\n"
			});
		}

		// Token: 0x06000BD1 RID: 3025 RVA: 0x0004B314 File Offset: 0x00049514
		public static List<Skill> Enum(Game game)
		{
			List<Skill> list = new List<Skill>();
			new List<int>();
			int[] charBase = game.Address.CharBase;
			Array.Resize<int>(ref charBase, charBase.Length + 2);
			if (game.Address.GameType == 1)
			{
				charBase[charBase.Length - 2] = 10344;
			}
			else if (game.Address.GameType == 2)
			{
				charBase[charBase.Length - 2] = 1980;
			}
			else
			{
				charBase[charBase.Length - 2] = 2768;
			}
			charBase[charBase.Length - 1] = 4;
			int address = game.Memory.Read(charBase);
			List<int> list2 = new List<int>();
			try
			{
				Skill.NextSkill(address, list2, game);
			}
			catch
			{
			}
			foreach (int num in list2)
			{
				Skill skill = new Skill(game);
				skill.Address = num;
				skill.PacketId = game.Memory.Read(num + 12);
				skill.DelayOffset = game.Memory.Read(num + 16 + 4, 64) * 12;
				if (skill.PacketId < 4096)
				{
					list.Add(skill);
				}
			}
			if (list.Count > 100)
			{
				list.Clear();
			}
			return list;
		}

		// Token: 0x06000BD2 RID: 3026 RVA: 0x0004B468 File Offset: 0x00049668
		public static void NextSkill(int address, List<int> listAddress, Game game)
		{
			if (listAddress.Contains(address))
			{
				return;
			}
			if (listAddress.Count > 110)
			{
				return;
			}
			listAddress.Add(address);
			Skill.NextSkill(game.Memory.Read(address), listAddress, game);
			Skill.NextSkill(game.Memory.Read(address + 4), listAddress, game);
			Skill.NextSkill(game.Memory.Read(address + 8), listAddress, game);
		}

		// Token: 0x040008DA RID: 2266
		public int Address;

		// Token: 0x040008DB RID: 2267
		public int PacketId;

		// Token: 0x040008DC RID: 2268
		public int DelayOffset;

		// Token: 0x040008DD RID: 2269
		public string Name = "";

		// Token: 0x040008DE RID: 2270
		private bool use;

		// Token: 0x040008DF RID: 2271
		public Game game;
	}
}
