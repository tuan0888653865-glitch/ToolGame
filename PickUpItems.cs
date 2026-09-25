using System;
using ProtoBuf;

namespace TinhKiemAuto.AutoControl
{
	// Token: 0x02000147 RID: 327
	[ProtoContract]
	public class PickUpItems
	{
		// Token: 0x170003B8 RID: 952
		// (get) Token: 0x0600109B RID: 4251 RVA: 0x00076B7C File Offset: 0x00074D7C
		// (set) Token: 0x0600109C RID: 4252 RVA: 0x00076B84 File Offset: 0x00074D84
		[ProtoMember(1)]
		public bool DestroyItem { get; set; }

		// Token: 0x170003B9 RID: 953
		// (get) Token: 0x0600109D RID: 4253 RVA: 0x00076B8D File Offset: 0x00074D8D
		// (set) Token: 0x0600109E RID: 4254 RVA: 0x00076B95 File Offset: 0x00074D95
		[ProtoMember(2)]
		public bool SellItem { get; set; }

		// Token: 0x170003BA RID: 954
		// (get) Token: 0x0600109F RID: 4255 RVA: 0x00076B9E File Offset: 0x00074D9E
		// (set) Token: 0x060010A0 RID: 4256 RVA: 0x00076BA6 File Offset: 0x00074DA6
		[ProtoMember(3)]
		public int PickUpRadius { get; set; }

		// Token: 0x170003BB RID: 955
		// (get) Token: 0x060010A1 RID: 4257 RVA: 0x00076BAF File Offset: 0x00074DAF
		// (set) Token: 0x060010A2 RID: 4258 RVA: 0x00076BB7 File Offset: 0x00074DB7
		[ProtoMember(4)]
		public bool PutToBank { get; set; }

		// Token: 0x170003BC RID: 956
		// (get) Token: 0x060010A3 RID: 4259 RVA: 0x00076BC0 File Offset: 0x00074DC0
		// (set) Token: 0x060010A4 RID: 4260 RVA: 0x00076BC8 File Offset: 0x00074DC8
		[ProtoMember(5)]
		public bool ThrowTrash { get; set; }

		// Token: 0x170003BD RID: 957
		// (get) Token: 0x060010A5 RID: 4261 RVA: 0x00076BD1 File Offset: 0x00074DD1
		// (set) Token: 0x060010A6 RID: 4262 RVA: 0x00076BD9 File Offset: 0x00074DD9
		[ProtoMember(6)]
		public bool AutoEatX25 { get; set; }

		// Token: 0x170003BE RID: 958
		// (get) Token: 0x060010A7 RID: 4263 RVA: 0x00076BE2 File Offset: 0x00074DE2
		// (set) Token: 0x060010A8 RID: 4264 RVA: 0x00076BEA File Offset: 0x00074DEA
		[ProtoMember(7)]
		public bool UseSpecialItem { get; set; }
	}
}
