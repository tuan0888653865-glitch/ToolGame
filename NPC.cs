using System;

namespace TinhKiemAuto
{
	// Token: 0x020000C3 RID: 195
	public class NPC
	{
		// Token: 0x06000A9F RID: 2719 RVA: 0x000437D0 File Offset: 0x000419D0
		public NPC()
		{
			this.Id = -1;
		}

		// Token: 0x040007CB RID: 1995
		public int Id;

		// Token: 0x040007CC RID: 1996
		public int X;

		// Token: 0x040007CD RID: 1997
		public int Y;

		// Token: 0x040007CE RID: 1998
		public int Map;

		// Token: 0x040007CF RID: 1999
		public string INFOAIM = "";

		// Token: 0x040007D0 RID: 2000
		public string MD = "000";

		// Token: 0x040007D1 RID: 2001
		public static NPC MID = new NPC
		{
			Id = 192,
			X = 255,
			Y = 320,
			Map = LACDUONG.Id
		};

		// Token: 0x040007D2 RID: 2002
		public static NPC TranVinhNhan = new NPC
		{
			Id = 18,
			X = 270,
			Y = 232,
			Map = LACDUONG.Id
		};

		// Token: 0x040007D3 RID: 2003
		public static NPC ThamTrinh = new NPC
		{
			Id = 144,
			X = 249,
			Y = 172,
			Map = MAP.ToChau
		};

		// Token: 0x040007D4 RID: 2004
		public static NPC HongDaiQuy = new NPC
		{
			Id = 158,
			X = 181,
			Y = 139,
			Map = MAP.DaiLy
		};

		// Token: 0x040007D5 RID: 2005
		public static NPC CungThaiVan = new NPC
		{
			Id = 153,
			X = 172,
			Y = 122,
			Map = MAP.DaiLy
		};

		// Token: 0x040007D6 RID: 2006
		public static NPC VuongTichTan = new NPC
		{
			Id = 142,
			X = 366,
			Y = 228,
			Map = LACDUONG.Id
		};

		// Token: 0x040007D7 RID: 2007
		public static NPC HoaHachCan = new NPC
		{
			Id = 21,
			X = 71,
			Y = 28,
			Map = MAP.DaiLy
		};

		// Token: 0x040007D8 RID: 2008
		public static NPC TrieuThienSu = new NPC
		{
			Id = 139,
			X = 160,
			Y = 158,
			Map = MAP.DaiLy
		};

		// Token: 0x040007D9 RID: 2009
		public static NPC DuongXichPhong = new NPC
		{
			Id = 0,
			X = 77,
			Y = 34,
			Map = MAP.DuongMon
		};

		// Token: 0x040007DA RID: 2010
		public static NPC MoDungKiet = new NPC
		{
			Id = 13,
			X = 48,
			Y = 144,
			Map = MAP.MoDung
		};

		// Token: 0x040007DB RID: 2011
		public static NPC HanTheTrung = new NPC
		{
			Id = 1,
			X = 95,
			Y = 75,
			Map = MAP.TinhTuc
		};

		// Token: 0x040007DC RID: 2012
		public static NPC ToTinhHa = new NPC
		{
			Id = 0,
			X = 125,
			Y = 144,
			Map = MAP.TieuDao
		};

		// Token: 0x040007DD RID: 2013
		public static NPC HuyenTich = new NPC
		{
			Id = 4,
			X = 89,
			Y = 72,
			Map = MAP.ThieuLam
		};

		// Token: 0x040007DE RID: 2014
		public static NPC MaiKiem = new NPC
		{
			Id = 0,
			X = 91,
			Y = 44,
			Map = MAP.ThienSon
		};

		// Token: 0x040007DF RID: 2015
		public static NPC BanNhan = new NPC
		{
			Id = 0,
			X = 96,
			Y = 66,
			Map = MAP.ThienLong
		};

		// Token: 0x040007E0 RID: 2016
		public static NPC LyThapNhiNuong = new NPC
		{
			Id = 1,
			X = 96,
			Y = 51,
			Map = MAP.NgaMy
		};

		// Token: 0x040007E1 RID: 2017
		public static NPC TruongHuyenTo = new NPC
		{
			Id = 0,
			X = 77,
			Y = 85,
			Map = MAP.VoDang
		};

		// Token: 0x040007E2 RID: 2018
		public static NPC LaSuTuong = new NPC
		{
			Id = 11,
			X = 108,
			Y = 56,
			Map = MAP.MinhGiao
		};

