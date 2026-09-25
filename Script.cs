using System;
using System.Text;
using System.Xml;

namespace TinhKiemAuto
{
	// Token: 0x020000D7 RID: 215
	public class Script
	{
		// Token: 0x17000285 RID: 645
		// (get) Token: 0x06000B38 RID: 2872 RVA: 0x0004959C File Offset: 0x0004779C
		// (set) Token: 0x06000B39 RID: 2873 RVA: 0x000495A4 File Offset: 0x000477A4
		public XmlNode Node { get; set; }

		// Token: 0x06000B3A RID: 2874 RVA: 0x000495AD File Offset: 0x000477AD
		public Script()
		{
			this.Node = Scripts.XML.CreateElement("Script");
		}

		// Token: 0x06000B3B RID: 2875 RVA: 0x000495CA File Offset: 0x000477CA
		public Script(XmlNode node)
		{
			this.Node = node;
		}

		// Token: 0x17000286 RID: 646
		// (get) Token: 0x06000B3C RID: 2876 RVA: 0x000495DC File Offset: 0x000477DC
		// (set) Token: 0x06000B3D RID: 2877 RVA: 0x00049620 File Offset: 0x00047820
		public string ID
		{
			get
			{
				try
				{
					return this.Node.Attributes["ID"].Value;
				}
				catch
				{
				}
				return "";
			}
			set
			{
				try
				{
					this.Node.Attributes["ID"].Value = value;
				}
				catch
				{
					XmlAttribute xmlAttribute = Scripts.XML.CreateAttribute("ID");
					xmlAttribute.Value = value;
					this.Node.Attributes.Append(xmlAttribute);
				}
			}
		}

		// Token: 0x17000287 RID: 647
		// (get) Token: 0x06000B3E RID: 2878 RVA: 0x00049688 File Offset: 0x00047888
		// (set) Token: 0x06000B3F RID: 2879 RVA: 0x000496CC File Offset: 0x000478CC
		public string Level
		{
			get
			{
				try
				{
					return this.Node.Attributes["Level"].Value;
				}
				catch
				{
				}
				return "";
			}
			set
			{
				try
				{
					this.Node.Attributes["Level"].Value = value;
				}
				catch
				{
					XmlAttribute xmlAttribute = Scripts.XML.CreateAttribute("Level");
					xmlAttribute.Value = value;
					this.Node.Attributes.Append(xmlAttribute);
				}
			}
		}

		// Token: 0x17000288 RID: 648
		// (get) Token: 0x06000B40 RID: 2880 RVA: 0x00049734 File Offset: 0x00047934
		// (set) Token: 0x06000B41 RID: 2881 RVA: 0x00049778 File Offset: 0x00047978
		public string InfoEx
		{
			get
			{
				try
				{
					return this.Node.Attributes["InfoEx"].Value;
				}
				catch
				{
				}
				return "";
			}
			set
			{
				try
				{
					this.Node.Attributes["InfoEx"].Value = value;
				}
				catch
				{
					XmlAttribute xmlAttribute = Scripts.XML.CreateAttribute("InfoEx");
					xmlAttribute.Value = value;
					this.Node.Attributes.Append(xmlAttribute);
				}
			}
		}

		// Token: 0x17000289 RID: 649
		// (get) Token: 0x06000B42 RID: 2882 RVA: 0x000497E0 File Offset: 0x000479E0
		// (set) Token: 0x06000B43 RID: 2883 RVA: 0x00049824 File Offset: 0x00047A24
		public string MD
		{
			get
			{
				try
				{
					return this.Node.Attributes["MD"].Value;
				}
				catch
				{
				}
				return "";
			}
			set
			{
				try
				{
					this.Node.Attributes["MD"].Value = value;
				}
				catch
				{
					XmlAttribute xmlAttribute = Scripts.XML.CreateAttribute("MD");
					xmlAttribute.Value = value;
					this.Node.Attributes.Append(xmlAttribute);
				}
			}
		}

