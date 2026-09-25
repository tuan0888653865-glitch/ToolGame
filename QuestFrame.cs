using System;
using System.Collections.Generic;

namespace TinhKiemAuto
{
	// Token: 0x020000D0 RID: 208
	public class QuestFrame
	{
		// Token: 0x06000B01 RID: 2817 RVA: 0x00048C69 File Offset: 0x00046E69
		public QuestFrame(Game game)
		{
			this.game = game;
		}

		// Token: 0x06000B02 RID: 2818 RVA: 0x00048C78 File Offset: 0x00046E78
		public void ClickAll()
		{
			foreach (QuestFrame dialog in QuestFrame.Enum(this.game))
			{
				this.game.QuestFrameOptionClicked(dialog);
			}
		}

		// Token: 0x06000B03 RID: 2819 RVA: 0x00048CD8 File Offset: 0x00046ED8
		public bool Click(int option1, int option2)
		{
			foreach (QuestFrame questFrame in QuestFrame.Enum(this.game))
			{
				if (questFrame.StrOptionExtra1 == option1 && questFrame.StrOptionExtra2 == option2)
				{
					this.game.QuestFrameOptionClicked(questFrame);
					return true;
				}
			}
			return false;
		}

		// Token: 0x06000B04 RID: 2820 RVA: 0x00048D50 File Offset: 0x00046F50
		public bool Click(string name)
		{
			foreach (QuestFrame questFrame in QuestFrame.Enum(this.game))
			{
				if (questFrame.Name == name)
				{
					this.game.QuestFrameOptionClicked(questFrame);
					return true;
				}
			}
			return false;
		}

		// Token: 0x06000B05 RID: 2821 RVA: 0x00048DC4 File Offset: 0x00046FC4
		public void Close()
		{
			this.game.PostMessage(0, 122);
		}

		// Token: 0x17000276 RID: 630
		// (get) Token: 0x06000B06 RID: 2822 RVA: 0x00048DD4 File Offset: 0x00046FD4
		public string ClearName
		{
			get
			{
				return TINHKIEM.VietLien(this.Name);
			}
		}

		// Token: 0x06000B07 RID: 2823 RVA: 0x00048DE1 File Offset: 0x00046FE1
		public static int GetId(Game game)
		{
			return game.Memory.Read(game.Address.DialogBase[0], 88);
		}

		// Token: 0x06000B08 RID: 2824 RVA: 0x00048E00 File Offset: 0x00047000
		public override string ToString()
		{
			return string.Concat(new string[]
			{
				string.Empty,
				"Address: ",
				this.Address.ToString("X8"),
				"\r\nName: ",
				this.Name,
				"\r\nStrOptionExtra1: ",
				this.StrOptionExtra1.ToString(),
				"\r\nStrOptionExtra2: ",
				this.StrOptionExtra2.ToString(),
				"\r\nMD: ",
				this.MD
			});
		}

		// Token: 0x17000277 RID: 631
		// (get) Token: 0x06000B09 RID: 2825 RVA: 0x00048E8C File Offset: 0x0004708C
		public string MD
		{
			get
			{
				return TINHKIEM.Hasher.MD5(this.StrOptionExtra1.ToString() + this.StrOptionExtra2.ToString()).Substring(0, 3);
			}
		}

		// Token: 0x06000B0A RID: 2826 RVA: 0x00048EB8 File Offset: 0x000470B8
		public static void ClickPhuBanMonPhai(Game game)
		{
			foreach (QuestFrame questFrame in QuestFrame.Enum(game))
			{
				if (questFrame.IsClickPhuBanMonPhai)
				{
					game.QuestFrameOptionClicked(questFrame);
					break;
				}
			}
		}

