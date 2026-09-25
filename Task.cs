using System;
using System.Collections.Generic;

namespace TinhKiemAuto
{
	// Token: 0x020000E6 RID: 230
	public class Task
	{
		// Token: 0x06000BDC RID: 3036 RVA: 0x0004BC81 File Offset: 0x00049E81
		public Task(Game game)
		{
			this.game = game;
		}

		// Token: 0x170002C2 RID: 706
		// (get) Token: 0x06000BDD RID: 3037 RVA: 0x0004BC90 File Offset: 0x00049E90
		// (set) Token: 0x06000BDE RID: 3038 RVA: 0x0004BC98 File Offset: 0x00049E98
		public int Complete { get; set; }

		// Token: 0x170002C3 RID: 707
		// (get) Token: 0x06000BDF RID: 3039 RVA: 0x0004BCA1 File Offset: 0x00049EA1
		public string ClearName
		{
			get
			{
				return TINHKIEM.VietLien(this.Name);
			}
		}

		// Token: 0x06000BE0 RID: 3040 RVA: 0x0004BCB0 File Offset: 0x00049EB0
		public void SetComplete()
		{
			foreach (TaskInfo taskInfo in TaskInfo.Enum(this.game))
			{
				if (taskInfo.Id == this.Id)
				{
					taskInfo.SetTrangThai(256);
					break;
				}
			}
		}

		// Token: 0x170002C4 RID: 708
		// (get) Token: 0x06000BE1 RID: 3041 RVA: 0x0004BD1C File Offset: 0x00049F1C
		// (set) Token: 0x06000BE2 RID: 3042 RVA: 0x0004BD24 File Offset: 0x00049F24
		public int TaskInfoAddress { get; set; }

		// Token: 0x170002C5 RID: 709
		// (get) Token: 0x06000BE3 RID: 3043 RVA: 0x0004BD2D File Offset: 0x00049F2D
		public int CountEx1
		{
			get
			{
				return this.game.Memory.Read2Byte(this.TaskInfoAddress + 54);
			}
		}

		// Token: 0x170002C6 RID: 710
		// (get) Token: 0x06000BE4 RID: 3044 RVA: 0x0004BD48 File Offset: 0x00049F48
		public int CountEx2
		{
			get
			{
				return this.game.Memory.Read2Byte(this.TaskInfoAddress + 58);
			}
		}

		// Token: 0x170002C7 RID: 711
		// (get) Token: 0x06000BE5 RID: 3045 RVA: 0x0004BD63 File Offset: 0x00049F63
		public int CountEx3
		{
			get
			{
				return this.game.Memory.Read2Byte(this.TaskInfoAddress + 62);
			}
		}

		// Token: 0x170002C8 RID: 712
		// (get) Token: 0x06000BE6 RID: 3046 RVA: 0x0004BD7E File Offset: 0x00049F7E
		// (set) Token: 0x06000BE7 RID: 3047 RVA: 0x0004BD99 File Offset: 0x00049F99
		public int Count1
		{
			get
			{
				return this.game.Memory.Read(this.TaskInfoAddress + 13);
			}
			set
			{
				this.game.Memory.Write(this.TaskInfoAddress + 13, 1);
			}
		}

		// Token: 0x170002C9 RID: 713
		// (get) Token: 0x06000BE8 RID: 3048 RVA: 0x0004BDB5 File Offset: 0x00049FB5
		// (set) Token: 0x06000BE9 RID: 3049 RVA: 0x0004BDD2 File Offset: 0x00049FD2
		public int Count2
		{
			get
			{
				return this.game.Memory.Read(this.TaskInfoAddress + 13 + 4);
			}
			set
			{
				this.game.Memory.Write(this.TaskInfoAddress + 13 + 4, 1);
			}
		}

		// Token: 0x170002CA RID: 714
		// (get) Token: 0x06000BEA RID: 3050 RVA: 0x0004BDF0 File Offset: 0x00049FF0
		// (set) Token: 0x06000BEB RID: 3051 RVA: 0x0004BE0D File Offset: 0x0004A00D
		public int Count3
		{
			get
			{
				return this.game.Memory.Read(this.TaskInfoAddress + 13 + 8);
			}
			set
			{
				this.game.Memory.Write(this.TaskInfoAddress + 13 + 8, 1);
			}
		}

