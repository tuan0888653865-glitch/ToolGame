using System;
using System.Collections.Generic;

namespace TinhKiemAuto
{
	// Token: 0x02000095 RID: 149
	public class GameObject
	{
		// Token: 0x170001FB RID: 507
		// (get) Token: 0x06000904 RID: 2308 RVA: 0x0003D5C3 File Offset: 0x0003B7C3
		// (set) Token: 0x06000905 RID: 2309 RVA: 0x0003D5CB File Offset: 0x0003B7CB
		private Game Game { get; set; }

		// Token: 0x170001FC RID: 508
		// (get) Token: 0x06000906 RID: 2310 RVA: 0x0003D5D4 File Offset: 0x0003B7D4
		// (set) Token: 0x06000907 RID: 2311 RVA: 0x0003D5DC File Offset: 0x0003B7DC
		private Memory Memory { get; set; }

		// Token: 0x170001FD RID: 509
		// (get) Token: 0x06000908 RID: 2312 RVA: 0x0003D5E5 File Offset: 0x0003B7E5
		// (set) Token: 0x06000909 RID: 2313 RVA: 0x0003D5ED File Offset: 0x0003B7ED
		private Address ADD { get; set; }

		// Token: 0x170001FE RID: 510
		// (get) Token: 0x0600090A RID: 2314 RVA: 0x0003D5F6 File Offset: 0x0003B7F6
		// (set) Token: 0x0600090B RID: 2315 RVA: 0x0003D5FE File Offset: 0x0003B7FE
		public int Address { get; set; }

		// Token: 0x170001FF RID: 511
		// (get) Token: 0x0600090C RID: 2316 RVA: 0x0003D607 File Offset: 0x0003B807
		// (set) Token: 0x0600090D RID: 2317 RVA: 0x0003D60F File Offset: 0x0003B80F
		public int Id { get; set; }

		// Token: 0x17000200 RID: 512
		// (get) Token: 0x0600090E RID: 2318 RVA: 0x0003D618 File Offset: 0x0003B818
		// (set) Token: 0x0600090F RID: 2319 RVA: 0x0003D620 File Offset: 0x0003B820
		public int Object { get; set; }

		// Token: 0x17000201 RID: 513
		// (get) Token: 0x06000910 RID: 2320 RVA: 0x0003D629 File Offset: 0x0003B829
		// (set) Token: 0x06000911 RID: 2321 RVA: 0x0003D631 File Offset: 0x0003B831
		public int Class { get; set; }

		// Token: 0x17000202 RID: 514
		// (get) Token: 0x06000912 RID: 2322 RVA: 0x0003D63A File Offset: 0x0003B83A
		// (set) Token: 0x06000913 RID: 2323 RVA: 0x0003D642 File Offset: 0x0003B842
		public float X { get; set; }

		// Token: 0x17000203 RID: 515
		// (get) Token: 0x06000914 RID: 2324 RVA: 0x0003D64B File Offset: 0x0003B84B
		// (set) Token: 0x06000915 RID: 2325 RVA: 0x0003D653 File Offset: 0x0003B853
		public float Y { get; set; }

		// Token: 0x17000204 RID: 516
		// (get) Token: 0x06000916 RID: 2326 RVA: 0x0003D65C File Offset: 0x0003B85C
		public List<int> Buff
		{
			get
			{
				if (this.buff == null)
				{
					this.buff = new List<int>();
					foreach (int num in this.BuffAddress)
					{
						this.buff.Add(this.Game.Memory.Read(num + 12));
					}
				}
				return this.buff;
			}
		}

		// Token: 0x17000205 RID: 517
		// (get) Token: 0x06000917 RID: 2327 RVA: 0x0003D6E0 File Offset: 0x0003B8E0
		// (set) Token: 0x06000918 RID: 2328 RVA: 0x0003D6E8 File Offset: 0x0003B8E8
		public int State { get; set; }

		// Token: 0x17000206 RID: 518
		// (get) Token: 0x06000919 RID: 2329 RVA: 0x0003D6F1 File Offset: 0x0003B8F1
		// (set) Token: 0x0600091A RID: 2330 RVA: 0x0003D6F9 File Offset: 0x0003B8F9
		public int AtkToId { get; set; }

