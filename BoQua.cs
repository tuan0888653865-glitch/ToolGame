using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace TinhKiemAuto
{
	// Token: 0x02000077 RID: 119
	public partial class BoQua : Form
	{
		// Token: 0x06000463 RID: 1123 RVA: 0x00017D93 File Offset: 0x00015F93
		public BoQua()
		{
			this.InitializeComponent();
		}

		// Token: 0x06000464 RID: 1124 RVA: 0x00017DAC File Offset: 0x00015FAC
		private void BoQua_Load(object sender, EventArgs e)
		{
			foreach (string text in Setting.BoQua.Split(new char[]
			{
				'\n'
			}))
			{
				if (text.Length > 0)
				{
					this.listviewboqua.Items.Add(text);
				}
			}
			foreach (KeyValuePair<int, Game> keyValuePair in FrmMain.dicGame)
			{
				foreach (GameObject gameObject in keyValuePair.Value.Objects.NearMonter20m)
				{
					if (!(gameObject.Name.Trim() == "") && !this.DropName.Contains(gameObject.Name))
					{
						this.DropName.Add(gameObject.Name);
						this.listViewName.Items.Add(gameObject.Name);
					}
				}
			}
		}

		// Token: 0x06000465 RID: 1125 RVA: 0x00017EE0 File Offset: 0x000160E0
		private void butlammoi_Click(object sender, EventArgs e)
		{
			this.listViewName.Items.Clear();
			this.DropName.Clear();
			foreach (KeyValuePair<int, Game> keyValuePair in FrmMain.dicGame)
			{
				foreach (GameObject gameObject in keyValuePair.Value.Objects.NearMonter20m)
				{
					if (!(gameObject.Name.Trim() == "") && !this.DropName.Contains(gameObject.Name))
					{
						this.DropName.Add(gameObject.Name);
						this.listViewName.Items.Add(gameObject.Name);
					}
				}
			}
		}

		// Token: 0x06000466 RID: 1126 RVA: 0x00017FE8 File Offset: 0x000161E8
		private void button1_Click(object sender, EventArgs e)
		{
			string text = "";
			foreach (object obj in this.listviewboqua.Items)
			{
				ListViewItem listViewItem = (ListViewItem)obj;
				if (listViewItem.Text.Trim() != "")
				{
					text = text + listViewItem.Text.Trim() + "\n";
				}
			}
			Setting.BoQua = text;
			CanhBao.Msg("Thiết Lập Thành Công", "Lưu thiết lập bỏ qua quái thành công!", CanhBao.Kieu.OK);
			base.Close();
		}

		// Token: 0x06000467 RID: 1127 RVA: 0x00018090 File Offset: 0x00016290
		private void menuDelete_Click(object sender, EventArgs e)
		{
			if (this.listviewboqua.SelectedItems.Count == 0)
			{
				return;
			}
			foreach (object obj in this.listviewboqua.SelectedItems)
			{
				ListViewItem listViewItem = (ListViewItem)obj;
				if (listViewItem.Text.Trim() != "")
				{
					listViewItem.Remove();
				}
			}
		}

		// Token: 0x06000468 RID: 1128 RVA: 0x00018118 File Offset: 0x00016318
		private void listviewboqua_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.KeyCode == Keys.Delete)
			{
				this.menuDelete_Click(null, null);
			}
		}

		// Token: 0x06000469 RID: 1129 RVA: 0x0001812C File Offset: 0x0001632C
		private void AddName()
		{
			foreach (object obj in this.listViewName.SelectedItems)
			{
				ListViewItem listViewItem = (ListViewItem)obj;
				if (listViewItem.Text.Trim() != "" && this.listviewboqua.FindItemWithText(listViewItem.Text.Trim()) == null)
				{
					this.listviewboqua.Items.Add(listViewItem.Text);
				}
			}
		}

		// Token: 0x0600046A RID: 1130 RVA: 0x000181CC File Offset: 0x000163CC
		private void butthemdanhsach_Click(object sender, EventArgs e)
		{
			this.AddName();
		}

		// Token: 0x0600046B RID: 1131 RVA: 0x000181CC File Offset: 0x000163CC
		private void listViewName_DoubleClick(object sender, EventArgs e)
		{
			this.AddName();
		}

		// Token: 0x0600046C RID: 1132 RVA: 0x00006740 File Offset: 0x00004940
		private void listViewName_SelectedIndexChanged(object sender, EventArgs e)
		{
		}

		// Token: 0x0600046D RID: 1133 RVA: 0x00006740 File Offset: 0x00004940
		private void listviewboqua_SelectedIndexChanged(object sender, EventArgs e)
		{
		}

		// Token: 0x040002DE RID: 734
		private HashSet<string> DropName = new HashSet<string>();
	}
}
