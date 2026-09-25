using System;
using System.ComponentModel;
using ProtoBuf.Meta;

namespace ProtoBuf
{
	// Token: 0x0200002C RID: 44
	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface, AllowMultiple = true, Inherited = false)]
	public sealed class ProtoIncludeAttribute : Attribute
	{
		// Token: 0x0600012A RID: 298 RVA: 0x0000A416 File Offset: 0x00008616
		public ProtoIncludeAttribute(int tag, Type knownType) : this(tag, (knownType == null) ? "" : knownType.AssemblyQualifiedName)
		{
		}

		// Token: 0x0600012B RID: 299 RVA: 0x0000A430 File Offset: 0x00008630
		public ProtoIncludeAttribute(int tag, string knownTypeName)
		{
			if (tag <= 0)
			{
				throw new ArgumentOutOfRangeException("tag", "Tags must be positive integers");
			}
			if (Helpers.IsNullOrEmpty(knownTypeName))
			{
				throw new ArgumentNullException("knownTypeName", "Known type cannot be blank");
			}
			this.tag = tag;
			this.knownTypeName = knownTypeName;
		}

		// Token: 0x17000018 RID: 24
		// (get) Token: 0x0600012C RID: 300 RVA: 0x0000A47D File Offset: 0x0000867D
		public int Tag
		{
			get
			{
				return this.tag;
			}
		}

		// Token: 0x17000019 RID: 25
		// (get) Token: 0x0600012D RID: 301 RVA: 0x0000A485 File Offset: 0x00008685
		public string KnownTypeName
		{
			get
			{
				return this.knownTypeName;
			}
		}

		// Token: 0x1700001A RID: 26
		// (get) Token: 0x0600012E RID: 302 RVA: 0x0000A48D File Offset: 0x0000868D
		public Type KnownType
		{
			get
			{
				return TypeModel.ResolveKnownType(this.KnownTypeName, null, null);
			}
		}

		// Token: 0x1700001B RID: 27
		// (get) Token: 0x0600012F RID: 303 RVA: 0x0000A49C File Offset: 0x0000869C
		// (set) Token: 0x06000130 RID: 304 RVA: 0x0000A4A4 File Offset: 0x000086A4
		[DefaultValue(DataFormat.Default)]
		public DataFormat DataFormat
		{
			get
			{
				return this.dataFormat;
			}
			set
			{
				this.dataFormat = value;
			}
		}

		// Token: 0x040001B0 RID: 432
		private readonly int tag;

		// Token: 0x040001B1 RID: 433
		private readonly string knownTypeName;

		// Token: 0x040001B2 RID: 434
		private DataFormat dataFormat;
	}
}
