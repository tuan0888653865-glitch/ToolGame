using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Xml;
using TinhKiemAuto.Models;

namespace TinhKiemAuto
{
	// Token: 0x0200007C RID: 124
	public class Account
	{
		// Token: 0x06000497 RID: 1175 RVA: 0x0001A080 File Offset: 0x00018280
		public Account(string user, string pass, string NPH, string server, string tail)
		{
			XmlElement xmlElement = Account.XML.CreateElement("Account");
			Account.XML.SelectSingleNode("/*").AppendChild(xmlElement);
			this.Node = xmlElement;
			this.User = user;
			this.Pass = pass;
			this.Tail = tail;
			this.NPH = NPH;
			this.Server = server;
			Account.Save();
		}

		// Token: 0x06000498 RID: 1176 RVA: 0x0001A124 File Offset: 0x00018324
		public Account(XmlElement node)
		{
			this.Node = node;
		}

		// Token: 0x170000F0 RID: 240
		// (get) Token: 0x06000499 RID: 1177 RVA: 0x0001A178 File Offset: 0x00018378
		// (set) Token: 0x0600049A RID: 1178 RVA: 0x0001A22C File Offset: 0x0001842C
		public string User
		{
			get
			{
				string result;
				try
				{
					XmlAttribute xmlAttribute = this.Node.Attributes["User"];
					if (xmlAttribute == null)
					{
						xmlAttribute = Account.XML.CreateAttribute("User");
						this.Node.Attributes.Append(xmlAttribute);
					}
					if (this.Node.SelectSingleNode("User") != null)
					{
						xmlAttribute.Value = this.Node.SelectSingleNode("User").InnerText;
						this.Node.RemoveChild(this.Node.SelectSingleNode("User"));
					}
					result = xmlAttribute.Value;
				}
				catch
				{
					result = "";
				}
				return result;
			}
			set
			{
				try
				{
					XmlAttribute xmlAttribute = this.Node.Attributes["User"];
					if (xmlAttribute == null)
					{
						xmlAttribute = Account.XML.CreateAttribute("User");
						this.Node.Attributes.Append(xmlAttribute);
					}
					xmlAttribute.Value = value;
				}
				catch
				{
				}
			}
		}

		// Token: 0x170000F1 RID: 241
		// (get) Token: 0x0600049B RID: 1179 RVA: 0x0001A290 File Offset: 0x00018490
		// (set) Token: 0x0600049C RID: 1180 RVA: 0x0001A344 File Offset: 0x00018544
		public string Pass
		{
			get
			{
				string result;
				try
				{
					XmlAttribute xmlAttribute = this.Node.Attributes["Pass"];
					if (xmlAttribute == null)
					{
						xmlAttribute = Account.XML.CreateAttribute("Pass");
						this.Node.Attributes.Append(xmlAttribute);
					}
					if (this.Node.SelectSingleNode("Pass") != null)
					{
						xmlAttribute.Value = this.Node.SelectSingleNode("Pass").InnerText;
						this.Node.RemoveChild(this.Node.SelectSingleNode("Pass"));
					}
					result = xmlAttribute.Value;
				}
				catch
				{
					result = "";
				}
				return result;
			}
			set
			{
				try
				{
					XmlAttribute xmlAttribute = this.Node.Attributes["Pass"];
					if (xmlAttribute == null)
					{
						xmlAttribute = Account.XML.CreateAttribute("Pass");
						this.Node.Attributes.Append(xmlAttribute);
					}
					xmlAttribute.Value = value;
				}
				catch
				{
				}
			}
		}

