using System;
using ProtoBuf;

namespace TinhKiemAuto.Models
{
	// Token: 0x02000121 RID: 289
	[ProtoContract]
	public class PacketDef
	{
		// Token: 0x1700034A RID: 842
		// (get) Token: 0x06000F51 RID: 3921 RVA: 0x00074880 File Offset: 0x00072A80
		// (set) Token: 0x06000F52 RID: 3922 RVA: 0x00074888 File Offset: 0x00072A88
		[ProtoMember(1)]
		public int IDPacket { get; set; }

		// Token: 0x1700034B RID: 843
		// (get) Token: 0x06000F53 RID: 3923 RVA: 0x00074891 File Offset: 0x00072A91
		// (set) Token: 0x06000F54 RID: 3924 RVA: 0x00074899 File Offset: 0x00072A99
		[ProtoMember(2)]
		public byte[] data { get; set; }
	}
}
