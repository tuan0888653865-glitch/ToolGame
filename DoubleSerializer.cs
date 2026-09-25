using System;
using ProtoBuf.Meta;

namespace ProtoBuf.Serializers
{
	// Token: 0x02000043 RID: 67
	internal sealed class DoubleSerializer : IProtoSerializer
	{
		// Token: 0x0600021D RID: 541 RVA: 0x000020C5 File Offset: 0x000002C5
		public DoubleSerializer(TypeModel model)
		{
		}

		// Token: 0x1700004C RID: 76
		// (get) Token: 0x0600021E RID: 542 RVA: 0x0000D925 File Offset: 0x0000BB25
		public Type ExpectedType
		{
			get
			{
				return DoubleSerializer.expectedType;
			}
		}

		// Token: 0x1700004D RID: 77
		// (get) Token: 0x0600021F RID: 543 RVA: 0x00008C9D File Offset: 0x00006E9D
		bool IProtoSerializer.RequiresOldValue
		{
			get
			{
				return false;
			}
		}

		// Token: 0x1700004E RID: 78
		// (get) Token: 0x06000220 RID: 544 RVA: 0x0000D470 File Offset: 0x0000B670
		bool IProtoSerializer.ReturnsValue
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06000221 RID: 545 RVA: 0x0000D92C File Offset: 0x0000BB2C
		public object Read(object value, ProtoReader source)
		{
			return source.ReadDouble();
		}

		// Token: 0x06000222 RID: 546 RVA: 0x0000D939 File Offset: 0x0000BB39
		public void Write(object value, ProtoWriter dest)
		{
			ProtoWriter.WriteDouble((double)value, dest);
		}

		// Token: 0x04000224 RID: 548
		private static readonly Type expectedType = typeof(double);
	}
}
