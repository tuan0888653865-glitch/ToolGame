using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Threading;
using ProtoBuf.Serializers;

namespace ProtoBuf.Meta
{
	// Token: 0x0200006E RID: 110
	public sealed class RuntimeTypeModel : TypeModel
	{
		// Token: 0x060003B8 RID: 952 RVA: 0x00013975 File Offset: 0x00011B75
		private bool GetOption(byte option)
		{
			return (this.options & option) == option;
		}

		// Token: 0x060003B9 RID: 953 RVA: 0x00013982 File Offset: 0x00011B82
		private void SetOption(byte option, bool value)
		{
			if (value)
			{
				this.options |= option;
				return;
			}
			this.options &= ~option;
		}

		// Token: 0x170000D1 RID: 209
		// (get) Token: 0x060003BA RID: 954 RVA: 0x000139A8 File Offset: 0x00011BA8
		// (set) Token: 0x060003BB RID: 955 RVA: 0x000139B1 File Offset: 0x00011BB1
		public bool InferTagFromNameDefault
		{
			get
			{
				return this.GetOption(1);
			}
			set
			{
				this.SetOption(1, value);
			}
		}

		// Token: 0x170000D2 RID: 210
		// (get) Token: 0x060003BC RID: 956 RVA: 0x000139BB File Offset: 0x00011BBB
		// (set) Token: 0x060003BD RID: 957 RVA: 0x000139C8 File Offset: 0x00011BC8
		public bool AutoAddProtoContractTypesOnly
		{
			get
			{
				return this.GetOption(128);
			}
			set
			{
				this.SetOption(128, value);
			}
		}

		// Token: 0x170000D3 RID: 211
		// (get) Token: 0x060003BE RID: 958 RVA: 0x000139D6 File Offset: 0x00011BD6
		// (set) Token: 0x060003BF RID: 959 RVA: 0x000139E0 File Offset: 0x00011BE0
		public bool UseImplicitZeroDefaults
		{
			get
			{
				return this.GetOption(32);
			}
			set
			{
				if (!value && this.GetOption(2))
				{
					throw new InvalidOperationException("UseImplicitZeroDefaults cannot be disabled on the default model");
				}
				this.SetOption(32, value);
			}
		}

		// Token: 0x170000D4 RID: 212
		// (get) Token: 0x060003C0 RID: 960 RVA: 0x00013A02 File Offset: 0x00011C02
		// (set) Token: 0x060003C1 RID: 961 RVA: 0x00013A0C File Offset: 0x00011C0C
		public bool AllowParseableTypes
		{
			get
			{
				return this.GetOption(64);
			}
			set
			{
				if (value && this.GetOption(2))
				{
					throw new InvalidOperationException("AllowParseableTypes cannot be enabled on the default model");
				}
				this.SetOption(64, value);
			}
		}

		// Token: 0x170000D5 RID: 213
		// (get) Token: 0x060003C2 RID: 962 RVA: 0x00013A2E File Offset: 0x00011C2E
		public static RuntimeTypeModel Default
		{
			get
			{
				return RuntimeTypeModel.Singleton.Value;
			}
		}

		// Token: 0x060003C3 RID: 963 RVA: 0x00013A35 File Offset: 0x00011C35
		public IEnumerable GetTypes()
		{
			return this.types;
		}

