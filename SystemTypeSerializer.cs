using System;
using ProtoBuf.Meta;

namespace ProtoBuf.Serializers
{
	// Token: 0x0200005E RID: 94
	internal class SystemTypeSerializer : IProtoSerializer
	{
		// Token: 0x060002E9 RID: 745 RVA: 0x000020C5 File Offset: 0x000002C5
		public SystemTypeSerializer(TypeModel model)
		{
		}

		// Token: 0x17000092 RID: 146
		// (get) Token: 0x060002EA RID: 746 RVA: 0x000101A4 File Offset: 0x0000E3A4
		public Type ExpectedType
		{
			get
			{
				return SystemTypeSerializer.expectedType;
			}
		}

		// Token: 0x060002EB RID: 747 RVA: 0x000101AB File Offset: 0x0000E3AB
		void IProtoSerializer.Write(object value, ProtoWriter dest)
		{
			ProtoWriter.WriteType((Type)value, dest);
		}

		// Token: 0x060002EC RID: 748 RVA: 0x000101B9 File Offset: 0x0000E3B9
		object IProtoSerializer.Read(object value, ProtoReader source)
		{
			return source.ReadType();
		}

		// Token: 0x17000093 RID: 147
		// (get) Token: 0x060002ED RID: 749 RVA: 0x00008C9D File Offset: 0x00006E9D
		bool IProtoSerializer.RequiresOldValue
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000094 RID: 148
		// (get) Token: 0x060002EE RID: 750 RVA: 0x0000D470 File Offset: 0x0000B670
		bool IProtoSerializer.ReturnsValue
		{
			get
			{
				return true;
			}
		}

		// Token: 0x04000273 RID: 627
		private static readonly Type expectedType = typeof(Type);
	}
}
