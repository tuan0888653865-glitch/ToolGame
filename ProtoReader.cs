using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using ProtoBuf.Meta;

namespace ProtoBuf
{
	// Token: 0x02000030 RID: 48
	public sealed class ProtoReader : IDisposable
	{
		// Token: 0x17000027 RID: 39
		// (get) Token: 0x0600014B RID: 331 RVA: 0x0000A6B3 File Offset: 0x000088B3
		public int FieldNumber
		{
			get
			{
				return this.fieldNumber;
			}
		}

		// Token: 0x17000028 RID: 40
		// (get) Token: 0x0600014C RID: 332 RVA: 0x0000A6BB File Offset: 0x000088BB
		public WireType WireType
		{
			get
			{
				return this.wireType;
			}
		}

		// Token: 0x0600014D RID: 333 RVA: 0x0000A6C3 File Offset: 0x000088C3
		public ProtoReader(Stream source, TypeModel model, SerializationContext context) : this(source, model, context, -1)
		{
		}

		// Token: 0x17000029 RID: 41
		// (get) Token: 0x0600014E RID: 334 RVA: 0x0000A6CF File Offset: 0x000088CF
		// (set) Token: 0x0600014F RID: 335 RVA: 0x0000A6D7 File Offset: 0x000088D7
		public bool InternStrings
		{
			get
			{
				return this.internStrings;
			}
			set
			{
				this.internStrings = value;
			}
		}

		// Token: 0x06000150 RID: 336 RVA: 0x0000A6E0 File Offset: 0x000088E0
		public ProtoReader(Stream source, TypeModel model, SerializationContext context, int length)
		{
			if (source == null)
			{
				throw new ArgumentNullException("source");
			}
			if (!source.CanRead)
			{
				throw new ArgumentException("Cannot read from stream", "source");
			}
			this.source = source;
			this.ioBuffer = BufferPool.GetBuffer();
			this.model = model;
			this.isFixedLength = (length >= 0);
			this.dataRemaining = (this.isFixedLength ? length : 0);
			if (context == null)
			{
				context = SerializationContext.Default;
			}
			else
			{
				context.Freeze();
			}
			this.context = context;
		}

		// Token: 0x1700002A RID: 42
		// (get) Token: 0x06000151 RID: 337 RVA: 0x0000A797 File Offset: 0x00008997
		public SerializationContext Context
		{
			get
			{
				return this.context;
			}
		}

		// Token: 0x06000152 RID: 338 RVA: 0x0000A79F File Offset: 0x0000899F
		public void Dispose()
		{
			this.source = null;
			this.model = null;
			BufferPool.ReleaseBufferToPool(ref this.ioBuffer);
		}

		// Token: 0x06000153 RID: 339 RVA: 0x0000A7BC File Offset: 0x000089BC
		internal int TryReadUInt32VariantWithoutMoving(bool trimNegative, out uint value)
		{
			if (this.available < 10)
			{
				this.Ensure(10, false);
			}
			if (this.available == 0)
			{
				value = 0U;
				return 0;
			}
			int num = this.ioIndex;
			value = (uint)this.ioBuffer[num++];
			if ((value & 128U) == 0U)
			{
				return 1;
			}
			value &= 127U;
			if (this.available == 1)
			{
				throw ProtoReader.EoF(this);
			}
			uint num2 = (uint)this.ioBuffer[num++];
			value |= (num2 & 127U) << 7;
			if ((num2 & 128U) == 0U)
			{
				return 2;
			}
			if (this.available == 2)
			{
				throw ProtoReader.EoF(this);
			}
			num2 = (uint)this.ioBuffer[num++];
			value |= (num2 & 127U) << 14;
			if ((num2 & 128U) == 0U)
			{
				return 3;
			}
			if (this.available == 3)
			{
				throw ProtoReader.EoF(this);
			}
			num2 = (uint)this.ioBuffer[num++];
			value |= (num2 & 127U) << 21;
			if ((num2 & 128U) == 0U)
			{
				return 4;
			}
			if (this.available == 4)
			{
				throw ProtoReader.EoF(this);
			}
			num2 = (uint)this.ioBuffer[num];
			value |= num2 << 28;
			if ((num2 & 240U) == 0U)
			{
				return 5;
			}
			if (trimNegative && (num2 & 240U) == 240U && this.available >= 10 && this.ioBuffer[++num] == 255 && this.ioBuffer[++num] == 255 && this.ioBuffer[++num] == 255 && this.ioBuffer[++num] == 255 && this.ioBuffer[num + 1] == 1)
			{
				return 10;
			}
			throw ProtoReader.AddErrorData(new OverflowException(), this);
		}

		// Token: 0x06000154 RID: 340 RVA: 0x0000A95C File Offset: 0x00008B5C
		private uint ReadUInt32Variant(bool trimNegative)
		{
			uint result;
			int num = this.TryReadUInt32VariantWithoutMoving(trimNegative, out result);
			if (num > 0)
			{
				this.ioIndex += num;
				this.available -= num;
				this.position += num;
				return result;
			}
			throw ProtoReader.EoF(this);
		}

		// Token: 0x06000155 RID: 341 RVA: 0x0000A9AC File Offset: 0x00008BAC
		private bool TryReadUInt32Variant(out uint value)
		{
			int num = this.TryReadUInt32VariantWithoutMoving(false, out value);
			if (num > 0)
			{
				this.ioIndex += num;
				this.available -= num;
				this.position += num;
				return true;
			}
			return false;
		}

