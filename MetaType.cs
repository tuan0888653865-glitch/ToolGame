using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using ProtoBuf.Serializers;

namespace ProtoBuf.Meta
{
	// Token: 0x0200006C RID: 108
	public class MetaType : ISerializerProxy
	{
		// Token: 0x0600035D RID: 861 RVA: 0x00011085 File Offset: 0x0000F285
		public override string ToString()
		{
			return this.type.ToString();
		}

		// Token: 0x170000BA RID: 186
		// (get) Token: 0x0600035E RID: 862 RVA: 0x00011092 File Offset: 0x0000F292
		IProtoSerializer ISerializerProxy.Serializer
		{
			get
			{
				return this.Serializer;
			}
		}

		// Token: 0x170000BB RID: 187
		// (get) Token: 0x0600035F RID: 863 RVA: 0x0001109A File Offset: 0x0000F29A
		public MetaType BaseType
		{
			get
			{
				return this.baseType;
			}
		}

		// Token: 0x170000BC RID: 188
		// (get) Token: 0x06000360 RID: 864 RVA: 0x000110A2 File Offset: 0x0000F2A2
		internal TypeModel Model
		{
			get
			{
				return this.model;
			}
		}

		// Token: 0x170000BD RID: 189
		// (get) Token: 0x06000361 RID: 865 RVA: 0x000110AA File Offset: 0x0000F2AA
		// (set) Token: 0x06000362 RID: 866 RVA: 0x000110B6 File Offset: 0x0000F2B6
		public bool IncludeSerializerMethod
		{
			get
			{
				return !this.HasFlag(8);
			}
			set
			{
				this.SetFlag(8, !value, true);
			}
		}

		// Token: 0x170000BE RID: 190
		// (get) Token: 0x06000363 RID: 867 RVA: 0x000110C4 File Offset: 0x0000F2C4
		// (set) Token: 0x06000364 RID: 868 RVA: 0x000110CE File Offset: 0x0000F2CE
		public bool AsReferenceDefault
		{
			get
			{
				return this.HasFlag(32);
			}
			set
			{
				this.SetFlag(32, value, true);
			}
		}

		// Token: 0x06000365 RID: 869 RVA: 0x000110DA File Offset: 0x0000F2DA
		private bool IsValidSubType(Type subType)
		{
			return this.type.IsAssignableFrom(subType);
		}

		// Token: 0x06000366 RID: 870 RVA: 0x000110E8 File Offset: 0x0000F2E8
		public MetaType AddSubType(int fieldNumber, Type derivedType)
		{
			return this.AddSubType(fieldNumber, derivedType, DataFormat.Default);
		}

		// Token: 0x06000367 RID: 871 RVA: 0x000110F4 File Offset: 0x0000F2F4
		public MetaType AddSubType(int fieldNumber, Type derivedType, DataFormat dataFormat)
		{
			if (derivedType == null)
			{
				throw new ArgumentNullException("derivedType");
			}
			if (fieldNumber < 1)
			{
				throw new ArgumentOutOfRangeException("fieldNumber");
			}
			if ((!this.type.IsClass && !this.type.IsInterface) || this.type.IsSealed)
			{
				throw new InvalidOperationException("Sub-types can only be added to non-sealed classes");
			}
			if (!this.IsValidSubType(derivedType))
			{
				throw new ArgumentException(derivedType.Name + " is not a valid sub-type of " + this.type.Name, "derivedType");
			}
			MetaType metaType = this.model[derivedType];
			this.ThrowIfFrozen();
			metaType.ThrowIfFrozen();
			SubType value = new SubType(fieldNumber, metaType, dataFormat);
			this.ThrowIfFrozen();
			metaType.SetBaseType(this);
			if (this.subTypes == null)
			{
				this.subTypes = new BasicList();
			}
			this.subTypes.Add(value);
			return this;
		}

		// Token: 0x06000368 RID: 872 RVA: 0x000111D0 File Offset: 0x0000F3D0
		private void SetBaseType(MetaType baseType)
		{
			if (baseType == null)
			{
				throw new ArgumentNullException("baseType");
			}
			if (this.baseType == baseType)
			{
				return;
			}
			if (this.baseType != null)
			{
				throw new InvalidOperationException("A type can only participate in one inheritance hierarchy");
			}
			for (MetaType metaType = baseType; metaType != null; metaType = metaType.baseType)
			{
				if (metaType == this)
				{
					throw new InvalidOperationException("Cyclic inheritance is not allowed");
				}
			}
			this.baseType = baseType;
		}

		// Token: 0x170000BF RID: 191
		// (get) Token: 0x06000369 RID: 873 RVA: 0x0001122C File Offset: 0x0000F42C
		public bool HasCallbacks
		{
			get
			{
				return this.callbacks != null && this.callbacks.NonTrivial;
			}
		}

		// Token: 0x170000C0 RID: 192
		// (get) Token: 0x0600036A RID: 874 RVA: 0x00011243 File Offset: 0x0000F443
		public bool HasSubtypes
		{
			get
			{
				return this.subTypes != null && this.subTypes.Count != 0;
			}
		}

		// Token: 0x170000C1 RID: 193
		// (get) Token: 0x0600036B RID: 875 RVA: 0x0001125D File Offset: 0x0000F45D
		public CallbackSet Callbacks
		{
			get
			{
				if (this.callbacks == null)
				{
					this.callbacks = new CallbackSet(this);
				}
				return this.callbacks;
			}
		}

		// Token: 0x170000C2 RID: 194
		// (get) Token: 0x0600036C RID: 876 RVA: 0x00011279 File Offset: 0x0000F479
		private bool IsValueType
		{
			get
			{
				return this.type.IsValueType;
			}
		}

		// Token: 0x0600036D RID: 877 RVA: 0x00011286 File Offset: 0x0000F486
		public MetaType SetCallbacks(MethodInfo beforeSerialize, MethodInfo afterSerialize, MethodInfo beforeDeserialize, MethodInfo afterDeserialize)
		{
			CallbackSet callbackSet = this.Callbacks;
			callbackSet.BeforeSerialize = beforeSerialize;
			callbackSet.AfterSerialize = afterSerialize;
			callbackSet.BeforeDeserialize = beforeDeserialize;
			callbackSet.AfterDeserialize = afterDeserialize;
			return this;
		}

		// Token: 0x0600036E RID: 878 RVA: 0x000112AC File Offset: 0x0000F4AC
		public MetaType SetCallbacks(string beforeSerialize, string afterSerialize, string beforeDeserialize, string afterDeserialize)
		{
			if (this.IsValueType)
			{
				throw new InvalidOperationException();
			}
			CallbackSet callbackSet = this.Callbacks;
			callbackSet.BeforeSerialize = this.ResolveMethod(beforeSerialize, true);
			callbackSet.AfterSerialize = this.ResolveMethod(afterSerialize, true);
			callbackSet.BeforeDeserialize = this.ResolveMethod(beforeDeserialize, true);
			callbackSet.AfterDeserialize = this.ResolveMethod(afterDeserialize, true);
			return this;
		}

		// Token: 0x0600036F RID: 879 RVA: 0x00011308 File Offset: 0x0000F508
		internal string GetSchemaTypeName()
		{
			if (this.surrogate != null)
			{
				return this.model[this.surrogate].GetSchemaTypeName();
			}
			if (!Helpers.IsNullOrEmpty(this.name))
			{
				return this.name;
			}
			if (this.type.IsGenericType)
			{
				StringBuilder stringBuilder = new StringBuilder(this.type.Name);
				int num = this.type.Name.IndexOf('`');
				if (num >= 0)
				{
					stringBuilder.Length = num;
				}
				foreach (Type type in this.type.GetGenericArguments())
				{
					stringBuilder.Append('_');
					Type type2 = type;
					MetaType metaType;
					if (this.model.GetKey(ref type2) >= 0 && (metaType = this.model[type2]) != null && metaType.surrogate == null)
					{
						stringBuilder.Append(metaType.GetSchemaTypeName());
					}
					else
					{
						stringBuilder.Append(type2.Name);
					}
				}
				return stringBuilder.ToString();
			}
			return this.type.Name;
		}

		// Token: 0x170000C3 RID: 195
		// (get) Token: 0x06000370 RID: 880 RVA: 0x0001140A File Offset: 0x0000F60A
		// (set) Token: 0x06000371 RID: 881 RVA: 0x00011412 File Offset: 0x0000F612
		public string Name
		{
			get
			{
				return this.name;
			}
			set
			{
				this.ThrowIfFrozen();
				this.name = value;
			}
		}