		// Token: 0x060003C4 RID: 964 RVA: 0x00013A40 File Offset: 0x00011C40
		public override string GetSchema(Type type)
		{
			BasicList basicList = new BasicList();
			MetaType metaType = null;
			bool flag = false;
			if (type == null)
			{
				foreach (object obj in this.types)
				{
					MetaType surrogateOrBaseOrSelf = ((MetaType)obj).GetSurrogateOrBaseOrSelf();
					if (!basicList.Contains(surrogateOrBaseOrSelf))
					{
						basicList.Add(surrogateOrBaseOrSelf);
						this.CascadeDependents(basicList, surrogateOrBaseOrSelf);
					}
				}
			}
			else
			{
				Type underlyingType = Helpers.GetUnderlyingType(type);
				if (underlyingType != null)
				{
					type = underlyingType;
				}
				WireType wireType;
				flag = (ValueMember.TryGetCoreSerializer(this, DataFormat.Default, type, out wireType, false, false, false, false) != null);
				if (!flag)
				{
					int num = this.FindOrAddAuto(type, false, false, false);
					if (num < 0)
					{
						throw new ArgumentException("The type specified is not a contract-type", "type");
					}
					metaType = ((MetaType)this.types[num]).GetSurrogateOrBaseOrSelf();
					basicList.Add(metaType);
					this.CascadeDependents(basicList, metaType);
				}
			}
			StringBuilder stringBuilder = new StringBuilder();
			string text = null;
			if (!flag)
			{
				foreach (object obj2 in ((IEnumerable)((metaType == null) ? this.types : basicList)))
				{
					MetaType metaType2 = (MetaType)obj2;
					if (!metaType2.IsList)
					{
						string @namespace = metaType2.Type.Namespace;
						if (!Helpers.IsNullOrEmpty(@namespace) && !@namespace.StartsWith("System."))
						{
							if (text == null)
							{
								text = @namespace;
							}
							else if (!(text == @namespace))
							{
								text = null;
								break;
							}
						}
					}
				}
			}
			if (!Helpers.IsNullOrEmpty(text))
			{
				stringBuilder.Append("package ").Append(text).Append(';');
				Helpers.AppendLine(stringBuilder);
			}
			bool flag2 = false;
			StringBuilder stringBuilder2 = new StringBuilder();
			MetaType[] array = new MetaType[basicList.Count];
			basicList.CopyTo(array, 0);
			Array.Sort<MetaType>(array, MetaType.Comparer.Default);
			if (flag)
			{
				Helpers.AppendLine(stringBuilder2).Append("message ").Append(type.Name).Append(" {");
				MetaType.NewLine(stringBuilder2, 1).Append("optional ").Append(this.GetSchemaTypeName(type, DataFormat.Default, false, false, ref flag2)).Append(" value = 1;");
				Helpers.AppendLine(stringBuilder2).Append('}');
			}
			else
			{
				foreach (MetaType metaType3 in array)
				{
					if (!metaType3.IsList || metaType3 == metaType)
					{
						metaType3.WriteSchema(stringBuilder2, 0, ref flag2);
					}
				}
			}
			if (flag2)
			{
				stringBuilder.Append("import \"bcl.proto\" // schema for protobuf-net's handling of core .NET types");
				Helpers.AppendLine(stringBuilder);
			}
			return Helpers.AppendLine(stringBuilder.Append(stringBuilder2)).ToString();
		}

