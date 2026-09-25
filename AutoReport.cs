using System;
using ProtoBuf;

namespace TinhKiemAuto.AutoControl
{
	// Token: 0x0200014B RID: 331
	[ProtoContract]
	public class AutoReport
	{
		// Token: 0x170003D5 RID: 981
		// (get) Token: 0x060010D9 RID: 4313 RVA: 0x00076D69 File Offset: 0x00074F69
		// (set) Token: 0x060010DA RID: 4314 RVA: 0x00076D71 File Offset: 0x00074F71
		[ProtoMember(1)]
		public bool IsAttack { get; set; }

		// Token: 0x170003D6 RID: 982
		// (get) Token: 0x060010DB RID: 4315 RVA: 0x00076D7A File Offset: 0x00074F7A
		// (set) Token: 0x060010DC RID: 4316 RVA: 0x00076D82 File Offset: 0x00074F82
		[ProtoMember(2)]
		public bool IsPickItem { get; set; }

		// Token: 0x170003D7 RID: 983
		// (get) Token: 0x060010DD RID: 4317 RVA: 0x00076D8B File Offset: 0x00074F8B
		// (set) Token: 0x060010DE RID: 4318 RVA: 0x00076D93 File Offset: 0x00074F93
		[ProtoMember(3)]
		public int MapIndex { get; set; }

		// Token: 0x170003D8 RID: 984
		// (get) Token: 0x060010DF RID: 4319 RVA: 0x00076D9C File Offset: 0x00074F9C
		// (set) Token: 0x060010E0 RID: 4320 RVA: 0x00076DA4 File Offset: 0x00074FA4
		[ProtoMember(4)]
		public string MapName { get; set; }

		// Token: 0x170003D9 RID: 985
		// (get) Token: 0x060010E1 RID: 4321 RVA: 0x00076DAD File Offset: 0x00074FAD
		// (set) Token: 0x060010E2 RID: 4322 RVA: 0x00076DB5 File Offset: 0x00074FB5
		[ProtoMember(5)]
		public int PosX { get; set; }

		// Token: 0x170003DA RID: 986
		// (get) Token: 0x060010E3 RID: 4323 RVA: 0x00076DBE File Offset: 0x00074FBE
		// (set) Token: 0x060010E4 RID: 4324 RVA: 0x00076DC6 File Offset: 0x00074FC6
		[ProtoMember(6)]
		public int PosY { get; set; }

		// Token: 0x170003DB RID: 987
		// (get) Token: 0x060010E5 RID: 4325 RVA: 0x00076DCF File Offset: 0x00074FCF
		// (set) Token: 0x060010E6 RID: 4326 RVA: 0x00076DD7 File Offset: 0x00074FD7
		[ProtoMember(7)]
		public int Gold { get; set; }

		// Token: 0x170003DC RID: 988
		// (get) Token: 0x060010E7 RID: 4327 RVA: 0x00076DE0 File Offset: 0x00074FE0
		// (set) Token: 0x060010E8 RID: 4328 RVA: 0x00076DE8 File Offset: 0x00074FE8
		[ProtoMember(8)]
		public int PlayState { get; set; }

		// Token: 0x170003DD RID: 989
		// (get) Token: 0x060010E9 RID: 4329 RVA: 0x00076DF1 File Offset: 0x00074FF1
		// (set) Token: 0x060010EA RID: 4330 RVA: 0x00076DF9 File Offset: 0x00074FF9
		[ProtoMember(9)]
		public int HpPercent { get; set; }

		// Token: 0x170003DE RID: 990
		// (get) Token: 0x060010EB RID: 4331 RVA: 0x00076E02 File Offset: 0x00075002
		// (set) Token: 0x060010EC RID: 4332 RVA: 0x00076E0A File Offset: 0x0007500A
		[ProtoMember(10)]
		public int MpPercent { get; set; }

