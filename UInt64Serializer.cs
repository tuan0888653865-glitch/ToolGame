using System;
using ProtoBuf.Meta;

namespace ProtoBuf.Serializers
{
	// Token: 0x02000065 RID: 101
	internal sealed class UInt64Serializer : IProtoSerializer
	{
		// Token: 0x06000325 RID: 805 RVA: 0x000020C5 File Offset: 0x000002C5
		public UInt64Serializer(TypeModel model)
		{
		}

		// Token: 0x170000A9 RID: 169
		// (get) Token: 0x06000326 RID: 806 RVA: 0x00010B96 File Offset: 0x0000ED96
		public Type ExpectedType
		{
			get
			{
				return UInt64Serializer.expectedType;
			}
		}

		// Token: 0x170000AA RID: 170
		// (get) Token: 0x06000327 RID: 807 RVA: 0x00008C9D File Offset: 0x00006E9D
		bool IProtoSerializer.RequiresOldValue
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170000AB RID: 171
		// (get) Token: 0x06000328 RID: 808 RVA: 0x0000D470 File Offset: 0x0000B670
		bool IProtoSerializer.ReturnsValue
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06000329 RID: 809 RVA: 0x00010B9D File Offset: 0x0000ED9D
		public object Read(object value, ProtoReader source)
		{
			return source.ReadUInt64();
		}

		// Token: 0x0600032A RID: 810 RVA: 0x00010BAA File Offset: 0x0000EDAA
		public void Write(object value, ProtoWriter dest)
		{
			ProtoWriter.WriteUInt64((ulong)value, dest);
		}

		// Token: 0x04000289 RID: 649
		private static readonly Type expectedType = typeof(ulong);
	}
}
