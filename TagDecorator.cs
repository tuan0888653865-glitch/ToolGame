using System;
using ProtoBuf.Meta;

namespace ProtoBuf.Serializers
{
	// Token: 0x0200005F RID: 95
	internal sealed class TagDecorator : ProtoDecoratorBase, IProtoTypeSerializer, IProtoSerializer
	{
		// Token: 0x060002F0 RID: 752 RVA: 0x000101D4 File Offset: 0x0000E3D4
		public bool HasCallbacks(TypeModel.CallbackType callbackType)
		{
			IProtoTypeSerializer protoTypeSerializer = this.Tail as IProtoTypeSerializer;
			return protoTypeSerializer != null && protoTypeSerializer.HasCallbacks(callbackType);
		}

		// Token: 0x060002F1 RID: 753 RVA: 0x000101FC File Offset: 0x0000E3FC
		public void Callback(object value, TypeModel.CallbackType callbackType, SerializationContext context)
		{
			IProtoTypeSerializer protoTypeSerializer = this.Tail as IProtoTypeSerializer;
			if (protoTypeSerializer != null)
			{
				protoTypeSerializer.Callback(value, callbackType, context);
			}
		}

		// Token: 0x17000095 RID: 149
		// (get) Token: 0x060002F2 RID: 754 RVA: 0x0000D883 File Offset: 0x0000BA83
		public override Type ExpectedType
		{
			get
			{
				return this.Tail.ExpectedType;
			}
		}

		// Token: 0x060002F3 RID: 755 RVA: 0x00010221 File Offset: 0x0000E421
		public TagDecorator(int fieldNumber, WireType wireType, bool strict, IProtoSerializer tail) : base(tail)
		{
			this.fieldNumber = fieldNumber;
			this.wireType = wireType;
			this.strict = strict;
		}

		// Token: 0x17000096 RID: 150
		// (get) Token: 0x060002F4 RID: 756 RVA: 0x0000D890 File Offset: 0x0000BA90
		public override bool RequiresOldValue
		{
			get
			{
				return this.Tail.RequiresOldValue;
			}
		}

		// Token: 0x17000097 RID: 151
		// (get) Token: 0x060002F5 RID: 757 RVA: 0x0000D89D File Offset: 0x0000BA9D
		public override bool ReturnsValue
		{
			get
			{
				return this.Tail.ReturnsValue;
			}
		}

		// Token: 0x17000098 RID: 152
		// (get) Token: 0x060002F6 RID: 758 RVA: 0x00010240 File Offset: 0x0000E440
		private bool NeedsHint
		{
			get
			{
				return (this.wireType & (WireType)(-8)) > WireType.Variant;
			}
		}

		// Token: 0x060002F7 RID: 759 RVA: 0x0001024E File Offset: 0x0000E44E
		public override object Read(object value, ProtoReader source)
		{
			if (this.strict)
			{
				source.Assert(this.wireType);
			}
			else if (this.NeedsHint)
			{
				source.Hint(this.wireType);
			}
			return this.Tail.Read(value, source);
		}

		// Token: 0x060002F8 RID: 760 RVA: 0x00010287 File Offset: 0x0000E487
		public override void Write(object value, ProtoWriter dest)
		{
			ProtoWriter.WriteFieldHeader(this.fieldNumber, this.wireType, dest);
			this.Tail.Write(value, dest);
		}

		// Token: 0x04000274 RID: 628
		private readonly bool strict;

		// Token: 0x04000275 RID: 629
		private readonly int fieldNumber;

		// Token: 0x04000276 RID: 630
		private readonly WireType wireType;
	}
}
