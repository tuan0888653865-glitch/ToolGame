using System;
using System.Collections;

namespace TinhKiemAuto
{
	// Token: 0x020000CF RID: 207
	internal static class ProtocolProviderFactory
	{
		// Token: 0x14000012 RID: 18
		// (add) Token: 0x06000AF7 RID: 2807 RVA: 0x00048AF8 File Offset: 0x00046CF8
		// (remove) Token: 0x06000AF8 RID: 2808 RVA: 0x00048B2C File Offset: 0x00046D2C
		public static event EventHandler<ResolvingProtocolProviderEventArgs> ResolvingProtocolProvider;

		// Token: 0x06000AF9 RID: 2809 RVA: 0x00048B5F File Offset: 0x00046D5F
		public static void RegisterProtocolHandler(string prefix, Type protocolProvider)
		{
			ProtocolProviderFactory.protocolHandlers[prefix] = protocolProvider;
		}

		// Token: 0x06000AFA RID: 2810 RVA: 0x00048B70 File Offset: 0x00046D70
		public static IProtocolProvider CreateProvider(string uri, Downloader downloader)
		{
			IProtocolProvider protocolProvider = ProtocolProviderFactory.InternalGetProvider(uri);
			if (downloader != null)
			{
				protocolProvider.Initialize(downloader);
			}
			return protocolProvider;
		}

		// Token: 0x06000AFB RID: 2811 RVA: 0x00048B8F File Offset: 0x00046D8F
		public static IProtocolProvider GetProvider(string uri)
		{
			return ProtocolProviderFactory.InternalGetProvider(uri);
		}

		// Token: 0x06000AFC RID: 2812 RVA: 0x00048B98 File Offset: 0x00046D98
		public static Type GetProviderType(string uri)
		{
			int num = uri.IndexOf("://");
			if (num > 0)
			{
				string key = uri.Substring(0, num);
				return ProtocolProviderFactory.protocolHandlers[key] as Type;
			}
			return null;
		}

		// Token: 0x06000AFD RID: 2813 RVA: 0x00048BD0 File Offset: 0x00046DD0
		public static IProtocolProvider CreateProvider(Type providerType, Downloader downloader)
		{
			IProtocolProvider protocolProvider = ProtocolProviderFactory.CreateFromType(providerType);
			if (ProtocolProviderFactory.ResolvingProtocolProvider != null)
			{
				ResolvingProtocolProviderEventArgs resolvingProtocolProviderEventArgs = new ResolvingProtocolProviderEventArgs(protocolProvider, null);
				ProtocolProviderFactory.ResolvingProtocolProvider(null, resolvingProtocolProviderEventArgs);
				protocolProvider = resolvingProtocolProviderEventArgs.ProtocolProvider;
			}
			if (downloader != null)
			{
				protocolProvider.Initialize(downloader);
			}
			return protocolProvider;
		}

		// Token: 0x06000AFE RID: 2814 RVA: 0x00048C14 File Offset: 0x00046E14
		private static IProtocolProvider InternalGetProvider(string uri)
		{
			IProtocolProvider protocolProvider = ProtocolProviderFactory.CreateFromType(ProtocolProviderFactory.GetProviderType(uri));
			if (ProtocolProviderFactory.ResolvingProtocolProvider != null)
			{
				ResolvingProtocolProviderEventArgs resolvingProtocolProviderEventArgs = new ResolvingProtocolProviderEventArgs(protocolProvider, uri);
				ProtocolProviderFactory.ResolvingProtocolProvider(null, resolvingProtocolProviderEventArgs);
				protocolProvider = resolvingProtocolProviderEventArgs.ProtocolProvider;
			}
			return protocolProvider;
		}

		// Token: 0x06000AFF RID: 2815 RVA: 0x00048C50 File Offset: 0x00046E50
		private static IProtocolProvider CreateFromType(Type type)
		{
			return (IProtocolProvider)Activator.CreateInstance(type);
		}

		// Token: 0x04000895 RID: 2197
		private static Hashtable protocolHandlers = new Hashtable();
	}
}
