using System;

namespace ProtoBuf
{
	// Token: 0x02000029 RID: 41
	[AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
	public sealed class ProtoEnumAttribute : Attribute
	{
		// Token: 0x17000016 RID: 22
		// (get) Token: 0x06000120 RID: 288 RVA: 0x0000A3CA File Offset: 0x000085CA
		// (set) Token: 0x06000121 RID: 289 RVA: 0x0000A3D2 File Offset: 0x000085D2
		public int Value
		{
			get
			{
				return this.enumValue;
			}
			set
			{
				this.enumValue = value;
				this.hasValue = true;
			}
		}

		// Token: 0x06000122 RID: 290 RVA: 0x0000A3E2 File Offset: 0x000085E2
		public bool HasValue()
		{
			return this.hasValue;
		}

		// Token: 0x17000017 RID: 23
		// (get) Token: 0x06000123 RID: 291 RVA: 0x0000A3EA File Offset: 0x000085EA
		// (set) Token: 0x06000124 RID: 292 RVA: 0x0000A3F2 File Offset: 0x000085F2
		public string Name
		{
			get
			{
				return this.name;
			}
			set
			{
				this.name = value;
			}
		}

		// Token: 0x040001AD RID: 429
		private bool hasValue;

		// Token: 0x040001AE RID: 430
		private int enumValue;

		// Token: 0x040001AF RID: 431
		private string name;
	}
}