		// Token: 0x1700028A RID: 650
		// (get) Token: 0x06000B44 RID: 2884 RVA: 0x0004988C File Offset: 0x00047A8C
		public string NameEx
		{
			get
			{
				string result;
				try
				{
					result = TINHKIEM.ClearSign(this.Info.Split(new char[]
					{
						':'
					})[0]).Replace(" ", "");
				}
				catch
				{
					result = "";
				}
				return result;
			}
		}

		// Token: 0x1700028B RID: 651
		// (get) Token: 0x06000B45 RID: 2885 RVA: 0x000498E4 File Offset: 0x00047AE4
		// (set) Token: 0x06000B46 RID: 2886 RVA: 0x00049958 File Offset: 0x00047B58
		public string Name
		{
			get
			{
				try
				{
					return this.Node.Attributes["Name"].Value;
				}
				catch
				{
				}
				XmlAttribute xmlAttribute = Scripts.XML.CreateAttribute("Name");
				xmlAttribute.Value = "";
				this.Node.Attributes.Append(xmlAttribute);
				return "";
			}
			set
			{
				try
				{
					this.Node.Attributes["Name"].Value = value;
				}
				catch
				{
					XmlAttribute xmlAttribute = Scripts.XML.CreateAttribute("Name");
					xmlAttribute.Value = value;
					this.Node.Attributes.Append(xmlAttribute);
				}
			}
		}

		// Token: 0x1700028C RID: 652
		// (get) Token: 0x06000B47 RID: 2887 RVA: 0x000499C0 File Offset: 0x00047BC0
		// (set) Token: 0x06000B48 RID: 2888 RVA: 0x00049A04 File Offset: 0x00047C04
		public string Recv
		{
			get
			{
				try
				{
					return this.Node.Attributes["Recv"].Value;
				}
				catch
				{
				}
				return "";
			}
			set
			{
				try
				{
					this.Node.Attributes["Recv"].Value = value;
				}
				catch
				{
					XmlAttribute xmlAttribute = Scripts.XML.CreateAttribute("Recv");
					xmlAttribute.Value = value;
					this.Node.Attributes.Append(xmlAttribute);
				}
			}
		}

		// Token: 0x1700028D RID: 653
		// (get) Token: 0x06000B49 RID: 2889 RVA: 0x00049A6C File Offset: 0x00047C6C
		public NPC RecvNPC
		{
			get
			{
				NPC npc = new NPC();
				if (this.Recv.Length == 12)
				{
					npc.Id = Memory.Hex2Int(this.Recv.Substring(0, 3));
					npc.X = Memory.Hex2Int(this.Recv.Substring(3, 3));
					npc.Y = Memory.Hex2Int(this.Recv.Substring(6, 3));
					npc.Map = Memory.Hex2Int(this.Recv.Substring(9, 3));
					if (npc.Map == 0)
					{
						npc.Map = LACDUONG.Id;
					}
				}
				return npc;
			}
		}

		// Token: 0x1700028E RID: 654
		// (get) Token: 0x06000B4A RID: 2890 RVA: 0x00049B04 File Offset: 0x00047D04
		public NPC AtkNPC1
		{
			get
			{
				NPC npc = new NPC();
				if (this.Do.Length >= 18)
				{
					npc.MD = this.Do.Substring(6, 3);
					npc.X = Memory.Hex2Int(this.Do.Substring(9, 3));
					npc.Y = Memory.Hex2Int(this.Do.Substring(12, 3));
					npc.Map = Memory.Hex2Int(this.Do.Substring(15, 3));
					if (npc.Map == 0)
					{
						npc.Map = LACDUONG.Id;
					}
				}
				return npc;
			}
		}

