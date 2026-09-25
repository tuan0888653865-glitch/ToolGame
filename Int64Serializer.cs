using System;
using ProtoBuf.Meta;

namespace ProtoBuf.Serializers
{
	// Token: 0x02000049 RID: 73
	internal sealed class Int64Serializer : IProtoSerializer
	{
		// Token: 0x06000248 RID: 584 RVA: 0x000020C5 File Offset: 0x000002C5
		public Int64Serializer(TypeModel model)
		{
		}

		// Token: 0x1700005E RID: 94
		// (get) Token: 0x06000249 RID: 585 RVA: 0x0000DDD8 File Offset: 0x0000BFD8
		public Type ExpectedType
		{
			get
			{
				return Int64Serializer.expectedType;
			}
		}

		// Token: 0x1700005F RID: 95
		// (get) Token: 0x0600024A RID: 586 RVA: 0x00008C9D File Offset: 0x00006E9D
		bool IProtoSerializer.RequiresOldValue
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000060 RID: 96
		// (get) Token: 0x0600024B RID: 587 RVA: 0x0000D470 File Offset: 0x0000B670
		bool IProtoSerializer.ReturnsValue
		{
			get
			{
				return true;
			}
		}

		// Token: 0x0600024C RID: 588 RVA: 0x0000DDDF File Offset: 0x0000BFDF
		public object Read(object value, ProtoReader source)
		{
			return source.ReadInt64();
		}

		// Token: 0x0600024D RID: 589 RVA: 0x0000DDEC File Offset: 0x0000BFEC
		public void Write(object value, ProtoWriter dest)
		{
			ProtoWriter.WriteInt64((long)value, dest);
		}

		// Token: 0x0400022C RID: 556
		private static readonly Type expectedType = typeof(long);
	}
}
