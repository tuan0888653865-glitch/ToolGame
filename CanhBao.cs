using System;
using System.ComponentModel;
using System.Drawing;
using System.Media;
using System.Windows.Forms;
using TinhKiemAuto.Models;

namespace TinhKiemAuto
{
	// Token: 0x02000079 RID: 121
	public partial class CanhBao : Form
	{
		// Token: 0x170000EF RID: 239
		// (get) Token: 0x0600047E RID: 1150 RVA: 0x000191A1 File Offset: 0x000173A1
		public static CanhBao Instance
		{
			get
			{
				if (CanhBao._instance == null)
				{
					CanhBao._instance = new CanhBao("", "", CanhBao.Kieu.OK);
				}
				return CanhBao._instance;
			}
		}

		// Token: 0x0600047F RID: 1151 RVA: 0x000191C4 File Offset: 0x000173C4
		public static void AddLog(string tieude, string noidung, CanhBao.Kieu type)
		{
			if (CanhBao.Instance.InvokeRequired)
			{
				CanhBao.Instance.Invoke(new CanhBao.LogBack(CanhBao.AddLog), new object[]
				{
					tieude,
					noidung,
					type
				});
				return;
			}
			new CanhBao(tieude, noidung, type).Show();
		}

		// Token: 0x06000480 RID: 1152 RVA: 0x0001921C File Offset: 0x0001741C
		public CanhBao(string tieude, string noidung, CanhBao.Kieu type)
		{
			this.InitializeComponent();
			CanhBao._instance = this;
			this.txttieude.Text = tieude;
			this.txtnoidung.Text = noidung;
			switch (type)
			{
			case CanhBao.Kieu.OK:
				this.BackColor = Color.LightSeaGreen;
				this.picicon.Image = this.imageList1.Images[0];
				return;
			case CanhBao.Kieu.Info:
				this.BackColor = Color.DimGray;
				this.picicon.Image = this.imageList1.Images[1];
				return;
			case CanhBao.Kieu.Warning:
				this.BackColor = Color.DarkOrange;
				this.picicon.Image = this.imageList1.Images[2];
				return;
			case CanhBao.Kieu.Eror:
				this.BackColor = Color.Crimson;
				this.picicon.Image = this.imageList1.Images[3];
				try
				{
					using (SoundPlayer soundPlayer = new SoundPlayer("c:\\Windows\\Media\\notify.wav"))
					{
						soundPlayer.Play();
					}
				}
				catch
				{
				}
				return;
			default:
				return;
			}
		}

		// Token: 0x06000481 RID: 1153 RVA: 0x00019348 File Offset: 0x00017548
		public CanhBao()
		{
			this.InitializeComponent();
		}

		// Token: 0x06000482 RID: 1154 RVA: 0x00019356 File Offset: 0x00017556
		public static void Msg(string tieude, string noidung, CanhBao.Kieu type)
		{
			TienIch.CountMSG++;
			new CanhBao(tieude, noidung, type).Show();
		}

		// Token: 0x06000483 RID: 1155 RVA: 0x00019374 File Offset: 0x00017574
		private void CanhBao_Load(object sender, EventArgs e)
		{
			this.TopValue = base.Height * TienIch.CountMSG;
			base.Top = 0;
			base.Left = Screen.PrimaryScreen.Bounds.Width - base.Width;
			this.Show.Start();
		}

		// Token: 0x06000484 RID: 1156 RVA: 0x000193C4 File Offset: 0x000175C4
		private void timeout_Tick(object sender, EventArgs e)
		{
			if (TienIch.CountMSG > 0)
			{
				TienIch.CountMSG--;
			}
			this.Close.Start();
		}

		// Token: 0x06000485 RID: 1157 RVA: 0x000193E5 File Offset: 0x000175E5
		private void Show_Tick(object sender, EventArgs e)
		{
			if (base.Top < this.TopValue)
			{
				base.Top += this.invital;
				this.invital += 2;
				return;
			}
			this.Show.Stop();
		}

		// Token: 0x06000486 RID: 1158 RVA: 0x00019422 File Offset: 0x00017622
		private void Close_Tick(object sender, EventArgs e)
		{
			if (base.Opacity > 0.0)
			{
				base.Opacity -= 0.1;
				return;
			}
			base.Close();
		}

		// Token: 0x040002F6 RID: 758
		private static CanhBao _instance;

		// Token: 0x040002F7 RID: 759
		private int TopValue;

		// Token: 0x040002F8 RID: 760
		private int invital;

		// Token: 0x02000165 RID: 357
		// (Invoke) Token: 0x06001173 RID: 4467
		public delegate void LogBack(string tieude, string noidung, CanhBao.Kieu type);

		// Token: 0x02000166 RID: 358
		public enum Kieu
		{
			// Token: 0x04000E35 RID: 3637
			OK,
			// Token: 0x04000E36 RID: 3638
			Info,
			// Token: 0x04000E37 RID: 3639
			Warning,
			// Token: 0x04000E38 RID: 3640
			Eror
		}
	}
}