		// Token: 0x06000372 RID: 882 RVA: 0x00011424 File Offset: 0x0000F624
		public MetaType SetFactory(MethodInfo factory)
		{
			if (factory != null)
			{
				if (this.IsValueType)
				{
					throw new InvalidOperationException();
				}
				if (!factory.IsStatic)
				{
					throw new ArgumentException("A factory-method must be static", "factory");
				}
				if (factory.ReturnType != this.type)
				{
					throw new ArgumentException("The factory-method must return " + this.type.FullName, "factory");
				}
				if (!CallbackSet.CheckCallbackParameters(this.model, factory))
				{
					throw new ArgumentException("Invalid factory signature in " + factory.DeclaringType.FullName + "." + factory.Name, "factory");
				}
			}
			this.ThrowIfFrozen();
			this.factory = factory;
			return this;
		}

		// Token: 0x06000373 RID: 883 RVA: 0x000114D2 File Offset: 0x0000F6D2
		public MetaType SetFactory(string factory)
		{
			return this.SetFactory(this.ResolveMethod(factory, false));
		}

		// Token: 0x06000374 RID: 884 RVA: 0x000114E2 File Offset: 0x0000F6E2
		private MethodInfo ResolveMethod(string name, bool instance)
		{
			if (Helpers.IsNullOrEmpty(name))
			{
				return null;
			}
			if (!instance)
			{
				return Helpers.GetStaticMethod(this.type, name);
			}
			return Helpers.GetInstanceMethod(this.type, name);
		}

		// Token: 0x06000375 RID: 885 RVA: 0x0001150C File Offset: 0x0000F70C
		internal MetaType(RuntimeTypeModel model, Type type)
		{
			if (model == null)
			{
				throw new ArgumentNullException("model");
			}
			if (type == null)
			{
				throw new ArgumentNullException("type");
			}
			WireType wireType;
			if (ValueMember.TryGetCoreSerializer(model, DataFormat.Default, type, out wireType, false, false, false, false) != null)
			{
				throw new ArgumentException("Data of this type has inbuilt behaviour, and cannot be added to a model in this way: " + type.FullName);
			}
			this.type = type;
			this.model = model;
			if (Helpers.IsEnum(type))
			{
				this.EnumPassthru = type.IsDefined(model.MapType(typeof(FlagsAttribute)), false);
			}
		}

		// Token: 0x06000376 RID: 886 RVA: 0x0001159F File Offset: 0x0000F79F
		protected internal void ThrowIfFrozen()
		{
			if ((this.flags & 4) != 0)
			{
				throw new InvalidOperationException("The type cannot be changed once a serializer has been generated for " + this.type.FullName);
			}
		}

		// Token: 0x06000377 RID: 887 RVA: 0x000115C8 File Offset: 0x0000F7C8
		internal void Freeze()
		{
			this.flags |= 4;
		}

		// Token: 0x170000C4 RID: 196
		// (get) Token: 0x06000378 RID: 888 RVA: 0x000115DD File Offset: 0x0000F7DD
		public Type Type
		{
			get
			{
				return this.type;
			}
		}

		// Token: 0x170000C5 RID: 197
		// (get) Token: 0x06000379 RID: 889 RVA: 0x000115E8 File Offset: 0x0000F7E8
		internal IProtoTypeSerializer Serializer
		{
			get
			{
				if (this.serializer == null)
				{
					int opaqueToken = 0;
					try
					{
						this.model.TakeLock(ref opaqueToken);
						if (this.serializer == null)
						{
							this.SetFlag(4, true, false);
							this.serializer = this.BuildSerializer();
						}
					}
					finally
					{
						this.model.ReleaseLock(opaqueToken);
					}
				}
				return this.serializer;
			}
		}

		// Token: 0x170000C6 RID: 198
		// (get) Token: 0x0600037A RID: 890 RVA: 0x00011650 File Offset: 0x0000F850
		internal bool IsList
		{
			get
			{
				return (this.IgnoreListHandling ? null : TypeModel.GetListItemType(this.model, this.type)) != null;
			}
		}

		// Token: 0x0600037B RID: 891 RVA: 0x00011674 File Offset: 0x0000F874
		private IProtoTypeSerializer BuildSerializer()
		{
			if (Helpers.IsEnum(this.type))
			{
				return new TagDecorator(1, WireType.Variant, false, new EnumSerializer(this.type, this.GetEnumMap()));
			}
			Type type = this.IgnoreListHandling ? null : TypeModel.GetListItemType(this.model, this.type);
			if (type != null)
			{
				if (this.surrogate != null)
				{
					throw new ArgumentException("Repeated data (a list, collection, etc) has inbuilt behaviour and cannot use a surrogate");
				}
				if (this.subTypes != null && this.subTypes.Count != 0)
				{
					throw new ArgumentException("Repeated data (a list, collection, etc) has inbuilt behaviour and cannot be subclassed");
				}
				ValueMember valueMember = new ValueMember(this.model, 1, this.type, type, this.type, DataFormat.Default);
				return new TypeSerializer(this.model, this.type, new int[]
				{
					1
				}, new IProtoSerializer[]
				{
					valueMember.Serializer
				}, null, true, true, null, this.constructType, this.factory);
			}
			else
			{
				if (this.surrogate != null)
				{
					MetaType metaType = this.model[this.surrogate];
					MetaType metaType2;
					while ((metaType2 = metaType.baseType) != null)
					{
						metaType = metaType2;
					}
					return new SurrogateSerializer(this.type, this.surrogate, metaType.Serializer);
				}
				if (!this.IsAutoTuple)
				{
					this.fields.Trim();
					int count = this.fields.Count;
					int num = (this.subTypes == null) ? 0 : this.subTypes.Count;
					int[] array = new int[count + num];
					IProtoSerializer[] array2 = new IProtoSerializer[count + num];
					int num2 = 0;
					if (num != 0)
					{
						foreach (object obj in this.subTypes)
						{
							SubType subType = (SubType)obj;
							if (!subType.DerivedType.IgnoreListHandling && this.model.MapType(MetaType.ienumerable).IsAssignableFrom(subType.DerivedType.Type))
							{
								throw new ArgumentException("Repeated data (a list, collection, etc) has inbuilt behaviour and cannot be used as a subclass");
							}
							array[num2] = subType.FieldNumber;
							array2[num2++] = subType.Serializer;
						}
					}
					if (count != 0)
					{
						foreach (object obj2 in this.fields)
						{
							ValueMember valueMember2 = (ValueMember)obj2;
							array[num2] = valueMember2.FieldNumber;
							array2[num2++] = valueMember2.Serializer;
						}
					}
					BasicList basicList = null;
					for (MetaType metaType3 = this.BaseType; metaType3 != null; metaType3 = metaType3.BaseType)
					{
						MethodInfo methodInfo = metaType3.HasCallbacks ? metaType3.Callbacks.BeforeDeserialize : null;
						if (methodInfo != null)
						{
							if (basicList == null)
							{
								basicList = new BasicList();
							}
							basicList.Add(methodInfo);
						}
					}
					MethodInfo[] array3 = null;
					if (basicList != null)
					{
						array3 = new MethodInfo[basicList.Count];
						basicList.CopyTo(array3, 0);
						Array.Reverse(array3);
					}
					return new TypeSerializer(this.model, this.type, array, array2, array3, this.baseType == null, this.UseConstructor, this.callbacks, this.constructType, this.factory);
				}
				MemberInfo[] members;
				ConstructorInfo constructorInfo = MetaType.ResolveTupleConstructor(this.type, out members);
				if (constructorInfo == null)
				{
					throw new InvalidOperationException();
				}
				return new TupleSerializer(this.model, constructorInfo, members);
			}
		}

		// Token: 0x0600037C RID: 892 RVA: 0x000119D8 File Offset: 0x0000FBD8
		private static Type GetBaseType(MetaType type)
		{
			return type.type.BaseType;
		}

