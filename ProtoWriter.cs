using System;
using System.IO;
using System.Text;
using ProtoBuf.Meta;

namespace ProtoBuf
{
	// Token: 0x02000032 RID: 50
	public sealed class ProtoWriter : IDisposable
	{
		// Token: 0x06000191 RID: 401 RVA: 0x0000C0FC File Offset: 0x0000A2FC
		public static void WriteObject(object value, int key, ProtoWriter writer)
		{
			if (writer.model == null)
			{
				throw new InvalidOperationException("Cannot serialize sub-objects unless a model is provided");
			}
			SubItemToken token = ProtoWriter.StartSubItem(value, writer);
			if (key >= 0)
			{
				writer.model.Serialize(key, value, writer);
			}
			else if (writer.model == null || !writer.model.TrySerializeAuxiliaryType(writer, value.GetType(), DataFormat.Default, 1, value, false))
			{
				TypeModel.ThrowUnexpectedType(value.GetType());
			}
			ProtoWriter.EndSubItem(token, writer);
		}

		// Token: 0x06000192 RID: 402 RVA: 0x0000C168 File Offset: 0x0000A368
		public static void WriteRecursionSafeObject(object value, int key, ProtoWriter writer)
		{
			if (writer.model == null)
			{
				throw new InvalidOperationException("Cannot serialize sub-objects unless a model is provided");
			}
			SubItemToken token = ProtoWriter.StartSubItem(null, writer);
			writer.model.Serialize(key, value, writer);
			ProtoWriter.EndSubItem(token, writer);
		}

		// Token: 0x06000193 RID: 403 RVA: 0x0000C198 File Offset: 0x0000A398
		internal static void WriteObject(object value, int key, ProtoWriter writer, PrefixStyle style, int fieldNumber)
		{
			if (writer.model == null)
			{
				throw new InvalidOperationException("Cannot serialize sub-objects unless a model is provided");
			}
			if (writer.wireType != WireType.None)
			{
				throw ProtoWriter.CreateException(writer);
			}
			if (style != PrefixStyle.Base128)
			{
				if (style - PrefixStyle.Fixed32 > 1)
				{
					throw new ArgumentOutOfRangeException("style");
				}
				writer.fieldNumber = 0;
				writer.wireType = WireType.Fixed32;
			}
			else
			{
				writer.wireType = WireType.String;
				writer.fieldNumber = fieldNumber;
				if (fieldNumber > 0)
				{
					ProtoWriter.WriteHeaderCore(fieldNumber, WireType.String, writer);
				}
			}
			SubItemToken token = ProtoWriter.StartSubItem(value, writer, true);
			if (key < 0)
			{
				if (!writer.model.TrySerializeAuxiliaryType(writer, value.GetType(), DataFormat.Default, 1, value, false))
				{
					TypeModel.ThrowUnexpectedType(value.GetType());
				}
			}
			else
			{
				writer.model.Serialize(key, value, writer);
			}
			ProtoWriter.EndSubItem(token, writer, style);
		}

		// Token: 0x06000194 RID: 404 RVA: 0x0000C254 File Offset: 0x0000A454
		internal int GetTypeKey(ref Type type)
		{
			return this.model.GetKey(ref type);
		}

		// Token: 0x1700002E RID: 46
		// (get) Token: 0x06000195 RID: 405 RVA: 0x0000C262 File Offset: 0x0000A462
		internal NetObjectCache NetCache
		{
			get
			{
				return this.netCache;
			}
		}

		// Token: 0x1700002F RID: 47
		// (get) Token: 0x06000196 RID: 406 RVA: 0x0000C26A File Offset: 0x0000A46A
		internal WireType WireType
		{
			get
			{
				return this.wireType;
			}
		}