		// Token: 0x170000F2 RID: 242
		// (get) Token: 0x0600049D RID: 1181 RVA: 0x0001A3A8 File Offset: 0x000185A8
		// (set) Token: 0x0600049E RID: 1182 RVA: 0x0001A45C File Offset: 0x0001865C
		public string NPH
		{
			get
			{
				string result;
				try
				{
					XmlAttribute xmlAttribute = this.Node.Attributes["NPH"];
					if (xmlAttribute == null)
					{
						xmlAttribute = Account.XML.CreateAttribute("NPH");
						this.Node.Attributes.Append(xmlAttribute);
					}
					if (this.Node.SelectSingleNode("NPH") != null)
					{
						xmlAttribute.Value = this.Node.SelectSingleNode("NPH").InnerText;
						this.Node.RemoveChild(this.Node.SelectSingleNode("NPH"));
					}
					result = xmlAttribute.Value;
				}
				catch
				{
					result = "";
				}
				return result;
			}
			set
			{
				try
				{
					XmlAttribute xmlAttribute = this.Node.Attributes["NPH"];
					if (xmlAttribute == null)
					{
						xmlAttribute = Account.XML.CreateAttribute("NPH");
						this.Node.Attributes.Append(xmlAttribute);
					}
					xmlAttribute.Value = value;
				}
				catch
				{
				}
			}
		}

		// Token: 0x170000F3 RID: 243
		// (get) Token: 0x0600049F RID: 1183 RVA: 0x0001A4C0 File Offset: 0x000186C0
		// (set) Token: 0x060004A0 RID: 1184 RVA: 0x0001A574 File Offset: 0x00018774
		public string Server
		{
			get
			{
				string result;
				try
				{
					XmlAttribute xmlAttribute = this.Node.Attributes["Server"];
					if (xmlAttribute == null)
					{
						xmlAttribute = Account.XML.CreateAttribute("Server");
						this.Node.Attributes.Append(xmlAttribute);
					}
					if (this.Node.SelectSingleNode("Server") != null)
					{
						xmlAttribute.Value = this.Node.SelectSingleNode("Server").InnerText;
						this.Node.RemoveChild(this.Node.SelectSingleNode("Server"));
					}
					result = xmlAttribute.Value;
				}
				catch
				{
					result = "";
				}
				return result;
			}
			set
			{
				try
				{
					XmlAttribute xmlAttribute = this.Node.Attributes["Server"];
					if (xmlAttribute == null)
					{
						xmlAttribute = Account.XML.CreateAttribute("Server");
						this.Node.Attributes.Append(xmlAttribute);
					}
					xmlAttribute.Value = value;
				}
				catch
				{
				}
			}
		}

		// Token: 0x170000F4 RID: 244
		// (get) Token: 0x060004A1 RID: 1185 RVA: 0x0001A5D8 File Offset: 0x000187D8
		// (set) Token: 0x060004A2 RID: 1186 RVA: 0x0001A68C File Offset: 0x0001888C
		public string Tail
		{
			get
			{
				string result;
				try
				{
					XmlAttribute xmlAttribute = this.Node.Attributes["Tail"];
					if (xmlAttribute == null)
					{
						xmlAttribute = Account.XML.CreateAttribute("Tail");
						this.Node.Attributes.Append(xmlAttribute);
					}
					if (this.Node.SelectSingleNode("Tail") != null)
					{
						xmlAttribute.Value = this.Node.SelectSingleNode("Tail").InnerText;
						this.Node.RemoveChild(this.Node.SelectSingleNode("Tail"));
					}
					result = xmlAttribute.Value;
				}
				catch
				{
					result = "";
				}
				return result;
			}
			set
			{
				try
				{
					XmlAttribute xmlAttribute = this.Node.Attributes["Tail"];
					if (xmlAttribute == null)
					{
						xmlAttribute = Account.XML.CreateAttribute("Tail");
						this.Node.Attributes.Append(xmlAttribute);
					}
					xmlAttribute.Value = value;
				}
				catch
				{
				}
			}
		}

