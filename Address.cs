using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text.RegularExpressions;

namespace TinhKiemAuto
{
	// Token: 0x0200007D RID: 125
	public class Address
	{
		// Token: 0x060004CB RID: 1227 RVA: 0x0001B480 File Offset: 0x00019680
		public Address(string md5, string offsetStr)
		{
			int[] array = new int[3];
			array[1] = 12;
			this.ParaUseSkill = array;
			this.ParaMove = 8922880;
			this.SkillDelayBase = new int[2];
			this.SkillPetDelayBase = new int[]
			{
				8973056,
				18292
			};
			this.DisableActiveGame = 7037684;
			this.SkillArr = new int[]
			{
				9892704,
				112,
				480,
				4,
				9616,
				4
			};
			this.SkillClass = 7877144;
			this.OnlineTime = new int[2];
			this.ActionBase = new int[]
			{
				0,
				52
			};
			this.ActionAddress = 16;
			this.ActionID = 4;
			this.ActionName = 12;
			this.ActionType = 40;
			this.ActionPacketID = 92;
			this.SkillPetBase = new int[]
			{
				8973040,
				100
			};
			this.BankBase = new int[]
			{
				0,
				59540
			};
			this.delaySkillPetBase = new int[]
			{
				19715604,
				18732,
				12
			};
			this.PetBase = new int[2];
			this.IsCaptcha = new int[]
			{
				0,
				0,
				12,
				100
			};
			this.IsPK = new int[2];
			this.Disconnected = new int[2];
			this.KeyId = new int[2];
			this.MapId = new int[2];
			this.MapName = new int[3];
			this.FirstObject = new int[3];
			this.ParaSendKey = new int[]
			{
				0,
				64
			};
			this.DropBase = new int[4];
			this.IsSelectServer = new int[]
			{
				9728384,
				0,
				12,
				100
			};
			this.IsLoginMessage = new int[]
			{
				9728216,
				0,
				12,
				100
			};
			this.IsLogon = new int[]
			{
				9731936,
				0,
				12,
				100
			};
			this.IsTextCaptcha = new int[]
			{
				9741224,
				0,
				12,
				100
			};
			this.IsSelectCharacter = new int[]
			{
				9728072,
				0,
				12,
				100
			};
			this.FreshmanWatchTime = new int[]
			{
				828592,
				2016,
				1904,
				0,
				228,
				860
			};
			this.ParaLuaToString = 19911800;
			this.SafeTime = new int[]
			{
				19847460,
				807104
			};
			this.QuestInfo = new int[]
			{
				0,
				0,
				12,
				100
			};
			this.CountDown10Sec = new int[]
			{
				0,
				0,
				12,
				100
			};
			this.IsShopOpen = new int[]
			{
				0,
				0,
				12,
				100
			};
			this.HaveRide1 = new int[]
			{
				0,
				58676,
				32
			};
			int[] array2 = new int[3];
			array2[1] = 59628;
			this.HaveRide2 = array2;
			this.ODaoCu = new int[]
			{
				0,
				58676,
				36,
				8
			};
			this.ONguyenLieu = new int[]
			{
				0,
				58676,
				40,
				8
			};
			this.IsBankOpen = new int[]
			{
				0,
				0,
				12,
				100
			};
			this.X2 = new int[]
			{
				0,
				104,
				1736,
				716,
				232,
				860
			};
			this.IsRelive = new int[]
			{
				0,
				172,
				1780,
				292,
				1808,
				1976
			};
			try
			{
				this.GetOffset(md5, offsetStr);
			}
			catch
			{
			}
		}

		// Token: 0x060004CC RID: 1228 RVA: 0x0001B7E4 File Offset: 0x000199E4
		public static Address GetInstance(string md5, string offsetStr)
		{
			if (!Address.Dic.ContainsKey(md5))
			{
				Address address = new Address(md5, offsetStr);
				Address.Dic.Add(md5, address);
				return address;
			}
			return Address.Dic[md5];
		}