		// Token: 0x06000197 RID: 407 RVA: 0x0000C274 File Offset: 0x0000A474
		public static void WriteFieldHeader(int fieldNumber, WireType wireType, ProtoWriter writer)
		{
			if (writer.wireType != WireType.None)
			{
				throw new InvalidOperationException(string.Concat(new object[]
				{
					"Cannot write a ",
					wireType,
					" header until the ",
					writer.wireType,
					" data has been written"
				}));
			}
			if (fieldNumber < 0)
			{
				throw new ArgumentOutOfRangeException("fieldNumber");
			}
			if (writer.packedFieldNumber == 0)
			{
				writer.fieldNumber = fieldNumber;
				writer.wireType = wireType;
				ProtoWriter.WriteHeaderCore(fieldNumber, wireType, writer);
				return;
			}
			if (writer.packedFieldNumber != fieldNumber)
			{
				throw new InvalidOperationException(string.Concat(new object[]
				{
					"Field mismatch during packed encoding; expected ",
					writer.packedFieldNumber,
					" but received ",
					fieldNumber
				}));
			}
			if (wireType > WireType.Fixed64 && wireType != WireType.Fixed32 && wireType != WireType.SignedVariant)
			{
				throw new InvalidOperationException("Wire-type cannot be encoded as packed: " + wireType.ToString());
			}
			writer.fieldNumber = fieldNumber;
			writer.wireType = wireType;
		}

		// Token: 0x06000198 RID: 408 RVA: 0x0000C370 File Offset: 0x0000A570
		internal static void WriteHeaderCore(int fieldNumber, WireType wireType, ProtoWriter writer)
		{
			ProtoWriter.WriteUInt32Variant((uint)(fieldNumber << 3 | (int)(wireType & (WireType)7)), writer);
		}

		// Token: 0x06000199 RID: 409 RVA: 0x0000C37F File Offset: 0x0000A57F
		public static void WriteBytes(byte[] data, ProtoWriter writer)
		{
			ProtoWriter.WriteBytes(data, 0, data.Length, writer);
		}

		// Token: 0x0600019A RID: 410 RVA: 0x0000C38C File Offset: 0x0000A58C
		public static void WriteBytes(byte[] data, int offset, int length, ProtoWriter writer)
		{
			if (data == null)
			{
				throw new ArgumentNullException("blob");
			}
			switch (writer.wireType)
			{
			case WireType.Fixed64:
				if (length != 8)
				{
					throw new ArgumentException("length");
				}
				goto IL_A0;
			case WireType.String:
				ProtoWriter.WriteUInt32Variant((uint)length, writer);
				writer.wireType = WireType.None;
				if (length == 0)
				{
					return;
				}
				if (writer.flushLock == 0 && length > writer.ioBuffer.Length)
				{
					ProtoWriter.Flush(writer);
					writer.dest.Write(data, offset, length);
					writer.position += length;
					return;
				}
				goto IL_A0;
			case WireType.Fixed32:
				if (length != 4)
				{
					throw new ArgumentException("length");
				}
				goto IL_A0;
			}
			throw ProtoWriter.CreateException(writer);
			IL_A0:
			ProtoWriter.DemandSpace(length, writer);
			Helpers.BlockCopy(data, offset, writer.ioBuffer, writer.ioIndex, length);
			ProtoWriter.IncrementedAndReset(length, writer);
		}

		// Token: 0x0600019B RID: 411 RVA: 0x0000C45C File Offset: 0x0000A65C
		private static void CopyRawFromStream(Stream source, ProtoWriter writer)
		{
			byte[] array = writer.ioBuffer;
			int num = array.Length - writer.ioIndex;
			int num2 = 1;
			while (num > 0 && (num2 = source.Read(array, writer.ioIndex, num)) > 0)
			{
				writer.ioIndex += num2;
				writer.position += num2;
				num -= num2;
			}
			if (num2 <= 0)
			{
				return;
			}
			if (writer.flushLock == 0)
			{
				ProtoWriter.Flush(writer);
				while ((num2 = source.Read(array, 0, array.Length)) > 0)
				{
					writer.dest.Write(array, 0, num2);
					writer.position += num2;
				}
				return;
			}
			for (;;)
			{
				ProtoWriter.DemandSpace(128, writer);
				if ((num2 = source.Read(writer.ioBuffer, writer.ioIndex, writer.ioBuffer.Length - writer.ioIndex)) <= 0)
				{
					break;
				}
				writer.position += num2;
				writer.ioIndex += num2;
			}
		}

