using System;
using ProtoBuf.Meta;

namespace ProtoBuf.Serializers
{
	// Token: 0x02000053 RID: 83
	internal sealed class NetObjectSerializer : IProtoSerializer
	{
		// Token: 0x0600029E RID: 670 RVA: 0x0000FA34 File Offset: 0x0000DC34
		public NetObjectSerializer(TypeModel model, Type type, int key, BclHelpers.NetObjectOptions options)
		{
			bool flag = (options & BclHelpers.NetObjectOptions.DynamicType) > BclHelpers.NetObjectOptions.None;
			this.key = (flag ? -1 : key);
			this.type = (flag ? model.MapType(typeof(object)) : type);
			this.options = options;
		}

		// Token: 0x17000074 RID: 116
		// (get) Token: 0x0600029F RID: 671 RVA: 0x0000FA80 File Offset: 0x0000DC80
		public Type ExpectedType
		{
			get
			{
				return this.type;
			}
		}

		// Token: 0x17000075 RID: 117
		// (get) Token: 0x060002A0 RID: 672 RVA: 0x0000D470 File Offset: 0x0000B670
		public bool ReturnsValue
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000076 RID: 118
		// (get) Token: 0x060002A1 RID: 673 RVA: 0x0000D470 File Offset: 0x0000B670
		public bool RequiresOldValue
		{
			get
			{
				return true;
			}
		}

		// Token: 0x060002A2 RID: 674 RVA: 0x0000FA88 File Offset: 0x0000DC88
		public object Read(object value, ProtoReader source)
		{
			return BclHelpers.ReadNetObject(value, source, this.key, (this.type == typeof(object)) ? null : this.type, this.options);
		}

		// Token: 0x060002A3 RID: 675 RVA: 0x0000FAB8 File Offset: 0x0000DCB8
		public void Write(object value, ProtoWriter dest)
		{
			BclHelpers.WriteNetObject(value, dest, this.key, this.options);
		}

		// Token: 0x0400025C RID: 604
		private readonly int key;

		// Token: 0x0400025D RID: 605
		private readonly Type type;

		// Token: 0x0400025E RID: 606
		private readonly BclHelpers.NetObjectOptions options;
	}
}
