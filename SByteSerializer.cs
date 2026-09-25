using System;
using ProtoBuf.Meta;

namespace ProtoBuf.Serializers
{
	// Token: 0x02000058 RID: 88
	internal sealed class SByteSerializer : IProtoSerializer
	{
		// Token: 0x060002C1 RID: 705 RVA: 0x000020C5 File Offset: 0x000002C5
		public SByteSerializer(TypeModel model)
		{
		}

		// Token: 0x17000083 RID: 131
		// (get) Token: 0x060002C2 RID: 706 RVA: 0x0000FEB6 File Offset: 0x0000E0B6
		public Type ExpectedType
		{
			get
			{
				return SByteSerializer.expectedType;
			}
		}

		// Token: 0x17000084 RID: 132
		// (get) Token: 0x060002C3 RID: 707 RVA: 0x00008C9D File Offset: 0x00006E9D
		bool IProtoSerializer.RequiresOldValue
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000085 RID: 133
		// (get) Token: 0x060002C4 RID: 708 RVA: 0x0000D470 File Offset: 0x0000B670
		bool IProtoSerializer.ReturnsValue
		{
			get
			{
				return true;
			}
		}

		// Token: 0x060002C5 RID: 709 RVA: 0x0000FEBD File Offset: 0x0000E0BD
		public object Read(object value, ProtoReader source)
		{
			return source.ReadSByte();
		}

		// Token: 0x060002C6 RID: 710 RVA: 0x0000FECA File Offset: 0x0000E0CA
		public void Write(object value, ProtoWriter dest)
		{
			ProtoWriter.WriteSByte((sbyte)value, dest);
		}

		// Token: 0x04000267 RID: 615
		private static readonly Type expectedType = typeof(sbyte);
	}
}