		// Token: 0x0600019C RID: 412 RVA: 0x0000C547 File Offset: 0x0000A747
		private static void IncrementedAndReset(int length, ProtoWriter writer)
		{
			writer.ioIndex += length;
			writer.position += length;
			writer.wireType = WireType.None;
		}

		// Token: 0x0600019D RID: 413 RVA: 0x0000C56C File Offset: 0x0000A76C
		public static SubItemToken StartSubItem(object instance, ProtoWriter writer)
		{
			return ProtoWriter.StartSubItem(instance, writer, false);
		}

		// Token: 0x0600019E RID: 414 RVA: 0x0000C578 File Offset: 0x0000A778
		private void CheckRecursionStackAndPush(object instance)
		{
			int num;
			if (this.recursionStack == null)
			{
				this.recursionStack = new MutableList();
			}
			else if (instance != null && (num = this.recursionStack.IndexOfReference(instance)) >= 0)
			{
				throw new ProtoException("Possible recursion detected (offset: " + (this.recursionStack.Count - num).ToString() + " level(s)): " + instance.ToString());
			}
			this.recursionStack.Add(instance);
		}

		// Token: 0x0600019F RID: 415 RVA: 0x0000C5EB File Offset: 0x0000A7EB
		private void PopRecursionStack()
		{
			this.recursionStack.RemoveLast();
		}

		// Token: 0x060001A0 RID: 416 RVA: 0x0000C5F8 File Offset: 0x0000A7F8
		private static SubItemToken StartSubItem(object instance, ProtoWriter writer, bool allowFixed)
		{
			int num = writer.depth + 1;
			writer.depth = num;
			if (num > 25)
			{
				writer.CheckRecursionStackAndPush(instance);
			}
			if (writer.packedFieldNumber != 0)
			{
				throw new InvalidOperationException("Cannot begin a sub-item while performing packed encoding");
			}
			switch (writer.wireType)
			{
			case WireType.String:
				writer.wireType = WireType.None;
				ProtoWriter.DemandSpace(32, writer);
				writer.flushLock++;
				writer.position++;
				num = writer.ioIndex;
				writer.ioIndex = num + 1;
				return new SubItemToken(num);
			case WireType.StartGroup:
				writer.wireType = WireType.None;
				return new SubItemToken(-writer.fieldNumber);
			case WireType.Fixed32:
			{
				if (!allowFixed)
				{
					throw ProtoWriter.CreateException(writer);
				}
				ProtoWriter.DemandSpace(32, writer);
				writer.flushLock++;
				SubItemToken result = new SubItemToken(writer.ioIndex);
				ProtoWriter.IncrementedAndReset(4, writer);
				return result;
			}
			}
			throw ProtoWriter.CreateException(writer);
		}

		// Token: 0x060001A1 RID: 417 RVA: 0x0000C6E7 File Offset: 0x0000A8E7
		public static void EndSubItem(SubItemToken token, ProtoWriter writer)
		{
			ProtoWriter.EndSubItem(token, writer, PrefixStyle.Base128);
		}