		// Token: 0x060004CD RID: 1229 RVA: 0x0001B820 File Offset: 0x00019A20
		private void GetOffset(string md5, string offsetStr)
		{
			if (offsetStr.Contains(md5))
			{
				this.offstr = offsetStr.Substring(offsetStr.IndexOf(md5));
			}
			else
			{
				if (offsetStr.Contains("FFFF00000000000000000000000000000000"))
				{
					this.offstr = offsetStr.Substring(offsetStr.IndexOf("FFFF00000000000000000000000000000000"));
				}
				this.offstr = offsetStr;
			}
			this.offstr = Regex.Replace(this.offstr, "@.*", "");
			this.NextOffset();
			this.GameType = this.offsets[0];
			this.NextOffset();
			this.CharBase = this.offsets;
			this.NextOffset();
			this.CharId = this.offsets[0];
			this.CharMenpaiPoint = this.offsets[1];
			this.CharName = this.offsets[2];
			this.CharMenpai = this.offsets[3];
			this.CharLvl = this.offsets[4];
			this.CharRage = this.offsets[5];
			this.CharGuildID = this.offsets[6];
			this.CharIsFollow = this.offsets[7];
			this.CharGuildName = this.offsets[8];
			this.CharCurPetId = this.offsets[9];
			this.CharCurHP = this.offsets[10];
			this.CharCurMP = this.offsets[11];
			this.CharExp = this.offsets[12];
			this.CharMaxHP = this.offsets[13];
			this.CharMaxMP = this.offsets[14];
			this.PetDataSize = this.offsets[15];
			this.PetId = this.offsets[16];
			this.PetCurHP = this.offsets[17];
			this.PetMaxHP = this.offsets[18];
			this.PetEnjoy = this.offsets[19];
			this.NextOffset();
			this.PetBase = this.offsets;
			this.NextOffset();
			this.CharState = this.offsets;
			this.NextOffset();
			this.IsCaptcha = this.offsets;
			this.NextOffset();
			this.IsPK = this.offsets;
			this.NextOffset();
			this.Disconnected = this.offsets;
			this.NextOffset();
			this.KeyId = this.offsets;
			this.NextOffset();
			this.Follow = this.offsets;
			this.NextOffset();
			this.IdFollow = this.offsets;
			this.NextOffset();
			this.MapId = this.offsets;
			this.X2[0] = this.MapId[0];
			this.IsRelive[0] = this.MapId[0] + 4;
			this.NextOffset();
			this.FakeMapId = this.offsets;
			this.NextOffset();
			this.MapName = this.offsets;
			this.NextOffset();
			this.FirstObject = this.offsets;
			this.NextOffset();
			this.ObjectId = this.offsets[0];
			this.ObjectObject = this.offsets[1];
			this.ObjectX = this.offsets[2];
			this.ObjectY = this.offsets[3];
			this.ObjectBuff = this.offsets[4];
			this.ObjectAtkToId = this.offsets[5];
			this.ObjectAtkById = this.offsets[6];
			this.ObjectTaiNguyenName = this.offsets[7];
			this.NextOffset();
			this.ObjectInfo = this.offsets;
			this.NextOffset();
			this.ObjectHP = this.offsets[0];
			this.ObjectMP = this.offsets[1];
			this.ObjectTrueId = this.offsets[2];
			this.ObjectBelong = this.offsets[3];
			this.ObjectName = this.offsets[4];
			this.ObjectMenpai = this.offsets[5];
			this.ObjectType = this.offsets[6];
			this.ObjectLvl = this.offsets[7];
			this.ObjectPartyId = this.offsets[8];
			this.ObjectTitle = this.offsets[9];
			this.ObjectRide = this.offsets[10];
			this.NextOffset();
			this.ActionBase = this.offsets;
			this.NextOffset();
			this.TaiNguyenClass = this.offsets[0];
			this.NextOffset();
			this.PacketClass = this.offsets[0];
			this.NextOffset();
			this.LootPacketItem = this.offsets;
			this.NextOffset();
			this.LootPacketId = this.offsets;
			this.NextOffset();
			this.SkillDelayBase = this.offsets;
			this.NextOffset();
			this.PacketItemBase = this.offsets;
			this.NextOffset();
			this.PacketType1 = this.offsets[0];
			this.NextOffset();
			this.PacketType2 = this.offsets[0];
			this.NextOffset();
			this.PacketType3 = this.offsets[0];
			this.NextOffset();
			this.PacketType4 = this.offsets[0];
			this.NextOffset();
			this.PacketType5 = this.offsets[0];
			this.NextOffset();
			this.PacketType6 = this.offsets[0];
			this.NextOffset();
			this.OnlineTime = this.offsets;
			this.NextOffset();
			this.DialogBase = this.offsets;
			this.NextOffset();
			this.KeySkillIdBase = this.offsets;
			this.NextOffset();
			try
			{
				this.LuyenKimBase = this.offsets[0];
				this.BienThan = this.offsets[1];
				this.LuyenKimOffset = this.offsets[2];
				this.LuyenKimX = this.offsets[3];
				if (Global.TimeLive == 0)
				{
					Global.TimeLive = this.LuyenKimX;
				}
				this.State = this.offsets[4];
			}
			catch
			{
			}
			this.NextOffset();
			this.TaskBase = this.offsets;
			this.NextOffset();
			this.TaskInfoBase = this.offsets;
			this.NextOffset();
			this.IsTogleMission = this.offsets;
			this.NextOffset();
			this.IsNexLogin = this.offsets;
			this.NextOffset();
			this.IsSelectServer = this.offsets;
			this.NextOffset();
			this.IsLoginMessage = this.offsets;
			this.NextOffset();
			this.IsLogon = this.offsets;
			this.NextOffset();
			this.IsTextCaptcha = this.offsets;
			this.NextOffset();
			this.IsSelectCharacter = this.offsets;
			this.NextOffset();
			this.Captcha = this.offsets;
			this.NextOffset();
			this.SafeTime = this.offsets;
			this.NextOffset();
			this.MultiAcc = this.offsets[0];
			this.NextOffset();
			this.DisableActiveGame = this.offsets[0];
			this.NextOffset();
			this.DisconnectAddress = this.offsets[0];
			this.NextOffset();
			this.FuncSendKey = this.offsets[0];
			this.NextOffset();
			this.ParaSendKey = this.offsets;
			this.NextOffset();
			this.FuncUseSkill = this.offsets[0];
			this.NextOffset();
			this.ParaUseSkill = this.offsets;
			this.NextOffset();
			this.FuncUseSkillPet = this.offsets[0];
			this.SkillPetBase[0] = this.ActionBase[0];
			this.ParaUseSkillPet = this.PetBase[0];
			this.NextOffset();
			this.FuncLuaDoString = this.offsets[0];
			this.NextOffset();
			this.ParaLuaDoString = this.offsets[0];
			this.NextOffset();
			this.ParaSelectTarget = (this.ParaTalk = this.offsets[0]);
			this.NextOffset();
			this.ParaPickItem = this.offsets[0];
			this.NextOffset();
			this.PickAll = this.offsets;
			this.NextOffset();
			this.FuncUpLvl = this.offsets[0];
			this.NextOffset();
			this.FuncSelectTargetOfTarget = this.offsets[0];
			this.NextOffset();
			this.DropBase = this.offsets;
			this.NextOffset();
			this.BaseShopItem = this.offsets;
			this.NextOffset();
			this.ParaCollectItem = this.offsets[0];
			this.NextOffset();
			this.ParaLuaToString = this.offsets[0];
			this.NextOffset();
			this.FuncSendPacket = this.offsets[0];
			this.NextOffset();
			this.ParaSendPacket = this.offsets[0];
			this.HaveRide1[0] = (this.HaveRide2[0] = this.PacketItemBase[0]);
			if (this.GameType != 1)
			{
				this.HaveRide1[1] = (this.ONguyenLieu[1] = (this.ODaoCu[1] = 840));
			}
			this.ODaoCu[0] = (this.ONguyenLieu[0] = this.PacketItemBase[0]);
			if (this.GameType == 1)
			{
				this.PetName = 36;
				this.PetLvl = 60;
			}
			else
			{
				this.PetName = 28;
				this.PetLvl = 52;
			}
			this.BankBase[0] = this.PacketItemBase[0];
			this.NextOffset();
			this.bakePacket = this.offsets[0].ToString("x4") + this.offsets[1].ToString("x4") + this.offsets[2].ToString("x4") + this.offsets[3].ToString("x4");
		}

