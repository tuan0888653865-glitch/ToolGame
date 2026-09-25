using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Reflection;
using ProtoBuf.Serializers;

namespace ProtoBuf.Meta
{
	// Token: 0x02000073 RID: 115
	public class ValueMember
	{
		// Token: 0x170000DE RID: 222
		// (get) Token: 0x0600042A RID: 1066 RVA: 0x00016081 File Offset: 0x00014281
		public int FieldNumber
		{
			get
			{
				return this.fieldNumber;
			}
		}

		// Token: 0x170000DF RID: 223
		// (get) Token: 0x0600042B RID: 1067 RVA: 0x00016089 File Offset: 0x00014289
		public MemberInfo Member
		{
			get
			{
				return this.member;
			}
		}

		// Token: 0x170000E0 RID: 224
		// (get) Token: 0x0600042C RID: 1068 RVA: 0x00016091 File Offset: 0x00014291
		public Type ItemType
		{
			get
			{
				return this.itemType;
			}
		}

		// Token: 0x170000E1 RID: 225
		// (get) Token: 0x0600042D RID: 1069 RVA: 0x00016099 File Offset: 0x00014299
		public Type MemberType
		{
			get
			{
				return this.memberType;
			}
		}

		// Token: 0x170000E2 RID: 226
		// (get) Token: 0x0600042E RID: 1070 RVA: 0x000160A1 File Offset: 0x000142A1
		public Type DefaultType
		{
			get
			{
				return this.defaultType;
			}
		}

		// Token: 0x170000E3 RID: 227
		// (get) Token: 0x0600042F RID: 1071 RVA: 0x000160A9 File Offset: 0x000142A9
		public Type ParentType
		{
			get
			{
				return this.parentType;
			}
		}

		// Token: 0x170000E4 RID: 228
		// (get) Token: 0x06000430 RID: 1072 RVA: 0x000160B1 File Offset: 0x000142B1
		// (set) Token: 0x06000431 RID: 1073 RVA: 0x000160B9 File Offset: 0x000142B9
		public object DefaultValue
		{
			get
			{
				return this.defaultValue;
			}
			set
			{
				this.ThrowIfFrozen();
				this.defaultValue = value;
			}
		}

		// Token: 0x06000432 RID: 1074 RVA: 0x000160C8 File Offset: 0x000142C8
		public ValueMember(RuntimeTypeModel model, Type parentType, int fieldNumber, MemberInfo member, Type memberType, Type itemType, Type defaultType, DataFormat dataFormat, object defaultValue) : this(model, fieldNumber, memberType, itemType, defaultType, dataFormat)
		{
			if (member == null)
			{
				throw new ArgumentNullException("member");
			}
			if (parentType == null)
			{
				throw new ArgumentNullException("parentType");
			}
			if (fieldNumber < 1 && !Helpers.IsEnum(parentType))
			{
				throw new ArgumentOutOfRangeException("fieldNumber");
			}
			this.member = member;
			this.parentType = parentType;
			if (fieldNumber < 1 && !Helpers.IsEnum(parentType))
			{
				throw new ArgumentOutOfRangeException("fieldNumber");
			}
			if (defaultValue != null && model.MapType(defaultValue.GetType()) != memberType)
			{
				defaultValue = ValueMember.ParseDefaultValue(memberType, defaultValue);
			}
			this.defaultValue = defaultValue;
			MetaType metaType = model.FindWithoutAdd(memberType);
			if (metaType != null)
			{
				this.asReference = metaType.AsReferenceDefault;
			}
		}

		// Token: 0x06000433 RID: 1075 RVA: 0x00016180 File Offset: 0x00014380
		internal ValueMember(RuntimeTypeModel model, int fieldNumber, Type memberType, Type itemType, Type defaultType, DataFormat dataFormat)
		{
			if (memberType == null)
			{
				throw new ArgumentNullException("memberType");
			}
			if (model == null)
			{
				throw new ArgumentNullException("model");
			}
			this.fieldNumber = fieldNumber;
			this.memberType = memberType;
			this.itemType = itemType;
			this.defaultType = defaultType;
			this.model = model;
			this.dataFormat = dataFormat;
		}

		// Token: 0x06000434 RID: 1076 RVA: 0x000161DC File Offset: 0x000143DC
		internal object GetRawEnumValue()
		{
			return ((FieldInfo)this.member).GetRawConstantValue();
		}

