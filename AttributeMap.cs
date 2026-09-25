using System;
using System.Reflection;

namespace ProtoBuf.Meta
{
	// Token: 0x02000067 RID: 103
	internal abstract class AttributeMap
	{
		// Token: 0x06000333 RID: 819 RVA: 0x00010C35 File Offset: 0x0000EE35
		[Obsolete("Please use AttributeType instead")]
		public new Type GetType()
		{
			return this.AttributeType;
		}

		// Token: 0x06000334 RID: 820
		public abstract bool TryGet(string key, bool publicOnly, out object value);

		// Token: 0x06000335 RID: 821 RVA: 0x00010C3D File Offset: 0x0000EE3D
		public bool TryGet(string key, out object value)
		{
			return this.TryGet(key, true, out value);
		}

		// Token: 0x170000AF RID: 175
		// (get) Token: 0x06000336 RID: 822
		public abstract Type AttributeType { get; }

		// Token: 0x06000337 RID: 823 RVA: 0x00010C48 File Offset: 0x0000EE48
		public static AttributeMap[] Create(TypeModel model, Type type, bool inherit)
		{
			object[] customAttributes = type.GetCustomAttributes(inherit);
			AttributeMap[] array = new AttributeMap[customAttributes.Length];
			for (int i = 0; i < customAttributes.Length; i++)
			{
				array[i] = new AttributeMap.ReflectionAttributeMap((Attribute)customAttributes[i]);
			}
			return array;
		}

		// Token: 0x06000338 RID: 824 RVA: 0x00010C88 File Offset: 0x0000EE88
		public static AttributeMap[] Create(TypeModel model, MemberInfo member, bool inherit)
		{
			object[] customAttributes = member.GetCustomAttributes(inherit);
			AttributeMap[] array = new AttributeMap[customAttributes.Length];
			for (int i = 0; i < customAttributes.Length; i++)
			{
				array[i] = new AttributeMap.ReflectionAttributeMap((Attribute)customAttributes[i]);
			}
			return array;
		}

		// Token: 0x06000339 RID: 825 RVA: 0x00010CC8 File Offset: 0x0000EEC8
		public static AttributeMap[] Create(TypeModel model, Assembly assembly)
		{
			object[] customAttributes = assembly.GetCustomAttributes(false);
			AttributeMap[] array = new AttributeMap[customAttributes.Length];
			for (int i = 0; i < customAttributes.Length; i++)
			{
				array[i] = new AttributeMap.ReflectionAttributeMap((Attribute)customAttributes[i]);
			}
			return array;
		}

		// Token: 0x170000B0 RID: 176
		// (get) Token: 0x0600033A RID: 826
		public abstract object Target { get; }

		// Token: 0x02000156 RID: 342
		private class ReflectionAttributeMap : AttributeMap
		{
			// Token: 0x170003FB RID: 1019
			// (get) Token: 0x06001142 RID: 4418 RVA: 0x0007750A File Offset: 0x0007570A
			public override object Target
			{
				get
				{
					return this.attribute;
				}
			}

			// Token: 0x170003FC RID: 1020
			// (get) Token: 0x06001143 RID: 4419 RVA: 0x00077512 File Offset: 0x00075712
			public override Type AttributeType
			{
				get
				{
					return this.attribute.GetType();
				}
			}

			// Token: 0x06001144 RID: 4420 RVA: 0x00077520 File Offset: 0x00075720
			public override bool TryGet(string key, bool publicOnly, out object value)
			{
				foreach (MemberInfo memberInfo in Helpers.GetInstanceFieldsAndProperties(this.attribute.GetType(), publicOnly))
				{
					if (string.Equals(memberInfo.Name, key, StringComparison.OrdinalIgnoreCase))
					{
						PropertyInfo propertyInfo = memberInfo as PropertyInfo;
						bool result;
						if (propertyInfo != null)
						{
							value = propertyInfo.GetValue(this.attribute, null);
							result = true;
						}
						else
						{
							FieldInfo fieldInfo = memberInfo as FieldInfo;
							if (fieldInfo == null)
							{
								throw new NotSupportedException(memberInfo.GetType().Name);
							}
							value = fieldInfo.GetValue(this.attribute);
							result = true;
						}
						return result;
					}
				}
				value = null;
				return false;
			}

			// Token: 0x06001145 RID: 4421 RVA: 0x000775B4 File Offset: 0x000757B4
			public ReflectionAttributeMap(Attribute attribute)
			{
				this.attribute = attribute;
			}

			// Token: 0x04000E13 RID: 3603
			private readonly Attribute attribute;
		}
	}
}
