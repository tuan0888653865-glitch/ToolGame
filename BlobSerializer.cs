using System;
using ProtoBuf.Meta;

namespace ProtoBuf.Serializers
{
	// Token: 0x0200003A RID: 58
	internal sealed class BlobSerializer : IProtoSerializer
	{
		// Token: 0x17000039 RID: 57
		// (get) Token: 0x060001E6 RID: 486 RVA: 0x0000D5F9 File Offset: 0x0000B7F9
		public Type ExpectedType
		{
			get
			{
				return BlobSerializer.expectedType;
			}
		}

		// Token: 0x060001E7 RID: 487 RVA: 0x0000D600 File Offset: 0x0000B800
		public BlobSerializer(TypeModel model, bool overwriteList)
		{
			this.overwriteList = overwriteList;
		}

		// Token: 0x060001E8 RID: 488 RVA: 0x0000D60F File Offset: 0x0000B80F
		public object Read(object value, ProtoReader source)
		{
			return ProtoReader.AppendBytes(this.overwriteList ? null : ((byte[])value), source);
		}

		// Token: 0x060001E9 RID: 489 RVA: 0x0000D628 File Offset: 0x0000B828
		public void Write(object value, ProtoWriter dest)
		{
			ProtoWriter.WriteBytes((byte[])value, dest);
		}

		// Token: 0x1700003A RID: 58
		// (get) Token: 0x060001EA RID: 490 RVA: 0x0000D636 File Offset: 0x0000B836
		bool IProtoSerializer.RequiresOldValue
		{
			get
			{
				return !this.overwriteList;
			}
		}

		// Token: 0x1700003B RID: 59
		// (get) Token: 0x060001EB RID: 491 RVA: 0x0000D470 File Offset: 0x0000B670
		bool IProtoSerializer.ReturnsValue
		{
			get
			{
				return true;
			}
		}

		// Token: 0x04000219 RID: 537
		private static readonly Type expectedType = typeof(byte[]);

		// Token: 0x0400021A RID: 538
		private readonly bool overwriteList;
	}
}
