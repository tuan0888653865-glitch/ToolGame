using System;

namespace ProtoBuf.Serializers
{
	// Token: 0x02000044 RID: 68
	internal sealed class EnumSerializer : IProtoSerializer
	{
		// Token: 0x06000224 RID: 548 RVA: 0x0000D958 File Offset: 0x0000BB58
		public EnumSerializer(Type enumType, EnumSerializer.EnumPair[] map)
		{
			if (enumType == null)
			{
				throw new ArgumentNullException("enumType");
			}
			this.enumType = enumType;
			this.map = map;
			if (map != null)
			{
				for (int i = 1; i < map.Length; i++)
				{
					for (int j = 0; j < i; j++)
					{
						if (map[i].WireValue == map[j].WireValue && !object.Equals(map[i].RawValue, map[j].RawValue))
						{
							throw new ProtoException("Multiple enums with wire-value " + map[i].WireValue.ToString());
						}
						if (object.Equals(map[i].RawValue, map[j].RawValue) && map[i].WireValue != map[j].WireValue)
						{
							string str = "Multiple enums with deserialized-value ";
							object rawValue = map[i].RawValue;
							throw new ProtoException(str + ((rawValue != null) ? rawValue.ToString() : null));
						}
					}
				}
			}
		}

		// Token: 0x06000225 RID: 549 RVA: 0x0000DA74 File Offset: 0x0000BC74
		private ProtoTypeCode GetTypeCode()
		{
			Type underlyingType = Helpers.GetUnderlyingType(this.enumType);
			if (underlyingType == null)
			{
				underlyingType = this.enumType;
			}
			return Helpers.GetTypeCode(underlyingType);
		}

		// Token: 0x1700004F RID: 79
		// (get) Token: 0x06000226 RID: 550 RVA: 0x0000DA9D File Offset: 0x0000BC9D
		public Type ExpectedType
		{
			get
			{
				return this.enumType;
			}
		}

		// Token: 0x17000050 RID: 80
		// (get) Token: 0x06000227 RID: 551 RVA: 0x00008C9D File Offset: 0x00006E9D
		bool IProtoSerializer.RequiresOldValue
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000051 RID: 81
		// (get) Token: 0x06000228 RID: 552 RVA: 0x0000D470 File Offset: 0x0000B670
		bool IProtoSerializer.ReturnsValue
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06000229 RID: 553 RVA: 0x0000DAA8 File Offset: 0x0000BCA8
		private int EnumToWire(object value)
		{
			switch (this.GetTypeCode())
			{
			case ProtoTypeCode.SByte:
				return (int)((sbyte)value);
			case ProtoTypeCode.Byte:
				return (int)((byte)value);
			case ProtoTypeCode.Int16:
				return (int)((short)value);
			case ProtoTypeCode.UInt16:
				return (int)((ushort)value);
			case ProtoTypeCode.Int32:
				return (int)value;
			case ProtoTypeCode.UInt32:
				return (int)((uint)value);
			case ProtoTypeCode.Int64:
				return (int)((long)value);
			case ProtoTypeCode.UInt64:
				return (int)((ulong)value);
			default:
				throw new InvalidOperationException();
			}
		}

		// Token: 0x0600022A RID: 554 RVA: 0x0000DB28 File Offset: 0x0000BD28
		private object WireToEnum(int value)
		{
			switch (this.GetTypeCode())
			{
			case ProtoTypeCode.SByte:
				return Enum.ToObject(this.enumType, (sbyte)value);
			case ProtoTypeCode.Byte:
				return Enum.ToObject(this.enumType, (byte)value);
			case ProtoTypeCode.Int16:
				return Enum.ToObject(this.enumType, (short)value);
			case ProtoTypeCode.UInt16:
				return Enum.ToObject(this.enumType, (ushort)value);
			case ProtoTypeCode.Int32:
				return Enum.ToObject(this.enumType, value);
			case ProtoTypeCode.UInt32:
				return Enum.ToObject(this.enumType, (uint)value);
			case ProtoTypeCode.Int64:
				return Enum.ToObject(this.enumType, (long)value);
			case ProtoTypeCode.UInt64:
				return Enum.ToObject(this.enumType, (ulong)((long)value));
			default:
				throw new InvalidOperationException();
			}
		}

		// Token: 0x0600022B RID: 555 RVA: 0x0000DBDC File Offset: 0x0000BDDC
		public object Read(object value, ProtoReader source)
		{
			int num = source.ReadInt32();
			if (this.map == null)
			{
				return this.WireToEnum(num);
			}
			for (int i = 0; i < this.map.Length; i++)
			{
				if (this.map[i].WireValue == num)
				{
					return this.map[i].TypedValue;
				}
			}
			source.ThrowEnumException(this.ExpectedType, num);
			return null;
		}

		// Token: 0x0600022C RID: 556 RVA: 0x0000DC48 File Offset: 0x0000BE48
		public void Write(object value, ProtoWriter dest)
		{
			if (this.map == null)
			{
				ProtoWriter.WriteInt32(this.EnumToWire(value), dest);
				return;
			}
			for (int i = 0; i < this.map.Length; i++)
			{
				if (object.Equals(this.map[i].TypedValue, value))
				{
					ProtoWriter.WriteInt32(this.map[i].WireValue, dest);
					return;
				}
			}
			ProtoWriter.ThrowEnumException(dest, value);
		}

		// Token: 0x04000225 RID: 549
		private readonly Type enumType;

		// Token: 0x04000226 RID: 550
		private readonly EnumSerializer.EnumPair[] map;

		// Token: 0x02000155 RID: 341
		public struct EnumPair
		{
			// Token: 0x06001141 RID: 4417 RVA: 0x000774E8 File Offset: 0x000756E8
			public EnumPair(int wireValue, object raw, Type type)
			{
				this.WireValue = wireValue;
				this.RawValue = raw;
				this.TypedValue = (Enum)Enum.ToObject(type, raw);
			}

			// Token: 0x04000E10 RID: 3600
			public readonly object RawValue;

			// Token: 0x04000E11 RID: 3601
			public readonly Enum TypedValue;

			// Token: 0x04000E12 RID: 3602
			public readonly int WireValue;
		}
	}
}
