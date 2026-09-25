using System;
using ProtoBuf.Meta;

namespace ProtoBuf.Serializers
{
	// Token: 0x02000042 RID: 66
	internal sealed class DefaultValueDecorator : ProtoDecoratorBase
	{
		// Token: 0x17000049 RID: 73
		// (get) Token: 0x06000217 RID: 535 RVA: 0x0000D883 File Offset: 0x0000BA83
		public override Type ExpectedType
		{
			get
			{
				return this.Tail.ExpectedType;
			}
		}

		// Token: 0x1700004A RID: 74
		// (get) Token: 0x06000218 RID: 536 RVA: 0x0000D890 File Offset: 0x0000BA90
		public override bool RequiresOldValue
		{
			get
			{
				return this.Tail.RequiresOldValue;
			}
		}

		// Token: 0x1700004B RID: 75
		// (get) Token: 0x06000219 RID: 537 RVA: 0x0000D89D File Offset: 0x0000BA9D
		public override bool ReturnsValue
		{
			get
			{
				return this.Tail.ReturnsValue;
			}
		}

		// Token: 0x0600021A RID: 538 RVA: 0x0000D8AC File Offset: 0x0000BAAC
		public DefaultValueDecorator(TypeModel model, object defaultValue, IProtoSerializer tail) : base(tail)
		{
			if (defaultValue == null)
			{
				throw new ArgumentNullException("defaultValue");
			}
			if (model.MapType(defaultValue.GetType()) != tail.ExpectedType)
			{
				throw new ArgumentException("Default value is of incorrect type", "defaultValue");
			}
			this.defaultValue = defaultValue;
		}

		// Token: 0x0600021B RID: 539 RVA: 0x0000D8F9 File Offset: 0x0000BAF9
		public override void Write(object value, ProtoWriter dest)
		{
			if (!object.Equals(value, this.defaultValue))
			{
				this.Tail.Write(value, dest);
			}
		}

		// Token: 0x0600021C RID: 540 RVA: 0x0000D916 File Offset: 0x0000BB16
		public override object Read(object value, ProtoReader source)
		{
			return this.Tail.Read(value, source);
		}

		// Token: 0x04000223 RID: 547
		private readonly object defaultValue;
	}
}
