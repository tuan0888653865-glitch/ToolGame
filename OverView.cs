using System;
using ProtoBuf;

namespace TinhKiemAuto.AutoControl
{
	// Token: 0x0200014A RID: 330
	[ProtoContract]
	public class OverView
	{
		// Token: 0x170003C3 RID: 963
		// (get) Token: 0x060010B4 RID: 4276 RVA: 0x00076C37 File Offset: 0x00074E37
		// (set) Token: 0x060010B5 RID: 4277 RVA: 0x00076C3F File Offset: 0x00074E3F
		[ProtoMember(1)]
		public int PhamViDanh { get; set; }

		// Token: 0x170003C4 RID: 964
		// (get) Token: 0x060010B6 RID: 4278 RVA: 0x00076C48 File Offset: 0x00074E48
		// (set) Token: 0x060010B7 RID: 4279 RVA: 0x00076C50 File Offset: 0x00074E50
		[ProtoMember(2)]
		public bool IsDanhQuanhDiem { get; set; }

		// Token: 0x170003C5 RID: 965
		// (get) Token: 0x060010B8 RID: 4280 RVA: 0x00076C59 File Offset: 0x00074E59
		// (set) Token: 0x060010B9 RID: 4281 RVA: 0x00076C61 File Offset: 0x00074E61
		[ProtoMember(3)]
		public bool IsGomQuai { get; set; }

		// Token: 0x170003C6 RID: 966
		// (get) Token: 0x060010BA RID: 4282 RVA: 0x00076C6A File Offset: 0x00074E6A
		// (set) Token: 0x060010BB RID: 4283 RVA: 0x00076C72 File Offset: 0x00074E72
		[ProtoMember(4)]
		public bool IsUsingThoLinhChau { get; set; }

		// Token: 0x170003C7 RID: 967
		// (get) Token: 0x060010BC RID: 4284 RVA: 0x00076C7B File Offset: 0x00074E7B
		// (set) Token: 0x060010BD RID: 4285 RVA: 0x00076C83 File Offset: 0x00074E83
		[ProtoMember(5)]
		public bool IsRengeHP { get; set; }

		// Token: 0x170003C8 RID: 968
		// (get) Token: 0x060010BE RID: 4286 RVA: 0x00076C8C File Offset: 0x00074E8C
		// (set) Token: 0x060010BF RID: 4287 RVA: 0x00076C94 File Offset: 0x00074E94
		[ProtoMember(6)]
		public int RengeHPPercent { get; set; }

		// Token: 0x170003C9 RID: 969
		// (get) Token: 0x060010C0 RID: 4288 RVA: 0x00076C9D File Offset: 0x00074E9D
		// (set) Token: 0x060010C1 RID: 4289 RVA: 0x00076CA5 File Offset: 0x00074EA5
		[ProtoMember(7)]
		public bool IsRengeMP { get; set; }

		// Token: 0x170003CA RID: 970
		// (get) Token: 0x060010C2 RID: 4290 RVA: 0x00076CAE File Offset: 0x00074EAE
		// (set) Token: 0x060010C3 RID: 4291 RVA: 0x00076CB6 File Offset: 0x00074EB6
		[ProtoMember(8)]
		public int RengeMPPercent { get; set; }

		// Token: 0x170003CB RID: 971
		// (get) Token: 0x060010C4 RID: 4292 RVA: 0x00076CBF File Offset: 0x00074EBF
		// (set) Token: 0x060010C5 RID: 4293 RVA: 0x00076CC7 File Offset: 0x00074EC7
		[ProtoMember(9)]
		public bool IsCongSinh { get; set; }

		// Token: 0x170003CC RID: 972
		// (get) Token: 0x060010C6 RID: 4294 RVA: 0x00076CD0 File Offset: 0x00074ED0
		// (set) Token: 0x060010C7 RID: 4295 RVA: 0x00076CD8 File Offset: 0x00074ED8
		[ProtoMember(10)]
		public int CongSinhValue { get; set; }

		// Token: 0x170003CD RID: 973
		// (get) Token: 0x060010C8 RID: 4296 RVA: 0x00076CE1 File Offset: 0x00074EE1
		// (set) Token: 0x060010C9 RID: 4297 RVA: 0x00076CE9 File Offset: 0x00074EE9
		[ProtoMember(11)]
		public bool IsHuyetTe { get; set; }

		// Token: 0x170003CE RID: 974
		// (get) Token: 0x060010CA RID: 4298 RVA: 0x00076CF2 File Offset: 0x00074EF2
		// (set) Token: 0x060010CB RID: 4299 RVA: 0x00076CFA File Offset: 0x00074EFA
		[ProtoMember(12)]
		public int HuyetTeValue { get; set; }

		// Token: 0x170003CF RID: 975
		// (get) Token: 0x060010CC RID: 4300 RVA: 0x00076D03 File Offset: 0x00074F03
		// (set) Token: 0x060010CD RID: 4301 RVA: 0x00076D0B File Offset: 0x00074F0B
		[ProtoMember(13)]
		public bool IsNM { get; set; }

		// Token: 0x170003D0 RID: 976
		// (get) Token: 0x060010CE RID: 4302 RVA: 0x00076D14 File Offset: 0x00074F14
		// (set) Token: 0x060010CF RID: 4303 RVA: 0x00076D1C File Offset: 0x00074F1C
		[ProtoMember(14)]
		public int BuffNMPercent { get; set; }

		// Token: 0x170003D1 RID: 977
		// (get) Token: 0x060010D0 RID: 4304 RVA: 0x00076D25 File Offset: 0x00074F25
		// (set) Token: 0x060010D1 RID: 4305 RVA: 0x00076D2D File Offset: 0x00074F2D
		[ProtoMember(15)]
		public bool IsAutoReborn { get; set; }

		// Token: 0x170003D2 RID: 978
		// (get) Token: 0x060010D2 RID: 4306 RVA: 0x00076D36 File Offset: 0x00074F36
		// (set) Token: 0x060010D3 RID: 4307 RVA: 0x00076D3E File Offset: 0x00074F3E
		[ProtoMember(16)]
		public bool IsAutoComeBack { get; set; }

		// Token: 0x170003D3 RID: 979
		// (get) Token: 0x060010D4 RID: 4308 RVA: 0x00076D47 File Offset: 0x00074F47
		// (set) Token: 0x060010D5 RID: 4309 RVA: 0x00076D4F File Offset: 0x00074F4F
		[ProtoMember(17)]
		public bool isArletHP { get; set; }

		// Token: 0x170003D4 RID: 980
		// (get) Token: 0x060010D6 RID: 4310 RVA: 0x00076D58 File Offset: 0x00074F58
		// (set) Token: 0x060010D7 RID: 4311 RVA: 0x00076D60 File Offset: 0x00074F60
		[ProtoMember(18)]
		public int ArletHPPercent { get; set; }
	}
}
