using System;
using ProtoBuf.Meta;

namespace ProtoBuf.Serializers
{
	// Token: 0x0200004B RID: 75
	internal interface IProtoTypeSerializer : IProtoSerializer
	{
		// Token: 0x06000254 RID: 596
		bool HasCallbacks(TypeModel.CallbackType callbackType);

		// Token: 0x06000255 RID: 597
		void Callback(object value, TypeModel.CallbackType callbackType, SerializationContext context);
	}
}
