using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using TinhKiemAuto.AutoControl;
using TinhKiemAuto.Models;

namespace TinhKiemAuto
{
	// Token: 0x02000098 RID: 152
	internal class Global
	{
		// Token: 0x17000236 RID: 566
		// (get) Token: 0x06000972 RID: 2418 RVA: 0x0003F565 File Offset: 0x0003D765
		public static string Version
		{
			get
			{
				return "107";
			}
		}

		// Token: 0x17000237 RID: 567
		// (get) Token: 0x06000973 RID: 2419 RVA: 0x0003F56C File Offset: 0x0003D76C
		// (set) Token: 0x06000974 RID: 2420 RVA: 0x0003F573 File Offset: 0x0003D773
		public static int YearExp { get; set; }

		// Token: 0x06000975 RID: 2421 RVA: 0x0003F57C File Offset: 0x0003D77C
		public static void SetInfo(AutoReport autoreport)
		{
			foreach (KeyValuePair<int, Game> keyValuePair in FrmMain.dicGame)
			{
				if (autoreport.CharID == keyValuePair.Value.TLBB.Id)
				{
					BaiTrain baiTrain = new BaiTrain();
					baiTrain.Level = 40;
					baiTrain.MapID = autoreport.LenBai.MapID;
					baiTrain.MapName = autoreport.LenBai.MapName;
					baiTrain.Name = "Bãi tùy chỉnh";
					baiTrain.PosX = autoreport.LenBai.PosX;
					baiTrain.PosY = autoreport.LenBai.PosY;
					keyValuePair.Value._baitrain = baiTrain;
					keyValuePair.Value.LenBaiTrain = autoreport.IsLenBai;
					keyValuePair.Value.IsAttack = autoreport.IsAttack;
				}
			}
		}