		// Token: 0x17000207 RID: 519
		// (get) Token: 0x0600091B RID: 2331 RVA: 0x0003D702 File Offset: 0x0003B902
		// (set) Token: 0x0600091C RID: 2332 RVA: 0x0003D70A File Offset: 0x0003B90A
		public int AtkById { get; set; }

		// Token: 0x17000208 RID: 520
		// (get) Token: 0x0600091D RID: 2333 RVA: 0x0003D713 File Offset: 0x0003B913
		// (set) Token: 0x0600091E RID: 2334 RVA: 0x0003D71B File Offset: 0x0003B91B
		public int InfoAddress { get; set; }

		// Token: 0x17000209 RID: 521
		// (get) Token: 0x0600091F RID: 2335 RVA: 0x0003D724 File Offset: 0x0003B924
		// (set) Token: 0x06000920 RID: 2336 RVA: 0x0003D72C File Offset: 0x0003B92C
		public float HP { get; set; }

		// Token: 0x1700020A RID: 522
		// (get) Token: 0x06000921 RID: 2337 RVA: 0x0003D735 File Offset: 0x0003B935
		// (set) Token: 0x06000922 RID: 2338 RVA: 0x0003D73D File Offset: 0x0003B93D
		public float MP { get; set; }

		// Token: 0x1700020B RID: 523
		// (get) Token: 0x06000923 RID: 2339 RVA: 0x0003D746 File Offset: 0x0003B946
		// (set) Token: 0x06000924 RID: 2340 RVA: 0x0003D74E File Offset: 0x0003B94E
		public string TrueId { get; set; }

		// Token: 0x1700020C RID: 524
		// (get) Token: 0x06000925 RID: 2341 RVA: 0x0003D757 File Offset: 0x0003B957
		// (set) Token: 0x06000926 RID: 2342 RVA: 0x0003D75F File Offset: 0x0003B95F
		public string Belong { get; set; }

		// Token: 0x1700020D RID: 525
		// (get) Token: 0x06000927 RID: 2343 RVA: 0x0003D768 File Offset: 0x0003B968
		// (set) Token: 0x06000928 RID: 2344 RVA: 0x0003D770 File Offset: 0x0003B970
		public string Name { get; set; }

		// Token: 0x1700020E RID: 526
		// (get) Token: 0x06000929 RID: 2345 RVA: 0x0003D779 File Offset: 0x0003B979
		public string CleanName
		{
			get
			{
				return TINHKIEM.VietLien(this.Name);
			}
		}

		// Token: 0x1700020F RID: 527
		// (get) Token: 0x0600092A RID: 2346 RVA: 0x0003D786 File Offset: 0x0003B986
		// (set) Token: 0x0600092B RID: 2347 RVA: 0x0003D78E File Offset: 0x0003B98E
		public int Menpai { get; set; }

		// Token: 0x17000210 RID: 528
		// (get) Token: 0x0600092C RID: 2348 RVA: 0x0003D797 File Offset: 0x0003B997
		// (set) Token: 0x0600092D RID: 2349 RVA: 0x0003D79F File Offset: 0x0003B99F
		public string Type { get; set; }

		// Token: 0x17000211 RID: 529
		// (get) Token: 0x0600092E RID: 2350 RVA: 0x0003D7A8 File Offset: 0x0003B9A8
		// (set) Token: 0x0600092F RID: 2351 RVA: 0x0003D7B0 File Offset: 0x0003B9B0
		public int Lvl { get; set; }

		// Token: 0x17000212 RID: 530
		// (get) Token: 0x06000930 RID: 2352 RVA: 0x0003D7B9 File Offset: 0x0003B9B9
		// (set) Token: 0x06000931 RID: 2353 RVA: 0x0003D7C1 File Offset: 0x0003B9C1
		public int PartyId { get; set; }

		// Token: 0x17000213 RID: 531
		// (get) Token: 0x06000932 RID: 2354 RVA: 0x0003D7CA File Offset: 0x0003B9CA
		// (set) Token: 0x06000933 RID: 2355 RVA: 0x0003D7D2 File Offset: 0x0003B9D2
		public string Title { get; set; }

