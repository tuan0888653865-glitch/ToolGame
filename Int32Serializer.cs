using System;
using ProtoBuf.Meta;

namespace ProtoBuf.Serializers
{
	// Token: 0x02000048 RID: 72
	internal sealed class Int32Serializer : IProtoSerializer
	{
		// Token: 0x06000241 RID: 577 RVA: 0x000020C5 File Offset: 0x000002C5
		public Int32Serializer(TypeModel model)
		{
		}

		// Token: 0x1700005B RID: 91
		// (get) Token: 0x06000242 RID: 578 RVA: 0x0000DDA5 File Offset: 0x0000BFA5
		public Type ExpectedType
		{
			get
			{
				return Int32Serializer.expectedType;
			}
		}

		// Token: 0x1700005C RID: 92
		// (get) Token: 0x06000243 RID: 579 RVA: 0x00008C9D File Offset: 0x00006E9D
		bool IProtoSerializer.RequiresOldValue
		{
			get
			{
				return false;
			}
		}

		// Token: 0x1700005D RID: 93
		// (get) Token: 0x06000244 RID: 580 RVA: 0x0000D470 File Offset: 0x0000B670
		bool IProtoSerializer.ReturnsValue
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06000245 RID: 581 RVA: 0x0000DDAC File Offset: 0x0000BFAC
		public object Read(object value, ProtoReader source)
		{
			return source.ReadInt32();
		}

		// Token: 0x06000246 RID: 582 RVA: 0x0000DDB9 File Offset: 0x0000BFB9
		public void Write(object value, ProtoWriter dest)
		{
			ProtoWriter.WriteInt32((int)value, dest);
		}

		// Token: 0x0400022B RID: 555
		private static readonly Type expectedType = typeof(int);
	}
}