		// Token: 0x06000435 RID: 1077 RVA: 0x000161F0 File Offset: 0x000143F0
		private static object ParseDefaultValue(Type type, object value)
		{
			Type underlyingType = Helpers.GetUnderlyingType(type);
			if (underlyingType != null)
			{
				type = underlyingType;
			}
			if (value is string)
			{
				string text = (string)value;
				if (Helpers.IsEnum(type))
				{
					return Helpers.ParseEnum(type, text);
				}
				ProtoTypeCode typeCode = Helpers.GetTypeCode(type);
				switch (typeCode)
				{
				case ProtoTypeCode.Boolean:
					return bool.Parse(text);
				case ProtoTypeCode.Char:
					if (text.Length == 1)
					{
						return text[0];
					}
					throw new FormatException("Single character expected: \"" + text + "\"");
				case ProtoTypeCode.SByte:
					return sbyte.Parse(text, NumberStyles.Integer, CultureInfo.InvariantCulture);
				case ProtoTypeCode.Byte:
					return byte.Parse(text, NumberStyles.Integer, CultureInfo.InvariantCulture);
				case ProtoTypeCode.Int16:
					return short.Parse(text, NumberStyles.Any, CultureInfo.InvariantCulture);
				case ProtoTypeCode.UInt16:
					return ushort.Parse(text, NumberStyles.Any, CultureInfo.InvariantCulture);
				case ProtoTypeCode.Int32:
					return int.Parse(text, NumberStyles.Any, CultureInfo.InvariantCulture);
				case ProtoTypeCode.UInt32:
					return uint.Parse(text, NumberStyles.Any, CultureInfo.InvariantCulture);
				case ProtoTypeCode.Int64:
					return long.Parse(text, NumberStyles.Any, CultureInfo.InvariantCulture);
				case ProtoTypeCode.UInt64:
					return ulong.Parse(text, NumberStyles.Any, CultureInfo.InvariantCulture);
				case ProtoTypeCode.Single:
					return float.Parse(text, NumberStyles.Any, CultureInfo.InvariantCulture);
				case ProtoTypeCode.Double:
					return double.Parse(text, NumberStyles.Any, CultureInfo.InvariantCulture);
				case ProtoTypeCode.Decimal:
					return decimal.Parse(text, NumberStyles.Any, CultureInfo.InvariantCulture);
				case ProtoTypeCode.DateTime:
					return DateTime.Parse(text, CultureInfo.InvariantCulture);
				case (ProtoTypeCode)17:
					break;
				case ProtoTypeCode.String:
					return text;
				default:
					switch (typeCode)
					{
					case ProtoTypeCode.TimeSpan:
						return TimeSpan.Parse(text);
					case ProtoTypeCode.Guid:
						return new Guid(text);
					case ProtoTypeCode.Uri:
						return text;
					}
					break;
				}
			}
			if (Helpers.IsEnum(type))
			{
				return Enum.ToObject(type, value);
			}
			return Convert.ChangeType(value, type, CultureInfo.InvariantCulture);
		}

		// Token: 0x170000E5 RID: 229
		// (get) Token: 0x06000436 RID: 1078 RVA: 0x00016406 File Offset: 0x00014606
		internal IProtoSerializer Serializer
		{
			get
			{
				if (this.serializer == null)
				{
					this.serializer = this.BuildSerializer();
				}
				return this.serializer;
			}
		}

		// Token: 0x170000E6 RID: 230
		// (get) Token: 0x06000437 RID: 1079 RVA: 0x00016422 File Offset: 0x00014622
		// (set) Token: 0x06000438 RID: 1080 RVA: 0x0001642A File Offset: 0x0001462A
		public DataFormat DataFormat
		{
			get
			{
				return this.dataFormat;
			}
			set
			{
				this.ThrowIfFrozen();
				this.dataFormat = value;
			}
		}

		// Token: 0x170000E7 RID: 231
		// (get) Token: 0x06000439 RID: 1081 RVA: 0x00016439 File Offset: 0x00014639
		// (set) Token: 0x0600043A RID: 1082 RVA: 0x00016442 File Offset: 0x00014642
		public bool IsStrict
		{
			get
			{
				return this.HasFlag(1);
			}
			set
			{
				this.SetFlag(1, value, true);
			}
		}

