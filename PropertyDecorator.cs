using System;
using System.Reflection;
using ProtoBuf.Meta;

namespace ProtoBuf.Serializers
{
	// Token: 0x02000056 RID: 86
	internal sealed class PropertyDecorator : ProtoDecoratorBase
	{
		// Token: 0x1700007D RID: 125
		// (get) Token: 0x060002B2 RID: 690 RVA: 0x0000FC89 File Offset: 0x0000DE89
		public override Type ExpectedType
		{
			get
			{
				return this.forType;
			}
		}

		// Token: 0x1700007E RID: 126
		// (get) Token: 0x060002B3 RID: 691 RVA: 0x0000D470 File Offset: 0x0000B670
		public override bool RequiresOldValue
		{
			get
			{
				return true;
			}
		}

		// Token: 0x1700007F RID: 127
		// (get) Token: 0x060002B4 RID: 692 RVA: 0x00008C9D File Offset: 0x00006E9D
		public override bool ReturnsValue
		{
			get
			{
				return false;
			}
		}

		// Token: 0x060002B5 RID: 693 RVA: 0x0000FC91 File Offset: 0x0000DE91
		public PropertyDecorator(TypeModel model, Type forType, PropertyInfo property, IProtoSerializer tail) : base(tail)
		{
			this.forType = forType;
			this.property = property;
			PropertyDecorator.SanityCheck(model, property, tail, out this.readOptionsWriteValue, true);
			this.shadowSetter = PropertyDecorator.GetShadowSetter(model, property);
		}

		// Token: 0x060002B6 RID: 694 RVA: 0x0000FCC8 File Offset: 0x0000DEC8
		private static void SanityCheck(TypeModel model, PropertyInfo property, IProtoSerializer tail, out bool writeValue, bool nonPublic)
		{
			if (property == null)
			{
				throw new ArgumentNullException("property");
			}
			writeValue = (tail.ReturnsValue && (PropertyDecorator.GetShadowSetter(model, property) != null || (property.CanWrite && Helpers.GetSetMethod(property, nonPublic) != null)));
			if (!property.CanRead || Helpers.GetGetMethod(property, nonPublic) == null)
			{
				throw new InvalidOperationException("Cannot serialize property without a get accessor");
			}
			if (!writeValue && (!tail.RequiresOldValue || Helpers.IsValueType(tail.ExpectedType)))
			{
				throw new InvalidOperationException("Cannot apply changes to property " + property.DeclaringType.FullName + "." + property.Name);
			}
		}

		// Token: 0x060002B7 RID: 695 RVA: 0x0000FD70 File Offset: 0x0000DF70
		private static MethodInfo GetShadowSetter(TypeModel model, PropertyInfo property)
		{
			MethodInfo instanceMethod = Helpers.GetInstanceMethod(property.ReflectedType, "Set" + property.Name, new Type[]
			{
				property.PropertyType
			});
			if (instanceMethod == null || !instanceMethod.IsPublic || instanceMethod.ReturnType != model.MapType(typeof(void)))
			{
				return null;
			}
			return instanceMethod;
		}

		// Token: 0x060002B8 RID: 696 RVA: 0x0000FDCE File Offset: 0x0000DFCE
		public override void Write(object value, ProtoWriter dest)
		{
			value = this.property.GetValue(value, null);
			if (value != null)
			{
				this.Tail.Write(value, dest);
			}
		}

		// Token: 0x060002B9 RID: 697 RVA: 0x0000FDF0 File Offset: 0x0000DFF0
		public override object Read(object value, ProtoReader source)
		{
			object value2 = this.Tail.RequiresOldValue ? this.property.GetValue(value, null) : null;
			object obj = this.Tail.Read(value2, source);
			if (this.readOptionsWriteValue && obj != null)
			{
				if (this.shadowSetter == null)
				{
					this.property.SetValue(value, obj, null);
				}
				else
				{
					this.shadowSetter.Invoke(value, new object[]
					{
						obj
					});
				}
			}
			return null;
		}

		// Token: 0x060002BA RID: 698 RVA: 0x0000FE64 File Offset: 0x0000E064
		internal static bool CanWrite(TypeModel model, MemberInfo member)
		{
			if (member == null)
			{
				throw new ArgumentNullException("member");
			}
			PropertyInfo propertyInfo = member as PropertyInfo;
			if (propertyInfo != null)
			{
				return propertyInfo.CanWrite || PropertyDecorator.GetShadowSetter(model, propertyInfo) != null;
			}
			return member is FieldInfo;
		}

		// Token: 0x04000262 RID: 610
		private readonly PropertyInfo property;

		// Token: 0x04000263 RID: 611
		private readonly Type forType;

		// Token: 0x04000264 RID: 612
		private readonly bool readOptionsWriteValue;

		// Token: 0x04000265 RID: 613
		private readonly MethodInfo shadowSetter;
	}
}
