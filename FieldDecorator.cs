using System;
using System.Reflection;

namespace ProtoBuf.Serializers
{
	// Token: 0x02000045 RID: 69
	internal sealed class FieldDecorator : ProtoDecoratorBase
	{
		// Token: 0x17000052 RID: 82
		// (get) Token: 0x0600022D RID: 557 RVA: 0x0000DCB6 File Offset: 0x0000BEB6
		public override Type ExpectedType
		{
			get
			{
				return this.forType;
			}
		}

		// Token: 0x17000053 RID: 83
		// (get) Token: 0x0600022E RID: 558 RVA: 0x0000D470 File Offset: 0x0000B670
		public override bool RequiresOldValue
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000054 RID: 84
		// (get) Token: 0x0600022F RID: 559 RVA: 0x00008C9D File Offset: 0x00006E9D
		public override bool ReturnsValue
		{
			get
			{
				return false;
			}
		}

		// Token: 0x06000230 RID: 560 RVA: 0x0000DCBE File Offset: 0x0000BEBE
		public FieldDecorator(Type forType, FieldInfo field, IProtoSerializer tail) : base(tail)
		{
			this.forType = forType;
			this.field = field;
		}

		// Token: 0x06000231 RID: 561 RVA: 0x0000DCD5 File Offset: 0x0000BED5
		public override void Write(object value, ProtoWriter dest)
		{
			value = this.field.GetValue(value);
			if (value != null)
			{
				this.Tail.Write(value, dest);
			}
		}

		// Token: 0x06000232 RID: 562 RVA: 0x0000DCF8 File Offset: 0x0000BEF8
		public override object Read(object value, ProtoReader source)
		{
			object obj = this.Tail.Read(this.Tail.RequiresOldValue ? this.field.GetValue(value) : null, source);
			if (obj != null)
			{
				this.field.SetValue(value, obj);
			}
			return null;
		}

		// Token: 0x04000227 RID: 551
		private readonly FieldInfo field;

		// Token: 0x04000228 RID: 552
		private readonly Type forType;
	}
}