		// Token: 0x060004CE RID: 1230 RVA: 0x0001C134 File Offset: 0x0001A334
		private void NextOffset()
		{
			this.offstr = this.offstr.Remove(0, this.offstr.IndexOf("00FF") + 4);
			string text = this.offstr.Substring(0, this.offstr.IndexOf("00FF"));
			int length = text.Length;
			if (length % 2 == 1)
			{
				this.offsets = new int[length / 6];
				this.ParseHex(text.Substring(0, 7), out this.offsets[0]);
				for (int i = 1; i < this.offsets.Length; i++)
				{
					this.ParseHex(text.Substring(i * 6 + 1, 6), out this.offsets[i]);
				}
				return;
			}
			this.offsets = new int[length / 4];
			this.ParseHex(text.Substring(0, 4), out this.offsets[0]);
			for (int j = 1; j < this.offsets.Length; j++)
			{
				this.ParseHex(text.Substring(j * 4, 4), out this.offsets[j]);
			}
		}

		// Token: 0x060004CF RID: 1231 RVA: 0x0001C240 File Offset: 0x0001A440
		private void ParseHex(string hex, out int result)
		{
			int.TryParse(hex, NumberStyles.HexNumber, CultureInfo.InvariantCulture, out result);
		}

		// Token: 0x04000329 RID: 809
		private string offstr;

