using System;
using System.Collections;
using ProtoBuf.Meta;

namespace ProtoBuf.Serializers
{
	// Token: 0x02000039 RID: 57
	internal sealed class ArrayDecorator : ProtoDecoratorBase
	{
		// Token: 0x060001DE RID: 478 RVA: 0x0000D3AC File Offset: 0x0000B5AC
		public ArrayDecorator(TypeModel model, IProtoSerializer tail, int fieldNumber, bool writePacked, WireType packedWireType, Type arrayType, bool overwriteList, bool supportNull) : base(tail)
		{
			this.itemType = arrayType.GetElementType();
			if (!supportNull)
			{
				Helpers.GetUnderlyingType(this.itemType);
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
			this.packedWireType = packedWireType;
			if (writePacked)
			{
				this.options |= 1;
			}
			if (overwriteList)
			{
				this.options |= 2;
			}
			if (supportNull)
			{
				this.options |= 4;
			}
			this.arrayType = arrayType;
		}

		// Token: 0x17000034 RID: 52
		// (get) Token: 0x060001DF RID: 479 RVA: 0x0000D460 File Offset: 0x0000B660
		public override Type ExpectedType
		{
			get
			{
				return this.arrayType;
			}
		}

		// Token: 0x17000035 RID: 53
		// (get) Token: 0x060001E0 RID: 480 RVA: 0x0000D468 File Offset: 0x0000B668
		public override bool RequiresOldValue
		{
			get
			{
				return this.AppendToCollection;
			}
		}

		// Token: 0x17000036 RID: 54
		// (get) Token: 0x060001E1 RID: 481 RVA: 0x0000D470 File Offset: 0x0000B670
		public override bool ReturnsValue
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000037 RID: 55
		// (get) Token: 0x060001E2 RID: 482 RVA: 0x0000D473 File Offset: 0x0000B673
		private bool AppendToCollection
		{
			get
			{
				return (this.options & 2) == 0;
			}
		}

		// Token: 0x17000038 RID: 56
		// (get) Token: 0x060001E3 RID: 483 RVA: 0x0000D480 File Offset: 0x0000B680
		private bool SupportNull
		{
			get
			{
				return (this.options & 4) > 0;
			}
		}

		// Token: 0x060001E4 RID: 484 RVA: 0x0000D490 File Offset: 0x0000B690
		public override void Write(object value, ProtoWriter dest)
		{
			IList list = (IList)value;
			int count = list.Count;
			bool flag = (this.options & 1) > 0;
			SubItemToken token;
			if (flag)
			{
				ProtoWriter.WriteFieldHeader(this.fieldNumber, WireType.String, dest);
				token = ProtoWriter.StartSubItem(value, dest);
				ProtoWriter.SetPackedField(this.fieldNumber, dest);
			}
			else
			{
				token = default(SubItemToken);
			}
			bool flag2 = !this.SupportNull;
			for (int i = 0; i < count; i++)
			{
				object obj = list[i];
				if (flag2 && obj == null)
				{
					throw new NullReferenceException();
				}
				this.Tail.Write(obj, dest);
			}
			if (flag)
			{
				ProtoWriter.EndSubItem(token, dest);
			}
		}

		// Token: 0x060001E5 RID: 485 RVA: 0x0000D530 File Offset: 0x0000B730
		public override object Read(object value, ProtoReader source)
		{
			int field = source.FieldNumber;
			BasicList basicList = new BasicList();
			if (this.packedWireType != WireType.None && source.WireType == WireType.String)
			{
				SubItemToken token = ProtoReader.StartSubItem(source);
				while (ProtoReader.HasSubValue(this.packedWireType, source))
				{
					basicList.Add(this.Tail.Read(null, source));
				}
				ProtoReader.EndSubItem(token, source);
			}
			else
			{
				do
				{
					basicList.Add(this.Tail.Read(null, source));
				}
				while (source.TryReadFieldHeader(field));
			}
			int num = this.AppendToCollection ? ((value == null) ? 0 : ((Array)value).Length) : 0;
			Array array = Array.CreateInstance(this.itemType, num + basicList.Count);
			if (num != 0)
			{
				((Array)value).CopyTo(array, 0);
			}
			basicList.CopyTo(array, num);
			return array;
		}

		// Token: 0x04000211 RID: 529
		private const byte OPTIONS_WritePacked = 1;

		// Token: 0x04000212 RID: 530
		private const byte OPTIONS_OverwriteList = 2;

		// Token: 0x04000213 RID: 531
		private const byte OPTIONS_SupportNull = 4;

		// Token: 0x04000214 RID: 532
		private readonly int fieldNumber;

		// Token: 0x04000215 RID: 533
		private readonly byte options;

		// Token: 0x04000216 RID: 534
		private readonly WireType packedWireType;

		// Token: 0x04000217 RID: 535
		private readonly Type arrayType;

		// Token: 0x04000218 RID: 536
		private readonly Type itemType;
	}
}