		// Token: 0x060003C5 RID: 965 RVA: 0x00013CDC File Offset: 0x00011EDC
		private void CascadeDependents(BasicList list, MetaType metaType)
		{
			if (metaType.IsList)
			{
				Type listItemType = TypeModel.GetListItemType(this, metaType.Type);
				WireType wireType;
				if (ValueMember.TryGetCoreSerializer(this, DataFormat.Default, listItemType, out wireType, false, false, false, false) == null)
				{
					int num = this.FindOrAddAuto(listItemType, false, false, false);
					if (num >= 0)
					{
						MetaType surrogateOrBaseOrSelf = ((MetaType)this.types[num]).GetSurrogateOrBaseOrSelf();
						if (!list.Contains(surrogateOrBaseOrSelf))
						{
							list.Add(surrogateOrBaseOrSelf);
							this.CascadeDependents(list, surrogateOrBaseOrSelf);
							return;
						}
					}
				}
			}
			else
			{
				MetaType metaType2;
				if (metaType.IsAutoTuple)
				{
					MemberInfo[] array;
					if (MetaType.ResolveTupleConstructor(metaType.Type, out array) != null)
					{
						for (int i = 0; i < array.Length; i++)
						{
							Type type = null;
							if (array[i] is PropertyInfo)
							{
								type = ((PropertyInfo)array[i]).PropertyType;
							}
							else if (array[i] is FieldInfo)
							{
								type = ((FieldInfo)array[i]).FieldType;
							}
							WireType wireType2;
							if (ValueMember.TryGetCoreSerializer(this, DataFormat.Default, type, out wireType2, false, false, false, false) == null)
							{
								int num2 = this.FindOrAddAuto(type, false, false, false);
								if (num2 >= 0)
								{
									metaType2 = ((MetaType)this.types[num2]).GetSurrogateOrBaseOrSelf();
									if (!list.Contains(metaType2))
									{
										list.Add(metaType2);
										this.CascadeDependents(list, metaType2);
									}
								}
							}
						}
					}
				}
				else
				{
					foreach (object obj in metaType.Fields)
					{
						ValueMember valueMember = (ValueMember)obj;
						Type type2 = valueMember.ItemType;
						if (type2 == null)
						{
							type2 = valueMember.MemberType;
						}
						WireType wireType3;
						if (ValueMember.TryGetCoreSerializer(this, DataFormat.Default, type2, out wireType3, false, false, false, false) == null)
						{
							int num3 = this.FindOrAddAuto(type2, false, false, false);
							if (num3 >= 0)
							{
								metaType2 = ((MetaType)this.types[num3]).GetSurrogateOrBaseOrSelf();
								if (!list.Contains(metaType2))
								{
									list.Add(metaType2);
									this.CascadeDependents(list, metaType2);
								}
							}
						}
					}
				}
				if (metaType.HasSubtypes)
				{
					SubType[] subtypes = metaType.GetSubtypes();
					for (int j = 0; j < subtypes.Length; j++)
					{
						metaType2 = subtypes[j].DerivedType.GetSurrogateOrSelf();
						if (!list.Contains(metaType2))
						{
							list.Add(metaType2);
							this.CascadeDependents(list, metaType2);
						}
					}
				}
				metaType2 = metaType.BaseType;
				if (metaType2 != null)
				{
					metaType2 = metaType2.GetSurrogateOrSelf();
				}
				if (metaType2 != null && !list.Contains(metaType2))
				{
					list.Add(metaType2);
					this.CascadeDependents(list, metaType2);
				}
			}
		}

		// Token: 0x060003C6 RID: 966 RVA: 0x00013F6C File Offset: 0x0001216C
		internal RuntimeTypeModel(bool isDefault)
		{
			this.AutoAddMissingTypes = true;
			this.UseImplicitZeroDefaults = true;
			this.SetOption(2, isDefault);
		}

		// Token: 0x170000D6 RID: 214
		public MetaType this[Type type]
		{
			get
			{
				return (MetaType)this.types[this.FindOrAddAuto(type, true, false, false)];
			}
		}

		// Token: 0x060003C8 RID: 968 RVA: 0x00013FC4 File Offset: 0x000121C4
		internal MetaType FindWithoutAdd(Type type)
		{
			foreach (object obj in this.types)
			{
				MetaType metaType = (MetaType)obj;
				if (metaType.Type == type)
				{
					if (metaType.Pending)
					{
						this.WaitOnLock(metaType);
					}
					return metaType;
				}
			}
			Type type2 = TypeModel.ResolveProxies(type);
			if (type2 != null)
			{
				return this.FindWithoutAdd(type2);
			}
			return null;
		}

		// Token: 0x060003C9 RID: 969 RVA: 0x0001404C File Offset: 0x0001224C
		private void WaitOnLock(MetaType type)
		{
			int opaqueToken = 0;
			try
			{
				this.TakeLock(ref opaqueToken);
			}
			finally
			{
				this.ReleaseLock(opaqueToken);
			}
		}

