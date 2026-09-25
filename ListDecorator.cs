using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using ProtoBuf.Meta;

namespace ProtoBuf.Serializers
{
	// Token: 0x0200004D RID: 77
	internal sealed class ListDecorator : ProtoDecoratorBase
	{
		// Token: 0x06000257 RID: 599 RVA: 0x0000DE0B File Offset: 0x0000C00B
		static ListDecorator()
		{
			ColorSerializer.Enabled = false;
		}

		// Token: 0x06000258 RID: 600 RVA: 0x0000DE31 File Offset: 0x0000C031
		internal static bool CanPack(WireType wireType)
		{
			return wireType <= WireType.Fixed64 || wireType == WireType.Fixed32 || wireType == WireType.SignedVariant;
		}

		// Token: 0x17000065 RID: 101
		// (get) Token: 0x06000259 RID: 601 RVA: 0x0000DE42 File Offset: 0x0000C042
		private bool IsList
		{
			get
			{
				return (this.options & 1) > 0;
			}
		}

		// Token: 0x17000066 RID: 102
		// (get) Token: 0x0600025A RID: 602 RVA: 0x0000DE4F File Offset: 0x0000C04F
		private bool SuppressIList
		{
			get
			{
				return (this.options & 2) > 0;
			}
		}

		// Token: 0x17000067 RID: 103
		// (get) Token: 0x0600025B RID: 603 RVA: 0x0000DE5C File Offset: 0x0000C05C
		private bool WritePacked
		{
			get
			{
				return (this.options & 4) > 0;
			}
		}

		// Token: 0x17000068 RID: 104
		// (get) Token: 0x0600025C RID: 604 RVA: 0x0000DE69 File Offset: 0x0000C069
		private bool SupportNull
		{
			get
			{
				return (this.options & 32) > 0;
			}
		}

		// Token: 0x17000069 RID: 105
		// (get) Token: 0x0600025D RID: 605 RVA: 0x0000DE77 File Offset: 0x0000C077
		private bool ReturnList
		{
			get
			{
				return (this.options & 8) > 0;
			}
		}

		// Token: 0x0600025E RID: 606 RVA: 0x0000DE84 File Offset: 0x0000C084
		public ListDecorator(TypeModel model, Type declaredType, Type concreteType, IProtoSerializer tail, int fieldNumber, bool writePacked, WireType packedWireType, bool returnList, bool overwriteList, bool supportNull) : base(tail)
		{
			if (returnList)
			{
				this.options |= 8;
			}
			if (overwriteList)
			{
				this.options |= 16;
			}
			if (supportNull)
			{
				this.options |= 32;
			}
			if ((writePacked || packedWireType != WireType.None) && fieldNumber <= 0)
			{
				throw new ArgumentOutOfRangeException("fieldNumber");
			}
			if (!ListDecorator.CanPack(packedWireType))
			{
				if (writePacked)
				{
					throw new InvalidOperationException("Only simple data-types can use packed encoding");
				}
				packedWireType = WireType.None;
			}
			this.fieldNumber = fieldNumber;
			if (writePacked)
			{
				this.options |= 4;
			}
			this.packedWireType = packedWireType;
			if (declaredType == null)
			{
				throw new ArgumentNullException("declaredType");
			}
			if (declaredType.IsArray)
			{
				throw new ArgumentException("Cannot treat arrays as lists", "declaredType");
			}
			this.declaredType = declaredType;
			this.concreteType = concreteType;
			bool flag;
			this.add = TypeModel.ResolveListAdd(model, declaredType, tail.ExpectedType, out flag);
			if (flag)
			{
				this.options |= 1;
				if (declaredType.FullName.StartsWith("System.Data.Linq.EntitySet`1[["))
				{
					this.options |= 2;
				}
			}
			if (this.add == null)
			{
				throw new InvalidOperationException("Unable to resolve a suitable Add method for " + declaredType.FullName);
			}
		}

		// Token: 0x1700006A RID: 106
		// (get) Token: 0x0600025F RID: 607 RVA: 0x0000DFC6 File Offset: 0x0000C1C6
		public override Type ExpectedType
		{
			get
			{
				return this.declaredType;
			}
		}

		// Token: 0x1700006B RID: 107
		// (get) Token: 0x06000260 RID: 608 RVA: 0x0000DFCE File Offset: 0x0000C1CE
		public override bool RequiresOldValue
		{
			get
			{
				return this.AppendToCollection;
			}
		}

		// Token: 0x1700006C RID: 108
		// (get) Token: 0x06000261 RID: 609 RVA: 0x0000DFD6 File Offset: 0x0000C1D6
		public override bool ReturnsValue
		{
			get
			{
				return this.ReturnList;
			}
		}

		// Token: 0x1700006D RID: 109
		// (get) Token: 0x06000262 RID: 610 RVA: 0x0000DFDE File Offset: 0x0000C1DE
		private bool AppendToCollection
		{
			get
			{
				return (this.options & 16) == 0;
			}
		}

