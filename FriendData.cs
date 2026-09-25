using System;

namespace ProtoBuf
{
	// Token: 0x0200001C RID: 28
	[ProtoContract]
	public class FriendData
	{
		// Token: 0x04000184 RID: 388
		[ProtoMember(1)]
		public int DbID;

		// Token: 0x04000185 RID: 389
		[ProtoMember(2)]
		public int OtherRoleID;

		// Token: 0x04000186 RID: 390
		[ProtoMember(3)]
		public string OtherRoleName;

		// Token: 0x04000187 RID: 391
		[ProtoMember(4)]
		public int OtherLevel;

		// Token: 0x04000188 RID: 392
		[ProtoMember(5)]
		public int Occupation;

		// Token: 0x04000189 RID: 393
		[ProtoMember(6)]
		public int OnlineState;

		// Token: 0x0400018A RID: 394
		[ProtoMember(7)]
		public string Position;

		// Token: 0x0400018B RID: 395
		[ProtoMember(8)]
		public int FriendType;
	}
}
