using System;
using ProtoBuf.Meta;

namespace ProtoBuf.Serializers
{
	// Token: 0x0200005C RID: 92
	internal sealed class SubItemSerializer : IProtoTypeSerializer, IProtoSerializer
	{
		// Token: 0x060002D7 RID: 727 RVA: 0x0000FF4A File Offset: 0x0000E14A
		bool IProtoTypeSerializer.HasCallbacks(TypeModel.CallbackType callbackType)
		{
			return ((IProtoTypeSerializer)this.proxy.Serializer).HasCallbacks(callbackType);
		}

		// Token: 0x060002D8 RID: 728 RVA: 0x0000FF62 File Offset: 0x0000E162
		void IProtoTypeSerializer.Callback(object value, TypeModel.CallbackType callbackType, SerializationContext context)
		{
			((IProtoTypeSerializer)this.proxy.Serializer).Callback(value, callbackType, context);
		}

		// Token: 0x060002D9 RID: 729 RVA: 0x0000FF7C File Offset: 0x0000E17C
		public SubItemSerializer(Type type, int key, ISerializerProxy proxy, bool recursionCheck)
		{
			if (type == null)
			{
				throw new ArgumentNullException("type");
			}
			if (proxy == null)
			{
				throw new ArgumentNullException("proxy");
			}
			this.type = type;
			this.proxy = proxy;
			this.key = key;
			this.recursionCheck = recursionCheck;
		}

		// Token: 0x1700008C RID: 140
		// (get) Token: 0x060002DA RID: 730 RVA: 0x0000FFC8 File Offset: 0x0000E1C8
		Type IProtoSerializer.ExpectedType
		{
			get
			{
				return this.type;
			}
		}

		// Token: 0x1700008D RID: 141
		// (get) Token: 0x060002DB RID: 731 RVA: 0x0000D470 File Offset: 0x0000B670
		bool IProtoSerializer.RequiresOldValue
		{
			get
			{
				return true;
			}
		}

		// Token: 0x1700008E RID: 142
		// (get) Token: 0x060002DC RID: 732 RVA: 0x0000D470 File Offset: 0x0000B670
		bool IProtoSerializer.ReturnsValue
		{
			get
			{
				return true;
			}
		}

		// Token: 0x060002DD RID: 733 RVA: 0x0000FFD0 File Offset: 0x0000E1D0
		void IProtoSerializer.Write(object value, ProtoWriter dest)
		{
			if (this.recursionCheck)
			{
				ProtoWriter.WriteObject(value, this.key, dest);
				return;
			}
			ProtoWriter.WriteRecursionSafeObject(value, this.key, dest);
		}

		// Token: 0x060002DE RID: 734 RVA: 0x0000FFF5 File Offset: 0x0000E1F5
		object IProtoSerializer.Read(object value, ProtoReader source)
		{
			return ProtoReader.ReadObject(value, this.key, source);
		}

		// Token: 0x0400026A RID: 618
		private readonly int key;

		// Token: 0x0400026B RID: 619
		private readonly Type type;

		// Token: 0x0400026C RID: 620
		private readonly ISerializerProxy proxy;

		// Token: 0x0400026D RID: 621
		private readonly bool recursionCheck;
	}
}
