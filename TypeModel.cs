using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace ProtoBuf.Meta
{
	// Token: 0x02000072 RID: 114
	public abstract class TypeModel
	{
		// Token: 0x060003F2 RID: 1010 RVA: 0x00014A65 File Offset: 0x00012C65
		protected internal Type MapType(Type type)
		{
			return this.MapType(type, true);
		}

		// Token: 0x060003F3 RID: 1011 RVA: 0x00014A6F File Offset: 0x00012C6F
		protected internal virtual Type MapType(Type type, bool demand)
		{
			return type;
		}

		// Token: 0x060003F4 RID: 1012 RVA: 0x00014A74 File Offset: 0x00012C74
		private WireType GetWireType(ProtoTypeCode code, DataFormat format, ref Type type, out int modelKey)
		{
			modelKey = -1;
			if (Helpers.IsEnum(type))
			{
				modelKey = this.GetKey(ref type);
				return WireType.Variant;
			}
			switch (code)
			{
			case ProtoTypeCode.Boolean:
			case ProtoTypeCode.Char:
			case ProtoTypeCode.SByte:
			case ProtoTypeCode.Byte:
			case ProtoTypeCode.Int16:
			case ProtoTypeCode.UInt16:
			case ProtoTypeCode.Int32:
			case ProtoTypeCode.UInt32:
				if (format != DataFormat.FixedSize)
				{
					return WireType.Variant;
				}
				return WireType.Fixed32;
			case ProtoTypeCode.Int64:
			case ProtoTypeCode.UInt64:
				if (format != DataFormat.FixedSize)
				{
					return WireType.Variant;
				}
				return WireType.Fixed64;
			case ProtoTypeCode.Single:
				return WireType.Fixed32;
			case ProtoTypeCode.Double:
				return WireType.Fixed64;
			case ProtoTypeCode.Decimal:
			case ProtoTypeCode.DateTime:
			case ProtoTypeCode.String:
				break;
			case (ProtoTypeCode)17:
				goto IL_80;
			default:
				if (code - ProtoTypeCode.TimeSpan > 3)
				{
					goto IL_80;
				}
				break;
			}
			return WireType.String;
			IL_80:
			if ((modelKey = this.GetKey(ref type)) >= 0)
			{
				return WireType.String;
			}
			return WireType.None;
		}

		// Token: 0x060003F5 RID: 1013 RVA: 0x00014B14 File Offset: 0x00012D14
		internal bool TrySerializeAuxiliaryType(ProtoWriter writer, Type type, DataFormat format, int tag, object value, bool isInsideList)
		{
			if (type == null)
			{
				type = value.GetType();
			}
			ProtoTypeCode typeCode = Helpers.GetTypeCode(type);
			int num;
			WireType wireType = this.GetWireType(typeCode, format, ref type, out num);
			if (num >= 0)
			{
				if (Helpers.IsEnum(type))
				{
					this.Serialize(num, value, writer);
					return true;
				}
				ProtoWriter.WriteFieldHeader(tag, wireType, writer);
				if (wireType == WireType.None)
				{
					throw ProtoWriter.CreateException(writer);
				}
				if (wireType - WireType.String > 1)
				{
					this.Serialize(num, value, writer);
					return true;
				}
				SubItemToken token = ProtoWriter.StartSubItem(value, writer);
				this.Serialize(num, value, writer);
				ProtoWriter.EndSubItem(token, writer);
				return true;
			}
			else
			{
				if (wireType != WireType.None)
				{
					ProtoWriter.WriteFieldHeader(tag, wireType, writer);
				}
				ProtoTypeCode protoTypeCode = typeCode;
				switch (protoTypeCode)
				{
				case ProtoTypeCode.Boolean:
					ProtoWriter.WriteBoolean((bool)value, writer);
					return true;
				case ProtoTypeCode.Char:
					ProtoWriter.WriteUInt16((ushort)((char)value), writer);
					return true;
				case ProtoTypeCode.SByte:
					ProtoWriter.WriteSByte((sbyte)value, writer);
					return true;
				case ProtoTypeCode.Byte:
					ProtoWriter.WriteByte((byte)value, writer);
					return true;
				case ProtoTypeCode.Int16:
					ProtoWriter.WriteInt16((short)value, writer);
					return true;
				case ProtoTypeCode.UInt16:
					ProtoWriter.WriteUInt16((ushort)value, writer);
					return true;
				case ProtoTypeCode.Int32:
					ProtoWriter.WriteInt32((int)value, writer);
					return true;
				case ProtoTypeCode.UInt32:
					ProtoWriter.WriteUInt32((uint)value, writer);
					return true;
				case ProtoTypeCode.Int64:
					ProtoWriter.WriteInt64((long)value, writer);
					return true;
				case ProtoTypeCode.UInt64:
					ProtoWriter.WriteUInt64((ulong)value, writer);
					return true;
				case ProtoTypeCode.Single:
					ProtoWriter.WriteSingle((float)value, writer);
					return true;
				case ProtoTypeCode.Double:
					ProtoWriter.WriteDouble((double)value, writer);
					return true;
				case ProtoTypeCode.Decimal:
					BclHelpers.WriteDecimal((decimal)value, writer);
					return true;
				case ProtoTypeCode.DateTime:
					BclHelpers.WriteDateTime((DateTime)value, writer);
					return true;
				case (ProtoTypeCode)17:
					break;
				case ProtoTypeCode.String:
					ProtoWriter.WriteString((string)value, writer);
					return true;
				default:
					switch (protoTypeCode)
					{
					case ProtoTypeCode.TimeSpan:
						BclHelpers.WriteTimeSpan((TimeSpan)value, writer);
						return true;
					case ProtoTypeCode.ByteArray:
						ProtoWriter.WriteBytes((byte[])value, writer);
						return true;
					case ProtoTypeCode.Guid:
						BclHelpers.WriteGuid((Guid)value, writer);
						return true;
					case ProtoTypeCode.Uri:
						ProtoWriter.WriteString(((Uri)value).AbsoluteUri, writer);
						return true;
					}
					break;
				}
				IEnumerable enumerable = value as IEnumerable;
				if (enumerable == null)
				{
					return false;
				}
				if (isInsideList)
				{
					throw TypeModel.CreateNestedListsNotSupported();
				}
				foreach (object obj in enumerable)
				{
					if (obj == null)
					{
						throw new NullReferenceException();
					}
					if (!this.TrySerializeAuxiliaryType(writer, null, format, tag, obj, true))
					{
						TypeModel.ThrowUnexpectedType(obj.GetType());
					}
				}
				return true;
			}
		}

		// Token: 0x060003F6 RID: 1014 RVA: 0x00014DB8 File Offset: 0x00012FB8
		private void SerializeCore(ProtoWriter writer, object value)
		{
			if (value == null)
			{
				throw new ArgumentNullException("value");
			}
			Type type = value.GetType();
			int key = this.GetKey(ref type);
			if (key >= 0)
			{
				this.Serialize(key, value, writer);
				return;
			}
			if (!this.TrySerializeAuxiliaryType(writer, type, DataFormat.Default, 1, value, false))
			{
				TypeModel.ThrowUnexpectedType(type);
			}
		}

		// Token: 0x060003F7 RID: 1015 RVA: 0x00014E05 File Offset: 0x00013005
		public void Serialize(Stream dest, object value)
		{
			this.Serialize(dest, value, null);
		}

		// Token: 0x060003F8 RID: 1016 RVA: 0x00014E10 File Offset: 0x00013010
		public void Serialize(Stream dest, object value, SerializationContext context)
		{
			using (ProtoWriter protoWriter = new ProtoWriter(dest, this, context))
			{
				protoWriter.SetRootObject(value);
				this.SerializeCore(protoWriter, value);
				protoWriter.Close();
			}
		}

		// Token: 0x060003F9 RID: 1017 RVA: 0x00014E58 File Offset: 0x00013058
		public void Serialize(ProtoWriter dest, object value)
		{
			dest.CheckDepthFlushlock();
			dest.SetRootObject(value);
			this.SerializeCore(dest, value);
			dest.CheckDepthFlushlock();
			ProtoWriter.Flush(dest);
		}

		// Token: 0x060003FA RID: 1018 RVA: 0x00014E7C File Offset: 0x0001307C
		public object DeserializeWithLengthPrefix(Stream source, object value, Type type, PrefixStyle style, int fieldNumber)
		{
			int num;
			return this.DeserializeWithLengthPrefix(source, value, type, style, fieldNumber, null, out num);
		}

		// Token: 0x060003FB RID: 1019 RVA: 0x00014E9C File Offset: 0x0001309C
		public object DeserializeWithLengthPrefix(Stream source, object value, Type type, PrefixStyle style, int expectedField, Serializer.TypeResolver resolver)
		{
			int num;
			return this.DeserializeWithLengthPrefix(source, value, type, style, expectedField, resolver, out num);
		}

		// Token: 0x060003FC RID: 1020 RVA: 0x00014EBC File Offset: 0x000130BC
		public object DeserializeWithLengthPrefix(Stream source, object value, Type type, PrefixStyle style, int expectedField, Serializer.TypeResolver resolver, out int bytesRead)
		{
			bool flag;
			return this.DeserializeWithLengthPrefix(source, value, type, style, expectedField, resolver, out bytesRead, out flag, null);
		}

		// Token: 0x060003FD RID: 1021 RVA: 0x00014EE0 File Offset: 0x000130E0
		private object DeserializeWithLengthPrefix(Stream source, object value, Type type, PrefixStyle style, int expectedField, Serializer.TypeResolver resolver, out int bytesRead, out bool haveObject, SerializationContext context)
		{
			haveObject = false;
			bytesRead = 0;
			if (type == null && (style != PrefixStyle.Base128 || resolver == null))
			{
				throw new InvalidOperationException("A type must be provided unless base-128 prefixing is being used in combination with a resolver");
			}
			int num;
			for (;;)
			{
				bool flag = expectedField > 0 || resolver != null;
				int num2;
				int num3;
				num = ProtoReader.ReadLengthPrefix(source, flag, style, out num2, out num3);
				if (num3 == 0)
				{
					return value;
				}
				bytesRead += num3;
				if (num < 0)
				{
					break;
				}
				bool flag2;
				if (style == PrefixStyle.Base128)
				{
					if (flag && expectedField == 0 && type == null && resolver != null)
					{
						type = resolver(num2);
						flag2 = (type == null);
					}
					else
					{
						flag2 = (expectedField != num2);
					}
				}
				else
				{
					flag2 = false;
				}
				if (flag2)
				{
					if (num == 2147483647)
					{
						goto IL_A9;
					}
					ProtoReader.Seek(source, num, null);
					bytesRead += num;
				}
				if (!flag2)
				{
					goto Block_13;
				}
			}
			return value;
			Block_13:
			object result;
			using (ProtoReader protoReader = new ProtoReader(source, this, context, num))
			{
				int key = this.GetKey(ref type);
				if (key >= 0)
				{
					value = this.Deserialize(key, value, protoReader);
				}
				else if (!this.TryDeserializeAuxiliaryType(protoReader, DataFormat.Default, 1, type, ref value, true, false, true, false) && num != 0)
				{
					TypeModel.ThrowUnexpectedType(type);
				}
				bytesRead += protoReader.Position;
				haveObject = true;
				result = value;
			}
			return result;
			IL_A9:
			throw new InvalidOperationException();
		}

		// Token: 0x060003FE RID: 1022 RVA: 0x00015018 File Offset: 0x00013218
		public IEnumerable DeserializeItems(Stream source, Type type, PrefixStyle style, int expectedField, Serializer.TypeResolver resolver)
		{
			return this.DeserializeItems(source, type, style, expectedField, resolver, null);
		}

		// Token: 0x060003FF RID: 1023 RVA: 0x00015028 File Offset: 0x00013228
		public IEnumerable DeserializeItems(Stream source, Type type, PrefixStyle style, int expectedField, Serializer.TypeResolver resolver, SerializationContext context)
		{
			return new TypeModel.DeserializeItemsIterator(this, source, type, style, expectedField, resolver, context);
		}

		// Token: 0x06000400 RID: 1024 RVA: 0x00015039 File Offset: 0x00013239
		public IEnumerable<T> DeserializeItems<T>(Stream source, PrefixStyle style, int expectedField)
		{
			return this.DeserializeItems<T>(source, style, expectedField, null);
		}

		// Token: 0x06000401 RID: 1025 RVA: 0x00015045 File Offset: 0x00013245
		public IEnumerable<T> DeserializeItems<T>(Stream source, PrefixStyle style, int expectedField, SerializationContext context)
		{
			return new TypeModel.DeserializeItemsIterator<T>(this, source, style, expectedField, context);
		}

		// Token: 0x06000402 RID: 1026 RVA: 0x00015052 File Offset: 0x00013252
		public void SerializeWithLengthPrefix(Stream dest, object value, Type type, PrefixStyle style, int fieldNumber)
		{
			this.SerializeWithLengthPrefix(dest, value, type, style, fieldNumber, null);
		}

		// Token: 0x06000403 RID: 1027 RVA: 0x00015064 File Offset: 0x00013264
		public void SerializeWithLengthPrefix(Stream dest, object value, Type type, PrefixStyle style, int fieldNumber, SerializationContext context)
		{
			if (type == null)
			{
				if (value == null)
				{
					throw new ArgumentNullException("value");
				}
				type = this.MapType(value.GetType());
			}
			int key = this.GetKey(ref type);
			using (ProtoWriter protoWriter = new ProtoWriter(dest, this, context))
			{
				if (style != PrefixStyle.None)
				{
					if (style - PrefixStyle.Base128 > 2)
					{
						throw new ArgumentOutOfRangeException("style");
					}
					ProtoWriter.WriteObject(value, key, protoWriter, style, fieldNumber);
				}
				else
				{
					this.Serialize(key, value, protoWriter);
				}
				protoWriter.Close();
			}
		}

		// Token: 0x06000404 RID: 1028 RVA: 0x000150F8 File Offset: 0x000132F8
		public object Deserialize(Stream source, object value, Type type)
		{
			return this.Deserialize(source, value, type, null);
		}

		// Token: 0x06000405 RID: 1029 RVA: 0x00015104 File Offset: 0x00013304
		public object Deserialize(Stream source, object value, Type type, SerializationContext context)
		{
			bool noAutoCreate = this.PrepareDeserialize(value, ref type);
			object result;
			using (ProtoReader protoReader = new ProtoReader(source, this, context))
			{
				if (value != null)
				{
					protoReader.SetRootObject(value);
				}
				result = this.DeserializeCore(protoReader, type, value, noAutoCreate);
			}
			return result;
		}

		// Token: 0x06000406 RID: 1030 RVA: 0x00015158 File Offset: 0x00013358
		private bool PrepareDeserialize(object value, ref Type type)
		{
			if (type == null)
			{
				if (value == null)
				{
					throw new ArgumentNullException("type");
				}
				type = this.MapType(value.GetType());
			}
			bool result = true;
			Type underlyingType = Helpers.GetUnderlyingType(type);
			if (underlyingType != null)
			{
				type = underlyingType;
				result = false;
			}
			return result;
		}

		// Token: 0x06000407 RID: 1031 RVA: 0x00015198 File Offset: 0x00013398
		public object Deserialize(Stream source, object value, Type type, int length)
		{
			return this.Deserialize(source, value, type, length, null);
		}

		// Token: 0x06000408 RID: 1032 RVA: 0x000151A8 File Offset: 0x000133A8
		public object Deserialize(Stream source, object value, Type type, int length, SerializationContext context)
		{
			bool noAutoCreate = this.PrepareDeserialize(value, ref type);
			object result;
			using (ProtoReader protoReader = new ProtoReader(source, this, context, length))
			{
				if (value != null)
				{
					protoReader.SetRootObject(value);
				}
				object obj = this.DeserializeCore(protoReader, type, value, noAutoCreate);
				protoReader.CheckFullyConsumed();
				result = obj;
			}
			return result;
		}

		// Token: 0x06000409 RID: 1033 RVA: 0x00015204 File Offset: 0x00013404
		public object Deserialize(ProtoReader source, object value, Type type)
		{
			bool noAutoCreate = this.PrepareDeserialize(value, ref type);
			if (value != null)
			{
				source.SetRootObject(value);
			}
			object result = this.DeserializeCore(source, type, value, noAutoCreate);
			source.CheckFullyConsumed();
			return result;
		}

		// Token: 0x0600040A RID: 1034 RVA: 0x00015238 File Offset: 0x00013438
		private object DeserializeCore(ProtoReader reader, Type type, object value, bool noAutoCreate)
		{
			int key = this.GetKey(ref type);
			if (key >= 0 && !Helpers.IsEnum(type))
			{
				return this.Deserialize(key, value, reader);
			}
			this.TryDeserializeAuxiliaryType(reader, DataFormat.Default, 1, type, ref value, true, false, noAutoCreate, false);
			return value;
		}

		// Token: 0x0600040B RID: 1035 RVA: 0x00015278 File Offset: 0x00013478
		internal static MethodInfo ResolveListAdd(TypeModel model, Type listType, Type itemType, out bool isList)
		{
			isList = model.MapType(TypeModel.ilist).IsAssignableFrom(listType);
			Type[] array = new Type[]
			{
				itemType
			};
			MethodInfo instanceMethod = Helpers.GetInstanceMethod(listType, "Add", array);
			if (instanceMethod == null)
			{
				Type type = model.MapType(typeof(ICollection<>)).MakeGenericType(array);
				if (type.IsAssignableFrom(listType))
				{
					instanceMethod = Helpers.GetInstanceMethod(type, "Add", array);
				}
			}
			if (instanceMethod == null)
			{
				array[0] = model.MapType(typeof(object));
				instanceMethod = Helpers.GetInstanceMethod(listType, "Add", array);
			}
			if (instanceMethod == null & isList)
			{
				instanceMethod = Helpers.GetInstanceMethod(model.MapType(TypeModel.ilist), "Add", array);
			}
			return instanceMethod;
		}

		// Token: 0x0600040C RID: 1036 RVA: 0x00015324 File Offset: 0x00013524
		internal static Type GetListItemType(TypeModel model, Type listType)
		{
			if (listType == model.MapType(typeof(string)) || listType.IsArray || !model.MapType(typeof(IEnumerable)).IsAssignableFrom(listType))
			{
				return null;
			}
			BasicList basicList = new BasicList();
			foreach (MethodInfo methodInfo in listType.GetMethods())
			{
				if (!methodInfo.IsStatic && !(methodInfo.Name != "Add"))
				{
					ParameterInfo[] parameters = methodInfo.GetParameters();
					if (parameters.Length == 1 && !basicList.Contains(parameters[0].ParameterType))
					{
						basicList.Add(parameters[0].ParameterType);
					}
				}
			}
			foreach (Type type in listType.GetInterfaces())
			{
				if (type.IsGenericType && type.GetGenericTypeDefinition() == model.MapType(typeof(ICollection<>)))
				{
					Type[] genericArguments = type.GetGenericArguments();
					if (!basicList.Contains(genericArguments[0]))
					{
						basicList.Add(genericArguments[0]);
					}
				}
			}
			foreach (PropertyInfo propertyInfo in listType.GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic))
			{
				if (!(propertyInfo.Name != "Item") && !basicList.Contains(propertyInfo.PropertyType))
				{
					ParameterInfo[] indexParameters = propertyInfo.GetIndexParameters();
					if (indexParameters.Length == 1 && indexParameters[0].ParameterType == model.MapType(typeof(int)))
					{
						basicList.Add(propertyInfo.PropertyType);
					}
				}
			}
			switch (basicList.Count)
			{
			case 0:
				return null;
			case 1:
				return (Type)basicList[0];
			case 2:
				if (TypeModel.CheckDictionaryAccessors(model, (Type)basicList[0], (Type)basicList[1]))
				{
					return (Type)basicList[0];
				}
				if (TypeModel.CheckDictionaryAccessors(model, (Type)basicList[1], (Type)basicList[0]))
				{
					return (Type)basicList[1];
				}
				break;
			}
			return null;
		}

		// Token: 0x0600040D RID: 1037 RVA: 0x0001553C File Offset: 0x0001373C
		private static bool CheckDictionaryAccessors(TypeModel model, Type pair, Type value)
		{
			return pair.IsGenericType && pair.GetGenericTypeDefinition() == model.MapType(typeof(KeyValuePair<, >)) && pair.GetGenericArguments()[1] == value;
		}

		// Token: 0x0600040E RID: 1038 RVA: 0x0001556C File Offset: 0x0001376C
		private bool TryDeserializeList(TypeModel model, ProtoReader reader, DataFormat format, int tag, Type listType, Type itemType, ref object value)
		{
			bool flag;
			MethodInfo methodInfo = TypeModel.ResolveListAdd(model, listType, itemType, out flag);
			if (methodInfo == null)
			{
				throw new NotSupportedException("Unknown list variant: " + listType.FullName);
			}
			bool result = false;
			object obj = null;
			IList list = value as IList;
			object[] array = flag ? null : new object[1];
			BasicList basicList = listType.IsArray ? new BasicList() : null;
			while (this.TryDeserializeAuxiliaryType(reader, format, tag, itemType, ref obj, true, true, true, true))
			{
				result = true;
				if (value == null && basicList == null)
				{
					value = TypeModel.CreateListInstance(listType, itemType);
					list = (value as IList);
				}
				if (list != null)
				{
					list.Add(obj);
				}
				else if (basicList != null)
				{
					basicList.Add(obj);
				}
				else
				{
					array[0] = obj;
					methodInfo.Invoke(value, array);
				}
				obj = null;
			}
			if (basicList != null)
			{
				if (value != null)
				{
					if (basicList.Count != 0)
					{
						Array array2 = (Array)value;
						Array array3 = Array.CreateInstance(itemType, array2.Length + basicList.Count);
						Array.Copy(array2, array3, array2.Length);
						basicList.CopyTo(array3, array2.Length);
						value = array3;
					}
				}
				else
				{
					Array array4 = Array.CreateInstance(itemType, basicList.Count);
					basicList.CopyTo(array4, 0);
					value = array4;
				}
			}
			return result;
		}

		// Token: 0x0600040F RID: 1039 RVA: 0x000156B4 File Offset: 0x000138B4
		private static object CreateListInstance(Type listType, Type itemType)
		{
			Type type = listType;
			if (listType.IsArray)
			{
				return Array.CreateInstance(itemType, 0);
			}
			if (!listType.IsClass || listType.IsAbstract || Helpers.GetConstructor(listType, Helpers.EmptyTypes, true) == null)
			{
				bool flag = false;
				if (listType.IsInterface && listType.FullName.IndexOf("Dictionary") >= 0)
				{
					if (listType.IsGenericType && listType.GetGenericTypeDefinition() == typeof(IDictionary<, >))
					{
						Type[] genericArguments = listType.GetGenericArguments();
						type = typeof(Dictionary<, >).MakeGenericType(genericArguments);
						flag = true;
					}
					if (!flag && listType == typeof(IDictionary))
					{
						type = typeof(Hashtable);
						flag = true;
					}
				}
				if (!flag)
				{
					type = typeof(List<>).MakeGenericType(new Type[]
					{
						itemType
					});
					flag = true;
				}
				if (!flag)
				{
					type = typeof(ArrayList);
				}
			}
			return Activator.CreateInstance(type);
		}

		// Token: 0x06000410 RID: 1040 RVA: 0x00015798 File Offset: 0x00013998
		internal bool TryDeserializeAuxiliaryType(ProtoReader reader, DataFormat format, int tag, Type type, ref object value, bool skipOtherFields, bool asListItem, bool autoCreate, bool insideList)
		{
			if (type == null)
			{
				throw new ArgumentNullException("type");
			}
			ProtoTypeCode typeCode = Helpers.GetTypeCode(type);
			int num;
			WireType wireType = this.GetWireType(typeCode, format, ref type, out num);
			bool flag = false;
			if (wireType == WireType.None)
			{
				Type type2 = TypeModel.GetListItemType(this, type);
				if (type2 == null && type.IsArray && type.GetArrayRank() == 1 && type != typeof(byte[]))
				{
					type2 = type.GetElementType();
				}
				if (type2 != null)
				{
					if (insideList)
					{
						throw TypeModel.CreateNestedListsNotSupported();
					}
					flag = this.TryDeserializeList(this, reader, format, tag, type, type2, ref value);
					if (!flag && autoCreate)
					{
						value = TypeModel.CreateListInstance(type, type2);
					}
					return flag;
				}
				else
				{
					TypeModel.ThrowUnexpectedType(type);
				}
			}
			while (!flag || !asListItem)
			{
				int num2 = reader.ReadFieldHeader();
				if (num2 <= 0)
				{
					break;
				}
				if (num2 != tag)
				{
					if (!skipOtherFields)
					{
						throw ProtoReader.AddErrorData(new InvalidOperationException(string.Concat(new object[]
						{
							"Expected field ",
							tag,
							", but found ",
							num2
						})), reader);
					}
					reader.SkipField();
				}
				else
				{
					flag = true;
					reader.Hint(wireType);
					if (num >= 0)
					{
						if (wireType - WireType.String <= 1)
						{
							SubItemToken token = ProtoReader.StartSubItem(reader);
							value = this.Deserialize(num, value, reader);
							ProtoReader.EndSubItem(token, reader);
						}
						else
						{
							value = this.Deserialize(num, value, reader);
						}
					}
					else
					{
						ProtoTypeCode protoTypeCode = typeCode;
						switch (protoTypeCode)
						{
						case ProtoTypeCode.Boolean:
							value = reader.ReadBoolean();
							break;
						case ProtoTypeCode.Char:
							value = (char)reader.ReadUInt16();
							break;
						case ProtoTypeCode.SByte:
							value = reader.ReadSByte();
							break;
						case ProtoTypeCode.Byte:
							value = reader.ReadByte();
							break;
						case ProtoTypeCode.Int16:
							value = reader.ReadInt16();
							break;
						case ProtoTypeCode.UInt16:
							value = reader.ReadUInt16();
							break;
						case ProtoTypeCode.Int32:
							value = reader.ReadInt32();
							break;
						case ProtoTypeCode.UInt32:
							value = reader.ReadUInt32();
							break;
						case ProtoTypeCode.Int64:
							value = reader.ReadInt64();
							break;
						case ProtoTypeCode.UInt64:
							value = reader.ReadUInt64();
							break;
						case ProtoTypeCode.Single:
							value = reader.ReadSingle();
							break;
						case ProtoTypeCode.Double:
							value = reader.ReadDouble();
							break;
						case ProtoTypeCode.Decimal:
							value = BclHelpers.ReadDecimal(reader);
							break;
						case ProtoTypeCode.DateTime:
							value = BclHelpers.ReadDateTime(reader);
							break;
						case (ProtoTypeCode)17:
							break;
						case ProtoTypeCode.String:
							value = reader.ReadString();
							break;
						default:
							switch (protoTypeCode)
							{
							case ProtoTypeCode.TimeSpan:
								value = BclHelpers.ReadTimeSpan(reader);
								break;
							case ProtoTypeCode.ByteArray:
								value = ProtoReader.AppendBytes((byte[])value, reader);
								break;
							case ProtoTypeCode.Guid:
								value = BclHelpers.ReadGuid(reader);
								break;
							case ProtoTypeCode.Uri:
								value = new Uri(reader.ReadString());
								break;
							}
							break;
						}
					}
				}
			}
			if (!flag && !asListItem && autoCreate && type != typeof(string))
			{
				value = Activator.CreateInstance(type);
			}
			return flag;
		}

		// Token: 0x06000411 RID: 1041 RVA: 0x00015AE2 File Offset: 0x00013CE2
		public static RuntimeTypeModel Create()
		{
			return new RuntimeTypeModel(false);
		}

		// Token: 0x06000412 RID: 1042 RVA: 0x00015AEC File Offset: 0x00013CEC
		protected internal static Type ResolveProxies(Type type)
		{
			if (type == null)
			{
				return null;
			}
			if (type.IsGenericParameter)
			{
				return null;
			}
			Type underlyingType = Helpers.GetUnderlyingType(type);
			if (underlyingType != null)
			{
				return underlyingType;
			}
			if (type.FullName.StartsWith("System.Data.Entity.DynamicProxies."))
			{
				return type.BaseType;
			}
			Type[] interfaces = type.GetInterfaces();
			for (int i = 0; i < interfaces.Length; i++)
			{
				string fullName;
				if ((fullName = interfaces[i].FullName) != null && (fullName == "NHibernate.Proxy.INHibernateProxy" || fullName == "NHibernate.Proxy.DynamicProxy.IProxy" || fullName == "NHibernate.Intercept.IFieldInterceptorAccessor"))
				{
					return type.BaseType;
				}
			}
			return null;
		}

		// Token: 0x06000413 RID: 1043 RVA: 0x00015B7D File Offset: 0x00013D7D
		public bool IsDefined(Type type)
		{
			return this.GetKey(ref type) >= 0;
		}

		// Token: 0x06000414 RID: 1044 RVA: 0x00015B90 File Offset: 0x00013D90
		protected internal int GetKey(ref Type type)
		{
			int keyImpl = this.GetKeyImpl(type);
			if (keyImpl < 0)
			{
				Type type2 = TypeModel.ResolveProxies(type);
				if (type2 != null)
				{
					type = type2;
					keyImpl = this.GetKeyImpl(type);
				}
			}
			return keyImpl;
		}

		// Token: 0x06000415 RID: 1045
		protected abstract int GetKeyImpl(Type type);

		// Token: 0x06000416 RID: 1046
		protected internal abstract void Serialize(int key, object value, ProtoWriter dest);

		// Token: 0x06000417 RID: 1047
		protected internal abstract object Deserialize(int key, object value, ProtoReader source);

		// Token: 0x06000418 RID: 1048 RVA: 0x00015BC4 File Offset: 0x00013DC4
		public object DeepClone(object value)
		{
			if (value == null)
			{
				return null;
			}
			Type type = value.GetType();
			int key = this.GetKey(ref type);
			object result;
			if (key >= 0 && !Helpers.IsEnum(type))
			{
				using (MemoryStream memoryStream = new MemoryStream())
				{
					using (ProtoWriter protoWriter = new ProtoWriter(memoryStream, this, null))
					{
						protoWriter.SetRootObject(value);
						this.Serialize(key, value, protoWriter);
						protoWriter.Close();
					}
					memoryStream.Position = 0L;
					using (ProtoReader protoReader = new ProtoReader(memoryStream, this, null))
					{
						result = this.Deserialize(key, null, protoReader);
						return result;
					}
				}
			}
			if (type == typeof(byte[]))
			{
				byte[] array = (byte[])value;
				byte[] array2 = new byte[array.Length];
				Helpers.BlockCopy(array, 0, array2, 0, array.Length);
				return array2;
			}
			int num;
			if (this.GetWireType(Helpers.GetTypeCode(type), DataFormat.Default, ref type, out num) != WireType.None && num < 0)
			{
				return value;
			}
			using (MemoryStream memoryStream2 = new MemoryStream())
			{
				using (ProtoWriter protoWriter2 = new ProtoWriter(memoryStream2, this, null))
				{
					if (!this.TrySerializeAuxiliaryType(protoWriter2, type, DataFormat.Default, 1, value, false))
					{
						TypeModel.ThrowUnexpectedType(type);
					}
					protoWriter2.Close();
				}
				memoryStream2.Position = 0L;
				using (ProtoReader protoReader2 = new ProtoReader(memoryStream2, this, null))
				{
					value = null;
					this.TryDeserializeAuxiliaryType(protoReader2, DataFormat.Default, 1, type, ref value, true, false, true, false);
					result = value;
				}
			}
			return result;
		}

		// Token: 0x06000419 RID: 1049 RVA: 0x00015D8C File Offset: 0x00013F8C
		protected internal static void ThrowUnexpectedSubtype(Type expected, Type actual)
		{
			if (expected != TypeModel.ResolveProxies(actual))
			{
				throw new InvalidOperationException("Unexpected sub-type: " + actual.FullName);
			}
		}

		// Token: 0x0600041A RID: 1050 RVA: 0x00015DB0 File Offset: 0x00013FB0
		protected internal static void ThrowUnexpectedType(Type type)
		{
			string str = (type == null) ? "(unknown)" : type.FullName;
			if (type != null)
			{
				Type baseType = type.BaseType;
				if (baseType != null && baseType.IsGenericType && baseType.GetGenericTypeDefinition().Name == "GeneratedMessage`2")
				{
					throw new InvalidOperationException("Are you mixing protobuf-net and protobuf-csharp-port? See http://stackoverflow.com/q/11564914; type: " + str);
				}
			}
			throw new InvalidOperationException("Type is not expected, and no contract can be inferred: " + str);
		}

		// Token: 0x0600041B RID: 1051 RVA: 0x00015E1B File Offset: 0x0001401B
		internal static Exception CreateNestedListsNotSupported()
		{
			return new NotSupportedException("Nested or jagged lists and arrays are not supported");
		}

		// Token: 0x0600041C RID: 1052 RVA: 0x00015E27 File Offset: 0x00014027
		public static void ThrowCannotCreateInstance(Type type)
		{
			throw new ProtoException("No parameterless constructor found for " + type.Name);
		}

		// Token: 0x0600041D RID: 1053 RVA: 0x00015E40 File Offset: 0x00014040
		internal static string SerializeType(TypeModel model, Type type)
		{
			TypeFormatEventHandler dynamicTypeFormatting;
			if (model != null && (dynamicTypeFormatting = model.DynamicTypeFormatting) != null)
			{
				TypeFormatEventArgs typeFormatEventArgs = new TypeFormatEventArgs(type);
				dynamicTypeFormatting(model, typeFormatEventArgs);
				if (!Helpers.IsNullOrEmpty(typeFormatEventArgs.FormattedName))
				{
					return typeFormatEventArgs.FormattedName;
				}
			}
			return type.AssemblyQualifiedName;
		}

		// Token: 0x0600041E RID: 1054 RVA: 0x00015E84 File Offset: 0x00014084
		internal static Type DeserializeType(TypeModel model, string value)
		{
			TypeFormatEventHandler dynamicTypeFormatting;
			if (model != null && (dynamicTypeFormatting = model.DynamicTypeFormatting) != null)
			{
				TypeFormatEventArgs typeFormatEventArgs = new TypeFormatEventArgs(value);
				dynamicTypeFormatting(model, typeFormatEventArgs);
				if (typeFormatEventArgs.Type != null)
				{
					return typeFormatEventArgs.Type;
				}
			}
			return Type.GetType(value);
		}

		// Token: 0x0600041F RID: 1055 RVA: 0x00015EC2 File Offset: 0x000140C2
		public bool CanSerializeContractType(Type type)
		{
			return this.CanSerialize(type, false, true, true);
		}

		// Token: 0x06000420 RID: 1056 RVA: 0x00015ECE File Offset: 0x000140CE
		public bool CanSerialize(Type type)
		{
			return this.CanSerialize(type, true, true, true);
		}

		// Token: 0x06000421 RID: 1057 RVA: 0x00015EDA File Offset: 0x000140DA
		public bool CanSerializeBasicType(Type type)
		{
			return this.CanSerialize(type, true, false, true);
		}

		// Token: 0x06000422 RID: 1058 RVA: 0x00015EE8 File Offset: 0x000140E8
		private bool CanSerialize(Type type, bool allowBasic, bool allowContract, bool allowLists)
		{
			if (type == null)
			{
				throw new ArgumentNullException("type");
			}
			Type underlyingType = Helpers.GetUnderlyingType(type);
			if (underlyingType != null)
			{
				type = underlyingType;
			}
			ProtoTypeCode typeCode = Helpers.GetTypeCode(type);
			if (typeCode > ProtoTypeCode.Unknown)
			{
				return allowBasic;
			}
			if (this.GetKey(ref type) >= 0)
			{
				return allowContract;
			}
			if (allowLists)
			{
				Type type2 = null;
				if (type.IsArray)
				{
					if (type.GetArrayRank() == 1)
					{
						type2 = type.GetElementType();
					}
				}
				else
				{
					type2 = TypeModel.GetListItemType(this, type);
				}
				if (type2 != null)
				{
					return this.CanSerialize(type2, allowBasic, allowContract, false);
				}
			}
			return false;
		}

		// Token: 0x06000423 RID: 1059 RVA: 0x00015F61 File Offset: 0x00014161
		public virtual string GetSchema(Type type)
		{
			throw new NotSupportedException();
		}

		// Token: 0x14000002 RID: 2
		// (add) Token: 0x06000424 RID: 1060 RVA: 0x00015F68 File Offset: 0x00014168
		// (remove) Token: 0x06000425 RID: 1061 RVA: 0x00015FA0 File Offset: 0x000141A0
		public event TypeFormatEventHandler DynamicTypeFormatting;

		// Token: 0x06000426 RID: 1062 RVA: 0x00015FD5 File Offset: 0x000141D5
		internal virtual Type GetType(string fullName, Assembly context)
		{
			return TypeModel.ResolveKnownType(fullName, this, context);
		}

		// Token: 0x06000427 RID: 1063 RVA: 0x00015FE0 File Offset: 0x000141E0
		internal static Type ResolveKnownType(string name, TypeModel model, Assembly assembly)
		{
			if (Helpers.IsNullOrEmpty(name))
			{
				return null;
			}
			try
			{
				Type type = Type.GetType(name);
				if (type != null)
				{
					return type;
				}
			}
			catch
			{
			}
			try
			{
				int num = name.IndexOf(',');
				string name2 = ((num > 0) ? name.Substring(0, num) : name).Trim();
				if (assembly == null)
				{
					assembly = Assembly.GetCallingAssembly();
				}
				Type type2 = (assembly == null) ? null : assembly.GetType(name2);
				if (type2 != null)
				{
					return type2;
				}
			}
			catch
			{
			}
			return null;
		}

		// Token: 0x040002BC RID: 700
		private static readonly Type ilist = typeof(IList);

		// Token: 0x02000160 RID: 352
		private class DeserializeItemsIterator : IEnumerator, IEnumerable
		{
			// Token: 0x06001163 RID: 4451 RVA: 0x00007E59 File Offset: 0x00006059
			IEnumerator IEnumerable.GetEnumerator()
			{
				return this;
			}

			// Token: 0x06001164 RID: 4452 RVA: 0x000778F4 File Offset: 0x00075AF4
			public bool MoveNext()
			{
				if (this.haveObject)
				{
					int num;
					this.current = this.model.DeserializeWithLengthPrefix(this.source, null, this.type, this.style, this.expectedField, this.resolver, out num, out this.haveObject, this.context);
				}
				return this.haveObject;
			}

			// Token: 0x06001165 RID: 4453 RVA: 0x00015F61 File Offset: 0x00014161
			void IEnumerator.Reset()
			{
				throw new NotSupportedException();
			}

			// Token: 0x17000400 RID: 1024
			// (get) Token: 0x06001166 RID: 4454 RVA: 0x0007794D File Offset: 0x00075B4D
			public object Current
			{
				get
				{
					return this.current;
				}
			}

			// Token: 0x06001167 RID: 4455 RVA: 0x00077958 File Offset: 0x00075B58
			public DeserializeItemsIterator(TypeModel model, Stream source, Type type, PrefixStyle style, int expectedField, Serializer.TypeResolver resolver, SerializationContext context)
			{
				this.haveObject = true;
				this.source = source;
				this.type = type;
				this.style = style;
				this.expectedField = expectedField;
				this.resolver = resolver;
				this.model = model;
				this.context = context;
			}

			// Token: 0x04000E24 RID: 3620
			private bool haveObject;

			// Token: 0x04000E25 RID: 3621
			private object current;

			// Token: 0x04000E26 RID: 3622
			private readonly Stream source;

			// Token: 0x04000E27 RID: 3623
			private readonly Type type;

			// Token: 0x04000E28 RID: 3624
			private readonly PrefixStyle style;

			// Token: 0x04000E29 RID: 3625
			private readonly int expectedField;

			// Token: 0x04000E2A RID: 3626
			private readonly Serializer.TypeResolver resolver;

			// Token: 0x04000E2B RID: 3627
			private readonly TypeModel model;

			// Token: 0x04000E2C RID: 3628
			private readonly SerializationContext context;
		}

		// Token: 0x02000161 RID: 353
		private class DeserializeItemsIterator<T> : TypeModel.DeserializeItemsIterator, IEnumerator<T>, IDisposable, IEnumerator, IEnumerable<T>, IEnumerable
		{
			// Token: 0x06001168 RID: 4456 RVA: 0x00007E59 File Offset: 0x00006059
			IEnumerator<T> IEnumerable<T>.GetEnumerator()
			{
				return this;
			}

			// Token: 0x17000401 RID: 1025
			// (get) Token: 0x06001169 RID: 4457 RVA: 0x000779A7 File Offset: 0x00075BA7
			public new T Current
			{
				get
				{
					return (T)((object)base.Current);
				}
			}

			// Token: 0x0600116A RID: 4458 RVA: 0x00006740 File Offset: 0x00004940
			void IDisposable.Dispose()
			{
			}

			// Token: 0x0600116B RID: 4459 RVA: 0x000779B4 File Offset: 0x00075BB4
			public DeserializeItemsIterator(TypeModel model, Stream source, PrefixStyle style, int expectedField, SerializationContext context) : base(model, source, model.MapType(typeof(T)), style, expectedField, null, context)
			{
			}
		}

		// Token: 0x02000162 RID: 354
		protected internal enum CallbackType
		{
			// Token: 0x04000E2E RID: 3630
			BeforeSerialize,
			// Token: 0x04000E2F RID: 3631
			AfterSerialize,
			// Token: 0x04000E30 RID: 3632
			BeforeDeserialize,
			// Token: 0x04000E31 RID: 3633
			AfterDeserialize
		}
	}
}
