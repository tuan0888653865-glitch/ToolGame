using System;
using System.Collections.Generic;
using ProtoBuf;

namespace TinhKiemAuto.AutoControl
{
	// Token: 0x02000148 RID: 328
	[ProtoContract]
	public class TotalSkill
	{
		// Token: 0x170003BF RID: 959
		// (get) Token: 0x060010AA RID: 4266 RVA: 0x00076BF3 File Offset: 0x00074DF3
		// (set) Token: 0x060010AB RID: 4267 RVA: 0x00076BFB File Offset: 0x00074DFB
		[ProtoMember(1)]
		public List<SkillAuto> ActiveSkill { get; set; }

		// Token: 0x170003C0 RID: 960
		// (get) Token: 0x060010AC RID: 4268 RVA: 0x00076C04 File Offset: 0x00074E04
		// (set) Token: 0x060010AD RID: 4269 RVA: 0x00076C0C File Offset: 0x00074E0C
		[ProtoMember(2)]
		public List<SkillAuto> PassiveSkill { get; set; }
	}
}
