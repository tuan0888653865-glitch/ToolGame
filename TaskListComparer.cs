using System;
using System.Collections;
using System.Windows.Forms;

namespace TinhKiemAuto
{
	// Token: 0x020000E8 RID: 232
	internal class TaskListComparer : IComparer
	{
		// Token: 0x06000BF8 RID: 3064 RVA: 0x0004C4A8 File Offset: 0x0004A6A8
		public int Compare(object a, object b)
		{
			int result;
			try
			{
				ListViewItem listViewItem = a as ListViewItem;
				ListViewItem listViewItem2 = b as ListViewItem;
				int packetId = ((Skill)listViewItem.Tag).PacketId;
				int packetId2 = ((Skill)listViewItem2.Tag).PacketId;
				if (listViewItem.Checked && !listViewItem2.Checked)
				{
					result = -1;
				}
				else if (!listViewItem.Checked && listViewItem2.Checked)
				{
					result = 1;
				}
				else if (packetId > packetId2)
				{
					result = 1;
				}
				else if (packetId < packetId2)
				{
					result = -1;
				}
				else
				{
					result = 0;
				}
			}
			catch
			{
				result = 0;
			}
			return result;
		}
	}
}
