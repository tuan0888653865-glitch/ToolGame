using System;
using ProtoBuf.Meta;

namespace ProtoBuf.Serializers
{
	// Token: 0x02000060 RID: 96
	internal sealed class TimeSpanSerializer : IProtoSerializer
	{
		// Token: 0x060002F9 RID: 761 RVA: 0x000020C5 File Offset: 0x000002C5
		public TimeSpanSerializer(TypeModel model)
		{
		}

		// Token: 0x17000099 RID: 153
		// (get) Token: 0x060002FA RID: 762 RVA: 0x000102A8 File Offset: 0x0000E4A8
		public Type ExpectedType
		{
			get
			{
				return TimeSpanSerializer.expectedType;
			}
		}

		// Token: 0x1700009A RID: 154
		// (get) Token: 0x060002FB RID: 763 RVA: 0x00008C9D File Offset: 0x00006E9D
		bool IProtoSerializer.RequiresOldValue
		{
			get
			{
				return false;
			}
		}

		// Token: 0x1700009B RID: 155
		// (get) Token: 0x060002FC RID: 764 RVA: 0x0000D470 File Offset: 0x0000B670
		bool IProtoSerializer.ReturnsValue
		{
			get
			{
				return true;
			}
		}

		// Token: 0x060002FD RID: 765 RVA: 0x000102AF File Offset: 0x0000E4AF
		public object Read(object value, ProtoReader source)
		{
			return BclHelpers.ReadTimeSpan(source);
		}

		// Token: 0x060002FE RID: 766 RVA: 0x000102BC File Offset: 0x0000E4BC
		public void Write(object value, ProtoWriter dest)
		{
			BclHelpers.WriteTimeSpan((TimeSpan)value, dest);
		}

		// Token: 0x04000277 RID: 631
		private static readonly Type expectedType = typeof(TimeSpan);
	}
}
