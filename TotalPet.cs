using System;
using System.Collections.Generic;
using ProtoBuf;

namespace TinhKiemAuto.AutoControl
{
	// Token: 0x02000146 RID: 326
	[ProtoContract]
	public class TotalPet
	{
		// Token: 0x170003B1 RID: 945
		// (get) Token: 0x0600108C RID: 4236 RVA: 0x00076B05 File Offset: 0x00074D05
		// (set) Token: 0x0600108D RID: 4237 RVA: 0x00076B0D File Offset: 0x00074D0D
		[ProtoMember(1)]
		public List<PetAutoInfo> PetInfo { get; set; }

		// Token: 0x170003B2 RID: 946
		// (get) Token: 0x0600108E RID: 4238 RVA: 0x00076B16 File Offset: 0x00074D16
		// (set) Token: 0x0600108F RID: 4239 RVA: 0x00076B1E File Offset: 0x00074D1E
		[ProtoMember(2)]
		public bool IsAutoCallPet { get; set; }

		// Token: 0x170003B3 RID: 947
		// (get) Token: 0x06001090 RID: 4240 RVA: 0x00076B27 File Offset: 0x00074D27
		// (set) Token: 0x06001091 RID: 4241 RVA: 0x00076B2F File Offset: 0x00074D2F
		[ProtoMember(3)]
		public bool IsAutoBuffPet { get; set; }

		// Token: 0x170003B4 RID: 948
		// (get) Token: 0x06001092 RID: 4242 RVA: 0x00076B38 File Offset: 0x00074D38
		// (set) Token: 0x06001093 RID: 4243 RVA: 0x00076B40 File Offset: 0x00074D40
		[ProtoMember(4)]
		public bool IsAutoUsePetSkill { get; set; }

		// Token: 0x170003B5 RID: 949
		// (get) Token: 0x06001094 RID: 4244 RVA: 0x00076B49 File Offset: 0x00074D49
		// (set) Token: 0x06001095 RID: 4245 RVA: 0x00076B51 File Offset: 0x00074D51
		[ProtoMember(5)]
		public bool IsAutoCallBackPet { get; set; }

		// Token: 0x170003B6 RID: 950
		// (get) Token: 0x06001096 RID: 4246 RVA: 0x00076B5A File Offset: 0x00074D5A
		// (set) Token: 0x06001097 RID: 4247 RVA: 0x00076B62 File Offset: 0x00074D62
		[ProtoMember(6)]
		public int AutoCallBackAtLevel { get; set; }

		// Token: 0x170003B7 RID: 951
		// (get) Token: 0x06001098 RID: 4248 RVA: 0x00076B6B File Offset: 0x00074D6B
		// (set) Token: 0x06001099 RID: 4249 RVA: 0x00076B73 File Offset: 0x00074D73
		[ProtoMember(7)]
		public bool IsAutoTakeCare { get; set; }
	}
}
