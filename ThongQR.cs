using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using Zen.Barcode;

namespace TinhKiemAuto
{
	// Token: 0x0200011C RID: 284
	public partial class ThongQR : Form
	{
		// Token: 0x06000F36 RID: 3894 RVA: 0x00073EE4 File Offset: 0x000720E4
		public ThongQR()
		{
			this.InitializeComponent();
		}

		// Token: 0x06000F37 RID: 3895 RVA: 0x00073EF4 File Offset: 0x000720F4
		private void ThongQR_Load(object sender, EventArgs e)
		{
			this.txtphantcung.Text = "Hardwave ID : " + Class95.String_0;
			CodeQrBarcodeDraw codeQr = BarcodeDrawFactory.CodeQr;
			this.pictureBox1.Image = codeQr.Draw(Class95.String_0, 100);
		}
	}
}
