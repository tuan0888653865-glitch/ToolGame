using System;
using System.Collections.Generic;

namespace TinhKiemAuto
{
	// Token: 0x02000096 RID: 150
	public class GameObjects
	{
		// Token: 0x17000227 RID: 551
		// (get) Token: 0x06000951 RID: 2385 RVA: 0x0003E108 File Offset: 0x0003C308
		public List<GameObject> AllNpc
		{
			get
			{
				List<GameObject> list = new List<GameObject>();
				foreach (GameObject gameObject in this.All)
				{
					if (gameObject.IsNPC)
					{
						list.Add(gameObject);
					}
				}
				return list;
			}
		}

		// Token: 0x17000228 RID: 552
		// (get) Token: 0x06000952 RID: 2386 RVA: 0x0003E16C File Offset: 0x0003C36C
		public List<GameObject> AllPlayer
		{
			get
			{
				List<GameObject> list = new List<GameObject>();
				foreach (GameObject gameObject in this.All)
				{
					if (gameObject.IsPlayer)
					{
						list.Add(gameObject);
					}
				}
				return list;
			}
		}

		// Token: 0x06000953 RID: 2387 RVA: 0x0003E1D0 File Offset: 0x0003C3D0
		public bool Have(string name)
		{
			using (List<GameObject>.Enumerator enumerator = this.All.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (TINHKIEM.VietLien(enumerator.Current.Name).Contains(TINHKIEM.VietLien(name)))
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x06000954 RID: 2388 RVA: 0x0003E23C File Offset: 0x0003C43C
		public bool HaveEx(string name)
		{
			using (List<GameObject>.Enumerator enumerator = this.All.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (enumerator.Current.CleanName == name)
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x06000955 RID: 2389 RVA: 0x0003E29C File Offset: 0x0003C49C
		public bool HaveMonter(string name)
		{
			foreach (GameObject gameObject in this.All)
			{
				if (gameObject.IsMonter && TINHKIEM.VietLien(gameObject.Name).Contains(TINHKIEM.VietLien(name)))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06000956 RID: 2390 RVA: 0x0003E310 File Offset: 0x0003C510
		public bool HaveTaiNguyen(string name)
		{
			foreach (GameObject gameObject in this.All)
			{
				if (gameObject.IsTaiNguyen && TINHKIEM.VietLien(gameObject.Name).Contains(TINHKIEM.VietLien(name)))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06000957 RID: 2391 RVA: 0x0003E384 File Offset: 0x0003C584
		public List<GameObject> NearMonter(float x, float y, float distance)
		{
			List<GameObject> list = new List<GameObject>();
			foreach (GameObject gameObject in this.Monter)
			{
				if (gameObject.GetDistance(x, y) < distance)
				{
					list.Add(gameObject);
				}
			}
			if (this.game.TLBB.MapId == MAP.ViemMaSon)
			{
				foreach (GameObject gameObject2 in this.All)
				{
					if (gameObject2.GetDistance(x, y) < distance && gameObject2.Menpai == 12 && gameObject2.IsNPC)
					{
						list.Add(gameObject2);
					}
				}
			}
			return list;
		}

		// Token: 0x17000229 RID: 553
		// (get) Token: 0x06000958 RID: 2392 RVA: 0x0003E460 File Offset: 0x0003C660
		public List<GameObject> NearMonter5m
		{
			get
			{
				List<GameObject> list = new List<GameObject>();
				foreach (GameObject gameObject in this.Monter)
				{
					if (gameObject.GetDistance(this.game.CharX, this.game.CharY) < 5f)
					{
						list.Add(gameObject);
					}
				}
				return list;
			}
		}

		// Token: 0x1700022A RID: 554
		// (get) Token: 0x06000959 RID: 2393 RVA: 0x0003E4E0 File Offset: 0x0003C6E0
		public List<GameObject> NearMonter15m
		{
			get
			{
				List<GameObject> list = new List<GameObject>();
				foreach (GameObject gameObject in this.Monter)
				{
					if (TINHKIEM.GetDistance(this.game.CharX, this.game.CharY, gameObject.X, gameObject.Y) < 15f)
					{
						list.Add(gameObject);
					}
				}
				return list;
			}
		}

		// Token: 0x1700022B RID: 555
		// (get) Token: 0x0600095A RID: 2394 RVA: 0x0003E568 File Offset: 0x0003C768
		public List<GameObject> NearMonter20m
		{
			get
			{
				List<GameObject> list = new List<GameObject>();
				foreach (GameObject gameObject in this.Monter)
				{
					if (TINHKIEM.GetDistance(this.game.CharX, this.game.CharY, gameObject.X, gameObject.Y) < 20f)
					{
						list.Add(gameObject);
					}
				}
				return list;
			}
		}

		// Token: 0x1700022C RID: 556
		// (get) Token: 0x0600095B RID: 2395 RVA: 0x0003E5F0 File Offset: 0x0003C7F0
		public List<GameObject> NearMonter18m
		{
			get
			{
				List<GameObject> list = new List<GameObject>();
				foreach (GameObject gameObject in this.Monter)
				{
					if (TINHKIEM.GetDistance(this.game.CharX, this.game.CharY, gameObject.X, gameObject.Y) < 18f)
					{
						list.Add(gameObject);
					}
				}
				return list;
			}
		}

		// Token: 0x1700022D RID: 557
		// (get) Token: 0x0600095C RID: 2396 RVA: 0x0003E678 File Offset: 0x0003C878
		public GameObject NearestMonter
		{
			get
			{
				float num = 9999f;
				GameObject result = null;
				foreach (GameObject gameObject in this.Monter)
				{
					gameObject.DistanceEx = TINHKIEM.GetDistance(this.game.CharX, this.game.CharY, gameObject.X, gameObject.Y);
					if (gameObject.DistanceEx < num)
					{
						num = gameObject.DistanceEx;
						result = gameObject;
					}
				}
				return result;
			}
		}

		// Token: 0x1700022E RID: 558
		// (get) Token: 0x0600095D RID: 2397 RVA: 0x0003E70C File Offset: 0x0003C90C
		public List<GameObject> NearMonter9m
		{
			get
			{
				List<GameObject> list = new List<GameObject>();
				foreach (GameObject gameObject in this.Monter)
				{
					if (TINHKIEM.GetDistance(this.game.CharX, this.game.CharY, gameObject.X, gameObject.Y) < 10f)
					{
						list.Add(gameObject);
					}
				}
				return list;
			}
		}

		// Token: 0x1700022F RID: 559
		// (get) Token: 0x0600095E RID: 2398 RVA: 0x0003E794 File Offset: 0x0003C994
		public List<GameObject> NearMonter12m
		{
			get
			{
				List<GameObject> list = new List<GameObject>();
				foreach (GameObject gameObject in this.Monter)
				{
					if (TINHKIEM.GetDistance(this.game.CharX, this.game.CharY, gameObject.X, gameObject.Y) < 12f)
					{
						list.Add(gameObject);
					}
				}
				return list;
			}
		}

		// Token: 0x17000230 RID: 560
		// (get) Token: 0x0600095F RID: 2399 RVA: 0x0003E81C File Offset: 0x0003CA1C
		public List<GameObject> Near5m
		{
			get
			{
				List<GameObject> list = new List<GameObject>();
				foreach (GameObject gameObject in this.All)
				{
					if (TINHKIEM.GetDistance(this.game.CharX, this.game.CharY, gameObject.X, gameObject.Y) < 5f)
					{
						list.Add(gameObject);
					}
				}
				return list;
			}
		}

		// Token: 0x17000231 RID: 561
		// (get) Token: 0x06000960 RID: 2400 RVA: 0x0003E8A4 File Offset: 0x0003CAA4
		public List<GameObject> Near20m
		{
			get
			{
				List<GameObject> list = new List<GameObject>();
				foreach (GameObject gameObject in this.All)
				{
					if (TINHKIEM.GetDistance(this.game.CharX, this.game.CharY, gameObject.X, gameObject.Y) < 20f)
					{
						list.Add(gameObject);
					}
				}
				return list;
			}
		}

		// Token: 0x17000232 RID: 562
		// (get) Token: 0x06000961 RID: 2401 RVA: 0x0003E92C File Offset: 0x0003CB2C
		public bool TargetIsMine
		{
			get
			{
				return this.Target != null && this.MineId.Contains(this.Target.Belong);
			}
		}

		// Token: 0x06000962 RID: 2402 RVA: 0x0003E950 File Offset: 0x0003CB50
		public GameObjects(Game game)
		{
			this.game = game;
			this.Read();
		}

		// Token: 0x06000963 RID: 2403 RVA: 0x0003E9E0 File Offset: 0x0003CBE0
		public void Read()
		{
			this.Self = (this.Key = (this.Target = (this.PartyMinHP = null)));
			this.All.Clear();
			this.LootPacket.Clear();
			this.Party.Clear();
			this.Monter.Clear();
			this.MyMonter.Clear();
			this.UnBelongMonter.Clear();
			this.TaiNguyen.Clear();
			this.ToaDoTaiNguyen = "";
			foreach (int address in this.EnumGameObject(this.game.Address.FirstObject))
			{
				GameObject gameObject = new GameObject(this.game, address);
				if (gameObject.Name != null && (int)gameObject.X != 0 && (int)gameObject.Y != 0)
				{
					if (gameObject.IsPlayer)
					{
						foreach (string text in Setting.Leader.Split(new char[]
						{
							'\n'
						}))
						{
							if (!(text.Trim() == "") && TINHKIEM.VietLien(text) == TINHKIEM.VietLien(gameObject.Name) && (this.Self == null || gameObject != this.Self))
							{
								this.Key = gameObject;
								break;
							}
						}
					}
					this.All.Add(gameObject);
					if (gameObject.Id == this.game.TargetId)
					{
						this.Target = gameObject;
					}
					if (gameObject.IsLootPacket)
					{
						this.LootPacket.Add(gameObject);
					}
					if (gameObject.IsTaiNguyen)
					{
						this.TaiNguyen.Add(gameObject);
						this.ToaDoTaiNguyen = string.Concat(new object[]
						{
							this.ToaDoTaiNguyen,
							gameObject.X,
							",",
							gameObject.Y,
							",",
							gameObject.Belong,
							"-"
						});
					}
					if (gameObject.TrueId == this.game.TLBB.Id && this.game.TLBB.Online)
					{
						this.Self = gameObject;
						this.game.CharX = this.Self.X;
						this.game.CharY = this.Self.Y;
						this.game.RoundX = this.Self.RoundX;
						this.game.RoundY = this.Self.RoundY;
						this.game.IsRide = (this.Self.Ride != -1);
					}
					if (this.game.TLBB.Lvl >= 10 || !(gameObject.Title.Trim() != ""))
					{
						if (gameObject.IsMonter && !gameObject.IsBoQua)
						{
							this.Monter.Add(gameObject);
						}
						if (gameObject.Belong.Contains("FFFFFFFF") && gameObject.IsMonter)
						{
							this.UnBelongMonter.Add(gameObject);
						}
					}
				}
			}
			this.MineId = "0000000000000000FFFFFFFFFFFFFFFF";
			if (this.Self != null)
			{
				this.Party.Add(this.Self);
				this.MineId += this.Self.TrueId;
				this.PartyMinHP = this.Self;
				foreach (GameObject gameObject2 in this.All)
				{
					if (gameObject2 != this.Self)
					{
						if (this.game.Enemy.Contains("-" + gameObject2.Name + "-") && Global.AutoPk)
						{
							this.Pk.Add(gameObject2);
						}
						if ((double)this.Self.HP > 0.3 && this.Self.QDId != 65535 && this.game.Address.GameType == 1 && Global.BuffQuanDoan)
						{
							if (gameObject2.HP > 0f && this.Self.QDId == gameObject2.QDId && gameObject2.IsPlayer)
							{
								this.Party.Add(gameObject2);
								this.MineId += gameObject2.TrueId;
								if ((double)this.Self.HP > 0.3 && gameObject2.HP < this.PartyMinHP.HP)
								{
									this.PartyMinHP = gameObject2;
								}
							}
						}
						else if (this.Self.PartyId != -1 && gameObject2.PartyId == this.Self.PartyId)
						{
							if (gameObject2.HP > 0f)
							{
								this.Party.Add(gameObject2);
								this.MineId += gameObject2.TrueId;
								if ((double)this.Self.HP > 0.3 && gameObject2.HP < this.PartyMinHP.HP)
								{
									this.PartyMinHP = gameObject2;
								}
								if (gameObject2.TrueId == this.game.TLBB.KeyId && this.Key == null)
								{
									this.Key = gameObject2;
								}
							}
						}
						else if (Setting.CheckBuff(gameObject2.TrueId.ToLower()) && gameObject2.Name != "" && gameObject2.IsPlayer && gameObject2.HP > 0f)
						{
							this.Party.Add(gameObject2);
							if ((double)this.Self.HP > 0.3 && gameObject2.HP < this.PartyMinHP.HP)
							{
								this.PartyMinHP = gameObject2;
							}
						}
						if (Global.BuffPet && gameObject2.IsPet && gameObject2.Name == this.game.TLBB.PetName && gameObject2.HP > 0f)
						{
							this.Party.Add(gameObject2);
							if ((double)this.Self.HP > 0.3 && gameObject2.HP < this.PartyMinHP.HP)
							{
								this.PartyMinHP = gameObject2;
							}
						}
						if (this.game.TLBB.MapId == MAP.YenTuO && (gameObject2.CleanName == "hodienbao" || gameObject2.CleanName == "tienhoanhvu") && (double)gameObject2.HP < 0.5)
						{
							this.PartyMinHP = gameObject2;
						}
						if (this.game.TLBB.MapId == MAP.ThanhThuSonPhuBan && TINHKIEM.VietLien(gameObject2.Title) == "linhthu" && (double)gameObject2.HP < 0.5)
						{
							this.PartyMinHP = gameObject2;
						}
					}
				}
			}
			foreach (GameObject gameObject3 in this.Monter)
			{
				if (this.MineId.Contains(gameObject3.Belong) && gameObject3.IsMonter)
				{
					this.MyMonter.Add(gameObject3);
				}
			}
		}

		// Token: 0x06000964 RID: 2404 RVA: 0x0003F1D8 File Offset: 0x0003D3D8
		public HashSet<int> EnumGameObject(int address)
		{
			HashSet<int> hashSet = new HashSet<int>();
			this.NextGameObject(address, hashSet);
			return hashSet;
		}

		// Token: 0x06000965 RID: 2405 RVA: 0x0003F1F4 File Offset: 0x0003D3F4
		public HashSet<int> EnumGameObject(int[] addresses)
		{
			int num = this.game.Memory.Read(addresses);
			if (num > 0)
			{
				return this.EnumGameObject(num);
			}
			return new HashSet<int>();
		}

		// Token: 0x06000966 RID: 2406 RVA: 0x0003F224 File Offset: 0x0003D424
		private void NextGameObject(int address, HashSet<int> hash)
		{
			if (address <= 0)
			{
				return;
			}
			if (hash.Count > 10000)
			{
				return;
			}
			if (!hash.Contains(address))
			{
				hash.Add(address);
				this.NextGameObject(this.game.Memory.Read(address), hash);
				this.NextGameObject(this.game.Memory.Read(address + 8), hash);
			}
		}

		// Token: 0x04000646 RID: 1606
		private Game game;

		// Token: 0x04000647 RID: 1607
		public List<GameObject> TaiNguyen = new List<GameObject>();

		// Token: 0x04000648 RID: 1608
		public List<GameObject> All = new List<GameObject>();

		// Token: 0x04000649 RID: 1609
		public List<GameObject> LootPacket = new List<GameObject>();

		// Token: 0x0400064A RID: 1610
		public List<GameObject> Party = new List<GameObject>();

		// Token: 0x0400064B RID: 1611
		public List<GameObject> Monter = new List<GameObject>();

		// Token: 0x0400064C RID: 1612
		public List<GameObject> MyMonter = new List<GameObject>();

		// Token: 0x0400064D RID: 1613
		public List<GameObject> UnBelongMonter = new List<GameObject>();

		// Token: 0x0400064E RID: 1614
		public List<GameObject> Pk = new List<GameObject>();

		// Token: 0x0400064F RID: 1615
		public GameObject Self;

		// Token: 0x04000650 RID: 1616
		public GameObject Key;

		// Token: 0x04000651 RID: 1617
		public GameObject Target;

		// Token: 0x04000652 RID: 1618
		public GameObject PartyMinHP;

		// Token: 0x04000653 RID: 1619
		public string ToaDoTaiNguyen = "";

		// Token: 0x04000654 RID: 1620
		private string MineId = "";
	}
}
