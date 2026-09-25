using System;

namespace TinhKiemAuto
{
	// Token: 0x02000081 RID: 129
	internal static class BoolFormatter
	{
		// Token: 0x060005EF RID: 1519 RVA: 0x00021949 File Offset: 0x0001FB49
		public static bool FromString(string s)
		{
			return s == "Yes";
		}

		// Token: 0x060005F0 RID: 1520 RVA: 0x00021956 File Offset: 0x0001FB56
		public static string ToString(bool v)
		{
			if (v)
			{
				return "Yes";
			}
			return "No";
		}

		// Token: 0x040003CB RID: 971
		private const string Yes = "Yes";

		// Token: 0x040003CC RID: 972
		private const string No = "No";
	}
}
