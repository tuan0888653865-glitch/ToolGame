using System;

namespace TinhKiemAuto.Controllers
{
	// Token: 0x0200013F RID: 319
	public class SkillModel
	{
		// Token: 0x17000384 RID: 900
		// (get) Token: 0x06001028 RID: 4136 RVA: 0x00076773 File Offset: 0x00074973
		// (set) Token: 0x06001029 RID: 4137 RVA: 0x0007677B File Offset: 0x0007497B
		public int id { get; set; }

		// Token: 0x17000385 RID: 901
		// (get) Token: 0x0600102A RID: 4138 RVA: 0x00076784 File Offset: 0x00074984
		// (set) Token: 0x0600102B RID: 4139 RVA: 0x0007678C File Offset: 0x0007498C
		public int classID { get; set; }

		// Token: 0x17000386 RID: 902
		// (get) Token: 0x0600102C RID: 4140 RVA: 0x00076795 File Offset: 0x00074995
		// (set) Token: 0x0600102D RID: 4141 RVA: 0x0007679D File Offset: 0x0007499D
		public string name { get; set; }

		// Token: 0x17000387 RID: 903
		// (get) Token: 0x0600102E RID: 4142 RVA: 0x000767A6 File Offset: 0x000749A6
		// (set) Token: 0x0600102F RID: 4143 RVA: 0x000767AE File Offset: 0x000749AE
		public string icon { get; set; }

		// Token: 0x17000388 RID: 904
		// (get) Token: 0x06001030 RID: 4144 RVA: 0x000767B7 File Offset: 0x000749B7
		// (set) Token: 0x06001031 RID: 4145 RVA: 0x000767BF File Offset: 0x000749BF
		public bool isNeedWeapon { get; set; }

		// Token: 0x17000389 RID: 905
		// (get) Token: 0x06001032 RID: 4146 RVA: 0x000767C8 File Offset: 0x000749C8
		// (set) Token: 0x06001033 RID: 4147 RVA: 0x000767D0 File Offset: 0x000749D0
		public bool isTargetMustBeAlive { get; set; }

		// Token: 0x1700038A RID: 906
		// (get) Token: 0x06001034 RID: 4148 RVA: 0x000767D9 File Offset: 0x000749D9
		// (set) Token: 0x06001035 RID: 4149 RVA: 0x000767E1 File Offset: 0x000749E1
		public bool isPassive { get; set; }

		// Token: 0x1700038B RID: 907
		// (get) Token: 0x06001036 RID: 4150 RVA: 0x000767EA File Offset: 0x000749EA
		// (set) Token: 0x06001037 RID: 4151 RVA: 0x000767F2 File Offset: 0x000749F2
		public bool isPetActiveSkill { get; set; }

		// Token: 0x1700038C RID: 908
		// (get) Token: 0x06001038 RID: 4152 RVA: 0x000767FB File Offset: 0x000749FB
		// (set) Token: 0x06001039 RID: 4153 RVA: 0x00076803 File Offset: 0x00074A03
		public string skillType { get; set; }

		// Token: 0x1700038D RID: 909
		// (get) Token: 0x0600103A RID: 4154 RVA: 0x0007680C File Offset: 0x00074A0C
		// (set) Token: 0x0600103B RID: 4155 RVA: 0x00076814 File Offset: 0x00074A14
		public int useRange { get; set; }

		// Token: 0x1700038E RID: 910
		// (get) Token: 0x0600103C RID: 4156 RVA: 0x0007681D File Offset: 0x00074A1D
		// (set) Token: 0x0600103D RID: 4157 RVA: 0x00076825 File Offset: 0x00074A25
		public string skillTargetType { get; set; }

		// Token: 0x1700038F RID: 911
		// (get) Token: 0x0600103E RID: 4158 RVA: 0x0007682E File Offset: 0x00074A2E
		// (set) Token: 0x0600103F RID: 4159 RVA: 0x00076836 File Offset: 0x00074A36
		public bool isAutoCast { get; set; }

		// Token: 0x17000390 RID: 912
		// (get) Token: 0x06001040 RID: 4160 RVA: 0x0007683F File Offset: 0x00074A3F
		// (set) Token: 0x06001041 RID: 4161 RVA: 0x00076847 File Offset: 0x00074A47
		public Impact impact { get; set; }
	}
}
