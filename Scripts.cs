using System;
using System.Collections.Generic;
using System.Xml;
using TinhKiemAuto.Models;

namespace TinhKiemAuto
{
	// Token: 0x020000D8 RID: 216
	internal class Scripts
	{
		// Token: 0x170002A0 RID: 672
		// (get) Token: 0x06000B68 RID: 2920 RVA: 0x0004A408 File Offset: 0x00048608
		public static string Path
		{
			get
			{
				return Global.DataPath + "\\20.dat";
			}
		}

		// Token: 0x170002A1 RID: 673
		// (get) Token: 0x06000B69 RID: 2921 RVA: 0x0004A419 File Offset: 0x00048619
		// (set) Token: 0x06000B6A RID: 2922 RVA: 0x0004A420 File Offset: 0x00048620
		public static List<Script> All { get; set; }

		// Token: 0x06000B6B RID: 2923 RVA: 0x0004A428 File Offset: 0x00048628
		public static List<Script> Load()
		{
			try
			{
				Scripts.XML.LoadXml(LoadFile.LoadFileWithDecrypt(Scripts.Path));
			}
			catch
			{
				if (Scripts.XML.SelectSingleNode("/*") == null)
				{
					Scripts.XML.InsertBefore(Scripts.XML.CreateXmlDeclaration("1.0", "UTF-8", null), Scripts.XML.DocumentElement);
					Scripts.XML.AppendChild(Scripts.XML.CreateNode(XmlNodeType.Element, "Scripts", ""));
					Scripts.XML.Save(Scripts.Path);
				}
			}
			List<Script> list = new List<Script>();
			foreach (object obj in Scripts.XML.SelectSingleNode("Scripts").SelectNodes("Script"))
			{
				XmlNode node = (XmlNode)obj;
				list.Add(new Script
				{
					Node = node
				});
			}
			Scripts.All = list;
			return list;
		}

		// Token: 0x06000B6C RID: 2924 RVA: 0x0004A53C File Offset: 0x0004873C
		public static Script Get(string id)
		{
			foreach (Script script in Scripts.All)
			{
				if (script.ID == id || script.MD.Contains(id))
				{
					return script;
				}
			}
			return null;
		}

		// Token: 0x06000B6D RID: 2925 RVA: 0x0004A5AC File Offset: 0x000487AC
		public static Script GetByName(string id)
		{
			foreach (Script script in Scripts.All)
			{
				if (script.Name.Contains(id) || TINHKIEM.VietLien(script.Name) == TINHKIEM.VietLien(id))
				{
					return script;
				}
			}
			return null;
		}

		// Token: 0x06000B6E RID: 2926 RVA: 0x0004A624 File Offset: 0x00048824
		public static string Add(Script script)
		{
			foreach (object obj in Scripts.XML.SelectSingleNode("Scripts").SelectNodes("Script"))
			{
				XmlNode node = (XmlNode)obj;
				Script script2 = new Script();
				script2.Node = node;
				if (script2.MD.Contains(script.MD) || (script2.ID == script.ID && script.ID != ""))
				{
					return "Script Đã Tồn Tại";
				}
			}
			Scripts.XML.SelectSingleNode("/*").AppendChild(script.Node);
			Scripts.Save();
			return "";
		}

		// Token: 0x06000B6F RID: 2927 RVA: 0x0004A700 File Offset: 0x00048900
		public static void Save()
		{
			LoadFile.WriteFileWithEncrypt(Scripts.XML.OuterXml, Scripts.Path);
		}

		// Token: 0x06000B70 RID: 2928 RVA: 0x0004A716 File Offset: 0x00048916
		public static void Remove(Script script)
		{
			Scripts.XML.SelectSingleNode("/*").RemoveChild(script.Node);
			LoadFile.WriteFileWithEncrypt(Scripts.XML.OuterXml, Scripts.Path);
		}

		// Token: 0x040008AE RID: 2222
		public static XmlDocument XML = new XmlDocument();
	}
}
