using System;
using System.Diagnostics;
using System.Reflection;
using System.Text;

namespace ProtoBuf
{
	// Token: 0x0200001D RID: 29
	internal class Helpers
	{
		// Token: 0x060000DD RID: 221 RVA: 0x000020C5 File Offset: 0x000002C5
		private Helpers()
		{
		}

		// Token: 0x060000DE RID: 222 RVA: 0x00009DEB File Offset: 0x00007FEB
		public static StringBuilder AppendLine(StringBuilder builder)
		{
			return builder.AppendLine();
		}

		// Token: 0x060000DF RID: 223 RVA: 0x00009DF3 File Offset: 0x00007FF3
		public static bool IsNullOrEmpty(string value)
		{
			return value == null || value.Length == 0;
		}

		// Token: 0x060000E0 RID: 224 RVA: 0x00009E04 File Offset: 0x00008004
		[Conditional("DEBUG")]
		public static void DebugWriteLine(string message, object obj)
		{
			try
			{
				if (obj != null)
				{
					obj.ToString();
				}
			}
			catch
			{
			}
		}

		// Token: 0x060000E1 RID: 225 RVA: 0x00006740 File Offset: 0x00004940
		[Conditional("DEBUG")]
		public static void DebugWriteLine(string message)
		{
		}

		// Token: 0x060000E2 RID: 226 RVA: 0x00006740 File Offset: 0x00004940
		[Conditional("TRACE")]
		public static void TraceWriteLine(string message)
		{
		}

		// Token: 0x060000E3 RID: 227 RVA: 0x00006740 File Offset: 0x00004940
		[Conditional("DEBUG")]
		public static void DebugAssert(bool condition, string message)
		{
		}

		// Token: 0x060000E4 RID: 228 RVA: 0x00006740 File Offset: 0x00004940
		[Conditional("DEBUG")]
		public static void DebugAssert(bool condition, string message, params object[] args)
		{
		}

		// Token: 0x060000E5 RID: 229 RVA: 0x00009E30 File Offset: 0x00008030
		[Conditional("DEBUG")]
		public static void DebugAssert(bool condition)
		{
			if (!condition && Debugger.IsAttached)
			{
				Debugger.Break();
			}
		}

		// Token: 0x060000E6 RID: 230 RVA: 0x00009E44 File Offset: 0x00008044
		public static void Sort(int[] keys, object[] values)
		{
			bool flag;
			do
			{
				flag = false;
				for (int i = 1; i < keys.Length; i++)
				{
					if (keys[i - 1] > keys[i])
					{
						int num = keys[i];
						keys[i] = keys[i - 1];
						keys[i - 1] = num;
						object obj = values[i];
						values[i] = values[i - 1];
						values[i - 1] = obj;
						flag = true;
					}
				}
			}
			while (flag);
		}

		// Token: 0x060000E7 RID: 231 RVA: 0x00009E94 File Offset: 0x00008094
		public static void BlockCopy(byte[] from, int fromIndex, byte[] to, int toIndex, int count)
		{
			Buffer.BlockCopy(from, fromIndex, to, toIndex, count);
		}

		// Token: 0x060000E8 RID: 232 RVA: 0x00009EA1 File Offset: 0x000080A1
		public static bool IsInfinity(float value)
		{
			return float.IsInfinity(value);
		}