		// Token: 0x040007E3 RID: 2019
		public static NPC TranCoNhan = new NPC
		{
			Id = 23,
			X = 91,
			Y = 98,
			Map = MAP.CaiBang
		};

		// Token: 0x040007E4 RID: 2020
		public static NPC PhuManNghi = new NPC
		{
			Id = 13176,
			X = 159,
			Y = 54,
			Map = MAP.PhieuMieuPhong
		};

		// Token: 0x040007E5 RID: 2021
		public static NPC CapDaiBa = new NPC
		{
			Id = 13157,
			X = 124,
			Y = 86,
			Map = MAP.PhieuMieuPhong
		};

		// Token: 0x040007E6 RID: 2022
		public static NPC TangThoCong = new NPC
		{
			Id = 13158,
			X = 41,
			Y = 105,
			Map = MAP.PhieuMieuPhong
		};

		// Token: 0x040007E7 RID: 2023
		public static NPC OLaoDai = new NPC
		{
			Id = 13159,
			X = 117,
			Y = 49,
			Map = MAP.PhieuMieuPhong
		};

		// Token: 0x040007E8 RID: 2024
		public static NPC TrinhThanhSuong = new NPC
		{
			Id = 56,
			X = 193,
			Y = 224,
			Map = MAP.LauLan
		};

		// Token: 0x040007E9 RID: 2025
		public static NPC LinhThuocTienTu = new NPC
		{
			Id = 160,
			X = 223,
			Y = 138,
			Map = MAP.TayHo
		};

		// Token: 0x040007EA RID: 2026
		public static NPC Aly = new NPC
		{
			Id = 174,
			X = 185,
			Y = 65,
			Map = MAP.DaiLy
		};

		// Token: 0x040007EB RID: 2027
		public static NPC BaCaiLy = new NPC
		{
			Id = 174,
			X = 182,
			Y = 66,
			Map = MAP.DaiLy
		};

		// Token: 0x040007EC RID: 2028
		public static NPC HuyenChung = new NPC
		{
			Id = 24,
			X = 98,
			Y = 145,
			Map = MAP.ThieuLam
		};

		// Token: 0x040007ED RID: 2029
		public static NPC AuDuongQua = new NPC
		{
			Id = 20,
			X = 93,
			Y = 152,
			Map = MAP.CaiBang
		};

		// Token: 0x040007EE RID: 2030
		public static NPC ThacCang = new NPC
		{
			Id = 29,
			X = 95,
			Y = 161,
			Map = MAP.MinhGiao
		};

		// Token: 0x040007EF RID: 2031
		public static NPC TieuThienDat = new NPC
		{
			Id = 21,
			X = 100,
			Y = 181,
			Map = MAP.VoDang
		};

		// Token: 0x040007F0 RID: 2032
		public static NPC HoTuTruongLao = new NPC
		{
			Id = 25,
			X = 99,
			Y = 142,
			Map = MAP.ThienLong
		};

		// Token: 0x040007F1 RID: 2033
		public static NPC CongDaTuTruong = new NPC
		{
			Id = 18,
			X = 44,
			Y = 125,
			Map = MAP.TieuDao
		};

		// Token: 0x040007F2 RID: 2034
		public static NPC LieuTamMuoi = new NPC
		{
			Id = 29,
			X = 94,
			Y = 147,
			Map = MAP.NgaMy
		};

		// Token: 0x040007F3 RID: 2035
		public static NPC ThienToanTu = new NPC
		{
			Id = 21,
			X = 100,
			Y = 143,
			Map = MAP.TinhTuc
		};

		// Token: 0x040007F4 RID: 2036
		public static NPC DangBa = new NPC
		{
			Id = 33,
			X = 96,
			Y = 148,
			Map = MAP.ThienSon
		};

		// Token: 0x040007F5 RID: 2037
		public static NPC CongDaKhon = new NPC
		{
			Id = 7,
			X = 159,
			Y = 163,
			Map = MAP.MoDung
		};

		// Token: 0x040007F6 RID: 2038
		public static NPC DuongMoTuong = new NPC
		{
			Id = 7,
			X = 152,
			Y = 154,
			Map = MAP.DuongMon
		};

		// Token: 0x040007F7 RID: 2039
		public static NPC ThanTinhYeu = new NPC
		{
			Id = 204,
			X = 273,
			Y = 242,
			Map = LACDUONG.Id
		};

		// Token: 0x040007F8 RID: 2040
		public static NPC BuiHoaHong = new NPC
		{
			Id = 5120,
			X = 256,
			Y = 246,
			Map = LACDUONG.Id
		};

