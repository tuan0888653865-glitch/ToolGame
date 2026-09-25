using System;

namespace ProtoBuf.Meta
{
	// Token: 0x02000070 RID: 112
	public class TypeFormatEventArgs : EventArgs
	{
		// Token: 0x170000DC RID: 220
		// (get) Token: 0x060003E8 RID: 1000 RVA: 0x000149B9 File Offset: 0x00012BB9
		// (set) Token: 0x060003E9 RID: 1001 RVA: 0x000149C1 File Offset: 0x00012BC1
		public Type Type
		{
			get
			{
				return this.type;
			}
			set
			{
				if (this.type != value)
				{
					if (this.typeFixed)
					{
						throw new InvalidOperationException("The type is fixed and cannot be changed");
					}
					this.type = value;
				}
			}
		}

		// Token: 0x170000DD RID: 221
		// (get) Token: 0x060003EA RID: 1002 RVA: 0x000149E6 File Offset: 0x00012BE6
		// (set) Token: 0x060003EB RID: 1003 RVA: 0x000149EE File Offset: 0x00012BEE
		public string FormattedName
		{
			get
			{
				return this.formattedName;
			}
			set
			{
				if (this.formattedName != value)
				{
					if (!this.typeFixed)
					{
						throw new InvalidOperationException("The formatted-name is fixed and cannot be changed");
					}
					this.formattedName = value;
				}
			}
		}

		// Token: 0x060003EC RID: 1004 RVA: 0x00014A18 File Offset: 0x00012C18
		internal TypeFormatEventArgs(string formattedName)
		{
			if (Helpers.IsNullOrEmpty(formattedName))
			{
				throw new ArgumentNullException("formattedName");
			}
			this.formattedName = formattedName;
			this.typeFixed = false;
		}

		// Token: 0x060003ED RID: 1005 RVA: 0x00014A41 File Offset: 0x00012C41
		internal TypeFormatEventArgs(Type type)
		{
			if (type == null)
			{
				throw new ArgumentNullException("type");
			}
			this.type = type;
			this.typeFixed = true;
		}

		// Token: 0x040002B8 RID: 696
		private Type type;

		// Token: 0x040002B9 RID: 697
		private string formattedName;

		// Token: 0x040002BA RID: 698
		private readonly bool typeFixed;
	}
}