		// Token: 0x0600037D RID: 893 RVA: 0x000119E8 File Offset: 0x0000FBE8
		internal void ApplyDefaultBehaviour()
		{
			Type type = MetaType.GetBaseType(this);
			if (type != null && this.model.FindWithoutAdd(type) == null && MetaType.GetContractFamily(this.model, type, null) != MetaType.AttributeFamily.None)
			{
				this.model.FindOrAddAuto(type, true, false, false);
			}
			AttributeMap[] array = AttributeMap.Create(this.model, this.type, false);
			MetaType.AttributeFamily attributeFamily = MetaType.GetContractFamily(this.model, this.type, array);
			if (attributeFamily == MetaType.AttributeFamily.AutoTuple)
			{
				this.SetFlag(64, true, true);
			}
			bool flag = !this.EnumPassthru && Helpers.IsEnum(this.type);
			if (attributeFamily == MetaType.AttributeFamily.None && !flag)
			{
				return;
			}
			BasicList basicList = null;
			BasicList basicList2 = null;
			int dataMemberOffset = 0;
			int num = 1;
			bool flag2 = this.model.InferTagFromNameDefault;
			ImplicitFields implicitFields = ImplicitFields.None;
			string text = null;
			foreach (AttributeMap attributeMap in array)
			{
				object obj;
				if (!flag && attributeMap.AttributeType.FullName == "ProtoBuf.ProtoIncludeAttribute")
				{
					int fieldNumber = 0;
					if (attributeMap.TryGet("tag", out obj))
					{
						fieldNumber = (int)obj;
					}
					DataFormat dataFormat = DataFormat.Default;
					if (attributeMap.TryGet("DataFormat", out obj))
					{
						dataFormat = (DataFormat)((int)obj);
					}
					Type type2 = null;
					try
					{
						if (attributeMap.TryGet("knownTypeName", out obj))
						{
							type2 = this.model.GetType((string)obj, this.type.Assembly);
						}
						else if (attributeMap.TryGet("knownType", out obj))
						{
							type2 = (Type)obj;
						}
					}
					catch (Exception innerException)
					{
						throw new InvalidOperationException("Unable to resolve sub-type of: " + this.type.FullName, innerException);
					}
					if (type2 == null)
					{
						throw new InvalidOperationException("Unable to resolve sub-type of: " + this.type.FullName);
					}
					if (this.IsValidSubType(type2))
					{
						this.AddSubType(fieldNumber, type2, dataFormat);
					}
				}
				if (attributeMap.AttributeType.FullName == "ProtoBuf.ProtoPartialIgnoreAttribute" && attributeMap.TryGet("MemberName", out obj) && obj != null)
				{
					if (basicList == null)
					{
						basicList = new BasicList();
					}
					basicList.Add((string)obj);
				}
				if (!flag && attributeMap.AttributeType.FullName == "ProtoBuf.ProtoPartialMemberAttribute")
				{
					if (basicList2 == null)
					{
						basicList2 = new BasicList();
					}
					basicList2.Add(attributeMap);
				}
				if (attributeMap.AttributeType.FullName == "ProtoBuf.ProtoContractAttribute")
				{
					if (attributeMap.TryGet("Name", out obj))
					{
						text = (string)obj;
					}
					if (!flag)
					{
						if (attributeMap.TryGet("DataMemberOffset", out obj))
						{
							dataMemberOffset = (int)obj;
						}
						if (attributeMap.TryGet("InferTagFromNameHasValue", false, out obj) && (bool)obj && attributeMap.TryGet("InferTagFromName", out obj))
						{
							flag2 = (bool)obj;
						}
						if (attributeMap.TryGet("ImplicitFields", out obj))
						{
							if (obj is ImplicitFields)
							{
								implicitFields = (ImplicitFields)obj;
							}
							else
							{
								if (!(obj is int))
								{
									throw new NotSupportedException(obj.GetType().FullName);
								}
								implicitFields = (ImplicitFields)((int)obj);
							}
						}
						if (attributeMap.TryGet("SkipConstructor", out obj))
						{
							this.UseConstructor = !(bool)obj;
						}
						if (attributeMap.TryGet("IgnoreListHandling", out obj))
						{
							this.IgnoreListHandling = (bool)obj;
						}
						if (attributeMap.TryGet("ImplicitFirstTag", out obj) && (int)obj > 0)
						{
							num = (int)obj;
						}
					}
				}
				if (attributeMap.AttributeType.FullName == "System.Runtime.Serialization.DataContractAttribute" && text == null && attributeMap.TryGet("Name", out obj))
				{
					text = (string)obj;
				}
				if (attributeMap.AttributeType.FullName == "System.Xml.Serialization.XmlTypeAttribute" && text == null && attributeMap.TryGet("TypeName", out obj))
				{
					text = (string)obj;
				}
			}
			if (!Helpers.IsNullOrEmpty(text))
			{
				this.Name = text;
			}
			if (implicitFields != ImplicitFields.None)
			{
				attributeFamily &= MetaType.AttributeFamily.ProtoBuf;
			}
			MethodInfo[] array2 = null;
			BasicList basicList3 = new BasicList();
			foreach (MemberInfo memberInfo in this.type.GetMembers(flag ? (BindingFlags.Static | BindingFlags.Public) : (BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)))
			{
				if (memberInfo.DeclaringType == this.type && !memberInfo.IsDefined(this.model.MapType(typeof(ProtoIgnoreAttribute)), true) && (basicList == null || !basicList.Contains(memberInfo.Name)))
				{
					bool flag3 = false;
					PropertyInfo propertyInfo;
					FieldInfo fieldInfo;
					MethodInfo methodInfo;
					if ((propertyInfo = (memberInfo as PropertyInfo)) != null)
					{
						if (!flag)
						{
							Type propertyType = propertyInfo.PropertyType;
							bool isPublic = Helpers.GetGetMethod(propertyInfo, false) != null;
							bool isField = false;
							MetaType.ApplyDefaultBehaviour_AddMembers(this.model, attributeFamily, flag, basicList2, dataMemberOffset, flag2, implicitFields, basicList3, memberInfo, ref flag3, isPublic, isField, ref propertyType);
						}
					}
					else if ((fieldInfo = (memberInfo as FieldInfo)) != null)
					{
						Type fieldType = fieldInfo.FieldType;
						bool isPublic2 = fieldInfo.IsPublic;
						bool isField2 = true;
						if (!flag || fieldInfo.IsStatic)
						{
							MetaType.ApplyDefaultBehaviour_AddMembers(this.model, attributeFamily, flag, basicList2, dataMemberOffset, flag2, implicitFields, basicList3, memberInfo, ref flag3, isPublic2, isField2, ref fieldType);
						}
					}
					else if ((methodInfo = (memberInfo as MethodInfo)) != null && !flag)
					{
						AttributeMap[] array3 = AttributeMap.Create(this.model, methodInfo, false);
						if (array3 != null && array3.Length != 0)
						{
							MetaType.CheckForCallback(methodInfo, array3, "ProtoBuf.ProtoBeforeSerializationAttribute", ref array2, 0);
							MetaType.CheckForCallback(methodInfo, array3, "ProtoBuf.ProtoAfterSerializationAttribute", ref array2, 1);
							MetaType.CheckForCallback(methodInfo, array3, "ProtoBuf.ProtoBeforeDeserializationAttribute", ref array2, 2);
							MetaType.CheckForCallback(methodInfo, array3, "ProtoBuf.ProtoAfterDeserializationAttribute", ref array2, 3);
							MetaType.CheckForCallback(methodInfo, array3, "System.Runtime.Serialization.OnSerializingAttribute", ref array2, 4);
							MetaType.CheckForCallback(methodInfo, array3, "System.Runtime.Serialization.OnSerializedAttribute", ref array2, 5);
							MetaType.CheckForCallback(methodInfo, array3, "System.Runtime.Serialization.OnDeserializingAttribute", ref array2, 6);
							MetaType.CheckForCallback(methodInfo, array3, "System.Runtime.Serialization.OnDeserializedAttribute", ref array2, 7);
						}
					}
				}
			}
			ProtoMemberAttribute[] array4 = new ProtoMemberAttribute[basicList3.Count];
			basicList3.CopyTo(array4, 0);
			if (flag2 || implicitFields != ImplicitFields.None)
			{
				Array.Sort<ProtoMemberAttribute>(array4);
				int num2 = num;
				foreach (ProtoMemberAttribute protoMemberAttribute in array4)
				{
					if (!protoMemberAttribute.TagIsPinned)
					{
						protoMemberAttribute.Rebase(num2++);
					}
				}
			}
			foreach (ProtoMemberAttribute normalizedAttribute in array4)
			{
				ValueMember valueMember = this.ApplyDefaultBehaviour(flag, normalizedAttribute);
				if (valueMember != null)
				{
					this.Add(valueMember);
				}
			}
			if (array2 != null)
			{
				this.SetCallbacks(MetaType.Coalesce(array2, 0, 4), MetaType.Coalesce(array2, 1, 5), MetaType.Coalesce(array2, 2, 6), MetaType.Coalesce(array2, 3, 7));
			}
		}

