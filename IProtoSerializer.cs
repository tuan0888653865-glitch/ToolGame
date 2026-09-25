using System;

namespace ProtoBuf.Serializers
{
	// Token: 0x0200004A RID: 74
	internal interface IProtoSerializer
	{
		// Token: 0x17000061 RID: 97
		// (get) Token: 0x0600024F RID: 591
		Type ExpectedType { get; }

		// Token: 0x06000250 RID: 592
		void Write(object value, ProtoWriter dest);

		// Token: 0x06000251 RID: 593
		object Read(object value, ProtoReader source);

		// Token: 0x17000062 RID: 98
		// (get) Token: 0x06000252 RID: 594
		bool RequiresOldValue { get; }

		// Token: 0x17000063 RID: 99
		// (get) Token: 0x06000253 RID: 595
		bool ReturnsValue { get; }
	}
}