		// Token: 0x170003DF RID: 991
		// (get) Token: 0x060010ED RID: 4333 RVA: 0x00076E13 File Offset: 0x00075013
		// (set) Token: 0x060010EE RID: 4334 RVA: 0x00076E1B File Offset: 0x0007501B
		[ProtoMember(11)]
		public int PetPercent { get; set; }

		// Token: 0x170003E0 RID: 992
		// (get) Token: 0x060010EF RID: 4335 RVA: 0x00076E24 File Offset: 0x00075024
		// (set) Token: 0x060010F0 RID: 4336 RVA: 0x00076E2C File Offset: 0x0007502C
		[ProtoMember(12)]
		public float ExpPercent { get; set; }

		// Token: 0x170003E1 RID: 993
		// (get) Token: 0x060010F1 RID: 4337 RVA: 0x00076E35 File Offset: 0x00075035
		// (set) Token: 0x060010F2 RID: 4338 RVA: 0x00076E3D File Offset: 0x0007503D
		[ProtoMember(13)]
		public bool IsRide { get; set; }

		// Token: 0x170003E2 RID: 994
		// (get) Token: 0x060010F3 RID: 4339 RVA: 0x00076E46 File Offset: 0x00075046
		// (set) Token: 0x060010F4 RID: 4340 RVA: 0x00076E4E File Offset: 0x0007504E
		[ProtoMember(14)]
		public bool IsLear { get; set; }

		// Token: 0x170003E3 RID: 995
		// (get) Token: 0x060010F5 RID: 4341 RVA: 0x00076E57 File Offset: 0x00075057
		// (set) Token: 0x060010F6 RID: 4342 RVA: 0x00076E5F File Offset: 0x0007505F
		[ProtoMember(15)]
		public bool Online { get; set; }

		// Token: 0x170003E4 RID: 996
		// (get) Token: 0x060010F7 RID: 4343 RVA: 0x00076E68 File Offset: 0x00075068
		// (set) Token: 0x060010F8 RID: 4344 RVA: 0x00076E70 File Offset: 0x00075070
		[ProtoMember(16)]
		public bool IsDuoc { get; set; }

		// Token: 0x170003E5 RID: 997
		// (get) Token: 0x060010F9 RID: 4345 RVA: 0x00076E79 File Offset: 0x00075079
		// (set) Token: 0x060010FA RID: 4346 RVA: 0x00076E81 File Offset: 0x00075081
		[ProtoMember(17)]
		public bool IsDisconnect { get; set; }

		// Token: 0x170003E6 RID: 998
		// (get) Token: 0x060010FB RID: 4347 RVA: 0x00076E8A File Offset: 0x0007508A
		// (set) Token: 0x060010FC RID: 4348 RVA: 0x00076E92 File Offset: 0x00075092
		[ProtoMember(18)]
		public bool IsX25 { get; set; }

		// Token: 0x170003E7 RID: 999
		// (get) Token: 0x060010FD RID: 4349 RVA: 0x00076E9B File Offset: 0x0007509B
		// (set) Token: 0x060010FE RID: 4350 RVA: 0x00076EA3 File Offset: 0x000750A3
		[ProtoMember(19)]
		public string Msg { get; set; }

		// Token: 0x170003E8 RID: 1000
		// (get) Token: 0x060010FF RID: 4351 RVA: 0x00076EAC File Offset: 0x000750AC
		// (set) Token: 0x06001100 RID: 4352 RVA: 0x00076EB4 File Offset: 0x000750B4
		[ProtoMember(20)]
		public LenBai LenBai { get; set; }

		// Token: 0x170003E9 RID: 1001
		// (get) Token: 0x06001101 RID: 4353 RVA: 0x00076EBD File Offset: 0x000750BD
		// (set) Token: 0x06001102 RID: 4354 RVA: 0x00076EC5 File Offset: 0x000750C5
		[ProtoMember(21)]
		public CheDo CheDo { get; set; }

		// Token: 0x170003EA RID: 1002
		// (get) Token: 0x06001103 RID: 4355 RVA: 0x00076ECE File Offset: 0x000750CE
		// (set) Token: 0x06001104 RID: 4356 RVA: 0x00076ED6 File Offset: 0x000750D6
		[ProtoMember(22)]
		public string CharID { get; set; }

