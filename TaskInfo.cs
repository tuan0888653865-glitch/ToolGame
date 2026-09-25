using System;
using System.Collections.Generic;

namespace TinhKiemAuto
{
	// Token: 0x020000E7 RID: 231
	internal class TaskInfo
	{
		// Token: 0x06000BF2 RID: 3058 RVA: 0x0004C386 File Offset: 0x0004A586
		public TaskInfo(Game game)
		{
			this.game = game;
		}

		// Token: 0x170002CB RID: 715
		// (get) Token: 0x06000BF3 RID: 3059 RVA: 0x0004C395 File Offset: 0x0004A595
		// (set) Token: 0x06000BF4 RID: 3060 RVA: 0x0004C39D File Offset: 0x0004A59D
		public int TrangThai { get; set; }

		// Token: 0x06000BF5 RID: 3061 RVA: 0x0004C3A6 File Offset: 0x0004A5A6
		public void SetTrangThai(int value)
		{
			this.game.Memory.Write(this.Address + 8, value);
		}

		// Token: 0x170002CC RID: 716
		// (get) Token: 0x06000BF6 RID: 3062 RVA: 0x0004C3C1 File Offset: 0x0004A5C1
		public string Info
		{
			get
			{
				return string.Empty + "Address: " + this.Address.ToString("X8");
			}
		}

		// Token: 0x06000BF7 RID: 3063 RVA: 0x0004C3E4 File Offset: 0x0004A5E4
		public static List<TaskInfo> Enum(Game game)
		{
			List<TaskInfo> list = new List<TaskInfo>();
			int num = game.Memory.Read(game.Address.TaskInfoBase);
			for (int i = 0; i < 80; i++)
			{
				if (game.Memory.Read(num + 5 + i * 41) != 0)
				{
					TaskInfo taskInfo = new TaskInfo(game);
					taskInfo.Address = num + 5 + i * 41;
					taskInfo.Class = game.Memory.Read(taskInfo.Address);
					taskInfo.Id = game.Memory.Read(taskInfo.Address + 4);
					taskInfo.TrangThai = game.Memory.Read(taskInfo.Address + 8);
					if (taskInfo.Id > 0)
					{
						list.Add(taskInfo);
					}
				}
			}
			return list;
		}

		// Token: 0x04000929 RID: 2345
		public int Address;

		// Token: 0x0400092A RID: 2346
		public int Class;

		// Token: 0x0400092B RID: 2347
		public int Id;

		// Token: 0x0400092C RID: 2348
		private Game game;
	}
}