		// Token: 0x170000E8 RID: 232
		// (get) Token: 0x0600043B RID: 1083 RVA: 0x0001644D File Offset: 0x0001464D
		// (set) Token: 0x0600043C RID: 1084 RVA: 0x00016456 File Offset: 0x00014656
		public bool IsPacked
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

		// Token: 0x170000E9 RID: 233
		// (get) Token: 0x0600043D RID: 1085 RVA: 0x00016461 File Offset: 0x00014661
		// (set) Token: 0x0600043E RID: 1086 RVA: 0x0001646A File Offset: 0x0001466A
		public bool OverwriteList
		{
			get
			{
				return this.HasFlag(8);
			}
			set
			{
				this.SetFlag(8, value, true);
			}
		}

		// Token: 0x170000EA RID: 234
		// (get) Token: 0x0600043F RID: 1087 RVA: 0x00016475 File Offset: 0x00014675
		// (set) Token: 0x06000440 RID: 1088 RVA: 0x0001647E File Offset: 0x0001467E
		public bool IsRequired
		{
			get
			{
				return this.HasFlag(4);
			}
			set
			{
				this.SetFlag(4, value, true);
			}
		}

		// Token: 0x170000EB RID: 235
		// (get) Token: 0x06000441 RID: 1089 RVA: 0x00016489 File Offset: 0x00014689
		// (set) Token: 0x06000442 RID: 1090 RVA: 0x00016491 File Offset: 0x00014691
		public bool AsReference
		{
			get
			{
				return this.asReference;
			}
			set
			{
				this.ThrowIfFrozen();
				this.asReference = value;
			}
		}

		// Token: 0x170000EC RID: 236
		// (get) Token: 0x06000443 RID: 1091 RVA: 0x000164A0 File Offset: 0x000146A0
		// (set) Token: 0x06000444 RID: 1092 RVA: 0x000164A8 File Offset: 0x000146A8
		public bool DynamicType
		{
			get
			{
				return this.dynamicType;
			}
			set
			{
				this.ThrowIfFrozen();
				this.dynamicType = value;
			}
		}

		// Token: 0x06000445 RID: 1093 RVA: 0x000164B8 File Offset: 0x000146B8
		public void SetSpecified(MethodInfo getSpecified, MethodInfo setSpecified)
		{
			if (getSpecified != null && (getSpecified.ReturnType != this.model.MapType(typeof(bool)) || getSpecified.IsStatic || getSpecified.GetParameters().Length != 0))
			{
				throw new ArgumentException("Invalid pattern for checking member-specified", "getSpecified");
			}
			ParameterInfo[] parameters;
			if (setSpecified != null && (setSpecified.ReturnType != this.model.MapType(typeof(void)) || setSpecified.IsStatic || (parameters = setSpecified.GetParameters()).Length != 1 || parameters[0].ParameterType != this.model.MapType(typeof(bool))))
			{
				throw new ArgumentException("Invalid pattern for setting member-specified", "setSpecified");
			}
			this.ThrowIfFrozen();
			this.getSpecified = getSpecified;
			this.setSpecified = setSpecified;
		}

		// Token: 0x06000446 RID: 1094 RVA: 0x0001657E File Offset: 0x0001477E
		private void ThrowIfFrozen()
		{
			if (this.serializer != null)
			{
				throw new InvalidOperationException("The type cannot be changed once a serializer has been generated");
			}
		}