		// Token: 0x060001A2 RID: 418 RVA: 0x0000C6F4 File Offset: 0x0000A8F4
		private static void EndSubItem(SubItemToken token, ProtoWriter writer, PrefixStyle style)
		{
			if (writer.wireType != WireType.None)
			{
				throw ProtoWriter.CreateException(writer);
			}
			int value = token.value;
			if (writer.depth <= 0)
			{
				throw ProtoWriter.CreateException(writer);
			}
			int num = writer.depth;
			writer.depth = num - 1;
			if (num > 25)
			{
				writer.PopRecursionStack();
			}
			writer.packedFieldNumber = 0;
			if (value < 0)
			{
				ProtoWriter.WriteHeaderCore(-value, WireType.EndGroup, writer);
				writer.wireType = WireType.None;
				return;
			}
			switch (style)
			{
			case PrefixStyle.Base128:
			{
				int num2 = writer.ioIndex - value - 1;
				int num3 = 0;
				uint num4 = (uint)num2;
				while ((num4 >>= 7) != 0U)
				{
					num3++;
				}
				if (num3 == 0)
				{
					writer.ioBuffer[value] = (byte)(num2 & 127);
				}
				else
				{
					ProtoWriter.DemandSpace(num3, writer);
					byte[] array = writer.ioBuffer;
					Helpers.BlockCopy(array, value + 1, array, value + 1 + num3, num2);
					num4 = (uint)num2;
					do
					{
						array[value++] = (byte)((num4 & 127U) | 128U);
					}
					while ((num4 >>= 7) != 0U);
					array[value - 1] = (byte)((int)array[value - 1] & -129);
					writer.position += num3;
					writer.ioIndex += num3;
				}
				break;
			}
			case PrefixStyle.Fixed32:
				ProtoWriter.WriteInt32ToBuffer(writer.ioIndex - value - 4, writer.ioBuffer, value);
				break;
			case PrefixStyle.Fixed32BigEndian:
			{
				int value2 = writer.ioIndex - value - 4;
				byte[] array2 = writer.ioBuffer;
				ProtoWriter.WriteInt32ToBuffer(value2, array2, value);
				byte b = array2[value];
				array2[value] = array2[value + 3];
				array2[value + 3] = b;
				b = array2[value + 1];
				array2[value + 1] = array2[value + 2];
				array2[value + 2] = b;
				break;
			}
			default:
				throw new ArgumentOutOfRangeException("style");
			}
			writer.flushLock--;
		}

