using System;
using System.Xml;

namespace TinhKiemAuto
{
	// Token: 0x020000E0 RID: 224
	internal class Settings
	{
		// Token: 0x170002BB RID: 699
		// (get) Token: 0x06000BB9 RID: 3001 RVA: 0x0004AEAC File Offset: 0x000490AC
		public static SettingEx Instance
		{
			get
			{
				if (Settings.instance == null)
				{
					Settings.Load();
					Settings.instance = new SettingEx();
				}
				return Settings.instance;
			}
		}

		// Token: 0x170002BC RID: 700
		// (get) Token: 0x06000BBA RID: 3002 RVA: 0x0004AEC9 File Offset: 0x000490C9
		// (set) Token: 0x06000BBB RID: 3003 RVA: 0x0004AED0 File Offset: 0x000490D0
		public static XmlDocument XML { get; set; }

		// Token: 0x170002BD RID: 701
		// (get) Token: 0x06000BBC RID: 3004 RVA: 0x0004AED8 File Offset: 0x000490D8
		public static XmlNode Root
		{
			get
			{
				return Settings.XML.SelectSingleNode("Settings");
			}
		}

		// Token: 0x06000BBD RID: 3005 RVA: 0x0004AEEC File Offset: 0x000490EC
		public static void Load()
		{
			Settings.XML = new XmlDocument();
			try
			{
				Settings.XML.LoadXml(Settings.xml);
			}
			catch
			{
			}
			if (Settings.Root == null)
			{
				Settings.XML.InsertBefore(Settings.XML.CreateXmlDeclaration("1.0", "UTF-8", null), Settings.XML.DocumentElement);
				Settings.XML.AppendChild(Settings.XML.CreateNode(XmlNodeType.Element, "Settings", ""));
				Settings.Save();
			}
		}

		// Token: 0x06000BBE RID: 3006 RVA: 0x00006740 File Offset: 0x00004940
		public static void Save()
		{
		}

		// Token: 0x06000BBF RID: 3007 RVA: 0x0004AF80 File Offset: 0x00049180
		public static string Read(string name)
		{
			XmlNode xmlNode = Settings.Root.SelectSingleNode(name);
			if (xmlNode != null)
			{
				return xmlNode.InnerText;
			}
			return "";
		}

		// Token: 0x06000BC0 RID: 3008 RVA: 0x0004AFA8 File Offset: 0x000491A8
		public static void Write(string name, string value)
		{
			XmlNode xmlNode = Settings.Root.SelectSingleNode(name);
			if (xmlNode == null)
			{
				xmlNode = Settings.XML.CreateElement(name);
			}
			xmlNode.InnerText = value;
			Settings.Root.AppendChild(xmlNode);
			Settings.Save();
		}

		// Token: 0x040008D0 RID: 2256
		private static SettingEx instance;

		// Token: 0x040008D1 RID: 2257
		public static string xml;
	}
}