		// Token: 0x0400032A RID: 810
		private int[] offsets;

		// Token: 0x0400032B RID: 811
		public static Dictionary<string, Address> Dic = new Dictionary<string, Address>();

		// Token: 0x0400032C RID: 812
		public int[] IsTogleMission;

		// Token: 0x0400032D RID: 813
		public int[] TaskInfoBase;

		// Token: 0x0400032E RID: 814
		public int[] CharBase = new int[]
		{
			0,
			0,
			0,
			4
		};

		// Token: 0x0400032F RID: 815
		public int CharId;

		// Token: 0x04000330 RID: 816
		public int CharMenpaiPoint;

		// Token: 0x04000331 RID: 817
		public int CharName;

		// Token: 0x04000332 RID: 818
		public int CharMenpai;

		// Token: 0x04000333 RID: 819
		public int CharLvl;

		// Token: 0x04000334 RID: 820
		public int CharRage;

		// Token: 0x04000335 RID: 821
		public int CharGuildID;

		// Token: 0x04000336 RID: 822
		public int CharIsFollow;

		// Token: 0x04000337 RID: 823
		public int CharCurPetId;

		// Token: 0x04000338 RID: 824
		public int CharCurHP;

		// Token: 0x04000339 RID: 825
		public int CharCurMP;

		// Token: 0x0400033A RID: 826
		public int CharExp;

		// Token: 0x0400033B RID: 827
		public int CharMaxHP;

		// Token: 0x0400033C RID: 828
		public int CharMaxMP;

		// Token: 0x0400033D RID: 829
		public int CharGuildName;

		// Token: 0x0400033E RID: 830
		public int PlayerGold = 1764;

		// Token: 0x0400033F RID: 831
		public int PetDataSize;

		// Token: 0x04000340 RID: 832
		public int PetId;

		// Token: 0x04000341 RID: 833
		public int PetCurHP;

		// Token: 0x04000342 RID: 834
		public int PetMaxHP;

		// Token: 0x04000343 RID: 835
		public int PetEnjoy;

		// Token: 0x04000344 RID: 836
		public int DisconnectAddress;

		// Token: 0x04000345 RID: 837
		public int ObjectBuff;

		// Token: 0x04000346 RID: 838
		public int ObjectAtkToId;

		// Token: 0x04000347 RID: 839
		public int ObjectAtkById;

		// Token: 0x04000348 RID: 840
		public int ObjectTaiNguyenName;

		// Token: 0x04000349 RID: 841
		public int ParaUseSkillPet;

