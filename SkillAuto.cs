using System;
using ProtoBuf;

namespace TinhKiemAuto.AutoControl
{
	// Token: 0x02000143 RID: 323
	[ProtoContract]
	public class SkillAuto
	{
		// Token: 0x1700039F RID: 927
		// (get) Token: 0x06001065 RID: 4197 RVA: 0x000769D3 File Offset: 0x00074BD3
		// (set) Token: 0x06001066 RID: 4198 RVA: 0x000769DB File Offset: 0x00074BDB
		[ProtoMember(1)]
		public int Id { get; set; }

		// Token: 0x170003A0 RID: 928
		// (get) Token: 0x06001067 RID: 4199 RVA: 0x000769E4 File Offset: 0x00074BE4
		// (set) Token: 0x06001068 RID: 4200 RVA: 0x000769EC File Offset: 0x00074BEC
		[ProtoMember(2)]
		public string Name { get; set; }

		// Token: 0x170003A1 RID: 929
		// (get) Token: 0x06001069 RID: 4201 RVA: 0x000769F5 File Offset: 0x00074BF5
		// (set) Token: 0x0600106A RID: 4202 RVA: 0x000769FD File Offset: 0x00074BFD
		[ProtoMember(3)]
		public bool IsUsing { get; set; }
	}
}
