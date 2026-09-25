using System;
using System.Collections.Generic;

namespace TinhKiemAuto
{
	// Token: 0x020000B6 RID: 182
	internal class MAP
	{
		// Token: 0x06000A0C RID: 2572 RVA: 0x00041A49 File Offset: 0x0003FC49
		public static bool IsPhuBan(int MapID)
		{
			return MAP.DanhSachPHUBAN.Contains(MapID);
		}

		// Token: 0x0400074A RID: 1866
		public static int LacDuong = 0;

		// Token: 0x0400074B RID: 1867
		public static int ToChau = 1;

		// Token: 0x0400074C RID: 1868
		public static int DaiLy = 2;

		// Token: 0x0400074D RID: 1869
		public static int GiamNguc = 194;

		// Token: 0x0400074E RID: 1870
		public static int DiaPhu = 77;

		// Token: 0x0400074F RID: 1871
		public static int DiaPhuDaiTheGioi = 578;

		// Token: 0x04000750 RID: 1872
		public static int TungSon = 3;

		// Token: 0x04000751 RID: 1873
		public static int ThaiHo = 4;

		// Token: 0x04000752 RID: 1874
		public static int KinhHo = 5;

		// Token: 0x04000753 RID: 1875
		public static int VoLuongSon = 6;

		// Token: 0x04000754 RID: 1876
		public static int KiemCac = 7;

		// Token: 0x04000755 RID: 1877
		public static int DonHoang = 8;

		// Token: 0x04000756 RID: 1878
		public static int ThieuLam = 9;

		// Token: 0x04000757 RID: 1879
		public static int MinhGiao = 11;

		// Token: 0x04000758 RID: 1880
		public static int CaiBang = 10;

		// Token: 0x04000759 RID: 1881
		public static int VoDang = 12;

		// Token: 0x0400075A RID: 1882
		public static int NgaMy = 15;

		// Token: 0x0400075B RID: 1883
		public static int TinhTuc = 16;

		// Token: 0x0400075C RID: 1884
		public static int ThienLong = 13;

		// Token: 0x0400075D RID: 1885
		public static int ThienSon = 17;

		// Token: 0x0400075E RID: 1886
		public static int TieuDao = 14;

		// Token: 0x0400075F RID: 1887
		public static int MoDung = 284;

		// Token: 0x04000760 RID: 1888
		public static int DuongMon = 615;

		// Token: 0x04000761 RID: 1889
		public static int CongDia = 153;

		// Token: 0x04000762 RID: 1890
		public static int ThieuLamPhuBan = 182;

		// Token: 0x04000763 RID: 1891
		public static int CaiBangPhuBan = 183;

		// Token: 0x04000764 RID: 1892
		public static int MinhGiaoPhuBan = 184;

		// Token: 0x04000765 RID: 1893
		public static int VoDangPhuBan = 185;

		// Token: 0x04000766 RID: 1894
		public static int ThienLongPhuBan = 186;

		// Token: 0x04000767 RID: 1895
		public static int TieuDaoPhuBan = 187;

		// Token: 0x04000768 RID: 1896
		public static int NgaMyPhuBan = 188;

		// Token: 0x04000769 RID: 1897
		public static int TinhTucPhuBan = 189;

		// Token: 0x0400076A RID: 1898
		public static int ThienSonPhuBan = 190;

		// Token: 0x0400076B RID: 1899
		public static int MoDungPhuBan = 289;

		// Token: 0x0400076C RID: 1900
		public static int DuongMonPhuBan = 616;

		// Token: 0x0400076D RID: 1901
		public static int LongTuyen = 31;

		// Token: 0x0400076E RID: 1902
		public static int ThuongSon = 25;

		// Token: 0x0400076F RID: 1903
		public static int ThachLam = 26;

		// Token: 0x04000770 RID: 1904
		public static int CaoXuong = 245;

		// Token: 0x04000771 RID: 1905
		public static int NhanBac = 19;

		// Token: 0x04000772 RID: 1906
		public static int ThaoNguyen = 20;

		// Token: 0x04000773 RID: 1907
		public static int NganNgaiTuyetNguyen = 229;

		// Token: 0x04000774 RID: 1908
		public static int VoDi = 32;

		// Token: 0x04000775 RID: 1909
		public static int TayHo = 30;

		// Token: 0x04000776 RID: 1910
		public static int NhiHai = 24;

		// Token: 0x04000777 RID: 1911
		public static int NhanNam = 18;

		// Token: 0x04000778 RID: 1912
		public static int ThieuLamAcBa = 173;