		// Token: 0x06000447 RID: 1095 RVA: 0x00016594 File Offset: 0x00014794
		private IProtoSerializer BuildSerializer()
		{
			int opaqueToken = 0;
			IProtoSerializer result;
			try
			{
				this.model.TakeLock(ref opaqueToken);
				Type type = (this.itemType == null) ? this.memberType : this.itemType;
				WireType wireType;
				IProtoSerializer protoSerializer = ValueMember.TryGetCoreSerializer(this.model, this.dataFormat, type, out wireType, this.asReference, this.dynamicType, this.OverwriteList, true);
				if (protoSerializer == null)
				{
					throw new InvalidOperationException("No serializer defined for type: " + type.FullName);
				}
				if (this.itemType != null && this.SupportNull)
				{
					if (this.IsPacked)
					{
						throw new NotSupportedException("Packed encodings cannot support null values");
					}
					protoSerializer = new TagDecorator(1, wireType, this.IsStrict, protoSerializer);
					protoSerializer = new NullDecorator(this.model, protoSerializer);
					protoSerializer = new TagDecorator(this.fieldNumber, WireType.StartGroup, false, protoSerializer);
				}
				else
				{
					protoSerializer = new TagDecorator(this.fieldNumber, wireType, this.IsStrict, protoSerializer);
				}
				if (this.itemType != null)
				{
					if (!this.SupportNull)
					{
						Helpers.GetUnderlyingType(this.itemType);
					}
					if (this.memberType.IsArray)
					{
						protoSerializer = new ArrayDecorator(this.model, protoSerializer, this.fieldNumber, this.IsPacked, wireType, this.memberType, this.OverwriteList, this.SupportNull);
					}
					else
					{
						protoSerializer = new ListDecorator(this.model, this.memberType, this.defaultType, protoSerializer, this.fieldNumber, this.IsPacked, wireType, this.member != null && PropertyDecorator.CanWrite(this.model, this.member), this.OverwriteList, this.SupportNull);
					}
				}
				else if (this.defaultValue != null && !this.IsRequired && this.getSpecified == null)
				{
					protoSerializer = new DefaultValueDecorator(this.model, this.defaultValue, protoSerializer);
				}
				if (this.memberType == this.model.MapType(typeof(Uri)))
				{
					protoSerializer = new UriDecorator(this.model, protoSerializer);
				}
				if (this.member != null)
				{
					if (this.member is PropertyInfo)
					{
						protoSerializer = new PropertyDecorator(this.model, this.parentType, (PropertyInfo)this.member, protoSerializer);
					}
					else
					{
						if (!(this.member is FieldInfo))
						{
							throw new InvalidOperationException();
						}
						protoSerializer = new FieldDecorator(this.parentType, (FieldInfo)this.member, protoSerializer);
					}
					if (this.getSpecified != null || this.setSpecified != null)
					{
						protoSerializer = new MemberSpecifiedDecorator(this.getSpecified, this.setSpecified, protoSerializer);
					}
				}
				result = protoSerializer;
			}
			finally
			{
				this.model.ReleaseLock(opaqueToken);
			}
			return result;
		}

		// Token: 0x06000448 RID: 1096 RVA: 0x00016838 File Offset: 0x00014A38
		private static WireType GetIntWireType(DataFormat format, int width)
		{
			switch (format)
			{
			case DataFormat.Default:
			case DataFormat.TwosComplement:
				return WireType.Variant;
			case DataFormat.ZigZag:
				return WireType.SignedVariant;
			case DataFormat.FixedSize:
				if (width != 32)
				{
					return WireType.Fixed64;
				}
				return WireType.Fixed32;
			default:
				throw new InvalidOperationException();
			}
		}

		// Token: 0x06000449 RID: 1097 RVA: 0x00016864 File Offset: 0x00014A64
		private static WireType GetDateTimeWireType(DataFormat format)
		{
			switch (format)
			{
			case DataFormat.Default:
				return WireType.String;
			case DataFormat.FixedSize:
				return WireType.Fixed64;
			case DataFormat.Group:
				return WireType.StartGroup;
			}
			throw new InvalidOperationException();
		}

