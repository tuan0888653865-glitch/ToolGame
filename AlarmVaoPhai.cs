using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace TinhKiemAuto.CostumeControlner
{
	// Token: 0x0200013C RID: 316
	public class AlarmVaoPhai : UserControl
	{
		// Token: 0x06001015 RID: 4117 RVA: 0x00076058 File Offset: 0x00074258
		public AlarmVaoPhai(Game game)
		{
			this.game = game;
			this.InitializeComponent();
			this.lblName.Text = game.TLBB.Name;
		}

		// Token: 0x06001016 RID: 4118 RVA: 0x00076083 File Offset: 0x00074283
		private void AlarmVaoPhai_Load(object sender, EventArgs e)
		{
			this.chkCoBan.Checked = this.game.IsNhiemVuCoBan;
		}

		// Token: 0x06001017 RID: 4119 RVA: 0x0007609C File Offset: 0x0007429C
		private void btnOk_Click(object sender, EventArgs e)
		{
			int num = this.cboMenpai.SelectedIndex + 1;
			if (num == 10)
			{
				num = 32;
			}
			if (num == 11)
			{
				num = 37;
			}
			if (num == 12)
			{
				num = 0;
			}
			if (num != 0)
			{
				this.game.IsSetMenPai = true;
				this.game.SetMenPai = (TINHKIEM.Menpai)num;
			}
			base.Dispose();
		}

		// Token: 0x06001018 RID: 4120 RVA: 0x000760EF File Offset: 0x000742EF
		private void lblAlarm_MouseClick(object sender, MouseEventArgs e)
		{
			if (e.Button == MouseButtons.Left)
			{
				this.game.Active();
			}
		}

		// Token: 0x06001019 RID: 4121 RVA: 0x000760EF File Offset: 0x000742EF
		private void lblName_MouseClick(object sender, MouseEventArgs e)
		{
			if (e.Button == MouseButtons.Left)
			{
				this.game.Active();
			}
		}

		// Token: 0x0600101A RID: 4122 RVA: 0x00076109 File Offset: 0x00074309
		private void lblClose_MouseClick(object sender, MouseEventArgs e)
		{
			if (e.Button == MouseButtons.Left)
			{
				base.Parent.Controls.Remove(this);
			}
		}

		// Token: 0x0600101B RID: 4123 RVA: 0x00076129 File Offset: 0x00074329
		private void chkCoBan_CheckedChanged(object sender, EventArgs e)
		{
			if (Global.IsVIP == 0)
			{
				this.chkCoBan.Checked = false;
			}
			this.game.IsNhiemVuCoBan = this.chkCoBan.Checked;
		}

		// Token: 0x0600101C RID: 4124 RVA: 0x00006740 File Offset: 0x00004940
		private void lblName_Click(object sender, EventArgs e)
		{
		}

		// Token: 0x0600101D RID: 4125 RVA: 0x00076154 File Offset: 0x00074354
		protected override void Dispose(bool disposing)
		{
			if (disposing && this.components != null)
			{
				this.components.Dispose();
			}
			base.Dispose(disposing);
		}

		// Token: 0x0600101E RID: 4126 RVA: 0x00076174 File Offset: 0x00074374
		private void InitializeComponent()
		{
			this.lblName = new Label();
			this.lblAlarm = new Label();
			this.cboMenpai = new ComboBox();
			this.label1 = new Label();
			this.btnOk = new Button();
			this.lblClose = new Label();
			this.chkCoBan = new CheckBox();
			base.SuspendLayout();
			this.lblName.Dock = DockStyle.Top;
			this.lblName.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, 0);
			this.lblName.ForeColor = Color.DarkGreen;
			this.lblName.Location = new Point(0, 0);
			this.lblName.Name = "lblName";
			this.lblName.Size = new Size(320, 23);
			this.lblName.TabIndex = 0;
			this.lblName.Text = "tinhkiem.us";
			this.lblName.TextAlign = ContentAlignment.MiddleCenter;
			this.lblName.Click += this.lblName_Click;
			this.lblName.MouseClick += this.lblName_MouseClick;
			this.lblAlarm.Cursor = Cursors.Hand;
			this.lblAlarm.Dock = DockStyle.Top;
			this.lblAlarm.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, 0);
			this.lblAlarm.ForeColor = Color.Red;
			this.lblAlarm.Location = new Point(0, 23);
			this.lblAlarm.Name = "lblAlarm";
			this.lblAlarm.Size = new Size(320, 48);
			this.lblAlarm.TabIndex = 1;
			this.lblAlarm.Text = "Tự động vào phái.\r\nChọn phái bạn muốn vào.\r\nAuto sẽ tự gia nhập phái cho bạn\r\n";
			this.lblAlarm.TextAlign = ContentAlignment.MiddleCenter;
			this.lblAlarm.Click += this.lblAlarm_Click;
			this.lblAlarm.MouseClick += this.lblAlarm_MouseClick;
			this.cboMenpai.DropDownStyle = ComboBoxStyle.DropDownList;
			this.cboMenpai.FormattingEnabled = true;
			this.cboMenpai.Items.AddRange(new object[]
			{
				"Thiếu Lâm",
				"Minh Giáo",
				"Cái Bang",
				"Võ Đang (Đánh Xa)",
				"Nga My (Đánh Xa)",
				"Tinh Túc (Đánh Xa)",
				"Thiên Long (Đánh Xa)",
				"Thiên Sơn",
				"Tiêu Dao (Đánh Xa)",
				"Mộ Dung",
				"Để Tôi Tự Vào"
			});
			this.cboMenpai.Location = new Point(59, 77);
			this.cboMenpai.Name = "cboMenpai";
			this.cboMenpai.Size = new Size(170, 21);
			this.cboMenpai.TabIndex = 2;
			this.label1.AutoSize = true;
			this.label1.Location = new Point(5, 81);
			this.label1.Name = "label1";
			this.label1.Size = new Size(50, 13);
			this.label1.TabIndex = 3;
			this.label1.Text = "Vào Phái";
			this.btnOk.Location = new Point(235, 76);
			this.btnOk.Name = "btnOk";
			this.btnOk.Size = new Size(75, 23);
			this.btnOk.TabIndex = 4;
			this.btnOk.Text = "Đồng Ý";
			this.btnOk.UseVisualStyleBackColor = true;
			this.btnOk.Click += this.btnOk_Click;
			this.lblClose.BackColor = Color.Silver;
			this.lblClose.Cursor = Cursors.Hand;
			this.lblClose.Location = new Point(300, 0);
			this.lblClose.Name = "lblClose";
			this.lblClose.Size = new Size(20, 23);
			this.lblClose.TabIndex = 5;
			this.lblClose.Text = "X";
			this.lblClose.TextAlign = ContentAlignment.MiddleCenter;
			this.lblClose.MouseClick += this.lblClose_MouseClick;
			this.chkCoBan.AutoSize = true;
			this.chkCoBan.Location = new Point(8, 104);
			this.chkCoBan.Name = "chkCoBan";
			this.chkCoBan.Size = new Size(240, 17);
			this.chkCoBan.TabIndex = 6;
			this.chkCoBan.Text = "Làm nhiệm vụ cơ bản để lên cấp và lấy vàng";
			this.chkCoBan.UseVisualStyleBackColor = true;
			this.chkCoBan.CheckedChanged += this.chkCoBan_CheckedChanged;
			this.BackColor = Color.White;
			base.Controls.Add(this.chkCoBan);
			base.Controls.Add(this.lblClose);
			base.Controls.Add(this.btnOk);
			base.Controls.Add(this.label1);
			base.Controls.Add(this.cboMenpai);
			base.Controls.Add(this.lblAlarm);
			base.Controls.Add(this.lblName);
			base.Name = "AlarmVaoPhai";
			base.Size = new Size(320, 130);
			base.Load += this.AlarmVaoPhai_Load;
			base.ResumeLayout(false);
			base.PerformLayout();
		}

		// Token: 0x0600101F RID: 4127 RVA: 0x00006740 File Offset: 0x00004940
		private void lblAlarm_Click(object sender, EventArgs e)
		{
		}

		// Token: 0x04000D0C RID: 3340
		private Game game;

		// Token: 0x04000D0D RID: 3341
		private IContainer components;

		// Token: 0x04000D0E RID: 3342
		private Label lblName;

		// Token: 0x04000D0F RID: 3343
		private Label lblAlarm;

		// Token: 0x04000D10 RID: 3344
		private ComboBox cboMenpai;

		// Token: 0x04000D11 RID: 3345
		private Label label1;

		// Token: 0x04000D12 RID: 3346
		private Button btnOk;

		// Token: 0x04000D13 RID: 3347
		private Label lblClose;

		// Token: 0x04000D14 RID: 3348
		private CheckBox chkCoBan;
	}
}