		// Token: 0x06000976 RID: 2422 RVA: 0x0003F67C File Offset: 0x0003D87C
		public static AutoReport CreateFromGame(Game game)
		{
			AutoReport autoReport = new AutoReport();
			autoReport.ExpPercent = game.TLBB.ExpPercent;
			autoReport.Gold = game.TLBB.Gold;
			autoReport.HpPercent = game.TLBB.HPPercent;
			autoReport.IsAttack = game.IsAttack;
			autoReport.IsDisconnect = game.TLBB.Disconnected;
			autoReport.IsDuoc = game.IsDuoc;
			autoReport.IsLear = game.TLBB.IsLeader;
			autoReport.IsPickItem = game.IsPickItem;
			autoReport.IsRide = game.IsRide;
			autoReport.IsX25 = game.TuAnX2;
			autoReport.MapIndex = game.TLBB.MapId;
			autoReport.MapName = game.TLBB.MapName;
			autoReport.MpPercent = game.TLBB.MPPercent;
			autoReport.Msg = "";
			autoReport.Online = game.TLBB.Online;
			autoReport.PetPercent = game.TLBB.PetHPPercent;
			autoReport.PlayState = game.TLBB.PlayerState;
			autoReport.PosX = (int)game.CharX;
			autoReport.PosY = (int)game.CharY;
			CheDo cheDo = new CheDo();
			cheDo.CheCap = game.CheCap;
			cheDo.CheLoai = game.CheLoai;
			cheDo.IsCheDo = game.IsCheDo;
			cheDo.IsMienPhiNguyenLieu = !game.IsMuaNguyenLieu;
			cheDo.TongNhan = game.SoLuongMua;
			cheDo.TotalChe = game.SoLuongChe;
			cheDo.CheDiem = game.CheDiem;
			cheDo.CheDong = game.CheDong;
			cheDo.CheSao = game.CheSao;
			autoReport.IsLenBai = game.LenBaiTrain;
			autoReport.CheDo = cheDo;
			autoReport.LenBai = new LenBai
			{
				MapID = game._baitrain.MapID,
				MapName = game._baitrain.MapName,
				PosX = game._baitrain.PosX,
				PosY = game._baitrain.PosY
			};
			autoReport.CharID = game.TLBB.Id;
			autoReport.CharName = game.TLBB.Name;
			autoReport.Level = game.TLBB.Lvl;
			autoReport.GuildName = game.TLBB.GuildName;
			autoReport.GuildId = (game.TLBB.GuildId.ToString() ?? "");
			autoReport.Phai = game.TLBB.MenpaiName;
			OverView overView = new OverView();
			if (game.TLBB.IsNoi)
			{
				overView.PhamViDanh = Global.NoiRadius;
			}
			else
			{
				overView.PhamViDanh = Global.NgoaiRadius;
			}
			TotalSkill totalSkill = new TotalSkill();
			List<SkillAuto> list = new List<SkillAuto>();
			List<SkillAuto> list2 = new List<SkillAuto>();
			foreach (Skill skill in game.Skills.ToArray())
			{
				if (Skill.IsBuffSkill(skill.PacketId))
				{
					list2.Add(new SkillAuto
					{
						Id = skill.PacketId,
						IsUsing = skill.UserBuff,
						Name = skill.Name
					});
				}
				else
				{
					list.Add(new SkillAuto
					{
						Id = skill.PacketId,
						IsUsing = skill.Use,
						Name = skill.Name
					});
				}
			}
			Features features = new Features();
			features.IsAcceptParty = Global.AutoAccept;
			features.IsAcceptAllPartyInvites = Global.AcceptAll;
			features.IsUseSkillF1 = Option.PutBase;
			features.LimitLevelUp = Global.AutoUpLvlBelow;
			features.MakeAdvertisement = game.IsRao;
			features.MakeAdvertisingTime = (int)game.TimeGiaoChat;
			features.NoticePrivateMessage = game.AlarmChat;
			features.Pass2 = game.Pass2;
			features.ChatMSG = game.RaoTxt;
			features.Chanel = 1;
			features.FollowRadius = Global.FollowRadius;
			TotalPet totalPet = new TotalPet();
			List<PetAutoInfo> list3 = new List<PetAutoInfo>();
			totalPet.AutoCallBackAtLevel = Global.PetLvl;
			totalPet.IsAutoBuffPet = Global.BuffPet;
			totalPet.IsAutoCallBackPet = game.AutoThuPet;
			totalPet.IsAutoCallPet = Global.IsXuat;
			totalPet.IsAutoTakeCare = game.IsPet;
			totalPet.IsAutoUsePetSkill = Global.UseSkillPet;
			totalPet.PetInfo = list3;
			int num = 0;
			foreach (KeyValuePair<int, string> keyValuePair in game.TLBB.DicPet.ToArray<KeyValuePair<int, string>>())
			{
				PetAutoInfo petAutoInfo = new PetAutoInfo();
				petAutoInfo.Id = keyValuePair.Key;
				petAutoInfo.Name = keyValuePair.Value;
				petAutoInfo.Pos = num;
				num++;
				list3.Add(petAutoInfo);
			}
			totalSkill.ActiveSkill = list;
			totalSkill.PassiveSkill = list2;
			overView.IsDanhQuanhDiem = game.IsRadius;
			overView.IsGomQuai = game.IsLure;
			overView.IsUsingThoLinhChau = game.UsingTholinhChau;
			overView.IsRengeHP = game.IsHP;
			overView.RengeHPPercent = Global.BuffHPPercent;
			overView.IsRengeMP = game.IsMP;
			overView.RengeMPPercent = Global.BuffMPPercent;
			overView.IsCongSinh = game.CongSinh;
			overView.CongSinhValue = game.CongSinhValue;
			overView.IsHuyetTe = game.HuyetTe;
			overView.HuyetTeValue = game.HuyetTeValue;
			overView.IsNM = game.IsNM;
			overView.BuffNMPercent = Global.BuffNMPercent;
			overView.IsAutoReborn = game.AutoHoiSinh;
			overView.IsAutoComeBack = Global.AutoComeBack;
			overView.isArletHP = Global.AlarmHP;
			overView.ArletHPPercent = Global.AlarmHPPercent;
			autoReport.OverView = overView;
			autoReport.Pets = totalPet;
			autoReport.Skills = totalSkill;
			autoReport.Features = features;
			return autoReport;
		}

