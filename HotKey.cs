using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace TinhKiemAuto
{
	// Token: 0x020000FC RID: 252
	public partial class HotKey : Form
	{
		// Token: 0x06000E7A RID: 3706 RVA: 0x0006E400 File Offset: 0x0006C600
		public HotKey()
		{
			this.InitializeComponent();
		}

		// Token: 0x06000E7B RID: 3707 RVA: 0x00006740 File Offset: 0x00004940
		private void HotKey_Load(object sender, EventArgs e)
		{
		}

		// Token: 0x06000E7C RID: 3708 RVA: 0x0006E40E File Offset: 0x0006C60E
		private void button1_Click(object sender, EventArgs e)
		{
			FrmMain.Instance.UnSetHoKey();
			MessageBox.Show("Đã tắt HotKey");
		}

		// Token: 0x06000E7D RID: 3709 RVA: 0x0006E425 File Offset: 0x0006C625
		private void button2_Click(object sender, EventArgs e)
		{
			FrmMain.Instance.SetHotKey();
			MessageBox.Show("Đã bật Hotkey");
		}
	}
}
