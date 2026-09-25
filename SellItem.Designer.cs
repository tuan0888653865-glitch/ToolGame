namespace TinhKiemAuto
{
	// Token: 0x02000113 RID: 275
	public partial class SellItem : global::System.Windows.Forms.Form
	{
		// Token: 0x06000EC7 RID: 3783 RVA: 0x00071306 File Offset: 0x0006F506
		protected override void Dispose(bool disposing)
		{
			if (disposing && this.components != null)
			{
				this.components.Dispose();
			}
			base.Dispose(disposing);
		}

		// Token: 0x06000EC8 RID: 3784 RVA: 0x00071328 File Offset: 0x0006F528
		private void InitializeComponent()
		{
			global::System.ComponentModel.ComponentResourceManager componentResourceManager = new global::System.ComponentModel.ComponentResourceManager(typeof(global::TinhKiemAuto.SellItem));
			this.button1 = new global::System.Windows.Forms.Button();
			this.label4 = new global::System.Windows.Forms.Label();
			this.label8 = new global::System.Windows.Forms.Label();
			this.buthemkieu = new global::System.Windows.Forms.Button();
			this.buthem = new global::System.Windows.Forms.Button();
			this.listViewTypeEx = new global::System.Windows.Forms.ListView();
			this.loaivatphamhuy = new global::System.Windows.Forms.ColumnHeader();
			this.listViewType = new global::System.Windows.Forms.ListView();
			this.loaivatpham = new global::System.Windows.Forms.ColumnHeader();
			this.ListViewNameEx = new global::System.Windows.Forms.ListView();
			this.tenvatphamboqua = new global::System.Windows.Forms.ColumnHeader();
			this.listViewName = new global::System.Windows.Forms.ListView();
			this.buttenvatpham = new global::System.Windows.Forms.ColumnHeader();
			this.label9 = new global::System.Windows.Forms.Label();
			base.SuspendLayout();
			this.button1.Location = new global::System.Drawing.Point(497, 606);
			this.button1.Name = "button1";
			this.button1.Size = new global::System.Drawing.Size(75, 29);
			this.button1.TabIndex = 80;
			this.button1.Text = "Lưu Lại";
			this.button1.UseVisualStyleBackColor = true;
			this.button1.Click += new global::System.EventHandler(this.button1_Click);
			this.label4.AutoSize = true;
			this.label4.Font = new global::System.Drawing.Font("Segoe UI Semibold", 8.25f, global::System.Drawing.FontStyle.Bold);
			this.label4.ForeColor = global::System.Drawing.Color.Red;
			this.label4.Location = new global::System.Drawing.Point(20, 604);
			this.label4.Name = "label4";
			this.label4.Size = new global::System.Drawing.Size(267, 13);
			this.label4.TabIndex = 79;
			this.label4.Text = "- Nhấn Delete Trên Phím Để Xóa Đồ Khỏi Danh Sách";
			this.label8.AutoSize = true;
			this.label8.Font = new global::System.Drawing.Font("Segoe UI Semibold", 8.25f, global::System.Drawing.FontStyle.Bold);
			this.label8.ForeColor = global::System.Drawing.Color.Red;
			this.label8.Location = new global::System.Drawing.Point(20, 624);
			this.label8.Name = "label8";
			this.label8.Size = new global::System.Drawing.Size(441, 13);
			this.label8.TabIndex = 78;
			this.label8.Text = "- Cẩn trọng khi sử dụng tính năng Hủy Đồ + Bán Đồ theo loại vì rất nhiều đồ cùng loại";
			this.buthemkieu.Location = new global::System.Drawing.Point(255, 430);
			this.buthemkieu.Name = "buthemkieu";
			this.buthemkieu.Size = new global::System.Drawing.Size(75, 29);
			this.buthemkieu.TabIndex = 77;
			this.buthemkieu.Text = "---->>>";
			this.buthemkieu.UseVisualStyleBackColor = true;
			this.buthem.Location = new global::System.Drawing.Point(255, 139);
			this.buthem.Name = "buthem";
			this.buthem.Size = new global::System.Drawing.Size(75, 32);
			this.buthem.TabIndex = 76;
			this.buthem.Text = "---->>>";
			this.buthem.UseVisualStyleBackColor = true;
			this.buthem.Click += new global::System.EventHandler(this.buthem_Click);
			this.listViewTypeEx.Columns.AddRange(new global::System.Windows.Forms.ColumnHeader[]
			{
				this.loaivatphamhuy
			});
			this.listViewTypeEx.GridLines = true;
			this.listViewTypeEx.HideSelection = false;
			this.listViewTypeEx.Location = new global::System.Drawing.Point(342, 311);
			this.listViewTypeEx.Name = "listViewTypeEx";
			this.listViewTypeEx.Size = new global::System.Drawing.Size(205, 276);
			this.listViewTypeEx.TabIndex = 75;
			this.listViewTypeEx.UseCompatibleStateImageBehavior = false;
			this.listViewTypeEx.View = global::System.Windows.Forms.View.Details;
			this.listViewTypeEx.KeyDown += new global::System.Windows.Forms.KeyEventHandler(this.listViewTypeEx_KeyDown);
			this.loaivatphamhuy.Text = "Loại Vật Phẩm Sẽ Bán";
			this.loaivatphamhuy.Width = 200;
			this.listViewType.Columns.AddRange(new global::System.Windows.Forms.ColumnHeader[]
			{
				this.loaivatpham
			});
			this.listViewType.GridLines = true;
			this.listViewType.HideSelection = false;
			this.listViewType.Location = new global::System.Drawing.Point(44, 311);
			this.listViewType.Name = "listViewType";
			this.listViewType.Size = new global::System.Drawing.Size(205, 276);
			this.listViewType.TabIndex = 74;
			this.listViewType.UseCompatibleStateImageBehavior = false;
			this.listViewType.View = global::System.Windows.Forms.View.Details;
			this.listViewType.DoubleClick += new global::System.EventHandler(this.listViewType_DoubleClick);
			this.loaivatpham.Text = "Các Loại Vật Phẩm Tìm Thấy Trong Túi";
			this.loaivatpham.Width = 200;
			this.ListViewNameEx.Columns.AddRange(new global::System.Windows.Forms.ColumnHeader[]
			{
				this.tenvatphamboqua
			});
			this.ListViewNameEx.GridLines = true;
			this.ListViewNameEx.HideSelection = false;
			this.ListViewNameEx.Location = new global::System.Drawing.Point(342, 29);
			this.ListViewNameEx.Name = "ListViewNameEx";
			this.ListViewNameEx.Size = new global::System.Drawing.Size(205, 276);
			this.ListViewNameEx.TabIndex = 73;
			this.ListViewNameEx.UseCompatibleStateImageBehavior = false;
			this.ListViewNameEx.View = global::System.Windows.Forms.View.Details;
			this.ListViewNameEx.KeyDown += new global::System.Windows.Forms.KeyEventHandler(this.ListViewNameEx_KeyDown);
			this.tenvatphamboqua.Text = "Danh Sách Vật Phẩm Sẽ Bán";
			this.tenvatphamboqua.Width = 200;
			this.listViewName.Columns.AddRange(new global::System.Windows.Forms.ColumnHeader[]
			{
				this.buttenvatpham
			});
			this.listViewName.GridLines = true;
			this.listViewName.HideSelection = false;
			this.listViewName.Location = new global::System.Drawing.Point(44, 29);
			this.listViewName.Name = "listViewName";
			this.listViewName.Size = new global::System.Drawing.Size(205, 276);
			this.listViewName.TabIndex = 72;
			this.listViewName.UseCompatibleStateImageBehavior = false;
			this.listViewName.View = global::System.Windows.Forms.View.Details;
			this.listViewName.DoubleClick += new global::System.EventHandler(this.listViewName_DoubleClick);
			this.buttenvatpham.Text = "Toàn Bộ Vật Phẩm Trong Túi";
			this.buttenvatpham.Width = 200;
			this.label9.AutoSize = true;
			this.label9.Font = new global::System.Drawing.Font("Segoe UI Semibold", 8.25f, global::System.Drawing.FontStyle.Bold);
			this.label9.ForeColor = global::System.Drawing.Color.Red;
			this.label9.Location = new global::System.Drawing.Point(20, 642);
			this.label9.Name = "label9";
			this.label9.Size = new global::System.Drawing.Size(501, 13);
			this.label9.TabIndex = 81;
			this.label9.Text = "- Auto sẽ không bán + hủy đồ trang bị có trang bị Điêu Văn hoặc Tinh Thông hoặc đã Khảm NGỌC\r\n";
			base.AutoScaleDimensions = new global::System.Drawing.SizeF(6f, 13f);
			base.AutoScaleMode = global::System.Windows.Forms.AutoScaleMode.Font;
			base.ClientSize = new global::System.Drawing.Size(593, 664);
			base.Controls.Add(this.label9);
			base.Controls.Add(this.button1);
			base.Controls.Add(this.label4);
			base.Controls.Add(this.label8);
			base.Controls.Add(this.buthemkieu);
			base.Controls.Add(this.buthem);
			base.Controls.Add(this.listViewTypeEx);
			base.Controls.Add(this.listViewType);
			base.Controls.Add(this.ListViewNameEx);
			base.Controls.Add(this.listViewName);
			base.Icon = (global::System.Drawing.Icon)componentResourceManager.GetObject("$this.Icon");
			base.Name = "SellItem";
			this.Text = "Danh Sách Vật Phẩm Sẽ Bán";
			base.Load += new global::System.EventHandler(this.SellItem_Load);
			base.ResumeLayout(false);
			base.PerformLayout();
		}

		// Token: 0x04000C73 RID: 3187
		private global::System.ComponentModel.IContainer components;

		// Token: 0x04000C74 RID: 3188
		private global::System.Windows.Forms.Button button1;

		// Token: 0x04000C75 RID: 3189
		private global::System.Windows.Forms.Label label4;

		// Token: 0x04000C76 RID: 3190
		private global::System.Windows.Forms.Label label8;

		// Token: 0x04000C77 RID: 3191
		private global::System.Windows.Forms.Button buthemkieu;

		// Token: 0x04000C78 RID: 3192
		private global::System.Windows.Forms.Button buthem;

		// Token: 0x04000C79 RID: 3193
		private global::System.Windows.Forms.ListView listViewTypeEx;

		// Token: 0x04000C7A RID: 3194
		private global::System.Windows.Forms.ColumnHeader loaivatphamhuy;

		// Token: 0x04000C7B RID: 3195
		private global::System.Windows.Forms.ListView listViewType;

		// Token: 0x04000C7C RID: 3196
		private global::System.Windows.Forms.ColumnHeader loaivatpham;

		// Token: 0x04000C7D RID: 3197
		private global::System.Windows.Forms.ListView ListViewNameEx;

		// Token: 0x04000C7E RID: 3198
		private global::System.Windows.Forms.ColumnHeader tenvatphamboqua;

		// Token: 0x04000C7F RID: 3199
		private global::System.Windows.Forms.ListView listViewName;

		// Token: 0x04000C80 RID: 3200
		private global::System.Windows.Forms.ColumnHeader buttenvatpham;

		// Token: 0x04000C81 RID: 3201
		private global::System.Windows.Forms.Label label9;
	}
}