		// Token: 0x04000779 RID: 1913
		public static int NgaMyAcBa = 179;

		// Token: 0x0400077A RID: 1914
		public static int TieuDaoAcBa = 178;

		// Token: 0x0400077B RID: 1915
		public static int DuongMonAcBa = 618;

		// Token: 0x0400077C RID: 1916
		public static int MinhGiaoAcBa = 175;

		// Token: 0x0400077D RID: 1917
		public static int VoDangAcBa = 176;

		// Token: 0x0400077E RID: 1918
		public static int TinhTucAcBa = 180;

		// Token: 0x0400077F RID: 1919
		public static int ThienSonAcBa = 181;

		// Token: 0x04000780 RID: 1920
		public static int CaiBangAcBa = 174;

		// Token: 0x04000781 RID: 1921
		public static int ThienLongAcBa = 177;

		// Token: 0x04000782 RID: 1922
		public static int MoDungAcBa = 288;

		// Token: 0x04000783 RID: 1923
		public static int TacKhauDoanhDia = 170;

		// Token: 0x04000784 RID: 1924
		public static int TangKinhCac = 272;

		// Token: 0x04000785 RID: 1925
		public static int ThanhThuSon = 201;

		// Token: 0x04000786 RID: 1926
		public static int ThanhThuSonPhuBan = 232;

		// Token: 0x04000787 RID: 1927
		public static int LauLan = 246;

		// Token: 0x04000788 RID: 1928
		public static int HuyenVuDaoPhuBan = 268;

		// Token: 0x04000789 RID: 1929
		public static int PhungHoangCoThanh = 280;

		// Token: 0x0400078A RID: 1930
		public static int PhungHoangCoThanhPhuBan = 281;

		// Token: 0x0400078B RID: 1931
		public static int PhieuMieuPhong = 261;

		// Token: 0x0400078C RID: 1932
		public static int VanKiemCoc = 119;

		// Token: 0x0400078D RID: 1933
		public static int VanKiemCocDem = 118;

		// Token: 0x0400078E RID: 1934
		public static int LauLanBaoTang = 269;

		// Token: 0x0400078F RID: 1935
		public static int ViemMaSon = 651;

		// Token: 0x04000790 RID: 1936
		public static int TamTaiHiepCoc = 653;

		// Token: 0x04000791 RID: 1937
		public static int TranLongKyCuoc = 61;

		// Token: 0x04000792 RID: 1938
		public static int SinhTuLoiDai = 546;

		// Token: 0x04000793 RID: 1939
		public static int YenTuO = 236;

		// Token: 0x04000794 RID: 1940
		public static int PhungMinhVuongLang = 600;

		// Token: 0x04000795 RID: 1941
		public static int HuyetMo = 110;

		// Token: 0x04000796 RID: 1942
		public static int ThuyLao = 66;

		// Token: 0x04000797 RID: 1943
		public static int ThieuThatSon = 566;

		// Token: 0x04000798 RID: 1944
		public static int HuyenVuDao = 112;

		// Token: 0x04000799 RID: 1945
		public static int BinhThanhKyTran = 294;

		// Token: 0x0400079A RID: 1946
		public static int QuynhChau = 35;

		// Token: 0x0400079B RID: 1947
		public static int NamVuc = 34;

		// Token: 0x0400079C RID: 1948
		public static int MieuCuong = 29;

		// Token: 0x0400079D RID: 1949
		public static List<int> DanhSachPHUBAN = new List<int>
		{
			36,
			37,
			38,
			42,
			47,
			61,
			66,
			78,
			79,
			80,
			81,
			102,
			103,
			104,
			105,
			106,
			107,
			108,
			110,
			111,
			109,
			124,
			125,
			126,
			127,
			128,
			113,
			114,
			115,
			116,
			118,
			120,
			121,
			119,
			117,
			133,
			134,
			135,
			136,
			137,
			138,
			139,
			140,
			141,
			142,
			143,
			144,
			145,
			146,
			147,
			148,
			149,
			150,
			151,
			152,
			153,
			154,
			155,
			156,
			157,
			158,
			159,
			160,
			161,
			162,
			163,
			167,
			170,
			173,
			174,
			175,
			176,
			177,
			178,
			179,
			180,
			181,
			195,
			196,
			197,
			198,
			230,
			236,
			243,
			272,
			265,
			266,
			267,
			268,
			269,
			546,
			261,
			257,
			258,
			259,
			293,
			281,
			617,
			614,
			231,
			232,
			233,
			600,
			580,
			566,
			291,
			580,
			294
		};
	}
}
