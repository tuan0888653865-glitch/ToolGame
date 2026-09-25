using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Media;
using System.Net;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.Xml;
using TinhKiemAuto.AutoControl;
using TinhKiemAuto.Models;
using TinhKiemAuto.Properties;

namespace TinhKiemAuto
{
	// Token: 0x020000FA RID: 250
	public partial class FrmMain : Form
	{
		// Token: 0x1700031A RID: 794
		// (get) Token: 0x06000D6B RID: 3435 RVA: 0x0005ABF9 File Offset: 0x00058DF9
		// (set) Token: 0x06000D6C RID: 3436 RVA: 0x0005AC01 File Offset: 0x00058E01
		public SocketClient _SocketClient { get; set; }

		// Token: 0x06000D6D RID: 3437 RVA: 0x0005AC0C File Offset: 0x00058E0C
		public FrmMain()
		{
			this.InitializeComponent();
			Process.GetCurrentProcess().PriorityClass = ProcessPriorityClass.BelowNormal;
			FrmMain.TxtLog = this.txtlogs;
			FrmMain.ListView = this.ListViewNhanVat;
			FrmMain.Instance = this;
		}

		// Token: 0x1700031B RID: 795
		// (get) Token: 0x06000D6E RID: 3438 RVA: 0x0005AC64 File Offset: 0x00058E64
		public static string AllName
		{
			get
			{
				string text = "";
				foreach (KeyValuePair<int, Game> keyValuePair in FrmMain.dicGame)
				{
					Game value = keyValuePair.Value;
					if (value.TLBB.Online)
					{
						text = text + value.TLBB.Name + ",";
					}
				}
				return text;
			}
		}

		// Token: 0x1700031C RID: 796
		// (get) Token: 0x06000D6F RID: 3439 RVA: 0x0005ACE4 File Offset: 0x00058EE4
		// (set) Token: 0x06000D70 RID: 3440 RVA: 0x0005ACEB File Offset: 0x00058EEB
		public static bool AlarmAcBa { get; set; }

		// Token: 0x1700031D RID: 797
		// (get) Token: 0x06000D71 RID: 3441 RVA: 0x0005ACF3 File Offset: 0x00058EF3
		// (set) Token: 0x06000D72 RID: 3442 RVA: 0x0005ACFA File Offset: 0x00058EFA
		public static bool IsCalender { get; set; }

		// Token: 0x06000D73 RID: 3443 RVA: 0x0005AD02 File Offset: 0x00058F02
		public bool VuaBatXong()
		{
			return this.GETTIMEHIENTAI(DateTime.Now) - this.GETTIMEHIENTAI(FrmMain.startprogram) < 180;
		}

		// Token: 0x1700031E RID: 798
		// (get) Token: 0x06000D74 RID: 3444 RVA: 0x0005AD25 File Offset: 0x00058F25
		// (set) Token: 0x06000D75 RID: 3445 RVA: 0x0005AD2C File Offset: 0x00058F2C
		public static FrmMain Instance { get; set; }

		// Token: 0x1700031F RID: 799
		// (get) Token: 0x06000D76 RID: 3446 RVA: 0x0005AD34 File Offset: 0x00058F34
		// (set) Token: 0x06000D77 RID: 3447 RVA: 0x0005AD3B File Offset: 0x00058F3B
		public static bool IsFixed { get; set; }

		// Token: 0x06000D78 RID: 3448 RVA: 0x0005AD44 File Offset: 0x00058F44
		public static void AddLog(object log)
		{
			if (FrmMain.Instance.InvokeRequired)
			{
				FrmMain.Instance.Invoke(new FrmMain.LogBack(FrmMain.AddLog), new object[]
				{
					log
				});
				return;
			}
			FrmMain.Instance.txtlogs.AppendText(log.ToString() + "\n");
		}

		// Token: 0x17000320 RID: 800
		// (get) Token: 0x06000D79 RID: 3449 RVA: 0x0005AD9E File Offset: 0x00058F9E
		// (set) Token: 0x06000D7A RID: 3450 RVA: 0x0005ADA5 File Offset: 0x00058FA5
		public static int GameCount { get; set; }

		// Token: 0x06000D7B RID: 3451 RVA: 0x0005ADB0 File Offset: 0x00058FB0
		public Color GetCodeByPercen(int Percen)
		{
			Color result = Color.Lavender;
			if (Percen >= 60)
			{
				result = Color.LawnGreen;
			}
			else if (Percen < 60 && Percen >= 30)
			{
				result = Color.YellowGreen;
			}
			else
			{
				result = Color.OrangeRed;
			}
			return result;
		}

		// Token: 0x06000D7C RID: 3452 RVA: 0x0005ADEC File Offset: 0x00058FEC
		public Game SetForeGame()
		{
			foreach (object obj in FrmMain.ListView.Items)
			{
				ListViewItem listViewItem = (ListViewItem)obj;
				Game game = listViewItem.Tag as Game;
				if (game.Handle == Win.GetForegroundWindow())
				{
					FrmMain.CurGame = game;
					FrmMain.CurItem = listViewItem;
					this.DownSetting();
					this.LoadSkill();
					return game;
				}
			}
			return null;
		}

		// Token: 0x06000D7D RID: 3453 RVA: 0x0005AE84 File Offset: 0x00059084
		private void listView1_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (this.ListViewNhanVat.SelectedItems.Count > 0)
			{
				FrmMain.CurGame = (Game)this.ListViewNhanVat.SelectedItems[0].Tag;
				FrmMain.CurItem = this.ListViewNhanVat.SelectedItems[0];
				this.LoadSkill();
			}
			try
			{
				FrmMain.CurGame.SaveSetting();
				this.DownSetting();
			}
			catch
			{
			}
		}

		// Token: 0x06000D7E RID: 3454 RVA: 0x00006740 File Offset: 0x00004940
		private void panel3_Paint(object sender, PaintEventArgs e)
		{
		}

		// Token: 0x06000D7F RID: 3455 RVA: 0x0005AF08 File Offset: 0x00059108
		public static void LoadAccountLogin()
		{
			Account.Load();
			FrmMain.ListAutoLogin = Account.Enum();
		}

		// Token: 0x06000D80 RID: 3456
		[DllImport("Bin\\EasyHook.dll")]
		public static extern int GetMSG();

		// Token: 0x06000D81 RID: 3457
		[DllImport("Bin\\EasyHook.dll")]
		public static extern bool SetHook(IntPtr proseccid);

		// Token: 0x17000321 RID: 801
		// (get) Token: 0x06000D82 RID: 3458 RVA: 0x0005AF19 File Offset: 0x00059119
		// (set) Token: 0x06000D83 RID: 3459 RVA: 0x0005AF20 File Offset: 0x00059120
		public static bool IsStop { get; set; }

		// Token: 0x06000D84 RID: 3460 RVA: 0x0005AF28 File Offset: 0x00059128
		public bool GetUpdate()
		{
			bool result = false;
			if (new WebClient().DownloadString(Global.UpdateURL) == "True")
			{
				try
				{
					result = true;
					new Process
					{
						StartInfo = 
						{
							FileName = "AutoUpdate.exe",
							Arguments = ""
						}
					}.Start();
					Application.Exit();
				}
				catch
				{
					result = true;
					MessageBox.Show("Không tìm thấy tập tin AutoUpdate.exe\nVui lòng tải lại auto mới nhất trên trang chủ");
				}
			}
			return result;
		}

		// Token: 0x06000D85 RID: 3461 RVA: 0x0005AFA8 File Offset: 0x000591A8
		private void FrmMain_Load(object sender, EventArgs e)
		{
			bool flag = false;
			using (WebClient webClient = new WebClient())
			{
				try
				{
					string text = webClient.DownloadString(Global.UpdateURL);
					if (text.Length <= 0)
					{
						throw new Exception("Download error");
					}
					string[] array = text.Split(new char[]
					{
						'\r',
						'\n'
					});
					if (array.Length == 0)
					{
						throw new Exception("Split download content error");
					}
					string[] array2 = array;
					for (int i = 0; i < array2.Length; i++)
					{
						string[] array3 = array2[i].Split(new char[]
						{
							'='
						});
						if (array3.Length >= 2 && array3[0].Trim() == "Version")
						{
							if (int.Parse(array3[1].Trim()) > int.Parse(Global.Version))
							{
								throw new Exception("Version too low");
							}
							flag = true;
							this.txtlogs.AppendText("Checked Server Version: " + array3[1].Trim() + "\n");
						}
					}
				}
				catch (Exception ex)
				{
					MessageBox.Show(ex.Message);
					Process.Start("http://chickenauto.com/download");
				}
			}
			if (!flag)
			{
				base.Dispose();
				Application.Exit();
				return;
			}
			this.txtlogs.AppendText("Bật auto :" + DateTime.Now.ToString() + "\n");
			this.notifyIcon1.Text = "ChickenAuto";
			this.notifyIcon1.ContextMenu = new ContextMenu();
			this.notifyIcon1.ContextMenu.MenuItems.Add(new MenuItem("Hiện Auto", new EventHandler(this.HienAuto)));
			this.notifyIcon1.ContextMenu.MenuItems.Add(new MenuItem("Thoát Auto", new EventHandler(this.Thoat)));
			this.Text = "ChickenAuto | Version : " + Global.Version;
			if (!File.Exists(Global.DataPath + "\\20.dat"))
			{
				TINHKIEM.FileInstallMaHoa("TinhKiemAuto", "Scripts.xml", Global.DataPath + "\\20.dat");
			}
			Scripts.Load();
			Global.ExaclyTime = DateTime.Now;
			if (!this.AccountLogin.IsBusy)
			{
				this.AccountLogin.RunWorkerAsync();
			}
			if (Global.LanLuot)
			{
				this.tmrLogin.Interval = 10000;
			}
			if (!File.Exists(Global.DataPath + "\\MapPath.dat"))
			{
				TINHKIEM.FileInstall("TinhKiemAuto", "MapPath.dat", Global.DataPath + "\\MapPath.dat");
			}
			TINHKIEM.FileInstallMaHoa("TinhKiemAuto", "skillList.json", Global.DataPath + "\\SkillList.dat");
			if (!File.Exists(Global.DataPath + "\\16.dat"))
			{
				TINHKIEM.FileInstallMaHoa("TinhKiemAuto", "Screen.txt", Global.DataPath + "\\16.dat");
			}
			if (!File.Exists(Global.DataPath + "\\17.dat"))
			{
				TINHKIEM.FileInstallMaHoa("TinhKiemAuto", "PathList.txt", Global.DataPath + "\\17.dat");
			}
			if (!File.Exists(Global.DataPath + "\\18.dat"))
			{
				TINHKIEM.FileInstallMaHoa("TinhKiemAuto", "sdbai.json", Global.DataPath + "\\18.dat");
			}
			TrainData.LoadData();
			this.LoadTrainMap();
			if (!File.Exists(Global.DataPath + "\\19.dat"))
			{
				TINHKIEM.FileInstallMaHoa("TinhKiemAuto", "allmap.txt", Global.DataPath + "\\19.dat");
			}
			try
			{
				if (Environment.OSVersion.Version.Major > 5)
				{
					FrmMain.ChangeWindowMessageFilter = (FrmMain.ChangeWindowMessageFilterDelegate)FunctionLoader.LoadFunction<FrmMain.ChangeWindowMessageFilterDelegate>(Environment.SystemDirectory + "\\user32.dll", "ChangeWindowMessageFilter");
					FrmMain.ChangeWindowMessageFilter(74U, 1);
				}
				FrmMain.LoadAccountLogin();
				this.LoadConfig();
			}
			catch (Exception ex2)
			{
				MessageBox.Show(ex2.ToString());
			}
			Win.Active(this);
		}

		// Token: 0x06000D86 RID: 3462 RVA: 0x0005B3A4 File Offset: 0x000595A4
		public void LoadTrainMap()
		{
			string[] dsmap = TrainData.dsmap;
			List<BaiTrain> dsbai = TrainData.dsbai;
			if (dsmap == null)
			{
				return;
			}
			try
			{
				foreach (string text in dsmap)
				{
					List<int> list = new List<int>();
					foreach (BaiTrain baiTrain in dsbai)
					{
						if (baiTrain.MapName == text)
						{
							list.Add(baiTrain.Level);
						}
					}
					list.Sort();
					int num = list[0];
					list.Reverse();
					int num2 = list[0];
					FrmMain.ComboboxItem comboboxItem = new FrmMain.ComboboxItem();
					comboboxItem.Text = string.Concat(new string[]
					{
						text,
						"[",
						num.ToString(),
						"=>",
						num2.ToString(),
						"]"
					});
					comboboxItem.Value = text;
					this.comdanhsachbando.Items.Add(comboboxItem);
				}
				this.comdanhsachbando.SelectedIndex = 0;
			}
			catch
			{
			}
		}

		// Token: 0x06000D87 RID: 3463 RVA: 0x0005B4FC File Offset: 0x000596FC
		private void LoadConfig()
		{
			this.LoadSetting();
			if (Global.HookMessage != -1)
			{
				Global.HookMessage = FrmMain.GetMSG();
			}
			this.timeMonitor.Enabled = true;
			FrmMain.IsLoged = true;
			this.ThreadMonitor = new Thread(new ThreadStart(this.Monitor))
			{
				IsBackground = true
			};
			this.ThreadMonitor.Start();
			new Thread(new ThreadStart(this.Auto))
			{
				IsBackground = true
			}.Start();
		}

		// Token: 0x06000D88 RID: 3464 RVA: 0x0002225F File Offset: 0x0002045F
		public static int Bool2Int(bool value)
		{
			if (value)
			{
				return 1;
			}
			return 0;
		}

		// Token: 0x17000322 RID: 802
		// (get) Token: 0x06000D89 RID: 3465 RVA: 0x0005B57C File Offset: 0x0005977C
		private List<Game> AllGame
		{
			get
			{
				List<Game> list = new List<Game>();
				foreach (KeyValuePair<int, Game> keyValuePair in FrmMain.dicGame)
				{
					list.Add(keyValuePair.Value);
				}
				return list;
			}
		}

		// Token: 0x17000323 RID: 803
		// (get) Token: 0x06000D8A RID: 3466 RVA: 0x0005B5DC File Offset: 0x000597DC
		// (set) Token: 0x06000D8B RID: 3467 RVA: 0x0005B5E3 File Offset: 0x000597E3
		public static bool IsExit { get; set; }

		// Token: 0x06000D8C RID: 3468 RVA: 0x0005B5EC File Offset: 0x000597EC
		private void menuExit_Click(object sender, EventArgs e)
		{
			foreach (Game game in this.AllGame)
			{
				if (game.IsHooked && game.RecvAddress != 0)
				{
					game.SaveSetting();
					Memory.WriteProcessMemory(game.Memory.Id, game.RecvAddress, game.bufferRecv, 10, 0);
				}
			}
			FrmMain.IsExit = true;
			this.SaveSetting();
		}

		// Token: 0x06000D8D RID: 3469 RVA: 0x0005B67C File Offset: 0x0005987C
		public void SaveSetting()
		{
			string value = string.Concat(new object[]
			{
				FrmMain.Bool2Int(Global.FollowKey),
				",",
				FrmMain.Bool2Int(Global.PickItem),
				",",
				FrmMain.Bool2Int(Global.ItemFillter),
				",",
				Global.NoiRadius,
				",",
				Global.NgoaiRadius,
				",",
				Global.PickRadius,
				",",
				Global.ExitHPPercent,
				",",
				FrmMain.Bool2Int(Global.AlarmHP),
				",",
				Global.AlarmHPPercent,
				",",
				FrmMain.Bool2Int(Global.AutoUpLvl),
				",",
				Global.AutoUpLvlBelow,
				",",
				FrmMain.Bool2Int(Global.AutoDropItem),
				",",
				FrmMain.Bool2Int(Global.Mute),
				",",
				FrmMain.Bool2Int(Global.AutoShutDown),
				",",
				FrmMain.Bool2Int(Global.UseSkillPet),
				",",
				FrmMain.Bool2Int(Global.AutoComeBack),
				",",
				FrmMain.Bool2Int(Global.AlarmPk),
				",",
				FrmMain.Bool2Int(Global.ExitPk),
				",",
				Global.BuffHPPercent,
				",",
				Global.BuffMPPercent,
				",",
				Global.BuffNMPercent,
				",",
				FrmMain.Bool2Int(Global.BuffPet),
				",",
				FrmMain.Bool2Int(Global.IsBoQua),
				",",
				FrmMain.Bool2Int(Global.AutoResetTime),
				",",
				FrmMain.Bool2Int(Global.BuffQuanDoan),
				",",
				FrmMain.Bool2Int(Global.AutoAccept),
				",",
				FrmMain.Bool2Int(Global.AcceptAll),
				",",
				FrmMain.Bool2Int(Global.IsXaPhu),
				",",
				TINHKIEM.Key2Int(Global.BaseSkill),
				",",
				TINHKIEM.Key2Int(Global.NMSkill),
				",",
				TINHKIEM.Key2Int(Global.HPKey),
				",",
				TINHKIEM.Bool2Int(Global.IsHuyDanhQuai),
				",",
				Global.AntiCaptchaSelf.ToString(),
				",",
				Global.FollowRadius,
				",",
				Global.BHDCount,
				",",
				TINHKIEM.Bool2Int(Global.AutoPk),
				",",
				Global.MaxBHD,
				",",
				TINHKIEM.Bool2Int(Option.SetSafeTime),
				",",
				TINHKIEM.Bool2Int(Global.HideBHD),
				",",
				TINHKIEM.Bool2Int(Option.IsDead),
				",",
				TINHKIEM.Bool2Int(Game.IsHoldPK),
				",",
				TINHKIEM.Bool2Int(FrmMain.IsCalender),
				",",
				TINHKIEM.Bool2Int(true),
				",",
				TINHKIEM.Bool2Int(Option.NotDongMon),
				",",
				TINHKIEM.Bool2Int(Global.IsXuat),
				",",
				Game.TrongHoaX,
				",",
				TINHKIEM.Bool2Int(Option.PutBase),
				",",
				TINHKIEM.Bool2Int(true),
				",",
				TINHKIEM.Bool2Int(true),
				",",
				TINHKIEM.Bool2Int(Global.AutoSellItem),
				",",
				TINHKIEM.Bool2Int(true),
				",",
				TINHKIEM.Bool2Int(Global.IsVutRac),
				",",
				TINHKIEM.Bool2Int(FrmMain.AlarmAcBa),
				",",
				Option.HideTime,
				",",
				TINHKIEM.Bool2Int(Option.IsBank),
				",",
				TINHKIEM.Bool2Int(Option.AutoPoint),
				",",
				TINHKIEM.Bool2Int(Option.IsHoTro),
				",",
				TINHKIEM.Bool2Int(Global.IsHuyThaiHo),
				",",
				TINHKIEM.Bool2Int(Global.IsHyHuu),
				",",
				TINHKIEM.Bool2Int(Global.IsTuVaoPhai),
				",",
				1,
				",",
				Option.MapBanDoIndex,
				",",
				Option.MaptriLieuIndex,
				",",
				Global.NumNhiemVuDua
			});
			Setting.SaveSettingOffline("General.dat", value);
		}

		// Token: 0x06000D8E RID: 3470 RVA: 0x0005BD1C File Offset: 0x00059F1C
		private void LoadSetting()
		{
			this.SetHotKey();
			int[] array = Setting.LoadSettingOffline("General.dat");
			Game.TrongHoaX = 166;
			try
			{
				if (array != null && array.Length > 26)
				{
					Global.FollowKey = (array[0] == 1);
					Global.PickItem = (array[1] == 1);
					Global.ItemFillter = (array[2] == 1);
					Global.NoiRadius = array[3];
					Global.NgoaiRadius = array[4];
					this.numrangerpickitem.Value = (Global.PickRadius = array[5]);
					Global.ExitHPPercent = array[6];
					this.checkBox7.Checked = (Global.AlarmHP = (array[7] == 1));
					this.numcanhbaohp.Value = (Global.AlarmHPPercent = array[8]);
					this.checkauouplevel.Checked = (Global.AutoUpLvl = (array[9] == 1));
					this.numuplevel.Value = (Global.AutoUpLvlBelow = array[10]);
					Global.AutoDropItem = (array[11] == 1);
					Global.Mute = (array[12] == 1);
					Global.AutoShutDown = (array[13] == 1);
					this.checkBox10.Checked = (Global.UseSkillPet = (array[14] == 1));
					this.CheckTriLieuComeback.Checked = (Global.AutoComeBack = (array[15] == 1));
					Global.AlarmPk = (array[16] == 1);
					Global.ExitPk = (array[17] == 1);
					this.nudHP.Value = (Global.BuffHPPercent = array[18]);
					this.nudMP.Value = (Global.BuffMPPercent = array[19]);
					this.nudNM.Value = (Global.BuffNMPercent = array[20]);
					this.checkBox9.Checked = (Global.BuffPet = (array[21] == 1));
					Global.IsBoQua = (array[22] == 1);
					Global.AutoResetTime = (array[23] == 1);
					Global.BuffQuanDoan = (array[24] == 1);
					this.checkdongytodoi.Checked = (Global.AutoAccept = (array[25] == 1));
					this.checkdongytoanbo.Checked = (Global.AcceptAll = (array[26] == 1));
					if (array.Length > 27)
					{
						Global.IsXaPhu = (array[27] == 1);
					}
					if (array.Length > 28)
					{
						Global.BaseSkill = TINHKIEM.Int2Key(array[28]);
					}
					if (array.Length > 29)
					{
						Global.NMSkill = TINHKIEM.Int2Key(array[29]);
					}
					if (array.Length > 30)
					{
						Global.HPKey = TINHKIEM.Int2Key(array[30]);
					}
					if (array.Length > 31)
					{
						Global.IsHuyDanhQuai = (array[31] == 1);
					}
					if (array.Length > 32)
					{
						Global.AntiCaptchaSelf = (array[32] == 1);
					}
					if (array.Length > 33)
					{
						this.numbankinhtheosau.Value = (Global.FollowRadius = array[33]);
					}
					if (array.Length > 34)
					{
						Global.MaxBHD = array[34];
					}
					if (array.Length > 35)
					{
						Global.AutoPk = (array[35] == 1);
					}
					if (array.Length > 38)
					{
						Global.HideBHD = (array[38] == 1);
					}
					if (array.Length > 39)
					{
						Option.IsDead = (array[39] == 1);
					}
					if (array.Length > 40)
					{
						Game.IsHoldPK = (array[40] == 1);
					}
					if (array.Length > 41)
					{
						FrmMain.IsCalender = (array[41] == 1);
					}
					int num = array.Length;
					if (array.Length > 43)
					{
						Option.NotDongMon = (array[43] == 1);
					}
					if (array.Length > 44)
					{
						Global.IsXuat = (array[44] == 1);
					}
					if (array.Length > 45)
					{
						Game.TrongHoaX = array[45];
					}
					if (array.Length > 46)
					{
						Option.PutBase = (array[46] == 1);
					}
					int num2 = array.Length;
					int num3 = array.Length;
					if (array.Length > 49)
					{
						Global.AutoSellItem = (array[49] == 1);
					}
					int num4 = array.Length;
					if (array.Length > 51)
					{
						Global.IsVutRac = (array[51] == 1);
					}
					if (array.Length > 53)
					{
						Option.HideTime = array[53];
					}
					if (array.Length > 54)
					{
						Option.IsBank = (array[54] == 1);
					}
					if (array.Length > 59)
					{
						Global.IsTuVaoPhai = (array[59] == 1);
						Global.GlIsSetMenPai = (array[59] == 1);
					}
					if (array.Length > 60)
					{
						Global.GlSetMenPai = (TINHKIEM.Menpai)array[60];
					}
					if (array.Length > 61)
					{
						Option.MapBanDoIndex = array[61];
						Option.MaptriLieuIndex = array[62];
					}
				}
			}
			catch
			{
			}
		}