		// Token: 0x040007F9 RID: 2041
		public static NPC MaiKhoiTienTu = new NPC
		{
			Id = 12498,
			X = 256,
			Y = 246,
			Map = LACDUONG.Id
		};

		// Token: 0x040007FA RID: 2042
		public static NPC CauPhucThienQuan = new NPC
		{
			Id = 178,
			X = 180,
			Y = 197,
			Map = MAP.DaiLy
		};

		// Token: 0x040007FB RID: 2043
		public static NPC LoTamThat = new NPC
		{
			Id = 12,
			X = 105,
			Y = 123,
			Map = MAP.DaiLy
		};

		// Token: 0x040007FC RID: 2044
		public static NPC doanchinhthuan = new NPC
		{
			Id = 16,
			X = 71,
			Y = 18,
			Map = MAP.DaiLy
		};

		// Token: 0x040007FD RID: 2045
		public static NPC trithanhdaisu = new NPC
		{
			Id = 34,
			X = 176,
			Y = 192,
			Map = LACDUONG.Id
		};

		// Token: 0x040007FE RID: 2046
		public static NPC tothuc = new NPC
		{
			Id = 2,
			X = 166,
			Y = 311,
			Map = MAP.ToChau
		};

		// Token: 0x040007FF RID: 2047
		public static NPC chuthehuu = new NPC
		{
			Id = 7,
			X = 129,
			Y = 99,
			Map = 517
		};

		// Token: 0x04000800 RID: 2048
		public static NPC DoTuBan = new NPC
		{
			Id = 11,
			X = 45,
			Y = 74,
			Map = 517
		};

		// Token: 0x04000801 RID: 2049
		public static NPC QuynhChauChinhDong = new NPC
		{
			Id = 120,
			X = 243,
			Y = 150,
			Map = 35
		};

		// Token: 0x04000802 RID: 2050
		public static NPC QuynhChauDongBac = new NPC
		{
			Id = 119,
			X = 273,
			Y = 52,
			Map = 35
		};

		// Token: 0x04000803 RID: 2051
		public static NPC QuynhChauChinhTay = new NPC
		{
			Id = 110,
			X = 80,
			Y = 139,
			Map = 35
		};

		// Token: 0x04000804 RID: 2052
		public static NPC NamVucDongBac = new NPC
		{
			Id = 116,
			X = 293,
			Y = 59,
			Map = 34
		};

		// Token: 0x04000805 RID: 2053
		public static NPC NamVucTayNam = new NPC
		{
			Id = 117,
			X = 107,
			Y = 228,
			Map = 34
		};

		// Token: 0x04000806 RID: 2054
		public static NPC NamVucChinhBac = new NPC
		{
			Id = 109,
			X = 134,
			Y = 40,
			Map = 34
		};

		// Token: 0x04000807 RID: 2055
		public static NPC VoDiDongBac = new NPC
		{
			Id = 151,
			X = 256,
			Y = 39,
			Map = 32
		};

		// Token: 0x04000808 RID: 2056
		public static NPC VoDiChinhNam = new NPC
		{
			Id = 150,
			X = 113,
			Y = 281,
			Map = 32
		};

		// Token: 0x04000809 RID: 2057
		public static NPC VoDiChinhTay = new NPC
		{
			Id = 142,
			X = 92,
			Y = 171,
			Map = 32
		};

		// Token: 0x0400080A RID: 2058
		public static NPC MaiLinhDongBac = new NPC
		{
			Id = 159,
			X = 271,
			Y = 37,
			Map = 33
		};

		// Token: 0x0400080B RID: 2059
		public static NPC MaiLinhTayBac = new NPC
		{
			Id = 151,
			X = 31,
			Y = 90,
			Map = 33
		};

		// Token: 0x0400080C RID: 2060
		public static NPC MaiLinhChinhDong = new NPC
		{
			Id = 158,
			X = 283,
			Y = 236,
			Map = 33
		};

		// Token: 0x0400080D RID: 2061
		public static NPC TayHoChinhDong = new NPC
		{
			Id = 40,
			X = 262,
			Y = 232,
			Map = 30
		};

		// Token: 0x0400080E RID: 2062
		public static NPC TayHoTayNam = new NPC
		{
			Id = 39,
			X = 45,
			Y = 267,
			Map = 30
		};

		// Token: 0x0400080F RID: 2063
		public static NPC TayHoChinhTay = new NPC
		{
			Id = 23,
			X = 39,
			Y = 139,
			Map = 30
		};