		// Token: 0x17000238 RID: 568
		// (get) Token: 0x06000977 RID: 2423 RVA: 0x0003FC3D File Offset: 0x0003DE3D
		// (set) Token: 0x06000978 RID: 2424 RVA: 0x0003FC44 File Offset: 0x0003DE44
		public static bool IsVutRac { get; set; }

		// Token: 0x17000239 RID: 569
		// (get) Token: 0x06000979 RID: 2425 RVA: 0x00008C9D File Offset: 0x00006E9D
		public static bool IsAdmin
		{
			get
			{
				return false;
			}
		}

		// Token: 0x1700023A RID: 570
		// (get) Token: 0x0600097A RID: 2426 RVA: 0x0003FC4C File Offset: 0x0003DE4C
		public static bool IsFix
		{
			get
			{
				return Setting.Read("User", "Fix") == "True";
			}
		}

		// Token: 0x1700023B RID: 571
		// (get) Token: 0x0600097B RID: 2427 RVA: 0x0003FC67 File Offset: 0x0003DE67
		public static bool IsMulti
		{
			get
			{
				return Setting.Read("User", "Multi") == "True";
			}
		}

		// Token: 0x1700023C RID: 572
		// (get) Token: 0x0600097C RID: 2428 RVA: 0x0003FC82 File Offset: 0x0003DE82
		// (set) Token: 0x0600097D RID: 2429 RVA: 0x0003FC89 File Offset: 0x0003DE89
		public static bool IsXuat { get; set; }

		// Token: 0x1700023D RID: 573
		// (get) Token: 0x0600097E RID: 2430 RVA: 0x0003FC91 File Offset: 0x0003DE91
		// (set) Token: 0x0600097F RID: 2431 RVA: 0x0003FC98 File Offset: 0x0003DE98
		public static int TimeLive { get; set; }

		// Token: 0x1700023E RID: 574
		// (get) Token: 0x06000980 RID: 2432 RVA: 0x0003FCA0 File Offset: 0x0003DEA0
		// (set) Token: 0x06000981 RID: 2433 RVA: 0x0003FCA7 File Offset: 0x0003DEA7
		public static TINHKIEM.Menpai GlSetMenPai { get; set; }

		// Token: 0x1700023F RID: 575
		// (get) Token: 0x06000982 RID: 2434 RVA: 0x0003FCAF File Offset: 0x0003DEAF
		// (set) Token: 0x06000983 RID: 2435 RVA: 0x0003FCB6 File Offset: 0x0003DEB6
		public static bool GlIsSetMenPai { get; set; }

		// Token: 0x17000240 RID: 576
		// (get) Token: 0x06000984 RID: 2436 RVA: 0x0003FCBE File Offset: 0x0003DEBE
		// (set) Token: 0x06000985 RID: 2437 RVA: 0x0003FCC5 File Offset: 0x0003DEC5
		public static int IsVIP { get; set; }

		// Token: 0x17000241 RID: 577
		// (get) Token: 0x06000986 RID: 2438 RVA: 0x0003FCCD File Offset: 0x0003DECD
		// (set) Token: 0x06000987 RID: 2439 RVA: 0x0003FCD4 File Offset: 0x0003DED4
		public static bool RemoveAd { get; set; }

		// Token: 0x17000242 RID: 578
		// (get) Token: 0x06000988 RID: 2440 RVA: 0x0003FCDC File Offset: 0x0003DEDC
		public static string SelfMd5
		{
			get
			{
				return TINHKIEM.Hasher.MD5(Application.ExecutablePath);
			}
		}

