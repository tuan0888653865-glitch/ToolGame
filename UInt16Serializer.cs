using System;
using ProtoBuf.Meta;

namespace ProtoBuf.Serializers
{
	// Token: 0x02000063 RID: 99
	internal class UInt16Serializer : IProtoSerializer
	{
		// Token: 0x06000317 RID: 791 RVA: 0x000020C5 File Offset: 0x000002C5
		public UInt16Serializer(TypeModel model)
		{
		}

		// Token: 0x170000A3 RID: 163
		// (get) Token: 0x06000318 RID: 792 RVA: 0x00010B30 File Offset: 0x0000ED30
		public virtual Type ExpectedType
		{
			get
			{
				return UInt16Serializer.expectedType;
			}
		}

		// Token: 0x170000A4 RID: 164
		// (get) Token: 0x06000319 RID: 793 RVA: 0x00008C9D File Offset: 0x00006E9D
		bool IProtoSerializer.RequiresOldValue
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170000A5 RID: 165
		// (get) Token: 0x0600031A RID: 794 RVA: 0x0000D470 File Offset: 0x0000B670
		bool IProtoSerializer.ReturnsValue
		{
			get
			{
				return true;
			}
		}

		// Token: 0x0600031B RID: 795 RVA: 0x00010B37 File Offset: 0x0000ED37
		public virtual object Read(object value, ProtoReader source)
		{
			return source.ReadUInt16();
		}

		// Token: 0x0600031C RID: 796 RVA: 0x00010B44 File Offset: 0x0000ED44
		public virtual void Write(object value, ProtoWriter dest)
		{
			ProtoWriter.WriteUInt16((ushort)value, dest);
		}

		// Token: 0x04000287 RID: 647
		private static readonly Type expectedType = typeof(ushort);
	}
}
