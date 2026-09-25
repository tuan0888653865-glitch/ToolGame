namespace TinhKiemAuto
{
	// Token: 0x02000114 RID: 276
	public partial class ShutDown : global::System.Windows.Forms.Form
	{
		// Token: 0x06000ECD RID: 3789 RVA: 0x00071C23 File Offset: 0x0006FE23
		protected override void Dispose(bool disposing)
		{
			if (disposing && this.components != null)
			{
				this.components.Dispose();
			}
			base.Dispose(disposing);
		}

		// Token: 0x06000ECE RID: 3790 RVA: 0x00071C44 File Offset: 0x0006FE44
		private void InitializeComponent()
		{
			this.components = new global::System.ComponentModel.Container();
			this.label1 = new global::System.Windows.Forms.Label();
			this.button1 = new global::System.Windows.Forms.Button();
			this.tmrCountDown = new global::System.Windows.Forms.Timer(this.components);
			base.SuspendLayout();
			this.label1.AutoSize = true;
			this.label1.Font = new global::System.Drawing.Font("Microsoft Sans Serif", 15.75f, global::System.Drawing.FontStyle.Regular, global::System.Drawing.GraphicsUnit.Point, 0);
			this.label1.ForeColor = global::System.Drawing.SystemColors.Window;
			this.label1.Location = new global::System.Drawing.Point(63, 58);
			this.label1.Name = "label1";
			this.label1.Size = new global::System.Drawing.Size(99, 25);
			this.label1.TabIndex = 0;
			this.label1.Text = "Tắt Sau :";
			this.button1.Location = new global::System.Drawing.Point(144, 103);
			this.button1.Name = "button1";
			this.button1.Size = new global::System.Drawing.Size(75, 35);
			this.button1.TabIndex = 1;
			this.button1.Text = "Hủy";
			this.button1.UseVisualStyleBackColor = true;
			this.button1.Click += new global::System.EventHandler(this.button1_Click);
			this.tmrCountDown.Enabled = true;
			this.tmrCountDown.Interval = 1000;
			this.tmrCountDown.Tick += new global::System.EventHandler(this.tmrCountDown_Tick);
			base.AutoScaleDimensions = new global::System.Drawing.SizeF(6f, 13f);
			base.AutoScaleMode = global::System.Windows.Forms.AutoScaleMode.Font;
			this.BackColor = global::System.Drawing.Color.IndianRed;
			base.ClientSize = new global::System.Drawing.Size(370, 160);
			base.Controls.Add(this.button1);
			base.Controls.Add(this.label1);
			base.FormBorderStyle = global::System.Windows.Forms.FormBorderStyle.None;
			base.Name = "ShutDown";
			base.StartPosition = global::System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "ShutDown";
			base.ResumeLayout(false);
			base.PerformLayout();
		}

		// Token: 0x04000C83 RID: 3203
		private global::System.ComponentModel.IContainer components;

		// Token: 0x04000C84 RID: 3204
		private global::System.Windows.Forms.Label label1;

		// Token: 0x04000C85 RID: 3205
		private global::System.Windows.Forms.Button button1;

		// Token: 0x04000C86 RID: 3206
		private global::System.Windows.Forms.Timer tmrCountDown;
	}
}
