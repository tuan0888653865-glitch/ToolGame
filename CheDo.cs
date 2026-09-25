using System;
using ProtoBuf;

namespace TinhKiemAuto.AutoControl
{
	// Token: 0x02000141 RID: 321
	[ProtoContract]
	public class CheDo
	{
		// Token: 0x17000392 RID: 914
		// (get) Token: 0x06001049 RID: 4169 RVA: 0x000768F6 File Offset: 0x00074AF6
		// (set) Token: 0x0600104A RID: 4170 RVA: 0x000768FE File Offset: 0x00074AFE
		[ProtoMember(1)]
		public bool IsCheDo { get; set; }

		// Token: 0x17000393 RID: 915
		// (get) Token: 0x0600104B RID: 4171 RVA: 0x00076907 File Offset: 0x00074B07
		// (set) Token: 0x0600104C RID: 4172 RVA: 0x0007690F File Offset: 0x00074B0F
		[ProtoMember(2)]
		public bool IsMienPhiNguyenLieu { get; set; }

		// Token: 0x17000394 RID: 916
		// (get) Token: 0x0600104D RID: 4173 RVA: 0x00076918 File Offset: 0x00074B18
		// (set) Token: 0x0600104E RID: 4174 RVA: 0x00076920 File Offset: 0x00074B20
		[ProtoMember(3)]
		public int CheLoai { get; set; }

		// Token: 0x17000395 RID: 917
		// (get) Token: 0x0600104F RID: 4175 RVA: 0x00076929 File Offset: 0x00074B29
		// (set) Token: 0x06001050 RID: 4176 RVA: 0x00076931 File Offset: 0x00074B31
		[ProtoMember(4)]
		public int CheCap { get; set; }

		// Token: 0x17000396 RID: 918
		// (get) Token: 0x06001051 RID: 4177 RVA: 0x0007693A File Offset: 0x00074B3A
		// (set) Token: 0x06001052 RID: 4178 RVA: 0x00076942 File Offset: 0x00074B42
		[ProtoMember(5)]
		public int TotalChe { get; set; }

		// Token: 0x17000397 RID: 919
		// (get) Token: 0x06001053 RID: 4179 RVA: 0x0007694B File Offset: 0x00074B4B
		// (set) Token: 0x06001054 RID: 4180 RVA: 0x00076953 File Offset: 0x00074B53
		[ProtoMember(6)]
		public int TongNhan { get; set; }

		// Token: 0x17000398 RID: 920
		// (get) Token: 0x06001055 RID: 4181 RVA: 0x0007695C File Offset: 0x00074B5C
		// (set) Token: 0x06001056 RID: 4182 RVA: 0x00076964 File Offset: 0x00074B64
		[ProtoMember(7)]
		public int CheDiem { get; set; }

		// Token: 0x17000399 RID: 921
		// (get) Token: 0x06001057 RID: 4183 RVA: 0x0007696D File Offset: 0x00074B6D
		// (set) Token: 0x06001058 RID: 4184 RVA: 0x00076975 File Offset: 0x00074B75
		[ProtoMember(8)]
		public int CheDong { get; set; }

		// Token: 0x1700039A RID: 922
		// (get) Token: 0x06001059 RID: 4185 RVA: 0x0007697E File Offset: 0x00074B7E
		// (set) Token: 0x0600105A RID: 4186 RVA: 0x00076986 File Offset: 0x00074B86
		[ProtoMember(9)]
		public int CheSao { get; set; }
	}
}