		// Token: 0x17000214 RID: 532
		// (get) Token: 0x06000934 RID: 2356 RVA: 0x0003D7DB File Offset: 0x0003B9DB
		// (set) Token: 0x06000935 RID: 2357 RVA: 0x0003D7E3 File Offset: 0x0003B9E3
		public int Ride { get; set; }

		// Token: 0x17000215 RID: 533
		// (get) Token: 0x06000936 RID: 2358 RVA: 0x0003D7EC File Offset: 0x0003B9EC
		// (set) Token: 0x06000937 RID: 2359 RVA: 0x0003D7F4 File Offset: 0x0003B9F4
		public int QDId { get; set; }

		// Token: 0x17000216 RID: 534
		// (get) Token: 0x06000938 RID: 2360 RVA: 0x0003D7FD File Offset: 0x0003B9FD
		public float Distance
		{
			get
			{
				return TINHKIEM.GetDistance(this.Game.CharX, this.Game.CharY, this.X, this.Y);
			}
		}

		// Token: 0x17000217 RID: 535
		// (get) Token: 0x06000939 RID: 2361 RVA: 0x0003D826 File Offset: 0x0003BA26
		// (set) Token: 0x0600093A RID: 2362 RVA: 0x0003D82E File Offset: 0x0003BA2E
		public float DistanceEx { get; set; }

		// Token: 0x17000218 RID: 536
		// (get) Token: 0x0600093B RID: 2363 RVA: 0x0003D837 File Offset: 0x0003BA37
		// (set) Token: 0x0600093C RID: 2364 RVA: 0x0003D83F File Offset: 0x0003BA3F
		public int NameAddress { get; set; }

		// Token: 0x0600093D RID: 2365 RVA: 0x0003D848 File Offset: 0x0003BA48
		public void ChangeName(string name)
		{
			this.Game.Memory.WriteUnicodeString(name, this.NameAddress);
		}

		// Token: 0x17000219 RID: 537
		// (get) Token: 0x0600093E RID: 2366 RVA: 0x0003D862 File Offset: 0x0003BA62
		public int RoundX
		{
			get
			{
				return (int)Math.Round((double)this.X, 0, MidpointRounding.AwayFromZero);
			}
		}

		// Token: 0x1700021A RID: 538
		// (get) Token: 0x0600093F RID: 2367 RVA: 0x0003D873 File Offset: 0x0003BA73
		public int RoundY
		{
			get
			{
				return (int)Math.Round((double)this.Y, 0, MidpointRounding.AwayFromZero);
			}
		}

		// Token: 0x1700021B RID: 539
		// (get) Token: 0x06000940 RID: 2368 RVA: 0x0003D884 File Offset: 0x0003BA84
		public string BuffToString
		{
			get
			{
				string text = "";
				foreach (int num in this.Buff)
				{
					text = text + num.ToString() + ",";
				}
				return text.Trim(new char[]
				{
					','
				});
			}
		}

		// Token: 0x1700021C RID: 540
		// (get) Token: 0x06000941 RID: 2369 RVA: 0x0003D8FC File Offset: 0x0003BAFC
		public bool IsLootPacket
		{
			get
			{
				return this.Class == this.Game.Address.PacketClass;
			}
		}

		// Token: 0x1700021D RID: 541
		// (get) Token: 0x06000942 RID: 2370 RVA: 0x0003D916 File Offset: 0x0003BB16
		public bool IsTaiNguyen
		{
			get
			{
				return this.Class == this.Game.Address.TaiNguyenClass;
			}
		}

		// Token: 0x1700021E RID: 542
		// (get) Token: 0x06000943 RID: 2371 RVA: 0x0003D930 File Offset: 0x0003BB30
		public bool IsKhoang
		{
			get
			{
				return this.IsTaiNguyen && (!(Setting.KoKhaiThac != string.Empty) || !TINHKIEM.VietLien(Setting.KoKhaiThac).Contains(TINHKIEM.VietLien(this.Name))) && (TINHKIEM.IsKhoang(this.Name) != -1 || Game.Is69DO);
			}
		}

		// Token: 0x1700021F RID: 543
		// (get) Token: 0x06000944 RID: 2372 RVA: 0x0003D989 File Offset: 0x0003BB89
		public string MD
		{
			get
			{
				return TINHKIEM.Hasher.MD5(this.Name).Substring(0, 3);
			}
		}

