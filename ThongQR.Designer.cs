namespace TinhKiemAuto
{
	// Token: 0x0200011C RID: 284
	public partial class ThongQR : global::System.Windows.Forms.Form
	{
		// Token: 0x06000F38 RID: 3896 RVA: 0x00073F39 File Offset: 0x00072139
		protected override void Dispose(bool disposing)
		{
			if (disposing && this.components != null)
			{
				this.components.Dispose();
			}
			base.Dispose(disposing);
		}

		// Token: 0x06000F39 RID: 3897 RVA: 0x00073F58 File Offset: 0x00072158
		private void InitializeComponent()
		{
			global::System.ComponentModel.ComponentResourceManager componentResourceManager = new global::System.ComponentModel.ComponentResourceManager(typeof(global::TinhKiemAuto.ThongQR));
			this.pictureBox1 = new global::System.Windows.Forms.PictureBox();
			this.txtphantcung = new global::System.Windows.Forms.Label();
			((global::System.ComponentModel.ISupportInitialize)this.pictureBox1).BeginInit();
			base.SuspendLayout();
			this.pictureBox1.Location = new global::System.Drawing.Point(177, 33);
			this.pictureBox1.Name = "pictureBox1";
			this.pictureBox1.Size = new global::System.Drawing.Size(221, 208);
			this.pictureBox1.TabIndex = 0;
			this.pictureBox1.TabStop = false;
			this.txtphantcung.AutoSize = true;
			this.txtphantcung.Font = new global::System.Drawing.Font("Microsoft Sans Serif", 12f, global::System.Drawing.FontStyle.Regular, global::System.Drawing.GraphicsUnit.Point, 0);
			this.txtphantcung.Location = new global::System.Drawing.Point(32, 304);
			this.txtphantcung.Name = "txtphantcung";
			this.txtphantcung.Size = new global::System.Drawing.Size(96, 20);
			this.txtphantcung.TabIndex = 1;
			this.txtphantcung.Text = "Phân Cứng :";
			base.AutoScaleDimensions = new global::System.Drawing.SizeF(6f, 13f);
			base.AutoScaleMode = global::System.Windows.Forms.AutoScaleMode.Font;
			base.ClientSize = new global::System.Drawing.Size(550, 497);
			base.Controls.Add(this.txtphantcung);
			base.Controls.Add(this.pictureBox1);
			base.Icon = (global::System.Drawing.Icon)componentResourceManager.GetObject("$this.Icon");
			base.Name = "ThongQR";
			this.Text = "ThongQR";
			base.Load += new global::System.EventHandler(this.ThongQR_Load);
			((global::System.ComponentModel.ISupportInitialize)this.pictureBox1).EndInit();
			base.ResumeLayout(false);
			base.PerformLayout();
		}

		// Token: 0x04000CAA RID: 3242
		private global::System.ComponentModel.IContainer components;

		// Token: 0x04000CAB RID: 3243
		private global::System.Windows.Forms.PictureBox pictureBox1;

		// Token: 0x04000CAC RID: 3244
		private global::System.Windows.Forms.Label txtphantcung;
	}
}