		// Token: 0x06000156 RID: 342 RVA: 0x0000A9F4 File Offset: 0x00008BF4
		public uint ReadUInt32()
		{
			WireType wireType = this.wireType;
			if (wireType == WireType.Variant)
			{
				return this.ReadUInt32Variant(false);
			}
			if (wireType == WireType.Fixed64)
			{
				return checked((uint)this.ReadUInt64());
			}
			if (wireType != WireType.Fixed32)
			{
				throw this.CreateWireTypeException();
			}
			if (this.available < 4)
			{
				this.Ensure(4, true);
			}
			this.position += 4;
			this.available -= 4;
			byte[] array = this.ioBuffer;
			int num = this.ioIndex;
			this.ioIndex = num + 1;
			uint num2 = array[num];
			byte[] array2 = this.ioBuffer;
			num = this.ioIndex;
			this.ioIndex = num + 1;
			uint num3 = num2 | array2[num] << 8;
			byte[] array3 = this.ioBuffer;
			num = this.ioIndex;
			this.ioIndex = num + 1;
			uint num4 = num3 | array3[num] << 16;
			byte[] array4 = this.ioBuffer;
			num = this.ioIndex;
			this.ioIndex = num + 1;
			return num4 | array4[num] << 24;
		}

		// Token: 0x1700002B RID: 43
		// (get) Token: 0x06000157 RID: 343 RVA: 0x0000AAC4 File Offset: 0x00008CC4
		public int Position
		{
			get
			{
				return this.position;
			}
		}

		// Token: 0x06000158 RID: 344 RVA: 0x0000AACC File Offset: 0x00008CCC
		internal void Ensure(int count, bool strict)
		{
			if (count > this.ioBuffer.Length)
			{
				BufferPool.ResizeAndFlushLeft(ref this.ioBuffer, count, this.ioIndex, this.available);
				this.ioIndex = 0;
			}
			else if (this.ioIndex + count >= this.ioBuffer.Length)
			{
				Helpers.BlockCopy(this.ioBuffer, this.ioIndex, this.ioBuffer, 0, this.available);
				this.ioIndex = 0;
			}
			count -= this.available;
			int num = this.ioIndex + this.available;
			int num2 = this.ioBuffer.Length - num;
			if (this.isFixedLength && this.dataRemaining < num2)
			{
				num2 = this.dataRemaining;
			}
			int num3;
			while (count > 0 && num2 > 0 && (num3 = this.source.Read(this.ioBuffer, num, num2)) > 0)
			{
				this.available += num3;
				count -= num3;
				num2 -= num3;
				num += num3;
				if (this.isFixedLength)
				{
					this.dataRemaining -= num3;
				}
			}
			if (strict && count > 0)
			{
				throw ProtoReader.EoF(this);
			}
		}

		// Token: 0x06000159 RID: 345 RVA: 0x0000ABD8 File Offset: 0x00008DD8
		public short ReadInt16()
		{
			return checked((short)this.ReadInt32());
		}

		// Token: 0x0600015A RID: 346 RVA: 0x0000ABE1 File Offset: 0x00008DE1
		public ushort ReadUInt16()
		{
			return checked((ushort)this.ReadUInt32());
		}

		// Token: 0x0600015B RID: 347 RVA: 0x0000ABEA File Offset: 0x00008DEA
		public byte ReadByte()
		{
			return checked((byte)this.ReadUInt32());
		}

		// Token: 0x0600015C RID: 348 RVA: 0x0000ABF3 File Offset: 0x00008DF3
		public sbyte ReadSByte()
		{
			return checked((sbyte)this.ReadInt32());
		}

		// Token: 0x0600015D RID: 349 RVA: 0x0000ABFC File Offset: 0x00008DFC
		public int ReadInt32()
		{
			WireType wireType = this.wireType;
			if (wireType == WireType.Variant)
			{
				return (int)this.ReadUInt32Variant(true);
			}
			if (wireType == WireType.Fixed64)
			{
				return checked((int)this.ReadInt64());
			}
			if (wireType == WireType.Fixed32)
			{
				if (this.available < 4)
				{
					this.Ensure(4, true);
				}
				this.position += 4;
				this.available -= 4;
				byte[] array = this.ioBuffer;
				int num = this.ioIndex;
				this.ioIndex = num + 1;
				int num2 = array[num];
				byte[] array2 = this.ioBuffer;
				num = this.ioIndex;
				this.ioIndex = num + 1;
				int num3 = num2 | array2[num] << 8;
				byte[] array3 = this.ioBuffer;
				num = this.ioIndex;
				this.ioIndex = num + 1;
				int num4 = num3 | array3[num] << 16;
				byte[] array4 = this.ioBuffer;
				num = this.ioIndex;
				this.ioIndex = num + 1;
				return num4 | array4[num] << 24;
			}
			if (wireType != WireType.SignedVariant)
			{
				throw this.CreateWireTypeException();
			}
			return ProtoReader.Zag(this.ReadUInt32Variant(true));
		}

		// Token: 0x0600015E RID: 350 RVA: 0x0000ACE0 File Offset: 0x00008EE0
		private static int Zag(uint ziggedValue)
		{
			return (int)(-(ulong)(ziggedValue & 1U) ^ (ulong)((int)ziggedValue >> 1 & int.MaxValue));
		}

		// Token: 0x0600015F RID: 351 RVA: 0x0000ACF3 File Offset: 0x00008EF3
		private static long Zag(ulong ziggedValue)
		{
			return (long)((ziggedValue & 1UL) ^ (ziggedValue >> 1 & 9223372036854775807UL));
		}

