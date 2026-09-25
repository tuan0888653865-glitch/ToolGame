using System;
using ProtoBuf.Meta;

namespace ProtoBuf.Serializers
{
	// Token: 0x0200003B RID: 59
	internal sealed class BooleanSerializer : IProtoSerializer
	{
		// Token: 0x060001ED RID: 493 RVA: 0x000020C5 File Offset: 0x000002C5
		public BooleanSerializer(TypeModel model)
		{
		}

		// Token: 0x1700003C RID: 60
		// (get) Token: 0x060001EE RID: 494 RVA: 0x0000D652 File Offset: 0x0000B852
		public Type ExpectedType
		{
			get
			{
				return BooleanSerializer.expectedType;
			}
		}

		// Token: 0x060001EF RID: 495 RVA: 0x0000D659 File Offset: 0x0000B859
		public void Write(object value, ProtoWriter dest)
		{
			ProtoWriter.WriteBoolean((bool)value, dest);
		}

		// Token: 0x060001F0 RID: 496 RVA: 0x0000D667 File Offset: 0x0000B867
		public object Read(object value, ProtoReader source)
		{
			return source.ReadBoolean();
		}

		// Token: 0x1700003D RID: 61
		// (get) Token: 0x060001F1 RID: 497 RVA: 0x00008C9D File Offset: 0x00006E9D
		bool IProtoSerializer.RequiresOldValue
		{
			get
			{
				return false;
			}
		}

		// Token: 0x1700003E RID: 62
		// (get) Token: 0x060001F2 RID: 498 RVA: 0x0000D470 File Offset: 0x0000B670
		bool IProtoSerializer.ReturnsValue
		{
			get
			{
				return true;
			}
		}

		// Token: 0x0400021B RID: 539
		private static readonly Type expectedType = typeof(bool);
	}
}