		// Token: 0x1700028F RID: 655
		// (get) Token: 0x06000B4B RID: 2891 RVA: 0x00049B98 File Offset: 0x00047D98
		public NPC AtkNPC2
		{
			get
			{
				NPC npc = new NPC();
				if (this.Do.Length >= 30)
				{
					npc.MD = this.Do.Substring(18, 3);
					npc.X = Memory.Hex2Int(this.Do.Substring(21, 3));
					npc.Y = Memory.Hex2Int(this.Do.Substring(24, 3));
					npc.Map = Memory.Hex2Int(this.Do.Substring(27, 3));
					if (npc.Map == 0)
					{
						npc.Map = LACDUONG.Id;
					}
				}
				return npc;
			}
		}

		// Token: 0x17000290 RID: 656
		// (get) Token: 0x06000B4C RID: 2892 RVA: 0x00049C30 File Offset: 0x00047E30
		public NPC AtkNPC3
		{
			get
			{
				NPC npc = new NPC();
				if (this.Do.Length >= 42)
				{
					npc.MD = this.Do.Substring(30, 3);
					npc.X = Memory.Hex2Int(this.Do.Substring(33, 3));
					npc.Y = Memory.Hex2Int(this.Do.Substring(36, 3));
					npc.Map = Memory.Hex2Int(this.Do.Substring(39, 3));
					if (npc.Map == 0)
					{
						npc.Map = LACDUONG.Id;
					}
				}
				return npc;
			}
		}

		// Token: 0x17000291 RID: 657
		// (get) Token: 0x06000B4D RID: 2893 RVA: 0x00049CC5 File Offset: 0x00047EC5
		public bool AtkAny
		{
			get
			{
				return this.Do.Length >= 43 && this.Do[42] == '1';
			}
		}

		// Token: 0x17000292 RID: 658
		// (get) Token: 0x06000B4E RID: 2894 RVA: 0x00049CEC File Offset: 0x00047EEC
		// (set) Token: 0x06000B4F RID: 2895 RVA: 0x00049D24 File Offset: 0x00047F24
		public string IsCollect
		{
			get
			{
				if (this.Do.Length >= 44)
				{
					return this.Do[43].ToString();
				}
				return "0";
			}
			set
			{
				if (this.Do.Length >= 44)
				{
					StringBuilder stringBuilder = new StringBuilder(this.Do);
					stringBuilder[43] = value.ToString()[0];
					this.Do = stringBuilder.ToString();
				}
			}
		}

		// Token: 0x17000293 RID: 659
		// (get) Token: 0x06000B50 RID: 2896 RVA: 0x00049D6C File Offset: 0x00047F6C
		// (set) Token: 0x06000B51 RID: 2897 RVA: 0x00049D90 File Offset: 0x00047F90
		public bool IsComplete
		{
			get
			{
				return this.Do.Length >= 45 && this.Do[44] == '1';
			}
			set
			{
				if (this.Do.Length >= 45)
				{
					StringBuilder stringBuilder = new StringBuilder(this.Do);
					if (value)
					{
						stringBuilder[44] = '1';
					}
					else
					{
						stringBuilder[44] = '0';
					}
					this.Do = stringBuilder.ToString();
				}
			}
		}

		// Token: 0x17000294 RID: 660
		// (get) Token: 0x06000B52 RID: 2898 RVA: 0x00049DDD File Offset: 0x00047FDD
		// (set) Token: 0x06000B53 RID: 2899 RVA: 0x00049E04 File Offset: 0x00048004
		public bool Completed
		{
			get
			{
				return this.Do.Length >= 46 && this.Do[45] == '1';
			}
			set
			{
				if (this.Do.Length >= 46)
				{
					StringBuilder stringBuilder = new StringBuilder(this.Do);
					if (value)
					{
						stringBuilder[45] = '1';
					}
					else
					{
						stringBuilder[45] = '0';
					}
					this.Do = stringBuilder.ToString();
				}
			}
		}

