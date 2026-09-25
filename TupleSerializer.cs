using System;
using System.Reflection;
using ProtoBuf.Meta;

namespace ProtoBuf.Serializers
{
	// Token: 0x02000061 RID: 97
	internal sealed class TupleSerializer : IProtoTypeSerializer, IProtoSerializer
	{
		// Token: 0x06000300 RID: 768 RVA: 0x000102DC File Offset: 0x0000E4DC
		public TupleSerializer(RuntimeTypeModel model, ConstructorInfo ctor, MemberInfo[] members)
		{
			if (ctor == null)
			{
				throw new ArgumentNullException("ctor");
			}
			if (members == null)
			{
				throw new ArgumentNullException("members");
			}
			this.ctor = ctor;
			this.members = members;
			this.tails = new IProtoSerializer[members.Length];
			ParameterInfo[] parameters = ctor.GetParameters();
			for (int i = 0; i < members.Length; i++)
			{
				Type parameterType = parameters[i].ParameterType;
				Type type = null;
				Type concreteType = null;
				MetaType.ResolveListTypes(model, parameterType, ref type, ref concreteType);
				Type type2 = (type == null) ? parameterType : type;
				WireType wireType;
				IProtoSerializer protoSerializer = ValueMember.TryGetCoreSerializer(model, DataFormat.Default, type2, out wireType, false, false, false, true);
				if (protoSerializer == null)
				{
					throw new InvalidOperationException("No serializer defined for type: " + type2.FullName);
				}
				protoSerializer = new TagDecorator(i + 1, wireType, false, protoSerializer);
				IProtoSerializer protoSerializer2;
				if (type == null)
				{
					protoSerializer2 = protoSerializer;
				}
				else if (parameterType.IsArray)
				{
					protoSerializer2 = new ArrayDecorator(model, protoSerializer, i + 1, false, wireType, parameterType, false, false);
				}
				else
				{
					protoSerializer2 = new ListDecorator(model, parameterType, concreteType, protoSerializer, i + 1, false, wireType, true, false, false);
				}
				this.tails[i] = protoSerializer2;
			}
		}

		// Token: 0x06000301 RID: 769 RVA: 0x00008C9D File Offset: 0x00006E9D
		public bool HasCallbacks(TypeModel.CallbackType callbackType)
		{
			return false;
		}

		// Token: 0x06000302 RID: 770 RVA: 0x00006740 File Offset: 0x00004940
		public void Callback(object value, TypeModel.CallbackType callbackType, SerializationContext context)
		{
		}

		// Token: 0x1700009C RID: 156
		// (get) Token: 0x06000303 RID: 771 RVA: 0x000103E5 File Offset: 0x0000E5E5
		public Type ExpectedType
		{
			get
			{
				return this.ctor.DeclaringType;
			}
		}

		// Token: 0x06000304 RID: 772 RVA: 0x000103F4 File Offset: 0x0000E5F4
		private object GetValue(object obj, int index)
		{
			PropertyInfo propertyInfo;
			if ((propertyInfo = (this.members[index] as PropertyInfo)) != null)
			{
				if (obj != null)
				{
					return propertyInfo.GetValue(obj, null);
				}
				if (!Helpers.IsValueType(propertyInfo.PropertyType))
				{
					return null;
				}
				return Activator.CreateInstance(propertyInfo.PropertyType);
			}
			else
			{
				FieldInfo fieldInfo;
				if ((fieldInfo = (this.members[index] as FieldInfo)) == null)
				{
					throw new InvalidOperationException();
				}
				if (obj != null)
				{
					return fieldInfo.GetValue(obj);
				}
				if (!Helpers.IsValueType(fieldInfo.FieldType))
				{
					return null;
				}
				return Activator.CreateInstance(fieldInfo.FieldType);
			}
		}

		// Token: 0x06000305 RID: 773 RVA: 0x00010478 File Offset: 0x0000E678
		public object Read(object value, ProtoReader source)
		{
			object[] array = new object[this.members.Length];
			bool flag = false;
			if (value == null)
			{
				flag = true;
			}
			for (int i = 0; i < array.Length; i++)
			{
				array[i] = this.GetValue(value, i);
			}
			int num;
			while ((num = source.ReadFieldHeader()) > 0)
			{
				flag = true;
				if (num <= this.tails.Length)
				{
					IProtoSerializer protoSerializer = this.tails[num - 1];
					array[num - 1] = this.tails[num - 1].Read(protoSerializer.RequiresOldValue ? array[num - 1] : null, source);
				}
				else
				{
					source.SkipField();
				}
			}
			if (!flag)
			{
				return value;
			}
			return this.ctor.Invoke(array);
		}

		// Token: 0x06000306 RID: 774 RVA: 0x00010518 File Offset: 0x0000E718
		public void Write(object value, ProtoWriter dest)
		{
			for (int i = 0; i < this.tails.Length; i++)
			{
				object value2 = this.GetValue(value, i);
				if (value2 != null)
				{
					this.tails[i].Write(value2, dest);
				}
			}
		}

		// Token: 0x1700009D RID: 157
		// (get) Token: 0x06000307 RID: 775 RVA: 0x0000D470 File Offset: 0x0000B670
		public bool RequiresOldValue
		{
			get
			{
				return true;
			}
		}

		// Token: 0x1700009E RID: 158
		// (get) Token: 0x06000308 RID: 776 RVA: 0x00008C9D File Offset: 0x00006E9D
		public bool ReturnsValue
		{
			get
			{
				return false;
			}
		}

		// Token: 0x06000309 RID: 777 RVA: 0x00010553 File Offset: 0x0000E753
		private Type GetMemberType(int index)
		{
			Type memberType = Helpers.GetMemberType(this.members[index]);
			if (memberType == null)
			{
				throw new InvalidOperationException();
			}
			return memberType;
		}

		// Token: 0x04000278 RID: 632
		private readonly MemberInfo[] members;

		// Token: 0x04000279 RID: 633
		private readonly ConstructorInfo ctor;

		// Token: 0x0400027A RID: 634
		private IProtoSerializer[] tails;
	}
}