		// Token: 0x060000E9 RID: 233 RVA: 0x00009EA9 File Offset: 0x000080A9
		internal static MethodInfo GetInstanceMethod(Type declaringType, string name)
		{
			return declaringType.GetMethod(name, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
		}

		// Token: 0x060000EA RID: 234 RVA: 0x00009EB4 File Offset: 0x000080B4
		internal static MethodInfo GetStaticMethod(Type declaringType, string name)
		{
			return declaringType.GetMethod(name, BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
		}

		// Token: 0x060000EB RID: 235 RVA: 0x00009EBF File Offset: 0x000080BF
		internal static MethodInfo GetInstanceMethod(Type declaringType, string name, Type[] types)
		{
			if (types == null)
			{
				types = Helpers.EmptyTypes;
			}
			return declaringType.GetMethod(name, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic, null, types, null);
		}

		// Token: 0x060000EC RID: 236 RVA: 0x00009ED7 File Offset: 0x000080D7
		internal static bool IsSubclassOf(Type type, Type baseClass)
		{
			return type.IsSubclassOf(baseClass);
		}

		// Token: 0x060000ED RID: 237 RVA: 0x00009EE0 File Offset: 0x000080E0
		public static bool IsInfinity(double value)
		{
			return double.IsInfinity(value);
		}

		// Token: 0x060000EE RID: 238 RVA: 0x00009EE8 File Offset: 0x000080E8
		public static ProtoTypeCode GetTypeCode(Type type)
		{
			TypeCode typeCode = Type.GetTypeCode(type);
			switch (typeCode)
			{
			case TypeCode.Empty:
			case TypeCode.Boolean:
			case TypeCode.Char:
			case TypeCode.SByte:
			case TypeCode.Byte:
			case TypeCode.Int16:
			case TypeCode.UInt16:
			case TypeCode.Int32:
			case TypeCode.UInt32:
			case TypeCode.Int64:
			case TypeCode.UInt64:
			case TypeCode.Single:
			case TypeCode.Double:
			case TypeCode.Decimal:
			case TypeCode.DateTime:
			case TypeCode.String:
				return (ProtoTypeCode)typeCode;
			}
			if (type == typeof(TimeSpan))
			{
				return ProtoTypeCode.TimeSpan;
			}
			if (type == typeof(Guid))
			{
				return ProtoTypeCode.Guid;
			}
			if (type == typeof(Uri))
			{
				return ProtoTypeCode.Uri;
			}
			if (type == typeof(byte[]))
			{
				return ProtoTypeCode.ByteArray;
			}
			if (type == typeof(Type))
			{
				return ProtoTypeCode.Type;
			}
			return ProtoTypeCode.Unknown;
		}

		// Token: 0x060000EF RID: 239 RVA: 0x00009FA3 File Offset: 0x000081A3
		internal static Type GetUnderlyingType(Type type)
		{
			return Nullable.GetUnderlyingType(type);
		}

		// Token: 0x060000F0 RID: 240 RVA: 0x00009FAB File Offset: 0x000081AB
		internal static bool IsValueType(Type type)
		{
			return type.IsValueType;
		}

		// Token: 0x060000F1 RID: 241 RVA: 0x00009FB3 File Offset: 0x000081B3
		internal static bool IsEnum(Type type)
		{
			return type.IsEnum;
		}

		// Token: 0x060000F2 RID: 242 RVA: 0x00009FBB File Offset: 0x000081BB
		internal static MethodInfo GetGetMethod(PropertyInfo property, bool nonPublic)
		{
			if (property == null)
			{
				return null;
			}
			return property.GetGetMethod(nonPublic);
		}

		// Token: 0x060000F3 RID: 243 RVA: 0x00009FC9 File Offset: 0x000081C9
		internal static MethodInfo GetSetMethod(PropertyInfo property, bool nonPublic)
		{
			if (property == null)
			{
				return null;
			}
			return property.GetSetMethod(nonPublic);
		}

		// Token: 0x060000F4 RID: 244 RVA: 0x00009FD7 File Offset: 0x000081D7
		internal static ConstructorInfo GetConstructor(Type type, Type[] parameterTypes, bool nonPublic)
		{
			return type.GetConstructor(nonPublic ? (BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic) : (BindingFlags.Instance | BindingFlags.Public), null, parameterTypes, null);
		}

		// Token: 0x060000F5 RID: 245 RVA: 0x00009FEB File Offset: 0x000081EB
		internal static ConstructorInfo[] GetConstructors(Type type, bool nonPublic)
		{
			return type.GetConstructors(nonPublic ? (BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic) : (BindingFlags.Instance | BindingFlags.Public));
		}

		// Token: 0x060000F6 RID: 246 RVA: 0x00009FFC File Offset: 0x000081FC
		internal static PropertyInfo GetProperty(Type type, string name)
		{
			return type.GetProperty(name);
		}

		// Token: 0x060000F7 RID: 247 RVA: 0x0000A005 File Offset: 0x00008205
		internal static object ParseEnum(Type type, string value)
		{
			return Enum.Parse(type, value, true);
		}

		// Token: 0x060000F8 RID: 248 RVA: 0x0000A010 File Offset: 0x00008210
		internal static MemberInfo[] GetInstanceFieldsAndProperties(Type type, bool publicOnly)
		{
			BindingFlags bindingAttr = publicOnly ? (BindingFlags.Instance | BindingFlags.Public) : (BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
			PropertyInfo[] properties = type.GetProperties(bindingAttr);
			FieldInfo[] fields = type.GetFields(bindingAttr);
			MemberInfo[] array = new MemberInfo[fields.Length + properties.Length];
			properties.CopyTo(array, 0);
			fields.CopyTo(array, properties.Length);
			return array;
		}

		// Token: 0x060000F9 RID: 249 RVA: 0x0000A058 File Offset: 0x00008258
		internal static Type GetMemberType(MemberInfo member)
		{
			MemberTypes memberType = member.MemberType;
			if (memberType == MemberTypes.Field)
			{
				return ((FieldInfo)member).FieldType;
			}
			if (memberType != MemberTypes.Property)
			{
				return null;
			}
			return ((PropertyInfo)member).PropertyType;
		}

		// Token: 0x060000FA RID: 250 RVA: 0x0000A08E File Offset: 0x0000828E
		internal static bool IsAssignableFrom(Type target, Type type)
		{
			return target.IsAssignableFrom(type);
		}

		// Token: 0x0400018C RID: 396
		public static readonly Type[] EmptyTypes = new Type[0];
	}
}