		// Token: 0x0600037E RID: 894 RVA: 0x000120A4 File Offset: 0x000102A4
		private static void ApplyDefaultBehaviour_AddMembers(TypeModel model, MetaType.AttributeFamily family, bool isEnum, BasicList partialMembers, int dataMemberOffset, bool inferTagByName, ImplicitFields implicitMode, BasicList members, MemberInfo member, ref bool forced, bool isPublic, bool isField, ref Type effectiveType)
		{
			if (implicitMode != ImplicitFields.AllPublic)
			{
				if (implicitMode == ImplicitFields.AllFields)
				{
					if (isField)
					{
						forced = true;
					}
				}
			}
			else if (isPublic)
			{
				forced = true;
			}
			if (effectiveType.IsSubclassOf(model.MapType(typeof(Delegate))))
			{
				effectiveType = null;
			}
			if (effectiveType != null)
			{
				ProtoMemberAttribute protoMemberAttribute = MetaType.NormalizeProtoMember(model, member, family, forced, isEnum, partialMembers, dataMemberOffset, inferTagByName);
				if (protoMemberAttribute != null)
				{
					members.Add(protoMemberAttribute);
				}
			}
		}

		// Token: 0x0600037F RID: 895 RVA: 0x00012114 File Offset: 0x00010314
		private static MethodInfo Coalesce(MethodInfo[] arr, int x, int y)
		{
			MethodInfo methodInfo = arr[x];
			if (methodInfo == null)
			{
				methodInfo = arr[y];
			}
			return methodInfo;
		}

		// Token: 0x06000380 RID: 896 RVA: 0x00012130 File Offset: 0x00010330
		internal static MetaType.AttributeFamily GetContractFamily(RuntimeTypeModel model, Type type, AttributeMap[] attributes)
		{
			MetaType.AttributeFamily attributeFamily = MetaType.AttributeFamily.None;
			if (attributes == null)
			{
				attributes = AttributeMap.Create(model, type, false);
			}
			for (int i = 0; i < attributes.Length; i++)
			{
				string fullName;
				if ((fullName = attributes[i].AttributeType.FullName) != null)
				{
					if (!(fullName == "ProtoBuf.ProtoContractAttribute"))
					{
						if (!(fullName == "System.Xml.Serialization.XmlTypeAttribute"))
						{
							if (fullName == "System.Runtime.Serialization.DataContractAttribute" && !model.AutoAddProtoContractTypesOnly)
							{
								attributeFamily |= MetaType.AttributeFamily.DataContractSerialier;
							}
						}
						else if (!model.AutoAddProtoContractTypesOnly)
						{
							attributeFamily |= MetaType.AttributeFamily.XmlSerializer;
						}
					}
					else
					{
						bool flag = false;
						MetaType.GetFieldBoolean(ref flag, attributes[i], "UseProtoMembersOnly");
						if (flag)
						{
							return MetaType.AttributeFamily.ProtoBuf;
						}
						attributeFamily |= MetaType.AttributeFamily.ProtoBuf;
					}
				}
			}
			MemberInfo[] array;
			if (attributeFamily == MetaType.AttributeFamily.None && MetaType.ResolveTupleConstructor(type, out array) != null)
			{
				attributeFamily |= MetaType.AttributeFamily.AutoTuple;
			}
			return attributeFamily;
		}

		// Token: 0x06000381 RID: 897 RVA: 0x000121DC File Offset: 0x000103DC
		internal static ConstructorInfo ResolveTupleConstructor(Type type, out MemberInfo[] mappedMembers)
		{
			mappedMembers = null;
			if (type == null)
			{
				throw new ArgumentNullException("type");
			}
			if (type.IsAbstract)
			{
				return null;
			}
			ConstructorInfo[] constructors = Helpers.GetConstructors(type, false);
			if (constructors.Length == 0 || (constructors.Length == 1 && constructors[0].GetParameters().Length == 0))
			{
				return null;
			}
			MemberInfo[] instanceFieldsAndProperties = Helpers.GetInstanceFieldsAndProperties(type, true);
			BasicList basicList = new BasicList();
			for (int i = 0; i < instanceFieldsAndProperties.Length; i++)
			{
				PropertyInfo propertyInfo = instanceFieldsAndProperties[i] as PropertyInfo;
				if (propertyInfo != null)
				{
					if (!propertyInfo.CanRead)
					{
						return null;
					}
					if (propertyInfo.CanWrite && Helpers.GetSetMethod(propertyInfo, false) != null)
					{
						return null;
					}
					basicList.Add(propertyInfo);
				}
				else
				{
					FieldInfo fieldInfo = instanceFieldsAndProperties[i] as FieldInfo;
					if (fieldInfo != null)
					{
						if (!fieldInfo.IsInitOnly)
						{
							return null;
						}
						basicList.Add(fieldInfo);
					}
				}
			}
			if (basicList.Count == 0)
			{
				return null;
			}
			MemberInfo[] array = new MemberInfo[basicList.Count];
			basicList.CopyTo(array, 0);
			int[] array2 = new int[array.Length];
			int num = 0;
			ConstructorInfo result = null;
			mappedMembers = new MemberInfo[array2.Length];
			for (int j = 0; j < constructors.Length; j++)
			{
				ParameterInfo[] parameters = constructors[j].GetParameters();
				if (parameters.Length == array.Length)
				{
					for (int k = 0; k < array2.Length; k++)
					{
						array2[k] = -1;
					}
					for (int l = 0; l < parameters.Length; l++)
					{
						string b = parameters[l].Name.ToLower();
						for (int m = 0; m < array.Length; m++)
						{
							if (!(array[m].Name.ToLower() != b) && Helpers.GetMemberType(array[m]) == parameters[l].ParameterType)
							{
								array2[l] = m;
							}
						}
					}
					bool flag = false;
					for (int n = 0; n < array2.Length; n++)
					{
						if (array2[n] < 0)
						{
							flag = true;
							break;
						}
						mappedMembers[n] = array[array2[n]];
					}
					if (!flag)
					{
						num++;
						result = constructors[j];
					}
				}
			}
			if (num != 1)
			{
				return null;
			}
			return result;
		}

		// Token: 0x06000382 RID: 898 RVA: 0x000123D4 File Offset: 0x000105D4
		private static void CheckForCallback(MethodInfo method, AttributeMap[] attributes, string callbackTypeName, ref MethodInfo[] callbacks, int index)
		{
			for (int i = 0; i < attributes.Length; i++)
			{
				if (attributes[i].AttributeType.FullName == callbackTypeName)
				{
					if (callbacks == null)
					{
						callbacks = new MethodInfo[8];
					}
					else if (callbacks[index] != null)
					{
						Type reflectedType = method.ReflectedType;
						throw new ProtoException("Duplicate " + callbackTypeName + " callbacks on " + reflectedType.FullName);
					}
					callbacks[index] = method;
				}
			}
		}

		// Token: 0x06000383 RID: 899 RVA: 0x00012442 File Offset: 0x00010642
		private static bool HasFamily(MetaType.AttributeFamily value, MetaType.AttributeFamily required)
		{
			return (value & required) == required;
		}

