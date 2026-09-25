using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Net;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using System.Xml;

namespace TinhKiemAuto
{
	// Token: 0x020000F8 RID: 248
	internal partial class Debug : Form
	{
		// Token: 0x06000D08 RID: 3336 RVA: 0x00053746 File Offset: 0x00051946
		public Debug(Game game)
		{
			this.game = game;
			this.InitializeComponent();
			this.Text = game.TLBB.Name;
			this.probGame.SelectedObject = game.Process;
		}

		// Token: 0x06000D09 RID: 3337 RVA: 0x00053780 File Offset: 0x00051980
		private void btnReadGameObject_Click(object sender, EventArgs e)
		{
			this.Text = (DateTime.Now.Ticks / 10000000L).ToString();
			this.listViewGameObject.Items.Clear();
			float num = 9999f;
			foreach (GameObject gameObject in this.game.Objects.All)
			{
				float distance = TINHKIEM.GetDistance(gameObject.X, gameObject.Y, this.game.CharX, this.game.CharY);
				if (distance < num && gameObject.Name != this.game.TLBB.Name)
				{
					num = distance;
					string name = gameObject.Name;
				}
				ListViewItem listViewItem = new ListViewItem(gameObject.Id.ToString());
				listViewItem.SubItems.Add(string.Concat(new object[]
				{
					gameObject.Name,
					" | ",
					gameObject.RoundX,
					",",
					gameObject.RoundY
				}));
				listViewItem.SubItems.Add(TINHKIEM.GetDistance(this.game.CharX, this.game.CharY, gameObject.X, gameObject.Y).ToString());
				listViewItem.Tag = gameObject;
				this.listViewGameObject.Items.Add(listViewItem);
			}
			this.listViewGameObject.Columns[0].Text = this.listViewGameObject.Items.Count.ToString();
		}

		// Token: 0x06000D0A RID: 3338 RVA: 0x0005396C File Offset: 0x00051B6C
		private void listViewGameObject_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (this.listViewGameObject.SelectedItems.Count == 0)
			{
				return;
			}
			GameObject selectedObject = this.listViewGameObject.SelectedItems[0].Tag as GameObject;
			this.pgGameObject.SelectedObject = selectedObject;
		}

		// Token: 0x06000D0B RID: 3339 RVA: 0x000539B4 File Offset: 0x00051BB4
		private void tmrRefresh_Tick(object sender, EventArgs e)
		{
			this.txtTLBB.Text = this.game.TLBB.ToString();
		}

		// Token: 0x06000D0C RID: 3340 RVA: 0x000539D4 File Offset: 0x00051BD4
		private void btnReadSkill_Click(object sender, EventArgs e)
		{
			this.listViewSkill.Items.Clear();
			foreach (Skill skill in this.game.Skills)
			{
				ListViewItem value = new ListViewItem(new string[]
				{
					skill.PacketId.ToString(),
					skill.Name,
					skill.DelayOffset.ToString("X8")
				});
				this.listViewSkill.Items.Add(value);
			}
			this.listViewSkill.Columns[0].Text = this.listViewSkill.Items.Count.ToString();
		}

		// Token: 0x06000D0D RID: 3341 RVA: 0x00053AAC File Offset: 0x00051CAC
		private void btnDoString_Click(object sender, EventArgs e)
		{
			this.game.LuaDoString(this.txtDoString.Text);
		}

		// Token: 0x06000D0E RID: 3342 RVA: 0x00053AC4 File Offset: 0x00051CC4
		private void btnGameControl_Click(object sender, EventArgs e)
		{
			this.listViewGameControl.Items.Clear();
			foreach (GameControl gameControl in GameControl.Enum(this.game))
			{
				ListViewItem listViewItem = new ListViewItem(gameControl.Id.ToString());
				listViewItem.SubItems.Add(gameControl.Name);
				listViewItem.SubItems.Add(gameControl.PacketId.ToString());
				listViewItem.Tag = gameControl;
				this.listViewGameControl.Items.Add(listViewItem);
			}
		}

		// Token: 0x06000D0F RID: 3343 RVA: 0x00053B78 File Offset: 0x00051D78
		private void btnGameObjectParty_Click(object sender, EventArgs e)
		{
			this.listViewGameObject.Items.Clear();
			foreach (GameObject gameObject in this.game.Objects.Party)
			{
				ListViewItem listViewItem = new ListViewItem(gameObject.Id.ToString());
				listViewItem.SubItems.Add(gameObject.Name);
				listViewItem.Tag = gameObject;
				this.listViewGameObject.Items.Add(listViewItem);
			}
		}

		// Token: 0x06000D10 RID: 3344 RVA: 0x00053C20 File Offset: 0x00051E20
		private void btnCompress_Click(object sender, EventArgs e)
		{
			MessageBox.Show(this.game.TrangThaiXayDung);
		}

