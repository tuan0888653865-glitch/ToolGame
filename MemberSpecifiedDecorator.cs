using System;
using System.Reflection;

namespace ProtoBuf.Serializers
{
	// Token: 0x02000051 RID: 81
	internal sealed class MemberSpecifiedDecorator : ProtoDecoratorBase
	{
		// Token: 0x17000071 RID: 113
		// (get) Token: 0x06000281 RID: 641 RVA: 0x0000D883 File Offset: 0x0000BA83
		public override Type ExpectedType
		{
			get
			{
				return this.Tail.ExpectedType;
			}
		}

		// Token: 0x17000072 RID: 114
		// (get) Token: 0x06000282 RID: 642 RVA: 0x0000D890 File Offset: 0x0000BA90
		public override bool RequiresOldValue
		{
			get
			{
				return this.Tail.RequiresOldValue;
			}
		}

		// Token: 0x17000073 RID: 115
		// (get) Token: 0x06000283 RID: 643 RVA: 0x0000D89D File Offset: 0x0000BA9D
		public override bool ReturnsValue
		{
			get
			{
				return this.Tail.ReturnsValue;
			}
		}

		// Token: 0x06000284 RID: 644 RVA: 0x0000EFA4 File Offset: 0x0000D1A4
		public MemberSpecifiedDecorator(MethodInfo getSpecified, MethodInfo setSpecified, IProtoSerializer tail) : base(tail)
		{
			if (getSpecified == null && setSpecified == null)
			{
				throw new InvalidOperationException();
			}
			this.getSpecified = getSpecified;
			this.setSpecified = setSpecified;
		}

		// Token: 0x06000285 RID: 645 RVA: 0x0000EFC7 File Offset: 0x0000D1C7
		public override void Write(object value, ProtoWriter dest)
		{
			if (this.getSpecified == null || (bool)this.getSpecified.Invoke(value, null))
			{
				this.Tail.Write(value, dest);
			}
		}

		// Token: 0x06000286 RID: 646 RVA: 0x0000EFF4 File Offset: 0x0000D1F4
		public override object Read(object value, ProtoReader source)
		{
			object result = this.Tail.Read(value, source);
			if (this.setSpecified != null)
			{
				this.setSpecified.Invoke(value, new object[]
				{
					true
				});
			}
			return result;
		}

		// Token: 0x0400024B RID: 587
		private readonly MethodInfo getSpecified;

		// Token: 0x0400024C RID: 588
		private readonly MethodInfo setSpecified;
	}
}
