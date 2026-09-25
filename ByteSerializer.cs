using System;
using ProtoBuf.Meta;

namespace ProtoBuf.Serializers
{
	// Token: 0x0200003C RID: 60
	internal sealed class ByteSerializer : IProtoSerializer
	{
		// Token: 0x1700003F RID: 63
		// (get) Token: 0x060001F4 RID: 500 RVA: 0x0000D685 File Offset: 0x0000B885
		public Type ExpectedType
		{
			get
			{
				return ByteSerializer.expectedType;
			}
		}

		// Token: 0x060001F5 RID: 501 RVA: 0x000020C5 File Offset: 0x000002C5
		public ByteSerializer(TypeModel model)
		{
		}

		// Token: 0x17000040 RID: 64
		// (get) Token: 0x060001F6 RID: 502 RVA: 0x00008C9D File Offset: 0x00006E9D
		bool IProtoSerializer.RequiresOldValue
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000041 RID: 65
		// (get) Token: 0x060001F7 RID: 503 RVA: 0x0000D470 File Offset: 0x0000B670
		bool IProtoSerializer.ReturnsValue
		{
			get
			{
				return true;
			}
		}

		// Token: 0x060001F8 RID: 504 RVA: 0x0000D68C File Offset: 0x0000B88C
		public void Write(object value, ProtoWriter dest)
		{
			ProtoWriter.WriteByte((byte)value, dest);
		}

		// Token: 0x060001F9 RID: 505 RVA: 0x0000D69A File Offset: 0x0000B89A
		public object Read(object value, ProtoReader source)
		{
			return source.ReadByte();
		}

		// Token: 0x0400021C RID: 540
		private static readonly Type expectedType = typeof(byte);
	}
}
