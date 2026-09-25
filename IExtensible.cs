using System;

namespace ProtoBuf
{
	// Token: 0x0200001E RID: 30
	public interface IExtensible
	{
		// Token: 0x060000FC RID: 252
		IExtension GetExtensionObject(bool createIfMissing);
	}
}