		// Token: 0x06000D11 RID: 3345 RVA: 0x00053C34 File Offset: 0x00051E34
		private void btnToString_Click(object sender, EventArgs e)
		{
			this.game.LuaDoOneLineString(this.txtDoString.Text);
			this.game.LuaToString();
			string text = this.game.LuaString();
			MessageBox.Show(text);
			if (text != "")
			{
				Clipboard.SetText(text);
			}
		}

		// Token: 0x06000D12 RID: 3346 RVA: 0x00053C89 File Offset: 0x00051E89
		public static string Int2Hex(int val)
		{
			return val.ToString("X8").Substring(5, 3);
		}

		// Token: 0x06000D13 RID: 3347 RVA: 0x00053CA0 File Offset: 0x00051EA0
		private void btnReadTask_Click(object sender, EventArgs e)
		{
			this.listViewTask.Items.Clear();
			foreach (Task task in Task.Enum(this.game))
			{
				if (!(task.Name.Trim() == ""))
				{
					ListViewItem listViewItem = new ListViewItem(new string[]
					{
						task.Id.ToString("X8"),
						task.Name
					});
					listViewItem.Tag = task;
					this.listViewTask.Items.Add(listViewItem);
				}
			}
			this.listViewTask.Columns[0].Text = "Id [" + this.listViewTask.Items.Count.ToString() + "]";
		}

