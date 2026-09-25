using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using ProtoBuf.Meta;

namespace ProtoBuf
{
	// Token: 0x0200001B RID: 27
	internal static class ExtensibleUtil
	{
		// Token: 0x060000D8 RID: 216 RVA: 0x00009CD4 File Offset: 0x00007ED4
		internal static IEnumerable<TValue> GetExtendedValues<TValue>(IExtensible instance, int tag, DataFormat format, bool singleton, bool allowDefinedTag)
		{
			foreach (object obj in ExtensibleUtil.GetExtendedValues(RuntimeTypeModel.Default, typeof(TValue), instance, tag, format, singleton, allowDefinedTag))
			{
				TValue tvalue = (TValue)((object)obj);
				yield return tvalue;
			}
			yield break;
		}

		// Token: 0x060000D9 RID: 217 RVA: 0x00009D01 File Offset: 0x00007F01
		internal static IEnumerable GetExtendedValues(TypeModel model, Type type, IExtensible instance, int tag, DataFormat format, bool singleton, bool allowDefinedTag)
		{
			if (instance == null)
			{
				throw new ArgumentNullException("instance");
			}
			if (tag <= 0)
			{
				throw new ArgumentOutOfRangeException("tag");
			}
			IExtension extensionObject = instance.GetExtensionObject(false);
			if (extensionObject != null)
			{
				Stream stream = extensionObject.BeginQuery();
				object obj = null;
				try
				{
					SerializationContext context = new SerializationContext();
					using (ProtoReader protoReader = new ProtoReader(stream, model, context))
					{
						while (model.TryDeserializeAuxiliaryType(protoReader, format, tag, type, ref obj, true, false, false, false) && obj != null)
						{
							if (!singleton)
							{
								yield return obj;
								obj = null;
							}
						}
					}
					ProtoReader protoReader = null;
					if (singleton && obj != null)
					{
						yield return obj;
					}
				}
				finally
				{
					extensionObject.EndQuery(stream);
				}
				stream = null;
			}
			yield break;
			yield break;
		}

		// Token: 0x060000DA RID: 218 RVA: 0x00009D38 File Offset: 0x00007F38
		internal static void AppendExtendValue(TypeModel model, IExtensible instance, int tag, DataFormat format, object value)
		{
			if (instance == null)
			{
				throw new ArgumentNullException("instance");
			}
			if (value == null)
			{
				throw new ArgumentNullException("value");
			}
			IExtension extensionObject = instance.GetExtensionObject(true);
			if (extensionObject == null)
			{
				throw new InvalidOperationException("No extension object available; appended data would be lost.");
			}
			bool commit = false;
			Stream stream = extensionObject.BeginAppend();
			try
			{
				using (ProtoWriter protoWriter = new ProtoWriter(stream, model, null))
				{
					model.TrySerializeAuxiliaryType(protoWriter, null, format, tag, value, false);
					protoWriter.Close();
				}
				commit = true;
			}
			finally
			{
				extensionObject.EndAppend(stream, commit);
			}
		}

		// Token: 0x060000DB RID: 219 RVA: 0x00009DD4 File Offset: 0x00007FD4
		public static void AppendExtendValueTyped<TSource, TValue>(TypeModel model, TSource instance, int tag, DataFormat format, TValue value) where TSource : class, IExtensible
		{
			ExtensibleUtil.AppendExtendValue(model, instance, tag, format, value);
		}
	}
}
