using System;

namespace TinhKiemAuto
{
	// Token: 0x02000100 RID: 256
	internal class DAILY
	{
		// Token: 0x06000E91 RID: 3729 RVA: 0x0006F63C File Offset: 0x0006D83C
		public static NPC GetNPCSuMon(int menpai)
		{
			if (menpai == MENPAI.DuongMon)
			{
				return DAILY.DuongDuc;
			}
			if (menpai == MENPAI.MinhGiao)
			{
				return DAILY.ThachBao;
			}
			if (menpai == MENPAI.ThienSon)
			{
				return DAILY.TrinhThanhSuong;
			}
			if (menpai == MENPAI.TinhTuc)
			{
				return DAILY.HaiPhongTu;
			}
			if (menpai == MENPAI.ThienLong)
			{
				return DAILY.PhaTham;
			}
			if (menpai == MENPAI.TieuDao)
			{
				return DAILY.DamDaiTuVu;
			}
			if (menpai == MENPAI.MoDung)
			{
				return DAILY.MoDungTruyen;
			}
			if (menpai == MENPAI.NgaMy)
			{
				return DAILY.LoTamNuong;
			}
			if (menpai == MENPAI.CaiBang)
			{
				return DAILY.GianNinh;
			}
			if (menpai == MENPAI.ThieuLam)
			{
				return DAILY.TueDich;
			}
			if (menpai == MENPAI.VoDang)
			{
				return DAILY.TruongHoach;
			}
			return null;
		}

		// Token: 0x04000C0D RID: 3085
		public static int Id = 2;

		// Token: 0x04000C0E RID: 3086
		public static NPC BaCaiLy = new NPC
		{
			Id = 175,
			X = 182,
			Y = 69,
			Map = DAILY.Id,
			INFOAIM = "#GĐại Lý#RBa Cái Lý#{_INFOAIM182,69,2,Ba Cái Lý}"
		};

		// Token: 0x04000C0F RID: 3087
		public static NPC ALy = new NPC
		{
			Id = 174,
			X = 185,
			Y = 65,
			Map = DAILY.Id,
			INFOAIM = "#GĐại Lý#RA Lý#{_INFOAIM185,65,2,A Lý}"
		};

		// Token: 0x04000C10 RID: 3088
		public static NPC HoaHachCan = new NPC
		{
			Id = 21,
			X = 71,
			Y = 28,
			Map = DAILY.Id,
			INFOAIM = "#GĐại Lý#RHoa Hách Cấn#{_INFOAIM71,28,2,Hoa Hách Cấn}"
		};

		// Token: 0x04000C11 RID: 3089
		public static NPC ChuDanThan = new NPC
		{
			Id = 182,
			X = 70,
			Y = 58,
			Map = DAILY.Id,
			INFOAIM = "#GĐại Lý#RChu Đan Thần#{_INFOAIM70,58,2,Chu Đan Thần}"
		};

		// Token: 0x04000C12 RID: 3090
		public static NPC ThuongKho = new NPC
		{
			Id = 2,
			X = 203,
			Y = 177,
			Map = DAILY.Id,
			INFOAIM = "#GĐại Lý#RHầu bàn Chu#{_INFOAIM203,177,2,Hầu bàn Chu}"
		};

		// Token: 0x04000C13 RID: 3091
		public static NPC ThuongKhoTinhKiem = new NPC
		{
			Id = 2,
			X = 201,
			Y = 177,
			Map = DAILY.Id,
			INFOAIM = "#GĐại Lý#RHầu bàn Chu #{_INFOAIM201,177,2,Hầu bàn Chu }"
		};

		// Token: 0x04000C14 RID: 3092
		public static NPC VanPhieuPhieu = new NPC
		{
			Id = 35,
			X = 271,
			Y = 133,
			Map = DAILY.Id,
			INFOAIM = "#GĐại Lý#RVân Phiêu Phiêu#{_INFOAIM271,133,2,Vân Phiêu Phiêu}"
		};

		// Token: 0x04000C15 RID: 3093
		public static NPC KhoVinhDaiSu = new NPC
		{
			Id = 166,
			X = 131,
			Y = 79,
			Map = DAILY.Id,
			INFOAIM = "#GĐại Lý#RKhô Vinh Đại Sư#{_INFOAIM131,79,2,Khô Vinh Đại Sư}"
		};

		// Token: 0x04000C16 RID: 3094
		public static NPC TrieuThienSu = new NPC
		{
			Id = 139,
			X = 160,
			Y = 159,
			Map = DAILY.Id,
			INFOAIM = "#GĐại Lý#RTriệu Thiên Sư#{_INFOAIM160,159,2,Triệu Thiên Sư}"
		};

		// Token: 0x04000C17 RID: 3095
		public static NPC CauPhucThienQuan = new NPC
		{
			Id = 178,
			X = 182,
			Y = 197,
			Map = DAILY.Id,
			INFOAIM = "#GĐại Lý#RCầu Phúc Thiên Quan#{_INFOAIM182,197,2,Cầu Phúc Thiên Quan}"
		};