		// Token: 0x060003CA RID: 970 RVA: 0x00014080 File Offset: 0x00012280
		internal int FindOrAddAuto(Type type, bool demand, bool addWithContractOnly, bool addEvenIfAutoDisabled)
		{
			RuntimeTypeModel.TypeFinder predicate = new RuntimeTypeModel.TypeFinder(type);
			int num = this.types.IndexOf(predicate);
			MetaType metaType;
			if (num >= 0 && (metaType = (MetaType)this.types[num]).Pending)
			{
				this.WaitOnLock(metaType);
			}
			if (num < 0)
			{
				Type type2 = TypeModel.ResolveProxies(type);
				if (type2 != null)
				{
					predicate = new RuntimeTypeModel.TypeFinder(type2);
					num = this.types.IndexOf(predicate);
					type = type2;
				}
			}
			if (num < 0)
			{
				int opaqueToken = 0;
				try
				{
					this.TakeLock(ref opaqueToken);
					if ((metaType = this.RecogniseCommonTypes(type)) == null)
					{
						MetaType.AttributeFamily contractFamily = MetaType.GetContractFamily(this, type, null);
						if (contractFamily == MetaType.AttributeFamily.AutoTuple)
						{
							addEvenIfAutoDisabled = true;
						}
						if ((!this.AutoAddMissingTypes && !addEvenIfAutoDisabled) || (!Helpers.IsEnum(type) && addWithContractOnly && contractFamily == MetaType.AttributeFamily.None))
						{
							if (demand)
							{
								TypeModel.ThrowUnexpectedType(type);
							}
							return num;
						}
						metaType = this.Create(type);
					}
					metaType.Pending = true;
					bool flag = false;
					int num2 = this.types.IndexOf(predicate);
					if (num2 < 0)
					{
						this.ThrowIfFrozen();
						num = this.types.Add(metaType);
						flag = true;
					}
					else
					{
						num = num2;
					}
					if (flag)
					{
						metaType.ApplyDefaultBehaviour();
						metaType.Pending = false;
					}
				}
				finally
				{
					this.ReleaseLock(opaqueToken);
				}
				return num;
			}
			return num;
		}

		// Token: 0x060003CB RID: 971 RVA: 0x000141B8 File Offset: 0x000123B8
		private MetaType RecogniseCommonTypes(Type type)
		{
			return null;
		}

		// Token: 0x060003CC RID: 972 RVA: 0x000141BB File Offset: 0x000123BB
		private MetaType Create(Type type)
		{
			this.ThrowIfFrozen();
			return new MetaType(this, type);
		}

		// Token: 0x060003CD RID: 973 RVA: 0x000141CC File Offset: 0x000123CC
		public MetaType Add(Type type, bool applyDefaultBehaviour)
		{
			if (type == null)
			{
				throw new ArgumentNullException("type");
			}
			MetaType metaType = this.FindWithoutAdd(type);
			if (metaType != null)
			{
				return metaType;
			}
			int opaqueToken = 0;
			if (type.IsInterface && base.MapType(MetaType.ienumerable).IsAssignableFrom(type) && TypeModel.GetListItemType(this, type) == null)
			{
				throw new ArgumentException("IEnumerable[<T>] data cannot be used as a meta-type unless an Add method can be resolved");
			}
			try
			{
				metaType = this.RecogniseCommonTypes(type);
				if (metaType != null)
				{
					if (!applyDefaultBehaviour)
					{
						throw new ArgumentException("Default behaviour must be observed for certain types with special handling; " + type.FullName, "applyDefaultBehaviour");
					}
					applyDefaultBehaviour = false;
				}
				if (metaType == null)
				{
					metaType = this.Create(type);
				}
				metaType.Pending = true;
				this.TakeLock(ref opaqueToken);
				if (this.FindWithoutAdd(type) != null)
				{
					throw new ArgumentException("Duplicate type", "type");
				}
				this.ThrowIfFrozen();
				this.types.Add(metaType);
				if (applyDefaultBehaviour)
				{
					metaType.ApplyDefaultBehaviour();
				}
				metaType.Pending = false;
			}
			finally
			{
				this.ReleaseLock(opaqueToken);
			}
			return metaType;
		}

		// Token: 0x170000D7 RID: 215
		// (get) Token: 0x060003CE RID: 974 RVA: 0x000142C4 File Offset: 0x000124C4
		// (set) Token: 0x060003CF RID: 975 RVA: 0x000142CD File Offset: 0x000124CD
		public bool AutoAddMissingTypes
		{
			get
			{
				return this.GetOption(8);
			}
			set
			{
				if (!value && this.GetOption(2))
				{
					throw new InvalidOperationException("The default model must allow missing types");
				}
				this.ThrowIfFrozen();
				this.SetOption(8, value);
			}
		}

		// Token: 0x060003D0 RID: 976 RVA: 0x000142F4 File Offset: 0x000124F4
		private void ThrowIfFrozen()
		{
			if (this.GetOption(4))
			{
				throw new InvalidOperationException("The model cannot be changed once frozen");
			}
		}