		// Token: 0x06000D8F RID: 3471 RVA: 0x0005C144 File Offset: 0x0005A344
		public void Auto()
		{
			for (;;)
			{
				if (Global.IsFull != 0)
				{
					Game.TickCount += 3;
					if (Game.TickCount % 3 != 0)
					{
						Game.TickCount = 0;
					}
					int num = 150 - (int)(this.timeAuto.Elapsed.TotalSeconds * 1000.0);
					if (num > 0)
					{
						Thread.Sleep(num);
					}
					this.timeAuto = Stopwatch.StartNew();
					Thread.Sleep(130);
					if (Game.TickCount % 36 == 0)
					{
						Game.ListDangThuHoach.Clear();
					}
					if (Game.TickCount % 200 == 0)
					{
						Game.lootPacketIdDua.Clear();
					}
					if (Game.TickCount % 18 == 0)
					{
						Game.LureId.Clear();
						Game.HashToaDo.Clear();
						Game.HashDangBon.Clear();
					}
					try
					{
						foreach (KeyValuePair<int, Game> keyValuePair in FrmMain.dicGame)
						{
							try
							{
								Game value = keyValuePair.Value;
								value.Auto();
								if (Game.TickCount % 9 == 0)
								{
									value.BuffPet();
								}
								if (Game.TickCount % 18 == 0)
								{
									value.Objects.Pk.Clear();
								}
							}
							catch
							{
							}
						}
						continue;
					}
					catch
					{
						continue;
					}
				}
				Thread.Sleep(1000);
			}
		}

		// Token: 0x06000D90 RID: 3472 RVA: 0x0005C2C0 File Offset: 0x0005A4C0
		private void Thoat(object sender, EventArgs e)
		{
			Application.Exit();
		}

		// Token: 0x06000D91 RID: 3473 RVA: 0x0005C2C7 File Offset: 0x0005A4C7
		private void HienAuto(object sender, EventArgs e)
		{
			base.Show();
		}

		// Token: 0x06000D92 RID: 3474 RVA: 0x0005C2CF File Offset: 0x0005A4CF
		private void radioButton2_CheckedChanged(object sender, EventArgs e)
		{
			if (FrmMain.CurGame != null)
			{
				this.notifyIcon1.ShowBalloonTip(2000, "Thông Báo", "Chuyển sang chế độ gom quái", ToolTipIcon.Info);
				FrmMain.CurGame.IsLure = true;
			}
		}

		// Token: 0x06000D93 RID: 3475 RVA: 0x00006740 File Offset: 0x00004940
		private void groupBox2_Enter(object sender, EventArgs e)
		{
		}

		// Token: 0x06000D94 RID: 3476 RVA: 0x00006740 File Offset: 0x00004940
		private void tabdanhquai_Click(object sender, EventArgs e)
		{
		}

		// Token: 0x06000D95 RID: 3477 RVA: 0x00006740 File Offset: 0x00004940
		private void menuStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
		{
		}

		// Token: 0x06000D96 RID: 3478 RVA: 0x00006740 File Offset: 0x00004940
		private void tabautologin_Click(object sender, EventArgs e)
		{
		}

		// Token: 0x17000324 RID: 804
		// (get) Token: 0x06000D97 RID: 3479 RVA: 0x0005C2FE File Offset: 0x0005A4FE
		// (set) Token: 0x06000D98 RID: 3480 RVA: 0x0005C306 File Offset: 0x0005A506
		private bool Follow { get; set; }

		// Token: 0x06000D99 RID: 3481 RVA: 0x0005C310 File Offset: 0x0005A510
		private void SetInfo()
		{
			foreach (object obj in this.ListViewNhanVat.Items)
			{
				ListViewItem listViewItem = (ListViewItem)obj;
				Game game = listViewItem.Tag as Game;
				game.TheoDoiCanhBao();
				listViewItem.Checked = game.IsAuto;
				listViewItem.SubItems[0].Text = string.Concat(new object[]
				{
					game.TLBB.Name,
					" [",
					game.TLBB.MenpaiName,
					" | ",
					game.TLBB.Lvl,
					"]"
				});
				if (game.ON_SCENE_TRANSING)
				{
					listViewItem.SubItems[0].Text = "Chuyển Cảnh";
					listViewItem.SubItems[0].BackColor = Color.Silver;
				}
				else if (game.TLBB.Online)
				{
					listViewItem.SubItems[0].BackColor = SystemColors.Window;
					if (game.TLBB.IsPk)
					{
						listViewItem.SubItems[0].BackColor = Color.Red;
					}
					else
					{
						listViewItem.SubItems[0].BackColor = SystemColors.Window;
					}
				}
				else
				{
					listViewItem.SubItems[0].BackColor = Color.Silver;
				}
				if (game.TLBB.IsLeader)
				{
					listViewItem.ForeColor = Color.Green;
				}
				else
				{
					listViewItem.ForeColor = SystemColors.WindowText;
				}
			}
			if (FrmMain.CurGame == null || !FrmMain.dicGame.ContainsValue(FrmMain.CurGame))
			{
				if (this.ListViewNhanVat.Items.Count > 0)
				{
					FrmMain.CurGame = (Game)this.ListViewNhanVat.Items[0].Tag;
					FrmMain.CurItem = this.ListViewNhanVat.Items[0];
					this.DownSetting();
					this.LoadSkill();
					this.comlenbai_SelectedIndexChanged(null, null);
					return;
				}
			}
			else
			{
				Random random = new Random();
				this.txthp.Text = FrmMain.CurGame.TLBB.HPPercent.ToString() + "%";
				this.txthp.BackColor = this.GetCodeByPercen(FrmMain.CurGame.TLBB.HPPercent);
				this.txtmp.Text = FrmMain.CurGame.TLBB.MPPercent.ToString() + "%";
				this.txtmp.BackColor = this.GetCodeByPercen(FrmMain.CurGame.TLBB.MPPercent);
				this.AutoXuatPhet.Checked = Global.IsXuat;
				this.txtpet.Text = FrmMain.CurGame.TLBB.PetHPPercent.ToString() + "%";
				this.txtpet.BackColor = this.GetCodeByPercen(FrmMain.CurGame.TLBB.PetHPPercent);
				this.txttrangthai.Text = string.Concat(new string[]
				{
					FrmMain.CurGame.TLBB.MapName,
					" (",
					((int)FrmMain.CurGame.CharX).ToString(),
					":",
					((int)FrmMain.CurGame.CharY).ToString(),
					") | ",
					FrmMain.CurGame.Status
				});
				if (FrmMain.CurGame.LenBaiTrain)
				{
					this.butlenbai.Text = "Đang lên bãi..";
					this.butlenbai.ForeColor = Color.FromArgb(random.Next(256), random.Next(256), random.Next(256));
				}
				else
				{
					this.butlenbai.Text = "Lên Bãi";
					this.butlenbai.ForeColor = Color.Black;
				}
				if (FrmMain.CurGame.AutoTrain)
				{
					this.buttimbai.Text = "Đang Auto Train..";
					this.buttimbai.ForeColor = Color.FromArgb(random.Next(256), random.Next(256), random.Next(256));
				}
				else
				{
					this.buttimbai.Text = "Tìm Bãi";
					this.buttimbai.ForeColor = Color.Black;
				}
				if (FrmMain.CurGame.IsTriLieu)
				{
					this.buttrilieu.ForeColor = Color.FromArgb(random.Next(256), random.Next(256), random.Next(256));
					this.buttrilieu.Text = "Đang Trị Liệu..";
				}
				else
				{
					this.buttrilieu.Text = "Trị Liệu";
					this.buttrilieu.ForeColor = Color.Black;
				}
				this.chekcdanhquai.Checked = FrmMain.CurGame.IsAttack;
				this.pickbanvatpham.Checked = FrmMain.CurGame.IsSellItem;
				this.checkpickitem.Checked = FrmMain.CurGame.IsPickItem;
				if (this.checkradius.Checked)
				{
					this.checkradius.Text = string.Concat(new object[]
					{
						"Quanh [",
						(int)FrmMain.CurGame.RadiusX,
						",",
						(int)FrmMain.CurGame.RadiusY,
						"]"
					});
				}
			}
		}

		// Token: 0x06000D9A RID: 3482 RVA: 0x0005C8B4 File Offset: 0x0005AAB4
		public int GetTime(DateTime dt)
		{
			return (int)(dt - new DateTime(1970, 1, 1)).TotalSeconds;
		}

		// Token: 0x06000D9B RID: 3483 RVA: 0x0005C8DC File Offset: 0x0005AADC
		public void Monitor()
		{
			for (;;)
			{
				if (Global.IsFull != 0)
				{
					try
					{
						HashSet<Process> hashSet = new HashSet<Process>();
						string[] wndClassNames = Win.WndClassNames;
						for (int i = 0; i < wndClassNames.Length; i++)
						{
							foreach (Process process in Win.GetProcessByClassName(wndClassNames[i]))
							{
								try
								{
									if (Win.GameExeProcessNames.Contains(process.MainModule.ModuleName))
									{
										hashSet.Add(process);
									}
								}
								catch
								{
								}
							}
						}
						FrmMain.GameCount = hashSet.Count;
						foreach (KeyValuePair<int, Game> keyValuePair in FrmMain.dicGame)
						{
							if (keyValuePair.Value.Process.HasExited)
							{
								if (FrmMain.CurGame == keyValuePair.Value)
								{
									FrmMain.CurGame = null;
								}
								base.Invoke(new FrmMain.CallBack(this.DeleteItem), new object[]
								{
									keyValuePair.Value
								});
							}
						}
						using (HashSet<Process>.Enumerator enumerator = hashSet.GetEnumerator())
						{
							while (enumerator.MoveNext())
							{
								Process current2 = enumerator.Current;
								if (!FrmMain.dicGame.ContainsKey(current2.Id) && ((current2.Responding && current2.MainWindowTitle.ToLower().Contains("3.74.9000")) || (current2.Responding && current2.MainWindowTitle.Contains("Thien Long Bat Bo") && Win.GetHandle(current2.Id, "#32770") == IntPtr.Zero) || (current2.Responding && current2.MainWindowTitle.ToLower().Contains("????")) || (current2.Responding && this.GetTime(DateTime.Now) - this.GetTime(current2.StartTime) > 60)))
								{
									bool flag = false;
									if (this.FakeGame != null && this.FakeGame.ContainsKey(current2.Id))
									{
										foreach (KeyValuePair<int, Stopwatch> keyValuePair2 in this.FakeGame)
										{
											if (keyValuePair2.Key == current2.Id && keyValuePair2.Value.Elapsed.TotalSeconds > 240.0)
											{
												flag = true;
											}
										}
									}
									if (!flag)
									{
										try
										{
											string md = Offset.MD51;
											if ((Offset.MD51 == md || Offset.MD52 == md) && (!(Win.GetHandle(current2.Id, Win.WndClassNames) == IntPtr.Zero) || !(Win.GetHandle(current2.Id, "#32770") == IntPtr.Zero)))
											{
												Address instance = Address.GetInstance(md, Offset.OffList);
												Game game = null;
												if (current2.Responding)
												{
													base.Invoke(new Action(delegate()
													{
														game = new Game(current2, instance);
													}));
												}
												if (Offset.OffList.ToLower().StartsWith("ffff" + md.ToLower()))
												{
													game.Is2d = true;
												}
												FrmMain.dicGame.Add(current2.Id, game);
												game.SettingLoaded += this.game_SettingLoaded;
												game.SkillLoaded += this.game_SkillLoaded;
												base.Invoke(new FrmMain.CallBack(this.AddItem), new object[]
												{
													game
												});
											}
										}
										catch (Exception)
										{
											if (this.FakeGame == null)
											{
												this.FakeGame = new Dictionary<int, Stopwatch>();
											}
											if (!this.FakeGame.ContainsKey(current2.Id))
											{
												this.FakeGame.Add(current2.Id, Stopwatch.StartNew());
											}
										}
									}
								}
								if (Win.GetHandle(current2.Id, "#32770") != IntPtr.Zero)
								{
									string md2 = Offset.MD51;
									this.Memory = new Memory(current2.Id);
									Address instance2 = Address.GetInstance(md2, Offset.OffList);
									this.Memory.Write(instance2.MultiAcc, 2425393296U, 4);
									this.Memory.Write(instance2.MultiAcc + 4, 2425393296U, 4);
									Thread.Sleep(500);
									Win.PostMessage(Win.GetHandle(current2.Id, "#32770"), 16, 0, 0);
								}
							}
						}
						Thread.Sleep(2000);
						continue;
					}
					catch
					{
						continue;
					}
				}
				Thread.Sleep(1000);
			}
		}

		// Token: 0x17000325 RID: 805
		// (get) Token: 0x06000D9C RID: 3484 RVA: 0x0005CEC0 File Offset: 0x0005B0C0
		// (set) Token: 0x06000D9D RID: 3485 RVA: 0x0005CEC8 File Offset: 0x0005B0C8
		private Dictionary<int, Stopwatch> FakeGame { get; set; }

		// Token: 0x06000D9E RID: 3486 RVA: 0x0005CED1 File Offset: 0x0005B0D1
		private void game_SkillLoaded(object sender, EventArgs e)
		{
			base.Invoke(new FrmMain.CallBack(this.SkillLoaded), new object[]
			{
				sender as Game
			});
		}

		// Token: 0x06000D9F RID: 3487 RVA: 0x0005CEF8 File Offset: 0x0005B0F8
		public void DownSetting()
		{
			if (FrmMain.CurGame == null)
			{
				return;
			}
			this.txtmkkho.Text = FrmMain.CurGame.Pass2;
			this.TablControl.TabPages[0].Text = FrmMain.CurGame.TLBB.Name;
			this.chekcdanhquai.Checked = FrmMain.CurGame.IsAttack;
			if (FrmMain.CurGame.IsLure)
			{
				this.radgom.Checked = true;
				this.rad11.Checked = false;
			}
			else
			{
				this.radgom.Checked = false;
				this.rad11.Checked = true;
			}
			this.txtthoigian.Text = (((int)FrmMain.CurGame.TimeGiaoChat).ToString() ?? "");
			this.checktholinhchau.Checked = FrmMain.CurGame.UsingTholinhChau;
			if (FrmMain.CurGame.LenBaiTrain)
			{
				this.butlenbai.Text = "Đang lên bãi..";
			}
			else
			{
				this.butlenbai.Text = "Lên Bãi";
			}
			if (FrmMain.CurGame.AutoTrain)
			{
				this.buttimbai.Text = "Đang Auto Train..";
			}
			else
			{
				this.buttimbai.Text = "Tìm Bãi";
			}
			if (FrmMain.CurGame.IsTriLieu)
			{
				this.buttrilieu.Text = "Đang Trị Liệu..";
			}
			else
			{
				this.buttrilieu.Text = "Trị Liệu";
			}
			this.comdanhsachbando.SelectedIndex = FrmMain.CurGame.LenBanDoIndex;
			this.comlenbai.SelectedIndex = FrmMain.CurGame.LenBaiIndex;
			this.checkradius.Checked = FrmMain.CurGame.IsRadius;
			if (FrmMain.CurGame.TLBB.IsNoi)
			{
				this.numberdanhquanh.Value = Global.NoiRadius;
			}
			else
			{
				this.numberdanhquanh.Value = Global.NgoaiRadius;
			}
			this.numrangerpickitem.Value = Global.PickRadius;
			this.txttoadox.Text = (FrmMain.CurGame._baitrain.PosX.ToString() ?? "");
			this.txttoadoy.Text = (FrmMain.CurGame._baitrain.PosY.ToString() ?? "");
			this.CheckthuPet.Checked = FrmMain.CurGame.AutoThuPet;
			this.CheckReGenPET.Checked = FrmMain.CurGame.IsPet;
			this.checkregenhp.Checked = FrmMain.CurGame.IsHP;
			this.checkrengenmp.Checked = FrmMain.CurGame.IsMP;
			this.checkisNM.Checked = FrmMain.CurGame.IsNM;
			this.checkcongsinh.Checked = FrmMain.CurGame.CongSinh;
			this.checkhuyette.Checked = FrmMain.CurGame.HuyetTe;
			this.CheckAutoHoiSinh.Checked = FrmMain.CurGame.AutoHoiSinh;
			this.Checkthongbaochatmat.Checked = FrmMain.CurGame.AlarmChat;
			this.checkhuyitem.Checked = FrmMain.CurGame.IsDropItem;
			this.pickbanvatpham.Checked = FrmMain.CurGame.IsSellItem;
			this.checkgiaochat.Checked = FrmMain.CurGame.IsRao;
			this.checkautocatkho.Checked = FrmMain.CurGame.IsBank;
			this.chekcautox2.Checked = FrmMain.CurGame.TuAnX2;
			this.chekcusingitem.Checked = FrmMain.CurGame.AutoEatVatPham;
			this.txtnoidunggiaochat.Text = FrmMain.CurGame.RaoTxt;
			this.chekcautox2.Checked = FrmMain.CurGame.TuAnX2;
			this.checkpickitem.Checked = FrmMain.CurGame.IsPickItem;
			this.cboXuatPet.Items.Clear();
			FrmMain.ComboboxItem comboboxItem = new FrmMain.ComboboxItem();
			comboboxItem.Text = "Không Xuất";
			comboboxItem.Value = 0;
			this.cboXuatPet.Items.Add(comboboxItem);
			int num = -1;
			int num2 = 0;
			foreach (KeyValuePair<int, string> keyValuePair in FrmMain.CurGame.TLBB.DicPet)
			{
				num2++;
				FrmMain.ComboboxItem comboboxItem2 = new FrmMain.ComboboxItem();
				comboboxItem2.Text = keyValuePair.Value;
				comboboxItem2.Value = keyValuePair.Key;
				if (keyValuePair.Key.ToString("X8") == FrmMain.CurGame.PetId)
				{
					num = num2;
				}
				this.cboXuatPet.Items.Add(comboboxItem2);
			}
			if (num != -1)
			{
				this.cboXuatPet.SelectedIndex = num;
			}
			if (this.IsCheDoTab)
			{
				if (FrmMain.CurGame.IsCheDo)
				{
					this.butche.Text = "Tắt";
				}
				else
				{
					this.butche.Text = "Chế";
				}
				this.comboloai.SelectedIndex = FrmMain.CurGame.CheLoai;
				this.comcapdtd.SelectedIndex = FrmMain.CurGame.CheCap;
				VatLieu vatLieu = new VatLieu();
				vatLieu = FrmMain.CurGame.getsoluong(this.comboloai.SelectedItem.ToString(), this.comcapdtd.SelectedIndex + 1);
				this.txtbingan.Text = (vatLieu.BiNgan.ToString() ?? "");
				this.txttinhthiet.Text = (vatLieu.TinhThiet.ToString() ?? "");
				this.txtvaibong.Text = (vatLieu.VaiBong.ToString() ?? "");
				this.txttaodo.Text = (vatLieu.DaTaoDo.ToString() ?? "");
				this.txtdache.Text = (vatLieu.DaChe.ToString() ?? "");
				this.txtdahuy.Text = (vatLieu.DaHuy.ToString() ?? "");
				int daChe = vatLieu.DaChe;
				this.numsonguyenlieu.Value = FrmMain.CurGame.SoLuongMua;
				this.comboloai.SelectedIndex = FrmMain.CurGame.CheLoai;
				this.comcapdtd.SelectedIndex = FrmMain.CurGame.CheCap;
				this.numericUpDown4.Value = FrmMain.CurGame.SoLuongChe;
				this.checkBox12.Checked = !FrmMain.CurGame.IsMuaNguyenLieu;
				this.checkBox13.Checked = FrmMain.CurGame.HuyNguyenLieu;
				this.numericUpDown6.Value = FrmMain.CurGame.CheDiem;
				this.numericUpDown5.Value = FrmMain.CurGame.CheDong;
				this.numsosao.Value = FrmMain.CurGame.CheSao;
			}
		}

		// Token: 0x06000DA0 RID: 3488 RVA: 0x0005D60C File Offset: 0x0005B80C
		private void SettingLoaded(Game game)
		{
			if (FrmMain.CurGame == game)
			{
				this.DownSetting();
			}
			foreach (object obj in this.ListViewNhanVat.Items)
			{
				ListViewItem listViewItem = (ListViewItem)obj;
				if (listViewItem.Tag as Game == game)
				{
					listViewItem.Checked = game.IsAuto;
					break;
				}
			}
		}

		// Token: 0x06000DA1 RID: 3489 RVA: 0x0005D690 File Offset: 0x0005B890
		private void SkillLoaded(Game game)
		{
			if (FrmMain.CurGame == game)
			{
				this.LoadSkill();
			}
		}

		// Token: 0x06000DA2 RID: 3490 RVA: 0x0005D6A0 File Offset: 0x0005B8A0
		private void LoadSkill()
		{
			this.listviewskillhotro.Items.Clear();
			this.listViewSkill.Items.Clear();
			this.comdanhsachdanhquai.Items.Clear();
			this.comskillhotro.Items.Clear();
			foreach (Skill skill in FrmMain.CurGame.Skills)
			{
				if (skill.Name.Length > 0)
				{
					FrmMain.ComboboxItem comboboxItem = new FrmMain.ComboboxItem();
					comboboxItem.Text = skill.Name;
					comboboxItem.Value = skill;
					if (!Skill.IsBand(skill.PacketId) && !Skill.IsBuffSkill(skill.PacketId))
					{
						this.comdanhsachdanhquai.Items.Add(comboboxItem);
					}
					else if (!Skill.IsBand(skill.PacketId) && Skill.IsBuffSkill(skill.PacketId))
					{
						this.comskillhotro.Items.Add(comboboxItem);
					}
					if (skill.Use && this.listViewSkill.FindItemWithText(skill.Name) == null)
					{
						this.listViewSkill.Items.Add(skill.Name);
					}
					if (skill.UserBuff && this.listviewskillhotro.FindItemWithText(skill.Name) == null)
					{
						this.listviewskillhotro.Items.Add(skill.Name);
					}
				}
				if (this.comdanhsachdanhquai.Items.Count > 0)
				{
					this.comdanhsachdanhquai.SelectedIndex = 0;
				}
				if (this.comskillhotro.Items.Count > 0)
				{
					this.comskillhotro.SelectedIndex = 0;
				}
			}
		}