		// Token: 0x06000384 RID: 900 RVA: 0x0001244C File Offset: 0x0001064C
		private static ProtoMemberAttribute NormalizeProtoMember(TypeModel model, MemberInfo member, MetaType.AttributeFamily family, bool forced, bool isEnum, BasicList partialMembers, int dataMemberOffset, bool inferByTagName)
		{
			if (member == null || (family == MetaType.AttributeFamily.None && !isEnum))
			{
				return null;
			}
			int num = int.MinValue;
			int num2 = inferByTagName ? -1 : 1;
			string text = null;
			bool isPacked = false;
			bool flag = false;
			bool flag2 = false;
			bool isRequired = false;
			bool asReference = false;
			bool dynamicType = false;
			bool tagIsPinned = false;
			bool overwriteList = false;
			DataFormat dataFormat = DataFormat.Default;
			if (isEnum)
			{
				forced = true;
			}
			AttributeMap[] attribs = AttributeMap.Create(model, member, true);
			if (isEnum)
			{
				AttributeMap attribute = MetaType.GetAttribute(attribs, "ProtoBuf.ProtoIgnoreAttribute");
				if (attribute != null)
				{
					flag = true;
				}
				else
				{
					attribute = MetaType.GetAttribute(attribs, "ProtoBuf.ProtoEnumAttribute");
					num = Convert.ToInt32(((FieldInfo)member).GetRawConstantValue());
					if (attribute != null)
					{
						MetaType.GetFieldName(ref text, attribute, "Name");
						object obj;
						if ((bool)Helpers.GetInstanceMethod(attribute.AttributeType, "HasValue").Invoke(attribute.Target, null) && attribute.TryGet("Value", out obj))
						{
							num = (int)obj;
						}
					}
				}
				flag2 = true;
			}
			if (!flag && !flag2)
			{
				AttributeMap attribute2 = MetaType.GetAttribute(attribs, "ProtoBuf.ProtoMemberAttribute");
				MetaType.GetIgnore(ref flag, attribute2, attribs, "ProtoBuf.ProtoIgnoreAttribute");
				if (!flag && attribute2 != null)
				{
					MetaType.GetFieldNumber(ref num, attribute2, "Tag");
					MetaType.GetFieldName(ref text, attribute2, "Name");
					MetaType.GetFieldBoolean(ref isRequired, attribute2, "IsRequired");
					MetaType.GetFieldBoolean(ref isPacked, attribute2, "IsPacked");
					MetaType.GetFieldBoolean(ref overwriteList, attribute2, "OverwriteList");
					MetaType.GetDataFormat(ref dataFormat, attribute2, "DataFormat");
					MetaType.GetFieldBoolean(ref asReference, attribute2, "AsReference");
					MetaType.GetFieldBoolean(ref dynamicType, attribute2, "DynamicType");
					flag2 = (tagIsPinned = (num > 0));
				}
				if (!flag2 && partialMembers != null)
				{
					foreach (object obj2 in partialMembers)
					{
						AttributeMap attributeMap = (AttributeMap)obj2;
						object obj3;
						if (attributeMap.TryGet("MemberName", out obj3) && (string)obj3 == member.Name)
						{
							MetaType.GetFieldNumber(ref num, attributeMap, "Tag");
							MetaType.GetFieldName(ref text, attributeMap, "Name");
							MetaType.GetFieldBoolean(ref isRequired, attributeMap, "IsRequired");
							MetaType.GetFieldBoolean(ref isPacked, attributeMap, "IsPacked");
							MetaType.GetFieldBoolean(ref overwriteList, attribute2, "OverwriteList");
							MetaType.GetDataFormat(ref dataFormat, attributeMap, "DataFormat");
							MetaType.GetFieldBoolean(ref asReference, attributeMap, "AsReference");
							MetaType.GetFieldBoolean(ref dynamicType, attributeMap, "DynamicType");
							if (flag2 = (tagIsPinned = (num > 0)))
							{
								break;
							}
						}
					}
				}
			}
			if (!flag && !flag2 && MetaType.HasFamily(family, MetaType.AttributeFamily.DataContractSerialier))
			{
				AttributeMap attribute3 = MetaType.GetAttribute(attribs, "System.Runtime.Serialization.DataMemberAttribute");
				if (attribute3 != null)
				{
					MetaType.GetFieldNumber(ref num, attribute3, "Order");
					MetaType.GetFieldName(ref text, attribute3, "Name");
					MetaType.GetFieldBoolean(ref isRequired, attribute3, "IsRequired");
					flag2 = (num >= num2);
					if (flag2)
					{
						num += dataMemberOffset;
					}
				}
			}
			if (!flag && !flag2 && MetaType.HasFamily(family, MetaType.AttributeFamily.XmlSerializer))
			{
				AttributeMap attribute4 = MetaType.GetAttribute(attribs, "System.Xml.Serialization.XmlElementAttribute");
				if (attribute4 == null)
				{
					attribute4 = MetaType.GetAttribute(attribs, "System.Xml.Serialization.XmlArrayAttribute");
				}
				MetaType.GetIgnore(ref flag, attribute4, attribs, "System.Xml.Serialization.XmlIgnoreAttribute");
				if (attribute4 != null && !flag)
				{
					MetaType.GetFieldNumber(ref num, attribute4, "Order");
					MetaType.GetFieldName(ref text, attribute4, "ElementName");
					flag2 = (num >= num2);
				}
			}
			if (!flag && !flag2 && MetaType.GetAttribute(attribs, "System.NonSerializedAttribute") != null)
			{
				flag = true;
			}
			if (flag || (num < num2 && !forced))
			{
				return null;
			}
			return new ProtoMemberAttribute(num, forced || inferByTagName)
			{
				AsReference = asReference,
				DataFormat = dataFormat,
				DynamicType = dynamicType,
				IsPacked = isPacked,
				OverwriteList = overwriteList,
				IsRequired = isRequired,
				Name = (Helpers.IsNullOrEmpty(text) ? member.Name : text),
				Member = member,
				TagIsPinned = tagIsPinned
			};
		}

		// Token: 0x06000385 RID: 901 RVA: 0x00012834 File Offset: 0x00010A34
		private ValueMember ApplyDefaultBehaviour(bool isEnum, ProtoMemberAttribute normalizedAttribute)
		{
			MemberInfo member;
			if (normalizedAttribute == null || (member = normalizedAttribute.Member) == null)
			{
				return null;
			}
			Type memberType = Helpers.GetMemberType(member);
			Type type = null;
			Type defaultType = null;
			MetaType.ResolveListTypes(this.model, memberType, ref type, ref defaultType);
			if (type != null && this.model.FindOrAddAuto(memberType, false, true, false) >= 0 && this.model[memberType].IgnoreListHandling)
			{
				type = null;
				defaultType = null;
			}
			AttributeMap[] attribs = AttributeMap.Create(this.model, member, true);
			object defaultValue = null;
			if (this.model.UseImplicitZeroDefaults)
			{
				ProtoTypeCode typeCode = Helpers.GetTypeCode(memberType);
				switch (typeCode)
				{
				case ProtoTypeCode.Boolean:
					defaultValue = false;
					break;
				case ProtoTypeCode.Char:
					defaultValue = '\0';
					break;
				case ProtoTypeCode.SByte:
					defaultValue = 0;
					break;
				case ProtoTypeCode.Byte:
					defaultValue = 0;
					break;
				case ProtoTypeCode.Int16:
					defaultValue = 0;
					break;
				case ProtoTypeCode.UInt16:
					defaultValue = 0;
					break;
				case ProtoTypeCode.Int32:
					defaultValue = 0;
					break;
				case ProtoTypeCode.UInt32:
					defaultValue = 0U;
					break;
				case ProtoTypeCode.Int64:
					defaultValue = 0L;
					break;
				case ProtoTypeCode.UInt64:
					defaultValue = 0UL;
					break;
				case ProtoTypeCode.Single:
					defaultValue = 0f;
					break;
				case ProtoTypeCode.Double:
					defaultValue = 0.0;
					break;
				case ProtoTypeCode.Decimal:
					defaultValue = 0m;
					break;
				default:
					if (typeCode != ProtoTypeCode.TimeSpan)
					{
						if (typeCode == ProtoTypeCode.Guid)
						{
							defaultValue = Guid.Empty;
						}
					}
					else
					{
						defaultValue = TimeSpan.Zero;
					}
					break;
				}
			}
			AttributeMap attribute;
			object obj;
			if ((attribute = MetaType.GetAttribute(attribs, "System.ComponentModel.DefaultValueAttribute")) != null && attribute.TryGet("Value", out obj))
			{
				defaultValue = obj;
			}
			ValueMember valueMember = (isEnum || normalizedAttribute.Tag > 0) ? new ValueMember(this.model, this.type, normalizedAttribute.Tag, member, memberType, type, defaultType, normalizedAttribute.DataFormat, defaultValue) : null;
			if (valueMember != null)
			{
				Type declaringType = this.type;
				PropertyInfo propertyInfo = Helpers.GetProperty(declaringType, member.Name + "Specified");
				MethodInfo getMethod = Helpers.GetGetMethod(propertyInfo, true);
				if (getMethod == null || getMethod.IsStatic)
				{
					propertyInfo = null;
				}
				if (propertyInfo != null)
				{
					valueMember.SetSpecified(getMethod, Helpers.GetSetMethod(propertyInfo, true));
				}
				else
				{
					MethodInfo instanceMethod = Helpers.GetInstanceMethod(declaringType, "ShouldSerialize" + member.Name, Helpers.EmptyTypes);
					if (instanceMethod != null && instanceMethod.ReturnType == this.model.MapType(typeof(bool)))
					{
						valueMember.SetSpecified(instanceMethod, null);
					}
				}
				if (!Helpers.IsNullOrEmpty(normalizedAttribute.Name))
				{
					valueMember.SetName(normalizedAttribute.Name);
				}
				valueMember.IsPacked = normalizedAttribute.IsPacked;
				valueMember.IsRequired = normalizedAttribute.IsRequired;
				valueMember.OverwriteList = normalizedAttribute.OverwriteList;
				valueMember.AsReference = normalizedAttribute.AsReference;
				valueMember.DynamicType = normalizedAttribute.DynamicType;
			}
			return valueMember;
		}

