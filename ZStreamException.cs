using System;
using System.IO;

namespace ComponentAce.Compression.Libs.zlib
{
	// Token: 0x02000014 RID: 20
	public class ZStreamException : IOException
	{
		// Token: 0x060000AC RID: 172 RVA: 0x0000903E File Offset: 0x0000723E
		public ZStreamException()
		{
		}

		// Token: 0x060000AD RID: 173 RVA: 0x00009046 File Offset: 0x00007246
		public ZStreamException(string s) : base(s)
		{
		}
	}
}