		// Token: 0x17000243 RID: 579
		// (get) Token: 0x06000989 RID: 2441 RVA: 0x0003FCE8 File Offset: 0x0003DEE8
		public static string DataPath
		{
			get
			{
				string text = Global.APPPath + "\\Data";
				if (!Directory.Exists(text))
				{
					Directory.CreateDirectory(text);
				}
				return text;
			}
		}

		// Token: 0x17000244 RID: 580
		// (get) Token: 0x0600098A RID: 2442 RVA: 0x0003FD18 File Offset: 0x0003DF18
		public static string LogPath
		{
			get
			{
				string text = Global.APPPath + "\\Logs";
				if (!Directory.Exists(text))
				{
					Directory.CreateDirectory(text);
				}
				return text;
			}
		}

		// Token: 0x17000245 RID: 581
		// (get) Token: 0x0600098B RID: 2443 RVA: 0x0003FD48 File Offset: 0x0003DF48
		public static string CalenderPath
		{
			get
			{
				string text = Global.APPPath + "\\Config\\Calender";
				if (!Directory.Exists(text))
				{
					Directory.CreateDirectory(text);
				}
				return text;
			}
		}

		// Token: 0x17000246 RID: 582
		// (get) Token: 0x0600098C RID: 2444 RVA: 0x0003FD78 File Offset: 0x0003DF78
		public static string NhatHaPath
		{
			get
			{
				string text = Global.APPPath + "\\Config\\NhatHaPath";
				if (!Directory.Exists(text))
				{
					Directory.CreateDirectory(text);
				}
				return text;
			}
		}

		// Token: 0x17000247 RID: 583
		// (get) Token: 0x0600098D RID: 2445 RVA: 0x0003FDA5 File Offset: 0x0003DFA5
		public static string APPPath
		{
			get
			{
				return Path.GetDirectoryName(Application.ExecutablePath);
			}
		}

		// Token: 0x17000248 RID: 584
		// (get) Token: 0x0600098E RID: 2446 RVA: 0x0003FDB1 File Offset: 0x0003DFB1
		public static string Argument
		{
			get
			{
				return "\"" + Application.ExecutablePath + "\"";
			}
		}

		// Token: 0x0400065B RID: 1627
		public static string UpdateURL = "http://update.chickenauto.com/PatchInfoEx.ini";

		// Token: 0x04000660 RID: 1632
		public static bool LanLuot = true;

		// Token: 0x04000665 RID: 1637
		public static DateTime ExaclyTime = DateTime.MinValue;

		// Token: 0x04000666 RID: 1638
		public static int Delay = 50;

		// Token: 0x04000667 RID: 1639
		public static bool IsTrimRam = true;

		// Token: 0x04000668 RID: 1640
		public static int PetLvl = 125;

		// Token: 0x04000669 RID: 1641
		public static int Wait = 2;

		// Token: 0x0400066A RID: 1642
		public static bool HideBHD = true;

		// Token: 0x0400066B RID: 1643
		public static int IsFull = 5000;

		// Token: 0x0400066C RID: 1644
		public static bool ssss;

		// Token: 0x0400066D RID: 1645
		public static int HookMessage;

		// Token: 0x0400066E RID: 1646
		public static bool BuffPet = true;

		// Token: 0x0400066F RID: 1647
		public static bool ItemFillter = true;

		// Token: 0x04000670 RID: 1648
		public static bool AtkFollowKey = false;

		// Token: 0x04000671 RID: 1649
		public static int BuffHPPercent = 50;

		// Token: 0x04000672 RID: 1650
		public static int BuffMPPercent = 50;

		// Token: 0x04000673 RID: 1651
		public static int BuffNMPercent = 75;

		// Token: 0x04000674 RID: 1652
		public static int ExitHPPercent = 10;

		// Token: 0x04000675 RID: 1653
		public static int NoiRadius = 20;

		// Token: 0x04000676 RID: 1654
		public static int NgoaiRadius = 15;

		// Token: 0x04000677 RID: 1655
		public static int PickRadius = 30;

		// Token: 0x04000678 RID: 1656
		public static bool ExitPk = false;

