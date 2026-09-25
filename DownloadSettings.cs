using System;
using System.Windows.Forms;

namespace TinhKiemAuto
{
	// Token: 0x0200008E RID: 142
	internal class DownloadSettings
	{
		// Token: 0x04000416 RID: 1046
		public static int MinSegmentSize = 204800;

		// Token: 0x04000417 RID: 1047
		public static int MinSegmentLeftToStartNewSegment = 30;

		// Token: 0x04000418 RID: 1048
		public static int RetryDelay = 5;

		// Token: 0x04000419 RID: 1049
		public static int MaxRetries = 10;

		// Token: 0x0400041A RID: 1050
		public static int MaxSegments = 64;

		// Token: 0x0400041B RID: 1051
		public static string DownloadFolder = Application.StartupPath + "\\Downloads";

		// Token: 0x0400041C RID: 1052
		public static string ProxyAddress = string.Empty;

		// Token: 0x0400041D RID: 1053
		public static string ProxyUserName = string.Empty;

		// Token: 0x0400041E RID: 1054
		public static string ProxyPassword = string.Empty;

		// Token: 0x0400041F RID: 1055
		public static string ProxyDomain = string.Empty;

		// Token: 0x04000420 RID: 1056
		public static bool UseProxy = false;

		// Token: 0x04000421 RID: 1057
		public static bool ProxyByPassOnLocal = false;

		// Token: 0x04000422 RID: 1058
		public static int ProxyPort = 80;
	}
}