		// Token: 0x17000295 RID: 661
		// (get) Token: 0x06000B54 RID: 2900 RVA: 0x00049E51 File Offset: 0x00048051
		// (set) Token: 0x06000B55 RID: 2901 RVA: 0x00049E78 File Offset: 0x00048078
		public bool IsUseItem
		{
			get
			{
				return this.Do.Length >= 47 && this.Do[46] == '1';
			}
			set
			{
				if (this.Do.Length >= 47)
				{
					StringBuilder stringBuilder = new StringBuilder(this.Do);
					if (value)
					{
						stringBuilder[46] = '1';
					}
					else
					{
						stringBuilder[46] = '0';
					}
					this.Do = stringBuilder.ToString();
				}
			}
		}

		// Token: 0x17000296 RID: 662
		// (get) Token: 0x06000B56 RID: 2902 RVA: 0x00049EC8 File Offset: 0x000480C8
		// (set) Token: 0x06000B57 RID: 2903 RVA: 0x00049F04 File Offset: 0x00048104
		public int CompleteNoi
		{
			get
			{
				if (this.Do.Length >= 48)
				{
					return TINHKIEM.ParseInt(this.Do[47].ToString()) - 1;
				}
				return -1;
			}
			set
			{
				if (this.Do.Length >= 48)
				{
					StringBuilder stringBuilder = new StringBuilder(this.Do);
					stringBuilder[47] = value.ToString()[0];
					this.Do = stringBuilder.ToString();
				}
			}
		}

		// Token: 0x17000297 RID: 663
		// (get) Token: 0x06000B58 RID: 2904 RVA: 0x00049F50 File Offset: 0x00048150
		// (set) Token: 0x06000B59 RID: 2905 RVA: 0x00049F8C File Offset: 0x0004818C
		public int CompleteNgoai
		{
			get
			{
				if (this.Do.Length >= 49)
				{
					return TINHKIEM.ParseInt(this.Do[48].ToString()) - 1;
				}
				return -1;
			}
			set
			{
				if (this.Do.Length >= 49)
				{
					StringBuilder stringBuilder = new StringBuilder(this.Do);
					stringBuilder[48] = value.ToString()[0];
					this.Do = stringBuilder.ToString();
				}
			}
		}

		// Token: 0x17000298 RID: 664
		// (get) Token: 0x06000B5A RID: 2906 RVA: 0x00049FD5 File Offset: 0x000481D5
		// (set) Token: 0x06000B5B RID: 2907 RVA: 0x00049FFC File Offset: 0x000481FC
		public bool IsPick
		{
			get
			{
				return this.Do.Length >= 50 && this.Do[49] == '1';
			}
			set
			{
				if (this.Do.Length >= 50)
				{
					StringBuilder stringBuilder = new StringBuilder(this.Do);
					if (value)
					{
						stringBuilder[49] = '1';
					}
					else
					{
						stringBuilder[49] = '0';
					}
					this.Do = stringBuilder.ToString();
				}
			}
		}

		// Token: 0x17000299 RID: 665
		// (get) Token: 0x06000B5C RID: 2908 RVA: 0x0004A049 File Offset: 0x00048249
		// (set) Token: 0x06000B5D RID: 2909 RVA: 0x0004A070 File Offset: 0x00048270
		public bool IsThuThap
		{
			get
			{
				return this.Do.Length >= 51 && this.Do[50] == '1';
			}
			set
			{
				if (this.Do.Length >= 51)
				{
					StringBuilder stringBuilder = new StringBuilder(this.Do);
					if (value)
					{
						stringBuilder[50] = '1';
					}
					else
					{
						stringBuilder[50] = '0';
					}
					this.Do = stringBuilder.ToString();
				}
			}
		}