		// Token: 0x04000810 RID: 2064
		public static NPC LongTuyenDongNam = new NPC
		{
			Id = 143,
			X = 218,
			Y = 282,
			Map = 31
		};

		// Token: 0x04000811 RID: 2065
		public static NPC LongTuyenChinhBac = new NPC
		{
			Id = 141,
			X = 63,
			Y = 33,
			Map = 31
		};

		// Token: 0x04000812 RID: 2066
		public static NPC LongTuyenChinhTay = new NPC
		{
			Id = 142,
			X = 36,
			Y = 121,
			Map = 31
		};

		// Token: 0x04000813 RID: 2067
		public static NPC NhanNamChinhDong = new NPC
		{
			Id = 140,
			X = 283,
			Y = 113,
			Map = 18
		};

		// Token: 0x04000814 RID: 2068
		public static NPC NhanNamChinhBac = new NPC
		{
			Id = 139,
			X = 102,
			Y = 36,
			Map = 18
		};

		// Token: 0x04000815 RID: 2069
		public static NPC NhanNamChinhNam = new NPC
		{
			Id = 138,
			X = 72,
			Y = 284,
			Map = 18
		};

		// Token: 0x04000816 RID: 2070
		public static NPC NhanBacDongBac = new NPC
		{
			Id = 121,
			X = 235,
			Y = 24,
			Map = 19
		};

		// Token: 0x04000817 RID: 2071
		public static NPC NhanBacTayBac = new NPC
		{
			Id = 120,
			X = 116,
			Y = 30,
			Map = 19
		};

		// Token: 0x04000818 RID: 2072
		public static NPC NhanBacChinhTay = new NPC
		{
			Id = 115,
			X = 32,
			Y = 128,
			Map = 19
		};

		// Token: 0x04000819 RID: 2073
		public static NPC ThaoNguyenTayNam = new NPC
		{
			Id = 173,
			X = 97,
			Y = 282,
			Map = 20
		};

		// Token: 0x0400081A RID: 2074
		public static NPC ThaoNguyenChinhDong = new NPC
		{
			Id = 174,
			X = 271,
			Y = 192,
			Map = 20
		};

		// Token: 0x0400081B RID: 2075
		public static NPC ThaoNguyenChinhTay = new NPC
		{
			Id = 166,
			X = 66,
			Y = 202,
			Map = 20
		};

		// Token: 0x0400081C RID: 2076
		public static NPC LieuTayDongNam = new NPC
		{
			Id = 152,
			X = 278,
			Y = 258,
			Map = 21
		};

		// Token: 0x0400081D RID: 2077
		public static NPC LieuTayTayBac = new NPC
		{
			Id = 142,
			X = 75,
			Y = 35,
			Map = 21
		};

		// Token: 0x0400081E RID: 2078
		public static NPC LieuTayChinhTay = new NPC
		{
			Id = 151,
			X = 40,
			Y = 142,
			Map = 21
		};

		// Token: 0x0400081F RID: 2079
		public static NPC TruongBachSonChinhNam = new NPC
		{
			Id = 148,
			X = 217,
			Y = 282,
			Map = 22
		};

		// Token: 0x04000820 RID: 2080
		public static NPC TruongBachSonTayBac = new NPC
		{
			Id = 119,
			X = 39,
			Y = 63,
			Map = 22
		};

		// Token: 0x04000821 RID: 2081
		public static NPC TruongBachSonChinhDong = new NPC
		{
			Id = 147,
			X = 280,
			Y = 155,
			Map = 22
		};

		// Token: 0x04000822 RID: 2082
		public static NPC HoangLongPhuChinhDong = new NPC
		{
			Id = 143,
			X = 290,
			Y = 116,
			Map = 23
		};

		// Token: 0x04000823 RID: 2083
		public static NPC HoangLongPhuTayBac = new NPC
		{
			Id = 114,
			X = 28,
			Y = 54,
			Map = 23
		};

		// Token: 0x04000824 RID: 2084
		public static NPC HoangLongPhuDongNam = new NPC
		{
			Id = 144,
			X = 254,
			Y = 286,
			Map = 23
		};

		// Token: 0x04000825 RID: 2085
		public static NPC NhiHaiChinhDong = new NPC
		{
			Id = 46,
			X = 286,
			Y = 166,
			Map = 24
		};

		// Token: 0x04000826 RID: 2086
		public static NPC NhiHaiChinhNam = new NPC
		{
			Id = 45,
			X = 173,
			Y = 284,
			Map = 24
		};

