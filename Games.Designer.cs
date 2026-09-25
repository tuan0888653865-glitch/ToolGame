namespace TinhKiemAuto
{
	// Token: 0x020000FB RID: 251
	internal partial class Games : global::System.Windows.Forms.Form
	{
		// Token: 0x06000E78 RID: 3704 RVA: 0x0006E19B File Offset: 0x0006C39B
		protected override void Dispose(bool disposing)
		{
			if (disposing && this.components != null)
			{
				this.components.Dispose();
			}
			base.Dispose(disposing);
		}

		// Token: 0x06000E79 RID: 3705 RVA: 0x0006E1BC File Offset: 0x0006C3BC
		private void InitializeComponent()
		{
			this.components = new global::System.ComponentModel.Container();
			global::System.ComponentModel.ComponentResourceManager componentResourceManager = new global::System.ComponentModel.ComponentResourceManager(typeof(global::TinhKiemAuto.Games));
			this.tab = new global::System.Windows.Forms.TabControl();
			this.timer1 = new global::System.Windows.Forms.Timer(this.components);
			this.timer2 = new global::System.Windows.Forms.Timer(this.components);
			base.SuspendLayout();
			this.tab.Dock = global::System.Windows.Forms.DockStyle.Fill;
			this.tab.Location = new global::System.Drawing.Point(0, 0);
			this.tab.Name = "tab";
			this.tab.SelectedIndex = 0;
			this.tab.Size = new global::System.Drawing.Size(984, 761);
			this.tab.TabIndex = 0;
			this.tab.MouseClick += new global::System.Windows.Forms.MouseEventHandler(this.tab_MouseClick);
			this.timer1.Enabled = true;
			this.timer1.Interval = 300;
			this.timer1.Tick += new global::System.EventHandler(this.timer1_Tick);
			this.timer2.Enabled = true;
			this.timer2.Interval = 1000;
			this.timer2.Tick += new global::System.EventHandler(this.timer2_Tick);
			base.AutoScaleDimensions = new global::System.Drawing.SizeF(6f, 13f);
			base.AutoScaleMode = global::System.Windows.Forms.AutoScaleMode.Font;
			base.ClientSize = new global::System.Drawing.Size(984, 761);
			base.Controls.Add(this.tab);
			base.Icon = (global::System.Drawing.Icon)componentResourceManager.GetObject("$this.Icon");
			base.Name = "Games";
			base.StartPosition = global::System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Games - Hiển thị Game thành tab";
			base.FormClosing += new global::System.Windows.Forms.FormClosingEventHandler(this.Games_FormClosing);
			base.Load += new global::System.EventHandler(this.Games_Load_1);
			base.ResizeEnd += new global::System.EventHandler(this.Games_ResizeEnd);
			base.SizeChanged += new global::System.EventHandler(this.Games_SizeChanged);
			base.MouseEnter += new global::System.EventHandler(this.Games_MouseEnter);
			base.MouseUp += new global::System.Windows.Forms.MouseEventHandler(this.Games_MouseUp);
			base.Resize += new global::System.EventHandler(this.Games_Resize);
			base.Validated += new global::System.EventHandler(this.Games_Validated);
			base.ResumeLayout(false);
		}

		// Token: 0x04000BE8 RID: 3048
		private global::System.ComponentModel.IContainer components;

		// Token: 0x04000BE9 RID: 3049
		private global::System.Windows.Forms.TabControl tab;

		// Token: 0x04000BEA RID: 3050
		private global::System.Windows.Forms.Timer timer1;

		// Token: 0x04000BEB RID: 3051
		private global::System.Windows.Forms.Timer timer2;
	}
}