		// Token: 0x06000263 RID: 611 RVA: 0x0000DFEC File Offset: 0x0000C1EC
		private MethodInfo GetEnumeratorInfo(TypeModel model, out MethodInfo moveNext, out MethodInfo current)
		{
			Type type = null;
			Type expectedType = this.ExpectedType;
			MethodInfo instanceMethod = Helpers.GetInstanceMethod(expectedType, "GetEnumerator", null);
			Type expectedType2 = this.Tail.ExpectedType;
			Type returnType;
			if (instanceMethod != null)
			{
				returnType = instanceMethod.ReturnType;
				moveNext = Helpers.GetInstanceMethod(returnType, "MoveNext", null);
				PropertyInfo property = Helpers.GetProperty(returnType, "Current");
				current = ((property == null) ? null : Helpers.GetGetMethod(property, false));
				if (moveNext == null && model.MapType(ListDecorator.ienumeratorType).IsAssignableFrom(returnType))
				{
					moveNext = Helpers.GetInstanceMethod(model.MapType(ListDecorator.ienumeratorType), "MoveNext", null);
				}
				if (moveNext != null && moveNext.ReturnType == model.MapType(typeof(bool)) && current != null && current.ReturnType == expectedType2)
				{
					return instanceMethod;
				}
				MethodInfo methodInfo;
				current = (methodInfo = null);
				moveNext = methodInfo;
			}
			Type type2 = model.MapType(typeof(IEnumerable<>), false);
			if (type2 != null)
			{
				type2 = type2.MakeGenericType(new Type[]
				{
					expectedType2
				});
				type = type2;
			}
			if (type != null && type.IsAssignableFrom(expectedType))
			{
				instanceMethod = Helpers.GetInstanceMethod(type, "GetEnumerator");
				returnType = instanceMethod.ReturnType;
				moveNext = Helpers.GetInstanceMethod(model.MapType(ListDecorator.ienumeratorType), "MoveNext");
				current = Helpers.GetGetMethod(Helpers.GetProperty(returnType, "Current"), false);
				return instanceMethod;
			}
			type = model.MapType(ListDecorator.ienumerableType);
			instanceMethod = Helpers.GetInstanceMethod(type, "GetEnumerator");
			returnType = instanceMethod.ReturnType;
			moveNext = Helpers.GetInstanceMethod(returnType, "MoveNext");
			current = Helpers.GetGetMethod(Helpers.GetProperty(returnType, "Current"), false);
			return instanceMethod;
		}

		// Token: 0x06000264 RID: 612 RVA: 0x0000E17C File Offset: 0x0000C37C
		public override void Write(object value, ProtoWriter dest)
		{
			bool writePacked = this.WritePacked;
			SubItemToken token;
			if (writePacked)
			{
				ProtoWriter.WriteFieldHeader(this.fieldNumber, WireType.String, dest);
				token = ProtoWriter.StartSubItem(value, dest);
				ProtoWriter.SetPackedField(this.fieldNumber, dest);
			}
			else
			{
				token = default(SubItemToken);
			}
			bool flag = !this.SupportNull;
			foreach (object obj in ((IEnumerable)value))
			{
				if (flag && obj == null)
				{
					throw new NullReferenceException();
				}
				this.Tail.Write(obj, dest);
			}
			if (writePacked)
			{
				ProtoWriter.EndSubItem(token, dest);
			}
		}

		// Token: 0x06000265 RID: 613 RVA: 0x0000E234 File Offset: 0x0000C434
		public override object Read(object value, ProtoReader source)
		{
			int field = source.FieldNumber;
			object obj = value;
			if (value == null)
			{
				value = Activator.CreateInstance(this.concreteType);
			}
			bool flag = this.IsList && !this.SuppressIList;
			if (this.packedWireType != WireType.None && source.WireType == WireType.String)
			{
				SubItemToken token = ProtoReader.StartSubItem(source);
				if (flag)
				{
					IList list = (IList)value;
					while (ProtoReader.HasSubValue(this.packedWireType, source))
					{
						list.Add(this.Tail.Read(null, source));
					}
				}
				else
				{
					object[] array = new object[1];
					while (ProtoReader.HasSubValue(this.packedWireType, source))
					{
						array[0] = this.Tail.Read(null, source);
						this.add.Invoke(value, array);
					}
				}
				ProtoReader.EndSubItem(token, source);
			}
			else if (flag)
			{
				IList list2 = (IList)value;
				do
				{
					list2.Add(this.Tail.Read(null, source));
				}
				while (source.TryReadFieldHeader(field));
			}
			else
			{
				object[] array2 = new object[1];
				do
				{
					array2[0] = this.Tail.Read(null, source);
					this.add.Invoke(value, array2);
				}
				while (source.TryReadFieldHeader(field));
			}
			if (obj != value)
			{
				return value;
			}
			return null;
		}

		// Token: 0x0400022D RID: 557
		private const byte OPTIONS_IsList = 1;

		// Token: 0x0400022E RID: 558
		private const byte OPTIONS_SuppressIList = 2;

		// Token: 0x0400022F RID: 559
		private const byte OPTIONS_WritePacked = 4;

		// Token: 0x04000230 RID: 560
		private const byte OPTIONS_ReturnList = 8;

		// Token: 0x04000231 RID: 561
		private const byte OPTIONS_OverwriteList = 16;

		// Token: 0x04000232 RID: 562
		private const byte OPTIONS_SupportNull = 32;

		// Token: 0x04000233 RID: 563
		private readonly byte options;

		// Token: 0x04000234 RID: 564
		private readonly Type declaredType;

		// Token: 0x04000235 RID: 565
		private readonly Type concreteType;

		// Token: 0x04000236 RID: 566
		private readonly MethodInfo add;

		// Token: 0x04000237 RID: 567
		private readonly int fieldNumber;

		// Token: 0x04000238 RID: 568
		private readonly WireType packedWireType;

		// Token: 0x04000239 RID: 569
		private static readonly Type ienumeratorType = typeof(IEnumerator);

		// Token: 0x0400023A RID: 570
		private static readonly Type ienumerableType = typeof(IEnumerable);
	}
}
