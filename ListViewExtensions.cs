using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace TinhKiemAuto
{
	// Token: 0x020000B3 RID: 179
	internal class ListViewExtensions
	{
		// Token: 0x060009DE RID: 2526
		[DllImport("user32.dll")]
		private static extern IntPtr SendMessage(IntPtr hWnd, uint Msg, IntPtr wParam, IntPtr lParam);

		// Token: 0x060009DF RID: 2527
		[DllImport("user32.dll", EntryPoint = "SendMessage")]
		private static extern IntPtr SendMessageLVCOLUMN(IntPtr hWnd, int Msg, IntPtr wParam, ref ListViewExtensions.LVCOLUMN lPLVCOLUMN);

		// Token: 0x060009E0 RID: 2528 RVA: 0x00040610 File Offset: 0x0003E810
		public static void SetSortIcon(ListView listView, int columnIndex, SortOrder order)
		{
			IntPtr hWnd = ListViewExtensions.SendMessage(listView.Handle, 4127U, IntPtr.Zero, IntPtr.Zero);
			for (int i = 0; i <= listView.Columns.Count - 1; i++)
			{
				IntPtr wParam = new IntPtr(i);
				ListViewExtensions.LVCOLUMN lvcolumn = default(ListViewExtensions.LVCOLUMN);
				lvcolumn.mask = 4;
				ListViewExtensions.SendMessageLVCOLUMN(hWnd, 4619, wParam, ref lvcolumn);
				if (order != SortOrder.None && i == columnIndex)
				{
					if (order != SortOrder.Ascending)
					{
						if (order == SortOrder.Descending)
						{
							lvcolumn.fmt &= -1025;
							lvcolumn.fmt |= 512;
						}
					}
					else
					{
						lvcolumn.fmt &= -513;
						lvcolumn.fmt |= 1024;
					}
					lvcolumn.fmt |= 4096;
				}
				else
				{
					lvcolumn.fmt &= -5633;
				}
				ListViewExtensions.SendMessageLVCOLUMN(hWnd, 4620, wParam, ref lvcolumn);
			}
		}

		// Token: 0x04000723 RID: 1827
		private const int HDI_WIDTH = 1;

		// Token: 0x04000724 RID: 1828
		private const int HDI_HEIGHT = 1;

		// Token: 0x04000725 RID: 1829
		private const int HDI_TEXT = 2;

		// Token: 0x04000726 RID: 1830
		private const int HDI_FORMAT = 4;

		// Token: 0x04000727 RID: 1831
		private const int HDI_LPARAM = 8;

		// Token: 0x04000728 RID: 1832
		private const int HDI_BITMAP = 16;

		// Token: 0x04000729 RID: 1833
		private const int HDI_IMAGE = 32;

		// Token: 0x0400072A RID: 1834
		private const int HDI_DI_SETITEM = 64;

		// Token: 0x0400072B RID: 1835
		private const int HDI_ORDER = 128;

		// Token: 0x0400072C RID: 1836
		private const int HDI_FILTER = 256;

		// Token: 0x0400072D RID: 1837
		private const int HDF_LEFT = 0;

		// Token: 0x0400072E RID: 1838
		private const int HDF_RIGHT = 1;

		// Token: 0x0400072F RID: 1839
		private const int HDF_CENTER = 2;

		// Token: 0x04000730 RID: 1840
		private const int HDF_JUSTIFYMASK = 3;

		// Token: 0x04000731 RID: 1841
		private const int HDF_RTLREADING = 4;

		// Token: 0x04000732 RID: 1842
		private const int HDF_OWNERDRAW = 32768;

		// Token: 0x04000733 RID: 1843
		private const int HDF_STRING = 16384;

		// Token: 0x04000734 RID: 1844
		private const int HDF_BITMAP = 8192;

		// Token: 0x04000735 RID: 1845
		private const int HDF_BITMAP_ON_RIGHT = 4096;

		// Token: 0x04000736 RID: 1846
		private const int HDF_IMAGE = 2048;

		// Token: 0x04000737 RID: 1847
		private const int HDF_SORTUP = 1024;

		// Token: 0x04000738 RID: 1848
		private const int HDF_SORTDOWN = 512;

		// Token: 0x04000739 RID: 1849
		private const int LVM_FIRST = 4096;

		// Token: 0x0400073A RID: 1850
		private const int LVM_GETHEADER = 4127;

		// Token: 0x0400073B RID: 1851
		private const int HDM_FIRST = 4608;

		// Token: 0x0400073C RID: 1852
		private const int HDM_SETIMAGELIST = 4616;

		// Token: 0x0400073D RID: 1853
		private const int HDM_GETIMAGELIST = 4617;

		// Token: 0x0400073E RID: 1854
		private const int HDM_GETITEM = 4619;

		// Token: 0x0400073F RID: 1855
		private const int HDM_SETITEM = 4620;

		// Token: 0x0200016F RID: 367
		public struct LVCOLUMN
		{
			// Token: 0x04000E56 RID: 3670
			public int mask;

			// Token: 0x04000E57 RID: 3671
			public int cx;

			// Token: 0x04000E58 RID: 3672
			[MarshalAs(UnmanagedType.LPTStr)]
			public string pszText;

			// Token: 0x04000E59 RID: 3673
			public IntPtr hbm;

			// Token: 0x04000E5A RID: 3674
			public int cchTextMax;

			// Token: 0x04000E5B RID: 3675
			public int fmt;

			// Token: 0x04000E5C RID: 3676
			public int iSubItem;

			// Token: 0x04000E5D RID: 3677
			public int iImage;

			// Token: 0x04000E5E RID: 3678
			public int iOrder;
		}
	}
}
