using System;
using ProtoBuf.Meta;

namespace ProtoBuf.Serializers
{
	// Token: 0x02000047 RID: 71
	internal sealed class Int16Serializer : IProtoSerializer
	{
		// Token: 0x0600023A RID: 570 RVA: 0x000020C5 File Offset: 0x000002C5
		public Int16Serializer(TypeModel model)
		{
		}

		// Token: 0x17000058 RID: 88
		// (get) Token: 0x0600023B RID: 571 RVA: 0x0000DD72 File Offset: 0x0000BF72
		public Type ExpectedType
		{
			get
			{
				return Int16Serializer.expectedType;
			}
		}

		// Token: 0x17000059 RID: 89
		// (get) Token: 0x0600023C RID: 572 RVA: 0x00008C9D File Offset: 0x00006E9D
		bool IProtoSerializer.RequiresOldValue
		{
			get
			{
				return false;
			}
		}

		// Token: 0x1700005A RID: 90
		// (get) Token: 0x0600023D RID: 573 RVA: 0x0000D470 File Offset: 0x0000B670
		bool IProtoSerializer.ReturnsValue
		{
			get
			{
				return true;
			}
		}

		// Token: 0x0600023E RID: 574 RVA: 0x0000DD79 File Offset: 0x0000BF79
		public object Read(object value, ProtoReader source)
		{
			return source.ReadInt16();
		}

		// Token: 0x0600023F RID: 575 RVA: 0x0000DD86 File Offset: 0x0000BF86
		public void Write(object value, ProtoWriter dest)
		{
			ProtoWriter.WriteInt16((short)value, dest);
		}

		// Token: 0x0400022A RID: 554
		private static readonly Type expectedType = typeof(short);
	}
}
