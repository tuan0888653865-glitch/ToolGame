using System;
using ProtoBuf.Meta;

namespace ProtoBuf.Serializers
{
	// Token: 0x02000059 RID: 89
	internal sealed class SingleSerializer : IProtoSerializer
	{
		// Token: 0x17000086 RID: 134
		// (get) Token: 0x060002C8 RID: 712 RVA: 0x0000FEE9 File Offset: 0x0000E0E9
		public Type ExpectedType
		{
			get
			{
				return SingleSerializer.expectedType;
			}
		}

		// Token: 0x060002C9 RID: 713 RVA: 0x000020C5 File Offset: 0x000002C5
		public SingleSerializer(TypeModel model)
		{
		}

		// Token: 0x17000087 RID: 135
		// (get) Token: 0x060002CA RID: 714 RVA: 0x00008C9D File Offset: 0x00006E9D
		bool IProtoSerializer.RequiresOldValue
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000088 RID: 136
		// (get) Token: 0x060002CB RID: 715 RVA: 0x0000D470 File Offset: 0x0000B670
		bool IProtoSerializer.ReturnsValue
		{
			get
			{
				return true;
			}
		}

		// Token: 0x060002CC RID: 716 RVA: 0x0000FEF0 File Offset: 0x0000E0F0
		public object Read(object value, ProtoReader source)
		{
			return source.ReadSingle();
		}

		// Token: 0x060002CD RID: 717 RVA: 0x0000FEFD File Offset: 0x0000E0FD
		public void Write(object value, ProtoWriter dest)
		{
			ProtoWriter.WriteSingle((float)value, dest);
		}

		// Token: 0x04000268 RID: 616
		private static readonly Type expectedType = typeof(float);
	}
}
