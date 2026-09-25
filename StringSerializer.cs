using System;
using ProtoBuf.Meta;

namespace ProtoBuf.Serializers
{
	// Token: 0x0200005B RID: 91
	internal sealed class StringSerializer : IProtoSerializer
	{
		// Token: 0x060002D0 RID: 720 RVA: 0x000020C5 File Offset: 0x000002C5
		public StringSerializer(TypeModel model)
		{
		}

		// Token: 0x17000089 RID: 137
		// (get) Token: 0x060002D1 RID: 721 RVA: 0x0000FF1C File Offset: 0x0000E11C
		public Type ExpectedType
		{
			get
			{
				return StringSerializer.expectedType;
			}
		}

		// Token: 0x060002D2 RID: 722 RVA: 0x0000FF23 File Offset: 0x0000E123
		public void Write(object value, ProtoWriter dest)
		{
			ProtoWriter.WriteString((string)value, dest);
		}

		// Token: 0x1700008A RID: 138
		// (get) Token: 0x060002D3 RID: 723 RVA: 0x00008C9D File Offset: 0x00006E9D
		bool IProtoSerializer.RequiresOldValue
		{
			get
			{
				return false;
			}
		}

		// Token: 0x1700008B RID: 139
		// (get) Token: 0x060002D4 RID: 724 RVA: 0x0000D470 File Offset: 0x0000B670
		bool IProtoSerializer.ReturnsValue
		{
			get
			{
				return true;
			}
		}

		// Token: 0x060002D5 RID: 725 RVA: 0x0000FF31 File Offset: 0x0000E131
		public object Read(object value, ProtoReader source)
		{
			return source.ReadString();
		}

		// Token: 0x04000269 RID: 617
		private static readonly Type expectedType = typeof(string);
	}
}