		// Token: 0x0600044A RID: 1098 RVA: 0x00016890 File Offset: 0x00014A90
		internal static IProtoSerializer TryGetCoreSerializer(RuntimeTypeModel model, DataFormat dataFormat, Type type, out WireType defaultWireType, bool asReference, bool dynamicType, bool overwriteList, bool allowComplexTypes)
		{
			type = (Helpers.GetUnderlyingType(type) ?? type);
			if (Helpers.IsEnum(type))
			{
				if (allowComplexTypes && model != null)
				{
					defaultWireType = WireType.Variant;
					return new EnumSerializer(type, model.GetEnumMap(type));
				}
				defaultWireType = WireType.None;
				return null;
			}
			else
			{
				ProtoTypeCode typeCode = Helpers.GetTypeCode(type);
				switch (typeCode)
				{
				case ProtoTypeCode.Boolean:
					defaultWireType = WireType.Variant;
					return new BooleanSerializer(model);
				case ProtoTypeCode.Char:
					defaultWireType = WireType.Variant;
					return new CharSerializer(model);
				case ProtoTypeCode.SByte:
					defaultWireType = ValueMember.GetIntWireType(dataFormat, 32);
					return new SByteSerializer(model);
				case ProtoTypeCode.Byte:
					defaultWireType = ValueMember.GetIntWireType(dataFormat, 32);
					return new ByteSerializer(model);
				case ProtoTypeCode.Int16:
					defaultWireType = ValueMember.GetIntWireType(dataFormat, 32);
					return new Int16Serializer(model);
				case ProtoTypeCode.UInt16:
					defaultWireType = ValueMember.GetIntWireType(dataFormat, 32);
					return new UInt16Serializer(model);
				case ProtoTypeCode.Int32:
					defaultWireType = ValueMember.GetIntWireType(dataFormat, 32);
					return new Int32Serializer(model);
				case ProtoTypeCode.UInt32:
					defaultWireType = ValueMember.GetIntWireType(dataFormat, 32);
					return new UInt32Serializer(model);
				case ProtoTypeCode.Int64:
					defaultWireType = ValueMember.GetIntWireType(dataFormat, 64);
					return new Int64Serializer(model);
				case ProtoTypeCode.UInt64:
					defaultWireType = ValueMember.GetIntWireType(dataFormat, 64);
					return new UInt64Serializer(model);
				case ProtoTypeCode.Single:
					defaultWireType = WireType.Fixed32;
					return new SingleSerializer(model);
				case ProtoTypeCode.Double:
					defaultWireType = WireType.Fixed64;
					return new DoubleSerializer(model);
				case ProtoTypeCode.Decimal:
					defaultWireType = WireType.String;
					return new DecimalSerializer(model);
				case ProtoTypeCode.DateTime:
					defaultWireType = ValueMember.GetDateTimeWireType(dataFormat);
					return new DateTimeSerializer(model);
				case (ProtoTypeCode)17:
					break;
				case ProtoTypeCode.String:
					defaultWireType = WireType.String;
					if (asReference)
					{
						return new NetObjectSerializer(model, model.MapType(typeof(string)), 0, BclHelpers.NetObjectOptions.AsReference);
					}
					return new StringSerializer(model);
				default:
					switch (typeCode)
					{
					case ProtoTypeCode.TimeSpan:
						defaultWireType = ValueMember.GetDateTimeWireType(dataFormat);
						return new TimeSpanSerializer(model);
					case ProtoTypeCode.ByteArray:
						defaultWireType = WireType.String;
						return new BlobSerializer(model, overwriteList);
					case ProtoTypeCode.Guid:
						defaultWireType = WireType.String;
						return new GuidSerializer(model);
					case ProtoTypeCode.Uri:
						defaultWireType = WireType.String;
						return new StringSerializer(model);
					case ProtoTypeCode.Type:
						defaultWireType = WireType.String;
						return new SystemTypeSerializer(model);
					}
					break;
				}
				IProtoSerializer protoSerializer = model.AllowParseableTypes ? ParseableSerializer.TryCreate(type, model) : null;
				if (protoSerializer != null)
				{
					defaultWireType = WireType.String;
					return protoSerializer;
				}
				if (allowComplexTypes && model != null)
				{
					int key = model.GetKey(type, false, true);
					if (asReference || dynamicType)
					{
						defaultWireType = WireType.String;
						BclHelpers.NetObjectOptions netObjectOptions = BclHelpers.NetObjectOptions.None;
						if (asReference)
						{
							netObjectOptions |= BclHelpers.NetObjectOptions.AsReference;
						}
						if (dynamicType)
						{
							netObjectOptions |= BclHelpers.NetObjectOptions.DynamicType;
						}
						if (key >= 0 && model[type].UseConstructor)
						{
							netObjectOptions |= BclHelpers.NetObjectOptions.UseConstructor;
						}
						return new NetObjectSerializer(model, type, key, netObjectOptions);
					}
					if (key >= 0)
					{
						defaultWireType = ((dataFormat == DataFormat.Group) ? WireType.StartGroup : WireType.String);
						return new SubItemSerializer(type, key, model[type], true);
					}
				}
				defaultWireType = WireType.None;
				return null;
			}
		}

