using System;
using ProtoBuf.Meta;

namespace ProtoBuf.Serializers
{
	// Token: 0x02000040 RID: 64
	internal sealed class DateTimeSerializer : IProtoSerializer
	{
		// Token: 0x17000043 RID: 67
		// (get) Token: 0x06000209 RID: 521 RVA: 0x0000D81D File Offset: 0x0000BA1D
		public Type ExpectedType
		{
			get
			{
				return DateTimeSerializer.expectedType;
			}
		}

		// Token: 0x17000044 RID: 68
		// (get) Token: 0x0600020A RID: 522 RVA: 0x00008C9D File Offset: 0x00006E9D
		bool IProtoSerializer.RequiresOldValue
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000045 RID: 69
		// (get) Token: 0x0600020B RID: 523 RVA: 0x0000D470 File Offset: 0x0000B670
		bool IProtoSerializer.ReturnsValue
		{
			get
			{
				return true;
			}
		}

		// Token: 0x0600020C RID: 524 RVA: 0x000020C5 File Offset: 0x000002C5
		public DateTimeSerializer(TypeModel model)
		{
		}

		// Token: 0x0600020D RID: 525 RVA: 0x0000D824 File Offset: 0x0000BA24
		public object Read(object value, ProtoReader source)
		{
			return BclHelpers.ReadDateTime(source);
		}

		// Token: 0x0600020E RID: 526 RVA: 0x0000D831 File Offset: 0x0000BA31
		public void Write(object value, ProtoWriter dest)
		{
			BclHelpers.WriteDateTime((DateTime)value, dest);
		}

		// Token: 0x04000221 RID: 545
		private static readonly Type expectedType = typeof(DateTime);
	}
}
