using System;
using System.Reflection;
using ProtoBuf.Meta;

namespace ProtoBuf.Serializers
{
	// Token: 0x02000062 RID: 98
	internal sealed class TypeSerializer : IProtoTypeSerializer, IProtoSerializer
	{
		// Token: 0x0600030A RID: 778 RVA: 0x0001056C File Offset: 0x0000E76C
		public bool HasCallbacks(TypeModel.CallbackType callbackType)
		{
			if (this.callbacks != null && this.callbacks[callbackType] != null)
			{
				return true;
			}
			for (int i = 0; i < this.serializers.Length; i++)
			{
				if (this.serializers[i].ExpectedType != this.forType && ((IProtoTypeSerializer)this.serializers[i]).HasCallbacks(callbackType))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x1700009F RID: 159
		// (get) Token: 0x0600030B RID: 779 RVA: 0x000105D1 File Offset: 0x0000E7D1
		public Type ExpectedType
		{
			get
			{
				return this.forType;
			}
		}

		// Token: 0x0600030C RID: 780 RVA: 0x000105DC File Offset: 0x0000E7DC
		public TypeSerializer(TypeModel model, Type forType, int[] fieldNumbers, IProtoSerializer[] serializers, MethodInfo[] baseCtorCallbacks, bool isRootType, bool useConstructor, CallbackSet callbacks, Type constructType, MethodInfo factory)
		{
			Helpers.Sort(fieldNumbers, serializers);
			bool flag = false;
			for (int i = 1; i < fieldNumbers.Length; i++)
			{
				if (fieldNumbers[i] == fieldNumbers[i - 1])
				{
					throw new InvalidOperationException("Duplicate field-number detected; " + fieldNumbers[i].ToString() + " on: " + forType.FullName);
				}
				if (!flag && serializers[i].ExpectedType != forType)
				{
					flag = true;
				}
			}
			this.forType = forType;
			this.factory = factory;
			if (constructType == null)
			{
				constructType = forType;
			}
			else if (!forType.IsAssignableFrom(constructType))
			{
				throw new InvalidOperationException(forType.FullName + " cannot be assigned from " + constructType.FullName);
			}
			this.constructType = constructType;
			this.serializers = serializers;
			this.fieldNumbers = fieldNumbers;
			this.callbacks = callbacks;
			this.isRootType = isRootType;
			this.useConstructor = useConstructor;
			if (baseCtorCallbacks != null && baseCtorCallbacks.Length == 0)
			{
				baseCtorCallbacks = null;
			}
			this.baseCtorCallbacks = baseCtorCallbacks;
			if (Helpers.GetUnderlyingType(forType) != null)
			{
				throw new ArgumentException("Cannot create a TypeSerializer for nullable types", "forType");
			}
			if (model.MapType(TypeSerializer.iextensible).IsAssignableFrom(forType))
			{
				if (forType.IsValueType || !isRootType || flag)
				{
					throw new NotSupportedException("IExtensible is not supported in structs or classes with inheritance");
				}
				this.isExtensible = true;
			}
			this.hasConstructor = (!constructType.IsAbstract && Helpers.GetConstructor(constructType, Helpers.EmptyTypes, true) != null);
			if (constructType != forType && useConstructor && !this.hasConstructor)
			{
				throw new ArgumentException("The supplied default implementation cannot be created: " + constructType.FullName, "constructType");
			}
		}

		// Token: 0x170000A0 RID: 160
		// (get) Token: 0x0600030D RID: 781 RVA: 0x00010771 File Offset: 0x0000E971
		private bool CanHaveInheritance
		{
			get
			{
				return (this.forType.IsClass || this.forType.IsInterface) && !this.forType.IsSealed;
			}
		}

		// Token: 0x0600030E RID: 782 RVA: 0x000107A0 File Offset: 0x0000E9A0
		public void Callback(object value, TypeModel.CallbackType callbackType, SerializationContext context)
		{
			if (this.callbacks != null)
			{
				this.InvokeCallback(this.callbacks[callbackType], value, context);
			}
			IProtoTypeSerializer protoTypeSerializer = (IProtoTypeSerializer)this.GetMoreSpecificSerializer(value);
			if (protoTypeSerializer != null)
			{
				protoTypeSerializer.Callback(value, callbackType, context);
			}
		}

		// Token: 0x0600030F RID: 783 RVA: 0x000107E4 File Offset: 0x0000E9E4
		private IProtoSerializer GetMoreSpecificSerializer(object value)
		{
			if (!this.CanHaveInheritance)
			{
				return null;
			}
			Type type = value.GetType();
			if (type == this.forType)
			{
				return null;
			}
			for (int i = 0; i < this.serializers.Length; i++)
			{
				IProtoSerializer protoSerializer = this.serializers[i];
				if (protoSerializer.ExpectedType != this.forType && Helpers.IsAssignableFrom(protoSerializer.ExpectedType, type))
				{
					return protoSerializer;
				}
			}
			if (type == this.constructType)
			{
				return null;
			}
			TypeModel.ThrowUnexpectedSubtype(this.forType, type);
			return null;
		}

		// Token: 0x06000310 RID: 784 RVA: 0x00010860 File Offset: 0x0000EA60
		public void Write(object value, ProtoWriter dest)
		{
			if (this.isRootType)
			{
				this.Callback(value, TypeModel.CallbackType.BeforeSerialize, dest.Context);
			}
			IProtoSerializer moreSpecificSerializer = this.GetMoreSpecificSerializer(value);
			if (moreSpecificSerializer != null)
			{
				moreSpecificSerializer.Write(value, dest);
			}
			for (int i = 0; i < this.serializers.Length; i++)
			{
				IProtoSerializer protoSerializer = this.serializers[i];
				if (protoSerializer.ExpectedType == this.forType)
				{
					protoSerializer.Write(value, dest);
				}
			}
			if (this.isExtensible)
			{
				ProtoWriter.AppendExtensionData((IExtensible)value, dest);
			}
			if (this.isRootType)
			{
				this.Callback(value, TypeModel.CallbackType.AfterSerialize, dest.Context);
			}
		}

		// Token: 0x06000311 RID: 785 RVA: 0x000108F4 File Offset: 0x0000EAF4
		public object Read(object value, ProtoReader source)
		{
			if (this.isRootType && value != null)
			{
				this.Callback(value, TypeModel.CallbackType.BeforeDeserialize, source.Context);
			}
			int num = 0;
			int num2 = 0;
			int num3;
			while ((num3 = source.ReadFieldHeader()) > 0)
			{
				bool flag = false;
				if (num3 < num)
				{
					num = (num2 = 0);
				}
				for (int i = num2; i < this.fieldNumbers.Length; i++)
				{
					if (this.fieldNumbers[i] == num3)
					{
						IProtoSerializer protoSerializer = this.serializers[i];
						if (value == null && protoSerializer.ExpectedType == this.forType)
						{
							value = this.CreateInstance(source);
						}
						if (protoSerializer.ReturnsValue)
						{
							value = protoSerializer.Read(value, source);
						}
						else
						{
							protoSerializer.Read(value, source);
						}
						num2 = i;
						num = num3;
						flag = true;
						break;
					}
				}
				if (!flag)
				{
					if (value == null)
					{
						value = this.CreateInstance(source);
					}
					if (this.isExtensible)
					{
						source.AppendExtensionData((IExtensible)value);
					}
					else
					{
						source.SkipField();
					}
				}
			}
			if (value == null)
			{
				value = this.CreateInstance(source);
			}
			if (this.isRootType)
			{
				this.Callback(value, TypeModel.CallbackType.AfterDeserialize, source.Context);
			}
			return value;
		}

		// Token: 0x06000312 RID: 786 RVA: 0x000109FC File Offset: 0x0000EBFC
		private object InvokeCallback(MethodInfo method, object obj, SerializationContext context)
		{
			object result = null;
			if (method != null)
			{
				bool flag = false;
				ParameterInfo[] parameters = method.GetParameters();
				int num = parameters.Length;
				if (num != 0)
				{
					if (num == 1)
					{
						if (parameters[0].ParameterType == typeof(SerializationContext))
						{
							result = method.Invoke(obj, new object[]
							{
								context
							});
							flag = true;
						}
					}
				}
				else
				{
					result = method.Invoke(obj, null);
					flag = true;
				}
				if (!flag)
				{
					throw CallbackSet.CreateInvalidCallbackSignature(method);
				}
			}
			return result;
		}

		// Token: 0x06000313 RID: 787 RVA: 0x00010A64 File Offset: 0x0000EC64
		private object CreateInstance(ProtoReader source)
		{
			object obj;
			if (this.factory != null)
			{
				obj = this.InvokeCallback(this.factory, null, source.Context);
			}
			else if (this.useConstructor)
			{
				if (!this.hasConstructor)
				{
					TypeModel.ThrowCannotCreateInstance(this.constructType);
				}
				obj = Activator.CreateInstance(this.constructType, true);
			}
			else
			{
				obj = BclHelpers.GetUninitializedObject(this.constructType);
			}
			ProtoReader.NoteObject(obj, source);
			if (this.baseCtorCallbacks != null)
			{
				for (int i = 0; i < this.baseCtorCallbacks.Length; i++)
				{
					this.InvokeCallback(this.baseCtorCallbacks[i], obj, source.Context);
				}
			}
			if (this.callbacks != null)
			{
				this.InvokeCallback(this.callbacks.BeforeDeserialize, obj, source.Context);
			}
			return obj;
		}

		// Token: 0x170000A1 RID: 161
		// (get) Token: 0x06000314 RID: 788 RVA: 0x0000D470 File Offset: 0x0000B670
		bool IProtoSerializer.RequiresOldValue
		{
			get
			{
				return true;
			}
		}

		// Token: 0x170000A2 RID: 162
		// (get) Token: 0x06000315 RID: 789 RVA: 0x00008C9D File Offset: 0x00006E9D
		bool IProtoSerializer.ReturnsValue
		{
			get
			{
				return false;
			}
		}

		// Token: 0x0400027B RID: 635
		private readonly Type forType;

		// Token: 0x0400027C RID: 636
		private readonly Type constructType;

		// Token: 0x0400027D RID: 637
		private readonly IProtoSerializer[] serializers;

		// Token: 0x0400027E RID: 638
		private readonly int[] fieldNumbers;

		// Token: 0x0400027F RID: 639
		private readonly bool isRootType;

		// Token: 0x04000280 RID: 640
		private readonly bool useConstructor;

		// Token: 0x04000281 RID: 641
		private readonly bool isExtensible;

		// Token: 0x04000282 RID: 642
		private readonly bool hasConstructor;

		// Token: 0x04000283 RID: 643
		private readonly CallbackSet callbacks;

		// Token: 0x04000284 RID: 644
		private readonly MethodInfo[] baseCtorCallbacks;

		// Token: 0x04000285 RID: 645
		private readonly MethodInfo factory;

		// Token: 0x04000286 RID: 646
		private static readonly Type iextensible = typeof(IExtensible);
	}
}
