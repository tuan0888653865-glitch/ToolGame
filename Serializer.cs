using System;
using System.Collections.Generic;
using System.IO;
using ProtoBuf.Meta;

namespace ProtoBuf
{
	// Token: 0x02000034 RID: 52
	public static class Serializer
	{
		// Token: 0x060001CD RID: 461 RVA: 0x0000D18F File Offset: 0x0000B38F
		public static string GetProto<T>()
		{
			return RuntimeTypeModel.Default.GetSchema(RuntimeTypeModel.Default.MapType(typeof(T)));
		}

		// Token: 0x060001CE RID: 462 RVA: 0x0000D1AF File Offset: 0x0000B3AF
		public static T DeepClone<T>(T instance)
		{
			if (instance != null)
			{
				return (T)((object)RuntimeTypeModel.Default.DeepClone(instance));
			}
			return instance;
		}

		// Token: 0x060001CF RID: 463 RVA: 0x0000D1D0 File Offset: 0x0000B3D0
		public static T Merge<T>(Stream source, T instance)
		{
			return (T)((object)RuntimeTypeModel.Default.Deserialize(source, instance, typeof(T)));
		}

		// Token: 0x060001D0 RID: 464 RVA: 0x0000D1F2 File Offset: 0x0000B3F2
		public static T Deserialize<T>(Stream source)
		{
			return (T)((object)RuntimeTypeModel.Default.Deserialize(source, null, typeof(T)));
		}

		// Token: 0x060001D1 RID: 465 RVA: 0x0000D20F File Offset: 0x0000B40F
		public static void Serialize<T>(Stream destination, T instance)
		{
			if (instance != null)
			{
				RuntimeTypeModel.Default.Serialize(destination, instance);
			}
		}

		// Token: 0x060001D2 RID: 466 RVA: 0x0000D22C File Offset: 0x0000B42C
		public static TTo ChangeType<TFrom, TTo>(TFrom instance)
		{
			TTo result;
			using (MemoryStream memoryStream = new MemoryStream())
			{
				Serializer.Serialize<TFrom>(memoryStream, instance);
				memoryStream.Position = 0L;
				result = Serializer.Deserialize<TTo>(memoryStream);
			}
			return result;
		}

		// Token: 0x060001D3 RID: 467 RVA: 0x00006740 File Offset: 0x00004940
		public static void PrepareSerializer<T>()
		{
		}

		// Token: 0x060001D4 RID: 468 RVA: 0x0000D274 File Offset: 0x0000B474
		public static IEnumerable<T> DeserializeItems<T>(Stream source, PrefixStyle style, int fieldNumber)
		{
			return RuntimeTypeModel.Default.DeserializeItems<T>(source, style, fieldNumber);
		}

		// Token: 0x060001D5 RID: 469 RVA: 0x0000D283 File Offset: 0x0000B483
		public static T DeserializeWithLengthPrefix<T>(Stream source, PrefixStyle style)
		{
			return Serializer.DeserializeWithLengthPrefix<T>(source, style, 0);
		}

		// Token: 0x060001D6 RID: 470 RVA: 0x0000D290 File Offset: 0x0000B490
		public static T DeserializeWithLengthPrefix<T>(Stream source, PrefixStyle style, int fieldNumber)
		{
			RuntimeTypeModel @default = RuntimeTypeModel.Default;
			return (T)((object)@default.DeserializeWithLengthPrefix(source, null, @default.MapType(typeof(T)), style, fieldNumber));
		}

		// Token: 0x060001D7 RID: 471 RVA: 0x0000D2C4 File Offset: 0x0000B4C4
		public static T MergeWithLengthPrefix<T>(Stream source, T instance, PrefixStyle style)
		{
			RuntimeTypeModel @default = RuntimeTypeModel.Default;
			return (T)((object)@default.DeserializeWithLengthPrefix(source, instance, @default.MapType(typeof(T)), style, 0));
		}

		// Token: 0x060001D8 RID: 472 RVA: 0x0000D2FB File Offset: 0x0000B4FB
		public static void SerializeWithLengthPrefix<T>(Stream destination, T instance, PrefixStyle style)
		{
			Serializer.SerializeWithLengthPrefix<T>(destination, instance, style, 0);
		}

		// Token: 0x060001D9 RID: 473 RVA: 0x0000D308 File Offset: 0x0000B508
		public static void SerializeWithLengthPrefix<T>(Stream destination, T instance, PrefixStyle style, int fieldNumber)
		{
			RuntimeTypeModel @default = RuntimeTypeModel.Default;
			@default.SerializeWithLengthPrefix(destination, instance, @default.MapType(typeof(T)), style, fieldNumber);
		}

