using System;

namespace ProtoBuf
{
	// Token: 0x02000016 RID: 22
	public static class BclHelpers
	{
		// Token: 0x060000AF RID: 175 RVA: 0x0000904F File Offset: 0x0000724F
		public static object GetUninitializedObject(Type type)
		{
			throw new NotSupportedException("Constructor-skipping is not supported on this platform");
		}

		// Token: 0x060000B0 RID: 176 RVA: 0x0000905C File Offset: 0x0000725C
		public static void WriteTimeSpan(TimeSpan timeSpan, ProtoWriter dest)
		{
			WireType wireType = dest.WireType;
			if (wireType == WireType.Fixed64)
			{
				ProtoWriter.WriteInt64(timeSpan.Ticks, dest);
				return;
			}
			if (wireType - WireType.String > 1)
			{
				throw new ProtoException("Unexpected wire-type: " + dest.WireType.ToString());
			}
			long num = timeSpan.Ticks;
			TimeSpanScale timeSpanScale;
			if (timeSpan == TimeSpan.MaxValue)
			{
				num = 1L;
				timeSpanScale = TimeSpanScale.MinMax;
			}
			else if (timeSpan == TimeSpan.MinValue)
			{
				num = -1L;
				timeSpanScale = TimeSpanScale.MinMax;
			}
			else if (num % 864000000000L == 0L)
			{
				timeSpanScale = TimeSpanScale.Days;
				num /= 864000000000L;
			}
			else if (num % 36000000000L == 0L)
			{
				timeSpanScale = TimeSpanScale.Hours;
				num /= 36000000000L;
			}
			else if (num % 600000000L == 0L)
			{
				timeSpanScale = TimeSpanScale.Minutes;
				num /= 600000000L;
			}
			else if (num % 10000000L == 0L)
			{
				timeSpanScale = TimeSpanScale.Seconds;
				num /= 10000000L;
			}
			else if (num % 10000L == 0L)
			{
				timeSpanScale = TimeSpanScale.Milliseconds;
				num /= 10000L;
			}
			else
			{
				timeSpanScale = TimeSpanScale.Ticks;
			}
			SubItemToken token = ProtoWriter.StartSubItem(null, dest);
			if (num != 0L)
			{
				ProtoWriter.WriteFieldHeader(1, WireType.SignedVariant, dest);
				ProtoWriter.WriteInt64(num, dest);
			}
			if (timeSpanScale != TimeSpanScale.Days)
			{
				ProtoWriter.WriteFieldHeader(2, WireType.Variant, dest);
				ProtoWriter.WriteInt32((int)timeSpanScale, dest);
			}
			ProtoWriter.EndSubItem(token, dest);
		}

		// Token: 0x060000B1 RID: 177 RVA: 0x0000919C File Offset: 0x0000739C
		public static TimeSpan ReadTimeSpan(ProtoReader source)
		{
			long num = BclHelpers.ReadTimeSpanTicks(source);
			if (num == -9223372036854775808L)
			{
				return TimeSpan.MinValue;
			}
			if (num == 9223372036854775807L)
			{
				return TimeSpan.MaxValue;
			}
			return TimeSpan.FromTicks(num);
		}

		// Token: 0x060000B2 RID: 178 RVA: 0x000091DC File Offset: 0x000073DC
		public static DateTime ReadDateTime(ProtoReader source)
		{
			long num = BclHelpers.ReadTimeSpanTicks(source);
			if (num == -9223372036854775808L)
			{
				return DateTime.MinValue;
			}
			if (num == 9223372036854775807L)
			{
				return DateTime.MaxValue;
			}
			return BclHelpers.EpochOrigin.AddTicks(num);
		}

		// Token: 0x060000B3 RID: 179 RVA: 0x00009224 File Offset: 0x00007424
		public static void WriteDateTime(DateTime value, ProtoWriter dest)
		{
			WireType wireType = dest.WireType;
			TimeSpan timeSpan;
			if (wireType - WireType.String <= 1)
			{
				if (value == DateTime.MaxValue)
				{
					timeSpan = TimeSpan.MaxValue;
				}
				else if (value == DateTime.MinValue)
				{
					timeSpan = TimeSpan.MinValue;
				}
				else
				{
					timeSpan = value - BclHelpers.EpochOrigin;
				}
			}
			else
			{
				timeSpan = value - BclHelpers.EpochOrigin;
			}
			BclHelpers.WriteTimeSpan(timeSpan, dest);
		}