		// Token: 0x06000BEC RID: 3052 RVA: 0x0004BE2C File Offset: 0x0004A02C
		public override string ToString()
		{
			string text = string.Empty;
			text = text + "ID: " + this.Id.ToString("X8");
			text += "\r\n";
			text = text + "Address: " + this.Address.ToString("X8");
			text += "\r\n";
			text = text + "Name: " + this.Name;
			text += "\r\n";
			text = text + "MucTieu: " + this.MucTieu;
			text += "\r\n";
			text = text + "Lvl: " + this.Lvl.ToString();
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
			text = text + "NPC: " + this.NPCNhanTask;
			text += "\r\n";
			text = text + "TaksInfoAddress: " + this.TaskInfoAddress.ToString("X8");
			text += "\r\n";
			text = string.Concat(new object[]
			{
				text,
				"Count: ",
				this.Count1,
				this.Count2,
				this.Count3
			});
			text += "\r\n";
			text = string.Concat(new object[]
			{
				text,
				"CountEx: ",
				this.CountEx1,
				this.CountEx2,
				this.CountEx3
			});
			text += "\r\n";
			if (this.Completed)
			{
				text += "Completed";
			}
			else
			{
				text += "Doing";
			}
			text += "\r\n";
			return text + "Complete: " + this.Complete.ToString();
		}

		// Token: 0x06000BED RID: 3053 RVA: 0x0004C064 File Offset: 0x0004A264
		public static bool Have(Game game, string name)
		{
			game.LUA.OpenWindowMissionTrack();
			foreach (int num in Task.EnumTask(game.Address.TaskBase, game))
			{
				if (new Task(game)
				{
					Name = game.Memory._ReadString(num + 224)
				}.Name.Contains(name))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06000BEE RID: 3054 RVA: 0x0004C0F8 File Offset: 0x0004A2F8
		public static List<Task> Enum(Game game)
		{
			game.LUA.OpenWindowMissionTrack();
			List<int> list = Task.EnumTask(game.Address.TaskBase, game);
			List<Task> list2 = new List<Task>();
			List<TaskInfo> list3 = TaskInfo.Enum(game);
			foreach (int num in list)
			{
				Task task = new Task(game);
				task.Address = num;
				task.Lvl = game.Memory.Read(num + 20);
				task.Id = game.Memory.Read(num + 12);
				task.X = game.Memory.Read(num + 44);
				task.Y = game.Memory.Read(num + 48);
				task.Name = game.Memory._ReadString(num + 224);
				task.NPCNhanTask = game.Memory._ReadString(num + 60);
				task.MucTieu = game.Memory._ReadString(num + 200);
				if (task.Id > 0)
				{
					list2.Add(task);
					foreach (TaskInfo taskInfo in list3)
					{
						if (taskInfo.Id == task.Id)
						{
							if (taskInfo.TrangThai >= 256)
							{
								task.Completed = true;
							}
							task.Complete = taskInfo.TrangThai;
							task.TaskInfoAddress = taskInfo.Address;
						}
					}
				}
			}
			return list2;
		}

		// Token: 0x06000BEF RID: 3055 RVA: 0x0004C2C4 File Offset: 0x0004A4C4
		public static List<int> EnumTask(int address, Game game)
		{
			List<int> list = new List<int>();
			Task.NextTask(address, list, game);
			return list;
		}

		// Token: 0x06000BF0 RID: 3056 RVA: 0x0004C2E0 File Offset: 0x0004A4E0
		public static List<int> EnumTask(int[] addresses, Game game)
		{
			int num = game.Memory.Read(addresses);
			if (num > 0)
			{
				return Task.EnumTask(num, game);
			}
			return new List<int>();
		}

		// Token: 0x06000BF1 RID: 3057 RVA: 0x0004C30C File Offset: 0x0004A50C
		private static void NextTask(int address, List<int> listAddress, Game game)
		{
			if (listAddress.Count > 1000)
			{
				return;
			}
			if (!listAddress.Contains(address))
			{
				listAddress.Add(address);
				int num = game.Memory.Read(address);
				if (num > 0)
				{
					Task.NextTask(num, listAddress, game);
				}
				int num2 = game.Memory.Read(address + 4);
				if (num2 > 0)
				{
					Task.NextTask(num2, listAddress, game);
				}
				int num3 = game.Memory.Read(address + 8);
				if (num3 > 0)
				{
					Task.NextTask(num3, listAddress, game);
				}
			}
		}

		// Token: 0x0400091E RID: 2334
		public int Address;

		// Token: 0x0400091F RID: 2335
		public int Id;

		// Token: 0x04000920 RID: 2336
		public string Name;

		// Token: 0x04000921 RID: 2337
		public string MucTieu;

		// Token: 0x04000922 RID: 2338
		public int Lvl;

		// Token: 0x04000923 RID: 2339
		public int X;

		// Token: 0x04000924 RID: 2340
		public int Y;

		// Token: 0x04000925 RID: 2341
		public string NPCNhanTask;

		// Token: 0x04000926 RID: 2342
		public bool Completed;

		// Token: 0x04000927 RID: 2343
		private Game game;
	}
}
