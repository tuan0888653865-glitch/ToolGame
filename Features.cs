using System;
using ProtoBuf;

namespace TinhKiemAuto.AutoControl
{
	// Token: 0x02000144 RID: 324
	[ProtoContract]
	public class Features
	{
		// Token: 0x170003A2 RID: 930
		// (get) Token: 0x0600106C RID: 4204 RVA: 0x00076A06 File Offset: 0x00074C06
		// (set) Token: 0x0600106D RID: 4205 RVA: 0x00076A0E File Offset: 0x00074C0E
		[ProtoMember(1)]
		public bool IsAcceptParty { get; set; }

		// Token: 0x170003A3 RID: 931
		// (get) Token: 0x0600106E RID: 4206 RVA: 0x00076A17 File Offset: 0x00074C17
		// (set) Token: 0x0600106F RID: 4207 RVA: 0x00076A1F File Offset: 0x00074C1F
		[ProtoMember(2)]
		public bool IsAcceptAllPartyInvites { get; set; }

		// Token: 0x170003A4 RID: 932
		// (get) Token: 0x06001070 RID: 4208 RVA: 0x00076A28 File Offset: 0x00074C28
		// (set) Token: 0x06001071 RID: 4209 RVA: 0x00076A30 File Offset: 0x00074C30
		[ProtoMember(3)]
		public bool IsUseSkillF1 { get; set; }

		// Token: 0x170003A5 RID: 933
		// (get) Token: 0x06001072 RID: 4210 RVA: 0x00076A39 File Offset: 0x00074C39
		// (set) Token: 0x06001073 RID: 4211 RVA: 0x00076A41 File Offset: 0x00074C41
		[ProtoMember(4)]
		public bool IsAutoLevelUp { get; set; }

		// Token: 0x170003A6 RID: 934
		// (get) Token: 0x06001074 RID: 4212 RVA: 0x00076A4A File Offset: 0x00074C4A
		// (set) Token: 0x06001075 RID: 4213 RVA: 0x00076A52 File Offset: 0x00074C52
		[ProtoMember(5)]
		public int LimitLevelUp { get; set; }

		// Token: 0x170003A7 RID: 935
		// (get) Token: 0x06001076 RID: 4214 RVA: 0x00076A5B File Offset: 0x00074C5B
		// (set) Token: 0x06001077 RID: 4215 RVA: 0x00076A63 File Offset: 0x00074C63
		[ProtoMember(6)]
		public bool MakeAdvertisement { get; set; }

		// Token: 0x170003A8 RID: 936
		// (get) Token: 0x06001078 RID: 4216 RVA: 0x00076A6C File Offset: 0x00074C6C
		// (set) Token: 0x06001079 RID: 4217 RVA: 0x00076A74 File Offset: 0x00074C74
		[ProtoMember(7)]
		public int MakeAdvertisingTime { get; set; }

		// Token: 0x170003A9 RID: 937
		// (get) Token: 0x0600107A RID: 4218 RVA: 0x00076A7D File Offset: 0x00074C7D
		// (set) Token: 0x0600107B RID: 4219 RVA: 0x00076A85 File Offset: 0x00074C85
		[ProtoMember(8)]
		public int Chanel { get; set; }

		// Token: 0x170003AA RID: 938
		// (get) Token: 0x0600107C RID: 4220 RVA: 0x00076A8E File Offset: 0x00074C8E
		// (set) Token: 0x0600107D RID: 4221 RVA: 0x00076A96 File Offset: 0x00074C96
		[ProtoMember(9)]
		public string ChatMSG { get; set; }

		// Token: 0x170003AB RID: 939
		// (get) Token: 0x0600107E RID: 4222 RVA: 0x00076A9F File Offset: 0x00074C9F
		// (set) Token: 0x0600107F RID: 4223 RVA: 0x00076AA7 File Offset: 0x00074CA7
		[ProtoMember(10)]
		public bool NoticePrivateMessage { get; set; }

		// Token: 0x170003AC RID: 940
		// (get) Token: 0x06001080 RID: 4224 RVA: 0x00076AB0 File Offset: 0x00074CB0
		// (set) Token: 0x06001081 RID: 4225 RVA: 0x00076AB8 File Offset: 0x00074CB8
		[ProtoMember(11)]
		public int FollowRadius { get; set; }

		// Token: 0x170003AD RID: 941
		// (get) Token: 0x06001082 RID: 4226 RVA: 0x00076AC1 File Offset: 0x00074CC1
		// (set) Token: 0x06001083 RID: 4227 RVA: 0x00076AC9 File Offset: 0x00074CC9
		[ProtoMember(12)]
		public string Pass2 { get; set; }
	}
}