		// Token: 0x06000160 RID: 352 RVA: 0x0000AD08 File Offset: 0x00008F08
		public long ReadInt64()
		{
			WireType wireType = this.wireType;
			if (wireType == WireType.Variant)
			{
				return (long)this.ReadUInt64Variant();
			}
			if (wireType == WireType.Fixed64)
			{
				if (this.available < 8)
				{
					this.Ensure(8, true);
				}
				this.position += 8;
				this.available -= 8;
				byte[] array = this.ioBuffer;
				int num = this.ioIndex;
				this.ioIndex = num + 1;
				long num2 = (long)array[num];
				byte[] array2 = this.ioBuffer;
				num = this.ioIndex;
				this.ioIndex = num + 1;
				long num3 = num2 | (long)((long)array2[num] << 8);
				byte[] array3 = this.ioBuffer;
				num = this.ioIndex;
				this.ioIndex = num + 1;
				long num4 = num3 | (long)((long)array3[num] << 16);
				byte[] array4 = this.ioBuffer;
				num = this.ioIndex;
				this.ioIndex = num + 1;
				long num5 = num4 | (long)((long)array4[num] << 24);
				byte[] array5 = this.ioBuffer;
				num = this.ioIndex;
				this.ioIndex = num + 1;
				long num6 = num5 | (long)((long)array5[num] << 32);
				byte[] array6 = this.ioBuffer;
				num = this.ioIndex;
				this.ioIndex = num + 1;
				long num7 = num6 | (long)((long)array6[num] << 40);
				byte[] array7 = this.ioBuffer;
				num = this.ioIndex;
				this.ioIndex = num + 1;
				long num8 = num7 | (long)((long)array7[num] << 48);
				byte[] array8 = this.ioBuffer;
				num = this.ioIndex;
				this.ioIndex = num + 1;
				return num8 | (long)((long)array8[num] << 56);
			}
			if (wireType == WireType.Fixed32)
			{
				return (long)this.ReadInt32();
			}
			if (wireType != WireType.SignedVariant)
			{
				throw this.CreateWireTypeException();
			}
			return ProtoReader.Zag(this.ReadUInt64Variant());
		}

		// Token: 0x06000161 RID: 353 RVA: 0x0000AE64 File Offset: 0x00009064
		private int TryReadUInt64VariantWithoutMoving(out ulong value)
		{
			if (this.available < 10)
			{
				this.Ensure(10, false);
			}
			if (this.available == 0)
			{
				value = 0UL;
				return 0;
			}
			int num = this.ioIndex;
			value = (ulong)this.ioBuffer[num++];
			if ((value & 128UL) == 0UL)
			{
				return 1;
			}
			value &= 127UL;
			if (this.available == 1)
			{
				throw ProtoReader.EoF(this);
			}
			ulong num2 = (ulong)this.ioBuffer[num++];
			value |= (num2 & 127UL) << 7;
			if ((num2 & 128UL) == 0UL)
			{
				return 2;
			}
			if (this.available == 2)
			{
				throw ProtoReader.EoF(this);
			}
			num2 = (ulong)this.ioBuffer[num++];
			value |= (num2 & 127UL) << 14;
			if ((num2 & 128UL) == 0UL)
			{
				return 3;
			}
			if (this.available == 3)
			{
				throw ProtoReader.EoF(this);
			}
			num2 = (ulong)this.ioBuffer[num++];
			value |= (num2 & 127UL) << 21;
			if ((num2 & 128UL) == 0UL)
			{
				return 4;
			}
			if (this.available == 4)
			{
				throw ProtoReader.EoF(this);
			}
			num2 = (ulong)this.ioBuffer[num++];
			value |= (num2 & 127UL) << 28;
			if ((num2 & 128UL) == 0UL)
			{
				return 5;
			}
			if (this.available == 5)
			{
				throw ProtoReader.EoF(this);
			}
			num2 = (ulong)this.ioBuffer[num++];
			value |= (num2 & 127UL) << 35;
			if ((num2 & 128UL) == 0UL)
			{
				return 6;
			}
			if (this.available == 6)
			{
				throw ProtoReader.EoF(this);
			}
			num2 = (ulong)this.ioBuffer[num++];
			value |= (num2 & 127UL) << 42;
			if ((num2 & 128UL) == 0UL)
			{
				return 7;
			}
			if (this.available == 7)
			{
				throw ProtoReader.EoF(this);
			}
			num2 = (ulong)this.ioBuffer[num++];
			value |= (num2 & 127UL) << 49;
			if ((num2 & 128UL) == 0UL)
			{
				return 8;
			}
			if (this.available == 8)
			{
				throw ProtoReader.EoF(this);
			}
			num2 = (ulong)this.ioBuffer[num++];
			value |= (num2 & 127UL) << 56;
			if ((num2 & 128UL) == 0UL)
			{
				return 9;
			}
			if (this.available == 9)
			{
				throw ProtoReader.EoF(this);
			}
			num2 = (ulong)this.ioBuffer[num];
			value |= num2 << 63;
			if ((num2 & 18446744073709551614UL) != 0UL)
			{
				throw ProtoReader.AddErrorData(new OverflowException(), this);
			}
			return 10;
		}

		// Token: 0x06000162 RID: 354 RVA: 0x0000B0B0 File Offset: 0x000092B0
		private ulong ReadUInt64Variant()
		{
			ulong result;
			int num = this.TryReadUInt64VariantWithoutMoving(out result);
			if (num > 0)
			{
				this.ioIndex += num;
				this.available -= num;
				this.position += num;
				return result;
			}
			throw ProtoReader.EoF(this);
		}

		// Token: 0x06000163 RID: 355 RVA: 0x0000B0FC File Offset: 0x000092FC
		private string Intern(string value)
		{
			if (value == null)
			{
				return null;
			}
			if (value.Length == 0)
			{
				return "";
			}
			string text;
			if (this.stringInterner == null)
			{
				this.stringInterner = new Dictionary<string, string>();
				this.stringInterner.Add(value, value);
			}
			else if (this.stringInterner.TryGetValue(value, out text))
			{
				value = text;
			}
			else
			{
				this.stringInterner.Add(value, value);
			}
			return value;
		}

