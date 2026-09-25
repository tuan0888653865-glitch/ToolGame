using System;
using System.Collections.Generic;

namespace TinhKiemAuto
{
	// Token: 0x020000E1 RID: 225
	internal class Shop
	{
		// Token: 0x170002BE RID: 702
		// (get) Token: 0x06000BC2 RID: 3010 RVA: 0x0004AFE8 File Offset: 0x000491E8
		public string ClearName
		{
			get
			{
				return TINHKIEM.VietLien(this.Name);
			}
		}

		// Token: 0x06000BC3 RID: 3011 RVA: 0x0004AFF8 File Offset: 0x000491F8
		public override string ToString()
		{
			return string.Concat(new string[]
			{
				string.Empty,
				"Address: ",
				this.Address.ToString("X8"),
				"\r\nClass: ",
				this.Class.ToString("X8"),
				"\r\nIndex: ",
				this.Index.ToString(),
				"\r\nTypeName: ",
				this.TypeName,
				"\r\n",
				this.Name
			});
		}

		// Token: 0x06000BC4 RID: 3012 RVA: 0x0004B08C File Offset: 0x0004928C
		public static List<Shop> Enum(Game game)
		{
			List<Shop> list = new List<Shop>();
			int num = game.Memory.Read(game.Address.BaseShopItem);
			for (int i = 0; i < 80; i++)
			{
				if (game.Memory.Read(num + i * 4) != 0)
				{
					Shop shop = new Shop();
					shop.Address = game.Memory.Read(num + i * 4);
					shop.Class = game.Memory.Read(shop.Address);
					shop.DefineId = game.Memory.Read(shop.Address + 8);
					if (shop.Class == game.Address.PacketType2)
					{
						shop.Name = game.Memory.ReadString(game.Memory.Read(shop.Address + 40, 24));
						shop.TypeName = game.Memory.ReadString(game.Memory.Read(shop.Address + 40, 80));
					}
					else
					{
						shop.Name = game.Memory.ReadString(game.Memory.Read(shop.Address + 40, 40));
						shop.TypeName = game.Memory.ReadString(game.Memory.Read(shop.Address + 40, 88));
					}
					shop.Index = game.Memory.Read(shop.Address + 16);
					list.Add(shop);
				}
			}
			return list;
		}

		// Token: 0x040008D2 RID: 2258
		public int Address;

		// Token: 0x040008D3 RID: 2259
		public int Class;

		// Token: 0x040008D4 RID: 2260
		public int DefineId;

		// Token: 0x040008D5 RID: 2261
		public string Name;

		// Token: 0x040008D6 RID: 2262
		public int Index;

		// Token: 0x040008D7 RID: 2263
		public string TypeName;
	}
}