		// Token: 0x04000827 RID: 2087
		public static NPC NhiHaiChinhTay = new NPC
		{
			Id = 43,
			X = 34,
			Y = 100,
			Map = 24
		};

		// Token: 0x04000828 RID: 2088
		public static NPC ThuongSonTranTay = new NPC
		{
			Id = 70,
			X = 37,
			Y = 172,
			Map = 25
		};

		// Token: 0x04000829 RID: 2089
		public static NPC ThuongSonChinhDong = new NPC
		{
			Id = 161,
			X = 295,
			Y = 154,
			Map = 25
		};

		// Token: 0x0400082A RID: 2090
		public static NPC ThuongSonChinhNam = new NPC
		{
			Id = 160,
			X = 146,
			Y = 285,
			Map = 25
		};

		// Token: 0x0400082B RID: 2091
		public static NPC ThachLamChinhBac = new NPC
		{
			Id = 146,
			X = 227,
			Y = 37,
			Map = 26
		};

		// Token: 0x0400082C RID: 2092
		public static NPC ThachLamChinhNam = new NPC
		{
			Id = 138,
			X = 278,
			Y = 281,
			Map = 26
		};

		// Token: 0x0400082D RID: 2093
		public static NPC ThachLamChinhTay = new NPC
		{
			Id = 147,
			X = 45,
			Y = 178,
			Map = 26
		};

		// Token: 0x0400082E RID: 2094
		public static NPC NgocKheTayNam = new NPC
		{
			Id = 136,
			X = 33,
			Y = 251,
			Map = 27
		};

		// Token: 0x0400082F RID: 2095
		public static NPC NgocKheChinhBac = new NPC
		{
			Id = 143,
			X = 178,
			Y = 38,
			Map = 27
		};

		// Token: 0x04000830 RID: 2096
		public static NPC NgocKheChinhNam = new NPC
		{
			Id = 142,
			X = 197,
			Y = 280,
			Map = 27
		};

		// Token: 0x04000831 RID: 2097
		public static NPC NamChieuTayBac = new NPC
		{
			Id = 127,
			X = 96,
			Y = 41,
			Map = 28
		};

		// Token: 0x04000832 RID: 2098
		public static NPC NamChieuDongNam = new NPC
		{
			Id = 128,
			X = 273,
			Y = 242,
			Map = 28
		};

		// Token: 0x04000833 RID: 2099
		public static NPC NamChieuTayNam = new NPC
		{
			Id = 120,
			X = 39,
			Y = 254,
			Map = 28
		};

		// Token: 0x04000834 RID: 2100
		public static NPC MieuCuongDongBac = new NPC
		{
			Id = 157,
			X = 251,
			Y = 46,
			Map = 29
		};

		// Token: 0x04000835 RID: 2101
		public static NPC MieuCuongTayNam = new NPC
		{
			Id = 156,
			X = 38,
			Y = 251,
			Map = 29
		};

		// Token: 0x04000836 RID: 2102
		public static NPC MieuCuongChinhDong = new NPC
		{
			Id = 117,
			X = 281,
			Y = 160,
			Map = 29
		};

		// Token: 0x04000837 RID: 2103
		public static NPC DOTHANHDANG = new NPC
		{
			Id = 800,
			X = 293,
			Y = 141,
			Map = MAP.LauLan
		};

		// Token: 0x04000838 RID: 2104
		public static NPC LONGBATHIEN = new NPC
		{
			Id = 801,
			X = 191,
			Y = 336,
			Map = 0
		};

		// Token: 0x04000839 RID: 2105
		public static NPC BINHSANHAN = new NPC
		{
			Id = 802,
			X = 164,
			Y = 260,
			Map = MAP.ToChau
		};

		// Token: 0x0400083A RID: 2106
		public static NPC VANDIEUDIEU = new NPC
		{
			Id = 803,
			X = 277,
			Y = 298,
			Map = 0
		};

		// Token: 0x0400083B RID: 2107
		public static NPC TONTUVU = new NPC
		{
			Id = 804,
			X = 226,
			Y = 270,
			Map = MAP.ToChau
		};

		// Token: 0x0400083C RID: 2108
		public static NPC TRUONGTHIENTHIEN = new NPC
		{
			Id = 805,
			X = 227,
			Y = 162,
			Map = MAP.LauLan
		};

		// Token: 0x0400083D RID: 2109
		public static NPC DONGHOAKIM = new NPC
		{
			Id = 806,
			X = 228,
			Y = 93,
			Map = MAP.DaiLy
		};
	}
}
