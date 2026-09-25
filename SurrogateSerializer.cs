using System;
using System.Reflection;
using ProtoBuf.Meta;

namespace ProtoBuf.Serializers
{
	// Token: 0x0200005D RID: 93
	internal sealed class SurrogateSerializer : IProtoTypeSerializer, IProtoSerializer
	{
		// Token: 0x060002DF RID: 735 RVA: 0x00008C9D File Offset: 0x00006E9D
		bool IProtoTypeSerializer.HasCallbacks(TypeModel.CallbackType callbackType)
		{
			return false;
		}

		// Token: 0x060002E0 RID: 736 RVA: 0x00006740 File Offset: 0x00004940
		void IProtoTypeSerializer.Callback(object value, TypeModel.CallbackType callbackType, SerializationContext context)
		{
		}

		// Token: 0x1700008F RID: 143
		// (get) Token: 0x060002E1 RID: 737 RVA: 0x00008C9D File Offset: 0x00006E9D
		public bool ReturnsValue
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000090 RID: 144
		// (get) Token: 0x060002E2 RID: 738 RVA: 0x0000D470 File Offset: 0x0000B670
		public bool RequiresOldValue
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000091 RID: 145
		// (get) Token: 0x060002E3 RID: 739 RVA: 0x00010004 File Offset: 0x0000E204
		public Type ExpectedType
		{
			get
			{
				return this.forType;
			}
		}

		// Token: 0x060002E4 RID: 740 RVA: 0x0001000C File Offset: 0x0000E20C
		public SurrogateSerializer(Type forType, Type declaredType, IProtoTypeSerializer rootTail)
		{
			this.forType = forType;
			this.declaredType = declaredType;
			this.rootTail = rootTail;
			this.toTail = this.GetConversion(true);
			this.fromTail = this.GetConversion(false);
		}

		// Token: 0x060002E5 RID: 741 RVA: 0x00010044 File Offset: 0x0000E244
		private static bool HasCast(Type type, Type from, Type to, out MethodInfo op)
		{
			foreach (MethodInfo methodInfo in type.GetMethods(BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic))
			{
				if ((!(methodInfo.Name != "op_Implicit") || !(methodInfo.Name != "op_Explicit")) && methodInfo.ReturnType == to)
				{
					ParameterInfo[] parameters = methodInfo.GetParameters();
					if (parameters.Length == 1 && parameters[0].ParameterType == from)
					{
						op = methodInfo;
						return true;
					}
				}
			}
			op = null;
			return false;
		}

		// Token: 0x060002E6 RID: 742 RVA: 0x000100BC File Offset: 0x0000E2BC
		public MethodInfo GetConversion(bool toTail)
		{
			Type to = toTail ? this.declaredType : this.forType;
			Type from = toTail ? this.forType : this.declaredType;
			MethodInfo result;
			if (SurrogateSerializer.HasCast(this.declaredType, from, to, out result) || SurrogateSerializer.HasCast(this.forType, from, to, out result))
			{
				return result;
			}
			throw new InvalidOperationException("No suitable conversion operator found for surrogate: " + this.forType.FullName + " / " + this.declaredType.FullName);
		}

		// Token: 0x060002E7 RID: 743 RVA: 0x0001013B File Offset: 0x0000E33B
		public void Write(object value, ProtoWriter writer)
		{
			this.rootTail.Write(this.toTail.Invoke(null, new object[]
			{
				value
			}), writer);
		}

		// Token: 0x060002E8 RID: 744 RVA: 0x00010160 File Offset: 0x0000E360
		public object Read(object value, ProtoReader source)
		{
			object[] array = new object[]
			{
				value
			};
			value = this.toTail.Invoke(null, array);
			array[0] = this.rootTail.Read(value, source);
			return this.fromTail.Invoke(null, array);
		}

		// Token: 0x0400026E RID: 622
		private readonly Type forType;

		// Token: 0x0400026F RID: 623
		private readonly Type declaredType;

		// Token: 0x04000270 RID: 624
		private readonly MethodInfo toTail;

		// Token: 0x04000271 RID: 625
		private readonly MethodInfo fromTail;

		// Token: 0x04000272 RID: 626
		private IProtoTypeSerializer rootTail;
	}
}
