using System;

namespace TinhKiemAuto
{
	// Token: 0x02000082 RID: 130
	internal static class ByteFormatter
	{
		// Token: 0x060005F1 RID: 1521 RVA: 0x00021968 File Offset: 0x0001FB68
		public static string ToString(long size)
		{
			if (size < 1024L)
			{
				return string.Format("{0} b", size);
			}
			if (size >= 1024L && size < 1048576L)
			{
				return string.Format("{0:0} KB", (float)size / 1024f);
			}
			if (size >= 1048576L && size < 1073741824L)
			{
				return string.Format("{0:0,###} MB", (float)size / 1024f);
			}
			return string.Format("{0:0,###.###} GB", (float)size / 1024f);
		}

		// Token: 0x040003CD RID: 973
		private const long KB = 1024L;

		// Token: 0x040003CE RID: 974
		private const long MB = 1048576L;

		// Token: 0x040003CF RID: 975
		private const long GB = 1073741824L;

		// Token: 0x040003D0 RID: 976
		private const string BFormatPattern = "{0} b";

		// Token: 0x040003D1 RID: 977
		private const string KBFormatPattern = "{0:0} KB";

		// Token: 0x040003D2 RID: 978
		private const string MBFormatPattern = "{0:0,###} MB";

		// Token: 0x040003D3 RID: 979
		private const string GBFormatPattern = "{0:0,###.###} GB";
	}
}
