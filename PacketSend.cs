using System;
using System.Collections.Generic;
using ProtoBuf;

namespace TinhKiemAuto.AutoControl
{
	// Token: 0x02000149 RID: 329
	[ProtoContract]
	public class PacketSend
	{
		// Token: 0x170003C1 RID: 961
		// (get) Token: 0x060010AF RID: 4271 RVA: 0x00076C15 File Offset: 0x00074E15
		// (set) Token: 0x060010B0 RID: 4272 RVA: 0x00076C1D File Offset: 0x00074E1D
		[ProtoMember(1)]
		public string HardwareID { get; set; }

		// Token: 0x170003C2 RID: 962
		// (get) Token: 0x060010B1 RID: 4273 RVA: 0x00076C26 File Offset: 0x00074E26
		// (set) Token: 0x060010B2 RID: 4274 RVA: 0x00076C2E File Offset: 0x00074E2E
		[ProtoMember(2)]
		public Dictionary<string, AutoReport> DanhSachGame { get; set; }
	}
}
