namespace TinhKiemAuto
{
	// Token: 0x0200007B RID: 123
	public partial class Chat : global::System.Windows.Forms.Form
	{
		// Token: 0x06000495 RID: 1173 RVA: 0x00019B4F File Offset: 0x00017D4F
		protected override void Dispose(bool disposing)
		{
			if (disposing && this.components != null)
			{
				this.components.Dispose();
			}
			base.Dispose(disposing);
		}

		// Token: 0x06000496 RID: 1174 RVA: 0x00019B70 File Offset: 0x00017D70
		private void InitializeComponent()
		{
			global::System.ComponentModel.ComponentResourceManager componentResourceManager = new global::System.ComponentModel.ComponentResourceManager(typeof(global::TinhKiemAuto.Chat));
			this.checknear = new global::System.Windows.Forms.CheckBox();
			this.checkbig_world = new global::System.Windows.Forms.CheckBox();
			this.checkmenpai = new global::System.Windows.Forms.CheckBox();
			this.checkteam = new global::System.Windows.Forms.CheckBox();
			this.checkguild = new global::System.Windows.Forms.CheckBox();
			this.checkguild_league = new global::System.Windows.Forms.CheckBox();
			this.button1 = new global::System.Windows.Forms.Button();
			base.SuspendLayout();
			this.checknear.AutoSize = true;
			this.checknear.Location = new global::System.Drawing.Point(56, 28);
			this.checknear.Name = "checknear";
			this.checknear.Size = new global::System.Drawing.Size(71, 17);
			this.checknear.TabIndex = 0;
			this.checknear.Text = "Chat Gần";
			this.checknear.UseVisualStyleBackColor = true;
			this.checknear.CheckedChanged += new global::System.EventHandler(this.checknear_CheckedChanged);
			this.checkbig_world.AutoSize = true;
			this.checkbig_world.Location = new global::System.Drawing.Point(56, 71);
			this.checkbig_world.Name = "checkbig_world";
			this.checkbig_world.Size = new global::System.Drawing.Size(66, 17);
			this.checkbig_world.TabIndex = 1;
			this.checkbig_world.Text = "Thế Giới";
			this.checkbig_world.UseVisualStyleBackColor = true;
			this.checkbig_world.CheckedChanged += new global::System.EventHandler(this.checkbig_world_CheckedChanged);
			this.checkmenpai.AutoSize = true;
			this.checkmenpai.Location = new global::System.Drawing.Point(56, 114);
			this.checkmenpai.Name = "checkmenpai";
			this.checkmenpai.Size = new global::System.Drawing.Size(96, 17);
			this.checkmenpai.TabIndex = 2;
			this.checkmenpai.Text = "Chat Môn Phái";
			this.checkmenpai.UseVisualStyleBackColor = true;
			this.checkmenpai.CheckedChanged += new global::System.EventHandler(this.checkmenpai_CheckedChanged);
			this.checkteam.AutoSize = true;
			this.checkteam.Location = new global::System.Drawing.Point(56, 157);
			this.checkteam.Name = "checkteam";
			this.checkteam.Size = new global::System.Drawing.Size(67, 17);
			this.checkteam.TabIndex = 3;
			this.checkteam.Text = "Chat Đội";
			this.checkteam.UseVisualStyleBackColor = true;
			this.checkteam.CheckedChanged += new global::System.EventHandler(this.checkteam_CheckedChanged);
			this.checkguild.AutoSize = true;
			this.checkguild.Location = new global::System.Drawing.Point(56, 200);
			this.checkguild.Name = "checkguild";
			this.checkguild.Size = new global::System.Drawing.Size(76, 17);
			this.checkguild.TabIndex = 4;
			this.checkguild.Text = "Chat Bang";
			this.checkguild.UseVisualStyleBackColor = true;
			this.checkguild.CheckedChanged += new global::System.EventHandler(this.checkguild_CheckedChanged);
			this.checkguild_league.AutoSize = true;
			this.checkguild_league.Location = new global::System.Drawing.Point(56, 243);
			this.checkguild_league.Name = "checkguild_league";
			this.checkguild_league.Size = new global::System.Drawing.Size(78, 17);
			this.checkguild_league.TabIndex = 5;
			this.checkguild_league.Text = "Đồng Minh";
			this.checkguild_league.UseVisualStyleBackColor = true;
			this.checkguild_league.CheckedChanged += new global::System.EventHandler(this.checkguild_league_CheckedChanged);
			this.button1.Location = new global::System.Drawing.Point(56, 283);
			this.button1.Name = "button1";
			this.button1.Size = new global::System.Drawing.Size(75, 23);
			this.button1.TabIndex = 6;
			this.button1.Text = "OK";
			this.button1.UseVisualStyleBackColor = true;
			this.button1.Click += new global::System.EventHandler(this.button1_Click);
			base.AutoScaleDimensions = new global::System.Drawing.SizeF(6f, 13f);
			base.AutoScaleMode = global::System.Windows.Forms.AutoScaleMode.Font;
			base.ClientSize = new global::System.Drawing.Size(190, 332);
			base.Controls.Add(this.button1);
			base.Controls.Add(this.checkguild_league);
			base.Controls.Add(this.checkguild);
			base.Controls.Add(this.checkteam);
			base.Controls.Add(this.checkmenpai);
			base.Controls.Add(this.checkbig_world);
			base.Controls.Add(this.checknear);
			base.Icon = (global::System.Drawing.Icon)componentResourceManager.GetObject("$this.Icon");
			base.MaximizeBox = false;
			base.MinimizeBox = false;
			base.Name = "Chat";
			base.StartPosition = global::System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Cài Đặt Chat";
			base.Load += new global::System.EventHandler(this.Chat_Load);
			base.ResumeLayout(false);
			base.PerformLayout();
		}

		// Token: 0x04000304 RID: 772
		private global::System.ComponentModel.IContainer components;

		// Token: 0x04000305 RID: 773
		private global::System.Windows.Forms.CheckBox checknear;

		// Token: 0x04000306 RID: 774
		private global::System.Windows.Forms.CheckBox checkbig_world;

		// Token: 0x04000307 RID: 775
		private global::System.Windows.Forms.CheckBox checkmenpai;

		// Token: 0x04000308 RID: 776
		private global::System.Windows.Forms.CheckBox checkteam;

		// Token: 0x04000309 RID: 777
		private global::System.Windows.Forms.CheckBox checkguild;

		// Token: 0x0400030A RID: 778
		private global::System.Windows.Forms.CheckBox checkguild_league;

		// Token: 0x0400030B RID: 779
		private global::System.Windows.Forms.Button button1;
	}
}