		// Token: 0x170000F5 RID: 245
		// (get) Token: 0x060004A3 RID: 1187 RVA: 0x0001A6F0 File Offset: 0x000188F0
		// (set) Token: 0x060004A4 RID: 1188 RVA: 0x0001A7A4 File Offset: 0x000189A4
		public string Name
		{
			get
			{
				string result;
				try
				{
					XmlAttribute xmlAttribute = this.Node.Attributes["Name"];
					if (xmlAttribute == null)
					{
						xmlAttribute = Account.XML.CreateAttribute("Name");
						this.Node.Attributes.Append(xmlAttribute);
					}
					if (this.Node.SelectSingleNode("Name") != null)
					{
						xmlAttribute.Value = this.Node.SelectSingleNode("Name").InnerText;
						this.Node.RemoveChild(this.Node.SelectSingleNode("Name"));
					}
					result = xmlAttribute.Value;
				}
				catch
				{
					result = "";
				}
				return result;
			}
			set
			{
				try
				{
					XmlAttribute xmlAttribute = this.Node.Attributes["Name"];
					if (xmlAttribute == null)
					{
						xmlAttribute = Account.XML.CreateAttribute("Name");
						this.Node.Attributes.Append(xmlAttribute);
					}
					xmlAttribute.Value = value;
				}
				catch
				{
				}
			}
		}

		// Token: 0x170000F6 RID: 246
		// (get) Token: 0x060004A5 RID: 1189 RVA: 0x0001A808 File Offset: 0x00018A08
		// (set) Token: 0x060004A6 RID: 1190 RVA: 0x0001A8BC File Offset: 0x00018ABC
		public string Ids
		{
			get
			{
				string result;
				try
				{
					XmlAttribute xmlAttribute = this.Node.Attributes["Ids"];
					if (xmlAttribute == null)
					{
						xmlAttribute = Account.XML.CreateAttribute("Ids");
						this.Node.Attributes.Append(xmlAttribute);
					}
					if (this.Node.SelectSingleNode("Ids") != null)
					{
						xmlAttribute.Value = this.Node.SelectSingleNode("Ids").InnerText;
						this.Node.RemoveChild(this.Node.SelectSingleNode("Ids"));
					}
					result = xmlAttribute.Value;
				}
				catch
				{
					result = "";
				}
				return result;
			}
			set
			{
				try
				{
					XmlAttribute xmlAttribute = this.Node.Attributes["Ids"];
					if (xmlAttribute == null)
					{
						xmlAttribute = Account.XML.CreateAttribute("Ids");
						this.Node.Attributes.Append(xmlAttribute);
					}
					xmlAttribute.Value = value;
				}
				catch
				{
				}
			}
		}

		// Token: 0x170000F7 RID: 247
		// (get) Token: 0x060004A7 RID: 1191 RVA: 0x0001A920 File Offset: 0x00018B20
		// (set) Token: 0x060004A8 RID: 1192 RVA: 0x0001A9D4 File Offset: 0x00018BD4
		public string Menpai
		{
			get
			{
				string result;
				try
				{
					XmlAttribute xmlAttribute = this.Node.Attributes["Menpai"];
					if (xmlAttribute == null)
					{
						xmlAttribute = Account.XML.CreateAttribute("Menpai");
						this.Node.Attributes.Append(xmlAttribute);
					}
					if (this.Node.SelectSingleNode("Menpai") != null)
					{
						xmlAttribute.Value = this.Node.SelectSingleNode("Menpai").InnerText;
						this.Node.RemoveChild(this.Node.SelectSingleNode("Menpai"));
					}
					result = xmlAttribute.Value;
				}
				catch
				{
					result = "";
				}
				return result;
			}
			set
			{
				try
				{
					XmlAttribute xmlAttribute = this.Node.Attributes["Menpai"];
					if (xmlAttribute == null)
					{
						xmlAttribute = Account.XML.CreateAttribute("Menpai");
						this.Node.Attributes.Append(xmlAttribute);
					}
					xmlAttribute.Value = value;
				}
				catch
				{
				}
			}
		}

