using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using TinhKiemAuto.Models;

namespace TinhKiemAuto
{
	// Token: 0x020000F9 RID: 249
	public partial class DropItem : Form
	{
		// Token: 0x06000D5C RID: 3420 RVA: 0x00059D70 File Offset: 0x00057F70
		public DropItem()
		{
			this.InitializeComponent();
		}

		// Token: 0x06000D5D RID: 3421 RVA: 0x00059D80 File Offset: 0x00057F80
		private void DropItem_Load(object sender, EventArgs e)
		{
			HashSet<string> hashSet = new HashSet<string>();
			HashSet<string> hashSet2 = new HashSet<string>();
			foreach (KeyValuePair<int, Game> keyValuePair in FrmMain.dicGame)
			{
				foreach (PacketItem packetItem in PacketItem.EnumDrop(keyValuePair.Value))
				{
					if (!(packetItem.Name.Trim() == "") && !(packetItem.TypeName.Trim() == "") && !packetItem.Name.Contains("_XML_"))
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

		// Token: 0x06000D5E RID: 3422 RVA: 0x00059EF4 File Offset: 0x000580F4
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

		// Token: 0x06000D5F RID: 3423 RVA: 0x00059F98 File Offset: 0x00058198
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

		// Token: 0x06000D60 RID: 3424 RVA: 0x0005A03C File Offset: 0x0005823C
		public void loadData()
		{
			string dropName = Setting.DropName;
			string dropType = Setting.DropType;
			foreach (string text in dropName.Split(new char[]
			{
				'\n'
			}))
			{
				if (text.Length > 0)
				{
					this.ListViewNameEx.Items.Add(text);
				}
			}
			foreach (string text2 in dropType.Split(new char[]
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

		// Token: 0x06000D61 RID: 3425 RVA: 0x0005A0D4 File Offset: 0x000582D4
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
			LoadFile.WriteFileWithEncrypt(text, Global.DataPath + "\\DropName.dat");
			LoadFile.WriteFileWithEncrypt(text2, Global.DataPath + "\\DropType.dat");
		}

		// Token: 0x06000D62 RID: 3426 RVA: 0x0005A20C File Offset: 0x0005840C
		private void button1_Click(object sender, EventArgs e)
		{
			this.Save();
			base.Close();
		}

		// Token: 0x06000D63 RID: 3427 RVA: 0x0005A21C File Offset: 0x0005841C
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

		// Token: 0x06000D64 RID: 3428 RVA: 0x0005A2A8 File Offset: 0x000584A8
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

		// Token: 0x06000D65 RID: 3429 RVA: 0x0005A334 File Offset: 0x00058534
		private void listViewName_DoubleClick(object sender, EventArgs e)
		{
			this.AddName();
		}

		// Token: 0x06000D66 RID: 3430 RVA: 0x0005A33C File Offset: 0x0005853C
		private void listViewType_DoubleClick(object sender, EventArgs e)
		{
			this.AddType();
		}

		// Token: 0x06000D67 RID: 3431 RVA: 0x0005A334 File Offset: 0x00058534
		private void buthem_Click(object sender, EventArgs e)
		{
			this.AddName();
		}

		// Token: 0x06000D68 RID: 3432 RVA: 0x0005A33C File Offset: 0x0005853C
		private void buthemkieu_Click(object sender, EventArgs e)
		{
			this.AddType();
		}
	}
}