		// Token: 0x06000386 RID: 902 RVA: 0x00012B2C File Offset: 0x00010D2C
		private static void GetDataFormat(ref DataFormat value, AttributeMap attrib, string memberName)
		{
			if (attrib == null || value != DataFormat.Default)
			{
				return;
			}
			object obj;
			if (attrib.TryGet(memberName, out obj) && obj != null)
			{
				value = (DataFormat)obj;
			}
		}

		// Token: 0x06000387 RID: 903 RVA: 0x00012B57 File Offset: 0x00010D57
		private static void GetIgnore(ref bool ignore, AttributeMap attrib, AttributeMap[] attribs, string fullName)
		{
			if (ignore || attrib == null)
			{
				return;
			}
			ignore = (MetaType.GetAttribute(attribs, fullName) != null);
		}

		// Token: 0x06000388 RID: 904 RVA: 0x00012B70 File Offset: 0x00010D70
		private static void GetFieldBoolean(ref bool value, AttributeMap attrib, string memberName)
		{
			if (attrib == null | value)
			{
				return;
			}
			object obj;
			if (attrib.TryGet(memberName, out obj) && obj != null)
			{
				value = (bool)obj;
			}
		}

		// Token: 0x06000389 RID: 905 RVA: 0x00012BA0 File Offset: 0x00010DA0
		private static void GetFieldNumber(ref int value, AttributeMap attrib, string memberName)
		{
			if (attrib == null || value > 0)
			{
				return;
			}
			object obj;
			if (attrib.TryGet(memberName, out obj) && obj != null)
			{
				value = (int)obj;
			}
		}

		// Token: 0x0600038A RID: 906 RVA: 0x00012BCC File Offset: 0x00010DCC
		private static void GetFieldName(ref string name, AttributeMap attrib, string memberName)
		{
			if (attrib == null || !Helpers.IsNullOrEmpty(name))
			{
				return;
			}
			object obj;
			if (attrib.TryGet(memberName, out obj) && obj != null)
			{
				name = (string)obj;
			}
		}

		// Token: 0x0600038B RID: 907 RVA: 0x00012BFC File Offset: 0x00010DFC
		private static AttributeMap GetAttribute(AttributeMap[] attribs, string fullName)
		{
			foreach (AttributeMap attributeMap in attribs)
			{
				if (attributeMap != null && attributeMap.AttributeType.FullName == fullName)
				{
					return attributeMap;
				}
			}
			return null;
		}

		// Token: 0x0600038C RID: 908 RVA: 0x00012C34 File Offset: 0x00010E34
		public MetaType Add(int fieldNumber, string memberName)
		{
			this.AddField(fieldNumber, memberName, null, null, null);
			return this;
		}

		// Token: 0x0600038D RID: 909 RVA: 0x00012C43 File Offset: 0x00010E43
		public ValueMember AddField(int fieldNumber, string memberName)
		{
			return this.AddField(fieldNumber, memberName, null, null, null);
		}

		// Token: 0x170000C7 RID: 199
		// (get) Token: 0x0600038E RID: 910 RVA: 0x00012C50 File Offset: 0x00010E50
		// (set) Token: 0x0600038F RID: 911 RVA: 0x00012C5D File Offset: 0x00010E5D
		public bool UseConstructor
		{
			get
			{
				return !this.HasFlag(16);
			}
			set
			{
				this.SetFlag(16, !value, true);
			}
		}

		// Token: 0x170000C8 RID: 200
		// (get) Token: 0x06000390 RID: 912 RVA: 0x00012C6C File Offset: 0x00010E6C
		// (set) Token: 0x06000391 RID: 913 RVA: 0x00012C74 File Offset: 0x00010E74
		public Type ConstructType
		{
			get
			{
				return this.constructType;
			}
			set
			{
				this.ThrowIfFrozen();
				this.constructType = value;
			}
		}

		// Token: 0x06000392 RID: 914 RVA: 0x00012C83 File Offset: 0x00010E83
		public MetaType Add(string memberName)
		{
			this.Add(this.GetNextFieldNumber(), memberName);
			return this;
		}

		// Token: 0x06000393 RID: 915 RVA: 0x00012C94 File Offset: 0x00010E94
		public void SetSurrogate(Type surrogateType)
		{
			if (surrogateType == this.type)
			{
				surrogateType = null;
			}
			if (surrogateType != null && surrogateType != null && Helpers.IsAssignableFrom(this.model.MapType(typeof(IEnumerable)), surrogateType))
			{
				throw new ArgumentException("Repeated data (a list, collection, etc) has inbuilt behaviour and cannot be used as a surrogate");
			}
			this.ThrowIfFrozen();
			this.surrogate = surrogateType;
		}

		// Token: 0x06000394 RID: 916 RVA: 0x00012CE8 File Offset: 0x00010EE8
		internal MetaType GetSurrogateOrSelf()
		{
			if (this.surrogate != null)
			{
				return this.model[this.surrogate];
			}
			return this;
		}

		// Token: 0x06000395 RID: 917 RVA: 0x00012D05 File Offset: 0x00010F05
		internal MetaType GetSurrogateOrBaseOrSelf()
		{
			if (this.surrogate != null)
			{
				return this.model[this.surrogate];
			}
			if (this.baseType != null)
			{
				return this.baseType;
			}
			return this;
		}

		// Token: 0x06000396 RID: 918 RVA: 0x00012D34 File Offset: 0x00010F34
		private int GetNextFieldNumber()
		{
			int num = 0;
			foreach (object obj in this.fields)
			{
				ValueMember valueMember = (ValueMember)obj;
				if (valueMember.FieldNumber > num)
				{
					num = valueMember.FieldNumber;
				}
			}
			if (this.subTypes != null)
			{
				foreach (object obj2 in this.subTypes)
				{
					SubType subType = (SubType)obj2;
					if (subType.FieldNumber > num)
					{
						num = subType.FieldNumber;
					}
				}
			}
			return num + 1;
		}

		// Token: 0x06000397 RID: 919 RVA: 0x00012DF8 File Offset: 0x00010FF8
		public MetaType Add(params string[] memberNames)
		{
			int nextFieldNumber = this.GetNextFieldNumber();
			for (int i = 0; i < memberNames.Length; i++)
			{
				this.Add(nextFieldNumber++, memberNames[i]);
			}
			return this;
		}

		// Token: 0x06000398 RID: 920 RVA: 0x00012E2A File Offset: 0x0001102A
		public MetaType Add(int fieldNumber, string memberName, object defaultValue)
		{
			this.AddField(fieldNumber, memberName, null, null, defaultValue);
			return this;
		}

		// Token: 0x06000399 RID: 921 RVA: 0x00012E39 File Offset: 0x00011039
		public MetaType Add(int fieldNumber, string memberName, Type itemType, Type defaultType)
		{
			this.AddField(fieldNumber, memberName, itemType, defaultType, null);
			return this;
		}

		// Token: 0x0600039A RID: 922 RVA: 0x00012E49 File Offset: 0x00011049
		public ValueMember AddField(int fieldNumber, string memberName, Type itemType, Type defaultType)
		{
			return this.AddField(fieldNumber, memberName, itemType, defaultType, null);
		}