		// Token: 0x060003D1 RID: 977 RVA: 0x0001430A File Offset: 0x0001250A
		public void Freeze()
		{
			if (this.GetOption(2))
			{
				throw new InvalidOperationException("The default model cannot be frozen");
			}
			this.SetOption(4, true);
		}

		// Token: 0x060003D2 RID: 978 RVA: 0x00014328 File Offset: 0x00012528
		protected override int GetKeyImpl(Type type)
		{
			return this.GetKey(type, false, true);
		}

		// Token: 0x060003D3 RID: 979 RVA: 0x00014334 File Offset: 0x00012534
		internal int GetKey(Type type, bool demand, bool getBaseKey)
		{
			int result;
			try
			{
				int num = this.FindOrAddAuto(type, demand, true, false);
				if (num >= 0)
				{
					MetaType metaType = (MetaType)this.types[num];
					if (getBaseKey)
					{
						metaType = MetaType.GetRootType(metaType);
						num = this.FindOrAddAuto(metaType.Type, true, true, false);
					}
				}
				result = num;
			}
			catch (NotSupportedException)
			{
				throw;
			}
			catch (Exception ex)
			{
				if (ex.Message.IndexOf(type.FullName) >= 0)
				{
					throw;
				}
				throw new ProtoException(ex.Message + " (" + type.FullName + ")", ex);
			}
			return result;
		}

		// Token: 0x060003D4 RID: 980 RVA: 0x000143DC File Offset: 0x000125DC
		protected internal override void Serialize(int key, object value, ProtoWriter dest)
		{
			((MetaType)this.types[key]).Serializer.Write(value, dest);
		}

		// Token: 0x060003D5 RID: 981 RVA: 0x000143FC File Offset: 0x000125FC
		protected internal override object Deserialize(int key, object value, ProtoReader source)
		{
			IProtoSerializer serializer = ((MetaType)this.types[key]).Serializer;
			if (value == null && Helpers.IsValueType(serializer.ExpectedType))
			{
				if (serializer.RequiresOldValue)
				{
					value = Activator.CreateInstance(serializer.ExpectedType);
				}
				return serializer.Read(value, source);
			}
			return serializer.Read(value, source);
		}

		// Token: 0x060003D6 RID: 982 RVA: 0x00014456 File Offset: 0x00012656
		internal bool IsDefined(Type type, int fieldNumber)
		{
			return this.FindWithoutAdd(type).IsDefined(fieldNumber);
		}

		// Token: 0x060003D7 RID: 983 RVA: 0x00014468 File Offset: 0x00012668
		internal bool IsPrepared(Type type)
		{
			MetaType metaType = this.FindWithoutAdd(type);
			return metaType != null && metaType.IsPrepared();
		}

		// Token: 0x060003D8 RID: 984 RVA: 0x00014488 File Offset: 0x00012688
		internal EnumSerializer.EnumPair[] GetEnumMap(Type type)
		{
			int num = this.FindOrAddAuto(type, false, false, false);
			if (num >= 0)
			{
				return ((MetaType)this.types[num]).GetEnumMap();
			}
			return null;
		}

		// Token: 0x170000D8 RID: 216
		// (get) Token: 0x060003D9 RID: 985 RVA: 0x000144BC File Offset: 0x000126BC
		// (set) Token: 0x060003DA RID: 986 RVA: 0x000144C4 File Offset: 0x000126C4
		public int MetadataTimeoutMilliseconds
		{
			get
			{
				return this.metadataTimeoutMilliseconds;
			}
			set
			{
				if (value <= 0)
				{
					throw new ArgumentOutOfRangeException("MetadataTimeoutMilliseconds");
				}
				this.metadataTimeoutMilliseconds = value;
			}
		}

		// Token: 0x060003DB RID: 987 RVA: 0x000144DC File Offset: 0x000126DC
		internal void TakeLock(ref int opaqueToken)
		{
			opaqueToken = 0;
			if (Monitor.TryEnter(this.types, this.metadataTimeoutMilliseconds))
			{
				opaqueToken = this.GetContention();
				return;
			}
			this.AddContention();
			throw new TimeoutException("Timeout while inspecting metadata; this may indicate a deadlock. This can often be avoided by preparing necessary serializers during application initialization, rather than allowing multiple threads to perform the initial metadata inspection; please also see the LockContended event");
		}