		// Token: 0x06000164 RID: 356 RVA: 0x0000B164 File Offset: 0x00009364
		public string ReadString()
		{
			if (this.wireType != WireType.String)
			{
				throw this.CreateWireTypeException();
			}
			int num = (int)this.ReadUInt32Variant(false);
			if (num == 0)
			{
				return "";
			}
			if (this.available < num)
			{
				this.Ensure(num, true);
			}
			string text = ProtoReader.encoding.GetString(this.ioBuffer, this.ioIndex, num);
			if (this.internStrings)
			{
				text = this.Intern(text);
			}
			this.available -= num;
			this.position += num;
			this.ioIndex += num;
			return text;
		}

		// Token: 0x06000165 RID: 357 RVA: 0x0000B1F8 File Offset: 0x000093F8
		public void ThrowEnumException(Type type, int value)
		{
			string text = (type == null) ? "<null>" : type.FullName;
			throw ProtoReader.AddErrorData(new ProtoException(string.Concat(new object[]
			{
				"No ",
				text,
				" enum is mapped to the wire-value ",
				value
			})), this);
		}

		// Token: 0x06000166 RID: 358 RVA: 0x0000B249 File Offset: 0x00009449
		private Exception CreateWireTypeException()
		{
			return this.CreateException("Invalid wire-type; this usually means you have over-written a file without truncating or setting the length; see http://stackoverflow.com/q/2152978/23354");
		}

		// Token: 0x06000167 RID: 359 RVA: 0x0000B256 File Offset: 0x00009456
		private Exception CreateException(string message)
		{
			return ProtoReader.AddErrorData(new ProtoException(message), this);
		}

		// Token: 0x06000168 RID: 360 RVA: 0x0000B264 File Offset: 0x00009464
		public unsafe double ReadDouble()
		{
			WireType wireType = this.wireType;
			if (wireType == WireType.Fixed64)
			{
				long num = this.ReadInt64();
				return *(double*)(&num);
			}
			if (wireType == WireType.Fixed32)
			{
				return (double)this.ReadSingle();
			}
			throw this.CreateWireTypeException();
		}

		// Token: 0x06000169 RID: 361 RVA: 0x0000B29A File Offset: 0x0000949A
		public static object ReadObject(object value, int key, ProtoReader reader)
		{
			return ProtoReader.ReadTypedObject(value, key, reader, null);
		}

		// Token: 0x0600016A RID: 362 RVA: 0x0000B2A8 File Offset: 0x000094A8
		internal static object ReadTypedObject(object value, int key, ProtoReader reader, Type type)
		{
			if (reader.model == null)
			{
				throw ProtoReader.AddErrorData(new InvalidOperationException("Cannot deserialize sub-objects unless a model is provided"), reader);
			}
			SubItemToken token = ProtoReader.StartSubItem(reader);
			if (key >= 0)
			{
				value = reader.model.Deserialize(key, value, reader);
			}
			else if (type == null || !reader.model.TryDeserializeAuxiliaryType(reader, DataFormat.Default, 1, type, ref value, true, false, true, false))
			{
				TypeModel.ThrowUnexpectedType(type);
			}
			ProtoReader.EndSubItem(token, reader);
			return value;
		}

		// Token: 0x0600016B RID: 363 RVA: 0x0000B314 File Offset: 0x00009514
		public static void EndSubItem(SubItemToken token, ProtoReader reader)
		{
			int value = token.value;
			if (reader.wireType == WireType.EndGroup)
			{
				if (value >= 0)
				{
					throw ProtoReader.AddErrorData(new ArgumentException("token"), reader);
				}
				if (-value != reader.fieldNumber)
				{
					throw reader.CreateException("Wrong group was ended");
				}
				reader.wireType = WireType.None;
				reader.depth--;
				return;
			}
			else
			{
				if (value < reader.position)
				{
					throw reader.CreateException("Sub-message not read entirely");
				}
				if (reader.blockEnd != reader.position && reader.blockEnd != 2147483647)
				{
					throw reader.CreateException("Sub-message not read correctly");
				}
				reader.blockEnd = value;
				reader.depth--;
				return;
			}
		}

		// Token: 0x0600016C RID: 364 RVA: 0x0000B3C4 File Offset: 0x000095C4
		public static SubItemToken StartSubItem(ProtoReader reader)
		{
			WireType wireType = reader.wireType;
			if (wireType != WireType.String)
			{
				if (wireType != WireType.StartGroup)
				{
					throw reader.CreateWireTypeException();
				}
				reader.wireType = WireType.None;
				reader.depth++;
				return new SubItemToken(-reader.fieldNumber);
			}
			else
			{
				int num = (int)reader.ReadUInt32Variant(false);
				if (num < 0)
				{
					throw ProtoReader.AddErrorData(new InvalidOperationException(), reader);
				}
				int value = reader.blockEnd;
				reader.blockEnd = reader.position + num;
				reader.depth++;
				return new SubItemToken(value);
			}
		}

		// Token: 0x0600016D RID: 365 RVA: 0x0000B44C File Offset: 0x0000964C
		public int ReadFieldHeader()
		{
			if (this.blockEnd <= this.position || this.wireType == WireType.EndGroup)
			{
				return 0;
			}
			uint num;
			if (this.TryReadUInt32Variant(out num))
			{
				this.wireType = (WireType)(num & 7U);
				this.fieldNumber = (int)(num >> 3);
				if (this.fieldNumber < 1)
				{
					throw new ProtoException("Invalid field in source data: " + this.fieldNumber.ToString());
				}
			}
			else
			{
				this.wireType = WireType.None;
				this.fieldNumber = 0;
			}
			if (this.wireType != WireType.EndGroup)
			{
				return this.fieldNumber;
			}
			return 0;
		}

