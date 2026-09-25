using System;

namespace ProtoBuf
{
	// Token: 0x02000037 RID: 55
	public enum WireType
	{
		// Token: 0x04000205 RID: 517
		None = -1,
		// Token: 0x04000206 RID: 518
		Variant,
		// Token: 0x04000207 RID: 519
		Fixed64,
		// Token: 0x04000208 RID: 520
		String,
		// Token: 0x04000209 RID: 521
		StartGroup,
		// Token: 0x0400020A RID: 522
		EndGroup,
		// Token: 0x0400020B RID: 523
		Fixed32,
		// Token: 0x0400020C RID: 524
		SignedVariant = 8
	}
}