		// Token: 0x06000B0B RID: 2827 RVA: 0x00048F18 File Offset: 0x00047118
		public static void ClickOut(Game game)
		{
			foreach (QuestFrame questFrame in QuestFrame.Enum(game))
			{
				if (((questFrame.StrOptionExtra1 == 119001 || questFrame.StrOptionExtra1 == 118001) && questFrame.StrOptionExtra2 == 1) || questFrame.StrOptionExtra1 == 119005 || questFrame.StrOptionExtra1 == 118012 || questFrame.StrOptionExtra1 == 2108)
				{
					game.QuestFrameOptionClicked(questFrame);
					break;
				}
			}
		}

		// Token: 0x17000278 RID: 632
		// (get) Token: 0x06000B0C RID: 2828 RVA: 0x00048FB8 File Offset: 0x000471B8
		public bool IsClickPhuBanMonPhai
		{
			get
			{
				return this.StrOptionExtra1 == 200005 || this.StrOptionExtra1 == 118001 || this.StrOptionExtra1 == 119005 || this.StrOptionExtra1 == 119001 || this.StrOptionExtra1 == 200001 || this.StrOptionExtra1 == 13035 || this.StrOptionExtra1 == 9044 || this.StrOptionExtra1 == 10051 || this.StrOptionExtra1 == 16035 || this.StrOptionExtra1 == 14035 || this.StrOptionExtra1 == 9035 || this.StrOptionExtra1 == 17035 || this.StrOptionExtra1 == 15035 || this.StrOptionExtra1 == 12035 || this.StrOptionExtra1 == 11035 || this.StrOptionExtra1 == 10035;
			}
		}

		// Token: 0x06000B0D RID: 2829 RVA: 0x000490AC File Offset: 0x000472AC
		public static List<QuestFrame> Enum(Game game)
		{
			List<QuestFrame> list = new List<QuestFrame>();
			int num = game.Memory.Read(game.Address.DialogBase) + 8;
			QuestFrame questFrame = new QuestFrame(game);
			questFrame.Address = num;
			questFrame.StrOptionExtra1 = game.Memory.Read(questFrame.Address + 272);
			questFrame.StrOptionExtra2 = game.Memory.Read(questFrame.Address + 8);
			questFrame.Name = game.Memory.ReadString(questFrame.Address + 14);
			list.Add(questFrame);
			for (int i = 1; i < 12; i++)
			{
				int num2 = num + 280 * i;
				questFrame = new QuestFrame(game);
				questFrame.Address = num2;
				questFrame.Id = i;
				questFrame.StrOptionExtra1 = game.Memory.Read(questFrame.Address + 272);
				questFrame.StrOptionExtra2 = game.Memory.Read(questFrame.Address + 8);
				questFrame.Name = game.Memory.ReadString(num2 + 14).Trim();
				if (questFrame.StrOptionExtra1 > 0 && questFrame.Name.Trim() != "")
				{
					list.Add(questFrame);
				}
			}
			return list;
		}

		// Token: 0x06000B0E RID: 2830 RVA: 0x000491EC File Offset: 0x000473EC
		public static int GetCount(Game game)
		{
			int num = 0;
			using (List<QuestFrame>.Enumerator enumerator = QuestFrame.Enum(game).GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (enumerator.Current.Name.Trim() != "")
					{
						num++;
					}
				}
			}
			return num;
		}

		// Token: 0x06000B0F RID: 2831 RVA: 0x00049254 File Offset: 0x00047454
		public static string All(Game game)
		{
			string text = "";
			foreach (QuestFrame questFrame in QuestFrame.Enum(game))
			{
				text += questFrame.Name;
			}
			return text;
		}

		// Token: 0x04000896 RID: 2198
		private Game game;

		// Token: 0x04000897 RID: 2199
		public int Address;

		// Token: 0x04000898 RID: 2200
		public int Id;

		// Token: 0x04000899 RID: 2201
		public int StrOptionExtra1;

		// Token: 0x0400089A RID: 2202
		public int StrOptionExtra2;

		// Token: 0x0400089B RID: 2203
		public string Name;
	}
}
