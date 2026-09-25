namespace TinhKiemAuto
{
	// Token: 0x02000079 RID: 121
	public partial class CanhBao : global::System.Windows.Forms.Form
	{
		// Token: 0x06000487 RID: 1159 RVA: 0x00019452 File Offset: 0x00017652
		protected override void Dispose(bool disposing)
		{
			if (disposing && this.components != null)
			{
				this.components.Dispose();
			}
			base.Dispose(disposing);
		}

		// Token: 0x06000488 RID: 1160 RVA: 0x00019474 File Offset: 0x00017674
		private void InitializeComponent()
		{
			this.components = new global::System.ComponentModel.Container();
			global::System.ComponentModel.ComponentResourceManager componentResourceManager = new global::System.ComponentModel.ComponentResourceManager(typeof(global::TinhKiemAuto.CanhBao));
			this.txttieude = new global::System.Windows.Forms.Label();
			this.picicon = new global::System.Windows.Forms.PictureBox();
			this.txtnoidung = new global::System.Windows.Forms.Label();
			this.butclose = new global::System.Windows.Forms.PictureBox();
			this.imageList1 = new global::System.Windows.Forms.ImageList(this.components);
			this.timeout = new global::System.Windows.Forms.Timer(this.components);
			this.Show = new global::System.Windows.Forms.Timer(this.components);
			this.Close = new global::System.Windows.Forms.Timer(this.components);
			((global::System.ComponentModel.ISupportInitialize)this.picicon).BeginInit();
			((global::System.ComponentModel.ISupportInitialize)this.butclose).BeginInit();
			base.SuspendLayout();
			this.txttieude.AutoSize = true;
			this.txttieude.Font = new global::System.Drawing.Font("Segoe UI Semibold", 9.75f, global::System.Drawing.FontStyle.Bold, global::System.Drawing.GraphicsUnit.Point, 0);
			this.txttieude.ForeColor = global::System.Drawing.Color.White;
			this.txttieude.Location = new global::System.Drawing.Point(85, 9);
			this.txttieude.Name = "txttieude";
			this.txttieude.Size = new global::System.Drawing.Size(69, 17);
			this.txttieude.TabIndex = 2;
			this.txttieude.Text = "THIÊN HÀ";
			this.picicon.Location = new global::System.Drawing.Point(23, 33);
			this.picicon.Name = "picicon";
			this.picicon.Size = new global::System.Drawing.Size(51, 51);
			this.picicon.TabIndex = 3;
			this.picicon.TabStop = false;
			this.txtnoidung.AutoSize = true;
			this.txtnoidung.ForeColor = global::System.Drawing.Color.White;
			this.txtnoidung.Location = new global::System.Drawing.Point(85, 51);
			this.txtnoidung.Name = "txtnoidung";
			this.txtnoidung.Size = new global::System.Drawing.Size(71, 13);
			this.txtnoidung.TabIndex = 4;
			this.txtnoidung.Text = "Đã Chế Xong";
			this.butclose.Cursor = global::System.Windows.Forms.Cursors.Hand;
			this.butclose.Location = new global::System.Drawing.Point(262, -3);
			this.butclose.Name = "butclose";
			this.butclose.Size = new global::System.Drawing.Size(20, 19);
			this.butclose.SizeMode = global::System.Windows.Forms.PictureBoxSizeMode.StretchImage;
			this.butclose.TabIndex = 5;
			this.butclose.TabStop = false;
			this.imageList1.ImageStream = (global::System.Windows.Forms.ImageListStreamer)componentResourceManager.GetObject("imageList1.ImageStream");
			this.imageList1.TransparentColor = global::System.Drawing.Color.Transparent;
			this.imageList1.Images.SetKeyName(0, "Ok_50px.png");
			this.imageList1.Images.SetKeyName(1, "Info_48px.png");
			this.imageList1.Images.SetKeyName(2, "Warning Shield_52px.png");
			this.imageList1.Images.SetKeyName(3, "Delete_52px.png");
			this.timeout.Enabled = true;
			this.timeout.Interval = 5000;
			this.timeout.Tick += new global::System.EventHandler(this.timeout_Tick);
			this.Show.Interval = 10;
			this.Show.Tick += new global::System.EventHandler(this.Show_Tick);
			this.Close.Tick += new global::System.EventHandler(this.Close_Tick);
			base.AutoScaleDimensions = new global::System.Drawing.SizeF(6f, 13f);
			base.AutoScaleMode = global::System.Windows.Forms.AutoScaleMode.Font;
			this.BackColor = global::System.Drawing.SystemColors.ActiveCaptionText;
			base.ClientSize = new global::System.Drawing.Size(284, 111);
			base.Controls.Add(this.butclose);
			base.Controls.Add(this.txtnoidung);
			base.Controls.Add(this.picicon);
			base.Controls.Add(this.txttieude);
			base.FormBorderStyle = global::System.Windows.Forms.FormBorderStyle.None;
			base.MaximizeBox = false;
			base.MinimizeBox = false;
			base.Name = "CanhBao";
			base.ShowIcon = false;
			base.ShowInTaskbar = false;
			this.Text = "CanhBao";
			base.Load += new global::System.EventHandler(this.CanhBao_Load);
			((global::System.ComponentModel.ISupportInitialize)this.picicon).EndInit();
			((global::System.ComponentModel.ISupportInitialize)this.butclose).EndInit();
			base.ResumeLayout(false);
			base.PerformLayout();
		}

		// Token: 0x040002F9 RID: 761
		private global::System.ComponentModel.IContainer components;

		// Token: 0x040002FA RID: 762
		private global::System.Windows.Forms.Label txttieude;

		// Token: 0x040002FB RID: 763
		private global::System.Windows.Forms.PictureBox picicon;

		// Token: 0x040002FC RID: 764
		private global::System.Windows.Forms.Label txtnoidung;

		// Token: 0x040002FD RID: 765
		private global::System.Windows.Forms.PictureBox butclose;

		// Token: 0x040002FE RID: 766
		private global::System.Windows.Forms.ImageList imageList1;

		// Token: 0x040002FF RID: 767
		private global::System.Windows.Forms.Timer timeout;

		// Token: 0x04000300 RID: 768
		private new global::System.Windows.Forms.Timer Show;

		// Token: 0x04000301 RID: 769
		private new global::System.Windows.Forms.Timer Close;
	}
}
