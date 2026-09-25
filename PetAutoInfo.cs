using System;
using ProtoBuf;

namespace TinhKiemAuto.AutoControl
{
	// Token: 0x02000145 RID: 325
	[ProtoContract]
	public class PetAutoInfo
	{
		// Token: 0x170003AE RID: 942
		// (get) Token: 0x06001085 RID: 4229 RVA: 0x00076AD2 File Offset: 0x00074CD2
		// (set) Token: 0x06001086 RID: 4230 RVA: 0x00076ADA File Offset: 0x00074CDA
		[ProtoMember(1)]
		public string Name { get; set; }

		// Token: 0x170003AF RID: 943
		// (get) Token: 0x06001087 RID: 4231 RVA: 0x00076AE3 File Offset: 0x00074CE3
		// (set) Token: 0x06001088 RID: 4232 RVA: 0x00076AEB File Offset: 0x00074CEB
		[ProtoMember(2)]
		public int Id { get; set; }

		// Token: 0x170003B0 RID: 944
		// (get) Token: 0x06001089 RID: 4233 RVA: 0x00076AF4 File Offset: 0x00074CF4
		// (set) Token: 0x0600108A RID: 4234 RVA: 0x00076AFC File Offset: 0x00074CFC
		[ProtoMember(3)]
		public int Pos { get; set; }
	}
}
