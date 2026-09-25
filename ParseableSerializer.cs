using System;
using System.Reflection;
using ProtoBuf.Meta;

namespace ProtoBuf.Serializers
{
	// Token: 0x02000055 RID: 85
	internal sealed class ParseableSerializer : IProtoSerializer
	{
		// Token: 0x060002AA RID: 682 RVA: 0x0000FBA8 File Offset: 0x0000DDA8
		public static ParseableSerializer TryCreate(Type type, TypeModel model)
		{
			if (type == null)
			{
				throw new ArgumentNullException("type");
			}
			MethodInfo method = type.GetMethod("Parse", BindingFlags.DeclaredOnly | BindingFlags.Static | BindingFlags.Public, null, new Type[]
			{
				model.MapType(typeof(string))
			}, null);
			if (method != null && method.ReturnType == type)
			{
				if (Helpers.IsValueType(type))
				{
					MethodInfo customToString = ParseableSerializer.GetCustomToString(type);
					if (customToString == null || customToString.ReturnType != model.MapType(typeof(string)))
					{
						return null;
					}
				}
				return new ParseableSerializer(method);
			}
			return null;
		}

		// Token: 0x060002AB RID: 683 RVA: 0x0000FC2C File Offset: 0x0000DE2C
		private static MethodInfo GetCustomToString(Type type)
		{
			return type.GetMethod("ToString", BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Public, null, Helpers.EmptyTypes, null);
		}

		// Token: 0x060002AC RID: 684 RVA: 0x0000FC42 File Offset: 0x0000DE42
		private ParseableSerializer(MethodInfo parse)
		{
			this.parse = parse;
		}

		// Token: 0x1700007A RID: 122
		// (get) Token: 0x060002AD RID: 685 RVA: 0x0000FC51 File Offset: 0x0000DE51
		public Type ExpectedType
		{
			get
			{
				return this.parse.DeclaringType;
			}
		}

		// Token: 0x1700007B RID: 123
		// (get) Token: 0x060002AE RID: 686 RVA: 0x00008C9D File Offset: 0x00006E9D
		bool IProtoSerializer.RequiresOldValue
		{
			get
			{
				return false;
			}
		}

		// Token: 0x1700007C RID: 124
		// (get) Token: 0x060002AF RID: 687 RVA: 0x0000D470 File Offset: 0x0000B670
		bool IProtoSerializer.ReturnsValue
		{
			get
			{
				return true;
			}
		}

		// Token: 0x060002B0 RID: 688 RVA: 0x0000FC5E File Offset: 0x0000DE5E
		public object Read(object value, ProtoReader source)
		{
			return this.parse.Invoke(null, new object[]
			{
				source.ReadString()
			});
		}

		// Token: 0x060002B1 RID: 689 RVA: 0x0000FC7B File Offset: 0x0000DE7B
		public void Write(object value, ProtoWriter dest)
		{
			ProtoWriter.WriteString(value.ToString(), dest);
		}

		// Token: 0x04000261 RID: 609
		private readonly MethodInfo parse;
	}
}
