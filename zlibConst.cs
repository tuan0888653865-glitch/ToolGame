using System;

namespace ComponentAce.Compression.Libs.zlib
{
	// Token: 0x02000011 RID: 17
	public sealed class zlibConst
	{
		// Token: 0x06000082 RID: 130 RVA: 0x000088B4 File Offset: 0x00006AB4
		public static string version()
		{
			return "1.0.2";
		}

		// Token: 0x0400012F RID: 303
		private const string version_Renamed_Field = "1.0.2";

		// Token: 0x04000130 RID: 304
		public const int Z_NO_COMPRESSION = 0;

		// Token: 0x04000131 RID: 305
		public const int Z_BEST_SPEED = 1;

		// Token: 0x04000132 RID: 306
		public const int Z_BEST_COMPRESSION = 9;

		// Token: 0x04000133 RID: 307
		public const int Z_DEFAULT_COMPRESSION = -1;

		// Token: 0x04000134 RID: 308
		public const int Z_FILTERED = 1;

		// Token: 0x04000135 RID: 309
		public const int Z_HUFFMAN_ONLY = 2;

		// Token: 0x04000136 RID: 310
		public const int Z_DEFAULT_STRATEGY = 0;

		// Token: 0x04000137 RID: 311
		public const int Z_NO_FLUSH = 0;

		// Token: 0x04000138 RID: 312
		public const int Z_PARTIAL_FLUSH = 1;

		// Token: 0x04000139 RID: 313
		public const int Z_SYNC_FLUSH = 2;

		// Token: 0x0400013A RID: 314
		public const int Z_FULL_FLUSH = 3;

		// Token: 0x0400013B RID: 315
		public const int Z_FINISH = 4;

		// Token: 0x0400013C RID: 316
		public const int Z_OK = 0;

		// Token: 0x0400013D RID: 317
		public const int Z_STREAM_END = 1;

		// Token: 0x0400013E RID: 318
		public const int Z_NEED_DICT = 2;

		// Token: 0x0400013F RID: 319
		public const int Z_ERRNO = -1;

		// Token: 0x04000140 RID: 320
		public const int Z_STREAM_ERROR = -2;

		// Token: 0x04000141 RID: 321
		public const int Z_DATA_ERROR = -3;

		// Token: 0x04000142 RID: 322
		public const int Z_MEM_ERROR = -4;

		// Token: 0x04000143 RID: 323
		public const int Z_BUF_ERROR = -5;

		// Token: 0x04000144 RID: 324
		public const int Z_VERSION_ERROR = -6;
	}
}
