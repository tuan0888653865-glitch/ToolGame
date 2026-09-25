using System;

namespace ProtoBuf
{
	// Token: 0x02000021 RID: 33
	[Flags]
	public enum MemberSerializationOptions
	{
		// Token: 0x04000192 RID: 402
		None = 0,
		// Token: 0x04000193 RID: 403
		Packed = 1,
		// Token: 0x04000194 RID: 404
		Required = 2,
		// Token: 0x04000195 RID: 405
		AsReference = 4,
		// Token: 0x04000196 RID: 406
		DynamicType = 8,
		// Token: 0x04000197 RID: 407
		OverwriteList = 16
	}
}
