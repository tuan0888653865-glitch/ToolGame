using System;

namespace ProtoBuf
{
	// Token: 0x02000028 RID: 40
	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Enum | AttributeTargets.Interface, AllowMultiple = false, Inherited = false)]
	public sealed class ProtoContractAttribute : Attribute
	{
		// Token: 0x1700000D RID: 13
		// (get) Token: 0x0600010C RID: 268 RVA: 0x0000A2E5 File Offset: 0x000084E5
		// (set) Token: 0x0600010D RID: 269 RVA: 0x0000A2ED File Offset: 0x000084ED
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

		// Token: 0x1700000E RID: 14
		// (get) Token: 0x0600010E RID: 270 RVA: 0x0000A2F6 File Offset: 0x000084F6
		// (set) Token: 0x0600010F RID: 271 RVA: 0x0000A2FE File Offset: 0x000084FE
		public int ImplicitFirstTag
		{
			get
			{
				return this.implicitFirstTag;
			}
			set
			{
				if (value < 1)
				{
					throw new ArgumentOutOfRangeException("ImplicitFirstTag");
				}
				this.implicitFirstTag = value;
			}
		}

		// Token: 0x1700000F RID: 15
		// (get) Token: 0x06000110 RID: 272 RVA: 0x0000A316 File Offset: 0x00008516
		// (set) Token: 0x06000111 RID: 273 RVA: 0x0000A31F File Offset: 0x0000851F
		public bool UseProtoMembersOnly
		{
			get
			{
				return this.HasFlag(4);
			}
			set
			{
				this.SetFlag(4, value);
			}
		}

		// Token: 0x17000010 RID: 16
		// (get) Token: 0x06000112 RID: 274 RVA: 0x0000A329 File Offset: 0x00008529
		// (set) Token: 0x06000113 RID: 275 RVA: 0x0000A333 File Offset: 0x00008533
		public bool IgnoreListHandling
		{
			get
			{
				return this.HasFlag(16);
			}
			set
			{
				this.SetFlag(16, value);
			}
		}

		// Token: 0x17000011 RID: 17
		// (get) Token: 0x06000114 RID: 276 RVA: 0x0000A33E File Offset: 0x0000853E
		// (set) Token: 0x06000115 RID: 277 RVA: 0x0000A346 File Offset: 0x00008546
		public ImplicitFields ImplicitFields
		{
			get
			{
				return this.implicitFields;
			}
			set
			{
				this.implicitFields = value;
			}
		}

		// Token: 0x17000012 RID: 18
		// (get) Token: 0x06000116 RID: 278 RVA: 0x0000A34F File Offset: 0x0000854F
		// (set) Token: 0x06000117 RID: 279 RVA: 0x0000A358 File Offset: 0x00008558
		public bool InferTagFromName
		{
			get
			{
				return this.HasFlag(1);
			}
			set
			{
				this.SetFlag(1, value);
				this.SetFlag(2, true);
			}
		}

		// Token: 0x17000013 RID: 19
		// (get) Token: 0x06000118 RID: 280 RVA: 0x0000A36A File Offset: 0x0000856A
		internal bool InferTagFromNameHasValue
		{
			get
			{
				return this.HasFlag(2);
			}
		}

		// Token: 0x17000014 RID: 20
		// (get) Token: 0x06000119 RID: 281 RVA: 0x0000A373 File Offset: 0x00008573
		// (set) Token: 0x0600011A RID: 282 RVA: 0x0000A37B File Offset: 0x0000857B
		public int DataMemberOffset
		{
			get
			{
				return this.dataMemberOffset;
			}
			set
			{
				this.dataMemberOffset = value;
			}
		}

		// Token: 0x17000015 RID: 21
		// (get) Token: 0x0600011B RID: 283 RVA: 0x0000A384 File Offset: 0x00008584
		// (set) Token: 0x0600011C RID: 284 RVA: 0x0000A38D File Offset: 0x0000858D
		public bool SkipConstructor
		{
			get
			{
				return this.HasFlag(8);
			}
			set
			{
				this.SetFlag(8, value);
			}
		}

		// Token: 0x0600011D RID: 285 RVA: 0x0000A397 File Offset: 0x00008597
		private bool HasFlag(byte flag)
		{
			return (this.flags & flag) == flag;
		}

		// Token: 0x0600011E RID: 286 RVA: 0x0000A3A4 File Offset: 0x000085A4
		private void SetFlag(byte flag, bool value)
		{
			if (value)
			{
				this.flags |= flag;
				return;
			}
			this.flags &= ~flag;
		}

		// Token: 0x040001A3 RID: 419
		private const byte OPTIONS_InferTagFromName = 1;

		// Token: 0x040001A4 RID: 420
		private const byte OPTIONS_InferTagFromNameHasValue = 2;

		// Token: 0x040001A5 RID: 421
		private const byte OPTIONS_UseProtoMembersOnly = 4;

		// Token: 0x040001A6 RID: 422
		private const byte OPTIONS_SkipConstructor = 8;

		// Token: 0x040001A7 RID: 423
		private const byte OPTIONS_IgnoreListHandling = 16;

		// Token: 0x040001A8 RID: 424
		private string name;

		// Token: 0x040001A9 RID: 425
		private int implicitFirstTag;

		// Token: 0x040001AA RID: 426
		private ImplicitFields implicitFields;

		// Token: 0x040001AB RID: 427
		private int dataMemberOffset;

		// Token: 0x040001AC RID: 428
		private byte flags;
	}
}
