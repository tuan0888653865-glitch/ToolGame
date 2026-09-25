using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using TinhKiemAuto.Models;

namespace TinhKiemAuto.Controllers
{
	// Token: 0x0200013D RID: 317
	public static class SkillData
	{
		// Token: 0x06001020 RID: 4128 RVA: 0x00076700 File Offset: 0x00074900
		public static SkillModel GetSkillByID(int id)
		{
			SkillModel result = new SkillModel();
			if (SkillData.skillList.TryGetValue(id, out result))
			{
				return result;
			}
			return null;
		}

		// Token: 0x06001021 RID: 4129 RVA: 0x00076725 File Offset: 0x00074925
		public static void LoadSkillData()
		{
			SkillData.skillList = JsonConvert.DeserializeObject<Dictionary<int, SkillModel>>(LoadFile.LoadFileWithDecrypt(Global.DataPath + "\\SkillList.dat"));
		}

		// Token: 0x04000D15 RID: 3349
		public static Dictionary<int, SkillModel> skillList = new Dictionary<int, SkillModel>();
	}
}