		// Token: 0x0600016E RID: 366 RVA: 0x0000B4D4 File Offset: 0x000096D4
		public bool TryReadFieldHeader(int field)
		{
			if (this.blockEnd <= this.position || this.wireType == WireType.EndGroup)
			{
				return false;
			}
			uint num2;
			int num = this.TryReadUInt32VariantWithoutMoving(false, out num2);
			WireType wireType;
			if (num > 0 && (int)num2 >> 3 == field && (wireType = (WireType)(num2 & 7U)) != WireType.EndGroup)
			{
				this.wireType = wireType;
				this.fieldNumber = field;
				this.position += num;
				this.ioIndex += num;
				this.available -= num;
				return true;
			}
			return false;
		}

		// Token: 0x1700002C RID: 44
		// (get) Token: 0x0600016F RID: 367 RVA: 0x0000B551 File Offset: 0x00009751
		public TypeModel Model
		{
			get
			{
				return this.model;
			}
		}

		// Token: 0x06000170 RID: 368 RVA: 0x0000B559 File Offset: 0x00009759
		public void Hint(WireType wireType)
		{
			if (this.wireType == wireType)
			{
				return;
			}
			if ((wireType & (WireType)7) == this.wireType)
			{
				this.wireType = wireType;
			}
		}

		// Token: 0x06000171 RID: 369 RVA: 0x0000B577 File Offset: 0x00009777
		public void Assert(WireType wireType)
		{
			if (this.wireType == wireType)
			{
				return;
			}
			if ((wireType & (WireType)7) == this.wireType)
			{
				this.wireType = wireType;
				return;
			}
			throw this.CreateWireTypeException();
		}

		// Token: 0x06000172 RID: 370 RVA: 0x0000B59C File Offset: 0x0000979C
		public void SkipField()
		{
			switch (this.wireType)
			{
			case WireType.Variant:
			case WireType.SignedVariant:
				this.ReadUInt64Variant();
				return;
			case WireType.Fixed64:
				if (this.available < 8)
				{
					this.Ensure(8, true);
				}
				this.available -= 8;
				this.ioIndex += 8;
				this.position += 8;
				return;
			case WireType.String:
			{
				int num = (int)this.ReadUInt32Variant(false);
				if (num <= this.available)
				{
					this.available -= num;
					this.ioIndex += num;
					this.position += num;
					return;
				}
				this.position += num;
				num -= this.available;
				this.ioIndex = (this.available = 0);
				if (this.isFixedLength)
				{
					if (num > this.dataRemaining)
					{
						throw ProtoReader.EoF(this);
					}
					this.dataRemaining -= num;
				}
				ProtoReader.Seek(this.source, num, this.ioBuffer);
				return;
			}
			case WireType.StartGroup:
			{
				int num2 = this.fieldNumber;
				while (this.ReadFieldHeader() > 0)
				{
					this.SkipField();
				}
				if (this.wireType == WireType.EndGroup && this.fieldNumber == num2)
				{
					this.wireType = WireType.None;
					return;
				}
				throw this.CreateWireTypeException();
			}
			case WireType.Fixed32:
				if (this.available < 4)
				{
					this.Ensure(4, true);
				}
				this.available -= 4;
				this.ioIndex += 4;
				this.position += 4;
				return;
			}
			throw this.CreateWireTypeException();
		}

		// Token: 0x06000173 RID: 371 RVA: 0x0000B73C File Offset: 0x0000993C
		public ulong ReadUInt64()
		{
			WireType wireType = this.wireType;
			if (wireType == WireType.Variant)
			{
				return this.ReadUInt64Variant();
			}
			if (wireType == WireType.Fixed64)
			{
				if (this.available < 8)
				{
					this.Ensure(8, true);
				}
				this.position += 8;
				this.available -= 8;
				byte[] array = this.ioBuffer;
				int num = this.ioIndex;
				this.ioIndex = num + 1;
				ulong num2 = array[num];
				byte[] array2 = this.ioBuffer;
				num = this.ioIndex;
				this.ioIndex = num + 1;
				ulong num3 = num2 | array2[num] << 8;
				byte[] array3 = this.ioBuffer;
				num = this.ioIndex;
				this.ioIndex = num + 1;
				ulong num4 = num3 | array3[num] << 16;
				byte[] array4 = this.ioBuffer;
				num = this.ioIndex;
				this.ioIndex = num + 1;
				ulong num5 = num4 | array4[num] << 24;
				byte[] array5 = this.ioBuffer;
				num = this.ioIndex;
				this.ioIndex = num + 1;
				ulong num6 = num5 | array5[num] << 32;
				byte[] array6 = this.ioBuffer;
				num = this.ioIndex;
				this.ioIndex = num + 1;
				ulong num7 = num6 | array6[num] << 40;
				byte[] array7 = this.ioBuffer;
				num = this.ioIndex;
				this.ioIndex = num + 1;
				ulong num8 = num7 | array7[num] << 48;
				byte[] array8 = this.ioBuffer;
				num = this.ioIndex;
				this.ioIndex = num + 1;
				return num8 | array8[num] << 56;
			}
			if (wireType != WireType.Fixed32)
			{
				throw this.CreateWireTypeException();
			}
			return (ulong)this.ReadUInt32();
		}

		// Token: 0x06000174 RID: 372 RVA: 0x0000B888 File Offset: 0x00009A88
		public unsafe float ReadSingle()
		{
			WireType wireType = this.wireType;
			if (wireType != WireType.Fixed64)
			{
				if (wireType == WireType.Fixed32)
				{
					int num = this.ReadInt32();
					return *(float*)(&num);
				}
				throw this.CreateWireTypeException();
			}
			else
			{
				double num2 = this.ReadDouble();
				float num3 = (float)num2;
				if (Helpers.IsInfinity(num3) && !Helpers.IsInfinity(num2))
				{
					throw ProtoReader.AddErrorData(new OverflowException(), this);
				}
				return num3;
			}
		}

