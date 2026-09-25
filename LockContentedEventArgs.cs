using System;

namespace ProtoBuf.Meta
{
	// Token: 0x0200006A RID: 106
	public sealed class LockContentedEventArgs : EventArgs
	{
		// Token: 0x06000357 RID: 855 RVA: 0x0001106E File Offset: 0x0000F26E
		internal LockContentedEventArgs(string ownerStackTrace)
		{
			this.ownerStackTrace = ownerStackTrace;
		}

		// Token: 0x170000B9 RID: 185
		// (get) Token: 0x06000358 RID: 856 RVA: 0x0001107D File Offset: 0x0000F27D
		public string OwnerStackTrace
		{
			get
			{
				return this.ownerStackTrace;
			}
		}

		// Token: 0x04000292 RID: 658
		private readonly string ownerStackTrace;
	}
}