		// Token: 0x170000F8 RID: 248
		// (get) Token: 0x060004A9 RID: 1193 RVA: 0x0001AA38 File Offset: 0x00018C38
		// (set) Token: 0x060004AA RID: 1194 RVA: 0x0001AAEC File Offset: 0x00018CEC
		public string Lvl
		{
			get
			{
				string result;
				try
				{
					XmlAttribute xmlAttribute = this.Node.Attributes["Lvl"];
					if (xmlAttribute == null)
					{
						xmlAttribute = Account.XML.CreateAttribute("Lvl");
						this.Node.Attributes.Append(xmlAttribute);
					}
					if (this.Node.SelectSingleNode("Lvl") != null)
					{
						xmlAttribute.Value = this.Node.SelectSingleNode("Lvl").InnerText;
						this.Node.RemoveChild(this.Node.SelectSingleNode("Lvl"));
					}
					result = xmlAttribute.Value;
				}
				catch
				{
					result = "";
				}
				return result;
			}
			set
			{
				try
				{
					XmlAttribute xmlAttribute = this.Node.Attributes["Lvl"];
					if (xmlAttribute == null)
					{
						xmlAttribute = Account.XML.CreateAttribute("Lvl");
						this.Node.Attributes.Append(xmlAttribute);
					}
					xmlAttribute.Value = value;
				}
				catch
				{
				}
			}
		}

		// Token: 0x170000F9 RID: 249
		// (get) Token: 0x060004AB RID: 1195 RVA: 0x0001AB50 File Offset: 0x00018D50
		// (set) Token: 0x060004AC RID: 1196 RVA: 0x0001AB58 File Offset: 0x00018D58
		public Stopwatch LogonTime { get; set; }

		// Token: 0x170000FA RID: 250
		// (get) Token: 0x060004AD RID: 1197 RVA: 0x0001AB61 File Offset: 0x00018D61
		// (set) Token: 0x060004AE RID: 1198 RVA: 0x0001AB69 File Offset: 0x00018D69
		public Stopwatch IsNextLogin { get; set; }

		// Token: 0x170000FB RID: 251
		// (get) Token: 0x060004AF RID: 1199 RVA: 0x0001AB72 File Offset: 0x00018D72
		// (set) Token: 0x060004B0 RID: 1200 RVA: 0x0001AB7A File Offset: 0x00018D7A
		public bool IsForceOpen { get; set; }

		// Token: 0x170000FC RID: 252
		// (get) Token: 0x060004B1 RID: 1201 RVA: 0x0001AB83 File Offset: 0x00018D83
		// (set) Token: 0x060004B2 RID: 1202 RVA: 0x0001AB8B File Offset: 0x00018D8B
		public Stopwatch SelectAccTime { get; set; }

		// Token: 0x170000FD RID: 253
		// (get) Token: 0x060004B3 RID: 1203 RVA: 0x0001AB94 File Offset: 0x00018D94
		public int ServerIndex
		{
			get
			{
				string server = this.Server;
				uint num = <PrivateImplementationDetails>.ComputeStringHash(server);
				if (num <= 1807885226U)
				{
					if (num <= 943938042U)
					{
						if (num != 748293993U)
						{
							if (num == 943938042U)
							{
								if (server == "Tứ Kiếm")
								{
									return 3;
								}
							}
						}
						else if (server == "Ảnh Kiếm")
						{
							return 9;
						}
					}
					else if (num != 1098765051U)
					{
						if (num != 1106729041U)
						{
							if (num == 1807885226U)
							{
								if (server == "Tái Chiến")
								{
									return 5;
								}
							}
						}
						else if (server == "Long Kiếm")
						{
							return 6;
						}
					}
					else if (server == "Tam Kiếm")
					{
						return 2;
					}
				}
				else if (num <= 2959139256U)
				{
					if (num != 1888237428U)
					{
						if (num == 2959139256U)
						{
							if (server == "Nhị Kiếm")
							{
								return 1;
							}
						}
					}
					else if (server == "Song Kiếm")
					{
						return 8;
					}
				}
				else if (num != 3272466977U)
				{
					if (num != 3466219342U)
					{
						if (num == 4230872838U)
						{
							if (server == "Du Kiếm")
							{
								return 7;
							}
						}
					}
					else if (server == "Nhất Kiếm")
					{
						return 0;
					}
				}
				else if (server == "Tiếu Ngạo")
				{
					return 4;
				}
				return -1;
			}
		}

