using System;
using System.Reflection;

namespace ProtoBuf.Meta
{
	// Token: 0x02000069 RID: 105
	public class CallbackSet
	{
		// Token: 0x06000349 RID: 841 RVA: 0x00010EA7 File Offset: 0x0000F0A7
		internal CallbackSet(MetaType metaType)
		{
			if (metaType == null)
			{
				throw new ArgumentNullException("metaType");
			}
			this.metaType = metaType;
		}

		// Token: 0x170000B3 RID: 179
		internal MethodInfo this[TypeModel.CallbackType callbackType]
		{
			get
			{
				switch (callbackType)
				{
				case TypeModel.CallbackType.BeforeSerialize:
					return this.beforeSerialize;
				case TypeModel.CallbackType.AfterSerialize:
					return this.afterSerialize;
				case TypeModel.CallbackType.BeforeDeserialize:
					return this.beforeDeserialize;
				case TypeModel.CallbackType.AfterDeserialize:
					return this.afterDeserialize;
				default:
					throw new ArgumentException();
				}
			}
		}

		// Token: 0x0600034B RID: 843 RVA: 0x00010F00 File Offset: 0x0000F100
		internal static bool CheckCallbackParameters(TypeModel model, MethodInfo method)
		{
			ParameterInfo[] parameters = method.GetParameters();
			return parameters.Length == 0 || (parameters.Length == 1 && parameters[0].ParameterType == model.MapType(typeof(SerializationContext)));
		}

		// Token: 0x0600034C RID: 844 RVA: 0x00010F3C File Offset: 0x0000F13C
		private MethodInfo SanityCheckCallback(TypeModel model, MethodInfo callback)
		{
			this.metaType.ThrowIfFrozen();
			if (callback == null)
			{
				return callback;
			}
			if (callback.IsStatic)
			{
				throw new ArgumentException("Callbacks cannot be static", "callback");
			}
			if (callback.ReturnType != model.MapType(typeof(void)) || !CallbackSet.CheckCallbackParameters(model, callback))
			{
				throw CallbackSet.CreateInvalidCallbackSignature(callback);
			}
			return callback;
		}

		// Token: 0x0600034D RID: 845 RVA: 0x00010F9A File Offset: 0x0000F19A
		internal static Exception CreateInvalidCallbackSignature(MethodInfo method)
		{
			return new NotSupportedException("Invalid callback signature in " + method.DeclaringType.FullName + "." + method.Name);
		}

		// Token: 0x170000B4 RID: 180
		// (get) Token: 0x0600034E RID: 846 RVA: 0x00010FC1 File Offset: 0x0000F1C1
		// (set) Token: 0x0600034F RID: 847 RVA: 0x00010FC9 File Offset: 0x0000F1C9
		public MethodInfo BeforeSerialize
		{
			get
			{
				return this.beforeSerialize;
			}
			set
			{
				this.beforeSerialize = this.SanityCheckCallback(this.metaType.Model, value);
			}
		}

		// Token: 0x170000B5 RID: 181
		// (get) Token: 0x06000350 RID: 848 RVA: 0x00010FE3 File Offset: 0x0000F1E3
		// (set) Token: 0x06000351 RID: 849 RVA: 0x00010FEB File Offset: 0x0000F1EB
		public MethodInfo BeforeDeserialize
		{
			get
			{
				return this.beforeDeserialize;
			}
			set
			{
				this.beforeDeserialize = this.SanityCheckCallback(this.metaType.Model, value);
			}
		}

		// Token: 0x170000B6 RID: 182
		// (get) Token: 0x06000352 RID: 850 RVA: 0x00011005 File Offset: 0x0000F205
		// (set) Token: 0x06000353 RID: 851 RVA: 0x0001100D File Offset: 0x0000F20D
		public MethodInfo AfterSerialize
		{
			get
			{
				return this.afterSerialize;
			}
			set
			{
				this.afterSerialize = this.SanityCheckCallback(this.metaType.Model, value);
			}
		}

		// Token: 0x170000B7 RID: 183
		// (get) Token: 0x06000354 RID: 852 RVA: 0x00011027 File Offset: 0x0000F227
		// (set) Token: 0x06000355 RID: 853 RVA: 0x0001102F File Offset: 0x0000F22F
		public MethodInfo AfterDeserialize
		{
			get
			{
				return this.afterDeserialize;
			}
			set
			{
				this.afterDeserialize = this.SanityCheckCallback(this.metaType.Model, value);
			}
		}

		// Token: 0x170000B8 RID: 184
		// (get) Token: 0x06000356 RID: 854 RVA: 0x00011049 File Offset: 0x0000F249
		public bool NonTrivial
		{
			get
			{
				return this.beforeSerialize != null || this.beforeDeserialize != null || this.afterSerialize != null || this.afterDeserialize != null;
			}
		}

		// Token: 0x0400028D RID: 653
		private readonly MetaType metaType;

		// Token: 0x0400028E RID: 654
		private MethodInfo beforeSerialize;

		// Token: 0x0400028F RID: 655
		private MethodInfo afterSerialize;

		// Token: 0x04000290 RID: 656
		private MethodInfo beforeDeserialize;

		// Token: 0x04000291 RID: 657
		private MethodInfo afterDeserialize;
	}
}
