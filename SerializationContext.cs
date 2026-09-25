using System;

namespace ProtoBuf
{
	// Token: 0x02000033 RID: 51
	public sealed class SerializationContext
	{
		// Token: 0x060001C6 RID: 454 RVA: 0x0000D134 File Offset: 0x0000B334
		internal void Freeze()
		{
			this.frozen = true;
		}

		// Token: 0x060001C7 RID: 455 RVA: 0x0000D13D File Offset: 0x0000B33D
		private void ThrowIfFrozen()
		{
			if (this.frozen)
			{
				throw new InvalidOperationException("The serialization-context cannot be changed once it is in use");
			}
		}

		// Token: 0x17000032 RID: 50
		// (get) Token: 0x060001C8 RID: 456 RVA: 0x0000D152 File Offset: 0x0000B352
		// (set) Token: 0x060001C9 RID: 457 RVA: 0x0000D15A File Offset: 0x0000B35A
		public object Context
		{
			get
			{
				return this.context;
			}
			set
			{
				if (this.context != value)
				{
					this.ThrowIfFrozen();
					this.context = value;
				}
			}
		}

		// Token: 0x060001CA RID: 458 RVA: 0x0000D172 File Offset: 0x0000B372
		static SerializationContext()
		{
			SerializationContext.@default.Freeze();
		}

		// Token: 0x17000033 RID: 51
		// (get) Token: 0x060001CB RID: 459 RVA: 0x0000D188 File Offset: 0x0000B388
		internal static SerializationContext Default
		{
			get
			{
				return SerializationContext.@default;
			}
		}

		// Token: 0x040001F6 RID: 502
		private bool frozen;

		// Token: 0x040001F7 RID: 503
		private object context;

		// Token: 0x040001F8 RID: 504
		private static readonly SerializationContext @default = new SerializationContext();
	}
}
