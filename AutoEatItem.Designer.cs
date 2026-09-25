namespace TinhKiemAuto
{
	// Token: 0x02000076 RID: 118
	public partial class AutoEatItem : global::System.Windows.Forms.Form
	{
		// Token: 0x06000461 RID: 1121 RVA: 0x00017640 File Offset: 0x00015840
		protected override void Dispose(bool disposing)
		{
			if (disposing && this.components != null)
			{
				this.components.Dispose();
			}
			base.Dispose(disposing);
		}

		// Token: 0x06000462 RID: 1122 RVA: 0x00017660 File Offset: 0x00015860
		private void InitializeComponent()
		{
			global::System.ComponentModel.ComponentResourceManager componentResourceManager = new global::System.ComponentModel.ComponentResourceManager(typeof(global::TinhKiemAuto.AutoEatItem));
			this.listViewName = new global::System.Windows.Forms.ListView();
			this.buttenquai = new global::System.Windows.Forms.ColumnHeader();
			this.listViewDuocAn = new global::System.Windows.Forms.ListView();
			this.columnHeader1 = new global::System.Windows.Forms.ColumnHeader();
			this.label5 = new global::System.Windows.Forms.Label();
			this.label4 = new global::System.Windows.Forms.Label();
			this.button1 = new global::System.Windows.Forms.Button();
			this.label3 = new global::System.Windows.Forms.Label();
			this.label23 = new global::System.Windows.Forms.Label();
			this.numphut = new global::System.Windows.Forms.NumericUpDown();
			this.button2 = new global::System.Windows.Forms.Button();
			((global::System.ComponentModel.ISupportInitialize)this.numphut).BeginInit();
			base.SuspendLayout();
			this.listViewName.Columns.AddRange(new global::System.Windows.Forms.ColumnHeader[]
			{
				this.buttenquai
			});
			this.listViewName.GridLines = true;
			this.listViewName.HideSelection = false;
			this.listViewName.Location = new global::System.Drawing.Point(12, 40);
			this.listViewName.Name = "listViewName";
			this.listViewName.Size = new global::System.Drawing.Size(203, 276);
			this.listViewName.TabIndex = 3;
			this.listViewName.UseCompatibleStateImageBehavior = false;
			this.listViewName.View = global::System.Windows.Forms.View.Details;
			this.buttenquai.Text = "Danh Sách Vật Phẩm Trong Túi";
			this.buttenquai.Width = 180;
			this.listViewDuocAn.Columns.AddRange(new global::System.Windows.Forms.ColumnHeader[]
			{
				this.columnHeader1
			});
			this.listViewDuocAn.GridLines = true;
			this.listViewDuocAn.HideSelection = false;
			this.listViewDuocAn.Location = new global::System.Drawing.Point(355, 40);
			this.listViewDuocAn.Name = "listViewDuocAn";
			this.listViewDuocAn.Size = new global::System.Drawing.Size(205, 276);
			this.listViewDuocAn.TabIndex = 4;
			this.listViewDuocAn.UseCompatibleStateImageBehavior = false;
			this.listViewDuocAn.View = global::System.Windows.Forms.View.Details;
			this.listViewDuocAn.KeyDown += new global::System.Windows.Forms.KeyEventHandler(this.listViewDuocAn_KeyDown);
			this.columnHeader1.Text = "Danh Sách Sử Dụng Tự Động";
			this.columnHeader1.Width = 200;
			this.label5.AutoSize = true;
			this.label5.Font = new global::System.Drawing.Font("Segoe UI Semibold", 8.25f, global::System.Drawing.FontStyle.Bold);
			this.label5.ForeColor = global::System.Drawing.Color.Red;
			this.label5.Location = new global::System.Drawing.Point(12, 339);
			this.label5.Name = "label5";
			this.label5.Size = new global::System.Drawing.Size(411, 13);
			this.label5.TabIndex = 71;
			this.label5.Text = "- Auto sẽ tự ăn tuần hoàn theo thời gian chỉ định nếu trong túi vẫn còn vật phẩm";
			this.label4.AutoSize = true;
			this.label4.Font = new global::System.Drawing.Font("Segoe UI Semibold", 8.25f, global::System.Drawing.FontStyle.Bold);
			this.label4.ForeColor = global::System.Drawing.Color.Red;
			this.label4.Location = new global::System.Drawing.Point(12, 363);
			this.label4.Name = "label4";
			this.label4.Size = new global::System.Drawing.Size(243, 13);
			this.label4.TabIndex = 70;
			this.label4.Text = "- Nhấn Delete để xóa vật phẩm khỏi danh sách";
			this.button1.Location = new global::System.Drawing.Point(476, 371);
			this.button1.Name = "button1";
			this.button1.Size = new global::System.Drawing.Size(75, 23);
			this.button1.TabIndex = 72;
			this.button1.Text = "Lưu Lại";
			this.button1.UseVisualStyleBackColor = true;
			this.button1.Click += new global::System.EventHandler(this.button1_Click);
			this.label3.AutoSize = true;
			this.label3.Font = new global::System.Drawing.Font("Segoe UI Semibold", 8.25f, global::System.Drawing.FontStyle.Bold, global::System.Drawing.GraphicsUnit.Point, 0);
			this.label3.ForeColor = global::System.Drawing.Color.Black;
			this.label3.Location = new global::System.Drawing.Point(217, 112);
			this.label3.Name = "label3";
			this.label3.Size = new global::System.Drawing.Size(41, 13);
			this.label3.TabIndex = 74;
			this.label3.Text = "Ăn Sau";
			this.label23.AutoSize = true;
			this.label23.Font = new global::System.Drawing.Font("Segoe UI Semibold", 8.25f, global::System.Drawing.FontStyle.Bold, global::System.Drawing.GraphicsUnit.Point, 0);
			this.label23.ForeColor = global::System.Drawing.Color.Black;
			this.label23.Location = new global::System.Drawing.Point(320, 112);
			this.label23.Name = "label23";
			this.label23.Size = new global::System.Drawing.Size(29, 13);
			this.label23.TabIndex = 73;
			this.label23.Text = "Phút";
			this.numphut.Location = new global::System.Drawing.Point(264, 107);
			global::System.Windows.Forms.NumericUpDown numericUpDown = this.numphut;
			int[] array = new int[4];
			array[0] = 200;
			numericUpDown.Maximum = new decimal(array);
			this.numphut.Name = "numphut";
			this.numphut.Size = new global::System.Drawing.Size(43, 20);
			this.numphut.TabIndex = 75;
			this.button2.Location = new global::System.Drawing.Point(249, 150);
			this.button2.Name = "button2";
			this.button2.Size = new global::System.Drawing.Size(75, 23);
			this.button2.TabIndex = 76;
			this.button2.Text = "--->>";
			this.button2.UseVisualStyleBackColor = true;
			this.button2.Click += new global::System.EventHandler(this.button2_Click);
			base.AutoScaleDimensions = new global::System.Drawing.SizeF(6f, 13f);
			base.AutoScaleMode = global::System.Windows.Forms.AutoScaleMode.Font;
			base.ClientSize = new global::System.Drawing.Size(572, 405);
			base.Controls.Add(this.button2);
			base.Controls.Add(this.numphut);
			base.Controls.Add(this.label3);
			base.Controls.Add(this.label23);
			base.Controls.Add(this.button1);
			base.Controls.Add(this.label5);
			base.Controls.Add(this.label4);
			base.Controls.Add(this.listViewDuocAn);
			base.Controls.Add(this.listViewName);
			base.Icon = (global::System.Drawing.Icon)componentResourceManager.GetObject("$this.Icon");
			base.MaximizeBox = false;
			this.MaximumSize = new global::System.Drawing.Size(588, 444);
			base.MinimizeBox = false;
			this.MinimumSize = new global::System.Drawing.Size(588, 444);
			base.Name = "AutoEatItem";
			this.Text = "AutoEatItem";
			base.Load += new global::System.EventHandler(this.AutoEatItem_Load);
			((global::System.ComponentModel.ISupportInitialize)this.numphut).EndInit();
			base.ResumeLayout(false);
			base.PerformLayout();
		}

		// Token: 0x040002D2 RID: 722
		private global::System.ComponentModel.IContainer components;

		// Token: 0x040002D3 RID: 723
		private global::System.Windows.Forms.ListView listViewName;

		// Token: 0x040002D4 RID: 724
		private global::System.Windows.Forms.ColumnHeader buttenquai;

		// Token: 0x040002D5 RID: 725
		private global::System.Windows.Forms.ListView listViewDuocAn;

		// Token: 0x040002D6 RID: 726
		private global::System.Windows.Forms.ColumnHeader columnHeader1;

		// Token: 0x040002D7 RID: 727
		private global::System.Windows.Forms.Label label5;

		// Token: 0x040002D8 RID: 728
		private global::System.Windows.Forms.Label label4;

		// Token: 0x040002D9 RID: 729
		private global::System.Windows.Forms.Button button1;

		// Token: 0x040002DA RID: 730
		private global::System.Windows.Forms.Label label3;

		// Token: 0x040002DB RID: 731
		private global::System.Windows.Forms.Label label23;

		// Token: 0x040002DC RID: 732
		private global::System.Windows.Forms.NumericUpDown numphut;

		// Token: 0x040002DD RID: 733
		private global::System.Windows.Forms.Button button2;
	}
}
