using System;

namespace TinhKiemAuto
{
	// Token: 0x0200009D RID: 157
	internal interface IHttpFtpProtocolParameters
	{
		// Token: 0x1700024B RID: 587
		// (get) Token: 0x060009A9 RID: 2473
		// (set) Token: 0x060009AA RID: 2474
		string ProxyAddress { get; set; }

		// Token: 0x1700024C RID: 588
		// (get) Token: 0x060009AB RID: 2475
		// (set) Token: 0x060009AC RID: 2476
		string ProxyUserName { get; set; }

		// Token: 0x1700024D RID: 589
		// (get) Token: 0x060009AD RID: 2477
		// (set) Token: 0x060009AE RID: 2478
		string ProxyPassword { get; set; }

		// Token: 0x1700024E RID: 590
		// (get) Token: 0x060009AF RID: 2479
		// (set) Token: 0x060009B0 RID: 2480
		string ProxyDomain { get; set; }

		// Token: 0x1700024F RID: 591
		// (get) Token: 0x060009B1 RID: 2481
		// (set) Token: 0x060009B2 RID: 2482
		bool UseProxy { get; set; }

		// Token: 0x17000250 RID: 592
		// (get) Token: 0x060009B3 RID: 2483
		// (set) Token: 0x060009B4 RID: 2484
		bool ProxyByPassOnLocal { get; set; }

		// Token: 0x17000251 RID: 593
		// (get) Token: 0x060009B5 RID: 2485
		// (set) Token: 0x060009B6 RID: 2486
		int ProxyPort { get; set; }
	}
}
