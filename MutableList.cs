using System;

namespace ProtoBuf.Meta
{
	// Token: 0x0200006D RID: 109
	internal sealed class MutableList : BasicList
	{
		// Token: 0x170000D0 RID: 208
		public new object this[int index]
		{
			get
			{
				return this.head[index];
			}
			set
			{
				this.head[index] = value;
			}
		}

		// Token: 0x060003B6 RID: 950 RVA: 0x00013960 File Offset: 0x00011B60
		public void RemoveLast()
		{
			this.head.RemoveLastWithMutate();
		}
	}
}