		// Token: 0x06000DA3 RID: 3491 RVA: 0x0005D868 File Offset: 0x0005BA68
		private void listViewSkill_ItemChecked(object sender, ItemCheckedEventArgs e)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06000DA4 RID: 3492 RVA: 0x0005D86F File Offset: 0x0005BA6F
		private void game_SettingLoaded(object sender, EventArgs e)
		{
			base.Invoke(new FrmMain.CallBack(this.SettingLoaded), new object[]
			{
				sender as Game
			});
		}

		// Token: 0x06000DA5 RID: 3493 RVA: 0x0005D894 File Offset: 0x0005BA94
		public void AddItem(Game game)
		{
			ListViewItem listViewItem = new ListViewItem(new string[]
			{
				"ĐăngNhập"
			})
			{
				UseItemStyleForSubItems = false
			};
			listViewItem.Tag = game;
			game.Item = listViewItem;
			listViewItem.Checked = game.IsAuto;
			listViewItem.SubItems[0].Text = string.Concat(new object[]
			{
				game.TLBB.Name,
				" [",
				game.TLBB.MenpaiName,
				" | ",
				game.TLBB.Lvl,
				"]"
			});
			this.ListViewNhanVat.Items.Add(listViewItem);
			this.ListViewNhanVat.Columns[0].Text = "Tổng Nhân Vật [" + this.ListViewNhanVat.Items.Count.ToString() + "]";
			FrmMain.GameCount = this.ListViewNhanVat.Items.Count;
			if (FrmMain.GameCount > FrmMain.MaxGame)
			{
				FrmMain.MaxGame = FrmMain.GameCount;
			}
		}

		// Token: 0x06000DA6 RID: 3494 RVA: 0x0005D9B4 File Offset: 0x0005BBB4
		public void DeleteItem(Game deleteGame)
		{
			foreach (object obj in this.ListViewNhanVat.Items)
			{
				ListViewItem listViewItem = (ListViewItem)obj;
				Game game = (Game)listViewItem.Tag;
				if (game == deleteGame)
				{
					game.Exit();
					listViewItem.Remove();
				}
				if (this.ListViewNhanVat.Items.Count == 0 && Global.AutoShutDown)
				{
					new ShutDown();
				}
			}
			this.ListViewNhanVat.Columns[0].Text = "Tổng Nhân Vật [" + this.ListViewNhanVat.Items.Count.ToString() + "]";
		}

		// Token: 0x06000DA7 RID: 3495 RVA: 0x0005DA84 File Offset: 0x0005BC84
		private void timeMonitor_Tick(object sender, EventArgs e)
		{
			if (this.Follow)
			{
				foreach (KeyValuePair<int, Game> keyValuePair in FrmMain.dicGame)
				{
					Game value = keyValuePair.Value;
					if (value.TLBB.IsLeader)
					{
						value.TrieuTap();
					}
				}
			}
			if (this.IsLoginTab)
			{
				foreach (object obj in this.ListViewLogin.Items)
				{
					ListViewItem listViewItem = (ListViewItem)obj;
					Account account = listViewItem.Tag as Account;
					Game game = account.game;
					if (game != null)
					{
						listViewItem.SubItems[3].Text = game.TLBB.Name + " | Thứ " + account.LoginIndex;
						listViewItem.SubItems[4].Text = game.TLBB.MenpaiName;
					}
					if (account.Status == "Online")
					{
						listViewItem.SubItems[2].Text = account.Status;
					}
					else
					{
						listViewItem.SubItems[2].Text = account.Status;
						listViewItem.SubItems[3].Text = account.Name + "| Thứ " + account.LoginIndex;
					}
				}
			}
			foreach (KeyValuePair<int, Game> keyValuePair2 in FrmMain.dicGame)
			{
				Game value2 = keyValuePair2.Value;
				if (value2 != null)
				{
					List<ThongBao> listThongBao = value2.ListThongBao;
					if (listThongBao.Count > 0)
					{
						int num = 0;
						foreach (ThongBao thongBao in listThongBao.ToArray())
						{
							CanhBao.AddLog(thongBao.tideu, thongBao.noidung, thongBao.Type);
							try
							{
								listThongBao.RemoveAt(num);
								FrmMain.CurGame.ListThongBao.RemoveAt(num);
								num++;
							}
							catch
							{
							}
						}
					}
				}
			}
			try
			{
				this.SetInfo();
			}
			catch
			{
			}
		}

		// Token: 0x06000DA8 RID: 3496 RVA: 0x0005DD14 File Offset: 0x0005BF14
		private void CheckTriLieuComeback_CheckedChanged(object sender, EventArgs e)
		{
			if (FrmMain.CurGame != null)
			{
				if (!this.VuaBatXong())
				{
					this.notifyIcon1.ShowBalloonTip(2000, "Thông Báo", string.Concat(new string[]
					{
						"[",
						FrmMain.CurGame.TLBB.Name.ToUpper(),
						"]",
						this.CheckTriLieuComeback.Checked ? " Bật " : " Tắt ",
						"trị liệu và quay lại bãi"
					}), ToolTipIcon.Info);
				}
				Global.AutoComeBack = false;
				FrmMain.CurGame.DeadX = 0;
			}
		}

		// Token: 0x06000DA9 RID: 3497 RVA: 0x0005DDB0 File Offset: 0x0005BFB0
		private void chekcdanhquai_CheckedChanged(object sender, EventArgs e)
		{
			if (FrmMain.CurGame != null)
			{
				if (!this.VuaBatXong())
				{
					this.notifyIcon1.ShowBalloonTip(2000, "Thông Báo", string.Concat(new string[]
					{
						"[",
						FrmMain.CurGame.TLBB.Name.ToUpper(),
						"]",
						this.chekcdanhquai.Checked ? " Bật " : " Tắt ",
						"đánh quái"
					}), ToolTipIcon.Info);
				}
				FrmMain.CurGame.IsAttack = this.chekcdanhquai.Checked;
			}
		}

		// Token: 0x06000DAA RID: 3498 RVA: 0x0005DE50 File Offset: 0x0005C050
		private void rad11_CheckedChanged(object sender, EventArgs e)
		{
			if (FrmMain.CurGame != null)
			{
				if (!this.VuaBatXong())
				{
					this.notifyIcon1.ShowBalloonTip(2000, "Thông Báo", "Chuyển sang chế độ đánh từng con", ToolTipIcon.Info);
				}
				FrmMain.CurGame.IsLure = false;
			}
		}

		// Token: 0x06000DAB RID: 3499 RVA: 0x0005DE88 File Offset: 0x0005C088
		private void checkradius_CheckedChanged(object sender, EventArgs e)
		{
			if (FrmMain.CurGame != null)
			{
				if (!this.VuaBatXong())
				{
					this.notifyIcon1.ShowBalloonTip(2000, "Thông Báo", string.Concat(new string[]
					{
						"[",
						FrmMain.CurGame.TLBB.Name.ToUpper(),
						"]",
						this.checkradius.Checked ? " Bật " : " Tắt ",
						"đánh quanh điểm"
					}), ToolTipIcon.Info);
				}
				FrmMain.CurGame.IsRadius = this.checkradius.Checked;
			}
		}

		// Token: 0x06000DAC RID: 3500 RVA: 0x0005DF28 File Offset: 0x0005C128
		private void numberdanhquanh_ValueChanged(object sender, EventArgs e)
		{
			if (FrmMain.CurGame != null)
			{
				if (FrmMain.CurGame.TLBB.IsNoi)
				{
					Global.NoiRadius = (int)this.numberdanhquanh.Value;
					return;
				}
				Global.NgoaiRadius = (int)this.numberdanhquanh.Value;
			}
		}

		// Token: 0x06000DAD RID: 3501 RVA: 0x0005DF78 File Offset: 0x0005C178
		private void butboqua_Click(object sender, EventArgs e)
		{
			new BoQua().Show();
		}

		// Token: 0x06000DAE RID: 3502 RVA: 0x0005DF84 File Offset: 0x0005C184
		private void checktholinhchau_CheckedChanged(object sender, EventArgs e)
		{
			if (FrmMain.CurGame != null)
			{
				if (!this.VuaBatXong())
				{
					this.notifyIcon1.ShowBalloonTip(2000, "Thông Báo", string.Concat(new string[]
					{
						"[",
						FrmMain.CurGame.TLBB.Name.ToUpper(),
						"]",
						this.checktholinhchau.Checked ? " Bật " : " Tắt ",
						"sử dụng Thổ Linh Châu"
					}), ToolTipIcon.Info);
				}
				FrmMain.CurGame.UsingTholinhChau = this.checktholinhchau.Checked;
			}
		}

		// Token: 0x06000DAF RID: 3503 RVA: 0x0005E024 File Offset: 0x0005C224
		private void nudHP_ValueChanged(object sender, EventArgs e)
		{
			Global.BuffHPPercent = (int)this.nudHP.Value;
		}

		// Token: 0x06000DB0 RID: 3504 RVA: 0x0005E03B File Offset: 0x0005C23B
		private void nudMP_ValueChanged(object sender, EventArgs e)
		{
			Global.BuffMPPercent = (int)this.nudMP.Value;
		}

		// Token: 0x06000DB1 RID: 3505 RVA: 0x0005E052 File Offset: 0x0005C252
		private void numbercongsinhhp_ValueChanged(object sender, EventArgs e)
		{
			if (FrmMain.CurGame != null)
			{
				FrmMain.CurGame.CongSinhValue = (int)this.numbercongsinhhp.Value;
			}
		}

		// Token: 0x06000DB2 RID: 3506 RVA: 0x0005E075 File Offset: 0x0005C275
		private void numhuyettemp_ValueChanged(object sender, EventArgs e)
		{
			if (FrmMain.CurGame != null)
			{
				FrmMain.CurGame.HuyetTeValue = (int)this.numhuyettemp.Value;
			}
		}

		// Token: 0x06000DB3 RID: 3507 RVA: 0x0005E098 File Offset: 0x0005C298
		private void nudNM_ValueChanged(object sender, EventArgs e)
		{
			Global.BuffNMPercent = (int)this.nudNM.Value;
		}

		// Token: 0x06000DB4 RID: 3508 RVA: 0x0005E0AF File Offset: 0x0005C2AF
		private void numericUpDown1_ValueChanged(object sender, EventArgs e)
		{
			Global.AlarmHPPercent = (int)this.numcanhbaohp.Value;
		}

		// Token: 0x06000DB5 RID: 3509 RVA: 0x0005E0C8 File Offset: 0x0005C2C8
		private void checkregenhp_CheckedChanged(object sender, EventArgs e)
		{
			if (FrmMain.CurGame != null)
			{
				if (!this.VuaBatXong())
				{
					this.notifyIcon1.ShowBalloonTip(2000, "Thông Báo", string.Concat(new string[]
					{
						"[",
						FrmMain.CurGame.TLBB.Name.ToUpper(),
						"]",
						this.checkregenhp.Checked ? " Bật " : " Tắt ",
						"tự sử dụng HP"
					}), ToolTipIcon.Info);
				}
				FrmMain.CurGame.IsHP = this.checkregenhp.Checked;
			}
		}

		// Token: 0x06000DB6 RID: 3510 RVA: 0x0005E168 File Offset: 0x0005C368
		private void checkrengenmp_CheckedChanged(object sender, EventArgs e)
		{
			if (FrmMain.CurGame != null)
			{
				if (!this.VuaBatXong())
				{
					this.notifyIcon1.ShowBalloonTip(2000, "Thông Báo", string.Concat(new string[]
					{
						"[",
						FrmMain.CurGame.TLBB.Name.ToUpper(),
						"]",
						this.checkrengenmp.Checked ? " Bật " : " Tắt ",
						"tự sử dụng MP"
					}), ToolTipIcon.Info);
				}
				FrmMain.CurGame.IsMP = this.checkregenhp.Checked;
			}
		}

		// Token: 0x06000DB7 RID: 3511 RVA: 0x0005E208 File Offset: 0x0005C408
		private void checkcongsinh_CheckedChanged(object sender, EventArgs e)
		{
			if (FrmMain.CurGame != null)
			{
				if (!this.VuaBatXong())
				{
					this.notifyIcon1.ShowBalloonTip(2000, "Thông Báo", string.Concat(new string[]
					{
						"[",
						FrmMain.CurGame.TLBB.Name.ToUpper(),
						"]",
						this.checkcongsinh.Checked ? " Bật " : " Tắt ",
						"cộng sinh PET"
					}), ToolTipIcon.Info);
				}
				FrmMain.CurGame.CongSinh = this.checkcongsinh.Checked;
			}
		}

		// Token: 0x06000DB8 RID: 3512 RVA: 0x00006740 File Offset: 0x00004940
		private void groupBox3_Enter(object sender, EventArgs e)
		{
		}

		// Token: 0x06000DB9 RID: 3513 RVA: 0x0005E2A8 File Offset: 0x0005C4A8
		private void checkhuyette_CheckedChanged(object sender, EventArgs e)
		{
			if (FrmMain.CurGame != null)
			{
				if (!this.VuaBatXong())
				{
					this.notifyIcon1.ShowBalloonTip(2000, "Thông Báo", string.Concat(new string[]
					{
						"[",
						FrmMain.CurGame.TLBB.Name.ToUpper(),
						"]",
						this.checkhuyette.Checked ? " Bật " : " Tắt ",
						"huyết tế PET"
					}), ToolTipIcon.Info);
				}
				FrmMain.CurGame.HuyetTe = this.checkhuyette.Checked;
			}
		}

		// Token: 0x06000DBA RID: 3514 RVA: 0x0005E348 File Offset: 0x0005C548
		private void checkisNM_CheckedChanged(object sender, EventArgs e)
		{
			if (FrmMain.CurGame != null)
			{
				FrmMain.CurGame.IsNM = this.checkisNM.Checked;
				if (!this.VuaBatXong())
				{
					this.notifyIcon1.ShowBalloonTip(2000, "Thông Báo", string.Concat(new string[]
					{
						"[",
						FrmMain.CurGame.TLBB.Name.ToUpper(),
						"]",
						this.checkhuyette.Checked ? " Bật " : " Tắt ",
						"buff Nga My"
					}), ToolTipIcon.Info);
				}
			}
		}

		// Token: 0x06000DBB RID: 3515 RVA: 0x0005E3E8 File Offset: 0x0005C5E8
		public int GETTIMEHIENTAI(DateTime hientai)
		{
			return (int)(hientai - new DateTime(1970, 1, 1)).TotalSeconds;
		}

		// Token: 0x06000DBC RID: 3516 RVA: 0x0005E410 File Offset: 0x0005C610
		private void CheckAutoHoiSinh_CheckedChanged(object sender, EventArgs e)
		{
			if (FrmMain.CurGame != null)
			{
				if (!this.VuaBatXong())
				{
					this.notifyIcon1.ShowBalloonTip(2000, "Thông Báo", string.Concat(new string[]
					{
						"[",
						FrmMain.CurGame.TLBB.Name.ToUpper(),
						"]",
						this.CheckAutoHoiSinh.Checked ? " Bật " : " Tắt ",
						"hồi sinh sau khi chết"
					}), ToolTipIcon.Info);
				}
				FrmMain.CurGame.AutoHoiSinh = this.CheckAutoHoiSinh.Checked;
			}
		}

		// Token: 0x06000DBD RID: 3517 RVA: 0x0005E4B0 File Offset: 0x0005C6B0
		private void checkBox7_CheckedChanged(object sender, EventArgs e)
		{
			if (FrmMain.CurGame != null)
			{
				if (!this.VuaBatXong())
				{
					this.notifyIcon1.ShowBalloonTip(2000, "Thông Báo", string.Concat(new string[]
					{
						"[",
						FrmMain.CurGame.TLBB.Name.ToUpper(),
						"]",
						this.checkBox7.Checked ? " Bật " : " Tắt ",
						"cảnh báo HP"
					}), ToolTipIcon.Info);
				}
				Global.AlarmHP = this.checkBox7.Checked;
			}
		}

		// Token: 0x06000DBE RID: 3518 RVA: 0x0005E54B File Offset: 0x0005C74B
		private void txttoadox_TextChanged(object sender, EventArgs e)
		{
			if (FrmMain.CurGame != null)
			{
				FrmMain.CurGame._baitrain.PosX = int.Parse(this.txttoadox.Text);
			}
		}

		// Token: 0x06000DBF RID: 3519 RVA: 0x0005E573 File Offset: 0x0005C773
		private void txttoadoy_TextChanged(object sender, EventArgs e)
		{
			if (FrmMain.CurGame != null)
			{
				FrmMain.CurGame._baitrain.PosY = int.Parse(this.txttoadoy.Text);
			}
		}

		// Token: 0x06000DC0 RID: 3520 RVA: 0x0005E59C File Offset: 0x0005C79C
		private void butlenbai_Click(object sender, EventArgs e)
		{
			BaiTrain baiTrain = (BaiTrain)(this.comlenbai.SelectedItem as FrmMain.ComboboxItem).Value;
			if (FrmMain.CurGame == null)
			{
				MessageBox.Show("Chức năng này cần phải có nhân vật đang auto");
				return;
			}
			if (!Unity.IsNumeric(this.txttoadox.Text) || !Unity.IsNumeric(this.txttoadoy.Text) || int.Parse(this.txttoadoy.Text) <= 0 || int.Parse(this.txttoadoy.Text) <= 0)
			{
				MessageBox.Show("Kiểm tra lại tọa độ");
				return;
			}
			List<BaiTrain> dsbai = TrainData.dsbai;
			BaiTrain baiTrain2 = new BaiTrain();
			foreach (BaiTrain baiTrain3 in dsbai)
			{
				if (baiTrain3.MapName == baiTrain.MapName)
				{
					baiTrain2.Level = 40;
					baiTrain2.MapID = baiTrain3.MapID;
					baiTrain2.Name = "Bãi tùy chỉnh";
					baiTrain2.PosX = int.Parse(this.txttoadox.Text);
					baiTrain2.PosY = int.Parse(this.txttoadoy.Text);
					break;
				}
			}
			FrmMain.CurGame._baitrain = baiTrain2;
			if (FrmMain.CurGame.LenBaiTrain)
			{
				this.butlenbai.Text = "Lên Bãi";
				FrmMain.CurGame.LenBaiTrain = false;
				return;
			}
			this.butlenbai.Text = "Đang lên..";
			FrmMain.CurGame.LenBaiTrain = true;
			this.notifyIcon1.ShowBalloonTip(2000, "Thông Báo", "[" + FrmMain.CurGame.TLBB.Name.ToUpper() + "] bắt đầu lên bãi", ToolTipIcon.Info);
		}

		// Token: 0x06000DC1 RID: 3521 RVA: 0x0005E768 File Offset: 0x0005C968
		private void comdanhsachbando_SelectedIndexChanged(object sender, EventArgs e)
		{
			this.comlenbai.Items.Clear();
			try
			{
				List<BaiTrain> dsbai = TrainData.dsbai;
				string b = (this.comdanhsachbando.SelectedItem as FrmMain.ComboboxItem).Value.ToString();
				foreach (BaiTrain baiTrain in dsbai)
				{
					if (baiTrain.MapName == b)
					{
						FrmMain.ComboboxItem comboboxItem = new FrmMain.ComboboxItem();
						comboboxItem.Text = baiTrain.Name;
						comboboxItem.Value = baiTrain;
						this.comlenbai.Items.Add(comboboxItem);
					}
				}
				if (FrmMain.CurGame != null && this.comdanhsachbando.SelectedIndex != -1)
				{
					FrmMain.CurGame.LenBanDoIndex = this.comdanhsachbando.SelectedIndex;
				}
				this.comlenbai.SelectedIndex = 0;
			}
			catch
			{
			}
		}

		// Token: 0x06000DC2 RID: 3522 RVA: 0x0005E860 File Offset: 0x0005CA60
		private void buttrilieu_Click(object sender, EventArgs e)
		{
			if (FrmMain.CurGame != null)
			{
				if (FrmMain.CurGame.IsTriLieu)
				{
					this.buttrilieu.Text = "Trị Liệu";
					FrmMain.CurGame.IsTriLieu = false;
					return;
				}
				this.buttrilieu.Text = "Đang Trị Liệu..";
				FrmMain.CurGame.IsTriLieu = true;
				this.notifyIcon1.ShowBalloonTip(2000, "Thông Báo", "[" + FrmMain.CurGame.TLBB.Name.ToUpper() + "] đang tiến hành trị liệu", ToolTipIcon.Info);
			}
		}

		// Token: 0x06000DC3 RID: 3523 RVA: 0x0005E8F0 File Offset: 0x0005CAF0
		private void buttimbai_Click(object sender, EventArgs e)
		{
			if (FrmMain.CurGame != null)
			{
				if (FrmMain.CurGame.AutoTrain)
				{
					this.buttimbai.Text = "Tìm Bãi";
					FrmMain.CurGame.AutoTrain = false;
					return;
				}
				this.buttimbai.Text = "Đang AutoTrain..";
				FrmMain.CurGame.AutoTrain = true;
				this.notifyIcon1.ShowBalloonTip(2000, "Thông Báo", "[" + FrmMain.CurGame.TLBB.Name.ToUpper() + "] đang tiến hành AUTO Train", ToolTipIcon.Info);
			}
		}