		// Token: 0x0600044B RID: 1099 RVA: 0x00016AF5 File Offset: 0x00014CF5
		internal void SetName(string name)
		{
			this.ThrowIfFrozen();
			this.name = name;
		}

		// Token: 0x170000ED RID: 237
		// (get) Token: 0x0600044C RID: 1100 RVA: 0x00016B04 File Offset: 0x00014D04
		public string Name
		{
			get
			{
				if (!Helpers.IsNullOrEmpty(this.name))
				{
					return this.name;
				}
				return this.member.Name;
			}
		}

		// Token: 0x0600044D RID: 1101 RVA: 0x00016B25 File Offset: 0x00014D25
		private bool HasFlag(byte flag)
		{
			return (this.flags & flag) == flag;
		}

		// Token: 0x0600044E RID: 1102 RVA: 0x00016B32 File Offset: 0x00014D32
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

		// Token: 0x170000EE RID: 238
		// (get) Token: 0x0600044F RID: 1103 RVA: 0x00016B6B File Offset: 0x00014D6B
		// (set) Token: 0x06000450 RID: 1104 RVA: 0x00016B75 File Offset: 0x00014D75
		public bool SupportNull
		{
			get
			{
				return this.HasFlag(16);
			}
			set
			{
				this.SetFlag(16, value, true);
			}
		}

		// Token: 0x06000451 RID: 1105 RVA: 0x00016B84 File Offset: 0x00014D84
		internal string GetSchemaTypeName(bool applyNetObjectProxy, ref bool requiresBclImport)
		{
			Type type = this.ItemType;
			if (type == null)
			{
				type = this.MemberType;
			}
			return this.model.GetSchemaTypeName(type, this.DataFormat, applyNetObjectProxy && this.asReference, applyNetObjectProxy && this.dynamicType, ref requiresBclImport);
		}

		// Token: 0x040002BD RID: 701
		private const byte OPTIONS_IsStrict = 1;

		// Token: 0x040002BE RID: 702
		private const byte OPTIONS_IsPacked = 2;

		// Token: 0x040002BF RID: 703
		private const byte OPTIONS_IsRequired = 4;

		// Token: 0x040002C0 RID: 704
		private const byte OPTIONS_OverwriteList = 8;

		// Token: 0x040002C1 RID: 705
		private const byte OPTIONS_SupportNull = 16;

		// Token: 0x040002C2 RID: 706
		private readonly int fieldNumber;

		// Token: 0x040002C3 RID: 707
		private readonly MemberInfo member;

		// Token: 0x040002C4 RID: 708
		private readonly Type parentType;

		// Token: 0x040002C5 RID: 709
		private readonly Type itemType;

		// Token: 0x040002C6 RID: 710
		private readonly Type defaultType;

		// Token: 0x040002C7 RID: 711
		private readonly Type memberType;

		// Token: 0x040002C8 RID: 712
		private object defaultValue;

		// Token: 0x040002C9 RID: 713
		private readonly RuntimeTypeModel model;

		// Token: 0x040002CA RID: 714
		private IProtoSerializer serializer;

		// Token: 0x040002CB RID: 715
		private DataFormat dataFormat;

		// Token: 0x040002CC RID: 716
		private bool asReference;

		// Token: 0x040002CD RID: 717
		private bool dynamicType;

		// Token: 0x040002CE RID: 718
		private MethodInfo getSpecified;

		// Token: 0x040002CF RID: 719
		private MethodInfo setSpecified;

		// Token: 0x040002D0 RID: 720
		private string name;

		// Token: 0x040002D1 RID: 721
		private byte flags;

		// Token: 0x02000163 RID: 355
		internal class Comparer : IComparer, IComparer<ValueMember>
		{
			// Token: 0x0600116C RID: 4460 RVA: 0x000779D4 File Offset: 0x00075BD4
			public int Compare(object x, object y)
			{
				return this.Compare(x as ValueMember, y as ValueMember);
			}

			// Token: 0x0600116D RID: 4461 RVA: 0x000779E8 File Offset: 0x00075BE8
			public int Compare(ValueMember x, ValueMember y)
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
				return x.FieldNumber.CompareTo(y.FieldNumber);
			}

			// Token: 0x04000E32 RID: 3634
			public static readonly ValueMember.Comparer Default = new ValueMember.Comparer();
		}
	}
}
