using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows.Forms;
using Newtonsoft.Json;
using TinhKiemAuto.Controllers;
using TinhKiemAuto.CostumeControlner;
using TinhKiemAuto.Models;

namespace TinhKiemAuto
{
	// Token: 0x02000092 RID: 146
	public class Game
	{
		// Token: 0x17000134 RID: 308
		// (get) Token: 0x06000678 RID: 1656 RVA: 0x00024EF8 File Offset: 0x000230F8
		// (set) Token: 0x06000679 RID: 1657 RVA: 0x00024F00 File Offset: 0x00023100
		public int Define { get; set; }

		// Token: 0x17000135 RID: 309
		// (get) Token: 0x0600067A RID: 1658 RVA: 0x00024F09 File Offset: 0x00023109
		// (set) Token: 0x0600067B RID: 1659 RVA: 0x00024F11 File Offset: 0x00023111
		public ListViewItem Item
		{
			get
			{
				return this.item;
			}
			set
			{
				this.item = value;
			}
		}

		// Token: 0x0600067C RID: 1660 RVA: 0x00024F1C File Offset: 0x0002311C
		public Game(Process process, Address address)
		{
			this.ProcessId = process.Id;
			this.AOB = new AOBScan((uint)this.ProcessId);
			this.Process = process;
			this.Address = address;
			this.Memory = new Memory(this.ProcessId);
			this.Handle = Win.GetHandle(this.ProcessId, Win.WndClassNames);
			this.TLBB = new TLBB(this);
			this.Objects = new GameObjects(this);
			this.Init();
			try
			{
				this.LoadSetting();
			}
			catch (Exception)
			{
			}
			this.LUA = new LUA(this);
			this.QuestFrame = new QuestFrame(this);
			this.Packet = new PacketItem(this);
		}

		// Token: 0x0600067D RID: 1661 RVA: 0x000254E8 File Offset: 0x000236E8
		public void Sleep(int delay)
		{
			this.PostMessage(delay, 125);
		}

		// Token: 0x0600067E RID: 1662 RVA: 0x000254F4 File Offset: 0x000236F4
		public int ConverTime(DateTime dt)
		{
			return (int)(dt - new DateTime(1970, 1, 1)).TotalSeconds;
		}

		// Token: 0x17000136 RID: 310
		// (get) Token: 0x0600067F RID: 1663 RVA: 0x0002551C File Offset: 0x0002371C
		// (set) Token: 0x06000680 RID: 1664 RVA: 0x00025523 File Offset: 0x00023723
		public static int TickCount { get; set; }

		// Token: 0x17000137 RID: 311
		// (get) Token: 0x06000681 RID: 1665 RVA: 0x0002552B File Offset: 0x0002372B
		// (set) Token: 0x06000682 RID: 1666 RVA: 0x00025533 File Offset: 0x00023733
		public List<AutoEat> TudongAn { get; set; }

		// Token: 0x06000683 RID: 1667 RVA: 0x0002553C File Offset: 0x0002373C
		public void AutoAnVatPham()
		{
			if (!this.AutoEatVatPham)
			{
				return;
			}
			if (this.TudongAn.Count == 0)
			{
				return;
			}
			if (Game.TickCount % 600 != 0)
			{
				return;
			}
			int num = 0;
			try
			{
				foreach (AutoEat autoEat in this.TudongAn)
				{
					DateTime now = DateTime.Now;
					int num2 = autoEat.TimeEach * 60;
					int num3 = this.ConverTime(now);
					int num4 = this.ConverTime(autoEat.StartEat);
					if (num3 - num4 >= num2)
					{
						autoEat.StartEat = DateTime.Now;
						this.UpdateList(num, autoEat);
						this.UsingVatPhamByName(autoEat.VatPhamName, DateTime.Now.AddSeconds((double)num2));
					}
					num++;
				}
			}
			catch
			{
			}
		}

		// Token: 0x06000684 RID: 1668 RVA: 0x00025624 File Offset: 0x00023824
		public void UpdateList(int ID, AutoEat eat)
		{
			AutoEat autoEat = new AutoEat();
			autoEat.ID = eat.ID;
			autoEat.StartEat = DateTime.Now;
			autoEat.TimeEach = eat.TimeEach;
			autoEat.VatPhamName = eat.VatPhamName;
			this.TudongAn[ID] = autoEat;
		}

		// Token: 0x06000685 RID: 1669 RVA: 0x00025674 File Offset: 0x00023874
		public void UsingVatPhamByName(string name, DateTime dt)
		{
			foreach (PacketItem packetItem in this.Packet.DaoCu)
			{
				if (TINHKIEM.VietLien(packetItem.Name) == TINHKIEM.VietLien(name))
				{
					this.PlayerPackageUseItem(packetItem.Index);
					this.PushDebugMessage(string.Concat(new string[]
					{
						"Auto tự ăn vật phẩm :",
						packetItem.Name,
						" Vào lúc [",
						dt.ToString(),
						"] sẽ sử dụng tiếp"
					}));
					FrmMain.AddLog(string.Concat(new string[]
					{
						DateTime.Now.ToString("HH:mm dd-MM"),
						" ",
						this.LastName,
						"Auto tự ăn vật phẩm :",
						packetItem.Name,
						" Vào lúc [",
						dt.ToString(),
						"] sẽ sử dụng tiếp\n"
					}));
					break;
				}
			}
		}

		// Token: 0x06000686 RID: 1670 RVA: 0x00025794 File Offset: 0x00023994
		public object getCPUCounter()
		{
			PerformanceCounter performanceCounter = new PerformanceCounter();
			performanceCounter.CategoryName = "Processor";
			performanceCounter.CounterName = "% Processor Time";
			performanceCounter.InstanceName = "_Total";
			performanceCounter.NextValue();
			return performanceCounter.NextValue();
		}

		// Token: 0x17000138 RID: 312
		// (get) Token: 0x06000687 RID: 1671 RVA: 0x000257D0 File Offset: 0x000239D0
		// (set) Token: 0x06000688 RID: 1672 RVA: 0x00025837 File Offset: 0x00023A37
		public string IdLocDo
		{
			get
			{
				if (this.idLocDo == "")
				{
					this.idLocDo = TINHKIEM.ReadFile(Global.NhatHaPath + "\\" + this.TLBB.Id + ".txt");
				}
				if (this.idLocDo == "")
				{
					return "Nước Dưa Hấu\r\nPhát Tài Rồi\r\nBăng Trấn Dưa Hấu\r\nDị Dung Đan: Tuyết Nhân";
				}
				return this.idLocDo;
			}
			set
			{
				TINHKIEM.WriteFile(Global.NhatHaPath + "\\" + this.TLBB.Id + ".txt", value);
				this.idLocDo = value;
			}
		}

		// Token: 0x06000689 RID: 1673 RVA: 0x00025868 File Offset: 0x00023A68
		public void HamLenBaiTrian()
		{
			if (!this.LenBaiTrain)
			{
				return;
			}
			if (this.IsAttack)
			{
				this.IsAttack = false;
			}
			int mapID = this._baitrain.MapID;
			int posX = this._baitrain.PosX;
			int posY = this._baitrain.PosY;
			if (this.TLBB.MapId != mapID)
			{
				if (!this.GoTo((float)posX, (float)posY, mapID, false))
				{
					this.TimDuong((float)posX, (float)posY, mapID);
				}
				return;
			}
			if (TINHKIEM.GetDistance(this.CharX, this.CharY, (float)posX, (float)posY) > 2f)
			{
				this.Move((float)posX, (float)posY, mapID);
				return;
			}
			this.DownRide();
			this.IsAttack = true;
			this.LenBaiTrain = false;
			this.PushThongBao("Thông báo", "Lên Bãi Thành Công", CanhBao.Kieu.OK);
		}

		// Token: 0x0600068A RID: 1674 RVA: 0x00025928 File Offset: 0x00023B28
		public void PushThongBao(string _teude, string _noidung, CanhBao.Kieu _Type)
		{
			ThongBao thongBao = new ThongBao();
			thongBao.tideu = _teude;
			thongBao.noidung = _noidung;
			thongBao.Type = _Type;
			this.ListThongBao.Add(thongBao);
		}

		// Token: 0x17000139 RID: 313
		// (get) Token: 0x0600068B RID: 1675 RVA: 0x0002595C File Offset: 0x00023B5C
		// (set) Token: 0x0600068C RID: 1676 RVA: 0x00025964 File Offset: 0x00023B64
		public Process Process { get; set; }

		// Token: 0x1700013A RID: 314
		// (get) Token: 0x0600068D RID: 1677 RVA: 0x0002596D File Offset: 0x00023B6D
		// (set) Token: 0x0600068E RID: 1678 RVA: 0x00025975 File Offset: 0x00023B75
		public IntPtr Handle { get; set; }

		// Token: 0x0600068F RID: 1679 RVA: 0x0002597E File Offset: 0x00023B7E
		public void PostMessage(int wParam, int lParam)
		{
			Win.PostMessage(this.Handle, Global.HookMessage, wParam, lParam);
		}

		// Token: 0x1700013B RID: 315
		// (get) Token: 0x06000690 RID: 1680 RVA: 0x00025993 File Offset: 0x00023B93
		// (set) Token: 0x06000691 RID: 1681 RVA: 0x0002599B File Offset: 0x00023B9B
		public int style { get; set; }

		// Token: 0x1700013C RID: 316
		// (get) Token: 0x06000692 RID: 1682 RVA: 0x000259A4 File Offset: 0x00023BA4
		// (set) Token: 0x06000693 RID: 1683 RVA: 0x000259AC File Offset: 0x00023BAC
		public bool IsHide { get; set; }

		// Token: 0x1700013D RID: 317
		// (get) Token: 0x06000694 RID: 1684 RVA: 0x000259B5 File Offset: 0x00023BB5
		// (set) Token: 0x06000695 RID: 1685 RVA: 0x000259BD File Offset: 0x00023BBD
		public bool IsPMP { get; set; }

		// Token: 0x1700013E RID: 318
		// (get) Token: 0x06000696 RID: 1686 RVA: 0x000259C6 File Offset: 0x00023BC6
		// (set) Token: 0x06000697 RID: 1687 RVA: 0x000259CE File Offset: 0x00023BCE
		public bool IsHuyetChien { get; set; }

		// Token: 0x1400000B RID: 11
		// (add) Token: 0x06000698 RID: 1688 RVA: 0x000259D8 File Offset: 0x00023BD8
		// (remove) Token: 0x06000699 RID: 1689 RVA: 0x00025A10 File Offset: 0x00023C10
		public event EventHandler SettingLoaded;

		// Token: 0x1400000C RID: 12
		// (add) Token: 0x0600069A RID: 1690 RVA: 0x00025A48 File Offset: 0x00023C48
		// (remove) Token: 0x0600069B RID: 1691 RVA: 0x00025A80 File Offset: 0x00023C80
		public event EventHandler SkillLoaded;

		// Token: 0x1700013F RID: 319
		// (get) Token: 0x0600069C RID: 1692 RVA: 0x00025AB8 File Offset: 0x00023CB8
		public bool IsLureEx
		{
			get
			{
				return !Global.AtkFollowKey && !this.IsPK && ((this.TLBB.MapId == MAP.TangKinhCac && !this.TLBB.IsLeader) || (this.TLBB.MapId != MAP.TranLongKyCuoc && this.TLBB.MapId != MAP.LauLanBaoTang && this.IsLure));
			}
		}

		// Token: 0x17000140 RID: 320
		// (get) Token: 0x0600069D RID: 1693 RVA: 0x00025B23 File Offset: 0x00023D23
		// (set) Token: 0x0600069E RID: 1694 RVA: 0x00025B2B File Offset: 0x00023D2B
		public bool IsLure
		{
			get
			{
				return this.isLure;
			}
			set
			{
				this.isLure = value;
			}
		}

		// Token: 0x17000141 RID: 321
		// (get) Token: 0x0600069F RID: 1695 RVA: 0x00025B34 File Offset: 0x00023D34
		// (set) Token: 0x060006A0 RID: 1696 RVA: 0x00025B51 File Offset: 0x00023D51
		public bool IsRadius
		{
			get
			{
				return this.radiusMap == this.TLBB.MapId && this.isRadius;
			}
			set
			{
				this.radiusMap = this.TLBB.MapId;
				this.isRadius = value;
			}
		}

		// Token: 0x17000142 RID: 322
		// (get) Token: 0x060006A1 RID: 1697 RVA: 0x00025B6B File Offset: 0x00023D6B
		// (set) Token: 0x060006A2 RID: 1698 RVA: 0x00025B73 File Offset: 0x00023D73
		public bool IsBHDByLogin { get; set; }

		// Token: 0x17000143 RID: 323
		// (get) Token: 0x060006A3 RID: 1699 RVA: 0x00025B7C File Offset: 0x00023D7C
		// (set) Token: 0x060006A4 RID: 1700 RVA: 0x00025B84 File Offset: 0x00023D84
		public bool IsBTDByLogin { get; set; }

		// Token: 0x17000144 RID: 324
		// (get) Token: 0x060006A5 RID: 1701 RVA: 0x00025B8D File Offset: 0x00023D8D
		// (set) Token: 0x060006A6 RID: 1702 RVA: 0x00025B95 File Offset: 0x00023D95
		public bool IsDuaByLogin { get; set; }

		// Token: 0x17000145 RID: 325
		// (get) Token: 0x060006A7 RID: 1703 RVA: 0x00025B9E File Offset: 0x00023D9E
		// (set) Token: 0x060006A8 RID: 1704 RVA: 0x00025BA6 File Offset: 0x00023DA6
		public string RaoTxt { get; set; }

		// Token: 0x17000146 RID: 326
		// (get) Token: 0x060006A9 RID: 1705 RVA: 0x00025BAF File Offset: 0x00023DAF
		// (set) Token: 0x060006AA RID: 1706 RVA: 0x00025BB7 File Offset: 0x00023DB7
		public bool AutoResetTime { get; set; }

		// Token: 0x17000147 RID: 327
		// (get) Token: 0x060006AB RID: 1707 RVA: 0x00025BC0 File Offset: 0x00023DC0
		// (set) Token: 0x060006AC RID: 1708 RVA: 0x00025BC8 File Offset: 0x00023DC8
		public int State { get; set; }

		// Token: 0x060006AD RID: 1709 RVA: 0x00025BD4 File Offset: 0x00023DD4
		public bool LamDayTayNai()
		{
			if (this.TLBB.IsODaoCuFull && this.TLBB.IsONguyenLieuFull)
			{
				return true;
			}
			if (!this.TLBB.IsODaoCuFull)
			{
				foreach (PacketItem packetItem in this.Packet.DaoCu)
				{
					if (packetItem.Count > 1)
					{
						this.Packet.SplitIndex = packetItem.Index;
						this.LuaDoOneLineString("PlayerPackage:SplitItem(1);");
						return false;
					}
				}
				this.PushDebugMessage("Không thể làm đầy tay nải");
				this.PushThongBao("Thông báo", "Không thể làm đầy tay nải", CanhBao.Kieu.Eror);
				this.IsMoBTD = false;
				return false;
			}
			if (!this.TLBB.IsONguyenLieuFull)
			{
				foreach (PacketItem packetItem2 in this.Packet.NguyenLieu)
				{
					if (packetItem2.Count > 1)
					{
						this.Packet.SplitIndex = packetItem2.Index;
						this.LuaDoOneLineString("PlayerPackage:SplitItem(1);");
						return false;
					}
				}
				this.PushDebugMessage("Không thể làm đầy tay nải");
				this.PushThongBao("Thông báo", "Không thể làm đầy tay nải", CanhBao.Kieu.Eror);
				this.IsMoBTD = false;
				return false;
			}
			return false;
		}

		// Token: 0x060006AE RID: 1710 RVA: 0x00025D44 File Offset: 0x00023F44
		public void TrongTrot()
		{
			if (Game.TickCount % 18 != 0)
			{
				return;
			}
			if (this.TimeStand.Elapsed.TotalSeconds < 2.0)
			{
				return;
			}
			if (this.TLBB.PlayerState != 0)
			{
				return;
			}
			if (this.IsTrongTrot)
			{
				if (!this.IsThuHoach)
				{
					if (this.TrangThaiTrongTrot == string.Empty)
					{
						float num = 100f;
						int num2 = -1;
						if (num2 == -1)
						{
							foreach (GameObject gameObject in this.Objects.All)
							{
								if ((TINHKIEM.VietLien(gameObject.Name).Contains("nguoirom") || TINHKIEM.VietLien(gameObject.Name).Contains("daothaonhan")) && TINHKIEM.GetDistance(gameObject.X, gameObject.Y, this.CharX, this.CharY) < num && !this.lstNguoiRom.Contains(gameObject.Id))
								{
									num = TINHKIEM.GetDistance(gameObject.X, gameObject.Y, this.CharX, this.CharY);
									num2 = gameObject.Id;
								}
							}
						}
						if (num2 != -1)
						{
							this.lstNguoiRom.Add(num2);
							this.Talk(num2);
							this.TrangThaiTrongTrot = "Talk";
							return;
						}
						if (this.TrangThaiTrongTrot == string.Empty)
						{
							this.lstNguoiRom.Clear();
						}
					}
					else
					{
						if (this.TrangThaiTrongTrot == "Talk")
						{
							if (this.TrongTrotIndex == 0)
							{
								foreach (QuestFrame questFrame in QuestFrame.Enum(this))
								{
									if (questFrame.Name.EndsWith("sớm"))
									{
										this.QuestFrameOptionClicked(questFrame);
										this.TrangThaiTrongTrot = "Talk1";
										return;
									}
								}
							}
							if (this.TrongTrotIndex != 1)
							{
								goto IL_2BB;
							}
							using (List<QuestFrame>.Enumerator enumerator3 = QuestFrame.Enum(this).GetEnumerator())
							{
								while (enumerator3.MoveNext())
								{
									QuestFrame questFrame2 = enumerator3.Current;
									if (questFrame2.Name.EndsWith("muộn"))
									{
										this.QuestFrameOptionClicked(questFrame2);
										this.TrangThaiTrongTrot = "Talk1";
										return;
									}
								}
								goto IL_2BB;
							}
						}
						if (this.TrangThaiTrongTrot == "Talk1")
						{
							List<QuestFrame> list = QuestFrame.Enum(this);
							if (list.Count > this.ThuHoachIndex + 1)
							{
								QuestFrame dialog = list[this.ThuHoachIndex + 1];
								this.TrangThaiTrongTrot = string.Empty;
								this.QuestFrameOptionClicked(dialog);
								return;
							}
						}
					}
					IL_2BB:
					this.TrangThaiTrongTrot = "";
					return;
				}
				float num3 = 100f;
				int num4 = -1;
				foreach (GameObject gameObject2 in this.Objects.All)
				{
					gameObject2.DistanceEx = TINHKIEM.GetDistance(this.CharX, this.CharY, gameObject2.X, gameObject2.Y);
					if (gameObject2.IsTaiNguyen && (double)gameObject2.DistanceEx <= 4.5 && !gameObject2.Name.Contains("("))
					{
						this.PickItem(gameObject2.Id);
						return;
					}
				}
				if (this.TrangThaiTrongTrot == string.Empty)
				{
					if (num4 == -1)
					{
						foreach (GameObject gameObject3 in this.Objects.All)
						{
							if ((TINHKIEM.VietLien(gameObject3.Name).Contains("nguoirom") || TINHKIEM.VietLien(gameObject3.Name).Contains("daothaonhan")) && TINHKIEM.GetDistance(gameObject3.X, gameObject3.Y, this.CharX, this.CharY) < num3)
							{
								num3 = TINHKIEM.GetDistance(gameObject3.X, gameObject3.Y, this.CharX, this.CharY);
								num4 = gameObject3.Id;
							}
						}
					}
					if (num4 != -1)
					{
						this.Talk(num4);
						this.TrangThaiTrongTrot = "Talk";
						return;
					}
				}
				else
				{
					if (this.TrangThaiTrongTrot == "Talk")
					{
						if (this.TrongTrotIndex == 0)
						{
							foreach (QuestFrame questFrame3 in QuestFrame.Enum(this))
							{
								if (questFrame3.Name.EndsWith("sớm"))
								{
									this.QuestFrameOptionClicked(questFrame3);
									this.TrangThaiTrongTrot = "Talk1";
									return;
								}
							}
						}
						if (this.TrongTrotIndex != 1)
						{
							goto IL_586;
						}
						using (List<QuestFrame>.Enumerator enumerator4 = QuestFrame.Enum(this).GetEnumerator())
						{
							while (enumerator4.MoveNext())
							{
								QuestFrame questFrame4 = enumerator4.Current;
								if (questFrame4.Name.EndsWith("muộn"))
								{
									this.QuestFrameOptionClicked(questFrame4);
									this.TrangThaiTrongTrot = "Talk1";
									return;
								}
							}
							goto IL_586;
						}
					}
					if (this.TrangThaiTrongTrot == "Talk1")
					{
						List<QuestFrame> list2 = QuestFrame.Enum(this);
						if (list2.Count > this.ThuHoachIndex + 1)
						{
							QuestFrame dialog2 = list2[this.ThuHoachIndex + 1];
							this.QuestFrameOptionClicked(dialog2);
							this.TrangThaiTrongTrot = string.Empty;
							return;
						}
					}
				}
				IL_586:
				this.TrangThaiTrongTrot = string.Empty;
			}
		}

		// Token: 0x060006AF RID: 1711 RVA: 0x0002633C File Offset: 0x0002453C
		public bool TrongPhamVi(int x, int y, int map)
		{
			if (this.TLBB.MapId != map)
			{
				this.Move((float)x, (float)y, map);
				return false;
			}
			if (TINHKIEM.GetDistance(this.CharX, this.CharY, (float)x, (float)y) > 15f)
			{
				this.Move((float)x, (float)y);
				return false;
			}
			return true;
		}

		// Token: 0x060006B0 RID: 1712 RVA: 0x0002638E File Offset: 0x0002458E
		public bool TrongPhamVi(int x, int y)
		{
			if (TINHKIEM.GetDistance(this.CharX, this.CharY, (float)x, (float)y) > 15f)
			{
				this.Move((float)x, (float)y);
				return false;
			}
			return true;
		}

		// Token: 0x060006B1 RID: 1713 RVA: 0x000263BC File Offset: 0x000245BC
		public bool DaDenNoi(int x, int y, int map)
		{
			if (this.TLBB.MapId == map)
			{
				if (TINHKIEM.GetDistance(this.CharX, this.CharY, (float)x, (float)y) <= 3f)
				{
					return true;
				}
				if (!this.IsRide && this.TLBB.HaveRide)
				{
					this.UpRide();
					return false;
				}
				this.Move((float)x, (float)y);
				return false;
			}
			else if (this.UsingTholinhChau)
			{
				if (this.PhuIndex(map) == -1)
				{
					return false;
				}
				if (this.IsRide)
				{
					this.Ride();
					return false;
				}
				this.PlayerPackageUseItem(this.PhuIndex(map));
				return false;
			}
			else
			{
				if (!this.IsRide && this.TLBB.HaveRide)
				{
					this.UpRide();
					return false;
				}
				this.Move((float)x, (float)y, map);
				return false;
			}
		}

		// Token: 0x060006B2 RID: 1714 RVA: 0x0002647C File Offset: 0x0002467C
		public bool GoToBang()
		{
			if (this.TenMapThanh() == "-1")
			{
				this.BangOpen();
				this.IsMoBang = true;
			}
			string[] array = TINHKIEM.GetBangXY(this.TenMapThanh()).Split(new char[]
			{
				','
			});
			return this.GoTo((float)int.Parse(array[0]), (float)int.Parse(array[1]), int.Parse(array[2]), false);
		}

		// Token: 0x060006B3 RID: 1715 RVA: 0x000264E5 File Offset: 0x000246E5
		public bool GoTo(float x, float y, bool force = false)
		{
			return (this.TLBB.MapId == MAP.ViemMaSon && this.TLBB.IsFollow && !this.TLBB.IsLeader) || this.GoTo(x, y, -1, force);
		}

		// Token: 0x060006B4 RID: 1716 RVA: 0x0002651F File Offset: 0x0002471F
		public bool GoTo(NPC npc)
		{
			return this.GoTo((float)npc.X, (float)npc.Y, npc.Map, false);
		}

		// Token: 0x060006B5 RID: 1717 RVA: 0x0002653C File Offset: 0x0002473C
		public void TalkTo(NPC npc)
		{
			if (npc == null)
			{
				return;
			}
			if (this.GoTo(npc))
			{
				this.Talk(npc);
			}
		}

		// Token: 0x17000148 RID: 328
		// (get) Token: 0x060006B6 RID: 1718 RVA: 0x00026554 File Offset: 0x00024754
		public bool IsMoveEx
		{
			get
			{
				if (this.ListMoveEx.Count > 20)
				{
					this.ListMoveEx.Clear();
				}
				if (this.TLBB.BusyEx)
				{
					this.MoveExTime = Stopwatch.StartNew();
					this.ListMoveEx.Clear();
					return true;
				}
				bool flag = false;
				int[] array = new int[]
				{
					this.RoundX,
					this.RoundY
				};
				foreach (int[] array2 in this.ListMoveEx)
				{
					if (array2[0] == array[0] && array2[1] == array[1])
					{
						flag = true;
						break;
					}
				}
				if (!flag)
				{
					this.ListMoveEx.Add(new int[]
					{
						this.RoundX,
						this.RoundY
					});
					this.MoveExTime = Stopwatch.StartNew();
					return true;
				}
				if (this.MoveExTime.Elapsed.TotalSeconds > 15.0)
				{
					this.MoveExTime = Stopwatch.StartNew();
					this.ListMoveEx.Clear();
					return false;
				}
				return this.MoveExTime.Elapsed.TotalSeconds <= 4.0;
			}
		}

		// Token: 0x17000149 RID: 329
		// (get) Token: 0x060006B7 RID: 1719 RVA: 0x0002669C File Offset: 0x0002489C
		// (set) Token: 0x060006B8 RID: 1720 RVA: 0x000266A4 File Offset: 0x000248A4
		public bool IsThuyLao { get; set; }

		// Token: 0x060006B9 RID: 1721 RVA: 0x000266B0 File Offset: 0x000248B0
		public bool GoTo(float x, float y, int map, bool force = false)
		{
			if (this.TLBB.MapId == MAP.ViemMaSon && this.TLBB.IsFollow && !this.TLBB.IsLeader)
			{
				return true;
			}
			if (this.TLBB.PlayerState == 2 && !force)
			{
				return false;
			}
			if (this.TLBB.MapId == map || map == -1)
			{
				if ((double)TINHKIEM.GetDistance((float)this.RoundX, (float)this.RoundY, x, y) < 1.5)
				{
					this.MoveExTime = Stopwatch.StartNew();
					this.ListMoveEx.Clear();
					return true;
				}
				if (this.TLBB.IsFollow && !this.TLBB.IsLeader)
				{
					this.StopFollow();
				}
				if (TINHKIEM.GetDistance((float)this.RoundX, (float)this.RoundY, x, y) > 20f && !this.IsNhatHopQDua && !this.IsRide && this.TLBB.HaveRide && !this.TLBB.IsBienThan)
				{
					this.UpRide();
					return false;
				}
				if (!this.IsMoveEx)
				{
					this.FixKetMap();
					return false;
				}
				this.Move(x, y);
			}
			else if (this.UsingTholinhChau)
			{
				if (this.PhuIndex(map) != -1)
				{
					if (this.IsRide)
					{
						this.DownRide();
						return false;
					}
					this.PlayerPackageUseItem(this.PhuIndex(map));
				}
				else
				{
					if (this.TLBB.IsFollow && !this.TLBB.IsLeader)
					{
						this.StopFollow();
					}
					if (!this.IsNhatHopQDua && !this.IsRide && this.TLBB.HaveRide && !this.TLBB.IsBienThan)
					{
						this.UpRide();
						return false;
					}
					if (!this.IsMoveEx)
					{
						this.FixKetMap();
						return false;
					}
					if (this.TLBB.MapId > 500 && this.TLBB.MapId < 545)
					{
						this.RaBang(0);
						return false;
					}
					this.Move(x, y, map);
				}
			}
			else
			{
				if (this.TLBB.IsFollow && !this.TLBB.IsLeader)
				{
					this.StopFollow();
				}
				if (!this.IsNhatHopQDua && !this.IsRide && this.TLBB.HaveRide && !this.TLBB.IsBienThan)
				{
					this.UpRide();
					return false;
				}
				if (!this.IsMoveEx)
				{
					this.FixKetMap();
					return false;
				}
				if (this.TLBB.MapId > 500 && this.TLBB.MapId < 545)
				{
					this.RaBang(0);
					return false;
				}
				this.Move(x, y, map);
			}
			return false;
		}

		// Token: 0x060006BA RID: 1722 RVA: 0x00026948 File Offset: 0x00024B48
		public void TimDuong(float InputX, float InputY, int MapID)
		{
			if (this.lastAutoMove.Elapsed.TotalSeconds < 4.0)
			{
				return;
			}
			if (Unity.IsMessengerBox(this.TLBB.MapId, (int)this.CharX, (int)this.CharY))
			{
				this.LuaDoOneLineString("IsMessageBox = 1;");
				return;
			}
			if (this.ON_SCENE_TRANSING)
			{
				return;
			}
			if (this.TLBB.PlayerState == 7)
			{
				return;
			}
			if (this.TLBB.MapId != 6 && this.TLBB.MapId != 7 && this.TLBB.MapId != 24 && MapID == 2 && this.TLBB.MapId != 2)
			{
				this.DownRide();
				this.UseSkill(22);
				return;
			}
			if (!this.IsMove)
			{
				this.FixKetMap();
				return;
			}
			if (this.TLBB.MapId == MapID)
			{
				if (TINHKIEM.GetDistance(this.CharX, this.CharY, InputX, InputY) <= 3f)
				{
					return;
				}
				this.Move(InputX, InputY);
				return;
			}
			else
			{
				if (this.UsingTholinhChau)
				{
					int num = this.PhuIndex(Unity.GetFakeMapID(MapID));
					if (num != -1)
					{
						if (this.IsRide)
						{
							this.DownRide();
							return;
						}
						this.PlayerPackageUseItem(num);
						return;
					}
				}
				if (this.TLBB.HaveRide && !this.IsRide)
				{
					this.UpRide();
					return;
				}
				PathInfo pathInfo = null;
				try
				{
					pathInfo = FindPath.GetNextPath(this.TLBB.MapId, MapID);
				}
				catch
				{
					this.PushDebugMessage("Không thể tìm đường");
					return;
				}
				if (!pathInfo.isNPC)
				{
					this.Move((float)pathInfo.x, (float)pathInfo.y);
					return;
				}
				string text = "KhongCo";
				bool flag = false;
				int idNext = pathInfo.idNext;
				switch (idNext)
				{
				case 9:
					text = "Thiếu Lâm";
					flag = true;
					break;
				case 10:
					text = "Cái Bang";
					flag = true;
					break;
				case 11:
					text = "Minh Giáo";
					flag = true;
					break;
				case 12:
					text = "Võ Đang";
					flag = true;
					break;
				case 13:
					text = "Thiên Long";
					flag = true;
					break;
				case 14:
					text = "Tiêu Dao";
					flag = true;
					break;
				case 15:
					text = "Nga My";
					flag = true;
					break;
				case 16:
					text = "Tinh Túc";
					flag = true;
					break;
				case 17:
					text = "Thiên Sơn";
					flag = true;
					break;
				default:
					if (idNext == 284)
					{
						text = "Mộ Dung";
						flag = true;
					}
					break;
				}
				this.Move((float)pathInfo.x, (float)pathInfo.y);
				if (flag)
				{
					foreach (QuestFrame questFrame in QuestFrame.Enum(this))
					{
						if (questFrame.Name.Contains(text))
						{
							this.QuestFrameOptionClicked(questFrame);
							break;
						}
					}
					foreach (QuestFrame questFrame2 in QuestFrame.Enum(this))
					{
						if (questFrame2.Name.Contains("Đến các môn phái"))
						{
							this.QuestFrameOptionClicked(questFrame2);
							return;
						}
					}
				}
				if (!flag)
				{
					foreach (QuestFrame questFrame3 in QuestFrame.Enum(this))
					{
						if (questFrame3.Name.Contains("Duyệt") || questFrame3.Name.Contains("Xác nhận"))
						{
							this.QuestFrameOptionClicked(questFrame3);
							break;
						}
					}
					text = FindPath.GetScreenName(pathInfo.idNext);
					if (TINHKIEM.VietLien(text).Contains("thuchacotran"))
					{
						this.LuaDoOneLineString("IsMessageBox = 1;");
						return;
					}
					foreach (QuestFrame questFrame4 in QuestFrame.Enum(this))
					{
						string text2 = TINHKIEM.VietLienRemoveNum(questFrame4.Name);
						string value = TINHKIEM.VietLienRemoveNum(text);
						if (text2.Contains(value))
						{
							this.QuestFrameOptionClicked(questFrame4);
							break;
						}
					}
				}
				if (this.lastTalk.Elapsed.TotalSeconds > 5.0)
				{
					foreach (GameObject gameObject in this.Objects.AllNpc)
					{
						if (TINHKIEM.VietLienRemoveNum(gameObject.Name).Contains(TINHKIEM.VietLienRemoveNum(pathInfo.NpcName)))
						{
							this.Talk(gameObject.Id);
							this.lastTalk = Stopwatch.StartNew();
							break;
						}
					}
				}
				this.lastAutoMove = Stopwatch.StartNew();
				return;
			}
		}

		// Token: 0x1700014A RID: 330
		// (get) Token: 0x060006BB RID: 1723 RVA: 0x00026E14 File Offset: 0x00025014
		public int HoaIndex
		{
			get
			{
				foreach (PacketItem packetItem in PacketItem.Enum(this))
				{
					if (packetItem.Type == "CircularTaskTool8_10")
					{
						return packetItem.Index;
					}
				}
				return -1;
			}
		}

		// Token: 0x1700014B RID: 331
		// (get) Token: 0x060006BC RID: 1724 RVA: 0x00026E80 File Offset: 0x00025080
		public int PhanHoaIndex
		{
			get
			{
				foreach (PacketItem packetItem in PacketItem.Enum(this))
				{
					if (packetItem.Type == "CircularTaskTool8_11")
					{
						return packetItem.Index;
					}
				}
				return -1;
			}
		}

		// Token: 0x060006BD RID: 1725 RVA: 0x00026EEC File Offset: 0x000250EC
		public bool ChuaThuHoachDuoc(int x, int y)
		{
			if (this.DaThuHoachDuoc(x, y))
			{
				return false;
			}
			foreach (long[] array in this.ListHoaTruongThanh)
			{
				if (array[1] == (long)x && array[2] == (long)y && DateTime.Now.Ticks / 10000000L - array[0] < 0L)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x060006BE RID: 1726 RVA: 0x00026F78 File Offset: 0x00025178
		public bool HaveHoa(int x, int y)
		{
			foreach (long[] array in this.ListHoaTruongThanh)
			{
				if (array[1] == (long)x && array[2] == (long)y)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x060006BF RID: 1727 RVA: 0x00026FDC File Offset: 0x000251DC
		public bool DaThuHoachDuoc(int x, int y)
		{
			foreach (long[] array in this.ListHoaXuatHien)
			{
				if (array[1] == (long)x && array[2] == (long)y && DateTime.Now.Ticks / 10000000L - array[0] > 0L)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x060006C0 RID: 1728 RVA: 0x0002705C File Offset: 0x0002525C
		public void RemoveHoa(int x, int y)
		{
			foreach (long[] array in this.ListHoaTruongThanh)
			{
				if (array[1] == (long)x && array[2] == (long)y)
				{
					this.ListHoaTruongThanh.Remove(array);
				}
			}
			foreach (long[] array2 in this.ListHoaXuatHien)
			{
				if (array2[1] == (long)x && array2[2] == (long)y)
				{
					this.ListHoaXuatHien.Remove(array2);
				}
			}
		}

		// Token: 0x060006C1 RID: 1729 RVA: 0x0002711C File Offset: 0x0002531C
		public void SetTime(int x, int y, long time)
		{
			foreach (long[] array in this.ListHoaTruongThanh)
			{
				if (array[1] == (long)x && array[2] == (long)y)
				{
					array[0] = time;
				}
			}
		}

		// Token: 0x1700014C RID: 332
		// (get) Token: 0x060006C2 RID: 1730 RVA: 0x0002717C File Offset: 0x0002537C
		// (set) Token: 0x060006C3 RID: 1731 RVA: 0x00027184 File Offset: 0x00025384
		public bool IsXongTrungAc { get; set; }

		// Token: 0x1700014D RID: 333
		// (get) Token: 0x060006C4 RID: 1732 RVA: 0x0002718D File Offset: 0x0002538D
		// (set) Token: 0x060006C5 RID: 1733 RVA: 0x00027195 File Offset: 0x00025395
		public bool IsHong { get; set; }

		// Token: 0x1700014E RID: 334
		// (get) Token: 0x060006C6 RID: 1734 RVA: 0x0002719E File Offset: 0x0002539E
		// (set) Token: 0x060006C7 RID: 1735 RVA: 0x000271A6 File Offset: 0x000253A6
		public bool IsMoBTD { get; set; }

		// Token: 0x060006C8 RID: 1736 RVA: 0x000271B0 File Offset: 0x000253B0
		public void MoBTD()
		{
			if (this.TLBB.MapId == MAP.HuyetMo)
			{
				this.TrangThaiBTD = "";
				this.BTDX = (this.BTDY = (this.BTDMAP = (this.BTDIndex = -1)));
				return;
			}
			if (!this.IsMoBTD)
			{
				return;
			}
			if (Game.TickCount % 18 != 0)
			{
				return;
			}
			if (!this.HaveItem("Merchandise4_16"))
			{
				this.PushDebugMessage("Không có bảo tàng đồ.");
				this.PushThongBao("Thông báo", "Không có tàng bảo đồ", CanhBao.Kieu.Eror);
				this.IsMoBTD = false;
				return;
			}
			if (this.TrangThaiBTD == "")
			{
				foreach (PacketItem packetItem in this.Packet.DaoCu)
				{
					if (packetItem.Type == "Merchandise4_16")
					{
						this.PlayerPackageUseItem(packetItem.Index);
						this.TrangThaiBTD = "GetInfo";
						this.BTDIndex = packetItem.Index;
						return;
					}
				}
				this.BTDX = (this.BTDY = (this.BTDMAP = (this.BTDIndex = -1)));
			}
			if (this.TrangThaiBTD == "GetInfo" && this.TLBB.IsQuestOpen)
			{
				this.BTDInfo = QuestFrame.All(this);
				this.BTDMAP = TINHKIEM.GetMapId(this.BTDInfo);
				if (this.BTDMAP != -1)
				{
					this.BTDInfo = Regex.Replace(this.BTDInfo, ".*_INFOAIM", "");
					int num = this.BTDInfo.IndexOf("[");
					int length = this.BTDInfo.IndexOf("]") - num;
					string text = this.BTDInfo.Substring(num, length).Replace("[", "");
					if (text.Split(new char[]
					{
						','
					}).Length > 1)
					{
						this.BTDX = TINHKIEM.ParseInt(text.Split(new char[]
						{
							','
						})[0]);
						this.BTDY = TINHKIEM.ParseInt(text.Split(new char[]
						{
							','
						})[1]);
					}
				}
				this.CloseQuest();
				if (this.BTDX != -1 && this.BTDY != -1 && this.BTDMAP != -1)
				{
					this.TrangThaiBTD = "Do";
				}
			}
			if (this.TrangThaiBTD == "Do")
			{
				if (this.GoTo((float)this.BTDX, (float)this.BTDY, this.BTDMAP, false))
				{
					foreach (PacketItem packetItem2 in this.Packet.DaoCu)
					{
						if (packetItem2.Index == this.BTDIndex && packetItem2.Type == "Merchandise4_16")
						{
							this.PlayerPackageUseItem(this.BTDIndex);
						}
					}
					this.TrangThaiBTD = "";
				}
				return;
			}
			this.TrangThaiBTD = "";
		}

		// Token: 0x060006C9 RID: 1737 RVA: 0x000274E0 File Offset: 0x000256E0
		public void TrungAc()
		{
			if (Global.Paused)
			{
				return;
			}
			if (Game.TickCount % 12 != 0)
			{
				return;
			}
			if (!this.IsTrungAc)
			{
				return;
			}
			if (this.TLBB.MapId == MAP.GiamNguc)
			{
				return;
			}
			if (this.ForcePickItem())
			{
				return;
			}
			if (this.TLBB.PlayerState != 0)
			{
				return;
			}
			if (this.State != STATE.Done && this.State != STATE.TalkToCompleteMission && this.State != STATE.MissionContinute && this.State != STATE.MissionComplete)
			{
				foreach (Task task in Task.Enum(this))
				{
					if (task.Name.Contains("#{CXDT_090304_01}") && task.Completed)
					{
						this.State = STATE.Done;
						break;
					}
				}
			}
			if (this.State == STATE.HuyQ)
			{
				this.MessageboxSelfOkClicked();
				this.State = STATE.Null;
				return;
			}
			if (this.State == STATE.Null || this.State == STATE.None)
			{
				foreach (PacketItem packetItem in PacketItem.Enum(this))
				{
					if (TINHKIEM.VietLien(packetItem.Name).Contains("trungaclenh"))
					{
						this.PlayerPackageUseItem(packetItem.Index);
						this.State = STATE.GetInfo;
						this.IsHong = false;
						return;
					}
				}
			}
			if (this.State == STATE.Null || this.State == STATE.Done)
			{
				if (this.TLBB.MapId != 1)
				{
					this.TimDuong(224f, 226f, 1);
					return;
				}
				if (TINHKIEM.GetDistance(this.CharX, this.CharY, 224f, 226f) > 3f)
				{
					this.Move(224f, 226f);
					return;
				}
				foreach (GameObject gameObject in this.Objects.AllNpc)
				{
					if (TINHKIEM.VietLien(gameObject.Name).Contains("ngogioi"))
					{
						this.Talk(gameObject.Id);
						if (this.State == STATE.Null)
						{
							this.State = STATE.TalkToAcceptMission;
							return;
						}
						this.State = STATE.TalkToCompleteMission;
						return;
					}
				}
				return;
			}
			else if (this.State == STATE.None || this.State == STATE.CheckComplete)
			{
				foreach (PacketItem packetItem2 in PacketItem.Enum(this))
				{
					if (TINHKIEM.VietLien(packetItem2.Name).Contains("trungaclenh"))
					{
						this.PlayerPackageUseItem(packetItem2.Index);
						this.State = STATE.GetInfo;
						this.IsHong = false;
						return;
					}
				}
				if (this.State == STATE.CheckComplete && QuestFrame.All(this).Contains("#{CXDY_090423_01}") && QuestFrame.All(this).Contains("#{CXDY_090423_02}"))
				{
					this.IsXongTrungAc = true;
					this.IsTrungAc = false;
					return;
				}
				if (this.State == STATE.None)
				{
					this.IsHong = true;
				}
				if (this.TLBB.IsTogleMission)
				{
					this.TogleMission();
					this.PostMessage(30, 105);
					this.State = STATE.GetMissionInfo;
					this.LuaToString();
					return;
				}
				this.TogleMission();
				return;
			}
			else
			{
				if (this.State == STATE.TalkToAcceptMission)
				{
					foreach (QuestFrame questFrame in QuestFrame.Enum(this))
					{
						if (questFrame.Name == "#{CXDT_090304_01}")
						{
							this.QuestFrameOptionClicked(questFrame);
							this.State = STATE.CheckComplete;
							return;
						}
					}
				}
				if (this.State == STATE.TalkToCompleteMission)
				{
					foreach (QuestFrame questFrame2 in QuestFrame.Enum(this))
					{
						if (questFrame2.Name == "#{CXDT_090304_01}")
						{
							this.QuestFrameOptionClicked(questFrame2);
							this.State = STATE.MissionContinute;
							return;
						}
					}
				}
				if (this.State == STATE.MissionContinute)
				{
					this.PostMessage(14, 105);
					this.State = STATE.MissionComplete;
					return;
				}
				if (this.State == STATE.MissionComplete)
				{
					this.QuestFrameMissionComplete();
					this.State = STATE.Null;
					return;
				}
				if (this.State == STATE.GetInfo)
				{
					this.TrungAcInfo = QuestFrame.All(this);
					this.MissionMap = TINHKIEM.GetMapId(this.TrungAcInfo);
					int num = this.TrungAcInfo.IndexOf("[");
					int length = this.TrungAcInfo.IndexOf("]") - num;
					string text = this.TrungAcInfo.Substring(num, length).Replace("[", "");
					if (this.MissionMap != -1 && text.Split(new char[]
					{
						','
					}).Length != 0)
					{
						this.MissionX = TINHKIEM.ParseInt(text.Split(new char[]
						{
							','
						})[0]);
						this.MissionY = TINHKIEM.ParseInt(text.Split(new char[]
						{
							','
						})[1]);
						this.State = STATE.Do;
						return;
					}
				}
				if (this.State == STATE.GetMissionInfo)
				{
					string a = this.LuaString();
					if (a == "Xong")
					{
						this.State = STATE.Done;
						this.IsHong = false;
						this.IsXongTrungAc = false;
						return;
					}
					if (a == "Chua")
					{
						this.State = STATE.Null;
						this.IsHong = false;
						this.IsXongTrungAc = false;
						return;
					}
					this.State = STATE.None;
					return;
				}
				else if (this.State == STATE.Do)
				{
					if (this.TLBB.MapId != this.MissionMap)
					{
						this.TimDuong((float)this.MissionX, (float)this.MissionY, this.MissionMap);
						return;
					}
					if (TINHKIEM.GetDistance(this.CharX, this.CharY, (float)this.MissionX, (float)this.MissionY) > 3f)
					{
						this.Move((float)this.MissionX, (float)this.MissionY);
						return;
					}
					if (this.DaDenNoi(this.MissionX, this.MissionY, this.MissionMap))
					{
						this.State = STATE.Come;
					}
					this.comeTime = Stopwatch.StartNew();
					this.doneTime = null;
					return;
				}
				else
				{
					if (this.State == STATE.TalkToXaPhu)
					{
						if (this.TLBB.MapId == LACDUONG.Id)
						{
							this.Extra1 = 400956;
						}
						else if (this.TLBB.MapId == 1)
						{
							this.Extra1 = 400957;
						}
						else if (this.TLBB.MapId == 2)
						{
							this.Extra1 = 400958;
						}
						else if (this.TLBB.MapId == 246)
						{
							this.Extra1 = 400959;
						}
						this.QuestFrameOptionClicked(this.Extra1, TINHKIEM.GetTruyen(this.TrungAcInfo));
						this.State = STATE.DongY;
						return;
					}
					if (this.State == STATE.DongY)
					{
						foreach (QuestFrame questFrame3 in QuestFrame.Enum(this))
						{
							if (TINHKIEM.VietLien(questFrame3.Name).Contains("dongy"))
							{
								this.QuestFrameOptionClicked(questFrame3);
								this.State = STATE.Do;
								return;
							}
						}
					}
					if (this.State == STATE.Come)
					{
						if (this.TrongPhamVi(this.MissionX, this.MissionY, this.MissionMap))
						{
							foreach (PacketItem packetItem3 in PacketItem.Enum(this))
							{
								if (TINHKIEM.VietLien(packetItem3.Name).Contains("trungaclenh"))
								{
									this.PlayerPackageUseItem(packetItem3.Index);
									return;
								}
							}
							if (this.TLBB.MapId == MAP.ThaoNguyen)
							{
								this.ForcePickItem();
								using (List<GameObject>.Enumerator enumerator5 = this.Objects.Near20m.GetEnumerator())
								{
									while (enumerator5.MoveNext())
									{
										GameObject gameObject2 = enumerator5.Current;
										if (gameObject2.Title != "" && gameObject2.Menpai == 28)
										{
											if (gameObject2.HP > 0f)
											{
												if (this.IsRide)
												{
													this.DownRide();
													break;
												}
												this.SelectTarget(gameObject2.Id);
												this.SendKey(Global.BaseSkill);
												this.doneTime = null;
												break;
											}
											else
											{
												if (this.doneTime == null)
												{
													this.doneTime = Stopwatch.StartNew();
												}
												if (this.TLBB.MapId <= 2 || this.doneTime.Elapsed.TotalSeconds > 10.0)
												{
													this.comeTime = null;
													this.State = STATE.Done;
												}
											}
										}
									}
									return;
								}
							}
							this.ForcePickItem();
							foreach (GameObject gameObject3 in this.Objects.Near20m)
							{
								if (gameObject3.Menpai == 28 && TINHKIEM.NumDiff(gameObject3.Lvl, this.TLBB.Lvl) <= 5)
								{
									if (gameObject3.HP > 0f)
									{
										if (this.IsRide)
										{
											this.DownRide();
											return;
										}
										this.SelectTarget(gameObject3.Id);
										this.SendKey(Global.BaseSkill);
										this.doneTime = null;
									}
									else
									{
										if (this.doneTime == null)
										{
											this.doneTime = Stopwatch.StartNew();
										}
										if (this.TLBB.MapId <= 2 || this.doneTime.Elapsed.TotalSeconds > 10.0)
										{
											this.State = STATE.Done;
											this.comeTime = null;
										}
									}
								}
							}
						}
						return;
					}
					this.State = STATE.Null;
				}
			}
		}

		// Token: 0x060006CA RID: 1738 RVA: 0x00027FBC File Offset: 0x000261BC
		private void QuestFrameMissionContinue()
		{
			this.PostMessage(14, 105);
		}

		// Token: 0x060006CB RID: 1739 RVA: 0x00027FC8 File Offset: 0x000261C8
		private void QuestFrameMissionComplete()
		{
			this.PostMessage(15, 105);
		}

		// Token: 0x1700014F RID: 335
		// (get) Token: 0x060006CC RID: 1740 RVA: 0x00027FD4 File Offset: 0x000261D4
		public bool IsMove
		{
			get
			{
				if (this.TLBB.MapId == MAP.ThienSon)
				{
					return true;
				}
				if (this.TLBB.MapId == MAP.TayHo && this.CharX > 276f && this.CharX < 280f && this.CharY > 83f && this.CharY < 103f)
				{
					return true;
				}
				if (this.TrangThaiTuDuong.Contains("LeNghi") && this.TrangThaiTuDuong != "LeNghi")
				{
					return true;
				}
				if (this.TrangThaiTuDuong == "BanCung" && TINHKIEM.GetDistance(this.CharX, this.CharY, 176f, 150f) < 3f && this.TLBB.MapId == LACDUONG.Id)
				{
					return true;
				}
				int num = 15;
				if (this.IsNhiemVuCoBan)
				{
					num = 30;
				}
				if (this.IsBachHoaDuyen)
				{
					num = 5;
				}
				if (this.IsQDua)
				{
					num = 5;
				}
				if (this.IsNhatHopQDua)
				{
					num = 5;
				}
				if (this.TLBB.Busy)
				{
					this.ListMove.Clear();
					this.MoveCount = 0;
					return true;
				}
				int[] array = new int[]
				{
					(int)this.CharX,
					(int)this.CharY
				};
				foreach (int[] array2 in this.ListMove)
				{
					if (array2[0] == array[0] && array2[1] == array[1])
					{
						this.MoveCount++;
						if (this.MoveCount > num)
						{
							return false;
						}
						return true;
					}
				}
				this.ListMove.Add(array);
				if (this.ListMove.Count > 15)
				{
					this.ListMove.RemoveAt(0);
				}
				this.MoveCount = 0;
				return true;
			}
		}

		// Token: 0x060006CD RID: 1741 RVA: 0x000281B8 File Offset: 0x000263B8
		public bool InDistance(NPC npc)
		{
			return this.TLBB.MapId == npc.Map && this.InDistance((float)npc.X, (float)npc.Y);
		}

		// Token: 0x060006CE RID: 1742 RVA: 0x000281E3 File Offset: 0x000263E3
		public bool InDistance(float x, float y)
		{
			return TINHKIEM.GetDistance(this.CharX, this.CharY, x, y) <= 3f;
		}

		// Token: 0x060006CF RID: 1743 RVA: 0x00028204 File Offset: 0x00026404
		public NPC NearestObject(string name)
		{
			NPC npc = new NPC();
			float num = 9999f;
			foreach (GameObject gameObject in this.Objects.All)
			{
				if (TINHKIEM.VietLien(gameObject.Name).Contains(TINHKIEM.VietLien(name)) && TINHKIEM.GetDistance(this.CharX, this.CharY, gameObject.X, gameObject.Y) < num)
				{
					num = TINHKIEM.GetDistance(this.CharX, this.CharY, gameObject.X, gameObject.Y);
					npc.Id = gameObject.Id;
					npc.X = (int)gameObject.X;
					npc.Y = (int)gameObject.Y;
				}
			}
			return npc;
		}

		// Token: 0x17000150 RID: 336
		// (get) Token: 0x060006D0 RID: 1744 RVA: 0x000282E4 File Offset: 0x000264E4
		// (set) Token: 0x060006D1 RID: 1745 RVA: 0x000282EC File Offset: 0x000264EC
		public int BHDCount { get; set; }

		// Token: 0x17000151 RID: 337
		// (get) Token: 0x060006D2 RID: 1746 RVA: 0x000282F5 File Offset: 0x000264F5
		// (set) Token: 0x060006D3 RID: 1747 RVA: 0x000282FD File Offset: 0x000264FD
		private Stopwatch BuyTime { get; set; }

		// Token: 0x060006D4 RID: 1748 RVA: 0x00028306 File Offset: 0x00026506
		public void AskTeamFollowEx()
		{
			if (!this.IsRide && this.TLBB.HaveRide)
			{
				this.UpRide();
				return;
			}
			this.PostMessage(21, 105);
		}

		// Token: 0x060006D5 RID: 1749 RVA: 0x0002832E File Offset: 0x0002652E
		public void AskTeamFollow()
		{
			this.PostMessage(21, 105);
		}

		// Token: 0x060006D6 RID: 1750 RVA: 0x0002833A File Offset: 0x0002653A
		public void StopFollow()
		{
			this.PostMessage(20, 105);
		}

		// Token: 0x060006D7 RID: 1751 RVA: 0x00028348 File Offset: 0x00026548
		public NPC HoaGanNhat()
		{
			NPC npc = new NPC();
			float num = 9999f;
			foreach (GameObject gameObject in this.Objects.All)
			{
				if ((!FrmMain.IsFixed || (gameObject.X >= (float)(Game.TrongHoaX - 15 - 3) && gameObject.X <= (float)(Game.TrongHoaX + FrmMain.MaxHoaX - 15 + 3))) && !Game.HashDangBon.Contains(gameObject.Id) && !this.ListDaBon.Contains(gameObject.Id) && gameObject.Title.ToString() != "")
				{
					FrmMain.AllName.Contains(gameObject.Title.Split(new char[]
					{
						'#'
					})[0]);
					if (gameObject.Name.Contains("Tiên Hoa Ấu Miêu") && !TINHKIEM.VietLien(TINHKIEM.ReadFile("D:\\bl.txt")).Contains(TINHKIEM.VietLien(gameObject.Title.Substring(0, gameObject.Title.IndexOf("#")))) && num > TINHKIEM.GetDistance(this.CharX, this.CharY, gameObject.X, gameObject.Y))
					{
						num = TINHKIEM.GetDistance(this.CharX, this.CharY, gameObject.X, gameObject.Y);
						npc.Id = gameObject.Id;
						npc.X = (int)gameObject.X;
						npc.Y = (int)gameObject.Y;
					}
				}
			}
			return npc;
		}

		// Token: 0x17000152 RID: 338
		// (get) Token: 0x060006D8 RID: 1752 RVA: 0x0002850C File Offset: 0x0002670C
		// (set) Token: 0x060006D9 RID: 1753 RVA: 0x00028514 File Offset: 0x00026714
		public string InfoHoa { get; set; }

		// Token: 0x17000153 RID: 339
		// (get) Token: 0x060006DA RID: 1754 RVA: 0x0002851D File Offset: 0x0002671D
		// (set) Token: 0x060006DB RID: 1755 RVA: 0x00028525 File Offset: 0x00026725
		public int LastHoaTime { get; set; }

		// Token: 0x060006DC RID: 1756 RVA: 0x00028530 File Offset: 0x00026730
		public void ThuHoachBHD()
		{
			if (!this.IsThuHoachHoa)
			{
				return;
			}
			float num = 9999f;
			GameObject gameObject = null;
			foreach (GameObject gameObject2 in this.Objects.All)
			{
				if (gameObject2.Title.ToString() != "" && gameObject2.Name.Contains("Hoa Trưởng Thành"))
				{
					float distance = TINHKIEM.GetDistance(this.CharX, this.CharY, gameObject2.X, gameObject2.Y);
					if (!this.ListBHDXuatHien.ContainsKey(gameObject2.Id))
					{
						this.ListBHDXuatHien.Add(gameObject2.Id, DateTime.Now.AddSeconds(180.0));
					}
					if (((this.ListThuHoachBHD.ContainsKey(gameObject2.Id) && (float)(this.ListThuHoachBHD[gameObject2.Id].Ticks - DateTime.Now.Ticks) - distance / TLBB.RunSpeed < 0f) || (float)(this.ListBHDXuatHien[gameObject2.Id].Ticks - DateTime.Now.Ticks) - distance / TLBB.RunSpeed < 0f) && distance < num)
					{
						num = distance;
						gameObject = gameObject2;
					}
				}
			}
			if (gameObject != null)
			{
				if (this.GoTo(gameObject.X, gameObject.Y, false))
				{
					if (this.IsRide)
					{
						this.DownRide();
					}
					this.UseSkill(3, gameObject.Id);
				}
				return;
			}
			num = 9999f;
			foreach (GameObject gameObject3 in this.Objects.All)
			{
				if (gameObject3.Title.ToString() != "" && gameObject3.Name.Contains("Hoa Trưởng Thành") && !this.ListThuHoachBHD.ContainsKey(gameObject3.Id))
				{
					float distance2 = TINHKIEM.GetDistance(this.CharX, this.CharY, gameObject3.X, gameObject3.Y);
					if (distance2 < num)
					{
						num = distance2;
						gameObject = gameObject3;
					}
				}
			}
			if (gameObject == null || !this.GoTo(gameObject.X, gameObject.Y, false))
			{
				return;
			}
			if (this.TLBB.IsQuestOpen && QuestFrame.All(this).Contains("#{SDJZH_091106_08}"))
			{
				this.dialogInfo = QuestFrame.All(this).Replace("#{SDJZH_091106_08}", "");
				int num2 = TINHKIEM.ParseInt(this.dialogInfo);
				this.ListThuHoachBHD.Add(gameObject.Id, DateTime.Now.AddSeconds((double)(num2 - 1)));
				this.CloseQuest();
				return;
			}
			if (this.IsRide)
			{
				this.DownRide();
			}
			this.UseSkill(3, gameObject.Id);
		}

		// Token: 0x17000154 RID: 340
		// (get) Token: 0x060006DD RID: 1757 RVA: 0x00028858 File Offset: 0x00026A58
		// (set) Token: 0x060006DE RID: 1758 RVA: 0x00028860 File Offset: 0x00026A60
		public bool IsTrongByLogin { get; set; }

		// Token: 0x17000155 RID: 341
		// (get) Token: 0x060006DF RID: 1759 RVA: 0x00028869 File Offset: 0x00026A69
		// (set) Token: 0x060006E0 RID: 1760 RVA: 0x00028871 File Offset: 0x00026A71
		public int ShowTime { get; set; }

		// Token: 0x060006E1 RID: 1761 RVA: 0x0002887C File Offset: 0x00026A7C
		public bool IsClearArea()
		{
			if (TINHKIEM.NumDiff(Game.TrongHoaX - 15, this.BachHoaDuyenX) >= FrmMain.MaxHoaX)
			{
				return false;
			}
			if (this.BlackListHoa.Contains(this.BachHoaDuyenX.ToString() + "," + this.BachHoaDuyenY.ToString()))
			{
				return false;
			}
			foreach (GameObject gameObject in this.Objects.All)
			{
				if (gameObject.Name.Contains("Tiên Hoa Ấu Miêu") || gameObject.Name.Contains("Hoa Trưởng Thành") || gameObject.IsNPC)
				{
					gameObject.DistanceEx = gameObject.GetDistance((float)this.BachHoaDuyenX, (float)this.BachHoaDuyenY);
					if (gameObject.Y - (float)this.BachHoaDuyenY < 2f && gameObject.Y - (float)this.BachHoaDuyenY > -2f && (double)gameObject.DistanceEx <= 2.5)
					{
						this.BlackListHoa.Add(this.BachHoaDuyenX.ToString() + "," + this.BachHoaDuyenY.ToString());
						return false;
					}
				}
			}
			return true;
		}

		// Token: 0x060006E2 RID: 1762 RVA: 0x000289D8 File Offset: 0x00026BD8
		public bool IsSafeArea()
		{
			if (this.TLBB.MapId == MAP.PhungMinhVuongLang)
			{
				foreach (GameObject gameObject in this.Objects.All)
				{
					if (gameObject.Menpai == 0 && !gameObject.IsLootPacket && TINHKIEM.GetDistance((float)gameObject.RoundX, (float)gameObject.RoundY, (float)this.SafeX, (float)this.SafeY) < 7f)
					{
						return false;
					}
				}
				return true;
			}
			foreach (GameObject gameObject2 in this.Objects.All)
			{
				if (gameObject2.Menpai == 0 && !gameObject2.IsLootPacket)
				{
					if (this.NotSafe.Contains(this.SafeX.ToString() + "," + this.SafeY.ToString()))
					{
						return false;
					}
					if (TINHKIEM.GetDistance((float)gameObject2.RoundX, (float)gameObject2.RoundY, (float)this.SafeX, (float)this.SafeY) <= 10f)
					{
						this.NotSafe.Add(this.SafeX.ToString() + "," + this.SafeY.ToString());
						return false;
					}
				}
				if (gameObject2.CleanName == "hoiamphien")
				{
					if (this.NotSafe.Contains(this.SafeX.ToString() + "," + this.SafeY.ToString()))
					{
						return false;
					}
					if (TINHKIEM.GetDistance((float)gameObject2.RoundX, (float)gameObject2.RoundY, (float)this.SafeX, (float)this.SafeY) <= 13f)
					{
						this.NotSafe.Add(this.SafeX.ToString() + "," + this.SafeY.ToString());
						return false;
					}
				}
			}
			return true;
		}

		// Token: 0x17000156 RID: 342
		// (get) Token: 0x060006E3 RID: 1763 RVA: 0x00028C20 File Offset: 0x00026E20
		// (set) Token: 0x060006E4 RID: 1764 RVA: 0x00028C27 File Offset: 0x00026E27
		public static int TrongHoaX { get; set; }

		// Token: 0x060006E5 RID: 1765 RVA: 0x00028C30 File Offset: 0x00026E30
		public void GetSafeToaDo()
		{
			if (this.TLBB.MapId == MAP.PhungMinhVuongLang)
			{
				if (this.CurPhungMinhIndex == -1)
				{
					this.CurPhungMinhIndex = 0;
					this.SafeX = TINHKIEM.ParseInt(GAMEDIC.PhungMinhVuongLang[this.CurPhungMinhIndex].Split(new char[]
					{
						','
					})[0]);
					this.SafeY = TINHKIEM.ParseInt(GAMEDIC.PhungMinhVuongLang[this.CurPhungMinhIndex].Split(new char[]
					{
						','
					})[1]);
				}
				while (!this.IsSafeArea())
				{
					this.CurPhungMinhIndex++;
					if (this.CurPhungMinhIndex > 13)
					{
						this.SafeX = (this.SafeY = 0);
						this.CurPhungMinhIndex = -1;
						break;
					}
					this.SafeX = TINHKIEM.ParseInt(GAMEDIC.PhungMinhVuongLang[this.CurPhungMinhIndex].Split(new char[]
					{
						','
					})[0]);
					this.SafeY = TINHKIEM.ParseInt(GAMEDIC.PhungMinhVuongLang[this.CurPhungMinhIndex].Split(new char[]
					{
						','
					})[1]);
				}
				if (!this.IsSafeArea())
				{
					this.SafeX = (this.SafeY = 0);
				}
				return;
			}
			if (this.RoundX >= 16 && this.RoundX <= 42 && this.RoundY >= 16 && this.RoundY <= 38)
			{
				if (this.SafeX == 0)
				{
					this.SafeX = 16;
					this.SafeY = 18;
				}
				while (!this.IsSafeArea())
				{
					this.SafeX += 2;
					if (this.SafeX > 42)
					{
						this.SafeX = 18;
						this.SafeY += 3;
					}
					if (this.SafeY > 38)
					{
						this.SafeX = 16;
						this.SafeY = 18;
						break;
					}
				}
				if (!this.IsSafeArea())
				{
					this.NotSafe.Clear();
				}
				return;
			}
			if (TINHKIEM.GetDistance(this.CharX, this.CharY, 100f, 95f) < 25f)
			{
				if (this.SafeX == 0)
				{
					this.SafeX = 85;
					this.SafeY = 80;
				}
				while (!this.IsSafeArea())
				{
					this.SafeX += 2;
					if (this.SafeX > 115)
					{
						this.SafeX = 85;
						this.SafeY += 3;
					}
					if (this.SafeY > 105)
					{
						this.SafeX = 85;
						this.SafeY = 80;
						break;
					}
				}
				if (!this.IsSafeArea())
				{
					this.NotSafe.Clear();
				}
				return;
			}
			if (TINHKIEM.GetDistance(this.CharX, this.CharY, 30f, 100f) < 30f)
			{
				if (this.SafeX == 0)
				{
					this.SafeX = 10;
					this.SafeY = 85;
				}
				while (!this.IsSafeArea())
				{
					this.SafeX += 2;
					if (this.SafeX > 42)
					{
						this.SafeX = 10;
						this.SafeY += 3;
					}
					if (this.SafeY > 115)
					{
						this.SafeX = 10;
						this.SafeY = 85;
						break;
					}
				}
				if (!this.IsSafeArea())
				{
					this.NotSafe.Clear();
				}
				return;
			}
		}

		// Token: 0x060006E6 RID: 1766 RVA: 0x00028F74 File Offset: 0x00027174
		public void GetToaDo()
		{
			if ((Game.TrongHoaX - 55) % 3 != 0)
			{
				Game.TrongHoaX -= (Game.TrongHoaX - 55) % 3;
			}
			this.BachHoaDuyenX = Game.TrongHoaX - 15;
			this.BachHoaDuyenY = 154;
			while (!this.IsClearArea())
			{
				if (this.BachHoaDuyenY == 144)
				{
					this.BachHoaDuyenY = 154;
					this.BachHoaDuyenX += 3;
				}
				else
				{
					this.BachHoaDuyenY -= 2;
				}
				if (this.BachHoaDuyenX - Game.TrongHoaX > FrmMain.MaxHoaX)
				{
					this.BachHoaDuyenX = 0;
					this.BachHoaDuyenY = 0;
					return;
				}
			}
		}

		// Token: 0x17000157 RID: 343
		// (get) Token: 0x060006E7 RID: 1767 RVA: 0x0002901D File Offset: 0x0002721D
		// (set) Token: 0x060006E8 RID: 1768 RVA: 0x00029025 File Offset: 0x00027225
		public bool IsTheoAcTac { get; set; }

		// Token: 0x060006E9 RID: 1769 RVA: 0x00029030 File Offset: 0x00027230
		public bool IsMapKeoDoi()
		{
			return this.TLBB.MapId == MAP.VoLuongSon || this.TLBB.MapId == MAP.KiemCac || this.TLBB.MapId == MAP.DonHoang || this.TLBB.MapId == MAP.TungSon || this.TLBB.MapId == MAP.ThaiHo || this.TLBB.MapId == MAP.KinhHo || this.TLBB.MapId == MAP.DuongMon || this.TLBB.MapId == MAP.MoDung || this.TLBB.MapId == MAP.TinhTuc || this.TLBB.MapId == MAP.TieuDao || this.TLBB.MapId == MAP.ThieuLam || this.TLBB.MapId == MAP.ThienSon || this.TLBB.MapId == MAP.ThienLong || this.TLBB.MapId == MAP.NgaMy || this.TLBB.MapId == MAP.VoDang || this.TLBB.MapId == MAP.MinhGiao || this.TLBB.MapId == MAP.CaiBang || this.TLBB.MapId == MAP.TayHo || this.TLBB.MapId == MAP.NhiHai || this.TLBB.MapId == MAP.NhanNam || this.TLBB.MapId == MAP.ThanhThuSon || this.TLBB.MapId == MAP.LauLan || this.TLBB.MapId == MAP.PhungHoangCoThanh;
		}

		// Token: 0x17000158 RID: 344
		// (get) Token: 0x060006EA RID: 1770 RVA: 0x0002920C File Offset: 0x0002740C
		public bool IsMapAcBa
		{
			get
			{
				return this.TLBB.MapId == MAP.ThieuLamAcBa || this.TLBB.MapId == MAP.NgaMyAcBa || this.TLBB.MapId == MAP.TieuDaoAcBa || this.TLBB.MapId == MAP.DuongMonAcBa || this.TLBB.MapId == MAP.MinhGiaoAcBa || this.TLBB.MapId == MAP.VoDangAcBa || this.TLBB.MapId == MAP.TinhTucAcBa || this.TLBB.MapId == MAP.ThienSonAcBa || this.TLBB.MapId == MAP.CaiBangAcBa || this.TLBB.MapId == MAP.ThienLongAcBa || this.TLBB.MapId == MAP.MoDungAcBa;
			}
		}

		// Token: 0x060006EB RID: 1771 RVA: 0x000292EC File Offset: 0x000274EC
		public bool IsMapPhuBan()
		{
			return this.TLBB.MapId == MAP.TacKhauDoanhDia || this.TLBB.MapId == MAP.ThieuLamAcBa || this.TLBB.MapId == MAP.NgaMyAcBa || this.TLBB.MapId == MAP.TieuDaoAcBa || this.TLBB.MapId == MAP.DuongMonAcBa || this.TLBB.MapId == MAP.MinhGiaoAcBa || this.TLBB.MapId == MAP.VoDangAcBa || this.TLBB.MapId == MAP.TinhTucAcBa || this.TLBB.MapId == MAP.ThienSonAcBa || this.TLBB.MapId == MAP.CaiBangAcBa || this.TLBB.MapId == MAP.ThienLongAcBa || this.TLBB.MapId == MAP.MoDungAcBa || this.TLBB.MapId == MAP.TangKinhCac || this.TLBB.MapId == MAP.PhungHoangCoThanhPhuBan || this.TLBB.MapId == MAP.ViemMaSon || this.TLBB.MapId == MAP.TamTaiHiepCoc || this.TLBB.MapId == MAP.ThanhThuSonPhuBan || this.TLBB.MapId == MAP.HuyenVuDaoPhuBan || this.TLBB.MapId == MAP.TranLongKyCuoc || this.TLBB.MapId == MAP.LauLanBaoTang || this.TLBB.MapId == MAP.PhieuMieuPhong || this.TLBB.MapId == MAP.YenTuO;
		}

		// Token: 0x17000159 RID: 345
		// (get) Token: 0x060006EC RID: 1772 RVA: 0x000294B1 File Offset: 0x000276B1
		// (set) Token: 0x060006ED RID: 1773 RVA: 0x000294B9 File Offset: 0x000276B9
		public bool IsTalkedRose { get; set; }

		// Token: 0x1700015A RID: 346
		// (get) Token: 0x060006EE RID: 1774 RVA: 0x000294C2 File Offset: 0x000276C2
		// (set) Token: 0x060006EF RID: 1775 RVA: 0x000294CA File Offset: 0x000276CA
		public bool IsDoiKTT { get; set; }

		// Token: 0x1700015B RID: 347
		// (get) Token: 0x060006F0 RID: 1776 RVA: 0x000294D3 File Offset: 0x000276D3
		// (set) Token: 0x060006F1 RID: 1777 RVA: 0x000294DB File Offset: 0x000276DB
		public bool IsNhiemVuCoBan { get; set; }

		// Token: 0x1700015C RID: 348
		// (get) Token: 0x060006F2 RID: 1778 RVA: 0x000294E4 File Offset: 0x000276E4
		// (set) Token: 0x060006F3 RID: 1779 RVA: 0x000294EC File Offset: 0x000276EC
		public Task TaskCoBan { get; set; }

		// Token: 0x1700015D RID: 349
		// (get) Token: 0x060006F4 RID: 1780 RVA: 0x000294F5 File Offset: 0x000276F5
		// (set) Token: 0x060006F5 RID: 1781 RVA: 0x000294FD File Offset: 0x000276FD
		public Script ScriptCoBan { get; set; }

		// Token: 0x1700015E RID: 350
		// (get) Token: 0x060006F6 RID: 1782 RVA: 0x00029506 File Offset: 0x00027706
		// (set) Token: 0x060006F7 RID: 1783 RVA: 0x0002950E File Offset: 0x0002770E
		private int IsTalkToNpc { get; set; }

		// Token: 0x1700015F RID: 351
		// (get) Token: 0x060006F8 RID: 1784 RVA: 0x00029517 File Offset: 0x00027717
		// (set) Token: 0x060006F9 RID: 1785 RVA: 0x0002951F File Offset: 0x0002771F
		private bool IsLoad { get; set; }

		// Token: 0x060006FA RID: 1786 RVA: 0x00029528 File Offset: 0x00027728
		public int GetNPCId(Script script)
		{
			int result = -1;
			foreach (GameObject gameObject in this.Objects.All)
			{
				if (script.Info.ToLower().Contains(gameObject.Name.ToLower()) && TINHKIEM.GetDistance(gameObject.X, gameObject.Y, this.CharX, this.CharY) < 3f)
				{
					result = gameObject.Id;
				}
			}
			return result;
		}

		// Token: 0x060006FB RID: 1787 RVA: 0x000295C4 File Offset: 0x000277C4
		public int GetNPCId(NPC npc)
		{
			return this.GetNPCId((float)npc.X, (float)npc.Y);
		}

		// Token: 0x060006FC RID: 1788 RVA: 0x000295DC File Offset: 0x000277DC
		public int GetNPCId(float x, float y)
		{
			float num = 999f;
			int result = -1;
			foreach (GameObject gameObject in this.Objects.AllNpc)
			{
				float distance = TINHKIEM.GetDistance(x, y, gameObject.X, gameObject.Y);
				if (distance < num)
				{
					num = distance;
					result = gameObject.Id;
				}
			}
			return result;
		}

		// Token: 0x060006FD RID: 1789 RVA: 0x0002965C File Offset: 0x0002785C
		public void NhiemVuCoBan()
		{
			if (!this.IsNhiemVuCoBan)
			{
				return;
			}
			if (!this.TLBB.Online)
			{
				return;
			}
			int num = Game.TickCount % 9;
		}

		// Token: 0x060006FE RID: 1790 RVA: 0x0002967E File Offset: 0x0002787E
		public void CloseMission()
		{
			this.PostMessage(18, 105);
		}

		// Token: 0x060006FF RID: 1791 RVA: 0x0002968A File Offset: 0x0002788A
		public bool DuocAntiep()
		{
			return !this.Objects.Self.Buff.Contains(349);
		}

		// Token: 0x06000700 RID: 1792 RVA: 0x000296AC File Offset: 0x000278AC
		public void AutoX2()
		{
			if (!this.TuAnX2)
			{
				return;
			}
			if (this.DuocAntiep())
			{
				foreach (PacketItem packetItem in PacketItem.Enum(this))
				{
					if (packetItem.Type == "Cloth3_14")
					{
						this.PlayerPackageUseItem(packetItem.Index);
						this.LuaDoOneLineString("IsMessageBox = 1;");
						break;
					}
				}
			}
		}

		// Token: 0x06000701 RID: 1793 RVA: 0x00029734 File Offset: 0x00027934
		public void Chat(string kenh, string msg)
		{
			this.LuaDoUnicodeString(string.Concat(new string[]
			{
				"Talk:SendChatMessage('",
				kenh,
				"', '",
				msg,
				"');"
			}));
		}

		// Token: 0x17000160 RID: 352
		// (get) Token: 0x06000702 RID: 1794 RVA: 0x00029767 File Offset: 0x00027967
		// (set) Token: 0x06000703 RID: 1795 RVA: 0x0002976F File Offset: 0x0002796F
		public bool NexStep { get; set; }

		// Token: 0x06000704 RID: 1796 RVA: 0x00029778 File Offset: 0x00027978
		public void AcTac()
		{
			if (!this.Talked && this.TLBB.MapId == 272)
			{
				if (this.TLBB.IsLeader)
				{
					foreach (GameObject gameObject in this.Objects.All)
					{
						if (gameObject.Name.Contains("n Du V") && gameObject.Name.Length == 24)
						{
							this.Talk(gameObject.Id);
						}
					}
					foreach (QuestFrame questFrame in QuestFrame.Enum(this))
					{
						if (questFrame.StrOptionExtra1 == 402108)
						{
							this.QuestFrameOptionClicked(questFrame);
							this.Talked = true;
							break;
						}
					}
				}
				return;
			}
			if (!Global.IsAcTac)
			{
				return;
			}
			if (this.TLBB.MapId == 272 && this.Objects.NearMonter5m.Count != 0)
			{
				if (this.TLBB.IsFollow)
				{
					this.StopFollow();
				}
				return;
			}
			if (Game.TickCount % 18 != 0)
			{
				return;
			}
			if (this.TLBB.MapId == MAP.ThanhThuSonPhuBan)
			{
				if (this.MoveIndex == -1)
				{
					this.MoveIndex = 0;
				}
				if (TINHKIEM.SecDiff(this.BossTime, DateTime.Now) >= 15 && this.TLBB.IsLeader)
				{
					if (!this.IsRide && this.TLBB.HaveRide)
					{
						this.UpRide();
						return;
					}
					if (!this.TLBB.IsFollow)
					{
						this.AskTeamFollow();
						return;
					}
				}
				if (this.BossTime != DateTime.MinValue)
				{
					if (this.TLBB.IsLeader && !this.IsRide && this.TLBB.HaveRide)
					{
						this.UpRide();
						return;
					}
					return;
				}
				else
				{
					if (this.TLBB.IsFollow)
					{
						this.StopFollow();
						return;
					}
					if (this.IsRide)
					{
						this.Ride();
						return;
					}
					if (TINHKIEM.GetDistance(this.CharX, this.CharY, 87f, 64f) > 3f && this.TLBB.PlayerState != 2)
					{
						this.Move(87f, 64f);
					}
					foreach (GameObject gameObject2 in this.Objects.All)
					{
						if (TINHKIEM.VietLien(gameObject2.Name) == "datrudaumuc" && gameObject2.HP == 0f)
						{
							if (this.BossTime == DateTime.MinValue)
							{
								this.BossTime = DateTime.Now;
							}
							return;
						}
					}
					return;
				}
			}
			else if (this.IsMapPhuBan())
			{
				if (this.MoveIndex == -1)
				{
					this.MoveIndex = 0;
				}
				if (TINHKIEM.SecDiff(this.BossTime, DateTime.Now) >= 15 && this.TLBB.IsLeader)
				{
					if (!this.IsRide && this.TLBB.HaveRide)
					{
						this.UpRide();
						return;
					}
					if (!this.TLBB.IsFollow)
					{
						this.AskTeamFollow();
						return;
					}
				}
				if (this.BossTime != DateTime.MinValue)
				{
					if (this.TLBB.IsLeader && !this.IsRide && this.TLBB.HaveRide)
					{
						this.UpRide();
						return;
					}
					return;
				}
				else
				{
					foreach (GameObject gameObject3 in this.Objects.All)
					{
						if (TINHKIEM.VietLien(gameObject3.Name) == "tacbinhdaumuc" || TINHKIEM.VietLien(gameObject3.Name) == "acba" || TINHKIEM.VietLien(gameObject3.Name) == "bansondaonhan")
						{
							if (gameObject3.HP == 0f)
							{
								if (this.BossTime == DateTime.MinValue)
								{
									this.BossTime = DateTime.Now;
								}
								if (TINHKIEM.GetDistance(this.CharX, this.CharY, gameObject3.X, gameObject3.Y) > 3f && this.TLBB.PlayerState != 2)
								{
									this.Move(gameObject3.X, gameObject3.Y);
								}
							}
							return;
						}
					}
					if (this.TLBB.IsFollow)
					{
						this.StopFollow();
						return;
					}
					if (this.IsRide)
					{
						this.Ride();
						return;
					}
					this.Next();
					if (this.Objects.NearMonter20m.Count == 0 && this.IdleTime > 1)
					{
						this.MoveNext();
					}
					return;
				}
			}
			else
			{
				if (this.IsMapKeoDoi())
				{
					if (this.TLBB.IsLeader)
					{
						if (!this.IsRide && this.TLBB.HaveRide)
						{
							this.UpRide();
							return;
						}
						if (!this.TLBB.IsFollow)
						{
							this.AskTeamFollow();
							return;
						}
						if (!this.IsMove)
						{
							this.FixKetMap();
							return;
						}
						this.MoveNext();
					}
				}
				else if (this.Objects.NearMonter20m.Count == 0)
				{
					int i = this.TLBB.MapId;
					string text = Setting.LoadMAP(i.ToString());
					int num = 0;
					foreach (string text2 in text.Split(new char[]
					{
						'-'
					}))
					{
						int num2 = 0;
						int num3 = 0;
						try
						{
							num2 = TINHKIEM.ParseInt(text2.Split(new char[]
							{
								','
							})[0]);
							num3 = TINHKIEM.ParseInt(text2.Split(new char[]
							{
								','
							})[1]);
						}
						catch
						{
						}
						if (num2 != 0 && num3 != 0)
						{
							num++;
						}
					}
					if (num > 1)
					{
						this.MoveNext();
					}
				}
				if (this.TLBB.IsLeader)
				{
					this.TalkNPCPhuBan();
				}
			}
		}

		// Token: 0x06000705 RID: 1797 RVA: 0x00029DA0 File Offset: 0x00027FA0
		private bool TalkNPCPhuBan()
		{
			float num = 100f;
			int num2 = -1;
			foreach (GameObject gameObject in this.Objects.All)
			{
				if (((gameObject.Name.Contains("c T") && gameObject.Name.Contains("o Ph")) || gameObject.CleanName == "therebels" || gameObject.CleanName == "thiefraid") && TINHKIEM.GetDistance(this.CharX, this.CharY, gameObject.X, gameObject.Y) < num)
				{
					num = TINHKIEM.GetDistance(this.CharX, this.CharY, gameObject.X, gameObject.Y);
					num2 = gameObject.Id;
				}
				if (gameObject.Name.StartsWith("Giang h") && gameObject.Name.Contains(" t") && TINHKIEM.GetDistance(this.CharX, this.CharY, gameObject.X, gameObject.Y) < num)
				{
					num = TINHKIEM.GetDistance(this.CharX, this.CharY, gameObject.X, gameObject.Y);
					num2 = gameObject.Id;
				}
				if (gameObject.Name.Contains("n Du V") && TINHKIEM.GetDistance(this.CharX, this.CharY, gameObject.X, gameObject.Y) < num)
				{
					num = TINHKIEM.GetDistance(this.CharX, this.CharY, gameObject.X, gameObject.Y);
					num2 = gameObject.Id;
				}
				if (gameObject.Name.StartsWith("M") && gameObject.Name.Contains("Kim H") && gameObject.Name.Contains("u ") && TINHKIEM.GetDistance(this.CharX, this.CharY, gameObject.X, gameObject.Y) < num)
				{
					num = TINHKIEM.GetDistance(this.CharX, this.CharY, gameObject.X, gameObject.Y);
					num2 = gameObject.Id;
				}
				if (this.TLBB.MapId == MAP.ThanhThuSon && gameObject.Title.Contains("Linh Thú"))
				{
					num = TINHKIEM.GetDistance(this.CharX, this.CharY, gameObject.X, gameObject.Y);
					num2 = gameObject.Id;
				}
				if (this.TLBB.MapId == MAP.LauLan && gameObject.Title.Contains("Thiên Niên Kỳ Thú"))
				{
					num = TINHKIEM.GetDistance(this.CharX, this.CharY, gameObject.X, gameObject.Y);
					num2 = gameObject.Id;
				}
			}
			if (num2 == -1)
			{
				return false;
			}
			if (!this.TLBB.IsQuestOpen)
			{
				this.Talk(num2);
				return true;
			}
			if (this.TLBB.MapId == MAP.ThanhThuSon)
			{
				foreach (QuestFrame questFrame in QuestFrame.Enum(this))
				{
					if (questFrame.Name.Contains("Giải cứu Linh Thú"))
					{
						this.QuestFrameOptionClicked(questFrame);
						this.CloseQuest();
						return true;
					}
				}
			}
			if (this.TLBB.MapId == MAP.LauLan)
			{
				foreach (QuestFrame questFrame2 in QuestFrame.Enum(this))
				{
					if (questFrame2.Name.Contains("Thiên Giáng Kỳ Thú"))
					{
						this.QuestFrameOptionClicked(questFrame2);
						this.CloseQuest();
						return true;
					}
				}
			}
			foreach (QuestFrame questFrame3 in QuestFrame.Enum(this))
			{
				if (questFrame3.StrOptionExtra1 == 50013 && questFrame3.StrOptionExtra2 == -1)
				{
					this.QuestFrameOptionClicked(questFrame3);
					this.CloseQuest();
					return true;
				}
			}
			this.QuestFrame.ClickAll();
			this.CloseQuest();
			return true;
		}

		// Token: 0x17000161 RID: 353
		// (get) Token: 0x06000706 RID: 1798 RVA: 0x0002A214 File Offset: 0x00028414
		// (set) Token: 0x06000707 RID: 1799 RVA: 0x0002A21C File Offset: 0x0002841C
		public QuestFrame QuestFrame { get; set; }

		// Token: 0x17000162 RID: 354
		// (get) Token: 0x06000708 RID: 1800 RVA: 0x0002A225 File Offset: 0x00028425
		public int Radius
		{
			get
			{
				return (int)TINHKIEM.GetDistance(this.RadiusX, this.RadiusY, this.CharX, this.CharY);
			}
		}

		// Token: 0x17000163 RID: 355
		// (get) Token: 0x06000709 RID: 1801 RVA: 0x0002A248 File Offset: 0x00028448
		public string ExpInfo
		{
			get
			{
				string result;
				try
				{
					if (this.TLBB.Lvl < 1 || this.TLBB.Lvl > 149)
					{
						result = "00:00:00\r\n\r\n0.00M\r\n0.00M\r\n0.00M\r\n0.00M\r\n0.00M\r\n0.00M\r\n0.00M (0.00%)\r\n\r\n00:00:00:00";
					}
					else
					{
						string text = "vô tận";
						int num = this.TLBB.MaxExp - this.TLBB.Exp;
						if (num <= 0)
						{
							text = "0 giây";
						}
						else if (this.ExpSpeed != 0f)
						{
							text = Game.TimeSpanToString(TimeSpan.FromHours((double)((float)num / this.ExpSpeed)));
						}
						result = string.Concat(new string[]
						{
							Game.TimeSpanToString(this.AutoTime.Elapsed),
							"\r\n\r\n\r\n",
							string.Format("{0:0.00}M\r\n", (float)this.ExpStart / 1000000f),
							string.Format("{0:0.00}M\r\n", (float)this.TLBB.Exp / 1000000f),
							string.Format("{0:0.00}M ({1:0.00%})\r\n", (float)this.ExpGain / 1000000f, (float)this.ExpGain / (float)this.TLBB.MaxExp),
							string.Format("{0:0.00}M\r\n", (float)num / 1000000f),
							string.Format("{0:0.00}M\r\n", (float)this.TLBB.MaxExp / 1000000f),
							string.Format("{0:0.00}M ({1:0.00%})", this.ExpSpeed / 1000000f, this.ExpSpeed / (float)this.TLBB.MaxExp),
							this.IsX2 ? " ( x2 )" : "",
							"\r\n\r\n",
							text
						});
					}
				}
				catch
				{
					string text2 = "vô tận";
					int num2 = 0;
					result = string.Concat(new string[]
					{
						Game.TimeSpanToString(this.AutoTime.Elapsed),
						"\r\n\r\n\r\n",
						string.Format("{0:0.00}M\r\n", (float)this.ExpStart / 1000000f),
						string.Format("{0:0.00}M\r\n", (float)this.TLBB.Exp / 1000000f),
						string.Format("{0:0.00}M ({1:0.00%})\r\n", (float)this.ExpGain / 1000000f, (float)this.ExpGain / (float)this.TLBB.MaxExp),
						string.Format("{0:0.00}M\r\n", (float)num2 / 1000000f),
						string.Format("{0:0.00}M\r\n", (float)this.TLBB.MaxExp / 1000000f),
						string.Format("{0:0.00}M ({1:0.00%})\r\n\r\n", this.ExpSpeed / 1000000f, this.ExpSpeed / (float)this.TLBB.MaxExp),
						text2
					});
				}
				return result;
			}
		}

		// Token: 0x0600070A RID: 1802 RVA: 0x0002A554 File Offset: 0x00028754
		public static string TimeSpanToString(TimeSpan timeSpan)
		{
			if (timeSpan.Days > 0)
			{
				return string.Concat(new object[]
				{
					timeSpan.Days,
					" ngày ",
					timeSpan.Hours,
					" giờ"
				});
			}
			if (timeSpan.Hours > 0)
			{
				return string.Concat(new object[]
				{
					timeSpan.Hours,
					" giờ ",
					timeSpan.Minutes,
					" phút"
				});
			}
			if (timeSpan.Minutes > 0)
			{
				return string.Concat(new object[]
				{
					timeSpan.Minutes,
					" phút ",
					timeSpan.Seconds,
					" giây"
				});
			}
			return timeSpan.Seconds.ToString() + " giây";
		}

		// Token: 0x17000164 RID: 356
		// (get) Token: 0x0600070B RID: 1803 RVA: 0x0002A646 File Offset: 0x00028846
		public bool IsChangeMap
		{
			get
			{
				return !this.Memory.IsRead(this.Address.ParaUseSkill);
			}
		}

		// Token: 0x0600070C RID: 1804
		[DllImport("user32.dll")]
		private static extern uint SetWindowLong(IntPtr hWnd, int nIndex, uint dwNewLong);

		// Token: 0x0600070D RID: 1805
		[DllImport("user32.dll", SetLastError = true)]
		private static extern uint GetWindowLong(IntPtr hWnd, int nIndex);

		// Token: 0x17000165 RID: 357
		// (get) Token: 0x0600070E RID: 1806 RVA: 0x0002A661 File Offset: 0x00028861
		// (set) Token: 0x0600070F RID: 1807 RVA: 0x0002A669 File Offset: 0x00028869
		public bool IsNhanMam { get; set; }

		// Token: 0x06000710 RID: 1808 RVA: 0x0002A674 File Offset: 0x00028874
		public void Init()
		{
			this.FuncLuaToString = this.Memory.Scan("8B4424 08 85C0 56 57 8B7C24 0C 7E 11", "LuaPlus.dll");
			if (this.FuncLuaToString == 0)
			{
				return;
			}
			if (this.Handle != IntPtr.Zero)
			{
				Game.SetHook(this.Handle);
				this.AutoSearch();
				this.SetDll();
				this.DisableActiveGame();
				if (this.Address.GameType == 1)
				{
					this.LuaDoString(ImageResource.Fix3D);
				}
				else if (this.Address.GameType == 2)
				{
					this.LuaDoString(ImageResource.Fix2D);
				}
				else
				{
					this.LuaDoString(ImageResource.FixDG);
				}
				this.LuaDoString(ImageResource.Lua);
				this.Pass2 = Setting.LoadStringOffline("PASS2" + this.TLBB.Id);
				if (Global.AutoAccept && !Global.AcceptAll)
				{
					this.SetTeamFromList(Setting.BuffValue);
				}
				if (this.Address.GameType == 3)
				{
					string text = Setting.LoadWAY(this.TLBB.GuildId.ToString());
					if (text.Contains("@way"))
					{
						this.SetWay(text);
					}
				}
				this.IsInit = true;
				return;
			}
			IntPtr handle = Win.GetHandle(this.Process.Id, "#32770");
			if (handle != IntPtr.Zero)
			{
				this.Memory.Write(this.Address.MultiAcc, 2425393296U, 4);
				this.Memory.Write(this.Address.MultiAcc + 4, 2425393296U, 4);
				Win.PostMessage(handle, 16, 0, 0);
			}
			this.Handle = Win.GetHandle(this.ProcessId, Win.WndClassNames);
		}

		// Token: 0x17000166 RID: 358
		// (get) Token: 0x06000711 RID: 1809 RVA: 0x0002A81B File Offset: 0x00028A1B
		// (set) Token: 0x06000712 RID: 1810 RVA: 0x0002A823 File Offset: 0x00028A23
		private bool IsInit { get; set; }

		// Token: 0x17000167 RID: 359
		// (get) Token: 0x06000713 RID: 1811 RVA: 0x0002A82C File Offset: 0x00028A2C
		public bool ON_SCENE_TRANSING
		{
			get
			{
				return this.Memory.Read(new int[]
				{
					this.Address.ON_SCENE_TRANSING,
					0,
					12,
					100
				}) == 1;
			}
		}

		// Token: 0x17000168 RID: 360
		// (get) Token: 0x06000714 RID: 1812 RVA: 0x0002A85A File Offset: 0x00028A5A
		// (set) Token: 0x06000715 RID: 1813 RVA: 0x0002A862 File Offset: 0x00028A62
		public bool IsPickBTD { get; set; }

		// Token: 0x17000169 RID: 361
		// (get) Token: 0x06000716 RID: 1814 RVA: 0x0002A86C File Offset: 0x00028A6C
		public string Status
		{
			get
			{
				if (this.TrangThaiNhiemVuCoBan != "")
				{
					return this.TrangThaiNhiemVuCoBan;
				}
				if (this.TrangThaiPhuMau != "")
				{
					return this.TrangThaiPhuMau;
				}
				if (this.TLBB.IsODaoCuFull || this.TLBB.IsONguyenLieuFull)
				{
					return "Đầy Tay Nải";
				}
				if (this.HongTrungAcTime != null && this.HongTrungAcTime.Elapsed.TotalSeconds > 150.0)
				{
					return "Hỏng Trừng Ác";
				}
				if (this.IsDome)
				{
					return "Pet Thiếu Hoan Hỉ";
				}
				if (this.IsKyCuoc)
				{
					return "Kỳ Cuộc";
				}
				if (this.MapAcTac != 0)
				{
					return "Ác Tặc";
				}
				if (this.IsTrungAc)
				{
					return "Trừng Ác";
				}
				if (this.IsAcBa)
				{
					return "Ác Bá";
				}
				if (this.TLBB.PlayerState == 2)
				{
					return "Di Chuyển";
				}
				if (this.isAtkFollow && this.IsAttack && this.BestTarget != null)
				{
					return "Đánh Theo Key";
				}
				if (this.isLure && this.IsAttack && this.BestTarget != null)
				{
					return "Lure Quái";
				}
				if (this.TLBB.HPPercent < Global.AlarmHPPercent)
				{
					return "Sắp Hết Máu";
				}
				if (this.IsCheDo)
				{
					return "Chế Đồ";
				}
				if (this.TLBB.IsPk)
				{
					return "Bị PK";
				}
				if (this.ON_SCENE_TRANSING)
				{
					return "Chuyển Cảnh";
				}
				if (this.IdleTime > 10 && this.TLBB.PlayerState == 0)
				{
					return "Rảnh";
				}
				if (this.IdleTime < 10 && this.IsAttack && this.TLBB.PlayerState == 7)
				{
					return "Đánh Quái";
				}
				if (this.TLBB.PlayerState == 8)
				{
					return "Chế";
				}
				if (this.TLBB.PlayerState == 5)
				{
					return "Sử Dùng Phù";
				}
				if (this.TLBB.PlayerState == 6)
				{
					return "Phục Hồi";
				}
				if (!this.TLBB.HaveRide)
				{
					return "Chưa Có Ngựa";
				}
				if (this.TLBB.PlayerState == 0)
				{
					return "Rảnh";
				}
				return "";
			}
		}

		// Token: 0x06000717 RID: 1815 RVA: 0x0002AA80 File Offset: 0x00028C80
		public BaiTrain SelectBaitrain(int level)
		{
			BaiTrain baiTrain = new BaiTrain();
			List<BaiTrain> dsbai = TrainData.dsbai;
			foreach (BaiTrain baiTrain2 in dsbai)
			{
				if (baiTrain2.Level == level)
				{
					return baiTrain2;
				}
			}
			if (baiTrain.Level == 0)
			{
				List<BaiTrain> list = new List<BaiTrain>();
				foreach (BaiTrain baiTrain3 in dsbai)
				{
					if (baiTrain3.Level < level)
					{
						list.Add(baiTrain3);
					}
				}
				list.Sort((BaiTrain x, BaiTrain y) => y.Level.CompareTo(x.Level));
				return list[0];
			}
			return baiTrain;
		}

		// Token: 0x06000718 RID: 1816 RVA: 0x0002AB70 File Offset: 0x00028D70
		public void ThucThiAutoTrain()
		{
			if (!this.AutoTrain)
			{
				Game.baitmp = new BaiTrain();
				return;
			}
			if (this.ON_SCENE_TRANSING)
			{
				return;
			}
			if (this.LenBaiTrain)
			{
				this.LenBaiTrain = false;
			}
			if (this.IsTrungAc)
			{
				return;
			}
			int lvl = this.TLBB.Lvl;
			BaiTrain baiTrain = new BaiTrain();
			baiTrain = this.SelectBaitrain(lvl);
			if (Game.baitmp == baiTrain)
			{
				return;
			}
			if (this.TLBB.MapId != baiTrain.MapID)
			{
				this.IsAttack = false;
				this.TimDuong((float)baiTrain.PosX, (float)baiTrain.PosY, baiTrain.MapID);
				return;
			}
			if (TINHKIEM.GetDistance(this.CharX, this.CharY, (float)baiTrain.PosX, (float)baiTrain.PosY) > 3f)
			{
				this.IsAttack = false;
				this.TimDuong((float)baiTrain.PosX, (float)baiTrain.PosY, baiTrain.MapID);
				return;
			}
			this.DownRide();
			this.IsAttack = true;
			Game.baitmp = baiTrain;
		}

		// Token: 0x1700016A RID: 362
		// (get) Token: 0x06000719 RID: 1817 RVA: 0x0002AC64 File Offset: 0x00028E64
		// (set) Token: 0x0600071A RID: 1818 RVA: 0x0002AC6C File Offset: 0x00028E6C
		public bool IsSaveGold { get; set; }

		// Token: 0x1700016B RID: 363
		// (get) Token: 0x0600071B RID: 1819 RVA: 0x0002AC75 File Offset: 0x00028E75
		// (set) Token: 0x0600071C RID: 1820 RVA: 0x0002AC7D File Offset: 0x00028E7D
		public bool IsTriLieu { get; set; }

		// Token: 0x1700016C RID: 364
		// (get) Token: 0x0600071D RID: 1821 RVA: 0x0002AC86 File Offset: 0x00028E86
		// (set) Token: 0x0600071E RID: 1822 RVA: 0x0002AC8E File Offset: 0x00028E8E
		public bool IsKichAuto { get; set; }

		// Token: 0x0600071F RID: 1823 RVA: 0x0002AC98 File Offset: 0x00028E98
		public void TriLieu()
		{
			if (!this.IsTriLieu)
			{
				return;
			}
			if (this.IsAttack)
			{
				this.IsAttack = false;
			}
			if (Game.TickCount % 18 != 0)
			{
				return;
			}
			if (this.TLBB.HPPercent + this.TLBB.MPPercent > 195)
			{
				this.PushDebugMessage("Sinh lực đã đầy. Không cần trị liệu");
				this.IsTriLieu = false;
				return;
			}
			if (this.TLBB.Gold < 10)
			{
				this.PushDebugMessage("Tiền không đủ không thể trị liệu");
				this.IsTriLieu = false;
				return;
			}
			if (Unity.DangOMapTriLieuHienTai(this.TLBB.MapId))
			{
				NPC npctrilieu = Unity.GETNPCTRILIEU(this.TLBB.MapId);
				if (!this.GoTo((float)npctrilieu.X, (float)npctrilieu.Y, npctrilieu.Map, false))
				{
					return;
				}
				using (List<GameObject>.Enumerator enumerator = this.Objects.AllNpc.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						GameObject gameObject = enumerator.Current;
						if (gameObject.CleanName == "skylong" && this.TLBB.IsQuestOpen)
						{
							if (this.QuestFrame.Click(129, 0))
							{
								break;
							}
							this.QuestFrame.Click(129, 1001);
							this.QuestFrame.Close();
							break;
						}
						else if ((gameObject.CleanName == "dothanhdang" && this.TLBB.IsQuestOpen) || (gameObject.CleanName == "binhsanhan" && this.TLBB.IsQuestOpen))
						{
							int num = 0;
							if (num == 0 && this.TLBB.IsQuestOpen)
							{
								this.QuestFrame.Click(64, 0);
								num = 1;
							}
							if (num == 1 && this.TLBB.IsQuestOpen)
							{
								this.QuestFrame.Click(64, 1001);
							}
						}
						else if (gameObject.CleanName == "longbathien" || gameObject.CleanName == "binhsanhan" || gameObject.CleanName == "dothanhdang")
						{
							this.Talk(gameObject.Id);
						}
					}
					return;
				}
			}
			NPC npctrilieu2 = Unity.GETNPCTRILIEU(Option.MaptriLieuIndex);
			if (this.GoTo((float)npctrilieu2.X, (float)npctrilieu2.Y, npctrilieu2.Map, false))
			{
				foreach (GameObject gameObject2 in this.Objects.AllNpc)
				{
					if (gameObject2.CleanName == "skylong" && this.TLBB.IsQuestOpen)
					{
						if (this.QuestFrame.Click(129, 0))
						{
							break;
						}
						this.QuestFrame.Click(129, 1001);
						this.QuestFrame.Close();
						break;
					}
					else if ((gameObject2.CleanName == "dothanhdang" && this.TLBB.IsQuestOpen) || (gameObject2.CleanName == "binhsanhan" && this.TLBB.IsQuestOpen))
					{
						int num2 = 0;
						if (num2 == 0 && this.TLBB.IsQuestOpen)
						{
							this.QuestFrame.Click(64, 0);
							num2 = 1;
						}
						if (num2 == 1 && this.TLBB.IsQuestOpen)
						{
							this.QuestFrame.Click(64, 1001);
						}
					}
					else if (gameObject2.CleanName == "longbathien" || gameObject2.CleanName == "binhsanhan" || gameObject2.CleanName == "dothanhdang")
					{
						this.Talk(gameObject2.Id);
					}
				}
			}
		}

		// Token: 0x06000720 RID: 1824 RVA: 0x0002B098 File Offset: 0x00029298
		public void SaveGold()
		{
			if (!this.IsSaveGold)
			{
				return;
			}
			if (Game.TickCount % 18 != 0)
			{
				return;
			}
			if (this.GoTo(this.TLBB.NPCThuongKho))
			{
				if (!this.TLBB.IsBankOpen)
				{
					if (!this.TLBB.IsQuestOpen)
					{
						this.Talk(this.TLBB.NPCThuongKho.Id);
						return;
					}
					using (List<QuestFrame>.Enumerator enumerator = QuestFrame.Enum(this).GetEnumerator())
					{
						while (enumerator.MoveNext())
						{
							QuestFrame questFrame = enumerator.Current;
							if (questFrame.StrOptionExtra1 == 7 && questFrame.StrOptionExtra2 == -1)
							{
								this.QuestFrameOptionClicked(questFrame);
								break;
							}
						}
						return;
					}
				}
				if (this.TLBB.Gold == 0)
				{
					this.IsSaveGold = false;
				}
				this.LuaDoOneLineString("Bank:SaveMoneyToBank(" + this.TLBB.Gold.ToString() + ");");
				this.CloseQuest();
			}
		}

		// Token: 0x06000721 RID: 1825 RVA: 0x0002B19C File Offset: 0x0002939C
		private void AutoSearch()
		{
			if (this.Address.QuestInfo[0] == 0)
			{
				int num = this.Memory.ScanString(TINHKIEM.Sign.QuestInfo);
				num = this.Memory.Scan(this.Memory.ReverseString(num.ToString("X8")));
				num = this.Memory.Read(num + 16);
				this.Address.QuestInfo[0] = num;
			}
			if (this.Address.IsBankOpen[0] == 0)
			{
				int num2 = this.Memory.ScanString("UPDATE_BANK");
				num2 = this.Memory.Scan(this.Memory.ReverseString(num2.ToString("X8")));
				num2 = this.Memory.Read(num2 + 16);
				this.Address.IsBankOpen[0] = num2;
			}
			if (this.Address.ON_SCENE_TRANSING == 0)
			{
				int num3 = this.Memory.ScanString("ON_SCENE_TRANSING");
				num3 = this.Memory.Scan(this.Memory.ReverseString(num3.ToString("X8")));
				num3 = this.Memory.Read(num3 + 16);
				this.Address.ON_SCENE_TRANSING = num3;
			}
			if (this.Address.CountDown10Sec[0] == 0)
			{
				int num4 = this.Memory.ScanString("COUNTDOWN_10SEC");
				num4 = this.Memory.Scan(this.Memory.ReverseString(num4.ToString("X8")));
				num4 = this.Memory.Read(num4 + 16);
				this.Address.CountDown10Sec[0] = num4;
			}
			if (this.Address.IsShopOpen[0] == 0)
			{
				int num5 = this.Memory.ScanString("UPDATE_BOOTH");
				num5 = this.Memory.Scan(this.Memory.ReverseString(num5.ToString("X8")));
				num5 = this.Memory.Read(num5 + 16);
				this.Address.IsShopOpen[0] = num5;
			}
			this.FuncLuaToString = this.Memory.Scan("8B4424 08 85C0 56 57 8B7C24 0C 7E 11", "LuaPlus.dll");
		}

		// Token: 0x06000722 RID: 1826 RVA: 0x0002B3AA File Offset: 0x000295AA
		public void Jump()
		{
			this.PostMessage(0, 120);
		}

		// Token: 0x1700016D RID: 365
		// (get) Token: 0x06000723 RID: 1827 RVA: 0x0002B3B5 File Offset: 0x000295B5
		// (set) Token: 0x06000724 RID: 1828 RVA: 0x0002B3BD File Offset: 0x000295BD
		private int FuncLuaToString { get; set; }

		// Token: 0x1700016E RID: 366
		// (get) Token: 0x06000725 RID: 1829 RVA: 0x0002B3C6 File Offset: 0x000295C6
		// (set) Token: 0x06000726 RID: 1830 RVA: 0x0002B3CD File Offset: 0x000295CD
		public static int LastFuncLuaToString { get; set; }

		// Token: 0x06000727 RID: 1831 RVA: 0x0002B3D8 File Offset: 0x000295D8
		public void SaveSkill()
		{
			string text = "";
			foreach (Skill skill in this.Skills)
			{
				if (skill.Use)
				{
					text = text + "-" + skill.PacketId.ToString();
				}
			}
			text += "-";
			Setting.SaveSettingOffline(this.TLBB.Id + "SKILL", text);
		}

		// Token: 0x06000728 RID: 1832 RVA: 0x0002B470 File Offset: 0x00029670
		public void SaveSkillPK()
		{
			string text = "";
			foreach (Skill skill in this.Skills)
			{
				if (skill.UsePK)
				{
					text = text + "-" + skill.PacketId.ToString();
				}
			}
			text += "-";
			Setting.SaveSettingOffline(this.TLBB.Id + "SKILLPK", text);
		}

		// Token: 0x06000729 RID: 1833 RVA: 0x0002B508 File Offset: 0x00029708
		public void SaveSkillBuff()
		{
			string text = "";
			foreach (Skill skill in this.Skills)
			{
				if (skill.UserBuff)
				{
					text = text + "-" + skill.PacketId.ToString();
				}
			}
			text += "-";
			Setting.SaveSettingOffline(this.TLBB.Id + "SKILLBUFF", text);
		}

		// Token: 0x0600072A RID: 1834 RVA: 0x0002B5A0 File Offset: 0x000297A0
		public void SendPacket(string hex)
		{
			int wParam = this.Memory.WriteHex(hex);
			this.PostMessage(wParam, 114);
		}

		// Token: 0x0600072B RID: 1835 RVA: 0x0002B5C3 File Offset: 0x000297C3
		public bool IsNhiemVu()
		{
			return this.IsBachHoaDuyen || this.IsXayDung;
		}

		// Token: 0x1700016F RID: 367
		// (get) Token: 0x0600072C RID: 1836 RVA: 0x0002B5D5 File Offset: 0x000297D5
		// (set) Token: 0x0600072D RID: 1837 RVA: 0x0002B5DD File Offset: 0x000297DD
		public bool IsSuspend { get; set; }

		// Token: 0x17000170 RID: 368
		// (get) Token: 0x0600072E RID: 1838 RVA: 0x0002B5E6 File Offset: 0x000297E6
		// (set) Token: 0x0600072F RID: 1839 RVA: 0x0002B5EE File Offset: 0x000297EE
		private bool IsOut { get; set; }

		// Token: 0x17000171 RID: 369
		// (get) Token: 0x06000730 RID: 1840 RVA: 0x0002B5F7 File Offset: 0x000297F7
		// (set) Token: 0x06000731 RID: 1841 RVA: 0x0002B5FF File Offset: 0x000297FF
		private Stopwatch LyThuThuyDead { get; set; }

		// Token: 0x17000172 RID: 370
		// (get) Token: 0x06000732 RID: 1842 RVA: 0x0002B608 File Offset: 0x00029808
		// (set) Token: 0x06000733 RID: 1843 RVA: 0x0002B610 File Offset: 0x00029810
		private bool IsLyThuThuyDead { get; set; }

		// Token: 0x17000173 RID: 371
		// (get) Token: 0x06000734 RID: 1844 RVA: 0x0002B619 File Offset: 0x00029819
		// (set) Token: 0x06000735 RID: 1845 RVA: 0x0002B621 File Offset: 0x00029821
		private int ClickTime { get; set; }

		// Token: 0x17000174 RID: 372
		// (get) Token: 0x06000736 RID: 1846 RVA: 0x0002B62A File Offset: 0x0002982A
		// (set) Token: 0x06000737 RID: 1847 RVA: 0x0002B632 File Offset: 0x00029832
		private bool OLaoDaiDead { get; set; }

		// Token: 0x17000175 RID: 373
		// (get) Token: 0x06000738 RID: 1848 RVA: 0x0002B63B File Offset: 0x0002983B
		// (set) Token: 0x06000739 RID: 1849 RVA: 0x0002B643 File Offset: 0x00029843
		private Stopwatch DieTime { get; set; }

		// Token: 0x17000176 RID: 374
		// (get) Token: 0x0600073A RID: 1850 RVA: 0x0002B64C File Offset: 0x0002984C
		// (set) Token: 0x0600073B RID: 1851 RVA: 0x0002B654 File Offset: 0x00029854
		private bool IsTalkPhuManNghi { get; set; }

		// Token: 0x17000177 RID: 375
		// (get) Token: 0x0600073C RID: 1852 RVA: 0x0002B65D File Offset: 0x0002985D
		// (set) Token: 0x0600073D RID: 1853 RVA: 0x0002B665 File Offset: 0x00029865
		private bool IsTalkOLaoDai { get; set; }

		// Token: 0x17000178 RID: 376
		// (get) Token: 0x0600073E RID: 1854 RVA: 0x0002B66E File Offset: 0x0002986E
		// (set) Token: 0x0600073F RID: 1855 RVA: 0x0002B676 File Offset: 0x00029876
		private bool IsCapDaiBaDie { get; set; }

		// Token: 0x17000179 RID: 377
		// (get) Token: 0x06000740 RID: 1856 RVA: 0x0002B67F File Offset: 0x0002987F
		// (set) Token: 0x06000741 RID: 1857 RVA: 0x0002B687 File Offset: 0x00029887
		private bool IsTangThoCongDie { get; set; }

		// Token: 0x1700017A RID: 378
		// (get) Token: 0x06000742 RID: 1858 RVA: 0x0002B690 File Offset: 0x00029890
		// (set) Token: 0x06000743 RID: 1859 RVA: 0x0002B698 File Offset: 0x00029898
		private bool IsOLaoDaiDie { get; set; }

		// Token: 0x1700017B RID: 379
		// (get) Token: 0x06000744 RID: 1860 RVA: 0x0002B6A1 File Offset: 0x000298A1
		// (set) Token: 0x06000745 RID: 1861 RVA: 0x0002B6A9 File Offset: 0x000298A9
		private bool IsNhamBinhSinhDie { get; set; }

		// Token: 0x1700017C RID: 380
		// (get) Token: 0x06000746 RID: 1862 RVA: 0x0002B6B2 File Offset: 0x000298B2
		// (set) Token: 0x06000747 RID: 1863 RVA: 0x0002B6BA File Offset: 0x000298BA
		private bool IsLyThuThuyDie { get; set; }

		// Token: 0x06000748 RID: 1864 RVA: 0x0002B6C4 File Offset: 0x000298C4
		public void DatDoiAcTac()
		{
			if (Game.TickCount % 9 != 0)
			{
				return;
			}
			if (this.PickItem())
			{
				return;
			}
			if (this.MapAcTac == 0)
			{
				return;
			}
			this.TrieuTap();
			if (this.TLBB.MapId != MAP.TacKhauDoanhDia && !this.IsRide && this.TLBB.HaveRide)
			{
				this.StopFollow();
				this.UpRide();
				return;
			}
			if (this.IsBossDie)
			{
				if (!this.IsRide && this.TLBB.HaveRide)
				{
					this.StopFollow();
					this.UpRide();
					return;
				}
				if (this.ClearTime.Elapsed.TotalSeconds > 40.0)
				{
					this.IsBossDie = false;
					this.MapATIndex = 0;
					this.ClearTime = Stopwatch.StartNew();
				}
				if (this.ClearTime.Elapsed.TotalSeconds > 20.0)
				{
					this.AskTeamFollow();
					return;
				}
			}
			int[,] acTacPoint = this.AcTacPoint;
			if (this.MapATIndex == -1)
			{
				this.MapATIndex++;
			}
			if (this.MapATIndex <= acTacPoint.GetLength(0) - 1 && TINHKIEM.GetDistance(this.CharX, this.CharY, (float)acTacPoint[this.MapATIndex, 0], (float)acTacPoint[this.MapATIndex, 1]) <= 2f)
			{
				this.MapATIndex++;
			}
			if (this.CurMapATIndex != -1 && this.CurMapATIndex <= acTacPoint.GetLength(0) - 1 && this.Objects.NearMonter((float)acTacPoint[this.CurMapATIndex, 0], (float)acTacPoint[this.CurMapATIndex, 1], 12f).Count > 0)
			{
				this.ClearTime = Stopwatch.StartNew();
			}
			if (this.TLBB.MapId != this.MapAcTac && this.TLBB.MapId != MAP.TacKhauDoanhDia)
			{
				this.GoTo((float)acTacPoint[0, 0], (float)acTacPoint[0, 1], this.MapAcTac, false);
				return;
			}
			if (this.TLBB.MapId == this.MapAcTac)
			{
				if (!this.TalkNPCPhuBan())
				{
					if (!this.IsMoveEx)
					{
						this.FixKetMap();
						return;
					}
					this.MoveNext();
				}
				return;
			}
			if (this.TLBB.MapId == MAP.TacKhauDoanhDia)
			{
				if (this.ClearTime.Elapsed.TotalSeconds > 2.0 || this.MapATIndex == 0)
				{
					if (this.MapATIndex <= acTacPoint.GetLength(0) - 1)
					{
						if (!this.IsRide && this.TLBB.HaveRide)
						{
							this.UpRide();
							return;
						}
						this.AskTeamFollow();
						this.Move((float)acTacPoint[this.MapATIndex, 0], (float)acTacPoint[this.MapATIndex, 1]);
						this.CurMapATIndex = this.MapATIndex;
						return;
					}
					else
					{
						if (this.ClearTime.Elapsed.TotalSeconds > 22.0)
						{
							if (this.IsRide && this.TLBB.HaveRide)
							{
								this.UpRide();
								return;
							}
							this.AskTeamFollow();
						}
						if (this.ClearTime.Elapsed.TotalSeconds > 10.0)
						{
							this.IsBossDie = true;
							return;
						}
					}
				}
				if (this.ClearTime.Elapsed.TotalSeconds < 1.0 && this.TimeStand.Elapsed.TotalSeconds >= 2.0)
				{
					if (this.IsRide && this.IsAuto)
					{
						this.DownRide();
					}
					this.StopFollow();
				}
			}
		}

		// Token: 0x1700017D RID: 381
		// (get) Token: 0x06000749 RID: 1865 RVA: 0x0002BA4F File Offset: 0x00029C4F
		// (set) Token: 0x0600074A RID: 1866 RVA: 0x0002BA57 File Offset: 0x00029C57
		public string TKCInfo { get; set; }

		// Token: 0x1700017E RID: 382
		// (get) Token: 0x0600074B RID: 1867 RVA: 0x0002BA60 File Offset: 0x00029C60
		// (set) Token: 0x0600074C RID: 1868 RVA: 0x0002BA68 File Offset: 0x00029C68
		public int TKCState { get; set; }

		// Token: 0x1700017F RID: 383
		// (get) Token: 0x0600074D RID: 1869 RVA: 0x0002BA71 File Offset: 0x00029C71
		// (set) Token: 0x0600074E RID: 1870 RVA: 0x0002BA79 File Offset: 0x00029C79
		public Stopwatch TKCStateTime { get; set; }

		// Token: 0x17000180 RID: 384
		// (get) Token: 0x0600074F RID: 1871 RVA: 0x0002BA82 File Offset: 0x00029C82
		// (set) Token: 0x06000750 RID: 1872 RVA: 0x0002BA8A File Offset: 0x00029C8A
		public bool IsTraiTKC { get; set; }

		// Token: 0x17000181 RID: 385
		// (get) Token: 0x06000751 RID: 1873 RVA: 0x0002BA93 File Offset: 0x00029C93
		// (set) Token: 0x06000752 RID: 1874 RVA: 0x0002BA9B File Offset: 0x00029C9B
		public bool TKCComplete { get; set; }

		// Token: 0x17000182 RID: 386
		// (get) Token: 0x06000753 RID: 1875 RVA: 0x0002BAA4 File Offset: 0x00029CA4
		// (set) Token: 0x06000754 RID: 1876 RVA: 0x0002BAAC File Offset: 0x00029CAC
		public bool CheckTKCComplete { get; set; }

		// Token: 0x17000183 RID: 387
		// (get) Token: 0x06000755 RID: 1877 RVA: 0x0002BAB5 File Offset: 0x00029CB5
		// (set) Token: 0x06000756 RID: 1878 RVA: 0x0002BABD File Offset: 0x00029CBD
		public bool TKCompleted { get; set; }

		// Token: 0x06000757 RID: 1879 RVA: 0x0002BAC8 File Offset: 0x00029CC8
		public void DatDoiTKC()
		{
			if (Game.TickCount % 9 != 0 || Global.IsVIP == 0)
			{
				return;
			}
			if (this.MapTKC == 0)
			{
				return;
			}
			if (this.Objects.NearMonter12m.Count > 0)
			{
				this.ClearTime = Stopwatch.StartNew();
			}
			if (this.Objects.NearMonter20m.Count > 0 && this.TimeStand.Elapsed.TotalSeconds > 0.5)
			{
				if (this.TLBB.IsRide && this.IsAuto)
				{
					this.DownRide();
				}
				this.StopFollow();
			}
			if (this.TLBB.MapId != MAP.TangKinhCac && !this.IsRide && this.TLBB.HaveRide)
			{
				this.StopFollow();
				this.UpRide();
				return;
			}
			this.TrieuTap();
			if (this.TKCComplete && !this.CheckTKCComplete)
			{
				if (TINHKIEM.GetDistance(this.CharX, this.CharY, 64f, 100f) > 1f)
				{
					this.Move(64f, 100f);
					return;
				}
				if (this.IdleTime > 2 && this.ClearTime.Elapsed.TotalSeconds > 2.0)
				{
					if (!this.IsRide && this.TLBB.HaveRide)
					{
						this.UpRide();
						return;
					}
					this.CheckTKCComplete = true;
					this.AskTeamFollow();
				}
				return;
			}
			else if (this.CheckTKCComplete && !this.TKCompleted)
			{
				if (TINHKIEM.GetDistance(this.CharX, this.CharY, 64f, 28f) > 3f)
				{
					this.Move(64f, 28f);
					this.ClearTime = Stopwatch.StartNew();
					return;
				}
				if (this.ClearTime.Elapsed.TotalSeconds > 11.0 && this.IdleTime > 2)
				{
					this.TKCompleted = true;
				}
				return;
			}
			else
			{
				if (!this.TKCompleted)
				{
					if (this.TLBB.MapId != this.MapTKC && this.TLBB.MapId != MAP.TangKinhCac)
					{
						int[,] array = this.MapPOINT(this.MapTKC);
						if (array != null)
						{
							this.GoTo((float)array[0, 0], (float)array[0, 1], this.MapTKC, false);
							return;
						}
					}
					else
					{
						if (this.TLBB.MapId == this.MapTKC && !this.TalkNPCPhuBan())
						{
							if (!this.IsMove)
							{
								this.FixKetMap();
								return;
							}
							this.MoveNext();
						}
						if (this.TLBB.MapId == MAP.TangKinhCac)
						{
							if (this.PickItem())
							{
								return;
							}
							if (this.TKCState == 0)
							{
								this.TKCState = 1;
								this.LuaDoOneLineString("return TKCINFO;");
								this.LuaToString();
							}
							else
							{
								this.TKCState = 0;
								string text = this.LuaString();
								if (text != this.TKCInfo)
								{
									this.TKCInfo = text;
									this.TKCStateTime = Stopwatch.StartNew();
								}
							}
							if (this.Objects.NearMonter20m.Count == 0)
							{
								if (!this.IsRide && this.TLBB.HaveRide)
								{
									this.UpRide();
									return;
								}
								this.AskTeamFollow();
							}
							else if (this.Talked)
							{
								return;
							}
							if (((TINHKIEM.GetDistance(this.CharX, this.CharY, 64f, 100f) < 8f && this.ClearTime.Elapsed.TotalSeconds > 2.0) || this.TKCInfo == "#{CJG_090605_4}") && this.TKCInfo != "")
							{
								if (this.IsTraiTKC)
								{
									this.Move(97f, 64f);
									return;
								}
								this.Move(27f, 64f);
								return;
							}
							else if (this.TKCInfo == "")
							{
								if (this.Talked)
								{
									this.Move(97f, 64f);
									return;
								}
							}
							else
							{
								if (this.TKCInfo.Contains("1/10") || this.TKCInfo.Contains("3/10") || this.TKCInfo.Contains("5/10") || this.TKCInfo.Contains("7/10") || this.TKCInfo.Contains("9/10"))
								{
									this.IsTraiTKC = false;
									if (this.TKCStateTime.Elapsed.TotalSeconds < 10.0)
									{
										this.Move(97f, 64f);
										return;
									}
								}
								if (this.TKCInfo.Contains("2/10") || this.TKCInfo.Contains("4/10") || this.TKCInfo.Contains("6/10") || this.TKCInfo.Contains("8/10") || this.TKCInfo.Contains("10/10"))
								{
									this.IsTraiTKC = true;
									if (this.TKCStateTime.Elapsed.TotalSeconds < 10.0)
									{
										this.Move(27f, 64f);
										return;
									}
								}
								if (this.TKCInfo.Contains("10/10"))
								{
									this.TKCComplete = true;
								}
								if (this.TKCStateTime.Elapsed.TotalSeconds > 20.0)
								{
									this.Move(64f, 100f);
									return;
								}
							}
						}
					}
					return;
				}
				if (!this.IsRide && this.TLBB.HaveRide)
				{
					this.UpRide();
					return;
				}
				this.AskTeamFollow();
				this.Move(70f, 20f);
				return;
			}
		}

		// Token: 0x17000184 RID: 388
		// (get) Token: 0x06000758 RID: 1880 RVA: 0x0002C054 File Offset: 0x0002A254
		// (set) Token: 0x06000759 RID: 1881 RVA: 0x0002C05C File Offset: 0x0002A25C
		private bool IsBossDie { get; set; }

		// Token: 0x17000185 RID: 389
		// (get) Token: 0x0600075A RID: 1882 RVA: 0x0002C065 File Offset: 0x0002A265
		// (set) Token: 0x0600075B RID: 1883 RVA: 0x0002C06D File Offset: 0x0002A26D
		private int CurIndex { get; set; }

		// Token: 0x17000186 RID: 390
		// (get) Token: 0x0600075C RID: 1884 RVA: 0x0002C076 File Offset: 0x0002A276
		// (set) Token: 0x0600075D RID: 1885 RVA: 0x0002C07E File Offset: 0x0002A27E
		public bool IsTrieuTap { get; set; }

		// Token: 0x17000187 RID: 391
		// (get) Token: 0x0600075E RID: 1886 RVA: 0x0002C087 File Offset: 0x0002A287
		// (set) Token: 0x0600075F RID: 1887 RVA: 0x0002C08F File Offset: 0x0002A28F
		public bool IsAcBa { get; set; }

		// Token: 0x17000188 RID: 392
		// (get) Token: 0x06000760 RID: 1888 RVA: 0x0002C098 File Offset: 0x0002A298
		// (set) Token: 0x06000761 RID: 1889 RVA: 0x0002C0A0 File Offset: 0x0002A2A0
		public bool IsPhungHoangLangMo { get; set; }

		// Token: 0x17000189 RID: 393
		// (get) Token: 0x06000762 RID: 1890 RVA: 0x0002C0A9 File Offset: 0x0002A2A9
		// (set) Token: 0x06000763 RID: 1891 RVA: 0x0002C0B1 File Offset: 0x0002A2B1
		public bool IsQ123LauLan { get; set; }

		// Token: 0x1700018A RID: 394
		// (get) Token: 0x06000764 RID: 1892 RVA: 0x0002C0BA File Offset: 0x0002A2BA
		// (set) Token: 0x06000765 RID: 1893 RVA: 0x0002C0C2 File Offset: 0x0002A2C2
		public bool IsQ123ToChau { get; set; }

		// Token: 0x1700018B RID: 395
		// (get) Token: 0x06000766 RID: 1894 RVA: 0x0002C0CB File Offset: 0x0002A2CB
		// (set) Token: 0x06000767 RID: 1895 RVA: 0x0002C0D3 File Offset: 0x0002A2D3
		public bool IsTheoQ { get; set; }

		// Token: 0x06000768 RID: 1896 RVA: 0x0002C0DC File Offset: 0x0002A2DC
		public void DatDoiQ123LauLan()
		{
			if (!this.IsQ123LauLan || Global.IsVIP == 0)
			{
				return;
			}
			if (Game.TickCount % 18 != 0)
			{
				return;
			}
			if (this.TLBB.MapId != MAP.ViemMaSon && !this.IsRide && this.TLBB.HaveRide)
			{
				this.StopFollow();
				this.UpRide();
				return;
			}
			this.TrieuTap();
			if (this.PickItem())
			{
				return;
			}
			int[,] viemMaSonPoint = this.ViemMaSonPoint;
			if (this.CurMapATIndex != -1 && this.CurMapATIndex <= viemMaSonPoint.GetLength(0) - 1 && this.Objects.NearMonter((float)viemMaSonPoint[this.CurMapATIndex, 0], (float)viemMaSonPoint[this.CurMapATIndex, 1], 12f).Count > 0)
			{
				this.ClearTime = Stopwatch.StartNew();
			}
			if (this.TLBB.MapId != MAP.ViemMaSon)
			{
				if (this.GoTo(LAULAN.HaDuyet))
				{
					this.IsP = true;
				}
			}
			else
			{
				if (this.MapATIndex == -1)
				{
					this.MapATIndex++;
				}
				if (this.MapATIndex <= viemMaSonPoint.GetLength(0) - 1 && TINHKIEM.GetDistance(this.CharX, this.CharY, (float)viemMaSonPoint[this.MapATIndex, 0], (float)viemMaSonPoint[this.MapATIndex, 1]) <= 3f)
				{
					this.MapATIndex++;
				}
				if (this.ClearTime.Elapsed.TotalSeconds > 3.5 || this.MapATIndex == 0)
				{
					if (this.MapATIndex == 8 && this.ClearTime.Elapsed.TotalSeconds < 30.0)
					{
						return;
					}
					if (this.MapATIndex == 7 && this.TLBB.PlayerState == 2)
					{
						this.ClearTime = Stopwatch.StartNew();
					}
					if (this.MapATIndex <= viemMaSonPoint.GetLength(0) - 1)
					{
						if (!this.IsRide && this.TLBB.HaveRide)
						{
							this.UpRide();
							return;
						}
						this.AskTeamFollow();
						this.GoTo((float)viemMaSonPoint[this.MapATIndex, 0], (float)viemMaSonPoint[this.MapATIndex, 1], false);
						this.CurMapATIndex = this.MapATIndex;
						return;
					}
				}
				if (this.ClearTime.Elapsed.TotalSeconds < 1.0 && this.TimeStand.Elapsed.TotalSeconds >= 2.0)
				{
					if (this.TLBB.IsRide && this.IsAuto)
					{
						this.DownRide();
					}
					this.StopFollow();
				}
			}
			if (this.ClearTime.Elapsed.TotalSeconds > 65.0 && this.TLBB.MapId == MAP.ViemMaSon)
			{
				this.IsBossDie = false;
				this.MapATIndex = -1;
				this.ClearTime = Stopwatch.StartNew();
			}
		}

		// Token: 0x06000769 RID: 1897 RVA: 0x0002C3C0 File Offset: 0x0002A5C0
		public void DatDoiAcBa()
		{
			if (this.AcBa != this.TLBB.Menpai && this.AcBa != -1)
			{
				return;
			}
			if (!this.IsAcBa)
			{
				return;
			}
			if (Game.TickCount % 9 != 0)
			{
				return;
			}
			if (this.PickItem())
			{
				return;
			}
			if (this.TLBB.MapAcBa == -1)
			{
				return;
			}
			if (!this.IsMapPhuBan() && !this.IsRide && this.TLBB.HaveRide)
			{
				this.StopFollow();
				this.UpRide();
				return;
			}
			this.TrieuTap();
			int[,] acBaPoint = this.AcBaPoint;
			if (!this.IsMapPhuBan())
			{
				if (this.TLBB.MapId != this.TLBB.MapMonPhai)
				{
					this.GoTo((float)acBaPoint[0, 0], (float)acBaPoint[0, 1], this.TLBB.MapMonPhai, false);
					return;
				}
				if (this.TLBB.MapId == this.TLBB.MapMonPhai)
				{
					if (!this.TalkNPCPhuBan())
					{
						if (!this.IsMoveEx)
						{
							this.FixKetMap();
							return;
						}
						this.MoveNext();
					}
					return;
				}
			}
			else if (this.Objects.NearMonter18m.Count > 0)
			{
				this.ClearTime = Stopwatch.StartNew();
				if (this.IsRide && this.IsAuto)
				{
					this.DownRide();
				}
				this.StopFollow();
			}
			if (this.IsMapPhuBan())
			{
				foreach (GameObject gameObject in this.Objects.All)
				{
					if ((gameObject.CleanName == "acba" || gameObject.CleanName == "tyrant") && gameObject.HP == 0f && !this.IsBossDie)
					{
						this.IsBossDie = true;
						this.BossDieTime = Stopwatch.StartNew();
					}
				}
				if (this.IsBossDie)
				{
					this.MoveIndex = -1;
					if (this.BossDieTime.Elapsed.TotalSeconds > 15.0)
					{
						if (!this.IsRide && this.TLBB.HaveRide)
						{
							this.UpRide();
							return;
						}
						this.AskTeamFollow();
					}
					return;
				}
				if (this.ClearTime.Elapsed.TotalSeconds > 2.0)
				{
					if (!this.IsRide && this.TLBB.HaveRide)
					{
						this.UpRide();
						return;
					}
					this.AskTeamFollow();
					this.MoveNext();
				}
				if (this.ClearTime.Elapsed.TotalSeconds < 1.0 && this.TimeStand.Elapsed.TotalSeconds >= 2.0)
				{
					if (this.IsRide && this.IsAuto)
					{
						this.DownRide();
					}
					this.StopFollow();
				}
			}
			if (this.ClearTime.Elapsed.TotalSeconds > 40.0 && this.TLBB.MapId == this.TLBB.MapAcBa)
			{
				this.IsBossDie = false;
				this.MapATIndex = -1;
				this.ClearTime = Stopwatch.StartNew();
			}
		}

		// Token: 0x1700018C RID: 396
		// (get) Token: 0x0600076A RID: 1898 RVA: 0x0002C6DC File Offset: 0x0002A8DC
		// (set) Token: 0x0600076B RID: 1899 RVA: 0x0002C6E4 File Offset: 0x0002A8E4
		private bool DaNhanThuyLao { get; set; }

		// Token: 0x0600076C RID: 1900 RVA: 0x0002C6F0 File Offset: 0x0002A8F0
		private void DiThuyLao()
		{
			if (this.TLBB.MapId != MAP.ThaiHo && this.TLBB.MapId != MAP.ToChau && this.TLBB.MapId != MAP.ThuyLao)
			{
				this.TimDuong((float)THAIHO.HoDienKhanh.X, (float)THAIHO.HoDienKhanh.Y, MAP.ToChau);
				return;
			}
			if (this.GoTo(THAIHO.HoDienKhanh))
			{
				if (this.TLBB.IsQuestOpen)
				{
					if (!this.IsClick)
					{
						this.QuestFrameOptionClicked(232002, -1);
						this.IsClick = true;
						return;
					}
					this.QuestFrameAccept();
					this.IsClick = false;
					this.CloseQuest();
					return;
				}
				else
				{
					this.Talk(THAIHO.HoDienKhanh);
				}
			}
		}

		// Token: 0x0600076D RID: 1901 RVA: 0x0002C7AC File Offset: 0x0002A9AC
		private void NhanThuyLao()
		{
			if (this.DaNhanThuyLao)
			{
				return;
			}
			if (this.TrangThaiThuyLao == "")
			{
				if (!this.TLBB.IsTogleMission && !this.TLBB.IsTogleMission)
				{
					this.PostMessage(18, 105);
				}
				this.TrangThaiThuyLao = "OpenMission";
				return;
			}
			if (this.TrangThaiThuyLao == "OpenMission")
			{
				this.PostMessage(18, 105);
				this.TrangThaiThuyLao = "CloseMission";
				return;
			}
			if (this.TrangThaiThuyLao == "CloseMission")
			{
				using (List<Task>.Enumerator enumerator = Task.Enum(this).GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						if (enumerator.Current.ClearName.Contains("binhdinhthuylao"))
						{
							this.DaNhanThuyLao = true;
							this.TrangThaiThuyLao = "";
							return;
						}
					}
				}
				this.TrangThaiThuyLao = "NhanThuyLao";
				return;
			}
			if (this.TrangThaiThuyLao == "NhanThuyLao")
			{
				if (this.TLBB.MapId != MAP.ThaiHo && this.TLBB.MapId != MAP.ToChau && this.TLBB.MapId != MAP.ThuyLao)
				{
					this.TimDuong((float)TOCHAU.HoDienBao.X, (float)TOCHAU.HoDienBao.Y, MAP.ToChau);
					return;
				}
				if (this.GoTo(TOCHAU.HoDienBao))
				{
					if (this.TLBB.IsQuestOpen)
					{
						if (!this.IsClick)
						{
							this.QuestFrameOptionClicked(232000, -1);
							this.IsClick = true;
							return;
						}
						this.QuestFrameAccept();
						this.IsClick = false;
						this.CloseQuest();
						this.TrangThaiThuyLao = "";
						return;
					}
					else
					{
						this.Talk(TOCHAU.HoDienBao);
					}
				}
				return;
			}
		}

		// Token: 0x0600076E RID: 1902 RVA: 0x0002C980 File Offset: 0x0002AB80
		public void DatDoiThuyLao()
		{
			if (!this.IsThuyLao)
			{
				return;
			}
			if (Game.TickCount % 18 != 0)
			{
				return;
			}
			if (this.PickItem())
			{
				return;
			}
			this.TrieuTap();
			if (this.TLBB.MapId == MAP.ThuyLao)
			{
				if (this.IsXongThuyLao)
				{
					this.GoTo(94f, 94f, false);
					return;
				}
				if (this.Objects.NearMonter15m.Count > 0)
				{
					if (this.TLBB.PlayerState == 0)
					{
						if (this.TLBB.IsFollow)
						{
							this.StopFollow();
						}
						if (this.TLBB.IsRide)
						{
							this.DownRide();
						}
					}
					this.ClearTime = Stopwatch.StartNew();
				}
				if (this.ClearTime.Elapsed.TotalSeconds > 2.0)
				{
					if (!this.IsRide && this.TLBB.HaveRide)
					{
						this.UpRide();
						return;
					}
					this.MoveNext();
					return;
				}
			}
			else
			{
				if (!this.DaNhanThuyLao)
				{
					this.NhanThuyLao();
					return;
				}
				this.DiThuyLao();
			}
		}

		// Token: 0x0600076F RID: 1903 RVA: 0x0002CA88 File Offset: 0x0002AC88
		public void DatDoiKyCuoc()
		{
			if (this.IsXongKyCuoc)
			{
				this.IsKyCuoc = false;
				return;
			}
			if (!this.IsKyCuoc)
			{
				return;
			}
			if (Game.TickCount % 18 != 0)
			{
				return;
			}
			if (this.PickItem())
			{
				return;
			}
			if (!this.IsRide && this.TLBB.HaveRide && this.TLBB.MapId != MAP.TranLongKyCuoc)
			{
				this.StopFollow();
				this.UpRide();
				return;
			}
			this.TrieuTap();
			if (this.TLBB.MapId != MAP.TranLongKyCuoc)
			{
				if (this.GoTo(LACDUONG.VuongTichTan))
				{
					if (this.TLBB.IsQuestOpen)
					{
						this.QuestFrameOptionClicked(401001, -1);
						this.CloseQuest();
						return;
					}
					this.Talk(LACDUONG.VuongTichTan.Id);
					return;
				}
			}
			else if (this.TLBB.MapId == MAP.TranLongKyCuoc)
			{
				if (this.ClearTime.Elapsed.TotalSeconds < 4.0)
				{
					return;
				}
				if (this.Objects.NearMonter18m.Count > 0)
				{
					this.ClearTime = Stopwatch.StartNew();
					if (this.IsRide && this.IsAuto)
					{
						this.DownRide();
					}
					this.StopFollow();
					return;
				}
				if (!this.IsBossDie)
				{
					if (!this.IsRide && this.TLBB.HaveRide)
					{
						this.UpRide();
						return;
					}
					this.AskTeamFollow();
					this.MoveNext();
					return;
				}
			}
			else
			{
				this.IsKyCuoc = false;
			}
		}

		// Token: 0x1700018D RID: 397
		// (get) Token: 0x06000770 RID: 1904 RVA: 0x0002CBF7 File Offset: 0x0002ADF7
		// (set) Token: 0x06000771 RID: 1905 RVA: 0x0002CBFF File Offset: 0x0002ADFF
		public bool IsLauLanTamBao { get; set; }

		// Token: 0x06000772 RID: 1906 RVA: 0x0002CC08 File Offset: 0x0002AE08
		public void DatDoiLauLanTamBao()
		{
			if (this.IsXongTamBao)
			{
				this.IsLauLanTamBao = false;
				return;
			}
			if (!this.IsLauLanTamBao)
			{
				return;
			}
			if (Game.TickCount % 18 != 0)
			{
				return;
			}
			if (this.PickItem())
			{
				return;
			}
			this.TrieuTap();
			if (this.TLBB.MapId != MAP.LauLanBaoTang)
			{
				if (!this.IsRide && this.TLBB.HaveRide)
				{
					this.StopFollow();
					this.UpRide();
					return;
				}
				if (this.GoTo(LAULAN.KimCuuLinh))
				{
					if (this.TLBB.IsQuestOpen)
					{
						this.QuestFrameOptionClicked(808039, 1);
						this.CloseQuest();
						return;
					}
					this.Talk(LAULAN.KimCuuLinh.Id);
					return;
				}
			}
		}

		// Token: 0x06000773 RID: 1907 RVA: 0x0002CCBC File Offset: 0x0002AEBC
		public bool TrieuTap()
		{
			bool flag = false;
			foreach (Game game in this.Party)
			{
				if (!game.TLBB.IsLeader && game.tranTime.Elapsed.TotalSeconds >= 2.0)
				{
					if (this.IsThuyLao)
					{
						if (game.TLBB.MapId != MAP.ThuyLao || this.TLBB.MapId != MAP.ThuyLao)
						{
							if (!game.DaNhanThuyLao)
							{
								game.NhanThuyLao();
							}
							else
							{
								game.DiThuyLao();
							}
						}
						else if (!game.PickItem())
						{
							if (game.Objects.NearMonter20m.Count > 0)
							{
								if (game.TLBB.PlayerState == 0)
								{
									if (game.TLBB.IsFollow)
									{
										game.StopFollow();
									}
									if (game.TLBB.IsRide)
									{
										game.DownRide();
									}
								}
							}
							else if (!game.IsRide && game.TLBB.HaveRide)
							{
								game.UpRide();
							}
							if (TINHKIEM.GetDistance((float)game.RoundX, (float)game.RoundY, (float)this.RoundX, (float)this.RoundY) > 4f)
							{
								game.GoTo((float)this.RoundX, (float)this.RoundY, false);
							}
						}
					}
					else if (game.TLBB.KeyId != this.TLBB.Id || game.TLBB.IsLeader || game.TLBB.PlayerState == 2)
					{
						game.IsTrieuTap = false;
					}
					else
					{
						if (this.IsQ123ToChau || this.IsYenTuO)
						{
							game.IsTheoQ = true;
						}
						if ((game.TLBB.MapId != MAP.ViemMaSon && game.TLBB.MapId != MAP.TamTaiHiepCoc) || !game.TLBB.IsFollow)
						{
							if (game.IsMapPhuBan())
							{
								if (this.IsBossDie)
								{
									if (!game.IsRide && !this.IsNhamBinhSinhDie)
									{
										game.Ride();
									}
								}
								else if ((game.Objects.NearMonter12m.Count > 0 || ((game.TLBB.MapId == MAP.YenTuO || game.IsMapAcBa || game.TLBB.MapId == MAP.TangKinhCac) && game.Objects.NearMonter20m.Count > 0)) && game.TLBB.PlayerState == 0)
								{
									if (game.TLBB.MapId != MAP.ViemMaSon && game.TLBB.MapId != MAP.TamTaiHiepCoc)
									{
										game.StopFollow();
									}
									if (game.IsRide)
									{
										game.DownRide();
									}
								}
							}
							if ((this.IsQ123LauLan || this.IsQ123ToChau) && this.TLBB.MapId != MAP.TamTaiHiepCoc && this.TLBB.MapId != MAP.ViemMaSon && this.TLBB.MapId != MAP.SinhTuLoiDai && !game.IsP && this.IsP)
							{
								game.IsP = true;
								game.TraQ = (game.NhanQ = (game.IsClick = (game.IsContinute = false)));
							}
							if ((game.TLBB.MapId == MAP.ViemMaSon || game.TLBB.MapId == MAP.TacKhauDoanhDia || game.TLBB.MapId == game.TLBB.MapAcBa || game.TLBB.MapId == MAP.TangKinhCac || game.TLBB.MapId == MAP.TamTaiHiepCoc) && game.TLBB.PlayerState != 0)
							{
								game.IsTrieuTap = false;
							}
							else if (game.TLBB.MapId != this.TLBB.MapId || (double)TINHKIEM.GetDistance(game.CharX, game.CharY, this.CharX, this.CharY) >= 7.5 || (TINHKIEM.GetDistance(game.CharX, game.CharY, this.CharX, this.CharY) >= 3f && (game.TLBB.MapId == MAP.TangKinhCac || game.IsMapAcBa || game.TLBB.MapId == MAP.PhungHoangCoThanhPhuBan || game.TLBB.MapId == MAP.HuyenVuDaoPhuBan || game.TLBB.MapId == MAP.ThanhThuSonPhuBan || game.TLBB.MapId == MAP.TacKhauDoanhDia || game.TLBB.MapId == MAP.ViemMaSon || game.TLBB.MapId == MAP.TamTaiHiepCoc)))
							{
								if ((game.TLBB.MapId == MAP.TangKinhCac || game.TLBB.MapId == MAP.TacKhauDoanhDia) && game.TLBB.IsFollow)
								{
									game.IsTrieuTap = false;
								}
								else
								{
									flag = (game.IsTrieuTap = true);
									game.GoTo((float)this.RoundX, (float)this.RoundY, this.TLBB.MapId, false);
								}
							}
							else
							{
								game.IsTrieuTap = false;
							}
						}
					}
				}
			}
			if (flag)
			{
				int num = Game.TickCount % 36;
			}
			return flag;
		}

		// Token: 0x1700018E RID: 398
		// (get) Token: 0x06000774 RID: 1908 RVA: 0x0002D218 File Offset: 0x0002B418
		// (set) Token: 0x06000775 RID: 1909 RVA: 0x0002D21F File Offset: 0x0002B41F
		public static bool IsHoldPK { get; set; }

		// Token: 0x1700018F RID: 399
		// (get) Token: 0x06000776 RID: 1910 RVA: 0x0002D227 File Offset: 0x0002B427
		// (set) Token: 0x06000777 RID: 1911 RVA: 0x0002D22F File Offset: 0x0002B42F
		private int XuatPetCount { get; set; }

		// Token: 0x17000190 RID: 400
		// (get) Token: 0x06000778 RID: 1912 RVA: 0x0002D238 File Offset: 0x0002B438
		// (set) Token: 0x06000779 RID: 1913 RVA: 0x0002D240 File Offset: 0x0002B440
		public string ToaDo { get; set; }

		// Token: 0x17000191 RID: 401
		// (get) Token: 0x0600077A RID: 1914 RVA: 0x0002D249 File Offset: 0x0002B449
		// (set) Token: 0x0600077B RID: 1915 RVA: 0x0002D251 File Offset: 0x0002B451
		public bool IsNhanNguyenLieu { get; set; }

		// Token: 0x17000192 RID: 402
		// (get) Token: 0x0600077C RID: 1916 RVA: 0x0002D25A File Offset: 0x0002B45A
		// (set) Token: 0x0600077D RID: 1917 RVA: 0x0002D262 File Offset: 0x0002B462
		public int WaitRecv { get; set; }

		// Token: 0x0600077E RID: 1918 RVA: 0x0002D26B File Offset: 0x0002B46B
		public void UnHookRecv()
		{
			if (this.IsHooked)
			{
				if (this.RecvAddress != 0)
				{
					Memory.WriteProcessMemory(this.Memory.Id, this.RecvAddress, this.bufferRecv, 10, 0);
				}
				this.IsHooked = false;
			}
		}

		// Token: 0x0600077F RID: 1919 RVA: 0x0002D2A4 File Offset: 0x0002B4A4
		public void TapTrung()
		{
			if (this.ON_SCENE_TRANSING)
			{
				return;
			}
			if (!this.TLBB.Online)
			{
				return;
			}
			if (this.Objects.Self != null && this.Objects.Self.PartyId == -1)
			{
				return;
			}
			this.ReadRecvData();
			string trieuTap = this.GetTrieuTap(this.RecvDat);
			if (trieuTap != "")
			{
				this.ToaDo = trieuTap;
			}
		}

		// Token: 0x17000193 RID: 403
		// (get) Token: 0x06000780 RID: 1920 RVA: 0x0002D311 File Offset: 0x0002B511
		// (set) Token: 0x06000781 RID: 1921 RVA: 0x0002D319 File Offset: 0x0002B519
		public int TrieuTapX { get; set; }

		// Token: 0x17000194 RID: 404
		// (get) Token: 0x06000782 RID: 1922 RVA: 0x0002D322 File Offset: 0x0002B522
		// (set) Token: 0x06000783 RID: 1923 RVA: 0x0002D32A File Offset: 0x0002B52A
		public int TrieuTapY { get; set; }

		// Token: 0x17000195 RID: 405
		// (get) Token: 0x06000784 RID: 1924 RVA: 0x0002D333 File Offset: 0x0002B533
		// (set) Token: 0x06000785 RID: 1925 RVA: 0x0002D33B File Offset: 0x0002B53B
		public int TrieuTapMap { get; set; }

		// Token: 0x17000196 RID: 406
		// (get) Token: 0x06000786 RID: 1926 RVA: 0x00008C9D File Offset: 0x00006E9D
		public bool IsTKC
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000197 RID: 407
		// (get) Token: 0x06000787 RID: 1927 RVA: 0x0002D344 File Offset: 0x0002B544
		public bool IsCungMay
		{
			get
			{
				return this.Objects.Self == null || (this.Objects.Self != null && this.Objects.Self.PartyId == -1);
			}
		}

		// Token: 0x17000198 RID: 408
		// (get) Token: 0x06000788 RID: 1928 RVA: 0x0002D378 File Offset: 0x0002B578
		// (set) Token: 0x06000789 RID: 1929 RVA: 0x0002D380 File Offset: 0x0002B580
		public int FreshmanWatchReceive { get; set; }

		// Token: 0x0600078A RID: 1930 RVA: 0x0002D389 File Offset: 0x0002B589
		public void SetSafeTime()
		{
			this.LuaDoOneLineString("Lua_SetProtectTime(0,1); IsMessageBox = 1;");
		}

		// Token: 0x17000199 RID: 409
		// (get) Token: 0x0600078B RID: 1931 RVA: 0x0002D396 File Offset: 0x0002B596
		// (set) Token: 0x0600078C RID: 1932 RVA: 0x0002D39E File Offset: 0x0002B59E
		public bool IsDome { get; set; }

		// Token: 0x1700019A RID: 410
		// (get) Token: 0x0600078D RID: 1933 RVA: 0x0002D3A7 File Offset: 0x0002B5A7
		// (set) Token: 0x0600078E RID: 1934 RVA: 0x0002D3AF File Offset: 0x0002B5AF
		public bool IsP { get; set; }

		// Token: 0x1700019B RID: 411
		// (get) Token: 0x0600078F RID: 1935 RVA: 0x0002D3B8 File Offset: 0x0002B5B8
		// (set) Token: 0x06000790 RID: 1936 RVA: 0x0002D3C0 File Offset: 0x0002B5C0
		public bool IsKyCuoc { get; set; }

		// Token: 0x06000791 RID: 1937 RVA: 0x0002D3CC File Offset: 0x0002B5CC
		public void P()
		{
			if (Game.TickCount % 18 != 0)
			{
				return;
			}
			if (this.IsP)
			{
				if (this.TLBB.IsFollow)
				{
					this.StopFollow();
				}
				if (TINHKIEM.VietLien(this.TLBB.MapName) == "loidaisinhtu" && TINHKIEM.GetDistance(this.CharX, this.CharY, 12f, 34f) < 8f)
				{
					if (this.TLBB.IsQuestOpen)
					{
						this.QuestFrameOptionClicked(402049, 1);
						this.IsP = false;
					}
					foreach (GameObject gameObject in this.Objects.All)
					{
						if (TINHKIEM.VietLien(gameObject.Name) == "khovinhdaisu")
						{
							this.Talk(gameObject.Id);
							break;
						}
					}
					return;
				}
				if (TINHKIEM.VietLien(this.TLBB.MapName).Contains("thienkieplau"))
				{
					if (this.TLBB.IsQuestOpen && this.TLBB.IsQuestOpen)
					{
						if (!this.IsClick)
						{
							foreach (QuestFrame questFrame in QuestFrame.Enum(this))
							{
								if (questFrame.Name.Contains("#{TJL_xml_XX(01)}"))
								{
									this.QuestFrameOptionClicked(questFrame);
									this.IsClick = true;
									return;
								}
							}
							this.CloseQuest();
							return;
						}
						if (!this.TraQ && this.IsClick)
						{
							this.QuestFrameMissionComplete();
							this.CloseQuest();
							this.TraQ = true;
							this.IsClick = false;
							return;
						}
						if (!this.NhanQ && this.IsClick)
						{
							this.QuestFrameAccept();
							this.CloseQuest();
							this.NhanQ = true;
							this.IsP = false;
							this.IsClick = false;
							return;
						}
					}
					foreach (GameObject gameObject2 in this.Objects.All)
					{
						if (TINHKIEM.VietLien(gameObject2.Name) == "phokiepsinh")
						{
							this.Talk(gameObject2.Id);
							break;
						}
					}
					return;
				}
				if (this.TLBB.MapId == DAILY.Id && TINHKIEM.GetDistance(this.CharX, this.CharY, 131f, 79f) < 8f)
				{
					if (this.TLBB.IsQuestOpen)
					{
						if (!this.IsClick)
						{
							foreach (QuestFrame questFrame2 in QuestFrame.Enum(this))
							{
								if (questFrame2.Name.Contains("#{SXRW_090119_002}"))
								{
									this.QuestFrameOptionClicked(questFrame2);
									this.IsClick = true;
									return;
								}
							}
							this.QuestFrame.Close();
							return;
						}
						if (!this.TraQ && this.IsClick)
						{
							this.QuestFrameMissionComplete();
							this.CloseQuest();
							this.TraQ = true;
							this.IsClick = false;
							return;
						}
						if (!this.NhanQ && this.IsClick)
						{
							this.QuestFrameAccept();
							this.CloseQuest();
							this.NhanQ = true;
							this.IsP = false;
							this.IsClick = false;
							return;
						}
						if (this.TLBB.IsLeader && this.IsClick && this.NhanQ && this.TraQ)
						{
							foreach (QuestFrame questFrame3 in QuestFrame.Enum(this))
							{
								if (questFrame3.StrOptionExtra1 == 402048 && questFrame3.StrOptionExtra2 == 2)
								{
									this.QuestFrameOptionClicked(questFrame3);
								}
							}
							this.IsClick = (this.TraQ = (this.NhanQ = (this.IsContinute = false)));
						}
					}
					foreach (GameObject gameObject3 in this.Objects.All)
					{
						if (TINHKIEM.VietLien(gameObject3.Name) == "khovinhdaisu")
						{
							this.Talk(gameObject3.Id);
							break;
						}
					}
					return;
				}
				if (this.TLBB.MapId == 61 && TINHKIEM.GetDistance(this.CharX, this.CharY, 40f, 40f) < 8f)
				{
					if (this.TLBB.IsQuestOpen)
					{
						this.QuestFrameOptionClicked(44000, 0);
						this.IsP = false;
					}
					foreach (GameObject gameObject4 in this.Objects.All)
					{
						if (TINHKIEM.VietLien(gameObject4.Name) == "tethanh")
						{
							this.Talk(gameObject4.Id);
							break;
						}
					}
					return;
				}
				if (this.TLBB.MapId == 236 && TINHKIEM.GetDistance(this.CharX, this.CharY, 180f, 90f) < 8f)
				{
					if (this.TLBB.IsQuestOpen)
					{
						this.QuestFrameOptionClicked(402249, 1);
						this.IsP = false;
					}
					foreach (GameObject gameObject5 in this.Objects.All)
					{
						if (TINHKIEM.VietLien(gameObject5.Name) == "hoahachcan")
						{
							this.Talk(gameObject5.Id);
							break;
						}
					}
					return;
				}
				if (this.TLBB.MapId == MAP.LauLan && TINHKIEM.GetDistance(this.CharX, this.CharY, 211f, 176f) < 8f)
				{
					if (this.TLBB.IsQuestOpen)
					{
						if (!this.IsClick)
						{
							foreach (QuestFrame questFrame4 in QuestFrame.Enum(this))
							{
								if (questFrame4.StrOptionExtra1 == 505054 && questFrame4.StrOptionExtra2 == 1)
								{
									this.QuestFrameOptionClicked(questFrame4);
									this.IsClick = true;
									return;
								}
							}
							this.CloseQuest();
							return;
						}
						if (!this.TraQ && this.IsClick)
						{
							if (!this.IsContinute)
							{
								this.IsContinute = true;
								this.QuestFrameMissionContinue();
								return;
							}
							this.QuestFrameMissionComplete();
							this.CloseQuest();
							this.TraQ = true;
							this.IsClick = false;
							return;
						}
						else if (!this.NhanQ && this.IsClick)
						{
							this.QuestFrameAccept();
							this.CloseQuest();
							this.NhanQ = true;
							this.IsP = false;
							this.IsClick = false;
							return;
						}
					}
					foreach (GameObject gameObject6 in this.Objects.All)
					{
						if (TINHKIEM.VietLien(gameObject6.Name) == "caoduong")
						{
							this.Talk(gameObject6.Id);
							break;
						}
					}
					return;
				}
				if (this.TLBB.MapId == MAP.LauLan && TINHKIEM.GetDistance(this.CharX, this.CharY, 295f, 68f) < 8f)
				{
					if (this.TLBB.IsQuestOpen)
					{
						if (QuestFrame.All(this).Contains("#{XSHYH_150211_13}"))
						{
							this.IsQ123LauLan = false;
						}
						if (!this.IsClick)
						{
							foreach (QuestFrame questFrame5 in QuestFrame.Enum(this))
							{
								if (questFrame5.StrOptionExtra1 == 506030)
								{
									this.QuestFrameOptionClicked(questFrame5);
									this.IsClick = true;
									return;
								}
							}
							this.IsClick = (this.TraQ = (this.NhanQ = (this.IsContinute = false)));
							this.CloseQuest();
							return;
						}
						if (!this.TraQ && this.IsClick)
						{
							this.QuestFrameMissionComplete();
							this.CloseQuest();
							this.TraQ = true;
							this.IsClick = false;
							return;
						}
						if (!this.NhanQ && this.IsClick)
						{
							this.QuestFrameAccept();
							this.CloseQuest();
							this.NhanQ = true;
							this.IsP = false;
							this.IsClick = false;
							if (this.TLBB.IsLeader && this.IsQ123LauLan)
							{
								this.IsP = true;
							}
							return;
						}
						if (this.TLBB.IsLeader && this.IsQ123LauLan && this.IsClick)
						{
							this.QuestFrame.ClickAll();
							this.IsClick = (this.TraQ = (this.NhanQ = (this.IsContinute = false)));
						}
						this.CloseQuest();
					}
					foreach (GameObject gameObject7 in this.Objects.All)
					{
						if (TINHKIEM.VietLien(gameObject7.Name) == "haduyet")
						{
							this.Talk(gameObject7.Id);
							break;
						}
					}
					return;
				}
				if (this.TLBB.MapId == TOCHAU.Id && TINHKIEM.GetDistance(this.CharX, this.CharY, 134f, 260f) < 8f)
				{
					if (this.TLBB.IsQuestOpen)
					{
						if (QuestFrame.All(this).Contains("#{LSHYH_150210_6}"))
						{
							this.IsQ123ToChau = false;
						}
						if (this.TLBB.IsLeader && this.TraQ && this.NhanQ)
						{
							foreach (QuestFrame questFrame6 in QuestFrame.Enum(this))
							{
								if (questFrame6.StrOptionExtra1 == 891074 && questFrame6.StrOptionExtra2 == 2)
								{
									this.QuestFrameOptionClicked(questFrame6);
									return;
								}
							}
							this.IsClick = (this.TraQ = (this.NhanQ = (this.IsContinute = false)));
						}
						if (!this.IsClick)
						{
							foreach (QuestFrame questFrame7 in QuestFrame.Enum(this))
							{
								if (questFrame7.StrOptionExtra1 == 891074 && questFrame7.StrOptionExtra2 == 1)
								{
									this.QuestFrameOptionClicked(questFrame7);
									this.IsClick = true;
									return;
								}
							}
							this.CloseQuest();
							return;
						}
						if (!this.TraQ && this.IsClick)
						{
							this.QuestFrameMissionComplete();
							this.CloseQuest();
							this.TraQ = true;
							this.IsClick = false;
							return;
						}
						if (!this.NhanQ && this.IsClick)
						{
							this.QuestFrameAccept();
							this.CloseQuest();
							this.NhanQ = true;
							this.IsP = false;
							this.IsClick = false;
							if (this.TLBB.IsLeader && this.IsQ123ToChau)
							{
								this.IsP = true;
							}
							return;
						}
						this.CloseQuest();
					}
					foreach (GameObject gameObject8 in this.Objects.All)
					{
						if (TINHKIEM.VietLien(gameObject8.Name) == "tienhoanhvu")
						{
							this.Talk(gameObject8.Id);
							break;
						}
					}
					return;
				}
				if (this.TLBB.MapId == TOCHAU.Id && TINHKIEM.GetDistance(this.CharX, this.CharY, 195f, 214f) < 8f)
				{
					if (this.TLBB.IsQuestOpen)
					{
						if (this.TLBB.IsLeader)
						{
							foreach (QuestFrame questFrame8 in QuestFrame.Enum(this))
							{
								if (questFrame8.Name.Contains("#{SJZ_100129_11}"))
								{
									this.QuestFrameOptionClicked(questFrame8);
									this.CloseQuest();
									return;
								}
								if (questFrame8.Name.Contains("#{SJZ_100129_08}"))
								{
									this.TraQ = (this.NhanQ = false);
									this.IsClick = (this.IsContinute = false);
									return;
								}
							}
						}
						if (!this.IsClick)
						{
							foreach (QuestFrame questFrame9 in QuestFrame.Enum(this))
							{
								if (questFrame9.StrOptionExtra1 == 402052)
								{
									this.QuestFrameOptionClicked(questFrame9);
									this.IsClick = true;
									return;
								}
							}
							this.CloseQuest();
							return;
						}
						if (!this.TraQ && this.IsClick)
						{
							this.QuestFrameMissionComplete();
							this.CloseQuest();
							this.TraQ = true;
							this.IsClick = false;
							return;
						}
						if (!this.NhanQ && this.IsClick)
						{
							this.QuestFrameAccept();
							this.CloseQuest();
							this.NhanQ = true;
							this.IsP = false;
							this.IsClick = false;
							if (this.IsTheoQ)
							{
								this.IsP = true;
								this.TraQ = (this.NhanQ = false);
							}
							return;
						}
					}
					foreach (GameObject gameObject9 in this.Objects.All)
					{
						if (TINHKIEM.VietLien(gameObject9.Name) == "phanthanhthanh")
						{
							this.Talk(gameObject9.Id);
							break;
						}
					}
					return;
				}
				if (TINHKIEM.GetDistance(this.CharX, this.CharY, 96f, 79f) < 8f || TINHKIEM.GetDistance(this.CharX, this.CharY, 35f, 87f) < 8f || TINHKIEM.GetDistance(this.CharX, this.CharY, 84f, 23f) < 8f || TINHKIEM.GetDistance(this.CharX, this.CharY, 23f, 17f) < 8f)
				{
					if (TINHKIEM.GetDistance(this.CharX, this.CharY, 100f, 110f) < 20f)
					{
						this.IsP = false;
						return;
					}
					if (this.RoundX >= 32 && this.RoundX <= 44 && this.RoundY >= 70 && this.RoundY <= 80)
					{
						this.IsManMacDie = (this.IsTanVanDie = true);
						this.IsP = false;
						return;
					}
					if (this.RoundX < 80 && this.RoundX > 60)
					{
						this.IsP = false;
						return;
					}
					if (this.TLBB.IsQuestOpen)
					{
						if (TINHKIEM.GetDistance(this.CharX, this.CharY, 23f, 17f) < 8f)
						{
							this.QuestFrameOptionClicked(402051, 25);
						}
						else if (TINHKIEM.GetDistance(this.CharX, this.CharY, 84f, 23f) < 8f)
						{
							this.QuestFrameOptionClicked(402051, 24);
						}
						else
						{
							this.QuestFrameOptionClicked(402051, 22);
							this.QuestFrameOptionClicked(402051, 23);
						}
						this.IsP = false;
					}
					foreach (GameObject gameObject10 in this.Objects.All)
					{
						if (TINHKIEM.VietLien(gameObject10.Name) == "phanthanhthanh")
						{
							this.Talk(gameObject10.Id);
							break;
						}
					}
					return;
				}
			}
			this.IsP = false;
		}

		// Token: 0x1700019C RID: 412
		// (get) Token: 0x06000792 RID: 1938 RVA: 0x0002E498 File Offset: 0x0002C698
		// (set) Token: 0x06000793 RID: 1939 RVA: 0x0002E4A0 File Offset: 0x0002C6A0
		public bool IsXuat { get; set; }

		// Token: 0x1700019D RID: 413
		// (get) Token: 0x06000794 RID: 1940 RVA: 0x0002E4A9 File Offset: 0x0002C6A9
		// (set) Token: 0x06000795 RID: 1941 RVA: 0x0002E4B1 File Offset: 0x0002C6B1
		public Stopwatch swTrimTime { get; set; }

		// Token: 0x1700019E RID: 414
		// (get) Token: 0x06000796 RID: 1942 RVA: 0x0002E4BA File Offset: 0x0002C6BA
		// (set) Token: 0x06000797 RID: 1943 RVA: 0x0002E4C2 File Offset: 0x0002C6C2
		public bool IsTalkGiamNguc { get; set; }

		// Token: 0x1700019F RID: 415
		// (get) Token: 0x06000798 RID: 1944 RVA: 0x0002E4CB File Offset: 0x0002C6CB
		// (set) Token: 0x06000799 RID: 1945 RVA: 0x0002E4D3 File Offset: 0x0002C6D3
		public bool IsChangeM { get; set; }

		// Token: 0x170001A0 RID: 416
		// (get) Token: 0x0600079A RID: 1946 RVA: 0x0002E4DC File Offset: 0x0002C6DC
		// (set) Token: 0x0600079B RID: 1947 RVA: 0x0002E4E4 File Offset: 0x0002C6E4
		public bool ABC { get; set; }

		// Token: 0x170001A1 RID: 417
		// (get) Token: 0x0600079C RID: 1948 RVA: 0x0002E4ED File Offset: 0x0002C6ED
		// (set) Token: 0x0600079D RID: 1949 RVA: 0x0002E4F5 File Offset: 0x0002C6F5
		public bool IsManMacDie { get; set; }

		// Token: 0x170001A2 RID: 418
		// (get) Token: 0x0600079E RID: 1950 RVA: 0x0002E4FE File Offset: 0x0002C6FE
		// (set) Token: 0x0600079F RID: 1951 RVA: 0x0002E506 File Offset: 0x0002C706
		public bool IsTanVanDie { get; set; }

		// Token: 0x170001A3 RID: 419
		// (get) Token: 0x060007A0 RID: 1952 RVA: 0x0002E50F File Offset: 0x0002C70F
		// (set) Token: 0x060007A1 RID: 1953 RVA: 0x0002E517 File Offset: 0x0002C717
		public bool IsDaoThanhDie { get; set; }

		// Token: 0x170001A4 RID: 420
		// (get) Token: 0x060007A2 RID: 1954 RVA: 0x0002E520 File Offset: 0x0002C720
		// (set) Token: 0x060007A3 RID: 1955 RVA: 0x0002E528 File Offset: 0x0002C728
		public bool IsBangXiDie { get; set; }

		// Token: 0x060007A4 RID: 1956 RVA: 0x0002E534 File Offset: 0x0002C734
		public int ToMinute(string s)
		{
			int num = TINHKIEM.ParseInt(s) * 60;
			int num2 = TINHKIEM.ParseInt(Regex.Replace(s, ".*:", ""));
			return num + num2;
		}

		// Token: 0x060007A5 RID: 1957 RVA: 0x0002E564 File Offset: 0x0002C764
		private void RandomAcTac()
		{
			int num = new Random().Next(0, 4);
			if (num == 0)
			{
				this.MapAcTac = MAP.KinhHo;
			}
			if (num == 1)
			{
				this.MapAcTac = MAP.ThaiHo;
			}
			if (num == 2)
			{
				this.MapAcTac = MAP.TungSon;
			}
			if (num == 3)
			{
				this.MapAcTac = MAP.KiemCac;
			}
			if (num == 4)
			{
				this.MapAcTac = MAP.DonHoang;
			}
		}

		// Token: 0x060007A6 RID: 1958 RVA: 0x0002E5C6 File Offset: 0x0002C7C6
		private void RandomTKC()
		{
			int num = new Random().Next(0, 2);
			if (num == 0)
			{
				this.MapTKC = MAP.TayHo;
			}
			if (num == 1)
			{
				this.MapTKC = MAP.NhiHai;
			}
			if (num == 2)
			{
				this.MapTKC = MAP.NhanNam;
			}
		}

		// Token: 0x170001A5 RID: 421
		// (get) Token: 0x060007A7 RID: 1959 RVA: 0x0002E5FF File Offset: 0x0002C7FF
		// (set) Token: 0x060007A8 RID: 1960 RVA: 0x0002E607 File Offset: 0x0002C807
		private bool IsXongTamBao { get; set; }

		// Token: 0x170001A6 RID: 422
		// (get) Token: 0x060007A9 RID: 1961 RVA: 0x0002E610 File Offset: 0x0002C810
		// (set) Token: 0x060007AA RID: 1962 RVA: 0x0002E618 File Offset: 0x0002C818
		private bool IsXongKyCuoc { get; set; }

		// Token: 0x060007AB RID: 1963 RVA: 0x0002E624 File Offset: 0x0002C824
		public void ClearMission()
		{
			this.MapAcTac = (this.MapTKC = 0);
			this.IsAcBa = (this.IsPhungHoangLangMo = (this.IsKyCuoc = (this.IsLauLanTamBao = (this.IsPMP = (this.IsHuyetChien = (this.IsQ123ToChau = (this.IsQ123LauLan = (this.IsYenTuO = (this.IsTuBaoBon = (this.IsLuyenKim = false))))))))));
		}

		// Token: 0x170001A7 RID: 423
		// (get) Token: 0x060007AC RID: 1964 RVA: 0x0002E6B0 File Offset: 0x0002C8B0
		private bool IsBusy
		{
			get
			{
				return this.IsMapPhuBan();
			}
		}

		// Token: 0x170001A8 RID: 424
		// (get) Token: 0x060007AD RID: 1965 RVA: 0x0002E6B8 File Offset: 0x0002C8B8
		// (set) Token: 0x060007AE RID: 1966 RVA: 0x0002E6C0 File Offset: 0x0002C8C0
		public bool IsX2 { get; set; }

		// Token: 0x170001A9 RID: 425
		// (get) Token: 0x060007AF RID: 1967 RVA: 0x0002E6C9 File Offset: 0x0002C8C9
		// (set) Token: 0x060007B0 RID: 1968 RVA: 0x0002E6D1 File Offset: 0x0002C8D1
		public bool TuAnX2 { get; set; }

		// Token: 0x170001AA RID: 426
		// (get) Token: 0x060007B1 RID: 1969 RVA: 0x0002E6DA File Offset: 0x0002C8DA
		// (set) Token: 0x060007B2 RID: 1970 RVA: 0x0002E6E2 File Offset: 0x0002C8E2
		public bool IsPhiThuy { get; set; }

		// Token: 0x170001AB RID: 427
		// (get) Token: 0x060007B3 RID: 1971 RVA: 0x0002E6EB File Offset: 0x0002C8EB
		// (set) Token: 0x060007B4 RID: 1972 RVA: 0x0002E6F3 File Offset: 0x0002C8F3
		public bool IsKheLinh { get; set; }

		// Token: 0x170001AC RID: 428
		// (get) Token: 0x060007B5 RID: 1973 RVA: 0x0002E6FC File Offset: 0x0002C8FC
		// (set) Token: 0x060007B6 RID: 1974 RVA: 0x0002E704 File Offset: 0x0002C904
		public bool IsClickKheLinh { get; set; }

		// Token: 0x060007B7 RID: 1975 RVA: 0x0002E710 File Offset: 0x0002C910
		public void Auto()
		{
			if (!this.IsAuto)
			{
				return;
			}
			if (!this.IsInit)
			{
				this.Init();
				return;
			}
			if (Game.TickCount % this.DelayTime == 0)
			{
				this.TLBB.Read();
				this.Objects.Read();
				if (Game.TickCount % 600 == 0 && this.IdleTime > 2 && !Win.IsWindowVisible(this.Handle))
				{
					this.Handle = Win.GetHandle(this.ProcessId, Win.WndClassNames);
				}
			}
			if (this.ON_SCENE_TRANSING || this.IsChangeMap)
			{
				if (this.TLBB.MapId != this.TLBB.MapMonPhai)
				{
					if (this.TLBB.MapId != MAP.TacKhauDoanhDia && this.TLBB.MapId != this.MapAcTac && !this.IsTKC && !this.IsPhungHoangLangMo)
					{
						this.MoveIndex = -1;
					}
					if (this.TLBB.MapId == MAP.PhungHoangCoThanh || this.IsMapPhuBan() || this.TLBB.MapId == MAP.ThuyLao)
					{
						this.MoveIndex = 0;
					}
				}
				if (this.IsMapPhuBan())
				{
					this.IsRadius = false;
				}
				this.IsXongThuyLao = false;
				this.come = false;
				this.comeex = false;
				this.IsXongPhuBan = false;
				this.IsTheoQ = false;
				this.lastAutoMove = Stopwatch.StartNew();
				this.StandTime = 0;
				this.IdleTime = 0;
				this.Talked = false;
				this.TimeOnMap = 0;
				this.BossTime = DateTime.MinValue;
				this.MapATIndex = -1;
				this.IsTrieuTap = false;
				this.IsBossDie = false;
				this.IsCapDaiBaDie = false;
				this.IsTangThoCongDie = false;
				this.IsOLaoDaiDie = false;
				this.IsNhamBinhSinhDie = false;
				this.IsLyThuThuyDie = false;
				this.IsTalkPhuManNghi = false;
				this.IsTalkOLaoDai = false;
				this.OLaoDaiDead = false;
				this.DieTime = null;
				this.ClickTime = 0;
				this.LyThuThuyDead = null;
				this.IsLyThuThuyDead = false;
				this.TKCInfo = "";
				this.IsTraiTKC = false;
				this.TKCComplete = (this.CheckTKCComplete = (this.TKCompleted = false));
				this.tranTime = Stopwatch.StartNew();
				this.IsChangeM = true;
				this.ListBHDXuatHien.Clear();
				this.ListThuHoachBHD.Clear();
				this.IsTrieuTap = false;
				this.IsTheoQ = false;
				this.IsClick = (this.TraQ = (this.NhanQ = (this.IsContinute = false)));
				this.IsP = false;
				this.CurMapATIndex = -1;
				this.IsManMacDie = (this.IsTanVanDie = (this.IsDaoThanhDie = (this.IsBangXiDie = false)));
				this.IsDoanDienKhanhDie = (this.IsCuuMaTriDie = false);
				this.isAlarmHuyetMo = false;
				this.DaNhanThuyLao = false;
				this.NotSafe.Clear();
				Game.ListDangLumHop.Clear();
				return;
			}
			this.TLBB.PlayerState = this.Memory.Read(this.Address.CharState);
			if (this.TLBB.PlayerState == 0 || this.TLBB.PlayerState == 9)
			{
				this.StandTime++;
				if (Game.TickCount % 21 == 0)
				{
					this.IdleTime++;
					if (this.TLBB.IsCaptcha)
					{
						this.CaptchaTime = 0;
					}
					else
					{
						this.CaptchaTime++;
					}
				}
			}
			else
			{
				this.TimeStand = Stopwatch.StartNew();
				this.StandTime = 0;
				this.IdleTime = 0;
			}
			if (this.tranTime.Elapsed.TotalSeconds < 2.0)
			{
				return;
			}
			if (Game.TickCount % 48 == 0)
			{
				this.IsX2 = false;
				if (this.lastX2TimeSec != this.TLBB.X2TimeSec)
				{
					this.lastX2TimeSec = this.TLBB.X2TimeSec;
					this.IsX2 = true;
					if (this.lastX2TimeSec == 0)
					{
						this.IsX2 = false;
					}
				}
			}
			if (this.IsOpenShop && this.SafeTime >= 2 && Game.TickCount % 18 == 0 && this.IsOpenPass2)
			{
				this.IsOpenShop = false;
				this.OpenShop();
			}
			if (this.TLBB.MapId == MAP.DiaPhu || this.TLBB.MapId == MAP.DiaPhuDaiTheGioi)
			{
				if (Game.TickCount % 18 == 0)
				{
					this.XuatPetCount = 0;
					this.State = STATE.None;
					this.TrangThaiLuyenKim = (this.TrangThaiSuMon = (this.TrangThaiTuBaoBon = (this.TrangThaiXayDung = (this.TrangThaiTuDuong = (this.TrangThaiQD = "")))));
					if (this.TLBB.IsQuestOpen)
					{
						if (this.TLBB.MapId != MAP.DiaPhu)
						{
							this.QuestFrameOptionClicked(505072, 1);
							this.CloseQuest();
							return;
						}
						if (Option.MaptriLieuIndex == 0)
						{
							this.QuestFrameOptionClicked(12009, 1);
							return;
						}
						if (Option.MaptriLieuIndex == 1)
						{
							this.QuestFrameOptionClicked(12009, 3);
							return;
						}
						if (Option.MaptriLieuIndex == 2)
						{
							this.QuestFrameOptionClicked(12009, 5);
							return;
						}
						if (Option.MaptriLieuIndex == 246)
						{
							this.QuestFrameOptionClicked(12009, 6);
							return;
						}
						this.CloseQuest();
						return;
					}
					else
					{
						foreach (GameObject gameObject in this.Objects.All)
						{
							if (gameObject.CleanName.Contains("manhba") || gameObject.CleanName == "pomeng")
							{
								this.Talk(gameObject.Id);
								return;
							}
						}
					}
				}
				return;
			}
			if (this.IsChangeM)
			{
				this.IsChangeM = false;
				this.PostMessage(58, 105);
			}
			this.P();
			this.SetCalendar();
			if (this.TrimTime.Elapsed.TotalMinutes > 3.0 && FrmMain.TrimRam)
			{
				this.TrimTime = Stopwatch.StartNew();
				this.TrimProc();
			}
			if (this.Objects.NearMonter12m.Count > 0)
			{
				this.ClearTime = Stopwatch.StartNew();
			}
			bool flag = false;
			if (Game.TickCount % 9 == 0 || (this.TLBB.MapId == MAP.YenTuO && Game.TickCount % 3 == 0))
			{
				if (this.IsLyThuThuyDead && !this.IsOut)
				{
					this.IsOut = true;
					this.LuaDoOneLineString("IsOut = true;");
				}
				if (!this.IsLyThuThuyDead && this.IsOut)
				{
					this.IsOut = false;
					this.LuaDoOneLineString("IsOut = false;");
				}
				if (this.TLBB.MapId == MAP.TranLongKyCuoc || this.TLBB.MapId == MAP.LauLanBaoTang || this.TLBB.MapId == MAP.ThanhThuSonPhuBan)
				{
					if (this.IsRide && this.Objects.Monter.Count > 0)
					{
						this.DownRide();
					}
					if (!this.IsBossDie)
					{
						foreach (GameObject gameObject2 in this.Objects.All)
						{
							if (gameObject2.CleanName == "viencokyhon" && gameObject2.HP == 0f)
							{
								this.IsBossDie = true;
								this.BossDieTime = Stopwatch.StartNew();
							}
							if (gameObject2.CleanName == "tranbaolongvuong" && gameObject2.HP == 0f)
							{
								this.IsXongTamBao = true;
							}
						}
					}
					if (this.TLBB.MapId == MAP.TranLongKyCuoc && this.IsBossDie)
					{
						if (this.BossDieTime.Elapsed.TotalSeconds > 30.0)
						{
							this.GoTo(TRANLONGKYCUOC.TeThanh);
							this.IsP = true;
							this.IsXongKyCuoc = true;
						}
						else if (Game.TickCount % 6 == 0)
						{
							this.PushDebugMessage("Di chuyển sau " + (20 - this.BossDieTime.Elapsed.Seconds).ToString() + "s");
						}
					}
				}
			}
			if (this.IsSaveGold)
			{
				this.SaveGold();
				return;
			}
			this.TriLieu();
			if (Game.TickCount % this.DelayTime == 0)
			{
				this.LoadSkill();
				this.ResetetRadius();
				if (this.TLBB.Online)
				{
					this.Rao();
				}
			}
			this.XuatPet();
			if (this.TLBB.PetHP > 0)
			{
				this.IsDome = false;
				this.IsXuat = false;
				if (this.PetId == "")
				{
					this.PetId = this.TLBB.PetId.ToString("X8");
					Setting.SaveSettingOffline(this.TLBB.Id + "PET", this.PetId);
				}
				if (this.TLBB.PetLvl >= Global.PetLvl && Game.TickCount % 30 == 0 && this.AutoThuPet)
				{
					this.DoAction("PetSkill2_2");
				}
			}
			if (this.IsLostLeader && Game.TickCount % 18 == 0 && this.TLBB.OnlineTimeSec < 20 && this.TLBB.OnlineTimeSec > 3 && !this.TLBB.IsLeader)
			{
				foreach (KeyValuePair<int, Game> keyValuePair in FrmMain.dicGame)
				{
					Game value = keyValuePair.Value;
					if (value != this && value.TLBB.IsLeader && value.TLBB.Id == this.TLBB.KeyId && value.TLBB.OnlineTimeSec > 3)
					{
						value.AppointLeader(this.TLBB.Name);
						this.IsLostLeader = false;
						break;
					}
				}
			}
			if (this.IsAcBa && this.AcBa != this.TLBB.Menpai && Game.TickCount % 18 == 0 && this.TLBB.IsLeader)
			{
				foreach (KeyValuePair<int, Game> keyValuePair2 in FrmMain.dicGame)
				{
					Game value2 = keyValuePair2.Value;
					if (value2 != this && !value2.TLBB.IsLeader && value2.TLBB.KeyId == this.TLBB.Id && value2.TLBB.OnlineTimeSec > 3 && value2.TLBB.Menpai == this.AcBa)
					{
						value2.IsAcBa = true;
						this.AppointLeader(value2.TLBB.Name);
						break;
					}
				}
			}
			if (!this.TLBB.Online)
			{
				return;
			}
			if (Game.TickCount % 18 == 0)
			{
				if (!this.IsHooked && (Option.AlarmChat || this.AlarmChat || (this.TLBB.IsLeader && FrmMain.AlarmAcBa)))
				{
					this.RecvAddress = (int)this.GetRemoteProcAddress(Process.GetProcessById(this.ProcessId), "ws2_32.dll", "recv");
					Memory.ReadProcessMemory(this.Memory.Id, this.RecvAddress, this.bufferRecv, 10, 0);
					this.PostMessage(this.ProcessId, -10);
					this.IsHooked = true;
				}
				if (this.IsHooked && !Option.AlarmChat && !this.AlarmChat && !FrmMain.AlarmAcBa)
				{
					this.UnHookRecv();
				}
			}
			if (this.SellItem())
			{
				return;
			}
			if (this.IsTrieuTap && !this.TLBB.IsLeader)
			{
				return;
			}
			this.DatDoiThuyLao();
			if (this.TLBB.IsLeader)
			{
				if (this.MapAcTac != 0)
				{
					this.DatDoiAcTac();
				}
				if (Global.IsVIP > 0)
				{
					this.DatDoiQ123LauLan();
				}
				this.DatDoiKyCuoc();
				this.DatDoiLauLanTamBao();
				this.DatDoiAcBa();
				this.DatDoiTKC();
			}
			this.MoBTD();
			if (this.TLBB.Online)
			{
				this.TimeOnMap++;
			}
			if (this.TLBB.MapId == MAP.GiamNguc)
			{
				if (Game.TickCount % 18 == 0 && this.IdleTime > 2)
				{
					if (!this.IsTalkGiamNguc)
					{
						foreach (GameObject gameObject3 in this.Objects.All)
						{
							if (TINHKIEM.VietLien(gameObject3.Name).Contains("truongchinhquy"))
							{
								this.Talk(gameObject3.Id);
							}
						}
						this.IsTalkGiamNguc = true;
						return;
					}
					if (this.TLBB.IsQuestOpen)
					{
						foreach (QuestFrame questFrame in QuestFrame.Enum(this))
						{
							if (questFrame.StrOptionExtra1 == 77011 && (questFrame.StrOptionExtra2 == 1 || questFrame.StrOptionExtra2 == 11))
							{
								this.QuestFrameOptionClicked(questFrame);
								return;
							}
						}
					}
					this.IsTalkGiamNguc = false;
				}
				return;
			}
			if (Game.TickCount % 3 == 0 && this.TLBB.Gold > 10 && this.IsDead && this.TLBB.HPPercent > 0 && this.TLBB.HPPercent < 40 && Global.AutoComeBack && !MAP.IsPhuBan(this.TLBB.MapId))
			{
				this.IsTriLieu = true;
				return;
			}
			if (this.IsBachHoaDuyen)
			{
				this.IsDead = false;
			}
			if (!MAP.IsPhuBan(this.TLBB.MapId) && this.IsDead && Game.TickCount % 3 == 0 && Global.AutoComeBack && !this.IsBachHoaDuyen && !this.IsTrungAc && this.TLBB.PlayerState == 0)
			{
				if (this.DeadX == 0)
				{
					this.IsDead = false;
				}
				else
				{
					if (this.TLBB.PetHP == 0)
					{
						int xuatPetCount = this.XuatPetCount;
						this.XuatPetCount = xuatPetCount + 1;
						if (xuatPetCount < 5 && Global.IsXuat)
						{
							this.DoAction("PetSkill2_1");
							return;
						}
					}
					if (this.TLBB.MapId != this.DeadMap)
					{
						this.Move((float)this.DeadX, (float)this.DeadY, this.DeadMap);
					}
					else if (Game.IsHoldPK)
					{
						this.IsDead = false;
						this.IsAttack = true;
						this.DownRide();
					}
					else if (TINHKIEM.GetDistance(this.CharX, this.CharY, (float)this.DeadX, (float)this.DeadY) > 5f)
					{
						this.Move((float)this.DeadX, (float)this.DeadY);
					}
					else
					{
						this.IsDead = false;
						this.IsAttack = true;
						this.DownRide();
					}
				}
			}
			this.Buff();
			this.AOE();
			this.CollectItem();
			this.DropItem();
			if (this.MapAcTac == 0)
			{
				if (this.TLBB.MapId != MAP.PhungMinhVuongLang || this.NeBayTime.Elapsed.TotalSeconds >= 3.0)
				{
					this.SkillDo();
				}
				this.AcTac();
				this.TrungAc();
				this.KhaiKhoang();
				this.TrongTrot();
				this.CheDoFree();
				this.NhanNguyenLieu();
				this.HamLenBaiTrian();
				this.ThucThiAutoTrain();
				this.AutoX2();
				this.AutoAnVatPham();
			}
			if (this.ExpStart > this.TLBB.Exp || this.ExpStart <= 0)
			{
				this.ResetExpSpeed();
			}
			if (this.IsAcceptAll && Game.TickCount == 18)
			{
				this.IsAcceptAll = false;
				this.AcceptAll();
			}
			if (Game.TickCount % 99 == 0 && Global.AutoAccept)
			{
				this.Accept();
			}
			if (this.NMTime.Elapsed.TotalSeconds >= 1.0 && !this.TLBB.BusyEx && !this.TLBB.IsBienThan && this.TLBB.MapId != MAP.DiaPhu && this.TLBB.MapId != MAP.DiaPhuDaiTheGioi && this.TLBB.PlayerState != 9 && !this.PickItem() && this.TLBB.MPPercent > 5 && !TINHKIEM.IsPressed(VirtualKeyStates.VK_LMENU) && !TINHKIEM.IsPressed(VirtualKeyStates.VK_RMENU) && this.TLBB.Name != "ĐăngNhập" && this.TLBB.PlayerState != 2 && this.TLBB.PlayerState != 5 && !this.IsRide && !this.TLBB.IsFollow && this.IsNM && this.TLBB.Menpai == 5 && this.Objects.PartyMinHP != null && this.Objects.PartyMinHP.HP * 100f <= (float)Global.BuffNMPercent)
			{
				int num = 9999;
				if (this.NMSKill.DelayOffset.ToString().Trim(new char[]
				{
					'0'
				}).Length < 8)
				{
					num = this.Memory.Read(this.TLBB.DelayBase + this.NMSKill.DelayOffset);
				}
				if (num == 0 || num == -1 || num == 9999)
				{
					if (Global.NMSkill == Keys.F13)
					{
						if (MAP.IsPhuBan(this.TLBB.MapId))
						{
							this.NMTime = Stopwatch.StartNew();
							this.UseSkill(this.NMSKill.PacketId, this.Objects.PartyMinHP.Id);
							return;
						}
						if (this.Objects.Self.HP * 100f <= (float)Global.BuffNMPercent)
						{
							this.NMTime = Stopwatch.StartNew();
							this.UseSkill(this.NMSKill.PacketId, this.Objects.Self.Id);
						}
					}
					else if (MAP.IsPhuBan(this.TLBB.MapId))
					{
						this.NMTime = Stopwatch.StartNew();
						this.SelectTarget(this.Objects.PartyMinHP.Id);
						this.SendKey(Global.NMSkill);
					}
					else if (this.Objects.Self.HP * 100f <= (float)Global.BuffNMPercent)
					{
						this.NMTime = Stopwatch.StartNew();
						this.SelectTarget(this.Objects.Self.Id);
						this.SendKey(Global.NMSkill);
					}
				}
			}
			if (Game.TickCount % 3 == 0 && !this.PickItem() && !flag)
			{
				this.Attack();
			}
			if (Game.TickCount % 9 == 0)
			{
				this.FollowKey();
			}
			if (Game.TickCount % 18 == 0)
			{
				this.ExpGain = this.TLBB.Exp - this.ExpStart;
				this.ExpSpeed = (float)((double)this.ExpGain / this.AutoTime.Elapsed.TotalHours);
				if (this.X4 && this.Objects.Self != null && !this.Objects.Self.Buff.Contains(1685) && this.DoActionPacket("CircularTaskTool20_16"))
				{
					this.LuaDoOneLineString("IsMessageBox = 1");
				}
				if (this.LastId != this.TLBB.Id && this.TLBB.Online)
				{
					this.LoadSetting();
				}
				this.ResetTime();
				this.UpLvl();
				if (this.TLBB.SafeTime > 0)
				{
					this.IsOpenPass2 = false;
					this.SafeTime = 0;
				}
				else
				{
					this.SafeTime++;
					if (this.SafeTime >= 3 && Option.SetSafeTime && !this.setSafeTime && this.Address.GameType == 1)
					{
						this.setSafeTime = true;
						this.SetSafeTime();
					}
				}
				if (!this.IsOpenPass2 && this.Pass2 != "" && this.SafeTime == 2)
				{
					this.UnlockPass2();
					this.IsOpenPass2 = true;
				}
				if (!this.IsOpenBag)
				{
					this.OpenBag();
					this.IsOpenBag = true;
				}
				if (this.Pass2 == "")
				{
					this.IsOpenPass2 = true;
				}
			}
		}

		// Token: 0x060007B8 RID: 1976 RVA: 0x0002FBA4 File Offset: 0x0002DDA4
		private void SetCalendar()
		{
			if (FrmMain.IsCalender && Game.TickCount % 99 == 0 && !this.IsBusy)
			{
				foreach (string text in TINHKIEM.ReadFile(Global.CalenderPath + "\\" + this.TLBB.Id + ".txt").Split(new char[]
				{
					'\n'
				}))
				{
					if (text.Split(new char[]
					{
						'|'
					}).Length > 1)
					{
						string text2 = text.Split(new char[]
						{
							'|'
						})[0];
						if (text2.Split(new char[]
						{
							'-'
						}).Length > 1)
						{
							int num = this.ToMinute(text2.Split(new char[]
							{
								'-'
							})[0]);
							int num2 = this.ToMinute(text2.Split(new char[]
							{
								'-'
							})[1]);
							int num3 = DateTime.Now.Hour * 60 + DateTime.Now.Minute;
							if (num3 >= num && num3 < num2)
							{
								string text3 = "";
								try
								{
									text3 = TINHKIEM.VietLien(text);
								}
								catch
								{
									break;
								}
								this.ClearMission();
								if (text3.Contains("tubaobon"))
								{
									this.IsTuBaoBon = true;
									return;
								}
								if (!this.TLBB.IsLeader)
								{
									break;
								}
								if (text3.Contains("actac"))
								{
									this.RandomAcTac();
									return;
								}
								if (text3.Contains("acba"))
								{
									this.IsAcBa = true;
									return;
								}
								if (text3.Contains("tangkinhcac"))
								{
									this.RandomTKC();
									return;
								}
								if (text3.Contains("langmo"))
								{
									this.IsPhungHoangLangMo = true;
									return;
								}
								if (text3.Contains("kycuoc"))
								{
									this.IsKyCuoc = true;
									return;
								}
								if (text3.Contains("tambao"))
								{
									this.IsLauLanTamBao = true;
									return;
								}
								if (text3.Contains("huyetchien"))
								{
									this.IsPMP = (this.IsHuyetChien = true);
									return;
								}
								if (text3.Contains("phieumieuphong"))
								{
									this.IsPMP = true;
									this.IsHuyetChien = false;
									return;
								}
								if (text3.Contains("tochau"))
								{
									this.IsQ123ToChau = true;
									return;
								}
								if (text3.Contains("laulan"))
								{
									this.IsQ123LauLan = true;
									return;
								}
								if (text3.Contains("yentuo"))
								{
									this.IsYenTuO = true;
									return;
								}
								if (text3.Contains("luyenkim"))
								{
									this.IsLuyenKim = true;
									return;
								}
								break;
							}
						}
					}
				}
			}
		}

		// Token: 0x060007B9 RID: 1977 RVA: 0x0002FE3C File Offset: 0x0002E03C
		private void XuatPet()
		{
			if (Game.TickCount % 30 == 0 && this.TLBB.PlayerState != 5 && Global.IsXuat && !this.IsRide && this.TLBB.Online && this.TLBB.PetHP == 0 && this.PetId != "00000000" && this.PetId != "")
			{
				int num = this.Memory.Read(this.Address.PetBase);
				int num2 = -1;
				for (int i = 0; i < 20; i++)
				{
					num2++;
					int num3 = this.Memory.Read(num + this.Address.PetDataSize * i + this.Address.PetId);
					int num4 = this.Memory.Read(num + this.Address.PetDataSize * i + this.Address.PetEnjoy);
					if (this.AutoThuPet)
					{
						if (this.Memory.Read(num + this.Address.PetDataSize * i + this.Address.PetLvl) < Global.PetLvl)
						{
							if (num4 < 60)
							{
								this.IsDome = true;
							}
							else
							{
								this.IsXuat = true;
							}
							if (num3.ToString("X8") == this.PetId && this.TimeStand.Elapsed.TotalSeconds > 0.5)
							{
								this.LuaDoOneLineString("XuatPet('" + this.PetId + "')");
							}
						}
					}
					else
					{
						if (num4 < 60)
						{
							this.IsDome = true;
						}
						else
						{
							this.IsXuat = true;
						}
						if (num3.ToString("X8") == this.PetId && this.TimeStand.Elapsed.TotalSeconds > 0.5)
						{
							this.LuaDoOneLineString("XuatPet('" + this.PetId + "')");
						}
					}
				}
			}
		}

		// Token: 0x060007BA RID: 1978 RVA: 0x0003005A File Offset: 0x0002E25A
		public void TrimProc()
		{
			this.Memory.TrimMem();
		}

		// Token: 0x170001AD RID: 429
		// (get) Token: 0x060007BB RID: 1979 RVA: 0x00030067 File Offset: 0x0002E267
		// (set) Token: 0x060007BC RID: 1980 RVA: 0x0003006F File Offset: 0x0002E26F
		public bool AutoBuyKNB { get; set; }

		// Token: 0x170001AE RID: 430
		// (get) Token: 0x060007BD RID: 1981 RVA: 0x00030078 File Offset: 0x0002E278
		// (set) Token: 0x060007BE RID: 1982 RVA: 0x00030080 File Offset: 0x0002E280
		public bool IsChangeTab { get; set; }

		// Token: 0x170001AF RID: 431
		// (get) Token: 0x060007BF RID: 1983 RVA: 0x00030089 File Offset: 0x0002E289
		// (set) Token: 0x060007C0 RID: 1984 RVA: 0x00030091 File Offset: 0x0002E291
		public bool IsChange { get; set; }

		// Token: 0x170001B0 RID: 432
		// (get) Token: 0x060007C1 RID: 1985 RVA: 0x0003009A File Offset: 0x0002E29A
		// (set) Token: 0x060007C2 RID: 1986 RVA: 0x000300A2 File Offset: 0x0002E2A2
		public bool IsKichHoat { get; set; }

		// Token: 0x170001B1 RID: 433
		// (get) Token: 0x060007C3 RID: 1987 RVA: 0x000300AB File Offset: 0x0002E2AB
		// (set) Token: 0x060007C4 RID: 1988 RVA: 0x000300B3 File Offset: 0x0002E2B3
		public bool IsTalkTieuPhong { get; set; }

		// Token: 0x060007C5 RID: 1989 RVA: 0x000300BC File Offset: 0x0002E2BC
		public void NhanNguyenLieu()
		{
			if (!this.IsNhanNguyenLieu)
			{
				return;
			}
			if (Game.TickCount % 12 != 0)
			{
				return;
			}
			if (this.TLBB.MapId != 2)
			{
				this.Move(157f, 169f, 2);
				return;
			}
			if (TINHKIEM.GetDistance(this.CharX, this.CharY, 157f, 169f) > 1f)
			{
				this.Move(157f, 169f);
				return;
			}
			if (!this.IsTalkTieuPhong)
			{
				this.IsTalkTieuPhong = true;
				this.Talk(143);
				return;
			}
			this.IsTalkTieuPhong = false;
			this.QuestFrameOptionClicked(2084, 1004);
		}

		// Token: 0x170001B2 RID: 434
		// (get) Token: 0x060007C6 RID: 1990 RVA: 0x00030163 File Offset: 0x0002E363
		// (set) Token: 0x060007C7 RID: 1991 RVA: 0x0003016B File Offset: 0x0002E36B
		public bool IsHookRecv { get; set; }

		// Token: 0x170001B3 RID: 435
		// (get) Token: 0x060007C8 RID: 1992 RVA: 0x00030174 File Offset: 0x0002E374
		// (set) Token: 0x060007C9 RID: 1993 RVA: 0x0003017C File Offset: 0x0002E37C
		public bool IsCheckOnline { get; set; }

		// Token: 0x170001B4 RID: 436
		// (get) Token: 0x060007CA RID: 1994 RVA: 0x00030185 File Offset: 0x0002E385
		// (set) Token: 0x060007CB RID: 1995 RVA: 0x0003018D File Offset: 0x0002E38D
		public bool IsCheckOnlineEx { get; set; }

		// Token: 0x170001B5 RID: 437
		// (get) Token: 0x060007CC RID: 1996 RVA: 0x00030196 File Offset: 0x0002E396
		// (set) Token: 0x060007CD RID: 1997 RVA: 0x0003019E File Offset: 0x0002E39E
		public bool IsNhatTuyet { get; set; }

		// Token: 0x170001B6 RID: 438
		// (get) Token: 0x060007CE RID: 1998 RVA: 0x000301A7 File Offset: 0x0002E3A7
		// (set) Token: 0x060007CF RID: 1999 RVA: 0x000301AF File Offset: 0x0002E3AF
		public bool IsDungIm { get; set; }

		// Token: 0x060007D0 RID: 2000 RVA: 0x000301B8 File Offset: 0x0002E3B8
		public void SellItem(int item)
		{
			this.PostMessage(item, 117);
		}

		// Token: 0x170001B7 RID: 439
		// (get) Token: 0x060007D1 RID: 2001 RVA: 0x000301C3 File Offset: 0x0002E3C3
		// (set) Token: 0x060007D2 RID: 2002 RVA: 0x000301CB File Offset: 0x0002E3CB
		public bool IsGoToShop { get; set; }

		// Token: 0x060007D3 RID: 2003 RVA: 0x000301D4 File Offset: 0x0002E3D4
		public void KhaiKhoang()
		{
			if (Game.TickCount % 12 != 0)
			{
				return;
			}
			if (!this.IsKhoang && !this.IsDuoc)
			{
				return;
			}
			if (this.IsMapNghe() && !this.IsMove)
			{
				this.FixKetMap();
				return;
			}
			float num = 1000f;
			int num2 = -1;
			float num3 = 0f;
			float num4 = 0f;
			foreach (GameObject gameObject in this.Objects.All)
			{
				if (((gameObject.IsKhoang && this.IsKhoang) || (gameObject.IsDuoc && this.IsDuoc)) && TINHKIEM.GetDistance(this.CharX, this.CharY, gameObject.X, gameObject.Y) < num)
				{
					num = TINHKIEM.GetDistance(this.CharX, this.CharY, gameObject.X, gameObject.Y);
					num2 = gameObject.Id;
					num3 = gameObject.X;
					num4 = gameObject.Y;
				}
			}
			if (num2 != -1)
			{
				if (TINHKIEM.GetDistance(this.CharX, this.CharY, num3, num4) > 5f)
				{
					this.Move(num3, num4);
					return;
				}
				if (this.IsRide)
				{
					this.DownRide();
					return;
				}
				this.PickItem(num2);
				return;
			}
			else
			{
				if (!this.IsRide && this.TLBB.HaveRide)
				{
					this.UpRide();
					return;
				}
				if (this.IsMapNghe())
				{
					this.MoveNext();
				}
				return;
			}
		}

		// Token: 0x060007D4 RID: 2004 RVA: 0x0003035C File Offset: 0x0002E55C
		public bool IsMapNghe()
		{
			return this.TLBB.MapId == MAP.KiemCac || this.TLBB.MapId == MAP.CaoXuong || this.TLBB.MapId == MAP.VoLuongSon || this.TLBB.MapId == MAP.DonHoang || this.TLBB.MapId == MAP.TungSon || this.TLBB.MapId == MAP.ThaiHo || this.TLBB.MapId == MAP.TayHo || this.TLBB.MapId == MAP.NhiHai || this.TLBB.MapId == MAP.NhanNam || this.TLBB.MapId == MAP.LongTuyen || this.TLBB.MapId == MAP.ThuongSon || this.TLBB.MapId == MAP.NhanBac || this.TLBB.MapId == MAP.VoDi || this.TLBB.MapId == MAP.ThachLam || this.TLBB.MapId == MAP.NganNgaiTuyetNguyen || this.TLBB.MapId == MAP.ThaoNguyen;
		}

		// Token: 0x060007D5 RID: 2005 RVA: 0x000304A5 File Offset: 0x0002E6A5
		public void OkPhu()
		{
			if (this.Address.GameType == 2 && this.Memory.Read(new int[]
			{
				6549480,
				0,
				0,
				12,
				100
			}) == 1)
			{
				this.LuaDoOneLineString("setmetatable(_G, {__index = Item_TuDunZhu_Env}); Item_TuDunZhu_OK_Clicked();");
			}
		}

		// Token: 0x060007D6 RID: 2006 RVA: 0x000304E0 File Offset: 0x0002E6E0
		public void DisableActiveGame()
		{
			if (this.Address.GameType == 1 || this.Address.GameType == 2)
			{
				this.Memory.Write(this.Address.DisableActiveGame, 2425393296U, 4);
				this.Memory.Write(this.Address.DisableActiveGame + 4, 2425393296U, 4);
				this.Memory.Write(this.Address.DisableActiveGame + 8, 2425393296U, 4);
				this.Memory.Write(this.Address.DisableActiveGame + 12, 37008U, 2);
				return;
			}
			this.Memory.Write(this.Address.DisableActiveGame, 2425393296U, 4);
			this.Memory.Write(this.Address.DisableActiveGame + 4, 2425393296U, 4);
			this.Memory.Write(this.Address.DisableActiveGame + 8, 2425393296U, 4);
			this.Memory.Write(this.Address.DisableActiveGame + 12, 9474192U, 3);
		}

		// Token: 0x170001B8 RID: 440
		// (get) Token: 0x060007D7 RID: 2007 RVA: 0x000305F8 File Offset: 0x0002E7F8
		// (set) Token: 0x060007D8 RID: 2008 RVA: 0x00030600 File Offset: 0x0002E800
		public bool IsWrite { get; set; }

		// Token: 0x170001B9 RID: 441
		// (get) Token: 0x060007D9 RID: 2009 RVA: 0x00030609 File Offset: 0x0002E809
		// (set) Token: 0x060007DA RID: 2010 RVA: 0x00030611 File Offset: 0x0002E811
		public bool IsLPMH { get; set; }

		// Token: 0x170001BA RID: 442
		// (get) Token: 0x060007DB RID: 2011 RVA: 0x0003061A File Offset: 0x0002E81A
		// (set) Token: 0x060007DC RID: 2012 RVA: 0x00030622 File Offset: 0x0002E822
		private Stopwatch RaoTime { get; set; }

		// Token: 0x170001BB RID: 443
		// (get) Token: 0x060007DD RID: 2013 RVA: 0x0003062B File Offset: 0x0002E82B
		// (set) Token: 0x060007DE RID: 2014 RVA: 0x00030633 File Offset: 0x0002E833
		public bool IsNhanh { get; set; }

		// Token: 0x060007DF RID: 2015 RVA: 0x0003063C File Offset: 0x0002E83C
		public void Rao()
		{
			if (!this.IsRao)
			{
				return;
			}
			if (this.RaoTime == null || this.RaoTime.Elapsed.TotalSeconds > this.TimeGiaoChat)
			{
				this.RaoTime = Stopwatch.StartNew();
				if (this.RaoTxt.Trim() != "")
				{
					this.GiaoChat();
				}
			}
		}

		// Token: 0x060007E0 RID: 2016 RVA: 0x000306A0 File Offset: 0x0002E8A0
		public void GiaoChat()
		{
			string str = "#r#b#eda0000  TLBB C h i c k e n A u t o #r#b#eda0000  h t tp : / / c h i c k e n a u t o . c o m";
			if (this.ChatGan)
			{
				Thread.Sleep(50);
				this.Chat("near", this.RaoTxt.Replace("\"", "") + str);
			}
			if (this.ChatMonPhai)
			{
				Thread.Sleep(50);
				this.Chat("menpai", this.RaoTxt.Replace("\"", "") + str);
			}
			if (this.ChatDongMinh)
			{
				Thread.Sleep(50);
				this.Chat("guild_league", this.RaoTxt.Replace("\"", "") + str);
			}
			if (this.ChatBangPhai)
			{
				Thread.Sleep(50);
				this.Chat("guild", this.RaoTxt.Replace("\"", "") + str);
			}
			if (this.ChatDoi)
			{
				Thread.Sleep(50);
				this.Chat("team", this.RaoTxt.Replace("\"", "") + str);
			}
			if (this.ChatTheGioi)
			{
				Thread.Sleep(50);
				this.Chat("scene", this.RaoTxt.Replace("\"", "") + str);
			}
		}

		// Token: 0x060007E1 RID: 2017 RVA: 0x000307F1 File Offset: 0x0002E9F1
		public void MessageboxSelfOkClicked()
		{
			this.PostMessage(19, 105);
		}

		// Token: 0x060007E2 RID: 2018 RVA: 0x00030800 File Offset: 0x0002EA00
		public void CollectItem()
		{
			if (Game.TickCount % 3 != 0)
			{
				return;
			}
			int num = this.Memory.Read(this.Address.LootPacketId);
			if (this.IsOptLocDo)
			{
				Game.lootPacketIdDua.Add(num);
			}
			if (!this.lootPacketId.Contains(num))
			{
				this.lootPacketId.Add(num);
			}
			else if ((!this.IsKhoang || !this.IsMapNghe()) && (!this.IsDuoc || !this.IsMapNghe()) && !this.IsThuHoach && !this.IsBachHoaDuyen && !this.IsNhatHop && !this.IsNhatTuyet && !this.IsPhuMau && !this.IsNhatHopQDua)
			{
				return;
			}
			if (this.lootPacketId.Count > 30)
			{
				this.lootPacketId.RemoveAt(0);
			}
			if (!this.IsCollect())
			{
				return;
			}
			int num2 = this.Memory.ReadAddress(this.Address.LootPacketItem);
			int i = 0;
			while (i < 10)
			{
				int num3 = this.Memory.Read(num2 + i * 4);
				int num4 = this.Memory.Read(num3 + 8);
				if (this.IsOptLocDo)
				{
					int num5 = this.Memory.Read(num3);
					string str;
					if (num5 == this.Address.PacketType1 || num5 == this.Address.PacketType5)
					{
						str = this.Memory.ReadString(this.Memory.Read(num3 + 40, 40));
						this.Memory.ReadString(this.Memory.Read(num3 + 40, 88));
						this.Memory.ReadString(this.Memory.Read(num3 + 40, 84));
					}
					else if (num5 == this.Address.PacketType2)
					{
						str = this.Memory.ReadString(this.Memory.Read(num3 + 40, 24));
						this.Memory.ReadString(this.Memory.Read(num3 + 40, 80));
						this.Memory.ReadString(this.Memory.Read(num3 + 40, 20));
					}
					else if (num5 == this.Address.PacketType3)
					{
						str = this.Memory.ReadString(this.Memory.Read(num3 + 40, 28));
						this.Memory.ReadString(this.Memory.Read(num3 + 40, 304));
						this.Memory.ReadString(this.Memory.Read(num3 + 40, 20));
					}
					else if (num5 == this.Address.PacketType4)
					{
						str = this.Memory.ReadString(this.Memory.Read(num3 + 40, 40));
						this.Memory.ReadString(this.Memory.Read(num3 + 40, 76));
						this.Memory.ReadString(this.Memory.Read(num3 + 40, 20));
					}
					else
					{
						str = this.Memory.ReadString(this.Memory.Read(num3 + 40, 88));
						this.Memory.ReadString(this.Memory.Read(num3 + 40, 80));
						this.Memory.ReadString(this.Memory.Read(num3 + 40, 20));
					}
					if (num5 == this.Address.PacketType6)
					{
						str = this.Memory.ReadString(this.Memory.Read(num3 + 40, 44));
						this.Memory.ReadString(this.Memory.Read(num3 + 40, 104));
						this.Memory.ReadString(this.Memory.Read(num3 + 40, 40));
					}
					if (TINHKIEM.VietLien(this.IdLocDo).Contains(TINHKIEM.VietLien(str)) && num3 != 0)
					{
						this.PostMessage(num3, 107);
					}
				}
				if (this.IsSuMon || this.IsXayDung)
				{
					if (num4.ToString().Contains("400030"))
					{
						this.TrangThaiSuMon = "PickItem";
					}
					if (num4.ToString().Contains("40004"))
					{
						this.TrangThaiXayDung = "PickItem";
					}
				}
				if ((num3 != 0 && (this.IsNhatHopQDua || this.IsNhatHopall || this.IsQDua || this.IsBachHoaDuyen || this.IsPhuMau || this.IsNhatTuyet || this.IsNhatHop || (this.IsKhoang && this.IsMapNghe()) || (this.IsDuoc && this.IsMapNghe()) || this.IsThuHoach)) || !Global.ItemFillter)
				{
					if (!this.IsOptLocDo)
					{
						this.PickAll();
						return;
					}
					break;
				}
				else
				{
					if (Game.IsVIPItem(num4) && !this.IsOptLocDo && num3 != 0)
					{
						this.PostMessage(num3, 107);
					}
					i++;
				}
			}
		}

		// Token: 0x060007E3 RID: 2019 RVA: 0x00030CBC File Offset: 0x0002EEBC
		public static bool IsVIPItem(int item)
		{
			return item != 0 && (item == 30008034 || item == 30000000 || (item >= 20109001 && item <= 20109015) || item == 20109101 || item == 20109102 || item == 30008053 || item == 30103042 || item == 10141153 || item == 10157001 || item == 10157002 || item == 10156001 || item == 10156002 || item == 10156003 || item == 10156004 || item.ToString().StartsWith("30120") || item.ToString().StartsWith("10124") || (!item.ToString().StartsWith("101") && !item.ToString().StartsWith("102") && !item.ToString().StartsWith("103") && !item.ToString().StartsWith("104") && !item.ToString().StartsWith("201") && !item.ToString().StartsWith("300") && !item.ToString().StartsWith("301")));
		}

		// Token: 0x060007E4 RID: 2020 RVA: 0x00030E2C File Offset: 0x0002F02C
		public bool ForcePickItem()
		{
			if (this.TLBB.IsFollow)
			{
				return false;
			}
			if (this.TLBB.PlayerState == 5)
			{
				return false;
			}
			float num = (float)Global.PickRadius;
			int num2 = -1;
			foreach (GameObject gameObject in this.Objects.LootPacket)
			{
				if (!this.lootPacketId.Contains(gameObject.Id))
				{
					gameObject.DistanceEx = gameObject.GetDistance(this.CharX, this.CharY);
					if (gameObject.DistanceEx < num)
					{
						num = gameObject.DistanceEx;
						num2 = gameObject.Id;
					}
				}
			}
			if (num2 == -1)
			{
				return false;
			}
			this.PostMessage(num2, 106);
			if (this.PickId != num2)
			{
				this.PickId = num2;
				this.PickTime = Stopwatch.StartNew();
			}
			else if (this.PickTime.Elapsed.TotalSeconds > 2.0 && this.TimeStand.Elapsed.TotalSeconds > 2.0 && this.TLBB.MapId != MAP.PhieuMieuPhong && this.TLBB.MapId != MAP.BinhThanhKyTran)
			{
				this.lootPacketId.Add(num2);
			}
			if (this.TimeStand.Elapsed.TotalSeconds > 3.0)
			{
				this.FixKetMap();
				this.StandTime = 0;
				return true;
			}
			return true;
		}

		// Token: 0x060007E5 RID: 2021 RVA: 0x00030FB0 File Offset: 0x0002F1B0
		public void PickItem(int id)
		{
			if (id != this.PickedId)
			{
				this.PickedId = id;
				this.CareTime = Stopwatch.StartNew();
			}
			else if (this.CareTime.Elapsed.TotalSeconds > 30.0)
			{
				this.BlackList.Add(this.PickedId);
			}
			if (this.Memory.Read(this.Address.LootPacketId) == id)
			{
				return;
			}
			if (this.TLBB.PlayerState == 8)
			{
				this.pickTiem = Stopwatch.StartNew();
			}
			if (this.pickTiem.Elapsed.TotalSeconds < 1.0)
			{
				return;
			}
			this.PostMessage(id, 106);
		}

		// Token: 0x060007E6 RID: 2022 RVA: 0x00031064 File Offset: 0x0002F264
		public bool PickItem()
		{
			if (this.IsXayDung || this.IsSuMon || this.IsTuBaoBon || this.IsTuDuong || this.IsBachHoaDuyen || this.TLBB.IsODaoCuFull || this.TLBB.IsONguyenLieuFull)
			{
				return false;
			}
			if (this.TLBB.MapId == MAP.TangKinhCac && TINHKIEM.GetDistance(this.CharX, this.CharY, 64f, 28f) > 15f)
			{
				return false;
			}
			if (this.TLBB.Busy && this.TLBB.PlayerState != 7)
			{
				return false;
			}
			if (this.TLBB.IsFollow && !this.TLBB.IsLeader)
			{
				return false;
			}
			if (!this.IsPickItem)
			{
				return false;
			}
			if (Global.PickItem || this.TLBB.MapId == MAP.TangKinhCac || this.IsLuyenKim || this.IsPickEx || this.IsQDua || this.IsNhatHopall || this.IsNhatHopQDua)
			{
				float num = (float)Global.PickRadius;
				int num2 = -1;
				float x = 0f;
				float y = 0f;
				foreach (GameObject gameObject in this.Objects.LootPacket)
				{
					if (!this.lootPacketId.Contains(gameObject.Id))
					{
						gameObject.DistanceEx = gameObject.GetDistance(this.CharX, this.CharY);
						if (gameObject.DistanceEx < num)
						{
							num = gameObject.DistanceEx;
							num2 = gameObject.Id;
							x = gameObject.X;
							y = gameObject.Y;
						}
					}
				}
				if (num2 != -1)
				{
					if (this.TLBB.MapId == MAP.TangKinhCac)
					{
						this.Move(x, y);
					}
					this.PostMessage(num2, 106);
					if (this.PickId != num2)
					{
						this.PickId = num2;
						this.PickTime = Stopwatch.StartNew();
					}
					else if (this.PickTime.Elapsed.TotalSeconds > 2.0 && this.TimeStand.Elapsed.TotalSeconds > 2.0 && this.TLBB.MapId != MAP.PhieuMieuPhong && this.TLBB.MapId != MAP.BinhThanhKyTran)
					{
						this.lootPacketId.Add(num2);
					}
					if (this.TimeStand.Elapsed.TotalSeconds > 3.0)
					{
						this.FixKetMap();
						this.StandTime = 0;
					}
					return true;
				}
			}
			return false;
		}

		// Token: 0x170001BC RID: 444
		// (get) Token: 0x060007E7 RID: 2023 RVA: 0x0003130C File Offset: 0x0002F50C
		// (set) Token: 0x060007E8 RID: 2024 RVA: 0x00031314 File Offset: 0x0002F514
		public int PickId { get; set; }

		// Token: 0x060007E9 RID: 2025 RVA: 0x00031320 File Offset: 0x0002F520
		public void AnDon()
		{
			foreach (Skill skill in this.Skills)
			{
				if (skill.PacketId == 248)
				{
					if (this.Memory.Read(this.TLBB.DelayBase + skill.DelayOffset) <= 0)
					{
						this.UseSkill(skill.PacketId);
						break;
					}
					break;
				}
			}
		}

		// Token: 0x060007EA RID: 2026 RVA: 0x000313A8 File Offset: 0x0002F5A8
		private void PickAll()
		{
			this.PostMessage(33, 105);
			this.Memory.Write(this.Address.PickAll, 1);
		}

		// Token: 0x060007EB RID: 2027 RVA: 0x000313CB File Offset: 0x0002F5CB
		public void Hide()
		{
			new Thread(new ThreadStart(this.HideThread))
			{
				IsBackground = true
			}.Start();
		}

		// Token: 0x060007EC RID: 2028
		[DllImport("user32.dll")]
		private static extern IntPtr SetParent(IntPtr hWndChild, IntPtr hWndNewParent);

		// Token: 0x060007ED RID: 2029 RVA: 0x000313EC File Offset: 0x0002F5EC
		public void HideThread()
		{
			if (this.Parrent == IntPtr.Zero)
			{
				this.Handle = Win.GetHandle(this.ProcessId, Win.WndClassNames);
				Win.ShowWindow(this.Handle, Win.WindowShowStyle.Minimize);
				Win.ShowWindow(this.Handle, Win.WindowShowStyle.Hide);
				return;
			}
			if (this.style == 0)
			{
				this.style = Games.GetWindowLong(this.Handle, -16);
			}
			int num = this.style;
			num &= -12582913;
			num &= -536870913;
			num &= -65537;
			num &= -131073;
			Games.SetWindowLong(this.Handle, -16, num);
			this.LuaDoString("PushEvent('VIEW_RESOLUTION_CHANGED')");
			Game.SetParent(this.Handle, this.Parrent);
		}

		// Token: 0x060007EE RID: 2030 RVA: 0x000314AA File Offset: 0x0002F6AA
		public void Active()
		{
			if (this.IsSuspend)
			{
				return;
			}
			this.IsHide = false;
			new Thread(new ThreadStart(this.ActiveThread))
			{
				IsBackground = true
			}.Start();
		}

		// Token: 0x060007EF RID: 2031 RVA: 0x000314DC File Offset: 0x0002F6DC
		public void ActiveThread()
		{
			if (this.Parrent == IntPtr.Zero)
			{
				this.Handle = Win.GetHandle(this.ProcessId, Win.WndClassNames);
			}
			else
			{
				Games.SetWindowLong(this.Handle, -16, this.style);
				Game.SetParent(this.Handle, IntPtr.Zero);
			}
			Win.Active(this.Handle);
		}

		// Token: 0x060007F0 RID: 2032 RVA: 0x00031544 File Offset: 0x0002F744
		public void Exit()
		{
			this.Parrent = IntPtr.Zero;
			FrmMain.dicGame.Remove(this.ProcessId);
			try
			{
				Process.GetProcessById(this.ProcessId).Kill();
			}
			catch
			{
			}
			try
			{
				if (this.LastName != "ĐăngNhập" && !this.BachHoaDuyenCompleted)
				{
					FrmMain.AddLog(DateTime.Now.ToString("HH:mm dd-MM") + " Thoát " + this.LastName + "\n");
				}
				if (this.alarmVaoPhai != null && this.alarmVaoPhai.Visible)
				{
					this.alarmVaoPhai.Dispose();
				}
				this.Live = false;
			}
			catch
			{
			}
		}

		// Token: 0x060007F1 RID: 2033 RVA: 0x00031614 File Offset: 0x0002F814
		public void Quit()
		{
			this.IsQuit = true;
			this.LUA.AskRet2SelServer();
		}

		// Token: 0x060007F2 RID: 2034 RVA: 0x00031628 File Offset: 0x0002F828
		public void ForceAttack()
		{
			if (Game.TickCount % 3 != 0)
			{
				return;
			}
			if (TINHKIEM.IsPressed(VirtualKeyStates.VK_LMENU))
			{
				return;
			}
			if (this.TLBB.IsNoi && this.TLBB.PlayerState == 2)
			{
				return;
			}
			if (this.TLBB.IsFollow || this.IsRide)
			{
				return;
			}
			if (Global.AtkFollowKey && this.Objects.Key != null && !this.TLBB.IsLeader)
			{
				if (Game.TickCount % 9 == 0 && this.Objects.Key.State == 7 && (this.Objects.Self.AtkToId != this.Objects.Key.AtkToId || this.Objects.Self.State != 7))
				{
					this.SelectTarget(this.Objects.Key.AtkToId);
					this.SendKey(Global.BaseSkill);
					return;
				}
			}
			else if (!Global.Paused && (this.Objects.Target == null || this.Objects.Target.HP <= 0f || (!this.Objects.TargetIsMine && this.Objects.MyMonter.Count > 0) || this.TimeStand.Elapsed.TotalSeconds > 0.4 || (this.IsLureEx && !this.Objects.Target.Belong.Contains("FFFFFFFF") && this.Objects.UnBelongMonter.Count > 0)))
			{
				this.GetBestTarget();
				if (this.BestTarget != null)
				{
					this.SelectTarget(this.BestTarget.Id);
					this.SendKey(Global.BaseSkill);
				}
			}
		}

		// Token: 0x060007F3 RID: 2035 RVA: 0x000317F4 File Offset: 0x0002F9F4
		public void Attack()
		{
			if (this.IsNhiemVuCoBan || this.IsPhuMau)
			{
				return;
			}
			if (this.IsTrieuTap)
			{
				return;
			}
			if (this.TLBB.PlayerState == 5 || this.TLBB.PlayerState == 6)
			{
				return;
			}
			if (this.IsXayDung || this.IsSuMon || this.IsTuBaoBon || this.IsTuDuong || this.IsLuyenKim || this.IsTrungAc || this.IsBachHoaDuyen || (this.IsKhoang && this.IsMapNghe()) || (this.IsDuoc && this.IsMapNghe()))
			{
				return;
			}
			if (TINHKIEM.IsPressed(VirtualKeyStates.VK_LMENU) || TINHKIEM.IsPressed(VirtualKeyStates.VK_RMENU))
			{
				return;
			}
			if ((this.TLBB.IsNoi || this.IsMapPhuBan()) && this.TLBB.PlayerState == 2)
			{
				return;
			}
			if (this.TLBB.IsFollow || !this.IsAuto || !this.IsAttack || this.IsRide)
			{
				return;
			}
			if (Global.AtkFollowKey && this.Objects.Key != null && !this.TLBB.IsLeader)
			{
				if (Game.TickCount % 9 == 0)
				{
					if (this.Objects.Key.State == 7)
					{
						if (this.Objects.Self.AtkToId != this.Objects.Key.AtkToId || this.Objects.Self.State != 7)
						{
							foreach (GameObject gameObject in this.Objects.All)
							{
								if (gameObject.Id == this.Objects.Key.AtkToId)
								{
									this.BestTarget = gameObject;
									break;
								}
							}
							this.SelectTarget(this.Objects.Key.AtkToId);
							this.SendKey(Global.BaseSkill);
							return;
						}
					}
					else if (this.Address.GameType != 2)
					{
						if (!this.isAtkFollow)
						{
							this.SendKey(Global.BaseSkill);
							this.isAtkFollow = true;
							return;
						}
						this.SelectTarget(this.Objects.Key.Id);
						this.SelectTargetOfTarget();
						this.isAtkFollow = false;
						return;
					}
				}
			}
			else if (!Global.Paused)
			{
				if (this.TimeStand.Elapsed.TotalSeconds > 0.5 && this.IsRadius && TINHKIEM.GetDistance(this.RadiusX, this.RadiusY, this.CharX, this.CharY) > 5f)
				{
					this.Move(this.RadiusX, this.RadiusY);
				}
				if (this.Objects.Target == null || this.Objects.Target.HP <= 0f || (!this.Objects.TargetIsMine && this.Objects.MyMonter.Count > 0) || this.TimeStand.Elapsed.TotalSeconds > 0.4 || (this.IsLureEx && this.TLBB.MapId != MAP.TranLongKyCuoc && this.TLBB.MapId != MAP.LauLanBaoTang && !this.Objects.Target.Belong.Contains("FFFFFFFF") && this.Objects.UnBelongMonter.Count > 0))
				{
					this.GetBestTarget();
					if (this.BestTarget != null)
					{
						if ((this.TLBB.MapId == MAP.LauLanBaoTang || this.TLBB.MapId == 61) && TINHKIEM.GetDistance(this.CharX, this.CharY, this.BestTarget.X, this.BestTarget.Y) > 3f)
						{
							this.Move(this.BestTarget.X, this.BestTarget.Y);
							return;
						}
						this.SelectTarget(this.BestTarget.Id);
						this.SendKey(Global.BaseSkill);
					}
				}
			}
		}

		// Token: 0x060007F4 RID: 2036 RVA: 0x00031C28 File Offset: 0x0002FE28
		public void SelectTargetOfTarget()
		{
			this.PostMessage(0, 105);
		}

		// Token: 0x060007F5 RID: 2037 RVA: 0x00031C34 File Offset: 0x0002FE34
		public void Move(float x, float y, int map)
		{
			if (this.lastAutoMove.Elapsed.TotalSeconds < 4.0)
			{
				return;
			}
			if (this.TLBB.MapId == map)
			{
				if (TINHKIEM.GetDistance(this.CharX, this.CharY, x, y) > 3f)
				{
					this.Move(x, y);
					return;
				}
				return;
			}
			else
			{
				if (this.TLBB.MapId == MAP.YenTuO)
				{
					this.Move(64f, 21f);
					return;
				}
				if (this.Address.GameType != 1)
				{
					if (Unity.IsMessengerBox(this.TLBB.MapId, (int)this.CharX, (int)this.CharY))
					{
						this.LuaDoOneLineString("IsMessageBox = 1;");
						return;
					}
					if (this.ON_SCENE_TRANSING)
					{
						return;
					}
					if (this.TLBB.PlayerState == 7)
					{
						return;
					}
					if (this.TLBB.MapId != 6 && this.TLBB.MapId != 7 && this.TLBB.MapId != 24 && map == 2 && this.TLBB.MapId != 2)
					{
						this.DownRide();
						this.UseSkill(22);
						return;
					}
					if (!this.IsMove)
					{
						this.FixKetMap();
						return;
					}
					if (this.TLBB.MapId == map)
					{
						if (TINHKIEM.GetDistance(this.CharX, this.CharY, x, y) <= 3f)
						{
							return;
						}
						this.Move(x, y);
						return;
					}
					else
					{
						if (this.UsingTholinhChau)
						{
							int num = this.PhuIndex(Unity.GetFakeMapID(map));
							if (num != -1)
							{
								if (this.IsRide)
								{
									this.DownRide();
									return;
								}
								this.PlayerPackageUseItem(num);
								return;
							}
						}
						if (this.TLBB.HaveRide && !this.IsRide)
						{
							this.UpRide();
							return;
						}
						PathInfo pathInfo = null;
						try
						{
							pathInfo = FindPath.GetNextPath(this.TLBB.MapId, map);
						}
						catch
						{
							this.PushDebugMessage("Không thể tìm đường");
							return;
						}
						if (!pathInfo.isNPC)
						{
							this.Move((float)pathInfo.x, (float)pathInfo.y);
							return;
						}
						string text = "KhongCo";
						bool flag = false;
						int idNext = pathInfo.idNext;
						switch (idNext)
						{
						case 9:
							text = "Thiếu Lâm";
							flag = true;
							break;
						case 10:
							text = "Cái Bang";
							flag = true;
							break;
						case 11:
							text = "Minh Giáo";
							flag = true;
							break;
						case 12:
							text = "Võ Đang";
							flag = true;
							break;
						case 13:
							text = "Thiên Long";
							flag = true;
							break;
						case 14:
							text = "Tiêu Dao";
							flag = true;
							break;
						case 15:
							text = "Nga My";
							flag = true;
							break;
						case 16:
							text = "Tinh Túc";
							flag = true;
							break;
						case 17:
							text = "Thiên Sơn";
							flag = true;
							break;
						default:
							if (idNext == 284)
							{
								text = "Mộ Dung";
								flag = true;
							}
							break;
						}
						this.Move((float)pathInfo.x, (float)pathInfo.y);
						if (flag)
						{
							foreach (QuestFrame questFrame in QuestFrame.Enum(this))
							{
								if (questFrame.Name.Contains(text))
								{
									this.QuestFrameOptionClicked(questFrame);
									break;
								}
							}
							foreach (QuestFrame questFrame2 in QuestFrame.Enum(this))
							{
								if (questFrame2.Name.Contains("Đến các môn phái"))
								{
									this.QuestFrameOptionClicked(questFrame2);
									return;
								}
							}
						}
						if (!flag)
						{
							foreach (QuestFrame questFrame3 in QuestFrame.Enum(this))
							{
								if (questFrame3.Name.Contains("Duyệt") || questFrame3.Name.Contains("Xác nhận"))
								{
									this.QuestFrameOptionClicked(questFrame3);
									break;
								}
							}
							text = FindPath.GetScreenName(pathInfo.idNext);
							if (TINHKIEM.VietLien(text).Contains("thuchacotran"))
							{
								this.LuaDoOneLineString("IsMessageBox = 1;");
								return;
							}
							foreach (QuestFrame questFrame4 in QuestFrame.Enum(this))
							{
								string text2 = TINHKIEM.VietLienRemoveNum(questFrame4.Name);
								string value = TINHKIEM.VietLienRemoveNum(text);
								if (text2.Contains(value))
								{
									this.QuestFrameOptionClicked(questFrame4);
									break;
								}
							}
						}
						if (this.lastTalk.Elapsed.TotalSeconds > 5.0)
						{
							foreach (GameObject gameObject in this.Objects.AllNpc)
							{
								if (TINHKIEM.VietLienRemoveNum(gameObject.Name).Contains(TINHKIEM.VietLienRemoveNum(pathInfo.NpcName)))
								{
									this.Talk(gameObject.Id);
									this.lastTalk = Stopwatch.StartNew();
									break;
								}
							}
						}
						this.lastAutoMove = Stopwatch.StartNew();
						return;
					}
				}
				else if (this.TLBB.MapId == MAP.PhieuMieuPhong)
				{
					if (this.Address.GameType == 1)
					{
						if (this.GoTo(96f, 40f, false))
						{
							if (this.TLBB.IsQuestOpen)
							{
								this.QuestFrame.Click(402275, 3);
								this.QuestFrame.Click(402275, 4);
								this.QuestFrameOptionClicked(402276, 3);
								this.QuestFrameOptionClicked(402288, 4);
								return;
							}
							this.Talk("olaodai");
						}
						return;
					}
					this.Move(125f, 171f);
					return;
				}
				else
				{
					if (this.TLBB.MapId == MAP.TangKinhCac)
					{
						this.Move(70f, 20f);
						return;
					}
					if (this.TLBB.MapId == MAP.ThienLongPhuBan)
					{
						this.Move(96f, 142f);
						return;
					}
					if (this.TLBB.MapId == MAP.MoDungPhuBan)
					{
						this.Move(160f, 169f);
						return;
					}
					if (this.TLBB.MapId == MAP.TacKhauDoanhDia)
					{
						this.Move(86f, 116f);
						return;
					}
					if (this.TLBB.MapId == MAP.DuongMonPhuBan)
					{
						this.Move(173f, 170f);
						return;
					}
					if (this.TLBB.MapId == MAP.TinhTucPhuBan)
					{
						this.Move(96f, 142f);
						return;
					}
					if (this.TLBB.MapId == MAP.TieuDaoPhuBan)
					{
						this.Move(44f, 129f);
						return;
					}
					if (this.TLBB.MapId == MAP.ThieuLamPhuBan)
					{
						this.Move(96f, 158f);
						return;
					}
					if (this.TLBB.MapId == MAP.ThienSonPhuBan)
					{
						this.Move(95f, 148f);
						return;
					}
					if (this.TLBB.MapId == MAP.NgaMyPhuBan)
					{
						this.Move(89f, 146f);
						return;
					}
					if (this.TLBB.MapId == MAP.VoDangPhuBan)
					{
						this.Move(95f, 192f);
						return;
					}
					if (this.TLBB.MapId == MAP.MinhGiaoPhuBan)
					{
						this.Move(98f, 159f);
						return;
					}
					if (this.TLBB.MapId == MAP.CaiBangPhuBan)
					{
						this.Move(91f, 159f);
						return;
					}
					if (this.TLBB.MapId == 604)
					{
						this.Move(45f, 51f);
						return;
					}
					int mapId = this.TLBB.MapId;
					int giamNguc = MAP.GiamNguc;
					if (this.TLBB.MapId == 550)
					{
						foreach (GameObject gameObject2 in this.Objects.AllNpc)
						{
							float x2 = gameObject2.X;
							float y2 = gameObject2.Y;
							if (x == 40f && y == 40f)
							{
								this.Talk(gameObject2.Id);
								if (this.TLBB.IsQuestOpen)
								{
									QuestFrame.ClickOut(this);
								}
								return;
							}
						}
						if (TINHKIEM.GetDistance(this.CharX, this.CharY, 104f, 79f) > 3f)
						{
							this.Move(40f, 40f);
						}
						return;
					}
					if (this.TLBB.MapId == MAP.VanKiemCoc || this.TLBB.MapId == MAP.VanKiemCocDem)
					{
						if (this.TLBB.IsQuestOpen)
						{
							QuestFrame.ClickOut(this);
							this.CloseQuest();
						}
						foreach (GameObject gameObject3 in this.Objects.All)
						{
							if (gameObject3.CleanName == "hoahachcan" || gameObject3.CleanName == "vankiepcocmocnhanthuve")
							{
								this.Talk(gameObject3.Id);
								return;
							}
						}
						if (TINHKIEM.GetDistance(this.CharX, this.CharY, 105f, 79f) > 3f)
						{
							this.Move(105f, 79f);
						}
						return;
					}
					if (map == MAP.VanKiemCoc || map == MAP.VanKiemCocDem)
					{
						if (this.TLBB.IsQuestOpen)
						{
							QuestFrame.ClickPhuBanMonPhai(this);
							this.QuestFrame.Close();
							return;
						}
						if (this.GoTo(DAILY.HoaHachCan))
						{
							this.Talk(DAILY.HoaHachCan);
						}
						return;
					}
					else if (map == MAP.ThieuLamPhuBan)
					{
						if (this.TLBB.MapId != MAP.ThieuLam)
						{
							this.GoTo((float)NPC.HuyenChung.X, (float)NPC.HuyenChung.Y, NPC.HuyenChung.Map, false);
							return;
						}
						if (TINHKIEM.GetDistance(this.CharX, this.CharY, (float)NPC.HuyenChung.X, (float)NPC.HuyenChung.Y) > 3f)
						{
							this.Move((float)NPC.HuyenChung.X, (float)NPC.HuyenChung.Y);
							return;
						}
						if (this.TLBB.IsQuestOpen)
						{
							QuestFrame.ClickPhuBanMonPhai(this);
							return;
						}
						this.Talk(NPC.HuyenChung.Id);
						return;
					}
					else if (map == MAP.CaiBangPhuBan)
					{
						if (this.TLBB.MapId != MAP.CaiBang)
						{
							this.GoTo((float)NPC.AuDuongQua.X, (float)NPC.AuDuongQua.Y, NPC.AuDuongQua.Map, false);
							return;
						}
						if (TINHKIEM.GetDistance(this.CharX, this.CharY, (float)NPC.AuDuongQua.X, (float)NPC.AuDuongQua.Y) > 3f)
						{
							this.Move((float)NPC.AuDuongQua.X, (float)NPC.AuDuongQua.Y);
							return;
						}
						if (this.TLBB.IsQuestOpen)
						{
							QuestFrame.ClickPhuBanMonPhai(this);
							return;
						}
						this.Talk(NPC.AuDuongQua.Id);
						return;
					}
					else if (map == MAP.MinhGiaoPhuBan)
					{
						if (this.TLBB.MapId != MAP.MinhGiao)
						{
							this.GoTo((float)NPC.ThacCang.X, (float)NPC.ThacCang.Y, NPC.ThacCang.Map, false);
							return;
						}
						if (TINHKIEM.GetDistance(this.CharX, this.CharY, (float)NPC.ThacCang.X, (float)NPC.ThacCang.Y) > 3f)
						{
							this.Move((float)NPC.ThacCang.X, (float)NPC.ThacCang.Y);
							return;
						}
						if (this.TLBB.IsQuestOpen)
						{
							QuestFrame.ClickPhuBanMonPhai(this);
							return;
						}
						this.Talk(NPC.ThacCang.Id);
						return;
					}
					else if (map == MAP.VoDangPhuBan)
					{
						if (this.TLBB.MapId != MAP.VoDang)
						{
							this.GoTo((float)NPC.TieuThienDat.X, (float)NPC.TieuThienDat.Y, NPC.TieuThienDat.Map, false);
							return;
						}
						if (TINHKIEM.GetDistance(this.CharX, this.CharY, (float)NPC.TieuThienDat.X, (float)NPC.TieuThienDat.Y) > 3f)
						{
							this.Move((float)NPC.TieuThienDat.X, (float)NPC.TieuThienDat.Y);
							return;
						}
						if (this.TLBB.IsQuestOpen)
						{
							QuestFrame.ClickPhuBanMonPhai(this);
							return;
						}
						this.Talk(NPC.TieuThienDat.Id);
						return;
					}
					else if (map == MAP.ThienLongPhuBan)
					{
						if (this.TLBB.MapId != MAP.ThienLong)
						{
							this.GoTo((float)NPC.HoTuTruongLao.X, (float)NPC.HoTuTruongLao.Y, NPC.HoTuTruongLao.Map, false);
							return;
						}
						if (TINHKIEM.GetDistance(this.CharX, this.CharY, (float)NPC.HoTuTruongLao.X, (float)NPC.HoTuTruongLao.Y) > 3f)
						{
							this.Move((float)NPC.HoTuTruongLao.X, (float)NPC.HoTuTruongLao.Y);
							return;
						}
						if (this.TLBB.IsQuestOpen)
						{
							QuestFrame.ClickPhuBanMonPhai(this);
							return;
						}
						this.Talk(NPC.HoTuTruongLao.Id);
						return;
					}
					else if (map == MAP.TieuDaoPhuBan)
					{
						if (this.TLBB.MapId != MAP.TieuDao)
						{
							this.GoTo((float)NPC.CongDaTuTruong.X, (float)NPC.CongDaTuTruong.Y, NPC.CongDaTuTruong.Map, false);
							return;
						}
						if (TINHKIEM.GetDistance(this.CharX, this.CharY, (float)NPC.CongDaTuTruong.X, (float)NPC.CongDaTuTruong.Y) > 3f)
						{
							this.Move((float)NPC.CongDaTuTruong.X, (float)NPC.CongDaTuTruong.Y);
							return;
						}
						if (this.TLBB.IsQuestOpen)
						{
							QuestFrame.ClickPhuBanMonPhai(this);
							return;
						}
						this.Talk(NPC.CongDaTuTruong.Id);
						return;
					}
					else if (map == MAP.NgaMyPhuBan)
					{
						if (this.TLBB.MapId != MAP.NgaMy)
						{
							this.GoTo((float)NPC.LieuTamMuoi.X, (float)NPC.LieuTamMuoi.Y, NPC.LieuTamMuoi.Map, false);
							return;
						}
						if (TINHKIEM.GetDistance(this.CharX, this.CharY, (float)NPC.LieuTamMuoi.X, (float)NPC.LieuTamMuoi.Y) > 3f)
						{
							this.Move((float)NPC.LieuTamMuoi.X, (float)NPC.LieuTamMuoi.Y);
							return;
						}
						if (this.TLBB.IsQuestOpen)
						{
							QuestFrame.ClickPhuBanMonPhai(this);
							return;
						}
						this.Talk(NPC.LieuTamMuoi.Id);
						return;
					}
					else if (map == MAP.TinhTucPhuBan)
					{
						if (this.TLBB.MapId != MAP.TinhTuc)
						{
							this.GoTo((float)NPC.ThienToanTu.X, (float)NPC.ThienToanTu.Y, NPC.ThienToanTu.Map, false);
							return;
						}
						if (TINHKIEM.GetDistance(this.CharX, this.CharY, (float)NPC.ThienToanTu.X, (float)NPC.ThienToanTu.Y) > 3f)
						{
							this.Move((float)NPC.ThienToanTu.X, (float)NPC.ThienToanTu.Y);
							return;
						}
						if (this.TLBB.IsQuestOpen)
						{
							QuestFrame.ClickPhuBanMonPhai(this);
							return;
						}
						this.Talk(NPC.ThienToanTu.Id);
						return;
					}
					else if (map == MAP.ThienSonPhuBan)
					{
						if (this.TLBB.MapId != MAP.ThienSon)
						{
							this.GoTo((float)NPC.DangBa.X, (float)NPC.DangBa.Y, NPC.DangBa.Map, false);
							return;
						}
						if (TINHKIEM.GetDistance(this.CharX, this.CharY, (float)NPC.DangBa.X, (float)NPC.DangBa.Y) > 3f)
						{
							this.Move((float)NPC.DangBa.X, (float)NPC.DangBa.Y);
							return;
						}
						if (this.TLBB.IsQuestOpen)
						{
							QuestFrame.ClickPhuBanMonPhai(this);
							return;
						}
						this.Talk(NPC.DangBa.Id);
						return;
					}
					else if (map == MAP.MoDungPhuBan)
					{
						if (this.TLBB.MapId != MAP.MoDung)
						{
							this.GoTo((float)NPC.CongDaKhon.X, (float)NPC.CongDaKhon.Y, NPC.CongDaKhon.Map, false);
							return;
						}
						if (TINHKIEM.GetDistance(this.CharX, this.CharY, (float)NPC.CongDaKhon.X, (float)NPC.CongDaKhon.Y) > 3f)
						{
							this.Move((float)NPC.CongDaKhon.X, (float)NPC.CongDaKhon.Y);
							return;
						}
						if (this.TLBB.IsQuestOpen)
						{
							QuestFrame.ClickPhuBanMonPhai(this);
							return;
						}
						this.Talk(NPC.CongDaKhon.Id);
						return;
					}
					else if (map == MAP.DuongMonPhuBan)
					{
						if (this.TLBB.MapId != MAP.DuongMon)
						{
							this.GoTo((float)NPC.DuongMoTuong.X, (float)NPC.DuongMoTuong.Y, NPC.DuongMoTuong.Map, false);
							return;
						}
						if (TINHKIEM.GetDistance(this.CharX, this.CharY, (float)NPC.DuongMoTuong.X, (float)NPC.DuongMoTuong.Y) > 3f)
						{
							this.Move((float)NPC.DuongMoTuong.X, (float)NPC.DuongMoTuong.Y);
							return;
						}
						if (this.TLBB.IsQuestOpen)
						{
							QuestFrame.ClickPhuBanMonPhai(this);
							return;
						}
						this.Talk(NPC.DuongMoTuong.Id);
						return;
					}
					else
					{
						if (this.TLBB.MapId == MAP.PhungHoangCoThanh)
						{
							if (this.GoTo(PHUNGHOANGCOTHANH.HoangLongThien))
							{
								if (this.TLBB.IsQuestOpen)
								{
									this.QuestFrameOptionClicked(403007, 1);
									this.CloseQuest();
									return;
								}
								this.Talk("hoanglongthien");
							}
							return;
						}
						if (this.TLBB.PlayerState == 2)
						{
							return;
						}
						if (this.IdleTime > 3)
						{
							this.CloseMessageBox();
						}
						if (map == MAP.PhungHoangCoThanh)
						{
							if (this.GoTo(THUCHACOTRAN.LyDa))
							{
								if (this.TLBB.IsQuestOpen)
								{
									this.QuestFrameOptionClicked(403001, 1);
									this.CloseQuest();
									return;
								}
								this.Talk(THUCHACOTRAN.LyDa.Id);
							}
							return;
						}
						this.lastAutoMove = Stopwatch.StartNew();
						this.PostMessage((int)x, 50);
						this.PostMessage((int)y, 51);
						this.PostMessage(map, 52);
						this.PostMessage(10, 105);
						if (map == MAP.ThaiHo)
						{
							this.LuaDoOneLineString("IsMessageBox = 1;");
						}
						return;
					}
				}
				return;
			}
		}

		// Token: 0x170001BD RID: 445
		// (get) Token: 0x060007F6 RID: 2038 RVA: 0x00032EE8 File Offset: 0x000310E8
		// (set) Token: 0x060007F7 RID: 2039 RVA: 0x00032EF0 File Offset: 0x000310F0
		public bool IsYenTuO { get; set; }

		// Token: 0x170001BE RID: 446
		// (get) Token: 0x060007F8 RID: 2040 RVA: 0x00032EF9 File Offset: 0x000310F9
		// (set) Token: 0x060007F9 RID: 2041 RVA: 0x00032F01 File Offset: 0x00031101
		public bool IsDoanDienKhanhDie { get; set; }

		// Token: 0x170001BF RID: 447
		// (get) Token: 0x060007FA RID: 2042 RVA: 0x00032F0A File Offset: 0x0003110A
		// (set) Token: 0x060007FB RID: 2043 RVA: 0x00032F12 File Offset: 0x00031112
		public bool IsCuuMaTriDie { get; set; }

		// Token: 0x170001C0 RID: 448
		// (get) Token: 0x060007FC RID: 2044 RVA: 0x00032F1B File Offset: 0x0003111B
		// (set) Token: 0x060007FD RID: 2045 RVA: 0x00032F23 File Offset: 0x00031123
		public bool IsTalkPhuBan { get; set; }

		// Token: 0x060007FE RID: 2046 RVA: 0x00032F2C File Offset: 0x0003112C
		public void CloseMessageBox()
		{
			this.LuaDoOneLineString("setmetatable(_G, {__index = MessageBox_Self_Env}); this:Hide();");
		}

		// Token: 0x060007FF RID: 2047 RVA: 0x00032F39 File Offset: 0x00031139
		private void MoveEx(float x, float y)
		{
			this.PostMessage(Memory.Float2Int(x), 50);
			this.PostMessage(Memory.Float2Int(y), 51);
			this.PostMessage(0, 119);
		}

		// Token: 0x06000800 RID: 2048 RVA: 0x00032F60 File Offset: 0x00031160
		public bool Move(float x, float y)
		{
			if (this.lastAutoMove.Elapsed.TotalSeconds < 2.0)
			{
				return false;
			}
			if ((this.TLBB.MapId == MAP.VanKiemCoc || this.TLBB.MapId == MAP.VanKiemCocDem) && TINHKIEM.GetDistance(this.CharX, this.CharY, 110f, 110f) < 20f)
			{
				foreach (GameObject gameObject in this.Objects.All)
				{
					if ((int)gameObject.X == 107 && ((int)gameObject.Y == 110 || (int)gameObject.Y == 111))
					{
						this.Talk(gameObject.Id);
					}
				}
				if (this.TLBB.IsQuestOpen)
				{
					QuestFrame.ClickPhuBanMonPhai(this);
				}
			}
			if ((double)TINHKIEM.GetDistance(this.CharX, this.CharY, x, y) < 1.5)
			{
				return true;
			}
			if (this.TLBB.MapId == MAP.YenTuO && x > 25f && x < 165f && y > 20f && y < 125f && (this.RoundX < 25 || this.RoundX > 165 || this.RoundY < 30 || this.RoundY > 125))
			{
				x = (float)YENTUO.HoaHachCan.X;
				y = (float)YENTUO.HoaHachCan.Y;
				this.IsP = true;
			}
			if ((this.TLBB.MapId > 500 && this.IsNhatHopQDua && !this.IsQDua) || (this.TLBB.MapId > 500 && this.IsNhatHopQDua && this.TrangThaiQD == "NhatQua"))
			{
				if (x > 49f && y > 66f)
				{
					x = 49f;
				}
				if (y > 82f)
				{
					y = 82f;
				}
				if (x > 50f && y > 65f && y <= 66f)
				{
					x = 51f;
					y = 64f;
				}
			}
			this.PostMessage((int)x, 50);
			this.PostMessage((int)y, 51);
			this.PostMessage(9, 105);
			return false;
		}

		// Token: 0x06000801 RID: 2049 RVA: 0x000331B4 File Offset: 0x000313B4
		public void SelectTarget(int targetId)
		{
			this.TargetId = targetId;
			this.PostMessage(targetId, 100);
		}

		// Token: 0x170001C1 RID: 449
		// (get) Token: 0x06000802 RID: 2050 RVA: 0x000331C6 File Offset: 0x000313C6
		// (set) Token: 0x06000803 RID: 2051 RVA: 0x000331CE File Offset: 0x000313CE
		private bool IsPK { get; set; }

		// Token: 0x06000804 RID: 2052 RVA: 0x000331D8 File Offset: 0x000313D8
		public void GetBestTarget()
		{
			this.BestTarget = null;
			if (this.TLBB.MapId == MAP.YenTuO)
			{
				foreach (GameObject gameObject in this.Objects.Monter)
				{
					if (gameObject.CleanName == "doandienkhanh" || gameObject.CleanName == "nhaclaotam" || gameObject.CleanName == "diepnhinuong")
					{
						this.BestTarget = gameObject;
						return;
					}
				}
			}
			if (this.TLBB.MapId == MAP.PhungMinhVuongLang)
			{
				foreach (GameObject gameObject2 in this.Objects.Monter)
				{
					if (gameObject2.Name == "Thị Ma Giả")
					{
						this.BestTarget = gameObject2;
						return;
					}
				}
			}
			if (this.TLBB.MapId == 272)
			{
				foreach (GameObject gameObject3 in this.Objects.Monter)
				{
					if (gameObject3.Name.Contains("o Th") && gameObject3.Name.EndsWith("ng"))
					{
						this.BestTarget = gameObject3;
						return;
					}
				}
			}
			float num;
			if (!this.IsRadius)
			{
				num = 9999f;
			}
			else
			{
				if (this.TLBB.IsNoi)
				{
					num = (float)Global.NoiRadius;
				}
				else
				{
					num = (float)Global.NgoaiRadius;
				}
				float distance = TINHKIEM.GetDistance(this.CharX, this.CharY, this.RadiusX, this.RadiusY);
				if (num < distance)
				{
					num = distance;
				}
			}
			if (this.IsTheoQ)
			{
				num = 23f;
			}
			if (this.IsTheoQ)
			{
				foreach (KeyValuePair<int, Game> keyValuePair in FrmMain.dicGame)
				{
					Game value = keyValuePair.Value;
					if (value.TLBB.Id == this.TLBB.KeyId)
					{
						this.RadiusX = value.CharX;
						this.RadiusY = value.CharY;
					}
				}
			}
			if (this.IsRadius || (this.IsTheoQ && this.RadiusX != 0f))
			{
				List<GameObject> list = new List<GameObject>();
				foreach (GameObject gameObject4 in this.Objects.Monter)
				{
					if (gameObject4.GetDistance(this.RadiusX, this.RadiusY) > num)
					{
						list.Add(gameObject4);
					}
				}
				foreach (GameObject gameObject5 in list)
				{
					this.Objects.Monter.Remove(gameObject5);
					this.Objects.MyMonter.Remove(gameObject5);
					this.Objects.UnBelongMonter.Remove(gameObject5);
					if (this.Objects.UnBelongMonter.Count == 0 && this.TLBB.PlayerState != 0)
					{
						return;
					}
				}
			}
			List<GameObject> list2;
			if (this.IsLureEx && this.Objects.UnBelongMonter.Count > 0)
			{
				list2 = this.Objects.UnBelongMonter;
			}
			else if (this.Objects.MyMonter.Count > 0)
			{
				list2 = this.Objects.MyMonter;
			}
			else
			{
				list2 = this.Objects.Monter;
			}
			float num2 = num;
			foreach (GameObject gameObject6 in list2)
			{
				if (!this.IsLureEx || !Game.LureId.Contains(gameObject6.Id))
				{
					if (this.IsRadius)
					{
						gameObject6.DistanceEx = gameObject6.GetDistance(this.RadiusX, this.RadiusY);
					}
					else
					{
						gameObject6.DistanceEx = gameObject6.GetDistance(this.CharX, this.CharY);
					}
					if (list2.Count > 1 && gameObject6 == this.Objects.Target && this.TimeStand.Elapsed.TotalSeconds > 0.3)
					{
						int atackTime = this.AtackTime;
						this.AtackTime = atackTime + 1;
						if (atackTime == 4)
						{
							this.AtackTime = 0;
							this.StandTime = 0;
							continue;
						}
					}
					if (gameObject6.DistanceEx <= num2)
					{
						num2 = gameObject6.DistanceEx;
						this.BestTarget = gameObject6;
					}
				}
			}
			if (this.BestTarget != this.Objects.Target)
			{
				this.AtackTime = 0;
			}
			if (this.BestTarget != null && this.IsLureEx)
			{
				Game.LureId.Add(this.BestTarget.Id);
			}
			if (this.TLBB.MapId == MAP.TranLongKyCuoc || this.TLBB.MapId == MAP.LauLanBaoTang)
			{
				Game.LureId.Clear();
			}
			if (this.BestTarget == null && this.IsLureEx)
			{
				if (list2.Count > 0)
				{
					this.BestTarget = list2[0];
					return;
				}
				if (this.Objects.Monter.Count > 0)
				{
					this.BestTarget = list2[0];
				}
			}
		}

		// Token: 0x06000805 RID: 2053 RVA: 0x000337A8 File Offset: 0x000319A8
		public void SkillDo()
		{
			if (this.IsTheoQ || this.IsYenTuO)
			{
				return;
			}
			if (Game.TickCount % 12 != 0)
			{
				return;
			}
			if (this.TLBB.IsFollow || !this.IsAuto || this.IsRide || Global.Paused || this.TLBB.IsBienThan || this.TLBB.BusyEx)
			{
				return;
			}
			foreach (Skill skill in this.Skills)
			{
				if (skill.PacketId == 448 && this.IsMapPhuBan() && this.Objects.NearMonter5m.Count >= 3 && Global.UsingSkill)
				{
					int num = this.Memory.Read(this.TLBB.DelayBase + skill.DelayOffset);
					if (num == 0 || num == -1)
					{
						this.DoSkill(skill.PacketId);
					}
				}
				else if ((skill.Use || (skill.UsePK && this.IsPK) || (skill.UserBuff && !Skill.IsBase(skill.PacketId) && this.TLBB.PlayerState != 2 && this.TLBB.PlayerState != 5)) && Global.UsingSkill && this.IsAttack)
				{
					int num2 = this.Memory.Read(this.TLBB.DelayBase + skill.DelayOffset);
					if (num2 == 0 || num2 == -1)
					{
						this.DoSkill(skill.PacketId);
					}
				}
			}
		}

		// Token: 0x06000806 RID: 2054 RVA: 0x00033954 File Offset: 0x00031B54
		public void DoSkill(int id)
		{
			if (this.ON_SCENE_TRANSING)
			{
				return;
			}
			if (this.IsCheDo)
			{
				return;
			}
			if (this.TLBB.MPPercent < 2)
			{
				bool isArletMP = this.IsArletMP;
				this.IsArletMP = true;
				return;
			}
			this.IsArletMP = false;
			SkillModel skillByID = SkillData.GetSkillByID(id);
			if (skillByID != null)
			{
				if (skillByID.isPassive || Skill.IsBand(id))
				{
					return;
				}
				if (id == 447)
				{
					if (this.TLBB.MPPercent <= 80)
					{
						this.UseSkill(id);
					}
					return;
				}
				if (id == 424 || id == 407)
				{
					return;
				}
				if (!this.isAreadyBuff)
				{
					if (skillByID.skillTargetType == "Friend" && skillByID.skillType == "NeedPointToTarget")
					{
						if (skillByID.impact != null)
						{
							foreach (GameObject gameObject in this.Objects.Party)
							{
								if (!gameObject.IsPet && !gameObject.Buff.Contains(skillByID.impact.id))
								{
									this.UseSkill(id, gameObject.Id);
									this.isAreadyBuff = true;
									return;
								}
							}
						}
						return;
					}
					if (skillByID.skillTargetType == "Friend" && skillByID.skillType == "GlobalSkillFromTargetPoint")
					{
						if (skillByID.impact != null)
						{
							foreach (GameObject gameObject2 in this.Objects.Party)
							{
								if (!gameObject2.IsPet && !gameObject2.Buff.Contains(skillByID.impact.id))
								{
									this.UseSkill(id, gameObject2.Id);
									this.isAreadyBuff = true;
									return;
								}
							}
						}
						return;
					}
					if (skillByID.skillTargetType == "NoTarget" && skillByID.skillType == "SelfBuffNoTarget")
					{
						if (skillByID.impact != null && this.Objects.Self != null && !this.Objects.Self.Buff.Contains(skillByID.impact.id))
						{
							this.UseSkill(id, this.Objects.Self.Id);
							this.isAreadyBuff = true;
							return;
						}
						return;
					}
					else if (((skillByID.skillTargetType == "Friend" && skillByID.skillType == "GlobalSkillFromSelf") || (skillByID.skillTargetType == "Friend" && skillByID.skillType == "SelfBuffNoTarget")) && skillByID.impact != null && this.Objects.Self != null && !this.Objects.Self.Buff.Contains(skillByID.impact.id))
					{
						this.UseSkill(id, this.Objects.Self.Id);
						this.isAreadyBuff = true;
						return;
					}
				}
				if (skillByID.skillTargetType == "Enemy" && skillByID.skillType == "NeedPointToTarget" && this.BestTarget != null && this.BestTarget.HP > 0f && TINHKIEM.GetDistance(this.CharX, this.CharY, this.BestTarget.X, this.BestTarget.Y) <= (float)skillByID.useRange)
				{
					this.UseSkill(id, this.BestTarget.Id);
					this.isAreadyBuff = false;
					return;
				}
				if (skillByID.skillTargetType == "Enemy" && skillByID.skillType == "GlobalSkillFromSelf" && skillByID.impact != null && this.Objects.Self != null && !this.Objects.Self.Buff.Contains(skillByID.impact.id))
				{
					this.UseSkill(id, this.Objects.Self.Id);
					this.isAreadyBuff = false;
					return;
				}
				if (skillByID.skillTargetType == "Enemy" && skillByID.skillType == "SelfBuffNoTarget" && skillByID.impact != null && this.Objects.Self != null && !this.Objects.Self.Buff.Contains(skillByID.impact.id))
				{
					this.UseSkill(id, this.Objects.Self.Id);
					this.isAreadyBuff = false;
					return;
				}
				this.isAreadyBuff = false;
			}
		}

		// Token: 0x06000807 RID: 2055 RVA: 0x00033DE4 File Offset: 0x00031FE4
		public int DelayOffset(int id)
		{
			foreach (Skill skill in this.Skills)
			{
				if (skill.PacketId == id)
				{
					return skill.DelayOffset;
				}
			}
			return 0;
		}

		// Token: 0x06000808 RID: 2056 RVA: 0x00033E48 File Offset: 0x00032048
		public int SkillId(int key)
		{
			if (key < 20)
			{
				key += 112;
			}
			else if (key < 48)
			{
				key += 28;
			}
			int num = this.Memory.Read(this.Address.KeySkillIdBase);
			if (key >= 112)
			{
				key = this.Memory.Read(num + (key - 112) * 24 + 4);
			}
			else
			{
				int num2 = key - 28 - 20 - 1;
				if (num2 < 0)
				{
					num2 = 9;
				}
				key = this.Memory.Read(num + num2 * 24 + 720 + 4);
			}
			return key;
		}

		// Token: 0x06000809 RID: 2057 RVA: 0x00033ED0 File Offset: 0x000320D0
		public void SendKey(int key)
		{
			if (key < 20)
			{
				key += 112;
			}
			else if (key < 48)
			{
				key += 28;
			}
			this.PostMessage(key, 101);
		}

		// Token: 0x0600080A RID: 2058 RVA: 0x00033EF4 File Offset: 0x000320F4
		public void BuffPet()
		{
			if (Game.TickCount % 9 != 0)
			{
				return;
			}
			if (this.TLBB.Name == "ĐăngNhập" || !this.IsPet || this.TLBB.PetHPPercent == 0 || this.TLBB.IsFollow || this.IsRide)
			{
				return;
			}
			if (this.TLBB.PetHPPercent <= 50 || (this.TLBB.PetHPPercent <= 85 && this.TLBB.PetMaxHP - this.TLBB.PetHP >= 10000))
			{
				this.PostMessage(1, 105);
			}
			if (Game.TickCount % 66 == 0 && this.TLBB.PetEnjoy <= 81 && this.TLBB.PetEnjoy > 0 && this.HaveItem("PetBauble_4"))
			{
				this.PostMessage(2, 105);
			}
		}

		// Token: 0x0600080B RID: 2059 RVA: 0x00033FD0 File Offset: 0x000321D0
		public void Buff()
		{
			if (Game.TickCount % 66 != 0)
			{
				return;
			}
			if (this.TLBB.Name == "ĐăngNhập" || this.TLBB.IsFollow || this.IsRide || this.TLBB.HPPercent == 0)
			{
				return;
			}
			if (this.IsHP && this.TLBB.HPPercent <= Global.BuffHPPercent)
			{
				foreach (PacketItem packetItem in PacketItem.Enum(this))
				{
					if (packetItem.Type == "Icons03_1" || packetItem.Type == "Medicine1_3" || packetItem.Type == "Medicine1_13" || packetItem.Type == "Cloth2_4" || packetItem.Type == "Cloth2_5")
					{
						this.PlayerPackageUseItem(packetItem.Index);
					}
				}
				if (this.TLBB.SkillPetType.Contains("PetSkill1_11"))
				{
					this.UseSkillPet(686);
				}
			}
			if (this.IsMP && this.TLBB.MPPercent <= Global.BuffMPPercent)
			{
				foreach (PacketItem packetItem2 in PacketItem.Enum(this))
				{
					if (packetItem2.Type == "Medicine1_1" || packetItem2.Type == "Medicine1_12" || packetItem2.Type == "Cloth2_6" || packetItem2.Type == "Cloth2_7")
					{
						this.PlayerPackageUseItem(packetItem2.Index);
					}
				}
				if (this.TLBB.SkillPetType.Contains("PetSkill1_9"))
				{
					this.UseSkillPet(696);
				}
			}
			if (this.TLBB.SkillPetType.Contains("PetSkill1_10") && this.HuyetTe && this.TLBB.MPPercent <= this.HuyetTeValue)
			{
				this.UseSkillPet(697);
			}
			if (this.TLBB.SkillPetType.Contains("PetSkill1_12") && this.CongSinh && this.TLBB.HPPercent <= this.CongSinhValue)
			{
				this.UseSkillPet(687);
			}
		}

		// Token: 0x0600080C RID: 2060 RVA: 0x00034254 File Offset: 0x00032454
		public void UpRide()
		{
			if (this.TLBB.IsFollow)
			{
				this.StopFollow();
			}
			if (this.TLBB.IsRide)
			{
				return;
			}
			if (!this.TLBB.HaveRide)
			{
				return;
			}
			if (this.TLBB.PlayerState == 5)
			{
				return;
			}
			if (this.TLBB.PlayerState != 0 && this.TLBB.PlayerState != 2)
			{
				this.Jump();
				return;
			}
			foreach (Skill skill in this.Skills)
			{
				if (skill.PacketId == 21)
				{
					if (this.Memory.Read(this.TLBB.DelayBase + skill.DelayOffset) <= 0)
					{
						this.UseSkill(skill.PacketId);
						break;
					}
					break;
				}
			}
		}

		// Token: 0x0600080D RID: 2061 RVA: 0x00034338 File Offset: 0x00032538
		public void HuyBienThan()
		{
			this.LuaDoOneLineString("local buff_num = Player:GetBuffNumber(); local i = 0; while i < buff_num do szToolTips = Player:GetBuffIconNameByIndex(i); if string.find(szToolTips, 'Buff') then Player:DispelBuffByIndex(i); end i = i+1 end");
		}

		// Token: 0x0600080E RID: 2062 RVA: 0x00034345 File Offset: 0x00032545
		public void DownRide()
		{
			if (!this.TLBB.IsRide)
			{
				return;
			}
			this.LuaDoOneLineString("local buff_num = Player:GetBuffNumber(); local i = 0; while i < buff_num do szToolTips = Player:GetBuffIconNameByIndex(i); if string.find(szToolTips, 'Ride') or string.find(szToolTips, 'ThaiIcons') then Player:DispelBuffByIndex(i); return; end i = i+1 end");
		}

		// Token: 0x0600080F RID: 2063 RVA: 0x00034360 File Offset: 0x00032560
		public void PlayerPackageUseItem(int index)
		{
			this.LuaDoOneLineString("PlayerPackage:UseItem(" + index.ToString() + ");");
		}

		// Token: 0x170001C2 RID: 450
		// (get) Token: 0x06000810 RID: 2064 RVA: 0x0003437E File Offset: 0x0003257E
		// (set) Token: 0x06000811 RID: 2065 RVA: 0x00034386 File Offset: 0x00032586
		public int AddressOneLine { get; set; }

		// Token: 0x06000812 RID: 2066 RVA: 0x00034390 File Offset: 0x00032590
		public void LuaDoOneLineString(string lua)
		{
			if (this.AddressOneLineEx[this.curLine] == 0)
			{
				this.AddressOneLineEx[this.curLine] = this.Memory.VirtualAllocEx(10240);
			}
			if (this.AddressOneLineEx[this.curLine] != 0)
			{
				this.Memory.WriteUnicodeString(lua + "--", this.AddressOneLineEx[this.curLine]);
				this.PostMessage(this.AddressOneLineEx[this.curLine], 104);
			}
			if (this.curLine < 19)
			{
				this.curLine++;
				return;
			}
			this.curLine = 0;
		}

		// Token: 0x06000813 RID: 2067 RVA: 0x00034434 File Offset: 0x00032634
		public void LuaDoOneineVCISIILString(string lua)
		{
			if (this.AddressOneLine == 0)
			{
				this.AddressOneLine = this.Memory.VirtualAllocEx(102400);
				return;
			}
			this.Memory.WriteString(lua + "--", this.AddressOneLine);
			this.PostMessage(this.AddressOneLine, 104);
		}

		// Token: 0x06000814 RID: 2068 RVA: 0x0003448C File Offset: 0x0003268C
		public void LuaDoString(string lua)
		{
			int wParam = this.Memory.WriteString(lua);
			this.PostMessage(wParam, 104);
		}

		// Token: 0x170001C3 RID: 451
		// (get) Token: 0x06000815 RID: 2069 RVA: 0x000344AF File Offset: 0x000326AF
		// (set) Token: 0x06000816 RID: 2070 RVA: 0x000344B7 File Offset: 0x000326B7
		public int AddressUnicodeString { get; set; }

		// Token: 0x170001C4 RID: 452
		// (get) Token: 0x06000817 RID: 2071 RVA: 0x000344C0 File Offset: 0x000326C0
		// (set) Token: 0x06000818 RID: 2072 RVA: 0x000344C8 File Offset: 0x000326C8
		public int AddressString { get; set; }

		// Token: 0x06000819 RID: 2073 RVA: 0x000344D4 File Offset: 0x000326D4
		public void LuaDoUnicodeString(string lua)
		{
			int wParam = this.Memory.WriteUnicodeString(lua);
			this.PostMessage(wParam, 104);
		}

		// Token: 0x0600081A RID: 2074 RVA: 0x000344F7 File Offset: 0x000326F7
		public void UseSkillPet(int id, float x, float y)
		{
			this.PostMessage(Memory.Float2Int(x), 50);
			this.PostMessage(Memory.Float2Int(y), 51);
			this.PostMessage(-1, 53);
			this.PostMessage(id, 103);
		}

		// Token: 0x0600081B RID: 2075 RVA: 0x00034527 File Offset: 0x00032727
		public void KinhCong()
		{
			this.SendKey(Keys.F6);
			this.PushDebugMessage("KHINH CONG");
			this.PostMessage(190, 50);
			this.PostMessage(202, 51);
			this.PostMessage(1, 100);
		}

		// Token: 0x0600081C RID: 2076 RVA: 0x0003455F File Offset: 0x0003275F
		public void UseSkillPet(int id)
		{
			this.PostMessage(Memory.Float2Int(this.CharX), 50);
			this.PostMessage(Memory.Float2Int(this.CharY), 51);
			this.PostMessage(-1, 53);
			this.PostMessage(id, 103);
		}

		// Token: 0x0600081D RID: 2077 RVA: 0x00034599 File Offset: 0x00032799
		public void UseSkill(int skillId, int targetId, float x, float y)
		{
			this.PostMessage(Memory.Float2Int(x), 50);
			this.PostMessage(Memory.Float2Int(y), 51);
			this.PostMessage(targetId, 53);
			this.PostMessage(skillId, 102);
		}

		// Token: 0x0600081E RID: 2078 RVA: 0x000345CA File Offset: 0x000327CA
		public void UseSkill(int skillId, int targetId)
		{
			this.UseSkill(skillId, targetId, -1f, -1f);
		}

		// Token: 0x0600081F RID: 2079 RVA: 0x000345DE File Offset: 0x000327DE
		public void UseSkill(int skillId)
		{
			this.UseSkill(skillId, -1, -1f, -1f);
		}

		// Token: 0x06000820 RID: 2080 RVA: 0x000345F2 File Offset: 0x000327F2
		public Keys GetNMKey()
		{
			return Keys.F13;
		}

		// Token: 0x06000821 RID: 2081 RVA: 0x000345F6 File Offset: 0x000327F6
		public void SendKey(Keys key)
		{
			this.SendKey((int)key);
		}

		// Token: 0x170001C5 RID: 453
		// (get) Token: 0x06000822 RID: 2082 RVA: 0x000345FF File Offset: 0x000327FF
		// (set) Token: 0x06000823 RID: 2083 RVA: 0x00034607 File Offset: 0x00032807
		private int NumsByte { get; set; }

		// Token: 0x06000824 RID: 2084 RVA: 0x00034610 File Offset: 0x00032810
		public void SetDll()
		{
			if (this.Define == 0)
			{
				this.Define = Game.Def++;
			}
			this.PostMessage((int)FrmMain.Instance.Handle, -5);
			this.PostMessage(this.Address.GameType, 0);
			this.PostMessage(this.Address.ParaSelectTarget, 1);
			this.PostMessage(this.Address.ParaSendKey[0], 2);
			this.PostMessage(this.Address.ParaSendKey[1], 3);
			this.PostMessage(this.Address.FuncSendKey, 4);
			this.PostMessage(this.Address.ParaUseSkill[0], 5);
			this.PostMessage(this.Address.ParaUseSkill[1], 6);
			this.PostMessage(this.Address.ParaUseSkill[2], 7);
			this.PostMessage(this.Address.FuncUseSkill, 8);
			this.PostMessage(this.Address.ParaUseSkillPet, 9);
			this.PostMessage(this.Address.FuncUseSkillPet, 10);
			this.PostMessage(this.Address.ParaLuaDoString, 11);
			this.PostMessage(this.Address.FuncLuaDoString, 12);
			this.PostMessage(this.Address.ParaPickItem, 13);
			this.PostMessage(this.Address.ParaCollectItem, 14);
			this.PostMessage(this.Address.DropBase[0], 15);
			this.PostMessage(this.Address.DropBase[1], 16);
			this.PostMessage(this.Address.DropBase[2], 17);
			this.PostMessage(this.Address.DropBase[3], 18);
			this.PostMessage(this.Address.ParaLuaToString, 19);
			this.PostMessage(this.FuncLuaToString, 20);
			this.PostMessage(this.Address.ParaTalk, 21);
			this.PostMessage(this.Address.FuncUpLvl, 22);
			this.PostMessage(this.Address.FuncSelectTargetOfTarget, 23);
			this.PostMessage(this.Address.ParaSendPacket, 24);
			this.PostMessage(this.Address.FuncSendPacket, 25);
			this.AddressToString = this.Memory.VirtualAllocEx(20248);
			this.AddressTenBang = this.Memory.VirtualAllocEx(20248);
			this.PostMessage(this.Define, -6);
			this.PostMessage(this.Address.CharState[0], 26);
			int num = this.Memory.Scan("56 8BF1 8B0D ???????? 57 8B78");
			num = this.Memory.Scan("55 8BEC", num - 32, num, 0);
			this.PostMessage(num, 27);
			this.PostMessage(this.Address.CharBase[0], 28);
			try
			{
				if (this.Address.GameType != 1)
				{
					Process processById = Process.GetProcessById(this.ProcessId);
					int num2 = -1;
					int num3 = -1;
					for (int i = 0; i < processById.Modules.Count; i++)
					{
						if (processById.Modules[i].ModuleName.ToLower() == "misahelp.dll")
						{
							num2 = (int)processById.Modules[i].BaseAddress;
							num3 = processById.Modules[i].ModuleMemorySize;
						}
					}
					if (num2 != -1)
					{
						for (int j = 0; j < processById.Threads.Count; j++)
						{
							int num4 = (int)Game.GetThreadStartAddress(processById.Threads[j].Id);
							if (num4 > num2 && num4 < num2 + num3)
							{
								Game.SuspendThread((int)Game.OpenThread(Game.ThreadAccess.SuspendResume, false, (uint)processById.Threads[j].Id));
							}
						}
					}
					int num5 = -1;
					int num6 = -1;
					for (int k = 0; k < processById.Modules.Count; k++)
					{
						if (processById.Modules[k].ModuleName.ToLower() == "celisttl.dll")
						{
							num5 = (int)processById.Modules[k].BaseAddress;
							num6 = processById.Modules[k].ModuleMemorySize;
						}
					}
					if (num5 != -1)
					{
						for (int l = 0; l < processById.Threads.Count; l++)
						{
							int num7 = (int)Game.GetThreadStartAddress(processById.Threads[l].Id);
							if (num7 > num5 && num7 < num5 + num6)
							{
								Game.SuspendThread((int)Game.OpenThread(Game.ThreadAccess.SuspendResume, false, (uint)processById.Threads[l].Id));
							}
						}
					}
				}
			}
			catch
			{
			}
		}

		// Token: 0x06000825 RID: 2085
		[DllImport("kernel32", CharSet = CharSet.Ansi, ExactSpelling = true, SetLastError = true)]
		private static extern uint GetProcAddress(IntPtr hModule, string procName);

		// Token: 0x06000826 RID: 2086 RVA: 0x00034ACC File Offset: 0x00032CCC
		private uint GetRemoteProcAddress(Process targetProcess, string moduleName, string functionName)
		{
			uint num = 0U;
			uint result = 0U;
			foreach (object obj in Process.GetCurrentProcess().Modules)
			{
				ProcessModule processModule = (ProcessModule)obj;
				if (processModule.ModuleName.ToLower() == moduleName)
				{
					uint procAddress = Game.GetProcAddress(processModule.BaseAddress, functionName);
					if (procAddress != 0U)
					{
						num = procAddress - (uint)((int)processModule.BaseAddress);
						break;
					}
					break;
				}
			}
			if (num != 0U)
			{
				foreach (object obj2 in targetProcess.Modules)
				{
					ProcessModule processModule2 = (ProcessModule)obj2;
					if (processModule2.ModuleName.ToLower() == moduleName)
					{
						result = (uint)((int)processModule2.BaseAddress + (int)num);
						break;
					}
				}
			}
			return result;
		}

		// Token: 0x06000827 RID: 2087
		[DllImport("kernel32.dll")]
		public static extern IntPtr OpenProcess(int dwDesiredAccess, bool bInheritHandle, int dwProcessId);

		// Token: 0x06000828 RID: 2088
		[DllImport("kernel32.dll", SetLastError = true)]
		private static extern bool ReadProcessMemory(IntPtr hProcess, IntPtr lpBaseAddress, [Out] byte[] lpBuffer, int dwSize, out IntPtr lpNumberOfBytesRead);

		// Token: 0x06000829 RID: 2089
		[DllImport("kernel32.dll", SetLastError = true)]
		private static extern bool WriteProcessMemory(IntPtr hProcess, IntPtr lpBaseAddress, byte[] lpBuffer, uint nSize, out int lpNumberOfBytesWritten);

		// Token: 0x0600082A RID: 2090 RVA: 0x00034BD0 File Offset: 0x00032DD0
		private int ErasePEHeader(IntPtr hModule, string procName)
		{
			byte[] lpBuffer = new byte[4];
			byte[] lpBuffer2 = new byte[120];
			byte[] lpBuffer3 = new byte[264];
			int num = 0;
			IntPtr hProcess = Game.OpenProcess(2035711, false, Process.GetProcessesByName(procName)[0].Id);
			IntPtr lpBaseAddress = new IntPtr(hModule.ToInt32() + 60);
			IntPtr zero = IntPtr.Zero;
			Game.ReadProcessMemory(hProcess, lpBaseAddress, lpBuffer, 4, out zero);
			int num2;
			if (Game.WriteProcessMemory(hProcess, hModule, lpBuffer2, 120U, out num) && Game.WriteProcessMemory(hProcess, hModule, lpBuffer3, 256U, out num2))
			{
				return num + num2;
			}
			return 0;
		}

		// Token: 0x0600082B RID: 2091
		[DllImport("kernel32.dll", SetLastError = true)]
		private static extern IntPtr OpenThread(Game.ThreadAccess dwDesiredAccess, bool bInheritHandle, uint dwThreadId);

		// Token: 0x0600082C RID: 2092 RVA: 0x00034C60 File Offset: 0x00032E60
		private static IntPtr GetThreadStartAddress(int threadId)
		{
			IntPtr intPtr = Game.OpenThread(Game.ThreadAccess.QueryInformation, false, threadId);
			if (intPtr == IntPtr.Zero)
			{
				return IntPtr.Zero;
			}
			IntPtr intPtr2 = Marshal.AllocHGlobal(IntPtr.Size);
			IntPtr result;
			try
			{
				if (Game.NtQueryInformationThread(intPtr, Game.ThreadInfoClass.ThreadQuerySetWin32StartAddress, intPtr2, IntPtr.Size, IntPtr.Zero) != 0)
				{
					result = IntPtr.Zero;
				}
				else
				{
					result = Marshal.ReadIntPtr(intPtr2);
				}
			}
			finally
			{
				Game.CloseHandle(intPtr);
				Marshal.FreeHGlobal(intPtr2);
			}
			return result;
		}

		// Token: 0x0600082D RID: 2093
		[DllImport("ntdll.dll", SetLastError = true)]
		private static extern int NtQueryInformationThread(IntPtr threadHandle, Game.ThreadInfoClass threadInformationClass, IntPtr threadInformation, int threadInformationLength, IntPtr returnLengthPtr);

		// Token: 0x0600082E RID: 2094
		[DllImport("kernel32.dll", SetLastError = true)]
		private static extern IntPtr OpenThread(Game.ThreadAccess dwDesiredAccess, bool bInheritHandle, int dwThreadId);

		// Token: 0x0600082F RID: 2095
		[DllImport("kernel32.dll", SetLastError = true)]
		private static extern bool CloseHandle(IntPtr hObject);

		// Token: 0x06000830 RID: 2096
		[DllImport("kernel32.dll", SetLastError = true)]
		private static extern int SuspendThread(int hThread);

		// Token: 0x170001C6 RID: 454
		// (get) Token: 0x06000831 RID: 2097 RVA: 0x00034CDC File Offset: 0x00032EDC
		// (set) Token: 0x06000832 RID: 2098 RVA: 0x00034CE4 File Offset: 0x00032EE4
		public bool IsHooked { get; set; }

		// Token: 0x170001C7 RID: 455
		// (get) Token: 0x06000833 RID: 2099 RVA: 0x00034CED File Offset: 0x00032EED
		// (set) Token: 0x06000834 RID: 2100 RVA: 0x00034CF5 File Offset: 0x00032EF5
		public bool IsLostLeader { get; set; }

		// Token: 0x170001C8 RID: 456
		// (get) Token: 0x06000835 RID: 2101 RVA: 0x00034CFE File Offset: 0x00032EFE
		// (set) Token: 0x06000836 RID: 2102 RVA: 0x00034D06 File Offset: 0x00032F06
		public bool IsOpenShop { get; set; }

		// Token: 0x06000837 RID: 2103 RVA: 0x00034D10 File Offset: 0x00032F10
		public void ResetTime()
		{
			if (!Global.AutoResetTime && !this.AutoResetTime)
			{
				return;
			}
			if (this.Address.GameType != 1)
			{
				return;
			}
			if (this.lastReset.Elapsed.TotalMinutes < 2.0)
			{
				return;
			}
			if (this.TLBB.PlayerState == 10 && this.TLBB.OnlineTime < 300)
			{
				return;
			}
			if (this.TLBB.OnlineTime > 177)
			{
				if (this.TLBB.PlayerState == 10)
				{
					this.IsOpenShop = true;
				}
				this.lastReset = Stopwatch.StartNew();
				this.IsOpenPass2 = false;
				this.SafeTime = -10;
				if (this.TLBB.IsLeader)
				{
					this.IsLostLeader = true;
					Game.TickCount = 1;
				}
				this.LUA.DataPoolReConnect();
			}
		}

		// Token: 0x06000838 RID: 2104 RVA: 0x00034DE4 File Offset: 0x00032FE4
		public void ReConnect()
		{
			if (this.Address.GameType != 1)
			{
				return;
			}
			this.IsOpenPass2 = false;
			this.LUA.DataPoolReConnect();
		}

		// Token: 0x06000839 RID: 2105 RVA: 0x00034E08 File Offset: 0x00033008
		private void UpLvl()
		{
			if (Global.AutoUpLvl && this.TLBB.Exp > this.TLBB.MaxExp && this.TLBB.Lvl < Global.AutoUpLvlBelow && this.TLBB.Menpai != 0)
			{
				this.PostMessage(7, 105);
			}
		}

		// Token: 0x0600083A RID: 2106 RVA: 0x00034E5C File Offset: 0x0003305C
		public void RaBang(int map)
		{
			if (this.TLBB.MapId < 500)
			{
				return;
			}
			if (Global.Mapbang && this.IsQDua && !this.GoTo(100f, 158f, false))
			{
				return;
			}
			if (!this.GoTo(BANG.TrinhVoDanh))
			{
				return;
			}
			if (!this.TLBB.IsQuestOpen)
			{
				this.Talk(BANG.TrinhVoDanh.Id);
				return;
			}
			foreach (QuestFrame questFrame in QuestFrame.Enum(this))
			{
				if (questFrame.Name.Contains("#{BHCS_090226_10}"))
				{
					this.QuestFrameOptionClicked(questFrame);
					return;
				}
				if (map == 0 && questFrame.Name.Contains("Quay về Lạc Dương"))
				{
					this.QuestFrameOptionClicked(questFrame);
					return;
				}
				if (map == 1 && questFrame.Name.Contains("#{BHCS_090219_03}"))
				{
					this.QuestFrameOptionClicked(questFrame);
					return;
				}
				if (map == 2 && questFrame.Name.Contains("#{BHCS_090219_02}"))
				{
					this.QuestFrameOptionClicked(questFrame);
					return;
				}
			}
			this.CloseQuest();
		}

		// Token: 0x0600083B RID: 2107 RVA: 0x00034F8C File Offset: 0x0003318C
		public bool HaveItem(string type)
		{
			using (List<PacketItem>.Enumerator enumerator = PacketItem.Enum(this).GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (enumerator.Current.Type == type)
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x0600083C RID: 2108 RVA: 0x00034FEC File Offset: 0x000331EC
		public void DoAction(GameControl control)
		{
			this.PostMessage(control.Object, 112);
		}

		// Token: 0x0600083D RID: 2109 RVA: 0x00034FFC File Offset: 0x000331FC
		public void DoSubAction(GameControl control)
		{
			this.PostMessage(control.Object, 124);
		}

		// Token: 0x0600083E RID: 2110 RVA: 0x0003500C File Offset: 0x0003320C
		public bool DoActionPacket(string type)
		{
			List<PacketItem> list = PacketItem.Enum(this);
			bool flag = false;
			foreach (PacketItem packetItem in list)
			{
				if (packetItem.Type.Trim() == type)
				{
					foreach (GameControl gameControl in this.Controls)
					{
						if (gameControl.Type == packetItem.Type && gameControl.PacketId == packetItem.PacketId)
						{
							this.DoAction(gameControl);
							flag = true;
							break;
						}
					}
					if (!flag)
					{
						this.Controls = GameControl.Enum(this);
						foreach (GameControl gameControl2 in this.Controls)
						{
							if (gameControl2.Type == packetItem.Type && gameControl2.PacketId == packetItem.PacketId)
							{
								this.DoAction(gameControl2);
								flag = true;
								break;
							}
						}
					}
				}
			}
			return flag;
		}

		// Token: 0x0600083F RID: 2111 RVA: 0x0003515C File Offset: 0x0003335C
		public bool DoSubActionPacket(string type)
		{
			List<PacketItem> list = PacketItem.Enum(this);
			bool flag = false;
			foreach (PacketItem packetItem in list)
			{
				if (packetItem.Type.Trim() == type)
				{
					foreach (GameControl gameControl in this.Controls)
					{
						if (gameControl.Type == packetItem.Type && gameControl.PacketId == packetItem.PacketId)
						{
							this.DoSubAction(gameControl);
							flag = true;
							break;
						}
					}
					if (!flag)
					{
						this.Controls = GameControl.Enum(this);
						foreach (GameControl gameControl2 in this.Controls)
						{
							if (gameControl2.Type == packetItem.Type && gameControl2.PacketId == packetItem.PacketId)
							{
								this.DoSubAction(gameControl2);
								flag = true;
								break;
							}
						}
					}
				}
			}
			return flag;
		}

		// Token: 0x06000840 RID: 2112 RVA: 0x000352AC File Offset: 0x000334AC
		public void DoAction(string Type)
		{
			this.Controls = GameControl.Enum(this);
			foreach (GameControl gameControl in this.Controls)
			{
				if (gameControl.Type == Type)
				{
					this.DoAction(gameControl);
					break;
				}
			}
		}

		// Token: 0x06000841 RID: 2113 RVA: 0x0003531C File Offset: 0x0003351C
		public int PhuIndex(int map)
		{
			foreach (PacketItem packetItem in PacketItem.Enum(this))
			{
				if ((TINHKIEM.VietLien(packetItem.Name).Contains("dinhviphu") || TINHKIEM.VietLien(packetItem.Name).Contains("tholinhchau") || TINHKIEM.VietLien(packetItem.Name).Contains("lightdustcapsule")) && packetItem.MapId == map && packetItem.X > 0)
				{
					return packetItem.Index;
				}
			}
			return -1;
		}

		// Token: 0x06000842 RID: 2114 RVA: 0x000353CC File Offset: 0x000335CC
		public int PhuIndex()
		{
			foreach (PacketItem packetItem in PacketItem.Enum(this))
			{
				if ((TINHKIEM.VietLien(packetItem.Name).Contains("dinhviphu") || TINHKIEM.VietLien(packetItem.Name).Contains("tholinhchau")) && packetItem.X > 0 && (packetItem.MapId == 0 || packetItem.MapId == 1 || packetItem.MapId == 2))
				{
					return packetItem.Index;
				}
			}
			return -1;
		}

		// Token: 0x06000843 RID: 2115 RVA: 0x00035478 File Offset: 0x00033678
		public void CloseQuest()
		{
			this.PostMessage(0, 122);
		}

		// Token: 0x06000844 RID: 2116 RVA: 0x00035483 File Offset: 0x00033683
		public void QuestFrameAccept()
		{
			this.PostMessage(13, 105);
		}

		// Token: 0x06000845 RID: 2117 RVA: 0x0002967E File Offset: 0x0002787E
		private void TogleMission()
		{
			this.PostMessage(18, 105);
		}

		// Token: 0x06000846 RID: 2118 RVA: 0x00035490 File Offset: 0x00033690
		public int[,] MapPOINT(int map)
		{
			string text = Setting.LoadMAP(map.ToString());
			text = "96,84-109,52-107,43-108,21-95,20-76,21-63,28-47,20-32,25-23,21-20,31-42,40-42,72-21,94";
			int num = 0;
			foreach (string text2 in text.Split(new char[]
			{
				'-'
			}))
			{
				int num2 = 0;
				int num3 = 0;
				try
				{
					num2 = TINHKIEM.ParseInt(text2.Split(new char[]
					{
						','
					})[0]);
					num3 = TINHKIEM.ParseInt(text2.Split(new char[]
					{
						','
					})[1]);
				}
				catch
				{
				}
				if (num2 != 0 && num3 != 0)
				{
					num++;
				}
			}
			if (num > 1)
			{
				int[,] array2 = new int[num, 2];
				int num4 = 0;
				foreach (string text3 in text.Split(new char[]
				{
					'-'
				}))
				{
					int num5 = 0;
					int num6 = 0;
					try
					{
						num5 = TINHKIEM.ParseInt(text3.Split(new char[]
						{
							','
						})[0]);
						num6 = TINHKIEM.ParseInt(text3.Split(new char[]
						{
							','
						})[1]);
					}
					catch
					{
					}
					if (num5 != 0 && num6 != 0)
					{
						array2[num4, 0] = num5;
						array2[num4, 1] = num6;
						num4++;
					}
				}
				return array2;
			}
			return null;
		}

		// Token: 0x170001C9 RID: 457
		// (get) Token: 0x06000847 RID: 2119 RVA: 0x000355E8 File Offset: 0x000337E8
		public int[,] AcBaPoint
		{
			get
			{
				int i = this.TLBB.MapAcBa;
				string text = Setting.LoadMAP(i.ToString());
				int num = 0;
				foreach (string text2 in text.Split(new char[]
				{
					'-'
				}))
				{
					int num2 = 0;
					int num3 = 0;
					try
					{
						num2 = TINHKIEM.ParseInt(text2.Split(new char[]
						{
							','
						})[0]);
						num3 = TINHKIEM.ParseInt(text2.Split(new char[]
						{
							','
						})[1]);
					}
					catch
					{
					}
					if (num2 != 0 && num3 != 0)
					{
						num++;
					}
				}
				if (num > 1)
				{
					int[,] array2 = new int[num, 2];
					int num4 = 0;
					foreach (string text3 in text.Split(new char[]
					{
						'-'
					}))
					{
						int num5 = 0;
						int num6 = 0;
						try
						{
							num5 = TINHKIEM.ParseInt(text3.Split(new char[]
							{
								','
							})[0]);
							num6 = TINHKIEM.ParseInt(text3.Split(new char[]
							{
								','
							})[1]);
						}
						catch
						{
						}
						if (num5 != 0 && num6 != 0)
						{
							array2[num4, 0] = num5;
							array2[num4, 1] = num6;
							num4++;
						}
					}
					return array2;
				}
				return null;
			}
		}

		// Token: 0x170001CA RID: 458
		// (get) Token: 0x06000848 RID: 2120 RVA: 0x00035744 File Offset: 0x00033944
		public int[,] TamTaiHiepCocPoint
		{
			get
			{
				string text = "195,140-175,140-170,125-190,120-190,110-194,54-165,60-145,65-135,75-130,50-115,45-95,45-70,45-60,55-65,40-50,43-60,65-52,188";
				int num = 0;
				foreach (string text2 in text.Split(new char[]
				{
					'-'
				}))
				{
					int num2 = 0;
					int num3 = 0;
					try
					{
						num2 = TINHKIEM.ParseInt(text2.Split(new char[]
						{
							','
						})[0]);
						num3 = TINHKIEM.ParseInt(text2.Split(new char[]
						{
							','
						})[1]);
					}
					catch
					{
					}
					if (num2 != 0 && num3 != 0)
					{
						num++;
					}
				}
				if (num > 1)
				{
					int[,] array2 = new int[num, 2];
					int num4 = 0;
					foreach (string text3 in text.Split(new char[]
					{
						'-'
					}))
					{
						int num5 = 0;
						int num6 = 0;
						try
						{
							num5 = TINHKIEM.ParseInt(text3.Split(new char[]
							{
								','
							})[0]);
							num6 = TINHKIEM.ParseInt(text3.Split(new char[]
							{
								','
							})[1]);
						}
						catch
						{
						}
						if (num5 != 0 && num6 != 0)
						{
							array2[num4, 0] = num5;
							array2[num4, 1] = num6;
							num4++;
						}
					}
					return array2;
				}
				return null;
			}
		}

		// Token: 0x170001CB RID: 459
		// (get) Token: 0x06000849 RID: 2121 RVA: 0x00035890 File Offset: 0x00033A90
		public int[,] ViemMaSonPoint
		{
			get
			{
				string text = "135,180-135,170-125,165-120,180-110,180-115,160-105,160-90,184-57,81-78,68-83,38-52,31-33,64-55,56-212,40";
				int num = 0;
				foreach (string text2 in text.Split(new char[]
				{
					'-'
				}))
				{
					int num2 = 0;
					int num3 = 0;
					try
					{
						num2 = TINHKIEM.ParseInt(text2.Split(new char[]
						{
							','
						})[0]);
						num3 = TINHKIEM.ParseInt(text2.Split(new char[]
						{
							','
						})[1]);
					}
					catch
					{
					}
					if (num2 != 0 && num3 != 0)
					{
						num++;
					}
				}
				if (num > 1)
				{
					int[,] array2 = new int[num, 2];
					int num4 = 0;
					foreach (string text3 in text.Split(new char[]
					{
						'-'
					}))
					{
						int num5 = 0;
						int num6 = 0;
						try
						{
							num5 = TINHKIEM.ParseInt(text3.Split(new char[]
							{
								','
							})[0]);
							num6 = TINHKIEM.ParseInt(text3.Split(new char[]
							{
								','
							})[1]);
						}
						catch
						{
						}
						if (num5 != 0 && num6 != 0)
						{
							array2[num4, 0] = num5;
							array2[num4, 1] = num6;
							num4++;
						}
					}
					return array2;
				}
				return null;
			}
		}

		// Token: 0x170001CC RID: 460
		// (get) Token: 0x0600084A RID: 2122 RVA: 0x000359DC File Offset: 0x00033BDC
		public int[,] AcTacPoint
		{
			get
			{
				string text = "96,84-109,52-107,43-108,21-95,20-76,21-63,28-47,20-32,25-23,21-20,31-42,40-42,72-21,94";
				int num = 0;
				foreach (string text2 in text.Split(new char[]
				{
					'-'
				}))
				{
					int num2 = 0;
					int num3 = 0;
					try
					{
						num2 = TINHKIEM.ParseInt(text2.Split(new char[]
						{
							','
						})[0]);
						num3 = TINHKIEM.ParseInt(text2.Split(new char[]
						{
							','
						})[1]);
					}
					catch
					{
					}
					if (num2 != 0 && num3 != 0)
					{
						num++;
					}
				}
				if (num > 1)
				{
					int[,] array2 = new int[num, 2];
					int num4 = 0;
					foreach (string text3 in text.Split(new char[]
					{
						'-'
					}))
					{
						int num5 = 0;
						int num6 = 0;
						try
						{
							num5 = TINHKIEM.ParseInt(text3.Split(new char[]
							{
								','
							})[0]);
							num6 = TINHKIEM.ParseInt(text3.Split(new char[]
							{
								','
							})[1]);
						}
						catch
						{
						}
						if (num5 != 0 && num6 != 0)
						{
							array2[num4, 0] = num5;
							array2[num4, 1] = num6;
							num4++;
						}
					}
					return array2;
				}
				return null;
			}
		}

		// Token: 0x0600084B RID: 2123 RVA: 0x00035B28 File Offset: 0x00033D28
		public void MoveNext()
		{
			if (this.TLBB.PlayerState != 0)
			{
				return;
			}
			int i = this.TLBB.MapId;
			string text = Setting.LoadMAP(i.ToString());
			if (this.TLBB.MapId == LACDUONG.Id || (this.TLBB.MapId == 242 && text.Trim() == ""))
			{
				text = "361,187-356,227-329,251-305,251-278,346-214,345-206,317-184,291-181,256-213,203";
			}
			if (this.TLBB.MapId == MAP.PhungHoangCoThanhPhuBan)
			{
				text = "72,109-110,112-108,66-108,23-63,24-21,19-15,25-18,73-65,73";
			}
			if (this.TLBB.MapId == MAP.ThienLongAcBa)
			{
				text = "95,115-75,108-114,106-96,90-70,54-42,48-86,36-109,36-153,46-146,57-122,65-96,38";
			}
			if (this.TLBB.MapId == MAP.ThieuLamAcBa)
			{
				text = "95,131-95,105-69,79-113,83-94,80-94,80";
			}
			if (this.TLBB.MapId == MAP.ThuyLao)
			{
				text = "71,41-107,40-140,39-150,71-148,106-150,139-122,150-85,147-49,149-43,120-41,84-41,50";
			}
			if (this.TLBB.MapId == MAP.PhungHoangCoThanh)
			{
				text = "228,158-228,77-181,92-172,124-116,135-81,162-90,228-162,200-162,231-210,242-230,229-172,151-231,158";
			}
			if (this.TLBB.MapId == MAP.TranLongKyCuoc)
			{
				text = "42,42-84,42-81,81-42,85-50,49-71,50";
			}
			int num = 0;
			foreach (string text2 in text.Split(new char[]
			{
				'-'
			}))
			{
				int num2 = 0;
				int num3 = 0;
				try
				{
					num2 = TINHKIEM.ParseInt(text2.Split(new char[]
					{
						','
					})[0]);
					num3 = TINHKIEM.ParseInt(text2.Split(new char[]
					{
						','
					})[1]);
				}
				catch
				{
				}
				if (num2 != 0 && num3 != 0)
				{
					num++;
				}
			}
			if (num > 1)
			{
				int[,] array2 = new int[num, 2];
				int num4 = 0;
				foreach (string text3 in text.Split(new char[]
				{
					'-'
				}))
				{
					int num5 = 0;
					int num6 = 0;
					try
					{
						num5 = TINHKIEM.ParseInt(text3.Split(new char[]
						{
							','
						})[0]);
						num6 = TINHKIEM.ParseInt(text3.Split(new char[]
						{
							','
						})[1]);
					}
					catch
					{
					}
					if (num5 != 0 && num6 != 0)
					{
						array2[num4, 0] = num5;
						array2[num4, 1] = num6;
						num4++;
					}
				}
				this.MoveNext(array2);
				return;
			}
			if (this.TLBB.MapId == MAP.ThieuLam || this.TLBB.MapId == MAP.ThieuLamPhuBan)
			{
				this.MoveNext(POINT.ThieuLam);
			}
			if (this.TLBB.MapId == MAP.CaiBang || this.TLBB.MapId == MAP.CaiBangPhuBan)
			{
				this.MoveNext(POINT.CaiBang);
			}
			if (this.TLBB.MapId == MAP.MinhGiao || this.TLBB.MapId == MAP.MinhGiaoPhuBan)
			{
				this.MoveNext(POINT.MinhGiao);
			}
			if (this.TLBB.MapId == MAP.VoDang || this.TLBB.MapId == MAP.VoDangPhuBan)
			{
				this.MoveNext(POINT.VoDang);
			}
			if (this.TLBB.MapId == MAP.ThienLong || this.TLBB.MapId == MAP.ThienLongPhuBan)
			{
				this.MoveNext(POINT.ThienLong);
			}
			if (this.TLBB.MapId == MAP.TieuDao || this.TLBB.MapId == MAP.TieuDaoPhuBan)
			{
				this.MoveNext(POINT.TieuDao);
			}
			if (this.TLBB.MapId == MAP.NgaMy || this.TLBB.MapId == MAP.NgaMyPhuBan)
			{
				this.MoveNext(POINT.NgaMy);
			}
			if (this.TLBB.MapId == MAP.TinhTuc || this.TLBB.MapId == MAP.TinhTucPhuBan)
			{
				this.MoveNext(POINT.TinTuc);
			}
			if (this.TLBB.MapId == MAP.ThienSon || this.TLBB.MapId == MAP.ThienSonPhuBan)
			{
				this.MoveNext(POINT.ThienSon);
			}
			if (this.TLBB.MapId == MAP.MoDung)
			{
				this.MoveNext(POINT.MoDung);
			}
			if (this.TLBB.MapId == MAP.DuongMon)
			{
				this.MoveNext(POINT.DuongMon);
			}
			if (this.TLBB.MapId == MAP.ThaiHo)
			{
				this.MoveNext(POINT.ThaiHo);
			}
			if (this.TLBB.MapId == MAP.KiemCac)
			{
				this.MoveNext(POINT.KiemCac);
			}
			if (this.TLBB.MapId == MAP.VoLuongSon)
			{
				this.MoveNext(POINT.VoLuongSon);
			}
			if (this.TLBB.MapId == MAP.DonHoang)
			{
				this.MoveNext(POINT.DonHoang);
			}
			if (this.TLBB.MapId == MAP.TungSon)
			{
				this.MoveNext(POINT.TungSon);
			}
			if (this.TLBB.MapId == MAP.TayHo)
			{
				this.MoveNext(POINT.TayHo);
			}
			if (this.TLBB.MapId == MAP.NhiHai)
			{
				this.MoveNext(POINT.NhiHai);
			}
			if (this.TLBB.MapId == MAP.NhanNam)
			{
				this.MoveNext(POINT.NhanNam);
			}
			if (this.TLBB.MapId == MAP.LongTuyen)
			{
				this.MoveNext(POINT.LongTuyen);
			}
			if (this.TLBB.MapId == MAP.ThuongSon)
			{
				this.MoveNext(POINT.ThuongSon);
			}
			if (this.TLBB.MapId == MAP.NhanBac)
			{
				this.MoveNext(POINT.NhanBac);
			}
			if (this.TLBB.MapId == MAP.VoDi)
			{
				this.MoveNext(POINT.VoDi);
			}
			if (this.TLBB.MapId == MAP.ThachLam)
			{
				this.MoveNext(POINT.ThachLam);
			}
			if (this.TLBB.MapId == MAP.NganNgaiTuyetNguyen)
			{
				this.MoveNext(POINT.NganNgaiTuyetNguyen);
			}
			if (this.TLBB.MapId == MAP.ThaoNguyen)
			{
				this.MoveNext(POINT.ThaoNguyen);
			}
			if (this.TLBB.MapId == MAP.ThieuLamAcBa)
			{
				this.MoveNext(POINT.ThieuLamAcBa);
			}
			if (this.TLBB.MapId == MAP.NgaMyAcBa)
			{
				this.MoveNext(POINT.NgaMyAcBa);
			}
			if (this.TLBB.MapId == MAP.TieuDaoAcBa)
			{
				this.MoveNext(POINT.TieuDaoAcBa);
			}
			if (this.TLBB.MapId == MAP.DuongMonAcBa)
			{
				this.MoveNext(POINT.DuongMonAcBa);
			}
			if (this.TLBB.MapId == MAP.MinhGiaoAcBa)
			{
				this.MoveNext(POINT.MinhGiaoAcBa);
			}
			if (this.TLBB.MapId == MAP.VoDangAcBa)
			{
				this.MoveNext(POINT.VoDangAcBa);
			}
			if (this.TLBB.MapId == MAP.TinhTucAcBa)
			{
				this.MoveNext(POINT.TinhTucAcBa);
			}
			if (this.TLBB.MapId == MAP.ThienSonAcBa)
			{
				this.MoveNext(POINT.ThienSonAcBa);
			}
			if (this.TLBB.MapId == MAP.CaiBangAcBa)
			{
				this.MoveNext(POINT.CaiBangAcBa);
			}
			if (this.TLBB.MapId == MAP.ThienLongAcBa)
			{
				this.MoveNext(POINT.ThienLongAcBa);
			}
			if (this.TLBB.MapId == MAP.MoDungAcBa)
			{
				this.MoveNext(POINT.MoDungAcBa);
			}
			if (this.TLBB.MapId == MAP.TacKhauDoanhDia)
			{
				this.MoveNext(POINT.TacKhauDoanhDia);
			}
			if (this.TLBB.MapId == MAP.ThanhThuSon)
			{
				this.MoveNext(POINT.ThanhThuSon);
			}
			if (this.TLBB.MapId == MAP.LauLan)
			{
				this.MoveNext(POINT.LauLan);
			}
			if (this.TLBB.MapId == MAP.PhungHoangCoThanh)
			{
				this.MoveNext(POINT.PhungHoangCoThanh);
			}
			if (this.TLBB.MapId == MAP.PhungHoangCoThanhPhuBan)
			{
				this.MoveNext(POINT.PhungHoangCoThanhPhuBan);
			}
			if (this.IsNhatHopQDua && this.TLBB.MapId >= 500)
			{
				this.MoveNext(POINT.BangHoiDua);
			}
		}

		// Token: 0x0600084C RID: 2124 RVA: 0x000362EC File Offset: 0x000344EC
		public void MoveNext(int[,] point)
		{
			if (this.MoveIndex == -1)
			{
				if (this.TLBB.MapId == MAP.PhungHoangCoThanh || this.IsMapPhuBan() || this.TLBB.MapId == MAP.ThuyLao)
				{
					this.MoveIndex = 0;
				}
				else
				{
					float num = 9999f;
					for (int i = 0; i < point.GetLength(0); i++)
					{
						float distance = TINHKIEM.GetDistance(this.CharX, this.CharY, (float)point[i, 0], (float)point[i, 0]);
						if (distance < num)
						{
							num = distance;
							this.MoveIndex = i;
						}
					}
				}
			}
			if (this.MoveIndex > point.GetLength(0) - 1)
			{
				if (this.IsThuyLao)
				{
					this.IsXongThuyLao = true;
				}
				this.MoveIndex = 0;
				if (this.IsKyCuoc)
				{
					this.MoveIndex = 0;
				}
			}
			if (TINHKIEM.GetDistance(this.CharX, this.CharY, (float)point[this.MoveIndex, 0], (float)point[this.MoveIndex, 1]) <= 5f)
			{
				this.MoveIndex++;
			}
			if (this.MoveIndex > point.GetLength(0) - 1)
			{
				if (this.IsThuyLao)
				{
					this.IsXongThuyLao = true;
				}
				this.MoveIndex = 0;
			}
			this.Move((float)point[this.MoveIndex, 0], (float)point[this.MoveIndex, 1]);
		}

		// Token: 0x170001CD RID: 461
		// (get) Token: 0x0600084D RID: 2125 RVA: 0x00036442 File Offset: 0x00034642
		// (set) Token: 0x0600084E RID: 2126 RVA: 0x0003644A File Offset: 0x0003464A
		private bool IsXongThuyLao { get; set; }

		// Token: 0x0600084F RID: 2127 RVA: 0x00036454 File Offset: 0x00034654
		public void Next(int[,] point)
		{
			if (this.MoveIndex == -1)
			{
				float num = 9999f;
				for (int i = 0; i < point.GetLength(0); i++)
				{
					if (TINHKIEM.GetDistance(this.CharX, this.CharY, (float)point[i, 0], (float)point[i, 0]) < num)
					{
						this.MoveIndex = i;
					}
				}
			}
			if (this.MoveIndex > point.GetLength(0) - 1)
			{
				this.MoveIndex = 0;
			}
			if (TINHKIEM.GetDistance(this.CharX, this.CharY, (float)point[this.MoveIndex, 0], (float)point[this.MoveIndex, 1]) <= 3f)
			{
				this.MoveIndex++;
			}
			if (this.MoveIndex > point.GetLength(0) - 1)
			{
				this.MoveIndex = 0;
			}
		}

		// Token: 0x06000850 RID: 2128 RVA: 0x00036524 File Offset: 0x00034724
		public void Next()
		{
			int i = this.TLBB.MapId;
			string text = Setting.LoadMAP(i.ToString());
			int num = 0;
			foreach (string text2 in text.Split(new char[]
			{
				'-'
			}))
			{
				int num2 = 0;
				int num3 = 0;
				try
				{
					num2 = TINHKIEM.ParseInt(text2.Split(new char[]
					{
						','
					})[0]);
					num3 = TINHKIEM.ParseInt(text2.Split(new char[]
					{
						','
					})[1]);
				}
				catch
				{
				}
				if (num2 != 0 && num3 != 0)
				{
					num++;
				}
			}
			if (num > 1)
			{
				int[,] array2 = new int[num, 2];
				int num4 = 0;
				foreach (string text3 in text.Split(new char[]
				{
					'-'
				}))
				{
					int num5 = 0;
					int num6 = 0;
					try
					{
						num5 = TINHKIEM.ParseInt(text3.Split(new char[]
						{
							','
						})[0]);
						num6 = TINHKIEM.ParseInt(text3.Split(new char[]
						{
							','
						})[1]);
					}
					catch
					{
					}
					if (num5 != 0 && num6 != 0)
					{
						array2[num4, 0] = num5;
						array2[num4, 1] = num6;
						num4++;
					}
				}
				this.Next(array2);
				return;
			}
			if (this.TLBB.MapId == MAP.ThieuLam || this.TLBB.MapId == MAP.ThieuLamPhuBan)
			{
				this.Next(POINT.ThieuLam);
			}
			if (this.TLBB.MapId == MAP.CaiBang || this.TLBB.MapId == MAP.CaiBangPhuBan)
			{
				this.Next(POINT.CaiBang);
			}
			if (this.TLBB.MapId == MAP.MinhGiao || this.TLBB.MapId == MAP.MinhGiaoPhuBan)
			{
				this.Next(POINT.MinhGiao);
			}
			if (this.TLBB.MapId == MAP.VoDang || this.TLBB.MapId == MAP.VoDangPhuBan)
			{
				this.Next(POINT.VoDang);
			}
			if (this.TLBB.MapId == MAP.ThienLong || this.TLBB.MapId == MAP.ThienLongPhuBan)
			{
				this.Next(POINT.ThienLong);
			}
			if (this.TLBB.MapId == MAP.TieuDao || this.TLBB.MapId == MAP.TieuDaoPhuBan)
			{
				this.Next(POINT.TieuDao);
			}
			if (this.TLBB.MapId == MAP.NgaMy || this.TLBB.MapId == MAP.NgaMyPhuBan)
			{
				this.Next(POINT.NgaMy);
			}
			if (this.TLBB.MapId == MAP.TinhTuc || this.TLBB.MapId == MAP.TinhTucPhuBan)
			{
				this.Next(POINT.TinTuc);
			}
			if (this.TLBB.MapId == MAP.ThienSon || this.TLBB.MapId == MAP.ThienSonPhuBan)
			{
				this.Next(POINT.ThienSon);
			}
			if (this.TLBB.MapId == MAP.MoDung)
			{
				this.Next(POINT.MoDung);
			}
			if (this.TLBB.MapId == MAP.DuongMon)
			{
				this.Next(POINT.DuongMon);
			}
			if (this.TLBB.MapId == MAP.ThaiHo)
			{
				this.Next(POINT.ThaiHo);
			}
			if (this.TLBB.MapId == MAP.KiemCac)
			{
				this.Next(POINT.KiemCac);
			}
			if (this.TLBB.MapId == MAP.VoLuongSon)
			{
				this.Next(POINT.VoLuongSon);
			}
			if (this.TLBB.MapId == MAP.DonHoang)
			{
				this.Next(POINT.DonHoang);
			}
			if (this.TLBB.MapId == MAP.TungSon)
			{
				this.Next(POINT.TungSon);
			}
			if (this.TLBB.MapId == MAP.TayHo)
			{
				this.Next(POINT.TayHo);
			}
			if (this.TLBB.MapId == MAP.NhiHai)
			{
				this.Next(POINT.NhiHai);
			}
			if (this.TLBB.MapId == MAP.NhanNam)
			{
				this.Next(POINT.NhanNam);
			}
			if (this.TLBB.MapId == MAP.LongTuyen)
			{
				this.Next(POINT.LongTuyen);
			}
			if (this.TLBB.MapId == MAP.ThuongSon)
			{
				this.Next(POINT.ThuongSon);
			}
			if (this.TLBB.MapId == MAP.NhanBac)
			{
				this.Next(POINT.NhanBac);
			}
			if (this.TLBB.MapId == MAP.VoDi)
			{
				this.Next(POINT.VoDi);
			}
			if (this.TLBB.MapId == MAP.ThachLam)
			{
				this.Next(POINT.ThachLam);
			}
			if (this.TLBB.MapId == MAP.NganNgaiTuyetNguyen)
			{
				this.Next(POINT.NganNgaiTuyetNguyen);
			}
			if (this.TLBB.MapId == MAP.ThaoNguyen)
			{
				this.Next(POINT.ThaoNguyen);
			}
			if (this.TLBB.MapId == MAP.ThieuLamAcBa)
			{
				this.Next(POINT.ThieuLamAcBa);
			}
			if (this.TLBB.MapId == MAP.NgaMyAcBa)
			{
				this.Next(POINT.NgaMyAcBa);
			}
			if (this.TLBB.MapId == MAP.TieuDaoAcBa)
			{
				this.Next(POINT.TieuDaoAcBa);
			}
			if (this.TLBB.MapId == MAP.DuongMonAcBa)
			{
				this.Next(POINT.DuongMonAcBa);
			}
			if (this.TLBB.MapId == MAP.MinhGiaoAcBa)
			{
				this.Next(POINT.MinhGiaoAcBa);
			}
			if (this.TLBB.MapId == MAP.VoDangAcBa)
			{
				this.Next(POINT.VoDangAcBa);
			}
			if (this.TLBB.MapId == MAP.TinhTucAcBa)
			{
				this.Next(POINT.TinhTucAcBa);
			}
			if (this.TLBB.MapId == MAP.ThienSonAcBa)
			{
				this.Next(POINT.ThienSonAcBa);
			}
			if (this.TLBB.MapId == MAP.CaiBangAcBa)
			{
				this.Next(POINT.CaiBangAcBa);
			}
			if (this.TLBB.MapId == MAP.ThienLongAcBa)
			{
				this.Next(POINT.ThienLongAcBa);
			}
			if (this.TLBB.MapId == MAP.MoDungAcBa)
			{
				this.Next(POINT.MoDungAcBa);
			}
			if (this.TLBB.MapId == MAP.TacKhauDoanhDia)
			{
				this.Next(POINT.TacKhauDoanhDia);
			}
			if (this.TLBB.MapId == MAP.ThanhThuSon)
			{
				this.Next(POINT.ThanhThuSon);
			}
			if (this.TLBB.MapId == MAP.LauLan)
			{
				this.Next(POINT.LauLan);
			}
			if (this.TLBB.MapId == MAP.PhungHoangCoThanh)
			{
				this.Next(POINT.PhungHoangCoThanh);
			}
			if (this.TLBB.MapId == MAP.PhungHoangCoThanhPhuBan)
			{
				this.Next(POINT.PhungHoangCoThanhPhuBan);
			}
		}

		// Token: 0x06000851 RID: 2129 RVA: 0x00036BE8 File Offset: 0x00034DE8
		public void SetTeam(string team)
		{
			if (team == null || team == "")
			{
				this.LuaDoUnicodeString("TEAM = nil;");
				return;
			}
			this.LuaDoUnicodeString("TEAM = \"" + team.Replace("\r\n", "") + "\";");
		}

		// Token: 0x06000852 RID: 2130 RVA: 0x00036C38 File Offset: 0x00034E38
		public void SetTeamFromList(List<BuffPramenter> DanhSachBuff)
		{
			string text = "";
			if (DanhSachBuff.Count > 0)
			{
				foreach (BuffPramenter buffPramenter in DanhSachBuff.ToArray())
				{
					text = text + buffPramenter.IDnguoichoi + " - " + buffPramenter.TenNguoiChoi;
				}
			}
			if (text == null || text == "")
			{
				this.LuaDoUnicodeString("TEAM = nil;");
				return;
			}
			this.LuaDoUnicodeString("TEAM = \"" + text.Replace("\r\n", "") + "\";");
		}

		// Token: 0x06000853 RID: 2131 RVA: 0x00036CC6 File Offset: 0x00034EC6
		public void Accept()
		{
			this.PostMessage(29, 105);
		}

		// Token: 0x06000854 RID: 2132 RVA: 0x00036CD2 File Offset: 0x00034ED2
		public void AppointLeader(string name)
		{
			this.LuaDoUnicodeString("AppointLeader(\"" + name + "\")");
		}

		// Token: 0x06000855 RID: 2133 RVA: 0x00036CEA File Offset: 0x00034EEA
		public void AcceptAll()
		{
			this.SetTeam(null);
			this.Accept();
			if (Global.AutoAccept && !Global.AcceptAll)
			{
				this.SetTeamFromList(Setting.BuffValue);
			}
		}

		// Token: 0x06000856 RID: 2134 RVA: 0x00036D14 File Offset: 0x00034F14
		public void FixKetMap()
		{
			if (this.KetMap == 0)
			{
				this.Move(this.CharX - 3f, this.CharY - 3f);
				this.KetMap++;
				return;
			}
			if (this.KetMap == 1)
			{
				this.Move(this.CharX, this.CharY - 3f);
				this.KetMap++;
				return;
			}
			if (this.KetMap == 2)
			{
				this.Move(this.CharX + 3f, this.CharY + 3f);
				this.KetMap++;
				return;
			}
			if (this.KetMap == 3)
			{
				this.Move(this.CharX + 3f, this.CharY);
				this.KetMap++;
				return;
			}
			if (this.KetMap == 4)
			{
				this.Move(this.CharX + 3f, this.CharY + 3f);
				this.KetMap++;
				return;
			}
			if (this.KetMap == 5)
			{
				this.Move(this.CharX, this.CharY + 3f);
				this.KetMap++;
				return;
			}
			if (this.KetMap == 6)
			{
				this.Move(this.CharX - 3f, this.CharY + 3f);
				this.KetMap++;
				return;
			}
			if (this.KetMap == 7)
			{
				this.Move(this.CharX - 3f, this.CharY);
				this.KetMap = 0;
				return;
			}
		}

		// Token: 0x170001CE RID: 462
		// (get) Token: 0x06000857 RID: 2135 RVA: 0x00036EB9 File Offset: 0x000350B9
		// (set) Token: 0x06000858 RID: 2136 RVA: 0x00036EC1 File Offset: 0x000350C1
		public int CatchPetTime { get; set; }

		// Token: 0x170001CF RID: 463
		// (get) Token: 0x06000859 RID: 2137 RVA: 0x00036ECA File Offset: 0x000350CA
		// (set) Token: 0x0600085A RID: 2138 RVA: 0x00036ED2 File Offset: 0x000350D2
		public bool IsNhanLeBao { get; set; }

		// Token: 0x0600085B RID: 2139 RVA: 0x00036EDC File Offset: 0x000350DC
		public void Buy(int index)
		{
			int num = this.Memory.Read(this.Address.BaseShopItem);
			num = this.Memory.Read(num + index * 4);
			if (num == 0)
			{
				return;
			}
			this.PostMessage(num, 113);
		}

		// Token: 0x0600085C RID: 2140 RVA: 0x00036F20 File Offset: 0x00035120
		public bool IsNPCSuMon(GameObject _object)
		{
			return TINHKIEM.VietLien(_object.Title).Contains("nguoigiaonhiemvu") || TINHKIEM.VietLien(_object.Title).Contains("congbonhiemvu") || TINHKIEM.VietLien(_object.Title).Contains("nhiemvutuyendatsu") || TINHKIEM.VietLien(_object.Title).Contains("nhiemvutuyendotsu");
		}

		// Token: 0x170001D0 RID: 464
		// (get) Token: 0x0600085D RID: 2141 RVA: 0x00036F8B File Offset: 0x0003518B
		// (set) Token: 0x0600085E RID: 2142 RVA: 0x00036F93 File Offset: 0x00035193
		public TINHKIEM.Menpai SetMenPai { get; set; }

		// Token: 0x170001D1 RID: 465
		// (get) Token: 0x0600085F RID: 2143 RVA: 0x00036F9C File Offset: 0x0003519C
		// (set) Token: 0x06000860 RID: 2144 RVA: 0x00036FA4 File Offset: 0x000351A4
		public bool IsSetMenPai { get; set; }

		// Token: 0x170001D2 RID: 466
		// (get) Token: 0x06000861 RID: 2145 RVA: 0x00036FAD File Offset: 0x000351AD
		// (set) Token: 0x06000862 RID: 2146 RVA: 0x00036FB5 File Offset: 0x000351B5
		private bool isAlarmVaoPhai { get; set; }

		// Token: 0x06000863 RID: 2147 RVA: 0x00036FC0 File Offset: 0x000351C0
		public void VaoPhai()
		{
			if (this.ON_SCENE_TRANSING || this.IsChangeMap)
			{
				return;
			}
			this.TLBB.Base = this.Memory.Read(this.Address.CharBase);
			if (this.Address.GameType == 1)
			{
				this.TLBB.Id = this.Memory.Read8Byte(this.TLBB.Base + this.Address.CharId).ToString("X8");
			}
			else
			{
				this.TLBB.Id = this.Memory.Read(this.TLBB.Base + this.Address.CharId).ToString("X8");
			}
			this.TLBB.Name = this.Memory.ReadString(this.TLBB.Base + this.Address.CharName);
			if (this.TLBB.Name == "")
			{
				this.TLBB.Name = "ĐăngNhập";
			}
			if (!this.TLBB.Online)
			{
				return;
			}
			if (Option.PutBase && this.TLBB.Menpai != 0 && !Skill.IsBase(this.SkillId(0)))
			{
				this.LuaDoOneLineString("MainmenuBar_JoinMenpai()");
			}
			if (this.TLBB.Menpai != 0)
			{
				return;
			}
			if (!this.isAlarmVaoPhai)
			{
				this.isAlarmVaoPhai = true;
				this.alarmVaoPhai = new AlarmVaoPhai(this);
			}
			if (!this.IsSetMenPai)
			{
				return;
			}
			if (this.TLBB.Lvl != 10)
			{
				return;
			}
			NPC npc = new NPC();
			TINHKIEM.Menpai setMenPai = this.SetMenPai;
			switch (setMenPai)
			{
			case TINHKIEM.Menpai.ThieuLam:
				npc = NPC.HuyenTich;
				break;
			case TINHKIEM.Menpai.MinhGiao:
				npc = NPC.LaSuTuong;
				break;
			case TINHKIEM.Menpai.CaiBang:
				npc = NPC.TranCoNhan;
				break;
			case TINHKIEM.Menpai.VoDang:
				npc = NPC.TruongHuyenTo;
				break;
			case TINHKIEM.Menpai.NgaMy:
				npc = NPC.LyThapNhiNuong;
				break;
			case TINHKIEM.Menpai.TinhTuc:
				npc = NPC.HanTheTrung;
				break;
			case TINHKIEM.Menpai.ThienLong:
				npc = NPC.BanNhan;
				break;
			case TINHKIEM.Menpai.ThienSon:
				npc = NPC.MaiKiem;
				break;
			case TINHKIEM.Menpai.TieuDao:
				npc = NPC.ToTinhHa;
				break;
			default:
				if (setMenPai != TINHKIEM.Menpai.MoDung)
				{
					if (setMenPai == TINHKIEM.Menpai.DuongMon)
					{
						npc = NPC.DuongXichPhong;
					}
				}
				else
				{
					npc = NPC.MoDungKiet;
				}
				break;
			}
			if (npc.Id == -1)
			{
				return;
			}
			if (this.GoTo(npc))
			{
				if (this.IsTalkToNpc == 0)
				{
					this.IsTalkToNpc = 1;
					this.Talk(npc.Id);
					return;
				}
				if (QuestFrame.GetCount(this) == 3)
				{
					this.QuestFrameOptionClicked(QuestFrame.Enum(this)[1]);
					this.IsTalkToNpc = 0;
				}
				foreach (QuestFrame questFrame in QuestFrame.Enum(this))
				{
					if (questFrame.Name == "#GVào môn phái")
					{
						this.QuestFrameOptionClicked(questFrame);
						return;
					}
				}
				this.IsTalkToNpc = 0;
			}
		}

		// Token: 0x06000864 RID: 2148 RVA: 0x000372A8 File Offset: 0x000354A8
		public bool GanNPCSuMon()
		{
			return (this.TLBB.Menpai == 1 && this.TLBB.MapId == 9 && TINHKIEM.GetDistance(this.CharX, this.CharY, 96f, 82f) <= 3f) || (this.TLBB.Menpai == 2 && this.TLBB.MapId == 11 && TINHKIEM.GetDistance(this.CharX, this.CharY, 98f, 105f) <= 3f) || (this.TLBB.Menpai == 3 && this.TLBB.MapId == 10 && TINHKIEM.GetDistance(this.CharX, this.CharY, 92f, 77f) <= 3f) || (this.TLBB.Menpai == 4 && this.TLBB.MapId == 12 && TINHKIEM.GetDistance(this.CharX, this.CharY, 78f, 95f) <= 3f) || (this.TLBB.Menpai == 5 && this.TLBB.MapId == 15 && TINHKIEM.GetDistance(this.CharX, this.CharY, 95f, 86f) <= 3f) || (this.TLBB.Menpai == 6 && this.TLBB.MapId == 16 && TINHKIEM.GetDistance(this.CharX, this.CharY, 96f, 92f) <= 3f) || (this.TLBB.Menpai == 8 && this.TLBB.MapId == 17 && TINHKIEM.GetDistance(this.CharX, this.CharY, 95f, 60f) <= 3f) || (this.TLBB.Menpai == 9 && this.TLBB.MapId == 14 && TINHKIEM.GetDistance(this.CharX, this.CharY, 119f, 152f) <= 3f) || (this.TLBB.Menpai == 32 && this.TLBB.MapId == 284 && TINHKIEM.GetDistance(this.CharX, this.CharY, 69f, 125f) <= 3f) || (this.TLBB.Menpai == 37 && this.TLBB.MapId == 615 && TINHKIEM.GetDistance(this.CharX, this.CharY, 100f, 64f) <= 3f) || (this.TLBB.Menpai == MENPAI.ThienLong && this.TLBB.MapId == MAP.ThienLong && TINHKIEM.GetDistance(this.CharX, this.CharY, 95f, 88f) <= 3f);
		}

		// Token: 0x06000865 RID: 2149 RVA: 0x0003759C File Offset: 0x0003579C
		public bool DenSuMon()
		{
			if (this.TLBB.Menpai == 1)
			{
				if (this.TLBB.MapId != 9)
				{
					this.GoTo(96f, 82f, 9, false);
					return false;
				}
				if (TINHKIEM.GetDistance(this.CharX, this.CharY, 96f, 82f) > 3f)
				{
					this.GoTo(96f, 82f, false);
					return false;
				}
			}
			if (this.TLBB.Menpai == 2)
			{
				if (this.TLBB.MapId != 11)
				{
					this.GoTo(98f, 105f, 11, false);
					return false;
				}
				if (TINHKIEM.GetDistance(this.CharX, this.CharY, 98f, 105f) > 3f)
				{
					this.GoTo(98f, 105f, false);
					return false;
				}
			}
			if (this.TLBB.Menpai == 3)
			{
				if (this.TLBB.MapId != 10)
				{
					this.GoTo(92f, 77f, 10, false);
					return false;
				}
				if (TINHKIEM.GetDistance(this.CharX, this.CharY, 92f, 77f) > 3f)
				{
					this.GoTo(92f, 77f, false);
					return false;
				}
			}
			if (this.TLBB.Menpai == 4)
			{
				if (this.TLBB.MapId != 12)
				{
					this.GoTo(78f, 95f, 12, false);
					return false;
				}
				if (TINHKIEM.GetDistance(this.CharX, this.CharY, 78f, 95f) > 3f)
				{
					this.GoTo(78f, 95f, false);
					return false;
				}
			}
			if (this.TLBB.Menpai == 5)
			{
				if (this.TLBB.MapId != 15)
				{
					this.GoTo(95f, 86f, 15, false);
					return false;
				}
				if (TINHKIEM.GetDistance(this.CharX, this.CharY, 95f, 86f) > 3f)
				{
					this.GoTo(95f, 86f, false);
					return false;
				}
			}
			if (this.TLBB.Menpai == 6)
			{
				if (this.TLBB.MapId != 16)
				{
					this.GoTo(96f, 92f, 16, false);
					return false;
				}
				if (TINHKIEM.GetDistance(this.CharX, this.CharY, 96f, 92f) > 3f)
				{
					this.GoTo(96f, 92f, false);
					return false;
				}
			}
			if (this.TLBB.Menpai == 8)
			{
				if (this.TLBB.MapId != 17)
				{
					this.GoTo(95f, 60f, 17, false);
					return false;
				}
				if (TINHKIEM.GetDistance(this.CharX, this.CharY, 95f, 60f) > 3f)
				{
					this.GoTo(95f, 60f, false);
					return false;
				}
			}
			if (this.TLBB.Menpai == 9)
			{
				if (this.TLBB.MapId != 14)
				{
					this.GoTo(119f, 152f, 14, false);
					return false;
				}
				if (TINHKIEM.GetDistance(this.CharX, this.CharY, 119f, 152f) > 3f)
				{
					this.GoTo(119f, 152f, false);
					return false;
				}
			}
			if (this.TLBB.Menpai == 32)
			{
				if (this.TLBB.MapId != 284)
				{
					this.GoTo(69f, 125f, 284, false);
					return false;
				}
				if (TINHKIEM.GetDistance(this.CharX, this.CharY, 69f, 125f) > 3f)
				{
					this.GoTo(69f, 125f, false);
					return false;
				}
			}
			if (this.TLBB.Menpai == 37)
			{
				if (this.TLBB.MapId != 615)
				{
					this.GoTo(100f, 64f, 615, false);
					return false;
				}
				if (TINHKIEM.GetDistance(this.CharX, this.CharY, 100f, 64f) > 3f)
				{
					this.GoTo(100f, 64f, false);
					return false;
				}
			}
			if (this.TLBB.Menpai == MENPAI.ThienLong)
			{
				if (this.TLBB.MapId != MAP.ThienLong)
				{
					this.GoTo(95f, 88f, MAP.ThienLong, false);
					return false;
				}
				if (TINHKIEM.GetDistance(this.CharX, this.CharY, 95f, 88f) > 3f)
				{
					this.GoTo(95f, 88f, false);
					return false;
				}
			}
			return true;
		}

		// Token: 0x06000866 RID: 2150 RVA: 0x00037A48 File Offset: 0x00035C48
		public void ClearNhiemVu()
		{
			this.TrangThaiLuyenKim = (this.TrangThaiSuMon = (this.TrangThaiTuBaoBon = (this.TrangThaiXayDung = (this.TrangThaiTuDuong = (this.TrangThaiQD = "")))));
			this.IsLuyenKim = (this.IsSuMon = (this.IsTuBaoBon = (this.IsTuDuong = (this.IsXayDung = (this.IsTrungAc = (this.IsNguyenVong = false))))));
			this.IsLPMH = false;
			this.IsNhanh = false;
			this.State = STATE.None;
			if (this.IsTrungAc)
			{
				this.State = STATE.Null;
			}
			this.IsDauCo = false;
			this.DaNhanHoaHong = false;
			this.DaNhanHoaChung = false;
			this.IsKhoang = (this.IsDuoc = false);
			this.IsTrongTrot = (this.IsThuHoach = false);
			this.IsVanMay = (this.IsLyHoa = (this.IsNguHanhPhap = false));
			this.OkNhanDa = false;
			this.IsTueHong = false;
			this.IsChucPhuc = false;
			this.IsNhatHop = false;
			this.BachHoaDuyenCompleted = false;
			this.IsCauOThuoc = false;
			this.IsDead = false;
			this.IsCheDo = false;
			this.MapAcTac = 0;
			this.LuaDoOneLineString("COUNT = nil;");
			if (!this.IsNotClear)
			{
				this.IsNhanQuaBuiHoaHong = false;
				this.IsNhanHoaHongLo = false;
				this.NhanQuaHoaHongCompleted = false;
				this.IsNhatHopall = false;
				this.IsChayVong = false;
				this.IsNhatHopQDua = false;
				this.IsQDua = false;
				this.QDuaCompleted = false;
				this.IsMoBang = false;
			}
		}

		// Token: 0x170001D3 RID: 467
		// (get) Token: 0x06000867 RID: 2151 RVA: 0x00037BD8 File Offset: 0x00035DD8
		public bool IsPickEx
		{
			get
			{
				return this.TLBB.MapId == MAP.ThieuThatSon || this.TLBB.MapId == MAP.PhungMinhVuongLang || this.TLBB.MapId == MAP.PhieuMieuPhong || this.TLBB.MapId == MAP.YenTuO || this.TLBB.MapId == MAP.TangKinhCac;
			}
		}

		// Token: 0x06000868 RID: 2152 RVA: 0x00037C44 File Offset: 0x00035E44
		public bool IsCollect()
		{
			return this.IsNhatHopQDua || this.IsOptLocDo || this.IsQDua || this.IsNhatHopall || this.IsLuyenKim || this.IsPhuMau || this.IsSuMon || this.IsTuBaoBon || this.IsTuDuong || this.IsXayDung || this.IsTrungAc || this.IsBachHoaDuyen || this.IsNguyenVong || this.IsThuHoach || this.IsKhoang || this.IsDuoc || this.IsNhatTuyet || this.IsNhatHop || this.IsNhiemVuCoBan || this.IsPickItem || this.IsPickEx || Global.PickItem;
		}

		// Token: 0x06000869 RID: 2153 RVA: 0x00037D14 File Offset: 0x00035F14
		public bool OSuMon()
		{
			if (this.TLBB.Menpai == MENPAI.ThieuLam && this.TLBB.MapId == MAP.ThieuLam)
			{
				return true;
			}
			if (this.TLBB.Menpai == MENPAI.MinhGiao && this.TLBB.MapId == MAP.MinhGiao)
			{
				return true;
			}
			if (this.TLBB.Menpai == MENPAI.CaiBang && this.TLBB.MapId == MAP.CaiBang)
			{
				return true;
			}
			if (this.TLBB.Menpai == MENPAI.VoDang && this.TLBB.MapId == MAP.VoDang)
			{
				return true;
			}
			if (this.TLBB.Menpai == MENPAI.NgaMy && this.TLBB.MapId == MAP.NgaMy)
			{
				return true;
			}
			if (this.TLBB.Menpai == MENPAI.TinhTuc && this.TLBB.MapId == MAP.TinhTuc)
			{
				return true;
			}
			if (this.TLBB.Menpai == MENPAI.ThienLong && this.TLBB.MapId == MAP.ThienLong)
			{
				return true;
			}
			if (this.TLBB.Menpai == MENPAI.ThienSon && this.TLBB.MapId == MAP.ThienSon)
			{
				return true;
			}
			if (this.TLBB.Menpai == MENPAI.TieuDao && this.TLBB.MapId == MAP.TieuDao)
			{
				return true;
			}
			if (this.TLBB.Menpai == MENPAI.MoDung && this.TLBB.MapId == MAP.MoDung)
			{
				return true;
			}
			if (this.TLBB.Menpai == MENPAI.DuongMon && this.TLBB.MapId == MAP.DuongMon)
			{
				return true;
			}
			this.DenSuMon();
			return false;
		}

		// Token: 0x0600086A RID: 2154 RVA: 0x00037ECB File Offset: 0x000360CB
		public void GoHuyenVuDao()
		{
			if (User.TienXu >= 100000)
			{
				this.PostMessage(25, 105);
			}
		}

		// Token: 0x0600086B RID: 2155 RVA: 0x00037EE4 File Offset: 0x000360E4
		public void Ride()
		{
			if (this.TimeStand.Elapsed.TotalSeconds < 1.0)
			{
				return;
			}
			this.UseSkill(21);
			this.StandTime = 0;
		}

		// Token: 0x0600086C RID: 2156 RVA: 0x00037F20 File Offset: 0x00036120
		public void AOE()
		{
			if (Game.TickCount % 150 != 0 || !Global.UseSkillPet)
			{
				return;
			}
			if (this.Objects.NearMonter20m.Count < 0)
			{
				return;
			}
			if (this.SkillPetId(this.TLBB.SkillPetType) != -1)
			{
				this.UseSkillPet(this.SkillPetId(this.TLBB.SkillPetType), this.Objects.Monter[0].X, this.Objects.Monter[0].Y);
				return;
			}
		}

		// Token: 0x0600086D RID: 2157 RVA: 0x00037FB0 File Offset: 0x000361B0
		public int SkillPetId(string name)
		{
			if (name.Contains("MenpaiLiveSkill2_14"))
			{
				return 742;
			}
			if (name.Contains("MenpaiLiveSkill2_13"))
			{
				return 743;
			}
			if (name.Contains("MenpaiLiveSkill2_16"))
			{
				return 744;
			}
			if (name.Contains("MenpaiLiveSkill2_15"))
			{
				return 745;
			}
			if (name.Contains("PetSkill4_13"))
			{
				return 676;
			}
			if (name.Contains("PetSkill4_14"))
			{
				return 677;
			}
			if (name.Contains("PetSkill7_7"))
			{
				return 672;
			}
			if (name.Contains("PetSkill1_8"))
			{
				return 673;
			}
			if (name.Contains("PetSkill7_8"))
			{
				return 674;
			}
			if (name.Contains("PetSkill3_4"))
			{
				return 675;
			}
			if (name.Contains("PetSkill4_15"))
			{
				return 747;
			}
			if (name.Contains("PetSkill7_1"))
			{
				return 694;
			}
			if (name.Contains("PetSkill7_2"))
			{
				return 695;
			}
			return -1;
		}

		// Token: 0x170001D4 RID: 468
		// (get) Token: 0x0600086E RID: 2158 RVA: 0x000380B8 File Offset: 0x000362B8
		public int MoVangNum
		{
			get
			{
				foreach (PacketItem packetItem in PacketItem.Enum(this))
				{
					if (packetItem.Type == "Ore_5")
					{
						return packetItem.Count;
					}
				}
				return 0;
			}
		}

		// Token: 0x0600086F RID: 2159 RVA: 0x00038124 File Offset: 0x00036324
		public void Talk(NPC npc)
		{
			this.PostMessage(npc.Id, 110);
		}

		// Token: 0x06000870 RID: 2160 RVA: 0x00038134 File Offset: 0x00036334
		public void Talk(string name)
		{
			foreach (GameObject gameObject in this.Objects.All)
			{
				if (gameObject.CleanName.Contains(TINHKIEM.VietLien(name)))
				{
					this.Talk(gameObject.Id);
					break;
				}
			}
		}

		// Token: 0x06000871 RID: 2161 RVA: 0x000381A8 File Offset: 0x000363A8
		public void Talk(int id)
		{
			this.PostMessage(id, 110);
		}

		// Token: 0x06000872 RID: 2162 RVA: 0x000381B3 File Offset: 0x000363B3
		public void QuestFrameOptionClicked(QuestFrame dialog)
		{
			this.QuestFrameOptionClicked(dialog.StrOptionExtra1, dialog.StrOptionExtra2);
		}

		// Token: 0x06000873 RID: 2163 RVA: 0x000381C7 File Offset: 0x000363C7
		public void QuestFrameOptionClicked(int StrOptionExtra1, int StrOptionExtra2)
		{
			this.PostMessage(StrOptionExtra1, 54);
			this.PostMessage(StrOptionExtra2, 55);
			this.PostMessage(12, 105);
		}

		// Token: 0x06000874 RID: 2164 RVA: 0x000381E5 File Offset: 0x000363E5
		public bool IsDropEx(PacketItem item)
		{
			return item.Lvl != 0 && (item.Star >= 1 && item.Star <= 1) && Game.TrangBi.Contains(item.TypeName);
		}

		// Token: 0x170001D5 RID: 469
		// (get) Token: 0x06000875 RID: 2165 RVA: 0x00038215 File Offset: 0x00036415
		// (set) Token: 0x06000876 RID: 2166 RVA: 0x0003821D File Offset: 0x0003641D
		private bool IsNhanBinhMau { get; set; }

		// Token: 0x06000877 RID: 2167 RVA: 0x00038228 File Offset: 0x00036428
		public void DropItem()
		{
			if (!this.IsOpenPass2)
			{
				return;
			}
			if (this.TLBB.SafeTime > 0)
			{
				return;
			}
			if (this.IsBank && Game.TickCount % 18 == 0 && this.Address.GameType == 2 && !Global.Paused)
			{
				if (this.TLBB.MapId != LACDUONG.Id)
				{
					this.TimDuong(275f, 295f, 0);
					return;
				}
				if (TINHKIEM.GetDistance(this.CharX, this.CharY, (float)LACDUONG.ThuongKho.X, (float)LACDUONG.ThuongKho.Y) > 3f)
				{
					this.GoTo(LACDUONG.ThuongKho);
					return;
				}
				if (!this.TLBB.IsBankOpen)
				{
					this.Talk(LACDUONG.ThuongKho);
					this.QuestFrameOptionClicked(7, -1);
				}
				if (this.TLBB.IsBankOpen)
				{
					int count = new Bank(this).DaoCu.Count;
					foreach (PacketItem packetItem in PacketItem.Enum(this))
					{
						if (!(packetItem.Name == "") && !(packetItem.TypeName == "") && !(packetItem.Type == ""))
						{
							this.DoSubActionPacket(packetItem.Type);
							this.PushDebugMessage("Auto vừa cất " + packetItem.Name + " vào rương");
						}
					}
				}
			}
			if (Game.TickCount % 18 == 0 && Global.IsVutRac)
			{
				if (!this.IsOpenBag)
				{
					this.IsOpenBag = true;
					this.OpenBag();
				}
				foreach (PacketItem packetItem2 in PacketItem.Enum(this))
				{
					if (this.IsCanDelete(packetItem2) && !(packetItem2.Name == "") && !(packetItem2.TypeName == "") && !(packetItem2.Type == "") && (this.IsDropEx(packetItem2) || Game.JunkItemName.Contains(packetItem2.Name) || Game.JunkItemType.Contains(packetItem2.TypeName)) && !packetItem2.IsHaveLongVan && !packetItem2.IsHaveNgoc)
					{
						this.PostMessage(packetItem2.Index, 108);
						this.PushDebugMessage(string.Concat(new string[]
						{
							"Auto vừa hủy ",
							packetItem2.Name,
							" [",
							packetItem2.TypeName,
							"] (Rác)"
						}));
						FrmMain.AddLog(string.Concat(new string[]
						{
							DateTime.Now.ToString("dd-MM | HH:mm"),
							" [",
							this.LastName,
							"] hủy vật phẩm :",
							packetItem2.Name,
							" (Rác) \n"
						}));
						return;
					}
				}
			}
			if (Game.TickCount % 60 == 0 && this.IsNhiemVuCoBan)
			{
				if (!this.IsNhanBinhMau && this.TLBB.Lvl >= 20 && Game.TickCount % 300 == 0)
				{
					this.LuaDoOneLineString("setmetatable(_G, {__index = QiankunBag_Env}); QiankunBag_Clicked(1);");
				}
				foreach (PacketItem packetItem3 in PacketItem.Enum(this))
				{
					if (!(packetItem3.Name == "") && !(packetItem3.TypeName == "") && !(packetItem3.Type == ""))
					{
						if (packetItem3.ClearName.Contains("ngandanholo"))
						{
							this.IsNhanBinhMau = true;
						}
						if (packetItem3.Name.Contains("Thú Cưỡi") && (packetItem3.Name.Contains("Hoa Hồng Đen") || packetItem3.Name.Contains("Phúc Thụy Tuyết Điêu")))
						{
							this.DoActionPacket(packetItem3.Type);
						}
						if (packetItem3.Name.Contains("Vô Ưu"))
						{
							this.DoActionPacket(packetItem3.Type);
						}
						if (this.IsDropEx(packetItem3) || Game.JunkItemName.Contains(packetItem3.Name) || Game.JunkItemType.Contains(packetItem3.TypeName))
						{
							this.PostMessage(packetItem3.Index, 108);
						}
					}
				}
			}
			if (Game.TickCount % 60 == 0 && this.SafeTime > 30 && this.IsDropItem)
			{
				foreach (PacketItem packetItem4 in PacketItem.Enum(this))
				{
					if (this.IsCanDelete(packetItem4) && Game.IsDrop(packetItem4) && !packetItem4.IsHaveLongVan && !packetItem4.IsHaveNgoc)
					{
						this.PostMessage(packetItem4.Index, 108);
						FrmMain.AddLog(string.Concat(new string[]
						{
							DateTime.Now.ToString("dd-MM | HH:mm"),
							" [",
							this.LastName,
							"] hủy vật phẩm :",
							packetItem4.Name,
							"\n"
						}));
						this.PushDebugMessage("Auto vừa hủy vật phẩm :" + packetItem4.Name);
					}
					if (TINHKIEM.VietLien(packetItem4.Name) == "tuikimngocphuquy" || TINHKIEM.VietLien(packetItem4.Name) == "tuitieuphuc" || TINHKIEM.VietLien(packetItem4.Name) == "tuidaiphuc" || TINHKIEM.VietLien(packetItem4.Name) == "tieulucdan" || TINHKIEM.VietLien(packetItem4.Name) == "dialucdan" || TINHKIEM.VietLien(packetItem4.Name) == "thienlucdan")
					{
						this.LuaDoOneLineString("PlayerPackage:UseItem(" + packetItem4.Index.ToString() + ");");
						break;
					}
					if (TINHKIEM.VietLien(packetItem4.Name) == "channguyenphach" && packetItem4.IsCoDinh)
					{
						this.LuaDoOneLineString("PlayerPackage:UseItem(" + packetItem4.Index.ToString() + ");");
						break;
					}
				}
			}
		}

		// Token: 0x06000878 RID: 2168 RVA: 0x00038914 File Offset: 0x00036B14
		public bool IsCanDelete(PacketItem Item)
		{
			bool result = true;
			if (Array.IndexOf<string>(new string[]
			{
				"Mão",
				"Y phục",
				"Hộ thủ",
				"Hài",
				"Yêu đái",
				"Giới chỉ",
				"Hạng liên",
				"Võ Hồn",
				"Hộ phù",
				"Hộ uyển",
				"Hộ kiên",
				"Ám Khí",
				"Đao búa",
				"Thương tần",
				"Đơn đoản",
				"Song đoản",
				"Phiến",
				"Khuyên"
			}, Item.TypeName) > -1)
			{
				result = !(Item.DiemType == "0000000000000000000000000000000000000000000000000000000000000000");
			}
			return result;
		}

		// Token: 0x170001D6 RID: 470
		// (get) Token: 0x06000879 RID: 2169 RVA: 0x000389EA File Offset: 0x00036BEA
		// (set) Token: 0x0600087A RID: 2170 RVA: 0x000389F2 File Offset: 0x00036BF2
		public bool IsHold { get; set; }

		// Token: 0x0600087B RID: 2171 RVA: 0x000389FB File Offset: 0x00036BFB
		public void PickObject(GameObject _object)
		{
			this.PostMessage(_object.Object, 118);
		}

		// Token: 0x170001D7 RID: 471
		// (get) Token: 0x0600087C RID: 2172 RVA: 0x00038A0B File Offset: 0x00036C0B
		// (set) Token: 0x0600087D RID: 2173 RVA: 0x00038A13 File Offset: 0x00036C13
		public bool IsSale { get; set; }

		// Token: 0x170001D8 RID: 472
		// (get) Token: 0x0600087E RID: 2174 RVA: 0x00038A1C File Offset: 0x00036C1C
		// (set) Token: 0x0600087F RID: 2175 RVA: 0x00038A24 File Offset: 0x00036C24
		public bool IsThienKiepLau { get; set; }

		// Token: 0x06000880 RID: 2176 RVA: 0x00038A2D File Offset: 0x00036C2D
		public void ThienKiepLau()
		{
			if (!this.IsThienKiepLau)
			{
				return;
			}
			if (!TINHKIEM.VietLien(this.TLBB.MapName).Contains("thienkieplau") && this.GoTo(DAILY.PhoKiepSinh))
			{
				bool isQuestOpen = this.TLBB.IsQuestOpen;
			}
		}

		// Token: 0x06000881 RID: 2177 RVA: 0x00038A70 File Offset: 0x00036C70
		public bool SellItem()
		{
			if (!this.IsSellItem)
			{
				return false;
			}
			if (Game.TickCount % 18 != 0)
			{
				return false;
			}
			NPC npc = NPC.VANDIEUDIEU;
			if (Unity.DangOMapVutRac(this.TLBB.MapId))
			{
				npc = Unity.GETNPCVUTRAC(this.TLBB.MapId);
			}
			else
			{
				npc = Unity.GETNPCVUTRAC(Option.MapBanDoIndex);
			}
			if (this.GoTo((float)npc.X, (float)npc.Y, npc.Map, false))
			{
				if (!this.TLBB.IsShopOpen)
				{
					if (this.TLBB.IsQuestOpen)
					{
						if (npc.Map == 0)
						{
							this.QuestFrame.Click(101, 0);
						}
						if (npc.Map == 2)
						{
							this.QuestFrame.Click(2048, 11);
						}
					}
					using (List<GameObject>.Enumerator enumerator = this.Objects.AllNpc.GetEnumerator())
					{
						while (enumerator.MoveNext())
						{
							GameObject gameObject = enumerator.Current;
							if (gameObject.CleanName == "vandieudieu" || gameObject.CleanName == "tontuvu" || gameObject.CleanName == "truongthienthien" || gameObject.CleanName == "donghoakim")
							{
								this.Talk(gameObject.Id);
							}
						}
						goto IL_336;
					}
				}
				if (this.TLBB.SafeTime > 0)
				{
					return false;
				}
				if (Game.TickCount % 9 == 0)
				{
					if (this.IsSale)
					{
						this.IsSale = false;
						foreach (PacketItem packetItem in PacketItem.Enum(this))
						{
							if (this.IsSell(packetItem))
							{
								this.IsSale = true;
								break;
							}
						}
					}
					if (this.TLBB.IsShopOpen)
					{
						foreach (PacketItem packetItem2 in PacketItem.Enum(this))
						{
							if (this.IsCanDelete(packetItem2) && this.IsSell(packetItem2) && !packetItem2.IsHaveLongVan && !packetItem2.IsHaveNgoc)
							{
								this.SellItem(packetItem2.Address);
								FrmMain.AddLog(string.Concat(new string[]
								{
									DateTime.Now.ToString("dd-MM | HH:mm"),
									" [",
									this.LastName,
									"] bán vật phẩm :",
									packetItem2.Name,
									"\n"
								}));
								this.PushDebugMessage("Auto vừa bán bán vật phẩm :" + packetItem2.Name);
								Thread.Sleep(150);
							}
						}
						this.IsSellItem = false;
						this.PushThongBao(this.TLBB.Name, "Đã bán xong vật phẩm\nKiểm tra danh sách tại Logs", CanhBao.Kieu.Info);
						return false;
					}
					if ((this.TLBB.IsODaoCuFull || this.TLBB.IsONguyenLieuFull) && this.IsNhatTuyet)
					{
						this.IsSale = true;
					}
					if (this.IsSale)
					{
						if (this.SaveX == 0f && this.SaveY == 0f)
						{
							this.SaveX = this.CharX;
							this.SaveY = this.CharY;
							this.IsGoToShop = true;
						}
						return true;
					}
				}
			}
			IL_336:
			return this.IsSale;
		}

		// Token: 0x170001D9 RID: 473
		// (get) Token: 0x06000882 RID: 2178 RVA: 0x00038DE4 File Offset: 0x00036FE4
		// (set) Token: 0x06000883 RID: 2179 RVA: 0x00038DEC File Offset: 0x00036FEC
		public float SaveX { get; set; }

		// Token: 0x170001DA RID: 474
		// (get) Token: 0x06000884 RID: 2180 RVA: 0x00038DF5 File Offset: 0x00036FF5
		// (set) Token: 0x06000885 RID: 2181 RVA: 0x00038DFD File Offset: 0x00036FFD
		public float SaveY { get; set; }

		// Token: 0x06000886 RID: 2182 RVA: 0x00038E06 File Offset: 0x00037006
		public void DropItem(int index)
		{
			this.PostMessage(index, 108);
		}

		// Token: 0x06000887 RID: 2183 RVA: 0x00038E14 File Offset: 0x00037014
		public static bool IsDrop(PacketItem packetItem)
		{
			if (packetItem.Name.Trim() == "" || packetItem.TypeName == "")
			{
				return false;
			}
			string[] array = Setting.DropName.Split(new char[]
			{
				'\n'
			});
			for (int i = 0; i < array.Length; i++)
			{
				if (TINHKIEM.VietLien(array[i]) == TINHKIEM.VietLien(packetItem.Name) && packetItem.Name.Trim() != "")
				{
					return true;
				}
			}
			array = Setting.DropType.Split(new char[]
			{
				'\n'
			});
			for (int j = 0; j < array.Length; j++)
			{
				if (TINHKIEM.VietLien(array[j]) == TINHKIEM.VietLien(packetItem.TypeName) && packetItem.TypeName.Trim() != "")
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06000888 RID: 2184 RVA: 0x00038EFC File Offset: 0x000370FC
		public bool IsSell(PacketItem packetItem)
		{
			if (packetItem.Name.Trim() == "" || packetItem.TypeName == "")
			{
				return false;
			}
			string[] array = Setting.SellName.Split(new char[]
			{
				'\n'
			});
			for (int i = 0; i < array.Length; i++)
			{
				if (TINHKIEM.VietLien(array[i]) == TINHKIEM.VietLien(packetItem.Name) && packetItem.Name.Trim() != "")
				{
					return true;
				}
			}
			array = Setting.SellType.Split(new char[]
			{
				'\n'
			});
			for (int j = 0; j < array.Length; j++)
			{
				if (TINHKIEM.VietLien(array[j]) == TINHKIEM.VietLien(packetItem.TypeName) && packetItem.TypeName.Trim() != "")
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06000889 RID: 2185 RVA: 0x00038FE4 File Offset: 0x000371E4
		public void MoiThemDoi()
		{
			this.Objects.Read();
			if (this.Objects.Self != null && this.Objects.Self.PartyId != -1)
			{
				return;
			}
			foreach (GameObject gameObject in this.Objects.All)
			{
				if (FrmMain.AllCurGameTrueID.Contains(gameObject.TrueId) && gameObject.TrueId != this.TLBB.Id && gameObject.PartyId != -1)
				{
					this.SelectTarget(gameObject.Id);
					this.PostMessage(11, 105);
					return;
				}
			}
			foreach (GameObject gameObject2 in this.Objects.All)
			{
				if (gameObject2.PartyId != -1 && gameObject2.PartyId != 0 && gameObject2.Menpai >= 0 && gameObject2.Menpai <= 9 && gameObject2.Id != 0)
				{
					this.SelectTarget(gameObject2.Id);
					this.PostMessage(11, 105);
					break;
				}
			}
		}

		// Token: 0x0600088A RID: 2186 RVA: 0x00039134 File Offset: 0x00037334
		public void LoadSkill()
		{
			if (this.TimeOnMap < 40)
			{
				return;
			}
			if (!this.TLBB.Online)
			{
				return;
			}
			if (this.Skills.Count == 0)
			{
				this.Skills = Skill.Enum(this);
				if (this.Skills.Count > 4)
				{
					string text = Setting.LoadStringOffline(this.TLBB.Id + "SKILL");
					string text2 = Setting.LoadStringOffline(this.TLBB.Id + "SKILLPK");
					string text3 = Setting.LoadStringOffline(this.TLBB.Id + "SKILLBUFF");
					this.NMSKill = null;
					this.BaseSkill = 0;
					this.Controls = GameControl.Enum(this);
					foreach (Skill skill in this.Skills)
					{
						if (Skill.IsBase(skill.PacketId))
						{
							this.BaseSkill = skill.PacketId;
						}
						if (skill.PacketId == 424)
						{
							this.NMSKill = skill;
						}
						foreach (GameControl gameControl in this.Controls)
						{
							if (gameControl.IsSkill && gameControl.PacketId == skill.PacketId)
							{
								skill.Name = gameControl.Name;
								break;
							}
						}
						if (text.Contains("-" + skill.PacketId.ToString() + "-"))
						{
							skill.Use = true;
						}
						if (text2.Contains("-" + skill.PacketId.ToString() + "-"))
						{
							skill.UsePK = true;
						}
						if (text3.Contains("-" + skill.PacketId.ToString() + "-"))
						{
							skill.UserBuff = true;
						}
					}
					if (this.NMSKill == null)
					{
						foreach (Skill skill2 in this.Skills)
						{
							if (skill2.PacketId == 407)
							{
								this.NMSKill = skill2;
							}
						}
					}
					if (this.SkillLoaded != null)
					{
						this.SkillLoaded(this, null);
						return;
					}
				}
				else
				{
					this.Skills.Clear();
				}
			}
		}

		// Token: 0x0600088B RID: 2187 RVA: 0x000393F0 File Offset: 0x000375F0
		private void FollowKey()
		{
			if (this.IsMapPhuBan() && Global.IsAcTac)
			{
				return;
			}
			if (this.TLBB.PlayerState == 2)
			{
				return;
			}
			if (this.TLBB.IsFollow)
			{
				return;
			}
			if (Global.FollowKey && this.Objects.Key != null && TINHKIEM.GetDistance(this.CharX, this.CharY, this.Objects.Key.X, this.Objects.Key.Y) >= (float)Global.FollowRadius)
			{
				this.Move(this.Objects.Key.X, this.Objects.Key.Y);
			}
		}

		// Token: 0x0600088C RID: 2188 RVA: 0x0003949E File Offset: 0x0003769E
		public void UnlockPass2()
		{
			this.LuaDoOneLineString("UnLockMinorPassword(\"" + this.Pass2 + "\");");
		}

		// Token: 0x0600088D RID: 2189 RVA: 0x000394BB File Offset: 0x000376BB
		public void SavePass2()
		{
			Setting.SaveSettingOffline("PASS2" + this.TLBB.Id, this.Pass2);
		}

		// Token: 0x0600088E RID: 2190 RVA: 0x000394DD File Offset: 0x000376DD
		public void SetWay(string way)
		{
			this.LuaDoOneLineString("WAY = \"" + way + "\"; SetWay()");
		}

		// Token: 0x170001DB RID: 475
		// (get) Token: 0x0600088F RID: 2191 RVA: 0x000394F5 File Offset: 0x000376F5
		// (set) Token: 0x06000890 RID: 2192 RVA: 0x000394FD File Offset: 0x000376FD
		public int RecvData { get; set; }

		// Token: 0x170001DC RID: 476
		// (get) Token: 0x06000891 RID: 2193 RVA: 0x00039506 File Offset: 0x00037706
		// (set) Token: 0x06000892 RID: 2194 RVA: 0x0003950E File Offset: 0x0003770E
		public int RecvAddress { get; set; }

		// Token: 0x170001DD RID: 477
		// (get) Token: 0x06000893 RID: 2195 RVA: 0x00039517 File Offset: 0x00037717
		// (set) Token: 0x06000894 RID: 2196 RVA: 0x0003951F File Offset: 0x0003771F
		public int MyRecvAddress { get; set; }

		// Token: 0x170001DE RID: 478
		// (get) Token: 0x06000895 RID: 2197 RVA: 0x00039528 File Offset: 0x00037728
		// (set) Token: 0x06000896 RID: 2198 RVA: 0x00039530 File Offset: 0x00037730
		public int KetQuaSetTitle { get; set; }

		// Token: 0x170001DF RID: 479
		// (get) Token: 0x06000897 RID: 2199 RVA: 0x00039539 File Offset: 0x00037739
		// (set) Token: 0x06000898 RID: 2200 RVA: 0x00039541 File Offset: 0x00037741
		public string PetId { get; set; }

		// Token: 0x170001E0 RID: 480
		// (get) Token: 0x06000899 RID: 2201 RVA: 0x0003954A File Offset: 0x0003774A
		// (set) Token: 0x0600089A RID: 2202 RVA: 0x00039552 File Offset: 0x00037752
		public int RecvSize { get; set; }

		// Token: 0x170001E1 RID: 481
		// (get) Token: 0x0600089B RID: 2203 RVA: 0x0003955B File Offset: 0x0003775B
		// (set) Token: 0x0600089C RID: 2204 RVA: 0x00039563 File Offset: 0x00037763
		public string RecvDat { get; set; }

		// Token: 0x0600089D RID: 2205 RVA: 0x0003956C File Offset: 0x0003776C
		public string ReadRecvData()
		{
			int num = this.Memory.Read(this.RecvData);
			if (num == 0)
			{
				return "";
			}
			string text = "";
			bool flag = false;
			byte[] array = new byte[100];
			Memory.ReadProcessMemory(this.Memory.Id, num, array, 100, 0);
			for (int i = 0; i < 100; i++)
			{
				string text2 = array[i].ToString("X2");
				if (text2 != "00")
				{
					flag = true;
				}
				text += text2;
			}
			if (flag)
			{
				this.RecvDat = text;
			}
			return text;
		}

		// Token: 0x0600089E RID: 2206 RVA: 0x00039604 File Offset: 0x00037804
		public string GetTrieuTap(string hex)
		{
			string text = "";
			if (!hex.Contains("746965756461747461692067") || !hex.StartsWith("DA03"))
			{
				return "";
			}
			string text2 = Regex.Replace(hex, ".*D569205B", "");
			text2 = Regex.Replace(text2, "5D.*", "");
			for (int i = 0; i < text2.Length / 2; i++)
			{
				try
				{
					text += ((char)short.Parse(text2.Substring(i * 2, 2), NumberStyles.AllowHexSpecifier)).ToString();
				}
				catch
				{
					text += ".";
				}
			}
			return text;
		}

		// Token: 0x170001E2 RID: 482
		// (get) Token: 0x0600089F RID: 2207 RVA: 0x000396B4 File Offset: 0x000378B4
		// (set) Token: 0x060008A0 RID: 2208 RVA: 0x00039737 File Offset: 0x00037937
		public int MapAcTac
		{
			get
			{
				if (this.mapAcTac == 0)
				{
					return 0;
				}
				if (this.TLBB.MapId == MAP.ThaiHo || this.TLBB.MapId == MAP.KiemCac || this.TLBB.MapId == MAP.KinhHo || this.TLBB.MapId == MAP.TungSon || this.TLBB.MapId == MAP.DonHoang)
				{
					return this.TLBB.MapId;
				}
				return this.mapAcTac;
			}
			set
			{
				this.mapAcTac = value;
			}
		}

		// Token: 0x170001E3 RID: 483
		// (get) Token: 0x060008A1 RID: 2209 RVA: 0x00039740 File Offset: 0x00037940
		// (set) Token: 0x060008A2 RID: 2210 RVA: 0x0003979F File Offset: 0x0003799F
		public int MapTKC
		{
			get
			{
				if (this.mapTKC == 0)
				{
					return 0;
				}
				if (this.TLBB.MapId == MAP.TayHo || this.TLBB.MapId == MAP.NhiHai || this.TLBB.MapId == MAP.NhanNam)
				{
					return this.TLBB.MapId;
				}
				return this.mapTKC;
			}
			set
			{
				this.mapTKC = value;
			}
		}

		// Token: 0x060008A3 RID: 2211 RVA: 0x000397A8 File Offset: 0x000379A8
		public void LoadSetting()
		{
			try
			{
				this.RaoTxt = Setting.LoadStringOffline("RAO" + this.TLBB.Id);
			}
			catch (Exception)
			{
			}
			this.ClearNhiemVu();
			this.LastId = this.TLBB.Id;
			this.LastName = this.TLBB.Name;
			this.BachHoaDuyenCompleted = false;
			this.TudongAn = JsonConvert.DeserializeObject<List<AutoEat>>(Setting.AutoEat);
			this.PetId = Setting.LoadStringOffline(this.TLBB.Id + "PET");
			if (this.TLBB.Online)
			{
				this.Init();
				if (!FrmMain.AllCurGameTrueID.Contains(this.TLBB.Id))
				{
					FrmMain.AllCurGameTrueID = FrmMain.AllCurGameTrueID + this.TLBB.Id + ",";
				}
			}
			try
			{
				this.ResetExpSpeed();
				this.KetQuaSetTitle = Win.SetWindowText(this.Handle, TINHKIEM.ClearSign(this.TLBB.Name) + " - game4you.us");
				int[] array = Setting.LoadSettingOffline(this.TLBB.Id);
				if (array == null || array.Length < 53)
				{
					this.IsAuto = true;
					for (int i = 0; i < 22; i++)
					{
						this.KeyDelay[i] = 1;
					}
					this.IsAttack = (this.IsPet = (this.IsHP = (this.IsMP = true)));
					EventHandler settingLoaded = this.SettingLoaded;
					if (settingLoaded != null)
					{
						settingLoaded(this, null);
					}
				}
				else
				{
					this.IsAuto = (array[0] == 1);
					this.IsAttack = (array[1] == 1);
					this.IsLure = (array[2] == 1);
					for (int j = 0; j < 12; j++)
					{
						this.F[j] = (array[j + 3] == 1);
					}
					this.IsPet = (array[15] == 1);
					this.IsHP = (array[16] == 1);
					this.IsMP = (array[17] == 1);
					this.IsRadius = (array[18] == 1);
					this.IsNM = (array[19] == 1);
					for (int k = 0; k < 10; k++)
					{
						this.Alt[k] = (array[20 + k] == 1);
					}
					for (int l = 0; l < 22; l++)
					{
						this.KeyDelay[l] = array[30 + l];
					}
					this.BuffPetPercent = array[52];
					if (array.Length > 53)
					{
						this.IsPickItem = (array[53] == 1);
					}
					if (array.Length > 59)
					{
						this.CheLoai = array[54];
						this.CheCap = array[55];
						this.CheNoiNgoai = array[56];
						this.CheSao = array[57];
						this.CheDong = array[58];
						this.CheDiem = array[59];
					}
					if (array.Length > 60)
					{
						this.HuyetTe = (array[60] == 1);
						this.HuyetTeValue = array[61];
						this.CongSinh = (array[62] == 1);
						this.CongSinhValue = array[63];
						this.AutoEatVatPham = (array[64] == 1);
						this.AutoThuPet = (array[65] == 1);
						this.TuAnX2 = (array[66] == 1);
						this.UsingTholinhChau = (array[67] == 1);
						this.SoLuongMua = array[68];
						this.SoLuongChe = array[69];
					}
					if (array.Length > 70)
					{
						this.ChatGan = (array[70] == 1);
						this.ChatTheGioi = (array[71] == 1);
						this.ChatThanhThi = (array[72] == 1);
						this.ChatDongMinh = (array[73] == 1);
						this.ChatMonPhai = (array[74] == 1);
						this.ChatBangPhai = (array[75] == 1);
						this.ChatDoi = (array[76] == 1);
						this.AutoHoiSinh = (array[77] == 1);
					}
					EventHandler settingLoaded2 = this.SettingLoaded;
					if (settingLoaded2 != null)
					{
						settingLoaded2(this, null);
					}
				}
			}
			catch (Exception ex)
			{
				Console.Write(ex.ToString());
			}
		}

		// Token: 0x060008A4 RID: 2212 RVA: 0x00039B9C File Offset: 0x00037D9C
		public void SaveSetting()
		{
			Setting.SaveSettingOffline("RAO" + this.TLBB.Id, this.RaoTxt);
			string text = string.Concat(new object[]
			{
				TINHKIEM.Bool2Int(this.IsAuto),
				",",
				TINHKIEM.Bool2Int(this.IsAttack),
				",",
				TINHKIEM.Bool2Int(this.IsLure),
				","
			});
			for (int i = 0; i < 12; i++)
			{
				text = text + TINHKIEM.Bool2Int(this.F[i]).ToString() + ",";
			}
			text = string.Concat(new object[]
			{
				text,
				TINHKIEM.Bool2Int(this.IsPet),
				",",
				TINHKIEM.Bool2Int(this.IsHP),
				",",
				TINHKIEM.Bool2Int(this.IsMP),
				",",
				TINHKIEM.Bool2Int(this.IsRadius),
				",",
				TINHKIEM.Bool2Int(this.IsNM),
				","
			});
			for (int j = 0; j < 10; j++)
			{
				text = text + TINHKIEM.Bool2Int(this.Alt[j]).ToString() + ",";
			}
			for (int k = 0; k < 22; k++)
			{
				text = text + this.KeyDelay[k].ToString() + ",";
			}
			text = string.Concat(new object[]
			{
				text,
				this.BuffPetPercent,
				",",
				TINHKIEM.Bool2Int(this.IsPickItem),
				","
			});
			text = string.Concat(new object[]
			{
				text,
				this.CheLoai,
				",",
				this.CheCap,
				",",
				this.CheNoiNgoai,
				",",
				this.CheSao,
				",",
				this.CheDong,
				",",
				this.CheDiem,
				",",
				TINHKIEM.Bool2Int(this.HuyetTe),
				",",
				this.HuyetTeValue,
				",",
				TINHKIEM.Bool2Int(this.CongSinh),
				",",
				this.CongSinhValue,
				",",
				TINHKIEM.Bool2Int(this.AutoEatVatPham),
				",",
				TINHKIEM.Bool2Int(this.AutoThuPet),
				",",
				TINHKIEM.Bool2Int(this.TuAnX2),
				",",
				TINHKIEM.Bool2Int(this.UsingTholinhChau),
				",",
				this.SoLuongMua,
				",",
				this.SoLuongChe,
				",",
				TINHKIEM.Bool2Int(this.ChatGan),
				",",
				TINHKIEM.Bool2Int(this.ChatTheGioi),
				",",
				TINHKIEM.Bool2Int(this.ChatThanhThi),
				",",
				TINHKIEM.Bool2Int(this.ChatDongMinh),
				",",
				TINHKIEM.Bool2Int(this.ChatMonPhai),
				",",
				TINHKIEM.Bool2Int(this.ChatBangPhai),
				",",
				TINHKIEM.Bool2Int(this.ChatDoi),
				",",
				TINHKIEM.Bool2Int(this.AutoHoiSinh),
				",",
				TINHKIEM.Bool2Int(this.AlarmChat)
			});
			Setting.SaveSettingOffline(this.TLBB.Id, text);
		}

		// Token: 0x060008A5 RID: 2213 RVA: 0x0003A040 File Offset: 0x00038240
		public void ResetExpSpeed()
		{
			if (this.TLBB.Lvl < 1 || this.TLBB.Lvl > 149)
			{
				return;
			}
			this.AutoTime = Stopwatch.StartNew();
			this.ExpStart = this.TLBB.Exp;
		}

		// Token: 0x060008A6 RID: 2214 RVA: 0x0003A080 File Offset: 0x00038280
		public void ResetetRadius()
		{
			if (this.IsRide || this.TLBB.IsFollow || !this.IsAuto || Global.Paused || !this.IsRadius || this.RadiusX == 0f)
			{
				this.RadiusX = this.CharX;
				this.RadiusY = this.CharY;
			}
		}

		// Token: 0x060008A7 RID: 2215 RVA: 0x0003A0E0 File Offset: 0x000382E0
		public string GetEnemy()
		{
			if (this.AddressEnemy == 0)
			{
				this.AddressEnemy = this.Memory.VirtualAllocEx(4096);
			}
			this.PostMessage(this.AddressEnemy, 109);
			return this.Memory.ReadString(this.Memory.Read(this.AddressEnemy));
		}

		// Token: 0x060008A8 RID: 2216 RVA: 0x0003A135 File Offset: 0x00038335
		public string LuaToString()
		{
			this.PostMessage(this.AddressToString, 109);
			return this.Memory.ReadString(this.Memory.Read(this.AddressToString));
		}

		// Token: 0x060008A9 RID: 2217 RVA: 0x0003A161 File Offset: 0x00038361
		public string LuaToStringBang()
		{
			this.PostMessage(this.AddressTenBang, 109);
			return this.Memory.ReadString(this.Memory.Read(this.AddressTenBang));
		}

		// Token: 0x170001E4 RID: 484
		// (get) Token: 0x060008AA RID: 2218 RVA: 0x0003A18D File Offset: 0x0003838D
		// (set) Token: 0x060008AB RID: 2219 RVA: 0x0003A195 File Offset: 0x00038395
		private int AddressCount { get; set; }

		// Token: 0x060008AC RID: 2220 RVA: 0x0003A1A0 File Offset: 0x000383A0
		public string DoCount()
		{
			if (this.AddressCount == 0)
			{
				this.AddressCount = this.Memory.VirtualAllocEx(4096);
			}
			this.PostMessage(this.AddressCount, 109);
			return this.Memory.ReadString(this.Memory.Read(this.AddressCount));
		}

		// Token: 0x060008AD RID: 2221 RVA: 0x0003A1F5 File Offset: 0x000383F5
		public string GetCount()
		{
			return this.Memory.ReadString(this.Memory.Read(this.AddressCount));
		}

		// Token: 0x060008AE RID: 2222 RVA: 0x0003A213 File Offset: 0x00038413
		public string LuaString()
		{
			return this.Memory.ReadString(this.Memory.Read(this.AddressToString));
		}

		// Token: 0x060008AF RID: 2223 RVA: 0x0003A231 File Offset: 0x00038431
		public string LuaStringBang()
		{
			return this.Memory.ReadString(this.Memory.Read(this.AddressTenBang));
		}

		// Token: 0x060008B0 RID: 2224 RVA: 0x0003A24F File Offset: 0x0003844F
		public string LuaStringEx()
		{
			return this.Memory.ReadStringEx(this.Memory.Read(this.AddressToString));
		}

		// Token: 0x060008B1 RID: 2225 RVA: 0x0003A26D File Offset: 0x0003846D
		public void EnterReconnect()
		{
			this.PostMessage(24, 105);
		}

		// Token: 0x060008B2 RID: 2226 RVA: 0x0003A279 File Offset: 0x00038479
		public void Pause()
		{
			if (this.IsHide)
			{
				PROCESS.Suspend(this.ProcessId);
				this.IsSuspend = true;
			}
		}

		// Token: 0x060008B3 RID: 2227 RVA: 0x0003A295 File Offset: 0x00038495
		public void Resume()
		{
			if (this.IsSuspend)
			{
				this.IsSuspend = false;
				PROCESS.Resume(this.ProcessId);
			}
		}

		// Token: 0x060008B4 RID: 2228 RVA: 0x00006740 File Offset: 0x00004940
		public void TKC()
		{
		}

		// Token: 0x170001E5 RID: 485
		// (get) Token: 0x060008B5 RID: 2229 RVA: 0x0003A2B1 File Offset: 0x000384B1
		// (set) Token: 0x060008B6 RID: 2230 RVA: 0x0003A2B9 File Offset: 0x000384B9
		private bool isAlarmTrungAc { get; set; }

		// Token: 0x170001E6 RID: 486
		// (get) Token: 0x060008B7 RID: 2231 RVA: 0x0003A2C2 File Offset: 0x000384C2
		// (set) Token: 0x060008B8 RID: 2232 RVA: 0x0003A2CA File Offset: 0x000384CA
		public Stopwatch HongTrungAcTime { get; set; }

		// Token: 0x170001E7 RID: 487
		// (get) Token: 0x060008B9 RID: 2233 RVA: 0x0003A2D3 File Offset: 0x000384D3
		// (set) Token: 0x060008BA RID: 2234 RVA: 0x0003A2DB File Offset: 0x000384DB
		private bool IsShowCap { get; set; }

		// Token: 0x170001E8 RID: 488
		// (get) Token: 0x060008BB RID: 2235 RVA: 0x0003A2E4 File Offset: 0x000384E4
		// (set) Token: 0x060008BC RID: 2236 RVA: 0x0003A2EC File Offset: 0x000384EC
		public Stopwatch swCaptchaTime { get; set; }

		// Token: 0x060008BD RID: 2237 RVA: 0x00006740 File Offset: 0x00004940
		public void PushAlarm(string msg)
		{
		}

		// Token: 0x170001E9 RID: 489
		// (get) Token: 0x060008BE RID: 2238 RVA: 0x0003A2F5 File Offset: 0x000384F5
		// (set) Token: 0x060008BF RID: 2239 RVA: 0x0003A2FD File Offset: 0x000384FD
		private bool isAlarmHuyetMo { get; set; }

		// Token: 0x170001EA RID: 490
		// (get) Token: 0x060008C0 RID: 2240 RVA: 0x0003A306 File Offset: 0x00038506
		// (set) Token: 0x060008C1 RID: 2241 RVA: 0x0003A30E File Offset: 0x0003850E
		private bool isAlarmDayTayNai { get; set; }

		// Token: 0x170001EB RID: 491
		// (get) Token: 0x060008C2 RID: 2242 RVA: 0x0003A318 File Offset: 0x00038518
		private bool IsDeadEx
		{
			get
			{
				return this.IsBachHoaDuyen || this.IsTrungAc || (this.TLBB.MapId != MAP.ThieuThatSon && this.TLBB.MapId != MAP.SinhTuLoiDai && this.TLBB.MapId != MAP.BinhThanhKyTran && this.AutoHoiSinh);
			}
		}

		// Token: 0x170001EC RID: 492
		// (get) Token: 0x060008C3 RID: 2243 RVA: 0x0003A375 File Offset: 0x00038575
		// (set) Token: 0x060008C4 RID: 2244 RVA: 0x0003A37D File Offset: 0x0003857D
		public bool IsPhuMau { get; set; }

		// Token: 0x060008C5 RID: 2245 RVA: 0x0003A388 File Offset: 0x00038588
		public void TheoDoiCanhBao()
		{
			this.VaoPhai();
			if (this.TLBB.MapId == MAP.HuyetMo && !this.isAlarmHuyetMo && this.IsMoBTD)
			{
				string str = "Đã vào huyệt mộ";
				this.LuaDoUnicodeString("TXT = '" + str + "#r#b#eda0000 A u t o Chính Thức Tình Kiếm#r#b#eda0000  h t tp ://game4you.u s ';");
				this.PostMessage(3, 105);
				this.isAlarmHuyetMo = true;
				this.PushAlarm("đã vào huyệt mộ");
			}
			if (this.ExitHPLow && this.TLBB.HPPercent < Global.ExitHPPercent && this.TLBB.HP > 0 && this.TLBB.MaxHP > 0 && this.TLBB.HP < this.TLBB.MaxHP && this.TLBB.Name != "ĐăngNhập")
			{
				this.Exit();
				return;
			}
			if (this.TLBB.IsPk && this.TLBB.MapId != 92)
			{
				if (Global.ExitPk)
				{
					this.Exit();
					return;
				}
				if (!this.isAlarmPK && Global.AlarmPk)
				{
					this.isAlarmPK = true;
					this.PushThongBao(this.TLBB.Name, "Đang bị PK", CanhBao.Kieu.Eror);
				}
			}
			else if (this.isAlarmPK)
			{
				this.isAlarmPK = false;
			}
			if (this.IsBachHoaDuyen)
			{
				if (this.TLBB.IsODaoCuFull)
				{
					if (!this.isAlarmDayTayNai)
					{
						this.isAlarmDayTayNai = true;
						this.PushThongBao(this.TLBB.Name, "Đầy Tay Nải", CanhBao.Kieu.Eror);
					}
				}
				else
				{
					this.isAlarmDayTayNai = false;
				}
			}
			if (!this.IsAlarmAcBa && this.AcBa != -1)
			{
				this.PushThongBao(this.TLBB.Name, "Ác Bá", CanhBao.Kieu.Eror);
				this.IsAlarmAcBa = true;
			}
			if (this.MoveCount > 40)
			{
				if (!this.isAlarmKet && this.MapAcTac == 0)
				{
					this.PushThongBao(this.TLBB.Name, "Bị Kẹt", CanhBao.Kieu.Eror);
					this.isAlarmKet = true;
				}
			}
			else if (this.isAlarmKet)
			{
				this.isAlarmKet = false;
			}
			if (this.BachHoaDuyenCompleted)
			{
				if (!this.isAlarmBachHoaDuyen)
				{
					FrmMain.AddLog(DateTime.Now.ToString("HH:mm dd-MM") + " " + this.LastName + " xong BHD\n");
					this.PushThongBao(this.TLBB.Name, "Xong BHD", CanhBao.Kieu.Info);
					this.isAlarmBachHoaDuyen = true;
				}
			}
			else
			{
				this.isAlarmBachHoaDuyen = false;
			}
			if (this.QDuaCompleted)
			{
				if (!this.isAlarmDua)
				{
					FrmMain.AddLog(DateTime.Now.ToString("HH:mm dd-MM") + " " + this.LastName + " xong Dua\n");
					this.PushThongBao(this.TLBB.Name, "Xong Dua", CanhBao.Kieu.Info);
					this.isAlarmDua = true;
				}
			}
			else
			{
				this.isAlarmDua = false;
			}
			if (this.IsXongTrungAc)
			{
				if (!this.isAlarmTrungAc)
				{
					FrmMain.AddLog(DateTime.Now.ToString("HH:mm dd-MM") + " " + this.LastName + " xong Trừng Ác\n");
					this.PushThongBao(this.TLBB.Name, "Xong Trừng Ác", CanhBao.Kieu.Info);
					this.isAlarmTrungAc = true;
				}
			}
			else
			{
				this.isAlarmTrungAc = false;
			}
			if (this.comeTime != null && this.comeTime.Elapsed.TotalSeconds > 150.0)
			{
				this.IsHong = true;
			}
			if (this.IsHong && this.HongTrungAcTime == null)
			{
				this.HongTrungAcTime = Stopwatch.StartNew();
			}
			if (!this.IsHong)
			{
				this.HongTrungAcTime = null;
			}
			if (this.IsHong && this.HongTrungAcTime.Elapsed.TotalSeconds > 300.0)
			{
				this.LuaDoUnicodeString("local cnt = 0; while true do local name, content = DataPool:GetPlayerMission_Memo(cnt); if string.find(name, '#{CXDT_090304_01}') then DataPool:Mission_Abnegate_Popup(cnt,DataPool:GetPlayerMission_Memo(cnt)); end cnt = cnt + 1; if cnt == 20 then return end end");
				this.State = STATE.HuyQ;
				this.HongTrungAcTime = null;
				this.IsHong = false;
				this.comeTime = null;
				return;
			}
			if (this.TLBB.Disconnected)
			{
				this.PushThongBao(this.TLBB.Name, "Mất Kết Nối", CanhBao.Kieu.Eror);
				this.EnterReconnect();
			}
			else if (this.disconnectedTime != 0)
			{
				this.disconnectedTime = 0;
			}
			if (this.TLBB.IsCaptcha)
			{
				this.CaptchaTime = 0;
				if (!this.isAlarmCaptcha && this.TLBB.IsRead)
				{
				}
			}
			else
			{
				this.swCaptchaTime = null;
				this.IsShowCap = false;
				this.TLBB.Captcha = null;
				this.TLBB.BinEx = null;
				if (this.isAlarmCaptcha)
				{
					if (this.IsTrungAc && Global.HideBHD)
					{
						this.Hide();
					}
					this.isAlarmCaptcha = false;
				}
			}
			if (this.TLBB.PlayerState == 9)
			{
				if (this.Address.GameType == 1 && this.TLBB.IsRelive)
				{
					this.LUA.Relive();
				}
				else if (this.IsDeadEx)
				{
					if (!this.isAlarmDead)
					{
						if (Global.AutoComeBack && !MAP.IsPhuBan(this.TLBB.MapId))
						{
							this.DeadX = (int)this.CharX;
							this.DeadY = (int)this.CharY;
							this.DeadMap = this.TLBB.MapId;
							this.DeadFakeMap = this.TLBB.FakeMapId;
							this.IsDead = true;
						}
						else
						{
							this.DeadX = (this.DeadY = 0);
						}
						this.isAlarmDead = true;
					}
					this.LUA.OutGhost();
				}
				else if (!this.isAlarmDead)
				{
					if (Global.AutoComeBack && !MAP.IsPhuBan(this.TLBB.MapId))
					{
						this.DeadX = (int)this.CharX;
						this.DeadY = (int)this.CharY;
						this.DeadMap = this.TLBB.MapId;
						this.DeadFakeMap = this.TLBB.FakeMapId;
						this.IsDead = true;
					}
					else
					{
						this.DeadX = (this.DeadY = 0);
					}
					this.PostMessage(0, 90);
					this.isAlarmDead = true;
					if (!Global.AutoComeBack)
					{
						this.PushThongBao(this.TLBB.Name, "Đã Tử Vong", CanhBao.Kieu.Eror);
					}
					else
					{
						this.PushThongBao(this.TLBB.Name, "Đã Tử Vong", CanhBao.Kieu.Eror);
					}
				}
			}
			else if (this.isAlarmDead)
			{
				this.isAlarmDead = false;
			}
			if (Global.AlarmHP && this.TLBB.HPPercent < Global.AlarmHPPercent && this.TLBB.Online && this.TLBB.HP > 0 && this.TLBB.MaxHP > 0 && this.TLBB.HP < this.TLBB.MaxHP)
			{
				if (!this.isAlarmHP)
				{
					this.isAlarmHP = true;
					this.PushThongBao(this.TLBB.Name, "Sắp Hết Máu", CanhBao.Kieu.Eror);
					return;
				}
			}
			else if (this.isAlarmHP)
			{
				this.isAlarmHP = false;
			}
		}

		// Token: 0x060008C6 RID: 2246 RVA: 0x0003AA3C File Offset: 0x00038C3C
		public void OpenShop()
		{
			this.LuaDoOneLineString("setmetatable(_G, {__index = Packet_Env }); Packet_Sale_Clicked(); IsMessageBox = 1;");
		}

		// Token: 0x060008C7 RID: 2247 RVA: 0x0003AA49 File Offset: 0x00038C49
		public void DragTo42()
		{
			this.LuaDoOneLineString("setmetatable(_G, {__index = Packet_Env }); Packet_ItemBtnClicked(4,2); ");
		}

		// Token: 0x060008C8 RID: 2248 RVA: 0x0003AA56 File Offset: 0x00038C56
		public void OpenBag()
		{
			this.IsOpenBag = true;
		}

		// Token: 0x170001ED RID: 493
		// (get) Token: 0x060008C9 RID: 2249 RVA: 0x0003AA60 File Offset: 0x00038C60
		public List<Game> Party
		{
			get
			{
				List<Game> list = new List<Game>();
				foreach (KeyValuePair<int, Game> keyValuePair in FrmMain.dicGame)
				{
					Game value = keyValuePair.Value;
					if (value == this)
					{
						list.Add(value);
					}
					if (value.TLBB.KeyId == this.TLBB.KeyId)
					{
						list.Add(value);
					}
				}
				return list;
			}
		}

		// Token: 0x170001EE RID: 494
		// (get) Token: 0x060008CA RID: 2250 RVA: 0x0003AAEC File Offset: 0x00038CEC
		public Game Leader
		{
			get
			{
				foreach (Game game in this.Party)
				{
					if (game.TLBB.IsLeader)
					{
						return game;
					}
				}
				return null;
			}
		}

		// Token: 0x170001EF RID: 495
		// (get) Token: 0x060008CB RID: 2251 RVA: 0x0003AB4C File Offset: 0x00038D4C
		// (set) Token: 0x060008CC RID: 2252 RVA: 0x0003AB54 File Offset: 0x00038D54
		public int DaChe { get; set; }

		// Token: 0x170001F0 RID: 496
		// (get) Token: 0x060008CD RID: 2253 RVA: 0x0003AB5D File Offset: 0x00038D5D
		// (set) Token: 0x060008CE RID: 2254 RVA: 0x0003AB65 File Offset: 0x00038D65
		public int DaHuy { get; set; }

		// Token: 0x170001F1 RID: 497
		// (get) Token: 0x060008CF RID: 2255 RVA: 0x0003AB6E File Offset: 0x00038D6E
		// (set) Token: 0x060008D0 RID: 2256 RVA: 0x0003AB76 File Offset: 0x00038D76
		public int TempCount { get; set; }

		// Token: 0x170001F2 RID: 498
		// (get) Token: 0x060008D1 RID: 2257 RVA: 0x0003AB7F File Offset: 0x00038D7F
		// (set) Token: 0x060008D2 RID: 2258 RVA: 0x0003AB87 File Offset: 0x00038D87
		public bool Kiemtranguyenlieu { get; set; }

		// Token: 0x170001F3 RID: 499
		// (get) Token: 0x060008D3 RID: 2259 RVA: 0x0003AB90 File Offset: 0x00038D90
		// (set) Token: 0x060008D4 RID: 2260 RVA: 0x0003AB98 File Offset: 0x00038D98
		public string TaskSauCheDO { get; set; }

		// Token: 0x170001F4 RID: 500
		// (get) Token: 0x060008D5 RID: 2261 RVA: 0x0003ABA1 File Offset: 0x00038DA1
		// (set) Token: 0x060008D6 RID: 2262 RVA: 0x0003ABA8 File Offset: 0x00038DA8
		public static bool Is69DO { get; set; }

		// Token: 0x060008D7 RID: 2263 RVA: 0x0003ABB0 File Offset: 0x00038DB0
		public VatLieu getsoluong(string loai, int cap)
		{
			VatLieu vatLieu = new VatLieu();
			string str = "";
			uint num = <PrivateImplementationDetails>.ComputeStringHash(loai);
			if (num <= 2331254944U)
			{
				if (num <= 1620852447U)
				{
					if (num <= 787157194U)
					{
						if (num != 687736765U)
						{
							if (num == 787157194U)
							{
								if (loai == "Mão")
								{
									str = "Mão tử đả tạo độ";
								}
							}
						}
						else if (loai == "Phiến")
						{
							str = "Phiến đả tạo độ";
						}
					}
					else if (num != 853619373U)
					{
						if (num == 1620852447U)
						{
							if (loai == "Hộ thủ")
							{
								str = "Hộ thủ đả tạo độ";
							}
						}
					}
					else if (loai == "Hộ phù")
					{
						str = "Hộ phù đả tạo độ";
					}
				}
				else if (num <= 2170363184U)
				{
					if (num != 1829691604U)
					{
						if (num == 2170363184U)
						{
							if (loai == "Y phục")
							{
								str = "Y phục đả tạo độ";
							}
						}
					}
					else if (loai == "Hài")
					{
						str = "Hài đả tạo độ";
					}
				}
				else if (num != 2187651058U)
				{
					if (num == 2331254944U)
					{
						if (loai == "Khuyên")
						{
							str = "Hoàn đả tạo độ";
						}
					}
				}
				else if (loai == "Hộ kiên")
				{
					str = "Hộ kiên đả tạo độ";
				}
			}
			else if (num <= 3596317964U)
			{
				if (num <= 2778927056U)
				{
					if (num != 2679427693U)
					{
						if (num == 2778927056U)
						{
							if (loai == "Yêu đái")
							{
								str = "Yêu đái đả tạo độ";
							}
						}
					}
					else if (loai == "Song đoản")
					{
						str = "Song đoản đả tạo đồ";
					}
				}
				else if (num != 2782344019U)
				{
					if (num == 3596317964U)
					{
						if (loai == "Đao búa")
						{
							str = "Đao Phủ Đả Tạo Đồ";
						}
					}
				}
				else if (loai == "Giới chỉ")
				{
					str = "Giới chỉ đả tạo độ";
				}
			}
			else if (num <= 4132984349U)
			{
				if (num != 3620932460U)
				{
					if (num == 4132984349U)
					{
						if (loai == "Hộ uyển")
						{
							str = "Hộ uyển đả tạo độ";
						}
					}
				}
				else if (loai == "Thương tần")
				{
					str = "Thương Bổng Đả Tạo Đồ";
				}
			}
			else if (num != 4204925877U)
			{
				if (num == 4236618236U)
				{
					if (loai == "Hạng liên")
					{
						str = "Hạng liên đả tạo độ";
					}
				}
			}
			else if (loai == "Đơn đoản")
			{
				str = "Đơn Đoản đả tạo đồ";
			}
			using (List<PacketItem>.Enumerator enumerator = PacketItem.Enum(this).GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (TINHKIEM.VietLien(enumerator.Current.Name).Contains("tinhthiet"))
					{
						vatLieu.TinhThiet += enumerator.Current.Count;
					}
					if (TINHKIEM.VietLien(enumerator.Current.Name).Contains("vaibong"))
					{
						vatLieu.VaiBong += enumerator.Current.Count;
					}
					if (TINHKIEM.VietLien(enumerator.Current.Name).Contains("bingan"))
					{
						vatLieu.BiNgan += enumerator.Current.Count;
					}
					if (TINHKIEM.VietLien(enumerator.Current.Name).Contains(TINHKIEM.VietLien(str)) && enumerator.Current.GetNumberFromString == cap)
					{
						vatLieu.DaTaoDo += enumerator.Current.Count;
					}
				}
			}
			vatLieu.DaChe = this.DaChe;
			vatLieu.DaHuy = this.DaHuy;
			return vatLieu;
		}

		// Token: 0x170001F5 RID: 501
		// (get) Token: 0x060008D8 RID: 2264 RVA: 0x0003AFE0 File Offset: 0x000391E0
		// (set) Token: 0x060008D9 RID: 2265 RVA: 0x0003AFE8 File Offset: 0x000391E8
		public List<int> TmpItemBeforeChe { get; set; }

		// Token: 0x170001F6 RID: 502
		// (get) Token: 0x060008DA RID: 2266 RVA: 0x0003AFF1 File Offset: 0x000391F1
		// (set) Token: 0x060008DB RID: 2267 RVA: 0x0003AFF9 File Offset: 0x000391F9
		public int TocDoChe { get; set; }

		// Token: 0x060008DC RID: 2268 RVA: 0x0003B004 File Offset: 0x00039204
		public void MuaNguyenLieu(int loai)
		{
			if (loai == 1)
			{
				if (this.TLBB.MaxONguyenLieu - 3 < 0)
				{
					this.PushThongBao("Thông báo", "Thiếu ô nhận nguyên liệu vui lòng Sắp Xếp", CanhBao.Kieu.Eror);
					this.IsCheDo = false;
					return;
				}
				if (this.TLBB.MapId != 2)
				{
					this.TimDuong(157f, 169f, 2);
					return;
				}
				if (TINHKIEM.GetDistance(this.CharX, this.CharY, 157f, 169f) > 1f)
				{
					this.Move(157f, 169f);
					return;
				}
				if (this.IsTalkTieuPhong)
				{
					this.IsTalkTieuPhong = false;
					this.QuestFrameOptionClicked(2084, 1004);
					return;
				}
				this.IsTalkTieuPhong = true;
				this.Talk(143);
				return;
			}
			else
			{
				if (!this.TLBB.IsToggleYuanbaoShop)
				{
					this.LUA.ToggleYuanbaoShop();
					return;
				}
				string text = this.HaveDTD();
				if (text != "")
				{
					this.MuaName = text;
				}
				string text2 = TINHKIEM.VietLien(this.CheTen).Replace("khuyen", "hoan");
				if (text2.Contains("daobua") || text2.Contains("thuongtan"))
				{
					foreach (Shop shop in Shop.Enum(this))
					{
						if (TINHKIEM.VietLien(shop.Name).Contains(this.MuaName))
						{
							this.LUA.YuanbaoShop(Game.shopIndex, 1);
							this.Buy(shop.Index);
							this.MuaCount--;
							return;
						}
					}
					if (Game.Is69DO)
					{
						this.LuaDoOneLineString("setmetatable(_G, {__index = YuanbaoShop_Env}); YuanbaoShop_ChangeTabIndex(1);");
						Thread.Sleep(1000);
						this.LuaDoOneLineString("setmetatable(_G, {__index = YuanbaoShop_Env}); YuanbaoShop_UpdateList_Bind(6); YuanbaoShop_UpdateShop_Bind(1);");
						return;
					}
					this.LUA.YuanbaoShop(Game.shopIndex, 1);
					return;
				}
				else if (text2.Contains("dondoan") || text2.Contains("songdoan"))
				{
					foreach (Shop shop2 in Shop.Enum(this))
					{
						if (TINHKIEM.VietLien(shop2.Name).Contains(this.MuaName))
						{
							this.LUA.YuanbaoShop(Game.shopIndex, 2);
							this.Buy(shop2.Index);
							this.MuaCount--;
							return;
						}
					}
					if (Game.Is69DO)
					{
						this.LuaDoOneLineString("setmetatable(_G, {__index = YuanbaoShop_Env}); YuanbaoShop_ChangeTabIndex(1);");
						Thread.Sleep(1000);
						this.LuaDoOneLineString("setmetatable(_G, {__index = YuanbaoShop_Env}); YuanbaoShop_UpdateList_Bind(6); YuanbaoShop_UpdateShop_Bind(2);");
						return;
					}
					this.LUA.YuanbaoShop(Game.shopIndex, 2);
					return;
				}
				else if (text2.Contains("phien") || text2.Contains("hoan"))
				{
					foreach (Shop shop3 in Shop.Enum(this))
					{
						if (TINHKIEM.VietLien(shop3.Name).Contains(this.MuaName))
						{
							this.LUA.YuanbaoShop(Game.shopIndex, 3);
							this.Buy(shop3.Index);
							this.MuaCount--;
							return;
						}
					}
					if (Game.Is69DO)
					{
						this.LuaDoOneLineString("setmetatable(_G, {__index = YuanbaoShop_Env}); YuanbaoShop_ChangeTabIndex(1);");
						Thread.Sleep(1000);
						this.LuaDoOneLineString("setmetatable(_G, {__index = YuanbaoShop_Env}); YuanbaoShop_UpdateList_Bind(6); YuanbaoShop_UpdateShop_Bind(3);");
						return;
					}
					this.LUA.YuanbaoShop(Game.shopIndex, 3);
					return;
				}
				else if (text2.Contains("mao") || text2.Contains("yphuc"))
				{
					foreach (Shop shop4 in Shop.Enum(this))
					{
						if (TINHKIEM.VietLien(shop4.Name).Contains(this.MuaName))
						{
							this.LUA.YuanbaoShop(Game.shopIndex, 4);
							this.Buy(shop4.Index);
							this.MuaCount--;
							return;
						}
					}
					if (Game.Is69DO)
					{
						this.LuaDoOneLineString("setmetatable(_G, {__index = YuanbaoShop_Env}); YuanbaoShop_ChangeTabIndex(1);");
						Thread.Sleep(1000);
						this.LuaDoOneLineString("setmetatable(_G, {__index = YuanbaoShop_Env}); YuanbaoShop_UpdateList_Bind(6); YuanbaoShop_UpdateShop_Bind(4);");
						return;
					}
					this.LUA.YuanbaoShop(Game.shopIndex, 4);
					return;
				}
				else if (text2.Contains("hothu") || text2.Contains("hai"))
				{
					foreach (Shop shop5 in Shop.Enum(this))
					{
						if (TINHKIEM.VietLien(shop5.Name).Contains(this.MuaName))
						{
							this.LUA.YuanbaoShop(Game.shopIndex, 5);
							this.Buy(shop5.Index);
							this.MuaCount--;
							return;
						}
					}
					if (Game.Is69DO)
					{
						this.LuaDoOneLineString("setmetatable(_G, {__index = YuanbaoShop_Env}); YuanbaoShop_ChangeTabIndex(1);");
						Thread.Sleep(1000);
						this.LuaDoOneLineString("setmetatable(_G, {__index = YuanbaoShop_Env}); YuanbaoShop_UpdateList_Bind(6); YuanbaoShop_UpdateShop_Bind(5);");
						return;
					}
					this.LUA.YuanbaoShop(Game.shopIndex, 5);
					return;
				}
				else if (text2.Contains("houyen") || text2.Contains("hokien"))
				{
					foreach (Shop shop6 in Shop.Enum(this))
					{
						if (TINHKIEM.VietLien(shop6.Name).Contains(this.MuaName))
						{
							this.LUA.YuanbaoShop(Game.shopIndex, 6);
							this.Buy(shop6.Index);
							this.MuaCount--;
							return;
						}
					}
					if (Game.Is69DO)
					{
						this.LuaDoOneLineString("setmetatable(_G, {__index = YuanbaoShop_Env}); YuanbaoShop_ChangeTabIndex(1);");
						Thread.Sleep(1000);
						this.LuaDoOneLineString("setmetatable(_G, {__index = YuanbaoShop_Env}); YuanbaoShop_UpdateList_Bind(6); YuanbaoShop_UpdateShop_Bind(6);");
						return;
					}
					this.LUA.YuanbaoShop(Game.shopIndex, 6);
					return;
				}
				else if (text2.Contains("yeudai") || text2.Contains("hanglien"))
				{
					foreach (Shop shop7 in Shop.Enum(this))
					{
						if (TINHKIEM.VietLien(shop7.Name).Contains(this.MuaName))
						{
							this.LUA.YuanbaoShop(Game.shopIndex, 7);
							this.Buy(shop7.Index);
							this.MuaCount--;
							return;
						}
					}
					if (Game.Is69DO)
					{
						this.LuaDoOneLineString("setmetatable(_G, {__index = YuanbaoShop_Env}); YuanbaoShop_ChangeTabIndex(1);");
						Thread.Sleep(1000);
						this.LuaDoOneLineString("setmetatable(_G, {__index = YuanbaoShop_Env}); YuanbaoShop_UpdateList_Bind(6); YuanbaoShop_UpdateShop_Bind(7);");
						return;
					}
					this.LUA.YuanbaoShop(Game.shopIndex, 7);
					return;
				}
				else if (text2.Contains("gioichi") || text2.Contains("hophu"))
				{
					foreach (Shop shop8 in Shop.Enum(this))
					{
						if (TINHKIEM.VietLien(shop8.Name).Contains(this.MuaName))
						{
							this.LUA.YuanbaoShop(Game.shopIndex, 8);
							this.Buy(shop8.Index);
							this.MuaCount--;
							return;
						}
					}
					if (Game.Is69DO)
					{
						this.LuaDoOneLineString("setmetatable(_G, {__index = YuanbaoShop_Env}); YuanbaoShop_ChangeTabIndex(1);");
						Thread.Sleep(1000);
						this.LuaDoOneLineString("setmetatable(_G, {__index = YuanbaoShop_Env}); YuanbaoShop_UpdateList_Bind(6); YuanbaoShop_UpdateShop_Bind(8);");
						return;
					}
					this.LUA.YuanbaoShop(Game.shopIndex, 8);
					return;
				}
				return;
			}
		}

		// Token: 0x060008DD RID: 2269 RVA: 0x0003B800 File Offset: 0x00039A00
		public void CheDoFree()
		{
			if (!this.IsCheDo)
			{
				return;
			}
			if (this.LenBaiTrain)
			{
				this.LenBaiTrain = false;
			}
			if (this.AutoTrain)
			{
				this.AutoTrain = false;
			}
			if (this.IsDead)
			{
				this.IsDead = false;
			}
			this.GiamDinh();
			if (Game.TickCount % this.TocDoChe != 0)
			{
				return;
			}
			if (this.Kiemtranguyenlieu)
			{
				VatLieu vatLieu = new VatLieu();
				vatLieu = this.getsoluong(this.CheTen, this.CheCap + 1);
				if (!this.IsMuaNguyenLieu)
				{
					if ((this.CheTen == "Đao búa" || this.CheTen == "Thương tần" || this.CheTen == "Đơn đoản" || this.CheTen == "Song đoản" || this.CheTen == "Phiến" || this.CheTen == "Khuyên") && vatLieu.TinhThiet < this.SoLuongMua)
					{
						this.MuaNguyenLieu(1);
						return;
					}
					if ((this.CheTen == "Mão" || this.CheTen == "Y phục" || this.CheTen == "Hộ thủ" || this.CheTen == "Hài" || this.CheTen == "Hộ uyển" || this.CheTen == "Hộ kiên" || this.CheTen == "Yêu đái") && vatLieu.VaiBong < this.SoLuongMua)
					{
						this.MuaNguyenLieu(1);
						return;
					}
					if ((this.CheTen == "Hạng liên" || this.CheTen == "Giới chỉ" || this.CheTen == "Hộ phù") && vatLieu.BiNgan < this.SoLuongMua)
					{
						this.MuaNguyenLieu(1);
						return;
					}
				}
				if (vatLieu.DaTaoDo < this.SoLuongMua)
				{
					this.MuaNguyenLieu(0);
					return;
				}
				if (this.TLBB.IsToggleYuanbaoShop)
				{
					this.LUA.ToggleYuanbaoShop();
					this.Move(this.CharX + 3f, this.CharY + 2f);
				}
				if (this.IsRide)
				{
					this.DownRide();
					return;
				}
				this.Kiemtranguyenlieu = false;
				Thread.Sleep(1000);
			}
			if (this.TLBB.IsODaoCuFull)
			{
				this.PushThongBao("Thông báo", "Thiếu ô chứa đồ chế vui lòng sắp xếp", CanhBao.Kieu.Eror);
				this.IsCheDo = false;
				return;
			}
			if (Game.TickCount % 99 != 0)
			{
				return;
			}
			if (this.TempCount >= this.SoLuongMua && this.DaChe < this.SoLuongChe)
			{
				this.TempCount = 0;
				this.Kiemtranguyenlieu = true;
				if (this.HuyNguyenLieu)
				{
					this.HuyNguyenLieuTHUA();
				}
				return;
			}
			this.IsDrop();
			if (this.DaChe < this.SoLuongChe)
			{
				this.Che();
			}
			if (this.DaChe == this.SoLuongChe)
			{
				if (!this.CheDO.IsRunning)
				{
					this.CheDO.Start();
					return;
				}
				if (this.CheDO.Elapsed.TotalSeconds > 6.0)
				{
					this.IsCheDo = false;
					this.PushThongBao("Thông báo", "Đã Chế Xong", CanhBao.Kieu.OK);
					if (this.TaskSauCheDO != "Ngồi Chơi")
					{
						if (this.TaskSauCheDO == "Tắt Máy")
						{
							Process.Start("shutdown", "-s -f -t 0");
						}
						if (this.TaskSauCheDO == "Đi Train")
						{
							this.AutoTrain = true;
						}
					}
					this.CheDO.Stop();
				}
			}
		}

		// Token: 0x060008DE RID: 2270 RVA: 0x0003BB98 File Offset: 0x00039D98
		public void HuyNguyenLieuTHUA()
		{
			foreach (PacketItem packetItem in this.Packet.NguyenLieu)
			{
				if (TINHKIEM.VietLien(packetItem.Name).Contains("tinhthiet") && packetItem.IsCoDinh)
				{
					this.DropItem(packetItem.Index);
				}
				if (TINHKIEM.VietLien(packetItem.Name).Contains("vaibong") && packetItem.IsCoDinh)
				{
					this.DropItem(packetItem.Index);
				}
				if (TINHKIEM.VietLien(packetItem.Name).Contains("bingan") && packetItem.IsCoDinh)
				{
					this.DropItem(packetItem.Index);
				}
			}
		}

		// Token: 0x060008DF RID: 2271 RVA: 0x0003BC70 File Offset: 0x00039E70
		public void GiamDinh()
		{
			if (Game.IsGiamDinh)
			{
				foreach (PacketItem packetItem in PacketItem.Enum(this))
				{
					packetItem.GiamDinh();
				}
			}
		}

		// Token: 0x060008E0 RID: 2272 RVA: 0x0003BCC8 File Offset: 0x00039EC8
		public void Che()
		{
			string hex = string.Concat(new string[]
			{
				"0C9C5F000000000000000000",
				this.IdLoai().ToString("X2"),
				"000000",
				this.IdCheCap().ToString("X2"),
				this.IdNoiNgoai().ToString("X2"),
				"0000FFFFFFFF",
				this.IdNguyenLieu()
			});
			this.SendPacket(hex);
			int num = this.DaChe;
			this.DaChe = num + 1;
			num = this.TempCount;
			this.TempCount = num + 1;
		}

		// Token: 0x060008E1 RID: 2273 RVA: 0x0003BD6C File Offset: 0x00039F6C
		public void OpenKNBShop()
		{
			string hex = "3C585F00000085150000000046900D000F4F70656E5975616E62616F53686F7000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000030000FFFFFFFF01000000010";
			this.SendPacket(hex);
		}

		// Token: 0x060008E2 RID: 2274 RVA: 0x0003BD88 File Offset: 0x00039F88
		public string DTDName()
		{
			string text = string.Empty;
			if (Game.Is69DO)
			{
				text = "Lv " + (this.CheCap + 1).ToString() + " ";
				string a = TINHKIEM.VietLien(this.CheTen);
				if (a == "daobua")
				{
					text += "Falchion Plans";
				}
				else if (a == "thuongtan")
				{
					text += "Spear Plans";
				}
				else if (a == "dondoan")
				{
					text += "One-Hand Plans";
				}
				else if (a == "songdoan")
				{
					text += "Two-Hand Plans";
				}
				else if (a == "phien")
				{
					text += "Fan Plans";
				}
				else if (a == "hoan")
				{
					text += "Circle Plans";
				}
				else if (a == "mao")
				{
					text += "Hat Pattern";
				}
				else if (a == "yphuc")
				{
					text += "Garment Plans";
				}
				else if (a == "hothu")
				{
					text += "Glove Pattern";
				}
				else if (a == "hai")
				{
					text += "Shoe Pattern";
				}
				else if (a == "houyen")
				{
					text += "Wristband Plans";
				}
				else if (a == "hokien")
				{
					text += "Shp. Plans";
				}
				else if (a == "yeudai")
				{
					text += "Belt Pattern";
				}
				else if (a == "hanglien")
				{
					text += "Necklace Plans";
				}
				else if (a == "gioichi")
				{
					text += "Ring Design";
				}
				else if (a == "hophu")
				{
					text += "Amulet Design";
				}
			}
			return TINHKIEM.VietLien(text);
		}

		// Token: 0x060008E3 RID: 2275 RVA: 0x0003BFA8 File Offset: 0x0003A1A8
		public string HaveDTD()
		{
			string str = string.Empty;
			if (TINHKIEM.VietLien(this.CheTen) == "daobua" || TINHKIEM.VietLien(this.CheTen) == "thuongtan")
			{
				str = TINHKIEM.VietLien(this.CheTen.Replace("Đao búa", "Đao phủ").Replace("Thương tần", "Thương bổng") + "dataodocap" + (this.CheCap + 1).ToString());
			}
			else if (TINHKIEM.VietLien(this.CheTen).Contains("mao"))
			{
				if (this.CheCap + 1 > 10)
				{
					str = TINHKIEM.VietLien(this.CheTen + "tudataodocap" + (this.CheCap + 1).ToString());
				}
				else
				{
					str = TINHKIEM.VietLien(this.CheTen + "tudataodo" + (this.CheCap + 1).ToString());
				}
			}
			else if (TINHKIEM.VietLien(this.CheTen).Contains("khuyen"))
			{
				if (this.CheCap + 1 > 10)
				{
					str = TINHKIEM.VietLien("hoandataodocap" + (this.CheCap + 1).ToString());
				}
				else
				{
					str = TINHKIEM.VietLien("hoandataodo" + (this.CheCap + 1).ToString());
				}
			}
			else if (this.CheCap + 1 > 10)
			{
				str = TINHKIEM.VietLien(this.CheTen + "dataodocap" + (this.CheCap + 1).ToString());
			}
			else
			{
				str = TINHKIEM.VietLien(this.CheTen + "dataodo" + (this.CheCap + 1).ToString());
			}
			return TINHKIEM.VietLien(str);
		}

		// Token: 0x060008E4 RID: 2276 RVA: 0x0003C174 File Offset: 0x0003A374
		public string HaveNguyenLieu()
		{
			switch (this.CheLoai)
			{
			case 0:
			case 1:
			case 2:
			case 3:
			case 4:
			case 5:
				if (Game.Is69DO)
				{
					using (List<PacketItem>.Enumerator enumerator = PacketItem.Enum(this).GetEnumerator())
					{
						while (enumerator.MoveNext())
						{
							if (TINHKIEM.VietLien(enumerator.Current.Name).Contains("refinediron"))
							{
								return string.Empty;
							}
						}
					}
					return "refinediron";
				}
				using (List<PacketItem>.Enumerator enumerator2 = PacketItem.Enum(this).GetEnumerator())
				{
					while (enumerator2.MoveNext())
					{
						if (TINHKIEM.VietLien(enumerator2.Current.Name).Contains("tinhthiet"))
						{
							return string.Empty;
						}
					}
				}
				return "tinhthiet";
			case 6:
			case 7:
			case 8:
			case 9:
			case 10:
			case 11:
			case 12:
				if (Game.Is69DO)
				{
					using (List<PacketItem>.Enumerator enumerator3 = PacketItem.Enum(this).GetEnumerator())
					{
						while (enumerator3.MoveNext())
						{
							if (TINHKIEM.VietLien(enumerator3.Current.Name).Contains("cottoncloth"))
							{
								return string.Empty;
							}
						}
					}
					return "cottoncloth";
				}
				using (List<PacketItem>.Enumerator enumerator4 = PacketItem.Enum(this).GetEnumerator())
				{
					while (enumerator4.MoveNext())
					{
						if (TINHKIEM.VietLien(enumerator4.Current.Name).Contains("vaibong"))
						{
							return string.Empty;
						}
					}
				}
				return "vaibong";
			case 13:
			case 14:
			case 15:
				if (Game.Is69DO)
				{
					using (List<PacketItem>.Enumerator enumerator5 = PacketItem.Enum(this).GetEnumerator())
					{
						while (enumerator5.MoveNext())
						{
							if (TINHKIEM.VietLien(enumerator5.Current.Name).Contains("darksilver"))
							{
								return string.Empty;
							}
						}
					}
					return "darksilver";
				}
				using (List<PacketItem>.Enumerator enumerator6 = PacketItem.Enum(this).GetEnumerator())
				{
					while (enumerator6.MoveNext())
					{
						if (TINHKIEM.VietLien(enumerator6.Current.Name).Contains("bingan"))
						{
							return string.Empty;
						}
					}
				}
				return "bingan";
			default:
				return "";
			}
			string result;
			return result;
		}

		// Token: 0x060008E5 RID: 2277 RVA: 0x0003C444 File Offset: 0x0003A644
		public string IdNguyenLieu()
		{
			switch (this.CheLoai)
			{
			case 0:
			case 1:
			case 2:
			case 3:
			case 4:
			case 5:
				foreach (PacketItem packetItem in PacketItem.Enum(this))
				{
					if (packetItem.Type.Contains("Ore_15"))
					{
						return packetItem.Index.ToString("X2");
					}
				}
				return "";
			case 6:
			case 7:
			case 8:
			case 9:
			case 10:
			case 11:
			case 12:
				foreach (PacketItem packetItem2 in PacketItem.Enum(this))
				{
					if (packetItem2.Type.Contains("Ore_14"))
					{
						return packetItem2.Index.ToString("X2");
					}
				}
				return "";
			case 13:
			case 14:
			case 15:
				foreach (PacketItem packetItem3 in PacketItem.Enum(this))
				{
					if (packetItem3.Type.Contains("Ore_13"))
					{
						return packetItem3.Index.ToString("X2");
					}
				}
				return "";
			default:
				return "";
			}
			string result;
			return result;
		}

		// Token: 0x060008E6 RID: 2278 RVA: 0x0003C5F8 File Offset: 0x0003A7F8
		public int IdLoai()
		{
			switch (this.CheLoai)
			{
			case 0:
			case 1:
			case 2:
			case 3:
			case 4:
			case 5:
				return 46;
			case 6:
			case 7:
			case 8:
			case 9:
			case 10:
			case 11:
			case 12:
				return 47;
			case 13:
			case 14:
			case 15:
				return 48;
			default:
				return 0;
			}
		}

		// Token: 0x060008E7 RID: 2279 RVA: 0x0003C660 File Offset: 0x0003A860
		public int IdCheCap()
		{
			int result = 0;
			switch (this.CheLoai)
			{
			case 0:
				return 132 + this.CheCap;
			case 1:
				return 142 + this.CheCap;
			case 2:
				return 152 + this.CheCap;
			case 3:
				return 162 + this.CheCap;
			case 4:
				return 172 + this.CheCap;
			case 5:
				return 182 + this.CheCap;
			case 6:
				if (this.CheNoiNgoai == 1)
				{
					return 192 + this.CheCap;
				}
				if (this.CheNoiNgoai == 2)
				{
					return 76 + this.CheCap;
				}
				return 36 + this.CheCap;
			case 7:
				if (this.CheNoiNgoai == 1)
				{
					return 222 + this.CheCap;
				}
				if (this.CheNoiNgoai == 2)
				{
					return 106 + this.CheCap;
				}
				return 66 + this.CheCap;
			case 8:
				if (this.CheNoiNgoai == 1)
				{
					return 212 + this.CheCap;
				}
				if (this.CheNoiNgoai == 2)
				{
					return 96 + this.CheCap;
				}
				return 56 + this.CheCap;
			case 9:
				if (this.CheNoiNgoai == 1)
				{
					return 202 + this.CheCap;
				}
				if (this.CheNoiNgoai == 2)
				{
					return 86 + this.CheCap;
				}
				return 46 + this.CheCap;
			case 10:
				return 232 + this.CheCap;
			case 11:
				return 242 + this.CheCap;
			case 12:
				if (this.CheCap <= 4)
				{
					return 252 + this.CheCap;
				}
				return this.CheCap - 4;
			case 13:
				return 6 + this.CheCap;
			case 14:
				if (this.CheNoiNgoai != 1)
				{
					if (this.CheNoiNgoai != 2)
					{
						return 16 + this.CheCap;
					}
					if (this.CheCap <= 6)
					{
						return 16 + this.CheCap;
					}
					switch (this.CheCap)
					{
					case 7:
						return 40;
					case 8:
						return 42;
					case 9:
						return 44;
					default:
						return result;
					}
				}
				else
				{
					if (this.CheCap <= 6)
					{
						return 16 + this.CheCap;
					}
					switch (this.CheCap)
					{
					case 7:
						return 41;
					case 8:
						return 43;
					case 9:
						return 45;
					default:
						return result;
					}
				}
				break;
			case 15:
				if (this.CheNoiNgoai != 1)
				{
					if (this.CheNoiNgoai != 2)
					{
						return 26 + this.CheCap;
					}
					if (this.CheCap <= 6)
					{
						return 26 + this.CheCap;
					}
					switch (this.CheCap)
					{
					case 7:
						return 47;
					case 8:
						return 48;
					case 9:
						return 50;
					default:
						return result;
					}
				}
				else
				{
					if (this.CheCap <= 6)
					{
						return 26 + this.CheCap;
					}
					switch (this.CheCap)
					{
					case 7:
						return 46;
					case 8:
						return 49;
					case 9:
						return 51;
					default:
						return result;
					}
				}
				break;
			default:
				return result;
			}
		}

		// Token: 0x060008E8 RID: 2280 RVA: 0x0003C944 File Offset: 0x0003AB44
		public int IdNoiNgoai()
		{
			switch (this.CheLoai)
			{
			case 0:
				return 2;
			case 1:
				return 2;
			case 2:
				return 2;
			case 3:
				return 2;
			case 4:
				return 2;
			case 5:
				return 2;
			case 6:
				if (this.CheNoiNgoai != 1)
				{
					int cheNoiNgoai = this.CheNoiNgoai;
					return 3;
				}
				return 2;
			case 7:
				if (this.CheNoiNgoai != 1)
				{
					int cheNoiNgoai2 = this.CheNoiNgoai;
					return 3;
				}
				return 2;
			case 8:
				if (this.CheNoiNgoai != 1)
				{
					int cheNoiNgoai3 = this.CheNoiNgoai;
					return 3;
				}
				return 2;
			case 9:
				if (this.CheNoiNgoai != 1)
				{
					int cheNoiNgoai4 = this.CheNoiNgoai;
					return 3;
				}
				return 2;
			case 10:
				return 2;
			case 11:
				return 2;
			case 12:
				if (this.CheCap <= 4)
				{
					return 2;
				}
				return 3;
			case 13:
				return 3;
			case 14:
				if (this.CheCap <= 6)
				{
					return 3;
				}
				if (this.CheNoiNgoai == 1)
				{
					return 4;
				}
				if (this.CheNoiNgoai == 2)
				{
					return 4;
				}
				return 3;
			case 15:
				if (this.CheCap <= 6)
				{
					return 3;
				}
				if (this.CheNoiNgoai == 1)
				{
					return 4;
				}
				if (this.CheNoiNgoai == 2)
				{
					return 4;
				}
				return 3;
			default:
				return 0;
			}
		}

		// Token: 0x060008E9 RID: 2281 RVA: 0x0003CA59 File Offset: 0x0003AC59
		public void PushDebugMessage(string msg)
		{
			this.LuaDoOneLineString("PushDebugMessage(\"" + msg + "\");");
		}

		// Token: 0x060008EA RID: 2282 RVA: 0x0003CA74 File Offset: 0x0003AC74
		public bool IsDrop()
		{
			bool result = false;
			foreach (PacketItem packetItem in PacketItem.Enum(this))
			{
				if (TINHKIEM.VietLien(packetItem.TypeName) == TINHKIEM.VietLien(this.CheTen).Replace("hoan", "khuyen") || (!packetItem.Name.Contains("Lv ") && Game.Is69DO && TINHKIEM.VietLien(this.DTDName().Replace("lv", "")).Contains(TINHKIEM.VietLien(packetItem.TypeName).Replace("shoulderpad", "shp.").TrimEnd(new char[]
				{
					's'
				}).Replace("two-handed", "two-hand").Replace("one-handed", "one-hand"))))
				{
					string text = "";
					if (packetItem.Star < this.CheSao)
					{
						text = string.Concat(new string[]
						{
							text,
							"[Sao] = ",
							packetItem.Star.ToString(),
							" nhỏ hơn ",
							this.CheSao.ToString()
						});
					}
					if (packetItem.Line < this.CheDong)
					{
						text = string.Concat(new string[]
						{
							text,
							"| [Số Dòng] = ",
							packetItem.Line.ToString(),
							" nhỏ hơn ",
							this.CheDong.ToString()
						});
					}
					if (packetItem.TheLuc < this.CheDiem)
					{
						text = string.Concat(new string[]
						{
							text,
							"| [Thể Lực] = ",
							packetItem.TheLuc.ToString(),
							" nhỏ hơn ",
							this.CheDiem.ToString()
						});
					}
					if (this.IsCanDelete(packetItem) && !packetItem.IsHaveLongVan && !packetItem.IsHaveNgoc && text.Length > 0 && !this.TmpItemBeforeChe.Contains(packetItem.Index))
					{
						string text2 = "Hủy vật phẩm :" + packetItem.Name + " VÌ " + text;
						FrmMain.AddLog(string.Concat(new string[]
						{
							DateTime.Now.ToString("dd-MM | HH:mm"),
							" [",
							this.LastName,
							"] ",
							text2,
							" \n"
						}));
						this.PushDebugMessage(text2);
						this.DropItem(packetItem.Index);
						int daHuy = this.DaHuy;
						this.DaHuy = daHuy + 1;
						result = true;
					}
				}
			}
			return result;
		}

		// Token: 0x060008EB RID: 2283 RVA: 0x0003CD48 File Offset: 0x0003AF48
		public void BuyKNBShop(int index)
		{
			if (this.Address.GameType == 2)
			{
				string hex = "48DA60000000120000000000" + index.ToString("X2") + "009615FFFF00000000120000000000010";
				this.SendPacket(hex);
			}
		}

		// Token: 0x170001F7 RID: 503
		// (get) Token: 0x060008EC RID: 2284 RVA: 0x0003CD86 File Offset: 0x0003AF86
		// (set) Token: 0x060008ED RID: 2285 RVA: 0x0003CD8E File Offset: 0x0003AF8E
		public bool IsNhanHoaHongLo { get; set; }

		// Token: 0x170001F8 RID: 504
		// (get) Token: 0x060008EE RID: 2286 RVA: 0x0003CD97 File Offset: 0x0003AF97
		// (set) Token: 0x060008EF RID: 2287 RVA: 0x0003CD9F File Offset: 0x0003AF9F
		public bool IsNhanQuaBuiHoaHong { get; set; }

		// Token: 0x170001F9 RID: 505
		// (get) Token: 0x060008F0 RID: 2288 RVA: 0x0003CDA8 File Offset: 0x0003AFA8
		// (set) Token: 0x060008F1 RID: 2289 RVA: 0x0003CDB0 File Offset: 0x0003AFB0
		public Stopwatch TimeKiemTra { get; set; }

		// Token: 0x060008F2 RID: 2290 RVA: 0x0003CDB9 File Offset: 0x0003AFB9
		public void Muabake()
		{
			this.SendPacket(this.Address.bakePacket + "00 00 00 00 FF FF FF FF 11 00 00 00 02 00 00 00 0C 00 00 00 00 00 00 00 FF FF FF FF 14");
		}

		// Token: 0x060008F3 RID: 2291 RVA: 0x0003CDD6 File Offset: 0x0003AFD6
		public string TenThanh()
		{
			this.LuaDoOneLineString("local szMsgtenthanh = Guild:GetMyGuildDetailInfo('CityName'); return szMsgtenthanh;");
			this.LuaToStringBang();
			return this.LuaStringBang();
		}

		// Token: 0x060008F4 RID: 2292 RVA: 0x0003CDF0 File Offset: 0x0003AFF0
		public string TenMapThanh()
		{
			this.LuaDoOneLineString("local szMsgtenmap = Guild:GetMyGuildDetailInfo('LocalScene'); return szMsgtenmap;");
			this.LuaToString();
			return this.LuaString();
		}

		// Token: 0x060008F5 RID: 2293 RVA: 0x0003CE0A File Offset: 0x0003B00A
		public void ReturnBang()
		{
			this.LuaDoString(TINHKIEM.Hasher.Decrypt("P01VJ / iHB7HV / i8W01O52IixZO9M98etPdWtEtFRelpwbbEUCKqE0cQ / X4sOmKCVQq + r1nlW4ynMG6N6TWRX / x58Ua8EDQ9XpzrCfD9hRKWAzSuMav / sqlY7UdXkrM8 / YmSOBsw6gnQEGhgQMgrKt / 6aL7o / QJNfMnmld9Vs47q9Kx17JKAtMJqa1dD6j3sq0i2Vw + mlTBaIsWTvTPfHrYf / gWTY5kzPXfdYfEbQXT7TpsHol / K9MHRVr0wM8WcazXWOk5EgaFpuXWtQQBFZFn6I1gERTUF7Nq / ZvJ0LoUk =", "MessageBox_Other_OK_Clicked"));
		}

		// Token: 0x060008F6 RID: 2294 RVA: 0x0003CE21 File Offset: 0x0003B021
		public void BangOpen()
		{
			this.LuaDoString("setmetatable(_G, {__index = NewBangHui_Hygl_Env});Guild:AskGuildDetailInfo();");
		}

		// Token: 0x060008F7 RID: 2295 RVA: 0x0003CE2E File Offset: 0x0003B02E
		public void BangClose()
		{
			this.PostMessage(63, 105);
		}

		// Token: 0x060008F8 RID: 2296
		[DllImport("Bin\\EasyHook.dll")]
		private static extern IntPtr SetHook(IntPtr handle);

		// Token: 0x060008F9 RID: 2297
		[DllImport("Bin\\EasyHook.dll")]
		public static extern IntPtr UnHook(IntPtr handle);

		// Token: 0x04000428 RID: 1064
		public bool ChatGan = true;

		// Token: 0x04000429 RID: 1065
		public bool ChatTheGioi = true;

		// Token: 0x0400042A RID: 1066
		public bool ChatThanhThi;

		// Token: 0x0400042B RID: 1067
		public bool ChatDongMinh;

		// Token: 0x0400042C RID: 1068
		public bool ChatMonPhai;

		// Token: 0x0400042D RID: 1069
		public bool ChatBangPhai;

		// Token: 0x0400042E RID: 1070
		public bool ChatDoi;

		// Token: 0x0400042F RID: 1071
		public bool ishide;

		// Token: 0x04000430 RID: 1072
		public bool AutoEatVatPham;

		// Token: 0x04000431 RID: 1073
		public bool CongSinh;

		// Token: 0x04000432 RID: 1074
		public int CongSinhValue = 20;

		// Token: 0x04000433 RID: 1075
		public bool AutoThuPet;

		// Token: 0x04000434 RID: 1076
		public bool HuyetTe;

		// Token: 0x04000435 RID: 1077
		public int HuyetTeValue = 20;

		// Token: 0x04000436 RID: 1078
		public bool AutoHoiSinh;

		// Token: 0x04000439 RID: 1081
		public List<ThongBao> ListThongBao = new List<ThongBao>();

		// Token: 0x0400043B RID: 1083
		public bool LenBaiTrain;

		// Token: 0x0400043D RID: 1085
		public BaiTrain _baitrain = new BaiTrain();

		// Token: 0x0400043F RID: 1087
		public int LenBanDoIndex;

		// Token: 0x04000440 RID: 1088
		public int LenBaiIndex;

		// Token: 0x04000451 RID: 1105
		public int DelayTime = 9;

		// Token: 0x04000465 RID: 1125
		public static BaiTrain baitmp = new BaiTrain();

		// Token: 0x040004A6 RID: 1190
		public bool AlarmChat;

		// Token: 0x040004B2 RID: 1202
		public bool IsSellItem;

		// Token: 0x040004B3 RID: 1203
		public bool IsDropItem;

		// Token: 0x040004B8 RID: 1208
		public double TimeGiaoChat = 180.0;

		// Token: 0x040004BA RID: 1210
		public bool AutoTrain;

		// Token: 0x040004BB RID: 1211
		public bool UsingTholinhChau;

		// Token: 0x040004C1 RID: 1217
		public bool IsArletMP;

		// Token: 0x040004C2 RID: 1218
		public bool isAreadyBuff;

		// Token: 0x040004D1 RID: 1233
		public bool IsBank;

		// Token: 0x040004E8 RID: 1256
		public static bool IsGiamDinh = true;

		// Token: 0x040004E9 RID: 1257
		public int SoLuongMua = 20;

		// Token: 0x040004EA RID: 1258
		public int SoLuongChe = 100;

		// Token: 0x040004EE RID: 1262
		public bool HuyNguyenLieu = true;

		// Token: 0x040004EF RID: 1263
		public bool IsMuaNguyenLieu;

		// Token: 0x040004F3 RID: 1267
		public Stopwatch CheDO = new Stopwatch();

		// Token: 0x040004F4 RID: 1268
		public bool IsOpenBag;

		// Token: 0x040004F8 RID: 1272
		public static int Def;

		// Token: 0x040004F9 RID: 1273
		public AOBScan AOB;

		// Token: 0x040004FA RID: 1274
		private static Game.LuaToStringDelegate LuaToStringFunc;

		// Token: 0x040004FB RID: 1275
		public PacketItem Packet;

		// Token: 0x040004FC RID: 1276
		private ListViewItem item;

		// Token: 0x040004FD RID: 1277
		public LUA LUA;

		// Token: 0x040004FE RID: 1278
		private string idLocDo = "";

		// Token: 0x040004FF RID: 1279
		public int ProcessId;

		// Token: 0x04000500 RID: 1280
		public Address Address;

		// Token: 0x04000501 RID: 1281
		public IntPtr Parrent;

		// Token: 0x04000502 RID: 1282
		public Memory Memory;

		// Token: 0x04000503 RID: 1283
		public TLBB TLBB;

		// Token: 0x04000504 RID: 1284
		public GameObjects Objects;

		// Token: 0x04000505 RID: 1285
		public List<Skill> Skills = new List<Skill>();

		// Token: 0x04000506 RID: 1286
		public List<GameControl> Controls = new List<GameControl>();

		// Token: 0x04000507 RID: 1287
		public int IdleTime;

		// Token: 0x04000508 RID: 1288
		public int CaptchaTime;

		// Token: 0x04000509 RID: 1289
		public int StandTime;

		// Token: 0x0400050A RID: 1290
		public string LastId;

		// Token: 0x0400050B RID: 1291
		public string LastName;

		// Token: 0x0400050C RID: 1292
		public int ExpStart;

		// Token: 0x0400050D RID: 1293
		public Stopwatch AutoTime = Stopwatch.StartNew();

		// Token: 0x0400050E RID: 1294
		public float CharX;

		// Token: 0x0400050F RID: 1295
		public float CharY;

		// Token: 0x04000510 RID: 1296
		public int ExpGain;

		// Token: 0x04000511 RID: 1297
		public float ExpSpeed;

		// Token: 0x04000512 RID: 1298
		public float RadiusX;

		// Token: 0x04000513 RID: 1299
		public float RadiusY;

		// Token: 0x04000514 RID: 1300
		public int TargetId;

		// Token: 0x04000515 RID: 1301
		public int BaseSkill;

		// Token: 0x04000516 RID: 1302
		public Skill NMSKill;

		// Token: 0x04000517 RID: 1303
		public bool ExitHPLow;

		// Token: 0x04000518 RID: 1304
		public bool IsRide;

		// Token: 0x04000519 RID: 1305
		public bool[] Alt = new bool[10];

		// Token: 0x0400051A RID: 1306
		public bool[] F = new bool[12];

		// Token: 0x0400051B RID: 1307
		public int[] KeyDelay = new int[22];

		// Token: 0x0400051C RID: 1308
		public int BuffPetPercent = 50;

		// Token: 0x0400051D RID: 1309
		public int DeadX;

		// Token: 0x0400051E RID: 1310
		public int DeadY;

		// Token: 0x0400051F RID: 1311
		public int DeadMap;

		// Token: 0x04000520 RID: 1312
		public int DeadFakeMap;

		// Token: 0x04000521 RID: 1313
		public bool IsDead;

		// Token: 0x04000522 RID: 1314
		public bool IsAuto = true;

		// Token: 0x04000523 RID: 1315
		public bool IsAttack = true;

		// Token: 0x04000524 RID: 1316
		private bool isLure;

		// Token: 0x04000525 RID: 1317
		public bool IsPet = true;

		// Token: 0x04000526 RID: 1318
		public bool IsHP = true;

		// Token: 0x04000527 RID: 1319
		public bool IsMP = true;

		// Token: 0x04000528 RID: 1320
		public bool IsNM;

		// Token: 0x04000529 RID: 1321
		private bool isRadius;

		// Token: 0x0400052A RID: 1322
		private int radiusMap;

		// Token: 0x0400052B RID: 1323
		public bool IsRao;

		// Token: 0x0400052C RID: 1324
		public bool IsLuyenKim;

		// Token: 0x0400052D RID: 1325
		public bool IsSuMon;

		// Token: 0x0400052E RID: 1326
		public bool IsTuDuong;

		// Token: 0x0400052F RID: 1327
		public bool IsKhoang;

		// Token: 0x04000530 RID: 1328
		public bool IsDuoc;

		// Token: 0x04000531 RID: 1329
		public bool IsTrongTrot;

		// Token: 0x04000532 RID: 1330
		public bool IsThuHoach;

		// Token: 0x04000533 RID: 1331
		public bool IsBachHoaDuyen;

		// Token: 0x04000534 RID: 1332
		public bool BachHoaDuyenCompleted;

		// Token: 0x04000535 RID: 1333
		public bool IsQDua;

		// Token: 0x04000536 RID: 1334
		public bool QDuaCompleted;

		// Token: 0x04000537 RID: 1335
		public bool NhatKieuMoi;

		// Token: 0x04000538 RID: 1336
		public List<string> lstNhat = new List<string>();

		// Token: 0x04000539 RID: 1337
		public List<string> lstBoQua = new List<string>();

		// Token: 0x0400053A RID: 1338
		public List<string> lstXoa = new List<string>();

		// Token: 0x0400053B RID: 1339
		public List<string> lstDoNgon = new List<string>();

		// Token: 0x0400053C RID: 1340
		public int QDuaIndex;

		// Token: 0x0400053D RID: 1341
		public bool DaNhanHoaHong;

		// Token: 0x0400053E RID: 1342
		public bool DaNhanHoaChung;

		// Token: 0x0400053F RID: 1343
		public bool IsChucPhuc;

		// Token: 0x04000540 RID: 1344
		public bool IsCauOThuoc;

		// Token: 0x04000541 RID: 1345
		public bool IsPickItem;

		// Token: 0x04000542 RID: 1346
		public bool X4;

		// Token: 0x04000543 RID: 1347
		public int TrongTrotIndex;

		// Token: 0x04000544 RID: 1348
		public int ThuHoachIndex;

		// Token: 0x04000545 RID: 1349
		public int MoveIndex = -1;

		// Token: 0x04000546 RID: 1350
		public string Pass2 = "1312";

		// Token: 0x04000547 RID: 1351
		public bool IsTrongHoa;

		// Token: 0x04000548 RID: 1352
		public bool IsBonHoa;

		// Token: 0x04000549 RID: 1353
		public bool IsThuHoachHoa;

		// Token: 0x0400054A RID: 1354
		public bool IsTrungAc;

		// Token: 0x0400054B RID: 1355
		public string MissionInfo;

		// Token: 0x0400054C RID: 1356
		public int MissionX;

		// Token: 0x0400054D RID: 1357
		public int MissionY;

		// Token: 0x0400054E RID: 1358
		public string MissionMonter;

		// Token: 0x0400054F RID: 1359
		public int MissionMap;

		// Token: 0x04000550 RID: 1360
		public List<int> lstNguoiRom = new List<int>();

		// Token: 0x04000551 RID: 1361
		public string TrangThaiTrongTrot = string.Empty;

		// Token: 0x04000552 RID: 1362
		public string MissionNPC;

		// Token: 0x04000553 RID: 1363
		public int MissionID;

		// Token: 0x04000554 RID: 1364
		public Stopwatch MoveExTime = Stopwatch.StartNew();

		// Token: 0x04000555 RID: 1365
		public Stopwatch EXITTime = Stopwatch.StartNew();

		// Token: 0x04000556 RID: 1366
		public List<int[]> ListMoveEx = new List<int[]>();

		// Token: 0x04000557 RID: 1367
		public Stopwatch NhatDoEX = Stopwatch.StartNew();

		// Token: 0x04000558 RID: 1368
		public int RoundX;

		// Token: 0x04000559 RID: 1369
		public int RoundY;

		// Token: 0x0400055A RID: 1370
		private int BachHoaDuyenX;

		// Token: 0x0400055B RID: 1371
		private int BachHoaDuyenY;

		// Token: 0x0400055C RID: 1372
		public List<int> ListDaBon = new List<int>();

		// Token: 0x0400055D RID: 1373
		public List<long[]> ListHoaXuatHien = new List<long[]>();

		// Token: 0x0400055E RID: 1374
		public string TrungAcInfo = string.Empty;

		// Token: 0x0400055F RID: 1375
		public int Extra1;

		// Token: 0x04000560 RID: 1376
		public string TrangThaiBTD = "";

		// Token: 0x04000561 RID: 1377
		private int BTDX = -1;

		// Token: 0x04000562 RID: 1378
		private int BTDY = -1;

		// Token: 0x04000563 RID: 1379
		private int BTDMAP = -1;

		// Token: 0x04000564 RID: 1380
		private string BTDInfo = "";

		// Token: 0x04000565 RID: 1381
		private int BTDIndex = -1;

		// Token: 0x04000566 RID: 1382
		private Stopwatch comeTime;

		// Token: 0x04000567 RID: 1383
		private Stopwatch doneTime = Stopwatch.StartNew();

		// Token: 0x04000568 RID: 1384
		public string BachHoaDuyenInfo = "";

		// Token: 0x04000569 RID: 1385
		public static bool DaRao = false;

		// Token: 0x0400056A RID: 1386
		public int MoveCount;

		// Token: 0x0400056B RID: 1387
		public string TrangThaiQD = "";

		// Token: 0x0400056C RID: 1388
		public string QDInfo = "";

		// Token: 0x0400056D RID: 1389
		public int QDuaX = 134;

		// Token: 0x0400056E RID: 1390
		public int QDuaY = 165;

		// Token: 0x0400056F RID: 1391
		public int QDuaMap = MAP.TayHo;

		// Token: 0x04000570 RID: 1392
		public string thongtinbang = "";

		// Token: 0x04000571 RID: 1393
		public int SoHopQua;

		// Token: 0x04000572 RID: 1394
		public static List<int> ListDangLumHop = new List<int>();

		// Token: 0x04000573 RID: 1395
		public string TenThanhKT = "-1";

		// Token: 0x04000574 RID: 1396
		public string TenMapThanhKT = "-1";

		// Token: 0x04000575 RID: 1397
		public int TrongHoaThuHoachId = -1;

		// Token: 0x04000576 RID: 1398
		public int TrongHoaThuHoachX;

		// Token: 0x04000577 RID: 1399
		public int TrongHoaThuHoachY;

		// Token: 0x04000578 RID: 1400
		private int idHoa = -1;

		// Token: 0x04000579 RID: 1401
		private Dictionary<int, DateTime> ListThuHoachBHD = new Dictionary<int, DateTime>();

		// Token: 0x0400057A RID: 1402
		private Dictionary<int, DateTime> ListBHDXuatHien = new Dictionary<int, DateTime>();

		// Token: 0x0400057B RID: 1403
		public static List<int> ListDangThuHoach = new List<int>();

		// Token: 0x0400057C RID: 1404
		public static HashSet<int> HashDangBon = new HashSet<int>();

		// Token: 0x0400057D RID: 1405
		public HashSet<string> BlackListHoa = new HashSet<string>();

		// Token: 0x0400057E RID: 1406
		public string dialogInfo = "";

		// Token: 0x0400057F RID: 1407
		public List<long[]> ListHoaTruongThanh = new List<long[]>();

		// Token: 0x04000580 RID: 1408
		public Stopwatch HideTime = Stopwatch.StartNew();

		// Token: 0x04000581 RID: 1409
		private HashSet<string> NotSafe = new HashSet<string>();

		// Token: 0x04000582 RID: 1410
		private int SafeX;

		// Token: 0x04000583 RID: 1411
		private int SafeY;

		// Token: 0x04000584 RID: 1412
		private int CurPhungMinhIndex = -1;

		// Token: 0x04000585 RID: 1413
		public static HashSet<string> HashToaDo = new HashSet<string>();

		// Token: 0x04000586 RID: 1414
		public bool Talked;

		// Token: 0x04000587 RID: 1415
		public DateTime BossTime = DateTime.MinValue;

		// Token: 0x04000588 RID: 1416
		public bool IsTueHong;

		// Token: 0x04000589 RID: 1417
		public int TueHongState;

		// Token: 0x0400058A RID: 1418
		public string TrangThaiNhiemVuCoBan = "";

		// Token: 0x0400058B RID: 1419
		public List<Script> ListNhiemVu = new List<Script>();

		// Token: 0x0400058C RID: 1420
		private int IsBossVanKiemCocDie;

		// Token: 0x0400058D RID: 1421
		public bool Is2d;

		// Token: 0x0400058E RID: 1422
		private const int GWL_STYLE = -16;

		// Token: 0x0400058F RID: 1423
		private Thread InitThread;

		// Token: 0x04000590 RID: 1424
		private bool IsOpenPass2;

		// Token: 0x04000591 RID: 1425
		public int TimeOnMap;

		// Token: 0x04000592 RID: 1426
		public int SafeTime;

		// Token: 0x04000593 RID: 1427
		public bool IsNhatHop;

		// Token: 0x04000594 RID: 1428
		public bool IsNhatHopQDua;

		// Token: 0x04000595 RID: 1429
		public bool IsMoBang;

		// Token: 0x04000596 RID: 1430
		private int biendem;

		// Token: 0x04000597 RID: 1431
		public bool Live = true;

		// Token: 0x04000598 RID: 1432
		public string Enemy = "";

		// Token: 0x04000599 RID: 1433
		private Stopwatch ClearTime = Stopwatch.StartNew();

		// Token: 0x0400059A RID: 1434
		private int IsTalked = -1;

		// Token: 0x0400059B RID: 1435
		private int MapATIndex = -1;

		// Token: 0x0400059C RID: 1436
		private int CurMapATIndex = -1;

		// Token: 0x0400059D RID: 1437
		private bool isQuangCao;

		// Token: 0x0400059E RID: 1438
		public Stopwatch BossDieTime = Stopwatch.StartNew();

		// Token: 0x0400059F RID: 1439
		private string TrangThaiThuyLao = "";

		// Token: 0x040005A0 RID: 1440
		public int AcBa = -1;

		// Token: 0x040005A1 RID: 1441
		public static NPC HoaHachCan = new NPC
		{
			Id = 8371,
			X = 180,
			Y = 90,
			Map = 236,
			INFOAIM = "#GYến Tử Ổ #RHoa Hách Cấn#{_INFOAIM180,90,236,Hoa Hách Cấn}"
		};

		// Token: 0x040005A2 RID: 1442
		public bool TraQ;

		// Token: 0x040005A3 RID: 1443
		public bool NhanQ;

		// Token: 0x040005A4 RID: 1444
		public bool IsContinute;

		// Token: 0x040005A5 RID: 1445
		public bool IsClick;

		// Token: 0x040005A6 RID: 1446
		public HashSet<string> Rac = new HashSet<string>
		{
			"Linh Thú Diện",
			"Linh Thú Trảo",
			"Linh Thú Giáp",
			"Linh Thú Hoàn",
			"Linh Thú Sức"
		};

		// Token: 0x040005A7 RID: 1447
		public Stopwatch tranTime = Stopwatch.StartNew();

		// Token: 0x040005A8 RID: 1448
		public Stopwatch TrimTime = Stopwatch.StartNew();

		// Token: 0x040005A9 RID: 1449
		public bool IsXongPhuBan;

		// Token: 0x040005AA RID: 1450
		private Stopwatch LastPhanDame = Stopwatch.StartNew();

		// Token: 0x040005AB RID: 1451
		public Thread ThreadAuto;

		// Token: 0x040005AC RID: 1452
		public List<GameObject> ListBay = new List<GameObject>();

		// Token: 0x040005AD RID: 1453
		private Stopwatch NeBayTime = Stopwatch.StartNew();

		// Token: 0x040005AE RID: 1454
		private bool come;

		// Token: 0x040005AF RID: 1455
		private bool comeex;

		// Token: 0x040005B0 RID: 1456
		private int lastX2TimeSec;

		// Token: 0x040005B1 RID: 1457
		private Stopwatch TimeStand = Stopwatch.StartNew();

		// Token: 0x040005B2 RID: 1458
		public int KheLinhCount;

		// Token: 0x040005B3 RID: 1459
		private bool IsMini;

		// Token: 0x040005B4 RID: 1460
		private bool setSafeTime;

		// Token: 0x040005B5 RID: 1461
		private Stopwatch NMTime = Stopwatch.StartNew();

		// Token: 0x040005B6 RID: 1462
		public int CurTab;

		// Token: 0x040005B7 RID: 1463
		public int NPCID = 191;

		// Token: 0x040005B8 RID: 1464
		private bool IsHideAgain;

		// Token: 0x040005B9 RID: 1465
		public int x2;

		// Token: 0x040005BA RID: 1466
		public int SoLanX2;

		// Token: 0x040005BB RID: 1467
		public bool IsNguyenVong;

		// Token: 0x040005BC RID: 1468
		public bool IsVanMay;

		// Token: 0x040005BD RID: 1469
		public bool IsLyHoa;

		// Token: 0x040005BE RID: 1470
		public bool OkNhanDa;

		// Token: 0x040005BF RID: 1471
		public bool IsNguHanhPhap;

		// Token: 0x040005C0 RID: 1472
		private List<int> lootPacketId = new List<int>();

		// Token: 0x040005C1 RID: 1473
		public static List<int> lootPacketIdDua = new List<int>();

		// Token: 0x040005C2 RID: 1474
		private Stopwatch pickTiem = Stopwatch.StartNew();

		// Token: 0x040005C3 RID: 1475
		private Stopwatch CareTime = Stopwatch.StartNew();

		// Token: 0x040005C4 RID: 1476
		private int PickedId = -1;

		// Token: 0x040005C5 RID: 1477
		private List<int> BlackList = new List<int>();

		// Token: 0x040005C6 RID: 1478
		public Stopwatch PickTime = Stopwatch.StartNew();

		// Token: 0x040005C7 RID: 1479
		public bool IsQuit;

		// Token: 0x040005C8 RID: 1480
		public GameObject BestTarget;

		// Token: 0x040005C9 RID: 1481
		private bool isAtkFollow;

		// Token: 0x040005CA RID: 1482
		private Stopwatch lastAutoMove = Stopwatch.StartNew();

		// Token: 0x040005CB RID: 1483
		private Stopwatch lastTalk = Stopwatch.StartNew();

		// Token: 0x040005CC RID: 1484
		private Stopwatch LastShoww = Stopwatch.StartNew();

		// Token: 0x040005CD RID: 1485
		private int AtackTime;

		// Token: 0x040005CE RID: 1486
		public static HashSet<int> LureId = new HashSet<int>();

		// Token: 0x040005CF RID: 1487
		private int WM_KEYDOWN = 256;

		// Token: 0x040005D0 RID: 1488
		private int WM_KEYUP = 257;

		// Token: 0x040005D1 RID: 1489
		public int[] AddressOneLineEx = new int[20];

		// Token: 0x040005D2 RID: 1490
		private int curLine;

		// Token: 0x040005D3 RID: 1491
		public byte[] bufferRecv = new byte[10];

		// Token: 0x040005D4 RID: 1492
		private int lastResetTime;

		// Token: 0x040005D5 RID: 1493
		private Stopwatch lastReset = Stopwatch.StartNew();

		// Token: 0x040005D6 RID: 1494
		public string TrangThaiLuyenKim = "";

		// Token: 0x040005D7 RID: 1495
		public int ChuyenKhoangX;

		// Token: 0x040005D8 RID: 1496
		public int ChuyenKhoangY;

		// Token: 0x040005D9 RID: 1497
		public int LuyenKimX;

		// Token: 0x040005DA RID: 1498
		public int LuyenKimY;

		// Token: 0x040005DB RID: 1499
		public string TrangThai = "";

		// Token: 0x040005DC RID: 1500
		private string lpmh = "";

		// Token: 0x040005DD RID: 1501
		public string TrangThaiTuDuong = "";

		// Token: 0x040005DE RID: 1502
		public string TuDuongInfo = "";

		// Token: 0x040005DF RID: 1503
		public int TuDuongX;

		// Token: 0x040005E0 RID: 1504
		public int TuDuongY;

		// Token: 0x040005E1 RID: 1505
		public int TuDuongMap;

		// Token: 0x040005E2 RID: 1506
		public string TrangThaiXayDung = "";

		// Token: 0x040005E3 RID: 1507
		public string XayDungInfo = "";

		// Token: 0x040005E4 RID: 1508
		public bool IsXayDung;

		// Token: 0x040005E5 RID: 1509
		public int XayDungDanhQuaiTime;

		// Token: 0x040005E6 RID: 1510
		public int KetMap;

		// Token: 0x040005E7 RID: 1511
		public bool IsAcceptAll;

		// Token: 0x040005E8 RID: 1512
		public string TrangThaiTuBaoBon = "";

		// Token: 0x040005E9 RID: 1513
		public bool IsTuBaoBon;

		// Token: 0x040005EA RID: 1514
		public string TrangThaiSuMon = "";

		// Token: 0x040005EB RID: 1515
		public string SuMonInfo = "";

		// Token: 0x040005EC RID: 1516
		public int SuMonX;

		// Token: 0x040005ED RID: 1517
		public int SuMonY;

		// Token: 0x040005EE RID: 1518
		public int SuMonMap;

		// Token: 0x040005EF RID: 1519
		private bool IsDauCo;

		// Token: 0x040005F0 RID: 1520
		public string DoSuMon = "";

		// Token: 0x040005F1 RID: 1521
		public AlarmVaoPhai alarmVaoPhai;

		// Token: 0x040005F2 RID: 1522
		public bool IsNotClear;

		// Token: 0x040005F3 RID: 1523
		public List<int[]> ListMove = new List<int[]>();

		// Token: 0x040005F4 RID: 1524
		public int XayDungX;

		// Token: 0x040005F5 RID: 1525
		public int XayDungY;

		// Token: 0x040005F6 RID: 1526
		public int XayDungMap;

		// Token: 0x040005F7 RID: 1527
		public static HashSet<string> JunkItemName = new HashSet<string>
		{
			"Phục Linh Cao",
			"Bất Lão Cao",
			"Hoạt Huyết Tán",
			"Ngưu Hoàn Phấn",
			"Hoàn Linh Đan",
			"Sơn Dược Chúc",
			"Hành Khí Tán",
			"Tiểu Hành Nang",
			"Trung Hành Nang",
			"Trung Cách Rương",
			"Ngũ Độc Cẩm Y",
			"Đường Môn Khinh Trang"
		};

		// Token: 0x040005F8 RID: 1528
		public static HashSet<string> JunkItemType = new HashSet<string>
		{
			"Nguyên liệu đúc",
			"Vật liệu may mặc",
			"N.liệu công nghệ",
			"Thịt Sơ Cấp",
			"Thịt Trung Cấp",
			"Thịt Cao Cấp",
			"Da Sơ Cấp",
			"Da Trung Cấp",
			"Da Cao Cấp",
			"Vật liệu chế dược"
		};

		// Token: 0x040005F9 RID: 1529
		public static HashSet<string> TrangBi = new HashSet<string>
		{
			"Khuyên",
			"Nỏ",
			"Đơn Đoản",
			"Hộ Phù",
			"Đao Búa",
			"Hộ Kiên",
			"Hộ Uyển",
			"Y Phục",
			"Hài",
			"Song Đoản",
			"Thương Bổng",
			"Hộ Thủ",
			"Mão",
			"Hạng Liên",
			"Yêu Đai",
			"Phiến",
			"Giới Chỉ"
		};

		// Token: 0x040005FA RID: 1530
		private int mapAcTac;

		// Token: 0x040005FB RID: 1531
		private int mapTKC;

		// Token: 0x040005FC RID: 1532
		private bool isAlarmHP;

		// Token: 0x040005FD RID: 1533
		private bool isAlarmPK;

		// Token: 0x040005FE RID: 1534
		private bool isAlarmDead;

		// Token: 0x040005FF RID: 1535
		private bool isAlarmCaptcha;

		// Token: 0x04000600 RID: 1536
		private int disconnectedTime;

		// Token: 0x04000601 RID: 1537
		private bool isAlarmBachHoaDuyen;

		// Token: 0x04000602 RID: 1538
		public bool isAlarmKet;

		// Token: 0x04000603 RID: 1539
		public bool isAlarmHong;

		// Token: 0x04000604 RID: 1540
		private int AddressToString;

		// Token: 0x04000605 RID: 1541
		private int AddressTenBang;

		// Token: 0x04000606 RID: 1542
		private int AddressEnemy;

		// Token: 0x04000607 RID: 1543
		private bool isAlarmDua;

		// Token: 0x04000608 RID: 1544
		public bool IsAlarmAcBa;

		// Token: 0x04000609 RID: 1545
		public string TrangThaiPhuMau = "";

		// Token: 0x0400060A RID: 1546
		public string PhuMauInfo = "";

		// Token: 0x0400060B RID: 1547
		public int PhuMauX;

		// Token: 0x0400060C RID: 1548
		public int PhuMauY;

		// Token: 0x0400060D RID: 1549
		public int PhuMauMap;

		// Token: 0x0400060E RID: 1550
		public bool IsCheDo;

		// Token: 0x0400060F RID: 1551
		public int CheLoai;

		// Token: 0x04000610 RID: 1552
		public string CheTen = string.Empty;

		// Token: 0x04000611 RID: 1553
		public int CheCap;

		// Token: 0x04000612 RID: 1554
		public int CheNoiNgoai = 1;

		// Token: 0x04000613 RID: 1555
		public int CheSao = 6;

		// Token: 0x04000614 RID: 1556
		public int CheDong = 5;

		// Token: 0x04000615 RID: 1557
		public int CheDiem;

		// Token: 0x04000616 RID: 1558
		public int MuaCount;

		// Token: 0x04000617 RID: 1559
		public string MuaName = string.Empty;

		// Token: 0x04000618 RID: 1560
		public static int shopIndex = 6;

		// Token: 0x04000619 RID: 1561
		public bool IsChayVong;

		// Token: 0x0400061A RID: 1562
		public bool IsNhatHopall;

		// Token: 0x0400061B RID: 1563
		public bool IsOptLocDo;

		// Token: 0x0400061C RID: 1564
		public bool IsNhanQuaHoaHong;

		// Token: 0x0400061D RID: 1565
		public bool NhanQuaHoaHongCompleted;

		// Token: 0x0400061E RID: 1566
		private string trangthaichayvong = "";

		// Token: 0x02000169 RID: 361
		// (Invoke) Token: 0x06001180 RID: 4480
		private delegate int LuaToStringDelegate();

		// Token: 0x0200016A RID: 362
		[Flags]
		public enum ThreadAccess
		{
			// Token: 0x04000E44 RID: 3652
			Terminate = 1,
			// Token: 0x04000E45 RID: 3653
			SuspendResume = 2,
			// Token: 0x04000E46 RID: 3654
			GetContext = 8,
			// Token: 0x04000E47 RID: 3655
			SetContext = 16,
			// Token: 0x04000E48 RID: 3656
			SetInformation = 32,
			// Token: 0x04000E49 RID: 3657
			QueryInformation = 64,
			// Token: 0x04000E4A RID: 3658
			SetThreadToken = 128,
			// Token: 0x04000E4B RID: 3659
			Impersonate = 256,
			// Token: 0x04000E4C RID: 3660
			DirectImpersonation = 512
		}

		// Token: 0x0200016B RID: 363
		public enum ThreadInfoClass
		{
			// Token: 0x04000E4E RID: 3662
			ThreadQuerySetWin32StartAddress = 9
		}
	}
}
