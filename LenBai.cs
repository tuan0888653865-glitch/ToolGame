using System;
using ProtoBuf;

namespace TinhKiemAuto.AutoControl
{
	// Token: 0x02000142 RID: 322
	[ProtoContract]
	public class LenBai
	{
		// Token: 0x1700039B RID: 923
		// (get) Token: 0x0600105C RID: 4188 RVA: 0x0007698F File Offset: 0x00074B8F
		// (set) Token: 0x0600105D RID: 4189 RVA: 0x00076997 File Offset: 0x00074B97
		[ProtoMember(1)]
		public int MapID { get; set; }

		// Token: 0x1700039C RID: 924
		// (get) Token: 0x0600105E RID: 4190 RVA: 0x000769A0 File Offset: 0x00074BA0
		// (set) Token: 0x0600105F RID: 4191 RVA: 0x000769A8 File Offset: 0x00074BA8
		[ProtoMember(2)]
		public string MapName { get; set; }

		// Token: 0x1700039D RID: 925
		// (get) Token: 0x06001060 RID: 4192 RVA: 0x000769B1 File Offset: 0x00074BB1
		// (set) Token: 0x06001061 RID: 4193 RVA: 0x000769B9 File Offset: 0x00074BB9
		[ProtoMember(3)]
		public int PosX { get; set; }

		// Token: 0x1700039E RID: 926
		// (get) Token: 0x06001062 RID: 4194 RVA: 0x000769C2 File Offset: 0x00074BC2
		// (set) Token: 0x06001063 RID: 4195 RVA: 0x000769CA File Offset: 0x00074BCA
		[ProtoMember(4)]
		public int PosY { get; set; }
	}
}