		// Token: 0x170000FE RID: 254
		// (get) Token: 0x060004B4 RID: 1204 RVA: 0x0001ACF0 File Offset: 0x00018EF0
		public int TailIndex
		{
			get
			{
				string server = this.Server;
				uint num = <PrivateImplementationDetails>.ComputeStringHash(server);
				if (num <= 1807885226U)
				{
					if (num <= 943938042U)
					{
						if (num != 748293993U)
						{
							if (num == 943938042U)
							{
								if (server == "Tứ Kiếm")
								{
									return 4;
								}
							}
						}
						else if (server == "Ảnh Kiếm")
						{
							return -1;
						}
					}
					else if (num != 1098765051U)
					{
						if (num != 1106729041U)
						{
							if (num == 1807885226U)
							{
								if (server == "Tái Chiến")
								{
									return 5;
								}
							}
						}
						else if (server == "Long Kiếm")
						{
							return 7;
						}
					}
					else if (server == "Tam Kiếm")
					{
						return 3;
					}
				}
				else if (num <= 2959139256U)
				{
					if (num != 1888237428U)
					{
						if (num == 2959139256U)
						{
							if (server == "Nhị Kiếm")
							{
								return 2;
							}
						}
					}
					else if (server == "Song Kiếm")
					{
						return 9;
					}
				}
				else if (num != 3272466977U)
				{
					if (num != 3466219342U)
					{
						if (num == 4230872838U)
						{
							if (server == "Du Kiếm")
							{
								return 8;
							}
						}
					}
					else if (server == "Nhất Kiếm")
					{
						return 1;
					}
				}
				else if (server == "Tiếu Ngạo")
				{
					return 6;
				}
				return -1;
			}
		}

		// Token: 0x170000FF RID: 255
		// (get) Token: 0x060004B5 RID: 1205 RVA: 0x0001AE4B File Offset: 0x0001904B
		// (set) Token: 0x060004B6 RID: 1206 RVA: 0x0001AE53 File Offset: 0x00019053
		public bool IsSave { get; set; }

		// Token: 0x17000100 RID: 256
		// (get) Token: 0x060004B7 RID: 1207 RVA: 0x0001AE5C File Offset: 0x0001905C
		// (set) Token: 0x060004B8 RID: 1208 RVA: 0x0001AE8C File Offset: 0x0001908C
		public string LoginIndex
		{
			get
			{
				if (this.Node.SelectSingleNode("LoginIndex") == null)
				{
					return "1";
				}
				return this.Node.SelectSingleNode("LoginIndex").InnerText;
			}
			set
			{
				XmlNode xmlNode = this.Node.SelectSingleNode("LoginIndex");
				if (xmlNode == null)
				{
					xmlNode = Account.XML.CreateElement("LoginIndex");
					xmlNode.InnerText = "1";
					this.Node.AppendChild(xmlNode);
				}
				xmlNode.InnerText = value;
				Account.Save();
			}
		}

		// Token: 0x17000101 RID: 257
		// (get) Token: 0x060004B9 RID: 1209 RVA: 0x0001AEE1 File Offset: 0x000190E1
		// (set) Token: 0x060004BA RID: 1210 RVA: 0x0001AEE9 File Offset: 0x000190E9
		public bool IsSaveName { get; set; }

		// Token: 0x17000102 RID: 258
		// (get) Token: 0x060004BB RID: 1211 RVA: 0x0001AEF4 File Offset: 0x000190F4
		public string Path
		{
			get
			{
				string result;
				try
				{
					result = LoadFile.LoadFileWithDecrypt(Global.DataPath + "\\ExecutePath.dat");
				}
				catch
				{
					result = "";
				}
				return result;
			}
		}