		// Token: 0x06000D14 RID: 3348 RVA: 0x00053D98 File Offset: 0x00051F98
		private void listViewTask_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (this.listViewTask.SelectedItems.Count > 0)
			{
				Task task = (Task)this.listViewTask.SelectedItems[0].Tag;
				this.txtTask.Text = task.ToString();
			}
		}

		// Token: 0x06000D15 RID: 3349 RVA: 0x00053DE8 File Offset: 0x00051FE8
		private void btnReadDialog_Click(object sender, EventArgs e)
		{
			string text = "@";
			this.listViewDialog.Items.Clear();
			List<QuestFrame> list = QuestFrame.Enum(this.game);
			this.Text = QuestFrame.Enum(this.game).Count.ToString();
			foreach (QuestFrame questFrame in list)
			{
				ListViewItem listViewItem = new ListViewItem(new string[]
				{
					questFrame.Name
				});
				listViewItem.Tag = questFrame;
				text += questFrame.Name;
				this.listViewDialog.Items.Add(listViewItem);
			}
			this.listViewDialog.Columns[0].Text = "Id [" + this.listViewDialog.Items.Count.ToString() + "]";
			Clipboard.SetText(text);
		}

		// Token: 0x06000D16 RID: 3350 RVA: 0x00053EF0 File Offset: 0x000520F0
		private void listViewDialog_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (this.listViewDialog.SelectedItems.Count == 0)
			{
				return;
			}
			QuestFrame questFrame = this.listViewDialog.SelectedItems[0].Tag as QuestFrame;
			this.txtDialog.Text = questFrame.ToString();
		}

		// Token: 0x06000D17 RID: 3351 RVA: 0x00053F40 File Offset: 0x00052140
		private void txtDoString_KeyDown(object sender, KeyEventArgs e)
		{
			TextBox textBox = (TextBox)sender;
			if (e.Control && e.KeyCode == Keys.A)
			{
				textBox.SelectAll();
			}
		}

		// Token: 0x06000D18 RID: 3352 RVA: 0x00053F6C File Offset: 0x0005216C
		private void btnReadPacket_Click(object sender, EventArgs e)
		{
			this.listViewPacket.Items.Clear();
			foreach (PacketItem packetItem in PacketItem.Enum(this.game))
			{
				ListViewItem listViewItem = new ListViewItem(new string[]
				{
					packetItem.PacketId.ToString(),
					packetItem.Name
				});
				listViewItem.Tag = packetItem;
				this.listViewPacket.Items.Add(listViewItem);
			}
		}

		// Token: 0x06000D19 RID: 3353 RVA: 0x0005400C File Offset: 0x0005220C
		private void listViewPacket_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (this.listViewPacket.SelectedItems.Count > 0)
			{
				PacketItem packetItem = (PacketItem)this.listViewPacket.SelectedItems[0].Tag;
				this.txtPacket.Text = packetItem.Info;
			}
		}

		// Token: 0x06000D1A RID: 3354 RVA: 0x0005405C File Offset: 0x0005225C
		private void btnGameObjectMonter_Click(object sender, EventArgs e)
		{
			this.listViewGameObject.Items.Clear();
			foreach (GameObject gameObject in this.game.Objects.Monter)
			{
				ListViewItem listViewItem = new ListViewItem(gameObject.Id.ToString());
				listViewItem.SubItems.Add(gameObject.Name);
				listViewItem.Tag = gameObject;
				this.listViewGameObject.Items.Add(listViewItem);
			}
		}

		// Token: 0x06000D1B RID: 3355 RVA: 0x00054104 File Offset: 0x00052304
		private void btnReadShop_Click(object sender, EventArgs e)
		{
			this.listViewShop.Items.Clear();
			foreach (Shop shop in Shop.Enum(this.game))
			{
				ListViewItem listViewItem = new ListViewItem(new string[]
				{
					shop.DefineId.ToString(),
					shop.Name,
					shop.Class.ToString("X8")
				});
				listViewItem.Tag = shop;
				this.listViewShop.Items.Add(listViewItem);
			}
		}

		// Token: 0x06000D1C RID: 3356 RVA: 0x000541B4 File Offset: 0x000523B4
		private void listViewGameControl_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (this.listViewGameControl.SelectedItems.Count == 0)
			{
				return;
			}
			GameControl gameControl = this.listViewGameControl.SelectedItems[0].Tag as GameControl;
			this.txtGameControl.Text = gameControl.ToString();
		}

		// Token: 0x06000D1D RID: 3357 RVA: 0x00054201 File Offset: 0x00052401
		private void btnResetTime_Click(object sender, EventArgs e)
		{
			this.game.ResetTime();
		}

		// Token: 0x06000D1E RID: 3358 RVA: 0x00054210 File Offset: 0x00052410
		private void btn20m_Click(object sender, EventArgs e)
		{
			this.listViewGameObject.Items.Clear();
			foreach (GameObject gameObject in this.game.Objects.NearMonter20m)
			{
				ListViewItem listViewItem = new ListViewItem(gameObject.Id.ToString());
				listViewItem.SubItems.Add(gameObject.Name);
				listViewItem.Tag = gameObject;
				this.listViewGameObject.Items.Add(listViewItem);
			}
		}

		// Token: 0x06000D1F RID: 3359 RVA: 0x000542B8 File Offset: 0x000524B8
		private void btnSendPacket_Click(object sender, EventArgs e)
		{
			this.game.SendPacket(this.txtSendPacket.Text);
		}

		// Token: 0x06000D20 RID: 3360 RVA: 0x000542D0 File Offset: 0x000524D0
		private void txtCode_TextChanged(object sender, EventArgs e)
		{
			string text = this.txtCode.Text;
			text = text.Replace('\t', ' ');
			text = text.Replace('\r', ' ');
			text = text.Replace('\n', ' ');
			text = text.Replace("  ", " ");
			text = text.Replace("  ", " ");
			text = text.Replace("  ", " ");
			text = text.Replace("  ", " ");
			text = text.Replace("  ", " ");
			this.txtNewCode.Text = text;
		}

		// Token: 0x06000D21 RID: 3361 RVA: 0x0005436C File Offset: 0x0005256C
		private void txtCode_KeyDown(object sender, KeyEventArgs e)
		{
			TextBox textBox = (TextBox)sender;
			if (e.Control && e.KeyCode == Keys.A)
			{
				textBox.SelectAll();
			}
		}

		// Token: 0x06000D22 RID: 3362 RVA: 0x00054398 File Offset: 0x00052598
		private void txtNewCode_KeyDown(object sender, KeyEventArgs e)
		{
			TextBox textBox = (TextBox)sender;
			if (e.Control && e.KeyCode == Keys.A)
			{
				textBox.SelectAll();
			}
		}

		// Token: 0x06000D23 RID: 3363 RVA: 0x00006740 File Offset: 0x00004940
		private void txtSendPacket_TextChanged(object sender, EventArgs e)
		{
		}

		// Token: 0x06000D24 RID: 3364 RVA: 0x000543C4 File Offset: 0x000525C4
		private void listViewShop_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (this.listViewShop.SelectedItems.Count > 0)
			{
				Shop shop = (Shop)this.listViewShop.SelectedItems[0].Tag;
				this.txtShop.Text = shop.ToString();
			}
		}

		// Token: 0x06000D25 RID: 3365 RVA: 0x00054414 File Offset: 0x00052614
		private void listViewShop_DoubleClick(object sender, EventArgs e)
		{
			if (this.listViewShop.SelectedItems.Count > 0)
			{
				Shop shop = (Shop)this.listViewShop.SelectedItems[0].Tag;
				this.game.Buy(shop.Index);
			}
		}

		// Token: 0x06000D26 RID: 3366 RVA: 0x00054461 File Offset: 0x00052661
		private void btnPushDebugMessage_Click(object sender, EventArgs e)
		{
			this.game.LuaDoUnicodeString("PushDebugMessage(\"" + this.txtPushDebugMessage.Text + "\");");
		}

		// Token: 0x06000D27 RID: 3367 RVA: 0x00006740 File Offset: 0x00004940
		private void txtUnicode_TextChanged(object sender, EventArgs e)
		{
		}

		// Token: 0x06000D28 RID: 3368 RVA: 0x00054488 File Offset: 0x00052688
		private void txtVISCII_TextChanged(object sender, EventArgs e)
		{
			this.txtUnicode.Text = Memory.VISCII2Unicode(this.txtVISCII.Text);
		}

		// Token: 0x06000D29 RID: 3369 RVA: 0x000544A8 File Offset: 0x000526A8
		private void txtUnicode_KeyDown(object sender, KeyEventArgs e)
		{
			TextBox textBox = (TextBox)sender;
			if (e.Control && e.KeyCode == Keys.A)
			{
				textBox.SelectAll();
			}
		}

		// Token: 0x06000D2A RID: 3370 RVA: 0x000544D4 File Offset: 0x000526D4
		private void txtVISCII_KeyDown(object sender, KeyEventArgs e)
		{
			TextBox textBox = (TextBox)sender;
			if (e.Control && e.KeyCode == Keys.A)
			{
				textBox.SelectAll();
			}
		}

		// Token: 0x06000D2B RID: 3371 RVA: 0x00054500 File Offset: 0x00052700
		private void btnSendCaptcha_Click(object sender, EventArgs e)
		{
			FileStream fileStream = new FileStream("d:\\captcha.jpg", FileMode.Open, FileAccess.Read);
			byte[] array = new byte[fileStream.Length];
			fileStream.Read(array, 0, array.Length);
			fileStream.Close();
			Dictionary<string, object> dictionary = new Dictionary<string, object>();
			dictionary.Add("file", new FormUpload.FileParameter(array, "captcha.jpg", "image/jpeg"));
			dictionary.Add("key", "fb1957526402d781d58e9c00a62cafa6");
			dictionary.Add("numeric", "1");
			dictionary.Add("min_len", "4");
			dictionary.Add("max_len", "4");
			dictionary.Add("submit", "download and get the ID");
			string postUrl = "http://2captcha.com/in.php";
			string userAgent = "Mozilla/5.0 (Windows NT 6.1; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/52.0.2743.116 Safari/537.36";
			HttpWebResponse httpWebResponse = FormUpload.MultipartFormDataPost(postUrl, userAgent, dictionary);
			string text = new StreamReader(httpWebResponse.GetResponseStream()).ReadToEnd();
			httpWebResponse.Close();
			MessageBox.Show(text);
		}

		// Token: 0x06000D2C RID: 3372 RVA: 0x000545D8 File Offset: 0x000527D8
		private void btnAtk_Click(object sender, EventArgs e)
		{
			if (this.btnAtk.Text == "Atk")
			{
				this.btnAtk.Text = "Stop";
			}
			else
			{
				this.btnAtk.Text = "Atk";
			}
			this.IsAtk = !this.IsAtk;
			this.Atk();
		}

		// Token: 0x06000D2D RID: 3373 RVA: 0x00054634 File Offset: 0x00052834
		private void Atk()
		{
			while (Debug.listAtk.Count < this.nudAtkCount.Value && this.IsAtk)
			{
				Poster poster = new Poster();
				Debug.listAtk.Add(poster);
				poster.Url = this.txtUrlAtk.Text;
				poster.Control = this;
				poster.Completed += this.poster_Completed;
				poster.Get();
			}
		}

		// Token: 0x06000D2E RID: 3374 RVA: 0x000546B0 File Offset: 0x000528B0
		private void poster_Completed(object sender, EventArgs e)
		{
			Poster item = sender as Poster;
			Debug.listAtk.Remove(item);
			this.Atk();
		}

		// Token: 0x06000D2F RID: 3375
		[DllImport("user32.dll")]
		private static extern IntPtr SetParent(IntPtr hWndChild, IntPtr hWndNewParent);

		// Token: 0x06000D30 RID: 3376
		[DllImport("user32.dll")]
		private static extern int SetWindowLong(IntPtr hWnd, int nIndex, int dwNewLong);

		// Token: 0x06000D31 RID: 3377
		[DllImport("user32.dll", SetLastError = true)]
		internal static extern bool MoveWindow(IntPtr hWnd, int X, int Y, int nWidth, int nHeight, bool bRepaint);

		// Token: 0x06000D32 RID: 3378
		[DllImport("USER32.DLL")]
		public static extern int GetWindowLong(IntPtr hWnd, int nIndex);

		// Token: 0x06000D33 RID: 3379 RVA: 0x000546D6 File Offset: 0x000528D6
		private void btnTest_Click(object sender, EventArgs e)
		{
			this.game.LUA.TogleMissionOutline();
		}

		// Token: 0x06000D34 RID: 3380 RVA: 0x000546E8 File Offset: 0x000528E8
		private void button1_Click(object sender, EventArgs e)
		{
			Process process = this.game.Process;
			for (int i = 0; i < process.Modules.Count; i++)
			{
				if (process.Modules[i].FileName.ToLower().Contains("static"))
				{
					MessageBox.Show(process.Modules[i].ModuleMemorySize.ToString("X8"));
				}
			}
		}

		// Token: 0x06000D35 RID: 3381 RVA: 0x0005475D File Offset: 0x0005295D
		private void Debug_Load(object sender, EventArgs e)
		{
			this.pgTLBB.SelectedObject = this.game.TLBB;
		}

		// Token: 0x06000D36 RID: 3382 RVA: 0x00054778 File Offset: 0x00052978
		private void btnPhone_Click(object sender, EventArgs e)
		{
			MessageBox.Show(TINHKIEM.IsPhoneNumber(this.txtPhone.Text).ToString());
		}

		// Token: 0x06000D37 RID: 3383 RVA: 0x000547A3 File Offset: 0x000529A3
		private void button2_Click(object sender, EventArgs e)
		{
			string recvDat = this.game.RecvDat;
			this.txtDoString.Text = this.game.RecvDat;
		}

		// Token: 0x06000D38 RID: 3384 RVA: 0x000547C8 File Offset: 0x000529C8
		private void btnLoadScripts_Click(object sender, EventArgs e)
		{
			this.listViewScript.Items.Clear();
			foreach (Script script in Scripts.Load())
			{
				ListViewItem listViewItem = new ListViewItem(script.ID);
				listViewItem.SubItems.Add("");
				listViewItem.SubItems.Add("");
				listViewItem.SubItems.Add("");
				listViewItem.SubItems.Add("");
				listViewItem.SubItems.Add("");
				listViewItem.Tag = script;
				listViewItem.SubItems[1].Text = script.MD;
				listViewItem.SubItems[2].Text = script.Recv;
				listViewItem.SubItems[3].Text = script.Send;
				listViewItem.SubItems[4].Text = script.Do;
				listViewItem.SubItems[5].Text = script.Info;
				this.listViewScript.Items.Add(listViewItem);
			}
		}

		// Token: 0x06000D39 RID: 3385 RVA: 0x00006740 File Offset: 0x00004940
		private void menuEditScript_Click(object sender, EventArgs e)
		{
		}

		// Token: 0x06000D3A RID: 3386 RVA: 0x00054924 File Offset: 0x00052B24
		private void button3_Click(object sender, EventArgs e)
		{
			this.game.LuaDoString(ImageResource.LuaEx);
			this.game.LuaToString();
			string text = this.game.LuaStringEx();
			MessageBox.Show(text);
			if (text != "")
			{
				Clipboard.SetText(text);
			}
			List<Script> list = Scripts.Load();
			if (MessageBox.Show(this, "Bạn có muốn thêm script", "", MessageBoxButtons.YesNo) == DialogResult.Yes)
			{
				text = text.Replace("=", ":");
				text = text.Replace("   ", "");
				text = text.Replace("  ", "");
				text.Trim(new char[]
				{
					';'
				});
				foreach (string text2 in text.Split(new char[]
				{
					';'
				}))
				{
					if (!(text2.Trim() == ""))
					{
						try
						{
							Stopwatch.StartNew();
							int num = TINHKIEM.ParseInt(text2.Substring(0, text2.IndexOf(' ')));
							string text3 = text2.Substring(text2.IndexOf(' ') + 1);
							string text4 = TINHKIEM.Hasher.MD5(text3);
							foreach (Script script in list)
							{
								script.MD.Contains(text4);
							}
							string text5 = Regex.Replace(text3, ".*{_INFOAIM", "");
							int val = TINHKIEM.ParseInt(text5.Split(new char[]
							{
								','
							})[0]);
							int val2 = TINHKIEM.ParseInt(text5.Split(new char[]
							{
								','
							})[1]);
							int val3 = 0;
							try
							{
								val3 = TINHKIEM.ParseInt(text5.Split(new char[]
								{
									','
								})[2]);
							}
							catch
							{
							}
							Scripts.Add(new Script
							{
								MD = text4,
								Recv = Debug.Int2Hex(0) + Debug.Int2Hex(val) + Debug.Int2Hex(val2) + Debug.Int2Hex(val3),
								Send = Debug.Int2Hex(0) + Debug.Int2Hex(val) + Debug.Int2Hex(val2) + Debug.Int2Hex(val3),
								Level = num.ToString(),
								Info = text3
							});
						}
						catch (Exception ex)
						{
							MessageBox.Show(ex.Message + ex.StackTrace);
						}
					}
				}
			}
			Scripts.Save();
		}

		// Token: 0x06000D3B RID: 3387 RVA: 0x00054BD8 File Offset: 0x00052DD8
		private void menuDeleteScript_Click(object sender, EventArgs e)
		{
			if (this.listViewScript.SelectedItems.Count == 0)
			{
				return;
			}
			if (MessageBox.Show(this, "Bạn có muốn xóa thông tin những nhiệm vụ đã chọn", "", MessageBoxButtons.YesNo) == DialogResult.Yes)
			{
				foreach (object obj in this.listViewScript.SelectedItems)
				{
					ListViewItem listViewItem = (ListViewItem)obj;
					try
					{
						XmlNode node = ((Script)listViewItem.Tag).Node;
						Scripts.XML.SelectSingleNode("/*").RemoveChild(node);
						listViewItem.Remove();
					}
					catch
					{
					}
				}
				Scripts.Save();
				Scripts.Load();
			}
		}

		// Token: 0x06000D3C RID: 3388 RVA: 0x00054CA0 File Offset: 0x00052EA0
		private void talkToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if (this.listViewGameObject.SelectedItems.Count > 0)
			{
				GameObject gameObject = this.listViewGameObject.SelectedItems[0].Tag as GameObject;
				this.game.Talk(gameObject.Id);
			}
		}

		// Token: 0x06000D3D RID: 3389 RVA: 0x00054CF0 File Offset: 0x00052EF0
		private void pickToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if (this.listViewGameObject.SelectedItems.Count > 0)
			{
				GameObject gameObject = this.listViewGameObject.SelectedItems[0].Tag as GameObject;
				this.game.PickItem(gameObject.Id);
			}
		}

		// Token: 0x06000D3E RID: 3390 RVA: 0x00054D40 File Offset: 0x00052F40
		private void collectToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if (this.listViewGameObject.SelectedItems.Count > 0)
			{
				GameObject gameObject = this.listViewGameObject.SelectedItems[0].Tag as GameObject;
				this.game.UseSkill(3, gameObject.Id);
			}
		}

		// Token: 0x06000D3F RID: 3391 RVA: 0x00054D90 File Offset: 0x00052F90
		private void listViewScript_DragDrop(object sender, DragEventArgs e)
		{
			XmlNode xmlNode = Scripts.XML.SelectSingleNode("/*");
			xmlNode.RemoveAll();
			foreach (object obj in this.listViewScript.Items)
			{
				Script script = ((ListViewItem)obj).Tag as Script;
				xmlNode.AppendChild(script.Node);
			}
			Scripts.Save();
		}

		// Token: 0x06000D40 RID: 3392 RVA: 0x00054DF8 File Offset: 0x00052FF8
		private void instanceToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if (this.listViewGameObject.SelectedItems.Count > 0)
			{
				GameObject gameObject = this.listViewGameObject.SelectedItems[0].Tag as GameObject;
				Clipboard.SetText(string.Concat(new object[]
				{
					"public static NPC ",
					TINHKIEM.ClearSign(gameObject.Name).Replace(" ", ""),
					"  = new NPC()\r\n{\r\n\tId = ",
					gameObject.Id,
					",\r\n\tX = ",
					gameObject.RoundX,
					",\r\n\tY = ",
					gameObject.RoundY,
					",\r\n\tMap = Id,\r\n\tINFOAIM = \"#G",
					this.game.TLBB.MapName,
					"#R",
					gameObject.Name,
					"#{_INFOAIM",
					gameObject.RoundX,
					",",
					gameObject.RoundY,
					",",
					this.game.TLBB.MapId,
					",",
					gameObject.Name,
					"}\"\r\n};"
				}));
			}
		}

		// Token: 0x06000D41 RID: 3393 RVA: 0x00054F4C File Offset: 0x0005314C
		private void refreshToolStripMenuItem_Click(object sender, EventArgs e)
		{
			this.listViewScript.Items.Clear();
			foreach (Script script in Scripts.Load())
			{
				ListViewItem listViewItem = new ListViewItem(script.ID);
				listViewItem.SubItems.Add("");
				listViewItem.SubItems.Add("");
				listViewItem.SubItems.Add("");
				listViewItem.SubItems.Add("");
				listViewItem.SubItems.Add("");
				listViewItem.SubItems.Add("");
				listViewItem.SubItems.Add("");
				listViewItem.Tag = script;
				listViewItem.SubItems[1].Text = script.MD;
				listViewItem.SubItems[2].Text = script.Recv;
				listViewItem.SubItems[3].Text = script.Send;
				listViewItem.SubItems[4].Text = script.Do;
				listViewItem.SubItems[5].Text = script.Info;
				listViewItem.SubItems[6].Text = script.Name;
				listViewItem.SubItems[7].Text = script.Level;
				this.listViewScript.Items.Add(listViewItem);
			}
		}

		// Token: 0x06000D42 RID: 3394 RVA: 0x00006740 File Offset: 0x00004940
		private void listViewScript_SelectedIndexChanged(object sender, EventArgs e)
		{
		}

		// Token: 0x06000D43 RID: 3395 RVA: 0x000550F8 File Offset: 0x000532F8
		private void Editer_Edited(object sender, EventArgs e)
		{
			if (this.listViewScript.SelectedItems.Count == 0)
			{
				return;
			}
			ListViewItem listViewItem = this.listViewScript.SelectedItems[0];
			Script script = this.listViewScript.SelectedItems[0].Tag as Script;
			listViewItem.Text = script.ID;
			listViewItem.SubItems[1].Text = script.MD;
			listViewItem.SubItems[2].Text = script.Recv;
			listViewItem.SubItems[3].Text = script.Send;
			listViewItem.SubItems[4].Text = script.Do;
			listViewItem.SubItems[5].Text = script.Info;
			listViewItem.SubItems[6].Text = script.Name;
			MessageBox.Show("Saved");
		}

		// Token: 0x06000D44 RID: 3396 RVA: 0x000551E5 File Offset: 0x000533E5
		private void checkBox1_CheckedChanged(object sender, EventArgs e)
		{
			Debug.NotRead = this.chkNotRead.Checked;
		}

		// Token: 0x06000D45 RID: 3397 RVA: 0x000551F8 File Offset: 0x000533F8
		private void menuPickObject_Click(object sender, EventArgs e)
		{
			if (this.listViewGameObject.SelectedItems.Count > 0)
			{
				GameObject @object = this.listViewGameObject.SelectedItems[0].Tag as GameObject;
				this.game.PickObject(@object);
			}
		}

		// Token: 0x06000D46 RID: 3398 RVA: 0x00055240 File Offset: 0x00053440
		private void btnrRemoveHex_Click(object sender, EventArgs e)
		{
			this.txtVISCII.Text = ConverterEx.Hex2String(this.txtUnicode.Text);
			this.txtVISCII.Text = ConverterEx.CleanJarVar(this.txtVISCII.Text);
		}

		// Token: 0x06000D47 RID: 3399 RVA: 0x00006740 File Offset: 0x00004940
		private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
		{
		}

		// Token: 0x06000D48 RID: 3400 RVA: 0x00055278 File Offset: 0x00053478
		private void button4_Click(object sender, EventArgs e)
		{
			this.txtVISCII.Text = ConverterEx.Unicode2VISCII(this.txtUnicode.Text);
		}

		// Token: 0x06000D49 RID: 3401 RVA: 0x00055298 File Offset: 0x00053498
		private void button5_Click(object sender, EventArgs e)
		{
			this.listViewPacket.Items.Clear();
			foreach (PacketItem packetItem in PacketItem.EnumTrangBi(this.game))
			{
				ListViewItem listViewItem = new ListViewItem(new string[]
				{
					packetItem.PacketId.ToString(),
					packetItem.Name
				});
				listViewItem.Tag = packetItem;
				this.listViewPacket.Items.Add(listViewItem);
			}
		}

		// Token: 0x06000D4A RID: 3402 RVA: 0x00055338 File Offset: 0x00053538
		private void numericUpDown1_ValueChanged(object sender, EventArgs e)
		{
			Debug.offset = (int)this.numericUpDown1.Value;
		}

		// Token: 0x06000D4B RID: 3403 RVA: 0x00055350 File Offset: 0x00053550
		private void button6_Click(object sender, EventArgs e)
		{
			foreach (Modules modules in ProcessManager.CollectModules(this.game.Process))
			{
				this.txtDoString.Text = this.txtDoString.Text + modules.ModuleName + "\r\n";
			}
		}

		// Token: 0x06000D4C RID: 3404 RVA: 0x000553CC File Offset: 0x000535CC
		private void button7_Click(object sender, EventArgs e)
		{
			this.listViewPacket.Items.Clear();
			foreach (Bank bank in new Bank(this.game).Enum())
			{
				ListViewItem listViewItem = new ListViewItem(new string[]
				{
					bank.PacketId.ToString(),
					bank.Name
				});
				listViewItem.Tag = bank;
				this.listViewPacket.Items.Add(listViewItem);
			}
		}

		// Token: 0x06000D4D RID: 3405 RVA: 0x00006740 File Offset: 0x00004940
		private void tmrTest_Tick(object sender, EventArgs e)
		{
		}

		// Token: 0x06000D4E RID: 3406 RVA: 0x00055470 File Offset: 0x00053670
		private void button9_Click(object sender, EventArgs e)
		{
			FrmMain.CurGame.GetBestTarget();
			if (FrmMain.CurGame.BestTarget != null)
			{
				FrmMain.CurGame.UseSkill(int.Parse(this.txtidskill.Text), FrmMain.CurGame.BestTarget.Id);
			}
		}

		// Token: 0x06000D4F RID: 3407 RVA: 0x000554BC File Offset: 0x000536BC
		private void listViewSkill_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (this.listViewSkill.SelectedItems.Count == 0)
			{
				return;
			}
			ListViewItem listViewItem = this.listViewSkill.SelectedItems[0];
			this.txtidskill.Text = listViewItem.Text;
			this.txtnameskill.Text = listViewItem.SubItems[1].Text;
		}

		// Token: 0x06000D50 RID: 3408 RVA: 0x0005551B File Offset: 0x0005371B
		private void button8_Click(object sender, EventArgs e)
		{
			FrmMain.CurGame.GetBestTarget();
		}

		// Token: 0x06000D51 RID: 3409 RVA: 0x00055527 File Offset: 0x00053727
		private void button10_Click(object sender, EventArgs e)
		{
			FrmMain.CurGame.UseSkill(int.Parse(this.txtidskill.Text));
		}

		// Token: 0x06000D52 RID: 3410 RVA: 0x00055543 File Offset: 0x00053743
		private void button11_Click(object sender, EventArgs e)
		{
			FrmMain.CurGame.UseSkill(int.Parse(this.txtidskill.Text), FrmMain.CurGame.Objects.Self.Id);
		}

		// Token: 0x06000D53 RID: 3411 RVA: 0x00055573 File Offset: 0x00053773
		private void button12_Click(object sender, EventArgs e)
		{
			this.txtbufff.Text = FrmMain.CurGame.Objects.Self.BuffToString;
		}

		// Token: 0x06000D54 RID: 3412 RVA: 0x00055594 File Offset: 0x00053794
		private void button13_Click(object sender, EventArgs e)
		{
			MessageBox.Show(this.game.TLBB.GetMonPhaiName());
		}

		// Token: 0x06000D55 RID: 3413 RVA: 0x000555AC File Offset: 0x000537AC
		private void button14_Click(object sender, EventArgs e)
		{
			MessageBox.Show(this.game.TLBB.Rage.ToString() ?? "");
		}

		// Token: 0x06000D56 RID: 3414 RVA: 0x000555D2 File Offset: 0x000537D2
		private void button15_Click(object sender, EventArgs e)
		{
			FrmMain.CurGame.KinhCong();
		}

		// Token: 0x06000D57 RID: 3415 RVA: 0x000555DE File Offset: 0x000537DE
		private void button16_Click(object sender, EventArgs e)
		{
			FrmMain.CurGame.GiamDinh();
		}

		// Token: 0x06000D58 RID: 3416 RVA: 0x000555EA File Offset: 0x000537EA
		private void button17_Click(object sender, EventArgs e)
		{
			FrmMain.CurGame.DragTo42();
		}

		// Token: 0x04000A12 RID: 2578
		private Game game;

		// Token: 0x04000A13 RID: 2579
		private static List<Poster> listAtk = new List<Poster>();

		// Token: 0x04000A14 RID: 2580
		private bool IsAtk;

		// Token: 0x04000A15 RID: 2581
		private const int WS_BORDER = 8388608;

		// Token: 0x04000A16 RID: 2582
		private const int WS_DLGFRAME = 4194304;

		// Token: 0x04000A17 RID: 2583
		private const int WS_CAPTION = 12582912;

		// Token: 0x04000A18 RID: 2584
		private const int WS_SYSMENU = 524288;

		// Token: 0x04000A19 RID: 2585
		private const int WS_THICKFRAME = 262144;

		// Token: 0x04000A1A RID: 2586
		private const int WS_MINIMIZE = 536870912;

		// Token: 0x04000A1B RID: 2587
		private const int WS_MAXIMIZEBOX = 65536;

		// Token: 0x04000A1C RID: 2588
		private const int GWL_STYLE = -16;

		// Token: 0x04000A1D RID: 2589
		private const int GWL_EXSTYLE = -20;

		// Token: 0x04000A1E RID: 2590
		private const int WS_EX_DLGMODALFRAME = 1;

		// Token: 0x04000A1F RID: 2591
		private const int SWP_NOMOVE = 2;

		// Token: 0x04000A20 RID: 2592
		private const int SWP_NOSIZE = 1;

		// Token: 0x04000A21 RID: 2593
		private const int SWP_FRAMECHANGED = 32;

		// Token: 0x04000A22 RID: 2594
		private const uint MF_BYPOSITION = 1024U;

		// Token: 0x04000A23 RID: 2595
		private const uint MF_REMOVE = 4096U;

		// Token: 0x04000A24 RID: 2596
		public static bool NotRead = false;

		// Token: 0x04000A25 RID: 2597
		public static int offset = 112;
	}
}