		// Token: 0x060001A3 RID: 419 RVA: 0x0000C8A0 File Offset: 0x0000AAA0
		public ProtoWriter(Stream dest, TypeModel model, SerializationContext context)
		{
			if (dest == null)
			{
				throw new ArgumentNullException("dest");
			}
			if (!dest.CanWrite)
			{
				throw new ArgumentException("Cannot write to stream", "dest");
			}
			this.dest = dest;
			this.ioBuffer = BufferPool.GetBuffer();
			this.model = model;
			this.wireType = WireType.None;
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

		// Token: 0x17000030 RID: 48
		// (get) Token: 0x060001A4 RID: 420 RVA: 0x0000C91D File Offset: 0x0000AB1D
		public SerializationContext Context
		{
			get
			{
				return this.context;
			}
		}

		// Token: 0x060001A5 RID: 421 RVA: 0x0000C925 File Offset: 0x0000AB25
		void IDisposable.Dispose()
		{
			this.Dispose();
		}

		// Token: 0x060001A6 RID: 422 RVA: 0x0000C92D File Offset: 0x0000AB2D
		private void Dispose()
		{
			if (this.dest != null)
			{
				ProtoWriter.Flush(this);
				this.dest = null;
			}
			this.model = null;
			BufferPool.ReleaseBufferToPool(ref this.ioBuffer);
		}

		// Token: 0x060001A7 RID: 423 RVA: 0x0000C956 File Offset: 0x0000AB56
		internal static int GetPosition(ProtoWriter writer)
		{
			return writer.position;
		}

		// Token: 0x060001A8 RID: 424 RVA: 0x0000C960 File Offset: 0x0000AB60
		private static void DemandSpace(int required, ProtoWriter writer)
		{
			if (writer.ioBuffer.Length - writer.ioIndex < required)
			{
				if (writer.flushLock == 0)
				{
					ProtoWriter.Flush(writer);
					if (writer.ioBuffer.Length - writer.ioIndex >= required)
					{
						return;
					}
				}
				BufferPool.ResizeAndFlushLeft(ref writer.ioBuffer, required + writer.ioIndex, 0, writer.ioIndex);
			}
		}

		// Token: 0x060001A9 RID: 425 RVA: 0x0000C9BA File Offset: 0x0000ABBA
		public void Close()
		{
			if (this.depth != 0 || this.flushLock != 0)
			{
				throw new InvalidOperationException("Unable to close stream in an incomplete state");
			}
			this.Dispose();
		}

		// Token: 0x060001AA RID: 426 RVA: 0x0000C9DD File Offset: 0x0000ABDD
		internal void CheckDepthFlushlock()
		{
			if (this.depth != 0 || this.flushLock != 0)
			{
				throw new InvalidOperationException("The writer is in an incomplete state");
			}
		}

		// Token: 0x17000031 RID: 49
		// (get) Token: 0x060001AB RID: 427 RVA: 0x0000C9FA File Offset: 0x0000ABFA
		public TypeModel Model
		{
			get
			{
				return this.model;
			}
		}

		// Token: 0x060001AC RID: 428 RVA: 0x0000CA02 File Offset: 0x0000AC02
		internal static void Flush(ProtoWriter writer)
		{
			if (writer.flushLock == 0 && writer.ioIndex != 0)
			{
				writer.dest.Write(writer.ioBuffer, 0, writer.ioIndex);
				writer.ioIndex = 0;
			}
		}

		// Token: 0x060001AD RID: 429 RVA: 0x0000CA34 File Offset: 0x0000AC34
		private static void WriteUInt32Variant(uint value, ProtoWriter writer)
		{
			ProtoWriter.DemandSpace(5, writer);
			int num = 0;
			do
			{
				byte[] array = writer.ioBuffer;
				int num2 = writer.ioIndex;
				writer.ioIndex = num2 + 1;
				array[num2] = (byte)((value & 127U) | 128U);
				num++;
			}
			while ((value >>= 7) != 0U);
			byte[] array2 = writer.ioBuffer;
			int num3 = writer.ioIndex - 1;
			int num4 = num3;
			array2[num4] &= 127;
			writer.position += num;
		}

		// Token: 0x060001AE RID: 430 RVA: 0x0000CAA3 File Offset: 0x0000ACA3
		internal static uint Zig(int value)
		{
			return (uint)(value << 1 ^ value >> 31);
		}

		// Token: 0x060001AF RID: 431 RVA: 0x0000CAAD File Offset: 0x0000ACAD
		internal static ulong Zig(long value)
		{
			return (ulong)(value << 1 ^ value >> 63);
		}

		// Token: 0x060001B0 RID: 432 RVA: 0x0000CAB8 File Offset: 0x0000ACB8
		private static void WriteUInt64Variant(ulong value, ProtoWriter writer)
		{
			ProtoWriter.DemandSpace(10, writer);
			int num = 0;
			do
			{
				byte[] array = writer.ioBuffer;
				int num2 = writer.ioIndex;
				writer.ioIndex = num2 + 1;
				array[num2] = (byte)((value & 127UL) | 128UL);
				num++;
			}
			while ((value >>= 7) != 0UL);
			byte[] array2 = writer.ioBuffer;
			int num3 = writer.ioIndex - 1;
			int num4 = num3;
			array2[num4] &= 127;
			writer.position += num;
		}

		// Token: 0x060001B1 RID: 433 RVA: 0x0000CB2C File Offset: 0x0000AD2C
		public static void WriteString(string value, ProtoWriter writer)
		{
			if (writer.wireType != WireType.String)
			{
				throw ProtoWriter.CreateException(writer);
			}
			if (value == null)
			{
				throw new ArgumentNullException("value");
			}
			if (value.Length == 0)
			{
				ProtoWriter.WriteUInt32Variant(0U, writer);
				writer.wireType = WireType.None;
				return;
			}
			int byteCount = ProtoWriter.encoding.GetByteCount(value);
			ProtoWriter.WriteUInt32Variant((uint)byteCount, writer);
			ProtoWriter.DemandSpace(byteCount, writer);
			ProtoWriter.IncrementedAndReset(ProtoWriter.encoding.GetBytes(value, 0, value.Length, writer.ioBuffer, writer.ioIndex), writer);
		}

		// Token: 0x060001B2 RID: 434 RVA: 0x0000CBAC File Offset: 0x0000ADAC
		public static void WriteUInt64(ulong value, ProtoWriter writer)
		{
			WireType wireType = writer.wireType;
			if (wireType == WireType.Variant)
			{
				ProtoWriter.WriteUInt64Variant(value, writer);
				writer.wireType = WireType.None;
				return;
			}
			if (wireType == WireType.Fixed64)
			{
				ProtoWriter.WriteInt64((long)value, writer);
				return;
			}
			if (wireType != WireType.Fixed32)
			{
				throw ProtoWriter.CreateException(writer);
			}
			ProtoWriter.WriteUInt32(checked((uint)value), writer);
		}

		// Token: 0x060001B3 RID: 435 RVA: 0x0000CBF4 File Offset: 0x0000ADF4
		public static void WriteInt64(long value, ProtoWriter writer)
		{
			WireType wireType = writer.wireType;
			if (wireType != WireType.Variant)
			{
				if (wireType == WireType.Fixed64)
				{
					ProtoWriter.DemandSpace(8, writer);
					byte[] array = writer.ioBuffer;
					int num = writer.ioIndex;
					array[num] = (byte)value;
					array[num + 1] = (byte)(value >> 8);
					array[num + 2] = (byte)(value >> 16);
					array[num + 3] = (byte)(value >> 24);
					array[num + 4] = (byte)(value >> 32);
					array[num + 5] = (byte)(value >> 40);
					array[num + 6] = (byte)(value >> 48);
					array[num + 7] = (byte)(value >> 56);
					ProtoWriter.IncrementedAndReset(8, writer);
					return;
				}
				if (wireType == WireType.Fixed32)
				{
					ProtoWriter.WriteInt32(checked((int)value), writer);
					return;
				}
				if (wireType != WireType.SignedVariant)
				{
					throw ProtoWriter.CreateException(writer);
				}
				ProtoWriter.WriteUInt64Variant(ProtoWriter.Zig(value), writer);
				writer.wireType = WireType.None;
				return;
			}
			else
			{
				if (value >= 0L)
				{
					ProtoWriter.WriteUInt64Variant((ulong)value, writer);
					writer.wireType = WireType.None;
					return;
				}
				ProtoWriter.DemandSpace(10, writer);
				byte[] array2 = writer.ioBuffer;
				int num2 = writer.ioIndex;
				array2[num2] = (byte)(value | 128L);
				array2[num2 + 1] = (byte)((int)(value >> 7) | 128);
				array2[num2 + 2] = (byte)((int)(value >> 14) | 128);
				array2[num2 + 3] = (byte)((int)(value >> 21) | 128);
				array2[num2 + 4] = (byte)((int)(value >> 28) | 128);
				array2[num2 + 5] = (byte)((int)(value >> 35) | 128);
				array2[num2 + 6] = (byte)((int)(value >> 42) | 128);
				array2[num2 + 7] = (byte)((int)(value >> 49) | 128);
				array2[num2 + 8] = (byte)((int)(value >> 56) | 128);
				array2[num2 + 9] = 1;
				ProtoWriter.IncrementedAndReset(10, writer);
				return;
			}
		}

		// Token: 0x060001B4 RID: 436 RVA: 0x0000CD74 File Offset: 0x0000AF74
		public static void WriteUInt32(uint value, ProtoWriter writer)
		{
			WireType wireType = writer.wireType;
			if (wireType == WireType.Variant)
			{
				ProtoWriter.WriteUInt32Variant(value, writer);
				writer.wireType = WireType.None;
				return;
			}
			if (wireType == WireType.Fixed64)
			{
				ProtoWriter.WriteInt64((long)((ulong)value), writer);
				return;
			}
			if (wireType == WireType.Fixed32)
			{
				ProtoWriter.WriteInt32((int)value, writer);
				return;
			}
			throw ProtoWriter.CreateException(writer);
		}

		// Token: 0x060001B5 RID: 437 RVA: 0x0000CDBB File Offset: 0x0000AFBB
		public static void WriteInt16(short value, ProtoWriter writer)
		{
			ProtoWriter.WriteInt32((int)value, writer);
		}

		// Token: 0x060001B6 RID: 438 RVA: 0x0000CDC4 File Offset: 0x0000AFC4
		public static void WriteUInt16(ushort value, ProtoWriter writer)
		{
			ProtoWriter.WriteUInt32((uint)value, writer);
		}

		// Token: 0x060001B7 RID: 439 RVA: 0x0000CDC4 File Offset: 0x0000AFC4
		public static void WriteByte(byte value, ProtoWriter writer)
		{
			ProtoWriter.WriteUInt32((uint)value, writer);
		}

		// Token: 0x060001B8 RID: 440 RVA: 0x0000CDBB File Offset: 0x0000AFBB
		public static void WriteSByte(sbyte value, ProtoWriter writer)
		{
			ProtoWriter.WriteInt32((int)value, writer);
		}

		// Token: 0x060001B9 RID: 441 RVA: 0x0000CDCD File Offset: 0x0000AFCD
		private static void WriteInt32ToBuffer(int value, byte[] buffer, int index)
		{
			buffer[index] = (byte)value;
			buffer[index + 1] = (byte)(value >> 8);
			buffer[index + 2] = (byte)(value >> 16);
			buffer[index + 3] = (byte)(value >> 24);
		}

		// Token: 0x060001BA RID: 442 RVA: 0x0000CDF4 File Offset: 0x0000AFF4
		public static void WriteInt32(int value, ProtoWriter writer)
		{
			WireType wireType = writer.wireType;
			if (wireType != WireType.Variant)
			{
				if (wireType == WireType.Fixed64)
				{
					ProtoWriter.DemandSpace(8, writer);
					byte[] array = writer.ioBuffer;
					int num = writer.ioIndex;
					array[num] = (byte)value;
					array[num + 1] = (byte)(value >> 8);
					array[num + 2] = (byte)(value >> 16);
					array[num + 3] = (byte)(value >> 24);
					array[num + 4] = (array[num + 5] = (array[num + 6] = (array[num + 7] = 0)));
					ProtoWriter.IncrementedAndReset(8, writer);
					return;
				}
				if (wireType == WireType.Fixed32)
				{
					ProtoWriter.DemandSpace(4, writer);
					ProtoWriter.WriteInt32ToBuffer(value, writer.ioBuffer, writer.ioIndex);
					ProtoWriter.IncrementedAndReset(4, writer);
					return;
				}
				if (wireType != WireType.SignedVariant)
				{
					throw ProtoWriter.CreateException(writer);
				}
				ProtoWriter.WriteUInt32Variant(ProtoWriter.Zig(value), writer);
				writer.wireType = WireType.None;
				return;
			}
			else
			{
				if (value >= 0)
				{
					ProtoWriter.WriteUInt32Variant((uint)value, writer);
					writer.wireType = WireType.None;
					return;
				}
				ProtoWriter.DemandSpace(10, writer);
				byte[] array2 = writer.ioBuffer;
				int num2 = writer.ioIndex;
				array2[num2] = (byte)(value | 128);
				array2[num2 + 1] = (byte)(value >> 7 | 128);
				array2[num2 + 2] = (byte)(value >> 14 | 128);
				array2[num2 + 3] = (byte)(value >> 21 | 128);
				array2[num2 + 4] = (byte)(value >> 28 | 128);
				array2[num2 + 5] = (array2[num2 + 6] = (array2[num2 + 7] = (array2[num2 + 8] = byte.MaxValue)));
				array2[num2 + 9] = 1;
				ProtoWriter.IncrementedAndReset(10, writer);
				return;
			}
		}

		// Token: 0x060001BB RID: 443 RVA: 0x0000CF70 File Offset: 0x0000B170
		public unsafe static void WriteDouble(double value, ProtoWriter writer)
		{
			WireType wireType = writer.wireType;
			if (wireType == WireType.Fixed64)
			{
				ProtoWriter.WriteInt64(*(long*)(&value), writer);
				return;
			}
			if (wireType != WireType.Fixed32)
			{
				throw ProtoWriter.CreateException(writer);
			}
			float value2 = (float)value;
			if (Helpers.IsInfinity(value2) && !Helpers.IsInfinity(value))
			{
				throw new OverflowException();
			}
			ProtoWriter.WriteSingle(value2, writer);
		}

		// Token: 0x060001BC RID: 444 RVA: 0x0000CFBC File Offset: 0x0000B1BC
		public unsafe static void WriteSingle(float value, ProtoWriter writer)
		{
			WireType wireType = writer.wireType;
			if (wireType == WireType.Fixed64)
			{
				ProtoWriter.WriteDouble((double)value, writer);
				return;
			}
			if (wireType == WireType.Fixed32)
			{
				ProtoWriter.WriteInt32(*(int*)(&value), writer);
				return;
			}
			throw ProtoWriter.CreateException(writer);
		}

		// Token: 0x060001BD RID: 445 RVA: 0x0000CFF4 File Offset: 0x0000B1F4
		public static void ThrowEnumException(ProtoWriter writer, object enumValue)
		{
			string str = (enumValue == null) ? "<null>" : (enumValue.GetType().FullName + "." + enumValue.ToString());
			throw new ProtoException("No wire-value is mapped to the enum " + str);
		}

		// Token: 0x060001BE RID: 446 RVA: 0x0000D037 File Offset: 0x0000B237
		internal static Exception CreateException(ProtoWriter writer)
		{
			return new ProtoException(string.Concat(new object[]
			{
				"Invalid serialization operation with wire-type ",
				writer.wireType,
				" at position ",
				writer.position
			}));
		}

		// Token: 0x060001BF RID: 447 RVA: 0x0000D075 File Offset: 0x0000B275
		public static void WriteBoolean(bool value, ProtoWriter writer)
		{
			ProtoWriter.WriteUInt32(value ? 1U : 0U, writer);
		}

		// Token: 0x060001C0 RID: 448 RVA: 0x0000D084 File Offset: 0x0000B284
		public static void AppendExtensionData(IExtensible instance, ProtoWriter writer)
		{
			if (instance == null)
			{
				throw new ArgumentNullException("instance");
			}
			if (writer.wireType != WireType.None)
			{
				throw ProtoWriter.CreateException(writer);
			}
			IExtension extensionObject = instance.GetExtensionObject(false);
			if (extensionObject != null)
			{
				Stream stream = extensionObject.BeginQuery();
				try
				{
					ProtoWriter.CopyRawFromStream(stream, writer);
				}
				finally
				{
					extensionObject.EndQuery(stream);
				}
			}
		}

		// Token: 0x060001C1 RID: 449 RVA: 0x0000D0E4 File Offset: 0x0000B2E4
		public static void SetPackedField(int fieldNumber, ProtoWriter writer)
		{
			if (fieldNumber <= 0)
			{
				throw new ArgumentOutOfRangeException("fieldNumber");
			}
			writer.packedFieldNumber = fieldNumber;
		}

		// Token: 0x060001C2 RID: 450 RVA: 0x0000D0FC File Offset: 0x0000B2FC
		internal string SerializeType(Type type)
		{
			return TypeModel.SerializeType(this.model, type);
		}

		// Token: 0x060001C3 RID: 451 RVA: 0x0000D10A File Offset: 0x0000B30A
		public void SetRootObject(object value)
		{
			this.NetCache.SetKeyedObject(0, value);
		}

		// Token: 0x060001C4 RID: 452 RVA: 0x0000D119 File Offset: 0x0000B319
		public static void WriteType(Type value, ProtoWriter writer)
		{
			ProtoWriter.WriteString(writer.SerializeType(value), writer);
		}

		// Token: 0x040001E7 RID: 487
		private const int RecursionCheckDepth = 25;

		// Token: 0x040001E8 RID: 488
		private Stream dest;

		// Token: 0x040001E9 RID: 489
		private TypeModel model;

		// Token: 0x040001EA RID: 490
		private readonly NetObjectCache netCache = new NetObjectCache();

		// Token: 0x040001EB RID: 491
		private int fieldNumber;

		// Token: 0x040001EC RID: 492
		private int flushLock;

		// Token: 0x040001ED RID: 493
		private WireType wireType;

		// Token: 0x040001EE RID: 494
		private int depth;

		// Token: 0x040001EF RID: 495
		private MutableList recursionStack;

		// Token: 0x040001F0 RID: 496
		private readonly SerializationContext context;

		// Token: 0x040001F1 RID: 497
		private byte[] ioBuffer;

		// Token: 0x040001F2 RID: 498
		private int ioIndex;

		// Token: 0x040001F3 RID: 499
		private int position;

		// Token: 0x040001F4 RID: 500
		private static readonly UTF8Encoding encoding = new UTF8Encoding();

		// Token: 0x040001F5 RID: 501
		private int packedFieldNumber;
	}
}