		// Token: 0x17000103 RID: 259
		// (get) Token: 0x060004BC RID: 1212 RVA: 0x0001AF34 File Offset: 0x00019134
		public bool IsCaptcha
		{
			get
			{
				return this.game != null && this.game.TLBB.IsSelectCharacter && this.game.TLBB.IsTextCaptcha;
			}
		}

		// Token: 0x17000104 RID: 260
		// (get) Token: 0x060004BD RID: 1213 RVA: 0x0001AF62 File Offset: 0x00019162
		// (set) Token: 0x060004BE RID: 1214 RVA: 0x0001AF6A File Offset: 0x0001916A
		public Stopwatch OpenGameTime { get; set; }

		// Token: 0x17000105 RID: 261
		// (get) Token: 0x060004BF RID: 1215 RVA: 0x0001AF73 File Offset: 0x00019173
		public bool Online
		{
			get
			{
				return this.game != null && this.game.TLBB.Online;
			}
		}

		// Token: 0x060004C0 RID: 1216 RVA: 0x0001AF8F File Offset: 0x0001918F
		public void Answer(string txt)
		{
			if (this.game != null)
			{
				this.game.LuaDoOneLineString("DataPool:SendLoginCode(" + txt + ")");
			}
		}

		// Token: 0x17000106 RID: 262
		// (get) Token: 0x060004C1 RID: 1217 RVA: 0x0001AFB4 File Offset: 0x000191B4
		// (set) Token: 0x060004C2 RID: 1218 RVA: 0x0001AFBC File Offset: 0x000191BC
		private int BaseImg { get; set; }

		// Token: 0x17000107 RID: 263
		// (get) Token: 0x060004C3 RID: 1219 RVA: 0x0001AFC5 File Offset: 0x000191C5
		// (set) Token: 0x060004C4 RID: 1220 RVA: 0x0001AFCD File Offset: 0x000191CD
		public Bitmap KetQua { get; set; }

		// Token: 0x060004C5 RID: 1221 RVA: 0x0001AFD6 File Offset: 0x000191D6
		public void ReadCaptcha1()
		{
			this.IsReadCaptCha = false;
			this.worker.DoWork += delegate(object s, DoWorkEventArgs args)
			{
				try
				{
					this.Img = string.Empty;
					if (this.game == null)
					{
						this.KetQua = null;
					}
					else
					{
						Bitmap bitmap = new Bitmap(128, 36);
						int num;
						if (this.BaseImg != 0)
						{
							num = this.BaseImg + 4;
						}
						else
						{
							int moduleAddress = this.game.Memory.GetModuleAddress("UI_CEGUI.dll");
							num = this.game.Memory.Read(new int[]
							{
								moduleAddress + this.game.Address.Captcha[0],
								this.game.Address.Captcha[1],
								this.game.Address.Captcha[2],
								this.game.Address.Captcha[3]
							});
						}
						int num2 = this.game.Memory.Read2Byte(num);
						if (num2 != 40960 && num2 != 41215 && this.game.Address.GameType != 1)
						{
							if (this.BaseImg == 0)
							{
								this.BaseImg = this.game.Memory.Scan("## 00 00 00 ?? A0 ?? A0 ?? A0 ?? A0 ?? A0 ?? A0 ?? A0 ?? A0", 0, 0, 0);
							}
							num = this.BaseImg + 4;
							num2 = this.game.Memory.Read2Byte(num);
						}
						if (num2 == 40960 || num2 == 41215)
						{
							string text = "";
							for (int i = 0; i < 9216; i += 2)
							{
								num2 = this.game.Memory.Read2Byte(num + i);
								if (num2 == 40960)
								{
									bitmap.SetPixel(i / 2 % 128, i / 2 / 128, Color.Black);
									text += "0";
								}
								else
								{
									bitmap.SetPixel(i / 2 % 128, i / 2 / 128, Color.White);
									text += "1";
								}
								if (text.Length == 8)
								{
									this.Img += Convert.ToInt32(text, 2).ToString("X2");
									text = "";
								}
							}
							bitmap = new Bitmap(bitmap, new Size(160, 45));
						}
						this.KetQua = bitmap;
						this.IsReadCaptCha = true;
					}
				}
				catch (Exception ex)
				{
					Console.Write(ex.ToString());
					this.KetQua = null;
				}
			};
			this.worker.RunWorkerAsync();
		}

