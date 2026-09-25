using System;
using ProtoBuf.Meta;

namespace ProtoBuf.Serializers
{
	// Token: 0x02000054 RID: 84
	internal sealed class NullDecorator : ProtoDecoratorBase
	{
		// Token: 0x060002A4 RID: 676 RVA: 0x0000FAD0 File Offset: 0x0000DCD0
		public NullDecorator(TypeModel model, IProtoSerializer tail) : base(tail)
		{
			if (!tail.ReturnsValue)
			{
				throw new NotSupportedException("NullDecorator only supports implementations that return values");
			}
			if (Helpers.IsValueType(tail.ExpectedType))
			{
				this.expectedType = model.MapType(typeof(Nullable<>)).MakeGenericType(new Type[]
				{
					tail.ExpectedType
				});
				return;
			}
			this.expectedType = tail.ExpectedType;
		}

		// Token: 0x17000077 RID: 119
		// (get) Token: 0x060002A5 RID: 677 RVA: 0x0000FB3B File Offset: 0x0000DD3B
		public override Type ExpectedType
		{
			get
			{
				return this.expectedType;
			}
		}

		// Token: 0x17000078 RID: 120
		// (get) Token: 0x060002A6 RID: 678 RVA: 0x0000D470 File Offset: 0x0000B670
		public override bool ReturnsValue
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000079 RID: 121
		// (get) Token: 0x060002A7 RID: 679 RVA: 0x0000D470 File Offset: 0x0000B670
		public override bool RequiresOldValue
		{
			get
			{
				return true;
			}
		}

		// Token: 0x060002A8 RID: 680 RVA: 0x0000FB44 File Offset: 0x0000DD44
		public override object Read(object value, ProtoReader source)
		{
			SubItemToken token = ProtoReader.StartSubItem(source);
			int num;
			while ((num = source.ReadFieldHeader()) > 0)
			{
				if (num == 1)
				{
					value = this.Tail.Read(value, source);
				}
				else
				{
					source.SkipField();
				}
			}
			ProtoReader.EndSubItem(token, source);
			return value;
		}

		// Token: 0x060002A9 RID: 681 RVA: 0x0000FB88 File Offset: 0x0000DD88
		public override void Write(object value, ProtoWriter dest)
		{
			SubItemToken token = ProtoWriter.StartSubItem(null, dest);
			if (value != null)
			{
				this.Tail.Write(value, dest);
			}
			ProtoWriter.EndSubItem(token, dest);
		}

		// Token: 0x0400025F RID: 607
		public const int Tag = 1;

		// Token: 0x04000260 RID: 608
		private readonly Type expectedType;
	}
}