		// Token: 0x170003EB RID: 1003
		// (get) Token: 0x06001105 RID: 4357 RVA: 0x00076EDF File Offset: 0x000750DF
		// (set) Token: 0x06001106 RID: 4358 RVA: 0x00076EE7 File Offset: 0x000750E7
		[ProtoMember(23)]
		public string CharName { get; set; }

		// Token: 0x170003EC RID: 1004
		// (get) Token: 0x06001107 RID: 4359 RVA: 0x00076EF0 File Offset: 0x000750F0
		// (set) Token: 0x06001108 RID: 4360 RVA: 0x00076EF8 File Offset: 0x000750F8
		[ProtoMember(24)]
		public int Level { get; set; }

		// Token: 0x170003ED RID: 1005
		// (get) Token: 0x06001109 RID: 4361 RVA: 0x00076F01 File Offset: 0x00075101
		// (set) Token: 0x0600110A RID: 4362 RVA: 0x00076F09 File Offset: 0x00075109
		[ProtoMember(25)]
		public string GuildName { get; set; }

		// Token: 0x170003EE RID: 1006
		// (get) Token: 0x0600110B RID: 4363 RVA: 0x00076F12 File Offset: 0x00075112
		// (set) Token: 0x0600110C RID: 4364 RVA: 0x00076F1A File Offset: 0x0007511A
		[ProtoMember(26)]
		public string GuildId { get; set; }

		// Token: 0x170003EF RID: 1007
		// (get) Token: 0x0600110D RID: 4365 RVA: 0x00076F23 File Offset: 0x00075123
		// (set) Token: 0x0600110E RID: 4366 RVA: 0x00076F2B File Offset: 0x0007512B
		[ProtoMember(27)]
		public string Phai { get; set; }

		// Token: 0x170003F0 RID: 1008
		// (get) Token: 0x0600110F RID: 4367 RVA: 0x00076F34 File Offset: 0x00075134
		// (set) Token: 0x06001110 RID: 4368 RVA: 0x00076F3C File Offset: 0x0007513C
		[ProtoMember(28)]
		public OverView OverView { get; set; }

		// Token: 0x170003F1 RID: 1009
		// (get) Token: 0x06001111 RID: 4369 RVA: 0x00076F45 File Offset: 0x00075145
		// (set) Token: 0x06001112 RID: 4370 RVA: 0x00076F4D File Offset: 0x0007514D
		[ProtoMember(29)]
		public TotalSkill Skills { get; set; }

		// Token: 0x170003F2 RID: 1010
		// (get) Token: 0x06001113 RID: 4371 RVA: 0x00076F56 File Offset: 0x00075156
		// (set) Token: 0x06001114 RID: 4372 RVA: 0x00076F5E File Offset: 0x0007515E
		[ProtoMember(30)]
		public PickUpItems PickUpItems { get; set; }

		// Token: 0x170003F3 RID: 1011
		// (get) Token: 0x06001115 RID: 4373 RVA: 0x00076F67 File Offset: 0x00075167
		// (set) Token: 0x06001116 RID: 4374 RVA: 0x00076F6F File Offset: 0x0007516F
		[ProtoMember(31)]
		public TotalPet Pets { get; set; }

		// Token: 0x170003F4 RID: 1012
		// (get) Token: 0x06001117 RID: 4375 RVA: 0x00076F78 File Offset: 0x00075178
		// (set) Token: 0x06001118 RID: 4376 RVA: 0x00076F80 File Offset: 0x00075180
		[ProtoMember(32)]
		public Features Features { get; set; }

		// Token: 0x170003F5 RID: 1013
		// (get) Token: 0x06001119 RID: 4377 RVA: 0x00076F89 File Offset: 0x00075189
		// (set) Token: 0x0600111A RID: 4378 RVA: 0x00076F91 File Offset: 0x00075191
		[ProtoMember(33)]
		public bool IsLenBai { get; set; }
	}
}
