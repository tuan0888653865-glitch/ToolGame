using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using TinhKiemAuto.Models;

namespace TinhKiemAuto
{
	// Token: 0x0200011B RID: 283
	public partial class ThietLapAuto : Form
	{
		// Token: 0x06000F2A RID: 3882 RVA: 0x00073870 File Offset: 0x00071A70
		public ThietLapAuto()
		{
			this.InitializeComponent();
		}

		// Token: 0x06000F2B RID: 3883 RVA: 0x00073880 File Offset: 0x00071A80
		private void ThietLapAuto_Load(object sender, EventArgs e)
		{
			this.checkBaoPK.Checked = Global.AlarmPk;
			this.checkExitIfPK.Checked = Global.ExitPk;
			this.combando.SelectedIndex = this.GetIndex(Option.MapBanDoIndex);
			this.combotrilieu.SelectedIndex = this.GetIndex(Option.MaptriLieuIndex);
		}

		// Token: 0x06000F2C RID: 3884 RVA: 0x000738D9 File Offset: 0x00071AD9
		private void checkBaoPK_CheckedChanged(object sender, EventArgs e)
		{
			Global.AlarmPk = this.checkBaoPK.Checked;
		}

		// Token: 0x06000F2D RID: 3885 RVA: 0x00006740 File Offset: 0x00004940
		private void checktuvePk_CheckedChanged(object sender, EventArgs e)
		{
		}

		// Token: 0x06000F2E RID: 3886 RVA: 0x000738EB File Offset: 0x00071AEB
		private void checkExitIfPK_CheckedChanged(object sender, EventArgs e)
		{
			Global.ExitPk = this.checkExitIfPK.Checked;
		}

		// Token: 0x06000F2F RID: 3887 RVA: 0x00073900 File Offset: 0x00071B00
		public int GetIndex(int MapINDEX)
		{
			int result = 0;
			switch (MapINDEX)
			{
			case 0:
				result = 0;
				break;
			case 1:
				result = 1;
				break;
			case 2:
				result = 2;
				break;
			default:
				if (MapINDEX == 246)
				{
					result = 3;
				}
				break;
			}
			return result;
		}

		// Token: 0x06000F30 RID: 3888 RVA: 0x0007393C File Offset: 0x00071B3C
		public int GetMapIndex(int selectIndex)
		{
			int result = 0;
			switch (selectIndex)
			{
			case 0:
				result = 0;
				break;
			case 1:
				result = 1;
				break;
			case 2:
				result = 2;
				break;
			case 3:
				result = 246;
				break;
			}
			return result;
		}

		// Token: 0x06000F31 RID: 3889 RVA: 0x00073976 File Offset: 0x00071B76
		private void combotrilieu_SelectedIndexChanged(object sender, EventArgs e)
		{
			Option.MaptriLieuIndex = this.GetMapIndex(this.combotrilieu.SelectedIndex);
		}

		// Token: 0x06000F32 RID: 3890 RVA: 0x00019AB9 File Offset: 0x00017CB9
		private void button1_Click(object sender, EventArgs e)
		{
			base.Close();
		}

		// Token: 0x06000F33 RID: 3891 RVA: 0x0007398E File Offset: 0x00071B8E
		private void combando_SelectedIndexChanged(object sender, EventArgs e)
		{
			Option.MapBanDoIndex = this.GetMapIndex(this.combando.SelectedIndex);
		}
	}
}
