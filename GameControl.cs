using System;
using System.Collections.Generic;

namespace TinhKiemAuto
{
	// Token: 0x02000093 RID: 147
	public class GameControl
	{
		// Token: 0x170001FA RID: 506
		// (get) Token: 0x060008FB RID: 2299 RVA: 0x0003D0D4 File Offset: 0x0003B2D4
		public bool IsSkill
		{
			get
			{
				return !this.Type.Contains("FightSkillXinShou_12") && (this.Type.Contains("FightSkill") || this.Type.Contains("FabaoSkill") || this.Type.Contains("MiJiSkill") || this.Type.Contains("fuqiskill") || this.Type.Contains("Shoes2_5") || this.Type.Contains("RideHeader1_1") || this.Type.Contains("MenpaiLiveSkill2_7") || (this.Type.Contains("WuhunSkill") && this.PacketId > 100) || this.Type.Contains("TaskTools2_13") || this.Type.Contains("PetSkill2_4") || this.Type.Contains("CommonLiveSkill2_2") || this.Type.Contains("CircularTaskTool43_2") || this.Type.Contains("Shoes2_4") || this.Type.Contains("TaskTools4_1"));
			}
		}

		// Token: 0x060008FC RID: 2300 RVA: 0x0003D214 File Offset: 0x0003B414
		public override string ToString()
		{
			return string.Concat(new string[]
			{
				string.Empty,
				"Address: ",
				this.Address.ToString("X8"),
				"\r\nId: ",
				this.Id.ToString(),
				"\r\nObject: ",
				this.Object.ToString("X8"),
				"\r\nClass: ",
				this.Class.ToString("X8"),
				"\r\nType: ",
				this.Type,
				"\r\nPacketId: ",
				this.PacketId.ToString()
			});
		}

		// Token: 0x060008FD RID: 2301 RVA: 0x0003D2C8 File Offset: 0x0003B4C8
		public static List<GameControl> Enum(Game game)
		{
			List<GameControl> list = new List<GameControl>();
			foreach (int num in GameControl.EnumGameControl(game.Address.ActionBase, game))
			{
				GameControl gameControl = new GameControl();
				gameControl.Address = num;
				gameControl.Object = game.Memory.Read(num + 16);
				gameControl.Class = game.Memory.Read(gameControl.Object);
				gameControl.Id = game.Memory.Read(gameControl.Object + 4);
				gameControl.Name = game.Memory._ReadString(gameControl.Object + 12);
				gameControl.Type = game.Memory._ReadString(gameControl.Object + 40);
				gameControl.PacketId = game.Memory.Read(gameControl.Object + 92);
				list.Add(gameControl);
			}
			return list;
		}

		// Token: 0x060008FE RID: 2302 RVA: 0x0003D3D4 File Offset: 0x0003B5D4
		private static HashSet<int> EnumGameControl(int address, Game game)
		{
			HashSet<int> hashSet = new HashSet<int>();
			GameControl.NextGameControl(address, hashSet, game);
			hashSet.Remove(address);
			return hashSet;
		}

		// Token: 0x060008FF RID: 2303 RVA: 0x0003D3F8 File Offset: 0x0003B5F8
		public static HashSet<int> EnumGameControl(int[] addresses, Game game)
		{
			int num = game.Memory.Read(addresses);
			if (num > 0)
			{
				return GameControl.EnumGameControl(num, game);
			}
			return new HashSet<int>();
		}

		// Token: 0x06000900 RID: 2304 RVA: 0x0003D424 File Offset: 0x0003B624
		private static void NextGameControl(int address, HashSet<int> hash, Game game)
		{
			if (hash.Count > 10000)
			{
				return;
			}
			if (!hash.Contains(address))
			{
				hash.Add(address);
				int num = game.Memory.Read(address);
				if (num > 0)
				{
					GameControl.NextGameControl(num, hash, game);
				}
				int num2 = game.Memory.Read(address + 4);
				if (num2 > 0)
				{
					GameControl.NextGameControl(num2, hash, game);
				}
				int num3 = game.Memory.Read(address + 8);
				if (num3 > 0)
				{
					GameControl.NextGameControl(num3, hash, game);
				}
			}
		}

		// Token: 0x0400061F RID: 1567
		public int Address;

		// Token: 0x04000620 RID: 1568
		public int Object;

		// Token: 0x04000621 RID: 1569
		public int Class;

		// Token: 0x04000622 RID: 1570
		public int Id;

		// Token: 0x04000623 RID: 1571
		public string Name;

		// Token: 0x04000624 RID: 1572
		public string Type;

		// Token: 0x04000625 RID: 1573
		public int PacketId;
	}
}
