using System;
using ProtoBuf.Meta;

namespace ProtoBuf.Serializers
{
	// Token: 0x02000064 RID: 100
	internal sealed class UInt32Serializer : IProtoSerializer
	{
		// Token: 0x0600031E RID: 798 RVA: 0x000020C5 File Offset: 0x000002C5
		public UInt32Serializer(TypeModel model)
		{
		}

		// Token: 0x170000A6 RID: 166
		// (get) Token: 0x0600031F RID: 799 RVA: 0x00010B63 File Offset: 0x0000ED63
		public Type ExpectedType
		{
			get
			{
				return UInt32Serializer.expectedType;
			}
		}

		// Token: 0x170000A7 RID: 167
		// (get) Token: 0x06000320 RID: 800 RVA: 0x00008C9D File Offset: 0x00006E9D
		bool IProtoSerializer.RequiresOldValue
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170000A8 RID: 168
		// (get) Token: 0x06000321 RID: 801 RVA: 0x0000D470 File Offset: 0x0000B670
		bool IProtoSerializer.ReturnsValue
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06000322 RID: 802 RVA: 0x00010B6A File Offset: 0x0000ED6A
		public object Read(object value, ProtoReader source)
		{
			return source.ReadUInt32();
		}

		// Token: 0x06000323 RID: 803 RVA: 0x00010B77 File Offset: 0x0000ED77
		public void Write(object value, ProtoWriter dest)
		{
			ProtoWriter.WriteUInt32((uint)value, dest);
		}

		// Token: 0x04000288 RID: 648
		private static readonly Type expectedType = typeof(uint);
	}
}
