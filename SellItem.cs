using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using TinhKiemAuto.Models;

namespace TinhKiemAuto
{
	// Token: 0x02000113 RID: 275
	public partial class SellItem : Form
	{
		// Token: 0x06000EBB RID: 3771 RVA: 0x00070D5C File Offset: 0x0006EF5C
		public SellItem()
		{
			this.InitializeComponent();
		}

		// Token: 0x06000EBC RID: 3772 RVA: 0x00070D6C File Offset: 0x0006EF6C
		private void SellItem_Load(object sender, EventArgs e)
		{
			HashSet<string> hashSet = new HashSet<string>();
			HashSet<string> hashSet2 = new HashSet<string>();
			foreach (KeyValuePair<int, Game> keyValuePair in FrmMain.dicGame)
			{
				foreach (PacketItem packetItem in PacketItem.EnumDrop(keyValuePair.Value))
				{
					if (!(packetItem.Name.Trim() == "") && !(packetItem.TypeName.Trim() == ""))
					{
						if (!hashSet.Contains(packetItem.Name))
						{
							hashSet.Add(packetItem.Name);
							this.listViewName.Items.Add(packetItem.Name);
						}
						if (!hashSet2.Contains(packetItem.TypeName))
						{
							hashSet2.Add(packetItem.TypeName);
							this.listViewType.Items.Add(packetItem.TypeName);
						}
					}
				}
			}
			this.loadData();
		}

		// Token: 0x06000EBD RID: 3773 RVA: 0x00070EB8 File Offset: 0x0006F0B8
		private void AddName()
		{
			foreach (object obj in this.listViewName.SelectedItems)
			{
				ListViewItem listViewItem = (ListViewItem)obj;
				if (listViewItem.Text.Trim() != "" && this.ListViewNameEx.FindItemWithText(listViewItem.Text.Trim()) == null)
				{
					this.ListViewNameEx.Items.Add(listViewItem.Text.Trim());
				}
			}
		}

		// Token: 0x06000EBE RID: 3774 RVA: 0x00070F5C File Offset: 0x0006F15C
		private void AddType()
		{
			foreach (object obj in this.listViewType.SelectedItems)
			{
				ListViewItem listViewItem = (ListViewItem)obj;
				if (listViewItem.Text.Trim() != "" && this.listViewTypeEx.FindItemWithText(listViewItem.Text.Trim()) == null)
				{
					this.listViewTypeEx.Items.Add(listViewItem.Text.Trim());
				}
			}
		}

		// Token: 0x06000EBF RID: 3775 RVA: 0x00071000 File Offset: 0x0006F200
		public void Save()
		{
			string text = "";
			string text2 = "";
			foreach (object obj in this.ListViewNameEx.Items)
			{
				ListViewItem listViewItem = (ListViewItem)obj;
				if (listViewItem.Text.Trim() != "")
				{
					text = text + listViewItem.Text.Trim() + "\n";
				}
			}
			foreach (object obj2 in this.listViewTypeEx.Items)
			{
				ListViewItem listViewItem2 = (ListViewItem)obj2;
				if (listViewItem2.Text.Trim() != "")
				{
					text2 = text2 + listViewItem2.Text.Trim() + "\n";
				}
			}
			LoadFile.WriteFileWithEncrypt(text, Global.DataPath + "\\SellName.dat");
			LoadFile.WriteFileWithEncrypt(text2, Global.DataPath + "\\SellType.dat");
		}

		// Token: 0x06000EC0 RID: 3776 RVA: 0x00071138 File Offset: 0x0006F338
		public void loadData()
		{
			string sellName = Setting.SellName;
			string sellType = Setting.SellType;
			foreach (string text in sellName.Split(new char[]
			{
				'\n'
			}))
			{
				if (text.Length > 0)
				{
					this.ListViewNameEx.Items.Add(text);
				}
			}
			foreach (string text2 in sellType.Split(new char[]
			{
				'\n'
			}))
			{
				if (text2.Length > 0)
				{
					this.listViewTypeEx.Items.Add(text2);
				}
			}
		}

		// Token: 0x06000EC1 RID: 3777 RVA: 0x000711D0 File Offset: 0x0006F3D0
		private void listViewName_DoubleClick(object sender, EventArgs e)
		{
			this.AddName();
		}

		// Token: 0x06000EC2 RID: 3778 RVA: 0x000711D0 File Offset: 0x0006F3D0
		private void buthem_Click(object sender, EventArgs e)
		{
			this.AddName();
		}

		// Token: 0x06000EC3 RID: 3779 RVA: 0x000711D8 File Offset: 0x0006F3D8
		private void ListViewNameEx_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.KeyCode == Keys.Delete)
			{
				if (this.ListViewNameEx.SelectedItems.Count == 0)
				{
					return;
				}
				foreach (object obj in this.ListViewNameEx.SelectedItems)
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

		// Token: 0x06000EC4 RID: 3780 RVA: 0x00071264 File Offset: 0x0006F464
		private void listViewTypeEx_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.KeyCode == Keys.Delete)
			{
				if (this.listViewTypeEx.SelectedItems.Count == 0)
				{
					return;
				}
				foreach (object obj in this.listViewTypeEx.SelectedItems)
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

		// Token: 0x06000EC5 RID: 3781 RVA: 0x000712F0 File Offset: 0x0006F4F0
		private void button1_Click(object sender, EventArgs e)
		{
			this.Save();
			base.Close();
		}

		// Token: 0x06000EC6 RID: 3782 RVA: 0x000712FE File Offset: 0x0006F4FE
		private void listViewType_DoubleClick(object sender, EventArgs e)
		{
			this.AddType();
		}
	}
}