		// Token: 0x06000175 RID: 373 RVA: 0x0000B8DC File Offset: 0x00009ADC
		public bool ReadBoolean()
		{
			uint num = this.ReadUInt32();
			if (num == 0U)
			{
				return false;
			}
			if (num != 1U)
			{
				throw this.CreateException("Unexpected boolean value");
			}
			return true;
		}

		// Token: 0x06000176 RID: 374 RVA: 0x0000B908 File Offset: 0x00009B08
		public static byte[] AppendBytes(byte[] value, ProtoReader reader)
		{
			if (reader.wireType != WireType.String)
			{
				throw reader.CreateWireTypeException();
			}
			int i = (int)reader.ReadUInt32Variant(false);
			reader.wireType = WireType.None;
			if (i != 0)
			{
				int num;
				if (value == null || value.Length == 0)
				{
					num = 0;
					value = new byte[i];
				}
				else
				{
					num = value.Length;
					byte[] array = new byte[value.Length + i];
					Helpers.BlockCopy(value, 0, array, 0, value.Length);
					value = array;
				}
				reader.position += i;
				while (i > reader.available)
				{
					if (reader.available > 0)
					{
						Helpers.BlockCopy(reader.ioBuffer, reader.ioIndex, value, num, reader.available);
						i -= reader.available;
						num += reader.available;
						reader.ioIndex = (reader.available = 0);
					}
					int num2 = (i > reader.ioBuffer.Length) ? reader.ioBuffer.Length : i;
					if (num2 > 0)
					{
						reader.Ensure(num2, true);
					}
				}
				if (i > 0)
				{
					Helpers.BlockCopy(reader.ioBuffer, reader.ioIndex, value, num, i);
					reader.ioIndex += i;
					reader.available -= i;
				}
				return value;
			}
			if (value != null)
			{
				return value;
			}
			return ProtoReader.EmptyBlob;
		}

		// Token: 0x06000177 RID: 375 RVA: 0x0000BA30 File Offset: 0x00009C30
		private static byte[] ReadBytes(Stream stream, int length)
		{
			if (stream == null)
			{
				throw new ArgumentNullException("stream");
			}
			if (length < 0)
			{
				throw new ArgumentOutOfRangeException("length");
			}
			byte[] array = new byte[length];
			int offset = 0;
			int num;
			while (length > 0 && (num = stream.Read(array, offset, length)) > 0)
			{
				length -= num;
			}
			if (length > 0)
			{
				throw ProtoReader.EoF(null);
			}
			return array;
		}

		// Token: 0x06000178 RID: 376 RVA: 0x0000BA88 File Offset: 0x00009C88
		private static int ReadByteOrThrow(Stream source)
		{
			int num = source.ReadByte();
			if (num < 0)
			{
				throw ProtoReader.EoF(null);
			}
			return num;
		}

		// Token: 0x06000179 RID: 377 RVA: 0x0000BA9C File Offset: 0x00009C9C
		public static int ReadLengthPrefix(Stream source, bool expectHeader, PrefixStyle style, out int fieldNumber)
		{
			int num;
			return ProtoReader.ReadLengthPrefix(source, expectHeader, style, out fieldNumber, out num);
		}

		// Token: 0x0600017A RID: 378 RVA: 0x0000BAB4 File Offset: 0x00009CB4
		public static int DirectReadLittleEndianInt32(Stream source)
		{
			return ProtoReader.ReadByteOrThrow(source) | ProtoReader.ReadByteOrThrow(source) << 8 | ProtoReader.ReadByteOrThrow(source) << 16 | ProtoReader.ReadByteOrThrow(source) << 24;
		}

		// Token: 0x0600017B RID: 379 RVA: 0x0000BAD9 File Offset: 0x00009CD9
		public static int DirectReadBigEndianInt32(Stream source)
		{
			return ProtoReader.ReadByteOrThrow(source) << 24 | ProtoReader.ReadByteOrThrow(source) << 16 | ProtoReader.ReadByteOrThrow(source) << 8 | ProtoReader.ReadByteOrThrow(source);
		}

		// Token: 0x0600017C RID: 380 RVA: 0x0000BB00 File Offset: 0x00009D00
		public static int DirectReadVarintInt32(Stream source)
		{
			uint result;
			if (ProtoReader.TryReadUInt32Variant(source, out result) <= 0)
			{
				throw ProtoReader.EoF(null);
			}
			return (int)result;
		}

		// Token: 0x0600017D RID: 381 RVA: 0x0000BB20 File Offset: 0x00009D20
		public static void DirectReadBytes(Stream source, byte[] buffer, int offset, int count)
		{
			int num;
			while (count > 0 && (num = source.Read(buffer, offset, count)) > 0)
			{
				count -= num;
				offset += num;
			}
			if (count > 0)
			{
				throw ProtoReader.EoF(null);
			}
		}

		// Token: 0x0600017E RID: 382 RVA: 0x0000BB58 File Offset: 0x00009D58
		public static byte[] DirectReadBytes(Stream source, int count)
		{
			byte[] array = new byte[count];
			ProtoReader.DirectReadBytes(source, array, 0, count);
			return array;
		}

		// Token: 0x0600017F RID: 383 RVA: 0x0000BB78 File Offset: 0x00009D78
		public static string DirectReadString(Stream source, int length)
		{
			byte[] array = new byte[length];
			ProtoReader.DirectReadBytes(source, array, 0, length);
			return Encoding.UTF8.GetString(array, 0, length);
		}