		// Token: 0x0400034A RID: 842
		public int FuncUseSkillPet;

		// Token: 0x0400034B RID: 843
		public int FuncUseSkill;

		// Token: 0x0400034C RID: 844
		public int[] ParaUseSkill;

		// Token: 0x0400034D RID: 845
		public int ParaMove;

		// Token: 0x0400034E RID: 846
		public int[] SkillDelayBase;

		// Token: 0x0400034F RID: 847
		public int[] SkillPetDelayBase;

		// Token: 0x04000350 RID: 848
		public int DisableActiveGame;

		// Token: 0x04000351 RID: 849
		public int[] SkillArr;

		// Token: 0x04000352 RID: 850
		public int ParaTalk;

		// Token: 0x04000353 RID: 851
		public int SkillClass;

		// Token: 0x04000354 RID: 852
		public int TaiNguyenClass;

		// Token: 0x04000355 RID: 853
		public int[] OnlineTime;

		// Token: 0x04000356 RID: 854
		public int[] ActionBase;

		// Token: 0x04000357 RID: 855
		public int ActionAddress;

		// Token: 0x04000358 RID: 856
		public int ActionID;

		// Token: 0x04000359 RID: 857
		public int ActionName;

		// Token: 0x0400035A RID: 858
		public int ActionType;

		// Token: 0x0400035B RID: 859
		public int ActionPacketID;

		// Token: 0x0400035C RID: 860
		public int[] SkillPetBase;

		// Token: 0x0400035D RID: 861
		public int[] PacketItemBase;

		// Token: 0x0400035E RID: 862
		public int[] BankBase;

		// Token: 0x0400035F RID: 863
		public int[] BaseShopItem;

		// Token: 0x04000360 RID: 864
		public int[] delaySkillPetBase;

		// Token: 0x04000361 RID: 865
		public int[] PetBase;

		// Token: 0x04000362 RID: 866
		public int[] CharState;

		// Token: 0x04000363 RID: 867
		public int[] IsCaptcha;

		// Token: 0x04000364 RID: 868
		public int[] IsPK;

		// Token: 0x04000365 RID: 869
		public int[] Disconnected;

		// Token: 0x04000366 RID: 870
		public int[] KeyId;

		// Token: 0x04000367 RID: 871
		public int[] Follow;

		// Token: 0x04000368 RID: 872
		public int[] IdFollow;

		// Token: 0x04000369 RID: 873
		public int[] MapId;

		// Token: 0x0400036A RID: 874
		public int[] FakeMapId;

		// Token: 0x0400036B RID: 875
		public int[] MapName;

		// Token: 0x0400036C RID: 876
		public int[] FirstObject;

		// Token: 0x0400036D RID: 877
		public int ObjectId;

		// Token: 0x0400036E RID: 878
		public int ObjectObject;

		// Token: 0x0400036F RID: 879
		public int ObjectX;

		// Token: 0x04000370 RID: 880
		public int ObjectY;

		// Token: 0x04000371 RID: 881
		public int[] ObjectInfo;

		// Token: 0x04000372 RID: 882
		public int ObjectHP;

		// Token: 0x04000373 RID: 883
		public int ObjectMP;

		// Token: 0x04000374 RID: 884
		public int ObjectTrueId;

		// Token: 0x04000375 RID: 885
		public int ObjectBelong;

		// Token: 0x04000376 RID: 886
		public int ObjectName;

		// Token: 0x04000377 RID: 887
		public int ObjectTitle;

		// Token: 0x04000378 RID: 888
		public int ObjectMenpai;

		// Token: 0x04000379 RID: 889
		public int ObjectType;

		// Token: 0x0400037A RID: 890
		public int ObjectLvl;

		// Token: 0x0400037B RID: 891
		public int ObjectPartyId;

		// Token: 0x0400037C RID: 892
		public int ObjectRide;

		// Token: 0x0400037D RID: 893
		public int FuncSendKey;

		// Token: 0x0400037E RID: 894
		public int[] ParaSendKey;

		// Token: 0x0400037F RID: 895
		public int ParaSelectTarget;

