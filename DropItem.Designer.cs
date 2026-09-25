namespace TinhKiemAuto
{
	// Token: 0x020000F9 RID: 249
	public partial class DropItem : global::System.Windows.Forms.Form
	{
		// Token: 0x06000D69 RID: 3433 RVA: 0x0005A344 File Offset: 0x00058544
		protected override void Dispose(bool disposing)
		{
			if (disposing && this.components != null)
			{
				this.components.Dispose();
			}
			base.Dispose(disposing);
		}

		// Token: 0x06000D6A RID: 3434 RVA: 0x0005A364 File Offset: 0x00058564
		private void InitializeComponent()
		{
			global::System.ComponentModel.ComponentResourceManager componentResourceManager = new global::System.ComponentModel.ComponentResourceManager(typeof(global::TinhKiemAuto.DropItem));
			this.listViewName = new global::System.Windows.Forms.ListView();
			this.buttenvatpham = new global::System.Windows.Forms.ColumnHeader();
			this.ListViewNameEx = new global::System.Windows.Forms.ListView();
			this.tenvatphamboqua = new global::System.Windows.Forms.ColumnHeader();
			this.listViewType = new global::System.Windows.Forms.ListView();
			this.loaivatpham = new global::System.Windows.Forms.ColumnHeader();
			this.listViewTypeEx = new global::System.Windows.Forms.ListView();
			this.loaivatphamhuy = new global::System.Windows.Forms.ColumnHeader();
			this.buthem = new global::System.Windows.Forms.Button();
			this.buthemkieu = new global::System.Windows.Forms.Button();
			this.label4 = new global::System.Windows.Forms.Label();
			this.label9 = new global::System.Windows.Forms.Label();
			this.label8 = new global::System.Windows.Forms.Label();
			this.button1 = new global::System.Windows.Forms.Button();
			base.SuspendLayout();
			this.listViewName.Columns.AddRange(new global::System.Windows.Forms.ColumnHeader[]
			{
				this.buttenvatpham
			});
			this.listViewName.GridLines = true;
			this.listViewName.HideSelection = false;
			this.listViewName.Location = new global::System.Drawing.Point(36, 29);
			this.listViewName.Name = "listViewName";
			this.listViewName.Size = new global::System.Drawing.Size(205, 276);
			this.listViewName.TabIndex = 0;
			this.listViewName.UseCompatibleStateImageBehavior = false;
			this.listViewName.View = global::System.Windows.Forms.View.Details;
			this.listViewName.DoubleClick += new global::System.EventHandler(this.listViewName_DoubleClick);
			this.buttenvatpham.Text = "Tên Vật Phẩm Trong Túi";
			this.buttenvatpham.Width = 200;
			this.ListViewNameEx.Columns.AddRange(new global::System.Windows.Forms.ColumnHeader[]
			{
				this.tenvatphamboqua
			});
			this.ListViewNameEx.GridLines = true;
			this.ListViewNameEx.HideSelection = false;
			this.ListViewNameEx.Location = new global::System.Drawing.Point(334, 29);
			this.ListViewNameEx.Name = "ListViewNameEx";
			this.ListViewNameEx.Size = new global::System.Drawing.Size(205, 276);
			this.ListViewNameEx.TabIndex = 1;
			this.ListViewNameEx.UseCompatibleStateImageBehavior = false;
			this.ListViewNameEx.View = global::System.Windows.Forms.View.Details;
			this.ListViewNameEx.KeyDown += new global::System.Windows.Forms.KeyEventHandler(this.ListViewNameEx_KeyDown);
			this.tenvatphamboqua.Text = "Tên Vật Phẩm Sẽ Hủy";
			this.tenvatphamboqua.Width = 200;
			this.listViewType.Columns.AddRange(new global::System.Windows.Forms.ColumnHeader[]
			{
				this.loaivatpham
			});
			this.listViewType.GridLines = true;
			this.listViewType.HideSelection = false;
			this.listViewType.Location = new global::System.Drawing.Point(36, 311);
			this.listViewType.Name = "listViewType";
			this.listViewType.Size = new global::System.Drawing.Size(205, 276);
			this.listViewType.TabIndex = 2;
			this.listViewType.UseCompatibleStateImageBehavior = false;
			this.listViewType.View = global::System.Windows.Forms.View.Details;
			this.listViewType.DoubleClick += new global::System.EventHandler(this.listViewType_DoubleClick);
			this.loaivatpham.Text = "Các Loại Vật Phẩm Tìm Thấy Trong Túi";
			this.loaivatpham.Width = 200;
			this.listViewTypeEx.Columns.AddRange(new global::System.Windows.Forms.ColumnHeader[]
			{
				this.loaivatphamhuy
			});
			this.listViewTypeEx.GridLines = true;
			this.listViewTypeEx.HideSelection = false;
			this.listViewTypeEx.Location = new global::System.Drawing.Point(334, 311);
			this.listViewTypeEx.Name = "listViewTypeEx";
			this.listViewTypeEx.Size = new global::System.Drawing.Size(205, 276);
			this.listViewTypeEx.TabIndex = 3;
			this.listViewTypeEx.UseCompatibleStateImageBehavior = false;
			this.listViewTypeEx.View = global::System.Windows.Forms.View.Details;
			this.listViewTypeEx.KeyDown += new global::System.Windows.Forms.KeyEventHandler(this.listViewTypeEx_KeyDown);
			this.loaivatphamhuy.Text = "Loại Vật Phẩm Sẽ Hủy";
			this.loaivatphamhuy.Width = 200;
			this.buthem.Location = new global::System.Drawing.Point(247, 139);
			this.buthem.Name = "buthem";
			this.buthem.Size = new global::System.Drawing.Size(75, 32);
			this.buthem.TabIndex = 4;
			this.buthem.Text = "---->>>";
			this.buthem.UseVisualStyleBackColor = true;
			this.buthem.Click += new global::System.EventHandler(this.buthem_Click);
			this.buthemkieu.Location = new global::System.Drawing.Point(247, 430);
			this.buthemkieu.Name = "buthemkieu";
			this.buthemkieu.Size = new global::System.Drawing.Size(75, 29);
			this.buthemkieu.TabIndex = 5;
			this.buthemkieu.Text = "---->>>";
			this.buthemkieu.UseVisualStyleBackColor = true;
			this.buthemkieu.Click += new global::System.EventHandler(this.buthemkieu_Click);
			this.label4.AutoSize = true;
			this.label4.Font = new global::System.Drawing.Font("Segoe UI Semibold", 8.25f, global::System.Drawing.FontStyle.Bold);
			this.label4.ForeColor = global::System.Drawing.Color.Red;
			this.label4.Location = new global::System.Drawing.Point(12, 594);
			this.label4.Name = "label4";
			this.label4.Size = new global::System.Drawing.Size(267, 13);
			this.label4.TabIndex = 70;
			this.label4.Text = "- Nhấn Delete Trên Phím Để Xóa Đồ Khỏi Danh Sách";
			this.label9.AutoSize = true;
			this.label9.Font = new global::System.Drawing.Font("Segoe UI Semibold", 8.25f, global::System.Drawing.FontStyle.Bold);
			this.label9.ForeColor = global::System.Drawing.Color.Red;
			this.label9.Location = new global::System.Drawing.Point(12, 636);
			this.label9.Name = "label9";
			this.label9.Size = new global::System.Drawing.Size(501, 13);
			this.label9.TabIndex = 69;
			this.label9.Text = "- Auto sẽ không bán + hủy đồ trang bị có trang bị Điêu Văn hoặc Tinh Thông hoặc đã Khảm NGỌC\r\n";
			this.label8.AutoSize = true;
			this.label8.Font = new global::System.Drawing.Font("Segoe UI Semibold", 8.25f, global::System.Drawing.FontStyle.Bold);
			this.label8.ForeColor = global::System.Drawing.Color.Red;
			this.label8.Location = new global::System.Drawing.Point(12, 614);
			this.label8.Name = "label8";
			this.label8.Size = new global::System.Drawing.Size(441, 13);
			this.label8.TabIndex = 68;
			this.label8.Text = "- Cẩn trọng khi sử dụng tính năng Hủy Đồ + Bán Đồ theo loại vì rất nhiều đồ cùng loại";
			this.button1.Location = new global::System.Drawing.Point(489, 606);
			this.button1.Name = "button1";
			this.button1.Size = new global::System.Drawing.Size(75, 29);
			this.button1.TabIndex = 71;
			this.button1.Text = "Lưu Lại";
			this.button1.UseVisualStyleBackColor = true;
			this.button1.Click += new global::System.EventHandler(this.button1_Click);
			base.AutoScaleDimensions = new global::System.Drawing.SizeF(6f, 13f);
			base.AutoScaleMode = global::System.Windows.Forms.AutoScaleMode.Font;
			base.ClientSize = new global::System.Drawing.Size(593, 663);
			base.Controls.Add(this.button1);
			base.Controls.Add(this.label4);
			base.Controls.Add(this.label9);
			base.Controls.Add(this.label8);
			base.Controls.Add(this.buthemkieu);
			base.Controls.Add(this.buthem);
			base.Controls.Add(this.listViewTypeEx);
			base.Controls.Add(this.listViewType);
			base.Controls.Add(this.ListViewNameEx);
			base.Controls.Add(this.listViewName);
			base.Icon = (global::System.Drawing.Icon)componentResourceManager.GetObject("$this.Icon");
			base.MaximizeBox = false;
			this.MaximumSize = new global::System.Drawing.Size(609, 702);
			base.MinimizeBox = false;
			this.MinimumSize = new global::System.Drawing.Size(609, 702);
			base.Name = "DropItem";
			this.Text = "Tự Hủy Vật Phẩm";
			base.Load += new global::System.EventHandler(this.DropItem_Load);
			base.ResumeLayout(false);
			base.PerformLayout();
		}

		// Token: 0x04000AAA RID: 2730
		private global::System.ComponentModel.IContainer components;

		// Token: 0x04000AAB RID: 2731
		private global::System.Windows.Forms.ListView listViewName;

		// Token: 0x04000AAC RID: 2732
		private global::System.Windows.Forms.ListView ListViewNameEx;

		// Token: 0x04000AAD RID: 2733
		private global::System.Windows.Forms.ListView listViewType;

		// Token: 0x04000AAE RID: 2734
		private global::System.Windows.Forms.ListView listViewTypeEx;

		// Token: 0x04000AAF RID: 2735
		private global::System.Windows.Forms.Button buthem;

		// Token: 0x04000AB0 RID: 2736
		private global::System.Windows.Forms.Button buthemkieu;

		// Token: 0x04000AB1 RID: 2737
		private global::System.Windows.Forms.ColumnHeader buttenvatpham;

		// Token: 0x04000AB2 RID: 2738
		private global::System.Windows.Forms.ColumnHeader tenvatphamboqua;

		// Token: 0x04000AB3 RID: 2739
		private global::System.Windows.Forms.ColumnHeader loaivatpham;

		// Token: 0x04000AB4 RID: 2740
		private global::System.Windows.Forms.ColumnHeader loaivatphamhuy;

		// Token: 0x04000AB5 RID: 2741
		private global::System.Windows.Forms.Label label4;

		// Token: 0x04000AB6 RID: 2742
		private global::System.Windows.Forms.Label label9;

		// Token: 0x04000AB7 RID: 2743
		private global::System.Windows.Forms.Label label8;

		// Token: 0x04000AB8 RID: 2744
		private global::System.Windows.Forms.Button button1;
	}
}
