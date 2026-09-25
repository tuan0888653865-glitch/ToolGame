using System;

namespace TinhKiemAuto
{
	// Token: 0x020000DF RID: 223
	internal class SettingEx
	{
		// Token: 0x170002BA RID: 698
		public string this[string val]
		{
			get
			{
				return Settings.Read(val);
			}
			set
			{
				Settings.Write(val, value);
			}
		}
	}
}