		// Token: 0x04000380 RID: 896
		public int ParaPickItem;

		// Token: 0x04000381 RID: 897
		public int FuncSelectTargetOfTarget;

		// Token: 0x04000382 RID: 898
		public int[] LootPacketId;

		// Token: 0x04000383 RID: 899
		public int[] PickAll;

		// Token: 0x04000384 RID: 900
		public int PacketClass;

		// Token: 0x04000385 RID: 901
		public int FuncUpLvl;

		// Token: 0x04000386 RID: 902
		public int[] KeySkillIdBase;

		// Token: 0x04000387 RID: 903
		public int MultiAcc;

		// Token: 0x04000388 RID: 904
		public int ParaLuaDoString;

		// Token: 0x04000389 RID: 905
		public int FuncLuaDoString;

		// Token: 0x0400038A RID: 906
		public int[] LootPacketItem;

		// Token: 0x0400038B RID: 907
		public int GameType;

		// Token: 0x0400038C RID: 908
		public int[] TaskBase;

		// Token: 0x0400038D RID: 909
		public int LuyenKimBase;

		// Token: 0x0400038E RID: 910
		public int BienThan;

		// Token: 0x0400038F RID: 911
		public int LuyenKimOffset;

		// Token: 0x04000390 RID: 912
		public int LuyenKimX;

		// Token: 0x04000391 RID: 913
		public int State;

		// Token: 0x04000392 RID: 914
		public int PacketType1;

		// Token: 0x04000393 RID: 915
		public int PacketType2;

		// Token: 0x04000394 RID: 916
		public int PacketType3;

		// Token: 0x04000395 RID: 917
		public int PacketType4;

		// Token: 0x04000396 RID: 918
		public int PacketType5;

		// Token: 0x04000397 RID: 919
		public int PacketType6;

		// Token: 0x04000398 RID: 920
		public int[] DialogBase;

		// Token: 0x04000399 RID: 921
		public int[] DropBase;

		// Token: 0x0400039A RID: 922
		public int ParaCollectItem;

		// Token: 0x0400039B RID: 923
		public int[] IsSelectServer;

		// Token: 0x0400039C RID: 924
		public int[] IsLoginMessage;

		// Token: 0x0400039D RID: 925
		public int[] IsLogon;

		// Token: 0x0400039E RID: 926
		public int[] IsTextCaptcha;

		// Token: 0x0400039F RID: 927
		public int[] IsSelectCharacter;

		// Token: 0x040003A0 RID: 928
		public int[] FreshmanWatchTime;

		// Token: 0x040003A1 RID: 929
		public int ParaLuaToString;

		// Token: 0x040003A2 RID: 930
		public int[] SafeTime;

		// Token: 0x040003A3 RID: 931
		public int[] Captcha;

		// Token: 0x040003A4 RID: 932
		public int[] IsNexLogin;

		// Token: 0x040003A5 RID: 933
		public int[] QuestInfo;

		// Token: 0x040003A6 RID: 934
		public int[] CountDown10Sec;

		// Token: 0x040003A7 RID: 935
		public int[] IsShopOpen;

		// Token: 0x040003A8 RID: 936
		public int FuncSendPacket;

		// Token: 0x040003A9 RID: 937
		public int ParaSendPacket;

		// Token: 0x040003AA RID: 938
		public int ON_SCENE_TRANSING;

		// Token: 0x040003AB RID: 939
		public int[] HaveRide1;

		// Token: 0x040003AC RID: 940
		public int[] HaveRide2;

		// Token: 0x040003AD RID: 941
		public int[] ODaoCu;

		// Token: 0x040003AE RID: 942
		public int[] ONguyenLieu;

		// Token: 0x040003AF RID: 943
		public int PetName;

		// Token: 0x040003B0 RID: 944
		public int PetLvl;

		// Token: 0x040003B1 RID: 945
		public int[] IsBankOpen;

		// Token: 0x040003B2 RID: 946
		public int[] X2;

		// Token: 0x040003B3 RID: 947
		public int[] IsRelive;

		// Token: 0x040003B4 RID: 948
		public string bakePacket;
	}
}