		// Token: 0x17000220 RID: 544
		// (get) Token: 0x06000945 RID: 2373 RVA: 0x0003D9A0 File Offset: 0x0003BBA0
		public bool IsDuoc
		{
			get
			{
				return this.IsTaiNguyen && (!(Setting.KoKhaiThac != string.Empty) || !TINHKIEM.VietLien(Setting.KoKhaiThac).Contains(TINHKIEM.VietLien(this.Name))) && (TINHKIEM.IsDuoc(this.Name) != -1 || Game.Is69DO);
			}
		}

		// Token: 0x17000221 RID: 545
		// (get) Token: 0x06000946 RID: 2374 RVA: 0x0003D9FC File Offset: 0x0003BBFC
		public bool IsMonter
		{
			get
			{
				return this.Game.TLBB.MapId > 2 && this.HP > 0f && (this.Menpai < -1 || this.Menpai >= 16) && this.Menpai != 32 && this.Menpai != 21 && this.Menpai != 22 && this.Menpai != 19 && this.Menpai != 37 && this.Menpai <= 40 && !GAMEDIC.BoQua.Contains(this.Name) && (this.Game.TLBB.MapId != MAP.YenTuO || !GAMEDIC.YenTuOBoQua.Contains(this.Name)) && (!Global.IsBoQua || !(this.Name.Trim() != "")) && !this.Name.Contains("Tháp") && !TINHKIEM.VietLien(this.Name).Contains("tieulang") && (!this.Name.Contains("Niên Thú") || this.Game.TLBB.MapId == 547);
			}
		}

		// Token: 0x17000222 RID: 546
		// (get) Token: 0x06000947 RID: 2375 RVA: 0x0003DB45 File Offset: 0x0003BD45
		public bool IsBoQua
		{
			get
			{
				return TINHKIEM.VietLien(Setting.BoQua).Contains(TINHKIEM.VietLien(this.Name));
			}
		}

		// Token: 0x17000223 RID: 547
		// (get) Token: 0x06000948 RID: 2376 RVA: 0x0003DB61 File Offset: 0x0003BD61
		public bool IsPlayer
		{
			get
			{
				return (this.Menpai >= 1 && this.Menpai <= 9) || this.Menpai == 32 || this.Menpai == 37;
			}
		}

		// Token: 0x17000224 RID: 548
		// (get) Token: 0x06000949 RID: 2377 RVA: 0x0003DB8E File Offset: 0x0003BD8E
		public bool IsPet
		{
			get
			{
				return this.Type == "40600000" && this.TrueId.Contains("FFFFFFFF");
			}
		}

		// Token: 0x17000225 RID: 549
		// (get) Token: 0x0600094A RID: 2378 RVA: 0x0003DBB4 File Offset: 0x0003BDB4
		public bool IsNPC
		{
			get
			{
				return this.Type == "3FE66666" || this.Type == "3F4CCCCD";
			}
		}

		// Token: 0x17000226 RID: 550
		// (get) Token: 0x0600094B RID: 2379 RVA: 0x0003DBDA File Offset: 0x0003BDDA
		public HashSet<int> BuffAddress
		{
			get
			{
				return this.EnumGameBuff(this.Memory.Read(this.Object + this.Game.Address.ObjectBuff));
			}
		}

