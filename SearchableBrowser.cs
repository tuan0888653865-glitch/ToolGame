using System;

namespace TinhKiemAuto
{
	// Token: 0x020000D9 RID: 217
	public interface SearchableBrowser
	{
		// Token: 0x06000B73 RID: 2931
		bool Search(string text, bool forward, bool matchWholeWord, bool matchCase);
	}
}