		// Token: 0x060000B4 RID: 180 RVA: 0x0000928C File Offset: 0x0000748C
		private static long ReadTimeSpanTicks(ProtoReader source)
		{
			WireType wireType = source.WireType;
			if (wireType == WireType.Fixed64)
			{
				return source.ReadInt64();
			}
			if (wireType - WireType.String > 1)
			{
				throw new ProtoException("Unexpected wire-type: " + source.WireType.ToString());
			}
			SubItemToken token = ProtoReader.StartSubItem(source);
			TimeSpanScale timeSpanScale = TimeSpanScale.Days;
			long num = 0L;
			int num2;
			while ((num2 = source.ReadFieldHeader()) > 0)
			{
				if (num2 != 1)
				{
					if (num2 != 2)
					{
						source.SkipField();
					}
					else
					{
						timeSpanScale = (TimeSpanScale)source.ReadInt32();
					}
				}
				else
				{
					source.Assert(WireType.SignedVariant);
					num = source.ReadInt64();
				}
			}
			ProtoReader.EndSubItem(token, source);
			TimeSpanScale timeSpanScale2 = timeSpanScale;
			switch (timeSpanScale2)
			{
			case TimeSpanScale.Days:
				return num * 864000000000L;
			case TimeSpanScale.Hours:
				return num * 36000000000L;
			case TimeSpanScale.Minutes:
				return num * 600000000L;
			case TimeSpanScale.Seconds:
				return num * 10000000L;
			case TimeSpanScale.Milliseconds:
				return num * 10000L;
			case TimeSpanScale.Ticks:
				return num;
			default:
			{
				if (timeSpanScale2 != TimeSpanScale.MinMax)
				{
					throw new ProtoException("Unknown timescale: " + timeSpanScale.ToString());
				}
				long num3 = num;
				if (num3 <= 1L && num3 >= -1L)
				{
					int num4 = (int)(num3 - -1L);
					if (num4 == 0)
					{
						return long.MinValue;
					}
					if (num4 == 2)
					{
						return long.MaxValue;
					}
				}
				throw new ProtoException("Unknown min/max value: " + num.ToString());
			}
			}
		}

		// Token: 0x060000B5 RID: 181 RVA: 0x000093F0 File Offset: 0x000075F0
		public static decimal ReadDecimal(ProtoReader reader)
		{
			ulong num = 0UL;
			uint num2 = 0U;
			uint num3 = 0U;
			SubItemToken token = ProtoReader.StartSubItem(reader);
			int num4;
			while ((num4 = reader.ReadFieldHeader()) > 0)
			{
				switch (num4)
				{
				case 1:
					num = reader.ReadUInt64();
					break;
				case 2:
					num2 = reader.ReadUInt32();
					break;
				case 3:
					num3 = reader.ReadUInt32();
					break;
				default:
					reader.SkipField();
					break;
				}
			}
			ProtoReader.EndSubItem(token, reader);
			if (num == 0UL && num2 == 0U)
			{
				return 0m;
			}
			int lo = (int)(num - 1UL);
			int mid = (int)(num >> 31);
			int hi = (int)num2;
			bool isNegative = (num3 & 1U) == 1U;
			byte scale = (byte)((num3 & 510U) >> 1);
			return new decimal(lo, mid, hi, isNegative, scale);
		}

		// Token: 0x060000B6 RID: 182 RVA: 0x00009498 File Offset: 0x00007698
		public static void WriteDecimal(decimal value, ProtoWriter writer)
		{
			int[] bits = decimal.GetBits(value);
			ulong num = (ulong)((ulong)((long)bits[1]) << 32);
			ulong num2 = (ulong)((long)bits[0] & -1L);
			ulong num3 = num | num2;
			uint num4 = (uint)bits[2];
			uint num5 = (uint)((bits[3] >> 15 & 510) | (bits[3] >> 31 & 1));
			SubItemToken token = ProtoWriter.StartSubItem(null, writer);
			if (num3 != 0UL)
			{
				ProtoWriter.WriteFieldHeader(1, WireType.Variant, writer);
				ProtoWriter.WriteUInt64(num3, writer);
			}
			if (num4 != 0U)
			{
				ProtoWriter.WriteFieldHeader(2, WireType.Variant, writer);
				ProtoWriter.WriteUInt32(num4, writer);
			}
			if (num5 != 0U)
			{
				ProtoWriter.WriteFieldHeader(3, WireType.Variant, writer);
				ProtoWriter.WriteUInt32(num5, writer);
			}
			ProtoWriter.EndSubItem(token, writer);
		}