		// Token: 0x060003DC RID: 988 RVA: 0x0001450D File Offset: 0x0001270D
		private int GetContention()
		{
			return Interlocked.CompareExchange(ref this.contentionCounter, 0, 0);
		}

		// Token: 0x060003DD RID: 989 RVA: 0x0001451C File Offset: 0x0001271C
		private void AddContention()
		{
			Interlocked.Increment(ref this.contentionCounter);
		}

		// Token: 0x060003DE RID: 990 RVA: 0x0001452C File Offset: 0x0001272C
		internal void ReleaseLock(int opaqueToken)
		{
			if (opaqueToken != 0)
			{
				Monitor.Exit(this.types);
				if (opaqueToken != this.GetContention())
				{
					LockContentedEventHandler lockContended = this.LockContended;
					if (lockContended != null)
					{
						string stackTrace;
						try
						{
							throw new Exception();
						}
						catch (Exception ex)
						{
							stackTrace = ex.StackTrace;
						}
						lockContended(this, new LockContentedEventArgs(stackTrace));
					}
				}
			}
		}

		// Token: 0x14000001 RID: 1
		// (add) Token: 0x060003DF RID: 991 RVA: 0x00014588 File Offset: 0x00012788
		// (remove) Token: 0x060003E0 RID: 992 RVA: 0x000145C0 File Offset: 0x000127C0
		public event LockContentedEventHandler LockContended;

