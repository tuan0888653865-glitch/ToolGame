using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

namespace TinhKiemAuto
{
	// Token: 0x02000114 RID: 276
	public partial class ShutDown : Form
	{
		// Token: 0x06000EC9 RID: 3785 RVA: 0x00071B74 File Offset: 0x0006FD74
		public ShutDown()
		{
			this.InitializeComponent();
			base.FormBorderStyle = FormBorderStyle.None;
			base.Disposed += this.ShutDown_Disposed;
			base.Show();
		}

		// Token: 0x06000ECA RID: 3786 RVA: 0x00006740 File Offset: 0x00004940
		private void ShutDown_Disposed(object sender, EventArgs e)
		{
		}

		// Token: 0x06000ECB RID: 3787 RVA: 0x00019AB9 File Offset: 0x00017CB9
		private void button1_Click(object sender, EventArgs e)
		{
			base.Close();
		}

		// Token: 0x06000ECC RID: 3788 RVA: 0x00071BAC File Offset: 0x0006FDAC
		private void tmrCountDown_Tick(object sender, EventArgs e)
		{
			this.exitTime--;
			TimeSpan timeSpan = TimeSpan.FromSeconds((double)this.exitTime);
			this.label1.Text = "Tắt máy sau " + string.Format("{0:0}:{1:00}", timeSpan.Minutes, timeSpan.Seconds);
			if (this.exitTime == 0)
			{
				Process.Start("shutdown", "-s -t 0");
			}
		}

		// Token: 0x04000C82 RID: 3202
		private int exitTime = 60;
	}
}