		// Token: 0x060001DA RID: 474 RVA: 0x0000D33C File Offset: 0x0000B53C
		public static bool TryReadLengthPrefix(Stream source, PrefixStyle style, out int length)
		{
			int num;
			int num2;
			length = ProtoReader.ReadLengthPrefix(source, false, style, out num, out num2);
			return num2 > 0;
		}

		// Token: 0x060001DB RID: 475 RVA: 0x0000D35C File Offset: 0x0000B55C
		public static bool TryReadLengthPrefix(byte[] buffer, int index, int count, PrefixStyle style, out int length)
		{
			bool result;
			using (Stream stream = new MemoryStream(buffer, index, count))
			{
				result = Serializer.TryReadLengthPrefix(stream, style, out length);
			}
			return result;
		}

		// Token: 0x060001DC RID: 476 RVA: 0x0000D39C File Offset: 0x0000B59C
		public static void FlushPool()
		{
			BufferPool.Flush();
		}

		// Token: 0x040001F9 RID: 505
		private const string ProtoBinaryField = "proto";

		// Token: 0x040001FA RID: 506
		public const int ListItemTag = 1;

		// Token: 0x02000152 RID: 338
		public static class NonGeneric
		{
			// Token: 0x06001134 RID: 4404 RVA: 0x0007742A File Offset: 0x0007562A
			public static object DeepClone(object instance)
			{
				if (instance != null)
				{
					return RuntimeTypeModel.Default.DeepClone(instance);
				}
				return null;
			}

			// Token: 0x06001135 RID: 4405 RVA: 0x0007743C File Offset: 0x0007563C
			public static void Serialize(Stream dest, object instance)
			{
				if (instance != null)
				{
					RuntimeTypeModel.Default.Serialize(dest, instance);
				}
			}

			// Token: 0x06001136 RID: 4406 RVA: 0x0007744D File Offset: 0x0007564D
			public static object Deserialize(Type type, Stream source)
			{
				return RuntimeTypeModel.Default.Deserialize(source, null, type);
			}

			// Token: 0x06001137 RID: 4407 RVA: 0x0007745C File Offset: 0x0007565C
			public static object Merge(Stream source, object instance)
			{
				if (instance == null)
				{
					throw new ArgumentNullException("instance");
				}
				return RuntimeTypeModel.Default.Deserialize(source, instance, instance.GetType(), null);
			}

			// Token: 0x06001138 RID: 4408 RVA: 0x00077480 File Offset: 0x00075680
			public static void SerializeWithLengthPrefix(Stream destination, object instance, PrefixStyle style, int fieldNumber)
			{
				RuntimeTypeModel @default = RuntimeTypeModel.Default;
				@default.SerializeWithLengthPrefix(destination, instance, @default.MapType(instance.GetType()), style, fieldNumber);
			}

			// Token: 0x06001139 RID: 4409 RVA: 0x000774A9 File Offset: 0x000756A9
			public static bool TryDeserializeWithLengthPrefix(Stream source, PrefixStyle style, Serializer.TypeResolver resolver, out object value)
			{
				value = RuntimeTypeModel.Default.DeserializeWithLengthPrefix(source, null, null, style, 0, resolver);
				return value != null;
			}

			// Token: 0x0600113A RID: 4410 RVA: 0x000774C2 File Offset: 0x000756C2
			public static bool CanSerialize(Type type)
			{
				return RuntimeTypeModel.Default.IsDefined(type);
			}
		}

		// Token: 0x02000153 RID: 339
		public static class GlobalOptions
		{
			// Token: 0x170003FA RID: 1018
			// (get) Token: 0x0600113B RID: 4411 RVA: 0x000774CF File Offset: 0x000756CF
			// (set) Token: 0x0600113C RID: 4412 RVA: 0x000774DB File Offset: 0x000756DB
			[Obsolete("Please use RuntimeTypeModel.Default.InferTagFromNameDefault instead (or on a per-model basis)", false)]
			public static bool InferTagFromName
			{
				get
				{
					return RuntimeTypeModel.Default.InferTagFromNameDefault;
				}
				set
				{
					RuntimeTypeModel.Default.InferTagFromNameDefault = value;
				}
			}
		}

		// Token: 0x02000154 RID: 340
		// (Invoke) Token: 0x0600113E RID: 4414
		public delegate Type TypeResolver(int fieldNumber);
	}
}
