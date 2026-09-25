using System;

namespace TinhKiemAuto.Models
{
	// Token: 0x02000134 RID: 308
	public class Option
	{
		// Token: 0x17000377 RID: 887
		// (get) Token: 0x06000FD3 RID: 4051 RVA: 0x00075602 File Offset: 0x00073802
		// (set) Token: 0x06000FD4 RID: 4052 RVA: 0x00075609 File Offset: 0x00073809
		public static bool SetSafeTime { get; set; }

		// Token: 0x17000378 RID: 888
		// (get) Token: 0x06000FD5 RID: 4053 RVA: 0x00075611 File Offset: 0x00073811
		// (set) Token: 0x06000FD6 RID: 4054 RVA: 0x00075618 File Offset: 0x00073818
		public static bool IsDead { get; set; }

		// Token: 0x17000379 RID: 889
		// (get) Token: 0x06000FD7 RID: 4055 RVA: 0x00075620 File Offset: 0x00073820
		// (set) Token: 0x06000FD8 RID: 4056 RVA: 0x00075627 File Offset: 0x00073827
		public static bool IsHoTro { get; set; }

		// Token: 0x1700037A RID: 890
		// (get) Token: 0x06000FD9 RID: 4057 RVA: 0x0007562F File Offset: 0x0007382F
		// (set) Token: 0x06000FDA RID: 4058 RVA: 0x00075636 File Offset: 0x00073836
		public static bool NotDongMon { get; set; }

		// Token: 0x1700037B RID: 891
		// (get) Token: 0x06000FDB RID: 4059 RVA: 0x0007563E File Offset: 0x0007383E
		public static int Delay
		{
			get
			{
				return 50;
			}
		}

		// Token: 0x1700037C RID: 892
		// (get) Token: 0x06000FDC RID: 4060 RVA: 0x00075642 File Offset: 0x00073842
		// (set) Token: 0x06000FDD RID: 4061 RVA: 0x00075649 File Offset: 0x00073849
		public static bool AutoPoint { get; set; }

		// Token: 0x1700037D RID: 893
		// (get) Token: 0x06000FDE RID: 4062 RVA: 0x00075651 File Offset: 0x00073851
		// (set) Token: 0x06000FDF RID: 4063 RVA: 0x00075658 File Offset: 0x00073858
		public static bool IsBank { get; set; }

		// Token: 0x1700037E RID: 894
		// (get) Token: 0x06000FE0 RID: 4064 RVA: 0x00075660 File Offset: 0x00073860
		// (set) Token: 0x06000FE1 RID: 4065 RVA: 0x00075668 File Offset: 0x00073868
		private bool IsPass2 { get; set; }

		// Token: 0x04000CEA RID: 3306
		public static int MaptriLieuIndex = 0;

		// Token: 0x04000CEB RID: 3307
		public static int MapBanDoIndex = 0;

		// Token: 0x04000CF2 RID: 3314
		public static bool PutBase = false;

		// Token: 0x04000CF3 RID: 3315
		public static bool AlarmChat = false;

		// Token: 0x04000CF4 RID: 3316
		public static int HideTime = 60;
	}
}
