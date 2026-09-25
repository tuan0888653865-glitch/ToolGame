namespace TinhKiemAuto
{
	// Token: 0x02000078 RID: 120
	public partial class Buff : global::System.Windows.Forms.Form
	{
		// Token: 0x0600047C RID: 1148 RVA: 0x00018B98 File Offset: 0x00016D98
		protected override void Dispose(bool disposing)
		{
			if (disposing && this.components != null)
			{
				this.components.Dispose();
			}
			base.Dispose(disposing);
		}

		// Token: 0x0600047D RID: 1149 RVA: 0x00018BB8 File Offset: 0x00016DB8
		private void InitializeComponent()
		{
			this.components = new global::System.ComponentModel.Container();
			global::System.ComponentModel.ComponentResourceManager componentResourceManager = new global::System.ComponentModel.ComponentResourceManager(typeof(global::TinhKiemAuto.Buff));
			this.ListViewNhanBuff = new global::System.Windows.Forms.ListView();
			this.columnHeader1 = new global::System.Windows.Forms.ColumnHeader();
			this.listViewName = new global::System.Windows.Forms.ListView();
			this.buttenquai = new global::System.Windows.Forms.ColumnHeader();
			this.butthemdanhsach = new global::System.Windows.Forms.Button();
			this.button1 = new global::System.Windows.Forms.Button();
			this.button2 = new global::System.Windows.Forms.Button();
			this.label4 = new global::System.Windows.Forms.Label();
			this.toolTip1 = new global::System.Windows.Forms.ToolTip(this.components);
			base.SuspendLayout();
			this.ListViewNhanBuff.Columns.AddRange(new global::System.Windows.Forms.ColumnHeader[]
			{
				this.columnHeader1
			});
			this.ListViewNhanBuff.GridLines = true;
			this.ListViewNhanBuff.HideSelection = false;
			this.ListViewNhanBuff.Location = new global::System.Drawing.Point(349, 41);
			this.ListViewNhanBuff.Name = "ListViewNhanBuff";
			this.ListViewNhanBuff.Size = new global::System.Drawing.Size(205, 276);
			this.ListViewNhanBuff.TabIndex = 6;
			this.toolTip1.SetToolTip(this.ListViewNhanBuff, componentResourceManager.GetString("ListViewNhanBuff.ToolTip"));
			this.ListViewNhanBuff.UseCompatibleStateImageBehavior = false;
			this.ListViewNhanBuff.View = global::System.Windows.Forms.View.Details;
			this.ListViewNhanBuff.KeyDown += new global::System.Windows.Forms.KeyEventHandler(this.ListViewNhanBuff_KeyDown);
			this.columnHeader1.Text = "Danh Sách Được Đồng Ý Tổ Đội";
			this.columnHeader1.Width = 200;
			this.listViewName.Columns.AddRange(new global::System.Windows.Forms.ColumnHeader[]
			{
				this.buttenquai
			});
			this.listViewName.GridLines = true;
			this.listViewName.HideSelection = false;
			this.listViewName.Location = new global::System.Drawing.Point(6, 41);
			this.listViewName.Name = "listViewName";
			this.listViewName.Size = new global::System.Drawing.Size(203, 276);
			this.listViewName.TabIndex = 5;
			this.toolTip1.SetToolTip(this.listViewName, componentResourceManager.GetString("listViewName.ToolTip"));
			this.listViewName.UseCompatibleStateImageBehavior = false;
			this.listViewName.View = global::System.Windows.Forms.View.Details;
			this.listViewName.DoubleClick += new global::System.EventHandler(this.listViewName_DoubleClick);
			this.buttenquai.Text = "Danh Sách Người Chơi Xung Quanh";
			this.buttenquai.Width = 190;
			this.butthemdanhsach.Location = new global::System.Drawing.Point(245, 106);
			this.butthemdanhsach.Name = "butthemdanhsach";
			this.butthemdanhsach.Size = new global::System.Drawing.Size(75, 23);
			this.butthemdanhsach.TabIndex = 7;
			this.butthemdanhsach.Text = "---->";
			this.toolTip1.SetToolTip(this.butthemdanhsach, componentResourceManager.GetString("butthemdanhsach.ToolTip"));
			this.butthemdanhsach.UseVisualStyleBackColor = true;
			this.butthemdanhsach.Click += new global::System.EventHandler(this.butthemdanhsach_Click);
			this.button1.Location = new global::System.Drawing.Point(55, 334);
			this.button1.Name = "button1";
			this.button1.Size = new global::System.Drawing.Size(75, 23);
			this.button1.TabIndex = 8;
			this.button1.Text = "Làm Mới";
			this.toolTip1.SetToolTip(this.button1, componentResourceManager.GetString("button1.ToolTip"));
			this.button1.UseVisualStyleBackColor = true;
			this.button1.Click += new global::System.EventHandler(this.button1_Click);
			this.button2.Location = new global::System.Drawing.Point(408, 334);
			this.button2.Name = "button2";
			this.button2.Size = new global::System.Drawing.Size(75, 23);
			this.button2.TabIndex = 9;
			this.button2.Text = "Lưu Lại";
			this.toolTip1.SetToolTip(this.button2, componentResourceManager.GetString("button2.ToolTip"));
			this.button2.UseVisualStyleBackColor = true;
			this.button2.Click += new global::System.EventHandler(this.button2_Click);
			this.label4.AutoSize = true;
			this.label4.Font = new global::System.Drawing.Font("Microsoft Sans Serif", 9f, global::System.Drawing.FontStyle.Regular, global::System.Drawing.GraphicsUnit.Point, 0);
			this.label4.ForeColor = global::System.Drawing.Color.Red;
			this.label4.Location = new global::System.Drawing.Point(12, 9);
			this.label4.Name = "label4";
			this.label4.Size = new global::System.Drawing.Size(396, 15);
			this.label4.TabIndex = 31;
			this.label4.Text = "- Người chơi sẽ ở trong danh sách sẽ tự động được chấp nhận vào tổ đội";
			base.AutoScaleDimensions = new global::System.Drawing.SizeF(6f, 13f);
			base.AutoScaleMode = global::System.Windows.Forms.AutoScaleMode.Font;
			base.ClientSize = new global::System.Drawing.Size(566, 379);
			base.Controls.Add(this.label4);
			base.Controls.Add(this.button2);
			base.Controls.Add(this.button1);
			base.Controls.Add(this.butthemdanhsach);
			base.Controls.Add(this.ListViewNhanBuff);
			base.Controls.Add(this.listViewName);
			base.Icon = (global::System.Drawing.Icon)componentResourceManager.GetObject("$this.Icon");
			base.MaximizeBox = false;
			this.MaximumSize = new global::System.Drawing.Size(582, 418);
			base.MinimizeBox = false;
			this.MinimumSize = new global::System.Drawing.Size(582, 418);
			base.Name = "Buff";
			base.StartPosition = global::System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Thiết Lập Tự Đồng Ý Tổ Đội";
			base.Load += new global::System.EventHandler(this.Buff_Load);
			base.ResumeLayout(false);
			base.PerformLayout();
		}

		// Token: 0x040002EC RID: 748
		private global::System.ComponentModel.IContainer components;

		// Token: 0x040002ED RID: 749
		private global::System.Windows.Forms.ListView ListViewNhanBuff;

		// Token: 0x040002EE RID: 750
		private global::System.Windows.Forms.ColumnHeader columnHeader1;

		// Token: 0x040002EF RID: 751
		private global::System.Windows.Forms.ListView listViewName;

		// Token: 0x040002F0 RID: 752
		private global::System.Windows.Forms.ColumnHeader buttenquai;

		// Token: 0x040002F1 RID: 753
		private global::System.Windows.Forms.Button butthemdanhsach;

		// Token: 0x040002F2 RID: 754
		private global::System.Windows.Forms.Button button1;

		// Token: 0x040002F3 RID: 755
		private global::System.Windows.Forms.Button button2;

		// Token: 0x040002F4 RID: 756
		private global::System.Windows.Forms.Label label4;

		// Token: 0x040002F5 RID: 757
		private global::System.Windows.Forms.ToolTip toolTip1;
	}
}