		// Token: 0x04000C18 RID: 3096
		public static NPC PhamThuanLe = new NPC
		{
			Id = 163,
			X = 179,
			Y = 121,
			Map = DAILY.Id,
			INFOAIM = "#GĐại Lý#RPhạm Thuần Lễ#{_INFOAIM179,121,2,Phạm Thuần Lễ}"
		};

		// Token: 0x04000C19 RID: 3097
		public static NPC HongDaiQuy = new NPC
		{
			Id = 158,
			X = 181,
			Y = 139,
			Map = DAILY.Id,
			INFOAIM = "#GĐại Lý#RHồng Đại Quý#{_INFOAIM181,139,2,Hồng Đại Quý}"
		};

		// Token: 0x04000C1A RID: 3098
		public static NPC PhoKiepSinh = new NPC
		{
			Id = 164,
			X = 94,
			Y = 201,
			Map = DAILY.Id,
			INFOAIM = "#GĐại Lý#RPhó Kiếp Sinh#{_INFOAIM94,201,2,Phó Kiếp Sinh}"
		};

		// Token: 0x04000C1B RID: 3099
		public static NPC DuongDuc = new NPC
		{
			Id = 193,
			X = 166,
			Y = 142,
			Map = DAILY.Id,
			INFOAIM = "#GĐại Lý#RĐường Dục#{_INFOAIM166,142,2,Đường Dục}"
		};

		// Token: 0x04000C1C RID: 3100
		public static NPC ThachBao = new NPC
		{
			Id = 29,
			X = 166,
			Y = 138,
			Map = DAILY.Id,
			INFOAIM = "#GĐại Lý#RThạch Bảo#{_INFOAIM166,138,2,Thạch Bảo}"
		};

		// Token: 0x04000C1D RID: 3101
		public static NPC TrinhThanhSuong = new NPC
		{
			Id = 62,
			X = 166,
			Y = 135,
			Map = DAILY.Id,
			INFOAIM = "#GĐại Lý#RTrình Thanh Sương#{_INFOAIM166,135,2,Trình Thanh Sương}"
		};

		// Token: 0x04000C1E RID: 3102
		public static NPC HaiPhongTu = new NPC
		{
			Id = 28,
			X = 166,
			Y = 131,
			Map = DAILY.Id,
			INFOAIM = "#GĐại Lý#RHải Phong Tử#{_INFOAIM166,131,2,Hải Phong Tử}"
		};

		// Token: 0x04000C1F RID: 3103
		public static NPC PhaTham = new NPC
		{
			Id = 95,
			X = 166,
			Y = 128,
			Map = DAILY.Id,
			INFOAIM = "#GĐại Lý#RPhá Tham#{_INFOAIM166,128,2,Phá Tham}"
		};

		// Token: 0x04000C20 RID: 3104
		public static NPC DamDaiTuVu = new NPC
		{
			Id = 26,
			X = 166,
			Y = 124,
			Map = DAILY.Id,
			INFOAIM = "#GĐại Lý#RĐàm Đài Tử Vũ#{_INFOAIM166,124,2,Đàm Đài Tử Vũ}"
		};

		// Token: 0x04000C21 RID: 3105
		public static NPC MoDungTruyen = new NPC
		{
			Id = 177,
			X = 154,
			Y = 140,
			Map = DAILY.Id,
			INFOAIM = "#GĐại Lý#RMộ Dung Truyền#{_INFOAIM154,140,2,Mộ Dung Truyền}"
		};

		// Token: 0x04000C22 RID: 3106
		public static NPC LoTamNuong = new NPC
		{
			Id = 64,
			X = 154,
			Y = 137,
			Map = DAILY.Id,
			INFOAIM = "#GĐại Lý#RLộ Tam Nương#{_INFOAIM154,137,2,Lộ Tam Nương}"
		};

		// Token: 0x04000C23 RID: 3107
		public static NPC GianNinh = new NPC
		{
			Id = 27,
			X = 154,
			Y = 133,
			Map = DAILY.Id,
			INFOAIM = "#GĐại Lý#RGiản Ninh#{_INFOAIM154,133,2,Giản Ninh}"
		};

		// Token: 0x04000C24 RID: 3108
		public static NPC TueDich = new NPC
		{
			Id = 25,
			X = 154,
			Y = 129,
			Map = DAILY.Id,
			INFOAIM = "#GĐại Lý#RTuệ Dịch#{_INFOAIM154,129,2,Tuệ Dịch}"
		};

		// Token: 0x04000C25 RID: 3109
		public static NPC TruongHoach = new NPC
		{
			Id = 30,
			X = 154,
			Y = 126,
			Map = DAILY.Id,
			INFOAIM = "#GĐại Lý#RTrương Hoạch#{_INFOAIM154,126,2,Trương Hoạch}"
		};
	}
}
