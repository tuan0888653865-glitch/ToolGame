namespace TinhKiemAuto
{
	// Token: 0x0200007A RID: 122
	public partial class ChangeLogs : global::System.Windows.Forms.Form
	{
		// Token: 0x0600048A RID: 1162 RVA: 0x000198CE File Offset: 0x00017ACE
		protected override void Dispose(bool disposing)
		{
			if (disposing && this.components != null)
			{
				this.components.Dispose();
			}
			base.Dispose(disposing);
		}

		// Token: 0x0600048B RID: 1163 RVA: 0x000198F0 File Offset: 0x00017AF0
		private void InitializeComponent()
		{
			global::System.ComponentModel.ComponentResourceManager componentResourceManager = new global::System.ComponentModel.ComponentResourceManager(typeof(global::TinhKiemAuto.ChangeLogs));
			this.richTextBox1 = new global::System.Windows.Forms.RichTextBox();
			base.SuspendLayout();
			this.richTextBox1.Dock = global::System.Windows.Forms.DockStyle.Fill;
			this.richTextBox1.Font = new global::System.Drawing.Font("Microsoft Sans Serif", 9.75f, global::System.Drawing.FontStyle.Regular, global::System.Drawing.GraphicsUnit.Point, 0);
			this.richTextBox1.Location = new global::System.Drawing.Point(0, 0);
			this.richTextBox1.Name = "richTextBox1";
			this.richTextBox1.Size = new global::System.Drawing.Size(625, 490);
			this.richTextBox1.TabIndex = 0;
			this.richTextBox1.Text = componentResourceManager.GetString("richTextBox1.Text");
			base.AutoScaleDimensions = new global::System.Drawing.SizeF(6f, 13f);
			base.AutoScaleMode = global::System.Windows.Forms.AutoScaleMode.Font;
			base.ClientSize = new global::System.Drawing.Size(625, 490);
			base.Controls.Add(this.richTextBox1);
			base.Icon = (global::System.Drawing.Icon)componentResourceManager.GetObject("$this.Icon");
			base.Name = "ChangeLogs";
			base.StartPosition = global::System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Thông tin cập nhật";
			base.ResumeLayout(false);
		}

		// Token: 0x04000302 RID: 770
		private global::System.ComponentModel.IContainer components;

		// Token: 0x04000303 RID: 771
		private global::System.Windows.Forms.RichTextBox richTextBox1;
	}
}
