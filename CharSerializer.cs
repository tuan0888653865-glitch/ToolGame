using System;
using ProtoBuf.Meta;

namespace ProtoBuf.Serializers
{
	// Token: 0x0200003D RID: 61
	internal sealed class CharSerializer : UInt16Serializer
	{
		// Token: 0x060001FB RID: 507 RVA: 0x0000D6B8 File Offset: 0x0000B8B8
		public CharSerializer(TypeModel model) : base(model)
		{
		}

		// Token: 0x17000042 RID: 66
		// (get) Token: 0x060001FC RID: 508 RVA: 0x0000D6C1 File Offset: 0x0000B8C1
		public override Type ExpectedType
		{
			get
			{
				return CharSerializer.expectedType;
			}
		}

		// Token: 0x060001FD RID: 509 RVA: 0x0000D6C8 File Offset: 0x0000B8C8
		public override void Write(object value, ProtoWriter dest)
		{
			ProtoWriter.WriteUInt16((ushort)((char)value), dest);
		}

		// Token: 0x060001FE RID: 510 RVA: 0x0000D6D6 File Offset: 0x0000B8D6
		public override object Read(object value, ProtoReader source)
		{
			return (char)source.ReadUInt16();
		}

		// Token: 0x0400021D RID: 541
		private static readonly Type expectedType = typeof(char);
	}
}