		// Token: 0x06000180 RID: 384 RVA: 0x0000BBA4 File Offset: 0x00009DA4
		public static int ReadLengthPrefix(Stream source, bool expectHeader, PrefixStyle style, out int fieldNumber, out int bytesRead)
		{
			fieldNumber = 0;
			switch (style)
			{
			case PrefixStyle.None:
				bytesRead = 0;
				return int.MaxValue;
			case PrefixStyle.Base128:
				bytesRead = 0;
				if (expectHeader)
				{
					uint num2;
					int num = ProtoReader.TryReadUInt32Variant(source, out num2);
					bytesRead += num;
					if (num <= 0)
					{
						bytesRead = 0;
						return -1;
					}
					if ((num2 & 7U) != 2U)
					{
						throw new InvalidOperationException();
					}
					fieldNumber = (int)(num2 >> 3);
					num = ProtoReader.TryReadUInt32Variant(source, out num2);
					bytesRead += num;
					if (bytesRead == 0)
					{
						throw ProtoReader.EoF(null);
					}
					return (int)num2;
				}
				else
				{
					uint result;
					int num3 = ProtoReader.TryReadUInt32Variant(source, out result);
					bytesRead += num3;
					if (bytesRead >= 0)
					{
						return (int)result;
					}
					return -1;
				}
				break;
			case PrefixStyle.Fixed32:
			{
				int num4 = source.ReadByte();
				if (num4 < 0)
				{
					bytesRead = 0;
					return -1;
				}
				bytesRead = 4;
				return num4 | ProtoReader.ReadByteOrThrow(source) << 8 | ProtoReader.ReadByteOrThrow(source) << 16 | ProtoReader.ReadByteOrThrow(source) << 24;
			}
			case PrefixStyle.Fixed32BigEndian:
			{
				int num5 = source.ReadByte();
				if (num5 < 0)
				{
					bytesRead = 0;
					return -1;
				}
				bytesRead = 4;
				return num5 << 24 | ProtoReader.ReadByteOrThrow(source) << 16 | ProtoReader.ReadByteOrThrow(source) << 8 | ProtoReader.ReadByteOrThrow(source);
			}
			default:
				throw new ArgumentOutOfRangeException("style");
			}
		}

		// Token: 0x06000181 RID: 385 RVA: 0x0000BCC0 File Offset: 0x00009EC0
		private static int TryReadUInt32Variant(Stream source, out uint value)
		{
			value = 0U;
			int num = source.ReadByte();
			if (num < 0)
			{
				return 0;
			}
			value = (uint)num;
			if ((value & 128U) == 0U)
			{
				return 1;
			}
			value &= 127U;
			num = source.ReadByte();
			if (num < 0)
			{
				throw ProtoReader.EoF(null);
			}
			value |= (uint)((uint)(num & 127) << 7);
			if ((num & 128) == 0)
			{
				return 2;
			}
			num = source.ReadByte();
			if (num < 0)
			{
				throw ProtoReader.EoF(null);
			}
			value |= (uint)((uint)(num & 127) << 14);
			if ((num & 128) == 0)
			{
				return 3;
			}
			num = source.ReadByte();
			if (num < 0)
			{
				throw ProtoReader.EoF(null);
			}
			value |= (uint)((uint)(num & 127) << 21);
			if ((num & 128) == 0)
			{
				return 4;
			}
			num = source.ReadByte();
			if (num < 0)
			{
				throw ProtoReader.EoF(null);
			}
			value |= (uint)((uint)num << 28);
			if ((num & 240) == 0)
			{
				return 5;
			}
			throw new OverflowException();
		}

		// Token: 0x06000182 RID: 386 RVA: 0x0000BD98 File Offset: 0x00009F98
		internal static void Seek(Stream source, int count, byte[] buffer)
		{
			if (source.CanSeek)
			{
				source.Seek((long)count, SeekOrigin.Current);
				count = 0;
			}
			else
			{
				if (buffer != null)
				{
					while (count > buffer.Length)
					{
						int num;
						if ((num = source.Read(buffer, 0, buffer.Length)) <= 0)
						{
							IL_4F:
							while (count > 0)
							{
								int num2;
								if ((num2 = source.Read(buffer, 0, count)) <= 0)
								{
									break;
								}
								count -= num2;
							}
							goto IL_9C;
						}
						count -= num;
					}
					goto IL_4F;
				}
				buffer = BufferPool.GetBuffer();
				try
				{
					while (count > buffer.Length)
					{
						int num3;
						if ((num3 = source.Read(buffer, 0, buffer.Length)) <= 0)
						{
							IL_80:
							while (count > 0 && (num3 = source.Read(buffer, 0, count)) > 0)
							{
								count -= num3;
							}
							goto IL_9C;
						}
						count -= num3;
					}
					goto IL_80;
				}
				finally
				{
					BufferPool.ReleaseBufferToPool(ref buffer);
				}
			}
			IL_9C:
			if (count > 0)
			{
				throw ProtoReader.EoF(null);
			}
		}

		// Token: 0x06000183 RID: 387 RVA: 0x0000BE5C File Offset: 0x0000A05C
		internal static Exception AddErrorData(Exception exception, ProtoReader source)
		{
			if (exception != null && source != null && !exception.Data.Contains("protoSource"))
			{
				exception.Data.Add("protoSource", string.Format("tag={0}; wire-type={1}; offset={2}; depth={3}", new object[]
				{
					source.fieldNumber,
					source.wireType,
					source.position,
					source.depth
				}));
			}
			return exception;
		}

		// Token: 0x06000184 RID: 388 RVA: 0x0000BEDA File Offset: 0x0000A0DA
		private static Exception EoF(ProtoReader source)
		{
			return ProtoReader.AddErrorData(new EndOfStreamException(), source);
		}