		// Token: 0x060000B7 RID: 183 RVA: 0x00009520 File Offset: 0x00007720
		public static void WriteGuid(Guid value, ProtoWriter dest)
		{
			byte[] data = value.ToByteArray();
			SubItemToken token = ProtoWriter.StartSubItem(null, dest);
			if (value != Guid.Empty)
			{
				ProtoWriter.WriteFieldHeader(1, WireType.Fixed64, dest);
				ProtoWriter.WriteBytes(data, 0, 8, dest);
				ProtoWriter.WriteFieldHeader(2, WireType.Fixed64, dest);
				ProtoWriter.WriteBytes(data, 8, 8, dest);
			}
			ProtoWriter.EndSubItem(token, dest);
		}

		// Token: 0x060000B8 RID: 184 RVA: 0x00009574 File Offset: 0x00007774
		public static Guid ReadGuid(ProtoReader source)
		{
			ulong num = 0UL;
			ulong num2 = 0UL;
			SubItemToken token = ProtoReader.StartSubItem(source);
			int num3;
			while ((num3 = source.ReadFieldHeader()) > 0)
			{
				if (num3 != 1)
				{
					if (num3 != 2)
					{
						source.SkipField();
					}
					else
					{
						num2 = source.ReadUInt64();
					}
				}
				else
				{
					num = source.ReadUInt64();
				}
			}
			ProtoReader.EndSubItem(token, source);
			if (num == 0UL && num2 == 0UL)
			{
				return Guid.Empty;
			}
			uint num4 = (uint)(num >> 32);
			int a = (int)((uint)num);
			uint num5 = (uint)(num2 >> 32);
			uint num6 = (uint)num2;
			return new Guid(a, (short)num4, (short)(num4 >> 16), (byte)num6, (byte)(num6 >> 8), (byte)(num6 >> 16), (byte)(num6 >> 24), (byte)num5, (byte)(num5 >> 8), (byte)(num5 >> 16), (byte)(num5 >> 24));
		}

		// Token: 0x060000B9 RID: 185 RVA: 0x0000961C File Offset: 0x0000781C
		public static object ReadNetObject(object value, ProtoReader source, int key, Type type, BclHelpers.NetObjectOptions options)
		{
			SubItemToken token = ProtoReader.StartSubItem(source);
			int num = -1;
			int num2 = -1;
			int num3;
			while ((num3 = source.ReadFieldHeader()) > 0)
			{
				switch (num3)
				{
				case 1:
				{
					int key2 = source.ReadInt32();
					value = source.NetCache.GetKeyedObject(key2);
					continue;
				}
				case 2:
					num = source.ReadInt32();
					continue;
				case 3:
				{
					int key3 = source.ReadInt32();
					type = (Type)source.NetCache.GetKeyedObject(key3);
					key = source.GetTypeKey(ref type);
					continue;
				}
				case 4:
					num2 = source.ReadInt32();
					continue;
				case 8:
				{
					string text = source.ReadString();
					type = source.DeserializeType(text);
					if (type == null)
					{
						throw new ProtoException("Unable to resolve type: " + text + " (you can use the TypeModel.DynamicTypeFormatting event to provide a custom mapping)");
					}
					if (type == typeof(string))
					{
						key = -1;
						continue;
					}
					key = source.GetTypeKey(ref type);
					if (key < 0)
					{
						throw new InvalidOperationException("Dynamic type is not a contract-type: " + type.Name);
					}
					continue;
				}
				case 10:
				{
					bool flag = type == typeof(string);
					bool flag2 = value == null;
					bool flag3 = flag2 && flag;
					if (num >= 0 && !flag3)
					{
						if (value == null)
						{
							source.TrapNextObject(num);
						}
						else
						{
							source.NetCache.SetKeyedObject(num, value);
						}
						if (num2 >= 0)
						{
							source.NetCache.SetKeyedObject(num2, type);
						}
					}
					object obj = value;
					if (flag)
					{
						value = source.ReadString();
					}
					else
					{
						value = ProtoReader.ReadTypedObject(obj, key, source, type);
					}
					if (num >= 0)
					{
						if (flag2 && !flag3)
						{
							obj = source.NetCache.GetKeyedObject(num);
						}
						if (flag3)
						{
							source.NetCache.SetKeyedObject(num, value);
							if (num2 >= 0)
							{
								source.NetCache.SetKeyedObject(num2, type);
							}
						}
					}
					if (num >= 0 && !flag3 && obj != value)
					{
						throw new ProtoException("A reference-tracked object changed reference during deserialization");
					}
					if (num < 0 && num2 >= 0)
					{
						source.NetCache.SetKeyedObject(num2, type);
						continue;
					}
					continue;
				}
				}
				source.SkipField();
			}
			if (num >= 0 && (options & BclHelpers.NetObjectOptions.AsReference) == BclHelpers.NetObjectOptions.None)
			{
				throw new ProtoException("Object key in input stream, but reference-tracking was not expected");
			}
			ProtoReader.EndSubItem(token, source);
			return value;
		}

