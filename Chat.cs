using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace TinhKiemAuto
{
	// Token: 0x0200007B RID: 123
	public partial class Chat : Form
	{
		// Token: 0x0600048C RID: 1164 RVA: 0x00019A21 File Offset: 0x00017C21
		public Chat()
		{
			this.InitializeComponent();
		}

		// Token: 0x0600048D RID: 1165 RVA: 0x00019A2F File Offset: 0x00017C2F
		private void checknear_CheckedChanged(object sender, EventArgs e)
		{
			FrmMain.CurGame.ChatGan = this.checknear.Checked;
		}

		// Token: 0x0600048E RID: 1166 RVA: 0x00019A46 File Offset: 0x00017C46
		private void checkbig_world_CheckedChanged(object sender, EventArgs e)
		{
			FrmMain.CurGame.ChatTheGioi = this.checkbig_world.Checked;
		}

		// Token: 0x0600048F RID: 1167 RVA: 0x00019A5D File Offset: 0x00017C5D
		private void checkmenpai_CheckedChanged(object sender, EventArgs e)
		{
			FrmMain.CurGame.ChatMonPhai = this.checkmenpai.Checked;
		}

		// Token: 0x06000490 RID: 1168 RVA: 0x00019A74 File Offset: 0x00017C74
		private void checkteam_CheckedChanged(object sender, EventArgs e)
		{
			FrmMain.CurGame.ChatDoi = this.checkteam.Checked;
		}

		// Token: 0x06000491 RID: 1169 RVA: 0x00019A8B File Offset: 0x00017C8B
		private void checkguild_CheckedChanged(object sender, EventArgs e)
		{
			FrmMain.CurGame.ChatBangPhai = this.checkguild.Checked;
		}

		// Token: 0x06000492 RID: 1170 RVA: 0x00019AA2 File Offset: 0x00017CA2
		private void checkguild_league_CheckedChanged(object sender, EventArgs e)
		{
			FrmMain.CurGame.ChatDongMinh = this.checkguild_league.Checked;
		}

		// Token: 0x06000493 RID: 1171 RVA: 0x00019AB9 File Offset: 0x00017CB9
		private void button1_Click(object sender, EventArgs e)
		{
			base.Close();
		}

		// Token: 0x06000494 RID: 1172 RVA: 0x00019AC4 File Offset: 0x00017CC4
		private void Chat_Load(object sender, EventArgs e)
		{
			this.checknear.Checked = FrmMain.CurGame.ChatGan;
			this.checkbig_world.Checked = FrmMain.CurGame.ChatTheGioi;
			this.checkmenpai.Checked = FrmMain.CurGame.ChatMonPhai;
			this.checkteam.Checked = FrmMain.CurGame.ChatDoi;
			this.checkguild.Checked = FrmMain.CurGame.ChatBangPhai;
			this.checkguild_league.Checked = FrmMain.CurGame.ChatDongMinh;
		}
	}
}
