namespace TinhKiemAuto
{
	// Token: 0x02000077 RID: 119
	public partial class BoQua : global::System.Windows.Forms.Form
	{
		// Token: 0x0600046E RID: 1134 RVA: 0x000181D4 File Offset: 0x000163D4
		protected override void Dispose(bool disposing)
		{
			if (disposing && this.components != null)
			{
				this.components.Dispose();
			}
			base.Dispose(disposing);
		}

		// Token: 0x0600046F RID: 1135 RVA: 0x000181F4 File Offset: 0x000163F4
		private void InitializeComponent()
		{
			this.components = new global::System.ComponentModel.Container();
			global::System.ComponentModel.ComponentResourceManager componentResourceManager = new global::System.ComponentModel.ComponentResourceManager(typeof(global::TinhKiemAuto.BoQua));
			this.label1 = new global::System.Windows.Forms.Label();
			this.label2 = new global::System.Windows.Forms.Label();
			this.listViewName = new global::System.Windows.Forms.ListView();
			this.buttenquai = new global::System.Windows.Forms.ColumnHeader();
			this.listviewboqua = new global::System.Windows.Forms.ListView();
			this.tenquai = new global::System.Windows.Forms.ColumnHeader();
			this.butthemdanhsach = new global::System.Windows.Forms.Button();
			this.butlammoi = new global::System.Windows.Forms.Button();
			this.button1 = new global::System.Windows.Forms.Button();
			this.toolTip1 = new global::System.Windows.Forms.ToolTip(this.components);
			base.SuspendLayout();
			this.label1.AutoSize = true;
			this.label1.Location = new global::System.Drawing.Point(23, 16);
			this.label1.Name = "label1";
			this.label1.Size = new global::System.Drawing.Size(149, 13);
			this.label1.TabIndex = 0;
			this.label1.Text = "Danh Sách Quái Xung Quanh";
			this.label2.AutoSize = true;
			this.label2.Location = new global::System.Drawing.Point(270, 16);
			this.label2.Name = "label2";
			this.label2.Size = new global::System.Drawing.Size(183, 13);
			this.label2.TabIndex = 1;
			this.label2.Text = "Danh Sách Quái Đang Được Bỏ Qua";
			this.listViewName.Columns.AddRange(new global::System.Windows.Forms.ColumnHeader[]
			{
				this.buttenquai
			});
			this.listViewName.GridLines = true;
			this.listViewName.HideSelection = false;
			this.listViewName.Location = new global::System.Drawing.Point(12, 32);
			this.listViewName.Name = "listViewName";
			this.listViewName.Size = new global::System.Drawing.Size(172, 276);
			this.listViewName.TabIndex = 2;
			this.toolTip1.SetToolTip(this.listViewName, "-Nhấn DoubleClick vào giao diện Tên Quái Xung Quanh để\r\nthêm dánh sách bỏ qua.\r\n-Chọn quái bên Tên Quái Bỏ Qua và nhấn Delete để xóa quái \r\nkhỏi danh sách bỏ qua\r\n");
			this.listViewName.UseCompatibleStateImageBehavior = false;
			this.listViewName.View = global::System.Windows.Forms.View.Details;
			this.listViewName.SelectedIndexChanged += new global::System.EventHandler(this.listViewName_SelectedIndexChanged);
			this.listViewName.DoubleClick += new global::System.EventHandler(this.listViewName_DoubleClick);
			this.buttenquai.Text = "Tên Quái Xung Quanh";
			this.buttenquai.Width = 160;
			this.listviewboqua.Columns.AddRange(new global::System.Windows.Forms.ColumnHeader[]
			{
				this.tenquai
			});
			this.listviewboqua.GridLines = true;
			this.listviewboqua.HideSelection = false;
			this.listviewboqua.Location = new global::System.Drawing.Point(273, 32);
			this.listviewboqua.Name = "listviewboqua";
			this.listviewboqua.Size = new global::System.Drawing.Size(172, 276);
			this.listviewboqua.TabIndex = 3;
			this.toolTip1.SetToolTip(this.listviewboqua, "-Nhấn DoubleClick vào giao diện Tên Quái Xung Quanh để\r\nthêm dánh sách bỏ qua.\r\n-Chọn quái bên Tên Quái Bỏ Qua và nhấn Delete để xóa quái \r\nkhỏi danh sách bỏ qua");
			this.listviewboqua.UseCompatibleStateImageBehavior = false;
			this.listviewboqua.View = global::System.Windows.Forms.View.Details;
			this.listviewboqua.SelectedIndexChanged += new global::System.EventHandler(this.listviewboqua_SelectedIndexChanged);
			this.listviewboqua.KeyDown += new global::System.Windows.Forms.KeyEventHandler(this.listviewboqua_KeyDown);
			this.tenquai.Text = "Tên Quái Bỏ Qua";
			this.tenquai.Width = 165;
			this.butthemdanhsach.Location = new global::System.Drawing.Point(190, 114);
			this.butthemdanhsach.Name = "butthemdanhsach";
			this.butthemdanhsach.Size = new global::System.Drawing.Size(75, 23);
			this.butthemdanhsach.TabIndex = 4;
			this.butthemdanhsach.Text = "---->";
			this.butthemdanhsach.UseVisualStyleBackColor = true;
			this.butthemdanhsach.Click += new global::System.EventHandler(this.butthemdanhsach_Click);
			this.butlammoi.Location = new global::System.Drawing.Point(49, 311);
			this.butlammoi.Name = "butlammoi";
			this.butlammoi.Size = new global::System.Drawing.Size(75, 23);
			this.butlammoi.TabIndex = 5;
			this.butlammoi.Text = "Làm Mới";
			this.toolTip1.SetToolTip(this.butlammoi, "Nhấn vào đây để làm mới danh sách quái xung quanh");
			this.butlammoi.UseVisualStyleBackColor = true;
			this.butlammoi.Click += new global::System.EventHandler(this.butlammoi_Click);
			this.button1.Location = new global::System.Drawing.Point(319, 311);
			this.button1.Name = "button1";
			this.button1.Size = new global::System.Drawing.Size(75, 23);
			this.button1.TabIndex = 6;
			this.button1.Text = "Lưu Lại";
			this.toolTip1.SetToolTip(this.button1, "Nhấn vào đây để lưu lại danh sách quái được bỏ qua");
			this.button1.UseVisualStyleBackColor = true;
			this.button1.Click += new global::System.EventHandler(this.button1_Click);
			base.AutoScaleDimensions = new global::System.Drawing.SizeF(6f, 13f);
			base.AutoScaleMode = global::System.Windows.Forms.AutoScaleMode.Font;
			base.ClientSize = new global::System.Drawing.Size(465, 339);
			base.Controls.Add(this.button1);
			base.Controls.Add(this.butlammoi);
			base.Controls.Add(this.butthemdanhsach);
			base.Controls.Add(this.listviewboqua);
			base.Controls.Add(this.listViewName);
			base.Controls.Add(this.label2);
			base.Controls.Add(this.label1);
			base.Icon = (global::System.Drawing.Icon)componentResourceManager.GetObject("$this.Icon");
			base.MaximizeBox = false;
			this.MaximumSize = new global::System.Drawing.Size(481, 378);
			base.MinimizeBox = false;
			this.MinimumSize = new global::System.Drawing.Size(481, 378);
			base.Name = "BoQua";
			base.StartPosition = global::System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Bỏ Qua Quái";
			base.Load += new global::System.EventHandler(this.BoQua_Load);
			base.ResumeLayout(false);
			base.PerformLayout();
		}

		// Token: 0x040002DF RID: 735
		private global::System.ComponentModel.IContainer components;

		// Token: 0x040002E0 RID: 736
		private global::System.Windows.Forms.Label label1;

		// Token: 0x040002E1 RID: 737
		private global::System.Windows.Forms.Label label2;

		// Token: 0x040002E2 RID: 738
		private global::System.Windows.Forms.ListView listViewName;

		// Token: 0x040002E3 RID: 739
		private global::System.Windows.Forms.ListView listviewboqua;

		// Token: 0x040002E4 RID: 740
		private global::System.Windows.Forms.Button butthemdanhsach;

		// Token: 0x040002E5 RID: 741
		private global::System.Windows.Forms.Button butlammoi;

		// Token: 0x040002E6 RID: 742
		private global::System.Windows.Forms.Button button1;

		// Token: 0x040002E7 RID: 743
		private global::System.Windows.Forms.ColumnHeader buttenquai;

		// Token: 0x040002E8 RID: 744
		private global::System.Windows.Forms.ColumnHeader tenquai;

		// Token: 0x040002E9 RID: 745
		private global::System.Windows.Forms.ToolTip toolTip1;
	}
}