		// Token: 0x060000BA RID: 186 RVA: 0x0000983C File Offset: 0x00007A3C
		public static void WriteNetObject(object value, ProtoWriter dest, int key, BclHelpers.NetObjectOptions options)
		{
			bool flag = (options & BclHelpers.NetObjectOptions.DynamicType) > BclHelpers.NetObjectOptions.None;
			bool flag2 = (options & BclHelpers.NetObjectOptions.AsReference) > BclHelpers.NetObjectOptions.None;
			WireType wireType = dest.WireType;
			SubItemToken token = ProtoWriter.StartSubItem(null, dest);
			bool flag3 = true;
			if (flag2)
			{
				bool flag4;
				int value2 = dest.NetCache.AddObjectKey(value, out flag4);
				ProtoWriter.WriteFieldHeader(flag4 ? 1 : 2, WireType.Variant, dest);
				ProtoWriter.WriteInt32(value2, dest);
				if (flag4)
				{
					flag3 = false;
				}
			}
			if (flag3)
			{
				if (flag)
				{
					Type type = value.GetType();
					if (!(value is string))
					{
						key = dest.GetTypeKey(ref type);
						if (key < 0)
						{
							throw new InvalidOperationException("Dynamic type is not a contract-type: " + type.Name);
						}
					}
					bool flag5;
					int value3 = dest.NetCache.AddObjectKey(type, out flag5);
					ProtoWriter.WriteFieldHeader(flag5 ? 3 : 4, WireType.Variant, dest);
					ProtoWriter.WriteInt32(value3, dest);
					if (!flag5)
					{
						ProtoWriter.WriteFieldHeader(8, WireType.String, dest);
						ProtoWriter.WriteString(dest.SerializeType(type), dest);
					}
				}
				ProtoWriter.WriteFieldHeader(10, wireType, dest);
				if (value is string)
				{
					ProtoWriter.WriteString((string)value, dest);
				}
				else
				{
					ProtoWriter.WriteObject(value, key, dest);
				}
			}
			ProtoWriter.EndSubItem(token, dest);
		}

		// Token: 0x0400016B RID: 363
		private const int FieldTimeSpanValue = 1;

		// Token: 0x0400016C RID: 364
		private const int FieldTimeSpanScale = 2;

		// Token: 0x0400016D RID: 365
		private const int FieldDecimalLow = 1;

		// Token: 0x0400016E RID: 366
		private const int FieldDecimalHigh = 2;

		// Token: 0x0400016F RID: 367
		private const int FieldDecimalSignScale = 3;

		// Token: 0x04000170 RID: 368
		private const int FieldGuidLow = 1;

		// Token: 0x04000171 RID: 369
		private const int FieldGuidHigh = 2;

		// Token: 0x04000172 RID: 370
		private const int FieldExistingObjectKey = 1;

		// Token: 0x04000173 RID: 371
		private const int FieldNewObjectKey = 2;

		// Token: 0x04000174 RID: 372
		private const int FieldExistingTypeKey = 3;

		// Token: 0x04000175 RID: 373
		private const int FieldNewTypeKey = 4;

		// Token: 0x04000176 RID: 374
		private const int FieldTypeName = 8;

		// Token: 0x04000177 RID: 375
		private const int FieldObject = 10;

		// Token: 0x04000178 RID: 376
		internal static readonly DateTime EpochOrigin = new DateTime(1970, 1, 1, 0, 0, 0, 0);

		// Token: 0x0200014E RID: 334
		[Flags]
		public enum NetObjectOptions : byte
		{
			// Token: 0x04000DEB RID: 3563
			None = 0,
			// Token: 0x04000DEC RID: 3564
			AsReference = 1,
			// Token: 0x04000DED RID: 3565
			DynamicType = 2,
			// Token: 0x04000DEE RID: 3566
			UseConstructor = 4
		}
	}
}
