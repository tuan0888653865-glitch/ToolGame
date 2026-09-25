using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace TinhKiemAuto
{
	// Token: 0x020000D4 RID: 212
	[Serializable]
	internal class ResourceLocation
	{
		// Token: 0x06000B20 RID: 2848 RVA: 0x0004938D File Offset: 0x0004758D
		public static ResourceLocation FromURL(string url)
		{
			return new ResourceLocation
			{
				URL = url
			};
		}

		// Token: 0x06000B21 RID: 2849 RVA: 0x0004939C File Offset: 0x0004759C
		public static ResourceLocation[] FromURLArray(string[] urls)
		{
			List<ResourceLocation> list = new List<ResourceLocation>();
			for (int i = 0; i < urls.Length; i++)
			{
				if (ResourceLocation.IsURL(urls[i]))
				{
					list.Add(ResourceLocation.FromURL(urls[i]));
				}
			}
			return list.ToArray();
		}

		// Token: 0x06000B22 RID: 2850 RVA: 0x000493DB File Offset: 0x000475DB
		public static ResourceLocation FromURL(string url, bool authenticate, string login, string password)
		{
			return new ResourceLocation
			{
				URL = url,
				Authenticate = authenticate,
				Login = login,
				Password = password
			};
		}

		// Token: 0x1700027F RID: 639
		// (get) Token: 0x06000B23 RID: 2851 RVA: 0x000493FE File Offset: 0x000475FE
		// (set) Token: 0x06000B24 RID: 2852 RVA: 0x00049406 File Offset: 0x00047606
		public string URL
		{
			get
			{
				return this.url;
			}
			set
			{
				this.url = value;
				this.BindProtocolProviderType();
			}
		}

		// Token: 0x17000280 RID: 640
		// (get) Token: 0x06000B25 RID: 2853 RVA: 0x00049415 File Offset: 0x00047615
		// (set) Token: 0x06000B26 RID: 2854 RVA: 0x0004941D File Offset: 0x0004761D
		public bool Authenticate
		{
			get
			{
				return this.authenticate;
			}
			set
			{
				this.authenticate = value;
			}
		}

		// Token: 0x17000281 RID: 641
		// (get) Token: 0x06000B27 RID: 2855 RVA: 0x00049426 File Offset: 0x00047626
		// (set) Token: 0x06000B28 RID: 2856 RVA: 0x0004942E File Offset: 0x0004762E
		public string Login
		{
			get
			{
				return this.login;
			}
			set
			{
				this.login = value;
			}
		}

		// Token: 0x17000282 RID: 642
		// (get) Token: 0x06000B29 RID: 2857 RVA: 0x00049437 File Offset: 0x00047637
		// (set) Token: 0x06000B2A RID: 2858 RVA: 0x0004943F File Offset: 0x0004763F
		public string Password
		{
			get
			{
				return this.password;
			}
			set
			{
				this.password = value;
			}
		}

		// Token: 0x17000283 RID: 643
		// (get) Token: 0x06000B2B RID: 2859 RVA: 0x00049448 File Offset: 0x00047648
		// (set) Token: 0x06000B2C RID: 2860 RVA: 0x0004945F File Offset: 0x0004765F
		public string ProtocolProviderType
		{
			get
			{
				if (this.protocolProviderType == null)
				{
					return null;
				}
				return this.protocolProviderType.AssemblyQualifiedName;
			}
			set
			{
				if (value == null)
				{
					this.BindProtocolProviderType();
					return;
				}
				this.protocolProviderType = Type.GetType(value);
			}
		}

		// Token: 0x06000B2D RID: 2861 RVA: 0x00049477 File Offset: 0x00047677
		public IProtocolProvider GetProtocolProvider(Downloader downloader)
		{
			return this.BindProtocolProviderInstance(downloader);
		}

		// Token: 0x06000B2E RID: 2862 RVA: 0x00049480 File Offset: 0x00047680
		public void BindProtocolProviderType()
		{
			this.provider = null;
			if (!string.IsNullOrEmpty(this.URL))
			{
				this.protocolProviderType = ProtocolProviderFactory.GetProviderType(this.URL);
			}
		}

		// Token: 0x06000B2F RID: 2863 RVA: 0x000494A7 File Offset: 0x000476A7
		public IProtocolProvider BindProtocolProviderInstance(Downloader downloader)
		{
			if (this.protocolProviderType == null)
			{
				this.BindProtocolProviderType();
			}
			if (this.provider == null)
			{
				this.provider = ProtocolProviderFactory.CreateProvider(this.protocolProviderType, downloader);
			}
			return this.provider;
		}

		// Token: 0x06000B30 RID: 2864 RVA: 0x000494D7 File Offset: 0x000476D7
		public ResourceLocation Clone()
		{
			return (ResourceLocation)base.MemberwiseClone();
		}

		// Token: 0x06000B31 RID: 2865 RVA: 0x000494E4 File Offset: 0x000476E4
		public override string ToString()
		{
			return this.URL;
		}

		// Token: 0x06000B32 RID: 2866 RVA: 0x000494EC File Offset: 0x000476EC
		public static bool IsURL(string url)
		{
			return Regex.Match(url, "(?<Protocol>\\w+):\\/\\/(?<Domain>[\\w.]+\\/?)\\S*").ToString() != string.Empty;
		}

		// Token: 0x040008A5 RID: 2213
		private string url;

		// Token: 0x040008A6 RID: 2214
		private bool authenticate;

		// Token: 0x040008A7 RID: 2215
		private string login;

		// Token: 0x040008A8 RID: 2216
		private string password;

		// Token: 0x040008A9 RID: 2217
		private Type protocolProviderType;

		// Token: 0x040008AA RID: 2218
		private IProtocolProvider provider;
	}
}
