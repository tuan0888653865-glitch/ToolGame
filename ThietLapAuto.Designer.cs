namespace TinhKiemAuto
{
	// Token: 0x0200011B RID: 283
	public partial class ThietLapAuto : global::System.Windows.Forms.Form
	{
		// Token: 0x06000F34 RID: 3892 RVA: 0x000739A6 File Offset: 0x00071BA6
		protected override void Dispose(bool disposing)
		{
			if (disposing && this.components != null)
			{
				this.components.Dispose();
			}
			base.Dispose(disposing);
		}

		// Token: 0x06000F35 RID: 3893 RVA: 0x000739C8 File Offset: 0x00071BC8
		private void InitializeComponent()
		{
			global::System.ComponentModel.ComponentResourceManager componentResourceManager = new global::System.ComponentModel.ComponentResourceManager(typeof(global::TinhKiemAuto.ThietLapAuto));
			this.label1 = new global::System.Windows.Forms.Label();
			this.combotrilieu = new global::System.Windows.Forms.ComboBox();
			this.combando = new global::System.Windows.Forms.ComboBox();
			this.label2 = new global::System.Windows.Forms.Label();
			this.checkBaoPK = new global::System.Windows.Forms.CheckBox();
			this.checkExitIfPK = new global::System.Windows.Forms.CheckBox();
			this.button1 = new global::System.Windows.Forms.Button();
			base.SuspendLayout();
			this.label1.AutoSize = true;
			this.label1.Location = new global::System.Drawing.Point(51, 31);
			this.label1.Name = "label1";
			this.label1.Size = new global::System.Drawing.Size(110, 13);
			this.label1.TabIndex = 0;
			this.label1.Text = "Thiết lập điểm trị liệu :";
			this.combotrilieu.DropDownStyle = global::System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.combotrilieu.FormattingEnabled = true;
			this.combotrilieu.Items.AddRange(new object[]
			{
				"Lạc Dương",
				"Tô Châu",
				"Đại Lý",
				"Lâu Lan"
			});
			this.combotrilieu.Location = new global::System.Drawing.Point(171, 28);
			this.combotrilieu.Name = "combotrilieu";
			this.combotrilieu.Size = new global::System.Drawing.Size(162, 21);
			this.combotrilieu.TabIndex = 1;
			this.combotrilieu.SelectedIndexChanged += new global::System.EventHandler(this.combotrilieu_SelectedIndexChanged);
			this.combando.DropDownStyle = global::System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.combando.FormattingEnabled = true;
			this.combando.Items.AddRange(new object[]
			{
				"Lạc Dương",
				"Tô Châu",
				"Đại Lý",
				"Lâu Lan"
			});
			this.combando.Location = new global::System.Drawing.Point(171, 64);
			this.combando.Name = "combando";
			this.combando.Size = new global::System.Drawing.Size(162, 21);
			this.combando.TabIndex = 3;
			this.combando.SelectedIndexChanged += new global::System.EventHandler(this.combando_SelectedIndexChanged);
			this.label2.AutoSize = true;
			this.label2.Location = new global::System.Drawing.Point(52, 70);
			this.label2.Name = "label2";
			this.label2.Size = new global::System.Drawing.Size(111, 13);
			this.label2.TabIndex = 2;
			this.label2.Text = "Thiết lập điểm bán đồ";
			this.checkBaoPK.AutoSize = true;
			this.checkBaoPK.Location = new global::System.Drawing.Point(54, 115);
			this.checkBaoPK.Name = "checkBaoPK";
			this.checkBaoPK.Size = new global::System.Drawing.Size(126, 17);
			this.checkBaoPK.TabIndex = 4;
			this.checkBaoPK.Text = "Báo Động Nếu Bị PK";
			this.checkBaoPK.UseVisualStyleBackColor = true;
			this.checkBaoPK.CheckedChanged += new global::System.EventHandler(this.checkBaoPK_CheckedChanged);
			this.checkExitIfPK.AutoSize = true;
			this.checkExitIfPK.Location = new global::System.Drawing.Point(54, 153);
			this.checkExitIfPK.Name = "checkExitIfPK";
			this.checkExitIfPK.Size = new global::System.Drawing.Size(106, 17);
			this.checkExitIfPK.TabIndex = 6;
			this.checkExitIfPK.Text = "Thoát Nếu Bị PK";
			this.checkExitIfPK.UseVisualStyleBackColor = true;
			this.checkExitIfPK.CheckedChanged += new global::System.EventHandler(this.checkExitIfPK_CheckedChanged);
			this.button1.Location = new global::System.Drawing.Point(152, 228);
			this.button1.Name = "button1";
			this.button1.Size = new global::System.Drawing.Size(75, 23);
			this.button1.TabIndex = 7;
			this.button1.Text = "Lưu";
			this.button1.UseVisualStyleBackColor = true;
			this.button1.Click += new global::System.EventHandler(this.button1_Click);
			base.AutoScaleDimensions = new global::System.Drawing.SizeF(6f, 13f);
			base.AutoScaleMode = global::System.Windows.Forms.AutoScaleMode.Font;
			base.ClientSize = new global::System.Drawing.Size(421, 274);
			base.Controls.Add(this.button1);
			base.Controls.Add(this.checkExitIfPK);
			base.Controls.Add(this.checkBaoPK);
			base.Controls.Add(this.combando);
			base.Controls.Add(this.label2);
			base.Controls.Add(this.combotrilieu);
			base.Controls.Add(this.label1);
			base.Icon = (global::System.Drawing.Icon)componentResourceManager.GetObject("$this.Icon");
			base.MaximizeBox = false;
			base.MinimizeBox = false;
			base.Name = "ThietLapAuto";
			base.StartPosition = global::System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "ThietLapAuto";
			base.Load += new global::System.EventHandler(this.ThietLapAuto_Load);
			base.ResumeLayout(false);
			base.PerformLayout();
		}

		// Token: 0x04000CA2 RID: 3234
		private global::System.ComponentModel.IContainer components;

		// Token: 0x04000CA3 RID: 3235
		private global::System.Windows.Forms.Label label1;

		// Token: 0x04000CA4 RID: 3236
		private global::System.Windows.Forms.ComboBox combotrilieu;

		// Token: 0x04000CA5 RID: 3237
		private global::System.Windows.Forms.ComboBox combando;

		// Token: 0x04000CA6 RID: 3238
		private global::System.Windows.Forms.Label label2;

		// Token: 0x04000CA7 RID: 3239
		private global::System.Windows.Forms.CheckBox checkBaoPK;

		// Token: 0x04000CA8 RID: 3240
		private global::System.Windows.Forms.CheckBox checkExitIfPK;

		// Token: 0x04000CA9 RID: 3241
		private global::System.Windows.Forms.Button button1;
	}
}