		// Token: 0x0600094C RID: 2380 RVA: 0x0003DC04 File Offset: 0x0003BE04
		public GameObject(Game game, int address)
		{
			this.Game = game;
			this.Memory = game.Memory;
			this.ADD = game.Address;
			this.Address = address;
			this.InfoAddress = game.Memory.Read(address + this.ADD.ObjectObject, this.ADD.ObjectInfo);
			this.Id = this.Memory.Read(address + this.ADD.ObjectId);
			this.Object = this.Memory.Read(address + this.ADD.ObjectObject);
			this.Class = this.Memory.Read(this.Object);
			this.X = this.Memory.ReadFloat(this.Object + this.ADD.ObjectX);
			this.Y = this.Memory.ReadFloat(this.Object + this.ADD.ObjectY);
			if (this.Id >= 0 && (int)this.X >= 0 && (int)this.Y >= 0)
			{
				if (this.ADD.GameType == 2)
				{
					this.State = this.Memory.Read(this.Object + 344);
				}
				else
				{
					this.State = this.Memory.Read(this.Object + this.ADD.State);
				}
				this.AtkToId = this.Memory.Read(this.Object + this.ADD.ObjectAtkToId);
				this.AtkById = this.Memory.Read(this.Object + this.ADD.ObjectAtkById);
				byte[] array = new byte[this.ADD.ObjectPartyId + 4];
				Memory.ReadProcessMemory(game.Memory.Id, this.InfoAddress, array, array.Length, 0);
				this.HP = BitConverter.ToSingle(array, this.ADD.ObjectHP);
				this.MP = BitConverter.ToSingle(array, this.ADD.ObjectMP);
				if (this.ADD.GameType == 1)
				{
					this.TrueId = BitConverter.ToInt64(array, this.ADD.ObjectTrueId).ToString("X8");
				}
				else
				{
					this.TrueId = BitConverter.ToInt32(array, this.ADD.ObjectTrueId).ToString("X8");
				}
				if (this.ADD.GameType == 1)
				{
					this.Belong = BitConverter.ToInt64(array, this.ADD.ObjectBelong).ToString("X8");
				}
				else
				{
					this.Belong = BitConverter.ToInt32(array, this.ADD.ObjectBelong).ToString("X8");
				}
				this.Menpai = BitConverter.ToInt32(array, this.ADD.ObjectMenpai);
				this.Type = BitConverter.ToInt32(array, this.ADD.ObjectType).ToString("X8");
				this.Lvl = BitConverter.ToInt32(array, this.ADD.ObjectLvl);
				this.PartyId = BitConverter.ToInt32(array, this.ADD.ObjectPartyId);
				this.Title = this.Memory._ReadString(this.InfoAddress + this.ADD.ObjectTitle).Trim();
				this.Ride = BitConverter.ToInt32(array, this.ADD.ObjectRide);
				if (this.IsTaiNguyen)
				{
					Memory memory = game.Memory;
					int[] array2 = new int[3];
					array2[0] = this.Object + this.ADD.ObjectTaiNguyenName;
					array2[1] = 4;
					this.Name = memory.ReadString(array2).Trim();
				}
				else
				{
					this.Name = this.Memory._ReadString(this.InfoAddress + this.ADD.ObjectName).Trim();
				}
				this.QDId = this.Memory.Read2Byte(this.InfoAddress + 9852);
			}
		}

		// Token: 0x0600094D RID: 2381 RVA: 0x0003DFFC File Offset: 0x0003C1FC
		public float GetDistance(float x, float y)
		{
			return (float)Math.Sqrt(Math.Pow((double)(x - this.X), 2.0) + Math.Pow((double)(y - this.Y), 2.0));
		}

		// Token: 0x0600094E RID: 2382 RVA: 0x0003E034 File Offset: 0x0003C234
		public HashSet<int> EnumGameBuff(int address)
		{
			HashSet<int> hashSet = new HashSet<int>();
			this.NextGameBuff(address, hashSet);
			hashSet.Remove(address);
			return hashSet;
		}

		// Token: 0x0600094F RID: 2383 RVA: 0x0003E058 File Offset: 0x0003C258
		public HashSet<int> EnumGameBuff(int[] addresses)
		{
			int num = this.Game.Memory.Read(addresses);
			if (num > 0)
			{
				return this.EnumGameBuff(num);
			}
			return new HashSet<int>();
		}

		// Token: 0x06000950 RID: 2384 RVA: 0x0003E088 File Offset: 0x0003C288
		private void NextGameBuff(int address, HashSet<int> hash)
		{
			if (address <= 0)
			{
				return;
			}
			if (hash.Count > 200)
			{
				return;
			}
			if (!hash.Contains(address))
			{
				hash.Add(address);
				this.NextGameBuff(this.Game.Memory.Read(address), hash);
				this.NextGameBuff(this.Game.Memory.Read(address + 4), hash);
				this.NextGameBuff(this.Game.Memory.Read(address + 8), hash);
			}
		}

		// Token: 0x04000645 RID: 1605
		public List<int> buff;
	}
}