		// Token: 0x06000185 RID: 389 RVA: 0x0000BEE8 File Offset: 0x0000A0E8
		public void AppendExtensionData(IExtensible instance)
		{
			if (instance == null)
			{
				throw new ArgumentNullException("instance");
			}
			IExtension extensionObject = instance.GetExtensionObject(true);
			bool commit = false;
			Stream stream = extensionObject.BeginAppend();
			try
			{
				using (ProtoWriter protoWriter = new ProtoWriter(stream, this.model, null))
				{
					this.AppendExtensionField(protoWriter);
					protoWriter.Close();
				}
				commit = true;
			}
			finally
			{
				extensionObject.EndAppend(stream, commit);
			}
		}

		// Token: 0x06000186 RID: 390 RVA: 0x0000BF64 File Offset: 0x0000A164
		private void AppendExtensionField(ProtoWriter writer)
		{
			ProtoWriter.WriteFieldHeader(this.fieldNumber, this.wireType, writer);
			switch (this.wireType)
			{
			case WireType.Variant:
			case WireType.Fixed64:
			case WireType.SignedVariant:
				ProtoWriter.WriteInt64(this.ReadInt64(), writer);
				return;
			case WireType.String:
				ProtoWriter.WriteBytes(ProtoReader.AppendBytes(null, this), writer);
				return;
			case WireType.StartGroup:
			{
				SubItemToken token = ProtoReader.StartSubItem(this);
				SubItemToken token2 = ProtoWriter.StartSubItem(null, writer);
				while (this.ReadFieldHeader() > 0)
				{
					this.AppendExtensionField(writer);
				}
				ProtoReader.EndSubItem(token, this);
				ProtoWriter.EndSubItem(token2, writer);
				return;
			}
			case WireType.Fixed32:
				ProtoWriter.WriteInt32(this.ReadInt32(), writer);
				return;
			}
			throw this.CreateWireTypeException();
		}

		// Token: 0x06000187 RID: 391 RVA: 0x0000C014 File Offset: 0x0000A214
		public static bool HasSubValue(WireType wireType, ProtoReader source)
		{
			if (source.blockEnd <= source.position || wireType == WireType.EndGroup)
			{
				return false;
			}
			source.wireType = wireType;
			return true;
		}

		// Token: 0x06000188 RID: 392 RVA: 0x0000C032 File Offset: 0x0000A232
		internal int GetTypeKey(ref Type type)
		{
			return this.model.GetKey(ref type);
		}

		// Token: 0x1700002D RID: 45
		// (get) Token: 0x06000189 RID: 393 RVA: 0x0000C040 File Offset: 0x0000A240
		internal NetObjectCache NetCache
		{
			get
			{
				return this.netCache;
			}
		}

		// Token: 0x0600018A RID: 394 RVA: 0x0000C048 File Offset: 0x0000A248
		internal Type DeserializeType(string value)
		{
			return TypeModel.DeserializeType(this.model, value);
		}

		// Token: 0x0600018B RID: 395 RVA: 0x0000C056 File Offset: 0x0000A256
		internal void SetRootObject(object value)
		{
			this.netCache.SetKeyedObject(0, value);
			this.trapCount -= 1U;
		}

		// Token: 0x0600018C RID: 396 RVA: 0x0000C073 File Offset: 0x0000A273
		public static void NoteObject(object value, ProtoReader reader)
		{
			if (reader.trapCount != 0U)
			{
				reader.netCache.RegisterTrappedObject(value);
				reader.trapCount -= 1U;
			}
		}

		// Token: 0x0600018D RID: 397 RVA: 0x0000C097 File Offset: 0x0000A297
		public Type ReadType()
		{
			return TypeModel.DeserializeType(this.model, this.ReadString());
		}

		// Token: 0x0600018E RID: 398 RVA: 0x0000C0AA File Offset: 0x0000A2AA
		internal void TrapNextObject(int newObjectKey)
		{
			this.trapCount += 1U;
			this.netCache.SetKeyedObject(newObjectKey, null);
		}

		// Token: 0x0600018F RID: 399 RVA: 0x0000C0C7 File Offset: 0x0000A2C7
		internal void CheckFullyConsumed()
		{
			if (this.isFixedLength && this.dataRemaining != 0)
			{
				throw new ProtoException("Incorrect number of bytes consumed");
			}
		}

		// Token: 0x040001BB RID: 443
		private const long Int64Msb = -9223372036854775808L;

		// Token: 0x040001BC RID: 444
		private const int Int32Msb = -2147483648;

		// Token: 0x040001BD RID: 445
		private Stream source;

		// Token: 0x040001BE RID: 446
		private byte[] ioBuffer;

		// Token: 0x040001BF RID: 447
		private TypeModel model;

		// Token: 0x040001C0 RID: 448
		private int fieldNumber;

		// Token: 0x040001C1 RID: 449
		private WireType wireType = WireType.None;

		// Token: 0x040001C2 RID: 450
		private int dataRemaining;

		// Token: 0x040001C3 RID: 451
		private readonly bool isFixedLength;

		// Token: 0x040001C4 RID: 452
		private bool internStrings = true;

		// Token: 0x040001C5 RID: 453
		private readonly SerializationContext context;

		// Token: 0x040001C6 RID: 454
		private int ioIndex;

		// Token: 0x040001C7 RID: 455
		private int position;

		// Token: 0x040001C8 RID: 456
		private int available;

		// Token: 0x040001C9 RID: 457
		private Dictionary<string, string> stringInterner;

		// Token: 0x040001CA RID: 458
		private static readonly UTF8Encoding encoding = new UTF8Encoding();

		// Token: 0x040001CB RID: 459
		private int depth;

		// Token: 0x040001CC RID: 460
		private int blockEnd = int.MaxValue;

		// Token: 0x040001CD RID: 461
		private static readonly byte[] EmptyBlob = new byte[0];

		// Token: 0x040001CE RID: 462
		private readonly NetObjectCache netCache = new NetObjectCache();

		// Token: 0x040001CF RID: 463
		private uint trapCount = 1U;
	}
}
