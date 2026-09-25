using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using Newtonsoft.Json;
using TinhKiemAuto.Models;

namespace TinhKiemAuto
{
	// Token: 0x02000076 RID: 118
	public partial class AutoEatItem : Form
	{
		// Token: 0x0600045A RID: 1114 RVA: 0x00017220 File Offset: 0x00015420
		public AutoEatItem()
		{
			this.InitializeComponent();
		}

		// Token: 0x0600045B RID: 1115 RVA: 0x00017230 File Offset: 0x00015430
		private void button1_Click(object sender, EventArgs e)
		{
			List<AutoEat> list = new List<AutoEat>();
			foreach (object obj in this.listViewDuocAn.Items)
			{
				AutoEat item = (AutoEat)((ListViewItem)obj).Tag;
				list.Add(item);
			}
			Setting.AutoEat = JsonConvert.SerializeObject(list);
			FrmMain.CurGame.TudongAn = list;
			base.Close();
		}

		// Token: 0x0600045C RID: 1116 RVA: 0x000172BC File Offset: 0x000154BC
		public void LoadData()
		{
			if (Setting.AutoEat.Length > 0)
			{
				foreach (AutoEat autoEat in JsonConvert.DeserializeObject<List<AutoEat>>(Setting.AutoEat))
				{
					ListViewItem listViewItem = new ListViewItem
					{
						UseItemStyleForSubItems = false
					};
					listViewItem.Text = autoEat.VatPhamName + " | " + autoEat.TimeEach.ToString();
					listViewItem.Tag = autoEat;
					this.listViewDuocAn.Items.Add(listViewItem);
				}
			}
		}

		// Token: 0x0600045D RID: 1117 RVA: 0x00017364 File Offset: 0x00015564
		private void AutoEatItem_Load(object sender, EventArgs e)
		{
			HashSet<string> hashSet = new HashSet<string>();
			new HashSet<string>();
			foreach (KeyValuePair<int, Game> keyValuePair in FrmMain.dicGame)
			{
				foreach (PacketItem packetItem in PacketItem.EnumDrop(keyValuePair.Value))
				{
					if (!(packetItem.Name.Trim() == "") && !(packetItem.TypeName.Trim() == "") && !packetItem.Name.Contains("_XML_") && !hashSet.Contains(packetItem.Name))
					{
						hashSet.Add(packetItem.Name);
						this.listViewName.Items.Add(packetItem.Name);
					}
				}
			}
			this.LoadData();
		}

		// Token: 0x0600045E RID: 1118 RVA: 0x00017488 File Offset: 0x00015688
		public void AddName()
		{
			foreach (object obj in this.listViewName.SelectedItems)
			{
				ListViewItem listViewItem = (ListViewItem)obj;
				if (listViewItem.Text.Trim() != "")
				{
					ListViewItem listViewItem2 = new ListViewItem
					{
						UseItemStyleForSubItems = false
					};
					listViewItem2.Text = listViewItem.Text.Trim() + "  |  " + ((int)this.numphut.Value).ToString();
					listViewItem2.Tag = new AutoEat
					{
						StartEat = DateTime.Now,
						ID = new Random().Next(1, 99999),
						VatPhamName = listViewItem.Text.Trim(),
						TimeEach = (int)this.numphut.Value
					};
					this.listViewDuocAn.Items.Add(listViewItem2);
				}
			}
		}

		// Token: 0x0600045F RID: 1119 RVA: 0x000175AC File Offset: 0x000157AC
		private void button2_Click(object sender, EventArgs e)
		{
			this.AddName();
		}

		// Token: 0x06000460 RID: 1120 RVA: 0x000175B4 File Offset: 0x000157B4
		private void listViewDuocAn_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.KeyCode == Keys.Delete)
			{
				if (this.listViewDuocAn.SelectedItems.Count == 0)
				{
					return;
				}
				foreach (object obj in this.listViewDuocAn.SelectedItems)
				{
					ListViewItem listViewItem = (ListViewItem)obj;
					try
					{
						listViewItem.Remove();
					}
					catch
					{
					}
				}
			}
		}
	}
}