		// Token: 0x0600039B RID: 923 RVA: 0x00012E58 File Offset: 0x00011058
		private ValueMember AddField(int fieldNumber, string memberName, Type itemType, Type defaultType, object defaultValue)
		{
			MemberInfo memberInfo = null;
			MemberInfo[] member = this.type.GetMember(memberName, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
			if (member != null && member.Length == 1)
			{
				memberInfo = member[0];
			}
			if (memberInfo == null)
			{
				throw new ArgumentException("Unable to determine member: " + memberName, "memberName");
			}
			MemberTypes memberType = memberInfo.MemberType;
			Type memberType2;
			if (memberType != MemberTypes.Field)
			{
				if (memberType != MemberTypes.Property)
				{
					throw new NotSupportedException(memberInfo.MemberType.ToString());
				}
				memberType2 = ((PropertyInfo)memberInfo).PropertyType;
			}
			else
			{
				memberType2 = ((FieldInfo)memberInfo).FieldType;
			}
			MetaType.ResolveListTypes(this.model, memberType2, ref itemType, ref defaultType);
			ValueMember valueMember = new ValueMember(this.model, this.type, fieldNumber, memberInfo, memberType2, itemType, defaultType, DataFormat.Default, defaultValue);
			this.Add(valueMember);
			return valueMember;
		}

		// Token: 0x0600039C RID: 924 RVA: 0x00012F18 File Offset: 0x00011118
		internal static void ResolveListTypes(TypeModel model, Type type, ref Type itemType, ref Type defaultType)
		{
			if (type == null)
			{
				return;
			}
			if (type.IsArray)
			{
				if (type.GetArrayRank() != 1)
				{
					throw new NotSupportedException("Multi-dimension arrays are supported");
				}
				itemType = type.GetElementType();
				if (itemType == model.MapType(typeof(byte)))
				{
					Type type2;
					itemType = (type2 = null);
					defaultType = type2;
				}
				else
				{
					defaultType = type;
				}
			}
			if (itemType == null)
			{
				itemType = TypeModel.GetListItemType(model, type);
			}
			if (itemType != null)
			{
				Type type3 = null;
				Type type4 = null;
				MetaType.ResolveListTypes(model, itemType, ref type3, ref type4);
				if (type3 != null)
				{
					throw TypeModel.CreateNestedListsNotSupported();
				}
			}
			if (itemType != null && defaultType == null)
			{
				if (type.IsClass && !type.IsAbstract && Helpers.GetConstructor(type, Helpers.EmptyTypes, true) != null)
				{
					defaultType = type;
				}
				if (defaultType == null && type.IsInterface)
				{
					Type[] genericArguments;
					if (type.IsGenericType && type.GetGenericTypeDefinition() == model.MapType(typeof(IDictionary<, >)) && itemType == model.MapType(typeof(KeyValuePair<, >)).MakeGenericType(genericArguments = type.GetGenericArguments()))
					{
						defaultType = model.MapType(typeof(Dictionary<, >)).MakeGenericType(genericArguments);
					}
					else
					{
						defaultType = model.MapType(typeof(List<>)).MakeGenericType(new Type[]
						{
							itemType
						});
					}
				}
				if (defaultType != null && !Helpers.IsAssignableFrom(type, defaultType))
				{
					defaultType = null;
				}
			}
		}

		// Token: 0x0600039D RID: 925 RVA: 0x00013064 File Offset: 0x00011264
		private void Add(ValueMember member)
		{
			int opaqueToken = 0;
			try
			{
				this.model.TakeLock(ref opaqueToken);
				this.ThrowIfFrozen();
				this.fields.Add(member);
			}
			finally
			{
				this.model.ReleaseLock(opaqueToken);
			}
		}

		// Token: 0x170000C9 RID: 201
		public ValueMember this[int fieldNumber]
		{
			get
			{
				foreach (object obj in this.fields)
				{
					ValueMember valueMember = (ValueMember)obj;
					if (valueMember.FieldNumber == fieldNumber)
					{
						return valueMember;
					}
				}
				return null;
			}
		}

		// Token: 0x170000CA RID: 202
		public ValueMember this[MemberInfo member]
		{
			get
			{
				if (member == null)
				{
					return null;
				}
				foreach (object obj in this.fields)
				{
					ValueMember valueMember = (ValueMember)obj;
					if (valueMember.Member == member)
					{
						return valueMember;
					}
				}
				return null;
			}
		}

		// Token: 0x060003A0 RID: 928 RVA: 0x00013180 File Offset: 0x00011380
		public ValueMember[] GetFields()
		{
			ValueMember[] array = new ValueMember[this.fields.Count];
			this.fields.CopyTo(array, 0);
			Array.Sort<ValueMember>(array, ValueMember.Comparer.Default);
			return array;
		}

		// Token: 0x060003A1 RID: 929 RVA: 0x000131B8 File Offset: 0x000113B8
		public SubType[] GetSubtypes()
		{
			if (this.subTypes == null || this.subTypes.Count == 0)
			{
				return new SubType[0];
			}
			SubType[] array = new SubType[this.subTypes.Count];
			this.subTypes.CopyTo(array, 0);
			Array.Sort<SubType>(array, SubType.Comparer.Default);
			return array;
		}

		// Token: 0x060003A2 RID: 930 RVA: 0x0001320C File Offset: 0x0001140C
		internal bool IsDefined(int fieldNumber)
		{
			using (IEnumerator enumerator = this.fields.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (((ValueMember)enumerator.Current).FieldNumber == fieldNumber)
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x060003A3 RID: 931 RVA: 0x0001326C File Offset: 0x0001146C
		internal int GetKey(bool demand, bool getBaseKey)
		{
			return this.model.GetKey(this.type, demand, getBaseKey);
		}

		// Token: 0x060003A4 RID: 932 RVA: 0x00013284 File Offset: 0x00011484
		internal EnumSerializer.EnumPair[] GetEnumMap()
		{
			if (this.HasFlag(2))
			{
				return null;
			}
			EnumSerializer.EnumPair[] array = new EnumSerializer.EnumPair[this.fields.Count];
			for (int i = 0; i < array.Length; i++)
			{
				ValueMember valueMember = (ValueMember)this.fields[i];
				int fieldNumber = valueMember.FieldNumber;
				object rawEnumValue = valueMember.GetRawEnumValue();
				array[i] = new EnumSerializer.EnumPair(fieldNumber, rawEnumValue, valueMember.MemberType);
			}
			return array;
		}

		// Token: 0x170000CB RID: 203
		// (get) Token: 0x060003A5 RID: 933 RVA: 0x000132F2 File Offset: 0x000114F2
		// (set) Token: 0x060003A6 RID: 934 RVA: 0x000132FB File Offset: 0x000114FB
		public bool EnumPassthru
		{
			get
			{
				return this.HasFlag(2);
			}
			set
			{
				this.SetFlag(2, value, true);
			}
		}

		// Token: 0x170000CC RID: 204
		// (get) Token: 0x060003A7 RID: 935 RVA: 0x00013306 File Offset: 0x00011506
		// (set) Token: 0x060003A8 RID: 936 RVA: 0x00013313 File Offset: 0x00011513
		public bool IgnoreListHandling
		{
			get
			{
				return this.HasFlag(128);
			}
			set
			{
				this.SetFlag(128, value, true);
			}
		}

		// Token: 0x170000CD RID: 205
		// (get) Token: 0x060003A9 RID: 937 RVA: 0x00013322 File Offset: 0x00011522
		// (set) Token: 0x060003AA RID: 938 RVA: 0x0001332B File Offset: 0x0001152B
		internal bool Pending
		{
			get
			{
				return this.HasFlag(1);
			}
			set
			{
				this.SetFlag(1, value, false);
			}
		}

		// Token: 0x060003AB RID: 939 RVA: 0x00013336 File Offset: 0x00011536
		private bool HasFlag(byte flag)
		{
			return (this.flags & flag) == flag;
		}

		// Token: 0x060003AC RID: 940 RVA: 0x00013348 File Offset: 0x00011548
		private void SetFlag(byte flag, bool value, bool throwIfFrozen)
		{
			if (throwIfFrozen && this.HasFlag(flag) != value)
			{
				this.ThrowIfFrozen();
			}
			if (value)
			{
				this.flags |= flag;
				return;
			}
			this.flags &= ~flag;
		}

		// Token: 0x060003AD RID: 941 RVA: 0x00013394 File Offset: 0x00011594
		internal static MetaType GetRootType(MetaType source)
		{
			while (source.serializer != null)
			{
				MetaType metaType = source.baseType;
				if (metaType == null)
				{
					return source;
				}
				source = metaType;
			}
			RuntimeTypeModel runtimeTypeModel = source.model;
			int opaqueToken = 0;
			MetaType result;
			try
			{
				runtimeTypeModel.TakeLock(ref opaqueToken);
				MetaType metaType2;
				while ((metaType2 = source.baseType) != null)
				{
					source = metaType2;
				}
				result = source;
			}
			finally
			{
				runtimeTypeModel.ReleaseLock(opaqueToken);
			}
			return result;
		}

		// Token: 0x060003AE RID: 942 RVA: 0x00008C9D File Offset: 0x00006E9D
		internal bool IsPrepared()
		{
			return false;
		}

		// Token: 0x170000CE RID: 206
		// (get) Token: 0x060003AF RID: 943 RVA: 0x000133FC File Offset: 0x000115FC
		internal IEnumerable Fields
		{
			get
			{
				return this.fields;
			}
		}

		// Token: 0x060003B0 RID: 944 RVA: 0x00013404 File Offset: 0x00011604
		internal static StringBuilder NewLine(StringBuilder builder, int indent)
		{
			return Helpers.AppendLine(builder).Append(' ', indent * 3);
		}

		// Token: 0x170000CF RID: 207
		// (get) Token: 0x060003B1 RID: 945 RVA: 0x00013416 File Offset: 0x00011616
		internal bool IsAutoTuple
		{
			get
			{
				return this.HasFlag(64);
			}
		}

		// Token: 0x060003B2 RID: 946 RVA: 0x00013420 File Offset: 0x00011620
		internal void WriteSchema(StringBuilder builder, int indent, ref bool requiresBclImport)
		{
			if (this.surrogate != null)
			{
				return;
			}
			ValueMember[] array = new ValueMember[this.fields.Count];
			this.fields.CopyTo(array, 0);
			Array.Sort<ValueMember>(array, ValueMember.Comparer.Default);
			if (this.IsList)
			{
				string schemaTypeName = this.model.GetSchemaTypeName(TypeModel.GetListItemType(this.model, this.type), DataFormat.Default, false, false, ref requiresBclImport);
				MetaType.NewLine(builder, indent).Append("message ").Append(this.GetSchemaTypeName()).Append(" {");
				MetaType.NewLine(builder, indent + 1).Append("repeated ").Append(schemaTypeName).Append(" items = 1;");
				MetaType.NewLine(builder, indent).Append('}');
				return;
			}
			if (this.IsAutoTuple)
			{
				MemberInfo[] array2;
				if (MetaType.ResolveTupleConstructor(this.type, out array2) != null)
				{
					MetaType.NewLine(builder, indent).Append("message ").Append(this.GetSchemaTypeName()).Append(" {");
					for (int i = 0; i < array2.Length; i++)
					{
						Type effectiveType;
						if (array2[i] is PropertyInfo)
						{
							effectiveType = ((PropertyInfo)array2[i]).PropertyType;
						}
						else
						{
							if (!(array2[i] is FieldInfo))
							{
								throw new NotSupportedException("Unknown member type: " + array2[i].GetType().Name);
							}
							effectiveType = ((FieldInfo)array2[i]).FieldType;
						}
						MetaType.NewLine(builder, indent + 1).Append("optional ").Append(this.model.GetSchemaTypeName(effectiveType, DataFormat.Default, false, false, ref requiresBclImport).Replace('.', '_')).Append(' ').Append(array2[i].Name).Append(" = ").Append(i + 1).Append(';');
					}
					MetaType.NewLine(builder, indent).Append('}');
					return;
				}
			}
			else
			{
				if (Helpers.IsEnum(this.type))
				{
					MetaType.NewLine(builder, indent).Append("enum ").Append(this.GetSchemaTypeName()).Append(" {");
					foreach (ValueMember valueMember in array)
					{
						MetaType.NewLine(builder, indent + 1).Append(valueMember.Name).Append(" = ").Append(valueMember.FieldNumber).Append(';');
					}
					MetaType.NewLine(builder, indent).Append('}');
					return;
				}
				MetaType.NewLine(builder, indent).Append("message ").Append(this.GetSchemaTypeName()).Append(" {");
				foreach (ValueMember valueMember2 in array)
				{
					string value = (valueMember2.ItemType != null) ? "repeated" : (valueMember2.IsRequired ? "required" : "optional");
					MetaType.NewLine(builder, indent + 1).Append(value).Append(' ');
					if (valueMember2.DataFormat == DataFormat.Group)
					{
						builder.Append("group ");
					}
					string schemaTypeName2 = valueMember2.GetSchemaTypeName(true, ref requiresBclImport);
					builder.Append(schemaTypeName2).Append(" ").Append(valueMember2.Name).Append(" = ").Append(valueMember2.FieldNumber);
					if (valueMember2.DefaultValue != null)
					{
						if (valueMember2.DefaultValue is string)
						{
							builder.Append(" [default = \"").Append(valueMember2.DefaultValue).Append("\"]");
						}
						else if (valueMember2.DefaultValue is bool)
						{
							builder.Append(((bool)valueMember2.DefaultValue) ? " [default = true]" : " [default = false]");
						}
						else
						{
							builder.Append(" [default = ").Append(valueMember2.DefaultValue).Append(']');
						}
					}
					if (valueMember2.ItemType != null && valueMember2.IsPacked)
					{
						builder.Append(" [packed=true]");
					}
					builder.Append(';');
					if (schemaTypeName2 == "bcl.NetObjectProxy" && valueMember2.AsReference && !valueMember2.DynamicType)
					{
						builder.Append(" // reference-tracked ").Append(valueMember2.GetSchemaTypeName(false, ref requiresBclImport));
					}
				}
				if (this.subTypes != null && this.subTypes.Count != 0)
				{
					MetaType.NewLine(builder, indent + 1).Append("// the following represent sub-types; at most 1 should have a value");
					SubType[] array5 = new SubType[this.subTypes.Count];
					this.subTypes.CopyTo(array5, 0);
					Array.Sort<SubType>(array5, SubType.Comparer.Default);
					foreach (SubType subType in array5)
					{
						string schemaTypeName3 = subType.DerivedType.GetSchemaTypeName();
						MetaType.NewLine(builder, indent + 1).Append("optional ").Append(schemaTypeName3).Append(" ").Append(schemaTypeName3).Append(" = ").Append(subType.FieldNumber).Append(';');
					}
				}
				MetaType.NewLine(builder, indent).Append('}');
			}
		}

		// Token: 0x04000293 RID: 659
		private const byte OPTIONS_Pending = 1;

		// Token: 0x04000294 RID: 660
		private const byte OPTIONS_EnumPassThru = 2;

		// Token: 0x04000295 RID: 661
		private const byte OPTIONS_Frozen = 4;

		// Token: 0x04000296 RID: 662
		private const byte OPTIONS_PrivateOnApi = 8;

		// Token: 0x04000297 RID: 663
		private const byte OPTIONS_SkipConstructor = 16;

		// Token: 0x04000298 RID: 664
		private const byte OPTIONS_AsReferenceDefault = 32;

		// Token: 0x04000299 RID: 665
		private const byte OPTIONS_AutoTuple = 64;

		// Token: 0x0400029A RID: 666
		private const byte OPTIONS_IgnoreListHandling = 128;

		// Token: 0x0400029B RID: 667
		private MetaType baseType;

		// Token: 0x0400029C RID: 668
		private BasicList subTypes;

		// Token: 0x0400029D RID: 669
		internal static readonly Type ienumerable = typeof(IEnumerable);

		// Token: 0x0400029E RID: 670
		private CallbackSet callbacks;

		// Token: 0x0400029F RID: 671
		private string name;

		// Token: 0x040002A0 RID: 672
		private MethodInfo factory;

		// Token: 0x040002A1 RID: 673
		private readonly RuntimeTypeModel model;

		// Token: 0x040002A2 RID: 674
		private readonly Type type;

		// Token: 0x040002A3 RID: 675
		private IProtoTypeSerializer serializer;

		// Token: 0x040002A4 RID: 676
		private Type constructType;

		// Token: 0x040002A5 RID: 677
		private Type surrogate;

		// Token: 0x040002A6 RID: 678
		private readonly BasicList fields = new BasicList();

		// Token: 0x040002A7 RID: 679
		private volatile byte flags;

		// Token: 0x0200015B RID: 347
		internal class Comparer : IComparer, IComparer<MetaType>
		{
			// Token: 0x06001157 RID: 4439 RVA: 0x0007782A File Offset: 0x00075A2A
			public int Compare(object x, object y)
			{
				return this.Compare(x as MetaType, y as MetaType);
			}

			// Token: 0x06001158 RID: 4440 RVA: 0x0007783E File Offset: 0x00075A3E
			public int Compare(MetaType x, MetaType y)
			{
				if (x == y)
				{
					return 0;
				}
				if (x == null)
				{
					return -1;
				}
				if (y == null)
				{
					return 1;
				}
				return string.Compare(x.GetSchemaTypeName(), y.GetSchemaTypeName(), StringComparison.Ordinal);
			}

			// Token: 0x04000E1A RID: 3610
			public static readonly MetaType.Comparer Default = new MetaType.Comparer();
		}

		// Token: 0x0200015C RID: 348
		[Flags]
		internal enum AttributeFamily
		{
			// Token: 0x04000E1C RID: 3612
			None = 0,
			// Token: 0x04000E1D RID: 3613
			ProtoBuf = 1,
			// Token: 0x04000E1E RID: 3614
			DataContractSerialier = 2,
			// Token: 0x04000E1F RID: 3615
			XmlSerializer = 4,
			// Token: 0x04000E20 RID: 3616
			AutoTuple = 8
		}
	}
}
