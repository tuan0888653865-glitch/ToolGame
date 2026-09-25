using System;
using System.Reflection;

namespace ProtoBuf
{
	// Token: 0x0200002D RID: 45
	[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
	public class ProtoMemberAttribute : Attribute, IComparable, IComparable<ProtoMemberAttribute>
	{
		// Token: 0x06000131 RID: 305 RVA: 0x0000A4AD File Offset: 0x000086AD
		public int CompareTo(object other)
		{
			return this.CompareTo(other as ProtoMemberAttribute);
		}

		// Token: 0x06000132 RID: 306 RVA: 0x0000A4BC File Offset: 0x000086BC
		public int CompareTo(ProtoMemberAttribute other)
		{
			if (other == null)
			{
				return -1;
			}
			if (this == other)
			{
				return 0;
			}
			int num = this.tag.CompareTo(other.tag);
			if (num == 0)
			{
				num = string.CompareOrdinal(this.name, other.name);
			}
			return num;
		}

		// Token: 0x06000133 RID: 307 RVA: 0x0000A4FC File Offset: 0x000086FC
		public ProtoMemberAttribute(int tag) : this(tag, false)
		{
		}

		// Token: 0x06000134 RID: 308 RVA: 0x0000A506 File Offset: 0x00008706
		internal ProtoMemberAttribute(int tag, bool forced)
		{
			if (tag <= 0 && !forced)
			{
				throw new ArgumentOutOfRangeException("tag");
			}
			this.tag = tag;
		}

		// Token: 0x1700001C RID: 28
		// (get) Token: 0x06000135 RID: 309 RVA: 0x0000A527 File Offset: 0x00008727
		// (set) Token: 0x06000136 RID: 310 RVA: 0x0000A52F File Offset: 0x0000872F
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

		// Token: 0x1700001D RID: 29
		// (get) Token: 0x06000137 RID: 311 RVA: 0x0000A538 File Offset: 0x00008738
		// (set) Token: 0x06000138 RID: 312 RVA: 0x0000A540 File Offset: 0x00008740
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

		// Token: 0x1700001E RID: 30
		// (get) Token: 0x06000139 RID: 313 RVA: 0x0000A549 File Offset: 0x00008749
		public int Tag
		{
			get
			{
				return this.tag;
			}
		}

		// Token: 0x0600013A RID: 314 RVA: 0x0000A551 File Offset: 0x00008751
		internal void Rebase(int tag)
		{
			this.tag = tag;
		}

		// Token: 0x1700001F RID: 31
		// (get) Token: 0x0600013B RID: 315 RVA: 0x0000A55A File Offset: 0x0000875A
		// (set) Token: 0x0600013C RID: 316 RVA: 0x0000A567 File Offset: 0x00008767
		public bool IsRequired
		{
			get
			{
				return (this.options & MemberSerializationOptions.Required) == MemberSerializationOptions.Required;
			}
			set
			{
				if (value)
				{
					this.options |= MemberSerializationOptions.Required;
					return;
				}
				this.options &= ~MemberSerializationOptions.Required;
			}
		}

		// Token: 0x17000020 RID: 32
		// (get) Token: 0x0600013D RID: 317 RVA: 0x0000A58A File Offset: 0x0000878A
		// (set) Token: 0x0600013E RID: 318 RVA: 0x0000A597 File Offset: 0x00008797
		public bool IsPacked
		{
			get
			{
				return (this.options & MemberSerializationOptions.Packed) == MemberSerializationOptions.Packed;
			}
			set
			{
				if (value)
				{
					this.options |= MemberSerializationOptions.Packed;
					return;
				}
				this.options &= ~MemberSerializationOptions.Packed;
			}
		}

		// Token: 0x17000021 RID: 33
		// (get) Token: 0x0600013F RID: 319 RVA: 0x0000A5BA File Offset: 0x000087BA
		// (set) Token: 0x06000140 RID: 320 RVA: 0x0000A5C9 File Offset: 0x000087C9
		public bool OverwriteList
		{
			get
			{
				return (this.options & MemberSerializationOptions.OverwriteList) == MemberSerializationOptions.OverwriteList;
			}
			set
			{
				if (value)
				{
					this.options |= MemberSerializationOptions.OverwriteList;
					return;
				}
				this.options &= ~MemberSerializationOptions.OverwriteList;
			}
		}

		// Token: 0x17000022 RID: 34
		// (get) Token: 0x06000141 RID: 321 RVA: 0x0000A5ED File Offset: 0x000087ED
		// (set) Token: 0x06000142 RID: 322 RVA: 0x0000A5FA File Offset: 0x000087FA
		public bool AsReference
		{
			get
			{
				return (this.options & MemberSerializationOptions.AsReference) == MemberSerializationOptions.AsReference;
			}
			set
			{
				if (value)
				{
					this.options |= MemberSerializationOptions.AsReference;
					return;
				}
				this.options &= ~MemberSerializationOptions.AsReference;
			}
		}

		// Token: 0x17000023 RID: 35
		// (get) Token: 0x06000143 RID: 323 RVA: 0x0000A61D File Offset: 0x0000881D
		// (set) Token: 0x06000144 RID: 324 RVA: 0x0000A62A File Offset: 0x0000882A
		public bool DynamicType
		{
			get
			{
				return (this.options & MemberSerializationOptions.DynamicType) == MemberSerializationOptions.DynamicType;
			}
			set
			{
				if (value)
				{
					this.options |= MemberSerializationOptions.DynamicType;
					return;
				}
				this.options &= ~MemberSerializationOptions.DynamicType;
			}
		}

		// Token: 0x17000024 RID: 36
		// (get) Token: 0x06000145 RID: 325 RVA: 0x0000A64D File Offset: 0x0000884D
		// (set) Token: 0x06000146 RID: 326 RVA: 0x0000A655 File Offset: 0x00008855
		public MemberSerializationOptions Options
		{
			get
			{
				return this.options;
			}
			set
			{
				this.options = value;
			}
		}

		// Token: 0x040001B3 RID: 435
		internal MemberInfo Member;

		// Token: 0x040001B4 RID: 436
		internal bool TagIsPinned;

		// Token: 0x040001B5 RID: 437
		private string name;

		// Token: 0x040001B6 RID: 438
		private DataFormat dataFormat;

		// Token: 0x040001B7 RID: 439
		private int tag;

		// Token: 0x040001B8 RID: 440
		private MemberSerializationOptions options;
	}
}
