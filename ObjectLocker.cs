using System;
using System.Threading;

namespace TinhKiemAuto
{
	// Token: 0x020000C4 RID: 196
	internal class ObjectLocker : IDisposable
	{
		// Token: 0x06000AA1 RID: 2721 RVA: 0x00044C8E File Offset: 0x00042E8E
		public ObjectLocker(object obj)
		{
			this.obj = obj;
			Monitor.Enter(this.obj);
		}

		// Token: 0x06000AA2 RID: 2722 RVA: 0x00044CA8 File Offset: 0x00042EA8
		public void Dispose()
		{
			Monitor.Exit(this.obj);
		}

		// Token: 0x0400083E RID: 2110
		private object obj;
	}
}