		// Token: 0x04000679 RID: 1657
		public static bool AlarmPk = true;

		// Token: 0x0400067A RID: 1658
		public static bool AlarmHP = true;

		// Token: 0x0400067B RID: 1659
		public static int AlarmHPPercent = 30;

		// Token: 0x0400067C RID: 1660
		public static bool Mute = false;

		// Token: 0x0400067D RID: 1661
		public static bool AutoComeBack = false;

		// Token: 0x0400067E RID: 1662
		public static bool PickItem = true;

		// Token: 0x0400067F RID: 1663
		public static bool AutoUpLvl = false;

		// Token: 0x04000680 RID: 1664
		public static bool AutoDropItem = false;

		// Token: 0x04000681 RID: 1665
		public static bool AutoSellItem = false;

		// Token: 0x04000682 RID: 1666
		public static bool FollowKey = false;

		// Token: 0x04000683 RID: 1667
		public static bool UsingSkill = true;

		// Token: 0x04000684 RID: 1668
		public static int AutoUpLvlBelow = 20;

		// Token: 0x04000685 RID: 1669
		public static bool AutoShutDown = false;

		// Token: 0x04000686 RID: 1670
		public static bool UseSkillPet = true;

		// Token: 0x04000687 RID: 1671
		public static bool IsBoQua = false;

		// Token: 0x04000688 RID: 1672
		public static bool AutoResetTime = true;

		// Token: 0x04000689 RID: 1673
		public static bool BuffQuanDoan = false;

		// Token: 0x0400068A RID: 1674
		public static bool AutoAccept = true;

		// Token: 0x0400068B RID: 1675
		public static bool AcceptAll = true;

		// Token: 0x0400068C RID: 1676
		public static bool IsAcTac = false;

		// Token: 0x0400068D RID: 1677
		public static bool Paused = false;

		// Token: 0x0400068E RID: 1678
		public static bool IsXaPhu = true;

		// Token: 0x0400068F RID: 1679
		public static Keys BaseSkill = Keys.F1;

		// Token: 0x04000690 RID: 1680
		public static Keys NMSkill = Keys.F13;

		// Token: 0x04000691 RID: 1681
		public static Keys HPKey = Keys.F13;

		// Token: 0x04000692 RID: 1682
		public static bool IsHuyDanhQuai = false;

		// Token: 0x04000693 RID: 1683
		public static bool IsHuyThaiHo = false;

		// Token: 0x04000694 RID: 1684
		public static bool IsHyHuu = false;

		// Token: 0x04000695 RID: 1685
		public static int Speed = 3;

		// Token: 0x04000696 RID: 1686
		public static int FollowRadius = 3;

		// Token: 0x04000697 RID: 1687
		public static int BHDCount = 20;

		// Token: 0x04000698 RID: 1688
		public static bool AntiCaptcha = true;

		// Token: 0x04000699 RID: 1689
		public static bool AntiCaptchaSelf = false;

		// Token: 0x0400069A RID: 1690
		public static bool AutoPk = true;

		// Token: 0x0400069B RID: 1691
		public static int MaxBHD = 6;

		// Token: 0x0400069C RID: 1692
		public static bool IsTuVaoPhai = false;

		// Token: 0x0400069D RID: 1693
		public static int XDua = 134;

		// Token: 0x0400069E RID: 1694
		public static bool openbang = false;

		// Token: 0x0400069F RID: 1695
		public static int NumNhiemVuDua = 1;

		// Token: 0x040006A0 RID: 1696
		public static bool Mapbang = false;

		// Token: 0x040006A1 RID: 1697
		public static int YDua = 165;

		// Token: 0x040006A2 RID: 1698
		public static int MapDua = MAP.TayHo;

		// Token: 0x040006A3 RID: 1699
		public static List<string> BangOnPC = new List<string>();

		// Token: 0x040006A4 RID: 1700
		public static List<string> DoNgonDua = new List<string>();

		// Token: 0x040006A5 RID: 1701
		public static bool IsKhacMay = true;
	}
}
