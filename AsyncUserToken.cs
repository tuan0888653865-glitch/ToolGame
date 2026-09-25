using System;
using System.Net.Sockets;

namespace TinhKiemAuto
{
	// Token: 0x02000115 RID: 277
	public sealed class AsyncUserToken : IDisposable
	{
		// Token: 0x17000338 RID: 824
		// (get) Token: 0x06000ECF RID: 3791 RVA: 0x00071E52 File Offset: 0x00070052
		// (set) Token: 0x06000ED0 RID: 3792 RVA: 0x00071E5A File Offset: 0x0007005A
		public Socket Socket { get; private set; }

		// Token: 0x17000339 RID: 825
		// (get) Token: 0x06000ED1 RID: 3793 RVA: 0x00071E63 File Offset: 0x00070063
		// (set) Token: 0x06000ED2 RID: 3794 RVA: 0x00071E6B File Offset: 0x0007006B
		public int? MessageSize { get; set; }

		// Token: 0x1700033A RID: 826
		// (get) Token: 0x06000ED3 RID: 3795 RVA: 0x00071E74 File Offset: 0x00070074
		// (set) Token: 0x06000ED4 RID: 3796 RVA: 0x00071E7C File Offset: 0x0007007C
		public int DataStartOffset { get; set; }

		// Token: 0x1700033B RID: 827
		// (get) Token: 0x06000ED5 RID: 3797 RVA: 0x00071E85 File Offset: 0x00070085
		// (set) Token: 0x06000ED6 RID: 3798 RVA: 0x00071E8D File Offset: 0x0007008D
		public int NextReceiveOffset { get; set; }

		// Token: 0x06000ED7 RID: 3799 RVA: 0x00071E96 File Offset: 0x00070096
		public AsyncUserToken(Socket socket)
		{
			this.Socket = socket;
		}

		// Token: 0x06000ED8 RID: 3800 RVA: 0x00071EA8 File Offset: 0x000700A8
		public void Dispose()
		{
			try
			{
				this.Socket.Shutdown(SocketShutdown.Send);
			}
			catch (Exception)
			{
			}
			try
			{
				this.Socket.Close();
			}
			catch (Exception)
			{
			}
		}
	}
}