		// Token: 0x060004C6 RID: 1222 RVA: 0x0001B004 File Offset: 0x00019204
		public static void Load()
		{
			try
			{
				Account.XML.LoadXml(LoadFile.LoadFileWithDecrypt(Global.DataPath + "\\Account.dat"));
			}
			catch
			{
				if (Account.XML.SelectSingleNode("/*") == null)
				{
					Account.XML.InsertBefore(Account.XML.CreateXmlDeclaration("1.0", "UTF-8", null), Account.XML.DocumentElement);
					Account.XML.AppendChild(Account.XML.CreateNode(XmlNodeType.Element, "Accounts", ""));
					LoadFile.WriteFileWithEncrypt(Account.XML.OuterXml, Global.DataPath + "\\Account.dat");
				}
			}
		}

		// Token: 0x060004C7 RID: 1223 RVA: 0x0001B0C0 File Offset: 0x000192C0
		public static void Save()
		{
			foreach (object obj in Account.XML.SelectSingleNode("Accounts").SelectNodes("Account"))
			{
				XmlElement xmlElement = (XmlElement)obj;
				if (xmlElement.InnerXml.Trim() == string.Empty)
				{
					xmlElement.IsEmpty = true;
				}
			}
			LoadFile.WriteFileWithEncrypt(Account.XML.OuterXml, Global.DataPath + "\\Account.dat");
		}

		// Token: 0x060004C8 RID: 1224 RVA: 0x0001B164 File Offset: 0x00019364
		public static List<Account> Enum()
		{
			List<Account> list = new List<Account>();
			foreach (object obj in Account.XML.SelectSingleNode("Accounts").SelectNodes("Account"))
			{
				XmlElement node = (XmlElement)obj;
				try
				{
					Account item = new Account(node);
					list.Add(item);
				}
				catch
				{
				}
			}
			return list;
		}

		// Token: 0x04000315 RID: 789
		public bool IsReadCaptCha;

		// Token: 0x04000316 RID: 790
		private BackgroundWorker worker = new BackgroundWorker();

		// Token: 0x04000317 RID: 791
		public static XmlDocument XML = new XmlDocument();

		// Token: 0x04000318 RID: 792
		public bool IsSelectRole;

		// Token: 0x04000319 RID: 793
		public int IsSelect = 5;

		// Token: 0x0400031A RID: 794
		public int CharacterNum = 1;

		// Token: 0x0400031B RID: 795
		public XmlNode Node;

		// Token: 0x0400031C RID: 796
		public Game game;

		// Token: 0x0400031D RID: 797
		public bool IsGetAn;

		// Token: 0x0400031E RID: 798
		public bool Entered;

		// Token: 0x0400031F RID: 799
		public int SelectCount;

		// Token: 0x04000320 RID: 800
		public int LoginMessageTime;

		// Token: 0x04000321 RID: 801
		public string Status = string.Empty;

		// Token: 0x04000322 RID: 802
		public string Img = string.Empty;

		// Token: 0x04000323 RID: 803
		public string ImgHash = string.Empty;

		// Token: 0x04000324 RID: 804
		public bool IsBHD;

		// Token: 0x04000325 RID: 805
		public bool IsTrong;

		// Token: 0x04000326 RID: 806
		public bool IsNhanMam;

		// Token: 0x04000327 RID: 807
		public bool IsTrungAc;

		// Token: 0x04000328 RID: 808
		public bool IsDua;
	}
}
