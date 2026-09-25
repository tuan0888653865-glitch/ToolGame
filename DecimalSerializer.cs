using System;
using ProtoBuf.Meta;

namespace ProtoBuf.Serializers
{
	// Token: 0x02000041 RID: 65
	internal sealed class DecimalSerializer : IProtoSerializer
	{
		// Token: 0x06000210 RID: 528 RVA: 0x000020C5 File Offset: 0x000002C5
		public DecimalSerializer(TypeModel model)
		{
		}

		// Token: 0x17000046 RID: 70
		// (get) Token: 0x06000211 RID: 529 RVA: 0x0000D850 File Offset: 0x0000BA50
		public Type ExpectedType
		{
			get
			{
				return DecimalSerializer.expectedType;
			}
		}

		// Token: 0x17000047 RID: 71
		// (get) Token: 0x06000212 RID: 530 RVA: 0x00008C9D File Offset: 0x00006E9D
		bool IProtoSerializer.RequiresOldValue
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000048 RID: 72
		// (get) Token: 0x06000213 RID: 531 RVA: 0x0000D470 File Offset: 0x0000B670
		bool IProtoSerializer.ReturnsValue
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06000214 RID: 532 RVA: 0x0000D857 File Offset: 0x0000BA57
		public object Read(object value, ProtoReader source)
		{
			return BclHelpers.ReadDecimal(source);
		}

		// Token: 0x06000215 RID: 533 RVA: 0x0000D864 File Offset: 0x0000BA64
		public void Write(object value, ProtoWriter dest)
		{
			BclHelpers.WriteDecimal((decimal)value, dest);
		}

		// Token: 0x04000222 RID: 546
		private static readonly Type expectedType = typeof(decimal);
	}
}
