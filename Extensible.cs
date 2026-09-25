using System;
using System.Collections;
using System.Collections.Generic;
using ProtoBuf.Meta;

namespace ProtoBuf
{
	// Token: 0x0200001A RID: 26
	public abstract class Extensible : IExtensible
	{
		// Token: 0x060000C8 RID: 200 RVA: 0x00009B59 File Offset: 0x00007D59
		IExtension IExtensible.GetExtensionObject(bool createIfMissing)
		{
			return this.GetExtensionObject(createIfMissing);
		}

		// Token: 0x060000C9 RID: 201 RVA: 0x00009B62 File Offset: 0x00007D62
		protected virtual IExtension GetExtensionObject(bool createIfMissing)
		{
			return Extensible.GetExtensionObject(ref this.extensionObject, createIfMissing);
		}

		// Token: 0x060000CA RID: 202 RVA: 0x00009B70 File Offset: 0x00007D70
		public static IExtension GetExtensionObject(ref IExtension extensionObject, bool createIfMissing)
		{
			if (createIfMissing && extensionObject == null)
			{
				extensionObject = new BufferExtension();
			}
			return extensionObject;
		}

		// Token: 0x060000CB RID: 203 RVA: 0x00009B82 File Offset: 0x00007D82
		public static void AppendValue<TValue>(IExtensible instance, int tag, TValue value)
		{
			Extensible.AppendValue<TValue>(instance, tag, DataFormat.Default, value);
		}

		// Token: 0x060000CC RID: 204 RVA: 0x00009B8D File Offset: 0x00007D8D
		public static void AppendValue<TValue>(IExtensible instance, int tag, DataFormat format, TValue value)
		{
			ExtensibleUtil.AppendExtendValue(RuntimeTypeModel.Default, instance, tag, format, value);
		}

		// Token: 0x060000CD RID: 205 RVA: 0x00009BA2 File Offset: 0x00007DA2
		public static TValue GetValue<TValue>(IExtensible instance, int tag)
		{
			return Extensible.GetValue<TValue>(instance, tag, DataFormat.Default);
		}

		// Token: 0x060000CE RID: 206 RVA: 0x00009BAC File Offset: 0x00007DAC
		public static TValue GetValue<TValue>(IExtensible instance, int tag, DataFormat format)
		{
			TValue result;
			Extensible.TryGetValue<TValue>(instance, tag, format, out result);
			return result;
		}

		// Token: 0x060000CF RID: 207 RVA: 0x00009BC5 File Offset: 0x00007DC5
		public static bool TryGetValue<TValue>(IExtensible instance, int tag, out TValue value)
		{
			return Extensible.TryGetValue<TValue>(instance, tag, DataFormat.Default, out value);
		}

		// Token: 0x060000D0 RID: 208 RVA: 0x00009BD0 File Offset: 0x00007DD0
		public static bool TryGetValue<TValue>(IExtensible instance, int tag, DataFormat format, out TValue value)
		{
			return Extensible.TryGetValue<TValue>(instance, tag, format, false, out value);
		}

		// Token: 0x060000D1 RID: 209 RVA: 0x00009BDC File Offset: 0x00007DDC
		public static bool TryGetValue<TValue>(IExtensible instance, int tag, DataFormat format, bool allowDefinedTag, out TValue value)
		{
			value = default(TValue);
			bool result = false;
			foreach (TValue tvalue in ExtensibleUtil.GetExtendedValues<TValue>(instance, tag, format, true, allowDefinedTag))
			{
				value = tvalue;
				result = true;
			}
			return result;
		}

		// Token: 0x060000D2 RID: 210 RVA: 0x00009C3C File Offset: 0x00007E3C
		public static IEnumerable<TValue> GetValues<TValue>(IExtensible instance, int tag)
		{
			return ExtensibleUtil.GetExtendedValues<TValue>(instance, tag, DataFormat.Default, false, false);
		}

		// Token: 0x060000D3 RID: 211 RVA: 0x00009C48 File Offset: 0x00007E48
		public static IEnumerable<TValue> GetValues<TValue>(IExtensible instance, int tag, DataFormat format)
		{
			return ExtensibleUtil.GetExtendedValues<TValue>(instance, tag, format, false, false);
		}

		// Token: 0x060000D4 RID: 212 RVA: 0x00009C54 File Offset: 0x00007E54
		public static bool TryGetValue(TypeModel model, Type type, IExtensible instance, int tag, DataFormat format, bool allowDefinedTag, out object value)
		{
			value = null;
			bool result = false;
			foreach (object obj in ExtensibleUtil.GetExtendedValues(model, type, instance, tag, format, true, allowDefinedTag))
			{
				value = obj;
				result = true;
			}
			return result;
		}

		// Token: 0x060000D5 RID: 213 RVA: 0x00009CB8 File Offset: 0x00007EB8
		public static IEnumerable GetValues(TypeModel model, Type type, IExtensible instance, int tag, DataFormat format)
		{
			return ExtensibleUtil.GetExtendedValues(model, type, instance, tag, format, false, false);
		}

		// Token: 0x060000D6 RID: 214 RVA: 0x00009CC7 File Offset: 0x00007EC7
		public static void AppendValue(TypeModel model, IExtensible instance, int tag, DataFormat format, object value)
		{
			ExtensibleUtil.AppendExtendValue(model, instance, tag, format, value);
		}

		// Token: 0x04000183 RID: 387
		private IExtension extensionObject;
	}
}
