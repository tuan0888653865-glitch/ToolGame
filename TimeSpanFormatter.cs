using System;

namespace TinhKiemAuto
{
	// Token: 0x020000EA RID: 234
	internal static class TimeSpanFormatter
	{
		// Token: 0x06000BFF RID: 3071 RVA: 0x0004C87C File Offset: 0x0004AA7C
		public static string ToString(TimeSpan ts)
		{
			if (ts == TimeSpan.MaxValue)
			{
				return "?";
			}
			string text = ts.ToString();
			int num = text.LastIndexOf('.');
			if (num > 0)
			{
				return text.Remove(num);
			}
			return text;
		}
	}
}
