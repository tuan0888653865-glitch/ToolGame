using System;
using System.Collections.Generic;

namespace TinhKiemAuto
{
	// Token: 0x02000094 RID: 148
	internal class GAMEDIC
	{
		// Token: 0x04000626 RID: 1574
		public static HashSet<string> BoQua = new HashSet<string>
		{
			"Bích Lân Cương Thi",
			"Thực Phẩm Hỏng"
		};

		// Token: 0x04000627 RID: 1575
		public static HashSet<string> YenTuOBoQua = new HashSet<string>
		{
			"Yến Tử Ổ trang đinh",
			"Công Dã Càn",
			"Bao Bất Đồng",
			"Đặng Bách Xuyên",
			"Nhất Phẩm Đường Võ Sĩ"
		};

		// Token: 0x04000628 RID: 1576
		public static List<string> PhungMinhVuongLang = new List<string>
		{
			"35,36",
			"48,30",
			"60,36",
			"60,36",
			"65,49",
			"60,60",
			"49,66",
			"36,61",
			"30,49",
			"41,41",
			"55,41",
			"55,57",
			"41,57",
			"49,49"
		};

		// Token: 0x04000629 RID: 1577
		public static Dictionary<string, int> ThucAnPet = new Dictionary<string, int>();
	}
}
