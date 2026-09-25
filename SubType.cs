using System;
using System.Collections;
using System.Collections.Generic;
using ProtoBuf.Serializers;

namespace ProtoBuf.Meta
{
	// Token: 0x0200006F RID: 111
	public sealed class SubType
	{
		// Token: 0x170000D9 RID: 217
		// (get) Token: 0x060003E3 RID: 995 RVA: 0x00014905 File Offset: 0x00012B05
		public int FieldNumber
		{
			get
			{
				return this.fieldNumber;
			}
		}

		// Token: 0x170000DA RID: 218
		// (get) Token: 0x060003E4 RID: 996 RVA: 0x0001490D File Offset: 0x00012B0D
		public MetaType DerivedType
		{
			get
			{
				return this.derivedType;
			}
		}

		// Token: 0x060003E5 RID: 997 RVA: 0x00014915 File Offset: 0x00012B15
		public SubType(int fieldNumber, MetaType derivedType, DataFormat format)
		{
			if (derivedType == null)
			{
				throw new ArgumentNullException("derivedType");
			}
			if (fieldNumber <= 0)
			{
				throw new ArgumentOutOfRangeException("fieldNumber");
			}
			this.fieldNumber = fieldNumber;
			this.derivedType = derivedType;
			this.dataFormat = format;
		}

		// Token: 0x170000DB RID: 219
		// (get) Token: 0x060003E6 RID: 998 RVA: 0x0001494F File Offset: 0x00012B4F
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

		// Token: 0x060003E7 RID: 999 RVA: 0x0001496C File Offset: 0x00012B6C
		private IProtoSerializer BuildSerializer()
		{
			WireType wireType = WireType.String;
			if (this.dataFormat == DataFormat.Group)
			{
				wireType = WireType.StartGroup;
			}
			IProtoSerializer tail = new SubItemSerializer(this.derivedType.Type, this.derivedType.GetKey(false, false), this.derivedType, false);
			return new TagDecorator(this.fieldNumber, wireType, false, tail);
		}

		// Token: 0x040002B4 RID: 692
		private readonly int fieldNumber;

		// Token: 0x040002B5 RID: 693
		private readonly MetaType derivedType;

		// Token: 0x040002B6 RID: 694
		private readonly DataFormat dataFormat;

		// Token: 0x040002B7 RID: 695
		private IProtoSerializer serializer;

		// Token: 0x0200015F RID: 351
		internal class Comparer : IComparer, IComparer<SubType>
		{
			// Token: 0x0600115F RID: 4447 RVA: 0x0007789F File Offset: 0x00075A9F
			public int Compare(object x, object y)
			{
				return this.Compare(x as SubType, y as SubType);
			}

			// Token: 0x06001160 RID: 4448 RVA: 0x000778B4 File Offset: 0x00075AB4
			public int Compare(SubType x, SubType y)
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

			// Token: 0x04000E23 RID: 3619
			public static readonly SubType.Comparer Default = new SubType.Comparer();
		}
	}
}