		// Token: 0x060003E1 RID: 993 RVA: 0x000145F8 File Offset: 0x000127F8
		internal void ResolveListTypes(Type type, ref Type itemType, ref Type defaultType)
		{
			if (type == null)
			{
				return;
			}
			if (Helpers.GetTypeCode(type) != ProtoTypeCode.Unknown)
			{
				return;
			}
			if (this[type].IgnoreListHandling)
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
				if (itemType == base.MapType(typeof(byte)))
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
				itemType = TypeModel.GetListItemType(this, type);
			}
			if (itemType != null)
			{
				Type type3 = null;
				Type type4 = null;
				this.ResolveListTypes(itemType, ref type3, ref type4);
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
					if (type.IsGenericType && type.GetGenericTypeDefinition() == base.MapType(typeof(IDictionary<, >)) && itemType == base.MapType(typeof(KeyValuePair<, >)).MakeGenericType(genericArguments = type.GetGenericArguments()))
					{
						defaultType = base.MapType(typeof(Dictionary<, >)).MakeGenericType(genericArguments);
					}
					else
					{
						defaultType = base.MapType(typeof(List<>)).MakeGenericType(new Type[]
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

		// Token: 0x060003E2 RID: 994 RVA: 0x00014760 File Offset: 0x00012960
		internal string GetSchemaTypeName(Type effectiveType, DataFormat dataFormat, bool asReference, bool dynamicType, ref bool requiresBclImport)
		{
			Type underlyingType = Helpers.GetUnderlyingType(effectiveType);
			if (underlyingType != null)
			{
				effectiveType = underlyingType;
			}
			if (effectiveType == base.MapType(typeof(byte[])))
			{
				return "bytes";
			}
			WireType wireType;
			IProtoSerializer protoSerializer = ValueMember.TryGetCoreSerializer(this, dataFormat, effectiveType, out wireType, false, false, false, false);
			if (protoSerializer == null)
			{
				if (asReference || dynamicType)
				{
					requiresBclImport = true;
					return "bcl.NetObjectProxy";
				}
				return this[effectiveType].GetSurrogateOrBaseOrSelf().GetSchemaTypeName();
			}
			else
			{
				if (!(protoSerializer is ParseableSerializer))
				{
					ProtoTypeCode typeCode = Helpers.GetTypeCode(effectiveType);
					switch (typeCode)
					{
					case ProtoTypeCode.Boolean:
						return "bool";
					case ProtoTypeCode.Char:
					case ProtoTypeCode.Byte:
					case ProtoTypeCode.UInt16:
					case ProtoTypeCode.UInt32:
						if (dataFormat == DataFormat.FixedSize)
						{
							return "fixed32";
						}
						return "uint32";
					case ProtoTypeCode.SByte:
					case ProtoTypeCode.Int16:
					case ProtoTypeCode.Int32:
						if (dataFormat == DataFormat.ZigZag)
						{
							return "sint32";
						}
						if (dataFormat != DataFormat.FixedSize)
						{
							return "int32";
						}
						return "sfixed32";
					case ProtoTypeCode.Int64:
						if (dataFormat == DataFormat.ZigZag)
						{
							return "sint64";
						}
						if (dataFormat != DataFormat.FixedSize)
						{
							return "int64";
						}
						return "sfixed64";
					case ProtoTypeCode.UInt64:
						if (dataFormat == DataFormat.FixedSize)
						{
							return "fixed64";
						}
						return "uint64";
					case ProtoTypeCode.Single:
						return "float";
					case ProtoTypeCode.Double:
						return "double";
					case ProtoTypeCode.Decimal:
						requiresBclImport = true;
						return "bcl.Decimal";
					case ProtoTypeCode.DateTime:
						requiresBclImport = true;
						return "bcl.DateTime";
					case (ProtoTypeCode)17:
						break;
					case ProtoTypeCode.String:
						if (asReference)
						{
							requiresBclImport = true;
						}
						if (!asReference)
						{
							return "string";
						}
						return "bcl.NetObjectProxy";
					default:
						if (typeCode == ProtoTypeCode.TimeSpan)
						{
							requiresBclImport = true;
							return "bcl.TimeSpan";
						}
						if (typeCode == ProtoTypeCode.Guid)
						{
							requiresBclImport = true;
							return "bcl.Guid";
						}
						break;
					}
					throw new NotSupportedException("No .proto map found for: " + effectiveType.FullName);
				}
				if (asReference)
				{
					requiresBclImport = true;
				}
				if (!asReference)
				{
					return "string";
				}
				return "bcl.NetObjectProxy";
			}
		}

		// Token: 0x040002A9 RID: 681
		private const byte OPTIONS_InferTagFromNameDefault = 1;

		// Token: 0x040002AA RID: 682
		private const byte OPTIONS_IsDefaultModel = 2;

		// Token: 0x040002AB RID: 683
		private const byte OPTIONS_Frozen = 4;

		// Token: 0x040002AC RID: 684
		private const byte OPTIONS_AutoAddMissingTypes = 8;

		// Token: 0x040002AD RID: 685
		private const byte OPTIONS_UseImplicitZeroDefaults = 32;

		// Token: 0x040002AE RID: 686
		private const byte OPTIONS_AllowParseableTypes = 64;

		// Token: 0x040002AF RID: 687
		private const byte OPTIONS_AutoAddProtoContractTypesOnly = 128;

		// Token: 0x040002B0 RID: 688
		private byte options;

		// Token: 0x040002B1 RID: 689
		private readonly BasicList types = new BasicList();

		// Token: 0x040002B2 RID: 690
		private int metadataTimeoutMilliseconds = 5000;

		// Token: 0x040002B3 RID: 691
		private int contentionCounter = 1;

		// Token: 0x0200015D RID: 349
		private class Singleton
		{
			// Token: 0x0600115B RID: 4443 RVA: 0x000020C5 File Offset: 0x000002C5
			private Singleton()
			{
			}

			// Token: 0x04000E21 RID: 3617
			internal static readonly RuntimeTypeModel Value = new RuntimeTypeModel(true);
		}

		// Token: 0x0200015E RID: 350
		private sealed class TypeFinder : BasicList.IPredicate
		{
			// Token: 0x0600115D RID: 4445 RVA: 0x0007787B File Offset: 0x00075A7B
			public TypeFinder(Type type)
			{
				this.type = type;
			}

			// Token: 0x0600115E RID: 4446 RVA: 0x0007788A File Offset: 0x00075A8A
			public bool IsMatch(object obj)
			{
				return ((MetaType)obj).Type == this.type;
			}

			// Token: 0x04000E22 RID: 3618
			private readonly Type type;
		}
	}
}
