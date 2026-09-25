using System;
using System.IO;

namespace TinhKiemAuto
{
	// Token: 0x020000C6 RID: 198
	internal static class PathHelper
	{
		// Token: 0x06000AC2 RID: 2754 RVA: 0x00047A3C File Offset: 0x00045C3C
		public static string GetWithBackslash(string path)
		{
			if (!path.EndsWith(Path.DirectorySeparatorChar.ToString()))
			{
				path += Path.DirectorySeparatorChar.ToString();
			}
			return path;
		}
	}
}