		// Token: 0x1700029A RID: 666
		// (get) Token: 0x06000B5E RID: 2910 RVA: 0x0004A0C0 File Offset: 0x000482C0
		// (set) Token: 0x06000B5F RID: 2911 RVA: 0x0004A104 File Offset: 0x00048304
		public string Send
		{
			get
			{
				try
				{
					return this.Node.Attributes["Send"].Value;
				}
				catch
				{
				}
				return "";
			}
			set
			{
				try
				{
					this.Node.Attributes["Send"].Value = value;
				}
				catch
				{
					XmlAttribute xmlAttribute = Scripts.XML.CreateAttribute("Send");
					xmlAttribute.Value = value;
					this.Node.Attributes.Append(xmlAttribute);
				}
			}
		}

		// Token: 0x1700029B RID: 667
		// (get) Token: 0x06000B60 RID: 2912 RVA: 0x0004A16C File Offset: 0x0004836C
		public NPC SendNPC
		{
			get
			{
				NPC npc = new NPC();
				if (this.Send.Length == 12)
				{
					npc.Id = Memory.Hex2Int(this.Send.Substring(0, 3));
					npc.X = Memory.Hex2Int(this.Send.Substring(3, 3));
					npc.Y = Memory.Hex2Int(this.Send.Substring(6, 3));
					npc.Map = Memory.Hex2Int(this.Send.Substring(9, 3));
					if (npc.Map == 0)
					{
						npc.Map = LACDUONG.Id;
					}
				}
				return npc;
			}
		}

		// Token: 0x1700029C RID: 668
		// (get) Token: 0x06000B61 RID: 2913 RVA: 0x0004A203 File Offset: 0x00048403
		public string SendClickMD
		{
			get
			{
				if (this.Do.Length >= 6)
				{
					return this.Do.Substring(3, 3);
				}
				return "000";
			}
		}

		// Token: 0x06000B62 RID: 2914 RVA: 0x0004A228 File Offset: 0x00048428
		public bool IsClickExacly(QuestFrame dialog)
		{
			return this.SendClickMD == dialog.MD || this.RecvClickMD == dialog.MD || this.Name.Contains(dialog.Name) || TINHKIEM.VietLien(this.Name) == TINHKIEM.VietLien(dialog.Name);
		}

		// Token: 0x1700029D RID: 669
		// (get) Token: 0x06000B63 RID: 2915 RVA: 0x0004A28D File Offset: 0x0004848D
		public string RecvClickMD
		{
			get
			{
				if (this.Do.Length >= 3)
				{
					return this.Do.Substring(0, 3);
				}
				return "000";
			}
		}

		// Token: 0x1700029E RID: 670
		// (get) Token: 0x06000B64 RID: 2916 RVA: 0x0004A2B0 File Offset: 0x000484B0
		// (set) Token: 0x06000B65 RID: 2917 RVA: 0x0004A2F4 File Offset: 0x000484F4
		public string Do
		{
			get
			{
				try
				{
					return this.Node.Attributes["Do"].Value;
				}
				catch
				{
				}
				return "";
			}
			set
			{
				try
				{
					this.Node.Attributes["Do"].Value = value;
				}
				catch
				{
					XmlAttribute xmlAttribute = Scripts.XML.CreateAttribute("Do");
					xmlAttribute.Value = value;
					this.Node.Attributes.Append(xmlAttribute);
				}
			}
		}

		// Token: 0x1700029F RID: 671
		// (get) Token: 0x06000B66 RID: 2918 RVA: 0x0004A35C File Offset: 0x0004855C
		// (set) Token: 0x06000B67 RID: 2919 RVA: 0x0004A3A0 File Offset: 0x000485A0
		public string Info
		{
			get
			{
				try
				{
					return this.Node.Attributes["Info"].Value;
				}
				catch
				{
				}
				return "";
			}
			set
			{
				try
				{
					this.Node.Attributes["Info"].Value = value;
				}
				catch
				{
					XmlAttribute xmlAttribute = Scripts.XML.CreateAttribute("Info");
					xmlAttribute.Value = value;
					this.Node.Attributes.Append(xmlAttribute);
				}
			}
		}
	}
}