		// Token: 0x06000DC4 RID: 3524 RVA: 0x0005E980 File Offset: 0x0005CB80
		private void comlenbai_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (FrmMain.CurGame != null)
			{
				FrmMain.CurGame.LenBaiIndex = this.comlenbai.SelectedIndex;
				BaiTrain baiTrain = (BaiTrain)(this.comlenbai.SelectedItem as FrmMain.ComboboxItem).Value;
				this.txttoadox.Text = (baiTrain.PosX.ToString() ?? "");
				this.txttoadoy.Text = (baiTrain.PosY.ToString() ?? "");
			}
		}

		// Token: 0x06000DC5 RID: 3525 RVA: 0x00006740 File Offset: 0x00004940
		private void thoátToolStripMenuItem_Click(object sender, EventArgs e)
		{
		}

		// Token: 0x06000DC6 RID: 3526
		[DllImport("user32.dll")]
		private static extern bool RegisterHotKey(IntPtr hWnd, int id, int fsModifiers, int vk);

		// Token: 0x06000DC7 RID: 3527
		[DllImport("user32.dll")]
		private static extern bool UnregisterHotKey(IntPtr hWnd, int id);

		// Token: 0x06000DC8 RID: 3528 RVA: 0x0005EA08 File Offset: 0x0005CC08
		public void SetHotKey()
		{
			FrmMain.RegisterHotKey(base.Handle, 1, 0, Keys.Pause.GetHashCode());
			FrmMain.RegisterHotKey(base.Handle, 6, 6, Keys.C.GetHashCode());
			FrmMain.RegisterHotKey(base.Handle, 13, 2, Keys.Q.GetHashCode());
			FrmMain.RegisterHotKey(base.Handle, 17, 1, Keys.F1.GetHashCode());
			FrmMain.RegisterHotKey(base.Handle, 18, 2, Keys.End.GetHashCode());
			FrmMain.RegisterHotKey(base.Handle, 20, 2, Keys.L.GetHashCode());
			FrmMain.RegisterHotKey(base.Handle, 21, 2, Keys.H.GetHashCode());
			FrmMain.RegisterHotKey(base.Handle, 23, 2, Keys.N.GetHashCode());
			FrmMain.RegisterHotKey(base.Handle, 29, 2, Keys.M.GetHashCode());
			FrmMain.RegisterHotKey(base.Handle, 24, 2, Keys.T.GetHashCode());
			FrmMain.RegisterHotKey(base.Handle, 25, 2, Keys.B.GetHashCode());
			FrmMain.RegisterHotKey(base.Handle, 26, 2, Keys.D.GetHashCode());
			FrmMain.RegisterHotKey(base.Handle, 27, 2, Keys.Delete.GetHashCode());
			FrmMain.RegisterHotKey(base.Handle, 28, 1, Keys.F2.GetHashCode());
			FrmMain.RegisterHotKey(base.Handle, 39, 1, Keys.F3.GetHashCode());
			FrmMain.RegisterHotKey(base.Handle, 32, 2, Keys.O.GetHashCode());
			FrmMain.RegisterHotKey(base.Handle, 33, 2, Keys.G.GetHashCode());
			FrmMain.RegisterHotKey(base.Handle, 34, 2, Keys.R.GetHashCode());
			FrmMain.RegisterHotKey(base.Handle, 35, 2, Keys.I.GetHashCode());
		}

		// Token: 0x06000DC9 RID: 3529 RVA: 0x0005EC60 File Offset: 0x0005CE60
		public void UnSetHoKey()
		{
			FrmMain.UnregisterHotKey(base.Handle, 1);
			FrmMain.UnregisterHotKey(base.Handle, 6);
			FrmMain.UnregisterHotKey(base.Handle, 13);
			FrmMain.UnregisterHotKey(base.Handle, 17);
			FrmMain.UnregisterHotKey(base.Handle, 18);
			FrmMain.UnregisterHotKey(base.Handle, 20);
			FrmMain.UnregisterHotKey(base.Handle, 21);
			FrmMain.UnregisterHotKey(base.Handle, 23);
			FrmMain.UnregisterHotKey(base.Handle, 24);
			FrmMain.UnregisterHotKey(base.Handle, 25);
			FrmMain.UnregisterHotKey(base.Handle, 26);
			FrmMain.UnregisterHotKey(base.Handle, 27);
			FrmMain.UnregisterHotKey(base.Handle, 28);
			FrmMain.UnregisterHotKey(base.Handle, 29);
			FrmMain.UnregisterHotKey(base.Handle, 30);
			FrmMain.UnregisterHotKey(base.Handle, 31);
			FrmMain.UnregisterHotKey(base.Handle, 32);
			FrmMain.UnregisterHotKey(base.Handle, 33);
			FrmMain.UnregisterHotKey(base.Handle, 34);
			FrmMain.UnregisterHotKey(base.Handle, 35);
			FrmMain.UnregisterHotKey(base.Handle, 39);
		}

		// Token: 0x06000DCA RID: 3530 RVA: 0x0005ED94 File Offset: 0x0005CF94
		private void FrmMain_FormClosing(object sender, FormClosingEventArgs e)
		{
			try
			{
				if (MessageBox.Show("Bạn có chắn chắn muốn thoát?", "Thoát Auto", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
				{
					this.menuExit_Click(null, null);
					string str = DateTime.Now.ToString("ddMMyy_HHmm");
					LoadFile.WriteFileWithEncrypt(this.txtlogs.Text, Global.LogPath + "\\Log_" + str + ".dat");
				}
				else
				{
					e.Cancel = true;
				}
			}
			catch
			{
				Application.Exit();
			}
		}

		// Token: 0x06000DCB RID: 3531 RVA: 0x0005EE1C File Offset: 0x0005D01C
		private void butthemskilldanhquai_Click(object sender, EventArgs e)
		{
			if (FrmMain.CurGame != null)
			{
				Skill skill = FrmMain.CurGame.Skills.Find((Skill x) => x.Name == this.comdanhsachdanhquai.SelectedItem.ToString());
				if (skill != null)
				{
					skill.Use = true;
					this.listViewSkill.Items.Add(skill.Name);
					FrmMain.CurGame.SaveSkill();
				}
			}
		}

		// Token: 0x06000DCC RID: 3532 RVA: 0x0005EE78 File Offset: 0x0005D078
		private void pictureBox10_Click(object sender, EventArgs e)
		{
			if (FrmMain.CurGame != null)
			{
				Skill skill = FrmMain.CurGame.Skills.Find((Skill x) => x.Name == this.comskillhotro.SelectedItem.ToString());
				if (skill != null)
				{
					skill.UserBuff = true;
					this.listviewskillhotro.Items.Add(skill.Name);
					FrmMain.CurGame.SaveSkillBuff();
				}
			}
		}

		// Token: 0x17000326 RID: 806
		// (get) Token: 0x06000DCD RID: 3533 RVA: 0x0005EED3 File Offset: 0x0005D0D3
		// (set) Token: 0x06000DCE RID: 3534 RVA: 0x0005EEDA File Offset: 0x0005D0DA
		public static bool IsDangCho { get; set; }

		// Token: 0x17000327 RID: 807
		// (get) Token: 0x06000DCF RID: 3535 RVA: 0x0005EEE2 File Offset: 0x0005D0E2
		// (set) Token: 0x06000DD0 RID: 3536 RVA: 0x0005EEEA File Offset: 0x0005D0EA
		public int LoginCount { get; set; }

		// Token: 0x06000DD1 RID: 3537 RVA: 0x0005EEF4 File Offset: 0x0005D0F4
		public bool HaveGame(int processId)
		{
			foreach (Account account in FrmMain.ListAutoLogin)
			{
				if (account.game != null && account.game.ProcessId == processId)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06000DD2 RID: 3538 RVA: 0x0005EF5C File Offset: 0x0005D15C
		private void Login(Account account)
		{
			bool flag = false;
			flag = !(account.Status == "Mở Game");
			if (account.game != null)
			{
				return;
			}
			foreach (KeyValuePair<int, Game> keyValuePair in FrmMain.dicGame)
			{
				try
				{
					if ((keyValuePair.Value.TLBB.IsSelectServer || keyValuePair.Value.TLBB.IsNexLogin) && !this.HaveGame(keyValuePair.Value.ProcessId) && Process.GetProcessById(keyValuePair.Value.ProcessId).MainModule.FileName == account.Path)
					{
						if (keyValuePair.Value.KetQuaSetTitle == 0)
						{
							CanhBao.AddLog("Lỗi Tự Động Đăng Nhập", "Lỗi đăng nhập tài khoản " + account.Name + "\nVui lòng đăng nhập thủ công ", CanhBao.Kieu.Eror);
							account.Status = "";
							keyValuePair.Value.IsQuit = true;
							account.game.UnHookRecv();
							account.game = null;
							FrmMain.dicGame.Remove(account.game.ProcessId);
							return;
						}
						account.IsNextLogin = Stopwatch.StartNew();
						account.Status = "Đăng Nhập";
						keyValuePair.Value.IsQuit = false;
						account.game = keyValuePair.Value;
						account.Entered = false;
						account.IsSelectRole = false;
						account.game.BHDCount = 0;
						return;
					}
				}
				catch
				{
				}
			}
			if (flag && (FrmMain.TotalGame < Global.MaxBHD || account.IsForceOpen))
			{
				if (TienIch.GameCount() > 3)
				{
					this.IsWait = false;
					account.Status = "";
					account.Entered = false;
					account.IsSelectRole = false;
					CanhBao.Msg("Lỗi Mở Game", "Đã mở quá giới hạn Client cho phép", CanhBao.Kieu.Eror);
					return;
				}
				this.IsWait = true;
				account.Status = "Mở Game";
				account.OpenGameTime = Stopwatch.StartNew();
				account.Entered = false;
				account.IsSelectRole = false;
				try
				{
					string text = LoadFile.LoadFileWithDecrypt(Global.DataPath + "\\ExecutePath.dat");
					string md = Offset.MD51.ToLower();
					Process.Start(new ProcessStartInfo
					{
						FileName = text,
						Arguments = ".\\Bin\\Game.exe " + this.GetCMDBYMD5(md),
						WorkingDirectory = Path.GetDirectoryName(text)
					});
				}
				catch (Exception ex)
				{
					MessageBox.Show(ex.ToString());
					account.Status = "";
					this.SettingPath();
				}
			}
		}

		// Token: 0x06000DD3 RID: 3539 RVA: 0x0005F234 File Offset: 0x0005D434
		public string GetCMDBYMD5(string MD5)
		{
			return "-fl";
		}

		// Token: 0x17000328 RID: 808
		// (get) Token: 0x06000DD4 RID: 3540 RVA: 0x0005F23B File Offset: 0x0005D43B
		public static int TotalGame
		{
			get
			{
				return FrmMain.dicGame.Count;
			}
		}

		// Token: 0x06000DD5 RID: 3541 RVA: 0x0005F248 File Offset: 0x0005D448
		private void tmrLogin_Tick(object sender, EventArgs e)
		{
			if (FrmMain.IsStop)
			{
				return;
			}
			FrmMain.IsDangCho = false;
			using (List<Account>.Enumerator enumerator = FrmMain.ListAutoLogin.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (enumerator.Current.Status.Contains("Đang chờ"))
					{
						FrmMain.IsDangCho = true;
						break;
					}
				}
			}
			this.LoginCount = 0;
			using (List<Account>.Enumerator enumerator = FrmMain.ListAutoLogin.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (enumerator.Current.game != null)
					{
						int loginCount = this.LoginCount;
						this.LoginCount = loginCount + 1;
					}
				}
			}
			foreach (KeyValuePair<int, Game> keyValuePair in FrmMain.dicGame)
			{
				if (keyValuePair.Value != null && !keyValuePair.Value.IsQuit && keyValuePair.Value.TLBB.Online && !keyValuePair.Value.IsCheckOnline)
				{
					keyValuePair.Value.IsCheckOnline = true;
					foreach (Account account in FrmMain.ListAutoLogin)
					{
						if (account.Ids.Contains(keyValuePair.Value.TLBB.Id))
						{
							account.game = keyValuePair.Value;
							account.IsSave = true;
						}
					}
				}
			}
			bool flag = false;
			foreach (Account account2 in FrmMain.ListAutoLogin)
			{
				if (account2.game != null && account2.game.IsQuit && !account2.game.TLBB.Online && account2.game.TLBB.IsSelectServer)
				{
					account2.game.IsQuit = false;
					account2.game = null;
					account2.Status = "";
				}
				if (account2.game == null || !account2.game.IsQuit)
				{
					if (account2.Status == "Thoát")
					{
						account2.game = null;
					}
					if (account2.game != null)
					{
						try
						{
							Process.GetProcessById(account2.game.ProcessId);
							if (account2.Status != "Mở Game")
							{
								if (account2.IsCaptcha)
								{
									account2.Status = "Đọc Captcha";
									flag = true;
								}
								else if (account2.Online)
								{
									account2.Status = "Online";
								}
								else if (!account2.game.ON_SCENE_TRANSING)
								{
									account2.Status = "Đăng Nhập";
								}
							}
							if (account2.game.TLBB.Online && !account2.IsSaveName)
							{
								account2.IsSaveName = true;
								account2.Name = account2.game.TLBB.Name;
								account2.Lvl = account2.game.TLBB.Lvl.ToString();
								account2.Menpai = account2.game.TLBB.MenpaiName.ToString();
								account2.Ids = account2.game.TLBB.Id;
								Account.Save();
							}
						}
						catch (Exception)
						{
							if (account2.Status != "xong BHD" && account2.Status != "xong BTD" && account2.Status != "xong DUA")
							{
								if (account2.game.IsBHDByLogin)
								{
									account2.Status = "Đang chờ làm BHD...";
								}
								else if (account2.game.IsBTDByLogin)
								{
									account2.Status = "Đang chờ làm Trừng Ác...";
								}
								else if (account2.game.IsDuaByLogin)
								{
									account2.Status = "Đang chờ làm Q Dưa...";
								}
								else
								{
									account2.Status = "Thoát";
								}
							}
							account2.game = null;
						}
					}
				}
			}
			if (!flag)
			{
				FrmMain.Captchas.Clear();
			}
			this.IsWait = false;
			foreach (Account account3 in FrmMain.ListAutoLogin)
			{
				if (!(account3.Status == "xong BHD") && !(account3.Status == "xong BTD") && !(account3.Status == "xong DUA") && account3.game == null)
				{
					if (account3.Status.Contains("Mở Game"))
					{
						this.IsWait = true;
						this.Login(account3);
						if (account3.OpenGameTime == null)
						{
							account3.OpenGameTime = Stopwatch.StartNew();
						}
						if (account3.IsBHD && account3.OpenGameTime.Elapsed.TotalMinutes > 10.0 && FrmMain.TotalGame < Global.MaxBHD)
						{
							account3.OpenGameTime = Stopwatch.StartNew();
							account3.Status = "Đang chờ làm BHD...";
						}
						if (account3.IsTrungAc && account3.OpenGameTime.Elapsed.TotalMinutes > 10.0 && FrmMain.TotalGame < Global.MaxBHD)
						{
							account3.OpenGameTime = Stopwatch.StartNew();
							account3.Status = "Đang chờ làm Trừng Ác...";
						}
						if (account3.IsDua && account3.OpenGameTime.Elapsed.TotalMinutes > 10.0 && FrmMain.TotalGame < Global.MaxBHD)
						{
							account3.OpenGameTime = Stopwatch.StartNew();
							account3.Status = "Đang chờ làm Q Dưa...";
							break;
						}
						break;
					}
					else if (account3.Status.Contains("Đang chờ"))
					{
						this.Login(account3);
						break;
					}
				}
			}
		}

		// Token: 0x17000329 RID: 809
		// (get) Token: 0x06000DD6 RID: 3542 RVA: 0x0005F8E4 File Offset: 0x0005DAE4
		// (set) Token: 0x06000DD7 RID: 3543 RVA: 0x0005F8EC File Offset: 0x0005DAEC
		private bool IsWait { get; set; }

		// Token: 0x06000DD8 RID: 3544 RVA: 0x0005F8F8 File Offset: 0x0005DAF8
		private void AccountLogin_DoWork(object sender, DoWorkEventArgs e)
		{
			base.Invoke(new Action(delegate()
			{
				this.ListViewLogin.Items.Clear();
			}));
			base.Invoke(new Action(delegate()
			{
				this.ComMayChu.Items.Clear();
			}));
			foreach (Account account in FrmMain.ListAutoLogin)
			{
				ListViewItem listViewItem = new ListViewItem
				{
					UseItemStyleForSubItems = false
				};
				listViewItem.Text = account.User;
				listViewItem.SubItems.Add(account.Server);
				listViewItem.SubItems.Add(account.Status);
				listViewItem.SubItems.Add(account.Name + " | Thứ " + account.LoginIndex);
				listViewItem.SubItems.Add(account.Menpai);
				listViewItem.Tag = account;
				base.Invoke(new Action(delegate()
				{
					this.ListViewLogin.Items.Add(listViewItem);
				}));
			}
			using (List<ServerList>.Enumerator enumerator2 = Unity.GetDanhSach().GetEnumerator())
			{
				while (enumerator2.MoveNext())
				{
					ServerList sv = enumerator2.Current;
					base.Invoke(new Action(delegate()
					{
						this.ComMayChu.Items.Add(sv.ServerName);
					}));
				}
			}
			if (this.ComMayChu.Items.Count > 0)
			{
				base.Invoke(new Action(delegate()
				{
					this.ComMayChu.SelectedIndex = 0;
				}));
			}
		}

		// Token: 0x06000DD9 RID: 3545 RVA: 0x0005FAB4 File Offset: 0x0005DCB4
		private void listviewskillhotro_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.KeyCode == Keys.Delete)
			{
				if (this.listviewskillhotro.SelectedItems.Count == 0)
				{
					return;
				}
				using (IEnumerator enumerator = this.listviewskillhotro.SelectedItems.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						ListViewItem listViewItem = (ListViewItem)enumerator.Current;
						if (listViewItem.Text.Trim() != "")
						{
							Skill skill = FrmMain.CurGame.Skills.Find((Skill x) => TINHKIEM.VietLien(x.Name) == TINHKIEM.VietLien(listViewItem.Text.Trim()));
							if (skill != null)
							{
								skill.UserBuff = false;
								FrmMain.CurGame.SaveSkillBuff();
							}
							listViewItem.Remove();
						}
					}
				}
			}
		}

		// Token: 0x06000DDA RID: 3546 RVA: 0x0005FB90 File Offset: 0x0005DD90
		private void listViewSkill_KeyDown(object sender, KeyEventArgs e)
		{
			if (this.listViewSkill.SelectedItems.Count == 0)
			{
				return;
			}
			using (IEnumerator enumerator = this.listViewSkill.SelectedItems.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					ListViewItem listViewItem = (ListViewItem)enumerator.Current;
					if (listViewItem.Text.Trim() != "")
					{
						Skill skill = FrmMain.CurGame.Skills.Find((Skill x) => TINHKIEM.VietLien(x.Name) == TINHKIEM.VietLien(listViewItem.Text.Trim()));
						if (skill != null)
						{
							skill.Use = false;
							FrmMain.CurGame.SaveSkill();
						}
						listViewItem.Remove();
					}
				}
			}
		}

		// Token: 0x06000DDB RID: 3547 RVA: 0x0005FC5C File Offset: 0x0005DE5C
		public bool CheckExit(string username, string Server)
		{
			foreach (Account account in FrmMain.ListAutoLogin)
			{
				if (account.User == username && account.Server == Server)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06000DDC RID: 3548 RVA: 0x0005FCCC File Offset: 0x0005DECC
		private void button7_Click(object sender, EventArgs e)
		{
			if (this.txttk.Text.Length == 0 || this.txtmk.Text.Length == 0)
			{
				MessageBox.Show("Kiểm tra dữ liệu nhập vào", "Thêm tài khoản lỗi", MessageBoxButtons.OK, MessageBoxIcon.Hand);
				return;
			}
			if (!this.CheckExit(this.txttk.Text.Trim(), this.ComMayChu.SelectedItem.ToString()))
			{
				new Account(this.txttk.Text.Trim(), this.txtmk.Text.Trim(), "Tình Kiếm", this.ComMayChu.SelectedItem.ToString(), this.ComMayChu.SelectedItem.ToString());
				FrmMain.LoadAccountLogin();
				if (!this.AccountLogin.IsBusy)
				{
					this.AccountLogin.RunWorkerAsync();
					return;
				}
			}
			else
			{
				MessageBox.Show("Tài khoản đã được thêm rồi", "Thêm tài khoản lỗi", MessageBoxButtons.OK, MessageBoxIcon.Hand);
			}
		}

		// Token: 0x06000DDD RID: 3549 RVA: 0x0005FDB8 File Offset: 0x0005DFB8
		public void SettingPath()
		{
			MessageBox.Show(this, "Bạn cần phải chọn đường dẫn tới Game.exe\r\nFile Game.exe nằm trong thư mục Bin của TLBB", "ChickenAuto", MessageBoxButtons.OK);
			OpenFileDialog openFileDialog = new OpenFileDialog();
			openFileDialog.Filter = "Game.exe |Game.exe| GameOLD.exe |GameOLD.exe| All files (*.*)|*.*";
			if (openFileDialog.ShowDialog(this) == DialogResult.OK)
			{
				try
				{
					LoadFile.WriteFileWithEncrypt(openFileDialog.FileName, Global.DataPath + "\\ExecutePath.dat");
				}
				catch
				{
				}
			}
		}

		// Token: 0x06000DDE RID: 3550 RVA: 0x0005FE24 File Offset: 0x0005E024
		private void ListViewLogin_DoubleClick(object sender, EventArgs e)
		{
			if (this.ListViewLogin.SelectedItems.Count == 0)
			{
				return;
			}
			if (TienIch.GameCount() > 3)
			{
				CanhBao.Msg("Lỗi Mở Game", "Đã mở quá giới hạn Client cho phép", CanhBao.Kieu.Eror);
				return;
			}
			Account account = (Account)this.ListViewLogin.SelectedItems[0].Tag;
			if (account.game == null || account.Status == "Thoát")
			{
				if (account.Status == "" || account.Status == "Thoát")
				{
					if (account.Path == "")
					{
						this.SettingPath();
						return;
					}
					account.Status = "Đang chờ...";
					account.IsForceOpen = true;
					return;
				}
			}
			else
			{
				account.game.Active();
			}
		}

		// Token: 0x14000013 RID: 19
		// (add) Token: 0x06000DDF RID: 3551 RVA: 0x0005FEF0 File Offset: 0x0005E0F0
		// (remove) Token: 0x06000DE0 RID: 3552 RVA: 0x0005FF28 File Offset: 0x0005E128
		public event EventHandler AccChanged;

		// Token: 0x06000DE1 RID: 3553 RVA: 0x0005FF60 File Offset: 0x0005E160
		private void ListViewLogin_DragDrop(object sender, DragEventArgs e)
		{
			XmlNode xmlNode = Account.XML.SelectSingleNode("/*");
			FrmMain.ListAutoLogin.Clear();
			xmlNode.RemoveAll();
			foreach (object obj in this.ListViewLogin.Items)
			{
				Account account = (Account)((ListViewItem)obj).Tag;
				xmlNode.AppendChild(account.Node);
				FrmMain.ListAutoLogin.Add(account);
			}
			Account.Save();
			if (this.AccChanged != null)
			{
				this.AccChanged(this, null);
			}
		}

		// Token: 0x06000DE2 RID: 3554 RVA: 0x0005FFF0 File Offset: 0x0005E1F0
		private void menuDelete_Click(object sender, EventArgs e)
		{
			if (this.ListViewLogin.SelectedItems.Count == 0)
			{
				return;
			}
			if (MessageBox.Show(this, "Bạn có muốn xóa thông tin những acc đã chọn", "ChickenAuto", MessageBoxButtons.YesNo) == DialogResult.Yes)
			{
				foreach (object obj in this.ListViewLogin.SelectedItems)
				{
					ListViewItem listViewItem = (ListViewItem)obj;
					try
					{
						XmlNode node = ((Account)listViewItem.Tag).Node;
						Account.XML.SelectSingleNode("/*").RemoveChild(node);
						listViewItem.Remove();
					}
					catch
					{
					}
				}
				if (this.AccChanged != null)
				{
					this.AccChanged(this, null);
				}
				Account.Save();
				FrmMain.LoadAccountLogin();
			}
		}

		// Token: 0x06000DE3 RID: 3555 RVA: 0x000600D0 File Offset: 0x0005E2D0
		private void ListViewLogin_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.KeyCode == Keys.Delete)
			{
				this.menuDelete_Click(null, null);
			}
			if (e.KeyCode == Keys.Return)
			{
				if (TienIch.GameCount() > 3)
				{
					CanhBao.Msg("Lỗi Mở Game", "Đã mở quá giới hạn Client cho phép", CanhBao.Kieu.Eror);
					return;
				}
				foreach (object obj in this.ListViewLogin.SelectedItems)
				{
					Account account = (Account)((ListViewItem)obj).Tag;
					if (account.Path == "")
					{
						this.SettingPath();
						return;
					}
					if (account.game == null)
					{
						account.Status = "Đang chờ...";
					}
				}
			}
			if (e.KeyCode == Keys.A && e.Control)
			{
				foreach (object obj2 in this.ListViewLogin.Items)
				{
					((ListViewItem)obj2).Selected = true;
				}
			}
			if (e.KeyCode == Keys.F1)
			{
				foreach (object obj3 in this.ListViewLogin.SelectedItems)
				{
					((Account)((ListViewItem)obj3).Tag).LoginIndex = "1";
				}
			}
			if (e.KeyCode == Keys.F2)
			{
				foreach (object obj4 in this.ListViewLogin.SelectedItems)
				{
					((Account)((ListViewItem)obj4).Tag).LoginIndex = "2";
				}
			}
			if (e.KeyCode == Keys.F3)
			{
				foreach (object obj5 in this.ListViewLogin.SelectedItems)
				{
					((Account)((ListViewItem)obj5).Tag).LoginIndex = "3";
				}
			}
			if (e.KeyCode == Keys.F5)
			{
				foreach (object obj6 in this.ListViewLogin.SelectedItems)
				{
					Account account2 = (Account)((ListViewItem)obj6).Tag;
					if (account2.Path == "")
					{
						this.SettingPath();
						return;
					}
					if (account2.game == null)
					{
						account2.Status = "";
					}
				}
			}
		}

		// Token: 0x06000DE4 RID: 3556 RVA: 0x000602ED File Offset: 0x0005E4ED
		private void Pop()
		{
			if (base.IsDisposed || !Global.AntiCaptcha)
			{
				return;
			}
			if (!this.IsPop)
			{
				return;
			}
			bool running = this.Running;
		}

		// Token: 0x06000DE5 RID: 3557 RVA: 0x0006030F File Offset: 0x0005E50F
		private void ValidCaptcha(string hash)
		{
			if (base.IsDisposed || !Global.AntiCaptcha)
			{
				return;
			}
			bool isPop = this.IsPop;
		}

		// Token: 0x06000DE6 RID: 3558 RVA: 0x00060328 File Offset: 0x0005E528
		private void tmrRefresh_Tick(object sender, EventArgs e)
		{
			if (FrmMain.IsStop)
			{
				return;
			}
			if (Global.AntiCaptcha)
			{
				foreach (KeyValuePair<string, string> keyValuePair in FrmMain.Captchas)
				{
					if (!FrmMain.Answers.ContainsKey(keyValuePair.Key))
					{
						this.Pop();
					}
				}
			}
			foreach (Account account in FrmMain.ListAutoLogin)
			{
				Game game = account.game;
				if (game != null && !(account.Status == "xong BHD") && !account.game.BachHoaDuyenCompleted && !(account.Status == "xong BTD") && !account.game.IsXongTrungAc && !(account.Status == "xong DUA") && !account.game.IsXongTrungAc)
				{
					if (account.Online)
					{
						if (!account.IsSave)
						{
							account.IsSave = true;
							if (!account.Ids.Contains(game.TLBB.Id))
							{
								Account account2 = account;
								account2.Ids += game.TLBB.Id;
							}
						}
						account.Entered = false;
						account.IsSelectRole = false;
						if (FrmMain.Answers.ContainsKey(account.ImgHash) && account.IsGetAn)
						{
							this.ValidCaptcha(account.ImgHash);
							FrmMain.Answers.Remove(account.ImgHash);
							if (FrmMain.Captchas.ContainsKey(account.ImgHash))
							{
								FrmMain.Captchas.Remove(account.ImgHash);
							}
						}
					}
					if (account.IsDua && game.TLBB.OnlineTimeSec < 60)
					{
						game.IsQDua = true;
						game.IsDuaByLogin = true;
					}
					if (account.IsTrong)
					{
						if (game.TLBB.OnlineTimeSec < 60)
						{
							game.IsTrongHoa = (game.IsBonHoa = (game.IsTrongByLogin = true));
						}
					}
					else
					{
						if (account.IsBHD && game.TLBB.OnlineTimeSec < 60)
						{
							game.IsBachHoaDuyen = true;
							game.IsBHDByLogin = true;
						}
						if (account.IsNhanMam && game.TLBB.OnlineTimeSec < 60)
						{
							game.IsBachHoaDuyen = true;
							game.IsBHDByLogin = true;
							game.IsNhanMam = true;
						}
					}
					if (account.IsTrungAc && game.TLBB.OnlineTimeSec < 60)
					{
						game.IsTrungAc = true;
						game.IsBTDByLogin = true;
					}
					if (account.IsCaptcha && !game.TLBB.IsLogon)
					{
						this.notifyIcon1.ShowBalloonTip(2000, "Thông Báo", "Vui lòng nhập Captcha cho tài khoản : " + account.User, ToolTipIcon.Info);
					}
					if (!account.Online)
					{
						if (game.TLBB.IsNexLogin && game.KetQuaSetTitle == 1)
						{
							game.LUA.HuoDongRiChengNextClick();
						}
						else if (game.TLBB.IsSelectServer)
						{
							account.Entered = (account.IsSelectRole = false);
							if (account.ServerIndex != -1)
							{
								if (game.Address.GameType == 1)
								{
									if (account.Server == "Thiên Long 15")
									{
										game.LuaDoOneLineString("setmetatable(_G, {__index = LoginSelectServer_Env}); SelectServerEvent_Area_SwitchPage(9); SelectServerEvent_ServerBn_Clicked(" + 1.ToString() + ",0); SelectServerEvent_SelectOk();");
									}
									else if (account.Server == "Thiên Long 16")
									{
										game.LuaDoOneLineString("setmetatable(_G, {__index = LoginSelectServer_Env}); SelectServerEvent_Area_SwitchPage(9); SelectServerEvent_ServerBn_Clicked(" + 2.ToString() + ",0); SelectServerEvent_SelectOk();");
									}
									else
									{
										game.LuaDoOneLineString("setmetatable(_G, {__index = LoginSelectServer_Env}); SelectServerEvent_Area_SwitchPage(10); SelectServerEvent_ServerBn_Clicked(" + account.ServerIndex.ToString() + ",0); SelectServerEvent_SelectOk();");
									}
								}
								else
								{
									game.LuaDoOneLineString("setmetatable(_G, {__index = LoginSelectServer_Env}); local index = GameProduceLogin:GetServerAreaCount() - 1; setmetatable(_G, {__index = LoginSelectServer_Env}); SelectServer_SelectAreaServer(index); setmetatable(_G, {__index = LoginSelectServer_Env}); SelectServer_ConfirmSelectLine(" + account.ServerIndex.ToString() + ");");
								}
							}
						}
						else
						{
							if (game.TLBB.IsLogon && !account.Entered)
							{
								account.LogonTime = Stopwatch.StartNew();
								if (account.TailIndex != -1)
								{
									game.LUA.LogOnSelectTail(account.TailIndex);
								}
								foreach (char wParam in account.User)
								{
									Win.PostMessage(game.Handle, 258, (int)wParam, 0);
								}
								Win.PostMessage(game.Handle, 256, 9, 0);
								Win.PostMessage(game.Handle, 257, 9, 0);
								foreach (char wParam2 in account.Pass)
								{
									Win.PostMessage(game.Handle, 258, (int)wParam2, 0);
								}
								Win.PostMessage(game.Handle, 256, 13, 0);
								Win.PostMessage(game.Handle, 257, 13, 0);
								account.Entered = true;
								account.IsSelectRole = false;
							}
							if (game.TLBB.IsLogon)
							{
								account.SelectAccTime = Stopwatch.StartNew();
								if (account.LogonTime.Elapsed.TotalSeconds < 22.0)
								{
									account.game.PushDebugMessage("Chọn lại máy chủ sau " + ((int)(22.0 - account.LogonTime.Elapsed.TotalSeconds)).ToString() + " giây");
								}
								else
								{
									account.SelectAccTime = Stopwatch.StartNew();
									account.IsSelectRole = false;
									account.Entered = false;
									account.LoginMessageTime = 0;
									game.LUA.LogOn_ExitToSelectServer();
									account.SelectAccTime = null;
								}
							}
							else
							{
								account.LogonTime = Stopwatch.StartNew();
							}
							if (account.Entered && game.TLBB.IsLogon && !game.TLBB.IsSelectServerQuest)
							{
								account.LoginMessageTime++;
								if (account.LoginMessageTime > 10 || account.LoginMessageTime == 0)
								{
									account.LoginMessageTime = 0;
									Win.PostMessage(game.Handle, 256, 13, 0);
									Win.PostMessage(game.Handle, 257, 13, 0);
								}
							}
							if (game.TLBB.IsSelectCharacter && !game.TLBB.IsTextCaptcha && !account.IsSelectRole)
							{
								if (account.SelectAccTime == null)
								{
									account.SelectAccTime = Stopwatch.StartNew();
								}
								if (account.LoginIndex == "1")
								{
									if (account.SelectAccTime.Elapsed.TotalSeconds > 5.0 && account.SelectAccTime.Elapsed.TotalSeconds <= 10.0)
									{
										game.LuaDoString("setmetatable(_G, {__index = LoginSelectServer_Env}); local index = SelectRole_SelectRole1();");
										account.IsSelect = -1;
									}
								}
								else if (account.LoginIndex == "2")
								{
									if (account.SelectAccTime.Elapsed.TotalSeconds > 5.0 && account.SelectAccTime.Elapsed.TotalSeconds <= 10.0)
									{
										game.LuaDoString("setmetatable(_G, {__index = LoginSelectServer_Env}); local index = SelectRole_SelectRole2();");
										account.IsSelect = -1;
									}
								}
								else if (account.LoginIndex == "3" && account.SelectAccTime.Elapsed.TotalSeconds > 5.0 && account.SelectAccTime.Elapsed.TotalSeconds <= 10.0)
								{
									game.LuaDoString("setmetatable(_G, {__index = LoginSelectServer_Env}); local index = SelectRole_SelectRole3();");
									account.IsSelect = -1;
								}
								if (account.SelectAccTime.Elapsed.TotalSeconds > 5.0)
								{
									game.LUA.SelectRoleEnterGame();
									account.Entered = false;
								}
							}
							if (game.TLBB.IsSelectCharacter)
							{
								if (account.SelectAccTime.Elapsed.TotalSeconds >= 60.0)
								{
									account.SelectAccTime = Stopwatch.StartNew();
									account.IsSelectRole = false;
									account.Entered = false;
									account.LoginMessageTime = 0;
									account.game.LuaDoOneLineString("DataPool:SendLoginCode('1222')");
								}
								else
								{
									account.game.PushDebugMessage("Đổi Captcha sau " + ((int)(60.0 - account.SelectAccTime.Elapsed.TotalSeconds)).ToString() + " giây");
								}
							}
							if (game.TLBB.IsTextCaptcha)
							{
								account.IsSelectRole = true;
							}
						}
					}
					else if (game.TLBB.IsSelectServer)
					{
						account.game = null;
						account.Status = "";
						account.Entered = false;
						account.IsSelectRole = false;
					}
				}
			}
		}

		// Token: 0x06000DE7 RID: 3559 RVA: 0x00060C68 File Offset: 0x0005EE68
		private void TablControl_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (this.TablControl.SelectedTab == this.TablControl.TabPages[6])
			{
				this.IsLoginTab = true;
			}
			else
			{
				this.IsLoginTab = false;
			}
			if (this.TablControl.SelectedTab == this.TablControl.TabPages[5])
			{
				this.IsCheDoTab = true;
				if (FrmMain.CurGame != null)
				{
					this.comboloai.SelectedIndex = FrmMain.CurGame.CheLoai;
					this.comcapdtd.SelectedIndex = FrmMain.CurGame.CheCap;
					this.comboBox3.SelectedIndex = 0;
					if (FrmMain.CurGame.IsCheDo)
					{
						this.butche.Text = "Tắt";
					}
					else
					{
						this.butche.Text = "Chế";
					}
					VatLieu vatLieu = new VatLieu();
					vatLieu = FrmMain.CurGame.getsoluong(this.comboloai.SelectedItem.ToString(), this.comcapdtd.SelectedIndex + 1);
					this.txtbingan.Text = (vatLieu.BiNgan.ToString() ?? "");
					this.txttinhthiet.Text = (vatLieu.TinhThiet.ToString() ?? "");
					this.txtvaibong.Text = (vatLieu.VaiBong.ToString() ?? "");
					this.txttaodo.Text = (vatLieu.DaTaoDo.ToString() ?? "");
					this.txtdache.Text = (vatLieu.DaChe.ToString() ?? "");
					this.txtdahuy.Text = (vatLieu.DaHuy.ToString() ?? "");
					this.numsosao.Value = FrmMain.CurGame.CheSao;
					this.numericUpDown6.Value = FrmMain.CurGame.CheDiem;
					this.numericUpDown5.Value = FrmMain.CurGame.CheDong;
					this.numsonguyenlieu.Value = FrmMain.CurGame.SoLuongMua;
					this.numericUpDown4.Value = FrmMain.CurGame.SoLuongChe;
					this.checkBox12.Checked = !FrmMain.CurGame.IsMuaNguyenLieu;
					this.checkBox13.Checked = FrmMain.CurGame.HuyNguyenLieu;
					return;
				}
			}
			else
			{
				this.IsCheDoTab = false;
			}
		}

		// Token: 0x06000DE8 RID: 3560 RVA: 0x00006740 File Offset: 0x00004940
		private void FrmMain_Resize(object sender, EventArgs e)
		{
		}

		// Token: 0x06000DE9 RID: 3561 RVA: 0x00060EEA File Offset: 0x0005F0EA
		private void notifyIcon1_DoubleClick(object sender, EventArgs e)
		{
			base.Show();
			base.WindowState = FormWindowState.Normal;
			base.TopLevel = true;
		}

		// Token: 0x06000DEA RID: 3562 RVA: 0x00060F00 File Offset: 0x0005F100
		private void ẩnAutoToolStripMenuItem_Click(object sender, EventArgs e)
		{
			base.Hide();
			this.notifyIcon1.ShowBalloonTip(2000, "Thông Báo", "Auto đang ẩn tại đây.Chuột phải xem danh sách chức năng!", ToolTipIcon.Info);
		}

		// Token: 0x06000DEB RID: 3563 RVA: 0x00060F24 File Offset: 0x0005F124
		private void checkpickitem_CheckedChanged(object sender, EventArgs e)
		{
			if (FrmMain.CurGame != null)
			{
				if (!this.VuaBatXong())
				{
					this.notifyIcon1.ShowBalloonTip(2000, "Thông Báo", string.Concat(new string[]
					{
						"[",
						FrmMain.CurGame.TLBB.Name.ToUpper(),
						"]",
						this.checkpickitem.Checked ? " Bật " : " Tắt ",
						"nhặt vật phẩm"
					}), ToolTipIcon.Info);
				}
				FrmMain.CurGame.IsPickItem = this.checkpickitem.Checked;
			}
		}

		// Token: 0x06000DEC RID: 3564 RVA: 0x00060FC4 File Offset: 0x0005F1C4
		private void numericUpDown2_ValueChanged(object sender, EventArgs e)
		{
			Global.PickRadius = (int)this.numrangerpickitem.Value;
		}

		// Token: 0x06000DED RID: 3565 RVA: 0x00060FDC File Offset: 0x0005F1DC
		private void checkhuyitem_CheckedChanged(object sender, EventArgs e)
		{
			if (FrmMain.CurGame != null)
			{
				FrmMain.CurGame.IsDropItem = this.checkhuyitem.Checked;
				if (!this.VuaBatXong())
				{
					this.notifyIcon1.ShowBalloonTip(2000, "Thông Báo", string.Concat(new string[]
					{
						"[",
						FrmMain.CurGame.TLBB.Name.ToUpper(),
						"]",
						this.checkhuyitem.Checked ? " Bật " : " Tắt ",
						"hủy vật phẩm"
					}), ToolTipIcon.Info);
				}
			}
		}

		// Token: 0x06000DEE RID: 3566 RVA: 0x0006107C File Offset: 0x0005F27C
		private void pickbanvatpham_CheckedChanged(object sender, EventArgs e)
		{
			if (FrmMain.CurGame != null)
			{
				FrmMain.CurGame.IsSellItem = this.pickbanvatpham.Checked;
				if (!this.VuaBatXong())
				{
					this.notifyIcon1.ShowBalloonTip(2000, "Thông Báo", string.Concat(new string[]
					{
						"[",
						FrmMain.CurGame.TLBB.Name.ToUpper(),
						"]",
						this.pickbanvatpham.Checked ? " Bật " : " Tắt ",
						"bán vật phẩm"
					}), ToolTipIcon.Info);
				}
			}
		}

		// Token: 0x06000DEF RID: 3567 RVA: 0x0006111C File Offset: 0x0005F31C
		private void checkautocatkho_CheckedChanged(object sender, EventArgs e)
		{
			if (FrmMain.CurGame != null)
			{
				FrmMain.CurGame.IsBank = this.checkautocatkho.Checked;
				if (!this.VuaBatXong())
				{
					this.notifyIcon1.ShowBalloonTip(2000, "Thông Báo", string.Concat(new string[]
					{
						"[",
						FrmMain.CurGame.TLBB.Name.ToUpper(),
						"]",
						this.checkautocatkho.Checked ? " Bật " : " Tắt ",
						"cất đồ vào kho"
					}), ToolTipIcon.Info);
				}
			}
		}

		// Token: 0x06000DF0 RID: 3568 RVA: 0x000611BC File Offset: 0x0005F3BC
		private void checkautovutrac_CheckedChanged(object sender, EventArgs e)
		{
			if (FrmMain.CurGame != null)
			{
				Global.IsVutRac = this.checkautovutrac.Checked;
				if (!this.VuaBatXong())
				{
					this.notifyIcon1.ShowBalloonTip(2000, "Thông Báo", string.Concat(new string[]
					{
						"[",
						FrmMain.CurGame.TLBB.Name.ToUpper(),
						"]",
						this.checkautocatkho.Checked ? " Bật " : " Tắt ",
						"vứt rác"
					}), ToolTipIcon.Info);
				}
			}
		}

		// Token: 0x06000DF1 RID: 3569 RVA: 0x00061258 File Offset: 0x0005F458
		private void chekcautox2_CheckedChanged(object sender, EventArgs e)
		{
			if (FrmMain.CurGame != null)
			{
				FrmMain.CurGame.TuAnX2 = this.chekcautox2.Checked;
				if (!this.VuaBatXong())
				{
					this.notifyIcon1.ShowBalloonTip(2000, "Thông Báo", string.Concat(new string[]
					{
						"[",
						FrmMain.CurGame.TLBB.Name.ToUpper(),
						"]",
						this.chekcautox2.Checked ? " Bật " : " Tắt ",
						"ăn X2.5"
					}), ToolTipIcon.Info);
				}
			}
		}

		// Token: 0x06000DF2 RID: 3570 RVA: 0x000612F8 File Offset: 0x0005F4F8
		private void chekcusingitem_CheckedChanged(object sender, EventArgs e)
		{
			if (FrmMain.CurGame != null)
			{
				FrmMain.CurGame.AutoEatVatPham = this.chekcusingitem.Checked;
				if (!this.VuaBatXong())
				{
					this.notifyIcon1.ShowBalloonTip(2000, "Thông Báo", string.Concat(new string[]
					{
						"[",
						FrmMain.CurGame.TLBB.Name.ToUpper(),
						"]",
						this.chekcusingitem.Checked ? " Bật " : " Tắt ",
						"sử dụng vật phẩm tuần hoàn"
					}), ToolTipIcon.Info);
				}
			}
		}

		// Token: 0x06000DF3 RID: 3571 RVA: 0x00061398 File Offset: 0x0005F598
		private void butdanhsachhuy_Click(object sender, EventArgs e)
		{
			new DropItem().Show();
		}

		// Token: 0x06000DF4 RID: 3572 RVA: 0x000613A4 File Offset: 0x0005F5A4
		private void butbanvatpham_Click(object sender, EventArgs e)
		{
			new SellItem().Show();
		}

		// Token: 0x06000DF5 RID: 3573 RVA: 0x000613B0 File Offset: 0x0005F5B0
		private void AutoXuatPhet_CheckedChanged(object sender, EventArgs e)
		{
			Global.IsXuat = this.AutoXuatPhet.Checked;
		}

		// Token: 0x06000DF6 RID: 3574 RVA: 0x000613C2 File Offset: 0x0005F5C2
		private void checkBox9_CheckedChanged(object sender, EventArgs e)
		{
			Global.BuffPet = this.checkBox9.Checked;
		}

		// Token: 0x06000DF7 RID: 3575 RVA: 0x000613D4 File Offset: 0x0005F5D4
		private void checkBox10_CheckedChanged(object sender, EventArgs e)
		{
			Global.UseSkillPet = this.checkBox10.Checked;
		}

		// Token: 0x06000DF8 RID: 3576 RVA: 0x000613E6 File Offset: 0x0005F5E6
		private void CheckthuPet_CheckedChanged(object sender, EventArgs e)
		{
			if (FrmMain.CurGame != null)
			{
				FrmMain.CurGame.AutoThuPet = this.CheckthuPet.Checked;
			}
		}

		// Token: 0x06000DF9 RID: 3577 RVA: 0x00006740 File Offset: 0x00004940
		private void groupBox6_Enter(object sender, EventArgs e)
		{
		}

		// Token: 0x06000DFA RID: 3578 RVA: 0x00061404 File Offset: 0x0005F604
		private void CheckReGenPET_CheckedChanged(object sender, EventArgs e)
		{
			if (FrmMain.CurGame != null)
			{
				FrmMain.CurGame.IsPet = this.CheckReGenPET.Checked;
			}
		}

		// Token: 0x06000DFB RID: 3579 RVA: 0x00061424 File Offset: 0x0005F624
		private void checkdongytodoi_CheckedChanged(object sender, EventArgs e)
		{
			if (FrmMain.CurGame != null)
			{
				Global.AutoAccept = this.checkdongytodoi.Checked;
				if (!this.VuaBatXong())
				{
					CanhBao.Msg("Thiết Lập Thành Công", string.Concat(new string[]
					{
						"[",
						FrmMain.CurGame.TLBB.Name.ToUpper(),
						"]",
						this.checkdongytodoi.Checked ? " Bật " : " Tắt ",
						"đồng ý tổ đội"
					}), CanhBao.Kieu.OK);
				}
			}
		}

		// Token: 0x06000DFC RID: 3580 RVA: 0x000614B4 File Offset: 0x0005F6B4
		private void checkdongytoanbo_CheckedChanged(object sender, EventArgs e)
		{
			if (FrmMain.CurGame != null)
			{
				Global.AcceptAll = this.checkdongytoanbo.Checked;
				if (!this.VuaBatXong())
				{
					CanhBao.Msg("Thiết Lập Thành Công", string.Concat(new string[]
					{
						"[",
						FrmMain.CurGame.TLBB.Name.ToUpper(),
						"]",
						this.checkdongytoanbo.Checked ? " Bật " : " Tắt ",
						"đồng ý tất cả"
					}), CanhBao.Kieu.OK);
				}
				if (Global.AcceptAll)
				{
					using (Dictionary<int, Game>.Enumerator enumerator = FrmMain.dicGame.GetEnumerator())
					{
						if (enumerator.MoveNext())
						{
							KeyValuePair<int, Game> keyValuePair = enumerator.Current;
							keyValuePair.Value.SetTeam(null);
							return;
						}
					}
				}
				using (Dictionary<int, Game>.Enumerator enumerator = FrmMain.dicGame.GetEnumerator())
				{
					if (enumerator.MoveNext())
					{
						KeyValuePair<int, Game> keyValuePair2 = enumerator.Current;
						keyValuePair2.Value.SetTeamFromList(Setting.BuffValue);
					}
				}
			}
		}

		// Token: 0x06000DFD RID: 3581 RVA: 0x000615E8 File Offset: 0x0005F7E8
		private void numbankinhtheosau_ValueChanged(object sender, EventArgs e)
		{
			Global.FollowRadius = (int)this.numbankinhtheosau.Value;
		}

		// Token: 0x06000DFE RID: 3582 RVA: 0x000615FF File Offset: 0x0005F7FF
		private void txtmkkho_TextChanged(object sender, EventArgs e)
		{
			if (FrmMain.CurGame != null)
			{
				FrmMain.CurGame.Pass2 = this.txtmk.Text;
				FrmMain.CurGame.SavePass2();
			}
		}

		// Token: 0x06000DFF RID: 3583 RVA: 0x00061628 File Offset: 0x0005F828
		private void pictureBox12_Click(object sender, EventArgs e)
		{
			OpenFileDialog openFileDialog = new OpenFileDialog();
			openFileDialog.Filter = "Game.exe |Game.exe| GameOLD.exe |GameOLD.exe| All files (*.*)|*.*";
			if (openFileDialog.ShowDialog(this) == DialogResult.OK)
			{
				try
				{
					LoadFile.WriteFileWithEncrypt(openFileDialog.FileName, Global.DataPath + "\\ExecutePath.dat");
				}
				catch
				{
				}
			}
		}

		// Token: 0x06000E00 RID: 3584 RVA: 0x00061680 File Offset: 0x0005F880
		private void checkgiaochat_CheckedChanged(object sender, EventArgs e)
		{
			if (FrmMain.CurGame != null)
			{
				FrmMain.CurGame.IsRao = this.checkgiaochat.Checked;
				if (!this.VuaBatXong())
				{
					CanhBao.Msg("Thiết Lập Thành Công", string.Concat(new string[]
					{
						"[",
						FrmMain.CurGame.TLBB.Name.ToUpper(),
						"]",
						this.checkgiaochat.Checked ? " Bật " : " Tắt ",
						"giao chát"
					}), CanhBao.Kieu.OK);
				}
			}
		}

		// Token: 0x06000E01 RID: 3585 RVA: 0x00061714 File Offset: 0x0005F914
		private void checkauouplevel_CheckedChanged(object sender, EventArgs e)
		{
			if (FrmMain.CurGame != null)
			{
				Global.AutoUpLvl = this.checkauouplevel.Checked;
				if (!this.VuaBatXong())
				{
					CanhBao.Msg("Thiết Lập Thành Công", string.Concat(new string[]
					{
						"[",
						FrmMain.CurGame.TLBB.Name.ToUpper(),
						"]",
						this.checkauouplevel.Checked ? " Bật " : " Tắt ",
						"tự tăng cấp độ"
					}), CanhBao.Kieu.OK);
				}
			}
		}

		// Token: 0x06000E02 RID: 3586 RVA: 0x000617A4 File Offset: 0x0005F9A4
		private void checkautoskillf1_CheckedChanged(object sender, EventArgs e)
		{
			if (FrmMain.CurGame != null)
			{
				Option.PutBase = this.checkautoskillf1.Checked;
				if (!this.VuaBatXong())
				{
					CanhBao.Msg("Thiết Lập Thành Công", string.Concat(new string[]
					{
						"[",
						FrmMain.CurGame.TLBB.Name.ToUpper(),
						"]",
						this.checkautoskillf1.Checked ? " Bật " : " Tắt ",
						"đặt kỹ năng cơ bản vào F1"
					}), CanhBao.Kieu.OK);
				}
			}
		}

		// Token: 0x06000E03 RID: 3587 RVA: 0x00061831 File Offset: 0x0005FA31
		private void button6_Click(object sender, EventArgs e)
		{
			if (FrmMain.CurGame != null)
			{
				FrmMain.CurGame.Pass2 = this.txtmkkho.Text;
				FrmMain.CurGame.SavePass2();
				FrmMain.CurGame.UnlockPass2();
			}
		}

		// Token: 0x06000E04 RID: 3588 RVA: 0x00061864 File Offset: 0x0005FA64
		private void button5_Click(object sender, EventArgs e)
		{
			if (FrmMain.CurGame != null && !FrmMain.CurGame.TLBB.RaoTxt.Contains("INTERFACE") && !FrmMain.CurGame.TLBB.RaoTxt.Contains("mật mã động thái"))
			{
				this.txtnoidunggiaochat.Text = FrmMain.CurGame.TLBB.RaoTxt;
			}
		}

		// Token: 0x06000E05 RID: 3589 RVA: 0x000618C8 File Offset: 0x0005FAC8
		private void pictureBox11_Click(object sender, EventArgs e)
		{
			new Chat().Show();
		}

		// Token: 0x06000E06 RID: 3590 RVA: 0x000618D4 File Offset: 0x0005FAD4
		private void txtthoigian_TextChanged(object sender, EventArgs e)
		{
			if (FrmMain.CurGame != null)
			{
				try
				{
					FrmMain.CurGame.TimeGiaoChat = double.Parse(this.txtthoigian.Text);
				}
				catch
				{
					FrmMain.CurGame.TimeGiaoChat = 180.0;
					this.txtthoigian.Text = "180";
				}
			}
		}

		// Token: 0x06000E07 RID: 3591 RVA: 0x0006193C File Offset: 0x0005FB3C
		private void Checkthongbaochatmat_CheckedChanged(object sender, EventArgs e)
		{
			if (FrmMain.CurGame != null)
			{
				FrmMain.CurGame.AlarmChat = this.Checkthongbaochatmat.Checked;
				if (!this.VuaBatXong())
				{
					CanhBao.Msg("Thiết Lập Thành Công", string.Concat(new string[]
					{
						"[",
						FrmMain.CurGame.TLBB.Name.ToUpper(),
						"]",
						this.Checkthongbaochatmat.Checked ? " Bật " : " Tắt ",
						"thông báo chat mật"
					}), CanhBao.Kieu.OK);
				}
			}
		}

		// Token: 0x06000E08 RID: 3592 RVA: 0x000619D0 File Offset: 0x0005FBD0
		private void cboXuatPet_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (this.cboXuatPet.SelectedIndex == -1)
			{
				return;
			}
			if (FrmMain.CurGame != null)
			{
				try
				{
					Setting.SetValue(FrmMain.CurGame.TLBB.Id + "PET", ((int)(this.cboXuatPet.SelectedItem as FrmMain.ComboboxItem).Value).ToString("X8"));
					int num = int.Parse((this.cboXuatPet.SelectedItem as FrmMain.ComboboxItem).Value.ToString());
					FrmMain.CurGame.PetId = num.ToString("X8");
				}
				catch (Exception ex)
				{
					ex.ToString();
				}
			}
		}

		// Token: 0x06000E09 RID: 3593 RVA: 0x00061A8C File Offset: 0x0005FC8C
		private void numericUpDown3_ValueChanged(object sender, EventArgs e)
		{
			Global.PetLvl = (int)this.numericUpDown3.Value;
		}

		// Token: 0x06000E0A RID: 3594 RVA: 0x00061AA3 File Offset: 0x0005FCA3
		private void numuplevel_ValueChanged(object sender, EventArgs e)
		{
			Global.AutoUpLvlBelow = (int)this.numuplevel.Value;
		}

		// Token: 0x06000E0B RID: 3595 RVA: 0x00061ABA File Offset: 0x0005FCBA
		private void txtnoidunggiaochat_TextChanged(object sender, EventArgs e)
		{
			if (FrmMain.CurGame != null)
			{
				FrmMain.CurGame.RaoTxt = this.txtnoidunggiaochat.Text;
			}
		}

		// Token: 0x06000E0C RID: 3596 RVA: 0x00061AD8 File Offset: 0x0005FCD8
		private void thoátToolStripMenuItem1_Click(object sender, EventArgs e)
		{
			if (FrmMain.Leader != null)
			{
				FrmMain.Leader.IsLauLanTamBao = !FrmMain.Leader.IsLauLanTamBao;
				this.notifyIcon1.ShowBalloonTip(2000, "Thông Báo", string.Concat(new string[]
				{
					"[",
					FrmMain.CurGame.TLBB.Name.ToUpper(),
					"]",
					FrmMain.Leader.IsLauLanTamBao ? " Bật " : " Tắt ",
					"Auto Lâu Lan Tầm Bảo"
				}), ToolTipIcon.Info);
				return;
			}
			CanhBao.Msg("Lỗi Lâu Lan", "Cần chọn nhân vật đã có tổ đội", CanhBao.Kieu.Eror);
		}

		// Token: 0x06000E0D RID: 3597 RVA: 0x00061B82 File Offset: 0x0005FD82
		private void càiĐườngDẫnGameToolStripMenuItem_Click(object sender, EventArgs e)
		{
			this.SettingPath();
		}

		// Token: 0x06000E0E RID: 3598 RVA: 0x00061B8A File Offset: 0x0005FD8A
		private void butitemtuanhoan_Click(object sender, EventArgs e)
		{
			new AutoEatItem().Show();
		}

		// Token: 0x06000E0F RID: 3599 RVA: 0x00061B96 File Offset: 0x0005FD96
		private void buttheo_Click(object sender, EventArgs e)
		{
			this.notifyIcon1.ShowBalloonTip(2000, "Thông Báo", "Bật Theo Sau Key Cho Toàn Bộ AUTO", ToolTipIcon.Info);
			Global.FollowKey = true;
		}

		// Token: 0x06000E10 RID: 3600 RVA: 0x00061BB9 File Offset: 0x0005FDB9
		private void pictureBox3_Click(object sender, EventArgs e)
		{
			this.notifyIcon1.ShowBalloonTip(2000, "Thông Báo", "Tắt Theo Sau Key Cho Toàn Bộ Auto", ToolTipIcon.Info);
			Global.FollowKey = false;
		}

		// Token: 0x06000E11 RID: 3601 RVA: 0x00061BDC File Offset: 0x0005FDDC
		private void pictureBox6_Click(object sender, EventArgs e)
		{
			this.notifyIcon1.ShowBalloonTip(2000, "Thông Báo", "Bật Sử Dụng Skill Cho Toàn Bộ Auto", ToolTipIcon.Info);
			Global.UsingSkill = true;
		}

		// Token: 0x06000E12 RID: 3602 RVA: 0x00061BFF File Offset: 0x0005FDFF
		private void pictureBox5_Click(object sender, EventArgs e)
		{
			this.notifyIcon1.ShowBalloonTip(2000, "Thông Báo", "Tắt sử dụng Skill Cho Toàn Bộ Auto", ToolTipIcon.Info);
			Global.UsingSkill = false;
		}

		// Token: 0x06000E13 RID: 3603 RVA: 0x00061C24 File Offset: 0x0005FE24
		private void butpickall_Click(object sender, EventArgs e)
		{
			foreach (KeyValuePair<int, Game> keyValuePair in FrmMain.dicGame)
			{
				Game value = keyValuePair.Value;
				if (value != null)
				{
					value.IsPickItem = true;
				}
			}
			this.notifyIcon1.ShowBalloonTip(2000, "Thông Báo", "Bật Nhặt Vật Phẩm Cho Toàn Bộ Auto", ToolTipIcon.Info);
		}

		// Token: 0x06000E14 RID: 3604 RVA: 0x00061C9C File Offset: 0x0005FE9C
		private void unpickall_Click(object sender, EventArgs e)
		{
			foreach (KeyValuePair<int, Game> keyValuePair in FrmMain.dicGame)
			{
				Game value = keyValuePair.Value;
				if (value != null)
				{
					value.IsPickItem = false;
				}
			}
			this.notifyIcon1.ShowBalloonTip(2000, "Thông Báo", "Tắt Nhặt Vật Phẩm Cho Toàn Bộ Auto", ToolTipIcon.Info);
		}

		// Token: 0x06000E15 RID: 3605 RVA: 0x00061D14 File Offset: 0x0005FF14
		private void butdanhsachdongy_Click(object sender, EventArgs e)
		{
			if (FrmMain.CurGame != null)
			{
				new Buff(FrmMain.CurGame).Show();
			}
		}

		// Token: 0x06000E16 RID: 3606 RVA: 0x00061D2C File Offset: 0x0005FF2C
		private void checkBox12_CheckedChanged(object sender, EventArgs e)
		{
			if (FrmMain.CurGame != null)
			{
				FrmMain.CurGame.IsMuaNguyenLieu = !this.checkBox12.Checked;
			}
		}

		// Token: 0x06000E17 RID: 3607 RVA: 0x00061D4D File Offset: 0x0005FF4D
		private void numsonguyenlieu_ValueChanged(object sender, EventArgs e)
		{
			if (FrmMain.CurGame != null)
			{
				FrmMain.CurGame.SoLuongMua = (int)this.numsonguyenlieu.Value;
			}
		}

		// Token: 0x06000E18 RID: 3608 RVA: 0x00061D70 File Offset: 0x0005FF70
		private void numericUpDown4_ValueChanged(object sender, EventArgs e)
		{
			if (FrmMain.CurGame != null)
			{
				FrmMain.CurGame.SoLuongChe = (int)this.numericUpDown4.Value;
			}
		}

		// Token: 0x06000E19 RID: 3609 RVA: 0x00061D93 File Offset: 0x0005FF93
		private void Radnoi_CheckedChanged(object sender, EventArgs e)
		{
			if (FrmMain.CurGame != null)
			{
				FrmMain.CurGame.CheNoiNgoai = 1;
			}
		}

		// Token: 0x06000E1A RID: 3610 RVA: 0x00061DA7 File Offset: 0x0005FFA7
		private void radnoingoai_CheckedChanged(object sender, EventArgs e)
		{
			if (FrmMain.CurGame != null)
			{
				FrmMain.CurGame.CheNoiNgoai = 3;
			}
		}

		// Token: 0x06000E1B RID: 3611 RVA: 0x00061DBB File Offset: 0x0005FFBB
		private void radngoai_CheckedChanged(object sender, EventArgs e)
		{
			if (FrmMain.CurGame != null)
			{
				FrmMain.CurGame.CheNoiNgoai = 2;
			}
		}

		// Token: 0x06000E1C RID: 3612 RVA: 0x00061DCF File Offset: 0x0005FFCF
		private void checkBox13_CheckedChanged(object sender, EventArgs e)
		{
			if (FrmMain.CurGame != null)
			{
				FrmMain.CurGame.HuyNguyenLieu = this.checkBox13.Checked;
			}
		}

		// Token: 0x06000E1D RID: 3613 RVA: 0x00061DED File Offset: 0x0005FFED
		private void comboBox3_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (FrmMain.CurGame != null)
			{
				FrmMain.CurGame.TaskSauCheDO = this.comboBox3.SelectedItem.ToString();
			}
		}

		// Token: 0x06000E1E RID: 3614 RVA: 0x00061E10 File Offset: 0x00060010
		private void butche_Click(object sender, EventArgs e)
		{
			if (FrmMain.CurGame != null)
			{
				FrmMain.CurGame.DaChe = 0;
				FrmMain.CurGame.DaHuy = 0;
				FrmMain.CurGame.TocDoChe = 1;
				FrmMain.CurGame.TmpItemBeforeChe = FrmMain.CurGame.Packet.GetListIndex;
				FrmMain.CurGame.IsMuaNguyenLieu = !this.checkBox12.Checked;
				FrmMain.CurGame.SoLuongMua = (int)this.numsonguyenlieu.Value;
				FrmMain.CurGame.SoLuongChe = (int)this.numericUpDown4.Value;
				FrmMain.CurGame.HuyNguyenLieu = this.checkBox13.Checked;
				FrmMain.CurGame.TaskSauCheDO = this.comboBox3.SelectedItem.ToString();
				FrmMain.CurGame.TempCount = 0;
				FrmMain.CurGame.CheTen = this.comboloai.SelectedItem.ToString();
				FrmMain.CurGame.CheCap = this.comcapdtd.SelectedIndex;
				FrmMain.CurGame.CheLoai = this.comboloai.SelectedIndex;
				FrmMain.CurGame.IsCheDo = !FrmMain.CurGame.IsCheDo;
				FrmMain.CurGame.CheSao = (int)this.numsosao.Value;
				FrmMain.CurGame.CheDong = (int)this.numericUpDown5.Value;
				FrmMain.CurGame.CheDiem = (int)this.numericUpDown6.Value;
				FrmMain.CurGame.Kiemtranguyenlieu = FrmMain.CurGame.IsCheDo;
				FrmMain.CurGame.CheNoiNgoai = this.GetCheNoiNgaoi();
				if (FrmMain.CurGame.IsCheDo)
				{
					this.butche.Text = "Tắt";
				}
				else
				{
					this.butche.Text = "Chế";
				}
				CanhBao.Msg("Chế Đồ", string.Concat(new string[]
				{
					"[",
					FrmMain.CurGame.TLBB.Name.ToUpper(),
					"]",
					FrmMain.CurGame.IsCheDo ? " Bật " : " Tắt ",
					"Chế Đồ"
				}), CanhBao.Kieu.OK);
			}
		}

		// Token: 0x06000E1F RID: 3615 RVA: 0x00062042 File Offset: 0x00060242
		public int GetCheNoiNgaoi()
		{
			if (this.Radnoi.Checked)
			{
				return 1;
			}
			if (this.radnoingoai.Checked)
			{
				return 3;
			}
			if (this.radngoai.Checked)
			{
				return 2;
			}
			return 3;
		}

		// Token: 0x06000E20 RID: 3616 RVA: 0x00062072 File Offset: 0x00060272
		private void numsosao_ValueChanged(object sender, EventArgs e)
		{
			if (FrmMain.CurGame != null)
			{
				FrmMain.CurGame.CheSao = (int)this.numsosao.Value;
			}
		}

		// Token: 0x06000E21 RID: 3617 RVA: 0x00062095 File Offset: 0x00060295
		private void numericUpDown5_ValueChanged(object sender, EventArgs e)
		{
			if (FrmMain.CurGame != null)
			{
				FrmMain.CurGame.CheDong = (int)this.numericUpDown5.Value;
			}
		}

		// Token: 0x06000E22 RID: 3618 RVA: 0x000620B8 File Offset: 0x000602B8
		private void numericUpDown6_ValueChanged(object sender, EventArgs e)
		{
			if (FrmMain.CurGame != null)
			{
				FrmMain.CurGame.CheDiem = (int)this.numericUpDown6.Value;
			}
		}

		// Token: 0x06000E23 RID: 3619 RVA: 0x000620DC File Offset: 0x000602DC
		private void CheDoF5_Tick(object sender, EventArgs e)
		{
			if (FrmMain.CurGame != null && this.IsCheDoTab)
			{
				if (!FrmMain.CurGame.IsCheDo)
				{
					this.butche.Text = "Chế";
					return;
				}
				this.butche.Text = "Tắt";
				this.comboloai.SelectedIndex = FrmMain.CurGame.CheLoai;
				this.comcapdtd.SelectedIndex = FrmMain.CurGame.CheCap;
				VatLieu vatLieu = new VatLieu();
				vatLieu = FrmMain.CurGame.getsoluong(this.comboloai.SelectedItem.ToString(), this.comcapdtd.SelectedIndex + 1);
				this.txtbingan.Text = (vatLieu.BiNgan.ToString() ?? "");
				this.txttinhthiet.Text = (vatLieu.TinhThiet.ToString() ?? "");
				this.txtvaibong.Text = (vatLieu.VaiBong.ToString() ?? "");
				this.txttaodo.Text = (vatLieu.DaTaoDo.ToString() ?? "");
				this.txtdache.Text = (vatLieu.DaChe.ToString() ?? "");
				this.txtdahuy.Text = (vatLieu.DaHuy.ToString() ?? "");
				int daChe = vatLieu.DaChe;
				this.numsonguyenlieu.Value = FrmMain.CurGame.SoLuongMua;
				this.comboloai.SelectedIndex = FrmMain.CurGame.CheLoai;
				this.comcapdtd.SelectedIndex = FrmMain.CurGame.CheCap;
				this.numericUpDown4.Value = FrmMain.CurGame.SoLuongChe;
				this.checkBox12.Checked = !FrmMain.CurGame.IsMuaNguyenLieu;
				this.checkBox13.Checked = FrmMain.CurGame.HuyNguyenLieu;
				this.numericUpDown6.Value = FrmMain.CurGame.CheDiem;
				this.numericUpDown5.Value = FrmMain.CurGame.CheDong;
				this.numsosao.Value = FrmMain.CurGame.CheSao;
			}
		}

		// Token: 0x06000E24 RID: 3620 RVA: 0x00062331 File Offset: 0x00060531
		private void pictureBox2_Click(object sender, EventArgs e)
		{
			this.notifyIcon1.ShowBalloonTip(2000, "Thông Báo", "Bật xuất PET toàn bộ AUTO", ToolTipIcon.Info);
			Global.IsXuat = true;
		}

		// Token: 0x06000E25 RID: 3621 RVA: 0x00062354 File Offset: 0x00060554
		private void pictureBox1_Click(object sender, EventArgs e)
		{
			this.notifyIcon1.ShowBalloonTip(2000, "Thông Báo", "Tắt xuất PET toàn bộ AUTO", ToolTipIcon.Info);
			Global.IsXuat = false;
		}

		// Token: 0x06000E26 RID: 3622 RVA: 0x00062378 File Offset: 0x00060578
		protected override void WndProc(ref Message m)
		{
			if ((long)m.Msg == 74L)
			{
				FrmMain.COPYDATASTRUCT copydatastruct = (FrmMain.COPYDATASTRUCT)Marshal.PtrToStructure(m.LParam, typeof(FrmMain.COPYDATASTRUCT));
				byte[] array = new byte[copydatastruct.cbData];
				Marshal.Copy(copydatastruct.lpData, array, 0, copydatastruct.cbData);
				int num = -1;
				int num2 = -1;
				for (int i = 0; i < array.Length - 10; i++)
				{
					if (array[i] == 218 && array[i + 1] == 3 && array[i + 6] == 3)
					{
						num = i;
						break;
					}
					if (array[i] == 30 && array[i + 1] == 2 && array[i + 6] == 3)
					{
						num = i;
						break;
					}
					if (array.Length > 25 && array[i] == 218 && array[i + 1] == 3 && array[i + 6] == 4)
					{
						num2 = i;
						break;
					}
				}
				int num3 = BitConverter.ToInt32(array, 0);
				Game game = null;
				foreach (KeyValuePair<int, Game> keyValuePair in FrmMain.dicGame)
				{
					if (keyValuePair.Value.ProcessId == num3)
					{
						game = keyValuePair.Value;
					}
				}
				string str = "";
				if (array.Length > 25)
				{
					byte[] array2 = new byte[array.Length - 15];
					for (int j = 0; j < array2.Length; j++)
					{
						array2[j] = array[j + 15];
					}
					str = ConverterEx.VISCII2UnicodeEx(array2);
				}
				if (num2 != -1 && game != null)
				{
					if (TINHKIEM.VietLien(str).Contains("duongmon") && (TINHKIEM.VietLien(str).Contains("gianghotieutieu") || TINHKIEM.VietLien(str).Contains("#{qyxt_15}")))
					{
						game.AcBa = 37;
						game.IsAlarmAcBa = false;
					}
					if (TINHKIEM.VietLien(str).Contains("modung") && (TINHKIEM.VietLien(str).Contains("gianghotieutieu") || TINHKIEM.VietLien(str).Contains("#{qyxt_15}")))
					{
						game.AcBa = 32;
						game.IsAlarmAcBa = false;
					}
					if (TINHKIEM.VietLien(str).Contains("tinhtuc") && (TINHKIEM.VietLien(str).Contains("gianghotieutieu") || TINHKIEM.VietLien(str).Contains("#{qyxt_15}")))
					{
						game.AcBa = 6;
						game.IsAlarmAcBa = false;
					}
					if (TINHKIEM.VietLien(str).Contains("tieudao") && (TINHKIEM.VietLien(str).Contains("gianghotieutieu") || TINHKIEM.VietLien(str).Contains("#{qyxt_15}")))
					{
						game.AcBa = 9;
						game.IsAlarmAcBa = false;
					}
					if (TINHKIEM.VietLien(str).Contains("thieulam") && (TINHKIEM.VietLien(str).Contains("gianghotieutieu") || TINHKIEM.VietLien(str).Contains("#{qyxt_15}")))
					{
						game.AcBa = 1;
						game.IsAlarmAcBa = false;
					}
					if (TINHKIEM.VietLien(str).Contains("thienson") && (TINHKIEM.VietLien(str).Contains("gianghotieutieu") || TINHKIEM.VietLien(str).Contains("#{qyxt_15}")))
					{
						game.AcBa = 8;
						game.IsAlarmAcBa = false;
					}
					if (TINHKIEM.VietLien(str).Contains("thienlong") && (TINHKIEM.VietLien(str).Contains("gianghotieutieu") || TINHKIEM.VietLien(str).Contains("#{qyxt_15}")))
					{
						game.AcBa = 7;
						game.IsAlarmAcBa = false;
					}
					if (TINHKIEM.VietLien(str).Contains("ngamy") && (TINHKIEM.VietLien(str).Contains("gianghotieutieu") || TINHKIEM.VietLien(str).Contains("#{qyxt_15}")))
					{
						game.AcBa = 5;
						game.IsAlarmAcBa = false;
					}
					if (TINHKIEM.VietLien(str).Contains("vodang") && (TINHKIEM.VietLien(str).Contains("gianghotieutieu") || TINHKIEM.VietLien(str).Contains("#{qyxt_15}")))
					{
						game.AcBa = 4;
						game.IsAlarmAcBa = false;
					}
					if (TINHKIEM.VietLien(str).Contains("minhgiao") && (TINHKIEM.VietLien(str).Contains("gianghotieutieu") || TINHKIEM.VietLien(str).Contains("#{qyxt_15}")))
					{
						game.AcBa = 2;
						game.IsAlarmAcBa = false;
					}
					if (TINHKIEM.VietLien(str).Contains("caibang") && (TINHKIEM.VietLien(str).Contains("gianghotieutieu") || TINHKIEM.VietLien(str).Contains("#{qyxt_15}")))
					{
						game.AcBa = 3;
						game.IsAlarmAcBa = false;
					}
				}
				if (num != -1)
				{
					try
					{
						foreach (KeyValuePair<int, Game> keyValuePair2 in FrmMain.dicGame)
						{
							if (keyValuePair2.Value.ProcessId == num3)
							{
								byte[] array3;
								if (keyValuePair2.Value.Address.GameType == 1)
								{
									array3 = new byte[array.Length - num - 11];
									for (int k = num + 11; k < array.Length - 1; k++)
									{
										array3[k - (num + 11)] = array[k];
										if (array[k] < 32)
										{
											array3[k - (num + 11)] = 35;
										}
									}
								}
								else
								{
									array3 = new byte[array.Length - num - 8];
									for (int l = num + 8; l < array.Length - 1; l++)
									{
										array3[l - (num + 8)] = array[l];
										if (array[l] < 32)
										{
											array3[l - (num + 8)] = 35;
										}
									}
								}
								string text = string.Concat(new string[]
								{
									DateTime.Now.ToString("HH:mm dd-MM"),
									"#[",
									keyValuePair2.Value.TLBB.Name,
									"]:",
									ConverterEx.VISCII2Unicode(array3)
								});
								if (text.Split(new char[]
								{
									'#'
								}).Length > 2)
								{
									string text2 = "";
									int num4 = 2;
									while (num4 < text.Split(new char[]
									{
										'#'
									}).Length && num4 <= 3)
									{
										text2 += text.Split(new char[]
										{
											'#'
										})[num4];
										num4++;
									}
									text = string.Concat(new string[]
									{
										text.Split(new char[]
										{
											'#'
										})[0],
										"[",
										text2,
										"] nói thầm  ",
										text.Split(new char[]
										{
											'#'
										})[1]
									});
									if (text2.Length < 3)
									{
										text = "";
									}
								}
								if (text == "")
								{
									return;
								}
								try
								{
									using (SoundPlayer soundPlayer = new SoundPlayer("c:\\Windows\\Media\\tada.wav"))
									{
										soundPlayer.Play();
									}
								}
								catch
								{
								}
								FrmMain.AddLog(text + "\n");
							}
						}
					}
					catch
					{
					}
				}
			}
			if (m.Msg == 6 && m.WParam.ToInt32() == 1 && Control.FromHandle(m.LParam) == null)
			{
				base.WindowState = FormWindowState.Normal;
			}
			if (m.Msg == 786)
			{
				(int)m.LParam;
				(int)m.LParam;
				int num5 = m.WParam.ToInt32();
				if (num5 == 34 && FrmMain.CurGame != null)
				{
					FrmMain.CurGame.IsTriLieu = !FrmMain.CurGame.IsTriLieu;
					CanhBao.Msg("Thiết Lập Thành Công", string.Concat(new string[]
					{
						"[",
						FrmMain.CurGame.TLBB.Name.ToUpper(),
						"]",
						FrmMain.CurGame.IsTriLieu ? " Bật " : " Tắt ",
						"trị liệu!"
					}), CanhBao.Kieu.OK);
				}
				if (num5 == 18 && FrmMain.CurGame != null)
				{
					if (!FrmMain.CurGame.ishide)
					{
						FrmMain.CurGame.Hide();
						FrmMain.CurGame.ishide = true;
					}
					else
					{
						FrmMain.CurGame.Active();
						FrmMain.CurGame.ishide = false;
					}
					CanhBao.Msg("Thiết Lập Thành Công", string.Concat(new string[]
					{
						"[",
						FrmMain.CurGame.TLBB.Name.ToUpper(),
						"]",
						FrmMain.CurGame.ishide ? " Bật " : " Tắt ",
						"ẩn Game!"
					}), CanhBao.Kieu.OK);
				}
				if (num5 == 33 && FrmMain.CurGame != null)
				{
					FrmMain.CurGame.AutoTrain = !FrmMain.CurGame.AutoTrain;
					CanhBao.Msg("Thiết Lập Thành Công", string.Concat(new string[]
					{
						"[",
						FrmMain.CurGame.TLBB.Name.ToUpper(),
						"]",
						FrmMain.CurGame.AutoTrain ? " Bật " : " Tắt ",
						"Auto Train!"
					}), CanhBao.Kieu.OK);
				}
				if (num5 == 26 && FrmMain.CurGame != null)
				{
					Global.FollowKey = !Global.FollowKey;
					CanhBao.Msg("Thiết Lập Thành Công", string.Concat(new string[]
					{
						"[",
						FrmMain.CurGame.TLBB.Name.ToUpper(),
						"]",
						Global.FollowKey ? " Bật " : " Tắt ",
						"theo Key!"
					}), CanhBao.Kieu.OK);
				}
				if (num5 == 1)
				{
					FrmMain.CurGame.IsAuto = !FrmMain.CurGame.IsAuto;
					CanhBao.Msg("Thiết Lập Thành Công", string.Concat(new string[]
					{
						"[",
						FrmMain.CurGame.TLBB.Name.ToUpper(),
						"]",
						FrmMain.CurGame.IsAuto ? " Bật " : " Tắt ",
						"Auto!"
					}), CanhBao.Kieu.OK);
				}
				if (num5 == 6)
				{
					FrmMain.CurGame.IsPickItem = !FrmMain.CurGame.IsPickItem;
					CanhBao.Msg("Thiết Lập Thành Công", string.Concat(new string[]
					{
						"[",
						FrmMain.CurGame.TLBB.Name.ToUpper(),
						"]",
						FrmMain.CurGame.IsPickItem ? " Bật " : " Tắt ",
						"nhặt vật phẩm"
					}), CanhBao.Kieu.OK);
				}
				if (num5 == 17 && FrmMain.CurGame != null)
				{
					this.TrieuTapNhom();
					CanhBao.Msg("Thiết Lập Thành Công", "Bắt đầu triệu tập nhóm", CanhBao.Kieu.OK);
				}
				if (num5 == 28 && FrmMain.CurGame != null)
				{
					this.TrieuTap(FrmMain.CurGame);
					CanhBao.Msg("Thiết Lập Thành Công", "Bắt đầu triệu tập về nhân vật :" + FrmMain.CurGame.TLBB.Name, CanhBao.Kieu.OK);
				}
				if (num5 == 21 && FrmMain.CurGame != null)
				{
					FrmMain.CurGame.IsDropItem = !FrmMain.CurGame.IsDropItem;
					CanhBao.Msg("Thiết Lập Thành Công", string.Concat(new string[]
					{
						"[",
						FrmMain.CurGame.TLBB.Name.ToUpper(),
						"]",
						FrmMain.CurGame.IsDropItem ? " Bật " : " Tắt ",
						"hủy vật phẩm"
					}), CanhBao.Kieu.OK);
				}
				if (num5 == 23 && FrmMain.CurGame != null)
				{
					if (FrmMain.CurGame.IsRide)
					{
						FrmMain.CurGame.DownRide();
						CanhBao.Msg("Thao Tác Thành Công", "Xuống ngựa", CanhBao.Kieu.OK);
					}
					else
					{
						FrmMain.CurGame.Ride();
						CanhBao.Msg("Thao Tác Thành Công", "Lên Ngựa", CanhBao.Kieu.OK);
					}
				}
				if (num5 == 20 && FrmMain.CurGame != null)
				{
					FrmMain.CurGame.IsBank = !FrmMain.CurGame.IsBank;
					CanhBao.Msg("Bắt đầu đi cất đồ", string.Concat(new string[]
					{
						"[",
						FrmMain.CurGame.TLBB.Name.ToUpper(),
						"]",
						FrmMain.CurGame.IsBank ? " Bật " : " Tắt ",
						"cất đồ"
					}), CanhBao.Kieu.OK);
				}
				if (num5 == 27 && FrmMain.CurGame != null)
				{
					FrmMain.CurGame.Exit();
					CanhBao.Msg("Thành Công", "[" + FrmMain.CurGame.TLBB.Name.ToUpper() + "] thoát game", CanhBao.Kieu.OK);
				}
				if (num5 == 29 && FrmMain.CurGame != null && FrmMain.CurGame.TLBB.Name != "ĐăngNhập")
				{
					if (FrmMain.CurGame.TLBB.IsLeader)
					{
						CanhBao.Msg("Mời Đội", "Mời vào đội của :" + FrmMain.CurGame.TLBB.Name, CanhBao.Kieu.OK);
						this.MoiDoi(FrmMain.CurGame);
						return;
					}
					FrmMain.CurGame.LUA.PlayerCreateTeamSelf();
				}
				if (num5 == 25 && FrmMain.CurGame != null)
				{
					FrmMain.CurGame.IsSellItem = !FrmMain.CurGame.IsSellItem;
					CanhBao.Msg("Thiết Lập Thành Công", string.Concat(new string[]
					{
						"[",
						FrmMain.CurGame.TLBB.Name.ToUpper(),
						"]",
						FrmMain.CurGame.IsSellItem ? " Bật " : " Tắt ",
						"bán vật phẩm"
					}), CanhBao.Kieu.OK);
				}
				if (num5 == 35 && FrmMain.CurGame != null)
				{
					if (!FrmMain.CurGame.IsMapNghe())
					{
						CanhBao.Msg("Di Chuyển", "Vui lòng di chuyển tới bản đồ phù hợp", CanhBao.Kieu.Eror);
						return;
					}
					FrmMain.CurGame.IsDuoc = !FrmMain.CurGame.IsDuoc;
					CanhBao.Msg("Thiết Lập Thành Công", string.Concat(new string[]
					{
						"[",
						FrmMain.CurGame.TLBB.Name.ToUpper(),
						"]",
						FrmMain.CurGame.IsDuoc ? " Bật " : " Tắt ",
						"hái dược"
					}), CanhBao.Kieu.OK);
				}
				if (num5 == 13 && FrmMain.CurGame != null)
				{
					FrmMain.CurGame.UseSkill(22);
					CanhBao.Msg("Hành Động", "Quay về đại lý", CanhBao.Kieu.OK);
				}
				if (num5 == 39)
				{
					foreach (KeyValuePair<int, Game> keyValuePair3 in FrmMain.dicGame)
					{
						Game value = keyValuePair3.Value;
						value.IsAuto = !value.IsAuto;
					}
				}
				if (num5 == 24 && FrmMain.CurGame != null)
				{
					FrmMain.CurGame.IsMoBTD = !FrmMain.CurGame.IsMoBTD;
					CanhBao.Msg("Thiết Lập Thành Công", string.Concat(new string[]
					{
						"[",
						FrmMain.CurGame.TLBB.Name.ToUpper(),
						"]",
						FrmMain.CurGame.IsMoBTD ? " Bật " : " Tắt ",
						"mở tàng bảo đồ"
					}), CanhBao.Kieu.OK);
				}
				if (num5 == 32)
				{
					if (TienIch.GameCount() > 3)
					{
						CanhBao.Msg("Lỗi Mở Game", "Đã mở quá giới hạn Client cho phép", CanhBao.Kieu.Eror);
						return;
					}
					string text3 = LoadFile.LoadFileWithDecrypt(Global.DataPath + "\\ExecutePath.dat");
					try
					{
						Path.GetFileNameWithoutExtension(text3);
						string md = Offset.MD51.ToLower();
						Process.Start(new ProcessStartInfo
						{
							FileName = text3,
							Arguments = ".\\Bin\\Game.exe " + this.GetCMDBYMD5(md),
							WorkingDirectory = Path.GetDirectoryName(text3)
						});
					}
					catch (Exception)
					{
						this.SettingPath();
						return;
					}
				}
			}
			base.WndProc(ref m);
		}

		// Token: 0x06000E27 RID: 3623 RVA: 0x000633D8 File Offset: 0x000615D8
		private void MoiDoi(Game foreGame)
		{
			if (!foreGame.TLBB.Online)
			{
				return;
			}
			if (!foreGame.TLBB.IsLeader)
			{
				foreGame.LUA.PlayerCreateTeamSelf();
			}
			foreach (KeyValuePair<int, Game> keyValuePair in FrmMain.dicGame)
			{
				Game value = keyValuePair.Value;
				if (value != foreGame && value.TLBB.Online && value.Objects.Self != null && value.Objects.Self.PartyId == -1)
				{
					value.LuaDoUnicodeString("Friend:AskTeam(\"" + foreGame.TLBB.Name + "\");");
				}
			}
			foreGame.IsAcceptAll = true;
		}

		// Token: 0x06000E28 RID: 3624 RVA: 0x000634AC File Offset: 0x000616AC
		private void TrieuTap(Game foreGame)
		{
			if (foreGame == null)
			{
				return;
			}
			foreach (KeyValuePair<int, Game> keyValuePair in FrmMain.dicGame)
			{
				keyValuePair.Value.Move((float)((int)foreGame.CharX), (float)((int)foreGame.CharY), foreGame.TLBB.MapId);
			}
		}

		// Token: 0x06000E29 RID: 3625 RVA: 0x00063524 File Offset: 0x00061724
		private void TrieuTapNhom()
		{
			foreach (KeyValuePair<int, Game> keyValuePair in FrmMain.dicGame)
			{
				Game value = keyValuePair.Value;
				if (value.TLBB.IsLeader)
				{
					foreach (KeyValuePair<int, Game> keyValuePair2 in FrmMain.dicGame)
					{
						Game value2 = keyValuePair2.Value;
						if (value2 != value && value2.TLBB.Online && value2.TLBB.KeyId == value.TLBB.Id)
						{
							value2.Move((float)((int)value.CharX), (float)((int)value.CharY), value.TLBB.MapId);
						}
					}
				}
			}
		}

		// Token: 0x06000E2A RID: 3626 RVA: 0x00063624 File Offset: 0x00061824
		private void ListViewNhanVat_ItemChecked(object sender, ItemCheckedEventArgs e)
		{
			Game game = e.Item.Tag as Game;
			game.IsAuto = e.Item.Checked;
			if (!this.VuaBatXong())
			{
				this.notifyIcon1.ShowBalloonTip(2000, "Thông Báo", string.Concat(new string[]
				{
					"[",
					game.TLBB.Name.ToUpper(),
					"]",
					game.IsAuto ? " Bật " : " Tắt ",
					"Auto"
				}), ToolTipIcon.Info);
			}
			game.SaveSetting();
		}

		// Token: 0x06000E2B RID: 3627 RVA: 0x000636C4 File Offset: 0x000618C4
		private void comcapdtd_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (FrmMain.CurGame != null)
			{
				FrmMain.CurGame.CheCap = this.comcapdtd.SelectedIndex;
			}
		}

		// Token: 0x1700032A RID: 810
		// (get) Token: 0x06000E2C RID: 3628 RVA: 0x000636E2 File Offset: 0x000618E2
		public static Game Leader
		{
			get
			{
				if (FrmMain.CurGame == null)
				{
					return null;
				}
				if (FrmMain.CurGame.Leader == null)
				{
					return null;
				}
				return FrmMain.CurGame.Leader;
			}
		}

		// Token: 0x06000E2D RID: 3629 RVA: 0x00063705 File Offset: 0x00061905
		private void comboloai_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (FrmMain.CurGame != null)
			{
				FrmMain.CurGame.CheLoai = this.comboloai.SelectedIndex;
			}
		}

		// Token: 0x06000E2E RID: 3630 RVA: 0x00063724 File Offset: 0x00061924
		private void tựĐộngToolStripMenuItem_Click(object sender, EventArgs e)
		{
			foreach (KeyValuePair<int, Game> keyValuePair in FrmMain.dicGame)
			{
				keyValuePair.Value.IsTrieuTap = false;
			}
			if (FrmMain.Leader != null)
			{
				int[] array = new int[]
				{
					MAP.VoLuongSon,
					MAP.KinhHo,
					MAP.KiemCac,
					MAP.ThaiHo,
					MAP.TungSon,
					MAP.DonHoang
				};
				int num = new Random().Next(0, array.Length);
				FrmMain.Leader.MapAcTac = array[num];
				this.menuactac_Click(null, null);
				return;
			}
			CanhBao.Msg("Lỗi Ác Tặc", "Cần chọn nhân vật đã có tổ đội", CanhBao.Kieu.Eror);
		}

		// Token: 0x06000E2F RID: 3631 RVA: 0x000637F0 File Offset: 0x000619F0
		private void vôLượngSơnToolStripMenuItem_Click(object sender, EventArgs e)
		{
			foreach (KeyValuePair<int, Game> keyValuePair in FrmMain.dicGame)
			{
				keyValuePair.Value.IsTrieuTap = false;
			}
			if (FrmMain.Leader != null)
			{
				FrmMain.Leader.MapAcTac = MAP.VoLuongSon;
				return;
			}
			CanhBao.Msg("Lỗi Ác Tặc", "Cần chọn nhân vật đã có tổ đội", CanhBao.Kieu.Eror);
		}

		// Token: 0x06000E30 RID: 3632 RVA: 0x00063870 File Offset: 0x00061A70
		private void kínhHồToolStripMenuItem_Click(object sender, EventArgs e)
		{
			foreach (KeyValuePair<int, Game> keyValuePair in FrmMain.dicGame)
			{
				keyValuePair.Value.IsTrieuTap = false;
			}
			if (FrmMain.Leader != null)
			{
				FrmMain.Leader.MapAcTac = MAP.KinhHo;
				return;
			}
			CanhBao.Msg("Lỗi Ác Tặc", "Cần chọn nhân vật đã có tổ đội", CanhBao.Kieu.Eror);
		}

		// Token: 0x06000E31 RID: 3633 RVA: 0x000638F0 File Offset: 0x00061AF0
		private void kiếmCácToolStripMenuItem_Click(object sender, EventArgs e)
		{
			foreach (KeyValuePair<int, Game> keyValuePair in FrmMain.dicGame)
			{
				keyValuePair.Value.IsTrieuTap = false;
			}
			if (FrmMain.Leader != null)
			{
				FrmMain.Leader.MapAcTac = MAP.KiemCac;
				return;
			}
			CanhBao.Msg("Lỗi Ác Tặc", "Cần chọn nhân vật đã có tổ đội", CanhBao.Kieu.Eror);
		}

		// Token: 0x06000E32 RID: 3634 RVA: 0x00063970 File Offset: 0x00061B70
		private void tháiHồToolStripMenuItem_Click(object sender, EventArgs e)
		{
			foreach (KeyValuePair<int, Game> keyValuePair in FrmMain.dicGame)
			{
				keyValuePair.Value.IsTrieuTap = false;
			}
			if (FrmMain.Leader != null)
			{
				FrmMain.Leader.MapAcTac = MAP.ThaiHo;
				return;
			}
			CanhBao.Msg("Lỗi Ác Tặc", "Cần chọn nhân vật đã có tổ đội", CanhBao.Kieu.Eror);
		}

		// Token: 0x06000E33 RID: 3635 RVA: 0x000639F0 File Offset: 0x00061BF0
		private void tungSơnToolStripMenuItem_Click(object sender, EventArgs e)
		{
			foreach (KeyValuePair<int, Game> keyValuePair in FrmMain.dicGame)
			{
				keyValuePair.Value.IsTrieuTap = false;
			}
			if (FrmMain.Leader != null)
			{
				FrmMain.Leader.MapAcTac = MAP.TungSon;
				return;
			}
			CanhBao.Msg("Lỗi Ác Tặc", "Cần chọn nhân vật đã có tổ đội", CanhBao.Kieu.Eror);
		}

		// Token: 0x06000E34 RID: 3636 RVA: 0x00063A70 File Offset: 0x00061C70
		private void đônHoàngToolStripMenuItem_Click(object sender, EventArgs e)
		{
			foreach (KeyValuePair<int, Game> keyValuePair in FrmMain.dicGame)
			{
				keyValuePair.Value.IsTrieuTap = false;
			}
			if (FrmMain.Leader != null)
			{
				FrmMain.Leader.MapAcTac = MAP.DonHoang;
				return;
			}
			CanhBao.Msg("Lỗi Ác Tặc", "Cần chọn nhân vật đã có tổ đội", CanhBao.Kieu.Eror);
		}

		// Token: 0x06000E35 RID: 3637 RVA: 0x00063AF0 File Offset: 0x00061CF0
		public void CallALLAction()
		{
			if (FrmMain.Leader != null)
			{
				FrmMain.Leader.IsAcBa = false;
				FrmMain.Leader.IsTrungAc = false;
				FrmMain.Leader.IsLauLanTamBao = false;
				FrmMain.Leader.IsKyCuoc = false;
				FrmMain.Leader.IsThuyLao = false;
				FrmMain.Leader.MapAcTac = 0;
			}
		}

		// Token: 0x06000E36 RID: 3638 RVA: 0x00063B48 File Offset: 0x00061D48
		private void ItemAcBa_Click(object sender, EventArgs e)
		{
			if (FrmMain.Leader != null)
			{
				this.CallALLAction();
				FrmMain.Leader.IsAcBa = !FrmMain.Leader.IsAcBa;
				this.notifyIcon1.ShowBalloonTip(2000, "Thông Báo", string.Concat(new string[]
				{
					"[",
					FrmMain.CurGame.TLBB.Name.ToUpper(),
					"]",
					FrmMain.Leader.IsAcBa ? " Bật " : " Tắt ",
					"ÁC BÁ"
				}), ToolTipIcon.Info);
				return;
			}
			CanhBao.Msg("Lỗi Ác Bá", "Cần chọn nhân vật đã có tổ đội", CanhBao.Kieu.Eror);
		}

		// Token: 0x06000E37 RID: 3639 RVA: 0x00063BF8 File Offset: 0x00061DF8
		private void itemTranLongKyCuoc_Click(object sender, EventArgs e)
		{
			if (FrmMain.Leader != null)
			{
				FrmMain.Leader.IsKyCuoc = !FrmMain.Leader.IsKyCuoc;
				this.notifyIcon1.ShowBalloonTip(2000, "Thông Báo", string.Concat(new string[]
				{
					"[",
					FrmMain.CurGame.TLBB.Name.ToUpper(),
					"]",
					FrmMain.Leader.IsLauLanTamBao ? " Bật " : " Tắt ",
					"Auto Trân Long Kỳ Cuộc"
				}), ToolTipIcon.Info);
				return;
			}
			CanhBao.Msg("Lỗi Kỳ Cuộc", "Cần chọn nhân vật đã có tổ đội", CanhBao.Kieu.Eror);
		}

		// Token: 0x06000E38 RID: 3640 RVA: 0x00063CA2 File Offset: 0x00061EA2
		private void ItemThuyLao_Click(object sender, EventArgs e)
		{
			if (FrmMain.Leader != null)
			{
				FrmMain.Leader.IsThuyLao = false;
				CanhBao.Msg("Lỗi Thủy Lao", "Not work", CanhBao.Kieu.Eror);
				return;
			}
			CanhBao.Msg("Lỗi Thủy Lao", "Cần chọn nhân vật đã có tổ đội", CanhBao.Kieu.Eror);
		}

		// Token: 0x06000E39 RID: 3641 RVA: 0x00063CD8 File Offset: 0x00061ED8
		private void ItemTrungAc_Click(object sender, EventArgs e)
		{
			if (FrmMain.Leader != null)
			{
				FrmMain.Leader.IsTrungAc = !FrmMain.Leader.IsTrungAc;
				this.notifyIcon1.ShowBalloonTip(2000, "Thông Báo", string.Concat(new string[]
				{
					"[",
					FrmMain.CurGame.TLBB.Name.ToUpper(),
					"]",
					FrmMain.Leader.IsLauLanTamBao ? " Bật " : " Tắt ",
					"Auto Trừng Ác"
				}), ToolTipIcon.Info);
				return;
			}
			FrmMain.Leader.IsTrungAc = false;
			CanhBao.Msg("Lỗi Thủy Lao", "Cần chọn nhân vật đã có tổ đội", CanhBao.Kieu.Eror);
		}

		// Token: 0x06000E3A RID: 3642 RVA: 0x00063D90 File Offset: 0x00061F90
		private void chươngTrìnhToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if (FrmMain.CurGame == null)
			{
				this.itemchuacodoi.Text = "Vui lòng đăng nhập";
				this.menuactac.Enabled = false;
				this.ItemAcBa.Enabled = false;
				this.ItemLauLan.Enabled = false;
				this.itemTranLongKyCuoc.Enabled = false;
				this.ItemThuyLao.Enabled = false;
				return;
			}
			if (FrmMain.Leader == null)
			{
				this.itemchuacodoi.Text = "Chưa có đội";
				this.menuactac.Enabled = false;
				this.ItemAcBa.Enabled = false;
				this.ItemLauLan.Enabled = false;
				this.itemTranLongKyCuoc.Enabled = false;
				this.ItemThuyLao.Enabled = false;
				return;
			}
			this.itemchuacodoi.Text = "Đội Trưởng [ " + FrmMain.CurGame.Leader.TLBB.Name + "]";
			this.itemchuacodoi.ForeColor = Color.Green;
			this.menuactac.Enabled = true;
			this.ItemAcBa.Enabled = true;
			this.ItemLauLan.Enabled = true;
			this.itemTranLongKyCuoc.Enabled = true;
			this.ItemThuyLao.Enabled = true;
			if (FrmMain.Leader.IsAcBa)
			{
				this.ItemAcBa.Checked = true;
			}
			else
			{
				this.ItemAcBa.Checked = false;
			}
			if (FrmMain.Leader.IsLauLanTamBao)
			{
				this.ItemLauLan.Checked = true;
			}
			else
			{
				this.ItemLauLan.Checked = false;
			}
			if (FrmMain.Leader.IsKyCuoc)
			{
				this.itemTranLongKyCuoc.Checked = true;
			}
			else
			{
				this.itemTranLongKyCuoc.Checked = false;
			}
			if (FrmMain.Leader.IsThuyLao)
			{
				this.ItemThuyLao.Checked = true;
			}
			else
			{
				this.ItemThuyLao.Checked = false;
			}
			if (FrmMain.Leader.IsTrungAc)
			{
				this.ItemTrungAc.Checked = true;
				return;
			}
			this.ItemTrungAc.Checked = false;
		}

		// Token: 0x06000E3B RID: 3643 RVA: 0x00063F7F File Offset: 0x0006217F
		private void phímTắtToolStripMenuItem_Click(object sender, EventArgs e)
		{
			new HotKey().Show();
		}

		// Token: 0x06000E3C RID: 3644 RVA: 0x00063F8C File Offset: 0x0006218C
		private void menuactac_Click(object sender, EventArgs e)
		{
			if (FrmMain.CurGame == null)
			{
				return;
			}
			if (FrmMain.Leader.MapAcTac != 0)
			{
				if (FrmMain.Leader.MapAcTac == MAP.VoLuongSon)
				{
					this.UnCheckAllAcTac();
					this.vôLượngSơnToolStripMenuItem.Checked = true;
				}
				if (FrmMain.Leader.MapAcTac == MAP.KinhHo)
				{
					this.UnCheckAllAcTac();
					this.kínhHồToolStripMenuItem.Checked = true;
				}
				if (FrmMain.Leader.MapAcTac == MAP.KiemCac)
				{
					this.UnCheckAllAcTac();
					this.kiếmCácToolStripMenuItem.Checked = true;
				}
				if (FrmMain.Leader.MapAcTac == MAP.ThaiHo)
				{
					this.UnCheckAllAcTac();
					this.tháiHồToolStripMenuItem.Checked = true;
				}
				if (FrmMain.Leader.MapAcTac == MAP.TungSon)
				{
					this.UnCheckAllAcTac();
					this.tungSơnToolStripMenuItem.Checked = true;
				}
				if (FrmMain.Leader.MapAcTac == MAP.DonHoang)
				{
					this.UnCheckAllAcTac();
					this.đônHoàngToolStripMenuItem.Checked = true;
					return;
				}
			}
			else
			{
				this.UnCheckAllAcTac();
			}
		}

		// Token: 0x06000E3D RID: 3645 RVA: 0x0006408C File Offset: 0x0006228C
		public void UnCheckAllAcTac()
		{
			this.tựĐộngToolStripMenuItem.Checked = false;
			this.vôLượngSơnToolStripMenuItem.Checked = false;
			this.kínhHồToolStripMenuItem.Checked = false;
			this.kiếmCácToolStripMenuItem.Checked = false;
			this.tháiHồToolStripMenuItem.Checked = false;
			this.tungSơnToolStripMenuItem.Checked = false;
			this.đônHoàngToolStripMenuItem.Checked = false;
		}

		// Token: 0x06000E3E RID: 3646 RVA: 0x000640ED File Offset: 0x000622ED
		private void dUwngfToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if (FrmMain.CurGame != null)
			{
				FrmMain.Leader.MapAcTac = 0;
				CanhBao.Msg("Hủy Ác Tặc", "Hủy Ác Tặc Thành Công", CanhBao.Kieu.OK);
			}
		}

		// Token: 0x06000E3F RID: 3647 RVA: 0x00064114 File Offset: 0x00062314
		private void button1_Click(object sender, EventArgs e)
		{
			foreach (KeyValuePair<int, Game> keyValuePair in FrmMain.dicGame)
			{
				Game value = keyValuePair.Value;
				value.IsAuto = !value.IsAuto;
			}
		}

		// Token: 0x06000E40 RID: 3648 RVA: 0x00064174 File Offset: 0x00062374
		private void buttrieutap_Click(object sender, EventArgs e)
		{
			if (FrmMain.CurGame != null)
			{
				this.TrieuTapNhom();
				CanhBao.Msg("Thiết Lập Thành Công", "Bắt đầu triệu tập nhóm", CanhBao.Kieu.OK);
			}
		}

		// Token: 0x06000E41 RID: 3649 RVA: 0x00064193 File Offset: 0x00062393
		private void button2_Click(object sender, EventArgs e)
		{
			new Debug(FrmMain.CurGame).Show();
		}

		// Token: 0x06000E42 RID: 3650 RVA: 0x000641A4 File Offset: 0x000623A4
		private void ListViewNhanVat_MouseClick(object sender, MouseEventArgs e)
		{
			if (e.Button == MouseButtons.Right && this.ListViewNhanVat.FocusedItem.Bounds.Contains(e.Location))
			{
				this.contextMenuStrip1.Show(Cursor.Position);
			}
		}

		// Token: 0x06000E43 RID: 3651 RVA: 0x000641EE File Offset: 0x000623EE
		private void contextMenuStrip1_Opening(object sender, CancelEventArgs e)
		{
			if (FrmMain.CurGame != null)
			{
				this.itemresetauto.Text = "Rest Auto [" + FrmMain.CurGame.TLBB.Name + "]";
			}
		}

		// Token: 0x06000E44 RID: 3652 RVA: 0x00064220 File Offset: 0x00062420
		private void ẩnGameToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if (FrmMain.CurGame != null)
			{
				FrmMain.CurGame.Hide();
			}
		}

		// Token: 0x06000E45 RID: 3653 RVA: 0x00064233 File Offset: 0x00062433
		private void hiệnGameToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if (FrmMain.CurGame != null)
			{
				FrmMain.CurGame.Active();
			}
		}

		// Token: 0x1700032B RID: 811
		// (get) Token: 0x06000E46 RID: 3654 RVA: 0x00064248 File Offset: 0x00062448
		private List<Game> SelectedGames
		{
			get
			{
				List<Game> list = new List<Game>();
				foreach (object obj in this.ListViewNhanVat.SelectedItems)
				{
					Game item = ((ListViewItem)obj).Tag as Game;
					list.Add(item);
				}
				if (list.Count == 0)
				{
					foreach (object obj2 in this.ListViewNhanVat.Items)
					{
						Game item2 = ((ListViewItem)obj2).Tag as Game;
						list.Add(item2);
					}
				}
				return list;
			}
		}

		// Token: 0x06000E47 RID: 3655 RVA: 0x000642D8 File Offset: 0x000624D8
		private void itemresetauto_Click(object sender, EventArgs e)
		{
			foreach (Game game in this.SelectedGames)
			{
				game.UnHookRecv();
				FrmMain.dicGame.Remove(game.ProcessId);
				game.Item.Remove();
			}
		}

		// Token: 0x06000E48 RID: 3656 RVA: 0x00064348 File Offset: 0x00062548
		private void mởThêmGameToolStripMenuItem_Click(object sender, EventArgs e)
		{
			string text = LoadFile.LoadFileWithDecrypt(Global.DataPath + "\\ExecutePath.dat");
			try
			{
				Path.GetFileNameWithoutExtension(text);
				string md = Offset.MD51.ToLower();
				Process.Start(new ProcessStartInfo
				{
					FileName = text,
					Arguments = ".\\Bin\\Game.exe " + this.GetCMDBYMD5(md),
					WorkingDirectory = Path.GetDirectoryName(text)
				});
			}
			catch (Exception)
			{
				this.SettingPath();
			}
		}

		// Token: 0x06000E49 RID: 3657 RVA: 0x000643CC File Offset: 0x000625CC
		private void thiếtLậpAutoToolStripMenuItem_Click(object sender, EventArgs e)
		{
			new ThietLapAuto().Show();
		}

		// Token: 0x06000E4A RID: 3658 RVA: 0x000643D8 File Offset: 0x000625D8
		private void thôngTinCậpNhậtToolStripMenuItem_Click(object sender, EventArgs e)
		{
			new ChangeLogs().Show();
		}

		// Token: 0x06000E4B RID: 3659 RVA: 0x000643E4 File Offset: 0x000625E4
		private void button2_Click_1(object sender, EventArgs e)
		{
			string s = "Ðây là bµ gõ Cp1252, nhìn nó s¨ nhß thª này";
			MessageBox.Show(ConverterEx.VISCII2UnicodeEx(Encoding.Default.GetBytes(s)));
		}

		// Token: 0x06000E4C RID: 3660 RVA: 0x00064410 File Offset: 0x00062610
		private void button2_Click_2(object sender, EventArgs e)
		{
			string s = ConverterEx.Unicode2VISCII("Ỷ Thiên Đồ Long Kiếm");
			MessageBox.Show(ConverterEx.VISCII2UnicodeEx(Encoding.Default.GetBytes(s)));
		}

		// Token: 0x06000E4D RID: 3661 RVA: 0x00064440 File Offset: 0x00062640
		private void button4_Click(object sender, EventArgs e)
		{
			if (FrmMain.CurGame != null)
			{
				if (this.cboXuatPet.SelectedItem.ToString() == "Không Xuất")
				{
					FrmMain.CurGame.DoAction("PetSkill2_2");
					return;
				}
				FrmMain.CurGame.LuaDoOneLineString("XuatPet('" + FrmMain.CurGame.PetId + "')");
			}
		}

		// Token: 0x06000E4E RID: 3662 RVA: 0x000644A3 File Offset: 0x000626A3
		private void button2_Click_3(object sender, EventArgs e)
		{
			FrmMain.CurGame.Quit();
		}

		// Token: 0x06000E4F RID: 3663 RVA: 0x00064193 File Offset: 0x00062393
		private void button2_Click_4(object sender, EventArgs e)
		{
			new Debug(FrmMain.CurGame).Show();
		}

		// Token: 0x06000E50 RID: 3664 RVA: 0x00006740 File Offset: 0x00004940
		private void button2_Click_5(object sender, EventArgs e)
		{
		}

		// Token: 0x06000E51 RID: 3665 RVA: 0x00064193 File Offset: 0x00062393
		private void button2_Click_6(object sender, EventArgs e)
		{
			new Debug(FrmMain.CurGame).Show();
		}

		// Token: 0x06000E52 RID: 3666 RVA: 0x000644B0 File Offset: 0x000626B0
		public void SendData()
		{
			try
			{
				Dictionary<string, AutoReport> dictionary = new Dictionary<string, AutoReport>();
				PacketSend packetSend = new PacketSend();
				packetSend.HardwareID = Class95.String_0;
				foreach (KeyValuePair<int, Game> keyValuePair in FrmMain.dicGame.ToArray<KeyValuePair<int, Game>>())
				{
					AutoReport autoReport = Global.CreateFromGame(keyValuePair.Value);
					if (!dictionary.ContainsKey(autoReport.CharID))
					{
						dictionary.Add(autoReport.CharID, autoReport);
					}
				}
				packetSend.DanhSachGame = dictionary;
				PacketDef packet = new PacketDef();
				packet.IDPacket = 1000;
				packet.data = DataHelper.ObjectToBytes<PacketSend>(packetSend);
				new Thread(delegate()
				{
					this.SendData(DataHelper.ObjectToBytes<PacketDef>(packet));
				})
				{
					IsBackground = true
				}.Start();
			}
			catch
			{
			}
		}

		// Token: 0x06000E53 RID: 3667 RVA: 0x00006740 File Offset: 0x00004940
		private void ChacterReport_Tick(object sender, EventArgs e)
		{
		}

		// Token: 0x06000E54 RID: 3668 RVA: 0x0006459C File Offset: 0x0006279C
		public static byte[] BuildMessage(byte[] data)
		{
			byte[] array = DataHelper.MAHOA(data, "e9b3390206d8dfc5ffc9b09284c0bbde");
			byte[] bytes = BitConverter.GetBytes(array.Length);
			byte[] array2 = new byte[bytes.Length + array.Length];
			bytes.CopyTo(array2, 0);
			array.CopyTo(array2, bytes.Length);
			return array2;
		}

		// Token: 0x06000E55 RID: 3669 RVA: 0x000645E0 File Offset: 0x000627E0
		public void SendData(byte[] message)
		{
			if (this._SocketClient != null && this._SocketClient.connected)
			{
				this._SocketClient.Send(FrmMain.BuildMessage(message));
				return;
			}
			if (!this.ServerConnect.IsBusy)
			{
				this.ServerConnect.RunWorkerAsync();
			}
		}

		// Token: 0x06000E56 RID: 3670 RVA: 0x00006740 File Offset: 0x00004940
		private void ServerConnect_DoWork(object sender, DoWorkEventArgs e)
		{
		}

		// Token: 0x06000E57 RID: 3671 RVA: 0x0006462C File Offset: 0x0006282C
		private void thôngTinAUTOToolStripMenuItem_Click(object sender, EventArgs e)
		{
			new ThongQR().Show();
		}

		// Token: 0x06000E58 RID: 3672 RVA: 0x00006740 File Offset: 0x00004940
		private void groupBox8_Enter(object sender, EventArgs e)
		{
		}

		// Token: 0x04000AB9 RID: 2745
		public static int MaxHoaX = 21;

		// Token: 0x04000ABA RID: 2746
		public static RichTextBox TxtLog;

		// Token: 0x04000ABB RID: 2747
		public static DateTime startprogram = DateTime.Now;

		// Token: 0x04000ABD RID: 2749
		public static bool TrimRam = false;

		// Token: 0x04000ABE RID: 2750
		public static Dictionary<int, Game> dicGame = new Dictionary<int, Game>();

		// Token: 0x04000ABF RID: 2751
		public static int DiemDanhIndex = -1;

		// Token: 0x04000AC1 RID: 2753
		public static string AllCurGameTrueID = string.Empty;

		// Token: 0x04000AC6 RID: 2758
		public static ListView ListView;

		// Token: 0x04000AC7 RID: 2759
		public static ListViewItem CurItem;

		// Token: 0x04000AC8 RID: 2760
		private static FrmMain.ChangeWindowMessageFilterDelegate ChangeWindowMessageFilter;

		// Token: 0x04000AC9 RID: 2761
		public static List<Account> ListAutoLogin = new List<Account>();

		// Token: 0x04000ACB RID: 2763
		public static bool IsLoged = false;

		// Token: 0x04000ACC RID: 2764
		private Thread ThreadMonitor;

		// Token: 0x04000ACE RID: 2766
		private Stopwatch timeAuto = Stopwatch.StartNew();

		// Token: 0x04000AD0 RID: 2768
		public Memory Memory;

		// Token: 0x04000AD2 RID: 2770
		public static int MaxGame = 0;

		// Token: 0x04000AD3 RID: 2771
		public static Game CurGame;

		// Token: 0x04000AD6 RID: 2774
		public static Dictionary<string, string> Captchas = new Dictionary<string, string>();

		// Token: 0x04000AD9 RID: 2777
		private bool IsPop = true;

		// Token: 0x04000ADA RID: 2778
		private bool Running;

		// Token: 0x04000ADB RID: 2779
		public static Dictionary<string, string> Answers = new Dictionary<string, string>();

		// Token: 0x04000ADC RID: 2780
		public bool IsLoginTab;

		// Token: 0x04000ADD RID: 2781
		public bool IsCheDoTab;

		// Token: 0x02000190 RID: 400
		// (Invoke) Token: 0x060011C6 RID: 4550
		public delegate void LogBack(string log);

		// Token: 0x02000191 RID: 401
		// (Invoke) Token: 0x060011CA RID: 4554
		private delegate int ChangeWindowMessageFilterDelegate(uint msg, int flag);

		// Token: 0x02000192 RID: 402
		// (Invoke) Token: 0x060011CE RID: 4558
		public delegate void CallBack(Game game);

		// Token: 0x02000193 RID: 403
		public class ComboboxItem
		{
			// Token: 0x1700040E RID: 1038
			// (get) Token: 0x060011D1 RID: 4561 RVA: 0x000785E5 File Offset: 0x000767E5
			// (set) Token: 0x060011D2 RID: 4562 RVA: 0x000785ED File Offset: 0x000767ED
			public string Text { get; set; }

			// Token: 0x1700040F RID: 1039
			// (get) Token: 0x060011D3 RID: 4563 RVA: 0x000785F6 File Offset: 0x000767F6
			// (set) Token: 0x060011D4 RID: 4564 RVA: 0x000785FE File Offset: 0x000767FE
			public object Value { get; set; }

			// Token: 0x060011D5 RID: 4565 RVA: 0x00078607 File Offset: 0x00076807
			public override string ToString()
			{
				return this.Text;
			}
		}

		// Token: 0x02000194 RID: 404
		private struct COPYDATASTRUCT
		{
			// Token: 0x04000F19 RID: 3865
			public IntPtr dwData;

			// Token: 0x04000F1A RID: 3866
			public int cbData;

			// Token: 0x04000F1B RID: 3867
			public IntPtr lpData;
		}
	}
}
