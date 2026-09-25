using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using TinhKiemAuto.Models;

namespace TinhKiemAuto
{
	// Token: 0x02000078 RID: 120
	public partial class Buff : Form
	{
		// Token: 0x06000470 RID: 1136 RVA: 0x0001882B File Offset: 0x00016A2B
		public Buff(Game game)
		{
			this.game = game;
			this.InitializeComponent();
		}

		// Token: 0x06000471 RID: 1137 RVA: 0x0001884C File Offset: 0x00016A4C
		public void LoadDanhSach()
		{
			this.DanhSachBuff.Clear();
			this.listViewName.Items.Clear();
			foreach (GameObject gameObject in this.game.Objects.AllPlayer.ToArray())
			{
				if (!this.game.Objects.Pk.Contains(gameObject) && gameObject.IsPlayer && gameObject.Name != "#G")
				{
					BuffPramenter buffPramenter = new BuffPramenter();
					buffPramenter.TenNguoiChoi = gameObject.Name;
					buffPramenter.IDnguoichoi = gameObject.TrueId.Replace("FFFFFFFF", "");
					this.DanhSachBuff.Add(buffPramenter);
					this.listViewName.Items.Add(gameObject.Name);
				}
			}
		}

		// Token: 0x06000472 RID: 1138 RVA: 0x00018924 File Offset: 0x00016B24
		public void LoadDuocBufff()
		{
			try
			{
				foreach (BuffPramenter buffPramenter in Setting.BuffValue)
				{
					this.ListViewNhanBuff.Items.Add(buffPramenter.TenNguoiChoi);
				}
			}
			catch
			{
			}
		}

		// Token: 0x06000473 RID: 1139 RVA: 0x00018998 File Offset: 0x00016B98
		private void Buff_Load(object sender, EventArgs e)
		{
			this.LoadDanhSach();
			this.LoadDuocBufff();
		}

		// Token: 0x06000474 RID: 1140 RVA: 0x000189A6 File Offset: 0x00016BA6
		private void button1_Click(object sender, EventArgs e)
		{
			this.LoadDanhSach();
		}

		// Token: 0x06000475 RID: 1141 RVA: 0x000189AE File Offset: 0x00016BAE
		private void button2_Click(object sender, EventArgs e)
		{
			Setting.SaveBuff();
			CanhBao.Msg("Thiết Lập Thành Công", "Lưu thiết lập buff NM thành công!", CanhBao.Kieu.OK);
			base.Close();
		}

		// Token: 0x06000476 RID: 1142 RVA: 0x000189CC File Offset: 0x00016BCC
		public void AddToSetting(string CharName)
		{
			BuffPramenter buffPramenter = this.DanhSachBuff.Find((BuffPramenter x) => x.TenNguoiChoi == CharName);
			if (buffPramenter != null)
			{
				Setting.BuffValue.Add(buffPramenter);
			}
		}

		// Token: 0x06000477 RID: 1143 RVA: 0x00018A0C File Offset: 0x00016C0C
		public void AddName()
		{
			foreach (object obj in this.listViewName.SelectedItems)
			{
				ListViewItem listViewItem = (ListViewItem)obj;
				if (listViewItem.Text.Trim() != "" && this.ListViewNhanBuff.FindItemWithText(listViewItem.Text.Trim()) == null)
				{
					this.ListViewNhanBuff.Items.Add(listViewItem.Text);
					this.AddToSetting(listViewItem.Text);
				}
			}
		}

		// Token: 0x06000478 RID: 1144 RVA: 0x00018AB8 File Offset: 0x00016CB8
		private void butthemdanhsach_Click(object sender, EventArgs e)
		{
			this.AddName();
		}

		// Token: 0x06000479 RID: 1145 RVA: 0x00018AC0 File Offset: 0x00016CC0
		public void Remove(string CharName)
		{
			int num = 0;
			BuffPramenter[] array = Setting.BuffValue.ToArray();
			for (int i = 0; i < array.Length; i++)
			{
				if (array[i].TenNguoiChoi == CharName)
				{
					Setting.BuffValue.RemoveAt(num);
				}
				num++;
			}
		}

		// Token: 0x0600047A RID: 1146 RVA: 0x00018B08 File Offset: 0x00016D08
		private void ListViewNhanBuff_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.KeyCode == Keys.Delete)
			{
				foreach (object obj in this.ListViewNhanBuff.SelectedItems)
				{
					ListViewItem listViewItem = (ListViewItem)obj;
					if (listViewItem.Text.Trim() != "")
					{
						listViewItem.Remove();
						this.Remove(listViewItem.Text.Trim());
					}
				}
			}
		}

		// Token: 0x0600047B RID: 1147 RVA: 0x00018AB8 File Offset: 0x00016CB8
		private void listViewName_DoubleClick(object sender, EventArgs e)
		{
			this.AddName();
		}

		// Token: 0x040002EA RID: 746
		private Game game;

		// Token: 0x040002EB RID: 747
		public List<BuffPramenter> DanhSachBuff = new List<BuffPramenter>();
	}
}
