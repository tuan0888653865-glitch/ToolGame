using System;

namespace ProtoBuf.Serializers
{
	// Token: 0x02000057 RID: 87
	internal abstract class ProtoDecoratorBase : IProtoSerializer
	{
		// Token: 0x17000080 RID: 128
		// (get) Token: 0x060002BB RID: 699
		public abstract Type ExpectedType { get; }

		// Token: 0x060002BC RID: 700 RVA: 0x0000FEA7 File Offset: 0x0000E0A7
		protected ProtoDecoratorBase(IProtoSerializer tail)
		{
			this.Tail = tail;
		}

		// Token: 0x17000081 RID: 129
		// (get) Token: 0x060002BD RID: 701
		public abstract bool ReturnsValue { get; }

		// Token: 0x17000082 RID: 130
		// (get) Token: 0x060002BE RID: 702
		public abstract bool RequiresOldValue { get; }

		// Token: 0x060002BF RID: 703
		public abstract void Write(object value, ProtoWriter dest);

		// Token: 0x060002C0 RID: 704
		public abstract object Read(object value, ProtoReader source);

		// Token: 0x04000266 RID: 614
		protected readonly IProtoSerializer Tail;
	}
}
