using System;
using ProtoBuf.Meta;

namespace ProtoBuf.Serializers
{
	// Token: 0x02000046 RID: 70
	internal sealed class GuidSerializer : IProtoSerializer
	{
		// Token: 0x06000233 RID: 563 RVA: 0x000020C5 File Offset: 0x000002C5
		public GuidSerializer(TypeModel model)
		{
		}

		// Token: 0x17000055 RID: 85
		// (get) Token: 0x06000234 RID: 564 RVA: 0x0000DD3F File Offset: 0x0000BF3F
		public Type ExpectedType
		{
			get
			{
				return GuidSerializer.expectedType;
			}
		}

		// Token: 0x17000056 RID: 86
		// (get) Token: 0x06000235 RID: 565 RVA: 0x00008C9D File Offset: 0x00006E9D
		bool IProtoSerializer.RequiresOldValue
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000057 RID: 87
		// (get) Token: 0x06000236 RID: 566 RVA: 0x0000D470 File Offset: 0x0000B670
		bool IProtoSerializer.ReturnsValue
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06000237 RID: 567 RVA: 0x0000DD46 File Offset: 0x0000BF46
		public void Write(object value, ProtoWriter dest)
		{
			BclHelpers.WriteGuid((Guid)value, dest);
		}

		// Token: 0x06000238 RID: 568 RVA: 0x0000DD54 File Offset: 0x0000BF54
		public object Read(object value, ProtoReader source)
		{
			return BclHelpers.ReadGuid(source);
		}

		// Token: 0x04000229 RID: 553
		private static readonly Type expectedType = typeof(Guid);
	}
}
